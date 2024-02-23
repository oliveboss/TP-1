using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Qualite
    { // private int Quality { get; set; }


        

        

        public static void Afficher(int Quality)

        {
            if (Quality >= 3 && Quality <= 4)
            {
                Console.WriteLine("La qualité du vin est plus ou moins moyenne .");
            }
            else if (Quality >= 5 && Quality <= 8)
            {
                Console.WriteLine("La qualité du vin est bonne.");
            }
            else if (Quality >= 9 && Quality <= 10)
            {
                Console.WriteLine("La qualité du vin est excellente.");
            }
            else
            {
                Console.WriteLine("La qualité du vin n'est pas définie.");



            } 
        
        }
        }
}
