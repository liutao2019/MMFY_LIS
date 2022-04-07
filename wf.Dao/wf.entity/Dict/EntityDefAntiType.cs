using System;

namespace dcl.entity
{
    /// <summary>
    ///抗生素分类定义字典（用于判断多重耐药)
    ///旧表名:def_anti_type 新表名:Rel_anti_type
    /// </summary>
    [Serializable]
    public class EntityDefAntiType : EntityBase
    {
        /// <summary>
        /// 程序触发生成的自增型主键
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_id", MedName = "dt_id", WFName = "Rat_id")]
        public String DtID { get; set; }

        /// <summary>
        /// 抗生素ID【dict_antibio.anti_id】
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_anti_id", MedName = "dt_anti_id", WFName = "Rat_Dant_id")]
        public String DtAntiID { get; set; }

        /// <summary>
        /// 细菌ID【dict_bacteri.bac_id】
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_bt_id", MedName = "dt_bt_id", WFName = "Rat_Dbact_id")]
        public String DtBtID { get; set; }

        /// <summary>
        /// 0-单个抗生素判断多耐 1-多种抗生素大类判断多耐
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_flag", MedName = "dt_flag", WFName = "Rat_flag")]
        public String DtFlag { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public int DelFlag { get; set; }

        /// <summary>
        /// 抗生素名称
        /// </summary>
        [FieldMapAttribute(ClabName = "anti_cname", MedName = "ant_cname", WFName = "Dant_cname", DBColumn = false)]
        public String AntiName { get; set; }

        /// <summary>
        /// 细菌名称
        /// </summary>
        [FieldMapAttribute(ClabName = "bac_cname", MedName = "bac_cname", WFName = "Dbact_cname", DBColumn = false)]
        public String BtName { get; set; }

        /// <summary>
        /// 多耐显示
        /// </summary>
        public String DtFlagText
        {
            get
            {
                if (DtFlag == "0")
                {
                    return "单种抗生素判断多耐";
                }
                else
                {
                    return "多种抗生素大类判断多耐";
                }
            }
        }
    }
}