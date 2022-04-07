using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using lis.dto.Entity;
using dcl.root.dac;
using dcl.root.logon;
using System.Data.SqlClient;
using lis.dto;
using System.Data;
using lis.dto.FieldsCorrespandance;
using dcl.common;
using dcl.svr.result.CRUD;
using lis.dto.BarCodeEntity;

using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.result
{
    public class PatDeleteBLL
    {
        #region 普通报告删除
        /// <summary>
        /// 删除普通病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatCommonResult(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult, bool canDeleteAudited)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除普通病人资料");
            //PatientEnterService bll = new PatientEnterService();

            opResult.Data.Patient.RepId = pat_id;

            PatCommonBll bll = new PatCommonBll();
            DataTable tablePatient = bll.GetPatientState(pat_id);

            if (tablePatient.Rows.Count > 0)
            {
                string pat_flag = tablePatient.Rows[0]["pat_flag"].ToString();
                string pat_bar_code = tablePatient.Rows[0]["pat_bar_code"].ToString();

                if (
                        (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                        || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                        || pat_flag == LIS_Const.PATIENT_FLAG.Printed
                        ) && !canDeleteAudited
                    )
                {
                    opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                }
                else
                {

                    //删除病人资料、病人组合日志类
                    OperationLogger opLogPat = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                    //删除病人结果日志类
                    OperationLogger opLogPatResult = null;

                    //获取取消条码上机状态语句
                    SqlCommand cmdDelPatbcInfo = GetDelPatientBarcodeInfoBcname(pat_id);

                    //获取删除语句
                    SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);
                    //获取删除病人扩展信息语句
                    SqlCommand cmdDelPatExtInfo = GetDelPatientExtInfoCMD(pat_id);

                    List<SqlCommand> listCmdDelPatCombine = GetDelPatCombineCMD(caller, pat_id);

                    //填充日志信息
                    LogDelPatInfo(pat_id, ref opLogPat);
                    LogDelPatCombine(pat_id, ref opLogPat);

                    if (delWithResult)//如果删除病人结果则生成删除病人结果日志类
                    {
                        opLogPatResult = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                        //填充删除病人结果日志信息
                        LogDelPatResult(pat_id, ref opLogPatResult);
                    }

                    try
                    {
                        using (DBHelper helper = DBHelper.BeginTransaction())//事务
                        {
                            helper.ExecuteNonQuery(cmdDelPatbcInfo);//取消条码上机状

                            helper.ExecuteNonQuery(cmdDelPatInfo);//删除病人资料

                            helper.ExecuteNonQuery(cmdDelPatExtInfo);//删除病人扩展资料

                            foreach (SqlCommand cmdCombine in listCmdDelPatCombine)
                            {
                                helper.ExecuteNonQuery(cmdCombine);//删除病人组合
                            }

                            if (delWithResult)
                            {
                                opLogPatResult.Log();//记录日志删除结果日志
                                SqlCommand cmdDelResult = GetDelPatCommonResultCMD(pat_id);//获取删除语句
                                helper.ExecuteNonQuery(cmdDelResult);//删除结果
                            }

                            //new PatCommonBll().UpdateBarcodeStatus(caller, EnumOperationCode.DeletePatient.ToString(), pat_bar_code, string.Empty, DateTime.Now, helper);

                            helper.Commit();//提交事务

                            opLogPat.Log();//记录日志：删除病人资料、删除病人组合
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("DelPatCommonResult({0},{1})", pat_id, delWithResult), ex.ToString());
                        opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    }
                }
            }
            return opResult;
        }

        /// <summary>
        /// 批量删除普通病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatCommonResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult, bool canDeleteAudited)
        {
            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();

            foreach (string pat_id in listPatID)
            {
                EntityOperationResult opResult = DelPatCommonResult(caller, pat_id, delWithResult, canDeleteAudited);
                listOpResult.Add(opResult);
                //EntityOperationResult opResult = EntityOperationResult.GetNew("删除病人资料");
                //opResult.Key = pat_id;

                //PatCommonBll bll = new PatCommonBll();

                //DataTable tablePatient = bll.GetPatientState(pat_id);
                //if (tablePatient.Rows.Count == 0)
                //{
                //    continue;
                //}

                //string pat_flag = tablePatient.Rows[0]["pat_flag"].ToString();
                //string pat_bar_code = tablePatient.Rows[0]["pat_bar_code"].ToString();

                //if (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                //    || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                //    || pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                //{
                //    opResult.AddError(EnumOperationErrorCode.Audited, "该记录已审核");
                //}
                //else
                //{
                //    try
                //    {
                //        SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);

                //        List<SqlCommand> listCmdDelPatCombine = GetDelPatCombineCMD(pat_id);

                //        using (DBHelper helper = DBHelper.BeginTransaction())
                //        {
                //            helper.ExecuteNonQuery(cmdDelPatInfo);

                //            foreach (SqlCommand cmdCombine in listCmdDelPatCombine)
                //            {
                //                helper.ExecuteNonQuery(cmdCombine);
                //            }

                //            if (delWithResult)
                //            {
                //                SqlCommand cmdDelResult = GetDelPatCommonResultCMD(pat_id);
                //                helper.ExecuteNonQuery(cmdDelResult);
                //            }
                //            helper.Commit();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Logger.WriteException(this.GetType().Name, string.Format("BatchDelPatCommonResult({0},{1})", pat_id, delWithResult), ex.ToString());
                //        opResult.AddError(EnumOperationErrorCode.Exception, ex.ToString());
                //    }
                //}
                //listOpResult.Add(opResult);
            }

            return listOpResult;
        }

        /// <summary>
        /// 删除resulto表记录
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResult(EntityRemoteCallClientInfo caller, IEnumerable<string> resKeys)
        {
            EntityOperationResult opResult = new EntityOperationResult();

            StringBuilder sb = new StringBuilder();
            OperationLogger opLog = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, string.Empty);
            foreach (string res_key in resKeys)
            {
                opLog.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_key, string.Empty);
                sb.Append(string.Format(",{0}", res_key));
            }
            sb.Remove(0, 1);

            string sql = string.Format("delete from resulto where res_key in ({0})", sb.ToString());

            try
            {
                //using (DBHelper helper = DBHelper.BeginTransaction())
                //{
                DBHelper helper = new DBHelper();
                helper.ExecuteNonQuery(sql);
                //helper.Commit();

                opLog.Log();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "DeleteCommonResult", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return opResult;
        }
        #endregion

        #region 描述报告删除
        /// <summary>
        /// 删除描述结果病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatDescResult(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除描述结果病人资料");

            SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);
            SqlCommand cmdDelDescResult = GetDelPatDescResultCMD(pat_id);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    helper.ExecuteNonQuery(cmdDelPatInfo);
                    if (delWithResult)
                    {
                        helper.ExecuteNonQuery(cmdDelDescResult);
                    }
                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("DelPatDescResult({0},{1})", pat_id, delWithResult), ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }

        /// <summary>
        /// 批量删除病人描述报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatDescResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult)
        {
            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();

            foreach (string pat_id in listPatID)
            {
                EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除描述结果病人资料");
                opResult.Data.Patient.RepId = pat_id;

                PatientEnterService bll = new PatientEnterService();

                string pat_flag = bll.GetPatientState(pat_id);

                if (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                    || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                    || pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                {
                    opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                }
                else
                {
                    try
                    {
                        //获取删除语句
                        SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);

                        List<SqlCommand> listCmdDelPatCombine = GetDelPatCombineCMD(caller, pat_id);

                        using (DBHelper helper = DBHelper.BeginTransaction())
                        {
                            helper.ExecuteNonQuery(cmdDelPatInfo);

                            foreach (SqlCommand cmdCombine in listCmdDelPatCombine)
                            {
                                helper.ExecuteNonQuery(cmdCombine);//删除病人组合
                            }

                            if (delWithResult)//如果同时删除结果
                            {
                                SqlCommand cmdDelResult = GetDelPatDescResultCMD(pat_id);
                                helper.ExecuteNonQuery(cmdDelResult);
                            }
                            helper.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("BatchDelPatDescResult({0},{1})", pat_id, delWithResult), ex.ToString());
                        opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    }
                }
                listOpResult.Add(opResult);
            }

            return listOpResult;
        }
        #endregion

        #region 细菌报告删除
        /// <summary>
        /// 删除细菌病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatBacResult(string pat_id, bool delWithResult)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除细菌结果病人资料");

            SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    helper.ExecuteNonQuery(cmdDelPatInfo);
                    if (delWithResult)
                    {
                        List<SqlCommand> listCmdDelDescResult = GetDelPatBacResultCMD(pat_id);
                        foreach (SqlCommand cmd in listCmdDelDescResult)
                        {
                            helper.ExecuteNonQuery(cmd);
                        }
                    }
                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("DelPatBacResult({0},{1})", pat_id, delWithResult), ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }

        /// <summary>
        /// 批量删除细菌病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BacthDelPatBacResult(List<string> listPatID, bool delWithResult)
        {
            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();

            foreach (string pat_id in listPatID)
            {
                EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除细菌结果病人资料");
                opResult.Data.Patient.RepId = pat_id;

                PatientEnterService bll = new PatientEnterService();

                string pat_flag = bll.GetPatientState(pat_id);

                if (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                    || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                    || pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                {
                    opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                }
                else
                {
                    try
                    {
                        using (DBHelper helper = DBHelper.BeginTransaction())
                        {
                            SqlCommand cmdDelPatInfo = GetDelPatInfoCMD(pat_id);
                            helper.ExecuteNonQuery(cmdDelPatInfo);
                            if (delWithResult)
                            {
                                List<SqlCommand> listCmdDelDescResult = GetDelPatBacResultCMD(pat_id);
                                foreach (SqlCommand cmd in listCmdDelDescResult)
                                {
                                    helper.ExecuteNonQuery(cmd);
                                }
                            }
                            helper.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("BacthDelPatBacResult({0},{1})", pat_id, delWithResult), ex.ToString());
                        opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    }
                }
                listOpResult.Add(opResult);
            }

            return listOpResult;
        }
        #endregion

        #region 普通报告删除ForBabyFilter
        /// <summary>
        /// 删除普通病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatCommonResultForBabyFilter(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult, bool canDeleteAudited)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除普通病人资料");
            //PatientEnterService bll = new PatientEnterService();

            opResult.Data.Patient.RepId = pat_id;

            PatCommonBll bll = new PatCommonBll();
            DataTable tablePatient = bll.GetPatientStateForBabyFilter(pat_id);

            if (tablePatient.Rows.Count > 0)
            {
                string pat_flag = tablePatient.Rows[0]["pat_flag"].ToString();
                string pat_bar_code = tablePatient.Rows[0]["pat_bar_code"].ToString();

                if (
                        (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                        || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                        || pat_flag == LIS_Const.PATIENT_FLAG.Printed
                        ) && !canDeleteAudited
                    )
                {
                    opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                }
                else
                {

                    //删除病人资料、病人组合日志类
                    OperationLogger opLogPat = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                    //删除病人结果日志类
                    OperationLogger opLogPatResult = null;

                    //获取删除语句
                    SqlCommand cmdDelPatInfo = GetDelPatInfoCMDForBabyFilter(pat_id);
                    //获取删除病人扩展信息语句
                    SqlCommand cmdDelPatExtInfo = GetDelPatientExtInfoCMD(pat_id);

                    List<SqlCommand> listCmdDelPatCombine = GetDelPatCombineCMDForBabyFilter(caller, pat_id);

                    //填充日志信息
                    LogDelPatInfo(pat_id, ref opLogPat);
                    LogDelPatCombine(pat_id, ref opLogPat);

                    if (delWithResult)//如果删除病人结果则生成删除病人结果日志类
                    {
                        opLogPatResult = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

                        //填充删除病人结果日志信息
                        LogDelPatResult(pat_id, ref opLogPatResult);
                    }

                    try
                    {
                        using (DBHelper helper = DBHelper.BeginTransaction())//事务
                        {
                            helper.ExecuteNonQuery(cmdDelPatInfo);//删除病人资料

                            helper.ExecuteNonQuery(cmdDelPatExtInfo);//删除病人扩展资料

                            foreach (SqlCommand cmdCombine in listCmdDelPatCombine)
                            {
                                helper.ExecuteNonQuery(cmdCombine);//删除病人组合
                            }

                            if (delWithResult)
                            {
                                opLogPatResult.Log();//记录日志删除结果日志
                                SqlCommand cmdDelResult = GetDelPatCommonResultCMD(pat_id);//获取删除语句
                                helper.ExecuteNonQuery(cmdDelResult);//删除结果
                            }

                            //new PatCommonBll().UpdateBarcodeStatus(caller, EnumOperationCode.DeletePatient.ToString(), pat_bar_code, string.Empty, DateTime.Now, helper);

                            helper.Commit();//提交事务

                            opLogPat.Log();//记录日志：删除病人资料、删除病人组合
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("DelPatCommonResult({0},{1})", pat_id, delWithResult), ex.ToString());
                        opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    }
                }
            }
            return opResult;
        }

        /// <summary>
        /// 批量删除普通病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatCommonResultForBabyFilter(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult, bool canDeleteAudited)
        {
            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();

            foreach (string pat_id in listPatID)
            {
                EntityOperationResult opResult = DelPatCommonResultForBabyFilter(caller, pat_id, delWithResult, canDeleteAudited);
                listOpResult.Add(opResult);
            }

            return listOpResult;
        }

        /// <summary>
        /// 删除resulto表记录
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResultForBabyFilter(EntityRemoteCallClientInfo caller, IEnumerable<string> resKeys)
        {
            EntityOperationResult opResult = new EntityOperationResult();

            StringBuilder sb = new StringBuilder();
            OperationLogger opLog = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, string.Empty);
            foreach (string res_key in resKeys)
            {
                opLog.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_key, string.Empty);
                sb.Append(string.Format(",{0}", res_key));
            }
            sb.Remove(0, 1);

            string sql = string.Format("delete from resulto_newborn where res_key in ({0})", sb.ToString());

            try
            {
                //using (DBHelper helper = DBHelper.BeginTransaction())
                //{
                DBHelper helper = new DBHelper();
                helper.ExecuteNonQuery(sql);
                //helper.Commit();

                opLog.Log();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "DeleteCommonResult", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return opResult;
        }

        /// <summary>
        /// 获取删除病人信息SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatInfoCMDForBabyFilter(string pat_id)
        {
            string sqlDelPatInfo = string.Format("delete from patients_newborn where pat_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatInfo);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除普通病人结果的SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatCommonResultCMDForBabyFilter(string pat_id)
        {
            string sqlDelPatResult = string.Format("delete from resulto_newborn where res_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatResult);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除病人结果项目的SqlCommand
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itm_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatCommonResultItemsCMDForBabyFilter(string pat_id, IEnumerable<string> itemsID)
        {
            SqlCommand cmd = new SqlCommand();
            if (itemsID.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string itm_id in itemsID)
                {
                    sb.Append(string.Format(",'{0}'", itm_id));
                }
                sb.Remove(0, 1);

                string sqlDelete = string.Format("delete from resulto_newborn where res_id = '{0}' and res_itm_id in ({1})", pat_id, sb.ToString());
                cmd.CommandText = sqlDelete;
            }
            else
            {
                string sqlDelete = string.Format("delete from resulto_newborn where 1=2");
                cmd.CommandText = sqlDelete;
            }
            return cmd;
        }

        /// <summary>
        /// 获取删除病人组合的SqlCommand
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public List<SqlCommand> GetDelPatCombineCMDForBabyFilter(EntityRemoteCallClientInfo caller, string pat_id)
        {
            List<SqlCommand> listCmd = new List<SqlCommand>();

            string sqlDel = string.Format("delete from patients_mi_newborn where pat_id='{0}'", pat_id);

            //如果病人资料由条码录入,则置bc_cname的bc_flag为0
            string sqlSelectBarcode = string.Format(@"select top 1 pat_bar_code from patients_newborn where pat_id = '{0}' ", pat_id);

            DBHelper helper = new DBHelper();
            object objBarcode = helper.ExecuteScalar(sqlSelectBarcode);

            DataTable result = new PatCommonBll().getPatients(pat_id);

            if (!Compare.IsEmpty(objBarcode))//由条码号
            {
                string barcode = objBarcode.ToString();

                string sqlSelect = string.Format(@"
select
pat_com_id,
dict_combine.com_name as pat_com_name
from patients_mi_newborn
left join dict_combine on com_id = patients_mi_newborn.pat_com_id
where pat_id = '{0}' and pat_com_id is not null", pat_id);

                DataTable dtPatCom = helper.GetTable(sqlSelect);

                string com_name = string.Empty;

                if (dtPatCom.Rows.Count > 0)
                {
                    bool needComma = false;
                    string sqlComWhereIn = string.Empty;
                    foreach (DataRow drPatCom in dtPatCom.Rows)
                    {
                        if (needComma)
                        {
                            sqlComWhereIn += ",";
                            com_name += "+";
                        }

                        sqlComWhereIn += string.Format("'{0}'", drPatCom["pat_com_id"]);
                        com_name += string.Format("{0}", drPatCom["pat_com_name"]);

                        needComma = true;
                    }

                    string update = string.Format("update bc_cname set bc_flag = 0 where bc_bar_code = '{0}' and bc_lis_code in ({1})", barcode, sqlComWhereIn);
                    SqlCommand cmdUpdateBcCname = new SqlCommand(update);
                    listCmd.Add(cmdUpdateBcCname);

                    //存在patients_mi的pat_com_id与bc_cname.bc_lis_code不对应的情况
                    string sqlUpdateBcFlag = string.Format(@"
update bc_cname set bc_flag = 0 where bc_bar_code = '{0}' and bc_id not in (
select bc_cname.bc_id
from
patients_mi_newborn with(nolock)
inner join patients_newborn with(nolock) on patients_newborn.pat_id = patients_mi_newborn.pat_id
inner join bc_cname with(nolock) on bc_cname.bc_bar_code = patients_newborn.pat_bar_code and patients_mi_newborn.pat_com_id = bc_cname.bc_lis_code
where patients_newborn.pat_bar_code = '{0}')
", barcode);

                    SqlCommand cmdUpdateBcCname2 = new SqlCommand(sqlUpdateBcFlag);
                    listCmd.Add(cmdUpdateBcCname2);

                    //helper.ExecuteNonQuery(cmdUpdateBcCname);
                }

                //List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), barcode, "组合：" + com_name, DateTime.Now);
                //***************************************************************************************************


                string remark = string.Empty;
                if (result != null && result.Rows.Count > 0)
                {
                    remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                    result.Rows[0]["pat_id"].ToString(), result.Rows[0]["pat_name"].ToString(), result.Rows[0]["com_name"],
                    result.Rows[0]["pat_in_no"].ToString(), result.Rows[0]["itr_name"].ToString(), result.Rows[0]["pat_sid"].ToString());
                }
                else
                    remark = "组合：" + com_name;

                List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), barcode, remark, ServerDateTime.GetDatabaseServerDateTime(), pat_id);

                if (cmdUpdateBarcode.Count > 0)
                {
                    listCmd.AddRange(cmdUpdateBarcode);
                }
            }
            //********此处为新增，当没有条码号时删除，一样要更新bc_sign表********************************//

            else
            {
                string remark = string.Empty;
                if (result != null && result.Rows.Count > 0)
                {
                    remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                    result.Rows[0]["pat_id"].ToString(), result.Rows[0]["pat_name"].ToString(), result.Rows[0]["com_name"],
                    result.Rows[0]["pat_in_no"].ToString(), result.Rows[0]["itr_name"].ToString(), result.Rows[0]["pat_sid"].ToString());
                }
                List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), "", remark, ServerDateTime.GetDatabaseServerDateTime(), pat_id);

                if (cmdUpdateBarcode.Count > 0)
                {
                    listCmd.AddRange(cmdUpdateBarcode);
                }
            }

            //*******************************************************************************************//

            SqlCommand cmdDel = new SqlCommand(sqlDel);
            listCmd.Add(cmdDel);

            return listCmd;
        }
        #endregion

        /// <summary>
        /// 删除图像结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itm_ecd"></param>
        /// <returns></returns>
        public EntityOperationResult DeletePatPhotoResult(EntityRemoteCallClientInfo userInfo, string pat_id, string itm_ecd, long pres_key)
        {
            EntityOperationResult opResult = new EntityOperationResult();
            try
            {
                string sqlDelete = string.Format("delete from resulto_p where pres_key={0}", pres_key);

                OperationLogger oplog = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);
                oplog.Add_DelLog(SysOperationLogGroup.PAT_IMAGERESULT, itm_ecd, string.Empty);

                //using (DBHelper helper = DBHelper.BeginTransaction())
                //{
                DBHelper helper = new DBHelper();
                helper.ExecuteNonQuery(sqlDelete);
                //helper.Commit();

                oplog.Log();
                //}

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("DeletePatPhotoResult({0},{1})", pat_id, itm_ecd), ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }


        


        #region 获取删除语句
        /// <summary>
        /// 获取删除病人信息SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatInfoCMD(string pat_id)
        {
            string sqlDelPatInfo = string.Format("delete from patients where pat_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatInfo);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除病人扩展信息SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatientExtInfoCMD(string pat_id)
        {
            string sqlDelPatientExtInfo = string.Format("delete from patients_ext where pat_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatientExtInfo);
            return cmdDel;
        }

        /// <summary>
        /// 获取取消病人信息的条码上机状态SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatientBarcodeInfoBcname(string pat_id)
        {
            string sqlDelPatientBarcodeInfoBcname = string.Format(@"if((select count(pat_id) from patients
            where pat_bar_code=(select top 1 pat_bar_code from patients 
            where pat_id='{0}' and pat_bar_code is not null and pat_bar_code<>''))=1)
            begin
               update bc_cname set bc_flag=0 where bc_flag=1 and bc_bar_code=(select top 1 pat_bar_code 
               from patients 
               where pat_id='{0}')
             end", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatientBarcodeInfoBcname);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除普通病人结果的SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatCommonResultCMD(string pat_id)
        {
            string sqlDelPatResult = string.Format("delete from resulto where res_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatResult);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除病人结果项目的SqlCommand
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itm_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatCommonResultItemsCMD(string pat_id, IEnumerable<string> itemsID)
        {
            SqlCommand cmd = new SqlCommand();
            if (itemsID.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string itm_id in itemsID)
                {
                    sb.Append(string.Format(",'{0}'", itm_id));
                }
                sb.Remove(0, 1);

                string sqlDelete = string.Format("delete from {0} where res_id = '{1}' and res_itm_id in ({2})", PatientTable.PatientResultTableName, pat_id, sb.ToString());
                cmd.CommandText = sqlDelete;
            }
            else
            {
                string sqlDelete = string.Format("delete from {0} where 1=2", PatientTable.PatientResultTableName);
                cmd.CommandText = sqlDelete;
            }
            return cmd;
        }

        /// <summary>
        /// 获取删除病人组合的SqlCommand
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public List<SqlCommand> GetDelPatCombineCMD(EntityRemoteCallClientInfo caller, string pat_id)
        {
            List<SqlCommand> listCmd = new List<SqlCommand>();

            string sqlDel = string.Format("delete from patients_mi where pat_id='{0}'", pat_id);

            //如果病人资料由条码录入,则置bc_cname的bc_flag为0
            string sqlSelectBarcode = string.Format(@"select top 1 pat_bar_code from patients where pat_id = '{0}' ", pat_id);

            DBHelper helper = new DBHelper();
            object objBarcode = helper.ExecuteScalar(sqlSelectBarcode);

            DataTable result = new PatCommonBll().getPatients(pat_id);

            if (!Compare.IsEmpty(objBarcode))//由条码号
            {
                string barcode = objBarcode.ToString();

                string sqlSelect = string.Format(@"
select
pat_com_id,
dict_combine.com_name as pat_com_name
from patients_mi
left join dict_combine on com_id = patients_mi.pat_com_id
where pat_id = '{0}' and pat_com_id is not null", pat_id);

                DataTable dtPatCom = helper.GetTable(sqlSelect);

                string com_name = string.Empty;

                if (dtPatCom.Rows.Count > 0)
                {
                    bool needComma = false;
                    string sqlComWhereIn = string.Empty;
                    foreach (DataRow drPatCom in dtPatCom.Rows)
                    {
                        if (needComma)
                        {
                            sqlComWhereIn += ",";
                            com_name += "+";
                        }

                        sqlComWhereIn += string.Format("'{0}'", drPatCom["pat_com_id"]);
                        com_name += string.Format("{0}", drPatCom["pat_com_name"]);

                        needComma = true;
                    }

                    string update = string.Format("update bc_cname set bc_flag = 0 where bc_bar_code = '{0}' and bc_lis_code in ({1})", barcode, sqlComWhereIn);
                    SqlCommand cmdUpdateBcCname = new SqlCommand(update);
                    listCmd.Add(cmdUpdateBcCname);

                    //存在patients_mi的pat_com_id与bc_cname.bc_lis_code不对应的情况
                    string sqlUpdateBcFlag = string.Format(@"
update bc_cname set bc_flag = 0 where bc_bar_code = '{0}' and bc_id not in (
select bc_cname.bc_id
from
patients_mi with(nolock)
inner join patients with(nolock) on patients.pat_id = patients_mi.pat_id
inner join bc_cname with(nolock) on bc_cname.bc_bar_code = patients.pat_bar_code and patients_mi.pat_com_id = bc_cname.bc_lis_code
where patients.pat_bar_code = '{0}')
", barcode);

                    SqlCommand cmdUpdateBcCname2 = new SqlCommand(sqlUpdateBcFlag);
                    listCmd.Add(cmdUpdateBcCname2);

                    //helper.ExecuteNonQuery(cmdUpdateBcCname);
                }

                //List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), barcode, "组合：" + com_name, DateTime.Now);
                //***************************************************************************************************

                
                string remark = string.Empty;
                if (result != null && result.Rows.Count > 0)
                {
                    remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                    result.Rows[0]["pat_id"].ToString(), result.Rows[0]["pat_name"].ToString(), result.Rows[0]["com_name"],
                    result.Rows[0]["pat_in_no"].ToString(), result.Rows[0]["itr_name"].ToString(), result.Rows[0]["pat_sid"].ToString());
                }
                else
                    remark = "组合：" + com_name;

                List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), barcode, remark, ServerDateTime.GetDatabaseServerDateTime(), pat_id);

                if (cmdUpdateBarcode.Count > 0)
                {
                    listCmd.AddRange(cmdUpdateBarcode);
                }
            }
            //********此处为新增，当没有条码号时删除，一样要更新bc_sign表********************************//

            else
            {
                string remark = string.Empty;
                if (result != null && result.Rows.Count > 0)
                {
                    remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                    result.Rows[0]["pat_id"].ToString(),result.Rows[0]["pat_name"].ToString(),result.Rows[0]["com_name"],
                    result.Rows[0]["pat_in_no"].ToString(),result.Rows[0]["itr_name"].ToString(),result.Rows[0]["pat_sid"].ToString());
                }
                List<SqlCommand> cmdUpdateBarcode = new PatCommonBll().GetUpdateBarcodeStatusCommand(caller, EnumBarcodeOperationCode.DeletePatient.ToString(), "", remark, ServerDateTime.GetDatabaseServerDateTime(), pat_id);

                if (cmdUpdateBarcode.Count > 0)
                {
                    listCmd.AddRange(cmdUpdateBarcode);
                }
            }

            //*******************************************************************************************//

            SqlCommand cmdDel = new SqlCommand(sqlDel);
            listCmd.Add(cmdDel);

            return listCmd;
        }


        /// <summary>
        /// 获取删除普通病人结果的SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public SqlCommand GetDelPatDescResultCMD(string pat_id)
        {
            string sqlDelPatDescResult = string.Format("delete from cs_rlts where bsr_id='{0}'", pat_id);
            SqlCommand cmdDel = new SqlCommand(sqlDelPatDescResult);
            return cmdDel;
        }

        /// <summary>
        /// 获取删除细菌病人结果的SQL语句
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public List<SqlCommand> GetDelPatBacResultCMD(string pat_id)
        {
            List<SqlCommand> list = new List<SqlCommand>();

            string sqlDel_cs_rlts = string.Format("delete from cs_rlts where bsr_id='{0}'", pat_id);
            SqlCommand cmdDel_cs_rlts = new SqlCommand(sqlDel_cs_rlts);
            list.Add(cmdDel_cs_rlts);

            string sqlDel_an_rlts = string.Format("delete from an_rlts where anr_id='{0}'", pat_id);
            SqlCommand cmdDel_an_rlts = new SqlCommand(sqlDel_an_rlts);
            list.Add(cmdDel_an_rlts);

            string sqlDel_ba_rlts = string.Format("delete from ba_rlts where bar_id='{0}'", pat_id);
            SqlCommand cmdDel_ba_rlts = new SqlCommand(sqlDel_ba_rlts);
            list.Add(cmdDel_ba_rlts);

            return list;
        }
        #endregion

        /// <summary>
        /// 删除普通结果项目
        /// </summary>
        /// <param name="res_key"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResultItem(EntityRemoteCallClientInfo caller, long res_key, string pat_id)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除普通病人资料");

            string sqlDelete = "delete from resulto where res_key = @res_key";

            SqlCommand cmdDelete = new SqlCommand(sqlDelete);
            cmdDelete.Parameters.AddWithValue("res_key", res_key);


            DataTable dtResult = PatReadBLL.NewInstance.GetPatientCommonResultByKey(res_key);

            OperationLogger opLog = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            if (dtResult.Rows.Count > 0)
            {
                string itm_ecd = dtResult.Rows[0]["res_itm_ecd"].ToString();
                string res_chr = dtResult.Rows[0]["res_chr"].ToString();

                opLog.Add_DelLog(SysOperationLogGroup.PAT_RESULT, itm_ecd, res_chr);
            }

            try
            {
                //using (DBHelper helper = DBHelper.BeginTransaction())
                //{
                DBHelper helper = new DBHelper();
                helper.ExecuteNonQuery(cmdDelete);

                //helper.Commit();
                opLog.Log();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "删除普通结果项目", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return opResult;
        }


        /// <summary>
        /// 删除普通结果项目
        /// </summary>
        /// <param name="res_key"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResultItem(EntityRemoteCallClientInfo caller, string pat_id, string res_itm_id)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除普通病人资料");

            string sqlDelete = string.Format("delete from resulto where res_id ='{0}' and res_itm_id='{1}'", pat_id, res_itm_id);


            DataTable dtResult = PatReadBLL.NewInstance.GetPatientCommonResultByItemID(pat_id, res_itm_id);

            OperationLogger opLog = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            if (dtResult.Rows.Count > 0)
            {
                string itm_ecd = dtResult.Rows[0]["res_itm_ecd"].ToString();
                string res_chr = dtResult.Rows[0]["res_chr"].ToString();

                opLog.Add_DelLog(SysOperationLogGroup.PAT_RESULT, itm_ecd, res_chr);
            }

            try
            {
                //using (DBHelper helper = DBHelper.BeginTransaction())
                //{
                DBHelper helper = new DBHelper();
                helper.ExecuteNonQuery(sqlDelete);

                //helper.Commit();
                opLog.Log();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "删除普通结果项目", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return opResult;
        }

        /// <summary>
        /// 获得记录删除病人资料的日志对象
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="opLog"></param>
        private void LogDelPatInfo(string pat_id, ref OperationLogger opLog)
        {
            DataTable dtPatInfo = PatReadBLL.NewInstance.GetPatientInfo(pat_id);
            if (dtPatInfo.Rows.Count > 0)
            {
                DataRow drPatInfo = dtPatInfo.Rows[0];
                foreach (DataColumn col in dtPatInfo.Columns)
                {
                    string colName = col.ColumnName;

                    string currValue = string.Empty;

                    if (!Compare.IsNullOrDBNull(drPatInfo[colName]))
                    {
                        currValue = drPatInfo[colName].ToString();
                    }

                    string colCHS = FieldsNameConventer<PatientFields>.Instance.GetFieldCHS(colName);

                    if (colCHS != string.Empty)//有中文的对照才记录日志
                    {
                        opLog.Add_DelLog(SysOperationLogGroup.PAT_INFO, colCHS, currValue);
                    }
                }
            }
        }

        /// <summary>
        /// 获得记录删除病人组合的日志对象
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="opLog"></param>
        private void LogDelPatCombine(string pat_id, ref OperationLogger opLog)
        {
            DataTable dtPatCombine = PatReadBLL.NewInstance.GetPatientCombine(pat_id);

            foreach (DataRow drPatCom in dtPatCombine.Rows)
            {
                string com_name = string.Empty;
                if (!Compare.IsNullOrDBNull(drPatCom["pat_com_name"]))
                {
                    com_name = drPatCom["pat_com_name"].ToString();
                }
                else if (!Compare.IsNullOrDBNull(drPatCom["pat_com_name"]))
                {
                    com_name = drPatCom["pat_com_id"].ToString();
                }

                if (com_name != string.Empty)
                {
                    opLog.Add_DelLog(SysOperationLogGroup.PAT_COMBINE, com_name, string.Empty);
                }
            }
        }

        /// <summary>
        /// 获得记录删除病人结果的日志对象
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="opLog"></param>
        private void LogDelPatResult(string pat_id, ref OperationLogger opLog)
        {
            DataTable dtResult = PatReadBLL.NewInstance.GetPatientCommonResult(pat_id, false);

            foreach (DataRow drResult in dtResult.Rows)
            {
                string itm_ecd = drResult["res_itm_ecd"].ToString();
                string res_chr = drResult["res_chr"].ToString();

                opLog.Add_DelLog(SysOperationLogGroup.PAT_RESULT, itm_ecd, res_chr);
            }
        }
    }
}
