using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Model
{
    public class Seat
    {
        public string SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatRow { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string ShowId { get; set; }
    

        public Seat() { }
        public Seat(string seatNumber, string seatRow, bool isAvailable, decimal price, string showId  )
        {
            SeatId = $"{showId}_{seatRow}{seatNumber}";
            SeatNumber = seatNumber;
            SeatRow = seatRow;
            IsAvailable = isAvailable;
            Price = price;
            ShowId = showId;
          
        }
        public override string ToString()
        {
            return $"SeatId: {SeatId}, SeatNumber: {SeatNumber}, SeatRow: {SeatRow}, IsAvailable: {IsAvailable}, Price: {Price}, ShowId: {ShowId}";
        }
    }
}
