using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器结果中间表(表结构与resulto一样)
    /// 新表名:Lis_result_originaex
    /// </summary>
    [Serializable]
    public class EntityObrResultOriginalEx : EntityBase
    {
        /// <summary>
        ///唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_key", MedName = "obr_sn", WFName = "Lro_id")]
        public Int32 ObrSn { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_id", MedName = "obr_itr_id", WFName = "Lro_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///仪器原始代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_ori_id", MedName = "obr_source_itr_id", WFName = "Lro_source_Ditr_id")]
        public String ObrSourceItrId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid", MedName = "obr_sid", WFName = "Lro_sid")]
        public String ObrSid { get; set; }

        /// <summary>
        ///仪器通道码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_cno", MedName = "obr_mac_code", WFName = "Lro_Ricc_code")]
        public String ObrMacCode { get; set; }

        /// <summary>
        ///结果a
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_a", MedName = "obr_value", WFName = "Lro_value")]
        public String ObrValue { get; set; }

        /// <summary>
        ///结果b
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_b", MedName = "obr_value2", WFName = "Lro_value2")]
        public String ObrValue2 { get; set; }

        /// <summary>
        ///结果3
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_c", MedName = "obr_value3", WFName = "Lro_value3")]
        public String ObrValue3 { get; set; }

        /// <summary>
        ///结果4
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr_d", MedName = "obr_value4", WFName = "Lro_value4")]
        public String ObrValue4 { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_date", MedName = "obr_date", WFName = "Lro_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_exp", MedName = "obr_remark", WFName = "Lro_remark")]
        public String ObrRemark { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_data_type", MedName = "obr_data_type", WFName = "Lro_data_type")]
        public Int32 ObrDataType { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_read_flag", MedName = "obr_receiver_flag", WFName = "Lro_receiver_flag")]
        public Int32 ObrReceiverFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_id", MedName = "obr_id", WFName = "Lro_Lresdesc_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_critical_flag", MedName = "obr_critical_flag", WFName = "Lro_critical_flag")]
        public String ObrCriticalFlag { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [FieldMapAttribute(ClabName = "res_critical_flag", MedName = "obr_unit", WFName = "Lro_unit")]
        public String ObrUnit { get; set; }

        #region 附加字段 备注
        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "", WFName = "", DBColumn = false)]
        public String ObrMsg
        {
            get
            {
                if (string.IsNullOrEmpty(ItmId))
                    return "通道码设置错误";
                //else if (ObrDelFlag == "1")
                //    return "通道码已失效";
                else return "";
            }
        }
        #endregion

        #region 附加字段 项目编码
        /// <summary>
        ///项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "itm_id", DBColumn = false)]
        public String ItmId { get; set; }
        #endregion

        #region 附加字段 项目代码
        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "itm_ecode", DBColumn = false)]
        public String ItmEname { get; set; }
        #endregion

        #region 样本号整形 
        /// <summary>
        ///样本号整形
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid_int", MedName = "obr_sid_int", WFName = "obr_sid_int", DBColumn = false)]
        public Int64 ObrSidInt { get; set; }
        #endregion

    }
}
