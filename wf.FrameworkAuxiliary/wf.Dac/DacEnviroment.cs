using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    [Obsolete("替换为DACHelper")]
    public class DacEnviroment
    {
        private DacEnviroment()
        {

        }

        public static string ConnectionString
        {
            get
            {
                return DACConfig.Current.ConnectionString;
            }
        }

        public static EnumDataBaseDialet DialetType
        {
            get
            {
                return DACConfig.Current.DataBaseDialet;
            }
        }

        public static EnumDbDriver DriverType
        {
            get
            {
                return DACConfig.Current.DriverType;
            }
        }

        public static IDbConnection GetConnection()
        {
            return DbDriverHelper.GetCurrentDriver().CreateConnection(ConnectionString);
        }

        public static ITransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public static ITransaction BeginTransaction(IsolationLevel il)
        {
            IDbConnection conn = GetConnection();
            conn.Open();
            IDbTransaction transaction = conn.BeginTransaction(il);

            AdoTransaction tran = new AdoTransaction(transaction, DbDriverHelper.GetCurrentDriver(), DbDialetHelper.GetCurrentDialet());

            return tran;
        }
    }
}
