using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using dcl.svr.framedic;
using dcl.svr.cache;
using lis.dto;
using dcl.root.dac;
using lis.dto.Entity;
using dcl.root.logon;
using lis.dto.FieldsCorrespandance;
using dcl.common;
using dcl.svr.result.CRUD;

using dcl.pub.entities;
using Lib.DAC;
using System.Threading;
using dcl.root.dto;
using dcl.entity;

namespace dcl.svr.result
{
    public class PatUpdateBLL
    {

        /// <summary>
        /// 更新描述报告
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatDescResult(EntityRemoteCallClientInfo userInfo, DataSet dsData)
        {
            EntityOperationResult result = new EntityOperationResult();//.GetNew("更新病人、描述结果信息");
            result.ReturnResult = dsData;

            //现病人基本资料信息
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];

            string pat_id = dtPatInfo.Rows[0]["pat_id"].ToString();
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            //现组合信息
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];

            new PatInsertBLL().UpdatePatCName(dtPatInfo, dtPatCombine);

            //现描述结果信息
            DataTable dtPatDescResult = dsData.Tables[PatientTable.PatientDescResultTableName];


            PatReadBLL bll = new PatReadBLL();
            //原病人基本资料信息
            DataTable dtOriginPatInfo = bll.GetPatientInfo(pat_id);

            //原组合信息
            DataTable dtOriginPatCombine = bll.GetPatientCombine(pat_id);

            //原描述结果信息
            // DataTable dtOriginPatResult = bll.GetPatDescResult(pat_id);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    //更新病人基本信息
                    UpdatePatientInfo(dtPatInfo, dtOriginPatInfo, result, helper, opLogger);

                    //条码号
                    string pat_bar_code = null;
                    if (!common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
                    {
                        pat_bar_code = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                    }

                    //更新组合
                    UpdatePatientCombine(dtPatCombine, dtOriginPatCombine, pat_bar_code, result, helper, opLogger);

                    //更新描述内容
                    UpdateDescResult(dtPatDescResult, result, helper, opLogger);

                    opLogger.Log();

                    if (opLogger.logs.Count > 0)
                    {

                        //string sqlUpdate = string.Format(@"
                        //update patients set pat_modified_times = case when pat_modified_times is null then 1
                        //                                              else pat_modified_times + 1 end
                        //where pat_id = '{0}'", pat_id);

                        //helper.ExecuteNonQuery(sqlUpdate);
                    }

                    if (result.Success)
                    {
                        helper.Commit();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "UpdatePatDescResult", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }

        /// <summary>
        /// 更新普通报告
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatCommonResult(EntityRemoteCallClientInfo userInfo, DataSet dsData)
        {
            //创建操作返回信息
            EntityOperationResult result = new EntityOperationResult();//.GetNew("更新病人、结果信息");
            result.ReturnResult = dsData;

            //现病人基本信息
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];

            string pat_id = dtPatInfo.Rows[0]["pat_id"].ToString();

            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", pat_id, dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }

            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            //现组合
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];

            new PatInsertBLL().UpdatePatCName(dtPatInfo, dtPatCombine);

            //现结果
            DataTable dtPatResult = dsData.Tables[PatientTable.PatientResultTableName];

            //有权限操作保存结果的则可以不对比历史结果
            //if (!Convert.ToBoolean(userInfo.OperationName))
            //{
            //    #region 对比历史结果

            //    dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare checkHistoryRes = new dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare(null, null, EnumOperationCode.Unspecified, null);
            //    if (!checkHistoryRes.Savecheck(dtPatInfo, dtPatResult, ref result))
            //    {
            //        return result;
            //    }

            //    #endregion
            //}

            PatReadBLL bll = new PatReadBLL();

            //原病人基本资料信息
            DataTable dtOriginPatInfo = bll.GetPatientInfo(pat_id);

            if (dtOriginPatInfo.Rows.Count > 0 && dtOriginPatInfo.Rows[0]["pat_flag"] != null &&
                dtOriginPatInfo.Rows[0]["pat_flag"].ToString() == "2")
            {
                result.AddMessage(EnumOperationErrorCode.Exception, "该记录已报告", EnumOperationErrorLevel.Error);
                return result;
            }

            //原组合信息
            DataTable dtOriginPatCombine = bll.GetPatientCombine(pat_id);

            //原病人结果
            DataTable dtOriginPatResult = bll.GetPatientCommonResult(pat_id, false);

            //获取病人组合条码号**********************************************************
            string pat_bar_code = null;
            if (!common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                pat_bar_code = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }

            //****************************************************************

