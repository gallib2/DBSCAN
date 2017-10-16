using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public interface IAlgorithmData
    {
        double[][] sourceMatrix { get; set; }
        List<Point> pointsFromDataSet { get; set; }
        int numOfNewDimentions { get; set; }

    }
}
