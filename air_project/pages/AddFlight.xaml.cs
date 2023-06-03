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
        int ticketCount = 6, numberseats = 36;
        public AddFlight()
        {
            InitializeComponent();
            UpdateFlight();
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
                foreach (Flight f in air.Flight)
                {
                    idFlight.Items.Add(f.IdFlight);
                }

            }
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
                                 Город_Отправления = city1.CityName,
                                 Дата_Отправления = flight.Departure_Date,
                                 Город_Прибытия = city2.CityName,
                                 Дата_Прибытия = flight.Arrival_Date,
                                 Места = flight.Seats_Number,
                                 Стоимость = flight.RetailValue
                             };
                datagrid.ItemsSource = query1.ToList();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateFlight();
        }

        private void AddMyTicket(int flightId, int ticketCount)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };

                    foreach (char letter in letters)
                    {
                        for (int i = 1; i <= ticketCount; i++)
                        {
                            string place = $"{letter}{i}";

                            Ticket ticket = new Ticket()
                            {
                                IdFlight = flightId,
                                Place = place,
                            };

                            air.Ticket.Add(ticket);
                        }
                    }

                    air.SaveChanges();
                    MessageBox.Show("Билеты успешно добавлены.", "Success!");
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении билетов.");
            }
        }

        private void AddMyFlight(int departureCityId, DateTime departureDate, int arrivalCityId, DateTime arrivalDate, int seatsNumber, int retailValue)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    Flight flight = new Flight()
                    {
                        Departure_City = departureCityId,
                        Departure_Date = departureDate,
                        Arrival_City = arrivalCityId,
                        Arrival_Date = arrivalDate,
                        Seats_Number = seatsNumber,
                        RetailValue = retailValue
                    };

                    air.Flight.Add(flight);
                    MessageBox.Show("Рейс успешно добавлен.", "Success!");

                    air.SaveChanges();

                    AddMyTicket(flight.IdFlight, ticketCount);


                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении рейса.");
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

                        AddMyFlight(departureCityId, Convert.ToDateTime(depdate.Text), arrivalCityId, Convert.ToDateTime(arrdate.Text), numberseats, Convert.ToInt32(cost.Text));

                        depcity.Text = "";
                        depdate.Text = "";
                        arrcity.Text = "";
                        cost.Text = "";
                        arrdate.Text = "";
                        UpdateFlight();

                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }

        private void UpdateFlight(int flightId, int departureCityId, DateTime departureDate, int arrivalCityId, DateTime arrivalDate, int seatsNumber, int retailValue)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var flight = air.Flight.FirstOrDefault(f => f.IdFlight == flightId);
                    if (flight != null)
                    {
                        flight.Departure_City = departureCityId;
                        flight.Departure_Date = departureDate;
                        flight.Arrival_City = arrivalCityId;
                        flight.Arrival_Date = arrivalDate;
                        flight.Seats_Number = seatsNumber;
                        flight.RetailValue = retailValue;

                        air.SaveChanges();

                        MessageBox.Show("Рейс успешно обновлен.", "Success!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при обновлении рейса.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idFlight.SelectedItem == null || depcity.Text == "" || depdate.Text == "" || arrcity.Text == "" || arrdate.Text == "" || cost.Text == "")
                    {
                        MessageBox.Show("Заполните все данные и выберите рейс!");
                    }
                    else
                    {
                        int flightId = (int)idFlight.SelectedItem;
                        int departureCityId = air.City.FirstOrDefault(c => c.CityName == depcity.Text)?.IdCity ?? 0;
                        int arrivalCityId = air.City.FirstOrDefault(c => c.CityName == arrcity.Text)?.IdCity ?? 0;

                        UpdateFlight(flightId, departureCityId, Convert.ToDateTime(depdate.Text), arrivalCityId, Convert.ToDateTime(arrdate.Text), numberseats, Convert.ToInt32(cost.Text));

                        depcity.Text = "";
                        depdate.Text = "";
                        arrcity.Text = "";
                        cost.Text = "";
                        arrdate.Text = "";
                        idFlight.SelectedItem = null;
                        UpdateFlight();

                    }
                }

            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }


        private void DeleteFlight(int flightId)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var flight = air.Flight.FirstOrDefault(f => f.IdFlight == flightId);
                    if (flight != null)
                    {
                        air.Flight.Remove(flight);
                        air.SaveChanges();
                        MessageBox.Show("Рейс успешно удален.", "Success!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении рейса.");
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idFlight.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите рейс для удаления!");
                    }
                    else
                    {
                        int flightId = (int)idFlight.SelectedItem;
                        DeleteFlight(flightId);
                        depcity.Text = "";
                        depdate.Text = "";
                        arrcity.Text = "";
                        cost.Text = "";
                        arrdate.Text = "";
                        idFlight.SelectedItem = null;
                        UpdateFlight();

                    }
                }

            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }

        private void idFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idFlight.SelectedItem != null)
                    {
                        int selectedFlightId = (int)idFlight.SelectedItem;

                        var flight = air.Flight.FirstOrDefault(f => f.IdFlight == selectedFlightId);
                        if (flight != null)
                        {
                            var departureCity = air.City.FirstOrDefault(c => c.IdCity == flight.Departure_City);
                            var arrivalCity = air.City.FirstOrDefault(c => c.IdCity == flight.Arrival_City);

                            if (departureCity != null)
                                depcity.Text = departureCity.CityName;

                            if (arrivalCity != null)
                                arrcity.Text = arrivalCity.CityName;

                            depdate.Text = flight.Departure_Date.ToString();
                            arrdate.Text = flight.Arrival_Date.ToString();
                            cost.Text = flight.RetailValue.ToString();
                            numberseats = flight.Seats_Number;
                        }

                        Add.IsEnabled = false;

                    }
                    else
                    {
                        depcity.Text = "";
                        arrcity.Text = "";
                        depdate.Text = "";
                        arrdate.Text = "";
                        cost.Text = "";
                        Add.IsEnabled = true;

                    }


                }
                UpdateFlight();

            }
            catch
            {
                MessageBox.Show("Произошла ошибка!");
            }

        }
    }
}
