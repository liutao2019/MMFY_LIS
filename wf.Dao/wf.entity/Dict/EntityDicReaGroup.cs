using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaGroup : EntityBase
    {
        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return Rea_group_id;
            }
        }
        #endregion
        [FieldMapAttribute(ClabName = "Res_group", MedName = "Res_group", WFName = "Rea_group")]
        public System.String Rea_group { get; set; }
        [FieldMapAttribute(ClabName = "Res_group_id", MedName = "Res_group_id", WFName = "Rea_group_id")]
        public System.String Rea_group_id { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String del_flag { get; set; }
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int sort_no { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion
    }
}
