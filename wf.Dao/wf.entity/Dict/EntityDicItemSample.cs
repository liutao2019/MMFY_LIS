using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目标本表
    /// 旧表名:Def_itm_sample 新表名:Rel_itm_sample
    /// </summary>
    [Serializable()]
    public class EntityDicItemSample : EntityBase
    {

        public EntityDicItemSample()
        {
            this.ListPositiveResult = new List<string>();
            this.ListUrgentResult = new List<string>();
        }

        /// <summary>
        ///项目ID 对应dict_item表中的itm_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Ritm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///标本ID 对应dict_sample表中的sam_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_id", MedName = "itm_sam_id", WFName = "Ritm_sam_id")]
        public String ItmSamId { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pri", MedName = "itm_price", WFName = "Ritm_price")]
        public Decimal ItmPrice { get; set; }

        /// <summary>
        ///成本
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_cost", MedName = "itm_cost", WFName = "Ritm_cost")]
        public Decimal ItmCost { get; set; }

        /// <summary>
        ///实验方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_meams", MedName = "itm_method", WFName = "Ritm_method")]
        public String ItmMethod { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_unit", MedName = "itm_unit", WFName = "Ritm_unit")]
        public String ItmUnit { get; set; }

        /// <summary>
        ///默认值
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_defa", MedName = "itm_default", WFName = "Ritm_default")]
        public String ItmDefault { get; set; }

        /// <summary>
        ///结果类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_dtype", MedName = "itm_res_type", WFName = "Ritm_res_type")]
        public String ItmResType { get; set; }

        /// <summary>
        ///是否效验
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_vali", MedName = "itm_valid", WFName = "Ritm_valid")]
        public Int32 ItmValid { get; set; }

        /// <summary>
        ///临床意义
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sign", MedName = "itm_meaning", WFName = "Ritm_meaning")]
        public String ItmMeaning { get; set; }

        /// <summary>
        ///偏低提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_l_info", MedName = "itm_lower_tips", WFName = "Ritm_lower_tips")]
        public String ItmLowerTips { get; set; }

        /// <summary>
        ///偏高提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_h_info", MedName = "itm_upper_tips", WFName = "Ritm_upper_tips")]
        public String ItmUpperTips { get; set; }

        /// <summary>
        ///使用方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_use_exp", MedName = "itm_use_method", WFName = "Ritm_use_method")]
        public String ItmUseMethod { get; set; }

        /// <summary>
        ///互认标记
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_att_flag", MedName = "itm_accept_flag", WFName = "Ritm_accept_flag")]
        public Int32 ItmAcceptFlag { get; set; }

        /// <summary>
        ///最大小数位（0表示不限定小数位）
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_max_digit", MedName = "itm_max_digit", WFName = "Ritm_max_digit")]
        public Int32 ItmMaxDigit { get; set; }

        /// <summary>
        ///对照结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_comparison_result", MedName = "itm_compare_res", WFName = "Ritm_compare_res")]
        public String ItmCompareRes { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_del", MedName = "del_flag", WFName = "del_flag")]
        public String ItmDelFlag { get; set; }

        /// <summary>
        ///结果差异系数
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_diff", MedName = "itm_diff", WFName = "Ritm_diff")]
        public Decimal ItmDiff { get; set; }

        /// <summary>
        ///历史结果有效时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_HistroyResTime", MedName = "itm_valid_days", WFName = "Ritm_valid_days")]
        public Int32 ItmValidDays { get; set; }



        public List<string> ListPositiveResult { get; private set; }
        private string _ItmPositiveRes;
        /// <summary>
        ///阳性结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_positive_result", MedName = "itm_positive_res", WFName = "Ritm_positive_res")]
        public String ItmPositiveRes
        {
            get
            {
                return this._ItmPositiveRes;
            }
            set
            {
                this._ItmPositiveRes = value;

                this.ListPositiveResult.Clear();
                if (value != null && value.ToString() != string.Empty)
                {
                    this.ListPositiveResult.AddRange(value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }



        public List<string> ListUrgentResult { get; private set; }
        private string _ItmUrgentRes;
        /// <summary>
        ///危急结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_urgent_result", MedName = "itm_urgent_res", WFName = "Ritm_urgent_res")]
        public String ItmUrgentRes
        {
            get
            {
                return this._ItmUrgentRes;
            }
            set
            {
                this._ItmUrgentRes = value;

                this.ListUrgentResult.Clear();
                if (value != null && value.ToString() != string.Empty)
                {
                    this.ListUrgentResult.AddRange(value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_itr_id", MedName = "itm_itr_id", WFName = "Ritm_Ditr_id")]
        public String ItmItrId { get; set; }

        /// <summary>
        /// 危急值结果
        /// </summary>
        [FieldMapAttribute(ClabName = "danger_result",MedName ="danger_result", WFName = "Ritm_danger_result")]
        public String DangerResult { get; set; }

        /// <summary>
        /// 结果影响因素
        /// </summary>
        [FieldMapAttribute(ClabName = "result_influence",MedName = "result_influence", WFName = "Ritm_result_influence")]
        public String ResultInfluence { get; set; }

        /// <summary>
        /// 决定性结果
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_result",MedName = "Decisive_result", WFName = "Ritm_decisive_result")]
        public String DecisiveResult { get; set; }

        /// <summary>
        /// 意义
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_result_influence",MedName = "Decisive_result_influence", WFName = "Ritm_decisive_result_influence")]
        public String DecisiveResultInfluence { get; set; }

        #region  附加字段
        /// <summary>
        ///标本类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public String ItmSamName { get; set; }

        /// <summary>
        ///所属仪器
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItmItrImid { get; set; }
        

        #region  附加字段
        /// <summary>
        ///排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sort", MedName = "itm_sort", WFName = "itm_sort", DBColumn = false)]
        public Int32 ItmSort { get; set; }
        #endregion
        #endregion

    }
}
