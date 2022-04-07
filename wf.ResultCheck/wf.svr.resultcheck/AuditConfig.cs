using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class AuditConfig
    {
        public static EnumOperationErrorLevel GetOpErrLv(string name)
        {
            if (string.IsNullOrEmpty(name) || name == "无")
            {
                return EnumOperationErrorLevel.None;
            }

            if (name == "提示")
            {
                return EnumOperationErrorLevel.Message;
            }

            if (name == "警告")
            {
                return EnumOperationErrorLevel.Warn;
            }

            if (name == "错误")
            {
                return EnumOperationErrorLevel.Error;
            }

            return EnumOperationErrorLevel.None;
        }

        #region 参数

        public int SecondAuditUndoExpiredDays { get; set; }

        /// <summary>
        /// 二审发送危急值是否含[组合]
        /// </summary>
        public bool bSecondAuditUrgentContainCom { get; set; }

        /// <summary>
        /// 只有二审者可以取消二审
        /// </summary>
        public bool bSecondAuditUndoOnlySelf { get; set; }

        /// <summary>
        /// 取消二审时检查是否已阅读
        /// </summary>
        public bool bUndoAuditSecondCheckLookcode { get; set; }

        /// <summary>
        /// 审核检查与审核合并
        /// </summary>
        public bool MergeCheckWithAction { get; set; }

        /// <summary>
        /// 一步取消报告
        /// </summary>
        public bool OneStepCancelReport { get; set; }

        /// <summary>
        /// 允许跳过审核直接报告
        /// </summary>
        public bool bAllowStepAuditToReport { get; set; }

        /// <summary>
        /// 审核时是否写入体检危机值
        /// </summary>
        public bool Audit_SendTjCriticalToMsg { get; set; }

        /// <summary>
        /// 审核时取消病人召回信息
        /// </summary>
        public bool bCancelCallBackPatientOnAudit { get; set; }

        /// <summary>
        /// 启用病人召回
        /// </summary>
        public bool bAllowCallBackPatient { get; set; }

        /// <summary>
        /// 审核时是否能够插入必录缺省结果项目
        /// </summary>
        public bool bCanInsertDefultItemResult { get; set; }

        /// <summary>
        /// 查询历史结果是否只按姓名
        /// </summary>
        public bool bHistoryResultOnlySelectWithName { get; set; }

        /// <summary>
        /// 酶标类型结果使用数值结果判断偏高偏低
        /// </summary>
        public bool Lab_EiasaCheckItmResUseOdValue { get; set; }

        /// <summary>
        /// 查询历史结果字段（当"不按姓名为过滤条件时"此项才生效）
        /// </summary>
        public string strHistoryResultSelectField { get; set; }


        /// <summary>
        /// 设置每日单审时间段,格式(17:30,08:00)
        /// </summary>
        public string OneStepAuditTimeZone { get; set; }

        /// <summary>
        /// 需第三次审核的仪器ID
        /// </summary>
        public string Lab_ThreeAuditItrIDs { get; set; }

        /// <summary>
        /// 审核时判断该条码的所有组合是否已全部审核
        /// </summary>
        public bool Barcode_CheckCombineAllAudit { get; set; }

        /// <summary>
        /// 报告解读
        /// </summary>
        public bool Interpretation_Report { get; set; }
        /// <summary>
        /// 强制一审人与二审人必须不同
        /// </summary>
        public EnumOperationErrorLevel ForceAuditerAndReporterMustDiff { get; set; }

        /// <summary>
        /// 传染病阳性结果的报告不允许在门诊报告打印
        /// </summary>
        public bool IllReportNotAllowPrintMz { get; set; }

        /// <summary>
        /// 强制录入者与二审者必须不同
        /// </summary>
        public bool InputerAndReporterMustDiff { get; set; }

        /// <summary>
        /// 同一病人不同报告的相同项目结果对比的项目ID
        /// </summary>
        public string strSameItemResultContrastId { get; set; }

        /// <summary>
        /// 开启当前显示病人资料和数据库是否一致检查
        /// </summary>
        public bool CheckCurrentPatientInfo { get; set; }

        /// <summary>
        /// 效验公式只支持数字结果校验
        /// </summary>
        public bool CheckerClItemDealNumOnly { get; set; }


        /// <summary>
        /// 允许审核报告时修改录入者
        /// </summary>
        public bool Audit_AlloweditPat_i_code { get; set; }

        /// <summary>
        /// 二审时启用项目[线性范围]异常提示
        /// </summary>
        public bool Audit_EnableLineRangeWarning { get; set; }


        /// <summary>
        /// 添加系统配置：二审时启用项目[可报告范围]异常提示
        /// </summary>
        public bool Audit_EnableReportRangeWarning { get; set; }

        #region 审核错误等级配置

        /// <summary>
        ///1审错误等级：阳性结果
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_PositiveResult { get; set; }

        /// <summary>
        ///1审错误等级：结果类型不符
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_ResDataTypeError { get; set; }

        /// <summary>
        ///1审错误等级：超出阈值
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_OverThreshold { get; set; }

        /// <summary>
        ///1审错误等级：超出危急值
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_OverCritical { get; set; }

        /// <summary>
        ///1审错误等级：超出参考值
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_OverRef { get; set; }

        /// <summary>
        ///1审错误等级：仪器故障
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_ItrFalut { get; set; }

        /// <summary>
        ///1审错误等级：出现不允许的结果
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_ExistNotAllowValues { get; set; }

        /// <summary>
        ///1审错误等级：一审前未查看报告单
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_NotViewReportBeforeAudit { get; set; }

        /// <summary>
        ///2审错误等级：阳性结果
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_PositiveResult { get; set; }

        /// <summary>
        ///2审错误等级：结果类型不符
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_ResDataTypeError { get; set; }

        /// <summary>
        ///2审错误等级：超出阈值
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_OverThreshold { get; set; }

        /// <summary>
        ///2审错误等级：超出危急值
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_OverCritical { get; set; }

        /// <summary>
        ///2审错误等级：超出参考值
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_OverRef { get; set; }

        /// <summary>
        ///2审错误等级：仪器故障
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_ItrFalut { get; set; }

        /// <summary>
        ///2审错误等级：出现不允许的结果
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_ExistNotAllowValues { get; set; }

        /// <summary>
        ///2审错误等级：二审前未查看报告单
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_NotViewReportBeforeAudit { get; set; }

        /// <summary>
        ///取消2审错误等级：超出取消2审时间
        /// </summary>
        public EnumOperationErrorLevel UndoAudit_Second_ErrorLevel_DateExpired { get; set; }


        /// <summary>
        ///错误等级：检验报告时间检查
        /// </summary>
        public EnumOperationErrorLevel Audit_ReportTimeCheck { get; set; }
        /// <summary>
        /// 错误等级：负数结果检查
        /// </summary>
        public EnumOperationErrorLevel Audit_NegativeResultCheck { get; set; }
        /// <summary>
        /// 不进行负数结果检查的项目(项目id,项目id2)
        /// </summary>
        public List<string> Audit_AllowNegativeResult { get; set; }

        /// <summary>
        /// 不进行参考值范围结果检查的项目(项目id,项目id2)
        /// </summary>
        public List<string> Audit_AllowOverRefResult { get; set; }

        /// <summary>
        /// 不进行参考值范围结果检查的项目(项目id,项目id2)
        /// </summary>
        public List<string> Audit_AllowPosResult { get; set; }

        /// <summary>
        /// 错误等级：为零结果检查(配合进行为零结果检查仪器ID)
        /// </summary>
        public EnumOperationErrorLevel Audit_ItrZeroCheck { get; set; }



        /// <summary>
        ///1审错误等级：标本不匹配
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_NotAllowSample { get; set; }



        /// <summary>
        /// 进行为零结果检查的仪器(仪器id,仪器id2)
        /// </summary>
        public List<string> Audit_IncludeItrZeroCheck { get; set; }

        /// <summary>
        ///1审错误等级：历史结果对比
        /// </summary>
        public EnumOperationErrorLevel Audit_First_ErrorLevel_HistoryResultCompare { get; set; }

        /// <summary>
        ///2审错误等级：历史结果对比
        /// </summary>
        public EnumOperationErrorLevel Audit_Second_ErrorLevel_HistoryResultCompare { get; set; }
        /// <summary>
        /// 检验,采集,接收时间检查
        /// </summary>
        public EnumOperationErrorLevel Audit_PatSpcialTimeCheck { get; set; }
        /// <summary>
        /// 审核报警提示:标本状态包含字眼
        /// </summary>
        public string AuditTips_SampStatus { get; set; }

        #endregion

        #endregion

        public AuditConfig()
        {
            Audit_AllowNegativeResult = new List<string>();
            Audit_AllowOverRefResult = new List<string>();
            Audit_AllowPosResult = new List<string>();
            Audit_IncludeItrZeroCheck = new List<string>();
            this.SecondAuditUndoExpiredDays = 0;
        }
    }
}
