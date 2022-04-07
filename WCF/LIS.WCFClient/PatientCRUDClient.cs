using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;
using lis.dto.Entity;
using dcl.pub.entities;
using System.Data;
using dcl.entity;

namespace dcl.client.wcf
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public class PatientCRUDClient : System.ServiceModel.ClientBase<IPatientCRUD>, IPatientCRUD
    {
        private PatientCRUDClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public static PatientCRUDClient NewInstance
        {
            get
            {
                string _endpointConfigurationName = "PatientCRUD";
                string _remoteAddress = ConfigurationSettings.AppSettings["wcfAddr"];
                string remoteAddress = _remoteAddress + ConfigurationSettings.AppSettings["svc.PatientCRUD"];

                PatientCRUDClient serviceClient = new PatientCRUDClient(_endpointConfigurationName, remoteAddress);
                return serviceClient;
            }
        }

        #region Create
        public EntityOperationResult InsertBarCodePatient(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return base.Channel.InsertBarCodePatient(caller, dsData);
        }

        public EntityOperationResult InsertBarCodePatientForSZ(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return base.Channel.InsertBarCodePatientForSZ(caller, dsData);
        }

        public EntityOperationResult InsertBarCodePatientWithSignIn(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return base.Channel.InsertBarCodePatientWithSignIn(caller, dsData);
        }

        /// <summary>
        /// 插入普通病人结果资料（涉及病人资料表，病人检验组合表，病人普通结果表）
        /// </summary>
        public EntityOperationResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return base.Channel.InsertPatCommonResult(caller, dsData);
        }

        public EntityOperationResult InsertPatDescResult(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData)
        {
            return base.Channel.InsertPatDescResult(caller,dsData);
        }



        public EntityOperationResult InsertPatCommonResultItems(string pat_id, System.Data.DataTable dtResulto)
        {
            return base.Channel.InsertPatCommonResultItems(pat_id, dtResulto);
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
            return base.Channel.AddOrUpdatePatientExt(colName, colValue, pat_id);
        }
        #endregion

        #region Read
        public System.Data.DataTable GetPatientStatus(DateTime date, string pat_itr_id)
        {
            return base.Channel.GetPatientStatus(date, pat_itr_id);
        }

        public DataTable GetPatientStatusForOverTime(DateTime date, string pat_itr_id)
        {
            return base.Channel.GetPatientStatusForOverTime(date, pat_itr_id);
        }

        public System.Data.DataSet GetPatientWithCombine(string pat_id)
        {
            return base.Channel.GetPatientWithCombine(pat_id);
        }


        public DataTable GetCombineFeeInfo(string pat_id)
        {
            return base.Channel.GetCombineFeeInfo(pat_id);
        }

        public System.Data.DataTable GetPatientCommonResult(string pat_id, bool getHistory)
        {
            return base.Channel.GetPatientCommonResult(pat_id, getHistory);
        }

        /// <summary>
        /// 获取细菌报告结果类型 无菌=0 or 细菌 = 1
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public int GetBacResultType(string pat_id)
        {
            return base.Channel.GetBacResultType(pat_id);
        }

        /// <summary>
        /// 获取普通结果病人资料（病人基本资料、病人检验组合、病人普通结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public System.Data.DataSet GetCommonPatinentData(string pat_id)
        {
            return base.Channel.GetCommonPatinentData(pat_id);

            //byte[] data = GetCommonPatientData_compress(pat_id);

            //System.Data.DataSet ds = dcl.common.DataSetSerialization.Decompress(data);

            //return ds;
        }
        /// <summary>
        /// 获取普通结果病人资料（病人基本资料、病人检验组合、病人普通结果）新生儿
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public System.Data.DataSet GetCommonPatinentDataForBf(string pat_id)
        {
            return base.Channel.GetCommonPatinentDataForBf(pat_id);

            //byte[] data = GetCommonPatientData_compress(pat_id);

            //System.Data.DataSet ds = dcl.common.DataSetSerialization.Decompress(data);

            //return ds;
        }
        //public byte[] GetCommonPatientData_compress(string pat_id)
        //{
        //    return base.Channel.GetCommonPatientData_compress(pat_id);
        //}

        /// <summary>
        /// 获取描述结果病人资料（病人基本资料、病人检验组合、病人描述结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public System.Data.DataSet GetDescPatientData(string patID)
        {
            return base.Channel.GetDescPatientData(patID);
        }


        /// <summary>
        /// 获取病人基本信息
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientInfo(string patID)
        {
            return base.Channel.GetPatientInfo(patID);
        }

        public System.Data.DataTable GetPatientInfoForBabyFilter(string patID)
        {
            return base.Channel.GetPatientInfoForBabyFilter(patID);
        }

        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientCombine(string patID)
        {
            return base.Channel.GetPatientCombine(patID);
        }

        /// <summary>
        /// 获取描述报告结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatDescResult(string patID)
        {
            return base.Channel.GetPatDescResult(patID);
        }

        /// <summary>
        /// 获取病人历史结果（病人资料、历史结果）
        /// </summary>
        /// <param name="pat_id">病人ID</param>
        /// <returns></returns>
        public System.Data.DataSet GetPatCommonResultHistoryWithRef(string patID, int resultCount, DateTime? patDate)
        {
            return base.Channel.GetPatCommonResultHistoryWithRef(patID, resultCount, patDate);
        }

        /// <summary>
        /// 获取病人图像结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatResultImage(string pat_id)
        {
            return base.Channel.GetPatResultImage(pat_id);
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
            return base.Channel.GetPatientsList_Details(dtFrom, dtTo, type_id, itr_id);
        }

        /// <summary>
        /// 获取条码信息中的注意事项(bc_exp)
        /// </summary>
        /// <param name="BcSQLWhere"></param>
        /// <returns></returns>
        public System.String GetBarcodeExpNotice(string BcSQLWhere)
        {
            return base.Channel.GetBarcodeExpNotice(BcSQLWhere);
        }
        #endregion

        #region Update

        public EntityOperationResult UpdatePatientResult_byResKey(EntityRemoteCallClientInfo userInfo, System.Data.DataTable dtPatientResult)
        {
            return base.Channel.UpdatePatientResult_byResKey(userInfo, dtPatientResult);
        }

        public EntityOperationResult UpdatePatientResultItems_byResID_itmID(EntityRemoteCallClientInfo userInfo, System.Data.DataTable dtPatientResult)
        {
            return base.Channel.UpdatePatientResultItems_byResID_itmID(userInfo, dtPatientResult);
        }

        public String CopyPatientsInfo(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            return base.Channel.CopyPatientsInfo(pat_id, dtTime, strItrId);
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
            return base.Channel.CopyPatientsInfoCustomSid(pat_id, dtTime, strItrId, newSid);
        }


        public String UpdatePatientsExt(DataTable dtPatExt)
        {
            return base.Channel.UpdatePatientsExt(dtPatExt);
        }


        public DataTable GetPatientsExt(String pat_id)
        {
            return base.Channel.GetPatientsExt(pat_id);
        }

        public DataTable GetBacPatientsMsg(string pat_id)
        {
            return base.Channel.GetBacPatientsMsg(pat_id);
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
            return base.Channel.DelPatCommonResult(caller, pat_id, delWithResult, canDeleteAudited);
        }

        /// <summary>
        /// 删除图像结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itm_ecd"></param>
        /// <returns></returns>
        public EntityOperationResult DeletePatPhotoResult(EntityRemoteCallClientInfo caller, string pat_id, string itm_ecd, long pres_key)
        {
            return base.Channel.DeletePatPhotoResult(caller, pat_id, itm_ecd, pres_key);
        }

        /// <summary>
        /// 删除resulto表记录
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteCommonResult(EntityRemoteCallClientInfo caller, IEnumerable<string> resKeys)
        {
            return base.Channel.DeleteCommonResult(caller, resKeys);
        }

        public EntityOperationResult DeleteCommonResultItem_byKey(EntityRemoteCallClientInfo caller, long res_key, string pat_id)
        {
            return base.Channel.DeleteCommonResultItem_byKey(caller, res_key, pat_id);
        }

        public EntityOperationResult DeleteCommonResultItem_byItemID(EntityRemoteCallClientInfo caller, string pat_id, string res_itm_id)
        {
            return base.Channel.DeleteCommonResultItem_byItemID(caller, pat_id, res_itm_id);
        }
        #endregion


        /// <summary>
        /// 从院网接口/条码接口获取病人信息，医嘱信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interfaceID"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public dcl.pub.entities.InterfacePatientInfo GetPatientFromInterface(string id, string interfaceID, NetInterfaceType interfaceType)
        {
            return base.Channel.GetPatientFromInterface(id, interfaceID, interfaceType);
        }




        public System.Data.DataTable GetPatHistoryExp(string pat_in_no)
        {
            return base.Channel.GetPatHistoryExp(pat_in_no);
        }

        /// <summary>
        /// 获取病人扩展表patients_ext的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientExtData(string pat_id)
        {
            return base.Channel.GetPatientExtData(pat_id);
        }

        /// <summary>
        /// 获取组合相关的无菌与涂片
        /// </summary>
        /// <param name="strComIDs"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDictNobactByComID(string strComIDs)
        {
            return base.Channel.GetDictNobactByComID(strComIDs);
        }

        /// <summary>
        /// 获取条码中间表bc_barcode_mid信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBCBarcodeMidData(string sqlWhere)
        {
            return base.Channel.GetBCBarcodeMidData(sqlWhere);
        }

        #region IPatientCRUD 成员

        //20120920
        public void insertBC_Remark(string login_id, string bc_name, string barCode, string departDisplayValue, string bar_sequence, DateTime registTime)
        {
            base.Channel.insertBC_Remark(login_id, bc_name, barCode, departDisplayValue, bar_sequence, registTime);
        }

        #endregion


        #region IPatientCRUD 成员


        public void insertBcSignNewCheckReport(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData, string BarcodeOperation, string pat_id)
        {
            base.Channel.insertBcSignNewCheckReport(caller, dsData, BarcodeOperation, pat_id);
        }

        public string getPatIdByPatName(string sql)
        {
            return base.Channel.getPatIdByPatName(sql);
        }

        public System.Data.DataTable doSearchFuncByLogin(string LoginId)
        {
            return base.Channel.doSearchFuncByLogin(LoginId);
        }

        public bool saveColumnSort(string sort)
        {
            return base.Channel.saveColumnSort(sort);
        }


        public bool UpdatePatientsSamRem(string pat_id, string pat_sam_rem)
        {
            return base.Channel.UpdatePatientsSamRem(pat_id, pat_sam_rem);
        }


        public bool savePatFunctionSet(string sort, string Visible)
        {
            return base.Channel.savePatFunctionSet(sort, Visible);
        }

        public string CopyPatientsInfoCustomForBf(List<string> pat_id, DateTime dtTime, string strItrId)
        {
            return base.Channel.CopyPatientsInfoCustomForBf(pat_id, dtTime, strItrId);
        }

        public EntityOperationResult InsertPatCommonResultForBf(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            return base.Channel.InsertPatCommonResultForBf(caller, dsData);
        }

        public string ManuaConfirmFee(string pat_id,string barcode, List<string> comIDs)
        {
            return base.Channel.ManuaConfirmFee(pat_id,barcode, comIDs);
        }

        #endregion
    }
}
