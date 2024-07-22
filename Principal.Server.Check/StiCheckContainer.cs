using System.Collections.Generic;

namespace Principal.Server.Check
{
    public static class StiCheckContainer
    {
        private static List<StiCheck> checks;

        public static List<StiCheck> Checks
        {
            get
            {
                object obj = checks;
                if (obj == null)
                {
                    obj = new List<StiCheck>
                    {
                        new StiEndingDiskSpaceCheck(),
                        new StiLowDiskSpaceCheck(),
                        new StiNotEnoughDiskSpaceCheck(),
                        new StiIisIsNotInstalledCheck(),
                        new StiIisIsNotRunningCheck(),
                        new StiWindowsServiceIsNotRunningCheck(),
                        new StiWindowsServiceIsNotInstalledCheck(),

                        new StiPortAvailabilityCheck()
                    };
                    checks = (List<StiCheck>)obj;
                }
                return (List<StiCheck>)obj;
            }
        }
    }
}
