using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для MyDocs.xaml
    /// </summary>
    public partial class MyDocs : UserControl
    {
        Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF); // ne
        Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E); //da
        Button btn;
        public MyDocs()
        {
            InitializeComponent();

            otpr.DisplayDateEnd = DateTime.Today;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] number;
            btn = (Button)sender;
            

            
            switch(btn.Name)
            {
                case ("male"):
                    if (btn.Background is SolidColorBrush brush && brush.Color == desiredColor)
                    {
                        btn.Background = new SolidColorBrush(checkedColor);
                        female.Background = new SolidColorBrush(desiredColor);
                    }
                    break;

                case ("female"):
                    if (btn.Background is SolidColorBrush s && s.Color == desiredColor)
                    {
                        btn.Background = new SolidColorBrush(checkedColor);
                        male.Background = new SolidColorBrush(desiredColor);
                    }
                    break;
            }
        }

        private void male1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            if (grid != null)
            {
                switch (grid.Name)
                {
                    case ("male1"):
                        if (male.Background is SolidColorBrush brush && brush.Color == desiredColor)
                        {
                            male.Background = new SolidColorBrush(checkedColor);
                            female.Background = new SolidColorBrush(desiredColor);
                        }
                        break;

                    case ("female1"):
                        if (female.Background is SolidColorBrush s && s.Color == desiredColor)
                        {
                            female.Background = new SolidColorBrush(checkedColor);
                            male.Background = new SolidColorBrush(desiredColor);
                        }
                        break;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using(AirTicketsEntities air = new AirTicketsEntities())
            {
                foreach (Country country in air.Country)
                {
                    countries.Items.Add(country.CountryName);
                }
                foreach (Type_Document type in air.Type_Document)
                {
                    typeDoc.Items.Add(type.Type);
                }
            }
        }

        private void otpr_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Birthday.Text = otpr.SelectedDate.Value.Date.ToString("dd.MM.yyyy");
            otpr.Visibility = Visibility.Hidden;
        }

        private void myGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (otpr.Visibility == Visibility.Visible)
            {
                otpr.Visibility = Visibility.Hidden;

            }
            else
            {
                otpr.Visibility = Visibility.Visible;
                calendarPopup.IsOpen = true;
                calendarPopup.StaysOpen = true;
            }

        }
    }
}
