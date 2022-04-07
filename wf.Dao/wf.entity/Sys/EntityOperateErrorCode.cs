using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作结果数据
    /// </summary>
    [Serializable]
    public enum EnumOperateErrorCode
    {
        /// <summary>
        /// 没有项目
        /// </summary>
        ZeroItem = 1,

        /// <summary>
        /// 缺少项目
        /// </summary>
        ItemsLost = 2,

        /// <summary>
        /// 该记录已审核
        /// </summary>
        Audited = 3,

        /// <summary>
        /// 该记录已报告
        /// </summary>
        Reported = 4,

        /// <summary>
        /// 该记录已打印
        /// </summary>
        Printed = 5,

        /// <summary>
        /// 程序异常
        /// </summary>
        Exception = 6,

        /// <summary>
        /// 病人ID不存在
        /// </summary>
        IDNotExist = 7,

        /// <summary>
        /// 该记录为原始记录
        /// </summary>
        Netural = 8,

        /// <summary>
        /// 其他
        /// </summary>
        Others = 9,

        /// <summary>
        /// 样本号已存在
        /// </summary>
        SIDExist = 10,

        /// <summary>
        /// 超出参考值
        /// </summary>
        OverRef = 11,

        /// <summary>
        /// 超出危机值
        /// </summary>
        OverCritical = 12,

        /// <summary>
        /// 超出阈值
        /// </summary>
        OverThreshold = 13,

        /// <summary>
        /// 条码号已存在
        /// </summary>
        BarCodeExist = 14,

        /// <summary>
        /// 超出审核期限
        /// </summary>
        AuditDayExpire = 15,

        /// <summary>
        /// 项目性别不符
        /// </summary>
        ItemSexNotConform = 16,

        /// <summary>
        /// 数据类型不匹配
        /// </summary>
        DataTypeIncorrect = 17,

        /// <summary>
        /// 未审核
        /// </summary>
        NotAudit = 18,

        /// <summary>
        /// 未报告
        /// </summary>
        NotReport = 19,

        /// <summary>
        /// 序号已存在
        /// </summary>
        HostOrderExist = 20,

        /// <summary>
        /// 缺少必录项目
        /// </summary>
        ItemNotNullLost = 21,

        /// <summary>
        /// 缺少样本类别
        /// </summary>
        SampleIsNull = 22,

        /// <summary>
        /// 没有结果(细菌)
        /// </summary>
        NullResult = 23,

        /// <summary>
        /// 当前病人有召回信息
        /// </summary>
        HasCallBackMessage = 24,

        /// <summary>
        /// 有质控项目超过有效期
        /// </summary>
        QcDayExpire = 25,

        /// <summary>
        /// 质控失控
        /// </summary>
        QcOutOfControl = 26,

        /// <summary>
        /// 新增质控数据
        /// </summary>
        QcAddValue = 27,

        /// <summary>
        /// 阳性结果
        /// </summary>
        PositiveResult = 28,

        ///// <summary>
        ///// 病人资料未选组合
        ///// </summary>
        //CombineIsEmpty = 29,

        /// <summary>
        /// 历史结果对比超出范围
        /// </summary>
        ResultOverDiff = 30,

        /// <summary>
        /// 仪器故障
        /// </summary>
        ItrFault = 31,

        /// <summary>
        /// 仪器保养到期
        /// </summary>
        ItrMaintainDayExpire = 32,

        /// <summary>
        /// 结果小于0
        /// </summary>
        ResultLessThanZero = 33,

        /// <summary>
        /// 出现异常结果
        /// </summary>
        ExistedNotAllowValue = 34,

        /// <summary>
        /// 审核前未查看报告
        /// </summary>
        NotViewReportBeforeAudit = 35,

        /// <summary>
        /// 一审与二审人必须不同
        /// </summary>
        AuditerAndReporterMustDiff = 36,

        /// <summary>
        /// 自定义消息
        /// </summary>
        CustomMessage = 37,

        /// <summary>
        /// 自定义危急值
        /// </summary>
        CustomCriticalValue = 38,

        /// <summary>
        /// 病人需复查
        /// </summary>
        PatinetRecheck = 39,

        /// <summary>
        /// 非二审者本人不可取消
        /// </summary>
        UndoSecondAuditOnlySelf = 40,

        /// <summary>
        /// 条码已回退
        /// </summary>
        HaveReturned = 41,

        /// <summary>
        /// 无历史数据，结果超出参考区间上下限的%
        /// </summary>
        NoHistoryOverRefUp = 42,
        /// <summary>
        /// 无历史数据，结果超出参考区间上下限的%
        /// </summary>
        NoHistoryOverRefDown = 44,

        /// <summary>
        /// 历史结果对比超出审核规则设定范围
        /// </summary>
        ResultOverAuditRuleSet = 43,

        /// <summary>
        /// 阳性结果
        /// </summary>
        PositiveResultForBabyFilter = 45,

        /// <summary>
        /// 阳性结果
        /// </summary>
        PositiveResultForBabyFilter2 = 46,

        /// <summary>
        /// 扣费失败
        /// </summary>
        ChargeFall = 47,

        /// <summary>
        /// 已复查
        /// </summary>
        RecheckExist = 48,

        /// <summary>
        /// 非第一次筛查
        /// </summary>
        NotFirstCheck = 49,

        /// <summary>
        /// 非第一次筛查阳性
        /// </summary>
        NotFirstPos = 50,


        /// <summary>
        /// 线性
        /// </summary>
        overLineRange = 51,

        /// <summary>
        /// 可报告范围
        /// </summary>
        overReportRange = 52,

        /// <summary>
        /// 未预审
        /// </summary>
        NotPreAudit = 53,
        /// <summary>
        /// 已预审
        /// </summary>
        PreAudited = 54,

    }
}
