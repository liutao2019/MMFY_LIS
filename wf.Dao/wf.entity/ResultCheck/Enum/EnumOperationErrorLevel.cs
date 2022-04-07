using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public enum EnumOperationErrorLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,
    }
}
