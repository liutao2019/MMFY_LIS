using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbDriver
{
    /// <summary>
    /// OLEDB数据驱动
    /// </summary>
    internal class OledbDriver : DriverBase, IDbDriver
    {
        #region IDbDriver 成员

        public override System.Data.IDbConnection CreateConnection()
        {
            return new System.Data.OleDb.OleDbConnection();
        }

        public override System.Data.IDbCommand CreateCommand()
        {
            return new System.Data.OleDb.OleDbCommand();
        }

        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new System.Data.OleDb.OleDbDataAdapter();
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
