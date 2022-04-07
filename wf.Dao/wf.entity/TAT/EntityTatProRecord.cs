using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityTatProRecord:EntityBase
    {
        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_bar_code", MedName = "tpr_bar_code", WFName = "tpr_bar_code")]
        public String TprBarCode { get; set; }

        /// <summary>
        ///申请时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_apply_date", MedName = "tpr_apply_date", WFName = "tpr_apply_date")]
        public DateTime TprApplyDate { get; set; }

        /// <summary>
        ///采集时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_blood_date", MedName = "tpr_blood_date", WFName = "tpr_blood_date")]
        public DateTime TprBloodDate { get; set; }

        /// <summary>
        ///收取时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_send_date", MedName = "tpr_send_date", WFName = "tpr_send_date")]
        public DateTime TprSendDate { get; set; }

        /// <summary>
        ///送达时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_reach_date", MedName = "tpr_reach_date", WFName = "tpr_reach_date")]
        public DateTime TprReachDate { get; set; }

        /// <summary>
        ///签收时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_receiver_date", MedName = "tpr_receiver_date", WFName = "tpr_receiver_date")]
        public DateTime TprReceiverDate { get; set; }

        /// <summary>
        ///登记时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_reg_date", MedName = "tpr_reg_date", WFName = "tpr_reg_date")]
        public DateTime TprRegDate { get; set; }

        /// <summary>
        ///测试时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_test_date", MedName = "tpr_test_date", WFName = "tpr_test_date")]
        public DateTime TprTestDate { get; set; }

        /// <summary>
        ///检验时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_jy_date", MedName = "tpr_jy_date", WFName = "tpr_jy_date")]
        public DateTime TprJyDate { get; set; }

        /// <summary>
        ///审核时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_check_date", MedName = "tpr_check_date", WFName = "tpr_check_date")]
        public DateTime TprCheckDate { get; set; }

        /// <summary>
        ///报告时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_report_date", MedName = "tpr_report_date", WFName = "tpr_report_date")]
        public DateTime TprReportDate { get; set; }

        /// <summary>
        ///回退时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_return_date", MedName = "tpr_return_date", WFName = "tpr_return_date")]
        public DateTime TprReturnDate { get; set; }

        /// <summary>
        ///目标时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_target_date", MedName = "tpr_target_date", WFName = "tpr_target_date")]
        public DateTime TprTargetDate { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_remark", MedName = "tpr_remark", WFName = "tpr_remark")]
        public String TprRemark { get; set; }

        /// <summary>
        ///标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_flag", MedName = "tpr_flag", WFName = "tpr_flag")]
        public Int32 TprFlag { get; set; }

        /// <summary>
        ///状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_stauts", MedName = "tpr_stauts", WFName = "tpr_stauts")]
        public String TprStauts { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tpr_createdate", MedName = "tpr_createdate", WFName = "tpr_createdate")]
        public DateTime TprCreatedate { get; set; }
    }
}
