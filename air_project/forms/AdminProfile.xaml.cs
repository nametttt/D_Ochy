﻿using System;
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
        public User _user;
        
        public AdminProfile(User user)
        {
            InitializeComponent();
            _user = user;

        }

        public void LoadAdminPage()
        {
            MainHome mainhome = new MainHome(_user);
            MainContent.Content = mainhome;
        }

        private void exit_Selected(object sender, RoutedEventArgs e)
        {
                this.Hide();
                Auth auth = new Auth();
                auth.Show();
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

        private void changeticket_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/AddTicket.xaml", UriKind.Relative));
            Title = "Просмотр билетов";
        }

        private void addfly_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/AddFlight.xaml", UriKind.Relative));
            Title = "Управление рейсами";
        }


        private void usersWatch_Selected(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("/pages/AddUser.xaml", UriKind.Relative));
            Title = "Управление пользователями";
        }
    }
}
