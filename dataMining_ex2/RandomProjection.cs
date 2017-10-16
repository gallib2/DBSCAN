using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class RandomProjection : AlgorithmDimentionReduceData
    {
        public double[][] ReduceMatrix { get; set; }
        public static string fileName = "ResultRandomProjection.txt";

        public RandomProjection(int numberOfDimentions) : base(numberOfDimentions, fileName)
        {

        }

        public void RunRandomProjection()
        {
            CreateRandomMat(NumOfNewDimentions);
            SaveResultToFileAndUpdateDataSet(ReduceMatrix);
        }


        public void CreateRandomMat(int newDimForMatrix)
        {
            List<Point> points = new List<Point>();

            foreach (var item in DataHolder.LabaledPicturePixelStrArr)
            {
                points.Add(new Point(item));
            }
            double[][] sourceMatrix = GeneralUtils.CreateMatrixFromDataSet(points);

            int oldDim = sourceMatrix[0].Length;
            int numOfPoints = points.Count;

            double[][] probMatrix = createMatrixOfPropability(oldDim, newDimForMatrix);

            ReduceMatrix = new double[numOfPoints][];
            int j = 0;

            // double the dataset matrix with the propability matrix 
            // to reduce the dimensionality of the data set and create new matrix
            for (int row = 0; row < numOfPoints; row++)
            {
                j = 0;
                ReduceMatrix[row] = new double[newDimForMatrix];
                for (int col = 0; col < newDimForMatrix; col++)
                {
                    double newValue = 0;
                    for (int i = 0; i < oldDim; i++)
                    {
                        newValue += sourceMatrix[row][i] * probMatrix[i][col];
                    }
                    ReduceMatrix[row][col] = newValue;
                    j++;
                }
                
            }
        }


        public static double[][] createMatrixOfPropability(int row, int col)
        {
            int i, k, index = 0;
            double mul;
            double[][] ProbabilityMatrix = new double[row][];
            //create a new matrix with the seqarte of 3 dopble (1/-1/0) 
            //each value have a different propability 
            List<int> ProbabilityList = createProbabilityList();//new List<int>();

            for (i = 0; i < row; i++)
            {
                ProbabilityMatrix[i] = new double[col];
                for (k = 0; k < col; k++)
                {
                    Random rnd = new Random();
                    //index = rnd.Next() % 6;
                    index = rnd.Next(0, 6);
                    mul = (ProbabilityList[index]) * (Math.Sqrt(3));
                    ProbabilityMatrix[i][k] = mul;
                }
            }
            return ProbabilityMatrix;
        }

        private static List<int> createProbabilityList()
        {
            List<int> ProbabilityList = new List<int>();
            ProbabilityList.Add(0);
            ProbabilityList.Add(1);
            ProbabilityList.Add(0);
            ProbabilityList.Add(-1);
            ProbabilityList.Add(0);
            ProbabilityList.Add(0);

            return ProbabilityList;
        }
    }
}