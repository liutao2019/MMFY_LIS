using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// 数据访问方式
    /// </summary>
    public enum EnumDataAccessMode
    {
        /// <summary>
        /// 直连数据库
        /// </summary>
        DirectToDB,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
}
