using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 交接班表
    /// 旧表名:ho_record 新表名:Lis_ho_record
    /// </summary>
    [Serializable]
    public class EntityHoRecord: EntityBase
    {

        /// <summary>
        ///交班表ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_id", MedName = "hr_id", WFName = "Lhr_id")]
        public String HrId { get; set; }

        /// <summary>
        ///物理组ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_type_id", MedName = "hr_type_id", WFName = "Lhr_Dpro_id")]
        public String HrTypeId { get; set; }

        /// <summary>
        ///交接班人员ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_hand_code", MedName = "hr_hand_code", WFName = "Lhr_hand_Buser_id")]
        public String HrHandCode { get; set; }

        /// <summary>
        ///值班人员id
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_receive_code", MedName = "hr_receive_code", WFName = "Lhr_receive_Buser_id")]
        public String HrReceiveCode { get; set; }

        /// <summary>
        ///交班时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_hand_time", MedName = "hr_hand_time", WFName = "Lhr_hand_time")]
        public DateTime HrHandTime { get; set; }

        /// <summary>
        ///总签收标本
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_totalRecv_count", MedName = "hr_totalRecv_count", WFName = "Lhr_totalRecv_count")]
        public String HrTotalRecvCount { get; set; }

        /// <summary>
        ///无效标本
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_noqutity_count", MedName = "hr_noqutity_count", WFName = "Lhr_noqutity_count")]
        public String HrNoqutityCount { get; set; }

        /// <summary>
        ///发出报告数
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_report_count", MedName = "hr_report_count", WFName = "Lhr_report_count")]
        public String HrReportCount { get; set; }

        /// <summary>
        ///延时检验数
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_unreport_count", MedName = "hr_unreport_count", WFName = "Lhr_unreport_count")]
        public String HrUnreportCount { get; set; }

        /// <summary>
        ///危急值处理个数
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_urgent_count", MedName = "hr_urgent_count", WFName = "Lhr_urgent_count")]
        public String HrUrgentCount { get; set; }

        /// <summary>
        ///是否在控（1在控 0不在控）
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_qc_flag", MedName = "hr_qc_flag", WFName = "Lhr_qc_flag")]
        public String HrQcFlag { get; set; }

        /// <summary>
        ///失控处理信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_qc_reason", MedName = "hr_qc_reason", WFName = "Lhr_qc_reason")]
        public String HrQcReason { get; set; }

        /// <summary>
        ///是否完成仪器管理（1 已完成 0未完成）
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_itr_mflag", MedName = "hr_itr_mflag", WFName = "Lhr_itr_mflag")]
        public String HrItrMflag { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_itr_fault_id", MedName = "hr_itr_fault_id", WFName = "Lhr_fault_Ditr_id")]
        public String HrItrFaultId { get; set; }

        /// <summary>
        ///故障原因
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_itr_fault_reason", MedName = "hr_itr_fault_reason", WFName = "Lhr_fault_reason")]
        public String HrItrFaultReason { get; set; }

        /// <summary>
        ///恢复时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_itr_fault_time", MedName = "hr_itr_fault_time", WFName = "Lhr_fault_time")]
        public DateTime? HrItrFaultTime { get; set; }

        /// <summary>
        ///比对及校准
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_itr_judge", MedName = "hr_itr_judge", WFName = "Lhr_itr_judge")]
        public String HrItrJudge { get; set; }

        /// <summary>
        ///室内卫生
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_san_room", MedName = "hr_san_room", WFName = "Lhr_san_room")]
        public String HrSanRoom { get; set; }

        /// <summary>
        ///生物安全
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_san_microbe", MedName = "hr_san_microbe", WFName = "Lhr_san_microbe")]
        public String HrSanMicrobe { get; set; }

        /// <summary>
        ///医疗指令投诉
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_sp_complain", MedName = "hr_sp_complain", WFName = "Lhr_sp_complain")]
        public String HrSpComplain { get; set; }

        /// <summary>
        ///水电
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_sp_hydro", MedName = "hr_sp_hydro", WFName = "Lhr_sp_hydro")]
        public String HrSpHydro { get; set; }

        /// <summary>
        ///his、lis、自助打印机
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_sp_machine", MedName = "hr_sp_machine", WFName = "Lhr_sp_machine")]
        public String HrSpMachine { get; set; }

        /// <summary>
        ///传染性标本
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_sp_ifsam", MedName = "hr_sp_ifsam", WFName = "Lhr_sp_ifsam")]
        public String HrSpIfsam { get; set; }

        /// <summary>
        ///交班者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_handcomfirm_code", MedName = "hr_handcomfirm_code", WFName = "Lhr_handcomfirm_Buser_id")]
        public String HrHandcomfirmCode { get; set; }

        /// <summary>
        ///接班者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_recvconfirm_code", MedName = "hr_recvconfirm_code", WFName = "Lhr_recvconfirm_Buser_id")]
        public String HrRecvconfirmCode { get; set; }

        /// <summary>
        ///自定义字段1
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext1", MedName = "hr_ext1", WFName = "Lhr_ext1")]
        public String HrExt1 { get; set; }

        /// <summary>
        ///自定义字段2
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext2", MedName = "hr_ext2", WFName = "Lhr_ext2")]
        public String HrExt2 { get; set; }

        /// <summary>
        ///自定义字段3
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext3", MedName = "hr_ext3", WFName = "Lhr_ext3")]
        public String HrExt3 { get; set; }

        /// <summary>
        ///自定义字段4
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext4", MedName = "hr_ext4", WFName = "Lhr_ext4")]
        public String HrExt4 { get; set; }

        /// <summary>
        ///自定义字段5
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext5", MedName = "hr_ext5", WFName = "Lhr_ext5")]
        public String HrExt5 { get; set; }

        /// <summary>
        ///自定义字段6
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext6", MedName = "hr_ext6", WFName = "Lhr_ext6")]
        public String HrExt6 { get; set; }

        /// <summary>
        ///自定义字段7
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext7", MedName = "hr_ext7", WFName = "Lhr_ext7")]
        public String HrExt7 { get; set; }

        /// <summary>
        ///自定义字段8
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext8", MedName = "hr_ext8", WFName = "Lhr_ext8")]
        public String HrExt8 { get; set; }

        /// <summary>
        ///自定义字段9
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_ext9", MedName = "hr_ext9", WFName = "Lhr_ext9")]
        public String HrExt9 { get; set; }

        #region 附加字段 是否新增
        /// <summary>
        ///自定义字段9
        /// </summary>   
        [FieldMapAttribute(ClabName = "isnew", MedName = "isnew", WFName = "isnew", DBColumn = false)]
        public Boolean IsNew { get; set; }
        #endregion

        #region 附加字段 交班人姓名

        /// <summary>
        ///交班人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_hand_name", MedName = "hr_hand_name", WFName = "hr_hand_name", DBColumn = false)]
        public String HrHandName { get; set; }
        #endregion

        #region 附加字段 接班人姓名

        /// <summary>
        ///交班人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "hr_recvconfirm_name", MedName = "hr_recvconfirm_name", WFName = "hr_recvconfirm_name", DBColumn = false)]
        public String HrRecvConFirmName { get; set; }
        #endregion

        #region 附加字段 实验组名称

        /// <summary>
        ///实验组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "typename", MedName = "typename", WFName = "typename", DBColumn = false)]
        public String HrTypeName { get; set; }
        #endregion

    }
}
