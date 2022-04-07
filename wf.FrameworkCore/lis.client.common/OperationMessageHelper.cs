using System.Collections.Generic;
using System.Data;
using dcl.entity;

namespace dcl.client.common
{
    public class OperationMessageHelper
    {
        public static string GetErrorMessage(List<EntityOperationError> Errors)
        {
            string message = string.Empty;
            bool needNewLine = false;
            foreach (EntityOperationError err in Errors)
            {
                string msg = string.Empty;

                if (err.ErrorCode == EnumOperationErrorCode.IDNotExist)
                {
                    msg = "找不到病人资料，可能此病人信息已经删除，请刷新界面重新尝试";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.AuditDayExpire)
                {
                    msg = "超出取消2审时间:" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.HasCallBackMessage)
                {
                    msg = "召回病人;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ZeroItem)
                {
                    msg = "没有项目;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.AuditerAndReporterMustDiff)
                {
                    msg = string.Format("{0}者必须与{1}者不同;", LocalSetting.Current.Setting.AuditWord, LocalSetting.Current.Setting.ReportWord);
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotViewReportBeforeAudit)
                {
                    msg = "未查看报告单;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NullResult)
                {
                    message = "没有结果;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.SampleIsNull)
                {
                    msg = "没有标本类别;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.CustomCriticalValue)
                {
                    msg = "危急值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ItemsLost)
                {
                    msg = "缺少项目：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ItemNotNullLost)
                {
                    msg = "缺少必录项目：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.PositiveResultForBabyFilter)
                {
                    msg = "初筛阳性结果：" + err.Param + ";[须复查];";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.PositiveResultForBabyFilter2)
                {
                    msg = "二筛阳性结果：" + err.Param + ";[须电话病人复查];";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.RecheckExist)
                {
                    msg = "已报告复查：" + err.Param  ;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotFirstCheck)
                {
                    msg = "非初次筛查";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotFirstPos)
                {
                    msg = "初次筛查非阳性,无需复查";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.PositiveResult)
                {
                    msg = "阳性结果：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.overLineRange)
                {
                    msg = "超出线性范围值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.overReportRange)
                {
                    msg = "超出可报告范围值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.OverRef)
                {
                    msg = "超出参考值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.OverRef)
                {
                    msg = "超出参考值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.OverCritical)
                {
                    //系统配置：有超出危急值的项目时必须复查后才能报告  2014-1-13
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Audit_OverCritical_AndCallback") == "是"
                        && err.Param != null && err.Param.Contains("[须复查]"))
                    {
                        msg = "超出危急值：" + err.Param.Replace("[须复查]", "") + ";请复查危急值项目再报告;";
                    }
                    else
                    {
                        msg = "超出危急值：" + err.Param + ";";
                    }
                }
                else if (err.ErrorCode == EnumOperationErrorCode.OverThreshold)
                {
                    msg = "超出阈值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ChargeFall)
                {
                    msg = "本次确认失败，请查看费用清单";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.Audited)
                {
                    msg = "该记录已" + LocalSetting.Current.Setting.AuditWord + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.PreAudited)
                {
                    msg = "该记录已预审;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ZeroItem)
                {
                    msg = "没有项目;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.SIDExist)
                {
                    msg = "样本号已存在 ";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.HostOrderExist)
                {
                    msg = "序号已存在 ";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotAudit)
                {
                    msg = "该记录未" + LocalSetting.Current.Setting.AuditWord + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotPreAudit)
                {
                    msg = "该记录未预审;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NotReport)
                {
                    msg = "该记录未" + LocalSetting.Current.Setting.ReportWord + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.Reported)
                {
                    msg = "该记录已" + LocalSetting.Current.Setting.ReportWord + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.Printed)
                {
                    msg = "该记录已打印;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ItemSexNotConform)
                {
                    msg = "项目性别不符：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.DataTypeIncorrect)
                {
                    msg = "数据类型不匹配：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ExistedNotAllowValue)
                {
                    msg = "异常结果：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.Others)
                {
                    msg = err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.Exception)
                {
                    message = "操作失败：系统异常";
                    return message;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.QcDayExpire)
                {
                    msg = "质控项目:" + err.Param + "超过有效期,请重新做质控;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.QcOutOfControl)
                {
                    msg = "质控项目:" + err.Param + "失控;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.QcAddValue)
                {
                    msg = "新增质控项目:" + err.Param + "数据,请审核质控数据;";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ResultOverDiff)
                {
                    msg = "历史结果对比偏差：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ItrFault)
                {
                    msg = "仪器故障：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ItrMaintainDayExpire)
                {
                    msg = "仪器保养到期：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ResultLessThanZero)
                {
                    msg = "结果小于零：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NoHistoryOverRefUp)
                {
                    msg = "无历史数据，结果超出参考区间上限的%：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.NoHistoryOverRefDown)
                {
                    msg = "无历史数据，结果超出参考区间下限的%：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.ResultOverAuditRuleSet)
                {
                    msg = " 历史结果对比超出审核规则设定范围：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.CustomMessage)
                {
                    msg = err.CustomMessageTitle;
                    if (!string.IsNullOrEmpty(err.CustomMessageTitle))
                    {
                        msg = msg + "：";
                    }
                    msg = msg + err.Param;
                }
                else if (err.ErrorCode == EnumOperationErrorCode.PatinetRecheck)
                {
                    msg = "病人结果需复查！";
                }
                else if (err.ErrorCode == EnumOperationErrorCode.UndoSecondAuditOnlySelf)
                {
                    msg = "只有[帐号:" + err.Param + "]本人才能取消二审";
                }
                if (needNewLine)
                {
                    message = message + "\r\n" + msg;
                }
                else
                {
                    message = message + msg;
                }

                needNewLine = true;
            }
            return message;
        }

        public static string GetErrorMessage(List<EntityOperateError> Errors)
        {
            string message = string.Empty;
            bool needNewLine = false;
            foreach (EntityOperateError err in Errors)
            {
                string msg = string.Empty;

                if (err.ErrorCode == EnumOperateErrorCode.IDNotExist)
                {
                    msg = "找不到病人资料，可能此病人信息已经删除，请刷新界面重新尝试";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.AuditDayExpire)
                {
                    msg = "超出取消2审时间:" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.HasCallBackMessage)
                {
                    msg = "召回病人;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ZeroItem)
                {
                    msg = "没有项目;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.AuditerAndReporterMustDiff)
                {
                    msg = string.Format("{0}者必须与{1}者不同;", LocalSetting.Current.Setting.AuditWord, LocalSetting.Current.Setting.ReportWord);
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotViewReportBeforeAudit)
                {
                    msg = "未查看报告单;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NullResult)
                {
                    message = "没有结果;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.SampleIsNull)
                {
                    msg = "没有标本类别;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.CustomCriticalValue)
                {
                    msg = "危急值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ItemsLost)
                {
                    msg = "缺少项目：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ItemNotNullLost)
                {
                    msg = "缺少必录项目：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.PositiveResultForBabyFilter)
                {
                    msg = "初筛阳性结果：" + err.Param + ";[须复查];";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.PositiveResultForBabyFilter2)
                {
                    msg = "二筛阳性结果：" + err.Param + ";[须电话病人复查];";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.RecheckExist)
                {
                    msg = "已报告复查：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotFirstCheck)
                {
                    msg = "非初次筛查";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotFirstPos)
                {
                    msg = "初次筛查非阳性,无需复查";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.PositiveResult)
                {
                    msg = "阳性结果：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.overLineRange)
                {
                    msg = "超出线性范围值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.overReportRange)
                {
                    msg = "超出可报告范围值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.OverRef)
                {
                    msg = "超出参考值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.OverRef)
                {
                    msg = "超出参考值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.OverCritical)
                {
                    //系统配置：有超出危急值的项目时必须复查后才能报告  2014-1-13
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Audit_OverCritical_AndCallback") == "是"
                        && err.Param != null && err.Param.Contains("[须复查]"))
                    {
                        msg = "超出危急值：" + err.Param.Replace("[须复查]", "") + ";请复查危急值项目再报告;";
                    }
                    else
                    {
                        msg = "超出危急值：" + err.Param + ";";
                    }
                }
                else if (err.ErrorCode == EnumOperateErrorCode.OverThreshold)
                {
                    msg = "超出阈值：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ChargeFall)
                {
                    msg = "本次确认失败，请查看费用清单";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.Audited)
                {
                    msg = "该记录已" + LocalSetting.Current.Setting.AuditWord + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.PreAudited)
                {
                    msg = "该记录已预审;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ZeroItem)
                {
                    msg = "没有项目;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.SIDExist)
                {
                    msg = "样本号已存在 ";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.HostOrderExist)
                {
                    msg = "序号已存在 ";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotAudit)
                {
                    msg = "该记录未" + LocalSetting.Current.Setting.AuditWord + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotPreAudit)
                {
                    msg = "该记录未预审;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NotReport)
                {
                    msg = "该记录未" + LocalSetting.Current.Setting.ReportWord + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.Reported)
                {
                    msg = "该记录已" + LocalSetting.Current.Setting.ReportWord + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.Printed)
                {
                    msg = "该记录已打印;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ItemSexNotConform)
                {
                    msg = "项目性别不符：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.DataTypeIncorrect)
                {
                    msg = "数据类型不匹配：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ExistedNotAllowValue)
                {
                    msg = "异常结果：" + err.Param + ";";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.Others)
                {
                    msg = err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.Exception)
                {
                    message = "操作失败：系统异常";
                    return message;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.QcDayExpire)
                {
                    msg = "质控项目:" + err.Param + "超过有效期,请重新做质控;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.QcOutOfControl)
                {
                    msg = "质控项目:" + err.Param + "失控;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.QcAddValue)
                {
                    msg = "新增质控项目:" + err.Param + "数据,请审核质控数据;";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ResultOverDiff)
                {
                    msg = "历史结果对比偏差：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ItrFault)
                {
                    msg = "仪器故障：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ItrMaintainDayExpire)
                {
                    msg = "仪器保养到期：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ResultLessThanZero)
                {
                    msg = "结果小于零：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NoHistoryOverRefUp)
                {
                    msg = "无历史数据，结果超出参考区间上限的%：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.NoHistoryOverRefDown)
                {
                    msg = "无历史数据，结果超出参考区间下限的%：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.ResultOverAuditRuleSet)
                {
                    msg = " 历史结果对比超出审核规则设定范围：" + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.CustomMessage)
                {
                    msg = err.CustomMessageTitle;
                    if (!string.IsNullOrEmpty(err.CustomMessageTitle))
                    {
                        msg = msg + "：";
                    }
                    msg = msg + err.Param;
                }
                else if (err.ErrorCode == EnumOperateErrorCode.PatinetRecheck)
                {
                    msg = "病人结果需复查！";
                }
                else if (err.ErrorCode == EnumOperateErrorCode.UndoSecondAuditOnlySelf)
                {
                    msg = "只有[帐号:" + err.Param + "]本人才能取消二审";
                }
                if (needNewLine)
                {
                    message = message + "\r\n" + msg;
                }
                else
                {
                    message = message + msg;
                }

                needNewLine = true;
            }
            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkType">检查类型：0审核检查、1报告检查</param>
        /// <returns></returns>
        public static DataTable ExportToDataTable(EntityOperationResultList data, EnumOperationCode checkType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Selected", typeof(bool));
            dt.Columns.Add("PatSeq");
            dt.Columns.Add("PatID");
            dt.Columns.Add("PatName");
            dt.Columns.Add("PatBarcode");
            dt.Columns.Add("PatSex");
            dt.Columns.Add("PatAge");
            dt.Columns.Add("PatDepart");
            dt.Columns.Add("PatDiag");
            dt.Columns.Add("PatSID");
            dt.Columns.Add("Message");
            dt.Columns.Add("CheckSuccess", typeof(bool));
            dt.Columns.Add("CanContinue", typeof(bool));
            dt.Columns.Add("unSendUrg", typeof(bool));
            foreach (EntityOperationResult item in data)
            {
                DataRow dr = dt.NewRow();
                dr["PatName"] = item.Data.Patient.PidName;
                dr["PatID"] = item.Data.Patient.RepId;
                dr["PatSID"] = item.Data.Patient.RepSid;
                dr["PatSeq"] = item.Data.Patient.RepSerialNum;
                dr["PatBarcode"] = item.Data.Patient.RepBarCode;
                dr["PatSex"] = item.Data.Patient.PidSexExp;
                dr["PatAge"] = string.IsNullOrEmpty(item.Data.Patient.PidAgeExp) ? "" : item.Data.Patient.PidAgeExp.Replace("Y", "岁").Replace("M", "月").Replace("D", "天").Replace("H", "时").Replace("I", "分");
                dr["PatDepart"] = item.Data.Patient.PidDeptName;
                dr["PatDiag"] = item.Data.Patient.PidDiag;
                if (item.Success)
                {
                    dr["CheckSuccess"] = true;
                    dr["Selected"] = true;

                    if (checkType == EnumOperationCode.Audit)
                    {
                        //dr["Message"] = "可以继续审核";
                        dr["Message"] = "可以继续" + LocalSetting.Current.Setting.AuditWord;
                    }
                    else if (checkType == EnumOperationCode.Report)
                    {
                        //dr["Message"] = "可以继续报告";
                        dr["Message"] = "可以继续" + LocalSetting.Current.Setting.ReportWord;
                    }
                    else if (checkType == EnumOperationCode.UndoReport)
                    {
                        dr["Message"] = "可以继续取消二审";
                    }
                }
                else
                {
                    dr["CheckSuccess"] = false;
                    dr["Selected"] = false;
                    dr["Message"] = GetErrorMessage(item.Message);
                }

                dr["CanContinue"] = item.CanContinue;
                dr["unSendUrg"] = false;
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
