namespace Principal.Server.Check
{
    public abstract class StiServerCheck : StiCheck
    {
        public override string ElementName => base.Element?.ToString();
    }
}
