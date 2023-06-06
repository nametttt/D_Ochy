using System;
using System.Collections.Generic;
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
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для AddFlight.xaml
    /// </summary>
    public partial class AddFlight : Page
    {
        int ticketCount = 6, numberseats = 36;
        ChangeFlight changeFlight = new ChangeFlight();
        public AddFlight()
        {
            InitializeComponent();
            UpdateFlight();
            depdate.Minimum = DateTime.Today.AddDays(1);
            depdate.Maximum = DateTime.Today.AddMonths(1);
            depdate.Value = depdate.Minimum;

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


        public bool AddMyFlight(int departureCityId, DateTime departureDate, int arrivalCityId, DateTime arrivalDate, int seatsNumber, int retailValue)
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

                    changeFlight.AddMyTicket(flight.IdFlight, ticketCount);

                    return true;

                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении рейса.");
                return false;

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

                        changeFlight.UpdateFlight(flightId, departureCityId, Convert.ToDateTime(depdate.Text), arrivalCityId, Convert.ToDateTime(arrdate.Text), numberseats, Convert.ToInt32(cost.Text));

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
                        changeFlight.DeleteFlight(flightId);
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

        private void depdate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (depdate.Value.HasValue)
                {
                    arrdate.Minimum = null;
                    arrdate.Maximum = null;


                    DateTime newDate = depdate.Value.Value.AddMinutes(10);
                    arrdate.Value = newDate;
                    arrdate.Minimum = newDate;
                    arrdate.Maximum = newDate.AddHours(18);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Clipboard.SetText(ex.Message);
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
