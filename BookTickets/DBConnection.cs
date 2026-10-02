using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets
{
    public class DBConnection
    {        
        public SqlConnection _connection;

        public void DBConnect()
        {
            string connectionString = @"Data Source=mssql5.unoeuro.com;Initial Catalog=rachmei_dk_db_test;User ID=rachmei_dk;Password=bpzfhmew9FA2kxyEGBnR ;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public void DBDisconnect()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
