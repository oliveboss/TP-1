using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_1
{
    public class Arbre_de_decision
    {
        private Noeud Racine { get; set; }
        private List<string> Attributs { get; set; }
        // Propriété pour définir la profondeur maximale de l'arbre de décision


        public Arbre_de_decision(List<string> attributs)
        {
            Racine = null;
            Attributs = attributs;

        }

        public void ConstruireArbre(List<Vin> donnees, List<string> attributs, int maxDepth, int minSamplesSplit)
        {
            Racine = ConstruireArbreRecursif(donnees, attributs, maxDepth, minSamplesSplit);
        }

        private Noeud ConstruireArbreRecursif(List<Vin> donnees, List<string> attributs, int profondeurMax, int minSamplesSplit)
        {
            Noeud node = new Noeud();

            // Si toutes les instances ont la même classe ou la profondeur maximale est atteinte, créer une feuille
            if (donnees.Select(x => x.Qualite).Distinct().Count() == 1 || profondeurMax == 0)
            {
                node.EstFeuille = true;
                node.Classe = CalculerClasseMajoritaire(donnees);
                return node;
            }

            // Vérifier si le nombre d'échantillons dans les données est inférieur à la valeur minimale requise pour la division
            if (donnees.Count <= minSamplesSplit)
            {
                node.EstFeuille = true;
                node.Classe = CalculerClasseMajoritaire(donnees);
                return node;
            }

            // Calculer la classe majoritaire du nœud parent
            node.ClasseMajoritaire = CalculerClasseMajoritaire(donnees);

            // Sélectionner le meilleur attribut
            int meilleurAttributIndex = SelectionnerMeilleurAttribut(donnees, attributs);

            // Définir l'index de l'attribut pour le nœud actuel
            node.AttributIndex = meilleurAttributIndex;

            // Partitionner les données basées sur l'attribut sélectionné
            Dictionary<object, List<Vin>> donneesPartitionnees = PartitionnerDonnees(donnees, attributs[meilleurAttributIndex]);

            // Construire récursivement les sous-arbres pour chaque partition
            foreach (var kvp in donneesPartitionnees)
            {
                Noeud childNode = ConstruireArbreRecursif(kvp.Value, attributs.Except(new List<string> { attributs[meilleurAttributIndex] }).ToList(), profondeurMax - 1, minSamplesSplit);
                childNode.ValeurAttribut = kvp.Key;
                node.Enfants.Add(kvp.Key, childNode);
            }

            return node;
        }

        private int CalculerClasseMajoritaire(List<Vin> donnees)
        {
            // Compter le nombre d'occurrences de chaque classe
            Dictionary<int, int> occurrencesClasses = new Dictionary<int, int>();
            foreach (var vin in donnees)
            {
                if (occurrencesClasses.ContainsKey(vin.Qualite))
                {
                    occurrencesClasses[vin.Qualite]++;
                }
                else
                {
                    occurrencesClasses[vin.Qualite] = 1;
                }
            }

            // Trouver la classe avec le plus grand nombre d'occurrences
            int classeMajoritaire = 0;
            int maxOccurrences = 0;
            foreach (var kvp in occurrencesClasses)
            {
                if (kvp.Value > maxOccurrences)
                {
                    classeMajoritaire = kvp.Key;
                    maxOccurrences = kvp.Value;
                }
            }

            return classeMajoritaire;
        }

        private int SelectionnerMeilleurAttribut(List<Vin> donnees, List<string> attributs)
        {
            double entropieS = CalculerEntropie(donnees.Select(x => x.Qualite));

            double meilleurGainInformation = double.MinValue;
            int meilleurAttributIndex = -1;

            for (int i = 0; i < attributs.Count; i++)
            {
                if (attributs[i] == "Qualite") // Ignorer l'attribut de classe
                    continue;

                double gainInformation = entropieS - CalculerEntropiePonderee(donnees, attributs[i]);

                if (gainInformation > meilleurGainInformation)
                {
                    meilleurGainInformation = gainInformation;
                    meilleurAttributIndex = i;
                }
            }

            return meilleurAttributIndex;
        }

        private double CalculerEntropie(IEnumerable<int> classes)
        {
            Dictionary<int, int> occurencesClasses = classes.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            int totalInstances = classes.Count();

            double entropie = 0.0;
            foreach (var occurence in occurencesClasses.Values)
            {
                double probabilite = (double)occurence / totalInstances;
                entropie -= probabilite * Math.Log(probabilite, 2);
            }

            return entropie;
        }

        private double CalculerEntropiePonderee(List<Vin> donnees, string attribut)
        {
            Dictionary<object, List<Vin>> donneesPartitionnees = PartitionnerDonnees(donnees, attribut);
            double entropiePonderee = 0.0;

            foreach (var kvp in donneesPartitionnees)
            {
                double poidsPartition = (double)kvp.Value.Count / donnees.Count;
                entropiePonderee += poidsPartition * CalculerEntropie(kvp.Value.Select(x => x.Qualite));
            }

            return entropiePonderee;
        }
        private Dictionary<object, List<Vin>> PartitionnerDonnees(List<Vin> donnees, string attribut)
        {
            Dictionary<object, List<Vin>> partitions = new Dictionary<object, List<Vin>>();

            foreach (var instance in donnees)
            {
                // Vérifier si l'attribut existe dans la classe Vin
                if (!typeof(Vin).GetProperties().Any(p => p.Name == attribut))
                    throw new ArgumentException($"Nom d'attribut invalide : {attribut}");

                // Obtenir la valeur de l'attribut de l'instance de Vin
                var valeurAttribut = typeof(Vin).GetProperty(attribut).GetValue(instance);

                if (!partitions.ContainsKey(valeurAttribut))
                    partitions[valeurAttribut] = new List<Vin>();

                partitions[valeurAttribut].Add(instance);
            }

            return partitions;
        }



        public int Predire(Vin instance)
        {
            return PredireRecursif(instance, Racine);
        }

        private int PredireRecursif(Vin instance, Noeud noeud)
        {
            if (noeud.EstFeuille)
                return noeud.Classe;

            // Récupérer le nom de l'attribut correspondant à l'index stocké dans le noeud
            string attributName = Attributs[noeud.AttributIndex];

            // Utiliser le nom de l'attribut pour obtenir sa valeur à partir de l'instance Vin
            object valeurAttributInstance = instance.GetAttributeValue(attributName);

            if (noeud.Enfants.ContainsKey(valeurAttributInstance))
                return PredireRecursif(instance, noeud.Enfants[valeurAttributInstance]);

            // Si la valeur de l'attribut n'est pas présente dans les données d'entraînement,
            // retourner la classe majoritaire du nœud parent ou une autre stratégie appropriée
            return noeud.ClasseMajoritaire; // Ajoutez une propriété ClasseMajoritaire dans la classe Noeud
        }

    }
}
