using System.Collections.Generic;

namespace Principal.Server.Check
{
    public class StiCheckEngine
    {
        public double ProgressValue { get; private set; }

        public double ProgressMaximum { get; private set; }

        public void Check(BuildCheckDelegate buildCheck)
        {
            ProgressMaximum = 1.0;
            ProgressValue = 0.0;

            foreach (var check2 in StiCheckContainer.Checks)
            {
                if (!check2.Enabled)
                {
                    continue;
                }

                var obj = check2.ProcessCheck(check2);
                if (obj is StiCheck check)
                {
                    buildCheck(check);
                }

                if (obj is List<StiCheck> list)
                {
                    list.ForEach(delegate (StiCheck c)
                    {
                        buildCheck(c);
                    });
                    break;
                }
            }

            ProgressValue = 1.0;
        }
    }
}
