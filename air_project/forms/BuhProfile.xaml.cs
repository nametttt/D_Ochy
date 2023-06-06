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
    /// Логика взаимодействия для BuhProfile.xaml
    /// </summary>
    public partial class BuhProfile : Window
    {
        public User _user;
        public BuhProfile(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void watchtickets_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/AddTicket.xaml", UriKind.Relative));
            Title = "Просмтр билетов";
        }

        private void doc_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/Documentary.xaml", UriKind.Relative));
            Title = "Ведение документации";
        }

        private void watchfly_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/WatchFlight.xaml", UriKind.Relative));
            Title = "Просмотр рейсов";
        }

        private void exit_Selected(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth(); 
            this.Hide();
            auth.Show();
        }

        public void LoadAdminPage()
        {
            MainHome mainhome = new MainHome(_user);
            MainContent.Content = mainhome;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }

        private void admin_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Source = new Uri("/pages/MainHome.xaml", UriKind.Relative);
            MainContent.NavigationService.Navigate(new MainHome(_user));
        }
    }
}
