
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Proprietaire:Client
    {
        private List<Vignoble> vignoble;

        public Proprietaire(string nom, string prenom, int age)
        {
            this.Nom = nom;
            this.Prenom = prenom;
            this.Age = age;
        }

        public override void Afficher()
        {
            Console.WriteLine("Nom: " + Nom + "\nPrenom: " + Prenom + "\nAge: " + Age + "\nNombre de vignoble: " + NombreVignoble());
        }

        public int NombreVignoble()
        {
            if (vignoble != null)
            {
                return vignoble.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
