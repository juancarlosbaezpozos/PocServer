using System.IO;

namespace Principal.Controls
{
    public sealed class StiScaleImage
    {
        public string Name { get; }

        public int Width { get; }

        public int Height { get; }

        internal Stream Stream { get; set; }

        internal object WpfImage { get; set; }

        public StiScaleImage(string name, int width, int hidth, Stream stream)
        {
            Name = name;
            Width = width;
            Height = hidth;
            Stream = stream;
        }
    }
}
