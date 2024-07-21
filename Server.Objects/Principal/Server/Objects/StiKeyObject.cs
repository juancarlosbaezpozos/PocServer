using System;

namespace Principal.Server.Objects
{
    public abstract class StiKeyObject : StiObject
    {
        public string Key { get; set; } = Guid.NewGuid().ToString().Replace("-", string.Empty);

        protected internal bool? IsStored { get; set; }
    }
}
