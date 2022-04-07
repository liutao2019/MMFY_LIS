using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 班模板表
    /// 旧表名:dic_oa_shift 新表名:Dict_oa_duty
    /// </summary>
    [Serializable]
    public class EntityOaDicShift: EntityBase
    {
        /// <summary>
        ///班次ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_id", MedName = "shift_id",WFName = "Dduty_id")]
        public String ShiftId { get; set; }

        /// <summary>
        ///班次名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_name", MedName = "shift_name", WFName = "Dduty_name")]
        public String ShiftName { get; set; }

        /// <summary>
        ///班次开始时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_sdate", MedName = "shift_start_date", WFName = "Dduty_start_date")]
        public String ShiftStartDate { get; set; }

        /// <summary>
        ///班次结束时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_edate", MedName = "shift_end_date", WFName = "Dduty_end_date")]
        public String ShiftEndDate { get; set; }

        /// <summary>
        ///班次类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_flag", MedName = "shift_flag", WFName = "Dduty_flag")]
        public Int32 ShiftFlag { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_exp", MedName = "shift_remark", WFName = "Dduty_remark")]
        public String ShiftRemark { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        ///科室ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_dept_id", MedName = "shift_dept_id", WFName = "Dduty_Ddept_id")]

        public String ShiftDeptId { get; set; }
        #region 附加字段 物理组名称
        /// <summary>
        ///物理组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public String TypeName { get; set; }
        #endregion

        #region 附加字段 物理组拼音码
        /// <summary>
        ///物理组拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_py", MedName = "py_code1", WFName = "py_code1")]
        public String TypePy { get; set; }
        #endregion

        #region 附加字段 物理组五笔码
        /// <summary>
        ///物理组五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_wb", MedName = "web_code1", WFName = "wb_code1")]
        public String TypeWb { get; set; }
        #endregion
    }
}
