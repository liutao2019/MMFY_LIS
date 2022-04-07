using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbDialet
{
    internal class DialetDB2 : IDialet
    {
        protected override string KeywordFormatString
        {
            get { return "{0}"; }
        }
    }
}
