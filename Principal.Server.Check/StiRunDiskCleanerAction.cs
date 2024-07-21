using System;
using System.Diagnostics;
using System.IO;

namespace Principal.Server.Check
{
    public class StiRunDiskCleanerAction : StiServerCheckerAction
    {
        public override string Name => "Limpiar";

        public override string Description => "Ejecute la limpieza del disco.";

        public override void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            var arg = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)).TrimEnd('\\');
            Process.Start("cleanmgr", $"/d {arg}");
        }
    }
}
