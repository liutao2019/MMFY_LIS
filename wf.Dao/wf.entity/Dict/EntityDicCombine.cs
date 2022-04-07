using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 组合表
    /// 旧表名:Dic_itm_combine 新表名:Dict_itm_combine
    /// </summary>
    [Serializable()]
    public class EntityDicCombine : EntityBase
    {
        public EntityDicCombine()
        {
            Checked = false;
            ComUrgentFlag = 0;
        }
        /// <summary>
        ///编码
        /// </summary>        
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///名称
        /// </summary>          
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name")]
        public String ComName { get; set; }

        /// <summary>
        ///物理组
        /// </summary>                    
        [FieldMapAttribute(ClabName = "com_ctype", MedName = "com_lab_id", WFName = "Dcom_lab_id")]
        public String ComLabId { get; set; }

        /// <summary>
        ///价格
        /// </summary>    
        [FieldMapAttribute(ClabName = "com_pri", MedName = "com_price", WFName = "Dcom_price")]
        public Decimal ComPrice { get; set; }

        /// <summary>
        ///成本
        /// </summary>  
        [FieldMapAttribute(ClabName = "com_cost", MedName = "com_cost", WFName = "Dcom_cost")]
        public Decimal ComCost { get; set; }

        /// <summary>
        ///项目件数
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_cnt", MedName = "com_item_amount", WFName = "Dcom_item_amount")]
        public Int32 ComItemAmount { get; set; }

        /// <summary>
        ///备注
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_rem", MedName = "com_remark", WFName = "Dcom_remark")]
        public String ComRemark { get; set; }

        /// <summary>
        ///His代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_code", MedName = "com_code", WFName = "Dcom_code")]
        public String ComCode { get; set; }

        /// <summary>
        ///专业组
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_ptype", MedName = "com_pri_id", WFName = "Dcom_Dpro_id")]
        public String ComPriId { get; set; }

        /// <summary>
        ///默认标本ID
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_sam_id", MedName = "com_sam_id", WFName = "Dcom_Dsam_id")]
        public String ComSamId { get; set; }

        /// <summary>
        ///默认仪器
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_itr_id", MedName = "com_itr_id", WFName = "Dcom_Ditr_id")]
        public String ComItrId { get; set; }

        /// <summary>
        ///输入码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_incode", MedName = "c_code", WFName = "Dcom_c_code")]
        public String ComCCode { get; set; }

        /// <summary>
        ///发报告时间
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_reptimes", MedName = "com_report_time", WFName = "Dcom_report_time")]
        public Int32 ComReportTime { get; set; }

        /// <summary>
        /// 发报告时间单位 天/时/分
        /// </summary>
        [FieldMapAttribute(ClabName = "com_rep_unit", MedName = "com_time_unit", WFName = "Dcom_time_unit")]
        public string ComTimeUnit { get; set; }
        /// <summary>
        ///拼音码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_py", MedName = "py_code", WFName = "py_code")]
        public String ComPyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_wb", MedName = "wb_code", WFName = "wb_code")]
        public String ComWbCode { get; set; }

        /// <summary>
        ///排序
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ComSortNo { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_del", MedName = "del_flag", WFName = "del_flag")]
        public String ComDelFlag { get; set; }

        /// <summary>
        ///His收费代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_his_fee_code", MedName = "com_his_code", WFName = "Dcom_his_code")]
        public String ComHisCode { get; set; }

        /// <summary>
        ///拆分标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_split_flag", MedName = "com_split_flag", WFName = "Dcom_split_flag")]
        public Int32 ComSplitFlag { get; set; }

        /// <summary>
        ///取报告时间
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_get_report_time", MedName = "com_get_report_time", WFName = "Dcom_get_report_time")]
        public String ComGetReportTime { get; set; }

        /// <summary>
        ///危急值报告次数（暂定）
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_urgent_times", MedName = "com_urgent_report_time", WFName = "Dcom_urgent_report_time")]
        public String ComUrgentReportTime { get; set; }

        /// <summary>
        ///输血危急值次数（暂定）
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_bolld_send_urgent_times", MedName = "com_urgent_blood_send_times", WFName = "Dcom_urgent_blood_send_times")]
        public String ComUrgentBloodSendTimes { get; set; }

        /// <summary>
        ///输血正常次数（暂定）
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_bolld_send_common_times", MedName = "com_routine_blood_send_times", WFName = "Dcom_routine_blood_send_times")]
        public String ComRoutineBloodSendTimes { get; set; }

        /// <summary>
        ///质控标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_qc_flg", MedName = "com_qc_flag", WFName = "Dcom_qc_flag")]
        public Int32 ComQcFlag { get; set; }

        /// <summary>
        ///提示码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_labcode_type", MedName = "com_laboratory_code", WFName = "Dcom_laboratory_code")]
        public String ComLaboratoryCode { get; set; }

        /// <summary>
        ///加急标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_urgent_flag", MedName = "com_urgent_flag", WFName = "Dcom_urgent_flag")]
        public Int32 ComUrgentFlag { get; set; }

        /// <summary>
        ///共享标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_sme_flag", MedName = "com_sme_flag", WFName = "Dcom_sme_flag")]
        public Int32 ComSmeFlag { get; set; }

        /// <summary>
        ///提示状态
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_line_status", MedName = "com_online_status", WFName = "Dcom_online_status")]
        public String ComOnlineStatus { get; set; }

        /// <summary>
        ///颜色状态
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_line_color", MedName = "com_online_clolr", WFName = "Dcom_online_clolr")]
        public Int32 ComOnlineClolr { get; set; }

        /// <summary>
        ///组合缩写
        /// </summary>
        [FieldMapAttribute(ClabName = "com_classify", MedName = "com_classify", WFName = "Dcom_classify")]
        public string ComClassify { get; set; }

        /// <summary>
        ///结核实验药敏类型 （1-快速药敏；2-mic药敏；3-罗氏药敏；4-鉴定结果；5-分泌蛋白结果；）
        /// </summary>
        [FieldMapAttribute(ClabName = "com_mt_tpe", MedName = "com_mt_tpe", WFName = "Dcom_mt_tpe")]
        public Int32 ComMtTpe { get; set; }

        /// <summary>
        ///历史结果类型（1-普通药敏；）
        /// </summary>
        [FieldMapAttribute(ClabName = "com_rsl_flag", MedName = "com_rsl_flag", WFName = "Dcom_rsl_flag")]
        public Int32 ComRslFlag { get; set; }

        /// <summary>
        ///报表类型(数字)
        /// </summary>
        [FieldMapAttribute(ClabName = "com_rpt_type", MedName = "com_rpt_type", WFName = "Dcom_rpt_type")]
        public string ComRptType { get; set; }

        /// <summary>
        ///报表中文名称（
        /// </summary>
        [FieldMapAttribute(ClabName = "com_rpt_cname", MedName = "com_rpt_cname", WFName = "Dcom_rpt_cname")]
        public string ComRptCname { get; set; }

        /// <summary>
        ///
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_class", MedName = "com_class", WFName = "Dcom_class")]
        public String ComClass { get; set; }

        /// <summary>
        /// 采血前注意事项
        /// </summary>
        [FieldMapAttribute(ClabName = "com_blood_notice1", MedName = "com_sam_notice1", WFName = "Dcom_sam_notice")]
        public String ComSamNotice1 { get; set; }

        /// <summary>
        /// 组合意义
        /// </summary>
        [FieldMapAttribute(ClabName = "com_content", MedName = "com_content", WFName = "Dcom_content")]
        public String ComContent { get; set; }

        /// <summary>
        /// 标本采集
        /// </summary>
        [FieldMapAttribute(ClabName = "com_collection_notice", MedName = "com_collection_notice", WFName = "Dcom_collection_notice")]
        public String ComCollectionNotice { get; set; }

        /// <summary>
        /// 标本流转与存储
        /// </summary>
        [FieldMapAttribute(ClabName = "com_save_notice1", MedName = "com_save_notice1", WFName = "Dcom_save_notice")]
        public String ComSaveNotice1 { get; set; }

        /// <summary>
        /// 检测影响因素
        /// </summary>
        [FieldMapAttribute(ClabName = "com_influence", MedName = "com_influence", WFName = "Dcom_influence")]
        public String ComInfluence { get; set; }

        #region 附加字段 医院ID
        /// <summary>
        /// 医院ID
        /// </summary>
        public String ComHosID { get; set; }

        /// <summary>
        /// 专业组名称
        /// </summary>
        public String ComProName { get; set; }

        /// <summary>
        /// 拆分标志
        /// </summary>
        [FieldMapAttribute(ClabName = "bar_split", MedName = "bar_split", WFName = "bar_split", DBColumn = false)]
        public Int32 ComBarSplit { get; set; }

        #endregion

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 物理组名
        [FieldMapAttribute(ClabName = "ctype_name", MedName = "ctype_name", WFName = "ctype_name", DBColumn = false)]
        public String CTypeName { get; set; }
        #endregion

        #region 附加字段 专业组名
        [FieldMapAttribute(ClabName = "ptype_name", MedName = "ptype_name", WFName = "ptype_name", DBColumn = false)]
        public String PTypeName { get; set; }
        #endregion

        #region 附加字段 条码类型
        [FieldMapAttribute(ClabName = "bar_type", MedName = "bar_type", WFName = "bar_type", DBColumn = false)]
        public String BarType { get; set; }
        #endregion

        #region 附加字段 默认标本
        /// <summary>
        ///默认标本
        /// </summary>                       
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public String ComSamName { get; set; }
        #endregion

        #region 附加字段 组别
        /// <summary>
        ///0是专业组 1是物理组
        /// </summary>                       
        [FieldMapAttribute(ClabName = "type_flag", MedName = "type_flag", WFName = "type_flag", DBColumn = false)]
        public Int32 ComTypeFlag { get; set; }
        #endregion

        #region 附加字段 组别
        public String ComType
        {
            get
            {
                if (ComTypeFlag == 0)
                    return "专业组";
                else if (ComTypeFlag == 1)
                    return "物理组";
                else
                    return string.Empty;
            }
        }
        #endregion

        #region 附加字段 His收费名称
        /// <summary>
        ///His收费名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_his_name", MedName = "com_his_name", WFName = "com_his_name", DBColumn = false)]
        public String ComHisName { get; set; }
        #endregion

        #region 附加字段 仪器名称
        /// <summary>
        ///仪器名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public String ComItrName { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return ComId;
            }
        }
        #endregion

        #region 附加字段 是否紧急
        /// <summary>
        /// 是否紧急
        /// </summary>
        public Boolean Urgent { get; set; }
        #endregion
    }
}
