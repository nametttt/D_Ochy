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
using System.Windows.Media.Media3D;
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
        public string IdDoc { get; set; }

        public doc(string title, string owner, string idDoc)
        {
            Title = title;
            Owner = owner;
            IdDoc = idDoc;
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

        documentIdList dId = new documentIdList();


        public CustomerPage(User user)
        {
            _user = user;
            docs = new ObservableCollection<doc>();
            InitializeComponent();
            OutputData();

            AddCard();
            AddTickets();
            AddDocs();
        }

        public List<cardList> cards { get; set; }

        public List<ticketList> tickets { get; set; }



        public void AddDocs()
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    mydocs.ItemsSource = null;
                    DataContext = this;

                    docs.Clear();

                    bool isExist = false;
                    foreach(User us in db.User)
                    {
                        if(us.IdUser == _user.IdUser)
                        {
                            foreach (var i in db.Document)
                            {
                                if (i.Passenger.IdUser == _user.IdUser)
                                {
                                    isExist = true;
                                    NoActiveDoc.Visibility = Visibility.Collapsed;
                                    mydocs.Visibility = Visibility.Visible;
                                    doc doc = new doc(i.Type_Document.Type, $"{i.Passenger.Surname} {i.Passenger.Name} {i.Passenger.Patronymic}", i.IdDocument.ToString());
                                    docs.Add(doc);
                                }
                            }
                        }
                    }
                    if(!isExist)
                    {

                        NoActiveDoc.Visibility = Visibility.Visible;
                        mydocs.Visibility = Visibility.Collapsed;
                    }
                }
                mydocs.ItemsSource = docs;
            }
            catch { }
        }


        public void AddTickets()
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    bool IsTick = false;
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
                                                            foreach (Purchases pu in db.Purchases)
                                                            {
                                                                if (pu.IdPurchases == pt.IdPurchases)
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
                                                                                                IsTick = true;
                                                                                                var x = new ticketList($"{c.CityName} → {c1.CityName}", $"{f.Departure_Date.ToString("dd MMMM HH:mm")} → {f.Arrival_Date.ToString("dd MMMM HH:mm")}", $"{p.Surname} {p.Name}",
                                                                                                    f.IdFlight.ToString(), type.Type, d.Number, t.Place, qr.GenerateQRCode(t.IdFlight, t.Place), pu.PurchaseDate.ToString());
                                                                                                tickets.Add(x);

                                                                                                mytickets.Visibility = Visibility.Visible;
                                                                                                NoActive.Visibility = Visibility.Collapsed;
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
                    if (!IsTick)
                    {
                        mytickets.Visibility = Visibility.Collapsed;
                        NoActive.Visibility = Visibility.Visible;
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
            foreach (User u in _context.User)
            {
                if (u.Surname == "Ochy")
                {
                    emailmy.Content = u.Login;
                    string formattedPhone = $"{u.Phone[0]}({u.Phone.Substring(1, 3)})-{u.Phone.Substring(4, 3)}-{u.Phone.Substring(7, 2)}-{u.Phone.Substring(9, 2)}";

                    phonemy.Content = formattedPhone;

                }
            }
        }


        public bool AddCard()
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    bool IsCard = false;
                    mycards.ItemsSource = null;
                    DataContext = this;
                    cards = new List<cardList>();


                    foreach (var i in db.Card)
                    {
                        if (i.IdUser == _user.IdUser)
                        {
                            IsCard = true;
                            mycards.Visibility = Visibility.Visible;
                            NoActiveCard.Visibility = Visibility.Collapsed;
                            var x = new cardList("https://raw.githubusercontent.com/muhammederdem/credit-card-form/master/src/assets/images/chip.png",
                                i.IdCard.ToString(), $"{i.Month}/{i.Year}", i.OwnerName.ToString(), i.IdCard.ToString());

                            cards.Add(x);
                        }
                    }
                    if (!IsCard)
                    {
                        NoActiveCard.Visibility = Visibility.Visible;
                        mycards.Visibility = Visibility.Collapsed;
                    }

                    mycards.ItemsSource = cards;
                }
                return true;
            }
            catch
            {
                return false;

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
                MessageBox.Show("Успешное удаление аккаунта!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

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
                if (_user.IdUser == user.IdUser)
                {
                    if (string.IsNullOrWhiteSpace(Surname.Text) || string.IsNullOrWhiteSpace(CName.Text) || string.IsNullOrWhiteSpace(Patronymic.Text))
                    {
                        MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        string cleanedPhone = new string(Phone.Text.Where(char.IsDigit).ToArray());
                        if (user.Surname == Surname.Text && user.Name == CName.Text && user.Patronymic == Patronymic.Text && user.Phone == cleanedPhone)
                        {
                            MessageBox.Show("Введите данные для изменения!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                user.Phone = cleanedPhone;
                            }

                            MessageBox.Show("Данные успешно изменены!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }

            _context.SaveChanges();
        }


        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetHash g = new GetHash();
                if (string.IsNullOrEmpty(NowPassword.Password) || string.IsNullOrEmpty(NewPassword.Password) || string.IsNullOrEmpty(Password.Password))
                {
                    MessageBox.Show("Заполните все поля!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (g.GetHashString(NowPassword.Password) == _user.Password)
                {
                    if (NewPassword.Password == Password.Password)
                    {
                        if (NewPassword.Password.Length >= 6)
                        {
                            foreach (User user in _context.User)
                            {
                                if(user.IdUser == _user.IdUser)
                                {
                                    user.Password = g.GetHashString(NewPassword.Password);
                                }
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
                    Grid.Height = Cards.ActualHeight;
    

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
                    AddDocs();
                    MessageBox.Show("Документ успешно добавлен!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                Grid.Height = HeightDoc.ActualHeight + 20;
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

        private void delete_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock deleteButton = (TextBlock)sender;
                string cardId = deleteButton.Tag.ToString();

                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите удалить выбранную карту?", "Подтверждение удаления карты", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        Card selectedCard = db.Card.FirstOrDefault(c => c.IdCard.ToString() == cardId);

                        if (selectedCard != null)
                        {
                            db.Card.Remove(selectedCard);
                            db.SaveChanges();

                            MessageBox.Show("Карта успешно удалена!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Выбранная карта не найдена!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                AddCard();
                Grid.Height = Cards.ActualHeight;

            }
            catch {
                MessageBox.Show("Вы не можете удалить данную карту!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                TextBlock deleteButton = (TextBlock)sender;
                string dock = deleteButton.Tag.ToString();
                dId.IdDocument = dock;
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                        Document selectedDock = db.Document.FirstOrDefault(c => c.IdDocument.ToString() == dock);

                    if (selectedDock != null)
                    {
                        myadd.Visibility = Visibility.Collapsed;
                        mychanges.Visibility = Visibility.Visible;
                        changes.newdoc.Content = "Изменить документ";
                        changes.delets.Visibility = Visibility.Hidden;
                        var country = db.Country.FirstOrDefault(c => c.IdCountry == selectedDock.Passenger.Citizenship);
                        var type = db.Type_Document.FirstOrDefault(c => c.IdType == selectedDock.IdType);

                        if (country != null)
                            changes.countries.Text = country.CountryName;

                        if (type != null)
                            changes.typeDoc.Text = type.Type;

                        if(selectedDock.Passenger.Gender == "Женский")
                        {
                            changes.female.Background = new SolidColorBrush(checkedColor);
                            changes.male.Background = new SolidColorBrush(desiredColor);
                        }
                        else
                        {
                            changes.male.Background = new SolidColorBrush(checkedColor);
                            changes.female.Background = new SolidColorBrush(desiredColor);
                        }

                        changes.Surname.Text = selectedDock.Passenger.Surname;
                        changes.Name.Text = selectedDock.Passenger.Name;
                        changes.Patronymic.Text = selectedDock.Passenger.Patronymic;
                        changes.Birthday.Text = selectedDock.Passenger.Bithday.ToString("dd.MM.yyyy");
                        changes.DocNum.Text = selectedDock.Number;

                    }
                    else
                    {
                        MessageBox.Show("Документ не найден!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
            }
        }

        public void ChangeDoc(string Id)
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                Document selectedDock = db.Document.FirstOrDefault(c => c.IdDocument.ToString() == Id);
                if (selectedDock != null)
                {
                    selectedDock.Number = changes.DocNum.Text;
                    selectedDock.Passenger.Bithday = Convert.ToDateTime(changes.Birthday.Text);
                    selectedDock.Passenger.Surname = changes.Surname.Text;
                    selectedDock.Passenger.Name = changes.Name.Text;
                    selectedDock.Passenger.Patronymic = changes.Patronymic.Text;
                    string selectedSex = (changes.male.Background is SolidColorBrush brush && brush.Color == checkedColor) ? "Мужской" : "Женский";
                    selectedDock.Passenger.Gender = selectedSex;
                    int selectedCountry = changes.countries.SelectedIndex;
                    int docType = changes.typeDoc.SelectedIndex + 1;
                    selectedDock.Passenger.Citizenship = selectedCountry;
                    selectedDock.IdType = docType;
                    db.SaveChanges();
                    AddDocs(); 
                    Grid.Height = Docs.ActualHeight;
                    MessageBox.Show("Документ успешно изменен!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public void DeleteDoc(string Id)
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    Document selectedDock = db.Document.FirstOrDefault(c => c.IdDocument.ToString() == Id);
                    if (selectedDock != null)
                    {
                        MessageBoxResult result = MessageBox.Show("Действительно ли вы хотите удалить выбранный документ?", "Подтверждение удаления документа", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            db.Document.Remove(selectedDock);
                            db.SaveChanges();
                            AddDocs();
                            Grid.Height = Docs.ActualHeight;
                            MessageBox.Show("Выбранный документ успешно удален!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Вы не можете удалить данный документ!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ChangeDoc(dId.IdDocument);

            myadd.Visibility = Visibility.Visible;
            mychanges.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DeleteDoc(dId.IdDocument);

            myadd.Visibility = Visibility.Visible;
            mychanges.Visibility = Visibility.Collapsed;
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

    public class documentIdList
    {
        public string IdDocument { get; set; }
    }

        public class cardList
    {
        public string ChipImage { get; set; }
        public string IdCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CardHolder { get; set; }

        public cardList(string chipImage, string cardNumber, string expiryDate, string cardHolder, string idCard)
        {
            ChipImage = chipImage;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            CardHolder = cardHolder;
            IdCard = idCard;
        }
    }
}