using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Projet_C__Taxi_App.Models;

namespace Projet_C__Taxi_App.DAL
{
    public static class ClientRepository
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "TaxiApp.db");

        public static List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Clients", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = reader["Nom"].ToString(),
                            Prenom = reader["Prenom"].ToString(),
                            Telephone = reader["Telephone"].ToString(),
                            Adresse = reader["Adresse"].ToString()
                        });
                    }
                }
                connection.Close();
            }

            return clients;
        }
    }
}
