using System;

namespace MovieTicketBookingSystem
{
   

    public enum TicketType
    {
        Regular,
        VIP,
        Premium
    }

    public struct SeatLocation
    {
        public int Row { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            return $"Row {Row}, Seat {Number}";
        }
    }

    // Ticket Class

    public class Ticket
    {
        private string movieName;
        private double price;

        private static int ticketCounter = 0;

        public int TicketId { get; }

        public Ticket()
        {
            ticketCounter++;
            TicketId = ticketCounter;
        }

        public string MovieName
        {
            get => movieName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    movieName = value;
            }
        }

        public TicketType Type { get; set; }

        public SeatLocation Seat { get; set; }

        public double Price
        {
            get => price;
            set
            {
                if (value > 0)
                    price = value;
            }
        }

        // Calculated property
        public double PriceAfterTax
        {
            get { return Price * 1.14; }
        }

        public static int GetTotalTicketsSold()
        {
            return ticketCounter;
        }
    }

    // Cinema Class
 

    public class Cinema
    {
        private Ticket[] tickets = new Ticket[20];

        public Ticket this[int index]
        {
            get
            {
                if (index < 0 || index >= tickets.Length)
                    return null;
                return tickets[index];
            }
            set
            {
                if (index >= 0 && index < tickets.Length)
                    tickets[index] = value;
            }
        }

        public bool AddTicket(Ticket t)
        {
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] == null)
                {
                    tickets[i] = t;
                    return true;
                }
            }
            return false;
        }

        public Ticket GetMovieByName(string movieName)
        {
            foreach (var t in tickets)
            {
                if (t != null && t.MovieName == movieName)
                    return t;
            }
            return null;
        }
    }

    // BookingHelper Static Class


    public static class BookingHelper
    {
        private static int bookingCounter = 0;

        public static double CalcGroupDiscount(int numberOfTickets, double pricePerTicket)
        {
            double total = numberOfTickets * pricePerTicket;

            if (numberOfTickets >= 5)
                total *= 0.9; // 10% discount

            return total;
        }

        public static string GenerateBookingReference()
        {
            bookingCounter++;
            return $"BK-{bookingCounter}";
        }
    }



    class Program
    {
        static void Main()
        {
            Cinema cinema = new Cinema();

            // a) Enter data for 3 tickets
            for (int i = 0; i < 3; i++)
            {
                Ticket ticket = new Ticket();

                Console.Write("Enter movie name: ");
                ticket.MovieName = Console.ReadLine();

                Console.Write("Enter ticket type (0-Regular, 1-VIP, 2-Premium): ");
                ticket.Type = (TicketType)int.Parse(Console.ReadLine());

                Console.Write("Enter seat row: ");
                int row = int.Parse(Console.ReadLine());

                Console.Write("Enter seat number: ");
                int number = int.Parse(Console.ReadLine());

                ticket.Seat = new SeatLocation { Row = row, Number = number };

                Console.Write("Enter price: ");
                ticket.Price = double.Parse(Console.ReadLine());

                cinema.AddTicket(ticket);
                Console.WriteLine();
            }

            // b) Print all 3 tickets
            Console.WriteLine("=== Tickets ===");
            for (int i = 0; i < 3; i++)
            {
                Ticket t = cinema[i];
                if (t != null)
                {
                    Console.WriteLine(
                        $"ID: {t.TicketId}, Movie: {t.MovieName}, Type: {t.Type}, " +
                        $"Seat: {t.Seat}, Price: {t.Price}, Price After Tax: {t.PriceAfterTax}"
                    );
                }
            }

            // c) Search by movie name
            Console.Write("\nEnter movie name to search: ");
            string searchName = Console.ReadLine();

            Ticket found = cinema.GetMovieByName(searchName);
            if (found != null)
                Console.WriteLine($"Found Ticket ID: {found.TicketId}");
            else
                Console.WriteLine("Movie not found");

            // d) Total tickets sold
            Console.WriteLine($"\nTotal tickets sold: {Ticket.GetTotalTicketsSold()}");

            // e) Generate booking references
            Console.WriteLine(BookingHelper.GenerateBookingReference());
            Console.WriteLine(BookingHelper.GenerateBookingReference());

            // f) Group discount
            double discountedTotal = BookingHelper.CalcGroupDiscount(5, 80);
            Console.WriteLine($"Group total price: {discountedTotal}");
        }
    }
}