using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class WhonetDataExportArg
    {
        public WhonetDataExportArg()
        {
            this.CountryCode = "CHN";
        }

        /// <summary>
        /// 国家码：3位
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 实验室编码：3位
        /// </summary>
        public string LabCode { get; set; }

        /// <summary>
        /// 移除大于小于号
        /// </summary>
        public bool RemoveOperators { get; set; }

        /// <summary>
        /// true=导出药敏结果
        /// false=导出数值结果
        /// </summary>
        public bool ExportSIR { get; set; }
    }
}
