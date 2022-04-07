using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.result;
using lis.dto.Entity;
using dcl.pub.entities;
using dcl.svr.result.CRUD;
using System.Data;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.wcf
{
    public class PatientCRUDService : WCFServiceBase, IPatientCRUD
    {
        public PatientCRUDService()
        {

        }


        #region IPatientCRUD 成员

        #region Create
        public EntityOperationResult InsertBarCodePatient(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertBarCodePatient(caller, dsData);
        }
        public EntityOperationResult InsertBarCodePatientForSZ(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertBarCodePatientForSZ(caller, dsData);
        }

        public EntityOperationResult InsertBarCodePatientWithSignIn(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertBarCodePatientWithSignIn(caller, dsData);
        }

        public EntityOperationResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertPatCommonResult(caller, dsData);
        }

        public EntityOperationResult InsertPatCommonResultForBf(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertPatCommonResultForBf(caller, dsData);
        }

        public string ManuaConfirmFee(string pat_id, string barcode, List<string> comIDs)
        {
            return new PatientEnterService().ManuaConfirmFee(pat_id, barcode, comIDs);
        }

        public EntityOperationResult InsertPatDescResult(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return new PatInsertBLL().InsertPatDescResult(caller, dsData);
        }

        public EntityOperationResult InsertPatCommonResultItems(string pat_id, System.Data.DataTable dtResulto)
        {
            return new PatInsertBLL().InsertPatCommonResultItems(pat_id, dtResulto);
        }
        /// <summary>
        /// 新增或更新病人扩展表patients_ext信息
        /// </summary>
        /// <param name="colName">指定的列名</param>
        /// <param name="colValue">对应的列值</param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult AddOrUpdatePatientExt(string[] colName, string[] colValue, string pat_id)
        {
            return new PatInsertBLL().AddOrUpdatePatientExt(colName, colValue, pat_id);
        }

        #endregion

        #region Read

        public System.Data.DataTable GetPatientStatus(DateTime date, string pat_itr_id)
        {
            return new PatientEnterService().GetPatientStatus(date, pat_itr_id);
        }

        public DataTable GetPatientStatusForOverTime(DateTime date, string pat_itr_id)
        {
            return new PatientEnterService().GetPatientStatusForOverTime(date, pat_itr_id);
        }


        public System.Data.DataSet GetPatientWithCombine(string pat_id)
        {
            return PatReadBLL.NewInstance.GetPatientWithCombine(pat_id);
        }

        public System.Data.DataTable GetCombineFeeInfo(string pat_id)
        {
            return PatReadBLL.NewInstance.GetCombineFeeInfo(pat_id);
        }

        public System.Data.DataTable GetPatientCommonResult(string pat_id, bool getHistory)
        {
            return PatReadBLL.NewInstance.GetPatientCommonResult(pat_id, getHistory);
        }

        /// <summary>
        /// 获取细菌报告结果类型 无菌=0 or 细菌 = 1
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public int GetBacResultType(string pat_id)
        {
            return PatReadBLL.NewInstance.GetBacResultType(pat_id);
        }

        public System.Data.DataSet GetCommonPatinentData(string patID)
        {
            return PatReadBLL.NewInstance.GetCommonPatinentData(patID);
        }
        public System.Data.DataSet GetCommonPatinentDataForBf(string patID)
        {
            return PatReadBLL.NewInstance.GetCommonPatinentDataForBf(patID);
        }
        //public byte[] GetCommonPatientData_compress(string pat_id)
        //{
        //    return PatReadBLL.NewInstance.GetCommonPatientData_compress(pat_id);
        //}

        public System.Data.DataSet GetDescPatientData(string patID)
        {
            return PatReadBLL.NewInstance.GetDescPatientData(patID);
        }

        public System.Data.DataTable GetPatientInfo(string patID)
        {
            return PatReadBLL.NewInstance.GetPatientInfo(patID);
        }

        public System.Data.DataTable GetPatientInfoForBabyFilter(string patID)
        {
            return PatReadBLL.NewInstance.GetPatientInfoForBf(patID);
        }

        public System.Data.DataTable GetPatientCombine(string patID)
        {
            return PatReadBLL.NewInstance.GetPatientCombine(patID);
        }

        public System.Data.DataTable GetPatDescResult(string patID)
        {
            return PatReadBLL.NewInstance.GetPatDescResult(patID);
        }

        public System.Data.DataSet GetPatCommonResultHistoryWithRef(string patID, int resultCount, DateTime? patDate)
        {
            return PatReadBLL.NewInstance.GetPatCommonResultHistoryWithRef(patID, resultCount, patDate);
        }

        /// <summary>
        /// 获取病人图像结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatResultImage(string pat_id)
        {
            return new PatReadBLL().GetPatResultImage(pat_id);
        }

        /// <summary>
        /// 获取病人列表(详细资料)
        /// 当(仪器=空 and 组别!=空)列出当前组别所有仪器的病人列表
        /// </summary>
        /// <param name="dtFrom">开始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">组别</param>
        /// <param name="itr_id">仪器</param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientsList_Details(DateTime dtFrom, DateTime dtTo, string type_id, string itr_id)
        {
            return new PatReadBLL().GetPatientsList_Details(dtFrom, dtTo, type_id, itr_id);
        }


        /// <summary>
        /// 从院网接口/条码接口获取病人信息，医嘱信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interfaceID"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public dcl.pub.entities.InterfacePatientInfo GetPatientFromInterface(string id, string interfaceID, NetInterfaceType interfaceType)
        {
            return new PatReadBLL().GetPatientFromInterface(id, interfaceID, interfaceType);
        }

        /// <summary>
        /// 获取病人扩展表patients_ext的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientExtData(string pat_id)
        {
            return new PatReadBLL().GetPatientExtData(pat_id);
        }

        /// <summary>
        /// 获取组合相关的无菌与涂片
        /// </summary>
        /// <param name="strComIDs"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDictNobactByComID(string strComIDs)
        {
            return new PatReadBLL().GetDictNobactByComID(strComIDs);
        }

        /// <summary>
        /// 获取条码中间表bc_barcode_mid信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBCBarcodeMidData(string sqlWhere)
        {
            return new PatReadBLL().GetBCBarcodeMidData(sqlWhere);
        }

        /// <summary>
        /// 获取条码信息中的注意事项(bc_exp)
        /// </summary>
        /// <param name="BcSQLWhere"></param>
        /// <returns></returns>
        public System.String GetBarcodeExpNotice(string BcSQLWhere)
        {
            return new PatReadBLL().GetBarcodeExpNotice(BcSQLWhere);
        }

        #endregion

        #region Update
        public EntityOperationResult UpdatePatientResult_byResKey(EntityRemoteCallClientInfo userInfo, System.Data.DataTable dtPatientResult)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.UpdatePatientResultItems_byResKey(userInfo, dtPatientResult);
        }

        public EntityOperationResult UpdatePatientResultItems_byResID_itmID(EntityRemoteCallClientInfo userInfo, System.Data.DataTable dtPatientResult)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.UpdatePatientResultItems_byResID_itmID(userInfo, dtPatientResult);
        }

        public string CopyPatientsInfo(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.CopyPatientsInfo(pat_id, dtTime, strItrId);
        }
        /// <summary>
        /// 复制患者信息(可指定目标样本号)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId">仪器ID</param>
        /// <param name="newSid">新样本号</param>
        /// <returns></returns>
        public string CopyPatientsInfoCustomSid(List<string> pat_id, DateTime dtTime, String strItrId, List<decimal> newSid)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.CopyPatientsInfoCustomSid(pat_id, dtTime, strItrId, newSid);
        }

        /// <summary>
        /// 复制患者信息(新生儿)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId">仪器ID</param>
        /// <returns></returns>
        public string CopyPatientsInfoCustomForBf(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.CopyPatientsInfoCustomForBf(pat_id, dtTime, strItrId);
        }
        #endregion

        #region Delete

        /// <summary>
        /// 删除普通病人资料(删除病人信息，病人组合，病人结果)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult">是否删除病人结果</param>
        /// <returns></returns>
        public EntityOperationResult DelPatCommonResult(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult, bool canDeleteAudited)
        {
            if (caller != null && caller.BabyFilterFlag)
            {
                return new PatDeleteBLL().DelPatCommonResultForBabyFilter(caller, pat_id, delWithResult, canDeleteAudited);
            }
            return new PatDeleteBLL().DelPatCommonResult(caller, pat_id, delWithResult, canDeleteAudited);
        }

        /// <summary>
        /// 删除图像结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itm_ecd"></param>
        /// <returns></returns>
        public EntityOperationResult DeletePatPhotoResult(EntityRemoteCallClientInfo caller, string pat_id, string itm_ecd, long pres_key)
        {
            return new PatDeleteBLL().DeletePatPhotoResult(caller, pat_id, itm_ecd, pres_key);
        }

        /// <summary>
        /// 删除resulto表记录
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResult(EntityRemoteCallClientInfo caller, IEnumerable<string> resKeys)
        {
            return new PatDeleteBLL().DeleteCommonResult(caller, resKeys);
        }

        public EntityOperationResult DeleteCommonResultItem_byKey(EntityRemoteCallClientInfo caller, long res_key, string pat_id)
        {
            return new PatDeleteBLL().DeleteCommonResultItem(caller, res_key, pat_id);
        }

        public EntityOperationResult DeleteCommonResultItem_byItemID(EntityRemoteCallClientInfo caller, string pat_id, string res_itm_id)
        {
            return new PatDeleteBLL().DeleteCommonResultItem(caller, pat_id, res_itm_id);
        }
        #endregion


        public System.Data.DataTable GetPatHistoryExp(string pat_in_no)
        {
            return new PatReadBLL().GetPatHistoryExp(pat_in_no);
        }

        #endregion

        #region IPatientCRUD 成员

        //20120920
        public void insertBC_Remark(string login_id, string bc_name, string barCode, string departDisplayValue, string bar_sequence, DateTime registTime)
        {
            //new dcl.svr.sample.BCPatientBIZ().insertBC_Remark(login_id, bc_name, barCode, departDisplayValue, bar_sequence, registTime);
        }

        #endregion

        #region IPatientCRUD 成员

        /// <summary>
        /// 将新增检验报告记录登记到bc_sign表中
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="dsData"></param>
        public void insertBcSignNewCheckReport(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData, string BarcodeOperation, string pat_id)
        {
            new PatInsertBLL().insertBcSignNewCheckReport(caller, dsData, BarcodeOperation, pat_id);
        }

        #endregion

        #region IPatientCRUD 成员


        /// <summary>
        /// 根据病人姓名或者ID查找已删除的操作记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string getPatIdByPatName(string sql)
        {
            return new PatCommonBll().getPatIdByPatName(sql);
        }

        #endregion

        #region IPatientCRUD 成员


        public System.Data.DataTable doSearchFuncByLogin(string LoginId)
        {
            return new PatCommonBll().doSearchFuncByLogin(LoginId);
        }

        #endregion

        #region IPatientCRUD 成员


        public bool saveColumnSort(string sort)
        {
            return new PatCommonBll().saveColumnSort(sort);
        }


        public bool savePatFunctionSet(string sort, string Visible)
        {
            return new PatCommonBll().savePatFunctionSet(sort, Visible);
        }
        #endregion

        #region IPatientCRUD 成员


        public bool UpdatePatientsSamRem(string pat_id, string pat_sam_rem)
        {
            return new PatUpdateBLL().UpdatePatientsSamRem(pat_id, pat_sam_rem);
        }

        #endregion

        #region IPatientCRUD 成员


        public string UpdatePatientsExt(System.Data.DataTable dtPatExt)
        {
            DataRow drPatExt = dtPatExt.Rows[0];

            string strUpdateSql = string.Empty;

            string pat_id = "";

            if (drPatExt["msg_update"].ToString() == "0")
            {
                pat_id = drPatExt["pat_id"].ToString();
                strUpdateSql = string.Format("insert into patients_ext (pat_id,msg_content,msg_doc_num,msg_doc_name,msg_dep_tel,msg_date,msg_register_loginId,msg_register_userName) values ('{0}','{1}','{2}','{3}','{4}',getdate(),'{5}','{6}')",
                                        drPatExt["pat_id"].ToString(),
                                        drPatExt["msg_content"].ToString(),
                                        drPatExt["msg_doc_num"].ToString(),
                                        drPatExt["msg_doc_name"].ToString(),
                                        drPatExt["msg_dep_tel"].ToString(),
                                        drPatExt["msg_register_loginId"].ToString(),
                                        drPatExt["msg_register_userName"].ToString());
            }
            else
            {
                pat_id = drPatExt["pat_id"].ToString();
                strUpdateSql = string.Format("update patients_ext set msg_content='{1}',msg_doc_num='{2}',msg_doc_name='{3}',msg_dep_tel='{4}',msg_register_loginId='{5}',msg_register_userName='{6}',msg_date=getdate() where pat_id='{0}'",
                                                        drPatExt["pat_id"].ToString(),
                                                        drPatExt["msg_content"].ToString(),
                                                        drPatExt["msg_doc_num"].ToString(),
                                                        drPatExt["msg_doc_name"].ToString(),
                                                        drPatExt["msg_dep_tel"].ToString(),
                                                        drPatExt["msg_register_loginId"].ToString(),
                                                        drPatExt["msg_register_userName"].ToString());
            }

            if (!string.IsNullOrEmpty(pat_id))
            {
                string sendType = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid");
                if (sendType == "惠侨CDR")
                {
                    try
                    {
                        Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();
                        DataSet ds = cdr.UploadDangerousValuesToTable(pat_id);
                    }
                    catch
                    {
                    }
                }
            }


            SqlHelper helper = new SqlHelper();

            //危急记录后,同时取消危急值内部提示
            //系统配置：危急值记录确认同时取消危急值内部提醒
            if (!string.IsNullOrEmpty(pat_id)
                && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_CriticalValueInfoAndUnInsgin") == "是")
            {
                string strUpdateSqlmsg_content=" update msg_content set msf_insgin_flag='1' where msg_ext1='"+pat_id+"' ";
                helper.ExecuteNonQuery(strUpdateSqlmsg_content);
            }

            return helper.ExecuteNonQuery(strUpdateSql).ToString();
        }

        public DataTable GetPatientsExt(string pat_id)
        {
            string strSql = string.Format(@"select pat_id,msg_content,msg_doc_num,msg_doc_name,msg_dep_tel,msg_date,msg_register_userName from patients_ext where pat_id='{0}'", pat_id);

            SqlHelper helper = new SqlHelper();

            DataTable dtPatExt = helper.GetTable(strSql);

            dtPatExt.TableName = "patients_ext";

            return dtPatExt;
        }

        public DataTable GetBacPatientsMsg(string pat_id)
        {
            string strSql = string.Format(@"select 
patients.pat_c_name res_itm_ecd,
msg_content.msg_ext3 res_chr,
'' history_result1,
'' res_unit,
msg_content.msg_ext4 res_exp 
from patients
left join msg_content on patients.pat_id=msg_content.msg_ext1
 where patients.pat_id='{0}'", pat_id);

            SqlHelper helper = new SqlHelper();

            DataTable dtPatExt = helper.GetTable(strSql);

            dtPatExt.TableName = "patients_msg";

            return dtPatExt;
        }

        #endregion
    }
}
