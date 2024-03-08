using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Performance
    {
        public static double ValidationCroisee(List<ArbreDeDecision.Vin> donneesApprentissage, int folds, int maxDepth, int minSamplesSplit)
        {
            Random rand = new Random();
            List<ArbreDeDecision.Vin> donneesMelangees = donneesApprentissage.OrderBy(x => rand.Next()).ToList();

            List<List<ArbreDeDecision.Vin>> sousEnsembles = new List<List<ArbreDeDecision.Vin>>();
            int tailleSousEnsemble = donneesMelangees.Count / folds;
            for (int i = 0; i < folds; i++)
            {
                int debutIndex = i * tailleSousEnsemble;
                int finIndex = (i == folds - 1) ? donneesMelangees.Count : (i + 1) * tailleSousEnsemble;
                sousEnsembles.Add(donneesMelangees.GetRange(debutIndex, finIndex - debutIndex));
            }

            double sommePrecision = 0.0;
            foreach (var sousEnsemble in sousEnsembles)
            {

                List<ArbreDeDecision.Vin> donneesValidation = sousEnsemble;
                List<ArbreDeDecision.Vin> donneesEntrainement = donneesMelangees.Except(donneesValidation).ToList();

                ArbreDeDecision.Arbre_de_decision arbre = new ArbreDeDecision.Arbre_de_decision(new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" });
                arbre.ConstruireArbre(donneesEntrainement, new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" }, maxDepth, minSamplesSplit);

                double precision = EvaluerModele(arbre, donneesValidation);
                sommePrecision += precision;

                return 1; //chercher comment retourner la prédiction de la qualité

            }

            double precisionMoyenne = sommePrecision / folds;
            return precisionMoyenne;
        }

        public static double EvaluerModele(ArbreDeDecision.Arbre_de_decision arbre, List<ArbreDeDecision.Vin> donnees)
        {
            int predictionsCorrectes = 0;

            foreach (ArbreDeDecision.Vin instance in donnees)
            {
                int prediction = arbre.Predire(instance);
                if (prediction == instance.Qualite)
                {
                    predictionsCorrectes++;
                }
            }

            double precision = (double)predictionsCorrectes / donnees.Count;
            return precision;
        }






    }
}
