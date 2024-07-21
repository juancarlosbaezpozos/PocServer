using System;
using System.IO;
using System.Linq;

namespace Principal.Server.Check
{
    public class StiNotEnoughDiskSpaceCheck : StiServerCheck
    {
        public override string ShortMessage => "El espacio en disco en la unidad del sistema no es suficiente.";

        public override string LongMessage => "El espacio en disco en la unidad del sistema en la que instaló Principal Server no es suficiente (menos de 100 MB). Necesitas al menos 1 GB de espacio libre para funcionar correctamente.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsDiskFreeSpaceLowerToLimit()
        {
            try
            {
                var driveLetter = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                if ((from driveInfo in DriveInfo.GetDrives()
                     where driveInfo.Name.Contains(driveLetter)
                     select driveInfo).Any((DriveInfo driveInfo) => driveInfo.AvailableFreeSpace < 104857600))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (IsDiskFreeSpaceLowerToLimit())
            {
                return new StiNotEnoughDiskSpaceCheck
                {
                    Element = obj,
                    Actions = { new StiRunDiskCleanerAction() }
                };
            }
            return null;
        }
    }
}
