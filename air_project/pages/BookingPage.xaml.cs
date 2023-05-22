using air_project.forms;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Color = System.Windows.Media.Color;

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page
    {
        Button btn;
        string ass = "";
        public BookingPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] number;
            btn = (Button)sender;

            Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF);
            Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E);

            if (btn.Background is SolidColorBrush brush && brush.Color == desiredColor)
            {
                btn.Background = new SolidColorBrush(checkedColor);
                number = btn.Name.Split('n');
                ass += number[1] + " ";
            }
            else
            {
                btn.Background = new SolidColorBrush(desiredColor);
                number = btn.Name.Split('n');
                ass = ass.Replace($"{number[1]} ", "");
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var x = (BuyTicket)Application.Current.MainWindow;
            string s = ass; 
            x.txtPlace.Text = s;
        }
    }
}
