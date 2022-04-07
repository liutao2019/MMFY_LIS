using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 参考值、危急值、阈值偏高偏低标志
    /// </summary>
    [Flags]
    public enum EnumResRefFlag
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow = -1,

        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 负数
        /// </summary>
        //Negative = 1,

        /// <summary>
        /// 阳性
        /// </summary>
        Positive = 3,

        /// <summary>
        /// 弱阳性
        /// </summary>
        WeaklyPositive = 4,

        /// <summary>
        /// 出现不允许的结果
        /// </summary>
        ExistedNotAllowValues = 5,

        /// <summary>
        /// 自定义危急值
        /// </summary>
        CustomCriticalValue = 6,

        //注：从8开始使用'与'操作

        /// <summary>
        /// 高于参考值上限
        /// </summary>
        Greater1 = 8,

        /// <summary>
        /// 高于危急值上限
        /// </summary>
        Greater2 = 16,

        /// <summary>
        /// 高于阈值上限
        /// </summary>
        Greater3 = 32,

        /// <summary>
        /// 低于参考值下限
        /// </summary>
        Lower1 = 128,

        /// <summary>
        /// 低于危急值下限
        /// </summary>
        Lower2 = 256,

        /// <summary>
        /// 低于阈值下限
        /// </summary>
        Lower3 = 512,
    }
}
