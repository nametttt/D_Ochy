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

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для WatchFlight.xaml
    /// </summary>
    public partial class WatchFlight : Page
    {
        public WatchFlight()
        {
            InitializeComponent();
            UpdateFlight();
        }

        public void UpdateFlight()
        {
            datagrid.ItemsSource = null;
            using (AirTicketsEntities air = new AirTicketsEntities())
            {
                var query1 = from city1 in air.City
                             from city2 in air.City
                             from flight in air.Flight
                             where flight.Departure_City == city1.IdCity && flight.Arrival_City == city2.IdCity
                             select new
                             {
                                 Номер = flight.IdFlight,
                                 Откуда = city1.CityName,
                                 Отправление = flight.Departure_Date,
                                 Куда = city2.CityName,
                                 Прибытие = flight.Arrival_Date,
                                 Места = flight.Seats_Number,
                                 Стоимость = flight.RetailValue
                             };

                var flights = query1.ToList();

                var formattedFlights = flights.Select(f => new
                {
                    Номер = f.Номер,
                    Откуда = f.Откуда,
                    Отправление = f.Отправление.ToString("dd.MM.yyyy HH:mm"),
                    Куда = f.Куда,
                    Прибытие = f.Прибытие.ToString("dd.MM.yyyy HH:mm"),
                    Места = f.Места,
                    Стоимость = f.Стоимость
                });

                datagrid.ItemsSource = formattedFlights.ToList();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateFlight();
        }
    }
}
