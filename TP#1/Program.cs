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
            string cheminFichierApprentissage = @"C:\Users\amouz\OneDrive\Bureau\Donnes_D_Apprentissage.csv";
            string cheminFichierTest = @"C:\Users\amouz\OneDrive\Bureau\test_reduced.csv";
            string cheminFichierEchantillon = @"C:\Users\amouz\OneDrive\Bureau\samples_reduced";

            /*   string cheminFichierApprentissage = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\train_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\Donnes_D_Apprentissage.csv";
               string cheminFichierTest = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\test_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\test_reduced.csv";
               string cheminFichierEchantillon = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\samples_reduced";//"C:\Users\amouz\OneDrive\Bureau\samples_reduced";
            */

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
            // Création de oenologue
            Oenologue oenologue = new Oenologue("Byckel", "Koffi", 27);

            Proprietaire proprietaire = new Proprietaire("OLIVE", "KAPO", 20);



            Vignoble vignoble = new Vignoble(proprietaire);

            while (true)
            {
                Console.WriteLine("=== Menu Général ===");
                Console.WriteLine("1. Opérations de l'œnologue");
                Console.WriteLine("2. Opérations du propriétaire");
                Console.WriteLine("3. Opérations de l'administrateur");
                Console.WriteLine("4. Quitter");

                Console.Write("Choisissez une option : ");
                string choixMenu = Console.ReadLine();

                switch (choixMenu)
                {
                    case "1":
                        //Operations de l'oneologue
                        // Demander à l'utilisateur de saisir les informations sur le vin à prédire
                        while (true)
                        {
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
                            vinAAnalyser.Qualite = qualitePredite;

                            // Afficher la prédiction de qualité du vin
                            Console.WriteLine("La qualité du vin prédite est : " + qualitePredite);

                            // Afficher la qualité prédite en utilisant la méthode Afficher de la classe Qualité
                            Qualite.Afficher(qualitePredite);
                            oenologue.AssocierVin(vinAAnalyser);
                            oenologue.EvaluerQualitéVin();
                            Console.WriteLine("Voulez-vous sauvegarder les informations de ce vin ? (O/N)");
                            string choix = Console.ReadLine().ToUpper();
                            if (choix == "O")
                            {
                                DataLoader.SauvegarderDonneesVin(vinAAnalyser, @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_vin.txt");
                                Console.WriteLine("Informations sauvegardées avec succès.");
                            }
                            Console.WriteLine("Voulez-vous évaluer un autre vin ? (O/N)");
                            string reponse = Console.ReadLine().ToUpper();
                            if (reponse != "O")
                            {
                                // Sortir de la boucle si l'utilisateur ne veut pas évaluer un autre vin
                                break;
                            }
                        }
                        break;
                    case "2":

                        bool retourAuMenuGeneral = false;

                        while (!retourAuMenuGeneral)
                        {
                            Console.WriteLine("Menu du propriétaire :");
                            Console.WriteLine("1. Ajouter un terrain a votre vignoble");
                            Console.WriteLine("2. Supprimer un terrain  de sa vignoble");
                            Console.WriteLine("3. Quitter");

                            Console.Write("Choisissez une option : ");
                            string choix = Console.ReadLine();

                            switch (choix)
                            {
                                case "1":
                                    Console.WriteLine("Veuillez saisir les dimensions du terrain :");
                                    Console.Write("Longueur (m) : ");
                                    float longueurTerrain = float.Parse(Console.ReadLine());
                                    Console.Write("Largeur (m) : ");
                                    float largeurTerrain = float.Parse(Console.ReadLine());

                                    Terrain terrain = new Terrain(longueurTerrain, largeurTerrain);
                                    vignoble.AjouterTerrain(terrain);

                                    DataLoader.SauvegarderTerrain(terrain);
                                    break;

                                case "2":
                                    // Vérifier si la liste des terrains n'est pas vide
                                    if (vignoble.Terrains.Count > 0)
                                    {
                                        // Afficher la liste des terrains avec leurs indices
                                        Console.WriteLine("Liste des terrains :");
                                        for (int i = 0; i < vignoble.Terrains.Count; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. Terrain {i + 1}: Longueur = {vignoble.Terrains[i].Longueur} m, Largeur = {vignoble.Terrains[i].Largeur} m");
                                        }

                                        // Demander à l'utilisateur de choisir le terrain à supprimer
                                        Console.Write("Veuillez entrer le numéro du terrain à supprimer : ");
                                        int indiceTerrain;
                                        if (int.TryParse(Console.ReadLine(), out indiceTerrain))
                                        {
                                            // Vérifier si l'indice est valide
                                            if (indiceTerrain >= 1 && indiceTerrain <= vignoble.Terrains.Count)
                                            {
                                                // Supprimer le terrain de la liste
                                                Terrain terrainASupprimer = vignoble.Terrains[indiceTerrain - 1];
                                                vignoble.Terrains.RemoveAt(indiceTerrain - 1);
                                                DataLoader.SupprimerTerrain(vignoble.Terrains, indiceTerrain);
                                                Console.WriteLine($"Le terrain {indiceTerrain} a été supprimé avec succès.");


                                            }
                                            else
                                            {
                                                Console.WriteLine("Indice de terrain non valide.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Veuillez entrer un numéro valide.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Il n'y a aucun terrain à supprimer.");
                                    }


                                    break;

                                case "3":
                                    retourAuMenuGeneral = true;
                                    // Quitter le menu
                                    break;

                                default:
                                    Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                                    break;

                            }
                         
                            }
                        break;
                      

                    case "3":
                        // Opérations de l'administrateur
                        bool retour=false;
                        while (!retour)
                        {
                            Console.WriteLine("Opérations de l'administrateur :");
                            Console.WriteLine("1. Ajouter un propriétaire");
                            Console.WriteLine("2. Ajouter un œnologue");
                            Console.WriteLine("3. Retour au menu général");

                            Console.Write("Choisissez une option : ");
                            string choixAdmin = Console.ReadLine();

                            switch (choixAdmin)
                            {
                                case "1":
                                    // Logique pour ajouter un propriétaire
                                    break;
                                case "2":
                                    // Logique pour ajouter un œnologue
                                    break;
                                case "3":
                                    // Retour au menu général
                                    retour= true;
                                    break;
                                default:
                                    Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                                    break;
                            }
                        }
                        
                        break;
                    case "4":
                        // Quitter
                        Console.WriteLine("Au revoir !");
                        return;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                        break;
                }







            }



                }


            }
        }
    
