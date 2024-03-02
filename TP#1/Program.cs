using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cheminFichierApprentissage = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\train_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\Donnes_D_Apprentissage.csv";
            string cheminFichierTest = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\test_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\test_reduced.csv";
            string cheminFichierEchantillon = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\samples_reduced";//"C:\Users\amouz\OneDrive\Bureau\samples_reduced";

            // Charger les données d'apprentissage, de test et d'échantillon
            List<Vin> donneesApprentissage = DataLoader.ChargerDonneesApprentissage(cheminFichierApprentissage);
            List<Vin> donneesTest = DataLoader.ChargerDonneesApprentissage(cheminFichierTest);
            List<Vin> donneesEchantillon = DataLoader.ChargerDonneesEchantillons(cheminFichierEchantillon);

            // Fusionner les données d'apprentissage et d'échantillon
            List<Vin> donneesEntrainement = new List<Vin>(donneesApprentissage);
            donneesEntrainement.AddRange(donneesEchantillon);

            // Définir les attributs
            List<string> attributs = new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" };

            // Effectuer la recherche des meilleurs hyperparamètres
            int bestMaxDepth, bestMinSamplesSplit;
            Entrainement.RechercheHyperparametres(donneesEntrainement, attributs, out bestMaxDepth, out bestMinSamplesSplit);

            // Entraîner le modèle avec les meilleurs hyperparamètres
            Arbre_de_decision arbre = Entrainement.EntrainementModele(donneesEntrainement, attributs, bestMaxDepth, bestMinSamplesSplit);

            // Évaluation finale sur l'ensemble de test
            double testSetPrecision = Entrainement.EvaluationFinale(arbre, donneesTest);

            // Affichage de la précision sur l'ensemble de test
            Console.WriteLine("Précision sur l'ensemble de test : " + testSetPrecision);

            while (true)
            {
                // Demander à l'utilisateur de saisir les informations sur le vin à prédire
                Vin vinAAnalyser = new Vin();
                Console.WriteLine("Veuillez saisir les informations sur le vin à analyser :");
                Console.Write("Alcool : ");
                vinAAnalyser.Alcool = float.Parse(Console.ReadLine());
                Console.Write("Sulfate : ");
                vinAAnalyser.Sulfate = float.Parse(Console.ReadLine());
                Console.Write("Acide citrique : ");
                vinAAnalyser.Acide_citrique = float.Parse(Console.ReadLine());
                Console.Write("Acidité volatile : ");
                vinAAnalyser.Acidite_volatile = float.Parse(Console.ReadLine());

                // Utiliser le modèle pour prédire la qualité du vin
                int qualitePredite = arbre.Predire(vinAAnalyser);

                // Afficher la prédiction de qualité du vin
                Console.WriteLine("La qualité du vin prédite est : " + qualitePredite);

                // Afficher la qualité prédite en utilisant la méthode Afficher de la classe Qualité
                Qualite.Afficher(qualitePredite);

                Console.WriteLine("Voulez-vous évaluer un autre vin ? (O/N)");
                string reponse = Console.ReadLine().ToUpper();
                if (reponse != "O")
                {
                    Console.WriteLine("Voulez-vous sauvegarder les informations de ce vin ? (O/N)");
                    string choix = Console.ReadLine().ToUpper();
                    if (choix == "O")
                    {
                        SauvegarderDonneesVin(vinAAnalyser, @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\Données_sauvergardées.txt");
                        Console.WriteLine("Informations sauvegardées avec succès.");
                    }

                    break;
                }
               

               
                
            }

            Console.ReadKey();

        }
        public static void SauvegarderDonneesVin(Vin vin, string cheminFichier)
        {
            bool fichierExiste = File.Exists(cheminFichier);
            using (StreamWriter writer = new StreamWriter(cheminFichier,true))
            {
          
                string ligne = $"Alcool: {vin.Alcool},\nSulfate: {vin.Sulfate},\nAcide Citrique:{vin.Acide_citrique},\nAcidité volatile: {vin.Acidite_volatile},\nQualité: {vin.Qualite}";
                writer.WriteLine(ligne);
            }
        }

    }
}

