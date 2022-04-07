using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.ConnectionStringProvider
{
    internal class MSSqlConnStringBuider : ConnectionStringBuilder
    {

        public MSSqlConnStringBuider(EnumDataBaseDialet dbType)
            : base(EnumDbDriver.MSSql, dbType)
        {

        }
        string connStr = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        public override string Build()
        {
            string result=string.Empty;
            switch (this.DbType)
            {
                case EnumDataBaseDialet.SQL2000:
                case EnumDataBaseDialet.SQL2005:
                case EnumDataBaseDialet.SQL2008:
                    result = string.Format(connStr, this.Server, this.DbName, this.LoginName, this.LoginPassword);
                    break;

                default:
                    result = string.Format(connStr, this.Server, this.DbName, this.LoginName, this.LoginPassword);
                    break;
            }
            return result;
        }
    }
}
