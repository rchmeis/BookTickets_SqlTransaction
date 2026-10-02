using BookTickets.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Repository
{
    public class ShowRepository : ICRUD<Show>
    {
        internal DBConnection connection = new DBConnection();
        public Show Create(Show entity)
        {
            connection.DBConnect();
            SqlTransaction transaction = connection._connection.BeginTransaction();

            string query = $"INSERT INTO [0Show] (ShowId, Name, ShowDate, MaxSeats) VALUES (@ShowId, @Name, @Date, @MaxSeats)";
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection._connection, transaction))
                {
                    command.Parameters.AddWithValue("@ShowId", entity.ShowId);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@Date", entity.Date);
                    command.Parameters.AddWithValue("@MaxSeats", entity.MaxSeats);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Show created successfully.");
                        transaction.Commit();
                        return entity;
                    }
                    else
                    {
                        throw new Exception("Failed to create a show in the database.");
                    }
                }
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Fejl ved oprettelsen af show i databasen: {ex.Message}");
                throw;
            }
            finally
            {
                connection.DBDisconnect();
            }
            
        }

        public Show Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Show> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Show ReadById(string id)
        {
            throw new NotImplementedException();
        }

        public Show Update(Show item, string id)
        {
            throw new NotImplementedException();
        }
    }
}
