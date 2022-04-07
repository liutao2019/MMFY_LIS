using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;


namespace Lib.DAC.DbDriver
{
    internal class OracleDriver : DriverBase, IDbDriver
    {
        public override System.Data.IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        public override System.Data.IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        public override string NamedPrefix
        {
            get { return ":"; }
        }
    }
}
