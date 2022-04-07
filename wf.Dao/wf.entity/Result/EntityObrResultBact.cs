using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 细菌菌名数据表
    /// 旧表名:Obr_result_bact 新表名:Lis_result_bact
    /// </summary>
    [Serializable]
    public class EntityObrResultBact:EntityBase
    {
        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_id", MedName = "obr_id", WFName = "Lbac_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_mid", MedName = "obr_itr_id", WFName = "Lbac_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_date", MedName = "obr_date", WFName = "Lbac_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_sid", MedName = "obr_sid", WFName = "Lbac_Pma_sid")]
        public Decimal ObrSid { get; set; }

        /// <summary>
        ///细菌编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_bid", MedName = "obr_bac_id", WFName = "Lbac_Dbact_id")]
        public Decimal ObrBacId { get; set; }

        /// <summary>
        ///菌落记数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_bcnt", MedName = "obr_colony_count", WFName = "Lbac_colony_count")]
        public String ObrColonyCount { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_scripe", MedName = "obr_remark", WFName = "Lbac_remark")]
        public String ObrRemark { get; set; }

        /// <summary>
        ///排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        ///无菌结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_wjtext", MedName = "obr_sterile", WFName = "Lbac_sterile")]
        public String ObrSterile { get; set; }

        /// <summary>
        ///多重耐药标志 0 1为多重耐药
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_mrb_flag", MedName = "obr_mrb_flag", WFName = "Lbac_mrb_flag")]
        public Int32 ObrMrbFlag { get; set; }

        /// <summary>
        ///传染病标志 0 1为传染病
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_idd_flag", MedName = "obr_idd_flag", WFName = "Lbac_idd_flag")]
        public Int32 ObrIddFlag { get; set; }

        #region 附加码 细菌中文名称
        /// <summary>
        ///细菌中文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_cname", MedName = "bac_cname", WFName = "Dbact_cname", DBColumn =false)]
        public String BacCname { get; set; }
        #endregion

        #region 附加码 细菌英文名称
        /// <summary>
        ///细菌英文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_ename", MedName = "bac_ename", WFName = "Dbact_ename", DBColumn = false)]
        public String BacEname { get; set; }
        #endregion

        #region 附加码 细菌菌类
        /// <summary>
        ///细菌菌类
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_id", MedName = "btype_id", WFName = "Dbactt_id", DBColumn = false)]
        public String BtypeId { get; set; }
        #endregion

        #region 附加码 仪器名称
        /// <summary>
        ///仪器英文名
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_ename", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEName { get; set; }

        /// <summary>
        ///仪器中文名
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public String ItrName { get; set; }
        #endregion

        #region 附加字段 仪器子标题（报告单类型）
        /// <summary>
        /// 仪器子标题（报告单类型）
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_stitle", MedName = "itr_sub_title", WFName = "Ditr_sub_title", DBColumn = false)]
        public String DitrSubTitle { get; set; }
        #endregion
    }
}
