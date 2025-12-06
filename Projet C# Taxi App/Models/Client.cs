using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_C__Taxi_App.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }

        public Client() { }

        public Client(int id, string nom, string prenom, string telephone, string adresse)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            Adresse = adresse;
        }

        public override string ToString()
        {
            return $"{Nom} {Prenom} - {Telephone} - {Adresse}";
        }
    }
}
