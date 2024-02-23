using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Noeud
    {
        public int ClasseMajoritaire { get; set; } //  cette propriété pour stocker la classe majoritaire du nœud parent
        public int AttributIndex { get; set; }
        public object ValeurAttribut { get; set; }
        public bool EstFeuille { get; set; }
        public int Classe { get; set; }
        public Dictionary<object, Noeud> Enfants { get; set; }

        public Noeud()
        {
            Enfants = new Dictionary<object, Noeud>();
        }
    }
}
