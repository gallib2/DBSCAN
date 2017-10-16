using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class AlgorithmDimentionReduceData
    {
        public double[][] SourceMatrix { get; set; }
        public List<Point> PointsFromDataSet { get; set; }
        public int NumOfNewDimentions { get; set; }

        public readonly string fileNameToSaveResult; // = "kpcaResult";

        public AlgorithmDimentionReduceData(int numOfNewDimentions, string fileName)
        {
            NumOfNewDimentions = numOfNewDimentions;
            fileNameToSaveResult = fileName;
            createSourceMatrixAndGetPointsList();
        }


        private void createSourceMatrixAndGetPointsList()
        {
            PointsFromDataSet = GeneralUtils.CreateAndGetPointsListFromPicturesPixels(DataHolder.UnlabaledPicturePixelStrArr);
            SourceMatrix = GeneralUtils.CreateMatrixFromDataSet(PointsFromDataSet);
        }

        public void SaveResultToFileAndUpdateDataSet(double[][] resultMatrix)
        {
            FileUtils.WrtieResultFile(resultMatrix, fileNameToSaveResult);

            DataSetsGenerator.updateDataSet(fileNameToSaveResult);
        }
    }
}
