using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Projet_C__Taxi_App.Models;

namespace Projet_C__Taxi_App.DAL
{
    public static class CourseRepository
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "TaxiApp.db");

        public static void InsertCourse(Course course)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                cmd.CommandText = @"
                    INSERT INTO Courses (DateCourse, DistanceKm, DureeMinutes, Prix, Statut, ChauffeurId, ClientId)
                    VALUES (@DateCourse, @DistanceKm, @DureeMinutes, @Prix, @Statut, @ChauffeurId, @ClientId)";

                cmd.Parameters.AddWithValue("@DateCourse", course.DateCourse.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DistanceKm", course.DistanceKm);
                cmd.Parameters.AddWithValue("@DureeMinutes", course.DureeMinutes);
                cmd.Parameters.AddWithValue("@Prix", course.Prix);
                cmd.Parameters.AddWithValue("@Statut", course.Statut.ToString());
                cmd.Parameters.AddWithValue("@ChauffeurId", course.Chauffeur.Id);
                cmd.Parameters.AddWithValue("@ClientId", course.Client.Id);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<Course> GetAllCourses(List<Chauffeur> chauffeurs, List<Client> clients)
        {
            List<Course> courses = new List<Course>();

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Courses", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Id"]);
                        DateTime dateCourse = DateTime.Parse(reader["DateCourse"].ToString());
                        double distance = Convert.ToDouble(reader["DistanceKm"]);
                        double duree = Convert.ToDouble(reader["DureeMinutes"]);
                        double prix = Convert.ToDouble(reader["Prix"]);
                        StatutCourse statut = Enum.Parse<StatutCourse>(reader["Statut"].ToString());

                        int chauffeurId = Convert.ToInt32(reader["ChauffeurId"]);
                        int clientId = Convert.ToInt32(reader["ClientId"]);

                        // Find matching chauffeur and client from provided lists
                        Chauffeur chauffeur = chauffeurs.Find(c => c.Id == chauffeurId);
                        Client client = clients.Find(c => c.Id == clientId);

                        Course course = new Course(id, dateCourse, distance, duree, chauffeur, client, statut);
                        courses.Add(course);
                    }
                }
                connection.Close();
            }

            return courses;
        }
        public static void UpdateCourseStatus(int courseId, StatutCourse newStatut)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                var cmd = new SQLiteCommand(connection);

                cmd.CommandText = "UPDATE Courses SET Statut = @Statut WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Statut", newStatut.ToString()); // enum -> string
                cmd.Parameters.AddWithValue("@Id", courseId);

                int rows = cmd.ExecuteNonQuery();
                connection.Close();

                if (rows > 0)
                    Console.WriteLine($"Course {courseId} status updated to {newStatut}");
                else
                    Console.WriteLine($"Course {courseId} not found!");
            }
        }

    }
}
