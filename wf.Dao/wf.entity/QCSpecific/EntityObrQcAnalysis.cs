using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 
    /// 旧表名:obr_qc_analysis 新表名:Lis_qc_analysis
    /// </summary>
    public class EntityObrQcAnalysis : EntityBase
    {
        public EntityObrQcAnalysis()
        {

        }
        /// <summary>
        /// 主键
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_id", MedName = "qan_id", WFName = "Lqa_id")]
        public String QanId { get; set; }
        /// <summary>
        /// 仪器ID
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_itr_id", MedName = "qan_itr_id", WFName = "Lqa_Ditr_id")]
        public String QanItrId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_itm_id", MedName = "qan_itm_id", WFName = "Lqa_Ditm_id")]
        public String QanItmId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_date_start", MedName = "qan_date_start", WFName = "Lqa_date_start")]
        public DateTime QanDateStart { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_date_end", MedName = "qan_date_end", WFName = "Lqa_date_end")]
        public DateTime QanDateEnd { get; set; }

        /// <summary>
        /// 质控水平
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_level", MedName = "qan_level", WFName = "Lqa_level")]
        public String QanLevel { get; set; }


        /// <summary>
        /// 质控分析内容
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_ananlysis", MedName = "qan_ananlysis", WFName = "Lqa_ananlysis")]
        public String QanAnanlysis { get; set; }


        /// <summary>
        /// 审核者
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_audit_user_id", MedName = "qan_audit_user_id", WFName = "Lqa_audit_Buser_id")]
        public String QanAuditUserId { get; set; }

        /// <summary>
        /// 审核标志
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_audit_flag", MedName = "qan_audit_flag", WFName = "Lqa_audit_flag")]
        public Int32 QanAuditFlag { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [FieldMapAttribute(ClabName = "qan_audit_date", MedName = "qan_audit_date", WFName = "Lqa_audit_date")]
        public DateTime? QanAuditDate { get; set; }

        #region 审核人姓名
        /// <summary>
        /// 审核人姓名
        /// </summary>
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public string QanAuditUserName { get; set; }
        #endregion

        #region 仪器名称
        /// <summary>
        /// 仪器名称
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public string QanItrName { get; set; }
        #endregion

        #region 项目名称
        /// <summary>
        /// 项目名称
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn =false)]
        public string QanItmName { get; set; }
        #endregion
    }
}
