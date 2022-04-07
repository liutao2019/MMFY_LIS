using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Lib.DAC.DbDriver
{
    internal class SqlClientDriver : DriverBase, IDbDriver
    {
        public override System.Data.IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        #region IDbDriver 成员


        public override System.Data.IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new System.Data.SqlClient.SqlDataAdapter();
        }

        #endregion

        /// <summary>
        /// MsSql requires the use of a Named Prefix in the SQL statement.  
        /// </summary>
        /// <remarks>
        /// <see langword="true" /> because MsSql uses "<c>@</c>".
        /// </remarks>
        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        /// <summary>
        /// MsSql requires the use of a Named Prefix in the Parameter.  
        /// </summary>
        /// <remarks>
        /// <see langword="true" /> because MsSql uses "<c>@</c>".
        /// </remarks>
        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        /// <summary>
        /// sql参数前缀 
        /// </summary>
        /// <value>
        /// </value>
        public override string NamedPrefix
        {
            get { return "@"; }
        }

        public override bool SupportsMultipleQueries
        {
            get
            {
                return true;
            }
        }
    }
}
