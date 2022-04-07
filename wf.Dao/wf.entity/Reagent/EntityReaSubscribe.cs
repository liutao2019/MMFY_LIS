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
    public class EntityReaSubscribe : EntityBase
    {
        public EntityReaSubscribe()
        {
            ListReaSubscribeDetail = new List<EntityReaSubscribeDetail>();
        }
        [FieldMapAttribute(ClabName = "Rsb_id", MedName = "Rsb_id", WFName = "Rsb_id", DBIdentity = true)]
        public System.Int64 Rsb_id { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_no", MedName = "Rsb_no", WFName = "Rsb_no")]
        public System.String Rsb_no { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_date", MedName = "Rsb_date", WFName = "Rsb_date")]
        public System.DateTime? Rsb_date { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_auditdate", MedName = "Rsb_auditdate", WFName = "Rsb_auditdate")]
        public System.DateTime? Rsb_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_applier", MedName = "Rsb_applier", WFName = "Rsb_applier")]
        public System.String Rsb_applier { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_applydate", MedName = "Rsb_applydate", WFName = "Rsb_applydate")]
        public System.DateTime Rsb_applydate { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_auditor", MedName = "Rsb_auditor", WFName = "Rsb_auditor")]
        public System.String Rsb_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_status", MedName = "Rsb_status", WFName = "Rsb_status")]
        public System.String Rsb_status { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_printflag", MedName = "Rsb_printflag", WFName = "Rsb_printflag")]
        public System.Int32 Rsb_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_printid", MedName = "Rsb_printid", WFName = "Rsb_printid")]
        public System.String Rsb_printid { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_printdate", MedName = "Rsb_printdate", WFName = "Rsb_printdate")]
        public System.DateTime Rsb_printdate { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_returnid", MedName = "Rsb_returnid", WFName = "Rsb_returnid")]
        public System.String Rsb_returnid { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_returndate", MedName = "Rsb_returndate", WFName = "Rsb_returndate")]
        public System.DateTime Rsb_returndate { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_returnreason", MedName = "Rsb_returnreason", WFName = "Rsb_returnreason")]
        public System.String Rsb_returnreason { get; set; }
        [FieldMapAttribute(ClabName = "Rsb_remark", MedName = "Rsb_remark", WFName = "Rsb_remark")]
        public System.String Rsb_remark { get; set; }
        [FieldMapAttribute(ClabName = "ApplierName", MedName = "ApplierName", WFName = "ApplierName", DBColumn = false)]
        public System.String ApplierName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }
        [FieldMapAttribute(ClabName = "ReturnName", MedName = "ReturnName", WFName = "ReturnName", DBColumn = false)]
        public System.String ReturnName { get; set; }
        /// <summary>
        /// 采购明细
        /// </summary>
        public List<EntityReaSubscribeDetail> ListReaSubscribeDetail { get; set; }
    }
}
