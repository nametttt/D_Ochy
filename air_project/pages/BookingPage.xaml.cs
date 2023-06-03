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
        public string ass = "";
        public List<string> number= new List<string> { };
        BuyTicket x = null;
        private int IDflight;

        public BookingPage(int idFlight)
        {
            InitializeComponent();
            FreePlace();
            IDflight = idFlight;
            MapTicketsToButtons();
        }


        public void FreePlace()
        {
            
        }

        public void MapTicketsToButtons()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                List<string> purchasedTicketPlaces = db.Purchases_Ticket
                    .Where(pt => pt.Ticket.IdFlight == IDflight)
                    .Select(pt => pt.Ticket.Place)
                    .ToList();

                foreach (Button button in YourContainer.Children.OfType<Button>())
                {
                    string place = button.Name;

                    if (purchasedTicketPlaces.Contains(place))
                    {
                        button.Background = new SolidColorBrush(Colors.Black);
                        button.IsEnabled = false;
                    }
                }
            }
        }


    private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            foreach (Window window in Application.Current.Windows)
            {
                if (window is BuyTicket)
                {
                    x = (BuyTicket)window;
                    break;
                }
            }

            
            btn = (Button)sender;

            Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF);
            Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E);


            if (btn.Background is SolidColorBrush brush && brush.Color == desiredColor)
            {
                string place = btn.Name.ToString();

                if (number.Count == x.Colvo)
                {
                    MessageBox.Show("Максимум");
                    return;
                }

                btn.Background = new SolidColorBrush(checkedColor);
                number.Add(place);

                ass += place + " ";
            }
            else
            {
                string place = btn.Name.ToString();

                btn.Background = new SolidColorBrush(desiredColor);
                ass = ass.Replace(place + " ", "");
                number.Remove(place);
            }
        }
    }
}
