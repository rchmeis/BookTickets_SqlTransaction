using BookTickets.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;

namespace BookTickets
{
    public class BookTicket
    {
        internal DBConnection connection = new DBConnection();
        internal SqlTransaction transaction;

        public List<Seat> GetAvailableSeats(string showId)
        {
            List<Seat> availableSeats = new List<Seat>();

            connection.DBConnect();
            try
            {
                string query = "SELECT SeatId, SeatNumber, SeatRow, IsAvailable, Price, ShowId FROM [0Seat] WHERE IsAvailable = 1 AND ShowId = @ShowId";
                using (SqlCommand cmd = new SqlCommand(query, connection._connection))
                {
                    cmd.Parameters.AddWithValue("@ShowId", showId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seat seat = new Seat
                            {
                                SeatId = reader["SeatId"].ToString(),
                                SeatNumber = reader["SeatNumber"].ToString(),
                                SeatRow = reader["SeatRow"].ToString(),
                                IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                ShowId = reader["ShowId"].ToString()
                            };
                            availableSeats.Add(seat);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving available seats: {ex.Message}");
            }
            finally
            {
                connection.DBDisconnect();
            }

            return availableSeats;
        }



        public Booking MakeReservation(string showId, List<Seat> seats, string customerPhone)
        {
            
            if (seats == null || seats.Count == 0)
                throw new ArgumentException("No seats selected.");

            try
            {
                connection.DBConnect();
                transaction = connection._connection.BeginTransaction();
                string updateSeatQuery = "UPDATE [0Seat] SET IsAvailable = 0 WHERE SeatId = @SeatId AND ShowId = @ShowId AND IsAvailable = 1";

                using (SqlCommand command = new SqlCommand(updateSeatQuery, connection._connection, transaction))
                {
                    int totalrowsAffected = 0;
                    foreach (Seat seat in seats)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@SeatId", seat.SeatId);
                        command.Parameters.AddWithValue("@ShowId", showId);
                        int rowsAffected = command.ExecuteNonQuery();
                        totalrowsAffected += rowsAffected;

                        if (rowsAffected <= 0)
                        {
                            throw new Exception($"Seat {seat.SeatId} is no longer available.");
                        }

                    }
                }


                Console.WriteLine("All seats have been reserved temporarily. Confirm booking? (yes/no)");
                string input = Console.ReadLine()?.Trim().ToLower();


                if (input == "yes")
                {
                    Booking booking = new Booking
                    {
                        ShowId = showId,
                        CustomerPhone = customerPhone,
                    };
                    foreach (Seat seat in seats)
                    {
                        Ticket ticket = new Ticket
                        {
                            Booking = booking,
                            Seat = seat
                        };
                        booking.AddTicket(ticket);
                    }
                    string insertBookingQuery = @"
                                INSERT INTO [0Booking] (BookingId, ShowId, CustomerPhone, TotalPrice) 
                                VALUES (@BookingId, @ShowId, @CustomerPhone, @TotalPrice)";
                    using (SqlCommand bookingCmd = new SqlCommand(insertBookingQuery, connection._connection, transaction))
                    {
                        bookingCmd.Parameters.AddWithValue("@BookingId", booking.BookingId);
                        bookingCmd.Parameters.AddWithValue("@ShowId", booking.ShowId);
                        bookingCmd.Parameters.AddWithValue("@CustomerPhone", booking.CustomerPhone);
                        bookingCmd.Parameters.AddWithValue("@TotalPrice", booking.TotalPrice);
                        int rowsAffected = bookingCmd.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                        {
                            throw new Exception("Failed to insert booking.");
                        }
                    }


                    string insertTicketQuery = @"
                                INSERT INTO [0Ticket] (TicketId, BookingId, SeatId, Price) 
                                VALUES (@TicketId, @BookingId, @SeatId, @Price)";
                    using (SqlCommand ticketCmd = new SqlCommand(insertTicketQuery, connection._connection, transaction))
                    {
                        foreach (Ticket ticket in booking.Tickets)
                        {
                            ticketCmd.Parameters.Clear(); //altid vigtigt når samme command bruges i en loop
                            ticketCmd.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                            ticketCmd.Parameters.AddWithValue("@BookingId", booking.BookingId);
                            ticketCmd.Parameters.AddWithValue("@SeatId", ticket.Seat.SeatId);
                            ticketCmd.Parameters.AddWithValue("@Price", ticket.Price);
                            int rowsAffected = ticketCmd.ExecuteNonQuery();
                            if (rowsAffected <= 0)
                            {
                                throw new Exception($"Failed to insert ticket for seat {ticket.Seat.SeatId}.");
                            }
                        }
                    }

                    transaction.Commit();
                    Console.WriteLine("Booking confirmed and saved to the database.");

                    return booking;
                }
                else
                {
                    //user typed no or anything else, rollback the transaction and release the seats
                    transaction.Rollback();
                    Console.WriteLine("Booking canceled. Seats have been released.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                // Rollback releases the seat locks if an error occurs or seat wasn't available
                transaction?.Rollback();
                Console.WriteLine($"Error during booking: {ex.Message}");
                throw;

            }
            finally
            {
                connection.DBDisconnect();
            }

        }
    }
}