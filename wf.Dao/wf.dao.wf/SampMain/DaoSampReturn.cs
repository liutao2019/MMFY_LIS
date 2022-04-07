using dcl.dao.core;
using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampReturn))]
    public class DaoSampReturn : IDaoSampReturn
    {
        /// <summary>
        /// 处理回退条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean HandleSampReturn(String sampBarId)
        {
            bool result = true;
            try
            {
                DBManager helper = new DBManager();

                string strUpdateSql = "update Sample_return set Sret_proc_flag=1 where Sret_Smain_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                helper.ExecCommand(strUpdateSql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 保存回退信息
        /// </summary>
        /// <param name="sampReturn"></param>
        /// <returns></returns>
        public Boolean SaveSampReturn(EntitySampReturn sampReturn)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Sret_Smain_bar_id", sampReturn.SampBarId);
                values.Add("Sret_Smain_bar_code", sampReturn.SampBarCode);
                values.Add("Sret_return_reasons", sampReturn.ReturnReasons);
                values.Add("Sret_dept_code", sampReturn.ReturnDeptCode);
                values.Add("Sret_dept_name", sampReturn.ReturnDeptName);
                values.Add("Sret_user_id", sampReturn.ReturnUserId);
                values.Add("Sret_user_name", sampReturn.ReturnUserName);
                values.Add("Sret_read_flag", sampReturn.ReturnReadFlag);
                values.Add("Sret_date", sampReturn.ReturnDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Sret_Smain_id", sampReturn.ReturnSampSn);
                values.Add("Sret_proc_flag", sampReturn.ReturnProcFlag);
                values.Add("del_flag", sampReturn.DelFlag);
                values.Add("Sret_pat_Dsorc_id", sampReturn.PidSrcId);
                values.Add("Sret_receiver", sampReturn.ReturnReceiver);
                values.Add("return_message_type", sampReturn.MessageType);
                helper.InsertOperation("Sample_return", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 查询回退信息
        /// </summary>
        /// <param name="sampQc"></param>
        /// <returns></returns>
        public List<EntitySampReturn> GetSampReturn(EntitySampQC sampQc)
        {
            List<EntitySampReturn> listReturn = new List<EntitySampReturn>();
            try
            {
                string strSql = @"select 
Sample_return.*, 
Sample_main.Sma_pat_name ,
Sample_main.Sma_com_name,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_bed_no
from Sample_return
left join Sample_main on Sample_main.Sma_bar_id = Sample_return.Sret_Smain_bar_id
where Sample_return.del_flag = 0 and Sample_main.del_flag=0 ";

                string strWhere = string.Empty;

                if (!string.IsNullOrEmpty(sampQc.StartDate) && !string.IsNullOrEmpty(sampQc.EndDate))
                    strWhere = string.Format(" and Sample_return.Sret_date>='{0}' and Sample_return.Sret_date<'{1}' ",
                        sampQc.StartDate, sampQc.EndDate);

                if (sampQc.HandleProc == ReturnProc.已处理)
                    strWhere += " and Sample_return.Sret_proc_flag=1 ";

                if (sampQc.HandleProc == ReturnProc.未处理)
                    strWhere += " and Sample_return.Sret_proc_flag=0 ";

                //if (sampQc.SearchHospital)
                //{
                //    strWhere += "and Samp_return.pid_src_id = '108'";
                //}
                if (!string.IsNullOrEmpty(sampQc.PidDeptCode))
                {
                    if (sampQc.PidDeptCode.Contains("&"))
                    {
                        string[] strSplDeptCode = sampQc.PidDeptCode.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                        string deptCodeIn = "";
                        if (strSplDeptCode.Length > 0)
                        {
                            foreach (string item in strSplDeptCode)
                            {
                                deptCodeIn += string.Format(",'{0}'", item);
                            }

                            deptCodeIn = deptCodeIn.Remove(0, 1);

                            strWhere += string.Format(" and Sample_return.Sret_dept_code in({0}) ", deptCodeIn);
                        }
                    }
                    else
                        strWhere += string.Format(" and Sample_return.Sret_dept_code='{0}' ", sampQc.PidDeptCode);
                }

                strSql = strSql + strWhere;


                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                listReturn = EntityManager<EntitySampReturn>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }


            return listReturn;
        }

        public bool UpdateReturnMessage(EntitySampReturn sampReturn)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                string strUpdateSql = @"UPDATE Sample_return
     SET Sret_dept_code =@DeptCode,Sret_dept_name=@DeptName,Sret_user_id=@SenderCode,Sret_user_name=@SenderName,Sret_read_flag=@ReadFlag
      , Sret_Smain_id = @BarcodeId, Sret_proc_flag = @HandleFlag , del_flag = @DelFlag , Sret_pat_Dsorc_id = @OriID where Sret_id=@bcId";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@DeptCode", sampReturn.ReturnDeptCode));
                paramHt.Add(new SqlParameter("@DeptName", sampReturn.ReturnDeptName));
                paramHt.Add(new SqlParameter("@SenderCode", sampReturn.ReturnUserId));
                paramHt.Add(new SqlParameter("@SenderName", sampReturn.ReturnUserName));
                paramHt.Add(new SqlParameter("@ReadFlag", sampReturn.ReturnReadFlag));
                paramHt.Add(new SqlParameter("@BarcodeId", sampReturn.ReturnSampSn));
                paramHt.Add(new SqlParameter("@HandleFlag", sampReturn.ReturnProcFlag));
                paramHt.Add(new SqlParameter("@DelFlag", sampReturn.DelFlag));
                paramHt.Add(new SqlParameter("@OriID", sampReturn.PidSrcId));
                paramHt.Add(new SqlParameter("@bcId", sampReturn.ReturnSn));
                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
