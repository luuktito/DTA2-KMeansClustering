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
            while (clusters > vectors.Count()) {
                Console.Write("You can't create more clusters than datapoints, please select a value lower than " + vectors.Count() + ": ");
                clusters = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("*******************************************");

            //Get the best value for clusters (amount) by minimizing the SSE
            double[] SSEGroup = new double[10];
            for (var i = 1; i < 11; i++)
            {
                var KMeansAlgorithmDebug = new KMeansAlgorithm(iterations, i, vectors, true);
                KMeansAlgorithmDebug.MainLoop();
                SSEGroup[i-1] = KMeansAlgorithmDebug.SSE1;
            }
            Console.WriteLine("Best value for k (amount of clusters) after minimizing the SSE for k = 1 through 10:");
            Console.WriteLine("k = " + Convert.ToInt32(Array.IndexOf(SSEGroup, SSEGroup.Min()) + 1) + ": " + SSEGroup.Min());
            Console.WriteLine("Increasing k will almost always minimize the SSE, so it makes sense that k is high");
            Console.WriteLine("*******************************************");

            //Run the main K-Means Algorithm Loop
            var KMeansAlgorithm = new KMeansAlgorithm(iterations, clusters, vectors, false);
            KMeansAlgorithm.MainLoop();
        }
    }
}
