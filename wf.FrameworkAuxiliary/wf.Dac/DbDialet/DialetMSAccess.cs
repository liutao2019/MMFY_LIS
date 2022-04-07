using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbDialet
{
    internal class DialetMSAccess : IDialet
    {
        protected override string KeywordFormatString
        {
            get { return "{0}"; }
        }
    }
}
