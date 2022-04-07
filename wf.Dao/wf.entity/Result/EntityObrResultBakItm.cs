using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目结果备份表
    /// </summary>
    [Serializable]
    public class EntityObrResultBakItm : EntityBase
    {
        public EntityObrResultBakItm()
        {
            ResKey = 0;
            ResCastChr = 0;
            ResPrice = 0;
            ResFlag = 0;
            ResType = 0;
            ResRepType = 0;
            ResRefType = 0;
            ResRecheckFlag = 0;
        }

        /// <summary>
        ///唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_key", MedName = "res_key", WFName = "res_key")]
        public Int64 ResKey { get; set; }

        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_id", MedName = "res_id", WFName = "res_id")]
        public String ResId { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_id", MedName = "res_itr_id", WFName = "res_itr_id")]
        public String ResItrId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid", MedName = "res_sid", WFName = "res_sid")]
        public String ResSid { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_id", MedName = "res_itm_id", WFName = "res_itm_id")]
        public String ResItmId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_ecd", MedName = "res_itm_ecd", WFName = "res_itm_ecd")]
        public String ResItmEcd { get; set; }

        /// <summary>
        ///结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr", MedName = "res_chr", WFName = "res_chr")]
        public String ResChr { get; set; }

        /// <summary>
        ///OD结果(仪器类型为酶标时插入)
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_od_chr", MedName = "res_od_chr", WFName = "res_od_chr")]
        public String ResOdChr { get; set; }

        /// <summary>
        ///数值结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_cast_chr", MedName = "res_cast_chr", WFName = "res_cast_chr")]
        public Decimal ResCastChr { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_unit", MedName = "res_unit", WFName = "res_unit")]
        public String ResUnit { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_price", MedName = "res_price", WFName = "res_price")]
        public Decimal ResPrice { get; set; }

        /// <summary>
        ///参考值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_l", MedName = "res_ref_l", WFName = "res_ref_l")]
        public String ResRefL { get; set; }

        /// <summary>
        ///参考值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_h", MedName = "res_ref_h", WFName = "res_ref_h")]
        public String ResRefH { get; set; }

        /// <summary>
        ///参考值分期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_exp", MedName = "res_ref_exp", WFName = "res_ref_exp")]
        public String ResRefExp { get; set; }

        /// <summary>
        ///阳性标志(阳性为'3'，未知'-1',正常为'0'）
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_flag", MedName = "res_ref_flag", WFName = "res_ref_flag")]
        public String ResRefFlag { get; set; }

        /// <summary>
        ///实验方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_meams", MedName = "res_meams", WFName = "res_meams")]
        public String ResMeams { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_date", MedName = "res_date", WFName = "res_date")]
        public DateTime ResDate { get; set; }

        /// <summary>
        ///有效标志 0-历史结果 1-生效结果  默认为1
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_flag", MedName = "res_flag", WFName = "res_flag")]
        public Int32 ResFlag { get; set; }

        /// <summary>
        ///结果类型 0-手工输入 1仪器传输 2计算  默认为0
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_type", MedName = "res_type", WFName = "res_type")]
        public Int32 ResType { get; set; }

        /// <summary>
        ///报告类型 0-普通 1-OD结果 默认为0
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_rep_type", MedName = "res_rep_type", WFName = "res_rep_type")]
        public Int32 ResRepType { get; set; }

        /// <summary>
        ///组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_com_id", MedName = "res_com_id", WFName = "res_com_id")]
        public String ResComId { get; set; }

        /// <summary>
        ///报表打印时项目使用的编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_rep_ecd", MedName = "res_itm_rep_ecd", WFName = "res_itm_rep_ecd")]
        public String ResItmRepEcd { get; set; }

        /// <summary>
        ///仪器原始代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_ori_id", MedName = "res_itr_ori_id", WFName = "res_itr_ori_id")]
        public String ResItrOriId { get; set; }

        /// <summary>
        ///参考值类型 0=常规 1=分期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_type", MedName = "res_ref_type", WFName = "res_ref_type")]
        public Int32 ResRefType { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_exp", MedName = "res_exp", WFName = "res_exp")]
        public String ResExp { get; set; }

        /// <summary>
        ///复查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_recheck_flag", MedName = "res_recheck_flag", WFName = "res_recheck_flag")]
        public Int32 ResRecheckFlag { get; set; }

        /// <summary>
        ///项目结果2
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr2", MedName = "res_chr2", WFName = "res_chr2")]
        public String ResChr2 { get; set; }

        /// <summary>
        ///项目结果3
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr3", MedName = "res_chr3", WFName = "res_chr3")]
        public String ResChr3 { get; set; }

        /// <summary>
        ///备份ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bak_id", MedName = "bak_id", WFName = "bak_id")]
        public String BakId { get; set; }

        /// <summary>
        ///备份日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bak_date", MedName = "bak_date", WFName = "bak_date")]
        public DateTime BakDate { get; set; }

        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion 

        #region 附加字段 是否勾选
        /// <summary>
        /// 是否勾选
        /// </summary>
        public Boolean IsSelected { get; set; }
        #endregion
    }
}
