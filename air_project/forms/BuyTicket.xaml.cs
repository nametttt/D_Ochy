using air_project.pages;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace air_project.forms
{
    /// <summary>
    /// Логика взаимодействия для BuyTicket.xaml
    /// </summary>
    public partial class BuyTicket : Window
    {
        public User _user;
        public int Colvo = 0;

        Color desiredColor = Color.FromArgb(0xFF, 0xE2, 0xC5, 0xBF); // ne
        Color checkedColor = Color.FromArgb(0xFF, 0xA7, 0x87, 0x8E); //da

        public BuyTicket(User user)
        {
            InitializeComponent();
            _user = user;


            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                foreach (var i in db.Passenger)
                {
                    if (i.UserLogin == _user.Login)
                    {
                        foreach (var j in db.Document)
                        {
                            if (i.IdPassenger == j.IdPassenger)
                            {
                                documentsCombo.Items.Add($"{j.Type_Document.Type}: {i.Surname} {i.Name}");
                            }

                        }

                    }
                }
                foreach (var i in db.Card)
                {
                    if (i.UserLogin == user.Login)
                    {
                        long number = i.IdCard;
                        string closedNumber = new string('*', Math.Max(0, number.ToString().Length - 4)) + number.ToString().Substring(number.ToString().Length - 4);

                        cardsCombo.Items.Add($"{closedNumber}");
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Получение текущего содержимого фрейма (страницы)
            Page bookingPage = (Page)booking.Content;

            // Проверка, что страница найдена
            if (bookingPage != null)
            {
                // Проверка, что страница является экземпляром вашего класса страницы (BookingPage)
                if (bookingPage is BookingPage)
                {
                    // Приведение страницы к типу BookingPage для доступа к его членам
                    BookingPage bookingPageInstance = (BookingPage)bookingPage;

                    // Теперь вы можете получить доступ к полю number
                    List<string> numberList = bookingPageInstance.number;
                    if(numberList.Count< Colvo)
                    {
                        MessageBox.Show("Выбрано не всё");
                        return;
                    }

                    int? selectedCountry = docs.countries.SelectedIndex;
                    int? docType = docs.typeDoc.SelectedIndex + 1;
                    string selectedSex = (docs.male.Background is SolidColorBrush brush && brush.Color == checkedColor) ? "Мужской" : "Женский";
                    string surname = docs.Surname.Text;
                    string name = docs.Name.Text;
                    string patronymic = docs.Patronymic.Text;

                    DateTime birthday = DateTime.Parse(docs.Birthday.Text);
                    string docNumber = docs.DocNum.Text;

                    //Докинь тут проверки на пустоту
                    if (((documentsCombo.SelectedItem != null) || (selectedCountry != null && docType != null && selectedSex != null && surname != null && name != null && patronymic != null && birthday != null && docNumber != null)) && ((cardsCombo.SelectedItem != null) || (cards.Owner != null && cards.Month != null && cards.Year != null && cards.CardNum != null && cards.CVC != null)))
                    {
                        if ((docs.male.Background is SolidColorBrush brs && brs.Color == desiredColor) && (docs.female.Background is SolidColorBrush br && br.Color == desiredColor))
                        {
                            MessageBox.Show("Выберите пол");
                            return;
                        }
                        
                    }

                    // Используйте numberList здесь
                }
                else
                {
                    // Страница не является экземпляром класса BookingPage
                }
            }
            else
            {
                // Страница BookingPage не найдена внутри фрейма
            }

        }
    }
}
