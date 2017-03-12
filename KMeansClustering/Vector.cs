using System;
using System.Collections.Generic;
using System.Linq;

namespace KMeansClustering
{
    class Vector
    {
        public int centroid;
        public List<float> points = new List<float>();

        public Vector() {
        }

        public Vector(int size)
        {
            for (int i = 0; i < size; i++)
            {
                points.Add(0);
            }
        }

        public Vector(List<float> points) {
            this.points = points;
            centroid = 0;
        }

        public void AddPoint(float point) {
            points.Add(point);
        }

        public int Size() {
            return points.Count();
        }

        //Assigns the vector to the closest centroid that is given
        public void AssignToCentroid(List<Vector> centroids) {
            double distanceOld = int.MaxValue;
            for (int i = 0; i < centroids.Count(); i++) {
                double distanceNew = Distance(centroids[i], this);
                if (distanceNew < distanceOld)
                {
                    distanceOld = distanceNew; 
                    centroid = i;
                }
            }
        }

        //Calculates the distance between 2 vectors
        public static double Distance(Vector a, Vector b) {
            double distance = 0;
            for (int i = 0; i < a.points.Count(); i++) {
                distance += Math.Pow((a.points[i] - b.points[i]),2);
            }
            return Math.Sqrt(distance);
        }

        //Returns a vector containing the sum of all vectors in a cluster
        public Vector Sum(List<Vector> clusterSet) {
            for (int i = 0; i < clusterSet.Count(); i++) {
                for (int j = 0; j < points.Count(); j++) {
                    points[j] += clusterSet[i].points[j];
                }
            }
            return this;
        }

        //Returns a vector where the sum of all points in a cluster has been divided by the amount of vectors inside of that cluster
        public Vector Divide(int clusterSize) {
            for (int i = 0; i < points.Count(); i++)
            {
                points[i] /= clusterSize;
            }
            return this;
        }
    }
}
