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

            if (btn.Background is SolidColorBrush brush && brush.Color == Colors.Green)
            {
                btn.Background = new SolidColorBrush(Colors.Red);
                number = btn.Name.Split('n');
                ass += number[1]+" ";
            }
            else
            {
                btn.Background = new SolidColorBrush(Colors.Green);
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
