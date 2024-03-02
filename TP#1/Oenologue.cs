using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Oenologue : Client
    {
        public Vin Vin { get; set; }

        public Oenologue(string nom, string prenom, int age) 
        {
            this.Nom = nom;
            this.Prenom = prenom;
            this.Age = age;
        }

        public override void Afficher()
        {
            Console.WriteLine($"Nom: {Nom}\nPrenom: {Prenom}\nAge: {Age}");
        }

        public void AssocierVin(Vin vin)
        {
            this.Vin = vin;
        }

        public void EvaluerQualitéVin()
        {
            if (Vin == null)
            {
                Console.WriteLine("Aucun vin n'est associé à cet œnologue.");
            }
            else
            {
                
                Console.WriteLine($"Évaluation du vin par l'œnologue {Nom} {Prenom}:");
                
                if (Vin.Alcool > 12)
                {
                    Console.WriteLine("Ce vin nécessite une consommation avec modération");
                }
                else
                {
                    Console.WriteLine("ce vin est fait pour boire comme tu veux");
                }
            }
        }

    }
}
