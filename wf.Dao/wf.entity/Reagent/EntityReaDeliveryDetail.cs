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
    public class EntityReaDeliveryDetail : EntityBase
    {
        [FieldMapAttribute(ClabName = "Rdvd_no", MedName = "Rdvd_no", WFName = "Rdvd_no")]
        public System.String Rdvd_no { get; set; }
        [FieldMapAttribute(ClabName = "Rdvd_reaid", MedName = "Rdvd_reaid", WFName = "Rdvd_reaid")]
        public System.String Rdvd_reaid { get; set; }
        [FieldMapAttribute(ClabName = "Rdvd_reacount", MedName = "Rdvd_reacount", WFName = "Rdvd_reacount")]
        public System.Int32 Rdvd_reacount { get; set; }
        [FieldMapAttribute(ClabName = "Rdvd_count", MedName = "Rdvd_count", WFName = "Rdvd_count")]
        public System.Int32 Rdvd_count { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rdvd_id", MedName = "Rdvd_id", WFName = "Rdvd_id",DBIdentity =true)]
        public System.Int64 ObrSn { get; set; }
        [FieldMapAttribute(ClabName = "Rdvd_barcode", MedName = "Rdvd_barcode", WFName = "Rdvd_barcode")]
        public System.String Rdvd_barcode { get; set; }
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
        [FieldMapAttribute(ClabName = "Remark", MedName = "Remark", WFName = "Remark")]
        public System.String Remark { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        [FieldMapAttribute(ClabName = "isnew", MedName = "isnew", WFName = "isnew", DBColumn = false)]
        public int IsNew { get; set; }
        [FieldMapAttribute(ClabName = "ReagentName", MedName = "ReagentName", WFName = "ReagentName", DBColumn = false)]
        public System.String ReagentName { get; set; }
        [FieldMapAttribute(ClabName = "ReagentPackage", MedName = "ReagentPackage", WFName = "ReagentPackage", DBColumn = false)]
        public System.String ReagentPackage { get; set; }
        [FieldMapAttribute(ClabName = "PdtName", MedName = "PdtName", WFName = "PdtName", DBColumn = false)]
        public System.String PdtName { get; set; }
        [FieldMapAttribute(ClabName = "ApplyCount", MedName = "ApplyCount", WFName = "ApplyCount", DBColumn = false)]
        public System.Int32 ApplyCount { get; set; }
        [FieldMapAttribute(ClabName = "ApplyNo", MedName = "ApplyNo", WFName = "ApplyNo", DBColumn = false)]
        public System.String ApplyNo { get; set; }
        //入库单号
        [FieldMapAttribute(ClabName = "StoreNo", MedName = "StoreNo", WFName = "StoreNo")]
        public System.String StoreNo { get; set; }
    }
}
