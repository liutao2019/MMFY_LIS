using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目字典表
    /// 旧表名:Dic_itm_item 新表名:Dict_itm
    /// </summary>
    [Serializable()]
    public class EntityDicItmItem : EntityBase
    {
        public EntityDicItmItem()
        {
            ItmCaluFlag = 0;
        }

        /// <summary>
        ///项目ID
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name")]
        public String ItmName { get; set; }

        /// <summary>
        ///专业组
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ptype", MedName = "itm_pri_id", WFName = "Ditm_pri_id")]
        public String ItmPriId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode")]
        public String ItmEcode { get; set; }

        /// <summary>
        ///开始日期
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_sdate", MedName = "itm_start_date", WFName = "Ditm_start_date")]
        public DateTime ItmStartDate { get; set; }

        /// <summary>
        ///结束日期
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_edate", MedName = "itm_end_date", WFName = "Ditm_end_date")]
        public DateTime ItmEndDate { get; set; }

        /// <summary>
        ///收费标志 0-不收费 1-收费
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_chg_flag", MedName = "itm_charge_flag", WFName = "Ditm_charge_flag")]
        public Int32 ItmChargeFlag { get; set; }

        /// <summary>
        ///是否质控 0-不需质控 1-需质控
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_qc_flg", MedName = "itm_qc_flag", WFName = "Ditm_qc_flag")]
        public Int32 ItmQcFlag { get; set; }

        /// <summary>
        ///打印标志 0-不打印 1-打印
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_prt_flag", MedName = "itm_prt_flag", WFName = "Ditm_prt_flag")]
        public Int32 ItmPrtFlag { get; set; }

        /// <summary>
        ///镜检标志 0-非镜检 1-镜检
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ugr_flag", MedName = "itm_micr_flag", WFName = "Ditm_micr_flag")]
        public Int32 ItmMicrFlag { get; set; }

        /// <summary>
        ///是否计算项目 0-否 1-是  (当设置为是时，在检验报告中此项目将不能手工录入，将根据项目计算公式自动计算)
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_cal_flag", MedName = "itm_calu_flag", WFName = "Ditm_calu_flag")]
        public Int32 ItmCaluFlag { get; set; }

        /// <summary>
        ///传染病标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ill_flag", MedName = "itm_infection_flag", WFName = "Ditm_infection_flag")]
        public Int32 ItmInfectionFlag { get; set; }

        /// <summary>
        ///镜检类型
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ugr_id", MedName = "itm_micr_type", WFName = "Ditm_micr_type")]
        public String ItmMicrType { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_py", MedName = "py_code", WFName = "py_code")]
        public String ItmPyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_wb", MedName = "wb_code", WFName = "wb_code")]
        public String ItmWbCode { get; set; }

        /// <summary>
        ///删除标志 1=已删除 0=正常
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_del", MedName = "del_flag", WFName = "del_flag")]
        public String ItmDelFlag { get; set; }

        /// <summary>
        ///排序编号(序号)
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ItmSortNo { get; set; }

        /// <summary>
        ///报表打印时使用的项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_rep_ecd", MedName = "itm_rep_code", WFName = "Ditm_rep_code")]
        public String ItmRepCode { get; set; }

        /// <summary>
        ///性别限制  0-不限制 1-男 2-女
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_sex_limit", MedName = "itm_match_sex", WFName = "Ditm_match_sex")]
        public String ItmMatchSex { get; set; }

        /// <summary>
        ///描述双列报告时，项目显示的位置 0-左列 1-右列
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_rep_col", MedName = "itm_print_side", WFName = "Ditm_print_side")]
        public Int32 ItmPrintSide { get; set; }

        /// <summary>
        ///价格
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_pri", MedName = "itm_price", WFName = "Ditm_price")]
        public Decimal ItmPrice { get; set; }

        /// <summary>
        ///成本
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_cost", MedName = "itm_cost", WFName = "Ditm_cost")]
        public Decimal ItmCost { get; set; }

        /// <summary>
        ///实验方法
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_meams", MedName = "itm_method", WFName = "Ditm_method")]
        public String ItmMethod { get; set; }

        /// <summary>
        ///his组合的编码(现基本不用)
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_his_code", MedName = "itm_his_code", WFName = "Ditm_his_code")]
        public String ItmHisCode { get; set; }

        /// <summary>
        ///对照代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_con_code", MedName = "itm_contrast_code", WFName = "Ditm_contrast_code")]
        public String ItmContrastCode { get; set; }

        /// <summary>
        ///对照方式（暂定）
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_con_ftor", MedName = "itm_contrast_factor", WFName = "Ditm_contrast_factor")]
        public String ItmContrastFactor { get; set; }

        /// <summary>
        /// 质评标志
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_qc_type", MedName = "itm_qc_type", WFName = "Ditm_qc_type")]
        public Int32 ItmQcType { get; set; }

        /// <summary>
        /// 专业组名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public string ProName { get; set; }

        /// <summary>
        /// 项目意义
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_content", MedName = "itm_content", WFName = "Ditm_content")]
        public string ItmContent { get; set; }

        /// <summary>
        /// 正常值
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_ref", MedName = "itm_ref", WFName = "Ditm_ref")]
        public string ItmRef { get; set; }

        #region 附加字段
        /// <summary>
        /// 医院ID
        /// </summary>
        public String ItmHosID { get; set; }
        #endregion


        #region 附加字段 是否有项目特征
        /// <summary>
        /// 有无项目特征
        /// </summary>
        [FieldMapAttribute(ClabName = "propCount", MedName = "propCount", WFName = "propCount", DBColumn = false)]
        public String propCount { get; set; }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }
        #endregion

        #region 附加字段 组合id
        /// <summary>
        /// 是否选中
        /// </summary>
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Rici_Dcom_id", DBColumn =false)]
        public string ComId { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        /// 是否选中
        /// </summary>
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public string ComName { get; set; }
        #endregion

        /// <summary>
        /// 临床意义
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_sign", MedName = "itm_meaning", WFName = "Ritm_meaning", DBColumn = false)]
        public string ItmMeaning { get; set; }

        /// <summary>
        /// 标本结果影响因素
        /// </summary>
        [FieldMapAttribute(ClabName = "result_influence", MedName = "result_influence", WFName = "Ritm_result_influence", DBColumn = false)]
        public string ResultInfluence { get; set; }

    }
}
