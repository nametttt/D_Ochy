using air_project.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            using (AirTicketsEntities air = new AirTicketsEntities())
            {
                try
                {
                    string login = txtLogin.Text.Trim();
                    string password = txtPassword.Password.Trim();

                    if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                    {
                        MessageBox.Show("Пожалуйста, введите логин и пароль!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    foreach (User user in air.User)
                    {
                        GetHash g = new GetHash();
                        if (login == user.Login && password ==user.Password && user.Role == "Покупатель")
                        {
                            CustomerProfile w1 = new CustomerProfile(user);
                            w1.LoadCustomerPage();
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            w1.Show();
                            this.Hide();
                            return;
                        }
                        else if (login == user.Login && password == user.Password && user.Role == "Администратор")
                        {

                            AdminProfile admin = new AdminProfile(user);
                            admin.LoadAdminPage();
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            admin.Show();
                            this.Hide();
                            return;
                        }
                        else if (login == user.Login && password == user.Password && user.Role == "Бухгалтер")
                        {
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            BuhProfile buh = new BuhProfile(user);
                            buh.Show();
                            this.Hide();
                            return;
                        }
                    }

                    MessageBox.Show("Введен неверный логин или пароль!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    var i = ex;
                }
            }
    }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Reg_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Reg reg = new Reg();
            reg.Show();
            this.Hide();
        }

        private void Recovery_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Recovery recovery = new Recovery();
            recovery.Show();
            this.Hide();
        }

    }
}
