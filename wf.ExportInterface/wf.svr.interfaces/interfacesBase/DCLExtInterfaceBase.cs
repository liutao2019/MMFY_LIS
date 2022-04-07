using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using Lib.DataInterface.Implement;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;

namespace dcl.svr.interfaces
{
    public abstract class DCLExtInterfaceBase
    {

        #region 私有辅助性方法

        /// <summary>
        /// 批量执行院网接口委托定义
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        internal delegate void BatchExecuteInterface_Delegate(EntitySampOperation operation, List<EntitySampMain> listSampMain);

        /// <summary>
        /// 执行院网接口委托定义
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        internal delegate void ExecuteInterface_Delegate(EntitySampOperation operation, EntitySampMain sampMain);

        /// <summary>
        /// 上传或取消报告委托定义
        /// </summary>
        /// <param name="listRepId"></param>
        internal delegate NameValueCollection UploadOrUndoDCLReport_Delegate(List<string> listRepId);

        /// <summary>
        /// 批量执行院网接口
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        private void BatchExecuteInterface(EntitySampOperation operation, List<EntitySampMain> listSampMain)
        {
            foreach (EntitySampMain sampMain in listSampMain)
            {
                ExecuteInterfaceAfter(operation, sampMain);
            }
        }

        /// <summary>
        /// 获取接口分组名称
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="PidSrcId"></param>
        /// <returns></returns>
        private String GetGroupName(String StatusId, String PidSrcId)
        {
            string result = string.Empty;

            string strOperationName = string.Empty;

            switch (StatusId)
            {
                case "0":
                    strOperationName = "下载";
                    break;
                case "1":
                    strOperationName = "打印";
                    break;
                case "2":
                    strOperationName = "采集";
                    break;
                case "5":
                    strOperationName = "签收";
                    break;
                case "9":
                    strOperationName = "回退";
                    break;
                case "500"://删除明细
                    strOperationName = "删除";
                    break;
                case "510"://删除所有明细（删除条码）
                    strOperationName = "删除";
                    break;
                default:
                    break;
            }

            if (strOperationName != string.Empty)
            {
                string strOriName = string.Empty;
                switch (PidSrcId)
                {
                    case "107":
                        strOriName = "门诊";
                        break;
                    case "108":
                        strOriName = "住院";
                        break;
                    case "109":
                        strOriName = "体检";
                        break;
                    default:
                        strOriName = "其他";
                        break;
                }

                result = string.Format("条码_{0}_{1}后", strOriName, strOperationName);
                result = GetSpecialGroupName(result, strOriName, strOperationName);
            }

            return result;
        }

