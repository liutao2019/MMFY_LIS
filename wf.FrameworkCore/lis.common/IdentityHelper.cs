using Lib.DAC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.common
{
    public static class IdentityHelper
    {
        public static int GetMedIdentity(string tableName, string defaultId = "100000", int step = 1)
        {
            SqlHelper sql = new SqlHelper();
            tableName = tableName.ToUpper().Trim();
            string updateSql = @"update Sys_tableid set tab_current_id= tab_current_id + {1} where tab_id = '{0}';
                                   select tab_current_id from Sys_tableid where tab_id = '{0}'";
            object obj2 = sql.ExecuteScalar(string.Format(updateSql, tableName, step));

            if (obj2 == null)
            {
                sql.ExecuteNonQuery(GetInsertSqlMed(tableName, defaultId));
                return Convert.ToInt32(defaultId);
            }
            return int.Parse(obj2.ToString());
        }

        private static string GetInsertSqlClab(string tableName, string defaultId)
        {
            string insertSql = @"insert into SysTableID(tablename,curID) values('{0}','{1}');";
            insertSql = string.Format(insertSql, tableName, defaultId);
            return insertSql;
        }

        private static string GetInsertSqlMed(string tableName, string defaultId)
        {
            string insertSql = @"insert into Sys_tableid(tab_id,tab_current_id) values('{0}','{1}');";
            insertSql = string.Format(insertSql, tableName, defaultId);
            return insertSql;
        }

    }
}
