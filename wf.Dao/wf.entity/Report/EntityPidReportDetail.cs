using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人组合明细表
    /// 旧表名:pid_report_detail 新表名:Pat_lis_detail
    /// </summary>
    [Serializable]
    public class EntityPidReportDetail : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id", WFName = "Pdet_id")]
        public String RepId { get; set; }

        /// <summary>
        ///组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_com_id", MedName = "com_id", WFName = "Pdet_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///组合HIS编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_his_code", MedName = "order_code", WFName = "Pdet_com_code")]
        public String OrderCode { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_com_price", MedName = "order_price", WFName = "Pdet_price")]
        public String OrderPrice { get; set; }

        /// <summary>
        ///医嘱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_yz_id", MedName = "order_sn", WFName = "Pdet_order_sn")]
        public String OrderSn { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32? SortNo { get; set; }

        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_bar_code", MedName = "rep_bar_code", WFName = "Pdet_bar_code")]
        public String RepBarCode { get; set; }


        /// <summary>
        /// 申请单号（茂名妇幼收费依据）
        /// </summary>
        [FieldMapAttribute(ClabName = "Pdet_applyid", MedName = "Pdet_applyid", WFName = "Pdet_applyid")]
        public String ApplyID { get; set; }

        #region 附加字段 条码上机标志
        /// <summary>
        /// 条码上机标志
        /// </summary>
        public int SampFlag { get; set; }
        #endregion

        #region 附加字段 科室id
        /// <summary>
        /// 科室id
        /// </summary>
        public string PidDeptCode { get; set; }
        #endregion

        #region 附加字段 送检科室名称
        /// <summary>
        /// 送检科室名称
        /// </summary>
        public string PidDeptName { get; set; }
        #endregion

        #region 附加字段 医生id
        /// <summary>
        ///  医生id
        /// </summary>
        public string PidDoctorCode { get; set; }
        #endregion

        #region 附加字段 医生名称
        /// <summary>
        /// 医生名称
        /// </summary>
        public string PidDoctorName { get; set; }
        #endregion

        #region  附加字段 诊断
        /// <summary>
        /// 诊断
        /// </summary>
        public string PidDiag { get; set; }
        #endregion

        #region 附加字段 允许选中
        /// <summary>
        /// 允许选中
        /// </summary>
        public Boolean AllowSelect { get; set; }
        #endregion

        #region 附加字段 选中
        /// <summary>
        /// 选中
        /// </summary>
        public Boolean Selected { get; set; }
        #endregion

        #region 附加字段 判断组合是否可在此仪器登记
        /// <summary>
        /// 
        /// </summary>
        public bool CanSelect { get; set; }

         /// <summary>
         /// 描述
         /// </summary>
        public string Description { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_com_name", MedName = "pat_com_name", WFName = "pat_com_name", DBColumn = false)]
        public String PatComName { get; set; }
        #endregion

        #region 附加字段 组合排序 因为表本身有排序字段，所有就给一个别名
        /// <summary>
        ///组合排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_seq", MedName = "com_seq", WFName = "com_seq", DBColumn = false)]
        public String ComSeq { get; set; }
        #endregion
    }
}
