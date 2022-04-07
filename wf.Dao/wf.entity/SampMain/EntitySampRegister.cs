using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码登记表
    /// 旧表名:Samp_register 新表名:Sample_register
    /// </summary>
    [Serializable]
    public class EntitySampRegister : EntityBase
    {
        public EntitySampRegister()
        {
            Checked = false;
        }
        /// <summary>
        ///自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_id", MedName = "reg_sn", WFName = "Sreg_id")]
        public Int64 RegSn { get; set; }

        /// <summary>
        ///物理组
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_type", MedName = "reg_lab_id", WFName = "Sreg_lab_id")]
        public String RegLabId { get; set; }

        /// <summary>
        ///接收日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_date", MedName = "reg_date", WFName = "Sreg_date")]
        public DateTime RegDate { get; set; }

        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_bar_code", MedName = "reg_bar_code", WFName = "Sreg_bar_code")]
        public String RegBarCode { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_no", MedName = "reg_number", WFName = "Sreg_number")]
        public Int32 RegNumber { get; set; }

        /// <summary>
        ///试管架子类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_cus_code", MedName = "reg_rack_code", WFName = "Sreg_Dtrack_code")]
        public String RegRackCode { get; set; }

        /// <summary>
        ///试管架子号
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_etagere", MedName = "reg_rack_no", WFName = "Sreg_rack_no")]
        public Int32 RegRackNo { get; set; }

        /// <summary>
        ///试管孔号--X
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_place_x", MedName = "reg_x_place", WFName = "Sreg_x_place")]
        public Int32 RegXPlace { get; set; }

        /// <summary>
        ///试管孔号--Y
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_place_y", MedName = "reg_y_place", WFName = "Sreg_y_place")]
        public Int32 RegYPlace { get; set; }

        /// <summary>
        ///登记者
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_i_code", MedName = "reg_user_id", WFName = "Sreg_Buser_id")]
        public String RegUserId { get; set; }

        /// <summary>
        ///条码组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_bc_cname", MedName = "reg_com_name", WFName = "Sreg_com_name")]
        public String RegComName { get; set; }

        #region 附加字段 是否选中
        /// <summary>
        ///是否选中
        /// </summary>   
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        ///组合名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 病人类别编码(门诊、住院，对应dict_origin表)
        /// <summary>
        ///病人类别编码(门诊、住院，对应dict_origin表)
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_ori_id", MedName = "pid_src_id", WFName = "Sma_pat_src_id", DBColumn = false)]
        public String PidSrcId { get; set; }
        #endregion

        #region 附加字段 病人来源
        /// <summary>
        ///病人来源
        /// </summary>
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name", DBColumn = false)]
        public String SrcName { get; set; }
        #endregion

        #region 附加字段 用户姓名
        /// <summary>
        ///用户姓名
        /// </summary>
        [FieldMapAttribute(ClabName = "username", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion

        #region 附加字段 使用日期
        /// <summary>
        ///使用日期
        /// </summary>
        [FieldMapAttribute(ClabName = "st_bc_occ_date", MedName = "st_bc_occ_date", WFName = "st_order_occ_date", DBColumn = false)]
        public String StBcOccDate { get; set; }
        #endregion

        #region 附加字段 姓名
        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "pid_name", WFName = "Sma_pat_name", DBColumn = false)]
        public String PidName { get; set; }
        #endregion

        #region 附加字段 病人标识
        /// <summary>
        ///病人标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_in_no", MedName = "pid_in_no", WFName = "Sma_pat_in_no", DBColumn = false)]
        public String PidInNo { get; set; }
        #endregion

        #region 附加字段 UPID唯一号 目前滨海使用
        /// <summary>
        /// UPID唯一号 目前滨海使用
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_upid", MedName = "pid_unique_id", WFName = "Sma_pat_unique_id", DBColumn = false)]
        public String PidUniqueId { get; set; }
        #endregion

        #region 附加字段 性别(0-未知 1-男 2-女)
        /// <summary>
        ///性别(0-未知 1-男 2-女)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sex", MedName = "pid_sex", WFName = "Sma_pat_sex", DBColumn = false)]
        public String PidSex { get; set; }
        #endregion

        #region 附加字段 年龄
        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_age", MedName = "pid_age", WFName = "Sma_pat_age", DBColumn = false)]
        public String PidAge { get; set; }
        #endregion

        #region 附加字段 HIS科室名称
        /// <summary>
        ///HIS科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_name", MedName = "pid_dept_name", WFName = "Sma_pat_dept_name", DBColumn = false)]
        public String PidDeptName { get; set; }
        #endregion

        #region 附加字段 床号
        /// <summary>
        ///床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bed_no", MedName = "pid_bed_no", WFName = "Sma_pat_bed_no", DBColumn = false)]
        public String PidBedNo { get; set; }
        #endregion

        #region 附加字段 开单医生姓名
        /// <summary>
        ///开单医生姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_name", MedName = "pid_doctor_name", WFName = "Sma_doctor_name", DBColumn = false)]
        public String PidDoctorName { get; set; }
        #endregion

        #region 附加字段 就诊次数
        /// <summary>
        ///就诊次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_times", MedName = "pid_admiss_times", WFName = "Sma_pat_admiss_times", DBColumn = false)]
        public Decimal PidAdmissTimes { get; set; }
        #endregion

        #region 附加字段 诊断
        /// <summary>
        ///诊断
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_diag", MedName = "pid_diag", WFName = "Sma_pat_diag", DBColumn = false)]
        public String PidDiag { get; set; }
        #endregion

        #region 附加字段 标本编码
        /// <summary>
        ///标本编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_id", MedName = "samp_sam_id", WFName = "Sma_Dsam_id", DBColumn = false)]
        public String SampSamId { get; set; }
        #endregion

        #region 附加字段 标本名称
        /// <summary>
        ///标本名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_name", MedName = "samp_sam_name", WFName = "Sma_Dsam_name", DBColumn = false)]
        public String SampSamName { get; set; }
        #endregion

        #region 附加字段 标本状态
        /// <summary>
        ///标本状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status", MedName = "samp_status_id", WFName = "Sma_status_id", DBColumn = false)]
        public String SampStatusId { get; set; }
        #endregion

        #region 附加字段 检查项目
        /// <summary>
        ///检查项目
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ComName { get; set; }
        #endregion

        #region 附加字段 执行日期
        /// <summary>
        ///检查项目
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_occ_date", MedName = "samp_occ_date", WFName = "Sma_occ_date", DBColumn = false)]
        public String SampOccDate { get; set; }
        #endregion

        #region 附加字段 检验组合lis中的id
        /// <summary>
        ///检验组合lis中的id
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lis_code", MedName = "com_id", WFName = "Sdet_com_id", DBColumn = false)]
        public String LisComId { get; set; }
        #endregion

        #region 附加字段  标志
        /// <summary>
        ///标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_flag", MedName = "samp_flag", WFName = "Sdet_flag", DBColumn = false)]
        public Int32 SampFlag { get; set; }
        #endregion

        #region 附加字段  条码项目明细表 自增ID
        /// <summary>
        ///条码项目明细表 自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "det_sn", WFName = "Sdet_sn", DBColumn = false)]
        public string DetSn { get; set; }
        #endregion
    }
}
