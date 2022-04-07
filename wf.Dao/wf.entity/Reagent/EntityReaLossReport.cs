/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityReaLossReport : EntityBase
    {
        public EntityReaLossReport()
        {
            ListReaLossReportDetail = new List<EntityReaLossReportDetail>();
        }
        [FieldMapAttribute(ClabName = "Rlr_no", MedName = "Rlr_no", WFName = "Rlr_no")]
        public System.String Rlr_no { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_date", MedName = "Rlr_date", WFName = "Rlr_date")]
        public System.DateTime? Rlr_date { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_auditdate", MedName = "Rlr_auditdate", WFName = "Rlr_auditdate")]
        public System.DateTime? Rlr_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_operator", MedName = "Rlr_operator", WFName = "Rlr_operator")]
        public System.String Rlr_operator { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_auditor", MedName = "Rlr_auditor", WFName = "Rlr_auditor")]
        public System.String Rlr_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_status", MedName = "Rlr_status", WFName = "Rlr_status")]
        public System.String Rlr_status { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_printflag", MedName = "Rlr_printflag", WFName = "Rlr_printflag")]
        public System.Int32? Rlr_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32? del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_printid", MedName = "Rlr_printid", WFName = "Rlr_printid")]
        public System.String Rlr_printid { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_printdate", MedName = "Rlr_printdate", WFName = "Rlr_printdate")]
        public System.DateTime? Rlr_printdate { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_returnid", MedName = "Rlr_returnid", WFName = "Rlr_returnid")]
        public System.String Rlr_returnid { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_returndate", MedName = "Rlr_returndate", WFName = "Rlr_returndate")]
        public System.DateTime? Rlr_returndate { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_returnreason", MedName = "Rlr_returnreason", WFName = "Rlr_returnreason")]
        public System.String Rlr_returnreason { get; set; }
        [FieldMapAttribute(ClabName = "Rlr_remark", MedName = "Rlr_remark", WFName = "Rlr_remark")]
        public System.String Rlr_remark { get; set; }
        [FieldMapAttribute(ClabName = "ApplierName", MedName = "ApplierName", WFName = "ApplierName",DBColumn =false)]
        public System.String ApplierName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }
        [FieldMapAttribute(ClabName = "ReturnName", MedName = "ReturnName", WFName = "ReturnName", DBColumn = false)]
        public System.String ReturnName { get; set; }
        /// <summary>
        /// 申领明细
        /// </summary>
        public List<EntityReaLossReportDetail> ListReaLossReportDetail { get; set; }

    }
}
