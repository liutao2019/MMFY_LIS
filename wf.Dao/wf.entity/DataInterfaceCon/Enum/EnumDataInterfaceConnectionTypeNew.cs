using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public enum EnumDataInterfaceConnectionTypeNew
    {
        /// <summary>
        /// SQL数据库(sql server/oracle/access/excel等)
        /// </summary>
        SQL,

        /// <summary>
        /// 标准WebService
        /// </summary>
        WebService,

        [Obsolete]
        WCF,

        /// <summary>
        /// .Net Dll引用
        /// </summary>
        DotNetDll,

        /// <summary>
        /// 二进制Dll引用
        /// </summary>
        BiniaryDll,

        /// <summary>
        /// 命令行命令
        /// </summary>
        DOSCommand,
    }
}
