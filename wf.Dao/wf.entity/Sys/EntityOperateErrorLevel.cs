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
    public enum EnumOperateErrorLevel
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
