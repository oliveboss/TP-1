using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
   
        internal abstract class Client
        {
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public int Age { get; set; }

            public abstract void Afficher();


        }
    
}
