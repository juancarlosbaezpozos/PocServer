using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Principal.Server.Objects
{
    public static class StiScaleUI
    {
        private static bool isInitialized;

        private static int lockCount;

        private static SystemScaleID id;

        private static SystemScaleIconID iconId;

        private static double x;

        private static double y;

        public static bool IsNoScaling => Id == SystemScaleID.x1;

        public static bool IsPrinting
        {
            set
            {
                if (lockCount <= 0)
                {
                    if (value)
                    {
                        isInitialized = true;
                        x = 1.0;
                        y = 1.0;
                    }
                    else
                    {
                        isInitialized = false;
                        Initialize();
                    }
                }
            }
        }

        public static SystemScaleID Id
        {
            get
            {
                Initialize();
                return id;
            }
        }

        public static SystemScaleIconID IconId
        {
            get
            {
                Initialize();
                return iconId;
            }
        }

        public static double X
        {
            get
            {
                Initialize();
                return x;
            }
        }

        public static double Y
        {
            get
            {
                Initialize();
                return y;
            }
        }

        public static int I1 => (int)Math.Ceiling(Factor);

        public static int I2 => (int)Math.Ceiling(Factor * 2.0);

        public static int I3 => (int)Math.Ceiling(Factor * 3.0);

        public static int I4 => (int)Math.Ceiling(Factor * 4.0);

        public static int I5 => (int)Math.Ceiling(Factor * 5.0);

        public static int I6 => (int)Math.Ceiling(Factor * 6.0);

        public static int I7 => (int)Math.Ceiling(Factor * 7.0);

        public static int I8 => (int)Math.Ceiling(Factor * 8.0);

        public static int I9 => (int)Math.Ceiling(Factor * 9.0);

        public static int I10 => (int)Math.Ceiling(Factor * 10.0);

        public static int I11 => (int)Math.Ceiling(Factor * 11.0);

        public static int I12 => (int)Math.Ceiling(Factor * 12.0);

        public static int I13 => (int)Math.Ceiling(Factor * 13.0);

        public static int I14 => (int)Math.Ceiling(Factor * 14.0);

        public static int I15 => (int)Math.Ceiling(Factor * 15.0);

        public static int I16 => (int)Math.Ceiling(Factor * 16.0);

        public static int I17 => (int)Math.Ceiling(Factor * 17.0);

        public static int I18 => (int)Math.Ceiling(Factor * 18.0);

        public static int I19 => (int)Math.Ceiling(Factor * 19.0);

        public static int I20 => (int)Math.Ceiling(Factor * 20.0);

        public static double Factor => X;

        public static double System => 1.0 / X;

        public static double Step
        {
            get
            {
                if (Factor < 1.5)
                {
                    return 1.0;
                }
                if (Factor < 2.0)
                {
                    return 1.5;
                }
                return 2.0;
            }
        }

        public static string StepName => GetStepName(Step);

        public static string BigStepName => GetStepName(Step * 2.0);

        public static string ToIconName(SystemScaleIconID id)
        {
            return id switch
            {
                SystemScaleIconID.x100 => string.Empty,
                SystemScaleIconID.x125 => "_x1_25",
                SystemScaleIconID.x150 => "_x1_5",
                SystemScaleIconID.x175 => "_x1_75",
                SystemScaleIconID.x200 => "_x2",
                SystemScaleIconID.x225 => "_x2_25",
                SystemScaleIconID.x250 => "_x2_5",
                SystemScaleIconID.x275 => "_x2_75",
                SystemScaleIconID.x300 => "_x3",
                SystemScaleIconID.x325 => "_x3_25",
                SystemScaleIconID.x350 => "_x3_5",
                SystemScaleIconID.x375 => "_x3_75",
                SystemScaleIconID.x400 => "_x4",
                _ => string.Empty,
            };
        }

        public static int I(int value)
        {
            return XXI(value);
        }

        public static Rectangle I(Rectangle rect)
        {
            return new Rectangle(XXI(rect.X), YYI(rect.Y), XXI(rect.Width), YYI(rect.Height));
        }

        public static Point I(Point point)
        {
            return new Point(XXI(point.X), YYI(point.Y));
        }

        public static Size I(Size size)
        {
            return SizeI(size.Width, size.Height);
        }

        public static SizeF I(SizeF size)
        {
            return new SizeF((float)XX(size.Width), (float)YY(size.Height));
        }

        public static Size SizeI(int width, int height)
        {
            return new Size(XXI(width), YYI(height));
        }

        public static Point PointI(int left, int top)
        {
            return new Point(XXI(left), YYI(top));
        }

        public static double XX(double value)
        {
            return X * value;
        }

        public static double YY(double value)
        {
            return Y * value;
        }

        public static int XXI(double value)
        {
            return (int)Math.Ceiling(X * value);
        }

        public static int YYI(double value)
        {
            return (int)Math.Ceiling(Y * value);
        }

        private static string GetStepName(double scale)
        {
            if (scale == 1.5)
            {
                return "_x1_5";
            }
            if (scale == 2.0)
            {
                return "_x2";
            }
            if (scale == 3.0)
            {
                return "_x3";
            }
            if (scale == 4.0)
            {
                return "_x4";
            }
            return "";
        }

        internal static void Lock()
        {
            lockCount++;
            x = 1.0;
            y = 1.0;
            id = SystemScaleID.x1;
            isInitialized = true;
        }

        internal static void Unlock()
        {
            lockCount--;
            if (lockCount == 0)
            {
                isInitialized = false;
                Initialize();
            }
        }

        private static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }
            var num = 96;
            var num2 = 96;
            try
            {
                var dC = GetDC(IntPtr.Zero);
                if (dC != IntPtr.Zero)
                {
                    num = GetDeviceCaps(dC, 88);
                    num2 = GetDeviceCaps(dC, 90);
                    ReleaseDC(IntPtr.Zero, dC);
                }
            }
            catch
            {
            }
            x = (double)num / 96.0;
            y = (double)num2 / 96.0;
            var num3 = Math.Max(num, num2);
            if (num3 <= 96)
            {
                id = SystemScaleID.x1;
                iconId = SystemScaleIconID.x100;
            }
            else if (num3 <= 192)
            {
                id = SystemScaleID.x2;
                if (num3 <= 120)
                {
                    iconId = SystemScaleIconID.x125;
                }
                else if (num3 <= 144)
                {
                    iconId = SystemScaleIconID.x150;
                }
                else if (num3 <= 168)
                {
                    iconId = SystemScaleIconID.x175;
                }
                else
                {
                    iconId = SystemScaleIconID.x200;
                }
            }
            else if (num3 <= 288)
            {
                id = SystemScaleID.x3;
                if (num3 <= 216)
                {
                    iconId = SystemScaleIconID.x225;
                }
                else if (num3 <= 240)
                {
                    iconId = SystemScaleIconID.x250;
                }
                else if (num3 <= 264)
                {
                    iconId = SystemScaleIconID.x275;
                }
                else
                {
                    iconId = SystemScaleIconID.x300;
                }
            }
            else
            {
                id = SystemScaleID.x4;
                if (num3 <= 312)
                {
                    iconId = SystemScaleIconID.x325;
                }
                else if (num3 <= 336)
                {
                    iconId = SystemScaleIconID.x350;
                }
                else if (num3 <= 360)
                {
                    iconId = SystemScaleIconID.x375;
                }
                else
                {
                    iconId = SystemScaleIconID.x400;
                }
            }
            isInitialized = true;
        }

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
