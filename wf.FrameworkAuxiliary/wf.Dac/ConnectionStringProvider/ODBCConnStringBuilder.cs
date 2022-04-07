using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.ConnectionStringProvider
{
    internal class ODBCConnStringBuilder : ConnectionStringBuilder
    {

        public ODBCConnStringBuilder(EnumDataBaseDialet dbType)
            : base(EnumDbDriver.ODBC, dbType)
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
                    result = string.Format(@"DSN={0};UID={1};Pwd={2}", this.DbName, this.LoginName, this.LoginPassword);
                    //result = string.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));Persist Security Info=True;User ID={2};Password={3};",this.Server, this.DbName, this.LoginName, this.LoginPassword);
                    break;
                case EnumDataBaseDialet.SQL2000:
                case EnumDataBaseDialet.SQL2005:
                case EnumDataBaseDialet.SQL2008:
                    result = string.Format(@"DSN={0};UID={1};Pwd={2}", this.DbName, this.LoginName, this.LoginPassword);
                    break;
                default:
                    result = string.Format(@"DSN={0};UID={1};Pwd={2}", this.DbName, this.LoginName, this.LoginPassword);
                    break;
            }
            return result;
        }
    }
}
