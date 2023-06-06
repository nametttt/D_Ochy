using air_project.pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.Toolkit.Panels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Color = System.Windows.Media.Color;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;
using WrapPanel = System.Windows.Controls.WrapPanel;

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
        public int _count;
        Dictionary<string, string> cardNumbers = new Dictionary<string, string>();

        private int IDflight;

        Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF); // ne
        Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E); //da


        public BuyTicket(User user, int idFlight, int count)
        {
            InitializeComponent();
            _user = user;

            IDflight = idFlight;
            _count = count;
            MyInfo();

            MyInvents();


            bool CardIs = false;
            using (AirTicketsEntities air = new AirTicketsEntities())
            {
                foreach (User u in air.User)
                {
                    if (u.IdUser == _user.IdUser)
                    {
                        foreach (Card c in air.Card)
                        {
                            if (user.IdUser == c.IdUser)
                            {
                                CardIs = true;
                            }
                        }
                    }
                }
            }
            if (CardIs)
            {
                cardsCombo.Visibility = Visibility.Visible;
                ili.Visibility = Visibility.Visible;
            }
            else
            {
                cardsCombo.Visibility = Visibility.Collapsed;
                ili.Visibility = Visibility.Collapsed;
            }
        }




        public void MyInfo()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                List<PassengerDocument> passengerDocuments = new List<PassengerDocument>();


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

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Page bookingPage = (Page)booking.Content;
                List<object> selectedItems = new List<object>();

                if (bookingPage != null)
                {
                    if (bookingPage is BookingPage)
                    {
                        BookingPage bookingPageInstance = (BookingPage)bookingPage;

                        List<string> numberList = bookingPageInstance.number;
                        string place = bookingPageInstance.ass;
                        if (numberList.Count < Colvo)
                        {
                            MessageBox.Show("Выберите все места!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        for (int i = 1; i <= _count; i++)
                        {

                            WrapPanel newpassWrapPanel = (WrapPanel)FindName("newpass");
                            StackPanel tanushaStackPanel = FindStackPanelByName(newpassWrapPanel, $"tanusha{i}");


                            MyDocs docs = GetAllMyDocsControls(tanushaStackPanel).FirstOrDefault();

                            ComboBox documentsCombo = GetElementByName<ComboBox>(tanushaStackPanel, $"documentsCombo{i}");

                            if (documentsCombo != null && docs != null)
                            {
                                string selectedSex = (docs.male.Background is SolidColorBrush brush && brush.Color == checkedColor) ? "Мужской" : "Женский";
                                if (((documentsCombo.SelectedItem != null) || (docs.countries.SelectedItem != null || docs.typeDoc.SelectedItem != null || selectedSex != null || !string.IsNullOrEmpty(docs.Surname.Text) || !string.IsNullOrEmpty(docs.Name.Text) || !string.IsNullOrEmpty(docs.Patronymic.Text) || !string.IsNullOrEmpty(docs.Birthday.Text) || !string.IsNullOrEmpty(docs.DocNum.Text))) || ((cardsCombo.SelectedItem != null) || (!string.IsNullOrEmpty(cards.Owner.Text) || !string.IsNullOrEmpty(cards.Month.Text) || !string.IsNullOrEmpty(cards.Year.Text) || !string.IsNullOrEmpty(cards.CardNum.Text) || !string.IsNullOrEmpty(cards.CVC.Password))))
                                {
                                    using (AirTicketsEntities db = new AirTicketsEntities())
                                    {
                                        string selectedPlace = numberList[i - 1];

                                        int ticketIds = db.Ticket
                                        .Where(ticket => ticket.IdFlight == IDflight && ticket.Place == selectedPlace)
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


                                            Purchases p = new Purchases(selectedIdCard, DateTime.Now, totalCost);
                                            db.Purchases.Add(p);
                                            db.SaveChanges();

                                            Purchases_Ticket pt = new Purchases_Ticket(ticketIds, p.IdPurchases, documentId);
                                            db.Purchases_Ticket.Add(pt);
                                            db.SaveChanges();
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


                                        }

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                            }
                        }

                        MessageBox.Show("Благодарим Вас за покупку! Просмотреть купленные билеты Вы можете в личном кабинете.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Hide();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Заполните все данные!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        

        public void documentsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string comboBoxName = comboBox.Name;
            int index = int.Parse(comboBoxName.Substring(comboBoxName.Length - 1));

            WrapPanel newpassWrapPanel = (WrapPanel)FindName("newpass");
            StackPanel tanushaStackPanel = FindStackPanelByName(newpassWrapPanel, $"tanusha{index}");


            MyDocs docs = GetAllMyDocsControls(tanushaStackPanel).FirstOrDefault();
            TextBlock il = GetElementByName<TextBlock>(tanushaStackPanel, $"il{index}");
            ComboBox documentsComboBox = GetElementByName<ComboBox>(tanushaStackPanel, $"documentsCombo{index}");

            if (comboBox.SelectedItem == null)
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

        public void CardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFieldsAndToggleVisibility();
        }

        public void CardPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckFieldsAndToggleVisibility();
        }

        public void CheckFieldsAndToggleVisibility()
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

        private StackPanel FindStackPanelByName(DependencyObject parent, string stackPanelName)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is StackPanel stackPanel && stackPanel.Name == stackPanelName)
                {
                    return stackPanel;
                }

                StackPanel nestedStackPanel = FindStackPanelByName(child, stackPanelName);
                if (nestedStackPanel != null)
                {
                    return nestedStackPanel;
                }
            }

            return null;
        }


        private T GetElementByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            if (parent is T element && element.Name == name)
            {
                return element;
            }

            if (parent is Visual || parent is Visual3D)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    T result = GetElementByName<T>(child, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        private List<MyDocs> GetAllMyDocsControls(StackPanel stackPanel)
        {
            List<MyDocs> myDocsControls = FindVisualChildren<MyDocs>(stackPanel);

            return myDocsControls;
        }

        private List<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> children = new List<T>();

            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    if (child is T typedChild)
                    {
                        children.Add(typedChild);
                    }

                    List<T> nestedChildren = FindVisualChildren<T>(child);
                    children.AddRange(nestedChildren);
                }
            }

            return children;
        }

        public void DocumentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckVisibility();
        }

        public void DocumentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckVisibility();
        }

        public void DocumentRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckVisibility();
        }

        public void CheckVisibility()
        {
            for (int i = 1; i <= _count; i++)
            {

                WrapPanel newpassWrapPanel = (WrapPanel)FindName("newpass");
                StackPanel tanushaStackPanel = FindStackPanelByName(newpassWrapPanel, $"tanusha{i}");

                MyDocs docs = GetAllMyDocsControls(tanushaStackPanel).FirstOrDefault();
                TextBlock il = GetElementByName<TextBlock>(tanushaStackPanel, $"il{i}");
                ComboBox documentsComboBox = GetElementByName<ComboBox>(tanushaStackPanel, $"documentsCombo{i}");

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
                    documentsComboBox.Visibility = Visibility.Collapsed;
                    il.Visibility = Visibility.Collapsed;
                }
                else
                {
                    documentsComboBox.Visibility = Visibility.Visible;
                    il.Visibility = Visibility.Visible;
                }
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
