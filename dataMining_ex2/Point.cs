using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public class Point
    {
        public const int NOISE = -1;
        public const int UNCLASSIFIED = 0;
        public int ClusterId;
        public List<double> pixelLineData;
        public string Lable { get; set; }
        public double SilhouetteValue { get; set; } 

        public Point(string pixelLine)
        {
            pixelLineData = convertStringToIntList(pixelLine);
        }

        private List<double> convertStringToIntList(string pixelLine)
        {
            List<double> arr = new List<double>();
            var splitArr = pixelLine.Split(',');

            foreach (var item in splitArr)
            {
                double intItem;
                if(double.TryParse(item.Trim(), out intItem))
                {
                    arr.Add(intItem);
                }
                else
                {
                    Lable = item.Trim();
                }
            }

            return arr;
        }
    }
}
