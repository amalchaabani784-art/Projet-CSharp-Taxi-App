using System;
using System.IO;
using System.Data.SQLite;

namespace Projet_C__Taxi_App.DAL
{
    public static class DatabaseSetup
    {
        private static string dbPath = "Database/TaxiApp.db";

        // Call this method once to create the database and tables

        public static void InitializeDatabase()
        {
            // Ensure the Database folder exists
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullDbPath = Path.Combine(folderPath, "TaxiApp.db");

            // Create database file if it does not exist
            if (!File.Exists(fullDbPath))
            {
                SQLiteConnection.CreateFile(fullDbPath);
                Console.WriteLine("Database created at " + fullDbPath);
            }

            using (var connection = new SQLiteConnection($"Data Source={fullDbPath};Version=3;"))
            {
                connection.Open();

                // Create Chauffeurs table
                string sqlChauffeurs = @"
            CREATE TABLE IF NOT EXISTS Chauffeurs(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nom TEXT,
                Prenom TEXT,
                Telephone TEXT,
                NumeroPermis TEXT,
                Disponible INTEGER
            );";

                // Create Clients table
                string sqlClients = @"
            CREATE TABLE IF NOT EXISTS Clients(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nom TEXT,
                Prenom TEXT,
                Telephone TEXT,
                Adresse TEXT
            );";

                // Create Courses table
                string sqlCourses = @"
            CREATE TABLE IF NOT EXISTS Courses(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DateCourse TEXT,
                DistanceKm REAL,
                DureeMinutes REAL,
                Prix REAL,
                Statut TEXT,
                ChauffeurId INTEGER,
                ClientId INTEGER,
                FOREIGN KEY(ChauffeurId) REFERENCES Chauffeurs(Id),
                FOREIGN KEY(ClientId) REFERENCES Clients(Id)
            );";

                SQLiteCommand cmd = new SQLiteCommand(sqlChauffeurs, connection);
                cmd.ExecuteNonQuery();

                cmd.CommandText = sqlClients;
                cmd.ExecuteNonQuery();

                cmd.CommandText = sqlCourses;
                cmd.ExecuteNonQuery();

                connection.Close();
                Console.WriteLine("Tables created (if they did not exist).");
            }
        }

    }
}
