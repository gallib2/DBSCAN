using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    static public class GeneralUtils
    {
        public static double[][] CreateMatrixFromDataSet(List<Point> pointsList)
        {
            double[][] sourceMatrix = new double[pointsList.Count][/*pointsList[0].pixelLineDataLabled.Count*/];
            int i = 0, j = 0;

            foreach (var point in pointsList)
            {
                j = 0;
                sourceMatrix[i] = new double[pointsList[0].pixelLineData.Count];
                foreach (var cell in point.pixelLineData)
                {
                    sourceMatrix[i][j] = cell;
                    j++;
                }
                i++;
            }

            return sourceMatrix;
        }


        public static string AppendNameToEndOfStr(string dataLine, string name)
        {
            StringBuilder dataWithNameBuilder = new StringBuilder();

            dataWithNameBuilder.Append(dataLine);
            dataWithNameBuilder.Append(',');
            dataWithNameBuilder.Append(name);

            return dataWithNameBuilder.ToString();
        }

        public static string CreateHeadLine(int count, bool isLabled)
        {
            string nameHeadLine = "name";
            StringBuilder headLineBuilder = new StringBuilder();
            for(int i = 0; i < count; i++)
            {
                headLineBuilder.Append(i);
                if(i < count - 1)
                {
                    headLineBuilder.Append(",");
                }
            }

            if (isLabled)
            {
                headLineBuilder.Append("," + nameHeadLine);
            }

            return headLineBuilder.ToString();
        }

        public static string CreateNamesLine()
        {
            string namesList = null;
            int index = 0;
            StringBuilder nameListBuilder = new StringBuilder();

            foreach (string filePath in DataHolder.Files)
            {
                DirectoryInfo folderInfo = Directory.GetParent(filePath);
                string name = folderInfo.Name;
                nameListBuilder.Append(name);
                if (index < DataHolder.Files.Length - 1)
                {
                    nameListBuilder.Append(",");
                }
                index++;
            }
            namesList = nameListBuilder.ToString();

            return namesList;
        }



        public static List<Point> CreateAndGetPointsListFromPicturesPixels(List<string> picturePixelsList)
        {
            List<Point> points = new List<Point>();

            foreach (var item in picturePixelsList)
            {
                points.Add(new Point(item));
            }

            return points;
        }
    }
}
