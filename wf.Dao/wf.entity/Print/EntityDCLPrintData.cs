using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityDCLPrintData : EntityBase
    {
        public EntityDCLPrintData()
        {
            ReportSuffix = ".repx";
            ReportData = new DataSet();
        }

        /// <summary>
        /// 报表代码
        /// </summary>
        public String ReportCode { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public String ReportName { get; set; }

        /// <summary>
        /// 报表文件后缀
        /// </summary>
        public String ReportSuffix { get; }

        /// <summary>
        /// 报表数据
        /// </summary>
        public DataSet ReportData { set; get; }
    }
}