        /// <summary>
        /// 特殊化分组名称
        /// </summary>
        /// <param name="strGroupName"></param>
        /// <param name="strOperationName"></param>
        /// <param name="strOriName"></param>
        /// <returns></returns>
        private String GetSpecialGroupName(String strGroupName, String strOperationName, String strOriName)
        {
            if (strOperationName == "删除条码" &&
                strOriName == "门诊" &&
                ConfigurationManager.AppSettings["HospitalType"] == "1")
            {
                strGroupName = string.Format("分院_条码_{0}_{1}后", strOriName, strOperationName);
            }
            return strGroupName;
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        internal bool SaveSysInterfaceLog(EntitySysInterfaceLog eySysInfeLog)
        {
            bool isSave = false;
            IDaoSysInterfaceLog dao = DclDaoFactory.DaoHandler<IDaoSysInterfaceLog>();
            if (dao != null)
            {
                isSave = dao.SaveSysInterfaceLog(eySysInfeLog);
            }
            return isSave;
        }

        /// <summary>
        /// 通过接口获取数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private List<EntityInterfaceData> GetInterfaceData(EntityInterfaceExtParameter parameter)
        {
            List<EntityInterfaceData> listInterfaceData = new List<EntityInterfaceData>();

            string interfaceType = parameter.GetInterfaceTypeString();

            List<string> listParam = parameter.GetDownloadParameter();

            if (!string.IsNullOrEmpty(interfaceType))
            {
                HISInterfacesBIZ his = new HISInterfacesBIZ();
                List<EntitySysItfInterface> listInterface = his.GetSysInterface(interfaceType);
                if (listInterface != null && listInterface.Count > 0)
                {
                    foreach (EntitySysItfInterface sysItfInterface in listInterface)
                    {
                        EntityInterfaceData result = new EntityInterfaceData(sysItfInterface.ItfaceId);

                        HospitalInterface interfaces = new HospitalInterface(
                        sysItfInterface.ItfaceServer,
                        sysItfInterface.ItfaceDatabase,
                        sysItfInterface.ItfaceLogid,
                        sysItfInterface.ItfacePassword,
                        sysItfInterface.ItfaceConnectType,
                        sysItfInterface.ItfaceExecuteSql);

                        if (parameter.OutlinkInterface)
                            result.InterfaceData = parameter.OutlinkData;
                        else
                            result.InterfaceData = interfaces.ExeInterface(listParam.ToArray());

                        listInterfaceData.Add(result);
                    }
                }
            }
            return listInterfaceData;
        }


        internal EntityQcResultList GetPidResult(string strRepId)
        {
            //Lib.LogManager.Logger.LogInfo("GetPidResult", "准备查询结果ID：" + strRepId);
            EntityQcResultList pidResult = null;

            try
            {
                IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                IDaoObrResultDesc descDao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
                IDaoObrResultAnti antiDao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
                IDaoObrResultBact bactDao = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
                //Lib.LogManager.Logger.LogInfo("GetPidResult", "获取DAO");
                if (dao != null &&
                    detailDao != null &&
                    resultDao != null &&
                    descDao != null &&
                    antiDao != null &&
                    bactDao != null)
                {

                    pidResult = new EntityQcResultList();

                    EntityPidReportMain patient = dao.GetPatientInfo(strRepId);
                    pidResult.listPatients.Add(patient);
                    pidResult.patient = patient;
                    //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取病人信息");
                    pidResult.listRepDetail = detailDao.GetPidReportDetailByRepId(strRepId);
                    //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取组合信息");
                    if (patient.ItrReportType == "3" || patient.ItrReportType == "4")
                    {
                        List<EntityObrResultDesc> listDesc = descDao.GetObrResultDescById(strRepId);
                        if (listDesc != null && listDesc.Count > 0)
                            pidResult.listDesc = listDesc;
                        //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取描述信息");
                        List<EntityObrResultAnti> listAnti = antiDao.GetAntiResultById(strRepId);
                        if (listAnti != null && listAnti.Count > 0)
                            pidResult.listAnti = listAnti;
                        //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取药敏信息");
                        List<EntityObrResultBact> listBact = bactDao.GetBactResultById(strRepId);
                        if (listBact != null && listBact.Count > 0)
                            pidResult.listBact = listBact;
                        //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取细菌信息");
                    }
                    else
                    {
                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(patient.RepId);
                        pidResult.listResulto = resultDao.ObrResultQuery(resultQc);//.OrderBy(w => w.ResComSeq).ThenBy(w => w.ItmSeq).ToList();
                        //Lib.LogManager.Logger.LogInfo("GetPidResult", "抓取结果信息");
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            //Lib.LogManager.Logger.LogInfo("GetPidResult", "返回结果信息");
            return pidResult;
        }
        internal List<EntitySampMain> GetPidSample(string strBarcode)
        {
            //Lib.LogManager.Logger.LogInfo("GetPidResult", "准备查询结果ID：" + strRepId);
            List<EntitySampMain> pidSampleList = null;

            try
            {
                IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
                //Lib.LogManager.Logger.LogInfo("GetPidResult", "获取DAO");
                if (dao != null)
                {
                    EntitySampQC sampQC = new EntitySampQC();
                    sampQC.ListSampBarId.Add(strBarcode);

                    pidSampleList = new List<EntitySampMain>();

                    pidSampleList = dao.GetSampMain(sampQC);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            //Lib.LogManager.Logger.LogInfo("GetPidResult", "返回结果信息");
            return pidSampleList;
        }
        #endregion

        /// <summary>
        /// 获取所有医嘱处方
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual List<EntitySampMain> DownloadOrderData(EntityInterfaceExtParameter parameter)
        {
            return null;
        }


        /// <summary>
        /// 获取医嘱数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual List<EntityInterfaceData> DownloadInterfaceData(EntityInterfaceExtParameter parameter)
        {
            return GetInterfaceData(parameter);
        }

        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual List<EntityInterfaceData> DownloadPatientInfo(EntityInterfaceExtParameter parameter)
        {
            return GetInterfaceData(parameter);
        }

        /// <summary>
        /// 操作前执行接口
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public virtual string ExecuteInterfaceBefore(EntitySampOperation operation, EntitySampMain sampMain)
        {
            return string.Empty;
        }

        /// <summary>
        /// 操作后执行接口基方法
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        internal virtual void ExecuteInterfaceAfter(EntitySampOperation operation, EntitySampMain sampMain)
        {
            string strGroupName = GetGroupName(operation.OperationStatus, sampMain.PidSrcId);

            if (strGroupName != string.Empty)
            {
                //获取最新的医嘱信息
                foreach (EntitySampDetail item in sampMain.ListSampDetail)
                {

                    if (!string.IsNullOrEmpty(item.OrderSn))
                    {
                        EntitySysInterfaceLog log = new EntitySysInterfaceLog();

                        log.SampBarId = sampMain.SampBarId;
                        log.OrderSn = item.OrderSn;
                        log.OperationName = string.Format("[{0}][{1}]", operation.OperationStatus, operation.OperationStatusName);
                        log.OperationUserCode = operation.OperationWorkId;
                        log.OperationUserName = operation.OperationName;
                        log.OperationTime = DateTime.Now;

                        try
                        {
                            //删除明细只传删除的医嘱
                            if (operation.OperationStatus == "500" && item.DelFlag != "1")
                                continue;

                            List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                            list.Add(new InterfaceDataBindingItem("bc_in_no", sampMain.PidInNo));
                            list.Add(new InterfaceDataBindingItem("bc_yz_id", item.OrderSn));
                            list.Add(new InterfaceDataBindingItem("bc_bar_code", sampMain.SampBarId));
                            list.Add(new InterfaceDataBindingItem("bc_his_code", item.OrderCode));
                            list.Add(new InterfaceDataBindingItem("bc_times", sampMain.PidAdmissTimes));
                            list.Add(new InterfaceDataBindingItem("bc_pid", sampMain.PidPatno));
                            list.Add(new InterfaceDataBindingItem("bc_emp_id", sampMain.PidExamNo));
                            list.Add(new InterfaceDataBindingItem("bc_social_no", sampMain.PidSocialNo));
                            list.Add(new InterfaceDataBindingItem("op_code", operation.OperationWorkId));
                            list.Add(new InterfaceDataBindingItem("op_name", operation.OperationName));
                            list.Add(new InterfaceDataBindingItem("bc_app_no", sampMain.SampApplyNo));

                            DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                            dihelper.ExecuteNonQueryWithGroup(strGroupName, list.ToArray());

                            log.OperationSuccess = 1;
                        }
                        catch (Exception ex)
                        {
                            Lib.LogManager.Logger.LogException(ex);

                            log.OperationSuccess = 0;
                            log.OperationContent = ex.ToString();
                        }
                        SaveSysInterfaceLog(log);
                    }
                }
            }
        }

        /// <summary>
        /// 操作后执行接口(异步)
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public void ExecuteInterfaceAfterAsync(EntitySampOperation operation, EntitySampMain sampMain)
        {
            ExecuteInterface_Delegate updel = new ExecuteInterface_Delegate(ExecuteInterfaceAfter);
            updel.BeginInvoke(operation, sampMain, null, null);
        }

        /// <summary>
        /// 批量执行院网接口
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="listSampMain"></param>
        public void BatchExecuteInterfaceAfter(EntitySampOperation operation, List<EntitySampMain> listSampMain)
        {
            BatchExecuteInterface_Delegate updel = new BatchExecuteInterface_Delegate(BatchExecuteInterface);
            updel.BeginInvoke(operation, listSampMain, null, null);
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public virtual AuditInfo IdentityVerification(AuditInfo userInfo)
        {
            EntityUserQc userQc = new EntityUserQc();

            userQc.LoginId = userInfo.UserId;
            userQc.Password = dcl.common.EncryptClass.Encrypt(userInfo.Password);

            List<EntitySysUser> listUser = new List<EntitySysUser>();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            { 
                listUser = dao.SysUserQuery(userQc);
            }
            if (listUser.Count > 0)
            {
                AuditInfo auditInfo = new AuditInfo();
                auditInfo.UserId = listUser[0].UserLoginid;
                auditInfo.UserName = listUser[0].UserName;
                auditInfo.UserStfId = listUser[0].UserIncode;
                return auditInfo;
            }
            else
                return null;
        }

        /// <summary>
        /// 上传报告（异步）
        /// </summary>
        /// <param name="listRepId"></param>
        public void UploadDCLReportAsync(List<String> listRepId)
        {
            UploadOrUndoDCLReport_Delegate updel = new UploadOrUndoDCLReport_Delegate(UploadDCLReport);
            updel.BeginInvoke(listRepId, null, null);
        }

        /// <summary>
        /// 上传报告基方法
        /// </summary>
        /// <param name="strRepId"></param>
        /// <returns></returns>
        internal virtual NameValueCollection UploadDCLReport(List<String> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_Enable") == "开")
            {
                foreach (string pat_id in listRepId)
                {
                    try
                    {
                        //Lis.SendDataToLisCDR.CDRService cds = new Lis.SendDataToLisCDR.CDRService();
                        //string ret = cds.UploadResult(pat_id);

                        //cds.UploadPdfResult(pat_id);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("检验数据上传慧扬CDR", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 上传中期报告（异步）
        /// </summary>
        /// <param name="listRepId"></param>
        public void UploadDCLMidReportAsync(List<String> listRepId)
        {
            UploadOrUndoDCLReport_Delegate updel = new UploadOrUndoDCLReport_Delegate(UploadDCLMidReport);
            updel.BeginInvoke(listRepId, null, null);
        }


        /// <summary>
        /// 上传中期报告基方法
        /// </summary>
        /// <param name="strRepId"></param>
        /// <returns></returns>
        internal virtual NameValueCollection UploadDCLMidReport(List<String> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_Enable") == "开")
            {
                foreach (string pat_id in listRepId)
                {
                    try
                    {
                        //Lis.SendDataToLisCDR.CDRService cds = new Lis.SendDataToLisCDR.CDRService();
                        //string ret = cds.UploadResult(pat_id);

                        //cds.UploadPdfResult(pat_id);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("检验数据上传慧扬CDR", ex);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 取消报告（异步）
        /// </summary>
        /// <param name="listRepId"></param>
        public void UndoUploadDCLReportAsync(List<String> listRepId)
        {
            UploadOrUndoDCLReport_Delegate updel = new UploadOrUndoDCLReport_Delegate(UndoUploadDCLReport);
            updel.BeginInvoke(listRepId, null, null);
        }

        /// <summary>
        /// 取消报告基方法
        /// </summary>
        /// <param name="strRepId"></param>
        /// <returns></returns>
        internal virtual NameValueCollection UndoUploadDCLReport(List<String> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_Enable") == "开")
            {
                foreach (string pat_id in listRepId)
                {
                    try
                    {
                        //Lis.SendDataToLisCDR.CDRService cds = new Lis.SendDataToLisCDR.CDRService();
                        //string ret = cds.UndoUploadResult(pat_id);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("检验数据上传CDR", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取医生信息
        /// </summary>
        /// <returns></returns>
        public virtual List<EntityDicDoctor> GetDoctorInfo()
        {
            List<EntityDicDoctor> listDoc = new List<EntityDicDoctor>();
            try
            {
                EntityInterfaceExtParameter parameter = new EntityInterfaceExtParameter();
                parameter.DownloadType = InterfaceType.DoctorInterface;

                List<EntityInterfaceData> listInterfaceData = GetInterfaceData(parameter);

                if (listInterfaceData != null && listInterfaceData.Count > 0)
                {
                    DataSet ds = listInterfaceData[0].InterfaceData;

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dtDoc = ds.Tables[0];
                        if (dtDoc.Rows.Count > 0)
                        {
                            //拿到接口的对照信息
                            ContrastDefineBIZ contBiz = new ContrastDefineBIZ();
                            List<EntitySysItfContrast> listContrast = contBiz.GetSysContrast(listInterfaceData[0].InterfaceID);

                            listDoc = EntityManager<EntityDicDoctor>.ConvertToList(dtDoc, listContrast);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listDoc;
        }

        /// <summary>
        /// 获取科室信息
        /// </summary>
        /// <returns></returns>
        public virtual List<EntityDicPubDept> GetDepartInfo()
        {
            List<EntityDicPubDept> listDoc = new List<EntityDicPubDept>();
            try
            {
                EntityInterfaceExtParameter parameter = new EntityInterfaceExtParameter();
                parameter.DownloadType = InterfaceType.DepartInterface;

                List<EntityInterfaceData> listInterfaceData = GetInterfaceData(parameter);

                if (listInterfaceData != null && listInterfaceData.Count > 0)
                {
                    DataSet ds = listInterfaceData[0].InterfaceData;

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dtDoc = ds.Tables[0];
                        if (dtDoc.Rows.Count > 0)
                        {
                            //拿到接口的对照信息
                            ContrastDefineBIZ contBiz = new ContrastDefineBIZ();
                            List<EntitySysItfContrast> listContrast = contBiz.GetSysContrast(listInterfaceData[0].InterfaceID);

                            listDoc = EntityManager<EntityDicPubDept>.ConvertToList(dtDoc, listContrast);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listDoc;
        }
        /// <summary>
        /// 发送危急值短信
        /// </summary>
        /// <param name="patInfo">病人信息</param>
        /// <param name="listUrgentResult">危急值结果</param>
        public virtual void SendUrgentMessage(EntityPidReportMain patInfo, List<EntityObrResult> listUrgentResult)
        {
            return;
        }

        /// <summary>
        /// 获取门诊病人报告信息 茂名妇幼输入门诊号直接生成报告信息
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public virtual List<EntityPidReportMain> GetMzPatients(EntityInterfaceExtParameter Parameter)
        {
            return new List<EntityPidReportMain>();
        }
    }
}
