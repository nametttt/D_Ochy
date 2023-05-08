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
    /// Логика взаимодействия для AdminProfile.xaml
    /// </summary>
    public partial class AdminProfile : Window
    {
        public AdminProfile(User user)
        {
            InitializeComponent();
        }

        private void exit_Selected(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            this.Hide();
            auth.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }
    }
}
