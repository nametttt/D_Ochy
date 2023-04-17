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

        

        

        private void profile_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/CustomerPage.xaml", UriKind.Relative));
        }

        private void interest_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/Interests.xaml", UriKind.Relative));
        }
    }
}
