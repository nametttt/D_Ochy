using air_project.pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace air_project
{

    public class doc
    {
        public string Title { get; set; }
        public string Owner { get; set; }

        public doc(string title, string owner)
        {
            Title = title;
            Owner = owner;
        }
    }


    /// <summary>
    /// Логика взаимодействия для CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        public ObservableCollection<doc> docs { get; set; }


        Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF); // ne
        Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E); //da
        public AirTicketsEntities _context = new AirTicketsEntities();
        public User _user;


        public CustomerPage(User user)
        {
            _user = user;
            docs = new ObservableCollection<doc>();

            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.Document)
                {
                    if (i.Passenger.IdUser == _user.IdUser)
                    {
                        doc doc = new doc(i.Type_Document.Type, $"{i.Passenger.Surname} {i.Passenger.Name} {i.Passenger.Patronymic}");
                        docs.Add(doc);
                    }
                }
            }
            InitializeComponent();
            OutputData();
            AddCard();
            AddTickets();
        }

        public List<cardList> cards { get; set; }

        public List<ticketList> tickets { get; set; }


        public void AddTickets()
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    mytickets.ItemsSource = null;
                    DataContext = this;
                    CreateQR qr = new CreateQR();
                    tickets = new List<ticketList>();

                    foreach (Passenger p in db.Passenger)
                    {
                        if (p.IdUser == _user.IdUser)
                        {
                            foreach (Document d in db.Document)
                            {
                                if (d.IdPassenger == p.IdPassenger)
                                {
                                    foreach (Type_Document type in db.Type_Document)
                                    {
                                        if (type.IdType == d.IdType)
                                        {
                                            foreach (Purchases_Ticket pt in db.Purchases_Ticket)
                                            {
                                                if (pt.IdDocument == d.IdDocument)
                                                {
                                                    foreach (Ticket t in db.Ticket)
                                                    {
                                                        if (pt.IdTicket == t.IdTicket)
                                                        {
                                                            foreach(Purchases pu in db.Purchases)
                                                            {
                                                                if(pu.IdPurchases == pt.IdPurchases)
                                                                {
                                                                    foreach (Flight f in db.Flight)
                                                                    {
                                                                        if (t.IdFlight == f.IdFlight)
                                                                        {
                                                                            foreach (City c in db.City)
                                                                            {
                                                                                if (c.IdCity == f.Departure_City)
                                                                                {
                                                                                    foreach (City c1 in db.City)
                                                                                    {
                                                                                        if (c1.IdCity == f.Arrival_City)
                                                                                        {
                                                                                            if (f.Arrival_Date > DateTime.UtcNow)
                                                                                            {
                                                                                                var x = new ticketList($"{c.CityName} → {c1.CityName}", $"{f.Departure_Date.ToString("dd.MM HH:mm")} → {f.Arrival_Date.ToString("dd MMMM HH:mm")}", $"{p.Surname} {p.Name}",
                                                                                                    f.IdFlight.ToString(), type.Type, d.Number, t.Place, qr.GenerateQRCode(t.IdFlight, t.Place), pu.PurchaseDate.ToString());
                                                                                                tickets.Add(x);

                                                                                                mytickets.Visibility = Visibility.Visible;
                                                                                                NoActive.Visibility = Visibility.Collapsed;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                mytickets.Visibility = Visibility.Collapsed;
                                                                                                NoActive.Visibility = Visibility.Visible;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    mytickets.ItemsSource = tickets.OrderBy(ticket => ticket.PurchesDate);
                }
            }
            catch { }
        }



        public void OutputData()
        {
            foreach (User user in _context.User)
            {
                if (_user.Login == user.Login)
                {
                    CName.Text = user.Name;
                    Surname.Text = user.Surname;
                    Patronymic.Text = user.Patronymic;
                    Phone.Text = user.Phone;
                    Email.Text = user.Login;
                }
            }
        }


        public void AddCard()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                mycards.ItemsSource = null;
                DataContext = this;
                cards = new List<cardList>();

                foreach (var i in db.Card)
                {
                    if (i.IdUser == _user.IdUser)
                    {

                        var x = new cardList("https://raw.githubusercontent.com/muhammederdem/credit-card-form/master/src/assets/images/chip.png",
                            i.IdCard.ToString(), $"{i.Month}/{i.Year}", i.OwnerName.ToString());

                        cards.Add(x);
                    }
                }
                mycards.ItemsSource = cards;
            }
        }

        private void UserInfo_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = About.Height;
            About.Visibility = Visibility.Visible;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Doc_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = Docs.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Visible;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Ticket_Click(object sender, RoutedEventArgs e)
        {
            Grid.Height = Tickets.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Hidden;
            Tickets.Visibility = Visibility.Visible;
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {


            Grid.Height = Cards.ActualHeight;
            About.Visibility = Visibility.Hidden;
            Docs.Visibility = Visibility.Hidden;
            Cards.Visibility = Visibility.Visible;
            Tickets.Visibility = Visibility.Hidden;
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string digitsOnly = Regex.Replace(textBox.Text, "[^0-9]", "");

            string formattedNumber = string.Empty;
            int index = 0;

            if (digitsOnly.Length > 0)
            {
                formattedNumber = digitsOnly.Substring(index, 1);

                if (digitsOnly.Length > 1)
                    formattedNumber += "-" + digitsOnly.Substring(1, Math.Min(3, digitsOnly.Length - 1));

                if (digitsOnly.Length > 4)
                    formattedNumber += "-" + digitsOnly.Substring(4, Math.Min(3, digitsOnly.Length - 4));

                if (digitsOnly.Length > 7)
                    formattedNumber += "-" + digitsOnly.Substring(7, Math.Min(2, digitsOnly.Length - 7));

                if (digitsOnly.Length > 9)
                    formattedNumber += "-" + digitsOnly.Substring(9, Math.Min(2, digitsOnly.Length - 9));
            }

            textBox.TextChanged -= Phone_TextChanged;
            textBox.Text = formattedNumber;
            textBox.CaretIndex = textBox.Text.Length;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
            textBox.TextChanged += Phone_TextChanged;
        }




        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите выйти из аккаунта?", "Подтверждение выхода из аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Hide();
                Auth auth = new Auth();
                auth.Show();
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите удалить свой аккаунт? Восстановить его будет невозможно.", "Подтверждение удаления аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (User user in _context.User)
                {
                    if (_user.Login == user.Login)
                    {
                        _context.User.Remove(user);
                    }
                }
                _context.SaveChanges();
                Window parentWindow = Window.GetWindow(this);
                parentWindow.Hide();
                Auth auth = new Auth();
                auth.Show();
            }
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            foreach (User user in _context.User)
            {
                if (_user.Login == user.Login)
                {
                    if (string.IsNullOrWhiteSpace(Surname.Text) || string.IsNullOrWhiteSpace(CName.Text) || string.IsNullOrWhiteSpace(Patronymic.Text))
                    {
                        MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        user.Surname = Surname.Text;
                        user.Name = CName.Text;
                        user.Patronymic = Patronymic.Text;

                        if (string.IsNullOrWhiteSpace(Phone.Text))
                        {
                            user.Phone = null;
                        }
                        else
                        {
                            string cleanedPhone = new string(Phone.Text.Where(char.IsDigit).ToArray());
                            user.Phone = cleanedPhone;
                        }

                        MessageBox.Show("Данные успешно изменены!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            _context.SaveChanges();

        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NowPassword.Password) || string.IsNullOrEmpty(NewPassword.Password) || string.IsNullOrEmpty(Password.Password))
                {
                    MessageBox.Show("Заполните все поля!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (NowPassword.Password == _user.Password)
                {
                    if (NewPassword.Password == Password.Password)
                    {
                        if (NewPassword.Password.Length >= 6)
                        {
                            foreach (User user in _context.User)
                            {
                                user.Password = NewPassword.Password;
                            }
                            _context.SaveChanges();
                            MessageBox.Show("Пароль успешно изменен!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            NowPassword.Password = "";
                            NewPassword.Password = "";
                            Password.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("Новый пароль должен содержать не менее 6 символов!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                            NewPassword.Password = "";
                            Password.Password = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Повторно введенный пароль не соответствует новому!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                        NewPassword.Password = "";
                        Password.Password = "";
                    }
                }
                else
                {
                    MessageBox.Show("Текущий пароль неверен!", "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
                    NowPassword.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при изменении пароля: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string cardNumber = car.CardNum.Text;
                string expirationMonth = car.Month.Text;
                string expirationYear = car.Year.Text;
                string cvc = car.CVC.Password;
                string ownerName = car.Owner.Text;

                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(expirationMonth) ||
                    string.IsNullOrEmpty(expirationYear) || string.IsNullOrEmpty(cvc) ||
                    string.IsNullOrEmpty(ownerName))
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    Card newCard = new Card
                    {
                        CardNumber = cardNumber,
                        Month = expirationMonth,
                        Year = expirationYear,
                        CW_CVC = cvc,
                        OwnerName = ownerName,
                        IdUser = _user.IdUser
                    };

                    db.Card.Add(newCard);
                    db.SaveChanges();
                    AddCard();

                    car.CardNum.Text = string.Empty;
                    car.Month.Text = string.Empty;
                    car.Year.Text = string.Empty;
                    car.CVC.Password = string.Empty;
                    car.Owner.Text = string.Empty;
                    car.CardNum.FontSize = 18;
                    car.Month.FontSize = 18;
                    car.Year.FontSize = 18;
                    car.CVC.FontSize = 18;

                    MessageBox.Show("Карта успешно добавлена!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении карты: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(contr.countries.SelectedItem == null || contr.typeDoc.SelectedItem == null || contr.Surname.Text == null || contr.Name.Text == null || contr.Patronymic.Text == null || contr.Birthday.Text == null || contr.DocNum.Text == null)
            {
                MessageBox.Show("Заполните все поля!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if ((contr.male.Background is SolidColorBrush brs && brs.Color == desiredColor) && (contr.female.Background is SolidColorBrush br && br.Color == desiredColor))
                {
                    MessageBox.Show("Выберите пол!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int selectedCountry = contr.countries.SelectedIndex;
                int docType = contr.typeDoc.SelectedIndex + 1;
                string selectedSex = (contr.male.Background is SolidColorBrush brush && brush.Color == checkedColor) ? "Мужской" : "Женский";
                string surname = contr.Surname.Text;
                string name = contr.Name.Text;
                string patronymic = contr.Patronymic.Text;

                DateTime birthday = DateTime.Parse(contr.Birthday.Text);
                string docNumber = contr.DocNum.Text;

                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    Passenger ps = new Passenger(_user.IdUser, surname, name, patronymic, birthday, selectedSex, selectedCountry);
                    Document doc = new Document(docType, docNumber, ps.IdPassenger);
                    db.Passenger.Add(ps);
                    db.Document.Add(doc);
                    db.SaveChanges();
                    MessageBox.Show("Документ успешно добавлен!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                }

                contr.countries.Text = "Гражданство";
                contr.typeDoc.Text = "Тип документа";
                contr.male.Background = new SolidColorBrush(desiredColor);
                contr.female.Background = new SolidColorBrush(desiredColor);
                contr.Surname.Text = null;
                contr.Name.Text = null;
                contr.Patronymic.Text = null;
                contr.Birthday.Text = null;
                contr.DocNum.Text = null;
            }
           
        }

        private void change_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            Grid whyvisible = (Grid)textBlock.Tag;

            if (textBlock.Text == "Показать QR-код")
            {
                whyvisible.Visibility = Visibility.Visible;
                textBlock.Text = "Скрыть QR-код";
            }
            else
            {
                whyvisible.Visibility = Visibility.Hidden;
                textBlock.Text = "Показать QR-код";
            }
        }
    }

    public class ticketList
    {
        public string FromTo { get; set; }
        public string Time { get; set; }
        public string Passenger { get; set; }
        public string Flight { get; set; }
        public string Type { get; set; }
        public string Document { get; set; }
        public string Place { get; set; }
        public string QR { get; set; }
        public string PurchesDate { get; set; }
        private BitmapImage _qrImage;
        public BitmapImage QRImage
        {
            get { return _qrImage; }
            set
            {
                _qrImage = value;
            }
        }
        public ticketList(string fromTo, string time, string passenger, string flight, string type, string document, string place, string qR, string purchesDate)
        {
            FromTo = fromTo;
            Time = time;
            Passenger = passenger;
            Flight = flight;
            Type = type;
            Document = document;
            Place = place;
            QR = qR;
            QRImage = LoadImageFromBase64String(QR);
            PurchesDate = purchesDate;
        }

        private BitmapImage LoadImageFromBase64String(string base64String)
        {
            try
            {
                byte[] imageData = Convert.FromBase64String(base64String);
                BitmapImage image = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream(imageData))
                {
                    memoryStream.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = memoryStream;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки изображения: " + ex.Message);
                return null;
            }
        }
    }


    public class cardList
    {
        public string ChipImage { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CardHolder { get; set; }

        public cardList(string chipImage, string cardNumber, string expiryDate, string cardHolder)
        {
            ChipImage = chipImage;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            CardHolder = cardHolder;
        }
    }
}