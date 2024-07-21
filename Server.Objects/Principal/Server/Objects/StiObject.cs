using System;

namespace Principal.Server.Objects
{
    public abstract class StiObject : ICloneable
    {
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
