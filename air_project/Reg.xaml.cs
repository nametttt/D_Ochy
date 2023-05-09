using air_project.pages;
using System;
using System.Collections.Generic;
using System.Linq;
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
using MessageBox = System.Windows.MessageBox;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public Reg()
        {
            InitializeComponent();
        }

        private bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) || string.IsNullOrEmpty(txtPatronymic.Text) || string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtPassword1.Password) || !sogl.IsChecked.GetValueOrDefault())
                {
                    MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    if(txtPassword.Password.Length >= 6 || txtPassword1.Password.Length >= 6)
                    {
                        if (txtPassword.Password == txtPassword1.Password)
                        {
                            string userEmail = txtLogin.Text;
                            if (IsEmailValid(userEmail))
                            {
                                using (AirTicketsEntities db = new AirTicketsEntities())
                                {
                                    if (db.User.Any(u => u.Login == txtLogin.Text))
                                    {
                                        MessageBox.Show("Пользователь с такой почтой уже существует!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                    else
                                    {
                                        GetHash g = new GetHash();
                                        MyUser myuser = new MyUser();
                                        myuser.Email = txtLogin.Text;
                                        myuser.UserName = txtName.Text;
                                        myuser.Surname = txtSurname.Text;
                                        myuser.Patronymic = txtPatronymic.Text;
                                        myuser.Password = g.GetHashString(txtPassword.Password);

                                        Confirmation confirm = new Confirmation(txtLogin.Text, myuser);
                                        confirm.Show();
                                        this.Hide();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Некорректный адрес электронной почты!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Длина пароля не менее 6 символов!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }
    }
}
