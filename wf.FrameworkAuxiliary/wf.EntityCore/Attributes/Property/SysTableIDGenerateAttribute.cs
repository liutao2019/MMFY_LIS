using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class SysTableIDGenerateAttribute : IEntityPropertyAttribute
    {
        public string Rule { get; private set; }

        public SysTableIDGenerateAttribute()
            : this(null)
        { }

        public SysTableIDGenerateAttribute(string rule)
        {
            this.Rule = rule;
        }
    }
}
