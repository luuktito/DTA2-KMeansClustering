using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace KMeansClustering
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Vector> vectors = new List<Vector>();
            
            //Read WineData.csv containing 32 rows and 100 columns
            var lines = File.ReadAllLines("WineData.csv").Select(
                    line => line.Split(',').Select(float.Parse).ToList())
                .ToList();

            //Loop through all rows and columns
            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[i].Count; j++)
                {
                    if (vectors.ElementAtOrDefault(j) == null) {
                        vectors.Add(new Vector());
                    }
                    //Transpose the dataset, into a list of 100 vectors containing 32 points
                    vectors[j].AddPoint(lines[i][j]);
                }
            }

            //Get the users' input
            Console.Write("How many iterations would you like to make: ");
            var iterations = int.Parse(Console.ReadLine());
            Console.Write("How many clusters would you like to create: ");
            var clusters = int.Parse(Console.ReadLine());
            Console.WriteLine("*******************************************");

            //Run the main K-Means Algorithm Loop
            var KMeansAlgorithm = new KMeansAlgorithm(iterations, clusters, vectors);
            KMeansAlgorithm.MainLoop();
        }
    }
}
