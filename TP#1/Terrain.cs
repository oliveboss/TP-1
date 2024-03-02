using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Terrain
    {
        public float Longueur { get; set; }
        public float Largeur { get; set; }

        public Terrain(float longueur, float largeur)
        {
            Longueur = longueur;
            Largeur = largeur;
        }

        public float Surface(float longueur, float largeur)
        {
            return longueur * largeur;
        }

        public void Afficher()
        {
            Console.WriteLine("Le terrain a une surface de " + Longueur * Largeur + " mettre carré");
        }

    }
}
