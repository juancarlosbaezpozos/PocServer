using Principal.Server.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Principal.Controls
{
    public static class StiReportWpfImages
    {
        [StiScaleFolderDetails(64)]
        public static class BigProgressMarker
        {
            public static StiScaleImage GetByTheme(string theme)
            {
                return GetScaledResources("BigProgressMarker." + theme, 64);
            }
        }

        public static class CloudDockingPanel
        {
            public static StiScaleImage Pin()
            {
                return GetScaledResources("CloudDockingPanel.Pin", 16);
            }

            public static StiScaleImage Unpin()
            {
                return GetScaledResources("CloudDockingPanel.Unpin", 16);
            }
        }

        public static class CloudWindow
        {
            public static StiScaleImage DotSeparator()
            {
                return GetScaledResources("CloudWindow.DotSeparator", 99, 1);
            }

            public static StiScaleImage VerticalDotSeparator()
            {
                return GetScaledResources("CloudWindow.VerticalDotSeparator", 1, 100);
            }

            public static StiScaleImage Cancel()
            {
                return GetScaledResources("CloudWindow.Cancel", 32, 32);
            }
        }

        public static class ColorButton
        {
            public static StiScaleImage MoreColors()
            {
                return GetScaledResources("ColorButton.MoreColors", 16);
            }

            public static StiScaleImage NoColor()
            {
                return GetScaledResources("ColorButton.NoColor", 16);
            }

            public static StiScaleImage Transparent()
            {
                return GetScaledResources("ColorButton.Transparent", 16);
            }
        }

        public static class ComponentsPainters
        {
            public static StiScaleImage Collapsed()
            {
                return GetScaledResources("ComponentsPainters.Collapsed", 12);
            }

            public static StiScaleImage Condition()
            {
                return GetScaledResources("ComponentsPainters.Condition", 12);
            }

            public static StiScaleImage Expanded()
            {
                return GetScaledResources("ComponentsPainters.Expanded", 12);
            }

            public static StiScaleImage FilterIcon()
            {
                return GetScaledResources("ComponentsPainters.FilterIcon", 12);
            }

            public static StiScaleImage Interactions()
            {
                return GetScaledResources("ComponentsPainters.Interactions", 12);
            }

            public static StiScaleImage Locked()
            {
                return GetScaledResources("ComponentsPainters.Locked", 12);
            }

            public static StiScaleImage ResizeHor()
            {
                return GetScaledResources("ComponentsPainters.ResizeHor", 12, 8);
            }

            public static StiScaleImage ResizeVert()
            {
                return GetScaledResources("ComponentsPainters.ResizeVert", 8, 12);
            }

            public static StiScaleImage SortAsc()
            {
                return GetScaledResources("ComponentsPainters.SortAsc", 12, 12);
            }

            public static StiScaleImage SortDesc()
            {
                return GetScaledResources("ComponentsPainters.SortDesc", 12, 12);
            }

            public static StiScaleImage TableCellCheckBox()
            {
                return GetScaledResources("ComponentsPainters.TableCellCheckBox", 12);
            }

            public static StiScaleImage TableCellImage()
            {
                return GetScaledResources("ComponentsPainters.TableCellImage", 12);
            }

            public static StiScaleImage TableCellRichText()
            {
                return GetScaledResources("ComponentsPainters.TableCellRichText", 12);
            }
        }

        public static class Editor
        {
            public static StiScaleImage Copy()
            {
                return GetScaledResources("Editor.Copy", 16);
            }

            public static StiScaleImage Help()
            {
                return GetScaledResources("Editor.Help", 16, 16);
            }

            public static StiScaleImage Attachment()
            {
                return GetScaledResources("Editor.Attachment", 16, 16);
            }

            public static StiScaleImage AttachmentWhite()
            {
                return GetScaledResources("Editor.AttachmentWhite", 16, 16);
            }

            public static StiScaleImage Error()
            {
                return GetScaledResources("Editor.Error", 16, 16);
            }

            public static StiScaleImage ErrorWhite()
            {
                return GetScaledResources("Editor.ErrorWhite", 16, 16);
            }

            public static StiScaleImage Favorites()
            {
                return GetScaledResources("Editor.Favorites", 16, 16);
            }

            public static StiScaleImage FavoritesYellow()
            {
                return GetScaledResources("Editor.FavoritesYellow", 16, 16);
            }

            public static StiScaleImage Pin()
            {
                return GetScaledResources("Editor.Pin", 16, 16);
            }

            public static StiScaleImage Pinned()
            {
                return GetScaledResources("Editor.Pinned", 16, 16);
            }

            public static StiScaleImage RunDialogButton()
            {
                return GetScaledResources("Editor.RunDialogButton", 8, 8);
            }

            public static StiScaleImage Find()
            {
                return GetScaledResources("Editor.Find", 16, 16);
            }

            public static StiScaleImage Information()
            {
                return GetScaledResources("Editor.Information", 16, 16);
            }

            public static StiScaleImage ClosePanel()
            {
                return GetScaledResources("Editor.ClosePanel", 16, 16);
            }

            public static StiScaleImage Delete()
            {
                return GetScaledResources("Editor.Delete", 16);
            }

            public static StiScaleImage Settings()
            {
                return GetScaledResources("Editor.Settings", 16);
            }
        }

        public static class Engine
        {
            public static StiScaleImage Item()
            {
                return GetScaledResources("Engine.Item", 16);
            }

            public static StiScaleImage Items()
            {
                return GetScaledResources("Engine.Items", 16);
            }

            public static StiScaleImage Report()
            {
                return GetScaledResources("Engine.Report", 16);
            }
        }

        public static class ErrorMessageControl
        {
            public static StiScaleImage WarningWhite()
            {
                return GetScaledResources("ErrorMessageControl.WarningWhite", 32);
            }

            public static StiScaleImage Information()
            {
                return GetScaledResources("ErrorMessageControl.Information", 24);
            }
        }

        public static class LeftArrow
        {
            public static StiScaleImage Default()
            {
                return GetScaledResources("LeftArrow.Default", 16, 12);
            }
        }

        public static class MessageBox
        {
            public static StiScaleImage Error()
            {
                return GetScaledResources("MessageBox.Error", 32);
            }

            public static StiScaleImage Information()
            {
                return GetScaledResources("MessageBox.Information", 32);
            }

            public static StiScaleImage Question()
            {
                return GetScaledResources("MessageBox.Question", 32);
            }

            public static StiScaleImage Warning()
            {
                return GetScaledResources("MessageBox.Warning", 32);
            }

            public static StiScaleImage Warning64()
            {
                return GetScaledResources("MessageBox.Warning64", 64);
            }
        }

        [StiScaleFolderDetails(16)]
        public static class Meters
        {
            public static void GetByName(StiMeterItemImage? image, out StiScaleImage resIcon, out StiScaleImage resIconWhite)
            {
                if (!image.HasValue)
                {
                    resIcon = null;
                    resIconWhite = null;
                }
                else
                {
                    resIcon = GetScaledResources($"Meters.{image}", 16);
                    resIconWhite = GetScaledResources($"Meters.White{image}", 16);
                }
            }
        }

        [StiScaleFolderDetails(16)]
        public static class MetersCharts
        {
            public static void GetByName(StiMeterItemImage? image, out StiScaleImage resIcon, out StiScaleImage resIconWhite)
            {
                if (!image.HasValue)
                {
                    resIcon = null;
                    resIconWhite = null;
                }
                else
                {
                    resIcon = GetScaledResources("MetersCharts." + image.ToString().Substring(6), 16);
                    resIconWhite = GetScaledResources("MetersCharts.White" + image.ToString().Substring(6), 16);
                }
            }
        }

        public static class ProgressControl
        {
            public static StiScaleImage White()
            {
                return GetScaledResources("ProgressControl.White", 16);
            }
        }

        [StiScaleFolderDetails(16)]
        public static class ProgressMarker
        {
            public static StiScaleImage GetByTheme(string theme)
            {
                return GetScaledResources("ProgressMarker." + theme, 16);
            }
        }

        [StiScaleFolderDetails(32)]
        public static class Resources
        {
            public static StiScaleImage GetByTheme(string name)
            {
                return GetScaledResources("Resources." + name, 32);
            }
        }

        public static class SystemParts
        {
            public static StiScaleImage DropDown()
            {
                return GetScaledResources("SystemParts.DropDown", 16, 16);
            }

            public static StiScaleImage Remove()
            {
                return GetScaledResources("SystemParts.Remove", 16, 16);
            }
        }

        public static class Table
        {
            public static StiScaleImage Bubble()
            {
                return GetScaledResources("Table.Bubble", 16, 16);
            }

            public static StiScaleImage ColorScale()
            {
                return GetScaledResources("Table.ColorScale", 16, 16);
            }

            public static StiScaleImage DataBars()
            {
                return GetScaledResources("Table.DataBars", 16, 16);
            }

            public static StiScaleImage Indicator()
            {
                return GetScaledResources("Table.Indicator", 16, 16);
            }

            public static StiScaleImage Sparklines()
            {
                return GetScaledResources("Table.Sparklines", 16, 16);
            }

            public static StiScaleImage SparklinesArea()
            {
                return GetScaledResources("Table.SparklinesArea", 16, 16);
            }

            public static StiScaleImage SparklinesColumn()
            {
                return GetScaledResources("Table.SparklinesColumn", 16, 16);
            }

            public static StiScaleImage SparklinesWinLoss()
            {
                return GetScaledResources("Table.SparklinesWinLoss", 16, 16);
            }
        }

        public static class Trial
        {
            public static StiScaleImage Expired()
            {
                return GetScaledResources("Trial.Expired", 112);
            }
        }

        public static class Viewer
        {
            public static StiScaleImage BigBookmarks()
            {
                return GetScaledResources("Viewer.BigBookmarks", 32);
            }

            public static StiScaleImage BigClose()
            {
                return GetScaledResources("Viewer.BigClose", 32);
            }

            public static StiScaleImage BigDotMatrixMode()
            {
                return GetScaledResources("Viewer.BigDotMatrixMode", 32);
            }

            public static StiScaleImage BigEditor()
            {
                return GetScaledResources("Viewer.BigEditor", 32);
            }

            public static StiScaleImage BigFind()
            {
                return GetScaledResources("Viewer.BigFind", 32);
            }

            public static StiScaleImage BigFullScreen()
            {
                return GetScaledResources("Viewer.BigFullScreen", 32);
            }

            public static StiScaleImage BigPageSize()
            {
                return GetScaledResources("Viewer.BigPageSize", 32);
            }

            public static StiScaleImage BigPageWidth()
            {
                return GetScaledResources("Viewer.BigPageWidth", 32);
            }

            public static StiScaleImage BigParameters()
            {
                return GetScaledResources("Viewer.BigParameters", 32);
            }

            public static StiScaleImage BigPreviewDefault()
            {
                return GetScaledResources("Viewer.BigPreviewDefault", 32);
            }

            public static StiScaleImage BigPreviewHtml()
            {
                return GetScaledResources("Viewer.BigPreviewHtml", 32);
            }

            public static StiScaleImage BigPreviewJs()
            {
                return GetScaledResources("Viewer.BigPreviewJs", 32);
            }

            public static StiScaleImage BigPreviewWeb()
            {
                return GetScaledResources("Viewer.BigPreviewWeb", 32);
            }

            public static StiScaleImage BigReportFromFile()
            {
                return GetScaledResources("Viewer.BigReportFromFile", 32);
            }

            public static StiScaleImage BigReportPrint()
            {
                return GetScaledResources("Viewer.BigReportPrint", 32);
            }

            public static StiScaleImage BigResources()
            {
                return GetScaledResources("Viewer.BigResources", 32);
            }

            public static StiScaleImage BigSendEmail()
            {
                return GetScaledResources("Viewer.BigSendEmail", 32);
            }

            public static StiScaleImage BigThumbnails()
            {
                return GetScaledResources("Viewer.BigThumbnails", 32);
            }

            public static StiScaleImage Bookmarks()
            {
                return GetScaledResources("Viewer.Bookmarks", 16);
            }

            public static StiScaleImage CollapseAll()
            {
                return GetScaledResources("Viewer.CollapseAll", 16);
            }

            public static StiScaleImage DotMatrix()
            {
                return GetScaledResources("Viewer.DotMatrix", 16);
            }

            public static StiScaleImage Editor()
            {
                return GetScaledResources("Viewer.Editor", 16);
            }

            public static StiScaleImage ExpandAll()
            {
                return GetScaledResources("Viewer.ExpandAll", 16);
            }

            public static StiScaleImage Find()
            {
                return GetScaledResources("Viewer.Find", 16);
            }

            public static StiScaleImage FindClose()
            {
                return GetScaledResources("Viewer.FindClose", 16);
            }

            public static StiScaleImage FindNext()
            {
                return GetScaledResources("Viewer.FindNext", 16);
            }

            public static StiScaleImage FindPrevious()
            {
                return GetScaledResources("Viewer.FindPrevious", 16);
            }

            public static StiScaleImage FullScreen()
            {
                return GetScaledResources("Viewer.FullScreen", 16);
            }

            public static StiScaleImage Page()
            {
                return GetScaledResources("Viewer.Page", 16);
            }

            public static StiScaleImage PageDelete()
            {
                return GetScaledResources("Viewer.PageDelete", 16);
            }

            public static StiScaleImage PageDesign()
            {
                return GetScaledResources("Viewer.PageDesign", 16);
            }

            public static StiScaleImage PageFirst()
            {
                return GetScaledResources("Viewer.PageFirst", 16);
            }

            public static StiScaleImage PageHeight24()
            {
                return GetScaledResources("Viewer.PageHeight24", 24);
            }

            public static StiScaleImage PageLast()
            {
                return GetScaledResources("Viewer.PageLast", 16);
            }

            public static StiScaleImage PageNew()
            {
                return GetScaledResources("Viewer.PageNew", 16);
            }

            public static StiScaleImage PageNext()
            {
                return GetScaledResources("Viewer.PageNext", 16);
            }

            public static StiScaleImage PagePrevious()
            {
                return GetScaledResources("Viewer.PagePrevious", 16);
            }

            public static StiScaleImage PageSize()
            {
                return GetScaledResources("Viewer.PageSize", 16);
            }

            public static StiScaleImage PageWidth24()
            {
                return GetScaledResources("Viewer.PageWidth24", 24);
            }

            public static StiScaleImage Parameters()
            {
                return GetScaledResources("Viewer.Parameters", 16);
            }

            public static StiScaleImage ReportOpen()
            {
                return GetScaledResources("Viewer.ReportOpen", 16);
            }

            public static StiScaleImage ReportPrint()
            {
                return GetScaledResources("Viewer.ReportPrint", 16);
            }

            public static StiScaleImage ReportSave()
            {
                return GetScaledResources("Viewer.ReportSave", 16);
            }

            public static StiScaleImage Resources()
            {
                return GetScaledResources("Viewer.Resources", 16);
            }

            public static StiScaleImage SaveDocumentFile()
            {
                return GetScaledResources("Viewer.SaveDocumentFile", 16);
            }

            public static StiScaleImage SendEMail()
            {
                return GetScaledResources("Viewer.SendEMail", 16);
            }

            public static StiScaleImage Thumbs()
            {
                return GetScaledResources("Viewer.Thumbs", 16);
            }

            public static StiScaleImage ViewModeContinuous()
            {
                return GetScaledResources("Viewer.ViewModeContinuous", 16);
            }

            public static StiScaleImage ViewModeMultiplePages()
            {
                return GetScaledResources("Viewer.ViewModeMultiplePages", 16);
            }

            public static StiScaleImage ViewModeSinglePage()
            {
                return GetScaledResources("Viewer.ViewModeSinglePage", 16);
            }

            public static StiScaleImage ZoomMultiplePages()
            {
                return GetScaledResources("Viewer.ZoomMultiplePages", 16);
            }

            public static StiScaleImage ZoomOnePage()
            {
                return GetScaledResources("Viewer.ZoomOnePage", 16);
            }

            public static StiScaleImage ZoomPageWidth()
            {
                return GetScaledResources("Viewer.ZoomPageWidth", 16);
            }

            public static StiScaleImage ZoomTwoPages()
            {
                return GetScaledResources("Viewer.ZoomTwoPages", 16);
            }
        }

        private static readonly Dictionary<string, StiScaleImage> cacheResources;

        private static readonly string currentExtension;

        private static readonly double currentFactor;

        private static readonly SystemScaleIconID currentScaleID;

        private static readonly Assembly assembly;

        static StiReportWpfImages()
        {
            cacheResources = new Dictionary<string, StiScaleImage>();
            currentScaleID = StiScale.IconId;
            currentExtension = GetScaleName(currentScaleID);
            currentFactor = StiScale.Factor;
            assembly = typeof(StiReportWpfImages).Assembly;
            StiWpfSkinHelper.SkinChanged += ThemeHelper_SkinChanged;
        }

        public static BitmapImage ToReportWpfBitmapImage(this StiScaleImage scaleImage)
        {
            if (scaleImage == null)
            {
                return null;
            }
            var bitmapImage = scaleImage.WpfImage as BitmapImage;
            if (bitmapImage == null)
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = scaleImage.Stream;
                bitmapImage.EndInit();
                scaleImage.WpfImage = bitmapImage;
            }
            return bitmapImage;
        }

        public static System.Windows.Controls.Image ToReportWpfImage(this StiScaleImage scaleImage)
        {
            if (scaleImage == null)
            {
                return null;
            }
            var bitmapImage = scaleImage.WpfImage as BitmapImage;
            if (bitmapImage == null)
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = scaleImage.Stream;
                bitmapImage.EndInit();
                scaleImage.WpfImage = bitmapImage;
            }
            return new System.Windows.Controls.Image
            {
                Width = scaleImage.Width,
                Height = scaleImage.Height,
                Stretch = Stretch.Uniform,
                Source = bitmapImage,
                IsHitTestVisible = false
            };
        }

        public static void InitImage(System.Windows.Controls.Image image, StiScaleImage scaleImage, bool isHitTestVisible = false)
        {
            if (scaleImage != null)
            {
                var bitmapImage = scaleImage.WpfImage as BitmapImage;
                if (bitmapImage == null)
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = scaleImage.Stream;
                    bitmapImage.EndInit();
                    scaleImage.WpfImage = bitmapImage;
                }
                image.Width = scaleImage.Width;
                image.Height = scaleImage.Height;
                image.Stretch = Stretch.Uniform;
                image.Source = bitmapImage;
                image.IsHitTestVisible = isHitTestVisible;
            }
        }

        private static string GetScaleName(SystemScaleIconID id)
        {
            string scaleName;

            switch (id)
            {
                case SystemScaleIconID.x100:
                    scaleName = "";
                    break;
                case SystemScaleIconID.x125:
                    scaleName = "_x1_25";
                    break;
                case SystemScaleIconID.x150:
                    scaleName = "_x1_5";
                    break;
                case SystemScaleIconID.x175:
                    scaleName = "_x1_75";
                    break;
                case SystemScaleIconID.x200:
                    scaleName = "_x2";
                    break;
                case SystemScaleIconID.x225:
                    scaleName = "_x2_25";
                    break;
                case SystemScaleIconID.x250:
                    scaleName = "_x2_5";
                    break;
                case SystemScaleIconID.x275:
                    scaleName = "_x2_75";
                    break;
                case SystemScaleIconID.x300:
                    scaleName = "_x3";
                    break;
                case SystemScaleIconID.x325:
                    scaleName = "_x3_25";
                    break;
                case SystemScaleIconID.x350:
                    scaleName = "_x3_5";
                    break;
                case SystemScaleIconID.x375:
                    scaleName = "_x3_75";
                    break;
                case SystemScaleIconID.x400:
                    scaleName = "_x4";
                    break;
                default:
                    scaleName = "";
                    break;
            }

            return scaleName;
        }

        private static StiScaleImage GetImageX25(string imageName, int defaultWidth, int defaultHeight)
        {
            var num = defaultWidth / 4;
            var num2 = defaultHeight / 4;
            StiScaleImage result = null;

            switch (currentScaleID)
            {
                case SystemScaleIconID.x125:
                    result = IncreaseImage(SystemScaleIconID.x100, imageName, defaultWidth + num, defaultHeight + num2, defaultWidth, defaultHeight, defaultWidth, defaultHeight);
                    break;
                case SystemScaleIconID.x175:
                    result = IncreaseImage(SystemScaleIconID.x150, imageName, defaultWidth + num * 3, defaultHeight + num2 * 3, defaultWidth + num * 2, defaultHeight + num2 * 2, defaultWidth, defaultHeight);
                    break;
                case SystemScaleIconID.x225:
                    result = IncreaseImage(SystemScaleIconID.x200, imageName, defaultWidth * 2 + num, defaultHeight * 2 + num2, defaultWidth * 2, defaultHeight * 2, defaultWidth, defaultHeight);
                    break;
                case SystemScaleIconID.x275:
                    result = IncreaseImage(SystemScaleIconID.x250, imageName, defaultWidth * 2 + num * 3, defaultHeight * 2 + num2 * 3, defaultWidth * 2 + num * 2, defaultHeight * 2 + num2 * 2, defaultWidth, defaultHeight);
                    break;
                case SystemScaleIconID.x325:
                    result = IncreaseImage(SystemScaleIconID.x300, imageName, defaultWidth * 3 + num, defaultHeight * 3 + num2, defaultWidth * 3, defaultHeight * 3, defaultWidth, defaultHeight);
                    break;
                case SystemScaleIconID.x375:
                    result = IncreaseImage(SystemScaleIconID.x350, imageName, defaultWidth * 3 + num * 3, defaultHeight * 3 + num2 * 3, defaultWidth * 3 + num * 2, defaultHeight * 3 + num2 * 2, defaultWidth, defaultHeight);
                    break;
            }

            return result;
        }

        private static StiScaleImage IncreaseImage(SystemScaleIconID baseScale, string imageName, int canvasWidth, int canvasHeight, int imageWidth, int imageHeight, int defaultWidth, int defaultHeight)
        {
            var name = "Principal.Controls.Images." + imageName + GetScaleName(baseScale) + ".png";
            var manifestResourceStream = assembly.GetManifestResourceStream(name);
            if (manifestResourceStream == null)
            {
                return null;
            }

            var stream = new MemoryStream();
            using (var image = System.Drawing.Image.FromStream(manifestResourceStream))
            {
                using (var bitmap = new Bitmap(canvasWidth, canvasHeight))
                {
                    bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        using (var imageAttributes = new ImageAttributes())
                        {
                            imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
                            var x = (canvasWidth - imageWidth) / 2;
                            var y = (canvasHeight - imageHeight) / 2;
                            var destRect = new Rectangle(x, y, image.Width, image.Height);
                            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel,
                                imageAttributes);
                        }
                    }

                    bitmap.Save(stream, ImageFormat.Png);
                }
            }

            return new StiScaleImage(imageName, defaultWidth, defaultHeight, stream);
        }

        private static StiScaleImage GetScaledResources(string path, int defaultSize, bool addToCache = true)
        {
            return GetScaledResources(path, defaultSize, defaultSize, addToCache);
        }

        private static StiScaleImage GetScaledResources(string imageName, int defaultWidth, int defaultHeight, bool addToCache = true)
        {
            if (cacheResources.TryGetValue(imageName, out var value))
            {
                return value;
            }
            var name = "Principal.Controls.Images." + imageName + currentExtension + ".png";
            var manifestResourceStream = assembly.GetManifestResourceStream(name);
            if (manifestResourceStream != null)
            {
                value = new StiScaleImage(imageName, defaultWidth, defaultHeight, manifestResourceStream);
                if (addToCache)
                {
                    cacheResources.Add(imageName, value);
                }
                return value;
            }
            value = GetImageX25(imageName, defaultWidth, defaultHeight);
            if (value != null)
            {
                if (addToCache)
                {
                    cacheResources.Add(imageName, value);
                }
                return value;
            }
            var stream = FindMaxAvailableIcon(imageName);
            if (stream != null)
            {
                var stream2 = new MemoryStream();
                var width = (int)((double)defaultWidth * currentFactor);
                var height = (int)((double)defaultHeight * currentFactor);
                var destRect = new Rectangle(0, 0, width, height);
                using (var image = System.Drawing.Image.FromStream(stream))
                {
                    using (var bitmap = new Bitmap(width, height))
                    {
                        bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            using (var imageAttributes = new ImageAttributes())
                            {
                                imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
                                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                            }
                        }
                        bitmap.Save(stream2, ImageFormat.Png);
                    }
                }
                value = new StiScaleImage(imageName, defaultWidth, defaultHeight, stream2);
                if (addToCache)
                {
                    cacheResources.Add(imageName, value);
                }
                return value;
            }
            return null;
        }

        private static Stream FindMaxAvailableIcon(string imageName)
        {
            if (StiScale.Factor > 3.0)
            {
                var name = "Principal.Controls.Images." + imageName + "_x3.png";
                var manifestResourceStream = assembly.GetManifestResourceStream(name);
                if (manifestResourceStream != null)
                {
                    return manifestResourceStream;
                }
            }
            if (StiScale.Factor > 2.0)
            {
                var name2 = "Principal.Controls.Images." + imageName + "_x2.png";
                var manifestResourceStream2 = assembly.GetManifestResourceStream(name2);
                if (manifestResourceStream2 != null)
                {
                    return manifestResourceStream2;
                }
            }
            else
            {
                var name3 = "Principal.Controls.Images." + imageName + ".png";
                var manifestResourceStream3 = assembly.GetManifestResourceStream(name3);
                if (manifestResourceStream3 != null)
                {
                    return manifestResourceStream3;
                }
            }
            return null;
        }

        private static void ThemeHelper_SkinChanged(object sender, EventArgs e)
        {
            cacheResources.Clear();
        }
    }

}
