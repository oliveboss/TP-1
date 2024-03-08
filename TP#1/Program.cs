using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbreDeDecision;

namespace TP_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Plongez au cœur de l'art vinicole et découvrez le potentiel caché de chaque bouteille avec notre application experte en évaluation de vins. Ensemble, explorons les secrets des grands crus et donnons vie à votre expérience œnologique. Bienvenue dans votre voyage personnalisé vers l'excellence vinicole !");
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            string cheminFichierApprentissage = @"C:\Users\amouz\OneDrive\Bureau\Donnes_D_Apprentissage.csv";
            string cheminFichierTest = @"C:\Users\amouz\OneDrive\Bureau\test_reduced.csv";
            string cheminFichierEchantillon = @"C:\Users\amouz\OneDrive\Bureau\samples_reduced";

            /*   string cheminFichierApprentissage = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\train_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\Donnes_D_Apprentissage.csv";
               string cheminFichierTest = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\test_reduced.csv";//"C:\Users\amouz\OneDrive\Bureau\test_reduced.csv";
               string cheminFichierEchantillon = @"C:\Users\Joel Kayemba\OneDrive\Documents\Données - Qualité du Vin(3)\samples_reduced";//"C:\Users\amouz\OneDrive\Bureau\samples_reduced";
            */

            // Charger les données d'apprentissage, de test et d'échantillon
            List<ArbreDeDecision.Vin> donneesApprentissage = DataLoader.ChargerDonneesApprentissage(cheminFichierApprentissage);
            List<ArbreDeDecision.Vin> donneesTest = DataLoader.ChargerDonneesApprentissage(cheminFichierTest);
            List<ArbreDeDecision.Vin> donneesEchantillon = DataLoader.ChargerDonneesEchantillons(cheminFichierEchantillon);

            // Fusionner les données d'apprentissage et d'échantillon
            List<ArbreDeDecision.Vin> donneesEntrainement = new List<Vin>(donneesApprentissage);
            donneesEntrainement.AddRange(donneesEchantillon);

            // Définir les attributs
            List<string> attributs = new List<string> { "Alcool", "Sulfate", "Acide_citrique", "Acidite_volatile", "Qualite" };

            // Effectuer la recherche des meilleurs hyperparamètres
            int bestMaxDepth, bestMinSamplesSplit;
            Entrainement.RechercheHyperparametres(donneesEntrainement, attributs, out bestMaxDepth, out bestMinSamplesSplit);

            // Entraîner le modèle avec les meilleurs hyperparamètres
           ArbreDeDecision.Arbre_de_decision arbre = Entrainement.EntrainementModele(donneesEntrainement, attributs, bestMaxDepth, bestMinSamplesSplit);

            // Évaluation finale sur l'ensemble de test
            double testSetPrecision = Entrainement.EvaluationFinale(arbre, donneesTest);

            // Affichage de la précision sur l'ensemble de test
            Console.WriteLine($"Voici le moment de vérité : la précision sur notre ensemble de test atteint un impressionnant {testSetPrecision}%, révélant l'extraordinaire précision de notre modèle dans l'art délicat d'évaluer la qualité des vins. Célébrons ensemble cette réussite qui marque un pas de géant vers la maîtrise de l'univers vinicole !");
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            // Création de oenologue
            Proprietaire proprietaire = new Proprietaire("OLIVE", "KAPO", 20);



            Vignoble vignoble = new Vignoble(proprietaire);

            while (true)
            {
                Console.WriteLine("=== Menu Général ===");
                Console.WriteLine("1. Opérations de l'œnologue: Plongez dans l'analyse sensorielle et dévoilez les secrets cachés dans chaque millésime.");
                Console.WriteLine("2. Opérations du propriétaire: Gérez avec précision le patrimoine de votre domaine, de la vigne au verre.");
                Console.WriteLine("3. Opérations de l'administrateur: Supervisez l'orchestre de notre application pour garantir une symphonie sans fausse note.");
                Console.WriteLine("4. Quitter");

                Console.Write("Choisissez une option : ");
                string choixMenu = Console.ReadLine();

                switch (choixMenu)
                {
                    case "1":


                        //Operations de l'oneologue
                        // Demander à l'utilisateur de saisir les informations sur le vin à prédire

                        Oenologue oenologueChoisi = DataLoader.ChoisirOenologue();
                        while (true)
                        {
                            ArbreDeDecision.Vin vinAAnalyser = new ArbreDeDecision.Vin();
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
                            if (oenologueChoisi != null)
                            {
                                // Utiliser l'œnologue choisi pour évaluer la qualité du vin
                                
                                 oenologueChoisi.AssocierVin(vinAAnalyser);
                                 oenologueChoisi.EvaluerQualitéVin();
                               
                            }
                            Console.WriteLine("Voulez-vous sauvegarder les informations de ce vin ? (O/N)");
                            string choix = Console.ReadLine().ToUpper();
                            if (choix == "O")
                            {
                                DataLoader.SauvegarderDonneesVin(vinAAnalyser, @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_vin.txt",oenologueChoisi);
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
                        Proprietaire proprietaireChoisi = DataLoader.ChoisirProprietaire();
                        
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

                                    DataLoader.SauvegarderTerrain(terrain,proprietaireChoisi);
                                    break;
                                case "2":
                                    // Afficher la liste des terrains avec leurs indices
                                    Console.WriteLine("Liste des terrains :");
                                    string[] lignesTerrains = File.ReadAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Terrain.txt");
                                    if (lignesTerrains.Length == 0)
                                    {
                                        Console.WriteLine("Il n'y a aucun terrain à supprimer.");
                                    }
                                    else
                                    {
                                        for (int i = 0; i < lignesTerrains.Length; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {lignesTerrains[i]}");
                                        }

                                        // Demander à l'utilisateur de choisir le terrain à supprimer
                                        Console.Write("Veuillez entrer le numéro du terrain à supprimer : ");
                                        int indiceTerrain;
                                        if (int.TryParse(Console.ReadLine(), out indiceTerrain))
                                        {
                                            // Vérifier si l'indice est valide
                                            if (indiceTerrain >= 1 && indiceTerrain <= lignesTerrains.Length)
                                            {
                                                // Supprimer la ligne correspondant à l'indice dans le fichier
                                                List<string> lignesModifiees = lignesTerrains.ToList();
                                                lignesModifiees.RemoveAt(indiceTerrain - 1);
                                                File.WriteAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Terrain.txt", lignesModifiees);
                                                Console.WriteLine("Terrain supprimé avec succès !");
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
                            Console.WriteLine("2. Supprimer un propriétaire");
                            Console.WriteLine("3. Ajouter un œnologue");
                            Console.WriteLine("4. Supprimer un propriétaire");
                            Console.WriteLine("5. Retour au menu général");

                            Console.Write("Choisissez une option : ");
                            string choixAdmin = Console.ReadLine();

                            switch (choixAdmin)
                            {
                                case "1":
                                    // Logique pour ajouter un propriétaire
                                    Console.WriteLine("Veuillez saisir les informations du propriétaire :");
                                    Console.Write("Nom : ");
                                    string nomProprietaire = Console.ReadLine();
                                    Console.Write("Prénom : ");
                                    string prenomProprietaire = Console.ReadLine();
                                    Console.Write("Age : ");
                                    int ageProprietaire = int.Parse(Console.ReadLine());

                                    // Création d'une chaîne contenant les informations du nouveau propriétaire
                                    string nouvelleEntree = $"Nom: {nomProprietaire}, Prénom: {prenomProprietaire}, Âge: {ageProprietaire}";

                                    // Ajouter la nouvelle entrée à la fin du fichier
                                    File.AppendAllText(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Proprietaire.txt", nouvelleEntree + Environment.NewLine);

                                    Console.WriteLine("Propriétaire ajouté avec succès !");

                                    break;
                                
                                case "2":
                                    string cheminFichier = @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Terrain.txt";

                                    // Vérifier si le fichier de données des terrains existe
                                    if (File.Exists(cheminFichier))
                                    {
                                        // Lire toutes les lignes du fichier
                                        string[] lines = File.ReadAllLines(cheminFichier);

                                        // Vérifier si le fichier contient des données
                                        if (lines.Length > 0)
                                        {
                                            // Afficher la liste des terrains pour le propriétaire choisi
                                            Console.WriteLine("Liste des terrains :");
                                            bool terrainTrouve = false;
                                            for (int i = 0; i < lines.Length; i += 2)
                                            {
                                                string[] terrainInfo = lines[i].Split(new string[] { ", Propriétaire associé: " }, StringSplitOptions.None);
                                                string proprietaireInfo = terrainInfo[1];
                                                if (proprietaireInfo == $"{proprietaire.Nom} {proprietaire.Prenom}")
                                                {
                                                    Console.WriteLine(lines[i]);
                                                    terrainTrouve = true;
                                                }
                                            }

                                            if (!terrainTrouve)
                                            {
                                                Console.WriteLine("Aucun terrain trouvé pour ce propriétaire.");
                                            }
                                            else
                                            {
                                                // Demander à l'utilisateur de choisir le terrain à supprimer
                                                Console.Write("Veuillez entrer le numéro du terrain à supprimer : ");
                                                if (int.TryParse(Console.ReadLine(), out int choixTerrain))
                                                {
                                                    // Vérifier si l'indice est valide
                                                    if (choixTerrain >= 1 && choixTerrain <= lines.Length / 2)
                                                    {
                                                        // Supprimer les lignes correspondant au terrain choisi dans le fichier
                                                        List<string> modifiedLines = lines.ToList();
                                                        modifiedLines.RemoveAt((choixTerrain - 1) * 2); // Supprimer la ligne du terrain
                                                        modifiedLines.RemoveAt((choixTerrain - 1) * 2); // Supprimer la ligne du propriétaire
                                                        File.WriteAllLines(cheminFichier, modifiedLines);

                                                        Console.WriteLine($"Le terrain {choixTerrain} a été supprimé avec succès.");
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
                                        }
                                        else
                                        {
                                            Console.WriteLine("Il n'y a aucun terrain à supprimer.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Le fichier de données des terrains n'existe pas.");
                                    }
                                    break;

                                case "3":
                                    // Logique pour ajouter un œnologue
                                    Console.WriteLine("Veuillez saisir les informations de l'œnologue :");
                                    Console.Write("Nom : ");
                                    string nomOenologue = Console.ReadLine();
                                    Console.Write("Prénom : ");
                                    string prenomOenologue = Console.ReadLine();
                                    Console.Write("Age : ");
                                    int ageOenologue = int.Parse(Console.ReadLine());

                                    // Création d'une chaîne contenant les informations du nouvel œnologue
                                    string nouvelleEntreeOenologue = $"Nom: {nomOenologue}, Prénom: {prenomOenologue}, Âge: {ageOenologue}";

                                    // Ajouter la nouvelle entrée à la fin du fichier des œnologues
                                    File.AppendAllText(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Oenologue.txt", nouvelleEntreeOenologue + Environment.NewLine);

                                    Console.WriteLine("Œnologue ajouté avec succès !");

                                    break;


                                case "4":
                                    /// Supprimer un œnologue
                                    // Afficher la liste des œnologues avec leurs indices
                                    Console.WriteLine("Liste des œnologues :");
                                    string[] lignesOenologues = File.ReadAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Oenologue.txt");
                                    if (lignesOenologues.Length == 0)
                                    {
                                        Console.WriteLine("Il n'y a aucun œnologue à supprimer.");
                                    }
                                    else
                                    {
                                        for (int i = 0; i < lignesOenologues.Length; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {lignesOenologues[i]}");
                                        }

                                        // Demander à l'utilisateur de choisir l'œnologue à supprimer
                                        Console.Write("Veuillez entrer le numéro de l'œnologue à supprimer : ");
                                        int indiceOenologue;
                                        if (int.TryParse(Console.ReadLine(), out indiceOenologue))
                                        {
                                            // Vérifier si l'indice est valide
                                            if (indiceOenologue >= 1 && indiceOenologue <= lignesOenologues.Length)
                                            {
                                                // Supprimer la ligne correspondant à l'indice dans le fichier
                                                List<string> lignesModifiees = lignesOenologues.ToList();
                                                lignesModifiees.RemoveAt(indiceOenologue - 1);
                                                File.WriteAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Oenologue.txt", lignesModifiees);
                                                Console.WriteLine("Œnologue supprimé avec succès !");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Indice d'œnologue non valide.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Veuillez entrer un numéro valide.");
                                        }
                                    }
                                    break;

                               
                                    
                                case "5":
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
    
