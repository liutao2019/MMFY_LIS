using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    /// <summary>
    /// 数据库驱动类型枚举
    /// </summary>
    public enum EnumDbDriver
    {
        /// <summary>
        /// Microsoft SQL数据库专用
        /// </summary>
        MSSql,

        /// <summary>
        /// OLEDB驱动
        /// </summary>
        Oledb,

        /// <summary>
        /// ODBC驱动
        /// </summary>
        ODBC,

        /// <summary>
        /// Oracle驱动
        /// </summary>
        Oracle,
    }
}
