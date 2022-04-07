using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 组合明细表
    ///旧表名:Def_itm_combine_item 新表名:Rel_itm_combine_item
    /// </summary>
    [Serializable()]
    public class EntityDicCombineDetail : EntityBase
    {
        /// <summary>
        ///组合ID 对应dict_combine表中的com_id
        /// </summary> 
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Rici_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///项目ID 对应dict_item表中的itm_id
        /// </summary>  
        [FieldMapAttribute(ClabName = "com_itm_id", MedName = "com_itm_id", WFName = "Rici_Ditm_id")]
        public String ComItmId { get; set; }

        /// <summary>
        ///组合项目代码
        /// </summary> 
        [FieldMapAttribute(ClabName = "com_itm_ecd", MedName = "com_itm_ename", WFName = "Rici_Ditm_ecode")]
        public String ComItmEname { get; set; }

        /// <summary>
        ///状态
        /// </summary>  
        [FieldMapAttribute(ClabName = "com_flag", MedName = "com_flag", WFName = "Rici_flag")]
        public Int32 ComFlag { get; set; }

        /// <summary>
        ///是否必为必录项目
        /// </summary> 
        [FieldMapAttribute(ClabName = "com_popedom", MedName = "com_must_item", WFName = "Rici_must_item")]
        public String ComMustItem { get; set; }

        /// <summary>
        ///排序
        /// </summary>  
        [FieldMapAttribute(ClabName = "com_sort", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ComSortNo { get; set; }

        /// <summary>
        ///打印标记
        /// </summary>
        [FieldMapAttribute(ClabName = "com_print_flag", MedName = "com_print_flag", WFName = "Rici_print_flag")]
        public Int32 ComPrintFlag { get; set; }

        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ComItmName { get; set; }

        #endregion

        #region 附加字段 项目默认值
        /// <summary>
        /// 项目默认值
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_defa", MedName = "itm_default", WFName = "Ritm_default", DBColumn = false)]
        public String ItmDefault { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        /// 组合名称
        /// </summary>
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ComName { get; set; }
        #endregion

        #region 附加字段 性别限制
        /// <summary>
        ///性别限制  0-不限制 1-男 2-女
        /// </summary>  
        [FieldMapAttribute(ClabName = "itm_sex_limit", MedName = "itm_match_sex", WFName = "Ditm_match_sex", DBColumn = false)]
        public String ItmMatchSex { get; set; }
        #endregion

        #region 附加字段 排序编号
        /// <summary>
        ///排序编号(序号)
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_seq", MedName = "sort_no", WFName = "sort_no", DBColumn = false)]
        public Int32 ItmSortNo { get; set; }
        #endregion

        #region 附加字段 仪器ID

        /// <summary>
        ///仪器ID 项目标本中的
        /// </summary> 
        [FieldMapAttribute(ClabName = "itm_itr_id", MedName = "itm_itr_id", WFName = "Ritm_Ditr_id", DBColumn = false)]
        public String ItmItrId { get; set; }
        #endregion

        #region 附加字段 标本ID 对应dict_sample表中的sam_id
        /// <summary>
        ///标本ID 对应dict_sample表中的sam_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_id", MedName = "itm_sam_id", WFName = "Ritm_sam_id", DBColumn = false)]
        public string ItmSamId { get; set; }
        #endregion

        #region 附加字段 单位 项目标本表
        /// <summary>
        ///标本ID 对应dict_sample表中的sam_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_unit", MedName = "itm_unit", WFName = "Ritm_unit", DBColumn = false)]
        public string ItmUnit { get; set; }
        #endregion

        #region 附加字段 项目字典表中的项目代码  
        /// <summary>
        ///项目字典表中的项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode", DBColumn = false)]
        public string ItmEcode { get; set; }
        #endregion

        #region 附加字段 报表打印时使用的项目代码  
        /// <summary>
        ///报表打印时使用的项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_rep_ecd", MedName = "itm_rep_code", WFName = "Ditm_rep_code", DBColumn = false)]
        public string ItmRepCode { get; set; }
        #endregion

    }
}
