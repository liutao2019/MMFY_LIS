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
    public class EntityReaLossReportDetail : EntityBase
    {
        [FieldMapAttribute(ClabName = "Rld_no", MedName = "Rld_no", WFName = "Rld_no")]
        public System.String Rld_no { get; set; }
        [FieldMapAttribute(ClabName = "Rld_reaid", MedName = "Rld_reaid", WFName = "Rld_reaid")]
        public System.String Rld_reaid { get; set; }
        [FieldMapAttribute(ClabName = "Rld_reacount", MedName = "Rld_reacount", WFName = "Rld_reacount")]
        public System.Int32 Rld_reacount { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rld_id", MedName = "Rld_id", WFName = "Rld_id",DBIdentity =true)]
        public System.Int64 ObrSn { get; set; }
        [FieldMapAttribute(ClabName = "Rld_barcode", MedName = "Rld_barcode", WFName = "Rld_barcode")]
        public System.String Rld_barcode { get; set; }
        [FieldMapAttribute(ClabName = "Rld_remark", MedName = "Rld_remark", WFName = "Rld_remark")]
        public System.String Rld_remark { get; set; }
        [FieldMapAttribute(ClabName = "pdt_id", MedName = "pdt_id", WFName = "pdt_id")]
        public System.String pdt_id { get; set; }
        [FieldMapAttribute(ClabName = "sup_id", MedName = "sup_id", WFName = "sup_id")]
        public System.String sup_id { get; set; }
        [FieldMapAttribute(ClabName = "con_id", MedName = "con_id", WFName = "con_id")]
        public System.String con_id { get; set; }
        [FieldMapAttribute(ClabName = "unit_id", MedName = "unit_id", WFName = "unit_id")]
        public System.String unit_id { get; set; }
        [FieldMapAttribute(ClabName = "grp_id", MedName = "grp_id", WFName = "grp_id")]
        public System.String grp_id { get; set; }
        [FieldMapAttribute(ClabName = "pos_id", MedName = "pos_id", WFName = "pos_id")]
        public System.String pos_id { get; set; }
        [FieldMapAttribute(ClabName = "package", MedName = "package", WFName = "package")]
        public System.String package { get; set; }
        #region 附加字段 是否新增(可去掉特性)
        /// <summary>
        /// 是否新增
        /// </summary>
        [FieldMapAttribute(ClabName = "isnew", MedName = "isnew", WFName = "isnew", DBColumn = false)]
        public int IsNew { get; set; }
        #endregion
        [FieldMapAttribute(ClabName = "ReagentName", MedName = "ReagentName", WFName = "ReagentName", DBColumn = false)]
        public System.String ReagentName { get; set; }
        [FieldMapAttribute(ClabName = "ReagentPackage", MedName = "ReagentPackage", WFName = "ReagentPackage", DBColumn = false)]
        public System.String ReagentPackage { get; set; }
        [FieldMapAttribute(ClabName = "Rld_barstatus", MedName = "Rld_barstatus", WFName = "Rld_barstatus")]
        public System.Int32 Rld_barstatus { get; set; }
        [FieldMapAttribute(ClabName = "StoreNo", MedName = "StoreNo", WFName = "StoreNo")]
        public System.String StoreNo { get; set; }

    }
}
