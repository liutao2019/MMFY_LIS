using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码字典表
    /// 旧表名:Def_samp_merge_rule 新表名:Rel_sample_merge_rule
    /// </summary>
    [Serializable]
    public class EntitySampMergeRule : EntityBase
    {
        public EntitySampMergeRule()
        {
            ComCollectQuantity = 0;
            ComPrice = 0;
            ComBarcodeAmount = 0;
            ComReportTime = 0;
            ComDeadspaceVolume = 0;
            ComOutTime2 = 0;
            ComBarcodeType = 0;
        }
        /// <summary>
        ///大组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Rsmr_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///HIS组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_his_name", MedName = "com_his_name", WFName = "Rsmr_com_his_name")]
        public String ComHisName { get; set; }

        /// <summary>
        ///HIS组合编码(与HIS系统字典对应)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_his_code", MedName = "com_his_code", WFName = "Rsmr_com_his_code")]
        public String ComHisCode { get; set; }

        /// <summary>
        ///试管合并类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_m_type", MedName = "com_merge_type", WFName = "Rsmr_merge_type")]
        public String ComMergeType { get; set; }

        /// <summary>
        ///采集容器编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_cuv_code", MedName = "com_tube_code", WFName = "Rsmr_Dtub_code")]
        public String ComTubeCode { get; set; }

        /// <summary>
        ///HIS组合拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_his_py", MedName = "py_code", WFName = "py_code")]
        public String ComPyCode { get; set; }

        /// <summary>
        ///HIS组合五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_his_wb", MedName = "wb_code", WFName = "wb_code")]
        public String ComWbCode { get; set; }

        /// <summary>
        ///项目最小采集量
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_cap_sum", MedName = "com_collect_quantity", WFName = "Rsmr_collect_quantity")]
        public Decimal ComCollectQuantity { get; set; }

        /// <summary>
        ///采集量单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_cap_unit", MedName = "com_collect_unit", WFName = "Rsmr_collect_unit")]
        public String ComCollectUnit { get; set; }

        /// <summary>
        ///标本类别编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_sam_id", MedName = "com_sam_id", WFName = "Rsmr_Dsam_id")]
        public String ComSamId { get; set; }

        /// <summary>
        ///标本来源ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_ori_id", MedName = "com_src_id", WFName = "Rsmr_Dsorc_id")]
        public String ComSrcId { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_price", MedName = "com_price", WFName = "Rsmr_price")]
        public Decimal ComPrice { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_unit", MedName = "com_unit", WFName = "Rsmr_unit")]
        public String ComUnit { get; set; }

        /// <summary>
        ///执行专业组编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_exec_code", MedName = "com_exec_code", WFName = "Rsmr_exec_Dpro_id")]
        public String ComExecCode { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_del_flag", MedName = "del_flag", WFName = "del_flag")]
        public String ComDelFlag { get; set; }

        /// <summary>
        ///组合打印名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_print_name", MedName = "com_print_name", WFName = "Rsmr_print_name")]
        public String ComPrintName { get; set; }

        /// <summary>
        ///条码数量
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_amount", MedName = "com_barcode_amount", WFName = "Rsmr_barcode_amount")]
        public Int32 ComBarcodeAmount { get; set; }

        /// <summary>
        ///出报告时间(单位:分钟)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_out_time", MedName = "com_report_time", WFName = "Rsmr_report_time")]
        public Decimal ComReportTime { get; set; }

        /// <summary>
        ///医院编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_hospital_id", MedName = "com_org_id", WFName = "Rsmr_Dorg_id")]
        public String ComOrgId { get; set; }

        /// <summary>
        ///条码类型(预制条码时用,个别项目需要自动出条码 0-打印生成 1-预制生成)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_bar_type", MedName = "com_barcode_type", WFName = "Rsmr_barcode_type")]
        public Int32? ComBarcodeType { get; set; }

        /// <summary>
        ///大组合拆分标志(0-不拆分 1-拆分) 拆分标志为1时对应dict_combine_split
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_split_flag", MedName = "com_split_flag", WFName = "Rsmr_split_flag")]
        public Int32? ComSplitFlag { get; set; }

        /// <summary>
        ///LIS组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_lis_code", MedName = "com_lis_code", WFName = "Rsmr_lis_code")]
        public String ComLisCode { get; set; }

        /// <summary>
        ///LIS组合来源
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_combine_source", MedName = "com_source", WFName = "Rsmr_source")]
        public String ComSource { get; set; }

        /// <summary>
        ///采血注意事项
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_blood_notice", MedName = "com_sam_notice", WFName = "Rsmr_sam_notice")]
        public String ComSamNotice { get; set; }

        /// <summary>
        ///保存标本注意事项
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_save_notice", MedName = "com_save_notice", WFName = "Rsmr_save_notice")]
        public String ComSaveNotice { get; set; }

        /// <summary>
        ///拆分码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_split_code", MedName = "com_split_code", WFName = "Rsmr_split_code")]
        public String ComSplitCode { get; set; }

        /// <summary>
        ///规则编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_key", MedName = "rul_id", WFName = "Rsmr_id")]
        public Int64 ComRulId { get; set; }

        /// <summary>
        ///大组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_his_fee_code", MedName = "com_his_fee_code", WFName = "Rsmr_his_fee_code")]
        public String ComHisFeeCode { get; set; }

        /// <summary>
        ///标本备注ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_sample_remark_id", MedName = "com_sam_remark", WFName = "Rsmr_sam_remark")]
        public String ComSamRemark { get; set; }

        /// <summary>
        ///报告单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_rep_unit", MedName = "com_report_unit", WFName = "Rsmr_report_unit")]
        public String ComReportUnit { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_split_seq", MedName = "sort_no", WFName = "sort_no")]
        public String ComSortNo { get; set; }

        /// <summary>
        ///打印数量
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_print_count", MedName = "com_print_count", WFName = "Rsmr_print_count")]
        public Int32 ComPrintCount { get; set; }

        /// <summary>
        ///目的地
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_sam_dest", MedName = "com_test_dest", WFName = "Rsmr_test_dest")]
        public String ComTestDest { get; set; }

        /// <summary>
        ///死腔体积
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_deadspace_volume", MedName = "com_deadspace_volume", WFName = "Rsmr_deadspace_volume")]
        public Decimal ComDeadspaceVolume { get; set; }

        /// <summary>
        ///报告单位2
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_rep_unit2", MedName = "com_rep_unit2", WFName = "Rsmr_report_unit2")]
        public String ComRepUnit2 { get; set; }

        /// <summary>
        ///出报告时间(急)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_out_time2", MedName = "com_out_time2", WFName = "Rsmr_urgent_report_time")]
        public Decimal ComOutTime2 { get; set; }

        /// <summary>
        ///拆分保存大组合(特殊合并)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_father_flag", MedName = "com_father_flag", WFName = "Rsmr_father_flag")]
        public Int32 ComFatherFlag { get; set; }

        #region 附加码 组别
        /// <summary>
        /// 实验组别
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加码 来源
        /// <summary>
        /// 来源名称
        /// </summary>
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name", DBColumn = false)]
        public String SrcName { get; set; }
        #endregion

        #region 附加码 样本
        /// <summary>
        /// 样本
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public String SamName { get; set; }
        #endregion

        #region 附加码 试管
        /// <summary>
        /// 试管
        /// </summary>
        [FieldMapAttribute(ClabName = "cuv_name", MedName = "tub_name", WFName = "Dtub_name", DBColumn = false)]
        public String TubName { get; set; }

        #endregion

        #region 附加码 组合信息

        /// <summary>
        /// 组合名称
        /// </summary>          
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ComName { get; set; }

        /// <summary>
        /// 组合排序
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_seq", MedName = "com_seq", WFName = "sort_no", DBColumn = false)]
        public Int32 ComSeq { get; set; }

        /// <summary>
        /// 出报告时间
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_reptimes", MedName = "com_report_time", WFName = "Dcom_report_time", DBColumn = false)]
        public Decimal ComRepTimes { get; set; }

        #endregion

    }
}
