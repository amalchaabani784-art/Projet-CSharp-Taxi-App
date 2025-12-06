using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__Taxi_App.Models
{
    public class Chauffeur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string NumeroPermis { get; set; }
        public bool Disponible { get; set; }

        public Chauffeur() { }

        public Chauffeur(int id, string nom, string prenom, string telephone, string numeroPermis)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            NumeroPermis = numeroPermis;
            Disponible = true;
        }

        public override string ToString()
        {
            return $"{Nom} {Prenom} - Permis: {NumeroPermis} - Disponible: {Disponible}";
        }
    }
}


