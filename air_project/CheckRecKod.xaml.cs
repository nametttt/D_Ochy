using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для CheckRecKod.xaml
    /// </summary>
    public partial class CheckRecKod : Window
    {
        public User _user;
        public AirTicketsEntities _context = new AirTicketsEntities();
        public static string code = "";
        static MailAddress to;
        string _email = "";
        public CheckRecKod(string Email)
        {
            InitializeComponent();
            SendEmailAsync(Email).GetAwaiter();
            _email = Email;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }

        private static async Task SendEmailAsync(string email)
        {
            try
            {
                code = "";
                MailAddress from = new MailAddress("ochy.tickets@gmail.com", "OCHY - Авиабилеты");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Восстановление пароля";
                makeCode();

                string emailBody = $@"
                <html>
                <body style=""font-family: Arial, sans-serif;"">
                    <h1 style=""color: black;"">Восстановление пароля</h1>
                    <p style=""color: black; font-size: 16px;"">Для восстановления пароля на OCHY - Авиабилеты, введите следующий код подтверждения:</p>
                    <h2 style=""background-color: #f5f5f5; padding: 10px; border-radius: 5px; color: black;"">{code}</h2>
                    <p style=""color: black; font-size: 16px;"">Команда OCHY - Авиабилеты</p>
                </body>
                </html>";

                m.Body = emailBody;
                m.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("ochy.tickets@gmail.com", "ivrcjihrdhacolge");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static void makeCode()
        {
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                code += Convert.ToString(rand.Next(0, 9));
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            if (txtCode.Text == code)
            {
                GetHash g = new GetHash();
                foreach (User user in _context.User)
                {
                    if (_email == user.Login)
                    {
                        user.Password = g.GetHashString(txtPassword.Password);
                    }
                }
                _context.SaveChanges();
                MessageBox.Show("Пароль успешно изменен!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                Auth auth = new Auth();
                auth.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Код не совпадает!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
