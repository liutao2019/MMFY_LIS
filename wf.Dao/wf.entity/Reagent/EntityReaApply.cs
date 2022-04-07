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
    public class EntityReaApply : EntityBase
    {
        public EntityReaApply()
        {
            ListReaApplyDetail = new List<EntityReaApplyDetail>();
        }
        [FieldMapAttribute(ClabName = "Ray_id", MedName = "Ray_id", WFName = "Ray_id", DBIdentity = true)]
        public System.String Ray_id { get; set; }
        [FieldMapAttribute(ClabName = "Ray_no", MedName = "Ray_no", WFName = "Ray_no")]
        public System.String Ray_no { get; set; }
        [FieldMapAttribute(ClabName = "Ray_date", MedName = "Ray_date", WFName = "Ray_date")]
        public System.DateTime? Ray_date { get; set; }
        [FieldMapAttribute(ClabName = "Ray_applier", MedName = "Ray_applier", WFName = "Ray_applier")]
        public System.String Ray_applier { get; set; }
        [FieldMapAttribute(ClabName = "Ray_auditor", MedName = "Ray_auditor", WFName = "Ray_auditor")]
        public System.String Ray_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Ray_applydate", MedName = "Ray_applydate", WFName = "Ray_applydate")]
        public System.DateTime? Ray_applydate { get; set; }
        [FieldMapAttribute(ClabName = "Ray_auditdate", MedName = "Ray_auditdate", WFName = "Ray_auditdate")]
        public System.DateTime? Ray_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Ray_status", MedName = "Ray_status", WFName = "Ray_status")]
        public System.String Ray_status { get; set; }
        [FieldMapAttribute(ClabName = "Ray_printflag", MedName = "Ray_printflag", WFName = "Ray_printflag")]
        public System.Int32 Ray_printflag { get; set; }
        [FieldMapAttribute(ClabName = "Ray_rejectreason", MedName = "Ray_rejectreason", WFName = "Ray_rejectreason")]
        public System.String Ray_rejectreason { get; set; }
        [FieldMapAttribute(ClabName = "Ray_remark", MedName = "Ray_remark", WFName = "Ray_remark")]
        public System.String Ray_remark { get; set; }
        [FieldMapAttribute(ClabName = "ApplierName", MedName = "ApplierName", WFName = "ApplierName",DBColumn =false)]
        public System.String ApplierName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }
        [FieldMapAttribute(ClabName = "ReturnName", MedName = "ReturnName", WFName = "ReturnName", DBColumn = false)]
        public System.String ReturnName { get; set; }
        /// <summary>
        /// 申领明细
        /// </summary>
        public List<EntityReaApplyDetail> ListReaApplyDetail { get; set; }
        [FieldMapAttribute(ClabName = "Ray_returndate", MedName = "Ray_returndate", WFName = "Ray_returndate")]
        public System.DateTime? Ray_returndate { get; set; }
        [FieldMapAttribute(ClabName = "Ray_returnid", MedName = "Ray_returnid", WFName = "Ray_returnid")]
        public System.String Ray_returnid { get; set; }
        [FieldMapAttribute(ClabName = "Ray_printdate", MedName = "Ray_printdate", WFName = "Ray_printdate")]
        public System.DateTime? Ray_printdate { get; set; }
        [FieldMapAttribute(ClabName = "Ray_printid", MedName = "Ray_printid", WFName = "Ray_printid")]
        public System.String Ray_printid { get; set; }
    }
}
