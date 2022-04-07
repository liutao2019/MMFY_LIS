using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 排班计划表
    /// 旧表名:def_oa_shift_detail 新表名:Rel_oa_duty_detail
    /// </summary>
    [Serializable]
    public class EntityOaDicShiftDetail: EntityBase
    {
        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_user_id", MedName = "detail_user_id",WFName = "Rdutydet_Buser_id")]
        public String DetailUserId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_duty_id", MedName = "detail_shift_id",WFName = "Rdutydet_Dduty_id")]
        public String DetailShiftId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_duty_date", MedName = "detail_date", WFName = "Rdutydet_date")]
        public DateTime DetailDate { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_type_id", MedName = "detail_type", WFName = "Rdutydet_type")]
        public String DetailType { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_post", MedName = "detail_work_post", WFName = "Rdutydet_work_post")]
        public String DetailWorkPost { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_holiday_type1", MedName = "detail_holiday_a", WFName = "Rdutydet_holiday_a")]
        public String DetailHolidayA { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_holiday_type2", MedName = "detail_holiday_b", WFName = "Rdutydet_holiday_b")]
        public String DetailHolidayB { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ddetail_holiday_type3", MedName = "detail_holiday_c", WFName = "Rdutydet_holiday_c")]
        public String DetailHolidayC { get; set; }

        #region 附加字段 科室id
        /// <summary>
        ///科室id
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_dept_id", MedName = "shift_dept_id", WFName = "Dduty_Ddept_id")]
        public String ShiftDeptId { get; set; }
        #endregion

        #region 附加字段 班次名称
        /// <summary>
        ///班次名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_name", MedName = "shift_name", WFName = "Dduty_name")]
        public String ShiftName { get; set; }
        #endregion

        #region 附加字段  用户名称
        /// <summary>
        ///用户名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name")]
        public String UserName { get; set; }
        #endregion
    }
}
