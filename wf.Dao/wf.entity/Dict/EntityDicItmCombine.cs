using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 数据表实体类： 组合明细中间表
    /// 旧表名:Def_itm_combine_item 新表名:Rel_itm_combine_item
    /// </summary>
    [Serializable()]
    public class EntityDicItmCombine : EntityBase
    {

        /// <summary>
        ///编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Rici_Dcom_id")]
        public String ItmComId { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_itm_id", MedName = "com_itm_id", WFName = "Rici_Ditm_id")]
        public String ItmId { get; set; }


        /// <summary>
        ///项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_itm_ecd", MedName = "com_itm_ename", WFName = "Rici_Ditm_ecode")]
        public String ItmEname { get; set; }

        /// <summary>
        ///排序
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_sort", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ItmSort { get; set; }


        /// <summary>
        ///是否必为必录项目
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_popedom", MedName = "com_must_item", WFName = "Rici_must_item")]
        public Int32 ItmMustItem { get; set; }


        /// <summary>
        ///打印标记
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_print_flag", MedName = "com_print_flag", WFName = "Rici_print_flag")]
        public Int32 ItmPrintFlag { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldMapAttribute(ClabName = "com_flag", MedName = "com_flag", WFName = "Rici_flag")]
        public Int32 ItmComFlag { get; set; }




        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion

        #region 附加字段 专业组
        /// <summary>
        ///专业组
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ptype", MedName = "itm_pri_id", WFName = "Ditm_pri_id", DBColumn = false)]
        public String ItmPriId { get; set; }
        #endregion
    }
}
