using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataMining_ex2
{
    public static class ResizeImageService
    {
        private static int maxWidth = 32;
        private static int maxHeight = 30;

        public static int Width
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }

        public static int Height
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        private static void resizeImage(Image image, string imageName)
        {
            var destRect = new Rectangle(0, 0, Width, Height);
            var destImage = new Bitmap(Width, Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            ShaniSoft.Drawing.PNM.WritePNM(imageName, destImage);
        }

        public static void ResizeImages()
        {
            //Image image = ShaniSoft.Drawing.PNM.ReadPNM("C:\\Users\\Gallib\\Desktop\\faces\\an2i\\an2i_left_angry_open.pgm");
            //resizeImage(image, "new_" + "an2i_left_angry_open.pgm");

            foreach (string filePath in DataHolder.Files)
            {
                Image image = ShaniSoft.Drawing.PNM.ReadPNM(filePath);
                resizeImage(image, filePath);
            }
        }



    }
}
