using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
  public class Entrainement
    {

        public static void RechercheHyperparametres(List<ArbreDeDecision.Vin> donneesApprentissage, List<string> attributs, out int bestMaxDepth, out int bestMinSamplesSplit)
        {
            // Définir une grille de recherche pour les hyperparamètres
            List<int> maxDepths = new List<int> { 5, 10, 15 };
            List<int> minSamplesSplits = new List<int> { 2, 5, 10 };

            double bestPrecision = 0.0;
            bestMaxDepth = 0;
            bestMinSamplesSplit = 0;

            foreach (int maxDepth in maxDepths)
            {
                foreach (int minSamplesSplit in minSamplesSplits)
                {
                    double precision = Performance.ValidationCroisee(donneesApprentissage, 5, maxDepth, minSamplesSplit);
                    if (precision > bestPrecision)
                    {
                        bestPrecision = precision;
                        bestMaxDepth = maxDepth;
                        bestMinSamplesSplit = minSamplesSplit;
                    }
                }
            }
        }

        public static ArbreDeDecision.Arbre_de_decision EntrainementModele(List<ArbreDeDecision.Vin> donneesApprentissage, List<string> attributs, int maxDepth, int minSamplesSplit)
        {
            ArbreDeDecision.Arbre_de_decision arbre = new ArbreDeDecision.Arbre_de_decision(attributs);
            arbre.ConstruireArbre(donneesApprentissage, attributs, maxDepth, minSamplesSplit);
            return arbre;
        }

        public static double EvaluationFinale(ArbreDeDecision.Arbre_de_decision arbre, List<ArbreDeDecision.Vin> donneesTest)
        {
            return Performance.EvaluerModele(arbre, donneesTest);
        }

    }
}
