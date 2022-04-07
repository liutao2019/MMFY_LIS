using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 细菌药敏结果表
    /// 旧表名:Obr_result_anti 新表名:Lis_result_anti
    /// </summary>
    [Serializable]
    public class EntityObrResultAnti:EntityBase
    {
        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_id", MedName = "obr_id", WFName = "Lanti_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///细菌编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_bid", MedName = "obr_bac_id", WFName = "Lanti_Dbact_id")]
        public String ObrBacId { get; set; }

        /// <summary>
        ///抗生素编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_aid", MedName = "obr_ant_id", WFName = "Lanti_Dant_id")]
        public String ObrAntId { get; set; }

        /// <summary>
        ///使用方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_dose", MedName = "obr_ant_method", WFName = "Lanti_ant_method")]
        public String ObrAntMethod { get; set; }

        /// <summary>
        ///敏感度
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_mic", MedName = "obr_value", WFName = "Lanti_value")]
        public String ObrValue { get; set; }

        /// <summary>
        ///MIC值
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_smic1", MedName = "obr_value2", WFName = "Lanti_mic")]
        public String ObrValue2 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_smic2", MedName = "obr_value3", WFName = "Lanti_kb")]
        public String ObrValue3 { get; set; }

        /// <summary>
        ///对应的参考值
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_ref", MedName = "obr_ref", WFName = "Lanti_ref")]
        public String ObrRef { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_date", MedName = "obr_date", WFName = "Lanti_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_mid", MedName = "obr_itr_id", WFName = "Lanti_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_sid", MedName = "obr_sid", WFName = "Lanti_Pma_sid")]
        public Decimal ObrSid { get; set; }

        /// <summary>
        ///实验类型 MIC/ZONE
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_test_method", MedName = "obr_method", WFName = "Lanti_method")]
        public String ObrMethod { get; set; }

        /// <summary>
        ///菌落计数
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_bcnt", MedName = "obr_colony_count", WFName = "Lanti_colony_count")]
        public String ObrColonyCount { get; set; }

        /// <summary>
        ///单位(暂不用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_unit", MedName = "obr_unit", WFName = "Lanti_unit")]
        public String ObrUnit { get; set; }

        /// <summary>
        ///所属药敏卡
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_st_id", MedName = "obr_atype_id", WFName = "Lanti_Dantitype_id")]
        public String ObrAtypeId { get; set; }

        /// <summary>
        ///统计标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_row_flag", MedName = "obr_count_flag", WFName = "Lanti_count_flag")]
        public String ObrCountFlag { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_remark", MedName = "anr_remark", WFName = "Lanti_remark")]
        public String AnrRemark { get; set; }

        /// <summary>
        /// 敏感数据
        /// </summary>
        public String Hstd
        {
            get
            {
                if (ObrRef == "MIC")
                {
                    return SsHstd;
                }
                else
                {
                    return SsSzone;
                }
            }
        }

        /// <summary>
        /// 中介数据
        /// </summary>
        public String Mstd
        {
            get
            {
                if (ObrRef == "MIC")
                {
                    return SsMstd;
                }
                else
                {
                    return SsIzone;
                }
            }
        }

        /// <summary>
        /// 耐药数据
        /// </summary>
        public String Lstd
        {
            get
            {
                if (ObrRef == "MIC")
                {
                    return SsLstd;
                }
                else
                {
                    return SsRzone;
                }
            }
        }

        #region 附加字段 敏感MIC
        /// <summary>
        ///敏感MIC
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_hstd", MedName = "ss_hstd", WFName = "ss_hstd", DBColumn =false)]
        public String SsHstd { get; set; }
        #endregion

        #region 附加字段 zone敏感
        /// <summary>
        ///zone敏感
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_szone", MedName = "ss_szone", WFName = "ss_szone", DBColumn =false)]
        public String SsSzone { get; set; }

        [FieldMapAttribute(ClabName = "ss_zone", MedName = "ss_zone", WFName = "ss_zone", DBColumn = false)]
        public String Sszone { get; set; }

        #endregion

        #region 附加字段 中介MIC
        /// <summary>
        ///中介MIC
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_mstd", MedName = "ss_mstd", WFName = "ss_mstd", DBColumn =false)]
        public String SsMstd { get; set; }
        #endregion

        #region 附加字段 zone中介
        /// <summary>
        ///zone中介
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_izone", MedName = "ss_izone", WFName = "ss_izone", DBColumn =false)]
        public String SsIzone { get; set; }
        #endregion

        #region 附加字段 耐药MIC
        /// <summary>
        ///耐药MIC
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_lstd", MedName = "ss_lstd", WFName = "ss_lstd", DBColumn =false)]
        public String SsLstd { get; set; }
        #endregion

        #region 附加字段 zone耐药
        /// <summary>
        ///zone耐药
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_rzone", MedName = "ss_rzone", WFName = "ss_rzone", DBColumn =false)]
        public String SsRzone { get; set; }
        #endregion

        #region 附加字段 细菌名称
        /// <summary>
        ///细菌名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_cname", MedName = "bac_cname", WFName = "Dbact_cname", DBColumn =false)]
        public String BacName { get; set; }
        #endregion

        #region 附加字段 抗生素简码
        /// <summary>
        ///抗生素简码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_code", MedName = "ant_code", WFName = "Dant_code", DBColumn =false)]
        public String AntCode { get; set; }
        #endregion

        #region 附加字段 抗生素名称
        /// <summary>
        ///抗生素名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_cname", MedName = "ant_cname", WFName = "Dant_cname", DBColumn =false)]
        public String AntCname { get; set; }
        #endregion

        #region 附加字段 抗生素英文名称
        /// <summary>
        ///抗生素英文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_ename", MedName = "ant_ename", WFName = "Dant_ename", DBColumn = false)]
        public String AntEname { get; set; }
        #endregion

        #region 附加码 细菌菌类
        /// <summary>
        ///细菌菌类
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_id", MedName = "btype_id", WFName = "Dbactt_id", DBColumn = false)]
        public String BtypeId { get; set; }

        #endregion

        #region 附加码 抗生素字典表ID
        /// <summary>
        ///抗生素字典表ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_id", MedName = "ant_id", WFName = "Dant_id", DBColumn = false)]
        public String AntId { get; set; }

        #endregion

        /// <summary>
        /// 排序字段,,非数据库字段
        /// </summary>
        public string Sstype { get; set; }
        /// <summary>
        /// 分组,非数据库字段
        /// </summary>
        public string Ssaen { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_scripe", MedName = "obr_remark", WFName = "Lbac_remark", DBColumn = false)]
        public String ObrRemark { get; set; }

        /// <summary>
        ///无菌结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "bar_wjtext", MedName = "obr_sterile", WFName = "Lbac_sterile", DBColumn = false)]
        public String ObrSterile { get; set; }

        /// <summary>
        /// 细菌类别
        /// </summary>                       
        [FieldMapAttribute(ClabName = "bac_bt_id", MedName = "bac_bt_id", WFName = "Dbact_Dbactt_id", DBColumn = false)]
        public String BacBtId { get; set; }


        #region 附加字段 历史结果1()
        /// <summary>
        /// 历史结果1
        /// </summary>
        [FieldMapAttribute(ClabName = "history_result1", MedName = "history_result1", WFName = "history_result1", DBColumn = false)]
        public String HistoryResult1 { get; set; }
        #endregion

        #region 附加字段 历史结果2()
        /// <summary>
        /// 历史结果2
        /// </summary>
        [FieldMapAttribute(ClabName = "history_result2", MedName = "history_result2", WFName = "history_result2", DBColumn = false)]
        public String HistoryResult2 { get; set; }
        #endregion

        #region 附加字段 仪器名称
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
    }
}
