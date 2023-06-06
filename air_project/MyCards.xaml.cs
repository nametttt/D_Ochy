using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace air_project
{
    /// <summary>
    /// Логика взаимодействия для MyCards.xaml
    /// </summary>
    public partial class MyCards : UserControl
    {
        public MyCards()
        {
            InitializeComponent();
        }

        private void Owner_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("^[a-zA-Z]+$");

            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }


        private void CardNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int maxLength = 19;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
                return;
            }

            string currentText = textBox.Text.Replace(" ", "");

            string newText = currentText + e.Text;

            string formattedText = FormatCardNumber(newText);

            textBox.Text = formattedText;

            textBox.CaretIndex = textBox.Text.Length;

            e.Handled = true;
        }


        private string FormatCardNumber(string cardNumber)
        {
            string digitsOnly = Regex.Replace(cardNumber, @"[^\d]", "");

            StringBuilder formattedNumber = new StringBuilder();
            for (int i = 0; i < digitsOnly.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedNumber.Append(" ");
                }
                formattedNumber.Append(digitsOnly[i]);
            }

            return formattedNumber.ToString();
        }


        private bool IsNumericInput(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void Month_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            TextBox textBox = (TextBox)sender;
            int maxLength = 2;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
                return;
            }

            string input = textBox.Text + e.Text;

            if (!IsValidMonth(input))
            {
                e.Handled = true;
            }
        }

        private bool IsValidMonth(string input)
        {
            int month;

            if (int.TryParse(input, out month))
            {
                if (month >= 1 && month <= 12)
                {
                    return true;
                }
            }

            return false;
        }


        private void CVC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            PasswordBox textBox = (PasswordBox)sender;
            int maxLength = 3;

            if (!IsNumericInput(e.Text) || textBox.Password.Length >= maxLength)
            {
                e.Handled = true;
            }
        }

        private void Hyperlink_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CardNum.Text = string.Empty;
            Month.Text = string.Empty;
            Year.Text = string.Empty;
            CVC.Password = string.Empty;
            Owner.Text = string.Empty;
            CardNum.FontSize = 18;
            Month.FontSize = 18;
            Year.FontSize = 18;
            CVC.FontSize = 18;
        }

        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length + e.Text.Length > 2)
            {
                e.Handled = true;
                return;
            }

            string newText = textBox.Text + e.Text;

            if (!IsValidYear(newText))
            {
                e.Handled = true;
            }
        }

        private bool IsValidYear(string input)
        {
            if (input.Length == 1)
            {
                int digit = int.Parse(input);

                if (digit != 2 && digit != 3)
                {
                    return false;
                }
            }
            else if (input.Length == 2)
            {
                int firstDigit = int.Parse(input.Substring(0, 1));
                int secondDigit = int.Parse(input.Substring(1, 1));

                if ((firstDigit == 2 && secondDigit >= 3 && secondDigit <= 9) ||
                    (firstDigit == 3 && secondDigit == 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
