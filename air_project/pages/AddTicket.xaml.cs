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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace air_project.pages
{
    /// <summary>
    /// Логика взаимодействия для AddTicket.xaml
    /// </summary>
    public partial class AddTicket : Page
    {
        public AddTicket()
        {
            InitializeComponent();

            Style rowStyle = new Style(typeof(DataGridRow));
            DataTrigger trigger = new DataTrigger()
            {
                Binding = new Binding("Статус"),
                Value = "Куплен"
            };
            trigger.Setters.Add(new Setter(DataGridRow.BackgroundProperty, (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFAE3D5"))));


            rowStyle.Triggers.Add(trigger);
            datagrid1.RowStyle = rowStyle;

            using(AirTicketsEntities db = new AirTicketsEntities())
            {
                idFlight.Items.Clear();

                idFlight.Items.Add("Все рейсы");

                foreach (Flight f in db.Flight)
                {
                    idFlight.Items.Add(f.IdFlight);
                }

            }
            idFlight.SelectedIndex = 0;

        }

        public void UpdateTables()
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                datagrid1.ItemsSource = null;
                datagrid.ItemsSource = null;

                var query1 = from ticket in db.Ticket
                             join flight in db.Flight on ticket.IdFlight equals flight.IdFlight
                             join pt in db.Purchases_Ticket on ticket.IdTicket equals pt.IdTicket into gj
                             from subpt in gj.DefaultIfEmpty()
                             select new
                             {
                                 Номер = ticket.IdTicket,
                                 Рейс = flight.IdFlight,
                                 Место = ticket.Place,
                                 Статус = subpt == null ? "Свободен" : "Куплен"
                             };

                var distinctQuery1 = query1.GroupBy(t => t.Номер).Select(g => g.FirstOrDefault());
                datagrid1.ItemsSource = distinctQuery1.ToList();

                var purchasedTickets = db.Purchases_Ticket.Select(pt => pt.IdTicket).Distinct().ToList();
                var query = from ticket in db.Ticket
                            group ticket by ticket.IdFlight into g
                            select new
                            {
                                Рейс = g.Key,
                                Куплено = g.Count(t => purchasedTickets.Contains(t.IdTicket)),
                                Свободно = g.Count(t => !purchasedTickets.Contains(t.IdTicket)),
                                Итого = g.Count()
                            };

                datagrid.ItemsSource = query.ToList().Distinct();

            }
        }


        public bool UpdateTablesElse(int FlightId)
        {
            try
            {
                using (AirTicketsEntities db = new AirTicketsEntities())
                {
                    datagrid1.ItemsSource = null;
                    datagrid.ItemsSource = null;

                    var query1 = from ticket in db.Ticket
                                 join flight in db.Flight on ticket.IdFlight equals flight.IdFlight
                                 join pt in db.Purchases_Ticket on ticket.IdTicket equals pt.IdTicket into gj
                                 from subpt in gj.DefaultIfEmpty()
                                 where ticket.IdFlight == FlightId
                                 select new
                                 {
                                     Номер = ticket.IdTicket,
                                     Рейс = flight.IdFlight,
                                     Место = ticket.Place,
                                     Статус = subpt == null ? "Свободен" : "Куплен"
                                 };

                    var distinctQuery1 = query1.GroupBy(t => t.Номер).Select(g => g.FirstOrDefault());
                    datagrid1.ItemsSource = distinctQuery1.ToList();

                    var purchasedTickets = db.Purchases_Ticket.Select(pt => pt.IdTicket).Distinct().ToList();

                    var query = from ticket1 in db.Ticket
                                where ticket1.IdFlight == FlightId
                                group ticket1 by ticket1.IdFlight into g
                                select new
                                {
                                    Рейс = g.Key,
                                    Куплено = g.Count(t => purchasedTickets.Contains(t.IdTicket)),
                                    Свободно = g.Count(t => !purchasedTickets.Contains(t.IdTicket)),
                                    Итого = g.Count()
                                };

                    datagrid.ItemsSource = query.ToList().Distinct();
                    return true;

                }

            }
            catch {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (AirTicketsEntities db = new AirTicketsEntities())
            {
                idFlight.Items.Clear();

                idFlight.Items.Add("Все рейсы");

                foreach (Flight f in db.Flight)
                {
                    idFlight.Items.Add(f.IdFlight);
                }

            }
            idFlight.SelectedIndex = 0;

            UpdateTables();
        }

        private void idFlight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    if (idFlight.SelectedIndex == 0)
                        {
                            UpdateTables();
                        }
                   else
                    {
                         if (idFlight.SelectedItem != null)
                    {
                        int flightId = (int)idFlight.SelectedItem;

                        UpdateTablesElse(flightId);
                    }
                    }
                    
                }
            }
            catch
            {
            }

    }
}
}
