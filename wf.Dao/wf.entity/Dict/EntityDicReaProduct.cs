using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaProduct : EntityBase
    {
        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return Rpdt_id;
            }
        }
        #endregion
        [FieldMapAttribute(ClabName = "Rpdt_name", MedName = "Rpdt_name", WFName = "Rpdt_name")]
        public System.String Rpdt_name { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_id", MedName = "Rpdt_id", WFName = "Rpdt_id")]
        public System.String Rpdt_id { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_postcode", MedName = "Rpdt_postcode", WFName = "Rpdt_postcode")]
        public System.String Rpdt_postcode { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_address", MedName = "Rpdt_address", WFName = "Rpdt_address")]
        public System.String Rpdt_address { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_website", MedName = "Rpdt_website", WFName = "Rpdt_website")]
        public System.String Rpdt_website { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [FieldMapAttribute(ClabName = "Rpdt_contacts", MedName = "Rpdt_contacts", WFName = "Rpdt_contacts")]
        public System.String Rpdt_contacts { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        [FieldMapAttribute(ClabName = "Rpdt_post", MedName = "Rpdt_post", WFName = "Rpdt_post")]
        public System.String Rpdt_post { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_email", MedName = "Rpdt_email", WFName = "Rpdt_email")]
        public System.String Rpdt_email { get; set; }
        [FieldMapAttribute(ClabName = "Rpdt_phone", MedName = "Rpdt_phone", WFName = "Rpdt_phone")]
        public System.String Rpdt_phone { get; set; }
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String del_flag { get; set; }
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int sort_no { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion
    }
}
