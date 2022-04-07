using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.ConnectionStringProvider
{
    internal class OledbConnStringBuilder : ConnectionStringBuilder
    {

        public OledbConnStringBuilder(EnumDataBaseDialet dbType)
            : base(EnumDbDriver.Oledb, dbType)
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
                    result = string.Format(@"Provider=msdaora;Data Source={0};User Id={1};Password={2};", this.DbName, this.LoginName, this.LoginPassword);
                    break;
                case EnumDataBaseDialet.SQL2000:
                case EnumDataBaseDialet.SQL2005:
                case EnumDataBaseDialet.SQL2008:
                    result = string.Format(@"Provider=SQLOLEDB;Data Source={0};Initial Catalog={1};User ID={2};Password={3}", this.Server, this.DbName, this.LoginName, this.LoginPassword);
                    break;

                case EnumDataBaseDialet.Excel:
                    result = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", System.IO.Path.GetFullPath(this.Server));
                    //Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;Extended Properties='Excel 8.0;HDR=YES;IMEX=1'
                    break;

                case EnumDataBaseDialet.Access:
                    result = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", System.IO.Path.GetFullPath(this.Server));
                    break;

                case EnumDataBaseDialet.Access2007:
                    throw new NotSupportedException();
                    break;

                default:
                    break;
            }
            return result;
        }
    }
}
