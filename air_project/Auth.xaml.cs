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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                    foreach (User user in air.User)
                    {
                        GetHash g = new GetHash();
                        if (txtLogin.Text == user.Login && g.GetHashString(txtPassword.Password) == user.Password && user.Role == "Покупатель")
                        {
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            CustomerProfile w1 = new CustomerProfile(user);
                            w1.Show();
                            this.Hide();
                            return;
                        }

                        else if (txtLogin.Text == user.Login && (txtPassword.Password) == user.Password && user.Role == "Администратор")
                        {
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            AdminProfile admin = new AdminProfile(user);
                            admin.Show();
                            this.Hide();
                            return;
                        }
                        else if (txtLogin.Text == user.Login && (txtPassword.Password) == user.Password && user.Role == "Бухгалтер")
                        {
                            MessageBox.Show($"Добро пожаловать, {user.Name}!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            BuhProfile buh = new BuhProfile(user);
                            buh.Show();
                            this.Hide();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Пользователь еще не зарегистрирован!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
