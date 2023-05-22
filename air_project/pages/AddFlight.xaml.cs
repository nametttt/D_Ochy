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
    /// Логика взаимодействия для AddFlight.xaml
    /// </summary>
    public partial class AddFlight : Page
    {
        public AddFlight()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (AirTicketsEntities air = new AirTicketsEntities())
            {
                foreach (City city in air.City)
                {
                    depcity.Items.Add(city.CityName);
                }
                foreach (City city in air.City)
                {
                    arrcity.Items.Add(city.CityName);
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (AirTicketsEntities air = new AirTicketsEntities())
            {
                var query1 = from city1 in air.City
                             from city2 in air.City
                             from flight in air.Flight
                             where flight.Departure_City == city1.IdCity && flight.Arrival_City == city2.IdCity
                             select new
                             {
                                 Номер = flight.IdFlight,
                                 Город_Отправления = city1.CityName,
                                 Дата_Отправления = flight.Departure_Date,
                                 Город_Прибытия = city2.CityName,
                                 Дата_Прибытия = flight.Arrival_Date,
                                 Места = flight.Seats_Number
                             };
                datagrid.ItemsSource = query1.ToList();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (depcity.Text == "" || depdate.Text == "" || arrcity.Text == "" || arrdate.Text == "" || cost.Text == "")
                    {
                        MessageBox.Show("Заполните все данные!");
                    }
                    else
                    {
                        var departureCity = air.City.FirstOrDefault(c => c.CityName == depcity.Text);
                        int departureCityId = departureCity != null ? departureCity.IdCity : 0;

                        var arrivalCity = air.City.FirstOrDefault(c => c.CityName == arrcity.Text);
                        int arrivalCityId = arrivalCity != null ? arrivalCity.IdCity : 0;

                        Flight flight = new Flight()
                        {
                            Departure_City = departureCityId,
                            Departure_Date = Convert.ToDateTime(depdate.Text),
                            Arrival_City = arrivalCityId,
                            Arrival_Date = Convert.ToDateTime(arrdate.Text),
                            Seats_Number = 34
                        };

                        air.Flight.Add(flight);
                        air.SaveChanges();

                        for (int i = 1; i <= 34; i++)
                        {
                            Ticket ticket = new Ticket()
                            {
                                IdFlight = flight.IdFlight,
                                Place = i,
                                Cost = Convert.ToDecimal(cost.Text)
                            };

                            air.Ticket.Add(ticket);
                        }

                        air.SaveChanges();
                        MessageBox.Show($"Добавление прошло успешно.", "Success!");
                        depcity.Text = "";
                        depdate.Text = "";
                        arrcity.Text = "";
                        cost.Text = "";
                        arrdate.Text = "";
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }

        }
    }
}
