using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器质控数据字典表
    /// 旧表名:Def_qc_instrmt_channel  新表名:Rel_qc_instrmt_channel
    /// </summary>
    [Serializable]
    public class EntityDicQcInstrmtChannel : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "qc_bs", MedName = "channel_sn", WFName = "Rchan_id")]
        public Int32 ChannelSn { get; set; }

        /// <summary>
        /// 仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_mid", MedName = "itr_id", WFName = "Rchan_Ditr_id")]
        public String ItrId { get; set; }
        
        /// <summary>
        /// 项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itm", MedName = "itm_id", WFName = "Rchan_Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        /// 质控标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_no", MedName = "mat_level", WFName = "Rchan_level")]
        public String MatLevel { get; set; }

        /// <summary>
        /// 类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type", MedName = "channel_type", WFName = "Rchan_type")]
        public Int32 ChannelType { get; set; }

        /// <summary>
        /// 按样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_no_sid", MedName = "sid_ident", WFName = "Rchan_sid_ident")]
        public String SidIdent { get; set; }

        /// <summary>
        /// 按质控标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_no_code", MedName = "batch_ident", WFName = "Rchan_batch_ident")]
        public String BatchIdent { get; set; }
        
        /// <summary>
        /// 质控批号
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_r_no", MedName = "mat_batch_no", WFName = "Rchan_batch_no")]
        public String MatBatchNo { get; set; }

        /// <summary>
        /// 质控物 
        /// </summary>                 
        [FieldMapAttribute(ClabName = "qcm_par_detail_id", MedName = "mat_id", WFName = "Rchan_Dmat_id")]
        public String MatId { get; set; }

        /// <summary>
        /// 时间ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_time_id", MedName = "mat_time_id", WFName = "Rchan_Rqrt_id")]
        public String MatTimeId { get; set; }

        #region 附加字段1
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_par_detail_id_new", MedName = "mat_id_new", WFName = "mat_id_new", DBColumn = false)]
        public String MatIdNew { get; set; }
        #endregion

        #region 附加字段2
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_mid_new", MedName = "itr_id_new", WFName = "itr_id_new", DBColumn = false)]
        public String ItrIdNew { get; set; }
        #endregion

        #region 附加字段3
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_r_no_new", MedName = "mat_batch_no_new", WFName = "mat_batch_no_new", DBColumn = false)]
        public String MatBatchNoNew { get; set; }
        #endregion

        #region 附加字段4
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_no_new", MedName = "mat_level_new", WFName = "mat_level_new", DBColumn = false)]
        public String MatLevelNew { get; set; }
        #endregion

        #region 附加字段5
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type_new", MedName = "channel_type_new", WFName = "channel_type_new", DBColumn = false)]
        public Int32 ChannelTypeNew { get; set; }
        #endregion

        #region 附加字段6
        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type_name", MedName = "channel_type_name", WFName = "channel_type_name", DBColumn = false)]
        public String ChannelTypeName {
            get
            {
                if(ChannelTypeNew == 0)
                {
                    return "样本号";
                }
                else if(ChannelTypeNew == 1)
                {
                    return "质控标识";
                }
                else
                {
                    return "";
                }
            }
            set { }
        }
        #endregion

    }
}
