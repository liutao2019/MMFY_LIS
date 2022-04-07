using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbDriver
{
    /// <summary>
    /// ODBC数据驱动
    /// </summary>
    internal class ODBCDriver : DriverBase, IDbDriver
    {
        #region IDbDriver 成员

        public override System.Data.IDbConnection CreateConnection()
        {
            return new System.Data.Odbc.OdbcConnection();
        }

        public override System.Data.IDbCommand CreateCommand()
        {
            return new System.Data.Odbc.OdbcCommand();
        }

        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new System.Data.Odbc.OdbcDataAdapter();
        }

        #endregion

        public override bool UseNamedPrefixInSql
        {
            get { return false; }
        }

        public override bool UseNamedPrefixInParameter
        {
            get { return false; }
        }

        public override string NamedPrefix
        {
            get { return String.Empty; }
        }
    }
}
