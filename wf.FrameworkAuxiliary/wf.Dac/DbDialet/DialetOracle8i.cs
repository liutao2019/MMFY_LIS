using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbDialet
{
    internal class DialetOracle8i : IDialet
    {
        public override DateTime DateTimeMaxValue
        {
            get
            {
                //sql server 9999-12-31 23:59:59.997
                return new DateTime(9999, 12, 31, 23, 59, 59, 999);
            }
        }

        protected override string KeywordFormatString
        {
            get { return "\"{0}\""; }
        }
    }
}
