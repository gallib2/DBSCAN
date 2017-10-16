using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class ClusterAnalysisService
    {
        static List<double> m_EntropyValueOfAllClusterst = new List<double>();
        List<List<Point>> pointsByClusters;

        public ClusterAnalysisService(List<List<Point>> pointsByClusters)
        {
            this.pointsByClusters = pointsByClusters;
        }

        public void RunAnalysis()
        {
            SilhouetteCompute();
            EntropyCompute();
        }

        public void SilhouetteCompute()
        {
            int numCentroids = pointsByClusters.Count;

            SilhouetteCompute(numCentroids, DataHolder.DataLength);
        }

        private void SilhouetteCompute(int numCentroids, int dataLength)
        {
            Dictionary<int, double> silhouetteValueByClusterID = initKeyDictionary(pointsByClusters.Count);
            double sumForClusterSilhouetteValue = 0.0;
            double sum = 0.0;

            List<double[]> means = GetCentroids(pointsByClusters, numCentroids);

            int pointIndex = 0;
            int clusterIndex = 1;
            foreach (List<Point> currentClusterPoints in pointsByClusters)
            {
                pointIndex = 0;
                sumForClusterSilhouetteValue = 0;
                foreach (Point point in currentClusterPoints)
                {
                    point.SilhouetteValue = CalculatePointSilhouette(currentClusterPoints, pointIndex, clusterIndex, point, means);
                    sum += point.SilhouetteValue;
                    sumForClusterSilhouetteValue += point.SilhouetteValue;
                    pointIndex++;
                }
                silhouetteValueByClusterID[clusterIndex] = sumForClusterSilhouetteValue / currentClusterPoints.Count;
                clusterIndex++;
            }

            DataHolder.SilhouetteValueByClusterID = silhouetteValueByClusterID;
            DataHolder.SilhouetteValueGeneral = sum / dataLength;
            //return sum / dataLength;
        }

        private static Dictionary<int, double> initKeyDictionary(int count)
        {
            Dictionary<int, double> initDictionary = new Dictionary<int, double>(count);

            for (int i = 1; i <= count; i++)
            {
                initDictionary.Add(i, 0.0);
            }

            return initDictionary;
        }

        private static double CalculatePointSilhouette(List<Point> pointsByClusters, int pointIndex, int clusterIndex, Point point, List<double[]> means)
        {
            var a_i = MathUtils.CalculateAverageDistance(pointsByClusters, point, pointIndex);

            List<double> distancesToOtherCentroids = new List<double>();
            //var pointCentroisxdId = clustering[pointIndex];
            for (int i = 0; i < means.Count; i++)
            {
                if (i != clusterIndex - 1) distancesToOtherCentroids.Add(MathUtils.DistanceEuclidean(point.pixelLineData, means[i].ToList()));
            }
            var b_i = distancesToOtherCentroids.Count == 0 ? 0 : distancesToOtherCentroids.Min();
            return (b_i - a_i) / Math.Max(a_i, b_i);
        }

        private List<double[]> GetCentroids(List<List<Point>> pointsByClusters, int numCentroids)
        {
            List<double[]> means = new List<double[]>(numCentroids);
            for (int i = 0; i < numCentroids; i++)
            {
                means.Add(MathUtils.CalculateMean(pointsByClusters[i]));
            }
            return means;
        }


        // this function go over the cluster and culculate the entropy of each cluster 
        public void EntropyCompute()
        {
            m_EntropyValueOfAllClusterst = new List<double>();

            foreach (var currentClusterPoints in pointsByClusters)
            {
                Dictionary<string, int> clusterSortDataByLable = new Dictionary<string, int>();
                //sum the amount of elements of each taype 
                foreach (var point in currentClusterPoints)
                {
                    if (clusterSortDataByLable.ContainsKey(point.Lable))
                    {
                        clusterSortDataByLable[point.Lable]++;
                    }
                    else
                    {
                        clusterSortDataByLable[point.Lable] = 0;
                    }
                }

                double clusterEntroptvalue = culcPropabilityAndReturnFinalValue(clusterSortDataByLable, currentClusterPoints.Count);
                
                //add each entropy value to a list of all the entropy values of all the clusters 
                m_EntropyValueOfAllClusterst.Add(clusterEntroptvalue);
            }

            DataHolder.EntropyValueOfAllClusterst = m_EntropyValueOfAllClusterst;
        }

        //culc the entropy value acoording to the entropt mate eqastion 
        //this function return the entropy of a singel cluster 
        private double culcPropabilityAndReturnFinalValue(Dictionary<string, int> clusterSortDataByLable, int amountOfClusters)
        {
            double entropyValue = 0;

            foreach (var item in clusterSortDataByLable)
            {
                //culc the propability of each type in the cluster  
                double ProbValue = (double)item.Value / amountOfClusters;
                //double the propability in Log *(-1)
                entropyValue += ProbValue * Math.Log(ProbValue);
            }
            return entropyValue *= (-1);
        }




    }
}
