using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.Function
{
    class StandardSqlFunction : ISqlFunction
    {
        string _funcname;
        public StandardSqlFunction(string funcname)
        {
            this._funcname = funcname;
        }



        #region ISqlFunction 成员

        public string Build(List<object> args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this._funcname);
            sb.Append("(");

            for (int i = 0; i < args.Count; i++)
            {
                object arg = args[i];
                sb.Append(arg.ToString());

                if (i < (args.Count - 1))
                    sb.Append(Lib.DAC.SqlStringConst.CommaSpace);
            }
            sb.Append(")");

            return sb.ToString();
        }

        #endregion
    }
}
