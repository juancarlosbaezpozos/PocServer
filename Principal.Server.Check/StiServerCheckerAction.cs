namespace Principal.Server.Check
{
    public abstract class StiServerCheckerAction
    {
        public delegate void DoAfterAction(StiServerCheckerAction sender);

        public abstract string Name { get; }

        public abstract string Description { get; }

        public virtual bool RemoveCheckAfterInvokeAction => true;

        public event DoAfterAction OnAfterAction;

        public virtual void Invoke(object element, string elementName)
        {
            this.OnAfterAction?.Invoke(this);
        }
    }

}
