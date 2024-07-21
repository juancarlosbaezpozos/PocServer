using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Principal.Server.Objects;

namespace Principal.Server.Controller.Images;

public static class StiControllerImages
{
    public static class Items
    {
        public static BitmapImage Build(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Build", size);
        }

        public static BitmapImage Check(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Check", size);
        }

        public static BitmapImage ClearContent(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ClearContent", size);
        }

        public static BitmapImage Copy(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Copy", size);
        }

        public static BitmapImage Cut(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Cut", size);
        }

        public static BitmapImage Error(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Error", size);
        }

        public static BitmapImage FileOpen(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("FileOpen", size);
        }

        public static BitmapImage FileSave(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("FileSave", size);
        }

        public static BitmapImage Information(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Information", size);
        }

        public static BitmapImage Message(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Message", size);
        }

        public static BitmapImage Monitor(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Monitor", size);
        }

        public static BitmapImage Paste(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Paste", size);
        }

        public static BitmapImage Refresh(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Refresh", size);
        }

        public static BitmapImage RenderingMessage(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("RenderingMessage", size);
        }

        public static BitmapImage ServerCheck(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerCheck", size);
        }

        public static BitmapImage ServerService(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerService", size);
        }

        public static BitmapImage ServerSmtp(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerSmtp", size);
        }

        public static BitmapImage ServerStatusOk(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerStatusOk", size);
        }

        public static BitmapImage ServerStatusAlarm(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerStatusAlarm", size);
        }

        public static BitmapImage ServerStorage(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("ServerStorage", size);
        }

        public static BitmapImage Settings(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Settings", size);
        }

        public static BitmapImage Start(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Start", size);
        }

        public static BitmapImage Stop(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Stop", size);
        }

        public static BitmapImage View(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("View", size);
        }

        public static BitmapImage Warning(StiImageSize size = StiImageSize.Normal)
        {
            return GetImage("Warning", size);
        }
    }

    public static BitmapImage GetImage(string path, StiImageSize size = StiImageSize.Normal)
    {
        using var image = StiScaledImagesHelper.GetImage(typeof(StiControllerImages).Assembly, "Principal.Server.Controller.Images.Items." + path, size);
        var memoryStream = new MemoryStream();
        image.Save(memoryStream, ImageFormat.Png);
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memoryStream;
        bitmapImage.EndInit();
        return bitmapImage;
    }
}
