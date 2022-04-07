using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.ConnectionStringProvider
{
    public class ConnectionStrBuilderHelper
    {
        private ConnectionStrBuilderHelper()
        {

        }

        public static ConnectionStringBuilder CreateConnStrBuilder(EnumDbDriver driverType, EnumDataBaseDialet dbType)
        {
            ConnectionStringBuilder result = null;

            switch (driverType)
            {
                case EnumDbDriver.MSSql:
                    result = new MSSqlConnStringBuider(dbType);
                    break;
                case EnumDbDriver.Oledb:
                    result = new OledbConnStringBuilder(dbType);
                    break;
                case EnumDbDriver.ODBC:
                    result = new ODBCConnStringBuilder(dbType);
                    break;
                case EnumDbDriver.Oracle:
                    result = new OracleConnStringBuilder(dbType);
                    break;
                default:
                    throw new NotSupportedException(driverType.ToString());
                //break;
            }
            return result;
        }

        public static ConnectionStringBuilder CreateConnStrBuilder(string driverType, string dbType)
        {
            EnumDbDriver driverT = DbDriverHelper.GetDriverTypeByName(driverType);
            EnumDataBaseDialet dbT = DbDialetHelper.GetDialetTypeByName(dbType);

            return CreateConnStrBuilder(driverT, dbT);
        }
    }
}
