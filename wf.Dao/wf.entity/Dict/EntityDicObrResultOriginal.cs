using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器结果中间表(表结构与resulto一样) 
    /// 旧表名:Obr_Result_Original 新表名:Lis_result_original
    /// </summary>
    [Serializable]
    public class EntityDicObrResultOriginal : EntityBase
    {
        /// <summary>
        /// 唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_key", MedName = "obr_sn", WFName = "Lro_id", DBIdentity = true)]
        public Int32 ObrSn { get; set; }

        /// <summary>
        /// 仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_id", MedName = "obr_itr_id", WFName = "Lro_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        /// 仪器原始代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_ori_id", MedName = "obr_source_itr_id", WFName = "Lro_source_Ditr_id")]
        public String ObrSourceItrId { get; set; }

        /// <summary>
        /// 样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid", MedName = "obr_sid", WFName = "Lro_sid")]
        public String ObrSid { get; set; }

        /// <summary>
        /// 通道码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_cno", MedName = "obr_mac_code", WFName = "Lro_Ricc_code")]
        public String ObrMacCode { get; set; }

        /// <summary>
        /// 结果1
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_a", MedName = "obr_value", WFName = "Lro_value")]
        public String ObrValue { get; set; }

        /// <summary>
        /// 结果2
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_b", MedName = "obr_value2", WFName = "Lro_value2")]
        public String ObrValue2 { get; set; }

        /// <summary>
        /// 结果3
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_c", MedName = "obr_value3", WFName = "Lro_value3")]
        public String ObrValue3 { get; set; }

        /// <summary>
        /// 结果4
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_d", MedName = "obr_value4", WFName = "Lro_value4")]
        public String ObrValue4 { get; set; }

        /// <summary>
        /// 结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_date", MedName = "obr_date", WFName = "Lro_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_exp", MedName = "obr_remark", WFName = "Lro_remark")]
        public String ObrRemark { get; set; }

        /// <summary>
        /// 日期类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_data_type", MedName = "obr_data_type", WFName = "Lro_data_type")]
        public Int32 ObrDataType { get; set; }

        /// <summary>
        /// 读取标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_read_flag", MedName = "obr_receiver_flag", WFName = "Lro_receiver_flag")]
        public Int32 ObrReceiverFlag { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_id", MedName = "obr_id", WFName = "Lro_Lresdesc_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_critical_flag", MedName = "obr_critical_flag", WFName = "Lro_critical_flag")]
        public String ObrCriticalFlag { get; set; }

        #region 附加字段 项目ID
        /// <summary>
        /// 项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "itm_id", DBColumn = false)]
        public String ItmID { get; set; }
        #endregion

        #region 附加字段 项目代码
        /// <summary>
        /// 项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecd", WFName = "itm_ecd", DBColumn = false)]
        public String ItmEcd { get; set; }
        #endregion

        #region 附加字段 备注
        /// <summary>
        /// 备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg", MedName = "msg", WFName = "msg", DBColumn = false)]
        public String Msg { get; set; }
        #endregion

        #region 附加字段 勾选框
        /// <summary>
        ///  勾选框
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_select", MedName = "res_select", WFName = "res_select", DBColumn = false)]
        public String ResSelect { get; set; }
        #endregion

        #region 附加字段  小数位
        /// <summary>
        ///  小数位
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_dec", MedName = "mit_dec", WFName = "Ricc_dec_place", DBColumn = false)]
        public Double MitDec { get; set; }
        #endregion

        #region 附加字段  起始位置
        /// <summary>
        ///  起始位置
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_pos", MedName = "mit_pos", WFName = "Ricc_position", DBColumn = false)]
        public Double MitPos { get; set; }
        #endregion

        #region 附加字段  结果长度
        /// <summary>
        ///  结果长度
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_rlen", MedName = "mit_rlen", WFName = "Ricc_res_len", DBColumn = false)]
        public Int32 MitRlen { get; set; }
        #endregion

        #region 附加字段  结果类型
        /// <summary>
        ///  结果类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_type", MedName = "mit_type", WFName = "Ricc_type", DBColumn = false)]
        public String MitType { get; set; }
        #endregion

        #region 附加字段  双向标志
        /// <summary>
        ///  双向标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_flag", MedName = "mit_flag", WFName = "Ricc_flag", DBColumn = false)]
        public Int32 MitFlag { get; set; }
        #endregion

        #region 附加字段  样本号(int)
        /// <summary>
        ///  样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid_int", MedName = "res_sid_int", WFName = "res_sid_int", DBColumn = false)]
        public Double ResSidInt { get; set; }
        #endregion

        #region 附加字段 排序字段
        public int? RowSer { get; set; }

        #endregion

    }
}
