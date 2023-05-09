using air_project.pages;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        public AirTicketsEntities _context = new AirTicketsEntities();
        public User _user;

        public CustomerPage(User user)
        {
            InitializeComponent();
            _user = user;
            OutputData();
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

        private void UserInfo_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = About.Height;
            About.Visibility = Visibility.Visible;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Doc_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = Docs.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Visible;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Ticket_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = Tickets.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Visible;
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = Cards.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Visible;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string digitsOnly = Regex.Replace(textBox.Text, "[^0-9]", "");

            // Форматирование номера телефона
            string formattedNumber = string.Empty;
            int index = 0;

            if (digitsOnly.Length > 0)
            {
                formattedNumber = "+";
                formattedNumber += digitsOnly[index++];

                if (digitsOnly.Length > index)
                    formattedNumber += " (" + digitsOnly.Substring(index, Math.Min(3, digitsOnly.Length - index)) + ")";

                if (digitsOnly.Length > index + 3)
                    formattedNumber += " " + digitsOnly.Substring(index + 3, Math.Min(3, digitsOnly.Length - (index + 3)));

                if (digitsOnly.Length > index + 6)
                    formattedNumber += "-" + digitsOnly.Substring(index + 6, Math.Min(2, digitsOnly.Length - (index + 6)));

                if (digitsOnly.Length > index + 8)
                    formattedNumber += "-" + digitsOnly.Substring(index + 8, Math.Min(2, digitsOnly.Length - (index + 8)));
            }

            textBox.TextChanged -= Phone_TextChanged;
            textBox.Text = formattedNumber;
            textBox.CaretIndex = formattedNumber.Length;
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
                parentWindow.Close();
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
                _context.SaveChanges();
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Close();
                Auth auth = new Auth();
                auth.Show();
            }
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            foreach (User user in _context.User)
            {
                if (_user.Login == user.Login)
                {
                    if (string.IsNullOrWhiteSpace(Surname.Text) || string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Patronymic.Text))
                    {
                        MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            string cleanedPhone = new string(Phone.Text.Where(char.IsDigit).ToArray());
                            user.Phone = cleanedPhone;
                        }

                        MessageBox.Show("Данные успешно изменены!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string cardNumber = CardNum.Text;
                string expirationMonth = Month.Text;
                string expirationYear = Year.Text;
                string cvc = CVC.Password;
                string ownerName = Owner.Text;

                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(expirationMonth) ||
                    string.IsNullOrEmpty(expirationYear) || string.IsNullOrEmpty(cvc) ||
                    string.IsNullOrEmpty(ownerName))
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    Card newCard = new Card
                    {
                        IdCard = Convert.ToInt64(cardNumber),
                        Month = expirationMonth,
                        Year = Convert.ToInt32(expirationYear),
                        CW_CVC = Convert.ToInt32(cvc),
                        OwnerName = ownerName,
                        UserLogin = _user.Login
                    };

                    db.Card.Add(newCard);
                    db.SaveChanges();

                    CardNum.Text = string.Empty;
                    Month.Text = string.Empty;
                    Year.Text = string.Empty;
                    CVC.Password = string.Empty;
                    Owner.Text = string.Empty;
                    MessageBox.Show("Карта успешно добавлена!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении карты: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Owner_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("^[a-zA-Z]+$"); // Регулярное выражение для проверки только английских букв

            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }


        private void CardNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CardNum.FontSize = 22;

            TextBox textBox = (TextBox)sender;
            int maxLength = 16;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
            }
        }

        private bool IsNumericInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void Month_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Month.FontSize = 22;

            TextBox textBox = (TextBox)sender;
            int maxLength = 2;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
            }
        }

        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Year.FontSize = 22;

            TextBox textBox = (TextBox)sender;
            int maxLength = 2;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
            }
        }

        private void CVC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CVC.FontSize = 22;

            PasswordBox textBox = (PasswordBox)sender;
            int maxLength = 3;

            if (!IsNumericInput(e.Text) || textBox.Password.Length >= maxLength)
            {
                e.Handled = true;
            }
        }
    }
}