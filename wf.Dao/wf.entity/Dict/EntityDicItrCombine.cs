using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器包含组合
    /// 旧表名:Def_itr_combine 新表名:Rel_itr_combine
    /// </summary>
    [Serializable]
    public class EntityDicItrCombine : EntityBase
    {
        /// <summary>
        /// 仪器表ID
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_id", MedName = "itr_id", WFName = "Ric_Ditr_id")]
        public String ItrId { get; set; }

        /// <summary>
        /// 组合表/ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Ric_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        /// 开始标本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrcom_start_sid", MedName = "start_sid", WFName = "Ric_start_sid")]
        public Int32 StartSid { get; set; }

        /// <summary>
        /// 结束标本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrcom_end_sid", MedName = "end_sid", WFName = "Ric_end_sid")]
        public Int32 EndSid { get; set; }

        #region 附加字段 开始标本号(未包含)
        /// <summary>
        /// 开始标本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrcom_start_sid_notin", MedName = "start_sid_notin", WFName = "start_sid_notin", DBColumn = false)]
        public Int32 StartSidNotin { get; set; }
        #endregion

        #region 附加字段 结束标本号(未包含)
        /// <summary>
        /// 结束标本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrcom_end_sid_notin", MedName = "end_sid_notin", WFName = "end_sid_notin", DBColumn = false)]
        public Int32 EndSidNotin { get; set; }
        #endregion

        #region 附加字段 组合编码
        /// <summary>
        /// 组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "combine_com_id", MedName = "combine_com_id", WFName = "Dcom_id", DBColumn = false)]
        public string CombineComId { get; set; }
        #endregion

        #region 附加字段 组合的名称
        /// <summary>
        /// 组合的名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public string ComName { get; set; }
        #endregion

        #region 附加字段 HIS代码
        /// <summary>
        /// HIS代码
        /// </summary>    
        [FieldMapAttribute(ClabName = "com_code", MedName = "com_code", WFName = "Dcom_code", DBColumn = false)]
        public string ComCode { get; set; }
        #endregion

        #region 附加字段 组别名称
        /// <summary>
        /// 组别名称
        /// </summary>    
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public string TypeName { get; set; }
        #endregion

        #region 附加字段 组别的编码
        /// <summary>
        ///  组别的编码
        /// </summary>    
        [FieldMapAttribute(ClabName = "type_id", MedName = "pro_id", WFName = "Dpro_id", DBColumn = false)]
        public string TypeId { get; set; }
        #endregion

        #region 附加字段 获取不包含组合用，扩展字段
        /// <summary>
        /// 获取不包含组合用，扩展字段
        /// </summary>
        public bool IsNotIn { get; set; }
        #endregion

        #region 附加字段 拼音码
        /// <summary>
        ///拼音码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_py", MedName = "py_code", WFName = "py_code", DBColumn = false)]
        public String ComPyCode { get; set; }
        #endregion

        #region 附加字段 五笔码
        /// <summary>
        ///五笔码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_wb", MedName = "wb_code", WFName = "wb_code", DBColumn = false)]
        public String ComWbCode { get; set; }
        #endregion
    }
}
