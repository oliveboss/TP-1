using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{


    internal class Vignoble
    {
        public Proprietaire Propriétaire { get; set; }
        public List<Terrain> Terrains { get; set; } = new List<Terrain>(); // Utilisation d'une liste de terrains

        public List<ArbreDeDecision.Vin> Vins { get; set; } = new List<ArbreDeDecision.Vin>();

        public Vignoble(Proprietaire propriétaire)
        {
            Propriétaire = propriétaire;
        }

        public void AjouterTerrain(Terrain terrain)
        {
            Terrains.Add(terrain);
        }
        public void SupprimerTerrain(Terrain terrain)
        {
            Terrains.Remove(terrain);
        }
        public void Afficher()
        {
            Console.WriteLine("Informations sur le vignoble :");

            if (Propriétaire != null)
            {
                Propriétaire.Afficher();
            }
            else
            {
                Console.WriteLine("Aucun propriétaire n'est associé à ce vignoble");
            }
            if (Terrains.Count > 0)
            {
                Console.WriteLine("Terrains associés :");
                foreach (var terrain in Terrains)
                {
                    terrain.Afficher();
                }
            }
            else
            {
                Console.WriteLine("Aucun terrain n'est associé à ce vignoble");
            }

           
        }
    }
}
