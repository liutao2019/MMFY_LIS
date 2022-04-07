using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.pub.entities;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.result
{
    /// <summary>
    /// 仪器自动处理中期报告与最终报告(目前滨海使用)
    /// </summary>
    public class InstrmtMidandReportBIZ
    {
        private static object objLock = new object();

        private static InstrmtMidandReportBIZ _instance = null;

        /// <summary>
        /// 当时是否没在处理
        /// </summary>
        private static bool IsCurrNotDisposal { get; set; }

        public static InstrmtMidandReportBIZ Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InstrmtMidandReportBIZ();
                        }
                    }
                }
                return _instance;
            }
        }

        private InstrmtMidandReportBIZ()
        {
            IsCurrNotDisposal = true;
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            checkInstrmtData();
        }

        /// <summary>
        /// 检查仪器数据
        /// </summary>
        /// <returns></returns>
        private bool checkInstrmtData()
        {
            bool bln = false;
            string isAutoSendMidAndReport = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("isAutoSendMidAndReport");
            //系统配置：启用自动发中期与审核报告
            if (!string.IsNullOrEmpty(isAutoSendMidAndReport) && isAutoSendMidAndReport != "是")
            {
                return true;
            }

            if (IsCurrNotDisposal)
            {
                IsCurrNotDisposal = false;
            }
            else
            {
                return bln;//如果在处理中,则跳出不继续执行
            }

            try
            {
                string res_itr_id = "";
                string res_cno = "";
                string res_mid_chr = "";
                string res_rep_chr = "";

                string LoginID="";//一审
                string LoginName = "";

                string LoginID2 = "";//二审
                string LoginName2 = "";

                //系统配置：自动发中期与审核报告设置
                //仪器;通道码;中期内容;最终内容;一审者代码;二审者代码
                string strSet = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("setAutoSendMidAndReport");

                if (!string.IsNullOrEmpty(strSet) && strSet.Contains(";"))
                {
                    string[] strSetSpl = strSet.Split(new string[] { ";" }, StringSplitOptions.None);

                    //仪器
                    if (strSetSpl.Length >= 1)
                    {
                        res_itr_id = (string.IsNullOrEmpty(strSetSpl[0]) ? "" : strSetSpl[0]);
                    }
                    //通道码
                    if (strSetSpl.Length >= 2)
                    {
                        res_cno = (string.IsNullOrEmpty(strSetSpl[1]) ? "" : strSetSpl[1]);
                    }
                    //中期内容
                    if (strSetSpl.Length >= 3)
                    {
                        res_mid_chr = (string.IsNullOrEmpty(strSetSpl[2]) ? "" : strSetSpl[2]);
                    }
                    //最终内容
                    if (strSetSpl.Length >= 4)
                    {
                        res_rep_chr = (string.IsNullOrEmpty(strSetSpl[3]) ? "" : strSetSpl[3]);
                    }
                    //一审者代码
                    if (strSetSpl.Length >= 5)
                    {
                        LoginID = (string.IsNullOrEmpty(strSetSpl[4]) ? "" : strSetSpl[4]);
                    }
                    //二审者代码
                    if (strSetSpl.Length >= 6)
                    {
                        LoginID2 = (string.IsNullOrEmpty(strSetSpl[5]) ? "" : strSetSpl[5]);
                    }
                }

                if (!string.IsNullOrEmpty(LoginID))
                {
                    //操作者,名称
                    LoginName = CacheUserInfo.Current.GetUserNameByLoginID(LoginID);
                }

                if (!string.IsNullOrEmpty(LoginID2))
                {
                    //操作者,名称
                    LoginName2 = CacheUserInfo.Current.GetUserNameByLoginID(LoginID2);
                }

                //已经检查过的res_key
                string listResKey = "";

                DataTable dtbResult = GetInstrmtResultomidData(res_itr_id, res_cno);
                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    //属于中期报告的
                    List<string> midPatID = new List<string>();
                    //属于最终报告的
                    List<string> repPatID = new List<string>();

                    #region 遍历查询符合条件的

                    foreach (DataRow dr in dtbResult.Rows)
                    {
                        listResKey += dr["res_key"].ToString() + ",";

                        if (!string.IsNullOrEmpty(res_mid_chr) && !string.IsNullOrEmpty(dr["res_chr_a"].ToString())
                            && dr["res_chr_a"].ToString().Contains(res_mid_chr))
                        {
                            if (!midPatID.Contains(dr["pat_id"].ToString()))
                            {
                                midPatID.Add(dr["pat_id"].ToString());
                            }
                        }
                        else if (!string.IsNullOrEmpty(res_rep_chr) && !string.IsNullOrEmpty(dr["res_chr_a"].ToString())
                            && dr["res_chr_a"].ToString().Contains(res_rep_chr))
                        {
                            if (!repPatID.Contains(dr["pat_id"].ToString()))
                            {
                                repPatID.Add(dr["pat_id"].ToString());
                            }
                        }
                    } 
                    #endregion

                    if (!string.IsNullOrEmpty(listResKey) && listResKey.EndsWith(","))
                    {
                        listResKey = listResKey.TrimEnd(new char[]{','});
                    }

                    string strLog = "";//处理日志

                    //处理中期报告
                    if (midPatID.Count > 0)
                    {
                        List<string> midPatIDFailed = batchCommonMidReport(midPatID, LoginID, LoginName);

                        for (int i = 0; i < midPatID.Count; i++)
                        {
                            if (midPatIDFailed.Contains(midPatID[i]))
                            {
                                strLog += string.Format("pat_id:{0} =>自动中期报告(失败)\r\n", midPatID[i]);
                            }
                            else
                            {
                                strLog += string.Format("pat_id:{0} =>自动中期报告(成功)\r\n", midPatID[i]);
                            }
                        }
                    }

                    //处理最终报告
                    if (repPatID.Count > 0)
                    {
                        List<string> repPatIDFailed1 = batchCommonAudit(repPatID, LoginID, LoginName);
                        List<string> repPatIDFailed2 = batchCommonReport(repPatID, LoginID2, LoginName2);
                        for (int i = 0; i < repPatID.Count; i++)
                        {
                            if (repPatIDFailed1.Contains(repPatID[i]))
                            {
                                strLog += string.Format("pat_id:{0} =>自动一审(失败)\r\n", repPatID[i]);
                            }
                            else
                            {
                                strLog += string.Format("pat_id:{0} =>自动一审(成功)\r\n", repPatID[i]);
                            }

                            if (repPatIDFailed2.Contains(repPatID[i]))
                            {
                                strLog += string.Format("pat_id:{0} =>自动二审(失败)\r\n", repPatID[i]);
                            }
                            else
                            {
                                strLog += string.Format("pat_id:{0} =>自动二审(成功)\r\n", repPatID[i]);
                            }
                        }
                    }

                    //写处理后的日志
                    if (!string.IsNullOrEmpty(strLog))
                    {
                        //dcl.root.logon.Logger.WriteInfo("InstrmtMidandReportBIZ", "自动中期与报告处理日志", strLog);
                        Lib.LogManager.Logger.WriteLine("自动发中期与审核报告日志", strLog);
                    }

                    UpdateLabomanFlag(listResKey);

                    bln = true;
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ", "检查仪器数据", ex.ToString());
            }

            IsCurrNotDisposal = true;

            return bln;
        }

        /// <summary>
        /// 获取最多50条仪器信息
        /// </summary>
        /// <param name="res_itr_id">仪器代码</param>
        /// <param name="res_cno">通道码</param>
        /// <returns></returns>
        private DataTable GetInstrmtResultomidData(string res_itr_id,string res_cno)
        {
            DataTable dtbResult = null;

            if (string.IsNullOrEmpty(res_itr_id) || string.IsNullOrEmpty(res_cno))
            {
                return dtbResult;
            }

            try
            {
                //
                string strSQL =string.Format(@"select top 50 resulto_mid.*,patients.pat_id,patients.pat_flag 
from resulto_mid 
inner join patients on patients.pat_id=resulto_mid.res_id
where resulto_mid.res_itr_id='{0}' and resulto_mid.res_date>getdate()-1 and resulto_mid.res_cno='{1}'
and patients.pat_flag=0 and resulto_mid.res_laboman_flag is null
order by res_date asc", res_itr_id, res_cno);

                SqlHelper objHelper = new SqlHelper();

                dtbResult = objHelper.GetTable(strSQL,"dtResMid");

            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ","获取仪器数据",ex.ToString());
            }

            return dtbResult;
        }

        /// <summary>
        /// 标记已经检查
        /// </summary>
        /// <param name="ListResKey"></param>
        /// <returns></returns>
        private int UpdateLabomanFlag(string ListResKey)
        {
            int rvint = 0;

            if (string.IsNullOrEmpty(ListResKey)) return 0;

            try
            {
                string strSQL = string.Format(@"update resulto_mid set res_laboman_flag='1' where res_laboman_flag is null
and res_key in({0})", ListResKey);

                SqlHelper objHelper = new SqlHelper();
                rvint = objHelper.ExecuteNonQuery(strSQL);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ","更新标记",ex.ToString());
            }
            return rvint;
        }


        /// <summary>
        /// 处理中期报告
        /// </summary>
        /// <param name="midPatID"></param>
        /// <param name="LoginID"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private List<string> batchCommonMidReport(List<string> midPatID, string LoginID, string LoginName)
        {
            List<string> listRvFailed = new List<string>();
            try
            {
                EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();

                caller.LoginName = LoginName;
                caller.LoginID = LoginID;
                caller.Remarks = "自动处理中期报告";//

                PatientEnterService pes = new PatientEnterService();
                EntityOperationResultList opResList = pes.CommonMidReport(midPatID.ToArray(), EnumOperationCode.MidReport, caller, true);

                //返回失败的pat_id
                if (opResList != null && opResList.FailedCount > 0)
                {
                    for (int i = 0; i < opResList.FailedCount; i++)
                    {
                        if (listRvFailed.Contains(opResList.GetFailedOperations()[i].Data.Patient.RepId))
                        {
                            listRvFailed.Add(opResList.GetFailedOperations()[i].Data.Patient.RepId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ", "自动处理中期报告", ex.ToString());
            }

            return listRvFailed;
        }

        /// <summary>
        /// 自动一审
        /// </summary>
        /// <param name="repPatID"></param>
        /// <param name="LoginID"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private List<string> batchCommonAudit(List<string> repPatID, string LoginID, string LoginName)
        {
            List<string> listRvFailed = new List<string>();
            try
            {
                EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();

                caller.LoginName = LoginName;
                caller.LoginID = LoginID;
                caller.OperationName = false.ToString();
                caller.Remarks = "自动一审处理";//

                PatientEnterService pes = new PatientEnterService();
                EntityOperationResultList opResList = pes.CommonAuditCheck(repPatID.ToArray(), EnumOperationCode.Audit, caller);

                //返回失败的pat_id
                if (opResList != null && opResList.FailedCount > 0)
                {
                    for (int i = 0; i < opResList.FailedCount; i++)
                    {
                        if (listRvFailed.Contains(opResList.GetFailedOperations()[i].Data.Patient.RepId))
                        {
                            listRvFailed.Add(opResList.GetFailedOperations()[i].Data.Patient.RepId);
                        }
                    }
                }

                //提取检查通过的
                List<string> listSuccess = new List<string>();
                if (listRvFailed.Count > 0)
                {
                    for (int j = 0; j < repPatID.Count; j++)
                    {
                        if (!listRvFailed.Contains(repPatID[j]))
                        {
                            listSuccess.Add(repPatID[j]);
                        }
                    }
                }
                else
                {
                    listSuccess = repPatID;
                }

                //一审检查通过的
                pes = new PatientEnterService();
                pes.AuditBatch(listSuccess, caller);

            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ", "自动一审处理", ex.ToString());
            }

            return listRvFailed;

        }

        /// <summary>
        /// 自动二审
        /// </summary>
        /// <param name="repPatID"></param>
        /// <param name="LoginID"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        private List<string> batchCommonReport(List<string> repPatID, string LoginID, string LoginName)
        {
            List<string> listRvFailed = new List<string>();
            try
            {
                EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();

                caller.LoginName = LoginName;
                caller.LoginID = LoginID;
                caller.OperationName = false.ToString();
                caller.Remarks = "自动二审处理";//

                PatientEnterService pes = new PatientEnterService();
                EntityOperationResultList opResList = pes.CommonAuditCheck(repPatID.ToArray(), EnumOperationCode.Report, caller);

                //返回失败的pat_id
                if (opResList != null && opResList.FailedCount > 0)
                {
                    for (int i = 0; i < opResList.FailedCount; i++)
                    {
                        if (listRvFailed.Contains(opResList.GetFailedOperations()[i].Data.Patient.RepId))
                        {
                            listRvFailed.Add(opResList.GetFailedOperations()[i].Data.Patient.RepId);
                        }
                    }
                }

                //提取检查通过的
                List<string> listSuccess = new List<string>();
                if (listRvFailed.Count > 0)
                {
                    for (int j = 0; j < repPatID.Count; j++)
                    {
                        if (!listRvFailed.Contains(repPatID[j]))
                        {
                            listSuccess.Add(repPatID[j]);
                        }
                    }
                }
                else
                {
                    listSuccess = repPatID;
                }

                //二审检查通过的
                pes = new PatientEnterService();
                pes.ReportBatch(listSuccess, caller);

            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("InstrmtMidandReportBIZ", "自动二审处理", ex.ToString());
            }

            return listRvFailed;

        }

    }


    /**SQL脚本
     
--2015年04月22日 添加系统配置：启用自动发中期与审核报告
if not exists(select configCode from sysconfig where configCode='isAutoSendMidAndReport')
begin
INSERT INTO sysconfig ([configCode],[configGroup],[configItemName],[configItemType]
           ,[configItemValue],[configDict],[configType])
     VALUES
           ('isAutoSendMidAndReport'
           ,'其他'
           ,'启用自动发中期与审核报告'
           ,'枚举'
           ,'否'
           ,'是,否'
           ,'system')
end

go


--2015年04月22日 添加系统配置：自动发中期与审核报告设置
if not exists(select configCode from sysconfig where configCode='setAutoSendMidAndReport')
begin
INSERT INTO sysconfig ([configCode],[configGroup],[configItemName],[configItemType]
           ,[configItemValue],[configDict],[configType])
     VALUES
           ('setAutoSendMidAndReport'
           ,'其他'
           ,'自动发中期与审核报告设置'
           ,'字符串'
           ,'仪器;通道码;中期内容;最终内容;一审者代码;二审者代码'
           ,'仪器;通道码;中期内容;最终内容;一审者代码;二审者代码'
           ,'system')
end

go
     
     ***/
}
