using air_project.pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;

namespace air_project.forms
{
    /// <summary>
    /// Логика взаимодействия для BuyTicket.xaml
    /// </summary>
    public partial class BuyTicket : Window
    {
        public User _user;
        public int Colvo = 0;
        public int totalCost;
        Dictionary<string, string> cardNumbers = new Dictionary<string, string>();

        private int IDflight;

        Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF); // ne
        Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E); //da

        public BuyTicket(User user, int idFlight)
        {
            InitializeComponent();
            _user = user;
            IDflight = idFlight;

            MyInvents();
            MyInfo();
            
        }


        public void MyInfo()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                List<PassengerDocument> passengerDocuments = new List<PassengerDocument>();

                foreach (var i in db.Passenger)
                {
                    if (i.IdUser == _user.IdUser)
                    {
                        foreach (var j in db.Document)
                        {
                            if (i.IdPassenger == j.IdPassenger)
                            {
                                PassengerDocument passengerDocument = new PassengerDocument
                                {
                                    IdPassenger = i.IdPassenger,
                                    IdDocument = j.IdDocument,
                                    DisplayName = $"{j.Type_Document.Type}: {i.Surname} {i.Name}"
                                };

                                passengerDocuments.Add(passengerDocument);
                            }
                        }
                    }
                }

                documentsCombo.DisplayMemberPath = "DisplayName";
                documentsCombo.ItemsSource = passengerDocuments;


                foreach (var i in db.Card)
                {
                    if (i.IdUser == _user.IdUser)
                    {
                        string number = i.CardNumber;
                        string closedNumber = new string('*', Math.Max(0, number.ToString().Length - 4)) + number.ToString().Substring(number.ToString().Length - 4);

                        cardsCombo.Items.Add($"{closedNumber}");
                        cardNumbers.Add(closedNumber, number);
                    }
                }
            }
        }


        public void MyInvents()
        {
            cards.Owner.TextChanged += CardTextBox_TextChanged;
            cards.CVC.PasswordChanged += CardPasswordBox_PasswordChanged;
            cards.CardNum.TextChanged += CardTextBox_TextChanged;
            cards.Month.TextChanged += CardTextBox_TextChanged;
            cards.Year.TextChanged += CardTextBox_TextChanged;


            docs.countries.SelectionChanged += DocumentComboBox_SelectionChanged;
            docs.typeDoc.SelectionChanged += DocumentComboBox_SelectionChanged;
            docs.Surname.TextChanged += DocumentTextBox_TextChanged;
            docs.Name.TextChanged += DocumentTextBox_TextChanged;
            docs.Patronymic.TextChanged += DocumentTextBox_TextChanged;
            docs.Birthday.TextChanged += DocumentTextBox_TextChanged;
            docs.DocNum.TextChanged += DocumentTextBox_TextChanged;

            docs.male.Click += DocumentRadioButton_Checked;
            docs.female.Click += DocumentRadioButton_Checked;

            docs.male1.MouseDown += DocumentRadioButton_Checked;
            docs.female1.MouseDown += DocumentRadioButton_Checked;
            docs.clear.PreviewMouseDown += DocumentRadioButton_Checked;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Page bookingPage = (Page)booking.Content;

            if (bookingPage != null)
            {
                if (bookingPage is BookingPage)
                {
                    BookingPage bookingPageInstance = (BookingPage)bookingPage;

                    List<string> numberList = bookingPageInstance.number;
                    string place = bookingPageInstance.ass;
                    int IdTic = 0;
                    if (numberList.Count < Colvo)
                    {
                        MessageBox.Show("Выберите все места!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string selectedSex = (docs.male.Background is SolidColorBrush brush && brush.Color == checkedColor) ? "Мужской" : "Женский";

                    if (((documentsCombo.SelectedItem != null) || (docs.countries.SelectedItem != null || docs.typeDoc.SelectedItem != null || selectedSex != null || !string.IsNullOrEmpty(docs.Surname.Text) || !string.IsNullOrEmpty(docs.Name.Text) || !string.IsNullOrEmpty(docs.Patronymic.Text) || !string.IsNullOrEmpty(docs.Birthday.Text) || !string.IsNullOrEmpty(docs.DocNum.Text))) || ((cardsCombo.SelectedItem != null) || (!string.IsNullOrEmpty(cards.Owner.Text) || !string.IsNullOrEmpty(cards.Month.Text) || !string.IsNullOrEmpty(cards.Year.Text) || !string.IsNullOrEmpty(cards.CardNum.Text) || !string.IsNullOrEmpty(cards.CVC.Password))))
                    {
                        using (AirTicketsEntities db = new AirTicketsEntities())
                        {
                            int ticketIds = db.Ticket
                           .Where(ticket => ticket.IdFlight == IDflight && ticket.Place == place)
                           .Select(ticket => ticket.IdTicket)
                           .FirstOrDefault();

                            if (documentsCombo.SelectedItem != null && cardsCombo.SelectedItem != null)
                            {
                                string selectedClosedNumber = cardsCombo.SelectedItem.ToString();
                                string selectedCardNumber = cardNumbers[selectedClosedNumber];

                                int selectedIdCard = db.Card
                                    .Where(card => card.CardNumber == selectedCardNumber)
                                    .Select(card => card.IdCard)
                                    .FirstOrDefault();

                                PassengerDocument selectedPassengerDocument = (PassengerDocument)documentsCombo.SelectedItem;
                                int documentId = selectedPassengerDocument.IdDocument;
                                MessageBox.Show(documentId.ToString());


                                Purchases p = new Purchases(selectedIdCard, DateTime.Now, totalCost);
                                db.Purchases.Add(p);
                                db.SaveChanges();

                                Purchases_Ticket pt = new Purchases_Ticket(ticketIds, p.IdPurchases, documentId);
                                db.Purchases_Ticket.Add(pt);
                                db.SaveChanges();
                                MessageBox.Show("Успешная покупка билета!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            else if (documentsCombo.SelectedItem != null && (!string.IsNullOrEmpty(cards.Owner.Text) || !string.IsNullOrEmpty(cards.Month.Text) || !string.IsNullOrEmpty(cards.Year.Text) || !string.IsNullOrEmpty(cards.CardNum.Text) || !string.IsNullOrEmpty(cards.CVC.Password)))
                            {

                                PassengerDocument selectedPassengerDocument = (PassengerDocument)documentsCombo.SelectedItem;
                                int documentId = selectedPassengerDocument.IdDocument;

                                Card c = new Card(cards.CardNum.Text, cards.Month.Text, cards.Year.Text, cards.CVC.Password, cards.Owner.Text, _user.IdUser);
                                db.Card.Add(c);
                                db.SaveChanges();

                                Purchases p = new Purchases(c.IdCard, DateTime.Now, totalCost);
                                db.Purchases.Add(p);
                                db.SaveChanges();

                                Purchases_Ticket pt = new Purchases_Ticket(ticketIds, p.IdPurchases, documentId);
                                db.Purchases_Ticket.Add(pt);
                                db.SaveChanges();
                                MessageBox.Show("Успешная покупка билета!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                            else if ((docs.countries.SelectedItem != null || docs.typeDoc.SelectedItem != null || selectedSex != null || !string.IsNullOrEmpty(docs.Surname.Text) || !string.IsNullOrEmpty(docs.Name.Text) || !string.IsNullOrEmpty(docs.Patronymic.Text) || !string.IsNullOrEmpty(docs.Birthday.Text) || !string.IsNullOrEmpty(docs.DocNum.Text)) && cardsCombo.SelectedItem != null)
                            {
                                string selectedClosedNumber = cardsCombo.SelectedItem.ToString();
                                string selectedCardNumber = cardNumbers[selectedClosedNumber];
                                int selectedCountry = docs.countries.SelectedIndex;
                                DateTime birthday = DateTime.Parse(docs.Birthday.Text);
                                int docType = docs.typeDoc.SelectedIndex + 1;

                                int selectedIdCard = db.Card
                                    .Where(card => card.CardNumber == selectedCardNumber)
                                    .Select(card => card.IdCard)
                                    .FirstOrDefault();

                                Passenger p1 = new Passenger(_user.IdUser, docs.Surname.Text, docs.Name.Text, docs.Patronymic.Text, birthday, selectedSex, selectedCountry);
                                db.Passenger.Add(p1);
                                db.SaveChanges();

                                Document doc = new Document(docType, docs.DocNum.Text, p1.IdPassenger);
                                db.Document.Add(doc);
                                db.SaveChanges();

                                Purchases p = new Purchases(selectedIdCard, DateTime.Now, totalCost);
                                db.Purchases.Add(p);
                                db.SaveChanges();

                                Purchases_Ticket pt = new Purchases_Ticket(ticketIds, p.IdPurchases, doc.IdDocument);
                                db.Purchases_Ticket.Add(pt);
                                db.SaveChanges();
                                MessageBox.Show("Успешная покупка билета!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                int selectedCountry = docs.countries.SelectedIndex;
                                DateTime birthday = DateTime.Parse(docs.Birthday.Text);
                                int docType = docs.typeDoc.SelectedIndex + 1;

                                Card c = new Card(cards.CardNum.Text, cards.Month.Text, cards.Year.Text, cards.CVC.Password, cards.Owner.Text, _user.IdUser);
                                db.Card.Add(c);
                                db.SaveChanges();


                                Passenger p1 = new Passenger(_user.IdUser, docs.Surname.Text, docs.Name.Text, docs.Patronymic.Text, birthday, selectedSex, selectedCountry);
                                db.Passenger.Add(p1);
                                db.SaveChanges();

                                Document doc = new Document(docType, docs.DocNum.Text, p1.IdPassenger);
                                db.Document.Add(doc);
                                db.SaveChanges();

                                Purchases p = new Purchases(c.IdCard, DateTime.Now, totalCost);
                                db.Purchases.Add(p);
                                db.SaveChanges();

                                Purchases_Ticket pt = new Purchases_Ticket(ticketIds, p.IdPurchases, doc.IdDocument);
                                db.Purchases_Ticket.Add(pt);
                                db.SaveChanges();

                                MessageBox.Show("Успешная покупка билета!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }

        private void cardsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cardsCombo.SelectedItem == null)
            {
                cards.Visibility = Visibility.Visible;
                ili.Visibility = Visibility.Visible;

            }
            else
            {
                cards.Visibility = Visibility.Collapsed;
                ili.Visibility = Visibility.Collapsed;
            }

        }

        private void CardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFieldsAndToggleVisibility();
        }

        private void CardPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckFieldsAndToggleVisibility();
        }

        private void CheckFieldsAndToggleVisibility()
        {
            if (!string.IsNullOrWhiteSpace(cards.Owner.Text) ||
                 !string.IsNullOrWhiteSpace(cards.CVC.Password) ||
                 !string.IsNullOrWhiteSpace(cards.CardNum.Text) ||
                 !string.IsNullOrWhiteSpace(cards.Month.Text) ||
                 !string.IsNullOrWhiteSpace(cards.Year.Text))
            {
                cardsCombo.Visibility = Visibility.Collapsed;
                ili.Visibility = Visibility.Collapsed;
            }
            else
            {
                cardsCombo.Visibility = Visibility.Visible;
                cardsCombo.Text = "Выберите карту";
                ili.Visibility = Visibility.Visible;
            }
        }

        private void documentsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documentsCombo.SelectedItem == null)
            {
                docs.Visibility = Visibility.Visible;
                il.Visibility = Visibility.Visible;

            }
            else
            {
                docs.Visibility = Visibility.Collapsed;
                il.Visibility = Visibility.Collapsed;
            }
        }

        private void DocumentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckVisibility();
        }

        private void DocumentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckVisibility();
        }

        private void DocumentRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckVisibility();
        }

        public void CheckVisibility()
        {
            if (!string.IsNullOrEmpty(docs.Surname.Text) ||
                 !string.IsNullOrEmpty(docs.Name.Text) ||
                 !string.IsNullOrEmpty(docs.Patronymic.Text) ||
                 !string.IsNullOrEmpty(docs.Birthday.Text) ||
                 !string.IsNullOrEmpty(docs.DocNum.Text) ||
                 docs.countries.SelectedItem != null ||
                 docs.typeDoc.SelectedItem != null ||
                 ((docs.male.Background is SolidColorBrush maleBackground && maleBackground.Color == checkedColor &&
                 docs.female.Background is SolidColorBrush femaleBackground && femaleBackground.Color == desiredColor) ||
                 (docs.male.Background is SolidColorBrush maleBackground1 && maleBackground1.Color == desiredColor &&
                 docs.female.Background is SolidColorBrush femaleBackground1 && femaleBackground1.Color == checkedColor)))
            {
                documentsCombo.Visibility = Visibility.Collapsed;
                il.Visibility = Visibility.Collapsed;
            
            }
            else
            {
                documentsCombo.Visibility = Visibility.Visible;
                il.Visibility = Visibility.Visible;
            }
        }


        public class PassengerDocument
        {
            public int IdPassenger { get; set; }
            public int IdDocument { get; set; }
            public string DisplayName { get; set; }
        }

    }
}
