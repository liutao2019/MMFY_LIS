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
    public class EntityReaStorageDetail : EntityBase
    {
        /// <summary>
        ///唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "Rsd_id", MedName = "Rsd_id", WFName = "Rsd_id", DBIdentity = true)]
        public Int64 ObrSn { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_no", MedName = "Rsd_no", WFName = "Rsd_no")]
        public System.String Rsd_no { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_reaid", MedName = "Rsd_reaid", WFName = "Rsd_reaid")]
        public System.String Rsd_reaid { get; set; }
        //入库数量
        [FieldMapAttribute(ClabName = "Rsd_reacount", MedName = "Rsd_reacount", WFName = "Rsd_reacount")]
        public System.Int32 Rsd_reacount { get; set; }
        //当前数量
        [FieldMapAttribute(ClabName = "Rsd_count", MedName = "Rsd_count", WFName = "Rsd_count")]
        public System.Int32 Rsd_count { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_batchno", MedName = "Rsd_batchno", WFName = "Rsd_batchno")]
        public System.String Rsd_batchno { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_invoiceno", MedName = "Rsd_invoiceno", WFName = "Rsd_invoiceno")]
        public System.String Rsd_invoiceno { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_barcode", MedName = "Rsd_barcode", WFName = "Rsd_barcode")]
        public System.String Rsd_barcode { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_barcoderule", MedName = "Rsd_barcoderule", WFName = "Rsd_barcoderule")]
        public System.String Rsd_barcoderule { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_outerpacking", MedName = "Rsd_outerpacking", WFName = "Rsd_outerpacking")]
        public System.String Rsd_outerpacking { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_temperature", MedName = "Rsd_temperature", WFName = "Rsd_temperature")]
        public System.String Rsd_temperature { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_report", MedName = "Rsd_report", WFName = "Rsd_report")]
        public System.String Rsd_report { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32? del_flag { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_price", MedName = "Rsd_price", WFName = "Rsd_price")]
        public System.Decimal Rsd_price { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_totalprice", MedName = "Rsd_totalprice", WFName = "Rsd_totalprice")]
        public System.Decimal Rsd_totalprice { get; set; }
        [FieldMapAttribute(ClabName = "sort_no", MedName = "sort_no", WFName = "sort_no")]
        public System.String sort_no { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_groupid", MedName = "Rsd_groupid", WFName = "Rsd_groupid")]
        public System.String Rsd_groupid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_validdate", MedName = "Rsd_validdate", WFName = "Rsd_validdate")]
        public System.DateTime Rsd_validdate { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_pdtid", MedName = "Rsd_pdtid", WFName = "Rsd_pdtid")]
        public System.String Rsd_pdtid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_supid", MedName = "Rsd_supid", WFName = "Rsd_supid")]
        public System.String Rsd_supid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_unitid", MedName = "Rsd_unitid", WFName = "Rsd_unitid")]
        public System.String Rsd_unitid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_posid", MedName = "Rsd_posid", WFName = "Rsd_posid")]
        public System.String Rsd_posid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_conid", MedName = "Rsd_conid", WFName = "Rsd_conid")]
        public System.String Rsd_conid { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_package", MedName = "Rsd_package", WFName = "Rsd_package")]
        public System.String Rsd_package { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_status", MedName = "Rsd_status", WFName = "Rsd_status")]
        public System.String Rsd_status { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_purno", MedName = "Rsd_purno", WFName = "Rsd_purno")]
        public System.String Rsd_purno { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        [FieldMapAttribute(ClabName = "isnew", MedName = "isnew", WFName = "isnew", DBColumn = false)]
        public int IsNew { get; set; }
        [FieldMapAttribute(ClabName = "ReagentName", MedName = "ReagentName", WFName = "ReagentName", DBColumn = false)]
        public System.String ReagentName { get; set; }
        [FieldMapAttribute(ClabName = "SupName", MedName = "SupName", WFName = "SupName", DBColumn = false)]
        public System.String SupName { get; set; }
        [FieldMapAttribute(ClabName = "UnitName", MedName = "UnitName", WFName = "UnitName", DBColumn = false)]
        public System.String UnitName { get; set; }
        [FieldMapAttribute(ClabName = "PdtName", MedName = "PdtName", WFName = "PdtName", DBColumn = false)]
        public System.String PdtName { get; set; }
        [FieldMapAttribute(ClabName = "PosName", MedName = "PosName", WFName = "PosName", DBColumn = false)]
        public System.String PosName { get; set; }
        [FieldMapAttribute(ClabName = "ConName", MedName = "ConName", WFName = "ConName", DBColumn = false)]
        public System.String ConName { get; set; }
        [FieldMapAttribute(ClabName = "GroupName", MedName = "GroupName", WFName = "GroupName", DBColumn = false)]
        public System.String GroupName { get; set; }
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean CheckMarkSelection { get; set; }

        public string PurChaseID { get; set; }
    }
}
