using System;

namespace Principal.Controls
{
    public sealed class StiScaleFolderDetailsAttribute : Attribute
    {
        public int Size { get; }

        public bool SkipCategory { get; }

        public StiScaleFolderDetailsAttribute(int size)
        {
            Size = size;
        }

        public StiScaleFolderDetailsAttribute(bool skipCategory)
        {
            SkipCategory = skipCategory;
        }
    }
}
