using air_project.pages;
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
    public partial class Confirmation : Window
    {
        public static string code = "";
        static MailAddress to;
        private MyUser myUser;
        string _email = "";

        public Confirmation(string Email, MyUser regUser)
        {
            InitializeComponent();
            SendEmailAsync(Email).GetAwaiter();
            myUser = regUser;
        }

        private void btnMyEmail_Click(object sender, RoutedEventArgs e)
        {
            if (txtCode.Text == code)
            {
                using(AirTicketsEntities db = new AirTicketsEntities())
                {
                    MessageBox.Show("Спасибо за код!", "OCHY", MessageBoxButton.OK, MessageBoxImage.Information);
                    User user = new User(myUser.Email, myUser.Surname, myUser.UserName, myUser.Patronymic, myUser.Password, "Покупатель");
                    db.User.Add(user);
                    db.SaveChangesAsync();
                    MessageBox.Show("Пользователь успешно зарегистрирован!");
                    Auth auth = new Auth();
                    auth.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Код не совпадает!", "OCHY", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static async Task SendEmailAsync(string email)
        {
            try
            {
                code = "";
                MailAddress from = new MailAddress("ochy.tickets@gmail.com", "OCHY - Авиабилеты");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Подтверждение почты";
                makeCode();

                string emailBody = $@"
                <html>
                <body style=""font-family: Arial, sans-serif;"">
                    <h1 style=""color: black;"">Подтверждение почты</h1>
                    <p style=""color: black; font-size: 16px;"">Для завершения регистрации на OCHY - Авиабилеты, введите следующий код подтверждения:</p>
                    <h2 style=""background-color: #f5f5f5; padding: 10px; border-radius: 5px; color: black;"">{code}</h2>
                    <p style=""color: black; font-size: 16px;"">Спасибо за регистрацию!</p>
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }
    }
}
