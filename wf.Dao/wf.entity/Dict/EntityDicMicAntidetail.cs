using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 药敏标准
    /// 旧表名:Def_mic_antidetail 新表名:Rel_mic_antidetail
    /// </summary>
    [Serializable]
    public class EntityDicMicAntidetail : EntityBase
    {
        public EntityDicMicAntidetail()
        {
            AnsSortNo = 0;
            AnsDefFlag = "0";
        }
        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_st_id", MedName = "def_id", WFName = "Ranti_Dantitype_id")]
        public String AnsDefId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_anti_id", MedName = "anti_code", WFName = "Ranti_Dant_id")]
        public String AnsAntiCode { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_hstd", MedName = "std_upper_limit", WFName = "Ranti_std_upper_limit")]
        public String AnsStdUpperLimit { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_mstd", MedName = "std_middle_limit", WFName = "Ranti_std_middle_limit")]
        public String AnsStdMiddleLimit { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_lstd", MedName = "std_lower_limit", WFName = "Ranti_std_lower_limit")]
        public String AnsStdLowerLimit { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_lozone", MedName = "zone_lower_limit", WFName = "Ranti_zone_lower_limit")]
        public Decimal AnsZoneLowerLimit { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_hizone", MedName = "zone_upper_limit", WFName = "Ranti_zone_upper_limit")]
        public Decimal AnsZoneUpperLimit { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_aen", MedName = "anti_short_name", WFName = "Ranti_short_name")]
        public String AnsAntiShortName { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_flag", MedName = "def_flag", WFName = "Ranti_flag")]
        public String AnsDefFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_rzone", MedName = "zone_durgfast", WFName = "Ranti_zone_durgfast")]
        public String AnsZoneDurgfast { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_izone", MedName = "zone_intermed", WFName = "Ranti_zone_intermed")]
        public String AnsZoneIntermed { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_szone", MedName = "zone_sensitive", WFName = "Ranti_zone_sensitive")]
        public String AnsZoneSensitive { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_del", MedName = "del_flag", WFName = "del_flag")]
        public String AnsDelFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 AnsSortNo { get; set; }

        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_id", MedName = "def_sn", WFName = "Ranti_id")]
        public Int32? AnsDefSn { get; set; }


        /// <summary>
        ///浓度
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_concentration", MedName = "concentration", WFName = "Ranti_concentration")]
        public Decimal? Concentration { get; set; }

        /// <summary>
        ///药物分类（直接填写A、B、C、U、O、INV）
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_group", MedName = "group", WFName = "Ranti_group")]
        public string Group { get; set; }

        /// <summary>
        ///注释
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_notes", MedName = "notes", WFName = "Ranti_notes")]
        public string Notes { get; set; }

        /// <summary>
        ///KB法SDD范围
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_dzone", MedName = "zone_sdd", WFName = "Ranti_zone_sdd")]
        public string ZoneSdd { get; set; }

        /// <summary>
        ///KB法NS范围
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_nzone", MedName = "zone_ns", WFName = "Ranti_zone_ns")]
        public string ZoneNs { get; set; }

        /// <summary>
        ///MIC法NS范围
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_nstd", MedName = "std_ns", WFName = "Ranti_std_ns")]
        public string StdNs { get; set; }

        /// <summary>
        ///MIC法SDD范围
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_dstd", MedName = "std_sdd", WFName = "Ranti_std_sdd")]
        public string StdSdd { get; set; }

        /// <summary>
        ///报告标志（0-不上报临床【即不填默认上报】）
        /// </summary>
        [FieldMapAttribute(ClabName = "report_flag", MedName = "report_flag", WFName = "Ranti_report_flag")]
        public Int32 ReportFlag { get; set; }

        /// <summary>
        ///标本组
        /// </summary>
        [FieldMapAttribute(ClabName = "zone_sam_custom_type", MedName = "zone_sam_custom_type", WFName = "Ranti_sam_type")]
        public string ZoneSamCustomType { get; set; }


        #region 扩展字段

        /// <summary>
        /// 抗生素编码
        /// 旧表名:Dic_mic_antibio  新表名:Dict_mic_antibio
        /// </summary>  
        [FieldMapAttribute(ClabName = "anti_id", MedName = "ant_id", WFName = "Dant_id", DBColumn = false)]
        public String AntId { get; set; }

        /// <summary>
        ///菌种编码
        ///旧表名:Dic_mic_bacttype  新表名:Dict_mic_bacttype
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_id", MedName = "btype_id", WFName = "Dbactt_id", DBColumn = false)]
        public String BtypeId { get; set; }


        /// <summary>
        /// 抗生素名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "anti_cname", MedName = "ant_cname", WFName = "Dant_cname", DBColumn = false)]
        public String AntCname { get; set; }

        /// <summary>
        ///对应的参考值
        ///旧表名:Obr_result_anti  新表名:Lis_result_anti 
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_ref", MedName = "obr_ref", WFName = "Lanti_ref", DBColumn = false)]
        public String ObrRef { get; set; }


        [FieldMapAttribute(ClabName = "ss_zone", MedName = "ss_zone", WFName = "ss_zone", DBColumn = false)]
        public String Sszone { get; set; }

        public bool  isselected { get; set; }

        /// <summary>
        ///药敏卡名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_cname", MedName = "atype_name", WFName = "Dantitype_name", DBColumn = false)]
        public String AtypeName { get; set; }

        /// <summary>
        ///抗生素排序标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_seq", MedName = "anti_sort_no", WFName = "anti_sort_no", DBColumn = false)]
        public string AntiSortNo { get; set; }

        #endregion
    }
}
