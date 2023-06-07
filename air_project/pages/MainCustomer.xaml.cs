using air_project.forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using static air_project.forms.BuyTicket;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace air_project.pages
{

    public partial class MainCustomer : Page
    {
        public User _user;
        public int cost;
        TicketT t = new TicketT();
        public MainCustomer(User user)
        {
            InitializeComponent();
            _user = user;

            LoadCities();

            otpr.DisplayDateEnd = DateTime.Today.AddMonths(1);
            otpr.DisplayDateStart = DateTime.Today;



        }

        public void LoadCities()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.City)
                {
                    txtFrom.Items.Add(i.CityName);
                    txtTo.Items.Add(i.CityName);
                }

            }
        }


        private void otpr_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkboxNoDate.IsChecked != true)
            {
                txtvilet.Text = otpr.SelectedDate?.Date.ToString("dd.MM.yyyy");
                opop.Visibility = Visibility.Hidden;
            }
            else
            {
                txtvilet.Text = "";
            }
        }



        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (opop.Visibility == Visibility.Visible)
            {
                opop.Visibility = Visibility.Hidden;
            }
            else
            {
                opop.Visibility = Visibility.Visible;
            calendarPopup.IsOpen = true;
            calendarPopup.StaysOpen = true;
            calendarPopup.PlacementTarget = txtvilet;
            }
            if (pass.Visibility == Visibility.Visible)
            {
                pass.Visibility = Visibility.Hidden;
                opop.Visibility = Visibility.Visible;

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
            if (txtFrom.SelectedItem == null || txtTo.SelectedItem == null)
            {
                MessageBox.Show("Выберите маршрут!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                string from = txtFrom.Text;
                string to = txtTo.Text;
                City arr = new City();
                City dep = new City();
                string vilet = txtvilet.Text;
                int vzr = Convert.ToInt32(txt_first.Text);
                int podr = Convert.ToInt32(txt_second.Text);
                int deti = Convert.ToInt32(txt_third.Text);

                bool isExist = false;

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

                        if (vilet == "")
                        {


                            if (i.Arrival_City == arr.IdCity && i.Departure_City == dep.IdCity && i.Departure_Date > DateTime.Now)
                            {
                                tanushka.Visibility = Visibility.Visible;
                                NotFound.Visibility = Visibility.Collapsed;
                                isExist = true;
                                int t = 0;
                                int cost = 0;

                                foreach (var j in db.Ticket)
                                {
                                    if (j.IdFlight == i.IdFlight)
                                    {
                                        t++;
                                        cost = i.RetailValue;
                                        bool isTicketPurchased = false; // Флаг, указывающий, был ли билет куплен

                                        foreach (var pt in db.Purchases_Ticket)
                                        {
                                            if (pt.IdTicket == j.IdTicket)
                                            {
                                                isTicketPurchased = true;
                                                break;
                                            }
                                        }

                                        if (isTicketPurchased)
                                        {
                                            t--; // Уменьшаем значение t, если билет уже был куплен
                                        }
                                    }
                                }

                                if (t >= vzr + podr)
                                {
                                    TimeSpan duration = i.Arrival_Date.Subtract(i.Departure_Date);
                                    string dur;

                                    if (duration.Hours > 0)
                                    {
                                        dur = string.Format($"{duration.Hours} часов {duration.Minutes} минут в пути", duration);
                                    }
                                    else if (duration.Days > 0)
                                    {
                                        if (duration.Hours > 0)
                                        {
                                            dur = string.Format($"{duration.Days} дней, {duration.Hours} часов, {duration.Minutes} минут в пути", duration);
                                        }
                                        else
                                        {
                                            dur = string.Format($"{duration.Days} дней, {duration.Minutes} минут в пути", duration);
                                        }
                                    }
                                    else
                                    {
                                        dur = string.Format($"{duration.Minutes} минут в пути", duration);
                                    }

                                    var x = new TicketT()
                                    {
                                        TicketID = i.IdFlight,
                                        SeatsFree = $"Свободно {t} мест",
                                        Rise = $"Рейс №{i.IdFlight}",
                                        Title = $"Полёт в {arr.CityName}",
                                        Price = cost,
                                        MPrice = $"{cost} рублей",
                                        DepartureTime = i.Departure_Date,
                                        ArrivalTime = i.Arrival_Date,
                                        DepartureLocation = dep.CityName,
                                        ArrivalLocation = arr.CityName,
                                        Mars = $"{dep.CityName} → {arr.CityName}",
                                        Raznitsa = dur,
                                        DateTimeLet = $"{i.Departure_Date.ToString("dd MMMM HH:mm")} → {i.Arrival_Date.ToString("dd MMMM HH:mm")}"
                                    };
                                    tickets.Add(x);
                                }
                            }

                        }
                        else
                        {
                            string departureDateString = i.Departure_Date.ToString("dd.MM.yyyy");
                            if (i.Arrival_City == arr.IdCity && i.Departure_City == dep.IdCity && departureDateString == vilet && i.Departure_Date > DateTime.Now)
                            {
                                isExist = true;
                                tanushka.Visibility = Visibility.Visible;
                                NotFound.Visibility = Visibility.Collapsed;
                                int t = 0;
                                int cost = 0;

                                foreach (var j in db.Ticket)
                                {
                                    if (j.IdFlight == i.IdFlight)
                                    {
                                        t++;
                                        cost = i.RetailValue;
                                        bool isTicketPurchased = false; // Флаг, указывающий, был ли билет куплен

                                        foreach (var pt in db.Purchases_Ticket)
                                        {
                                            if (pt.IdTicket == j.IdTicket)
                                            {
                                                isTicketPurchased = true;
                                                break;
                                            }
                                        }

                                        if (isTicketPurchased)
                                        {
                                            t--; // Уменьшаем значение t, если билет уже был куплен
                                        }
                                    }
                                }

                                if (t >= vzr + podr)
                                {
                                    TimeSpan duration = i.Arrival_Date.Subtract(i.Departure_Date);
                                    string dur;

                                    if (duration.Hours > 0)
                                    {
                                        dur = string.Format($"{duration.Hours} часов {duration.Minutes} минут в пути", duration);
                                    }
                                    else if (duration.Days > 0)
                                    {
                                        if (duration.Hours > 0)
                                        {
                                            dur = string.Format($"{duration.Days} дней, {duration.Hours} часов, {duration.Minutes} минут в пути", duration);
                                        }
                                        else
                                        {
                                            dur = string.Format($"{duration.Days} дней, {duration.Minutes} минут в пути", duration);
                                        }
                                    }
                                    else
                                    {
                                        dur = string.Format($"{duration.Minutes} минут в пути", duration);
                                    }

                                    var x = new TicketT()
                                    {
                                        TicketID = i.IdFlight,
                                        SeatsFree = $"Свободно {t} мест",
                                        Title = $"Полёт в {arr.CityName}",
                                        Price = cost,
                                        MPrice = $"{cost} рублей",
                                        DepartureTime = i.Departure_Date,
                                        ArrivalTime = i.Arrival_Date,
                                        DepartureLocation = dep.CityName,
                                        ArrivalLocation = arr.CityName,
                                        Mars = $"{dep.CityName} → {arr.CityName}",
                                        Raznitsa = dur,
                                        Rise = $"Рейс №{i.IdFlight}",
                                        DateTimeLet = $"{i.Departure_Date.ToString("dd MMMM HH:mm")} → {i.Arrival_Date.ToString("dd MMMM HH:mm")}"
                                    };
                                    tickets.Add(x);
                                }
                            }
                        }



                    }
                    if(!isExist)
                    {
                            NotFound.Visibility = Visibility.Visible;
                            tanushka.Visibility = Visibility.Collapsed;
                    }
                    ticketListBox.ItemsSource = tickets;
                }
            }
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
                
            if (opop.Visibility == Visibility.Visible)
            {
                opop.Visibility = Visibility.Hidden;
                pass.Visibility = Visibility.Visible;

            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btn_mn_1":
                    if (txt_first.Text != "1")
                    {
                        txt_first.Text = (Convert.ToInt32(txt_first.Text) - 1).ToString();
                        if (Convert.ToInt32(txt_first.Text) == 1)
                        {
                            btn_mn_1.IsEnabled = false;
                            if (Convert.ToInt32(txt_third.Text) == 1)
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
                case "btn_mn_2":
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
                case "btn_mn_3":
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

            UpdateTotalSeats();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btn_pl_1":
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
                    if (Convert.ToUInt32(txt_first.Text) < Convert.ToUInt32(txt_third.Text) || (Convert.ToUInt32(txt_first.Text) == 1 && Convert.ToUInt32(txt_third.Text) == 1))
                    {
                        txt_third.Text = (Convert.ToUInt32(txt_third.Text) - 1).ToString();
                        btn_pl_3.IsEnabled = false;
                    }
                    break;
                case "btn_pl_2":
                    if (Convert.ToUInt32(txt_second.Text) == 4)
                    {
                        btn_pl_2.IsEnabled = false;
                    }
                    else
                    {
                        btn_pl_2.IsEnabled = true;
                    }
                    txt_second.Text = (Convert.ToInt32(txt_second.Text) + 1).ToString();
                    if (Convert.ToInt32(txt_second.Text) >= 1)
                    {
                        btn_mn_2.IsEnabled = true;
                    }
                    break;
                case "btn_pl_3":
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

            UpdateTotalSeats();
        }

        private void UpdateTotalSeats()
        {
            int totalSeats = Convert.ToInt32(txt_first.Text) + Convert.ToInt32(txt_second.Text) + Convert.ToInt32(txt_third.Text);
            txtNumSeats.Text = $"{totalSeats} пассажир(ов)";
        }


        private void checkboxNoDate_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            bool isChecked = checkbox.IsChecked ?? false;

            if (isChecked)
            {
                otpr.SelectedDate = null; 
                otpr.IsEnabled = false;
            }
            else
            {
                otpr.IsEnabled = true;
            }
        }



        private void ticketListBox_Selected(object sender, RoutedEventArgs e)
        {
            try
            {

                int vzr = Convert.ToInt32(txt_first.Text);
                int podr = Convert.ToInt32(txt_second.Text);
                int deti = Convert.ToInt32(txt_third.Text);
                TicketT tick = (TicketT)ticketListBox.SelectedItem;
                BookingPage booking = new BookingPage(tick.TicketID);
               


                TimeSpan duration = tick.ArrivalTime.Subtract(tick.DepartureTime);
                string dur;

                if (duration.Hours > 0)
                {
                    dur = string.Format($"{duration.Hours} часов {duration.Minutes} минут в пути", duration);
                }
                else if (duration.Days > 0)
                {
                    if (duration.Hours > 0)
                    {
                        dur = string.Format($"{duration.Days} дней, {duration.Hours} часов, {duration.Minutes} минут в пути", duration);
                    }
                    else
                    {
                        dur = string.Format($"{duration.Days} дней, {duration.Minutes} минут в пути", duration);
                    }
                }
                else
                {
                    dur = string.Format($"{duration.Minutes} минут в пути", duration);
                }

                BuyTicket bt = new BuyTicket(_user, tick.TicketID, vzr + podr + deti);

                WrapPanel passengersWrapPanel = new WrapPanel();
                passengersWrapPanel.Orientation = Orientation.Horizontal;
                passengersWrapPanel.Name = "xer";
                passengersWrapPanel.HorizontalAlignment = HorizontalAlignment.Center;

                for (int i = 0; i < vzr + podr + deti; i++)
                {
                    Border passengerBorder = new Border();
                    passengerBorder.CornerRadius = new CornerRadius(10);
                    passengerBorder.BorderThickness = new Thickness(1);
                    passengerBorder.Background = new SolidColorBrush(Color.FromArgb(255, 250, 227, 213));
                    passengerBorder.VerticalAlignment = VerticalAlignment.Top;
                    passengerBorder.Margin = new Thickness(0, 0, 0, 30);
                    passengerBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 204, 119, 94));

                    StackPanel passengerStackPanel = new StackPanel();
                    passengerStackPanel.Width = 900;
                    passengerStackPanel.Name = $"tanusha{i + 1}";
                    passengerStackPanel.Margin = new Thickness(10, 10, 10, 15);

                    TextBlock passengerInfoTextBlock = new TextBlock();
                    passengerInfoTextBlock.Text = "Информация о пассажире";
                    passengerInfoTextBlock.FontSize = 21;
                    passengerInfoTextBlock.FontFamily = new FontFamily("Candara");
                    passengerInfoTextBlock.Margin = new Thickness(10);
                    passengerInfoTextBlock.FontWeight = FontWeights.SemiBold;

                    TextBlock passengerDataTextBlock = new TextBlock();
                    passengerDataTextBlock.Text = "Заполните данные вручную или выберите данные пассажира из личного кабинета";
                    passengerDataTextBlock.FontSize = 18;
                    passengerDataTextBlock.Margin = new Thickness(10);
                    passengerDataTextBlock.FontFamily = new FontFamily("Candara");

                    ComboBox documentsComboBox = new ComboBox();
                    documentsComboBox.Style = (Style)FindResource("Combo");
                    documentsComboBox.Name = $"documentsCombo{i + 1}";
                    documentsComboBox.IsEditable = true;
                    documentsComboBox.Height = 38;
                    documentsComboBox.Text = "Выберите документ";
                    documentsComboBox.Margin = new Thickness(20, 10, 20, 0);

                    documentsComboBox.SelectionChanged += bt.documentsCombo_SelectionChanged;

                    bool isCountry = false;
                    List<PassengerDocument> passengerDocuments = new List<PassengerDocument>();


                    using (AirTicketsEntities db = new AirTicketsEntities())
                    {
                        
                        int depcity = 0, arrcity = 0, depcountry = 0, arrcountry = 0;
                        foreach(City city in db.City)
                        {
                            if (city.CityName == tick.DepartureLocation)
                            {
                                depcity = city.IdCity;
                            }
                            if(city.CityName == tick.ArrivalLocation)
                            {
                                arrcity = city.IdCity;
                            }
                        }
                        foreach(Country country in db.Country)
                        {
                            if (depcity == country.IdCountry)
                            {
                                depcountry = country.IdCountry;
                            }
                            if(arrcountry == country.IdCountry)
                            {
                                arrcountry = country.IdCountry;
                            }
                        }
                        if(depcountry != arrcountry)
                        {
                            isCountry = true;
                            foreach (var passenger in db.Passenger)
                            {
                                if (passenger.IdUser == _user.IdUser)
                                {
                                    foreach (var document in db.Document)
                                    {
                                        if (passenger.IdPassenger == document.IdPassenger)
                                        {
                                            foreach(Type_Document type in db.Type_Document)
                                            {
                                                if (type.Type == "Загранпаспорт")
                                                {
                                                    PassengerDocument passengerDocument = new PassengerDocument
                                                    {
                                                        IdPassenger = passenger.IdPassenger,
                                                        IdDocument = document.IdDocument,
                                                        DisplayName = $"{document.Type_Document.Type}: {passenger.Surname} {passenger.Name}"
                                                    };

                                                    passengerDocuments.Add(passengerDocument);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var passenger in db.Passenger)
                            {
                                if (passenger.IdUser == _user.IdUser)
                                {
                                    foreach (var document in db.Document)
                                    {
                                        if (passenger.IdPassenger == document.IdPassenger)
                                        {

                                            PassengerDocument passengerDocument = new PassengerDocument
                                            {
                                                IdPassenger = passenger.IdPassenger,
                                                IdDocument = document.IdDocument,
                                                DisplayName = $"{document.Type_Document.Type}: {passenger.Surname} {passenger.Name}"
                                            };

                                            passengerDocuments.Add(passengerDocument);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    documentsComboBox.DisplayMemberPath = "DisplayName";
                    documentsComboBox.ItemsSource = passengerDocuments;



                    TextBlock orTextBlock = new TextBlock();
                    orTextBlock.Text = "или";
                    orTextBlock.Name = $"il{i + 1}";
                    orTextBlock.Margin = new Thickness(10, 10, 10, 0);
                    orTextBlock.FontSize = 18;
                    orTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                    MyDocs docsControl = new MyDocs();
                    docsControl.SetValue(FrameworkElement.NameProperty, $"docs{i + 1}");

                    if(isCountry)
                    {
                        docsControl.countries.Items.Clear();
                        docsControl.countries.Items.Add("Загранпаспорт");
                    }

                    docsControl.Surname.TextChanged += bt.DocumentTextBox_TextChanged;
                    docsControl.Name.TextChanged += bt.DocumentTextBox_TextChanged;
                    docsControl.Patronymic.TextChanged += bt.DocumentTextBox_TextChanged;
                    docsControl.Birthday.TextChanged += bt.DocumentTextBox_TextChanged;
                    docsControl.DocNum.TextChanged += bt.DocumentTextBox_TextChanged;
                    docsControl.countries.SelectionChanged += bt.DocumentComboBox_SelectionChanged;
                    docsControl.typeDoc.SelectionChanged += bt.DocumentComboBox_SelectionChanged;
                    docsControl.male.Click += bt.DocumentRadioButton_Checked;
                    docsControl.female.Click += bt.DocumentRadioButton_Checked;
                    docsControl.male1.MouseDown += bt.DocumentRadioButton_Checked;
                    docsControl.female1.MouseDown += bt.DocumentRadioButton_Checked;
                    docsControl.clear.PreviewMouseDown += bt.DocumentRadioButton_Checked;

                    

                    bool IsExist = false;
                    using (AirTicketsEntities air = new AirTicketsEntities())
                    {
                        foreach (User user in air.User)
                        {
                            if (user.IdUser == _user.IdUser)
                            {
                                foreach (Passenger p in air.Passenger)
                                {
                                    if (p.IdUser == _user.IdUser)
                                    {
                                        IsExist = true;
                                    }
                                }

                            }
                        }
                    }

                    passengerStackPanel.Children.Add(passengerInfoTextBlock);
                    passengerStackPanel.Children.Add(passengerDataTextBlock);
                    if (IsExist)
                    {
                        passengerStackPanel.Children.Add(documentsComboBox);
                        passengerStackPanel.Children.Add(orTextBlock);
                    }

                    passengerStackPanel.Children.Add(docsControl);

                    passengerBorder.Child = passengerStackPanel;
                    passengersWrapPanel.Children.Add(passengerBorder);
                }

                bt.booking.Content = booking;
                bt.FlightId.Text = $"Рейс №{tick.TicketID}";
                bt.countries.Text = $"{tick.DepartureLocation} → {tick.ArrivalLocation}";
                bt.dates.Text = $"{tick.DepartureTime.ToString("dd MMMM HH:mm")} → {tick.ArrivalTime.ToString("dd MMMM HH:mm")}";
                bt.duration.Text = dur;
                cost = tick.Price * (vzr + podr + deti);
                bt.totalCost = cost;
                bt.btnAdd.Content = $"К оплате {cost}";
                bt.passengers.Text = PassengerQuantity(vzr, podr, deti);
                bt.Colvo = vzr + podr;

                bt.newpass.Children.Add(passengersWrapPanel);
                bt.Show();
            }
            catch { }

        }

        private String PassengerQuantity(int vzr, int deti, int malysh)
        {
            string pasQuan;
            if (deti == 0 && malysh>0) {
                pasQuan = $"{vzr} взрослых, {malysh} младенец";
                return pasQuan ;
            }
            else if (deti>0 && malysh == 0)
            {
                pasQuan = $"{vzr} взрослых, {deti} детских";
                return pasQuan;
            }
            else if (deti==0 && malysh == 0)
            {
                pasQuan = $"{vzr} взрослых";
                return pasQuan;
            }
            else 
            {
                pasQuan = $"{vzr} взрослых, {deti} детских,  {malysh} младенец";
                return pasQuan;
            }
        }



    }
}
