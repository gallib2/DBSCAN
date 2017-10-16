using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public static class MathUtils
    {
        /// <summary>
        /// calculate Euclidean distance from one point to another.
        /// </summary>
        /// <param name="dataTestLine"></param>
        /// <param name="dataLine"></param>
        /// <returns></returns>
        public static double DistanceEuclidean(Point dataTestLine, Point dataLine)
        {
            double distance = 0;
            int length = dataTestLine.pixelLineData.Count;

            for (int i = 0; i < length; i++)
            {
                distance += Math.Pow(dataTestLine.pixelLineData[i] - dataLine.pixelLineData[i], 2);
            }

            return Math.Sqrt(distance);
        }

        /// <summary>
        /// calculate Euclidean distance from one point to another.
        /// </summary>
        /// <param name="dataTestLine"></param>
        /// <param name="dataLine"></param>
        /// <returns></returns>
        public static double DistanceEuclidean(List<double> dataTestLine, List<double> dataLine)
        {
            double distance = 0;
            int length = dataTestLine.Count;

            for (int i = 0; i < length; i++)
            {
                distance += Math.Pow(dataTestLine[i] - dataLine[i], 2);
            }

            return Math.Sqrt(distance);
        }

        // Average distance from a point to all the other points in cluster
        public static double CalculateAverageDistance(List<Point> pointsByClusters, Point point, int pointIndex)
        {
            var totalDistance = pointsByClusters.Sum(currentPoint => MathUtils.DistanceEuclidean(point, currentPoint));

            return totalDistance / pointsByClusters.Count;
        }


        public static double[] CalculateMean(List<Point> pointsByCluster)
        {
            var length = pointsByCluster[0].pixelLineData.Count;
            double[] total = new double[length];
            foreach (var point in pointsByCluster)
            {
                for (var i = 0; i < length; i++)
                {
                    total[i] += point.pixelLineData[i];
                }
            }

            double[] mean = new double[length];
            for (var j = 0; j < length; j++)
            {
                mean[j] = total[j] / pointsByCluster.Count;
            }

            return mean;
        }
    }
}