            try
            {
                using (DBHelper transHelper = DBHelper.BeginTransaction())
                {
                    //更新病人基本信息
                    UpdatePatientInfo(dtPatInfo, dtOriginPatInfo, result, transHelper, opLogger);

                    //更新组合
                    UpdatePatientCombine(dtPatCombine, dtOriginPatCombine, pat_bar_code, result, transHelper, opLogger);

                    //更新结果
                    UpdatePatientResult(dtPatResult, dtOriginPatResult, result, transHelper, opLogger);

                    if (opLogger.logs.Count > 0)
                    {
                        string patID = pat_id;
                        if (changePatID)
                        {
                            patID = newPatID;
                            dtPatInfo.Rows[0]["pat_id"] = newPatID;
                        }
                    }

                    if (result.Success)
                    {
                        transHelper.Commit();
                    }

                    opLogger.Log();
                }

                //************************************************************************************
                //如果原组合与现在的组合比对少了的组合一律将bc_flag置为0
                if (!string.IsNullOrEmpty(pat_bar_code))
                {
                    string sqlUpdateBcFlag = string.Format(@"
update bc_cname set bc_flag = 0 where bc_bar_code = '{0}';
update bc_cname set bc_flag = 1 where bc_id in (
select bc_cname.bc_id
from
patients_mi with(nolock)
inner join patients with(nolock) on patients.pat_id = patients_mi.pat_id
inner join bc_cname with(nolock) on bc_cname.bc_bar_code = patients.pat_bar_code and patients_mi.pat_com_id = bc_cname.bc_lis_code
where patients.pat_bar_code = '{0}')
", pat_bar_code);

                    Thread th = new Thread(UpdateBCFlag);
                    th.Start(sqlUpdateBcFlag);
                   

                  
                }

                //*************************************************************************************

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "UpdatePatCommonResult", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }

        private void UpdateBCFlag(object sqlUpdateBcFlag)
        {
            SqlHelper helper = new SqlHelper();
            helper.ExecuteNonQuery(sqlUpdateBcFlag.ToString());
        }


        /// <summary>
        /// 更新普通报告(新生儿)
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatCommonResultForBf(EntityRemoteCallClientInfo userInfo, DataSet dsData)
        {
            return null;
        }

        /// <summary>
        /// 更新病人组合
        /// </summary>
        /// <param name="dtPatCombine"></param>
        /// <param name="result"></param>
        /// <param name="helper"></param>
        private void UpdatePatientCombine(DataTable dtPatCombine, DataTable dtOriginPatCombine, string barcode, EntityOperationResult result, DBHelper transHelper, OperationLogger logger)
        {
            if (result.Success)
            {
                //样本号
                string pat_sid = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();

                //病人ID
                string pat_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();

                //根据PatID改变,并转移组合信息
                UpdatePatCombineByPatIDchaged(dtPatCombine, dtOriginPatCombine, pat_id, result, transHelper, logger);

                StringBuilder sbCom_id = new StringBuilder();
                bool needComma = false;
                //更新病人ID,并于原组合比较
                foreach (DataRow drCurrentCombine in dtPatCombine.Rows)
                {
                    //当前组合ID
                    string curr_pat_com_id = drCurrentCombine["pat_com_id"].ToString();

                    drCurrentCombine["pat_id"] = pat_id;

                    if (needComma)
                    {
                        sbCom_id.Append(",");
                    }

                    sbCom_id.Append(string.Format(" '{0}' ", drCurrentCombine["pat_com_id"].ToString()));
                    needComma = true;

                    //在原有组合内查找是否存在当前组合ID
                    //不存在,则当前组合ID为新增的组合
                    if (dtOriginPatCombine.Select(string.Format("pat_com_id='{0}'", curr_pat_com_id)).Length == 0)
                    {
                        logger.Add_AddLog(SysOperationLogGroup.PAT_COMBINE, curr_pat_com_id, string.Empty);
                    }
                }

                foreach (DataRow drOriginCombine in dtOriginPatCombine.Rows)
                {
                    //原有组合ID
                    string origin_pat_com_id = drOriginCombine["pat_com_id"].ToString();

                    //在当前组合内查找是否存在原有组合ID
                    //不存在,则当前组合ID为已删除的组合
                    if (dtPatCombine.Select(string.Format("pat_com_id='{0}'", origin_pat_com_id)).Length == 0)
                    {
                        logger.Add_DelLog(SysOperationLogGroup.PAT_COMBINE, origin_pat_com_id, string.Empty);
                    }
                }

                //更新医嘱id
                //如果有使用条码，并根据配置是否更新patients_mi.pat_yz_id为bc_cname.bc_yz_id
                if (!string.IsNullOrEmpty(barcode)
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AutoFillOrderIdWhenSave") == "是"
                    && sbCom_id.Length > 0
                    )
                {
                    DBHelper helperGetYzID = new DBHelper();

                    //根据条码号与组合id查找出对应的医嘱(一次性取出该条码对应的医嘱)
                    string sqlSelectYzId = string.Format("select bc_yz_id,bc_lis_code from bc_cname with (nolock) where bc_bar_code = '{0}' and bc_lis_code in ({1})", barcode, sbCom_id.ToString());

                    DataTable tableYzId = helperGetYzID.GetTable(sqlSelectYzId);

                    foreach (DataRow rowCombine in dtPatCombine.Rows)
                    {
                        if (common.Compare.IsEmpty(rowCombine["pat_yz_id"]) || rowCombine["pat_yz_id"].ToString().Trim() != string.Empty)
                        {
                            string com_id = rowCombine["pat_com_id"].ToString();

                            DataRow[] drsYZ = tableYzId.Select(string.Format("bc_lis_code = '{0}'", com_id));

                            if (drsYZ.Length > 0 && !common.Compare.IsEmpty(drsYZ[0]["bc_yz_id"]))
                            {
                                rowCombine["pat_yz_id"] = drsYZ[0]["bc_yz_id"];
                            }
                        }
                    }
                }

                //生成SqlCommand
                List<SqlCommand> cmdsPatientMi = DBTableHelper.GenerateInsertCommand(PatientTable.PatientCombineTableName, null, dtPatCombine);

                try
                {

                    //先删除、再添加
                    transHelper.ExecuteNonQuery(string.Format("delete from patients_mi where pat_id = '{0}'", pat_id));
                    foreach (SqlCommand cmd in cmdsPatientMi)
                    {
                        transHelper.ExecuteNonQuery(cmd);
                    }
                }
                catch (Exception ex)
                {
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    Logger.WriteException(this.GetType().Name, "UpdatePatientCombine", ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// 更新病人组合
        /// </summary>
        /// <param name="dtPatCombine"></param>
        /// <param name="result"></param>
        /// <param name="helper"></param>
        private void UpdatePatientCombineForBf(DataTable dtPatCombine, DataTable dtOriginPatCombine, string barcode, EntityOperationResult result, DBHelper transHelper, OperationLogger logger)
        {
            
        }

        /// <summary>
        /// 根据PatID改变,并转移组合信息
        /// </summary>
        /// <param name="dtPatCombine">当前的组合信息</param>
        /// <param name="dtOriginPatCombine">原有的组合信息</param>
        /// <param name="nw_pat_id">新pat_id</param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <param name="logger"></param>
        private void UpdatePatCombineByPatIDchagedForBf(DataTable dtPatCombine, DataTable dtOriginPatCombine, string nw_pat_id, EntityOperationResult result, DBHelper transHelper, OperationLogger logger)
        {
            //如果pat_ID没改变,跳过
        }

        /// <summary>
        /// 根据PatID改变,并转移组合信息
        /// </summary>
        /// <param name="dtPatCombine">当前的组合信息</param>
        /// <param name="dtOriginPatCombine">原有的组合信息</param>
        /// <param name="nw_pat_id">新pat_id</param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <param name="logger"></param>
        private void UpdatePatCombineByPatIDchaged(DataTable dtPatCombine, DataTable dtOriginPatCombine, string nw_pat_id, EntityOperationResult result, DBHelper transHelper, OperationLogger logger)
        {
            //如果pat_ID没改变,跳过
            if (!changePatID) return;

            if (result.Success)
            {
                //原有的组合信息,不为空时才转移
                if (dtOriginPatCombine != null && dtOriginPatCombine.Rows.Count > 0)
                {
                    try
                    {
                        string old_pat_id = dtOriginPatCombine.Rows[0]["pat_id"].ToString();
                        //当前的组合信息,为空时才转移
                        if (dtPatCombine != null && dtPatCombine.Rows.Count <= 0)
                        {
                            //pat_id是否不一样,不一样才继续
                            if (old_pat_id != nw_pat_id && (!string.IsNullOrEmpty(nw_pat_id)))
                            {
                                //把原有的组合信息复制到当前的组合信息里
                                foreach (DataRow drOldCombine in dtOriginPatCombine.Rows)
                                {
                                    dtPatCombine.Rows.Add(drOldCombine.ItemArray);
                                }

                                foreach (DataRow drNewCombine in dtPatCombine.Rows)
                                {
                                    drNewCombine["pat_id"] = nw_pat_id;//更新为新pat_id
                                }

                                #region 临时表

                                DataTable dtTempPat = new DataTable("dtTempPat");
                                dtTempPat.Columns.Add("pat_c_name");
                                string pat_c_name = string.Empty;
                                DataRow drTempPat = dtTempPat.NewRow();
                                drTempPat["pat_c_name"] = pat_c_name;
                                dtTempPat.Rows.Add(drTempPat);

                                #endregion

                                new PatInsertBLL().UpdatePatCName(dtTempPat, dtPatCombine);

                                pat_c_name = dtTempPat.Rows[0]["pat_c_name"].ToString();

                                if (!string.IsNullOrEmpty(pat_c_name))
                                {
                                    //更新病人资料的组合信息
                                    transHelper.ExecuteNonQuery(string.Format("update patients set pat_c_name='{0}' where pat_id ='{1}'", pat_c_name, nw_pat_id));
                                }
                            }
                        }
                        //删除 原有的组合信息
                        transHelper.ExecuteNonQuery(string.Format("delete from patients_mi where pat_id = '{0}'", old_pat_id));
                    }
                    catch (Exception ex)
                    {
                        result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                        Logger.WriteException(this.GetType().Name, "UpdatePatCombineByPatIDchaged", ex.ToString());
                        //throw;
                    }
                }
            }
        }

        bool changePatID = false;

        /// <summary>
        /// 更新病人基本信息
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private void UpdatePatientInfo(DataTable dtPatientsInfo, DataTable dtOriginPatInfo, EntityOperationResult result, DBHelper transHelper, OperationLogger opLogger)
        {
            if (result.Success)
            {
                //new PatCommonBll().AdjustSampleDate(dtPatientsInfo);

                DataRow drPatient = dtPatientsInfo.Rows[0];

                //样本号
                string pat_sid = drPatient["pat_sid"].ToString();

                //仪器ID
                string itr_id = drPatient["pat_itr_id"].ToString();

                //日期
                DateTime pat_date = Convert.ToDateTime(drPatient["pat_date"]);

                //查找之前的样本号
                string sqlSelect = string.Format("select pat_sid from patients where pat_id='{0}'", drPatient["pat_id"].ToString());
                string prevSID = transHelper.ExecuteScalar(sqlSelect).ToString();


                //id
                string pat_id = itr_id + pat_date.ToString("yyyyMMdd") + pat_sid;

                drPatient["pat_id"] = pat_id;

                GeneratePatInfoUpdateLogger(dtPatientsInfo, dtOriginPatInfo, opLogger);




                //原样本号和当前样本号不同
                if (prevSID != drPatient["pat_sid"].ToString())
                {
                    //查找当前样本号是否已存在
                    object objSID = transHelper.ExecuteScalar(string.Format(@"
                        select top 1 pat_sid from 
                        patients with(nolock) where pat_sid='{0}' 
                        and pat_itr_id ='{1}' and pat_date >= '{2}' and pat_date<'{3}'",
                    drPatient["pat_sid"].ToString(),
                    drPatient["pat_itr_id"].ToString(),
                    ((DateTime)drPatient["pat_date"]).Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    ((DateTime)drPatient["pat_date"]).AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss")));

                    if (objSID != null && objSID != DBNull.Value)
                    {
                        //存在,返回错误信息
                        result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        changePatID = true;
                    }
                }

                if (result.Success)
                {
                    try
                    {
                        List<SqlCommand> cmdPatient = DBTableHelper.GenerateUpdateCommand("patients", new string[] { "pat_key" }, new string[] { "pat_modified_times", "pat_flag" }, dtPatientsInfo);
                        transHelper.ExecuteNonQuery(cmdPatient[0]);
                        if (changePatID)
                        {
                            SqlCommand cmdPatient2 = new SqlCommand();
                            newPatID = ResultoHelper.GenerateResID(drPatient["pat_itr_id"].ToString(), DateTime.Parse(drPatient["pat_date"].ToString()), pat_sid);
                            cmdPatient2.CommandText = string.Format("update patients set pat_id = '{0}' where pat_id ='{1}' ", newPatID, pat_id);

                            Logger.WriteException(this.GetType().Name, "UpdatePatientInfo，patID改变，日志记录", cmdPatient2.CommandText);

                            transHelper.ExecuteNonQuery(cmdPatient2);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                        Logger.WriteException(this.GetType().Name, "UpdatePatientInfo", ex.ToString());
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 更新病人基本信息
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private void UpdatePatientInfoForBf(DataTable dtPatientsInfo, DataTable dtOriginPatInfo, EntityOperationResult result, DBHelper transHelper, OperationLogger opLogger)
        {
        }

        string newPatID = "";

        public String CopyPatientsInfo(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            StringBuilder sbPatId = new StringBuilder();
            foreach (string strPatId in pat_id)
            {
                sbPatId.Append(string.Format(",'{0}'", strPatId));
            }

            sbPatId.Remove(0, 1);

            String strSql = string.Format("Select * from patients where pat_id in ({0})", sbPatId.ToString());

            DBHelper helper = new DBHelper();
            DataTable dtPat = helper.GetTable(strSql);
            dtPat.TableName = "patients";
            if (dtPat.Columns.Contains("pat_key"))
                dtPat.Columns.Remove("pat_key");


            String strSqlCombine = string.Format("Select * from patients_mi where pat_id in ({0})", sbPatId.ToString());
            DataTable dtPatMi = helper.GetTable(strSqlCombine);
            dtPatMi.TableName = "patients_mi";

            StringBuilder sbCopyPatId = new StringBuilder();
            ArrayList al = new ArrayList();
            foreach (DataRow drPat in dtPat.Rows)
            {
                string strPatOldId = drPat["pat_id"].ToString();
                string strPatItrOldId = drPat["pat_itr_id"].ToString();
                string strPatSid = drPat["pat_sid"].ToString();
                DateTime patolddate = Convert.ToDateTime(drPat["pat_date"]);
                string strPatId = strItrId + dtTime.ToString("yyyyMMdd") + strPatSid;
                drPat["pat_id"] = strPatId;
                drPat["pat_itr_id"] = strItrId;//更新仪器代码
                drPat["pat_date"] = dtTime;

                //不复制一审与二审的时间与操作者信息
                if (true)
                {
                    drPat["pat_report_date"] = DBNull.Value;
                    drPat["pat_flag"] = 0;
                    drPat["pat_send_flag"] = DBNull.Value;
                    drPat["pat_report_code"] = DBNull.Value;
                    drPat["pat_chk_date"] = DBNull.Value;
                    drPat["pat_chk_code"] = DBNull.Value;
                }

                sbCopyPatId.Append(string.Format(",'{0}'", strPatId));

                foreach (DataRow drPatMi in dtPatMi.Select("pat_id='" + strPatOldId + "'"))
                {
                    drPatMi["pat_id"] = strPatId;
                }
                if (drPat["pat_bar_code"] != null && drPat["pat_bar_code"] != DBNull.Value &&
                     !string.IsNullOrEmpty(drPat["pat_bar_code"].ToString()))
                {
                    string sqlBcSign = string.Format(@"
insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status, pat_id,bc_remark)
values('{0}','{1}',getdate(),'{3}','{4}','20','{5}','{6}')"
                                                     , ""
                                                     , ""
                                                     , ""
                                                     , string.Empty
                                                     , string.Empty
                                                     , strPatId
                                                     ,
                                                     string.Format("复制: 原资料日期：{0} 仪器代码：{1}->{2}",
                                                                   patolddate.ToString("yyyy-MM-dd"), strPatItrOldId,
                                                                   strItrId)
                        );

                    al.Add(sqlBcSign);
                }
            }

            sbCopyPatId.Remove(0, 1);

            String strCopySql = string.Format("Select * from patients where pat_id in ({0})", sbCopyPatId.ToString());

            DataTable dtCopyPat = helper.GetTable(strCopySql);


            String strMes = string.Empty;

            if (dtCopyPat.Rows.Count <= 0)
            {
                DbBase dao = DbBase.InConn();
                System.Collections.ArrayList s = dao.GetInsertSql(dtPat);
                dao.DoTran(s);
                dao.DoTran(dao.GetInsertSql(dtPatMi));
                dao.DoTran(al);
            }
            else
            {
                strMes = "此批病人有资料已经存在";
            }
            return strMes;
        }

        /// <summary>
        /// 复制患者信息(可指定目标样本号)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId">仪器ID</param>
        /// <param name="newSid">新样本号</param>
        /// <returns></returns>
        public String CopyPatientsInfoCustomSid(List<string> pat_id, DateTime dtTime, String strItrId, List<decimal> newSid)
        {
            StringBuilder sbPatId = new StringBuilder();
            foreach (string strPatId in pat_id)
            {
                sbPatId.Append(string.Format(",'{0}'", strPatId));
            }

            sbPatId.Remove(0, 1);

            String strSql = string.Format("Select * from patients where pat_id in ({0})", sbPatId.ToString());

            DBHelper helper = new DBHelper();
            DataTable dtPat = helper.GetTable(strSql);
            dtPat.TableName = "patients";
            if (dtPat.Columns.Contains("pat_key"))
                dtPat.Columns.Remove("pat_key");


            String strSqlCombine = string.Format("Select * from patients_mi where pat_id in ({0})", sbPatId.ToString());
            DataTable dtPatMi = helper.GetTable(strSqlCombine);
            dtPatMi.TableName = "patients_mi";

            int sidIndex = 0;//样本LIST索引
            StringBuilder sbCopyPatId = new StringBuilder();
            ArrayList al = new ArrayList();
            foreach (DataRow drPat in dtPat.Rows)
            {
                string strPatOldId = drPat["pat_id"].ToString();
                string strPatItrOldId = drPat["pat_itr_id"].ToString();
                DateTime patolddate = Convert.ToDateTime(drPat["pat_date"]);
                string strPatSid = newSid.Count > sidIndex ? newSid[sidIndex].ToString() : drPat["pat_sid"].ToString();
                string strPatId = strItrId + dtTime.ToString("yyyyMMdd") + strPatSid;
                drPat["pat_id"] = strPatId;
                drPat["pat_sid"] = strPatSid;//更新样本号
                drPat["pat_itr_id"] = strItrId;//更新仪器代码
                drPat["pat_date"] = dtTime;

                //不复制一审与二审的时间与操作者信息
                if (true)
                {
                    drPat["pat_report_date"] = DBNull.Value;
                    drPat["pat_flag"] = 0;
                    drPat["pat_send_flag"] = DBNull.Value;
                    drPat["pat_report_code"] = DBNull.Value;
                    drPat["pat_chk_date"] = DBNull.Value;
                    drPat["pat_chk_code"] = DBNull.Value;
                }

                sbCopyPatId.Append(string.Format(",'{0}'", strPatId));

                foreach (DataRow drPatMi in dtPatMi.Select("pat_id='" + strPatOldId + "'"))
                {
                    drPatMi["pat_id"] = strPatId;
                }
                if (drPat["pat_bar_code"] != null && drPat["pat_bar_code"] != DBNull.Value &&
                    !string.IsNullOrEmpty(drPat["pat_bar_code"].ToString()))
                {
                    string sqlBcSign = string.Format(@"
insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status, pat_id,bc_remark)
values('{0}','{1}',getdate(),'{3}','{4}','20','{5}','{6}')"
                                                     , ""
                                                     , ""
                                                     , ""
                                                     , string.Empty
                                                     , string.Empty
                                                     , strPatId
                                                     ,
                                                     string.Format("复制:原资料日期：{0} 仪器代码：{1}->{2}",
                                                                   patolddate.ToString("yyyy-MM-dd"), strPatItrOldId,
                                                                   strItrId)
                        );

                    al.Add(sqlBcSign);
                }

                sidIndex++;//递增索引
            }

            sbCopyPatId.Remove(0, 1);

            String strCopySql = string.Format("Select * from patients where pat_id in ({0})", sbCopyPatId.ToString());

            DataTable dtCopyPat = helper.GetTable(strCopySql);


            String strMes = string.Empty;

            if (dtCopyPat.Rows.Count <= 0)
            {
                DbBase dao = DbBase.InConn();
                System.Collections.ArrayList s = dao.GetInsertSql(dtPat);
                dao.DoTran(s);
                dao.DoTran(dao.GetInsertSql(dtPatMi));
            }
            else
            {
                strMes = "此批病人有资料已经存在";
            }
            return strMes;
        }

        public string CopyPatientsInfoCustomForBf(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            StringBuilder sbPatId = new StringBuilder();
            foreach (string strPatId in pat_id)
            {
                sbPatId.Append(string.Format(",'{0}'", strPatId));
            }

            sbPatId.Remove(0, 1);

            String strSql = string.Format("Select * from patients_newborn where pat_id in ({0})", sbPatId.ToString());

            DBHelper helper = new DBHelper();
            DataTable dtPat = helper.GetTable(strSql);
            dtPat.TableName = "patients_newborn";
            if (dtPat.Columns.Contains("pat_key"))
                dtPat.Columns.Remove("pat_key");


            String strSqlCombine = string.Format("Select * from patients_mi_newborn where pat_id in ({0})", sbPatId.ToString());
            DataTable dtPatMi = helper.GetTable(strSqlCombine);
            dtPatMi.TableName = "patients_mi_newborn";
            DictInstructmentBLL dll = new DictInstructmentBLL();
            String strMes = string.Empty;
            StringBuilder sbCopyPatId = new StringBuilder();
            foreach (DataRow drPat in dtPat.Rows)
            {
                string strPatOldId = drPat["pat_id"].ToString();
                string strPatSid = drPat["pat_sid"].ToString();
                //if (drPat["pat_date"] != null && drPat["pat_date"] != DBNull.Value)
                //{
                //    DateTime pat_date = (DateTime) drPat["pat_date"];


                strPatSid = dll.GetItrSID_MaxPlusOneForBF(dtTime, strItrId, true);

                strMes += string.Format("病人[{0}]生成新的复查样本号[{1}]\r\n", drPat["pat_name"], strPatSid);

                //}
                string strPatId = strItrId + dtTime.ToString("yyyyMMdd") + strPatSid;

                drPat["pat_prt_flag"] = 2;
                drPat["pat_id"] = strPatId;
                drPat["pat_sid"] = strPatSid;//更新样本号
                drPat["pat_itr_id"] = strItrId;//更新仪器代码
                drPat["pat_date"] = dtTime;
                drPat["pat_sam_rem"] = "复查";

                //不复制一审与二审的时间与操作者信息
                if (true)
                {
                    drPat["pat_report_date"] = DBNull.Value;
                    drPat["pat_flag"] = 0;
                    drPat["pat_send_flag"] = DBNull.Value;
                    drPat["pat_report_code"] = DBNull.Value;
                    drPat["pat_chk_date"] = DBNull.Value;
                    drPat["pat_chk_code"] = DBNull.Value;
                }

                sbCopyPatId.Append(string.Format(",'{0}'", strPatId));

                foreach (DataRow drPatMi in dtPatMi.Select("pat_id='" + strPatOldId + "'"))
                {
                    drPatMi["pat_id"] = strPatId;
                }

            }

            sbCopyPatId.Remove(0, 1);

            String strCopySql = string.Format("Select * from patients_newborn where pat_id in ({0})", sbCopyPatId.ToString());

            DataTable dtCopyPat = helper.GetTable(strCopySql);




            if (dtCopyPat.Rows.Count <= 0)
            {
                DbBase dao = DbBase.InConn();
                System.Collections.ArrayList s = dao.GetInsertSql(dtPat);
                dao.DoTran(s);
                dao.DoTran(dao.GetInsertSql(dtPatMi));
            }
            else
            {
                strMes = "此批病人有资料已经存在,复查失败，请重新审核";
            }
            return strMes;
        }

        /// <summary>
        /// 更新病人结果
        /// </summary>
        /// <param name="dtPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        private void UpdatePatientResult(DataTable dtPatientResult, DataTable dtOriginPatResult, EntityOperationResult result, DBHelper transHelper, OperationLogger opLogger)
        {
            if (result.Success)
            {
                //样本号
                string pat_sid = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();

                //病人ID
                string pat_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();

                //仪器ID
                string pat_itr_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_itr_id"].ToString();

                //样本类型ID
                string pat_sam_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sam_id"].ToString();


                //获取结果表结构
                PatientEnterService bll = new PatientEnterService();
                DataTable dtStruct = bll.GetPatientResultStruct();

                DateTime today = ServerDateTime.GetDatabaseServerDateTime();

                bool isVerify = ResultExistVerify();
                if (isVerify && !dtPatientResult.Columns.Contains("res_verify"))
                    dtPatientResult.Columns.Add("res_verify");

                #region 插入新结果信息
                //获取需要新增的行
                DataRow[] drsAddNew = dtPatientResult.Select("isnew=1");
                List<SqlCommand> cmdResultInsert = null;
                if (drsAddNew.Length > 0)
                {
                    foreach (DataRow drInsert in drsAddNew)
                    {
                        //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                        string itmecd = SQLFormater.Format(drInsert["res_itm_ecd"].ToString());
                        DataRow[] drsOrigin = dtOriginPatResult.Select(string.Format("res_itm_ecd='{0}'", itmecd));

                        if (drsOrigin.Length > 0)
                        {
                            drInsert["isnew"] = 0;
                            continue;
                        }


                        if (dcl.common.Compare.IsEmpty(drInsert["res_date"]))
                        {
                            drInsert["res_date"] = today;
                        }

                        drInsert["res_id"] = pat_id;
                        drInsert["res_sid"] = pat_sid;
                        drInsert["res_date"] = today;
                        drInsert["res_itr_id"] = pat_itr_id;
                        drInsert["res_flag"] = 1;
                        //drInsert["res_type"] = 0;
                        drInsert["res_rep_type"] = 0;

                        //drInsert["res_type"] = 0;
                        //drInsert["res_date"] = DateTime.Now;
                        //if (isVerify)
                        //    drInsert["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(drInsert["res_itm_id"].ToString() + ";" + drInsert["res_chr"].ToString());
                        opLogger.Add_AddLog(SysOperationLogGroup.PAT_RESULT, drInsert["res_itm_ecd"].ToString(), drInsert["res_chr"].ToString());
                    }
                    drsAddNew = dtPatientResult.Select("isnew=1");
                    if (drsAddNew.Length > 0)
                    {
                        DataTable dtResultAddNew = drsAddNew.CopyToDataTable();

                        cmdResultInsert = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName,
                                                                              new string[] { "res_key" }, dtResultAddNew);
                    }
                }
                #endregion

                #region 更新原结果信息
                //获取需要更新的行
                DataRow[] drsUpdate = dtPatientResult.Select("isnew=0");

                List<SqlCommand> cmdResultUpdate = null;
                if (drsUpdate.Length > 0)
                {
                    ////更新样本号
                    //foreach (DataRow dr in drsUpdate)
                    //{
                    //    dr["res_sid"] = pat_sid;
                    //    dr["res_itr_id"] = pat_itr_id;
                    //}

                    DataTable dtResultUpdate = drsUpdate.CopyToDataTable();

                    //获取原有结果记录

                    foreach (DataRow drCurrResult in dtResultUpdate.Rows)
                    {
                        if (dcl.common.Compare.IsEmpty(drCurrResult["res_date"]))
                        {
                            drCurrResult["res_date"] = today;
                        }

                        drCurrResult["res_sid"] = pat_sid;
                        drCurrResult["res_itr_id"] = pat_itr_id;
                        //if (isVerify)
                        //    drCurrResult["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(drCurrResult["res_itm_id"].ToString() + ";" + drCurrResult["res_chr"].ToString());
                        string itmecd = SQLFormater.Format(drCurrResult["res_itm_ecd"].ToString());
                        DataRow[] drsOrigin = dtOriginPatResult.Select(string.Format("res_itm_ecd='{0}'", itmecd));
                        if (drsOrigin.Length > 0)
                        {
                            DataRow drOrigin = drsOrigin[0];
                            if (drCurrResult["res_key"] == null || drCurrResult["res_key"] == DBNull.Value)
                            {
                                drCurrResult["res_key"] = drOrigin["res_key"];
                            }
                            //查找普通结果是否有更改
                            if (!ObjectEquals(drOrigin["res_chr"], drCurrResult["res_chr"]))
                            {
                                string currValue = string.Empty;
                                string oldValue = string.Empty;

                                if (drCurrResult["res_chr"] != null && drCurrResult["res_chr"] != DBNull.Value)
                                {
                                    currValue = drCurrResult["res_chr"].ToString();
                                }
                                if (drOrigin["res_chr"] != null && drOrigin["res_chr"] != DBNull.Value)
                                {
                                    oldValue = drOrigin["res_chr"].ToString();
                                }
                                drCurrResult["res_date"] = today;
                                //****************************************************
                                //将修改过的项目的结果类型置为手工
                                drCurrResult["res_type"] = 0;
                                //****************************************************

                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldValue + "→" + currValue);
                            }

                            //查找od结果是否有更改
                            if (!ObjectEquals(drOrigin["res_od_chr"], drCurrResult["res_od_chr"]))
                            {
                                string currValue = string.Empty;
                                string oldODValue = string.Empty;

                                if (drCurrResult["res_od_chr"] != null && drCurrResult["res_od_chr"] != DBNull.Value)
                                {
                                    currValue = drCurrResult["res_od_chr"].ToString();
                                }
                                if (drOrigin["res_od_chr"] != null && drOrigin["res_od_chr"] != DBNull.Value)
                                {
                                    oldODValue = drOrigin["res_od_chr"].ToString();
                                }
                                drCurrResult["res_date"] = today;
                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldODValue + "→" + currValue);
                            }
                        }
                    }

                    //"res_id", "res_itm_id", "res_itm_ecd", 

                    //生成更新SqlCommand
                    cmdResultUpdate = DBTableHelper.GenerateUpdateCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtResultUpdate);
                }
                #endregion



                #region 删除结果
                DataRow[] drsDelete = dtPatientResult.Select("isnew=2");
                List<SqlCommand> cmdResultDelete = null;
                if (drsDelete.Length > 0)
                {
                    cmdResultDelete = new List<SqlCommand>();

                    ////生成删除语句
                    //string sqlDelete = "delete from resulto where res_id='{0}' and res_itm_ecd='{1}'";
                    //foreach (DataRow drDel in drsDelete)
                    //{
                    //    //项目编码
                    //    string res_itm_ecd = drDel["res_itm_ecd"].ToString();

                    //    string sqlDel = string.Format(sqlDelete, pat_id, res_itm_ecd);

                    //    SqlCommand cmd = new SqlCommand(sqlDel);
                    //    cmdResultDelete.Add(cmd);

                    //    //在原结果表中查找被删除项目的结果值
                    //    DataRow[] drsOriginExistedItem = dtOriginPatResult.Select(string.Format("res_itm_ecd='{0}'", res_itm_ecd));
                    //    if (drsOriginExistedItem.Length > 0)
                    //    {
                    //        string originValue = string.Empty;
                    //        if (drsOriginExistedItem[0]["res_chr"] != null && drsOriginExistedItem[0]["res_chr"] != DBNull.Value)
                    //        {
                    //            originValue = drsOriginExistedItem[0]["res_chr"].ToString();
                    //        }

                    //        //记录删除日志
                    //        opLogger.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_itm_ecd, originValue);
                    //    }
                    //}

                    //生成删除语句
                    //string sqlDelete = "delete from resulto where res_id='{0}' and res_itm_id='{1}'";
                    string sqlDelete = "delete from resulto where res_key={0}";
                    foreach (DataRow drDel in drsDelete)
                    {
                        string res_itm_id = drDel["res_itm_id"].ToString();
                        string res_itm_ecd = drDel["res_itm_ecd"].ToString();

                        Int64 reskey = Convert.ToInt64(drDel["res_key"]);

                        string sqlDel = string.Format(sqlDelete, reskey);

                        SqlCommand cmd = new SqlCommand(sqlDel);
                        cmdResultDelete.Add(cmd);

                        if (drDel["res_itm_id"] != null && drDel["res_itm_id"] != DBNull.Value && drDel["res_itm_id"].ToString().Trim(null) != string.Empty)
                        {
                            //在原结果表中查找被删除项目的结果值
                            DataRow[] drsOriginExistedItem = dtOriginPatResult.Select(string.Format("res_itm_id='{0}'", res_itm_id));
                            if (drsOriginExistedItem.Length > 0)
                            {
                                string originValue = string.Empty;
                                if (drsOriginExistedItem[0]["res_chr"] != null && drsOriginExistedItem[0]["res_chr"] != DBNull.Value)
                                {
                                    originValue = drsOriginExistedItem[0]["res_chr"].ToString();
                                }

                                //记录删除日志
                                opLogger.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_itm_ecd, originValue);
                            }
                        }

                        #region 原有删除方式
                        //if (drDel["res_itm_id"] != null && drDel["res_itm_id"] != DBNull.Value && drDel["res_itm_id"].ToString().Trim(null) != string.Empty)
                        //{
                        //    string res_itm_id = drDel["res_itm_id"].ToString();
                        //    string res_itm_ecd = drDel["res_itm_ecd"].ToString();

                        //    string sqlDel = string.Format(sqlDelete, pat_id, res_itm_id);

                        //    SqlCommand cmd = new SqlCommand(sqlDel);
                        //    cmdResultDelete.Add(cmd);

                        //    //在原结果表中查找被删除项目的结果值
                        //    DataRow[] drsOriginExistedItem = dtOriginPatResult.Select(string.Format("res_itm_id='{0}'", res_itm_id));
                        //    if (drsOriginExistedItem.Length > 0)
                        //    {
                        //        string originValue = string.Empty;
                        //        if (drsOriginExistedItem[0]["res_chr"] != null && drsOriginExistedItem[0]["res_chr"] != DBNull.Value)
                        //        {
                        //            originValue = drsOriginExistedItem[0]["res_chr"].ToString();
                        //        }

                        //        //记录删除日志
                        //        opLogger.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_itm_ecd, originValue);
                        //    }
                        //} 
                        #endregion
                    }
                }
                #endregion

                try
                {
                    //执行sql(事务)
                    if (cmdResultUpdate != null)//更新结果
                    {
                        foreach (SqlCommand cmdResUpdate in cmdResultUpdate)
                        {
                            transHelper.ExecuteNonQuery(cmdResUpdate);
                        }
                    }

                    if (cmdResultInsert != null)//新增结果
                    {
                        foreach (SqlCommand cmdResInsert in cmdResultInsert)
                        {
                            transHelper.ExecuteNonQuery(cmdResInsert);
                        }
                    }


                    if (cmdResultDelete != null)//删除结果
                    {
                        foreach (SqlCommand cmdResDelete in cmdResultDelete)
                        {
                            transHelper.ExecuteNonQuery(cmdResDelete);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdatePatientResult", ex.ToString());
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    throw;
                }

            }
        }

        /// <summary>
        /// 更新病人结果
        /// </summary>
        /// <param name="dtPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        private void UpdatePatientResultForBf(DataTable dtPatientResult, DataTable dtOriginPatResult, EntityOperationResult result, DBHelper transHelper, OperationLogger opLogger)
        {
            if (result.Success)
            {
                //样本号
                string pat_sid = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();

                //病人ID
                string pat_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();

                //仪器ID
                string pat_itr_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_itr_id"].ToString();



                //获取结果表结构

                DateTime today = DateTime.Now;

                #region 插入新结果信息
                //获取需要新增的行
                DataRow[] drsAddNew = dtPatientResult.Select("isnew=1");
                List<SqlCommand> cmdResultInsert = null;
                if (drsAddNew.Length > 0)
                {
                    foreach (DataRow drInsert in drsAddNew)
                    {
                        //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                        string itmecd = SQLFormater.Format(drInsert["res_itm_ecd"].ToString());
                        DataRow[] drsOrigin = dtOriginPatResult.Select(string.Format("res_itm_ecd='{0}'", itmecd));

                        if (drsOrigin.Length > 0)
                        {
                            drInsert["isnew"] = 0;
                            continue;
                        }


                        if (dcl.common.Compare.IsEmpty(drInsert["res_date"]))
                        {
                            drInsert["res_date"] = DateTime.Now;
                        }

                        drInsert["res_id"] = pat_id;
                        drInsert["res_sid"] = pat_sid;
                        drInsert["res_date"] = today;
                        drInsert["res_itr_id"] = pat_itr_id;
                        drInsert["res_flag"] = 1;
                        //drInsert["res_type"] = 0;
                        drInsert["res_rep_type"] = 0;

                        //drInsert["res_type"] = 0;
                        //drInsert["res_date"] = DateTime.Now;

                        opLogger.Add_AddLog(SysOperationLogGroup.PAT_RESULT, drInsert["res_itm_ecd"].ToString(), drInsert["res_chr"].ToString());
                    }
                    drsAddNew = dtPatientResult.Select("isnew=1");
                    if (drsAddNew.Length > 0)
                    {
                        DataTable dtResultAddNew = drsAddNew.CopyToDataTable();

                        cmdResultInsert = DBTableHelper.GenerateInsertCommand("resulto_newborn",
                                                                              new string[] { "res_key" }, dtResultAddNew);
                    }
                }
                #endregion

                #region 更新原结果信息
                //获取需要更新的行
                DataRow[] drsUpdate = dtPatientResult.Select("isnew=0");

                List<SqlCommand> cmdResultUpdate = null;
                if (drsUpdate.Length > 0)
                {
                    ////更新样本号
                    //foreach (DataRow dr in drsUpdate)
                    //{
                    //    dr["res_sid"] = pat_sid;
                    //    dr["res_itr_id"] = pat_itr_id;
                    //}

                    DataTable dtResultUpdate = drsUpdate.CopyToDataTable();

                    //获取原有结果记录

                    foreach (DataRow drCurrResult in dtResultUpdate.Rows)
                    {
                        if (dcl.common.Compare.IsEmpty(drCurrResult["res_date"]))
                        {
                            drCurrResult["res_date"] = DateTime.Now;
                        }

                        drCurrResult["res_sid"] = pat_sid;
                        drCurrResult["res_itr_id"] = pat_itr_id;

                        string itmecd = SQLFormater.Format(drCurrResult["res_itm_ecd"].ToString());
                        DataRow[] drsOrigin = dtOriginPatResult.Select(string.Format("res_itm_ecd='{0}'", itmecd));
                        if (drsOrigin.Length > 0)
                        {
                            DataRow drOrigin = drsOrigin[0];
                            if (drCurrResult["res_key"] == null || drCurrResult["res_key"] == DBNull.Value)
                            {
                                drCurrResult["res_key"] = drOrigin["res_key"];
                            }
                            //查找普通结果是否有更改
                            if (!ObjectEquals(drOrigin["res_chr"], drCurrResult["res_chr"]))
                            {
                                string currValue = string.Empty;
                                string oldValue = string.Empty;

                                if (drCurrResult["res_chr"] != null && drCurrResult["res_chr"] != DBNull.Value)
                                {
                                    currValue = drCurrResult["res_chr"].ToString();
                                }
                                if (drOrigin["res_chr"] != null && drOrigin["res_chr"] != DBNull.Value)
                                {
                                    oldValue = drOrigin["res_chr"].ToString();
                                }
                                drCurrResult["res_date"] = today;
                                //****************************************************
                                //将修改过的项目的结果类型置为手工
                                drCurrResult["res_type"] = 0;
                                //****************************************************

                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldValue + "→" + currValue);
                            }

                            //查找od结果是否有更改
                            if (!ObjectEquals(drOrigin["res_od_chr"], drCurrResult["res_od_chr"]))
                            {
                                string currValue = string.Empty;
                                string oldODValue = string.Empty;

                                if (drCurrResult["res_od_chr"] != null && drCurrResult["res_od_chr"] != DBNull.Value)
                                {
                                    currValue = drCurrResult["res_od_chr"].ToString();
                                }
                                if (drOrigin["res_od_chr"] != null && drOrigin["res_od_chr"] != DBNull.Value)
                                {
                                    oldODValue = drOrigin["res_od_chr"].ToString();
                                }
                                drCurrResult["res_date"] = today;
                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldODValue + "→" + currValue);
                            }
                        }
                    }

                    //"res_id", "res_itm_id", "res_itm_ecd", 

                    //生成更新SqlCommand
                    cmdResultUpdate = DBTableHelper.GenerateUpdateCommand("resulto_newborn", new string[] { "res_key" }, dtResultUpdate);
                }
                #endregion



                #region 删除结果
                DataRow[] drsDelete = dtPatientResult.Select("isnew=2");
                List<SqlCommand> cmdResultDelete = null;
                if (drsDelete.Length > 0)
                {
                    cmdResultDelete = new List<SqlCommand>();



                    //生成删除语句
                    //string sqlDelete = "delete from resulto where res_id='{0}' and res_itm_id='{1}'";
                    string sqlDelete = "delete from resulto_newborn where res_key={0}";
                    foreach (DataRow drDel in drsDelete)
                    {
                        string res_itm_id = drDel["res_itm_id"].ToString();
                        string res_itm_ecd = drDel["res_itm_ecd"].ToString();

                        Int64 reskey = Convert.ToInt64(drDel["res_key"]);

                        string sqlDel = string.Format(sqlDelete, reskey);

                        SqlCommand cmd = new SqlCommand(sqlDel);
                        cmdResultDelete.Add(cmd);

                        if (drDel["res_itm_id"] != null && drDel["res_itm_id"] != DBNull.Value && drDel["res_itm_id"].ToString().Trim(null) != string.Empty)
                        {
                            //在原结果表中查找被删除项目的结果值
                            DataRow[] drsOriginExistedItem = dtOriginPatResult.Select(string.Format("res_itm_id='{0}'", res_itm_id));
                            if (drsOriginExistedItem.Length > 0)
                            {
                                string originValue = string.Empty;
                                if (drsOriginExistedItem[0]["res_chr"] != null && drsOriginExistedItem[0]["res_chr"] != DBNull.Value)
                                {
                                    originValue = drsOriginExistedItem[0]["res_chr"].ToString();
                                }

                                //记录删除日志
                                opLogger.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_itm_ecd, originValue);
                            }
                        }


                    }
                }
                #endregion

                try
                {
                    //执行sql(事务)
                    if (cmdResultUpdate != null)//更新结果
                    {
                        foreach (SqlCommand cmdResUpdate in cmdResultUpdate)
                        {
                            transHelper.ExecuteNonQuery(cmdResUpdate);
                        }
                    }

                    if (cmdResultInsert != null)//新增结果
                    {
                        foreach (SqlCommand cmdResInsert in cmdResultInsert)
                        {
                            transHelper.ExecuteNonQuery(cmdResInsert);
                        }
                    }


                    if (cmdResultDelete != null)//删除结果
                    {
                        foreach (SqlCommand cmdResDelete in cmdResultDelete)
                        {
                            transHelper.ExecuteNonQuery(cmdResDelete);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdatePatientResult", ex.ToString());
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    throw;
                }

            }
        }

        /// <summary>
        /// 病人录入界面结果改变时更新结果表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dtPatientResult"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatientResultItems_byResKey(EntityRemoteCallClientInfo userInfo, DataTable dtPatientResult)
        {
            EntityOperationResult ret = new EntityOperationResult();

            if (dtPatientResult.Rows.Count > 0)
            {
                //*********************************************************************
                //修改结果类型和结果时间
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                dtPatientResult.Rows[0]["res_type"] = 0;
                dtPatientResult.Rows[0]["res_date"] = time;

                //*********************************************************************
                string pat_id = dtPatientResult.Rows[0]["res_id"].ToString();

                OperationLogger opLogPat = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                string sqlSelect = "select top 1 res_chr from resulto where res_key = @res_key";

                DBHelper helper = new DBHelper();

                bool needFillItrId = false;
                string pat_sid = string.Empty;
                string pat_itr_id = string.Empty;

                if (dtPatientResult.Select("res_itr_id is null or res_sid is null").Length > 0)
                {
                    needFillItrId = true;

                    string sqlSelectPat = string.Format(@"
                    select top 1
                    pat_itr_id,
                    pat_sid
                    from patients
                    where patients.pat_id = '{0}'
                    ", pat_id);

                    DataTable dtpatient = helper.GetTable(sqlSelectPat);

                    if (dtpatient.Rows.Count > 0
                        && !dcl.common.Compare.IsEmpty(dtpatient.Rows[0]["pat_itr_id"])
                        && !dcl.common.Compare.IsEmpty(dtpatient.Rows[0]["pat_sid"]))
                    {
                        pat_sid = dtpatient.Rows[0]["pat_sid"].ToString();
                        pat_itr_id = dtpatient.Rows[0]["pat_itr_id"].ToString();
                    }
                }

                //生成修改日志
                foreach (DataRow drResult in dtPatientResult.Rows)
                {
                    long res_key = Convert.ToInt64(drResult["res_key"]);
                    string itm_ecd = drResult["res_itm_ecd"].ToString();
                    string newValue = string.Empty;

                    if (drResult["res_chr"] != null)
                    {
                        newValue = drResult["res_chr"].ToString();
                    }

                    decimal devCastChr = 0;
                    if (decimal.TryParse(drResult["res_chr"].ToString(), out devCastChr))
                    {
                        drResult["res_cast_chr"] = devCastChr;
                    }
                    else
                    {
                        drResult["res_cast_chr"] = DBNull.Value;
                    }

                    if (needFillItrId)
                    {
                        drResult["res_itr_id"] = pat_itr_id;
                        drResult["res_sid"] = pat_sid;
                    }

                    SqlCommand cmdSelect = new SqlCommand(sqlSelect);
                    cmdSelect.Parameters.AddWithValue("res_key", res_key);

                    object objPrevValue = helper.ExecuteScalar(cmdSelect);

                    if ((objPrevValue == null && newValue != string.Empty)
                        || (objPrevValue.ToString() != newValue))
                    {
                        opLogPat.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itm_ecd, objPrevValue.ToString() + "→" + newValue);
                    }
                }



                List<SqlCommand> listCmd = DBTableHelper.GenerateUpdateCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtPatientResult);


                try
                {
                    using (DBHelper transHelper = DBHelper.BeginTransaction())
                    {
                        foreach (SqlCommand cmd in listCmd)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }

                        //string sqlUpdate = string.Format(@"
                        //update patients set pat_modified_times =  1                                             
                        //where pat_id = '{0}' and isnull(pat_modified_times,0)=0 ", pat_id);

                        //transHelper.ExecuteNonQuery(sqlUpdate);

                        transHelper.Commit();
                    }

                    opLogPat.Log();
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdatePatientResult", ex.ToString());
                    ret.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                }
            }

            return ret;
        }

        /// <summary>
        /// 病人录入界面结果改变时更新结果表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="dtPatientResult"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatientResultItems_byResID_itmID(EntityRemoteCallClientInfo userInfo, DataTable dtPatientResult)
        {
            EntityOperationResult ret = new EntityOperationResult();

            if (dtPatientResult.Rows.Count > 0)
            {
                string pat_id = dtPatientResult.Rows[0]["res_id"].ToString();
                string res_itm_id = dtPatientResult.Rows[0]["res_itm_id"].ToString();

                OperationLogger opLogPat = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                string sqlSelect = "select top 1 res_chr from resulto where res_id = @res_id and res_itm_id = @res_itm_id";

                DBHelper helper = new DBHelper();

                //生成修改日志
                foreach (DataRow drResult in dtPatientResult.Rows)
                {
                    string itm_ecd = drResult["res_itm_ecd"].ToString();
                    string newValue = string.Empty;

                    if (drResult["res_chr"] != null)
                    {
                        newValue = drResult["res_chr"].ToString();
                    }

                    SqlCommand cmdSelect = new SqlCommand(sqlSelect);
                    SqlParameter p1 = cmdSelect.Parameters.AddWithValue("res_id", pat_id);
                    p1.DbType = DbType.AnsiString;

                    SqlParameter p2 = cmdSelect.Parameters.AddWithValue("res_itm_id", res_itm_id);
                    p2.DbType = DbType.AnsiString;

                    object objPrevValue = helper.ExecuteScalar(cmdSelect);

                    if ((objPrevValue == null && newValue != string.Empty)
                        || (objPrevValue.ToString() != newValue))
                    {
                        opLogPat.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itm_ecd, objPrevValue.ToString() + "→" + newValue);
                    }
                }

                List<SqlCommand> listCmd = DBTableHelper.GenerateUpdateCommand(PatientTable.PatientResultTableName, new string[] { "res_id", "res_itm_id" }, new string[] { "res_key" }, dtPatientResult);


                try
                {
                    using (DBHelper transHelper = DBHelper.BeginTransaction())
                    {
                        foreach (SqlCommand cmd in listCmd)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }

                        //string sqlUpdate = string.Format(@"
                        //update patients set pat_modified_times =  1                                             
                        //where pat_id = '{0}' and isnull(pat_modified_times,0)=0 ", pat_id);

                        //transHelper.ExecuteNonQuery(sqlUpdate);

                        transHelper.Commit();
                    }
                    opLogPat.Log();
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdatePatientResult", ex.ToString());
                    ret.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                }


            }

            return ret;
        }

        /// <summary>
        /// 更新描述报告描述内容
        /// </summary>
        /// <param name="dtPatientDescResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private void UpdateDescResult(DataTable dtPatientDescResult, EntityOperationResult result, DBHelper transHelper, OperationLogger opLogger)
        {
            if (result.Success)
            {

                //样本号
                string pat_sid = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();

                //病人ID
                string pat_id = result.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();


                //更新描述内容样本号
                dtPatientDescResult.Rows[0]["bsr_id"] = pat_id;


                List<SqlCommand> cmdDesc = DBTableHelper.GenerateUpdateCommand(PatientTable.PatientDescResultTableName, new string[] { "bsr_id" }, dtPatientDescResult);

                try
                {
                    int rowAffact = transHelper.ExecuteNonQuery(cmdDesc[0]);

                    if (rowAffact == 0)
                    {
                        List<SqlCommand> cmds = DBTableHelper.GenerateInsertCommand(PatientTable.PatientDescResultTableName, null, dtPatientDescResult);
                        transHelper.ExecuteNonQuery(cmds[0]);
                    }
                }
                catch (Exception ex)
                {
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    Logger.WriteException(this.GetType().Name, "UpdateDescResult", ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// 生成更改病人基本信息记录
        /// </summary>
        /// <param name="currentPatInfo"></param>
        /// <param name="operatorID"></param>
        /// <param name="ip"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private void GeneratePatInfoUpdateLogger(DataTable currentPatInfo, DataTable dtOriginPatInfo, OperationLogger opLogger)
        {
            //PatientEnterService bllPat = new PatientEnterService();

            ////获取原有资料
            //DataTable orgPatInfo = bllPat.GetPatientInfo(opLogger.OperationKey).Tables[0];

            PatientEnterService patientEnterService = new PatientEnterService();

            //获取表结构
            DataTable tbStruct = patientEnterService.GetPatientInfoStruct();
            bool labEnableNoBarCodeCheck = CacheSysConfig.Current.GetSystemConfig("Lab_EnableNoBarCodeCheck") == "是";

            if (dtOriginPatInfo.Rows.Count > 0 && currentPatInfo.Rows.Count > 0)
            {
                DataRow drOrgPat = dtOriginPatInfo.Rows[0];
                DataRow drCurrPat = currentPatInfo.Rows[0];

                //遍历每一列
                foreach (DataColumn col in tbStruct.Columns)
                {
                    string colName = col.ColumnName;

                    if (dtOriginPatInfo.Columns.Contains(colName)
                        && currentPatInfo.Columns.Contains(colName)
                        && colName != "pat_date"
                        && colName != "pat_exp"
                        && colName != "pat_comment"
                        )
                    {
                        if (!ObjectEquals(drOrgPat[colName], drCurrPat[colName]))
                        {

                            string currValue = string.Empty;
                            string oldValue = string.Empty;
                            if (drCurrPat[colName] != null && drCurrPat[colName] != DBNull.Value)
                            {
                                currValue = drCurrPat[colName].ToString();
                            }
                            if (drOrgPat[colName] != null && drOrgPat[colName] != DBNull.Value)
                            {
                                oldValue = drOrgPat[colName].ToString();
                            }
                            if (colName == "pat_name" && labEnableNoBarCodeCheck)
                            {
                                drCurrPat["pat_bar_code"] = string.Empty;
                            }
                            string colCHS = FieldsNameConventer<PatientFields>.Instance.GetFieldCHS(colName);
                            if (colCHS != string.Empty)//有中文的对照才记录日志
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_INFO, colCHS, oldValue + "→" + currValue);
                            }
                        }
                    }
                }
            }
        }

        bool ObjectEquals(object obj1, object obj2)
        {
            if (
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value))
                ||
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 != null && obj2 != DBNull.Value && obj2.ToString() == string.Empty))
                ||
                ((obj2 == null || obj2 == DBNull.Value) && (obj1 != null && obj1 != DBNull.Value && obj1.ToString() == string.Empty))
            )
            {
                return true;
            }
            else
            {
                if (obj1.ToString() == obj2.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 追加条码
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_date">源标本登记的日期</param>
        /// <param name="pat_bar_code">源标本条码号</param>
        /// <param name="barcode_to_append">需要追加的条码号</param>
        /// <param name="pat_itr_name">仪器名称</param>
        /// <param name="pat_sid">样本号</param>
        /// <param name="loginId">操作题工号</param>
        /// <param name="loginName">操作人姓名</param>
        public void PatientAdditionalBarcode(
                                string pat_id
                                , DateTime pat_date
                                , string pat_bar_code
                                , string barcode_to_append
                                , string pat_itr_name
                                , string pat_sid
                                , string loginId
                                , string loginName)
        {
            DBHelper helper = new DBHelper();

            //添加操作记录
            string sqlBcSign = string.Format(@"
insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status,bc_remark)
values('{0}','{1}',getdate(),'{3}','{4}','560','{5}')"
                , barcode_to_append
                , barcode_to_append
                , ""
                , string.Empty
                , string.Empty
                , string.Format("条码信息追加到 资料日期：{0} 仪器：{1}，样本号：{2}", pat_date.ToString("yyyy-MM-dd"), pat_itr_name, pat_sid)
                );
            helper.ExecuteNonQuery(sqlBcSign);
            //更新bc_patients表标志
            string sqlUpdate = string.Format(@"update bc_patients
                                               set bc_status = '560',bc_lastaction_time =getdate()
                                               where bc_bar_code = '{1}'", "", barcode_to_append);

            helper.ExecuteNonQuery(sqlUpdate);



            //更新原条码信息
            //            string sqlUpdate2 = string.Format(@"update bc_patients
            //                                               set bc_status ='560',bc_lastaction_time = '{0}'
            //                                               where bc_bar_code = '{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), barcode_to_append);

            //helper.ExecuteNonQuery(sqlUpdate);

            //更新原条码信息与报告单信息的日志
            string sqlBcSign2 = string.Format(@"
insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status,bc_remark,pat_id)
values('{0}','{1}',getdate(),'{3}','{4}','','{5}','{6}')", pat_bar_code
                , pat_bar_code
                , ""
                , string.Empty
                , string.Empty
                , string.Format("条码追加，条码号：{0}", barcode_to_append)
                , pat_id
                );

            helper.ExecuteNonQuery(sqlBcSign2);
        }

        /// <summary>
        /// 更新病人的标本备注信息。
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_sam_rem"></param>
        public bool UpdatePatientsSamRem(string pat_id, string pat_sam_rem)
        {
            try
            {
                DBHelper helper = new DBHelper();
                string sqlUpdate = string.Format(@"
UPDATE patients
SET pat_sam_rem='{0}'
WHERE patients.pat_id='{1}'", pat_sam_rem, pat_id);
                helper.ExecuteNonQuery(sqlUpdate);
            }
            catch (Exception ex)
            {

                return false;

            }
            return true;

        }


        public bool ResultExistVerify()
        {
            //string strSql = "select * from resulto where 1!=1";
            //DBHelper db = new DBHelper();
            //DataTable dtResult = db.GetTable(strSql);
            return false;// dtResult.Columns.Contains("res_verify");
        }
    }
}
