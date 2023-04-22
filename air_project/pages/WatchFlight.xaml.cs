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
    public partial class Ticket
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для WatchFlight.xaml
    /// </summary>
    public partial class WatchFlight : Page
    {
        public WatchFlight()
        {
            InitializeComponent();

            var tickets = new List<Ticket>()
            {
                new Ticket() { Title = "Flight to Paris", Price = 500.00, DepartureTime = new DateTime(2023, 05, 01, 10, 30, 00), ArrivalTime = new DateTime(2023, 05, 01, 14, 00, 00), DepartureLocation = "New York", ArrivalLocation = "Paris" },
                new Ticket() { Title = "Train to Amsterdam", Price = 50.00, DepartureTime = new DateTime(2023, 05, 03, 08, 00, 00), ArrivalTime = new DateTime(2023, 05, 03, 12, 00, 00), DepartureLocation = "Berlin", ArrivalLocation = "Amsterdam" },
                new Ticket() { Title = "Bus to Rome", Price = 20.00, DepartureTime = new DateTime(2023, 05, 05, 14, 00, 00), ArrivalTime = new DateTime(2023, 05, 05, 18, 00, 00), DepartureLocation = "Florence", ArrivalLocation = "Rome" }
            };

            ticketListBox.ItemsSource = tickets;
        }
    }
}
