using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace air_project.pages
{

    public class TicketT
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
                foreach (var i in db.City)
                {
                    txtFrom.Items.Add(i.CityName);
                    txtTo.Items.Add(i.CityName);
                }

            }
            //otpr.SelectedDate = DateTime.Now;
            //otpr.DisplayDateStart = DateTime.Today;
            otpr.DisplayDateEnd = DateTime.Today.AddMonths(1);
            otpr.DisplayDateEnd = DateTime.Today.AddMonths(1);

            
        }


        private void otpr_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            txtvilet.Text = otpr.SelectedDate.Value.Date.ToString("dd.MM.yyyy");
            otpr.Visibility = Visibility.Hidden;
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
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
            calendarPopup.PlacementTarget = txtvilet;
            }
            if (pass.Visibility == Visibility.Visible)
            {
                pass.Visibility = Visibility.Hidden;
                otpr.Visibility = Visibility.Visible;

            }

        }



        private void txtFrom_TextInput(object sender, TextCompositionEventArgs e)
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.City)
                {
                    var result = from item in db.City
                                 where item.CityName.StartsWith(txtFrom.Text)
                                 select item;

                    txtFrom.Items.Add(result);
                    MessageBox.Show(result.ToString());
                }

            }
        }

        private void btnSearchTickets_Click(object sender, RoutedEventArgs e)
        {
            tanushka.Visibility = Visibility.Visible;

            string from = txtFrom.Text;
            string to = txtTo.Text;
            City arr = new City();
            City dep = new City();
            string vilet = txtvilet.Text;
            DateTime date = new DateTime();
            if (vilet !="")
            {
                date = DateTime.Parse(vilet);
            }




            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                var tickets = new List<TicketT>();

                foreach (var i in db.City)
                {
                    if (i.CityName.ToLower() == from.ToLower())
                    {
                        dep = i;
                    }
                    if (i.CityName.ToLower() == to.ToLower())
                    {
                        arr = i;
                    }
                }

                foreach (var i in db.Flight)
                {
                    MessageBox.Show(date.ToString());

                    if (vilet == "")
                    {
                        if (i.Arrival_City == arr.IdCity && i.Departure_City == dep.IdCity)
                        {
                            var x = new TicketT() { Title = $"Полёт в {arr.CityName}", Price = 500.00, DepartureTime = i.Departure_Date, ArrivalTime = i.Arrival_Date, DepartureLocation = dep.CityName, ArrivalLocation = arr.CityName };
                            tickets.Add(x);
                            
                        }
                    }
                    else
                    {
                        if (i.Arrival_City == arr.IdCity && i.Departure_City == dep.IdCity && i.Departure_Date == date)
                        {
                            var x = new TicketT() { Title = $"Полёт в {arr.CityName}", Price = 500.00, DepartureTime = i.Departure_Date, ArrivalTime = i.Arrival_Date, DepartureLocation = dep.CityName, ArrivalLocation = arr.CityName };
                            tickets.Add(x);
                        }
                    }

                    
                }
                ticketListBox.ItemsSource = tickets;
            }
        }

        private void ticketListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("sss");
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pass.Visibility == Visibility.Visible)
            {
                pass.Visibility = Visibility.Hidden;
            }

            else
            {
                pass.Visibility = Visibility.Visible;
                passengerPopup.IsOpen = true;
                passengerPopup.StaysOpen = true;
            }

            if (otpr.Visibility == Visibility.Visible)
            {
                otpr.Visibility = Visibility.Hidden;
                pass.Visibility = Visibility.Visible;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch(btn.Name)
            {
                case ("btn_mn_1"):
                    if (txt_first.Text != "1")
                    {
                        txt_first.Text = (Convert.ToInt32(txt_first.Text)-1).ToString();
                        if (Convert.ToInt32(txt_first.Text) == 1 )
                        {
                            btn_mn_1.IsEnabled = false;
                            if (Convert.ToInt32(txt_third.Text) ==1)
                            {
                                btn_pl_3.IsEnabled = false;

                            }


                        }
                        if (Convert.ToUInt32(txt_first.Text) != 5)
                        {
                            btn_pl_1.IsEnabled = true;
                        }
                        if (Convert.ToUInt32(txt_first.Text) < Convert.ToUInt32(txt_third.Text))
                        {
                            txt_third.Text = (Convert.ToUInt32(txt_third.Text) - 1).ToString();
                            btn_pl_3.IsEnabled = false;
                        }
                    }
                    break;
                case ("btn_mn_2"):
                    if (txt_second.Text != "0")
                    {
                        txt_second.Text = (Convert.ToInt32(txt_second.Text) - 1).ToString();
                        if (Convert.ToInt32(txt_second.Text) == 0)
                        {
                            btn_mn_2.IsEnabled = false;
                        }
                        if (Convert.ToUInt32(txt_second.Text) != 5)
                        {
                            btn_pl_2.IsEnabled = true;
                        }
                    }
                    break;
                case ("btn_mn_3"):
                    if (txt_third.Text != "0")
                    {
                        txt_third.Text = (Convert.ToInt32(txt_third.Text) - 1).ToString();
                        if (Convert.ToInt32(txt_third.Text) == 0)
                        {
                            btn_mn_3.IsEnabled = false;
                        }
                        if (Convert.ToUInt32(txt_first.Text) != Convert.ToInt32(txt_third.Text))
                        {
                            btn_pl_3.IsEnabled = true;
                        }
                    }
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case ("btn_pl_1"):
                    if (Convert.ToUInt32(txt_first.Text) == 4)
                    {
                        btn_pl_1.IsEnabled = false;
                    }
                        txt_first.Text = (Convert.ToInt32(txt_first.Text) + 1).ToString();
                    if (Convert.ToUInt32(txt_first.Text) > 1)
                    {
                        btn_mn_1.IsEnabled = true;
                    }
                    if (Convert.ToUInt32(txt_first.Text) > Convert.ToUInt32(txt_third.Text))
                    {
                        btn_pl_3.IsEnabled = true;
                    }
                    if (Convert.ToUInt32(txt_first.Text) < Convert.ToUInt32(txt_third.Text)|| (Convert.ToUInt32(txt_first.Text)==1 && Convert.ToUInt32(txt_third.Text)==1))
                    {
                        txt_third.Text = (Convert.ToUInt32(txt_third.Text) - 1).ToString();
                        btn_pl_3.IsEnabled = false;
                    }
                    break;
                case ("btn_pl_2"):
                    if (Convert.ToUInt32(txt_second.Text) == 4)
                    {
                        btn_pl_2.IsEnabled = false;
                    }
                    else { btn_pl_2.IsEnabled = true; }
                    txt_second.Text = (Convert.ToInt32(txt_second.Text) + 1).ToString();
                    if (Convert.ToInt32(txt_second.Text) >= 1)
                    {
                        btn_mn_2.IsEnabled = true;
                    }
                    break;
                case ("btn_pl_3"):

                    if (Convert.ToUInt32(txt_first.Text) - 1 == Convert.ToUInt32(txt_third.Text))
                    {
                        btn_pl_3.IsEnabled = false;
                    }

                    if (Convert.ToUInt32(txt_third.Text) == 4)
                    {
                        btn_pl_3.IsEnabled = false;
                    }
                    txt_third.Text = (Convert.ToInt32(txt_third.Text) + 1).ToString();
                    if (Convert.ToInt32(txt_third.Text) >= 1)
                    {
                        btn_mn_3.IsEnabled = true;
                    }
                    if (Convert.ToUInt32(txt_first.Text) < Convert.ToUInt32(txt_third.Text))
                    {
                        txt_third.Text = (Convert.ToUInt32(txt_third.Text) - 1).ToString();
                        btn_pl_3.IsEnabled = false;
                    }
                    break;
            }
        }
    }
}
