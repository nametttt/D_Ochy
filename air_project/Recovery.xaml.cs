using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Window
    {
        public Recovery()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                string userEmail = txtLogin.Text;

                User existingUser = db.User.FirstOrDefault(u => u.Login == userEmail);
                if (existingUser == null)
                {
                    MessageBox.Show("Пользователя с такой почтой не существует!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                CheckRecKod recKod = new CheckRecKod(txtLogin.Text);
                recKod.Show();
                this.Hide();
            }
        }
    }
}
