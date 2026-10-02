using BookTickets.Model;
using BookTickets.Repository;

namespace BookTickets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowRepository showRepository = new ShowRepository();
            SeatRepository seatRepository = new SeatRepository();

            Show show = new Show("S001", "The Phantom of the Opera", new DateTime(2024, 12, 15));
            //showRepository.Create(show);
            //show.CreateSeats(seatRepository);

            BookTicket bookTicket = new BookTicket();
            List<Seat> availableSeats = bookTicket.GetAvailableSeats("S001");
            foreach (var seat in availableSeats)
            {
                Console.WriteLine(seat);
            }

            Console.WriteLine("List the seatIds you wish to book separated by commas:");
            string input = Console.ReadLine();

            List<string> seatIdsToBook = input.Split(',').Select(s => s.Trim()).ToList();
            List<Seat> seatsToBook = availableSeats.Where(s => seatIdsToBook.Contains(s.SeatId)).ToList();

            bookTicket.MakeReservation(show.ShowId, seatsToBook, "12121212");
        }
    }
}
