using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class MainManagerLogic
    {
        public delegate void DelegateExecuteAlgorithm();

        public void RunDBSCANWithDimentionReduceAlgorithm(DelegateExecuteAlgorithm RunChosenAlgorithm, int minPts, double eps)
        {
            RunChosenAlgorithm();
            ExecuteDBSCANAndStatistics(minPts, eps);
        }

        public void RunDBSCANWithDimentionReduceAlgorithm(DelegateExecuteAlgorithm RunChosenAlgorithm, int minPts, double eps, string fileName)
        {
            RunChosenAlgorithm();
            ExecuteDBSCANAndStatistics(minPts, eps, fileName);
        }

        public void ExecuteDBSCANAndStatistics(int minPts, double eps)
        {
            runBSCANAndStatistics(minPts, eps);

            FileUtils.WriteFinalResultsToFile();
        }

        public void ExecuteDBSCANAndStatistics(int minPts, double eps, string fileName)
        {
            runBSCANAndStatistics(minPts, eps);

            FileUtils.WriteFinalResultsToFile(fileName);
        }

        private void runBSCANAndStatistics(int minPts, double eps)
        {
            DataHolder.PointsByCluster = DBSCAN.RunDBSCAN(minPts, eps);

            ClusterAnalysisService clusterAnalysisService = new ClusterAnalysisService(DataHolder.PointsByCluster);
            clusterAnalysisService.RunAnalysis();
        }

        public void UpdateDataSetAndExecuteDBSCANAndStatistics(int minPts, double eps, string filePath)
        {
            DataSetsGenerator.updateDataSet(filePath);

            ExecuteDBSCANAndStatistics(minPts, eps);
        }

        public void ResizeImages(string facesFolderPath)
        {
            FileUtils.CreateFilesList(facesFolderPath);
            ResizeImageService.ResizeImages();
        }
    }
}
