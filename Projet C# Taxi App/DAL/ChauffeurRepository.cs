using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Projet_C__Taxi_App.Models;

namespace Projet_C__Taxi_App.DAL
{
    public static class ChauffeurRepository
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "TaxiApp.db");

        public static List<Chauffeur> GetAllChauffeurs()
        {
            List<Chauffeur> chauffeurs = new List<Chauffeur>();

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Chauffeurs", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chauffeurs.Add(new Chauffeur
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nom = reader["Nom"].ToString(),
                            Prenom = reader["Prenom"].ToString(),
                            Telephone = reader["Telephone"].ToString(),
                            NumeroPermis = reader["NumeroPermis"].ToString(),
                            Disponible = Convert.ToInt32(reader["Disponible"]) == 1
                        });
                    }
                }
                connection.Close();
            }

            return chauffeurs;
        }
    }
}
