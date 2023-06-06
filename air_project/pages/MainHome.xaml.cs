using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для MainHome.xaml
    /// </summary>
    public partial class MainHome : Page
    {
        public User _user;
        public AirTicketsEntities _context = new AirTicketsEntities();

        public MainHome(User user)
        {
            InitializeComponent();
            _user = user;

            OutputData();

        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string digitsOnly = Regex.Replace(textBox.Text, "[^0-9]", "");

            string formattedNumber = string.Empty;
            int index = 0;

            if (digitsOnly.Length > 0)
            {
                formattedNumber = digitsOnly.Substring(index, 1);

                if (digitsOnly.Length > 1)
                    formattedNumber += "-" + digitsOnly.Substring(1, Math.Min(3, digitsOnly.Length - 1));

                if (digitsOnly.Length > 4)
                    formattedNumber += "-" + digitsOnly.Substring(4, Math.Min(3, digitsOnly.Length - 4));

                if (digitsOnly.Length > 7)
                    formattedNumber += "-" + digitsOnly.Substring(7, Math.Min(2, digitsOnly.Length - 7));

                if (digitsOnly.Length > 9)
                    formattedNumber += "-" + digitsOnly.Substring(9, Math.Min(2, digitsOnly.Length - 9));
            }

            textBox.TextChanged -= Phone_TextChanged;
            textBox.Text = formattedNumber;
            textBox.CaretIndex = textBox.Text.Length;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
            textBox.TextChanged += Phone_TextChanged;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите выйти из аккаунта?", "Подтверждение выхода из аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Hide();
                Auth auth = new Auth();
                auth.Show();
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите удалить свой аккаунт? Восстановить его будет невозможно.", "Подтверждение удаления аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (User user in _context.User)
                {
                    if (_user.Login == user.Login)
                    {
                        _context.User.Remove(user);
                    }
                }
                MessageBox.Show("Успешное удаление аккаунта!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                _context.SaveChanges();
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Hide();
                Auth auth = new Auth();
                auth.Show();
            }
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
                foreach (User user in _context.User)
                {
                    if (_user.IdUser == user.IdUser)
                    {
                        if (string.IsNullOrWhiteSpace(Surname.Text) || string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Patronymic.Text))
                        {
                            MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            string cleanedPhone = new string(Phone.Text.Where(char.IsDigit).ToArray());
                            if (user.Surname == Surname.Text && user.Name == Name.Text && user.Patronymic == Patronymic.Text && user.Phone == cleanedPhone)
                            {
                                MessageBox.Show("Введите данные для изменения!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                user.Surname = Surname.Text;
                                user.Name = Name.Text;
                                user.Patronymic = Patronymic.Text;

                                if (string.IsNullOrWhiteSpace(Phone.Text))
                                {
                                    user.Phone = null;
                                }
                                else
                                {
                                    user.Phone = cleanedPhone;
                                }

                                MessageBox.Show("Данные успешно изменены!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }

                _context.SaveChanges();

        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NowPassword.Password) || string.IsNullOrEmpty(NewPassword.Password) || string.IsNullOrEmpty(Password.Password))
                {
                    MessageBox.Show("Заполните все поля!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (NowPassword.Password == _user.Password)
                {
                    if (NewPassword.Password == Password.Password)
                    {
                        if (NewPassword.Password.Length >= 6)
                        {
                            foreach (User user in _context.User)
                            {
                                user.Password = NewPassword.Password;
                            }
                            _context.SaveChanges();
                            MessageBox.Show("Пароль успешно изменен!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            NowPassword.Password = "";
                            NewPassword.Password = "";
                            Password.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("Новый пароль должен содержать не менее 6 символов!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                            NewPassword.Password = "";
                            Password.Password = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Повторно введенный пароль не соответствует новому!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                        NewPassword.Password = "";
                        Password.Password = "";
                    }
                }
                else
                {
                    MessageBox.Show("Текущий пароль неверен!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                    NowPassword.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при изменении пароля: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void OutputData()
        {
            foreach (User user in _context.User)
            {
                if (_user.Login == user.Login)
                {
                    Name.Text = user.Name;
                    Surname.Text = user.Surname;
                    Patronymic.Text = user.Patronymic;
                    Phone.Text = user.Phone;
                    Email.Text = user.Login;
                }
            }
        }
    }
}
