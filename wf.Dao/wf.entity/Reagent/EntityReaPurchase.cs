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
    public class EntityReaPurchase : EntityBase
    {
        public EntityReaPurchase()
        {
            ListReaPurchaseDetail = new List<EntityReaPurchaseDetail>();
        }
        [FieldMapAttribute(ClabName = "Rpc_id", MedName = "Rpc_id", WFName = "Rpc_id", DBIdentity = true)]
        public System.Int64 Rpc_id { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_no", MedName = "Rpc_no", WFName = "Rpc_no")]
        public System.String Rpc_no { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_date", MedName = "Rpc_date", WFName = "Rpc_date")]
        public System.DateTime? Rpc_date { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_auditdate", MedName = "Rpc_auditdate", WFName = "Rpc_auditdate")]
        public System.DateTime? Rpc_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_applier", MedName = "Rpc_applier", WFName = "Rpc_applier")]
        public System.String Rpc_applier { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_applydate", MedName = "Rpc_applydate", WFName = "Rpc_applydate")]
        public System.DateTime Rpc_applydate { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_auditor", MedName = "Rpc_auditor", WFName = "Rpc_auditor")]
        public System.String Rpc_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_status", MedName = "Rpc_status", WFName = "Rpc_status")]
        public System.String Rpc_status { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_printflag", MedName = "Rpc_printflag", WFName = "Rpc_printflag")]
        public System.Int32 Rpc_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_printid", MedName = "Rpc_printid", WFName = "Rpc_printid")]
        public System.String Rpc_printid { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_printdate", MedName = "Rpc_printdate", WFName = "Rpc_printdate")]
        public System.DateTime Rpc_printdate { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_returnid", MedName = "Rpc_returnid", WFName = "Rpc_returnid")]
        public System.String Rpc_returnid { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_returndate", MedName = "Rpc_returndate", WFName = "Rpc_returndate")]
        public System.DateTime Rpc_returndate { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_returnreason", MedName = "Rpc_returnreason", WFName = "Rpc_returnreason")]
        public System.String Rpc_returnreason { get; set; }
        [FieldMapAttribute(ClabName = "Rpc_remark", MedName = "Rpc_remark", WFName = "Rpc_remark")]
        public System.String Rpc_remark { get; set; }
        [FieldMapAttribute(ClabName = "ApplierName", MedName = "ApplierName", WFName = "ApplierName", DBColumn = false)]
        public System.String ApplierName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }
        [FieldMapAttribute(ClabName = "ReturnName", MedName = "ReturnName", WFName = "ReturnName", DBColumn = false)]
        public System.String ReturnName { get; set; }
        /// <summary>
        /// 采购明细
        /// </summary>
        public List<EntityReaPurchaseDetail> ListReaPurchaseDetail { get; set; }
    }
}
