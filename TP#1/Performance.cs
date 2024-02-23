using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Performance
    {
        public static double ValidationCroisee(List<Vin> donneesApprentissage, int folds, int maxDepth, int minSamplesSplit)
        {
            Random rand = new Random();
            List<Vin> donneesMelangees = donneesApprentissage.OrderBy(x => rand.Next()).ToList();

            List<List<Vin>> sousEnsembles = new List<List<Vin>>();
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

                List<Vin> donneesValidation = sousEnsemble;
                List<Vin> donneesEntrainement = donneesMelangees.Except(donneesValidation).ToList();

                Arbre_de_decision arbre = new Arbre_de_decision(new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" });
                arbre.ConstruireArbre(donneesEntrainement, new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" }, maxDepth, minSamplesSplit);

                double precision = EvaluerModele(arbre, donneesValidation);
                sommePrecision += precision;

                return 1; //chercher comment retourner la prédiction de la qualité

            }

            double precisionMoyenne = sommePrecision / folds;
            return precisionMoyenne;
        }

        public static double EvaluerModele(Arbre_de_decision arbre, List<Vin> donnees)
        {
            int predictionsCorrectes = 0;

            foreach (Vin instance in donnees)
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
