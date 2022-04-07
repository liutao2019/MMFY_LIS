using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResult))]
    public class DaoObrResult : DclDaoBase, IDaoObrResult
    {

        public bool InsertObrResult(EntityObrResult ObrResult)
        {
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lres_Pma_rep_id", ObrResult.ObrId);
                values.Add("Lres_Ditr_id", ObrResult.ObrItrId);
                values.Add("Lres_sid", ObrResult.ObrSid);
                values.Add("Lres_Ditm_id", ObrResult.ItmId);
                values.Add("Lres_itm_ename", ObrResult.ItmEname);
                values.Add("Lres_value", ObrResult.ObrValue);
                values.Add("Lres_value2", ObrResult.ObrValue2);
                values.Add("Lres_convert_value", ObrResult.ObrConvertValue);
                values.Add("Lres_unit", ObrResult.ObrUnit);
                values.Add("Lres_price", ObrResult.ItmPrice);
                values.Add("Lres_lower_limit", ObrResult.RefLowerLimit);
                values.Add("Lres_upper_limit", ObrResult.RefUpperLimit);
                values.Add("Lres_more", ObrResult.RefMore);
                values.Add("Lres_ref_flag", ObrResult.RefFlag);
                values.Add("Lres_itm_method", ObrResult.ObrItmMethod);
                values.Add("Lres_date", ObrResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lres_flag", ObrResult.ObrFlag);
                values.Add("Lres_type", ObrResult.ObrType);
                values.Add("Lres_itm_report_type", ObrResult.ObrReportType);
                values.Add("Lres_itm_Dcom_id", ObrResult.ItmComId);
                values.Add("Lres_itm_report_code", ObrResult.ItmReportCode);
                values.Add("Lres_source_Ditr_id", ObrResult.ObrSourceItrId);
                values.Add("Lres_ref_type", ObrResult.RefType);
                values.Add("Lres_remark", ObrResult.ObrRemark);
                values.Add("Lres_recheck_flag", ObrResult.ObrRecheckFlag);
                values.Add("Lres_value3", ObrResult.ObrValue3);
                values.Add("Lres_value4", ObrResult.ObrValue4);
                values.Add("Lres_warn1", ObrResult.ObrWarn1);
                values.Add("Lres_warn2", ObrResult.ObrWarn2);
                values.Add("Lres_warn3", ObrResult.ObrWarn3);
                values.Add("Lres_warn4", ObrResult.ObrWarn4);
                values.Add("Lres_warn5", ObrResult.ObrWarn5);
                values.Add("Lres_warn6", ObrResult.ObrWarn6);
                values.Add("Lres_warn7", ObrResult.ObrWarn7);
                values.Add("Lres_register", ObrResult.ObrRegister);
                values.Add("Lres_bone_value", ObrResult.ObrBoneValue);
                values.Add("Lres_bld_value", ObrResult.ObrBldValue);

                helper.InsertOperation("Lis_result", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }

        public bool UploadObrResult(EntityObrResult ObrResult)
        {
            try
            {
                DBManager helper = new DBManager("GHConnectionString");

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lres_Pma_rep_id", ObrResult.ObrId);
                values.Add("Lres_Ditr_id", ObrResult.ObrItrId);
                values.Add("Lres_sid", ObrResult.ObrSid);
                values.Add("Lres_Ditm_id", ObrResult.ItmId);
                values.Add("Lres_itm_ename", ObrResult.ItmEname);
                values.Add("Lres_value", ObrResult.ObrValue);
                values.Add("Lres_value2", ObrResult.ObrValue2);
                values.Add("Lres_convert_value", ObrResult.ObrConvertValue);
                values.Add("Lres_unit", ObrResult.ObrUnit);
                values.Add("Lres_price", ObrResult.ItmPrice);
                values.Add("Lres_lower_limit", ObrResult.RefLowerLimit);
                values.Add("Lres_upper_limit", ObrResult.RefUpperLimit);
                values.Add("Lres_more", ObrResult.RefMore);
                values.Add("Lres_ref_flag", ObrResult.RefFlag);
                values.Add("Lres_itm_method", ObrResult.ObrItmMethod);
                values.Add("Lres_date", ObrResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lres_flag", ObrResult.ObrFlag);
                values.Add("Lres_type", ObrResult.ObrType);
                values.Add("Lres_itm_report_type", ObrResult.ObrReportType);
                values.Add("Lres_itm_Dcom_id", ObrResult.ItmComId);
                values.Add("Lres_itm_report_code", ObrResult.ItmReportCode);
                values.Add("Lres_source_Ditr_id", ObrResult.ObrSourceItrId);
                values.Add("Lres_ref_type", ObrResult.RefType);
                values.Add("Lres_remark", ObrResult.ObrRemark);
                values.Add("Lres_recheck_flag", ObrResult.ObrRecheckFlag);
                values.Add("Lres_value3", ObrResult.ObrValue3);
                values.Add("Lres_value4", ObrResult.ObrValue4);
                values.Add("Lres_warn1", ObrResult.ObrWarn1);
                values.Add("Lres_warn2", ObrResult.ObrWarn2);
                values.Add("Lres_warn3", ObrResult.ObrWarn3);
                values.Add("Lres_warn4", ObrResult.ObrWarn4);
                values.Add("Lres_warn5", ObrResult.ObrWarn5);
                values.Add("Lres_warn6", ObrResult.ObrWarn6);
                values.Add("Lres_warn7", ObrResult.ObrWarn7);
                values.Add("Lres_register", ObrResult.ObrRegister);

                helper.InsertOperation("Lis_result", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }
        public bool UpdateObrResultByObrIdAndObrItmId(EntityObrResult ObrResult)
        {
            DBManager helper = GetDbManager();
            try
            {
                string whereUpdate = string.Empty;
                //生成结果录入时间,并检索其中已被审核或报告的数据
                if (!string.IsNullOrEmpty(ObrResult.ObrId) && !string.IsNullOrEmpty(ObrResult.ItmId))
                {
                    whereUpdate = " where Lres_Pma_rep_id='" + ObrResult.ObrId + "' and Lres_Ditm_id='" + ObrResult.ItmId + "' ";
                }
                string sqlUpdate = string.Format(@"update Lis_result set Lres_value=@obr_value ,Lres_convert_value=@obr_convert_value,Lres_value2=@obr_value2,
                    Lres_ref_flag =@ref_flag,Lres_itm_report_type=@obr_report_type,Lres_date =@obr_date,Lres_type=@obr_type {0} ", whereUpdate);
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_value", ObrResult.ObrValue));
                paramHt.Add(new SqlParameter("@obr_convert_value", ObrResult.ObrConvertValue));
                paramHt.Add(new SqlParameter("@obr_value2", ObrResult.ObrValue2));
                paramHt.Add(new SqlParameter("@ref_flag", ObrResult.RefFlag));
                paramHt.Add(new SqlParameter("@obr_report_type", ObrResult.ObrReportType));
                paramHt.Add(new SqlParameter("@obr_date", ObrResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss")));
                paramHt.Add(new SqlParameter("@obr_type", ObrResult.ObrType));
                helper.ExecCommand(sqlUpdate, paramHt);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }

        public bool UpdateObrFlagByCondition(EntityResultQC resultQc)
        {
            bool result = false;
            try
            {
                string sql = "update Lis_result set Lres_flag = 0";
                string where = string.Empty;
                DBManager helper = new DBManager();
                if (resultQc.ObrSn != null && !string.IsNullOrEmpty(resultQc.ObrSn.ToString()) && resultQc.ObrSn != "0")
                {
                    where = string.Format("where Lres_id='{0}'", resultQc.ObrSn.ToString());
                }
                if (resultQc.ListObrId.Count > 0 && !string.IsNullOrEmpty(resultQc.ListObrId[0]) && resultQc.listItmIds.Count > 0)
                {
                    string strItmId = string.Empty;
                    foreach (string itmId in resultQc.listItmIds)
                    {
                        strItmId += string.Format(",'{0}'", itmId);
                    }
                    strItmId = strItmId.Remove(0, 1);
                    where += string.Format("where Lres_Pma_rep_id='{0}' and Lres_Ditm_id in ({1})", resultQc.ListObrId[0], strItmId);
                }
                sql += where;
                if (!string.IsNullOrEmpty(where))
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        private string GetQueryWhere(EntityResultQC resultQc)
        {
            string where = string.Empty;

            //检验报告病人ID
            if (resultQc.ListObrId.Count > 0)
            {
                if (resultQc.ListObrId.Count == 1)
                {
                    where += string.Format(" and Lres_Pma_rep_id = '{0}'", resultQc.ListObrId[0]);
                }
                else
                {
                    string strObrIds = string.Empty;
                    foreach (string obrId in resultQc.ListObrId)
                    {
                        strObrIds += string.Format(",'{0}'", obrId);
                    }
                    strObrIds = strObrIds.Remove(0, 1);

                    where += string.Format(" and Lres_Pma_rep_id in ({0})", strObrIds);
                }
            }
            //报告有效标志
            if (resultQc.ObrFlag == "1")
            {
                where += string.Format(" and Lres_flag  = '{0}'", resultQc.ObrFlag);
            }
            if (resultQc.ResChrIsNull)
            {
                where += " and Lres_value is not null and Lres_value<>''";
            }
            //报告时间
            if (!string.IsNullOrEmpty(resultQc.StartObrDate) && !string.IsNullOrEmpty(resultQc.EndObrDate))
            {
                where += string.Format(" and Lres_date >= '{0}' and Lres_date < '{1}'", resultQc.StartObrDate, resultQc.EndObrDate);
            }
            //不属于检验和审核状态
            if (!resultQc.IsCheck)
            {
                where += "and isnull(Pat_lis_main.Pma_status,'') not in (2,4) ";
            }
            //仪器ID
            if (!string.IsNullOrEmpty(resultQc.ItrId))
            {
                where += string.Format(" and Lres_Ditr_id = '{0}'", resultQc.ItrId);
            }
            //是否只获取无病人资料的结果 并且结果合并不是复制查询
            if (resultQc.OnlyGetNonePatInfoResult && !resultQc.IsCopy)
            {
                where += " and Pat_lis_main.Pma_rep_id is null  ";
            }
            //主键ID
            if (!string.IsNullOrEmpty(resultQc.ObrSn))
            {
                where += string.Format(" and Lis_result.Lres_id = '{0}'", resultQc.ObrSn);
            }

            //项目是否打印
            if (resultQc.IsNullItmPrtFlag)
            {
                where += string.Format(" and isnull(Dict_itm.Ditm_prt_flag,'0')='1' ");
            }

            //项目ID
            if (!string.IsNullOrEmpty(resultQc.ItmId))
            {
                where += string.Format(" and Lis_result.Lres_Ditm_id='{0}'", resultQc.ItmId);
            }
            //项目ID为空
            if (resultQc.ItmIdIsNull)
            {
                where += "and Lis_result.Lres_Ditm_id is not null and Lis_result.Lres_Ditm_id <>";
            }
            //组合ID为空
            if (resultQc.ItmComIdIsNull)
            {
                where += "and itm_com_id is null or itm_com_id = '' or itm_com_id = '-1')";
            }
            //是否选中酶标
            if (resultQc.IsCheckMB)
            {
                where += string.Format(" and Lres_value2<>'' ");
            }
            //只获取2审的结果
            if (resultQc.OnlyReport)
            {
                where += string.Format("and (Pma_status=2 or Pma_status=4)");
            }
            //只取危急结果
            if (resultQc.UrgentFlag)
            {
                where += string.Format("and Lis_result.Lres_ref_flag in('6','16','24','32','40','48','56','256','384','512','640','768','896')");
            }
            #region 病人信息条件
            //报告ID
            if (!string.IsNullOrEmpty(resultQc.RepId))
            {
                where += string.Format(" and Pat_lis_main.Pma_rep_id <> '{0}'", resultQc.RepId);
            }
            //条码号
            if (!string.IsNullOrEmpty(resultQc.RepBarCode))
            {
                where += string.Format(" and Pat_lis_main.Pma_bar_code <> '{0}'", resultQc.RepBarCode);
            }
            //标本ID
            if (!string.IsNullOrEmpty(resultQc.PidSamId))
            {
                where += string.Format(" and Pat_lis_main.Pma_Dsam_id = '{0}'", resultQc.PidSamId);
            }
            //病人开始时间
            if (!string.IsNullOrEmpty(resultQc.StartRepInDate))
            {
                where += string.Format(" and Pat_lis_main.Pma_in_date >= '{0}'", resultQc.StartRepInDate);
            }
            //病人结束时间
            if (!string.IsNullOrEmpty(resultQc.EndRepInDate))
            {
                where += string.Format(" and Pat_lis_main.Pma_in_date <= '{0}'", resultQc.EndRepInDate);
                where += string.Format(" and ( Pat_lis_main.Pma_status = '0' or Pat_lis_main.Pma_status = '1' or Pat_lis_main.Pma_status = '2' or Pat_lis_main.Pma_status = '4')");
            }
            //病人ID
            if (!string.IsNullOrEmpty(resultQc.PidInNo))
            {
                where += string.Format(" and Pat_lis_main.Pma_pat_in_no = '{0}'", resultQc.PidInNo);
            }
            //病人姓名
            if (!string.IsNullOrEmpty(resultQc.PidSamId))
            {
                where += string.Format(" and Pat_lis_main.Pma_pat_name = '{0}'", resultQc.PidName);
            }
            //病人性别
            if (!string.IsNullOrEmpty(resultQc.PidSamId))
            {
                where += string.Format(" and Pat_lis_main.Pma_pat_sex = '{0}'", resultQc.PidSex);
            }
            //样本号
            if (!string.IsNullOrEmpty(resultQc.RepSid))
            {
                where += string.Format(" and Lis_result.Lres_sid='{0}'", resultQc.RepSid);
            }
            //独特ID
            if (!string.IsNullOrEmpty(resultQc.PidUniqueId))
            {
                where += string.Format(" and Pat_lis_main.Pma_unique_id='{0}'", resultQc.PidUniqueId);
            }
            //就诊卡号
            if (!string.IsNullOrEmpty(resultQc.PidSocialNo))
            {
                where += string.Format(" and Pat_lis_main.Pma_social_no='{0}'", resultQc.PidUniqueId);
            }
            //住院次数
            if (!string.IsNullOrEmpty(resultQc.PidAddmissTimes))
            {
                where += string.Format(" and Pat_lis_main.Pma_pat_addmiss_times='{0}'", resultQc.PidUniqueId);
            }
            #endregion
            return where;
        }
        public List<EntityObrResult> ObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            string where = GetQueryWhere(resultQc);
            if (!string.IsNullOrEmpty(where))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"select
Lis_result.*,
Dict_itm.Ditm_name , --项目名称
Dict_itm.Ditm_calu_flag, --是否计算项目
Dict_itm_combine.Dcom_name,--组合名称
--isnull(Dict_itm.sort_no, 0) as itm_seq,
Rel_itm_combine_item.sort_no as itm_seq,
Dict_itm_combine.sort_no,--组合排序
Rel_itm_combine_item.sort_no as com_mi_sort,--组合明细排序
dic_itr_instrument_warningsigns.itr_warn_trandate,--项目状态
Pma_pat_name,--姓名
Pma_pat_in_no,--病人Id
Pma_pat_dept_name,--科室名称
Pat_lis_main.Pma_status,
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_check_Buser_id,
Pat_lis_main.Pma_audit_Buser_id,
Pat_lis_main.Pma_report_date,
Dict_itm.Ditm_contrast_factor,
Dict_itm.Ditm_contrast_code,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_sub_title,
rtrim(isnull(Lis_result.Lres_lower_limit,''))+(case when  isnull(Lis_result.Lres_lower_limit,'') = '' or isnull(Lis_result.Lres_upper_limit,'') = '' then '' else '-' end)+Rtrim(isnull(Lis_result.Lres_upper_limit,'')) res_ref_range,
Rel_itm_result_tips.Rtip_value 
from Lis_result
left join Dict_itm on Dict_itm.Ditm_id = Lis_result.Lres_Ditm_id--join项目表
left join dic_itr_instrument_warningsigns on dic_itr_instrument_warningsigns.itr_warn_origdate=Lis_result.Lres_remark
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
left join Dict_itm_combine on Dict_itm_combine.Dcom_id = Lis_result.Lres_itm_Dcom_id
left join Dict_itr_instrument on Lis_result.Lres_Ditr_id=Dict_itr_instrument.Ditr_id
left join Rel_itm_result_tips on Rel_itm_result_tips.Rtip_id=Lis_result.Lres_ref_flag
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Dcom_id=Lis_result.Lres_itm_Dcom_id and 
            Rel_itm_combine_item.Rici_Ditm_id = Lis_result.Lres_Ditm_id
where 1=1 {0}", where);

                //Lib.LogManager.Logger.LogInfo("ObrResultQuery" + sql);
                DataTable dt = helper.ExecuteDtSql(sql);
                listResult = EntityManager<EntityObrResult>.ConvertToList(dt);

                if(listResult.Find(o => o.ItmId == "2082") != null)
                {
                    listResult.Find(o => o.ItmId == "2082").RefLowerLimit = string.Empty;
                    listResult.Find(o => o.ItmId == "2082").RefUpperLimit = string.Empty;
                }

            }
            return listResult;
        }

        private string GetQueryWhereEx(EntityResultQC resultQc)
        {
            string where = string.Empty;

            //病人唯一号标识集合
            if (resultQc.ListPidInNo.Count > 0)
            {
                if (resultQc.ListPidInNo.Count == 1)
                {
                    where += string.Format(" and Pma_pat_in_no = '{0}'", resultQc.ListPidInNo[0]);
                }
                else
                {
                    string strPidInNos = string.Empty;
                    foreach (string obrId in resultQc.ListPidInNo)
                    {
                        strPidInNos += string.Format(",'{0}'", obrId);
                    }
                    strPidInNos = strPidInNos.Remove(0, 1);

                    where += string.Format(" and Pma_pat_in_no in ({0})", strPidInNos);
                }
            }


            //项目ID编码集合
            if (resultQc.ListDitmEcode.Count > 0)
            {
                if (resultQc.ListDitmEcode.Count == 1)
                {
                    where += string.Format(" and Lis_result.Lres_itm_ename ='{0}'", resultQc.ListDitmEcode[0]);
                }
                else
                {
                    string strDitmEcodes = string.Empty;
                    foreach (string obrId in resultQc.ListDitmEcode)
                    {
                        strDitmEcodes += string.Format(",'{0}'", obrId);
                    }
                    strDitmEcodes = strDitmEcodes.Remove(0, 1);

                    where += string.Format(" and Lis_result.Lres_itm_ename in ({0})", strDitmEcodes);
                }
            }


            //项目ID
            if (!string.IsNullOrEmpty(resultQc.ItmId))
            {
                where += string.Format(" and Lis_result.Lres_Ditm_id='{0}'", resultQc.ItmId);
            }


            return where;
        }
        /// <summary>
        /// 检验结果查询（根据项目编码等条件）
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        public List<EntityObrResult> LisResultQuery(EntityResultQC resultQc)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            string where = GetQueryWhereEx(resultQc);
            if (!string.IsNullOrEmpty(where))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"select
                    Lis_result.*,
                    Pat_lis_main.Pma_pat_in_no
                    from Lis_result
                    left join Dict_itm on Dict_itm.Ditm_id = Lis_result.Lres_Ditm_id--join项目表
                    left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
                    where 1=1 {0}", where);

                //Lib.LogManager.Logger.LogInfo("LisResultQuery" + sql);
                DataTable dt = helper.ExecuteDtSql(sql);
                listResult = EntityManager<EntityObrResult>.ConvertToList(dt);
            }
            return listResult;
        }


        public bool UpdateObrResult(EntityObrResult ObrResult)
        {
            bool result = false;
            if (ObrResult != null)
            {
                DBManager helper = GetDbManager();
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBUpdateParameter(ObrResult);

                    values.Remove("Lres_id");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Lres_id", ObrResult.ObrSn);

                    helper.UpdateOperation("Lis_result", values, keys);
                    //string sg = helper.GetUpdateSql("Lis_result", values, keys);

                    //Lib.LogManager.Logger.LogInfo("UpdateObrResult" + sg);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw ex;
                }
            }
            return result;
        }

        public bool UpdateRecheckFalgByObrSn(string obrSn)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {

                string sql = string.Format("update Lis_result set Lres_recheck_flag = 1 where Lres_id='{0}'", obrSn);
                DBManager helper = new DBManager();
                try
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                string sql = string.Format("delete from Lis_result where Lres_id = '{0}'", obrSn);
                DBManager helper = GetDbManager();
                try
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public bool DeleteObrResultByObrId(string obrId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrId))
            {
                string sql = string.Format("delete from Lis_result where Lres_Pma_rep_id = '{0}'", obrId);
                string sql2 = string.Format("delete from lis_result_originalex where Lro_Lresdesc_id = '{0}'", obrId);
                DBManager helper = new DBManager();
                try
                {
                    helper.ExecCommand(sql);
                    helper.ExecCommand(sql2);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<EntityObrResult> GetObrResultByNoPat(List<string> listItrs, DateTime startDate, DateTime endDate)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            try
            {
                string strItr = string.Empty;
                if (listItrs != null && listItrs.Count > 0)
                {
                    foreach (string itr in listItrs)
                    {
                        strItr += string.Format(",'{0}'", itr);
                    }
                }
                strItr = strItr.Remove(0, 1);
                string sql = string.Format(@"SELECT Lres_Pma_rep_id,  Lres_sid 
,Lres_value ,Lres_itm_ename ,Dict_itr_instrument.Ditr_ename
FROM Lis_result 
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
inner join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Lis_result.Lres_Ditr_id
where Pat_lis_main.Pma_rep_id is null and Lis_result.Lres_Pma_rep_id is not null
and Lis_result.Lres_Ditr_id in ({0})
and (Lis_result.Lres_date >='{1}' and Lis_result.Lres_date <'{2}') and Lis_result.Lres_flag = 1
                ", strItr, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                DBManager helper = new DBManager();
                DataTable dtPats = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityObrResult>.ConvertToList(dtPats).OrderBy(i => i.ObrItrId).ThenBy(i => i.ObrSid.Length).ThenBy(i => i.ObrSid).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool DeleteObrResultByQC(EntityResultQC resultQc)
        {
            bool result = false;
            string sql = string.Format("delete from Lis_result where Lres_flag=1");
            if (resultQc.ListObrId.Count > 0)
            {
                sql += string.Format(@" and Lres_Pma_rep_id ='{0}'", resultQc.ListObrId[0]);
            }
            if (resultQc.listItmIds.Count > 0)
            {
                string strItmId = string.Empty;
                foreach (string itmId in resultQc.listItmIds)
                {
                    strItmId += string.Format(",'{0}'", itmId);
                }
                strItmId = strItmId.Remove(0, 1);
                sql += string.Format(@" and Lis_result.Lres_Ditm_id in ({0})", strItmId);
            }
            DBManager helper = new DBManager();
            try
            {
                helper.ExecCommand(sql);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateResultVauleByObrSn(string obrValue, string obrConvertValue, string obrSn)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(obrSn))
                {
                    string sql = "update Lis_result set Lres_value = @obr_value,Lres_convert_value = @obr_convert_value where Lres_id = @obr_sn";
                    List<DbParameter> paramHt = new List<DbParameter>();
                    DBManager helper = new DBManager();
                    if (string.IsNullOrEmpty(obrConvertValue))
                    {
                        sql = "update Lis_result set Lres_value = @obr_value where Lres_id = @obr_sn";
                    }
                    else
                    {
                        paramHt.Add(new SqlParameter("@obr_convert_value", obrConvertValue));
                    }
                    paramHt.Add(new SqlParameter("@obr_value", obrValue));
                    paramHt.Add(new SqlParameter("@obr_sn", obrSn));
                    result = true;
                    helper.ExecCommand(sql, paramHt);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateResultComIdByObrIdAndItmID(string comId, string obrId, string itmId)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(obrId) && !string.IsNullOrEmpty(itmId))
                {
                    string sql = "update Lis_result set Lres_itm_Dcom_id = @itm_com_id where Lres_Pma_rep_id = @obr_id and Lres_Ditm_id = @itm_id";
                    DBManager helper = new DBManager();
                    List<DbParameter> paramHt = new List<DbParameter>();
                    paramHt.Add(new SqlParameter("@itm_com_id", comId));
                    paramHt.Add(new SqlParameter("@obr_id", obrId));
                    paramHt.Add(new SqlParameter("@itm_id", itmId));
                    result = true;
                    helper.ExecCommand(sql, paramHt);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateResultRefMore(string obrSn, string obrId, string resMore, bool isClear)
        {
            bool result = false;
            try
            {
                string sql = string.Format("update Lis_result set Lres_more = '{0}' where 1=1 ", resMore);
                string where = string.Empty;
                if (!string.IsNullOrEmpty(obrSn) && !isClear)
                {
                    where = string.Format("and Lres_id='{0}'", obrSn);
                }
                //清空分期参考值
                if (isClear && !string.IsNullOrEmpty(obrId))
                {
                    where = string.Format("and Lres_Pma_rep_id='{0}'", obrId);
                }
                DBManager helper = new DBManager();
                if (!string.IsNullOrEmpty(where))
                {
                    sql += where;
                    result = true;
                    int updateFlag = helper.ExecCommand(sql);
                    //Lib.LogManager.Logger.LogInfo("更新状态["+ updateFlag + "]，更新分期参考值sql：" + sql);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityObrResult> GetResultInfoByBarcode(string RepBarcode, string repId)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@" select  Pma_status,
Pma_check_Buser_id,
Pma_audit_Buser_id,
Lres_Ditm_id,
Lres_itm_ename,
Lres_value,
Lres_convert_value
from Lis_result 
inner join Pat_lis_main on Lis_result.Lres_Pma_rep_id=Pat_lis_main.Pma_rep_id
where Pma_bar_code='{0}'
and Pma_rep_id<>'{1}' 
and Lis_result.Lres_flag=1", RepBarcode, repId);
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityObrResult>.ConvertToList(dt);
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }

        public List<EntityObrResult> GetResultHistory(EntityHistoryPatientQC qc)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            try
            {
                DBManager helper = new DBManager();
                string strWhere = string.Empty;
                if (qc.ListItrId.Count > 0)
                {
                    if (qc.ListItrId.Count == 1)
                    {
                        strWhere += " and Pma_Ditr_id ='" + qc.ListItrId[0] + "'";
                    }
                    else
                    {
                        string strItrs = string.Empty;
                        foreach (string item in qc.ListItrId)
                        {
                            strItrs += string.Format(",'{0}'", item);
                        }
                        strItrs = strItrs.Remove(0, 1);
                        strWhere += string.Format(" and Pma_Ditr_id in ({0})", strItrs);
                    }

                }
                if (qc.listSamId.Count>0)
                {
                    if (qc.listSamId.Count == 1)
                    {
                        strWhere += " and Pma_Dsam_id ='" + qc.listSamId[0] + "'";
                    }
                    else
                    {
                        string strSams = string.Empty;
                        foreach (string item in qc.listSamId)
                        {
                            strSams += string.Format(",'{0}'", item);
                        }
                        strSams = strSams.Remove(0, 1);
                        strWhere += string.Format(" and Pma_Dsam_id in ({0})", strSams);
                    }
                }
                if (!string.IsNullOrEmpty(qc.PidName))
                {
                    strWhere += string.Format("and Pma_pat_name='{0}'", qc.PidName);
                }
                if (!string.IsNullOrEmpty(qc.PidInNo))
                {
                    strWhere += string.Format("and Pma_pat_in_no='{0}'", qc.PidInNo);
                }
                if (!string.IsNullOrEmpty(qc.RepStatus))
                {
                    strWhere += string.Format("and Pma_status in ({0})", qc.RepStatus);
                }
                string sql = string.Format(@"SELECT res.Lres_Pma_rep_id,res.Lres_sid,res.Lres_value,res.Lres_Ditm_id,res.Lres_itm_ename, case when Pma_report_date is null then Pat_lis_main.Pma_in_date else Pma_report_date end as Lres_date,
res.Lres_lower_limit,res.Lres_upper_limit,res.Lres_type,res.Lres_recheck_flag,Dict_itm.Ditm_name,Dict_itm.sort_no as itm_seq,Pat_lis_main.Pma_pat_in_no
FROM Pat_lis_main 
INNER JOIN Lis_result AS res ON Pat_lis_main.Pma_rep_id = res.Lres_Pma_rep_id 
inner join Dict_itm on res.Lres_Ditm_id = Dict_itm.Ditm_id 
where  Lres_Pma_rep_id in
(SELECT top {0} Pma_rep_id from Pat_lis_main
where 1=1 {1}
and Pma_rep_id <> @rep_id
and Pma_in_date <= @rep_in_date
order by Pma_in_date desc)
and Lres_flag = 1 ", qc.ResultCount, strWhere);
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@rep_id", qc.RepId));
                paramHt.Add(new SqlParameter("@rep_in_date", qc.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss")));
                DataTable dt = helper.ExecuteDtSql(sql, paramHt);
                list = EntityManager<EntityObrResult>.ConvertToList(dt).OrderByDescending(w => w.ObrDate).ThenBy(w => w.ItmSeq).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
