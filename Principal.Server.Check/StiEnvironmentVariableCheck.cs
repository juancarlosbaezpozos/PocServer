using System;

namespace Principal.Server.Check
{
    public class StiEnvironmentVariableCheck : StiServerCheck
    {
        public override string ShortMessage => "Variables de entorno.";

        public override string LongMessage => "Las variables de entorno necesarias no están configuradas o tienen valores incorrectos. Asegúrese de que las variables de entorno \"AWS_ACCESS_KEY_ID\" y \"AWS_SECRET_ACCESS_KEY\" están configuradas correctamente.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool AreEnvironmentVariablesSetCorrectly()
        {
            try
            {
                var var1 = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var var2 = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

                if (string.IsNullOrEmpty(var1) || string.IsNullOrEmpty(var2))
                {
                    return false;
                }

                return var1.Length >= 5 && var2.Length >= 5;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (!AreEnvironmentVariablesSetCorrectly())
            {
                return new StiEnvironmentVariableCheck
                {
                    Element = obj,
                    Actions = { new StiRunEnvVarEditorAction() }
                };
            }
            return null;
        }
    }
}
