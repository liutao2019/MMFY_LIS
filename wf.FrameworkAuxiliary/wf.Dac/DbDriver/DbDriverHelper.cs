using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    public class DbDriverHelper
    {
        public static EnumDbDriver GetDriverTypeByName(string drivername)
        {
            EnumDbDriver driverType;
            switch (drivername.ToLower())
            {
                case "mssql":
                case "sql":
                    driverType = EnumDbDriver.MSSql;
                    break;

                case "oledb":
                    driverType = EnumDbDriver.Oledb;
                    break;

                case "odbc":
                    driverType = EnumDbDriver.ODBC;
                    break;

                case "oracle":
                    driverType = EnumDbDriver.Oracle;
                    break;

                default:
                    try
                    {
                        driverType = (EnumDbDriver)Enum.Parse(typeof(EnumDbDriver), drivername);
                    }
                    catch
                    {
                        driverType = EnumDbDriver.MSSql;
                    }
                    break;
            }
            return driverType;
        }

        /// <summary>
        /// 获取当前应用程序配置的驱动
        /// </summary>
        /// <returns></returns>
        public static IDbDriver GetCurrentDriver()
        {
            return DACConfig.Current.Driver;
        }

        /// <summary>
        /// 根据驱动类型获取数据库驱动
        /// </summary>
        /// <param name="driverType"></param>
        /// <returns></returns>
        public static IDbDriver GetDriver(EnumDbDriver driverType)
        {
            IDbDriver dbdriver = null;

            if (driverType == EnumDbDriver.MSSql)
            {
                dbdriver = new Lib.DAC.DbDriver.SqlClientDriver();
            }
            else if (driverType == EnumDbDriver.Oledb)
            {
                dbdriver = new Lib.DAC.DbDriver.OledbDriver();
            }
            else if (driverType == EnumDbDriver.ODBC)
            {
                dbdriver = new Lib.DAC.DbDriver.ODBCDriver();
            }
            else if (driverType == EnumDbDriver.Oracle)
            {
                dbdriver = new Lib.DAC.DbDriver.OracleDriver();
            }
            else
            {
                dbdriver = new Lib.DAC.DbDriver.SqlClientDriver();
            }
            return dbdriver;
        }
    }
}
