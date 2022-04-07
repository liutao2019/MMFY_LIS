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
    public class EntityReaSetting : EntityBase
    {
        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return Drea_id;
            }
        }
        #endregion
        [FieldMapAttribute(ClabName = "Drea_name", MedName = "Drea_name", WFName = "Drea_name")]
        public System.String Drea_name { get; set; }
        [FieldMapAttribute(ClabName = "Barcode", MedName = "Barcode", WFName = "Barcode", DBColumn = false)]
        public System.String Barcode { get; set; }
        [FieldMapAttribute(ClabName = "Drea_id", MedName = "Drea_id", WFName = "Drea_id")]
        public System.String Drea_id { get; set; }
        [FieldMapAttribute(ClabName = "Drea_package", MedName = "Drea_package", WFName = "Drea_package")]
        public System.String Drea_package { get; set; }
        [FieldMapAttribute(ClabName = "Drea_unit", MedName = "Drea_unit", WFName = "Drea_unit")]
        public System.String Drea_unit { get; set; }
        [FieldMapAttribute(ClabName = "Drea_product", MedName = "Drea_product", WFName = "Drea_product")]
        public System.String Drea_product { get; set; }
        [FieldMapAttribute(ClabName = "Drea_supplier", MedName = "Drea_supplier", WFName = "Drea_supplier")]
        public System.String Drea_supplier { get; set; }
        [FieldMapAttribute(ClabName = "Drea_group", MedName = "Drea_group", WFName = "Drea_group")]
        public System.String Drea_group { get; set; }
        [FieldMapAttribute(ClabName = "Drea_position", MedName = "Drea_position", WFName = "Drea_position")]
        public System.String Drea_position { get; set; }
        [FieldMapAttribute(ClabName = "Drea_condition", MedName = "Drea_condition", WFName = "Drea_condition")]
        public System.String Drea_condition { get; set; }
        [FieldMapAttribute(ClabName = "Drea_lower_limit", MedName = "Drea_lower_limit", WFName = "Drea_lower_limit")]
        public System.Int32? Drea_lower_limit { get; set; }
        [FieldMapAttribute(ClabName = "Drea_upper_limit", MedName = "Drea_upper_limit", WFName = "Drea_upper_limit")]
        public System.Int32? Drea_upper_limit { get; set; }
        [FieldMapAttribute(ClabName = "Drea_certificate", MedName = "Drea_certificate", WFName = "Drea_certificate")]
        public System.String Drea_certificate { get; set; }
        [FieldMapAttribute(ClabName = "Drea_tender", MedName = "Drea_tender", WFName = "Drea_tender")]
        public System.String Drea_tender { get; set; }
        [FieldMapAttribute(ClabName = "Drea_provincialno", MedName = "Drea_provincialno", WFName = "Drea_provincialno")]
        public System.String Drea_provincialno { get; set; }
        [FieldMapAttribute(ClabName = "Drea_method", MedName = "Drea_method", WFName = "Drea_method")]
        public System.String Drea_method { get; set; }
        [FieldMapAttribute(ClabName = "Drea_remark", MedName = "Drea_remark", WFName = "Drea_remark")]
        public System.String Drea_remark { get; set; }
        [FieldMapAttribute(ClabName = "Drea_printflag", MedName = "Drea_printflag", WFName = "Drea_printflag")]
        public System.Int32 Drea_printflag { get; set; }
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public System.Int32 del_flag { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "Drea_alarmdays", MedName = "Drea_alarmdays", WFName = "Drea_alarmdays")]
        public System.String Drea_alarmdays { get; set; }
        [FieldMapAttribute(ClabName = "Runit_name", MedName = "Runit_name", WFName = "Runit_name", DBColumn = false)]
        public System.String Runit_name { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_name", MedName = "Rpdt_name", WFName = "Rpdt_name", DBColumn = false)]
        public System.String Rpdt_name { get; set; }
        [FieldMapAttribute(ClabName = "Rea_group", MedName = "Rea_group", WFName = "Rea_group", DBColumn = false)]
        public System.String Rea_group { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_name", MedName = "Rsupplier_name", WFName = "Rsupplier_name", DBColumn = false)]
        public System.String Rsupplier_name { get; set; }
        [FieldMapAttribute(ClabName = "Rstr_position", MedName = "Rstr_position", WFName = "Rstr_position", DBColumn = false)]
        public System.String Rstr_position { get; set; }
        [FieldMapAttribute(ClabName = "Rstr_condition", MedName = "Rstr_condition", WFName = "Rstr_condition", DBColumn = false)]
        public System.String Rstr_condition { get; set; }
        [FieldMapAttribute(ClabName = "sort_no", MedName = "sort_no", WFName = "sort_no")]
        public System.String sort_no { get; set; }
        [FieldMapAttribute(ClabName = "Rri_Count", MedName = "Rri_Count", WFName = "Rri_Count", DBColumn = false)]
        public System.Int32 Rri_Count { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

    }
}
