using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控统计：实体类
    /// </summary>
    public class EntityQcStatistic : EntityBase
    {
        /// <summary>
        /// 项目—批号—水平
        /// </summary>   
        [FieldMapAttribute(ClabName = "ITEM", MedName = "ITEM", WFName = "ITEM", DBColumn = false)]
        public String ITEM { get; set; }

        /// <summary>
        /// 靶值（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_x", MedName = "mat_itm_x", WFName = "Rmatdet_itm_x", DBColumn = false)]
        public Double? MatItmX { get; set; }

        /// <summary>
        /// 标准差（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_sd", MedName = "mat_itm_sd", WFName = "Rmatdet_itm_sd", DBColumn = false)]
        public double? MatItmSd { get; set; }

        /// <summary>
        /// CV%
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cv", MedName = "mat_itm_cv", WFName = "Rmatdet_itm_cv")]
        public Decimal? MatItmCv { get; set; }

        /// <summary>
        /// 单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_unit", MedName = "mat_itm_unit", WFName = "Rmatdet_itm_unit")]
        public String MatItmUnit { get; set; }

        /// <summary>
        /// 允许范围CV%
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_allow_cv", MedName = "mat_allow_cv", WFName = "Rmatdet_allow_cv")]
        public Decimal? MatAllowCv { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itm_ecd", MedName = "qres_itm_id", WFName = "Lres_Ditm_id")]
        public String QresItmId { get; set; }

        /// <summary>
        ///质控物ID(qc_par_detail表ID)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_id", MedName = "qres_mat_det_id", WFName = "Lres_Rmatdet_id")]
        public String QresMatDetId { get; set; }

        /// <summary>
        /// N(统计数)
        /// </summary>   
        [FieldMapAttribute(ClabName = "N", MedName = "N", WFName = "N", DBColumn = false)]
        public Int32 N { get; set; }

        /// <summary>
        /// 失控数
        /// </summary>   
        [FieldMapAttribute(ClabName = "失控数", MedName = "失控数", WFName = "失控数", DBColumn = false)]
        public Int32 OutControlNumber { get; set; }

        /// <summary>
        /// 仪器名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "仪器名称", MedName = "仪器名称", WFName = "仪器名称", DBColumn = false)]
        public String ItrName { get; set; }

        /// <summary>
        /// 仪器
        /// </summary>   
        [FieldMapAttribute(ClabName = "仪器", MedName = "仪器", WFName = "仪器", DBColumn = false)]
        public String QresItrId { get; set; }

        /// <summary>
        /// 月份
        /// </summary>   
        [FieldMapAttribute(ClabName = "MONTH", MedName = "MONTH", WFName = "MONTH", DBColumn = false)]
        public String MONTH { get; set; }

        /// <summary>
        /// AVG值(平均值)
        /// </summary>   
        [FieldMapAttribute(ClabName = "AVG", MedName = "AVG", WFName = "AVG", DBColumn = false)]
        public Decimal AVG { get; set; }

        /// <summary>
        /// SD值
        /// </summary>   
        [FieldMapAttribute(ClabName = "SD", MedName = "SD", WFName = "SD", DBColumn = false)]
        public Decimal SD { get; set; }

        /// <summary>
        /// CV值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CV", MedName = "CV", WFName = "CV", DBColumn = false)]
        public Double CV { get; set; }

        /// <summary>
        /// 实际AVG值
        /// </summary>   
        [FieldMapAttribute(ClabName = "ActualAVG", MedName = "ActualAVG", WFName = "ActualAVG", DBColumn = false)]
        public Double ActualAVG { get; set; }

        /// <summary>
        /// 实际SD值
        /// </summary>   
        [FieldMapAttribute(ClabName = "ActualSD", MedName = "ActualSD", WFName = "ActualSD", DBColumn = false)]
        public Double ActualSD { get; set; }

        /// <summary>
        /// 实际CV值
        /// </summary>   
        [FieldMapAttribute(ClabName = "ActualCV", MedName = "ActualCV", WFName = "ActualCV", DBColumn = false)]
        public Double ActualCV { get; set; }

        /// <summary>
        /// ActualFlag值
        /// </summary>   
        [FieldMapAttribute(ClabName = "ActualFlag", MedName = "ActualFlag", WFName = "ActualFlag", DBColumn = false)]
        public Int32 ActualFlag { get; set; }

        /// <summary>
        /// CollectAVG值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectAVG", MedName = "CollectAVG", WFName = "CollectAVG", DBColumn = false)]
        public Double CollectAVG { get; set; }

        /// <summary>
        /// CollectSD值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectSD", MedName = "CollectSD", WFName = "CollectSD", DBColumn = false)]
        public Double CollectSD { get; set; }

        /// <summary>
        /// CollectCV值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectCV", MedName = "CollectCV", WFName = "CollectCV", DBColumn = false)]
        public Double CollectCV { get; set; }

        /// <summary>
        /// CollectN值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectN", MedName = "CollectN", WFName = "CollectN", DBColumn = false)]
        public Int32 CollectN { get; set; }

        /// <summary>
        /// CollectActualAVG值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectActualAVG", MedName = "CollectActualAVG", WFName = "CollectActualAVG", DBColumn = false)]
        public Double CollectActualAVG { get; set; }

        /// <summary>
        /// CollectActualSD值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectActualSD", MedName = "CollectActualSD", WFName = "CollectActualSD", DBColumn = false)]
        public Double CollectActualSD { get; set; }

        /// <summary>
        /// CollectActualCV值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectActualCV", MedName = "CollectActualCV", WFName = "CollectActualCV", DBColumn = false)]
        public Double CollectActualCV { get; set; }

        /// <summary>
        /// CollectActualN值
        /// </summary>   
        [FieldMapAttribute(ClabName = "CollectActualN", MedName = "CollectActualN", WFName = "CollectActualN", DBColumn = false)]
        public Int32 CollectActualN { get; set; }

        /// <summary>
        /// 图(界面上新增的)
        /// </summary>   
        //[FieldMapAttribute(ClabName = "图", MedName = "图", DBColumn = false)]
        //public String Chart { get; set; }

    }
}
