using System;
using Microsoft.Win32;

namespace Principal.Server.Check
{
    public class StiIisIsNotInstalledCheck : StiServerCheck
    {
        public override string ShortMessage => "Internet Information Services no está instalado.";

        public override string LongMessage => "Internet Information Services no está instalado en este sistema.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsServiceNotInstalled()
        {
            try
            {
                var registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\InetStp");
                if (registryKey != null)
                {
                    var value = registryKey.GetValue("MajorVersion");
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (IsServiceNotInstalled())
            {
                return new StiIisIsNotInstalledCheck
                {
                    Element = obj
                };
            }
            return null;
        }
    }
}
