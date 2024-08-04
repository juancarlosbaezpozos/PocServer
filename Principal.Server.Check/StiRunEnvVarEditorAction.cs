using System.Diagnostics;

namespace Principal.Server.Check
{
    public class StiRunEnvVarEditorAction : StiServerCheckerAction
    {
        public override string Name => "Editar Variables de Entorno";

        public override string Description => "Abra el editor de variables de entorno.";

        public override bool RemoveCheckAfterInvokeAction => true;

        public override void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            Process.Start("rundll32.exe", "sysdm.cpl,EditEnvironmentVariables");
        }
    }
}
