using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [Serializable]
    public class PropertyValueLogEntry
    {
        public object Value { get; set; }
        public DateTime LogTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Value, LogTime);
        }
    }
}
