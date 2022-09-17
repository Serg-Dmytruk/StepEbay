using System.Drawing;
using System.Drawing.Drawing2D;

namespace StepEbay.Common.Helpers
{
    public static class ImageHelper
    {
        private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            return orientationValue switch
            {
                1 => RotateFlipType.RotateNoneFlipNone,
                2 => RotateFlipType.RotateNoneFlipX,
                3 => RotateFlipType.Rotate180FlipNone,
                4 => RotateFlipType.Rotate180FlipX,
                5 => RotateFlipType.Rotate90FlipX,
                6 => RotateFlipType.Rotate90FlipNone,
                7 => RotateFlipType.Rotate270FlipX,
                8 => RotateFlipType.Rotate270FlipNone,
                _ => RotateFlipType.RotateNoneFlipNone
            };
        }

        public static Image FixedSize(Image image, int width, int height, bool needToFill = true)
        {
            #region Calc

            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale;
            double nScaleW;
            double nScaleH;

            nScaleW = (double)width / sourceWidth;
            nScaleH = (double)height / sourceHeight;

            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (height - sourceHeight * nScale) / 2;
                destX = (width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            var destWidth = (int)Math.Round(sourceWidth * nScale);
            var destHeight = (int)Math.Round(sourceHeight * nScale);

            #endregion

            Bitmap bmPhoto;

            try
            {
                bmPhoto = new Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format(
                    "destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, width, height), ex);
            }

            using var grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle to = new((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
            Rectangle from = new(sourceX, sourceY, sourceWidth, sourceHeight);

            grPhoto.DrawImage(image, to, from, GraphicsUnit.Pixel);

            return bmPhoto;
        }

        public static void SaveToPath(string path, MemoryStream stream)
        {
            using var image = Image.FromStream(stream);

            foreach (var prop in image.PropertyItems)
                if (prop.Id == 0x0112) //value of EXIF
                {
                    int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                    var rotateFlipType = GetOrientationToFlipType(orientationValue);
                    image.RotateFlip(rotateFlipType);
                    break;
                }

            image.Save(path);
        }

        public static void SaveToPath(string path, MemoryStream stream, int width, int height)
        {
            using var image = Image.FromStream(stream);
            {
                foreach (var prop in image.PropertyItems)
                    if (prop.Id == 0x0112) //value of EXIF
                    {
                        int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                        var rotateFlipType = GetOrientationToFlipType(orientationValue);
                        image.RotateFlip(rotateFlipType);
                        break;
                    }

                FixedSize(image, width, height).Save(path);
            }
        }
    }
}
