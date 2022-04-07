using System;

namespace dcl.entity
{
    /// <summary>
    ///抗生素大类字典
    ///旧表名:dic_antibio_type 新表名:Dict_antibio_type
    /// </summary>
    [Serializable]
    public class EntityDicAntibioType : EntityBase
    {
        /// <summary>
        /// 程序触发生成的自增型主键
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_id", MedName = "tp_id", WFName = "Dtp_id")]
        public String TpID { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_cname", MedName = "tp_cname", WFName = "Dtp_cname")]
        public String TpCName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_seq", MedName = "tp_seq", WFName = "sort_no")]
        public int TpSeq { get; set; }

        /// <summary>
        /// 英文简称
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_code", MedName = "tp_code", WFName = "Dtp_code")]
        public String TpCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_py", MedName = "tp_py", WFName = "py_code")]
        public String TpPY { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        [FieldMapAttribute(ClabName = "tp_wb", MedName = "tp_wb", WFName = "wb_code")]
        public String TpWB { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public int DelFlag { get; set; }
    }
}