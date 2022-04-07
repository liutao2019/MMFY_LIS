using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class EntityDeleteWithFlagAttribute : IEntityAttribute
    {
    }
}