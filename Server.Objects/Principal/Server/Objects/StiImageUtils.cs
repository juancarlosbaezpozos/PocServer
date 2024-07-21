using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;

namespace Principal.Server.Objects
{
    public class StiImageUtils
    {
        public static Bitmap GetImage(Assembly imageAssembly, string imageName, bool makeTransparent)
        {
            var manifestResourceStream = imageAssembly.GetManifestResourceStream(imageName);
            if (manifestResourceStream != null)
            {
                var bitmap = new Bitmap(manifestResourceStream);
                if (makeTransparent)
                {
                    MakeImageBackgroundAlphaZero(bitmap);
                }

                return bitmap;
            }

            throw new Exception($"Can't find image '{imageName}' in resources of '{imageAssembly}' assembly");
        }

        public static bool ExistsImage(Assembly imageAssembly, string imageName)
        {
            return imageAssembly.GetManifestResourceInfo(imageName) != null;
        }

        public static void MakeImageBackgroundAlphaZero(Bitmap image)
        {
            var pixel = image.GetPixel(0, image.Height - 1);
            image.MakeTransparent();
            var color = Color.FromArgb(0, pixel);
            image.SetPixel(0, image.Height - 1, color);
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var imageWidth = width;
            var imageHeight = height;
            if (image.Width == 16 && image.Height == 16)
            {
                var num = (decimal)StiScale.Factor;
                if (num != 1.25m)
                {
                    if (num != 2.25m)
                    {
                        if (num == 2.5m)
                        {
                            imageWidth = 32;
                            imageHeight = 32;
                        }
                    }
                    else
                    {
                        imageWidth = 32;
                        imageHeight = 32;
                    }
                }
                else
                {
                    imageWidth = 16;
                    imageHeight = 16;
                }
            }

            if (image.Width == 32 && image.Height == 32)
            {
                var num2 = (decimal)StiScale.Factor;
                if (num2 != 1.25m)
                {
                    if (num2 != 2.25m)
                    {
                        if (num2 == 2.5m)
                        {
                            imageWidth = 64;
                            imageHeight = 64;
                        }
                    }
                    else
                    {
                        imageWidth = 64;
                        imageHeight = 64;
                    }
                }
                else
                {
                    imageWidth = 32;
                    imageHeight = 32;
                }
            }

            return ResizeImage(image, width, height, imageWidth, imageHeight);
        }

        public static Bitmap ResizeImage(Image image, int canvasWidth, int canvasHeight, int imageWidth, int imageHeight, bool allowSampling = false)
        {
            if (allowSampling)
            {
                var destRect = new Rectangle(0, 0, canvasWidth, canvasHeight);
                var bitmap = new Bitmap(canvasWidth, canvasHeight);
                using var graphics = Graphics.FromImage(bitmap);
                using var imageAttr = new ImageAttributes();
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                return bitmap;
            }

            var destRect2 = new Rectangle((canvasWidth - imageWidth) / 2, (canvasHeight - imageHeight) / 2, imageWidth, imageHeight);
            var bitmap2 = new Bitmap(canvasWidth, canvasHeight);
            bitmap2.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using var graphics2 = Graphics.FromImage(bitmap2);
            graphics2.CompositingMode = CompositingMode.SourceCopy;
            graphics2.CompositingQuality = CompositingQuality.HighQuality;
            graphics2.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
            using var imageAttributes = new ImageAttributes();
            imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
            graphics2.DrawImage(image, destRect2, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
            return bitmap2;
        }
    }
}
