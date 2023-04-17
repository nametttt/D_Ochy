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
    /// Логика взаимодействия для Document.xaml
    /// </summary>
    public partial class Document : Window
    {
        public Document()
        {
            InitializeComponent();
        }

        private void watchtickets_Selected(object sender, RoutedEventArgs e)
        {
            WatchTickets buh = new WatchTickets();
            this.Hide();
            buh.Show();
        }

        private void admin_Selected(object sender, RoutedEventArgs e)
        {
            BuhProfile buh = new BuhProfile();
            this.Hide();
            buh.Show();
        }

        private void watchfly_Selected(object sender, RoutedEventArgs e)
        {
            WatchFlights buh = new WatchFlights();
            this.Hide();
            buh.Show();
        }

        private void exit_Selected(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            this.Hide();
            auth.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
