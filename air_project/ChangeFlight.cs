using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace air_project
{
    public class ChangeFlight
    {
        public bool AddMyTicket(int flightId, int ticketCount)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };

                    foreach (char letter in letters)
                    {
                        for (int i = 1; i <= ticketCount; i++)
                        {
                            string place = $"{letter}{i}";

                            Ticket ticket = new Ticket()
                            {
                                IdFlight = flightId,
                                Place = place,
                            };

                            air.Ticket.Add(ticket);
                        }
                    }

                    air.SaveChanges();
                    MessageBox.Show("Билеты успешно добавлены.", "Success!");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFlight(int flightId)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var flight = air.Flight.FirstOrDefault(f => f.IdFlight == flightId);
                    if (flight != null)
                    {
                        air.Flight.Remove(flight);
                        air.SaveChanges();
                        MessageBox.Show("Рейс успешно удален.", "Success!");
                    }
                }
                return true;

            }
            catch
            {
                return false;

            }
        }
        public bool UpdateFlight(int flightId, int departureCityId, DateTime departureDate, int arrivalCityId, DateTime arrivalDate, int seatsNumber, int retailValue)
        {
            try
            {
                using (AirTicketsEntities air = new AirTicketsEntities())
                {
                    var flight = air.Flight.FirstOrDefault(f => f.IdFlight == flightId);
                    if (flight != null)
                    {
                        flight.Departure_City = departureCityId;
                        flight.Departure_Date = departureDate;
                        flight.Arrival_City = arrivalCityId;
                        flight.Arrival_Date = arrivalDate;
                        flight.Seats_Number = seatsNumber;
                        flight.RetailValue = retailValue;

                        air.SaveChanges();

                        MessageBox.Show("Рейс успешно обновлен.", "Success!");

                    }
                }
                return true;

            }
            catch
            {
                return false;

            }
        }
    }
}
