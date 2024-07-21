using Microsoft.Win32;

namespace Principal.Controls
{
    public static class StiOSVersion
    {
        private static readonly bool isWindows11;

        static StiOSVersion()
        {
            try
            {
                var registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
                var s = registryKey?.GetValue("CurrentBuild").ToString();
                if (int.TryParse(s, out var result) && result >= 22000)
                {
                    isWindows11 = true;
                }
            }
            catch
            {
            }
        }

        public static bool IsWindows11()
        {
            return isWindows11;
        }
    }
}
