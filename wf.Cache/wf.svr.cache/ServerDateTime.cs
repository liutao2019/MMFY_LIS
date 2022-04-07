using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;
using System.Data;

namespace dcl.svr.cache
{
    public class ServerDateTime
    {

        /// <summary>
        /// 数据库服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDatabaseServerDateTime()
        {
            DateTime dtNow = DateTime.Now;
            try
            {
                string strSQL = @"SELECT GETDATE() ";
                SqlHelper helper = new SqlHelper();
                DataTable table = helper.GetTable(strSQL);
                dtNow =Convert.ToDateTime(Convert.ToDateTime(table.Rows[0][0]).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
            }

            return dtNow;
        }


    }
}
