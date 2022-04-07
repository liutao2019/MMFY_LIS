using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.ConnectionStringProvider
{
    internal class OracleConnStringBuilder : ConnectionStringBuilder
    {
        public OracleConnStringBuilder(EnumDataBaseDialet dbType)
            : base(EnumDbDriver.Oracle, dbType)
        {

        }

        public override string Build()
        {
            string result = string.Empty;
            switch (this.DbType)
            {
                case EnumDataBaseDialet.Oracle10g:
                case EnumDataBaseDialet.Oracle11g:
                case EnumDataBaseDialet.Oracle8i:
                case EnumDataBaseDialet.Oracle9i:
                    result = string.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));Persist Security Info=True;User ID={2};Password={3};", this.Server, this.DbName, this.LoginName, this.LoginPassword);
                    break;

                default:
                    result = string.Format(@"Data Source={0};user={1};password={2}", this.Server, this.LoginName, this.LoginPassword);
                    break;
            }
            return result;
        }
    }
}
