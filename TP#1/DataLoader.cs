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
        public static List<Vin> ChargerDonneesApprentissage(string cheminFichier)
        {
            List<Vin> donneesApprentissage = new List<Vin>();

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

                        Vin vin = new Vin
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
        public static List<Vin> ChargerDonneesEchantillons(string cheminRepertoire)
        {
            List<Vin> donneesEchantillons = new List<Vin>();

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
                                Vin vin = new Vin
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
    }
}
