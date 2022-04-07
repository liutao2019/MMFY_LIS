using System;
using System.Data;
using System.Data.SqlClient;
using Lib.DAC;
using dcl.root.logon;
using System.Configuration;
using dcl.root.dac;
using lis.dto;

namespace dcl.svr.result.CRUD
{
    public class LisHistoryResultController
    {
        public static SqlHelper GetSqlHelper(DateTime? start)
        {
            bool enableRead =
                dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
            string lisHistoryDateStr = ConfigurationManager.AppSettings["LisHistoryDate"];
            string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
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
                         SqlHelper sqlHelper = new SqlHelper(lisHistoryConnectionString, EnumDbDriver.MSSql);
                         return  sqlHelper;
                     }
                 }
                 catch (Exception ex)
                 {
                     Logger.WriteException("dcl.svr.result.CRUD.LisHistoryResultController", "GetSqlHelper", ex.ToString());
                 }
             }
             return new SqlHelper();
         }


        /// <summary>
        /// 获取病人基本信息(可能存在历史数据库)
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patDate"></param>
        /// <returns></returns>
        internal static DataTable GetPatientInfoForHistory(string patID, DateTime? patDate)
        {
            try
            {
                string sqlSelect =
                string.Format(@" SELECT top 1 patients.*, dict_instrmt.itr_ptype,  dict_instrmt.itr_mid, dict_instrmt.itr_name,
                                       dict_sample.sam_name,dict_checkb.chk_cname,dict_doctor.doc_name, PowerUserInfo.userName AS pat_i_code_name,
                                       dict_no_type.no_name, dict_origin.ori_name,pat_ctype_name = '',bc_status = '',bc_print_flag = 0,
                                       pat_sex_name = case when pat_sex = '1' then '男' when pat_sex = '2' then '女' else '' end
                                       FROM patients with(nolock)
                                       LEFT OUTER JOIN dict_origin ON patients.pat_ori_id = dict_origin.ori_id 
                                       LEFT OUTER JOIN dict_no_type ON patients.pat_no_id = dict_no_type.no_id
                                       LEFT OUTER JOIN PowerUserInfo ON patients.pat_i_code = PowerUserInfo.loginId
                                       LEFT OUTER JOIN dict_doctor ON patients.pat_doc_id = dict_doctor.doc_id
                                       LEFT OUTER JOIN dict_checkb ON patients.pat_chk_id = dict_checkb.chk_id
                                       LEFT OUTER JOIN dict_sample ON patients.pat_sam_id = dict_sample.sam_id
                                      LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    
                                       WHERE patients.pat_id ='{0}'  ", patID);

                DataTable dtPat = GetSqlHelper(patDate).GetTable(sqlSelect);


                dtPat.TableName = PatientTable.PatientInfoTableName;

                return dtPat;
            }
            catch (Exception ex)
            {
                Logger.WriteException("dcl.svr.result.CRUD.LisHistoryResultController", "GetPatientInfoForHistory获取病人信息出错,病人ID:" + patID, ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// 读取历史数据库检验结果数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dtPatDate"></param>
        internal static DataTable GetPatHistoryResultFromHistoryDataBase(string sql, DateTime dtPatDate)
        {
            bool enableRead =
               dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
            string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
            if (!string.IsNullOrEmpty(lisHistoryConnectionString) && enableRead)
            {
                try
                {
                    DBHelper helper = new DBHelper(lisHistoryConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("pat_date", dtPatDate);
                    DataTable dt = helper.GetTable(cmd);
                    dt.TableName = "PatientHistory";
                    return dt;
                }
                catch (Exception ex)
                {
                    Logger.WriteException("dcl.svr.result.CRUD.LisHistoryResultController", "读取历史数据库数据出错GetPatHistoryResultFromHistoryDataBase", ex.ToString());
                }
            
            }    
            return null;
        }

        internal static DataTable GetPatHistoryResultData(string sql, string tableName)
        {
            bool enableRead =
               dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
            string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
            if (!string.IsNullOrEmpty(lisHistoryConnectionString)&&enableRead)
            {
                try
                {
                    DBHelper helper = new DBHelper(lisHistoryConnectionString);
                    DataTable dt = helper.GetTable(sql);
                    dt.TableName = tableName;
                    return dt;
                }
                catch (Exception ex)
                {
                    Logger.WriteException("dcl.svr.result.CRUD.LisHistoryResultController", "读取历史数据库数据出错GetPatHistoryResultData", ex.ToString());
                }

            }
            return null;
        }

    }
}
