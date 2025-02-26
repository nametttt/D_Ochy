﻿using air_project.pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для CustomerProfile.xaml
    /// </summary>
    public partial class CustomerProfile : Window
    {
        public User _user;
        public CustomerProfile(User user)
        {
            InitializeComponent();
            _user = user;
        }

        public void LoadCustomerPage()
        {
            CustomerPage customerPage = new CustomerPage(_user);
            MainContent.Content = customerPage;
        }

        private void Profile_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Source = new Uri("/pages/CustomerPage.xaml", UriKind.Relative);
            MainContent.NavigationService.Navigate(new CustomerPage(_user));
            Title = "Личный кабинет";
        }

        private void Interest_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/Interests.xaml", UriKind.Relative));
            MainContent.NavigationService.Navigate(new Interests(_user));
            Title = "Интересное";
        }

        private void TicketsBuy_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/MainCustomer.xaml", UriKind.Relative));
            MainContent.NavigationService.Navigate(new MainCustomer(_user));
            Title = "Поиск билетов";
        }

        private void Exit_Selected(object sender, RoutedEventArgs e)
        {
                this.Hide();
                Auth auth = new Auth(); 
                auth.Show();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            Auth auth = new Auth();
            auth.Show();
        }
    }
}
