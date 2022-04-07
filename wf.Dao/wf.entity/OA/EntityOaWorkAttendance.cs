using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 考勤管理 
    /// 旧表名:oa_work_attendance 新表名:Oa_attendance_sheet
    /// </summary>
    [Serializable]
    public class EntityOaWorkAttendance: EntityBase
    {

        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_id", MedName = "ate_id", WFName = "Oate_id")]
        public String AteId { get; set; }

        /// <summary>
        ///代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_user_id", MedName = "ate_user_id", WFName = "Oate_Buser_id")]
        public String AteUserId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_duty_id", MedName = "ate_shift_id", WFName = "Oate_Dduty_id")]
        public String AteShiftId { get; set; }

        /// <summary>
        ///日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_date", MedName = "ate_date", WFName = "Oate_date")]
        public DateTime AteDate { get; set; }

        /// <summary>
        ///上班时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_sdate", MedName = "ate_start_date", WFName = "Oate_start_date")]
        public String AteStartDate { get; set; }

        /// <summary>
        ///下班时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_edate", MedName = "ate_end_date", WFName = "Oate_end_date")]
        public String AteEndDate { get; set; }

        /// <summary>
        ///用户类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_flag", MedName = "ate_flag", WFName = "Oate_flag")]
        public Int32 AteFlag { get; set; }

        /// <summary>
        ///规定上班时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_duty_sdate", MedName = "ate_shift_start_date", WFName = "Oate_shift_start_date")]
        public String AteShiftStartDate { get; set; }

        /// <summary>
        ///规定下班时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_duty_edate", MedName = "ate_shift_end_date", WFName = "Oate_shift_end_date")]
        public String AteShiftEndDate { get; set; }

        /// <summary>
        ///工作评价
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_exp", MedName = "ate_remark", WFName = "Oate_remark")]
        public String AteRemark { get; set; }

        /// <summary>
        ///工作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "attd_workhours", MedName = "ate_workhours", WFName = "Oate_workhours")]
        public Decimal AteWorkhours { get; set; }

        #region 附加字段 职工账号
        /// <summary>
        ///职工账号
        /// </summary>   
        [FieldMapAttribute(ClabName = "loginId", MedName = "user_loginId", WFName = "Buser_loginId", DBColumn = false)]
        public String AteLoginId { get; set; }
        #endregion

        #region 附加字段 姓名
        /// <summary>
        ///职工账号
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "userName", WFName = "Buser_name", DBColumn = false)]
        public String AteUserName { get; set; }
        #endregion

        #region 附加字段 用户类别
        /// <summary>
        ///用户类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "userType", MedName = "userType", WFName = "Buser_type", DBColumn = false)]
        public String AteUserType { get; set; }
        #endregion

        #region 附加字段 班次
        /// <summary>
        ///班次
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_name", MedName = "shift_name", WFName = "Dduty_name", DBColumn = false)]
        public String AteShiftName { get; set; }
        #endregion

        #region 附加字段 班次拼音码
        /// <summary>
        ///班次拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_py", MedName = "py_code", WFName = "py_code", DBColumn = false)]
        public String AtePyCode { get; set; }
        #endregion

        #region 班次五笔码
        /// <summary>
        ///班次拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "duty_wb", MedName = "wb_code", WFName = "wb_code", DBColumn = false)]
        public String AteWbCode { get; set; }
        #endregion
    }
}
