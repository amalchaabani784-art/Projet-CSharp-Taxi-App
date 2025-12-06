using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__Taxi_App.Models
{
    public class Course
    {
        public int Id { get; set; }
        public DateTime DateCourse { get; set; }
        public double DistanceKm { get; set; }
        public double DureeMinutes { get; set; }
        public double Prix { get; set; }

        public Chauffeur Chauffeur { get; set; }
        public Client Client { get; set; }

        public Course() { }

        public Course(int id, DateTime date, double distance, double duree, Chauffeur chauffeur, Client client)
        {
            Id = id;
            DateCourse = date;
            DistanceKm = distance;
            DureeMinutes = duree;
            Chauffeur = chauffeur;
            Client = client;
            Prix = CalculerPrix();
        }

        public double CalculerPrix()
        {
            // Example simple formula: 1.5 per km + 0.5 per minute
            return DistanceKm * 1.5 + DureeMinutes * 0.5;
        }

        public bool VerifierChauffeurDisponible()
        {
            return Chauffeur != null && Chauffeur.Disponible;
        }

        public override string ToString()
        {
            return $"{DateCourse}: {Client.Nom} avec {Chauffeur.Nom} - Prix: {Prix}€";
        }
    }
}
