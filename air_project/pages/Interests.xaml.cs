using air_project.pages;
using HamburgerMenu;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для Interests.xaml
    /// </summary>
    public partial class Interests : Page
    {
        public User _user;
        public Interests(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void ChangeMainContent(string destination)
        {
            Window mainWindow = Window.GetWindow(this);
            ContentControl mainContentControl = mainWindow.FindName("MainContent") as ContentControl;
            HamburgerMenuItem interestMenuItem = mainWindow.FindName("ticketsBuy") as HamburgerMenuItem;

            if (mainContentControl != null && interestMenuItem != null)
            {
                interestMenuItem.IsSelected = true;
                MainCustomer cust = new MainCustomer(_user);
                mainContentControl.Content = cust;
                cust.txtTo.SelectedItem = destination;
            }
        }

        private void btnMost_Click(object sender, RoutedEventArgs e)
        {
            ChangeMainContent("Санкт-Петербург");
        }

        private void btnMoscow_Click(object sender, RoutedEventArgs e)
        {
            ChangeMainContent("Москва");
        }

        private void btnElbrys_Click(object sender, RoutedEventArgs e)
        {
            ChangeMainContent("Нальчик");
        }

    }
}
