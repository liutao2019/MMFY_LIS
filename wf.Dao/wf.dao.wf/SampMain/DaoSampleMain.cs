using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampMain))]
    public class DaoSampleMain : DclDaoBase, IDaoSampMain
    {
        public List<EntitySampMain> GetSampMain(EntitySampQC sampQc)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();

            string strSql = @"select
Sample_main.*,
Dict_profession.Dpro_id,
Dict_profession.Dpro_name,
Dict_profession.Dpro_remark,
Dict_profession.Dpro_type,
Dict_profession.Dpro_Dorg_id,
Dict_profession.Dpro_link,
Dict_source.Dsorc_id,
Dict_source.Dsorc_name,
Dict_test_tube.Dtub_code,
Dict_test_tube.Dtub_name,
Dict_test_tube.Dtub_flag,
Dict_test_tube.Dtub_barcode_min,
Dict_test_tube.Dtub_barcode_max,
Dict_test_tube.Dtub_max_capcity,
Dict_test_tube.Dtub_unit,
Dict_test_tube.Dtub_max_com,
Dict_test_tube.Dtub_charge_code,
Dict_test_tube.Dtub_price,
Dict_test_tube.Dtub_color,
Dict_sample.Dsam_id,
Dict_sample.Dsam_name,
Dict_sample.Dsam_code,
Dict_sample.Dsam_Dpro_id,
Dict_sample.Dsam_custom_type,
Dict_sample.Dsam_trans_code,
Dict_sample_remark.Dsamr_id,
Dict_sample_remark.Dsamr_content,
Dict_sample_remark.Dsamr_newborn,
Dict_dept.Ddept_tel,
0 as com_line_color,
dbo.getAge(Sample_main.Sma_pat_age) barcode_age,
'' as ComCapSum,
Dict_profession.Dpro_remark,
bc_price = 0 {0}
from Sample_main  with(nolock)
LEFT OUTER JOIN Dict_profession ON Sample_main.Sma_type = Dict_profession.Dpro_id 
LEFT OUTER JOIN Dict_source on Sample_main.Sma_pat_src_id = Dict_source.Dsorc_id 
LEFT OUTER JOIN Dict_test_tube ON Sample_main.Sma_tub_code = Dict_test_tube.Dtub_code  
LEFT OUTER JOIN Dict_sample ON Sample_main.Sma_Dsam_id = Dict_sample.Dsam_id 
LEFT OUTER JOIN Dict_sample_remark ON Sample_main.Sma_rem_id = Dict_sample_remark.Dsamr_id  
LEFT OUTER JOIN Dict_dept ON Sample_main.Sma_pat_dept_code=Dict_dept.Ddept_code
{1}
where 1=1 
 ";
            string sqlWhere = string.Empty;
            //条码号
            if (sampQc.ListSampBarId.Count > 0)
            {
                if (sampQc.ListSampBarId.Count == 1)
                {
                    sqlWhere += "and Sample_main.Sma_bar_id='" + sampQc.ListSampBarId[0] + "'";
                }
                else
                {
                    string strBarId = string.Empty;
                    foreach (string barId in sampQc.ListSampBarId)   
                    {
                        strBarId += string.Format(",'{0}'", barId);
                    }

                    strBarId = strBarId.Remove(0, 1);

                    sqlWhere += "and Sample_main.Sma_bar_id in (" + strBarId + ")";
                }
            }

            //时间范围
            if (!string.IsNullOrEmpty(sampQc.StartDate) && !string.IsNullOrEmpty(sampQc.EndDate))
            {
                if (sampQc.SearchDateType == SearchDataType.标本生成时间)
                    sqlWhere += string.Format("and Sma_occ_date BETWEEN '{0}' AND '{1}'", sampQc.StartDate, sampQc.EndDate);
                else if (sampQc.SearchDateType == SearchDataType.标本最后操作时间)
                    sqlWhere += string.Format("and Sma_lastoper_date BETWEEN '{0}' AND '{1}'", sampQc.StartDate, sampQc.EndDate);
                else if (sampQc.SearchDateType == SearchDataType.标本下载时间)
                    sqlWhere += string.Format("and Sma_date BETWEEN '{0}' AND '{1}'", sampQc.StartDate, sampQc.EndDate);
                else if (sampQc.SearchDateType == SearchDataType.标本流程时间 && !string.IsNullOrEmpty(sampQc.SerchDateStatus))
                {
                    string strWhere = string.Format("Sproc_date BETWEEN '{0}' AND '{1}' and Sproc_status='{2}'", sampQc.StartDate, sampQc.EndDate, sampQc.SerchDateStatus);
                    sqlWhere += string.Format(@"and Sample_main.Sma_bar_code in ( select Sproc_Sma_bar_code from Sample_process_detial WITH(NOLOCK) where {0} group by Sproc_Sma_bar_code)", strWhere);
                }
            }

            //物理组
            if (sampQc.ListType.Count > 0)
            {
                string typeWhere = string.Empty;
                foreach (string item in sampQc.ListType)
                {
                    typeWhere += string.Format(",'{0}'", item);
                }
                typeWhere = typeWhere.Remove(0, 1);

                sqlWhere += string.Format(" and Sma_type in ({0})", typeWhere);
            }

            //科室代码
            if (!string.IsNullOrEmpty(sampQc.PidDeptCode))
            {
                sqlWhere += string.Format(" and Sma_pat_dept_code='{0}'", sampQc.PidDeptCode);
            }

            //科室名称
            if (!string.IsNullOrEmpty(sampQc.PidDeptName))
            {
                sqlWhere += string.Format(" and Sma_pat_dept_name='{0}'", sampQc.PidDeptName);
            }

            //条码状态
            if (sampQc.ListSampStatusId.Count > 0)
            {
                if (sampQc.ListSampStatusId.Count == 1)
                {
                    sqlWhere += string.Format(" and Sma_status_id='{0}'", sampQc.ListSampStatusId[0]);
                }
                else
                {
                    string statusWhere = string.Empty;
                    foreach (string item in sampQc.ListSampStatusId)
                    {
                        statusWhere += string.Format(",'{0}'", item);
                    }
                    statusWhere = statusWhere.Remove(0, 1);

                    sqlWhere += string.Format(" and Sma_status_id in ({0})", statusWhere);
                }
            }

            //其他 标本流转界面查询专用
            if (!string.IsNullOrEmpty(sampQc.SearchType) && !string.IsNullOrEmpty(sampQc.SearchValue))
            {
                sqlWhere += " AND ";
                if (sampQc.SearchType == "工号")
                {
                    if (sampQc.ListSampStatusId.Contains("2"))//采集界面，用工号去查询，只需要返回采集者是这个工号
                    {
                        sqlWhere += string.Format("  ( Sma_collection_user_id = '{0}') ", sampQc.SearchValue);
                    }
                    else
                    {
                        sqlWhere += string.Format(@"  ( Sma_print_user_id = '{0}' OR  
                                                   Sma_reach_user_id = '{0}' OR 
                                                   Sma_receiver_user_id = '{0}' OR 
                                                   Sma_user_id = '{0}' OR 
                                                   Sma_collection_user_id = '{0}') ",
                                                   sampQc.SearchValue);
                    }
                }
                else if (sampQc.SearchType == "姓名")
                {
                    sqlWhere += string.Format(@"  (Sma_pat_name LIKE '%{0}%' OR 
                                              Sma_print_user_name LIKE '%{0}%' OR  
                                              Sma_reach_user_name LIKE '%{0}%' OR 
                                              Sma_receiver_user_name LIKE '%{0}%' OR 
                                              Sma_user_name LIKE '%{0}%' OR 
                                              Sma_collection_user_name LIKE '%{0}%') ",
                                              sampQc.SearchValue);
                }
                else if (sampQc.SearchType == "病人ID")
                {
                    sqlWhere += string.Format(" (Sma_pat_in_no ='{0}' or Sma_pat_exam_company='{0}' or Sma_social_no='{0}')", sampQc.SearchValue);
                }
                else if (sampQc.SearchType == "批号")
                {
                    sqlWhere += string.Format(" (Sma_pack_no ='{0}')", sampQc.SearchValue);
                }
                else if (sampQc.SearchType == "条码号")
                {
                    sqlWhere += string.Format(" (Sma_bar_code ='{0}')", sampQc.SearchValue);
                }
                else if (sampQc.SearchType == "送达者")
                {
                    sqlWhere += string.Format(" (Sma_reach_user_name ='{0}')", sampQc.SearchValue);
                }
            }

            //姓名
            if (!string.IsNullOrEmpty(sampQc.PidName))
            {
                //全模糊
                if (sampQc.matchType == MatchType.QUANMOHU)
                {
                    sqlWhere += " and  Sample_main.Sma_pat_name like '%" + sampQc.PidName + "%'";
                }
                //半模糊
                if (sampQc.matchType == MatchType.BANMOHU)
                {
                    sqlWhere += " and  Sample_main.Sma_pat_name like '" + sampQc.PidName + "%'";
                }
                //全匹配
                if (sampQc.matchType == MatchType.QUANPIPEI)
                {
                    sqlWhere += " and Sample_main.Sma_pat_name = '" + sampQc.PidName + "'";
                }
            }

            //架子号
            if (!string.IsNullOrEmpty(sampQc.RegRackNo))
            {
                sqlWhere += string.Format(@" and Sample_main.Sma_bar_code in
                                           (select samp_bar_code from Samp_main_extend WITH(NOLOCK) 
                                           where Samp_main_extend.bc_rack_no='{0}' group by samp_bar_code)", sampQc.RegRackNo);
            }

            //检验项目
            if (!string.IsNullOrEmpty(sampQc.ComId) &&
                !string.IsNullOrEmpty(sampQc.StartDate) &&
                !string.IsNullOrEmpty(sampQc.EndDate))
            {
                sqlWhere += string.Format(@" and Sample_main.Sma_bar_id in (
                                           select Sdet_Sma_bar_id from Sample_detail where Sdet_date BETWEEN '{1}' AND '{2}' and Sdet_com_id='{0}')",
                                           sampQc.ComId, sampQc.StartDate, sampQc.EndDate);
            }
            //病历号
            if (!string.IsNullOrEmpty(sampQc.PidInNo))
            {
                if (!string.IsNullOrEmpty(sampQc.PidInNoEnd))
                    sqlWhere += string.Format(@" and Sample_main.Sma_pat_in_no >='{0}' and Sample_main.Sma_pat_in_no <='{1}'", sampQc.PidInNo, sampQc.PidInNoEnd);
                else
                    sqlWhere += string.Format(@" and Sample_main.Sma_pat_in_no ='{0}'", sampQc.PidInNo);
            }
            //HIS卡号/医保卡
            if (!string.IsNullOrEmpty(sampQc.PidSocialNo))
            {
                sqlWhere += string.Format(@" and (Sample_main.Sma_social_no ='{0}' or Sma_pat_exam_company='{0}')", sampQc.PidSocialNo);
            }
            //来源
            if (!string.IsNullOrEmpty(sampQc.PidSrcId))
            {
                sqlWhere += string.Format(@" and Sample_main.Sma_pat_src_id ='{0}'", sampQc.PidSrcId);
            }
            //床号
            if (!string.IsNullOrEmpty(sampQc.PidBedNo))
            {
                sqlWhere += string.Format(@" and Sample_main.Sma_pat_bed_no ='{0}'", sampQc.PidBedNo);
            }

            //手工条码
            if (!string.IsNullOrEmpty(sampQc.SampInfo))
            {
                sqlWhere += string.Format(@" and Sample_main.Sma_info ='{0}'", sampQc.SampInfo);
            }

            if (!sampQc.SearchDeleteSampMain)
            {
                sqlWhere += string.Format(@" and Sample_main.del_flag <>'1'");
            }
            //包号
            if (!string.IsNullOrEmpty(sampQc.PidUniqueId))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_pat_unique_id='{0}'", sampQc.PidUniqueId);
            }
            //医院ID
            if (!string.IsNullOrEmpty(sampQc.HospitalId))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_Dorg_id='{0}'", sampQc.HospitalId);
            }
            if(!string.IsNullOrEmpty(sampQc.CollectionUserID))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_collection_user_id='{0}'", sampQc.CollectionUserID);
            }
            if (!string.IsNullOrEmpty(sampQc.ReachUserID))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_reach_user_id='{0}'", sampQc.ReachUserID);
            }
            if (!string.IsNullOrEmpty(sampQc.SendUserID))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_user_id='{0}'", sampQc.SendUserID);
            }
            if (!string.IsNullOrEmpty(sampQc.ReceivedUserID))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_receiver_user_id='{0}'", sampQc.ReceivedUserID);
            }

            if (sampQc.LeftJoinSampleDetail && !string.IsNullOrEmpty(sampQc.ItrId))
            {
                string selectField = "";
                string leftJoinField = @"
                INNER JOIN (select distinct Sdet_Sma_bar_id
				from Sample_detail
				LEFT OUTER JOIN Rel_itr_combine on Rel_itr_combine.Ric_Dcom_id = Sample_detail.Sdet_com_id
				INNER JOIN Sample_main on Sample_main.Sma_bar_id = Sample_detail.Sdet_Sma_bar_id 
                LEFT OUTER JOIN Dict_profession ON Sample_main.Sma_type = Dict_profession.Dpro_id 
                LEFT OUTER JOIN Dict_source on Sample_main.Sma_pat_src_id = Dict_source.Dsorc_id 
                LEFT OUTER JOIN Dict_test_tube ON Sample_main.Sma_tub_code = Dict_test_tube.Dtub_code  
                LEFT OUTER JOIN Dict_sample ON Sample_main.Sma_Dsam_id = Dict_sample.Dsam_id 
                LEFT OUTER JOIN Dict_sample_remark ON Sample_main.Sma_rem_id = Dict_sample_remark.Dsamr_id  
                LEFT OUTER JOIN Dict_dept ON Sample_main.Sma_pat_dept_code=Dict_dept.Ddept_code
				where 1=1 {0}) fliter on fliter.Sdet_Sma_bar_id = Sample_main.Sma_bar_id ";

                if (!string.IsNullOrEmpty(sampQc.ItrId))
                {
                    string tempWhere = string.Format(@" and Rel_itr_combine.Ric_Ditr_id='{0}' ", sampQc.ItrId);
                    leftJoinField = string.Format(leftJoinField, sqlWhere + tempWhere);
                }

                strSql = string.Format(strSql, selectField, leftJoinField);
            }
            else
            {
                strSql = string.Format(strSql, "", "");
            }

            if (!string.IsNullOrEmpty(sampQc.PidIdentityCard))
            {
                sqlWhere += string.Format(@"and Sample_main.Sma_identity_card ='{0}'", sampQc.PidIdentityCard);
            }
            if(!string.IsNullOrEmpty(sampQc.SampYhsBarCode))
            {
                sqlWhere += string.Format(@"and Sample_main.sma_yhs_BarCode ='{0}'", sampQc.SampYhsBarCode);
            }


            string sqlMain = strSql + sqlWhere;
            DBManager helper = GetDbManager();
            DataTable dt = helper.ExecuteDtSql(sqlMain);
            //Lib.LogManager.Logger.LogInfo("sqlMain  :" + sqlMain);
            list = EntityManager<EntitySampMain>.ConvertToList(dt);
            return list;
        }

        /// <summary>
        /// 简单统计条码工作量
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        public List<EntitySampMain> SimpleStatisticSamp(EntitySampQC sampCondition)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();
            string strSql = @"select * from Sample_main  with(nolock) where 1 = 1 ";

            //时间范围
            if (!string.IsNullOrEmpty(sampCondition.StartDate) && !string.IsNullOrEmpty(sampCondition.EndDate))
            {
                if (sampCondition.SerchDateStatus == "2")//采集
                {
                    strSql += string.Format("and Sma_collection_date BETWEEN '{0}' AND '{1}'", sampCondition.StartDate, sampCondition.EndDate);
                }
                else if (sampCondition.SerchDateStatus == "3")//送检
                {
                    strSql += string.Format("and Sma_send_date BETWEEN '{0}' AND '{1}'", sampCondition.StartDate, sampCondition.EndDate);
                }
                else if (sampCondition.SerchDateStatus == "4")//送达
                {
                    strSql += string.Format("and Sma_reach_date BETWEEN '{0}' AND '{1}'", sampCondition.StartDate, sampCondition.EndDate);
                }
                else if (sampCondition.SerchDateStatus == "5")//签收
                {
                    strSql += string.Format("and Sma_receiver_date BETWEEN '{0}' AND '{1}'", sampCondition.StartDate, sampCondition.EndDate);
                }
            }

            if (!string.IsNullOrEmpty(sampCondition.CollectionUserID))
            {
                strSql += string.Format(@"and Sample_main.Sma_collection_user_id='{0}'", sampCondition.CollectionUserID);
            }
            if (!string.IsNullOrEmpty(sampCondition.ReachUserID))
            {
                strSql += string.Format(@"and Sample_main.Sma_reach_user_id='{0}'", sampCondition.ReachUserID);
            }
            if (!string.IsNullOrEmpty(sampCondition.SendUserID))
            {
                strSql += string.Format(@"and Sample_main.Sma_user_id='{0}'", sampCondition.SendUserID);
            }
            if (!string.IsNullOrEmpty(sampCondition.ReceivedUserID))
            {
                strSql += string.Format(@"and Sample_main.Sma_receiver_user_id='{0}'", sampCondition.ReceivedUserID);
            }
            DBManager helper = GetDbManager();
            DataTable dt = helper.ExecuteDtSql(strSql);
            //Lib.LogManager.Logger.LogInfo("sql:" + strSql);

            list = EntityManager<EntitySampMain>.ConvertToList(dt);
            return list;
        }

        public bool CreateSampMain(List<EntitySampMain> listSampMain)
        {
            //Stopwatch sw = new Stopwatch();

            //sw.Start();

            bool result = false;

            DBManager helper = new DBManager();

            IDbTransaction trans = helper.BeginTrans();

            PropertyInfo[] propertys = listSampMain[0].GetType().GetProperties();

            List<String> listSql = new List<string>();

            DaoSampDetail daoDetail = new DaoSampDetail();

            DaoSampProcessDetail daoProcess = new DaoSampProcessDetail();

            try
            {
                foreach (EntitySampMain sampMain in listSampMain)
                {
                    listSql.Add(helper.ConverToDBSQL(sampMain, propertys, "Sample_main"));

                    listSql.AddRange(daoDetail.CreateSampDetail(sampMain.ListSampDetail, helper));

                    listSql.AddRange(daoProcess.BatchSaveSampProcessDetail(sampMain.ListSampProcessDetail, helper));

                }

                //记录一次生成的条码
                //StringBuilder strSqlInfo = new StringBuilder();
                //foreach (string sql in listSql)
                //{
                //    strSqlInfo.AppendLine(sql);
                //}
                //Lib.LogManager.Logger.LogInfo(strSqlInfo.ToString());

                helper.ExecSql(listSql.ToArray());
                trans.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                trans.Rollback();
                Lib.LogManager.Logger.LogException(ex);
            }

            //Lib.LogManager.Logger.LogInfo("CreateSampMain:" + sw.ElapsedMilliseconds.ToString());
            //sw.Stop();
            return result;
        }


        /// <summary>
        /// 调用粤核酸接口生成条码信息
        /// </summary>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        public bool CreateSampMainByYSS(List<EntitySampMain> listSampMain)
        {
            //Stopwatch sw = new Stopwatch();

            //sw.Start();

            bool result = false;

            DBManager helper = new DBManager();

            IDbTransaction trans = helper.BeginTrans();

            PropertyInfo[] propertys = listSampMain[0].GetType().GetProperties();

            List<String> listSql = new List<string>();

            DaoSampDetail daoDetail = new DaoSampDetail();

            DaoSampProcessDetail daoProcess = new DaoSampProcessDetail();

            try
            {
                foreach (EntitySampMain sampMain in listSampMain)
                {
                    listSql.Add(helper.ConverToDBSQL(sampMain, propertys, "Sample_main"));

                    listSql.AddRange(daoDetail.CreateSampDetail(sampMain.ListSampDetail, helper));

                    listSql.AddRange(daoProcess.BatchSaveSampProcessDetail(sampMain.ListSampProcessDetail, helper));
                }

                //记录一次生成的条码
                //StringBuilder strSqlInfo = new StringBuilder();
                //foreach (string sql in listSql)
                //{
                //    strSqlInfo.AppendLine(sql);
                //}
                //Lib.LogManager.Logger.LogInfo(strSqlInfo.ToString());

                helper.ExecSql(listSql.ToArray());
                trans.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                trans.Rollback();
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }


        public Boolean DeleteSampMain(String sampBarId)
        {
            bool result = true;
            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "update Sample_main set del_flag='1' where Sma_bar_id=@samp_bar_id";

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

        public Boolean UpdateSampMainStatus(EntitySampOperation operation, List<EntitySampMain> listSampMain)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                string strUpdateValue = "Sma_lastoper_date = '{0}',Sma_status_id = '{1}',Sma_lastoper_Buser_id = '{2}'";

                switch (operation.OperationStatus)
                {
                    case "1"://打印条码
                        strUpdateValue += ",Sma_print_flag = 1, Sma_print_date = '{0}' ,Sma_print_user_id = '{2}',Sma_print_user_name = '{3}',Sma_status_name='已打印'";
                        break;
                    case "2"://采集
                        strUpdateValue += ",Sma_collection_flag = 1, Sma_collection_date = '{0}' ,Sma_collection_user_id = '{2}',Sma_collection_user_name = '{3}',Sma_status_name='已采集'";
                        break;
                    case "3"://收取
                        strUpdateValue += ",Sma_send_flag = 1, Sma_send_date = '{0}' ,Sma_user_id = '{2}',Sma_user_name = '{3}',Sma_status_name='已收取'";
                        break;
                    case "4"://送达
                        strUpdateValue += ",Sma_reach_flag = 1, Sma_reach_date = '{0}' ,Sma_reach_user_id = '{2}',Sma_reach_user_name = '{3}',Sma_status_name='已送达'";
                        break;
                    case "5"://签收
                        strUpdateValue += ",Sma_receiver_flag = 1, Sma_receiver_date = '{0}' ,Sma_receiver_user_id = '{2}',Sma_receiver_user_name = '{3}',Sma_status_name='已签收'";
                        break;
                    case "8"://二次送检
                        strUpdateValue += ",Sma_receiver_flag = 0, Sma_reach_flag = 0, Sma_send_flag = 0, Sma_second_receive_flag = {1},Sma_status_name='二次送检'";
                        break;
                    case "9"://回退
                        strUpdateValue += @",Sma_collection_flag = 0,Sma_collection_date = null,
                                             Sma_send_flag = 0,Sma_send_date = null,
                                             Sma_reach_flag = 0,Sma_reach_date = null,
                                             Sma_receiver_flag = 0,Sma_receiver_date = null,
                                             samp_return_times=isnull(samp_return_times,0)+1,Sma_status_name='已回退'";
                        break;
                    default:
                        break;
                }

                string strUpdateWhere = string.Empty;

                foreach (EntitySampMain item in listSampMain)
                {
                    strUpdateWhere += string.Format(",'{0}'", item.SampBarId);
                }

                strUpdateWhere = strUpdateWhere.Remove(0, 1);

                string strUpdateSql = string.Format("update Sample_main set {0} where Sma_bar_id in ({1})", strUpdateValue, strUpdateWhere);


                strUpdateSql = string.Format(strUpdateSql,
                                             operation.OperationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                             operation.OperationStatus,
                                             operation.OperationID,
                                             operation.OperationName);

                result = helper.ExecCommand(strUpdateSql) > 0;

                //二次签收需要把以前所有状态都置上
                if (operation.OperationStatus == "5")
                {
                    strUpdateWhere = string.Empty;

                    foreach (EntitySampMain item in listSampMain)
                    {
                        if (item.SecondReceiveFlag == 1)
                        {
                            strUpdateWhere += string.Format(",'{0}'", item.SampBarId);
                        }
                    }

                    if (strUpdateWhere != string.Empty)
                    {
                        strUpdateWhere = strUpdateWhere.Remove(0, 1);

                        string sql = string.Format(@"update Sample_main set Sma_receiver_flag=1,Sma_reach_flag=1,Sma_send_flag=1 where Sma_bar_code in ({0})", strUpdateWhere);
                        helper.ExecCommand(sql);
                    }
                }


            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public Boolean UpdateSampMainBatchNo(Int64 batchNo, List<EntitySampMain> listSampMain)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateWhere = string.Empty;

                foreach (EntitySampMain item in listSampMain)
                {
                    strUpdateWhere += string.Format(",'{0}'", item.SampBarId);
                }

                strUpdateWhere = strUpdateWhere.Remove(0, 1);

                string strUpdateSql = string.Format("update Sample_main set Sma_bar_batch_no={0} where Sma_bar_id in ({1})", batchNo, strUpdateWhere);

                return helper.ExecCommand(strUpdateSql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public Boolean UpdateSampMainUrgentFlag(Boolean urgent, String sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "UPDATE Sample_main SET Sma_urgent_flag=@samp_urgent_flag WHERE Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_urgent_flag", urgent ? 1 : 0));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;

        }

        public Boolean UpdateSampMainRemark(String remarkId, String remarkName, String sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "update Sample_main set  Sma_rem_id=@samp_rem_id, Sma_remark=@samp_remark where Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));
                paramHt.Add(new SqlParameter("@samp_rem_id", remarkId));
                paramHt.Add(new SqlParameter("@samp_remark", remarkName));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public Boolean UpdateSampMainComName(String comName, String sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "UPDATE Sample_main SET Sma_com_name=@bc_his_name WHERE Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@bc_his_name", comName));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public bool Returned(string barCode)
        {
            bool result = false;
            try
            {
                string sql = @"select Sma_status_id from Sample_main where Sma_bar_id = @barcode";
                DBManager helper = GetDbManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@barcode", barCode));
                DataTable dt = helper.ExecuteDtSql(sql, paramHt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Sma_status_id"].ToString() == "9")
                    {
                        result = true;
                    }
                    else
                        result = false;
                }
                else
                    result = false;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public EntitySampMain GetSampMainSampleInfo(string sampBarId)
        {
            EntitySampMain sampMain = new EntitySampMain();

            try
            {
                string strSql = string.Format(@"SELECT Sample_main.*,com_info.Dcom_his_codes
FROM Sample_main  WITH(NOLOCK) 
LEFT JOIN (
SELECT  A.Sdet_Sma_bar_id,
        STUFF(( SELECT  ',' + Dict_itm_combine.Dcom_his_code
                FROM    Sample_detail
                LEFT JOIN Dict_itm_combine ON Dict_itm_combine.Dcom_id = Sample_detail.Sdet_com_id
                WHERE   Sample_detail.Sdet_Sma_bar_id = A.Sdet_Sma_bar_id
              FOR
                XML PATH('')
              ), 1, 1, '') AS Dcom_his_codes
FROM    Sample_detail AS A
GROUP BY A.Sdet_Sma_bar_id) AS com_info 
ON com_info.Sdet_Sma_bar_id = Sample_main.Sma_bar_id
WHERE Sma_bar_id = '{0}'", sampBarId);

                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntitySampMain> list = EntityManager<EntitySampMain>.ConvertToList(dt);

                if (list != null && list.Count > 0)
                {
                    sampMain = list[0];
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return sampMain;
        }

        public bool UpdateSampMainSampleInfo(string sampleId, string sampleName, string sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "UPDATE Sample_main SET Sma_Dsam_id=@samp_sam_id ,Sma_Dsam_name=@samp_sam_name WHERE Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_sam_id", sampleId));
                paramHt.Add(new SqlParameter("@samp_sam_name", sampleName));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public bool UpdateSampMainNameAndSex(string name, string sex, string sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "UPDATE Sample_main SET Sma_pat_name=@pid_name ,Sma_pat_sex=@pid_sex WHERE Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@pid_name", name));
                paramHt.Add(new SqlParameter("@pid_sex", sex));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public bool UpdateSampMainOtherInfo(EntitySampMain sampMain, string sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = @"UPDATE Sample_main SET Sma_identity_name=@Sma_identity_name ,Sma_identity_card=@Sma_identity_card,
                                       Sma_pat_type = @Sma_pat_type, Sma_pat_address = @Sma_pat_address WHERE Sma_bar_id =@samp_bar_id ";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@Sma_identity_name", sampMain.PidIdentityName));
                paramHt.Add(new SqlParameter("@Sma_identity_card", sampMain.PidIdentityCard));
                paramHt.Add(new SqlParameter("@Sma_pat_type", sampMain.SampPatType));
                paramHt.Add(new SqlParameter("@Sma_pat_address", sampMain.PidAddress));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));
               
                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }


        public bool UpdateSampMainYHSBarCode(EntitySampMain sampMain, string sampBarId)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = @"UPDATE Sample_main SET Samp_yhs_BarCode= @Samp_yhs_BarCode WHERE Sma_bar_id =@samp_bar_id ";

                List<DbParameter> paramHt = new List<DbParameter>();

                paramHt.Add(new SqlParameter("@Samp_yhs_BarCode", sampMain.SampYhsBarCode));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                result = helper.ExecCommand(strUpdateSql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public Boolean ExistSampMain(String sampBarId)
        {
            bool result = false;

            try
            {
                string sql = @"select top 1 1 from Sample_main where Sma_bar_id=@samp_bar_id and del_flag='0'";
                DBManager helper = GetDbManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));
                DataTable dt = helper.ExecuteDtSql(sql, paramHt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool UpdateSampMainBarCode(string sampBarId, string sampPlaceCode)
        {
            bool result = false;

            DBManager helper = GetDbManager();
            IDbTransaction trans = helper.BeginTrans();
            try
            {
                helper.ExecSql(string.Format("update Sample_main set Sma_bar_id='{0}',Sma_bar_code='{0}' where Sma_bar_id='{1}'", sampPlaceCode, sampBarId));
                helper.ExecSql(string.Format("update Sample_detail set Sdet_Sma_bar_id='{0}',Sdet_bar_code='{0}' where Sdet_Sma_bar_id='{1}'", sampPlaceCode, sampBarId));
                helper.ExecSql(string.Format("update Sample_process_detial set Sproc_Sma_bar_id='{0}',Sproc_Sma_bar_code='{0}' where Sproc_Sma_bar_id='{1}'", sampPlaceCode, sampBarId));

                trans.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        /// <summary>
        /// 重置预置条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UndoSampMain(String sampBarId)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            try
            {
                string strSql = string.Format("update Sample_main set Sma_bar_code='',Sma_status_id = '0',Sma_print_flag = 0 where Sma_bar_id='{0}' and del_flag='0'", sampBarId);
                result = helper.ExecCommand(strSql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool UpdateSampReturnFlag(EntitySampMain sampMain)
        {
            bool result = false;

            DBManager helper = GetDbManager();
            try
            {
                string sql = "update Sample_main set Sma_return_flag=@sampFlag where Sma_id=@sampSn and del_flag=0";
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@sampFlag", sampMain.SampReturnFlag));
                paramHt.Add(new SqlParameter("@sampSn", sampMain.SampSn));
                result = helper.ExecCommand(sql, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
        public List<EntitySampMain> GetPatientsInfoByBcInNo(string pidInNo, string colName)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();
            try
            {
                string sql = string.Format(@"select top 1 * from Sample_main where Sma_pat_in_no ='{0}' ", pidInNo);

                if (!string.IsNullOrEmpty(colName))
                {
                    if (colName == "Sma_pat_in_no or Sma_social_no")
                    {
                        if (string.IsNullOrEmpty(pidInNo)) pidInNo = "";
                        sql = "select top 1 * from Sample_main where (Sma_social_no='" + pidInNo + "' or Sma_pat_in_no= ?) ";
                    }
                    else
                    {
                        sql = "select top 1 * from Sample_main where " + colName + "= ? ";
                    }
                }
                DBManager helper = GetDbManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntitySampMain>.ConvertToList(dt).OrderBy(i => i.SampDate).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool UpdateBarcodeBale(string pidUniqueId, List<string> listCode)
        {
            bool result = false;
            DBManager helper = GetDbManager();
            try
            {
                string strCode = string.Empty;
                foreach (string code in listCode)
                {
                    strCode += string.Format(",'{0}'", code);
                }
                strCode = strCode.Remove(0, 1);
                string sql = string.Format("update Sample_main set Sma_pat_unique_id='{0}' where Sma_bar_code in ({1})", pidUniqueId, strCode);
                result = helper.ExecCommand(sql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateSampBarType(string sampBarId, int samp_bar_type)
        {
            bool isUpdate = false;
            DBManager helper = GetDbManager();
            if (!string.IsNullOrEmpty(sampBarId))
            {
                try
                {
                    string sqlStr = string.Format(@"update Sample_main set Sma_bar_type={1} where Sma_bar_id='{0}' ", sampBarId, samp_bar_type);
                    isUpdate = helper.ExecCommand(sqlStr) > 0;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isUpdate;
        }

        public List<string> GetPackCount(string sampStatus, string deptCode)
        {
            List<string> list = new List<string>();
            try
            {
                string sqlWhere = "and Sma_collection_date>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and Sma_collection_date<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
                if (string.IsNullOrEmpty(deptCode))
                {
                    sqlWhere += " and (Sma_pat_dept_code='' or Sma_pat_dept_code is null)";
                }
                else
                    sqlWhere += " and Sma_pat_dept_code='" + deptCode + "'";
                string bc_status2 = string.Empty;
                if (sampStatus == "3")
                    bc_status2 = Convert.ToString(Convert.ToInt16(sampStatus) - 1);
                else
                    bc_status2 = Convert.ToString(Convert.ToInt16(sampStatus) - 2);
                string sql = string.Format(@"select
(SELECT count(distinct Sma_pat_unique_id) from Sample_main where 
Sma_pat_unique_id is not null and Sma_pat_unique_id<>'' and Sma_status_id={0} {1}) as trCount,
(SELECT count(distinct Sma_pat_unique_id) from Sample_main(nolock) where 
Sma_pat_unique_id is not null and Sma_pat_unique_id<>'' and Sma_status_id >= {2} and Sma_status_id<{0} {1}) as allCount", sampStatus, sqlWhere, bc_status2);
                DBManager helper = GetDbManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        list.Add(dt.Rows[0][i].ToString());
                    }
                }
                 
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        /// <summary>
        /// 获取收费失败的条码
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<EntitySampMain> GetFaultChargeSamp(EntitySampQC qc)
        {
            string sql = @"select Sample_Main.*,
sys_interface_log.operation_content Dsamr_id
from 
sys_interface_log(nolock)
left outer join Sample_Main(nolock) on Sample_Main.Sma_bar_code=sys_interface_log.samp_bar_id
where operation_name like '%收费操作%' and operation_success=0
and samp_bar_id not in(
select samp_bar_id from sys_interface_log where operation_name like '%收费操作%'
and operation_success=1
) ";
            if (qc.ListSampBarId?.Count>0 && !string.IsNullOrEmpty(qc.ListSampBarId[0]))
            {
                sql += string.Format(" and Sample_Main.Sma_bar_code='{0}'", qc.ListSampBarId[0]);
            }
            else
            {
                sql += string.Format(" and Sample_Main.Sma_date>='{0}'", qc.StartDate);
                sql += string.Format(" and Sample_Main.Sma_date<='{0}'", qc.EndDate);
                if (!string.IsNullOrEmpty(qc.PidDeptCode))
                {
                    sql += string.Format(" and Sample_Main.Sma_pat_dept_code='{0}'", qc.PidDeptCode);
                }
            }

            try
            {
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecSel(sql);
                return EntityManager<EntitySampMain>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySampMain>();
            }
        }


        public string GetNewBarcode()
        {
            try
            {
                string sql = @" update Base_barcode_generator set Bbargen_no = Bbargen_no +1 OUTPUT Inserted.Bbargen_no where Bbargen_id = 1";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecSel(sql);
                if (dt == null || dt.Rows.Count <= 0)
                    return string.Empty;

                return dt.Rows[0]["Bbargen_no"].ToString();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }
        public string GetOldBarcode(int id)
        {
            try
            {
                string sql = $" select Bbargen_no from Base_barcode_generator where Bbargen_id = {id}";
                List<DbParameter> paramHt = new List<DbParameter>();
                DBManager helper = new DBManager();
                object obj = helper.SelOne(sql, paramHt);

                return obj.ToString();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }
        public string UpdateNewBarcode(string barcode, int id)
        {
            try
            {
                string sql = string.Empty;
                if (string.IsNullOrEmpty(barcode))
                {
                    sql = string.Format(" update Base_barcode_generator set Bbargen_no = Bbargen_no +1 OUTPUT Inserted.Bbargen_no where Bbargen_id = {0}", id);
                }
                else
                {
                    sql = string.Format(" update Base_barcode_generator set Bbargen_no = {0} OUTPUT Inserted.Bbargen_no where Bbargen_id = {1}", barcode, id);
                }
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecSel(sql);
                if (dt == null || dt.Rows.Count <= 0)
                    return string.Empty;

                return dt.Rows[0]["Bbargen_no"].ToString();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }
    }
}
