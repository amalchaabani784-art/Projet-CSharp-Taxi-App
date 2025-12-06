using System;
using System.Data.SQLite;

namespace Projet_C__Taxi_App.DAL
{
    public static class DatabaseSeeder
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "TaxiApp.db");

        public static void SeedTestData()
        {
            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();
            var cmd = new SQLiteCommand(connection);

            // Check if Chauffeurs table is empty
            cmd.CommandText = "SELECT COUNT(*) FROM Chauffeurs";
            long chauffeursCount = (long)cmd.ExecuteScalar();

            if (chauffeursCount == 0)
            {
                var chauffeurs = new[]
                {
                    ("Ben Ali", "Mohamed", "21234567", "TN10001"),
                    ("Trabelsi", "Sami", "21345678", "TN10002"),
                    ("Haddad", "Amira", "21456789", "TN10003"),
                    ("Mansouri", "Karim", "21567890", "TN10004"),
                    ("Jaziri", "Leila", "21678901", "TN10005")
                };

                foreach (var c in chauffeurs)
                {
                    cmd.CommandText = $"INSERT INTO Chauffeurs (Nom, Prenom, Telephone, NumeroPermis, Disponible) VALUES ('{c.Item1}','{c.Item2}','{c.Item3}','{c.Item4}',1)";
                    cmd.ExecuteNonQuery();
                }
            }

            // Check if Clients table is empty
            cmd.CommandText = "SELECT COUNT(*) FROM Clients";
            long clientsCount = (long)cmd.ExecuteScalar();

            if (clientsCount == 0)
            {
                var clients = new[]
                {
                    ("Saidi", "Fatma", "22000001", "Rue de la République, Tunis"),
                    ("Hassine", "Ali", "22000002", "Avenue Bourguiba, Sfax"),
                    ("Ben Salah", "Mouna", "22000003", "Rue de Carthage, Tunis"),
                    ("Ghannouchi", "Amine", "22000004", "Avenue Habib Bourguiba, Sousse"),
                    ("Karray", "Sabrine", "22000005", "Rue de l'Indépendance, Monastir")
                };

                foreach (var c in clients)
                {
                    cmd.CommandText = $"INSERT INTO Clients (Nom, Prenom, Telephone, Adresse) VALUES ('{c.Item1}','{c.Item2}','{c.Item3}','{c.Item4}')";
                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }
}
