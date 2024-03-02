using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Oenologue : Client
    {
        private Vin vin;
        public override void Afficher()
        {
            Console.WriteLine("Nom: " + Nom + "\nPrenom: " + Prenom + "\nAge: " + Age);

        }

        public void EvaluerQualitéVin()
        {
            if (vin == null)
            {
                Console.WriteLine("Aucun vin n'est associé");

            }

        }

    }
}
