using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class KPCA : AlgorithmDimentionReduceData
    {
        public static string fileName = "kpcaResult.txt";

        public KPCA(int numOfDimentions) : base(numOfDimentions, fileName)
        {
            
        }

        public void RunKPCA()
        {
            Gaussian kernel = Gaussian.Estimate(SourceMatrix);
            var kpca = new KernelPrincipalComponentAnalysis(kernel);
            kpca.Learn(SourceMatrix);

            kpca.NumberOfOutputs = NumOfNewDimentions;
            double[][] resultMatrix = kpca.Transform(SourceMatrix);

            SaveResultToFileAndUpdateDataSet(resultMatrix);
        }
    }
}
