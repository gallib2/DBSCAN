using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dataMining_ex2
{
    public static class DataSetsGenerator
    {
        public static void createDataSet()
        {
            List<string> labaledPicturePixelStrArr = new List<string>();
            List<string> unlabaledPicturePixelStrArr = new List<string>();

            foreach (string filePath in DataHolder.Files)
            {
                PGM picture = new PGM(filePath);

                string labledPictuePixelsStr = createLabaledPicturePixelStr(picture, true);
                labaledPicturePixelStrArr.Add(labledPictuePixelsStr);

                string unlabledPictuePixelsStr = createLabaledPicturePixelStr(picture, false);
                unlabaledPicturePixelStrArr.Add(unlabledPictuePixelsStr);
            }

            DataHolder.LabaledPicturePixelStrArr = labaledPicturePixelStrArr;
            DataHolder.UnlabaledPicturePixelStrArr = unlabaledPicturePixelStrArr;
            DataHolder.DataLength = DataHolder.LabaledPicturePixelStrArr.Count;
            File.WriteAllLines("labeled_faces.txt", labaledPicturePixelStrArr);
            File.WriteAllLines("unlabeled_faces.txt", unlabaledPicturePixelStrArr);
        }

        public static void createDataSet(string filePathKpca)
        {
            string namesListString = GeneralUtils.CreateNamesLine();
            List<string> namesList = namesListString.Split(',').ToList();

            List<string> kpcaData = new List<string>();
            var reader = File.OpenText(filePathKpca);
            var data = reader.ReadToEnd();
            int i = 0;
            var splitedLines = data.Split('\n');

            foreach (var item in splitedLines)
            {
                if (i < splitedLines.Length - 1)
                {
                    StringBuilder sb = new StringBuilder(item);
                    sb.Append("," + namesList[i]);
                    kpcaData.Add(sb.ToString());
                    i++;
                }
            }

            DataHolder.LabaledPicturePixelStrArr = kpcaData;
            DataHolder.DataLength = DataHolder.LabaledPicturePixelStrArr.Count;
        }

        internal static void updateDataSet(string newDatafileName)
        {
            string namesListString = GeneralUtils.CreateNamesLine();
            List<string> namesList = namesListString.Split(',').ToList();

            List<string> labledData = new List<string>();
            List<string> unlabledData = new List<string>();
            var reader = File.OpenText(newDatafileName);
            var data = reader.ReadToEnd();
            int i = 0;
            var splitedLines = data.Split('\n');

            foreach (var item in splitedLines)
            {
                if (i < splitedLines.Length - 1)
                {
                    StringBuilder sbLabled = new StringBuilder(item);
                    StringBuilder sbUnLabled = new StringBuilder(item);
                    sbLabled.Append("," + namesList[i]);
                    labledData.Add(sbLabled.ToString());
                    unlabledData.Add(sbUnLabled.ToString());
                }
                i++;
            }

            DataHolder.LabaledPicturePixelStrArr = labledData;
            DataHolder.UnlabaledPicturePixelStrArr = unlabledData;
            DataHolder.DataLength = DataHolder.LabaledPicturePixelStrArr.Count;
        }

        public static string createLabaledPicturePixelStr(PGM picture, bool isLabaled)
        {
            StringBuilder picturePixelStr = new StringBuilder();
            int index = 0;

            foreach (var item in picture.Data)
            {
                picturePixelStr.Append(item.ToString());
                if(index < picture.Data.Length -1)
                {
                    picturePixelStr.Append(",");
                }
                index++;
            }

            if (isLabaled)
            {
                var folderInfo = Directory.GetParent(picture.FilePath);
                string FolderName = folderInfo.Name;
                picturePixelStr.Append(",");
                picturePixelStr.Append(FolderName);
            }

            return picturePixelStr.ToString();
        }
    }
}
