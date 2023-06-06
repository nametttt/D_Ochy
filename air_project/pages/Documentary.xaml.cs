using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для Documentary.xaml
    /// </summary>
    public partial class Documentary : Page
    {

        public int count1 = 0, count2 = 0;
        public List<SalesRecord> salesRecord { get; set; }

        public Documentary()
        {
            InitializeComponent();
            salesRecord = new List<SalesRecord>();
            UpdateMyTable();
        }

        public void UpdateMyTable()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                DateTime today = DateTime.Today;
                DateTime lastMonthDate = DateTime.Now.AddMonths(-1);

                salesRecord.Clear();

                foreach (Purchases_Ticket pt in db.Purchases_Ticket)
                {
                    foreach (Ticket ticket in db.Ticket)
                    {
                        if (ticket.IdTicket == pt.IdTicket)
                        {
                            foreach (Document d in db.Document)
                            {
                                if (d.IdDocument == pt.IdDocument)
                                {
                                    foreach (Passenger pass in db.Passenger)
                                    {
                                        if (pass.IdPassenger == d.IdPassenger)
                                        {
                                            foreach (Flight f in db.Flight)
                                            {
                                                if (ticket.IdFlight == f.IdFlight)
                                                {
                                                    foreach (Purchases pp in db.Purchases)
                                                    {
                                                        if (pp.IdPurchases == pt.IdPurchases)
                                                        {
                                                            foreach (City c1 in db.City)
                                                            {
                                                                if (c1.IdCity == f.Departure_City)
                                                                {
                                                                    foreach (City c2 in db.City)
                                                                    {
                                                                        if (c2.IdCity == f.Arrival_City)
                                                                        {
                                                                            var x = new SalesRecord()
                                                                            {
                                                                                FlightNumber = f.IdFlight.ToString(),
                                                                                Route = $"{c1.CityName} → {c2.CityName}",
                                                                                Date = $"{f.Departure_Date.ToString("dd MMMM HH:mm")} → {f.Arrival_Date.ToString("dd MMMM HH:mm")}",
                                                                                Passenger = $"{pass.Surname} {pass.Name}",
                                                                                SeatNumber = $"{ticket.Place}",
                                                                                TicketPrice = f.RetailValue.ToString(),
                                                                                PurchaseDate = pp.PurchaseDate.ToString("dd MMMM HH:mm")
                                                                            };
                                                                            salesRecord.Add(x);
                                                                        }
                                                                        if (pp.PurchaseDate.Date == today)
                                                                        {
                                                                            count1 += f.RetailValue;
                                                                        }
                                                                        if (pp.PurchaseDate.Date >= lastMonthDate)
                                                                        {
                                                                            count2 += f.RetailValue;
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
                MonthProfit.Text = $"{count2:N0} рублей";
                todayProfit.Text = $"{count1:N0} рублей";
                grid.ItemsSource = salesRecord;
            }
        }

        public class SalesRecord
        {
            public string FlightNumber { get; set; }
            public string Route { get; set; }
            public string Date { get; set; }
            public string Passenger { get; set; }
            public string SeatNumber { get; set; }
            public string TicketPrice { get; set; }
            public string PurchaseDate { get; set; }

            public SalesRecord() { }
            public SalesRecord(string flightNumber, string route, string date, string passenger, string seatNumber, string ticketPrice, string purchaseDate)
            {
                FlightNumber = flightNumber;
                Route = route;
                Date = date;
                Passenger = passenger;
                SeatNumber = seatNumber;
                TicketPrice = ticketPrice;
                PurchaseDate = purchaseDate;
            }
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (WordprocessingDocument document = WordprocessingDocument.Create(saveFileDialog.FileName, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = document.AddMainDocumentPart();
                        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();

                        Body body = mainPart.Document.AppendChild(new Body());

                        // Add profit information paragraphs
                        Paragraph profitInfoParagraph = new Paragraph();
                        profitInfoParagraph.AppendChild(new Run(new Text("Информация о прибыли")));

                        Paragraph todayProfitParagraph = new Paragraph();
                        todayProfitParagraph.AppendChild(new Run(new Text($"За сегодняшний день: {count1:N0} рублей")));

                        Paragraph monthProfitParagraph = new Paragraph();
                        monthProfitParagraph.AppendChild(new Run(new Text($"За месяц: {count2:N0} рублей")));

                        body.Append(profitInfoParagraph);
                        body.Append(todayProfitParagraph);
                        body.Append(monthProfitParagraph);

                        // Add table with gridlines
                        DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();
                        DocumentFormat.OpenXml.Wordprocessing.TableProperties tableProperties = new DocumentFormat.OpenXml.Wordprocessing.TableProperties();

                        // Apply table border style
                        TableBorders tableBorders = new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 },
                            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 },
                            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 },
                            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 },
                            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 },
                            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 2 }
                        );

                        tableProperties.Append(tableBorders);
                        table.AppendChild(tableProperties);

                        TableRow headerRow = new TableRow();
                        headerRow.Append(CreateTableCell("Рейс"));
                        headerRow.Append(CreateTableCell("Маршрут"));
                        headerRow.Append(CreateTableCell("Дата"));
                        headerRow.Append(CreateTableCell("Пассажир"));
                        headerRow.Append(CreateTableCell("Место"));
                        headerRow.Append(CreateTableCell("Цена"));
                        headerRow.Append(CreateTableCell("Дата покупки"));
                        table.AppendChild(headerRow);

                        foreach (SalesRecord record in salesRecord)
                        {
                            TableRow dataRow = new TableRow();
                            dataRow.Append(CreateTableCell(record.FlightNumber));
                            dataRow.Append(CreateTableCell(record.Route));
                            dataRow.Append(CreateTableCell(record.Date));
                            dataRow.Append(CreateTableCell(record.Passenger));
                            dataRow.Append(CreateTableCell(record.SeatNumber));
                            dataRow.Append(CreateTableCell(record.TicketPrice));
                            dataRow.Append(CreateTableCell(record.PurchaseDate));
                            table.AppendChild(dataRow);
                        }

                        body.Append(table);

                        // Save the Word document
                        mainPart.Document.Save();

                        MessageBox.Show("Отчет успешно сохранен.", "Сохранение отчета", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении отчета: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Clipboard.SetText(ex.Message);
            }
        }

        private DocumentFormat.OpenXml.Wordprocessing.TableCell CreateTableCell(string text)
        {
            DocumentFormat.OpenXml.Wordprocessing.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(text));
            paragraph.Append(run);
            cell.Append(paragraph);
            return cell;
        }
    }
}

