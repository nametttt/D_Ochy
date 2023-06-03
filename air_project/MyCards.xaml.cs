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
            CardNum.FontSize = 22;

            TextBox textBox = (TextBox)sender;
            int maxLength = 16;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
            }
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
            Month.FontSize = 22;

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

        private void Year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Year.FontSize = 22;

            TextBox textBox = (TextBox)sender;
            int maxLength = 2;

            if (!IsNumericInput(e.Text) || textBox.Text.Length >= maxLength)
            {
                e.Handled = true;
                return;
            }

            string input = textBox.Text + e.Text;

            if (!IsValidYear(input))
            {
                e.Handled = true;
            }
        }

        private bool IsValidYear(string input)
        {
            int year;

            if (int.TryParse(input, out year))
            {
                if (year >= 23 && year <= 30)
                {
                    return true;
                }
            }

            return false;
        }


        private void CVC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CVC.FontSize = 22;

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
    }
}
