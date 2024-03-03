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
                    Console.WriteLine("Ce vin nécessite une consommation avec modération et est fortement déconseillé pour une consommation au dessus de deux verres");
                }
                else if (Vin.Alcool <= 3)
                {
                    Console.WriteLine("Avec un taux d'alcool relativement faible, ce vin promet une légèreté rafraîchissante, idéale pour les dégustations en journée.");
                }
                else
                {
                    Console.WriteLine("Un équilibre parfait d'alcool qui souligne sa structure et sa complexité, ce vin est un compagnon idéal pour les mets raffinés.");
                }
            }
        }

    }
}
