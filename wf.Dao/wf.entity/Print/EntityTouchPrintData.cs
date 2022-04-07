using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityTouchPrintData : EntityBase
    {

        /// <summary>
        ///标识ID 
        /// </summary>   
        public String RepId { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>   
        public String RepItrId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>   
        public String PidName { get; set; }

        /// <summary>
        /// 样本采集时间
        /// </summary>   
        public String SampCollectionDate { get; set; }

        /// <summary>
        /// 组合名称
        /// </summary>   
        public String PidComName { get; set; }

        /// <summary>
        /// 报告日期
        /// </summary>   
        public String RepReportDate { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>   
        public String RepStatus { get; set; }

        /// <summary>
        /// 标本类别
        /// </summary>  
        public String RepSamName { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>  
        public String RepBarCode { get; set; }
        
        /// <summary>
        /// PDF报告(Base64)
        /// </summary>
        public String RepPDF { get; set; }
    }
}
