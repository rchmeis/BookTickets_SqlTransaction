using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Model
{
    public class Ticket
    {
        public static readonly int standardPrice = 100;
        public string TicketId { get; set; }
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }
        public decimal Price { get; set; }

        public Ticket() { TicketId = Guid.NewGuid().ToString(); }
        public Ticket(Booking booking, Seat seat)
        {            
            Booking = booking;
            Seat = seat;
            Price = standardPrice + seat.Price;
        }
        public override string ToString()
        {
            return $"TicketId: {TicketId},Booking: {Booking}, Seat: {Seat}, Price: {Price}";
        }
    }
}
