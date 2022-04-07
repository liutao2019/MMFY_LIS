using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public enum EnumOperationCode
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 审核
        /// </summary>
        Audit = 1,

        /// <summary>
        /// 报告
        /// </summary>
        Report = 2,

        /// <summary>
        /// 取消审核
        /// </summary>
        UndoAudit = 3,

        /// <summary>
        /// 取消报告
        /// </summary>
        UndoReport = 4,

        /// <summary>
        /// 中期报告
        /// </summary>
        [System.ComponentModel.Description("中期报告")]
        MidReport = 5,
        /// <summary>
        /// 预报告
        /// </summary>
        [System.ComponentModel.Description("预报告")]
        PreAudit = 6,

        /// <summary>
        /// 取消预报告
        /// </summary>
        [System.ComponentModel.Description("取消预报告")]
        UndoPreAudit = 7

    }
}
