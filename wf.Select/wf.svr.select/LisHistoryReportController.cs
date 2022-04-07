using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;

namespace dcl.svr.resultquery
{
    public class LisHistoryReportController
    {
        public static DataTable GetPatList(DateTime? start, DateTime? end, string sql)
        {
            string LisHistoryDateStr = System.Configuration.ConfigurationManager.AppSettings["LisHistoryDate"];
            string LisHistoryConnectionString = System.Configuration.ConfigurationManager.AppSettings["LisHistoryConnectionString"];
            //查询历史库数据
            if (start.HasValue
                && end.HasValue
                && !string.IsNullOrEmpty(LisHistoryConnectionString)
                && !string.IsNullOrEmpty(LisHistoryDateStr)
                )
            {
                try
                {
                    DateTime LisHistoryDate = DateTime.Parse(LisHistoryDateStr);
                    if (LisHistoryDate > start.Value)
                    {
                        SqlHelper sqlHelper = new SqlHelper(LisHistoryConnectionString, EnumDbDriver.MSSql);
                        DataTable result = sqlHelper.GetTable(sql);
                        result.TableName = "LisHistory";
                        return result;
                    }
                }
                catch (Exception ex)
                {

                    Lib.LogManager.Logger.LogException(ex);
                }
   

            }
            return null;
        }
        public static SqlHelper GetSqlHelper(DateTime? start)
        {
            SqlHelper sqlHelper;
            bool enableRead =
               dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
             string lisHistoryDateStr = System.Configuration.ConfigurationManager.AppSettings["LisHistoryDate"];
             string lisHistoryConnectionString = System.Configuration.ConfigurationManager.AppSettings["LisHistoryConnectionString"];
             //查询历史库数据
             if (start.HasValue && enableRead
                 && !string.IsNullOrEmpty(lisHistoryConnectionString)
                 && !string.IsNullOrEmpty(lisHistoryDateStr))
             {
                 try
                 {
                     DateTime lisHistoryDate = DateTime.Parse(lisHistoryDateStr);
                     if (lisHistoryDate > start.Value)
                     {
                         sqlHelper = new SqlHelper(lisHistoryConnectionString, EnumDbDriver.MSSql);
                         return  sqlHelper;
                     }
                 }
                 catch (Exception ex)
                 {
                     Lib.LogManager.Logger.LogException(ex);
                 }
             }
             return new SqlHelper();
         }

    }
}
