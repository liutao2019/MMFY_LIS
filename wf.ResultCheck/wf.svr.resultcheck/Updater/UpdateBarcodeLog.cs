using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;


using dcl.svr.cache;
using dcl.root.logon;
using System.Data;
using System.Threading;
using dcl.svr.sample;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 更新条码信息日志（bc_sign）
    /// </summary>
    public class UpdateBarcodeLog : AbstractAuditClass, IAuditUpdater
    {
        DateTime today;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info">病人基本信息</param>
        /// <param name="patientsMi"></param>
        /// <param name="config">审核配置</param>
        /// <param name="dtToday">当前操作时间由外部传进，确保批量操作与所有更新操作都为同一时间</param>
        /// <param name="auditType">审核类型</param>
        /// <param name="caller">操作者信息</param>
        public UpdateBarcodeLog(EntityPidReportMain pat_info, List<EntityPidReportDetail> patientsMi, AuditConfig config, DateTime dtToday, EnumOperationCode auditType, EntityRemoteCallClientInfo caller)
            : base(pat_info, patientsMi, null, auditType, config)
        {
            this.today = dtToday;
            this.Caller = caller;
        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            Thread t = new Thread(_Update);
            t.Start();

            string statusCode = null;
            if (auditType == EnumOperationCode.Audit)
            {
                statusCode = EnumBarcodeOperationCode.Audit.ToString();
            }
            else if (auditType == EnumOperationCode.Report)
            {
                statusCode = EnumBarcodeOperationCode.Report.ToString();
            }
            if (!string.IsNullOrEmpty(pat_info.RepBarCode) &&
                !string.IsNullOrEmpty(statusCode))
            {

                try
                {
                    //new TATRecordHelper().Record(pat_info.RepBarCode, statusCode, "", today.ToString());
                }
                catch
                { }
            }

        }

        public void _Update()
        {
            if (!string.IsNullOrEmpty(pat_info.RepBarCode))
            {
                try
                {
                    bool isCanUpdate = true;

                    string statusCode = null;//根据操作类型获得对于应操作的代码
                    if (auditType == EnumOperationCode.Audit)
                    {
                        statusCode = EnumBarcodeOperationCode.Audit.ToString();
                    }
                    else if (auditType == EnumOperationCode.Report)
                    {
                        statusCode = EnumBarcodeOperationCode.Report.ToString();
                    }
                    else if (auditType == EnumOperationCode.UndoAudit)
                    {
                        statusCode = EnumBarcodeOperationCode.UndoAudit.ToString();
                    }
                    else if (auditType == EnumOperationCode.UndoReport)
                    {
                        statusCode = EnumBarcodeOperationCode.UndoReport.ToString();
                    }
                    else if (auditType == EnumOperationCode.PreAudit)
                    {
                        statusCode = EnumBarcodeOperationCode.PreAudit.ToString();
                        isCanUpdate = false;
                    }
                    else if (auditType == EnumOperationCode.UndoPreAudit)
                    {
                        statusCode = EnumBarcodeOperationCode.UndoPreAudit.ToString();
                        isCanUpdate = false;
                    }
                    string remark = string.Empty;
                    if ((auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report) &&
                        config.Barcode_CheckCombineAllAudit)
                    {
                        try
                        {
                            string patflag = auditType == EnumOperationCode.Audit ? "1" : "2";

                            List<string> listcoms = new List<string>();
                            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                            if (detailDao != null)
                            {
                                listcoms = detailDao.GetPidReportDetailByBarcodeAndStatus(pat_info.RepBarCode, patflag);
                            }

                            StringBuilder sb = new StringBuilder();
                            List<string> list = new List<string>();
                            foreach (var mi in patients_mi)
                            {
                                list.Add(mi.ComId);
                            }

                            foreach (string row in listcoms)
                            {
                                list.Add(row);
                            }

                            object obj = new SampDetailBIZ().GetSampDetailCount(pat_info.RepBarCode, list);

                            if (obj != null && Convert.ToInt32(obj.ToString()) > 0)
                            {
                                isCanUpdate = false;
                                StringBuilder sb2 = new StringBuilder();
                                sb2.Append("本次审核组合:");
                                foreach (EntityPidReportDetail mi in patients_mi)
                                {
                                    EntityDicCombine ett_com = DictCombineCache.Current.GetCombineByID(mi.ComId, false);
                                    if (ett_com != null)
                                    {
                                        sb2.Append(string.Format("{0}; ", ett_com.ComName));
                                    }
                                }
                                remark = sb2.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException(this.GetType().Name, "Barcode_CheckCombineAllAudit", ex.ToString());
                        }
                    }
                    //插入反审原因
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_UndoReportRemark") == "是"
                        && statusCode == EnumBarcodeOperationCode.UndoReport.ToString())
                    {
                        remark = "反审原因：" + Caller.Remarks;
                    }

                    remark += string.Format("IP地址:{0}[{1}]", Caller.IPAddress, pat_info.RepAuditUserId);
                    bool result = false;
                    if (isCanUpdate)
                    //更新bc_patients的最后操作状态、最后操作时间
                    {
                        if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                        {
                            EntitySampOperation operation = new EntitySampOperation();
                            operation.OperationTime = today;
                            operation.OperationStatus = statusCode;
                            operation.OperationID = Caller.LoginID;
                            operation.OperationName = Caller.OperationName;
                            operation.OperationPlace = Caller.Location;
                            operation.OperationWorkId = Caller.OperatorSftId;
                            operation.Remark = remark;
                            operation.RepId = pat_info.RepId == null ? null : pat_info.RepId;
                            result = new SampMainBIZ().UpdateSampMainStatusByBarId(operation, pat_info.RepBarCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("UpdateBarcodeLog", ex);
                }
            }
            //****************************此处做了修改，为新增代码***********************************************/
            //倘若条码号没有的话，则只更新bc_sign表，不更新bc_patients表
            else
            {
                try
                {
                    string remark = string.Empty;

                    string statusCode = null;//根据操作类型获得对于应操作的代码
                    if (auditType == EnumOperationCode.Audit)
                    {
                        statusCode = EnumBarcodeOperationCode.Audit.ToString();
                    }
                    else if (auditType == EnumOperationCode.Report)
                    {
                        statusCode = EnumBarcodeOperationCode.Report.ToString();
                    }
                    else if (auditType == EnumOperationCode.UndoAudit)
                    {
                        statusCode = EnumBarcodeOperationCode.UndoAudit.ToString();
                    }
                    else if (auditType == EnumOperationCode.UndoReport)
                    {
                        statusCode = EnumBarcodeOperationCode.UndoReport.ToString();
                    }

                    //插入反审原因
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_UndoReportRemark") == "是"
                        && statusCode == EnumBarcodeOperationCode.UndoReport.ToString())
                    {
                        remark = "反审原因：" + Caller.Remarks;
                    }

                    remark += string.Format("IP地址:{0}", Caller.IPAddress);

                    //插入条码操作日志记录（bc_sign)
                    //****************************此处做了修改***********************************************/
                    EntitySampProcessDetail detail = new EntitySampProcessDetail();
                    detail.ProcDate = today;
                    detail.ProcUsercode = Caller.LoginID;
                    detail.ProcUsername = Caller.LoginName;
                    detail.ProcStatus = statusCode;
                    detail.ProcPlace = Caller.Location;
                    detail.ProcContent = remark;
                    detail.RepId = pat_info.RepId == null ? null : pat_info.RepId;
                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("UpdateBarcodeLog", ex);
                }
            }
            //*******************************************************************************************
        }

        #endregion
    }
}
