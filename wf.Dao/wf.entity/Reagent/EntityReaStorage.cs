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
    public class EntityReaStorage : EntityBase
    {
        public EntityReaStorage()
        {
            ListReaStorageDetail = new List<EntityReaStorageDetail>();
        }
        [FieldMapAttribute(ClabName = "Rsr_id", MedName = "Rsr_id", WFName = "Rsr_id", DBIdentity = true)]
        public System.Int64? Rsr_id { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_no", MedName = "Rsr_no", WFName = "Rsr_no")]
        public System.String Rsr_no { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_purno", MedName = "Rsr_purno", WFName = "Rsr_purno")]
        public System.String Rsr_purno { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_date", MedName = "Rsr_date", WFName = "Rsr_date")]
        public System.DateTime? Rsr_date { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_auditdate", MedName = "Rsr_auditdate", WFName = "Rsr_auditdate")]
        public System.DateTime? Rsr_auditdate { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_operator", MedName = "Rsr_operator", WFName = "Rsr_operator")]
        public System.String Rsr_operator { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_auditor", MedName = "Rsr_auditor", WFName = "Rsr_auditor")]
        public System.String Rsr_auditor { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_status", MedName = "Rsr_status", WFName = "Rsr_status")]
        public System.String Rsr_status { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_printflag", MedName = "Rsr_printflag", WFName = "Rsr_printflag")]
        public System.Int32? Rsr_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32? del_flag { get; set; }
        [FieldMapAttribute(ClabName = "out_flag", MedName = "out_flag", WFName = "out_flag")]
        public System.Int32? out_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_printid", MedName = "Rsr_printid", WFName = "Rsr_printid")]
        public System.String Rsr_printid { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_printdate", MedName = "Rsr_printdate", WFName = "Rsr_printdate")]
        public System.DateTime? Rsr_printdate { get; set; }
        [FieldMapAttribute(ClabName = "Rsr_returnreason", MedName = "Rsr_returnreason", WFName = "Rsr_returnreason")]
        public System.String Rsr_returnreason { get; set; }
        [FieldMapAttribute(ClabName = "OperatorName", MedName = "OperatorName", WFName = "OperatorName", DBColumn = false)]
        public System.String OperatorName { get; set; }
        [FieldMapAttribute(ClabName = "AuditorName", MedName = "AuditorName", WFName = "AuditorName", DBColumn = false)]
        public System.String AuditorName { get; set; }

        /// <summary>
        /// 入库明细
        /// </summary>
        public List<EntityReaStorageDetail> ListReaStorageDetail { get; set; }

        public string StorageMode { get; set; }
    }
}
