using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目特征表
    /// 旧表名:Def_itm_property 新表名:Rel_itm_property
    /// </summary>
    [Serializable]
    public class EntityDefItmProperty:EntityBase
    {
        public EntityDefItmProperty()
        {
            PtySortNo = 0;
        }
        /// <summary>
        ///自增主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "pro_id", MedName = "pty_id", WFName = "Rproty_id")]
        public String PtyId { get; set; }

        /// <summary>
        ///项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Rproty_Ditm_id")]
        public String PtyItmId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "Itm_ecd", MedName = "itm_ename", WFName = "Rproty_Ditm_ecode")]
        public String PtyItmEname { get; set; }

        /// <summary>
        ///项目特征
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_prop", MedName = "itm_property", WFName = "Rproty_name")]
        public String PtyItmProperty { get; set; }

        /// <summary>
        ///输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_incode", MedName = "c_code", WFName = "Rproty_c_code")]
        public String PtyCCode { get; set; }

        /// <summary>
        ///组别
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_type", MedName = "pro_id", WFName = "Rproty_Dpro_id")]
        public String PtyProId { get; set; }

        /// <summary>
        ///是否公用0-私用 1-公用
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_flag", MedName = "itm_flag", WFName = "Rproty_flag")]
        public Int32? PtyItmFlag { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_py", MedName = "py_code", WFName = "py_code")]
        public String PtyPyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_wb", MedName = "wb_code", WFName = "wb_code")]
        public String PtyWbCode { get; set; }

        /// <summary>
        ///排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 PtySortNo { get; set; }

        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public Int32 PtyItmName { get; set; }
        #endregion
    }
}
