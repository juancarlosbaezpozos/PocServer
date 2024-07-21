using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace Principal.Server.Objects
{
    public static class StiScaledImagesHelper
    {
        private const string ScaleX1_25 = "_x1_25";

        private const string ScaleX1_5 = "_x1_5";

        private const string ScaleX1_75 = "_x1_75";

        private const string ScaleX2 = "_x2";

        private const string ScaleX2_25 = "_x2_25";

        private const string ScaleX2_5 = "_x2_5";

        private const string ScaleX3 = "_x3";

        private const string ScaleX3_5 = "_x3_5";

        private const string ScaleX4 = "_x4";

        private const string ScaleX4_5 = "_x4_5";

        private const string ScaleX5 = "_x5";

        private const string ScaleX6 = "_x6";

        private const string ScaleX7 = "_x7";

        private const string ScaleX8 = "_x8";

        public static Image GetImage(Assembly assembly, string path, StiImageSize size = StiImageSize.Normal)
        {
            return size switch
            {
                StiImageSize.Normal => GetNormalImage(assembly, path),
                StiImageSize.OneHalf => GetOneHalfImage(assembly, path),
                _ => GetDoubleImage(assembly, path),
            };
        }

        private static Image GetNormalImage(Assembly assembly, string path)
        {
            var num = (decimal)StiScaleUI.Factor;
            if (num <= 2m)
            {
                if (num <= 1.25m)
                {
                    if (num == 1m)
                    {
                        return GetImageFromResources(assembly, path);
                    }
                    if (num == 1.25m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x1_25", "", 20, 16);
                    }
                }
                else
                {
                    if (num == 1.5m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x1_5");
                    }
                    if (num == 1.75m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x1_75", "_x1_5", 28, 24);
                    }
                    if (num == 2m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x2");
                    }
                }
            }
            else if (num <= 2.5m)
            {
                if (num == 2.25m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x2_25", "_x2", 36, 32);
                }
                if (num == 2.5m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x2_5", "_x2", 40, 32);
                }
            }
            else
            {
                if (num == 3m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x3", "_x1_5", 48, 48);
                }
                if (num == 3.5m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x3_5", "_x1_5", 56, 48);
                }
                if (num == 4m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x4", "_x2", 64, 64);
                }
            }
            return GetImageFromResourcesAndResize(assembly, path);
        }

        private static Image GetOneHalfImage(Assembly assembly, string path)
        {
            var num = (decimal)StiScaleUI.Factor;
            if (num <= 1.75m)
            {
                if (num <= 1.25m)
                {
                    if (num == 1m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x1_5");
                    }
                    if (num == 1.25m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x1_75", "_x1_5", 30, 24);
                    }
                }
                else
                {
                    if (num == 1.5m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, null, "_x2", 36, 32);
                    }
                    if (num == 1.75m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, null, "_x2", 42, 32);
                    }
                }
            }
            else if (num <= 2.25m)
            {
                if (num == 2m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x3", "_x1_5", 48, 48);
                }
                if (num == 2.25m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x3", "_x1_5", 54, 48);
                }
            }
            else
            {
                if (num == 2.5m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x3", "_x1_5", 60, 48);
                }
                if (num == 3m)
                {
                    return GetIfExistOrResizeImage(assembly, path, null, "_x4", 72, 64);
                }
            }
            return GetImageFromResourcesAndResize(assembly, path);
        }

        private static Image GetDoubleImage(Assembly assembly, string path)
        {
            var num = (decimal)StiScaleUI.Factor;
            if (num <= 2m)
            {
                if (num <= 1.25m)
                {
                    if (num == 1m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x2");
                    }
                    if (num == 1.25m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x2_5", "_x2", 40, 32);
                    }
                }
                else
                {
                    if (num == 1.5m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x3", "_x1_5", 48, 48);
                    }
                    if (num == 1.75m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x3_5", "_x1_5", 56, 48);
                    }
                    if (num == 2m)
                    {
                        return GetIfExistOrResizeImage(assembly, path, "_x4", "_x2", 64, 64);
                    }
                }
            }
            else if (num <= 2.5m)
            {
                if (num == 2.25m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x4_5", "_x2", 72, 64);
                }
                if (num == 2.5m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x5", "_x2", 80, 64);
                }
            }
            else
            {
                if (num == 3m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x6", "_x2", 96, 64);
                }
                if (num == 3.5m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x7");
                }
                if (num == 4m)
                {
                    return GetIfExistOrResizeImage(assembly, path, "_x8");
                }
            }
            return GetImageFromResourcesAndResize(assembly, path);
        }

        private static Image GetIfExistOrResizeImage(Assembly assembly, string path, string originalScale, string scale2 = null, int canvasSize = 0, int imageSize = 0)
        {
            if (originalScale != null && ExistImage(assembly, path + originalScale))
            {
                return GetImageFromResources(assembly, path + originalScale);
            }
            if (scale2 != null && ExistImage(assembly, path + scale2))
            {
                return GetImageFromResourcesAndResize(assembly, path + scale2, canvasSize, imageSize);
            }
            if (canvasSize > 0)
            {
                var nearestImageFromResourcesAndResize = GetNearestImageFromResourcesAndResize(assembly, path, canvasSize);
                if (nearestImageFromResourcesAndResize != null)
                {
                    return nearestImageFromResourcesAndResize;
                }
            }
            if (ExistImage(assembly, path ?? ""))
            {
                return GetImageFromResourcesAndResize(assembly, path);
            }
            if (ExistImage(assembly, path + "_x2"))
            {
                return GetImageFromResourcesAndResize(assembly, path + "_x2");
            }
            return null;
        }

        public static Bitmap GetNearestImageFromResourcesAndResize(Assembly assembly, string path, int canvasSize)
        {
            var list = new List<string> { "x1_25", "x1_5", "x1_75", "x2", "x2_25", "x2_5", "x3", "x3_5", "x4" };
            for (var num = list.Count - 1; num >= 0; num--)
            {
                var text = list[num];
                if (ExistImage(assembly, path + "_" + text))
                {
                    return GetImageFromResourcesAndResize(assembly, path + "_" + text, canvasSize, canvasSize, allowSampling: true) as Bitmap;
                }
            }
            return null;
        }

        public static Bitmap GetImageFromResources(Assembly assembly, string path)
        {
            return StiImageUtils.GetImage(assembly, AddImageExtToPath(path), makeTransparent: false);
        }

        private static Image GetImageFromResourcesAndResize(Assembly assembly, string path)
        {
            using var bitmap = GetImageFromResources(assembly, path);
            return StiImageUtils.ResizeImage(bitmap, StiScaleUI.XXI(bitmap.Width), StiScaleUI.YYI(bitmap.Height));
        }

        private static Image GetImageFromResourcesAndResize(Assembly assembly, string path, int canvasSize, int imageSize, bool allowSampling = false)
        {
            using var image = GetImageFromResources(assembly, path);
            return StiImageUtils.ResizeImage(image, canvasSize, canvasSize, imageSize, imageSize, allowSampling);
        }

        private static bool ExistImage(Assembly assembly, string path)
        {
            return StiImageUtils.ExistsImage(assembly, AddImageExtToPath(path));
        }

        private static string AddImageExtToPath(string path)
        {
            return path + ".png";
        }
    }

}
