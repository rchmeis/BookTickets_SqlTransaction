using BookTickets.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Model
{
    public class Show
    {
        public string ShowId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }        
        public int MaxSeats { get; set; } = 30;

        public Show()
        {            
        }
        public Show(string showId, string name, DateTime date)
        {
            ShowId = showId;
            Name = name;
            Date = date;
        }

        //helper method - creates 30 seats for the show and adds them to the seat repository
        public void CreateSeats(SeatRepository seatRepo)
        {
            int seatsPerRow = 10;
            List<Seat> seatsToCreate = new List<Seat>();
            for (int i = 1; i <= MaxSeats; i++)
            {
                string seatNumber = ((i - 1) % seatsPerRow + 1).ToString(); // Seat numbers 1-10
                string seatRow = ((char)('A' + (i - 1) / seatsPerRow)).ToString(); // Rows A, B, C for 30 seats
                bool isAvailable = true;
                decimal price = 10.0m; // Example price
                string showId = ShowId; // Associate the seat with the current show
                seatsToCreate.Add(new Seat(seatNumber, seatRow, isAvailable, price, showId));
            }
            seatRepo.CreateRange(seatsToCreate);
        }

        public override string ToString()
        {
            return $"ShowId: {ShowId}, Name: {Name}, Date: {Date}";
        }
    }
}
