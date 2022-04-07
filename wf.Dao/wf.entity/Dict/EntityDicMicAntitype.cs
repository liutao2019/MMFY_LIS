using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 药敏卡
    /// 旧表名:Dic_mic_antitype 新表名:Dic_mic_antitype
    /// </summary>
    [Serializable]
    public class EntityDicMicAntitype : EntityBase
    {
        public EntityDicMicAntitype()
        {
            ASortNo = 0;
        }
        /// <summary>
        /// 编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_id", MedName = "atype_id", WFName = "Dantitype_id")]
        public String AtypeId { get; set; }

        /// <summary>
        /// 药敏卡名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_cname", MedName = "atype_name", WFName = "Dantitype_name")]
        public String AtypeName { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 ADelFlag { get; set; }

        /// <summary>
        ///输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_incode", MedName = "c_code", WFName = "Dantitype_c_code")]
        public String ACCode { get; set; }

        /// <summary>
        ///药敏卡编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_code", MedName = "atype_code", WFName = "Dantitype_code")]
        public String AtypeCode { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_py", MedName = "py_code", WFName = "py_code")]
        public String APyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_wb", MedName = "wb_code", WFName = "wb_code")]
        public String AWbCode { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ASortNo { get; set; }

    }
}
