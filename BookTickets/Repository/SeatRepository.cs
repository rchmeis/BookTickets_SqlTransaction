using BookTickets.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Repository
{
    public class SeatRepository : ICRUD<Seat>
    {
        internal DBConnection connection = new DBConnection();
        
        public Seat Create(Seat entity)
        {
            connection.DBConnect();            
            string InsertQuery = "INSERT INTO 0Seat (SeatId, ShowId, SeatNumber, SeatRow, IsAvailable, Price) VALUES (@SeatId, @ShowId, @SeatNumber, @SeatRow, @IsAvailable, @Price)";
            try
            {
                using (SqlCommand command = new SqlCommand(InsertQuery, connection._connection))
                {
                    command.Parameters.AddWithValue("@SeatId", entity.SeatId);
                    command.Parameters.AddWithValue("@ShowId", entity.ShowId);
                    command.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
                    command.Parameters.AddWithValue("@SeatRow", entity.SeatRow);
                    command.Parameters.AddWithValue("@IsAvailable", entity.IsAvailable);
                    command.Parameters.AddWithValue("@Price", entity.Price);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine(entity + " inserted successfully.");
                        return entity;
                    }
                    else
                    {
                        throw new Exception("Failed to insert the seat into the database.");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error inserting seat: {ex.Message}");
                throw;
            }
            finally
            {
                connection.DBDisconnect();
            }  
            
        }
        public int CreateRange(List<Seat> seats)
        {
            connection.DBConnect();
            using (SqlTransaction transaction = connection._connection.BeginTransaction())
            {
                string InsertQuery = "INSERT INTO [0Seat] (SeatId, ShowId, SeatNumber, SeatRow, IsAvailable, Price) VALUES (@SeatId, @ShowId, @SeatNumber, @SeatRow, @IsAvailable, @Price)";
                try
                {
                    using (SqlCommand command = new SqlCommand(InsertQuery, connection._connection, transaction))
                    {
                        int totalRowsAffected = 0;
                        foreach (var entity in seats)
                        {
                            command.Parameters.Clear(); //vigtigt når vi genbruger samme command til at oprette flere entities

                            command.Parameters.AddWithValue("@SeatId", entity.SeatId);
                            command.Parameters.AddWithValue("@ShowId", entity.ShowId);
                            command.Parameters.AddWithValue("@SeatNumber", entity.SeatNumber);
                            command.Parameters.AddWithValue("@SeatRow", entity.SeatRow);
                            command.Parameters.AddWithValue("@IsAvailable", entity.IsAvailable);
                            command.Parameters.AddWithValue("@Price", entity.Price);
                            int rowsAffected = command.ExecuteNonQuery();
                            totalRowsAffected += rowsAffected;
                            if (rowsAffected <= 0)
                            {                                
                                throw new Exception($"Kunne ikke indsætte sædet med ID '{entity.SeatId}' i databasen.");
                            }
                        }
                        transaction.Commit();
                        Console.WriteLine(seats.Count + "Sæder tilføjet succesfuldt.");
                        return totalRowsAffected;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Fejl ved oprettelse af sæder. Handlingen blev rullet tilbage. {ex.Message}");
                    throw new Exception("Failed to insert seats into the database.");
                }
                finally
                {
                    connection.DBDisconnect();
                }
            }
        }
        public Seat Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Seat> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Seat ReadById(string id)
        {
            throw new NotImplementedException();
        }

        public Seat Update(Seat item, string id)
        {
            throw new NotImplementedException();
        }
    }
}
