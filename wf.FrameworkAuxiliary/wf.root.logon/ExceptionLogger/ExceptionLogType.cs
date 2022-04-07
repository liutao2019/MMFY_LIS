using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.root.logon
{
    /// <summary>
    /// 日志类型
    /// </summary>
    internal enum ExceptionLogType
    {
        /// <summary>
        /// 普通跟踪信息
        /// </summary>
        Info,

        /// <summary>
        /// 异常
        /// </summary>
        Exception,

        /// <summary>
        /// 业务异常、错误
        /// </summary>
        BusinessError,

        /// <summary>
        /// 致命异常
        /// </summary>
        Fetal
    }
}
