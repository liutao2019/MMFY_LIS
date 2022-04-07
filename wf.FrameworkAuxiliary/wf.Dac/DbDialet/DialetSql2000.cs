using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.Function;

namespace Lib.DAC.DbDialet
{
    internal class DialetSql2000 : IDialet
    {
        public DialetSql2000()
        {
            //RegisterFunction("abs", new StandardSqlFunction("abs"));
        }

        public override bool SupportIdentitySelect
        {
            get
            {
                return true;
            }
        }

        public override string IdentitySelectString
        {
            get
            {
                return "select scope_identity()";
            }
        }

        public override bool SupportSelectCurrentDate
        {
            get
            {
                return true;
            }
        }

        public override string CurrentDateSelectString
        {
            get
            {
                return "select getdate()";
            }

        }
        public override bool SupportMultiQueries
        {
            get
            {
                return true;
            }
        }

        protected override string KeywordFormatString
        {
            get { return "[{0}]"; }
        }
    }
}
