using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///质控物明细表
    ///旧表名:Dic_qc_materia 新表名:Dict_qc_materia
    /// </summary>
    [Serializable]
    public class EntityDicQcMateria : EntityBase
    {
        /// <summary>
        /// 质控物主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_key", MedName = "mat_sn", WFName = "Dmat_id")]
        public String MatSn { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_id", MedName = "mat_id", WFName = "Dmat_Ditr_id")]
        public String MatId { get; set; }

        /// <summary>
        /// 浓度
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_no_type", MedName = "mat_level", WFName = "Dmat_level")]
        public String MatLevel { get; set; }

        /// <summary>
        /// （未知）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_no", MedName = "mat_c_no", WFName = "Dmat_c_no")]
        public String MatCNo { get; set; }

        /// <summary>
        /// 质控批号
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_r_no", MedName = "mat_batch_no", WFName = "Dmat_batch_no")]
        public String MatBatchNo { get; set; }

        /// <summary>
        ///  英文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_ename", MedName = "mat_ename", WFName = "Dmat_ename")]
        public String MatEname { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cname", MedName = "mat_cname", WFName = "Dmat_cname")]
        public String MatCname { get; set; }

        /// <summary>
        /// 生成厂家
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_yield_manu", MedName = "mat_manufacturer", WFName = "Dmat_manufacturer")]
        public String MatManufacturer { get; set; }

        /// <summary>
        /// 使用结束日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_edate", MedName = "mat_date_end", WFName = "Dmat_date_end")]
        public DateTime MatDateEnd { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_i_name", MedName = "mat_user_id", WFName = "Dmat_Buser_name")]
        public String MatUserId { get; set; }

        /// <summary>
        /// 样本取值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sam_bot", MedName = "mat_sam_lower_limit", WFName = "Dmat_sam_lower_limit")]
        public Decimal MatSamLowerLimit { get; set; }

        /// <summary>
        /// 样本取值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sam_top", MedName = "mat_sam_upper_limit", WFName = "Dmat_sam_upper_limit")]
        public Decimal MatSamUpperLimit { get; set; }

        /// <summary>
        /// 最小样本数
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sam_num", MedName = "mat_sam_num", WFName = "Dmat_sam_num")]
        public Int32 MatSamNum { get; set; }

        /// <summary>
        ///  框架法
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sam_moth", MedName = "mat_method", WFName = "Dmat_method")]
        public String MatMethod { get; set; }

        /// <summary>
        /// 状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_state", MedName = "mat_staus", WFName = "Dmat_staus")]
        public Int32 MatStaus { get; set; }

        /// <summary>
        /// 使用开始日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sdate", MedName = "mat_date_start", WFName = "Dmat_date_start")]
        public DateTime? MatDateStart { get; set; }

        #region 附加字段 质控物主键别名
        /// <summary>
        /// 质控物主键别名
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_par_detail_id", MedName = "qcm_par_detail_id", WFName = "qcm_par_detail_id", DBColumn = false)]
        public String QcmParDetailId { get; set; }
        #endregion

        #region 附加字段 仪器ID别名
        /// <summary>
        /// 仪器ID别名
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_mid", MedName = "qcm_mid", WFName = "qcm_mid", DBColumn = false)]
        public String QcmMid { get; set; }
        #endregion

        #region 附加字段 质控批号别名
        /// <summary>
        /// 质控批号别名
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_r_no", MedName = "qcm_r_no", WFName = "qcm_r_no", DBColumn = false)]
        public String QcmRNo { get; set; }
        #endregion

        #region 附加字段 浓度别名
        /// <summary>
        /// 浓度别名
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_no", MedName = "qcm_c_no", WFName = "qcm_c_no", DBColumn = false)]
        public String QcmCNo { get; set; }
        #endregion

        #region 附加字段 类别
        /// <summary>
        /// 类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type", MedName = "channel_type", WFName = "channel_type", DBColumn = false)]
        public Int32 ChannelType { get; set; }
        #endregion

        #region 附加字段 按样本号
        /// <summary>
        /// 按样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_no_sid", MedName = "sid_ident", WFName = "Rchan_sid_ident", DBColumn = false)]
        public String SidIdent { get; set; }
        #endregion

        #region 附加字段 time_id
        /// <summary>
        ///  time_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_time_id", MedName = "mat_time_id", WFName = "Rchan_Rqrt_id", DBColumn = false)]
        public String MatTimeId { get; set; }
        #endregion

        #region 附加字段 开始时间
        /// <summary>
        ///  开始时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_start_time", MedName = "qrt_start_time", WFName = "Rqrt_start_time", DBColumn = false)]
        public DateTime QrtStartTime { get; set; }
        #endregion

        #region 附加字段 结束时间
        /// <summary>
        ///  结束时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_end_time", MedName = "qrt_end_time", WFName = "Rqrt_end_time", DBColumn = false)]
        public DateTime QrtEndTime { get; set; }
        #endregion

        #region 附加字段 跨期天数
        /// <summary>
        ///  跨期天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_day", MedName = "qrt_day", WFName = "Rqrt_day", DBColumn = false)]
        public Int32 QrtDay { get; set; }
        #endregion

        #region 附加字段 类别名称
        /// <summary>
        ///  类别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type_name", MedName = "qcm_type_name", WFName = "qcm_type_name", DBColumn = false)]
        public String QcmTypeName { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        /// 仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        /// 仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion


    }
}
