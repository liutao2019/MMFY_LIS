using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaSupplier : EntityBase
    {
        [FieldMapAttribute(ClabName = "Rsupplier_name", MedName = "Rsupplier_name", WFName = "Rsupplier_name")]
        public System.String Rsupplier_name { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_id", MedName = "Rsupplier_id", WFName = "Rsupplier_id")]
        public System.String Rsupplier_id { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String del_flag { get; set; }
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int sort_no { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_postcode", MedName = "Rsupplier_postcode", WFName = "Rsupplier_postcode")]
        public System.String Rsupplier_postcode { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_address", MedName = "Rsupplier_address", WFName = "Rsupplier_address")]
        public System.String Rsupplier_address { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_website", MedName = "Rsupplier_website", WFName = "Rsupplier_website")]
        public System.String Rsupplier_website { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [FieldMapAttribute(ClabName = "Rsupplier_contacts", MedName = "Rsupplier_contacts", WFName = "Rsupplier_contacts")]
        public System.String Rsupplier_contacts { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        [FieldMapAttribute(ClabName = "Rsupplier_post", MedName = "Rsupplier_post", WFName = "Rsupplier_post")]
        public System.String Rsupplier_post { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_email", MedName = "Rsupplier_email", WFName = "Rsupplier_email")]
        public System.String Rsupplier_email { get; set; }
        [FieldMapAttribute(ClabName = "Rsupplier_phone", MedName = "Rsupplier_phone", WFName = "Rsupplier_phone")]
        public System.String Rsupplier_phone { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return Rsupplier_id;
            }
        }
        #endregion
    }
}
