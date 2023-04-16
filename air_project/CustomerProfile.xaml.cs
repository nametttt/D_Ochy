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
    /// Логика взаимодействия для CustomerProfile.xaml
    /// </summary>
    public partial class CustomerProfile : Window
    {
        public CustomerProfile()
        {
            InitializeComponent();
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
    }
}
