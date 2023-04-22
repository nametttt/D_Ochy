using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

    public class Ticket
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
    }



    public partial class MainCustomer : Page
    {
        public MainCustomer()
        {
            InitializeComponent();

            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.Cities)
                {
                    txtFrom.Items.Add(i.CityName);
                    txtTo.Items.Add(i.CityName);
                }

            }
            otpr.SelectedDate = DateTime.Now;
            prilet.SelectedDate = DateTime.Now;
            otpr.DisplayDateStart = DateTime.Today;
            otpr.DisplayDateEnd = DateTime.Today.AddMonths(1);
            prilet.DisplayDateStart = DateTime.Today;
            otpr.DisplayDateEnd = DateTime.Today.AddMonths(1);


            var tickets = new List<Ticket>()
            {
                new Ticket() { Title = "Flight to Paris", Price = 500.00, DepartureTime = new DateTime(2023, 05, 01, 10, 30, 00), ArrivalTime = new DateTime(2023, 05, 01, 14, 00, 00), DepartureLocation = "New York", ArrivalLocation = "Paris" },
                new Ticket() { Title = "Train to Amsterdam", Price = 50.00, DepartureTime = new DateTime(2023, 05, 03, 08, 00, 00), ArrivalTime = new DateTime(2023, 05, 03, 12, 00, 00), DepartureLocation = "Berlin", ArrivalLocation = "Amsterdam" },
                new Ticket() { Title = "Bus to Rome", Price = 20.00, DepartureTime = new DateTime(2023, 05, 05, 14, 00, 00), ArrivalTime = new DateTime(2023, 05, 05, 18, 00, 00), DepartureLocation = "Florence", ArrivalLocation = "Rome" }
            };

            ticketListBox.ItemsSource = tickets;
        }

        private void otpr_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            txtvilet.Text = otpr.SelectedDate.Value.Date.ToString("dd.MM.yyyy");
        }

        private void prilet_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            txtottuda.Text = prilet.SelectedDate.Value.ToString("dd.MM.yyyy");
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            otpr.Visibility = Visibility.Visible;
            calendarPopup.IsOpen = true;
            calendarPopup.PlacementTarget = txtvilet;
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            prilet.Visibility = Visibility.Visible;
            calenPop.IsOpen = true;
            calenPop.PlacementTarget = txtottuda;
        }



        private void txtFrom_TextInput(object sender, TextCompositionEventArgs e)
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.Cities)
                {
                    var result = from item in db.Cities
                                 where item.CityName.StartsWith(txtFrom.Text)
                                 select item;

                    txtFrom.Items.Add(result);
                    MessageBox.Show(result.ToString());
                }

            }
        }
    }
}
