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
    public class EntityReaDelivery : EntityBase
    {
        public EntityReaDelivery()
        {
            ListReaDeliveryDetail = new List<EntityReaDeliveryDetail>();
        }
        [FieldMapAttribute(ClabName = "Rdl_id", MedName = "Rdl_id", WFName = "Rdl_id",DBIdentity =true)]
        public System.Int64 Rdl_id { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_no", MedName = "Rdl_no", WFName = "Rdl_no")]
        public System.String Rdl_no { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_date", MedName = "Rdl_date", WFName = "Rdl_date")]
        public System.DateTime? Rdl_date { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_auditdate", MedName = "Rdl_auditdate", WFName = "Rdl_auditdate")]
        public System.DateTime? Rdl_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_operator", MedName = "Rdl_operator", WFName = "Rdl_operator")]
        public System.String Rdl_operator { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_auditor", MedName = "Rdl_auditor", WFName = "Rdl_auditor")]
        public System.String Rdl_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_status", MedName = "Rdl_status", WFName = "Rdl_status")]
        public System.String Rdl_status { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_printflag", MedName = "Rdl_printflag", WFName = "Rdl_printflag")]
        public System.Int32? Rdl_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32? del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_printid", MedName = "Rdl_printid", WFName = "Rdl_printid")]
        public System.String Rdl_printid { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_applyid", MedName = "Rdl_applyid", WFName = "Rdl_applyid")]
        public System.String Rdl_applyid { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_deptid", MedName = "Rdl_deptid", WFName = "Rdl_deptid")]
        public System.String Rdl_deptid { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_claimid", MedName = "Rdl_claimid", WFName = "Rdl_claimid")]
        public System.String Rdl_claimid { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_returnreason", MedName = "Rdl_returnreason", WFName = "Rdl_returnreason")]
        public System.String Rdl_returnreason { get; set; }
        [FieldMapAttribute(ClabName = "Rdl_srno", MedName = "Rdl_srno", WFName = "Rdl_srno")]
        public System.String Rdl_srno { get; set; }
        [FieldMapAttribute(ClabName = "OperatorName", MedName = "OperatorName", WFName = "OperatorName", DBColumn = false)]
        public System.String OperatorName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }

        /// <summary>
        /// 出库明细
        /// </summary>
        public List<EntityReaDeliveryDetail> ListReaDeliveryDetail { get; set; }
    }

}
