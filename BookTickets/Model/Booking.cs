using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Model
{
    public class Booking
    {
        public string BookingId { get; set; }
        public string ShowId { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalPrice { get; private set; } = 0;
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        public Booking() {
            BookingId = Guid.NewGuid().ToString();
        }
        public Booking(string showId, string customerPhone, List<Ticket> tickets)
        {            
            ShowId = showId;
            CustomerPhone = customerPhone;
            Tickets = tickets;                  
        }
        public void AddTicket(Ticket ticket)
        {
            Tickets.Add(ticket);
            TotalPrice += ticket.Price;
        }

        public override string ToString()
        {
            return $"BookingId: {BookingId}, ShowId: {ShowId}, CustomerPhone: {CustomerPhone}, TotalPrice: {TotalPrice} Tickets: {Tickets.Count}";
        }   
    }
}
