using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
namespace TP_1
{
    internal class DataLoader
    {


        //methode pour recuperer les fichier avec la bibliotheque  csvhelper
        public static List<ArbreDeDecision.Vin> ChargerDonneesApprentissage(string cheminFichier)
        {
            List<ArbreDeDecision.Vin> donneesApprentissage = new List<ArbreDeDecision.Vin>();

            try
            {
                using (StreamReader reader = new StreamReader(cheminFichier))
                using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
                {
                    // Ignorer la première ligne (en-têtes)
                    csv.Read();

                    while (csv.Read())
                    {
                        float alcool = csv.GetField<float>(0);
                        float sulfate = csv.GetField<float>(1);
                        float acideCitrique = csv.GetField<float>(2);
                        float aciditeVolatile = csv.GetField<float>(3);
                        int qualite = csv.GetField<int>(4);

                        ArbreDeDecision.Vin vin = new ArbreDeDecision.Vin
                        {
                            Alcool = alcool,
                            Sulfate = sulfate,
                            Acide_citrique = acideCitrique,
                            Acidite_volatile = aciditeVolatile,
                            Qualite = qualite,
                        };
                        donneesApprentissage.Add(vin);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du chargement des données: " + ex.Message);
            }

            return donneesApprentissage;
        }
        public static List<ArbreDeDecision.Vin> ChargerDonneesEchantillons(string cheminRepertoire)
        {
            List<ArbreDeDecision.Vin> donneesEchantillons = new List<ArbreDeDecision.Vin>();

            try
            {
                // Vérifier si le répertoire spécifié existe
                if (!Directory.Exists(cheminRepertoire))
                {
                    Console.WriteLine($"Répertoire introuvable : {cheminRepertoire}");
                    return donneesEchantillons;
                }

                // Configuration CSV
                CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
                csvConfig.Delimiter = ";"; // Délimiteur point-virgule

                // Récupérer la liste des fichiers CSV dans le répertoire
                string[] fichiers = Directory.GetFiles(cheminRepertoire, "*.csv");

                foreach (string fichier in fichiers)
                {
                    using (StreamReader reader = new StreamReader(fichier))
                    using (CsvReader csv = new CsvReader(reader, csvConfig))
                    {
                        // Ignorer la première ligne (en-têtes)
                        csv.Read();
                        csv.ReadHeader(); // Passer à la deuxième ligne pour les données

                        while (csv.Read())
                        {
                            float alcool, sulfate, acideCitrique, aciditeVolatile;
                            int qualite;

                            // Tenter de convertir les valeurs en nombres
                            if (float.TryParse(csv.GetField("alcohol"), NumberStyles.Float, CultureInfo.InvariantCulture, out alcool) &&
                                float.TryParse(csv.GetField("sulphates"), NumberStyles.Float, CultureInfo.InvariantCulture, out sulfate) &&
                                float.TryParse(csv.GetField("citric acid"), NumberStyles.Float, CultureInfo.InvariantCulture, out acideCitrique) &&
                                float.TryParse(csv.GetField("volatile acidity"), NumberStyles.Float, CultureInfo.InvariantCulture, out aciditeVolatile) &&
                                int.TryParse(csv.GetField("quality"), out qualite))
                            {
                                ArbreDeDecision.Vin vin = new ArbreDeDecision.Vin
                                {
                                    Alcool = alcool,
                                    Sulfate = sulfate,
                                    Acide_citrique = acideCitrique,
                                    Acidite_volatile = aciditeVolatile,
                                    Qualite = qualite,
                                };
                                donneesEchantillons.Add(vin);
                            }
                            else
                            {
                                Console.WriteLine($"Erreur de format dans les valeurs des attributs dans le fichier : {fichier}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des données : {ex.Message}");
            }

            return donneesEchantillons;
        }
        private static int id;
        private static int i;
        static DataLoader()
        {
           
            ChargerDernierIndiceVin();

        }
        private static void ChargerDernierIndiceVin()
        {
            string cheminFichier = @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_vin.txt";
         id = ChargerDernierIndice(cheminFichier);
        }

      
        private static int ChargerDernierIndice(string cheminFichier)
        {
            try
            {
                if (File.Exists(cheminFichier))
                {
                    string[] lignes = File.ReadAllLines(cheminFichier);
                    int dernierIndice = 0;
                    foreach (string ligne in lignes)
                    {
                        if (ligne.StartsWith("Vin "))
                        {
                            string[] tokens = ligne.Split(' ');
                            if (int.TryParse(tokens[1], out int indice))
                            {
                                dernierIndice = Math.Max(dernierIndice, indice);
                            }
                        }
                    }
                    return dernierIndice;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du chargement du dernier indice : " + ex.Message);
                return 0;
            }
        }

        public static void SauvegarderDonneesVin(ArbreDeDecision.Vin vin, string cheminFichier, Oenologue oenologue)
        {
           
            try
            {
                using (StreamWriter writer = new StreamWriter(cheminFichier, true))
                {
                    // Écrire les informations du vin dans le fichier
                    writer.WriteLine($"Vin {id+1} : \n Alcool: " + vin.Alcool + ", Sulfate: " + vin.Sulfate + ", Acide citrique: " + vin.Acide_citrique + ", Acidité volatile: " + vin.Acidite_volatile + ", Qualité: " + vin.Qualite + "   Œnologue: " + oenologue.Nom + " " + oenologue.Prenom);
                }
                Console.WriteLine("Informations du vin sauvegardées avec succès.");
                id++;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde des données du vin : " + ex.Message);
            }
        }
      
     

        public static void SauvegarderTerrain(Terrain terrain, Proprietaire proprietaire)
        {
            
            try
            {
                string cheminFichier = @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Terrain.txt";
                using (StreamWriter writer = new StreamWriter(cheminFichier, true))
                {
                    writer.WriteLine($" Longueur = {terrain.Longueur} m, Largeur = {terrain.Largeur} m , Propriétaire associé: {proprietaire.Nom} {proprietaire.Prenom}");
                }
                Console.WriteLine("Terrain sauvegardé avec succès dans le fichier .txt.");
                i++;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde du terrain dans le fichier .txt : " + ex.Message);
            }
            
        }
       
        //Suvegarder oenologue
        public static void SauvegarderOenologue(Oenologue oenologue)
        {
            try
            {
                string cheminFichier = @"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_oenologue.txt";
                using (StreamWriter writer = new StreamWriter(cheminFichier, true))
                {
                    writer.WriteLine($"Nom: {oenologue.Nom}, Prénom: {oenologue.Prenom}, Âge: {oenologue.Age}");
                }
                Console.WriteLine("Oenologue sauvegardé avec succès dans le fichier .txt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde de l'oenologue dans le fichier .txt : " + ex.Message);
            }
        }

        public static Oenologue ChoisirOenologue()
        {
            while (true)
            {
                Console.WriteLine("Liste des œnologues disponibles :");
                string[] lignesOenologues = File.ReadAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Oenologue.txt");

                if (lignesOenologues.Length == 0)
                {
                    Console.WriteLine("Il n'y a aucun œnologue disponible pour évaluer la qualité.");
                    return null;
                }

                for (int i = 0; i < lignesOenologues.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {lignesOenologues[i]}");
                }

                // Demander à l'utilisateur de choisir l'œnologue
                Console.Write("Veuillez entrer le numéro de l'œnologue qui évaluera la qualité : ");
                if (int.TryParse(Console.ReadLine(), out int indiceOenologue))
                {
                    // Vérifier si l'indice est valide
                    if (indiceOenologue >= 1 && indiceOenologue <= lignesOenologues.Length)
                    {
                        // Récupérer les informations de l'œnologue choisi
                        string[] oenologueInfo = lignesOenologues[indiceOenologue - 1].Split(',');
                        string nomOenologue = oenologueInfo[0].Split(':')[1].Trim();
                        string prenomOenologue = oenologueInfo[1].Split(':')[1].Trim();
                        int ageOenologue = int.Parse(oenologueInfo[2].Split(':')[1].Trim());

                        // Créer et retourner l'objet Œnologue choisi
                        return new Oenologue(nomOenologue, prenomOenologue, ageOenologue);
                    }
                    else
                    {
                        Console.WriteLine("Indice d'œnologue non valide. Veuillez entrer un numéro entre 1 et " + lignesOenologues.Length + ".");
                    }
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un numéro valide.");
                }
            }
        }
        public static Proprietaire ChoisirProprietaire()
        {
            Console.WriteLine("Liste des propriétaires disponibles :");
            string[] lignesProprietaires = File.ReadAllLines(@"C:\Users\amouz\OneDrive\Bureau\TP#1\Donnees_sauvegarde\Donnees_Proprietaire.txt");

            if (lignesProprietaires.Length == 0)
            {
                Console.WriteLine("Il n'y a aucun propriétaire disponible.");
                return null;
            }

            for (int i = 0; i < lignesProprietaires.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {lignesProprietaires[i]}");
            }

            // Demander à l'utilisateur de choisir le propriétaire
            Console.Write("Veuillez entrer le numéro du propriétaire : ");
            if (int.TryParse(Console.ReadLine(), out int indiceProprietaire))
            {
                // Vérifier si l'indice est valide
                if (indiceProprietaire >= 1 && indiceProprietaire <= lignesProprietaires.Length)
                {
                    // Récupérer les informations du propriétaire choisi
                    string[] proprietaireInfo = lignesProprietaires[indiceProprietaire - 1].Split(',');
                    string nomProprietaire = proprietaireInfo[0].Split(':')[1].Trim();
                    string prenomProprietaire = proprietaireInfo[1].Split(':')[1].Trim();
                    int ageProprietaire = int.Parse(proprietaireInfo[2].Split(':')[1].Trim());

                    // Créer et retourner l'objet Proprietaire choisi
                    return new Proprietaire(nomProprietaire, prenomProprietaire, ageProprietaire);
                }
                else
                {
                    Console.WriteLine("Indice de propriétaire non valide.");
                }
            }
            else
            {
                Console.WriteLine("Veuillez entrer un numéro valide.");
            }

            return null;
        }


    }

}


