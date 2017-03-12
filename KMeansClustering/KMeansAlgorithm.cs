using System;
using System.Collections.Generic;
using System.Linq;

namespace KMeansClustering
{
    class KMeansAlgorithm
    {
        int iterations;
        int actualIterations;
        int clusters;
        double SSE;
        List<Vector> vectors;
        List<Vector> centroids;

        public KMeansAlgorithm(int iterations, int clusters, List<Vector> vectors)
        {
            this.iterations = iterations;
            this.clusters = clusters;
            this.vectors = vectors;
            centroids = new List<Vector>();
        }

        public void MainLoop() {
            //Generate a list of initial clusters
            GenerateCentroids(clusters);

            for (var i = 0; i < iterations; i++)
            {
                actualIterations = (i+1);
                var oldCentroids = new List<Vector>(centroids);
                foreach (var vector in vectors)
                {
                    //Assign each vector to the closest available centroid
                    vector.AssignToCentroid(centroids);
                }
        
                //Recompute the centroids of each cluster using the updated mean of all vectors
                RecomputeCentroids();

                //Check if any centroids have changed position, if not then break
                if (!HaveCentroidsChanged(oldCentroids)) {
                    break;
                }
            }

            ComputeSSE();
            PrintResults();
        }

        //Generate a list of initial clusters
        private void GenerateCentroids(int clusters) {
            for (int i = 0; i < clusters; i++) {
                //Add the new centroid using a random vector from the dataset
                centroids.Add(RandomVector());
            }
        }
        
        //Return a random vector from the dataset that hasn't been used as a centroid already
        private Vector RandomVector()
        {
            var randomVector = new Vector();
            while (true) {
                randomVector = vectors[new Random().Next(vectors.Count())];
                if (!centroids.Contains(randomVector))
                {
                    break;
                }
            }    
            return randomVector;
        }

        //Get the mean of all vectors in each cluster, move the centroids to those new vectors
        private void RecomputeCentroids()
        {
            for (int i = 0; i < clusters; i++) {
                var clusterSet = vectors.Where(vector => vector.centroid == i).ToList();
                var newCluster = new Vector(vectors.First().Size());
                newCluster = newCluster.Sum(clusterSet);
                newCluster = newCluster.Divide(clusterSet.Count());
                centroids[i] = newCluster;
            }
        }

        //Check if any centroids have changed position, checks if the list of vector points is the same as last iteration
        private bool HaveCentroidsChanged(List<Vector> oldCentroids) {
            var CentroidsChanged = false;
            for (int i = 0; i < centroids.Count(); i++) {
                if (!centroids[i].points.SequenceEqual(oldCentroids[i].points))
                {
                    CentroidsChanged = true;
                }
            }
            return CentroidsChanged;
        }

        //Computes the Sum of Squared Errors of all clusters after the program has run
        private void ComputeSSE() {
            for (int i = 0; i < clusters; i++)
            {
                var clusterSet = vectors.Where(vector => vector.centroid == i).ToList();

                foreach (var vector in clusterSet) {
                    SSE += Math.Pow(Vector.Distance(centroids[i], vector), 2);
                }
            }
        }

        //Print the results of the algorithm
        public void PrintResults() {
            Console.WriteLine("K-Means Results:");
            Console.WriteLine("Dataset size: " + vectors.Count() + " items, " + vectors.First().Size() + " dimensions");
            Console.WriteLine("Amount of selected iterations: " + iterations);
            Console.WriteLine("Amount of selected clusters: " + clusters);
            Console.WriteLine("Actual amount of iterations: " + actualIterations);
            Console.WriteLine("SSE: " + SSE.ToString("0.##"));

            Console.WriteLine("");

            for (int i = 0; i < centroids.Count(); i++) {
                var clusterSet = vectors.Where(vector => vector.centroid == i).ToList();
                var newCluster = new Vector(vectors.First().Size()).Sum(clusterSet);
                var newClusterDictionary = newCluster.points.Select((value, index) => new { value, index }).ToDictionary(x => x.index, x => x.value);
                var orderedCluster = newClusterDictionary.OrderByDescending(x => x.Value);

                Console.WriteLine("Cluster " + i + " contains " + clusterSet.Count() + " items (customers)");
                Console.WriteLine("*******************************************");
                
                foreach (var vector in orderedCluster) {
                    if (vector.Value > 0)
                    {
                        Console.Write("Wine " + (vector.Key + 1)  + " was purchased " + vector.Value);
                        if (vector.Value == 1) {
                            Console.WriteLine(" time");
                        }
                        else {
                            Console.WriteLine(" times");
                        }
                    }
                }
                Console.WriteLine("");
            }
            Console.SetCursorPosition(0, 0);
            Console.ReadKey(true);
        }
    }
}
