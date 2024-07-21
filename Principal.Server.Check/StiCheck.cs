using System;
using System.Collections.Generic;

namespace Principal.Server.Check
{
    public abstract class StiCheck : ICloneable
    {
        private List<StiServerCheckerAction> actions;

        public object Element { get; set; }

        public virtual bool PreviewVisible => false;

        public abstract string ElementName { get; }

        public abstract string ShortMessage { get; }

        public abstract string LongMessage { get; }

        public abstract StiCheckServerStatus ServerStatus { get; }

        public virtual bool DefaultStateEnabled => true;

        public bool Enabled { get; set; }

        public List<StiServerCheckerAction> Actions => actions ?? (actions = new List<StiServerCheckerAction>());

        public object Clone()
        {
            return MemberwiseClone() as StiCheck;
        }

        public abstract object ProcessCheck(object obj);

        public StiCheck()
        {
            Element = null;
            Enabled = DefaultStateEnabled;
        }
    }
}
