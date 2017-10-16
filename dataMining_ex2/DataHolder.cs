using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    static public class DataHolder
    {
        public static string[] Files { get; set; }
        public static List<string> LabaledPicturePixelStrArr { get; set; }
        public static List<string> UnlabaledPicturePixelStrArr { get; set; }
        public static int DataLength { get; set; }

        public static double SilhouetteValueGeneral { get; set; }

        // Key : clusterID, Value: Silhouette Value
        public static Dictionary<int, double> SilhouetteValueByClusterID { get; set; }

        public static List<double> EntropyValueOfAllClusterst { get; set; }
 
        public static List<List<Point>> PointsByCluster { get; set; }

        private static int currentDimentionsNumber;

        public static int CurrentDimentionsNumber
        {
            get { return UnlabaledPicturePixelStrArr.First().Split(',').Length; }
            set { currentDimentionsNumber = value; }
        }

    }
}
