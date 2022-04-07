using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 酶标板孔原始结果表
    /// 旧表名:Obr_Elisa_result 新表名:Lis_Elisa_result
    /// </summary>
    [Serializable]
    public class EntityObrElisaResult : EntityBase
    {
        /// <summary>
        ///仪器编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_itr_id", MedName = "res_itr_id", WFName = "Leres_Ditr_id")]
        public String ResItrId { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_date", MedName = "res_date", WFName = "Leres_date")]
        public DateTime ResDate { get; set; }

        /// <summary>
        ///板号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_bid", MedName = "res_elsaplate", WFName = "Leres_elsaplate")]
        public String ResElsaplate { get; set; }

        /// <summary>
        ///原始结果值
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_resulto", MedName = "res_value", WFName = "Leres_value")]
        public String ResValue { get; set; }

        /// <summary>
        ///结果编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_id", MedName = "res_id", WFName = "Leres_id")]
        public String ResId { get; set; }

        /// <summary>
        ///删除标注
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        ///计算标志 0-未计算 1-已计算
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_flag", MedName = "res_flag", WFName = "Leres_flag")]
        public Int32 ResFlag { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_itm_id", MedName = "res_itm_id", WFName = "Leres_Ditm_id")]
        public String ResItmId { get; set; }

        /// <summary>
        ///OD结果值
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_resulto_od", MedName = "res_resulto_od", WFName = "Leres_resulto_od")]
        public String ResResultoOd { get; set; }

        /// <summary>
        ///结果值
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_resulto_chr", MedName = "res_resulto_chr", WFName = "Leres_resulto_chr")]
        public String ResResultoChr { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_sid", MedName = "res_sid", WFName = "Leres_sid")]
        public String ResSid { get; set; }

        /// <summary>
        ///开始样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_start_sid", MedName = "res_start_sid", WFName = "Leres_start_sid")]
        public String ResStartSid { get; set; }

        /// <summary>
        ///结束样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_end_sid", MedName = "res_end_sid", WFName = "Leres_end_sid")]
        public String ResEndSid { get; set; }

        /// <summary>
        ///试剂批号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_reag_id", MedName = "res_reag_id", WFName = "Leres_reag_id")]
        public String ResReagId { get; set; }

        /// <summary>
        ///试剂有效日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_reag_date", MedName = "res_reag_date", WFName = "Leres_reag_date")]
        public DateTime ResReagDate { get; set; }

        /// <summary>
        ///试剂厂商
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_reag_manu", MedName = "res_reag_manu", WFName = "Leres_reag_manu")]
        public String ResReagManu { get; set; }

        /// <summary>
        ///阳性公式
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_pos_fmlu", MedName = "res_pos_fmlu", WFName = "Leres_pos_fmlu")]
        public String ResPosFmlu { get; set; }

        /// <summary>
        ///出厂日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "imm_manu_date", MedName = "res_manu_date", WFName = "Leres_manu_date")]
        public DateTime ResManuDate { get; set; }

        #region 附加字段 项目代码
        /// <summary>
        /// 项目代码
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode", DBColumn = false)]
        public string ItmEcd { get; set; }
        #endregion

        #region 附加字段 项目名称
        /// <summary>
        /// 项目名称
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public string ItmName { get; set; }
        #endregion

        #region 附加字段 项目报告代码
        /// <summary>
        /// 项目报告代码
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_rep_ecd", MedName = "itm_rep_code", WFName = "Ditm_rep_code", DBColumn = false)]
        public string ItmRepEcd { get; set; }
        #endregion
    }
}
