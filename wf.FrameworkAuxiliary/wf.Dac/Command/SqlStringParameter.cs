using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Lib.DAC
{
    /// <summary>
    /// SqlString参数
    /// </summary>
    [Serializable]
    public class SqlStringParameter : ISqlStringPart
    {
        public static SqlStringParameter Placeholder
        {
            get { return new SqlStringParameter(); }
        }

        private SqlStringParameter()
        {
            this.ParamIndex = -1;
        }

        public object Value { get; set; }
        public DbType DataType { get; set; }
        public int ParamIndex { get; internal set; }

        public override string ToString()
        {
            return SqlStringConst.SqlParameter;
        }
    }
}
