using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    public enum EnumDataInterfaceConverterType
    {
        None,

        /// <summary>
        /// 系统预置转换规则
        /// </summary>
        System,

        /// <summary>
        /// 对照转换
        /// </summary>
        Contrast,

        /// <summary>
        /// 数据接口
        /// </summary>
        DataInterface,

        /// <summary>
        /// 脚本转换
        /// </summary>
        Script,

        /// <summary>
        /// 插件dll转换
        /// </summary>
        PluginDll,
    }

    class ConverterSpecialValue
    {
        //public const string EMPTY_STRING = "@<emptystring>";
        public const string NULL = "@<null>";
        public const string DBNULL = "@<dbnull>";
        public const string DEFAULT = "@<default>";
    }
}
