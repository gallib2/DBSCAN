using Accord.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public static class FileUtils
    {
        /// <summary>
        /// Creates CSV file from the data
        /// </summary>
        /// <param name="fileNameToSave">the name of the file for the csv</param>
        /// <param name="data">the data to save</param>
        /// <param name="isLabled">if isLabeled=true then add the lables (e.g. names) for every row</param>
        public static void createCSVFile(string fileNameToSave, List<string> data, bool isLabled)
        {
            string headLineCSV = GeneralUtils.CreateHeadLine(data.Count, isLabled);
            List<string> dataToSave = new List<string>();
            int i = 0;
            string dataToInsert;
            List<string> nameList = GeneralUtils.CreateNamesLine().Split(',').ToList();

            dataToSave.Add(headLineCSV);
            foreach (var item in data)
            {
                dataToInsert = data[i];
                if (isLabled)
                {
                    dataToInsert = GeneralUtils.AppendNameToEndOfStr(data[i], nameList[i]);
                }

                dataToSave.Add(dataToInsert);
                i++;
            }

            File.WriteAllLines(fileNameToSave + ".csv", dataToSave);
        }

        public static void WriteFinalResultsToFile(string fileName = "finalResult.txt")
        {
            StringBuilder stringToFileBuilder = new StringBuilder();

            string clusterAnalysisString = getClusterAnalysisString();
            string clustersResult = getClustersResultString();

            stringToFileBuilder.Append(clusterAnalysisString);
            stringToFileBuilder.Append("\n");
            stringToFileBuilder.Append(clustersResult);

            File.WriteAllText(fileName, stringToFileBuilder.ToString());
        }

        private static string getClustersResultString()
        {
            StringBuilder clustersResultBuilder = new StringBuilder();

            clustersResultBuilder.Append("Number of Clusters: ");
            clustersResultBuilder.Append(DataHolder.PointsByCluster.Count);
            clustersResultBuilder.Append("\n");

            foreach (var pointsOfCurrCluster in DataHolder.PointsByCluster)
            {
                clustersResultBuilder.Append("clusterd ID: ");
                clustersResultBuilder.Append(pointsOfCurrCluster.First().ClusterId);
                clustersResultBuilder.Append("\n");

                foreach (var point in pointsOfCurrCluster)
                {
                    if (point.Lable != string.Empty)
                    {
                        clustersResultBuilder.Append(" lable: ");
                        clustersResultBuilder.Append(point.Lable);
                        clustersResultBuilder.Append("\n");
                    }
                    else
                    {
                        string pixelLinedataString = getPixelLinedataString(point.pixelLineData);
                        clustersResultBuilder.Append(" data line: ");
                        clustersResultBuilder.Append(pixelLinedataString);
                        clustersResultBuilder.Append("\n");
                    }
                }
            }

            return clustersResultBuilder.ToString();
        }

        private static string getPixelLinedataString(List<double> pixelLineData)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;

            foreach (var item in pixelLineData)
            {
                sb.Append(item);

                if (index < pixelLineData.Count - 1)
                {
                    sb.Append(", ");
                }
                index++;
            }

            return sb.ToString();
        }

        private static string getClusterAnalysisString()
        {
            StringBuilder clusterAnalysisStrBuilder = new StringBuilder();

            string silhouetteString = getSilhouetteString();
            string EntropyString = getEntropyString();

            clusterAnalysisStrBuilder.Append(silhouetteString);
            clusterAnalysisStrBuilder.Append('\n');
            clusterAnalysisStrBuilder.Append(EntropyString);

            return clusterAnalysisStrBuilder.ToString();
        }

        private static string getEntropyString()
        {
            StringBuilder EntropyStringBuilder = new StringBuilder();

            EntropyStringBuilder.Append("Entropy Result:");
            EntropyStringBuilder.Append("\n");
            EntropyStringBuilder.Append("Entropy Per Cluster: ");
            EntropyStringBuilder.Append("\n");

            int clusterIndex = 1;
            foreach (var value in DataHolder.EntropyValueOfAllClusterst)
            {
                EntropyStringBuilder.Append("Cluster ");
                EntropyStringBuilder.Append(clusterIndex);
                EntropyStringBuilder.Append(": ");
                EntropyStringBuilder.Append("Entropy value: ");
                EntropyStringBuilder.Append(value);
                EntropyStringBuilder.Append("\n");

                clusterIndex++;
            }

            return EntropyStringBuilder.ToString();
        }

        private static string getSilhouetteString()
        {
            StringBuilder silhouetteStringBuilder = new StringBuilder();

            silhouetteStringBuilder.Append("Silhouette Result:");
            silhouetteStringBuilder.Append("\n");
            silhouetteStringBuilder.Append("Silhouette value for all Clusters: ");
            silhouetteStringBuilder.Append(DataHolder.SilhouetteValueGeneral);
            silhouetteStringBuilder.Append("\n");
            silhouetteStringBuilder.Append("Silhouette Per Cluster: ");
            silhouetteStringBuilder.Append("\n");

            foreach (var pair in DataHolder.SilhouetteValueByClusterID)
            {
                silhouetteStringBuilder.Append("Cluster ");
                silhouetteStringBuilder.Append(pair.Key);
                silhouetteStringBuilder.Append(": ");
                silhouetteStringBuilder.Append("silhouette value : ");
                silhouetteStringBuilder.Append(pair.Value);
                silhouetteStringBuilder.Append("\n");
            }

            return silhouetteStringBuilder.ToString();
        }

        /// <summary>
        /// Creates the file list and save it
        /// </summary>
        /// <param name="directoryName">the directory of the '.pgm' files</param>
        public static void CreateFilesList(string directoryName)
        {
            DataHolder.Files = Directory.GetFiles(directoryName, "*.pgm", SearchOption.AllDirectories);
        }

        public static void WrtieResultFile(double[][] result, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.GetColumns(0).Length; i++)
            {
                for (int j = 0; j < result.GetRow(0).Length; j++)
                {
                    sb.Append(result[i][j]);
                    if (j < result.GetRow(0).Length - 1)
                        sb.Append(", ");
                }
                sb.Append('\n');
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public static List<string> getParamsFromFile(string fileName)
        {
            var reader = File.OpenText(fileName);
            var data = reader.ReadToEnd();
            int i = 0;
            var splitedLines = data.Split('\n');

            return splitedLines.ToList();
        }
    }
}
