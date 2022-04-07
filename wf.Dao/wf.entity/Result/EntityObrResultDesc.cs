using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 描述报告结果表
    /// 旧表:Obr_result_desc 新表:Lis_result_desc
    /// </summary>
    [Serializable]
    public class EntityObrResultDesc:EntityBase
    {
        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_id", MedName = "obr_id", WFName = "Lrd_id", DBIdentity = true)]
        public String ObrId { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_mid", MedName = "obr_itr_id", WFName = "Lrd_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_date", MedName = "obr_date", WFName = "Lrd_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_sid", MedName = "obr_sid", WFName = "Lrd_sid")]
        public Decimal ObrSid { get; set; }

        /// <summary>
        ///涂片结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_cname", MedName = "obr_value", WFName = "Lrd_value")]
        public String ObrValue { get; set; }

        /// <summary>
        ///检测对象（院感）
        /// </summary>   
        [FieldMapAttribute(ClabName = "Lrd_checkobj", MedName = "Lrd_checkobj", WFName = "Lrd_checkobj")]
        public String ObrCheckObj { get; set; }

        /// <summary>
        ///描述内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_describe", MedName = "obr_describe", WFName = "Lrd_describe")]
        public String ObrDescribe { get; set; } 

        /// <summary>
        ///结果类型 0-涂片结果 1-描述结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_res_flag", MedName = "obr_flag", WFName = "Lrd_flag")]
        public Int32 ObrFlag { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        ///阳性标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bsr_i_flag", MedName = "obr_positive_flag", WFName = "Lrd_positive_flag")]
        public Int32 ObrPositiveFlag { get; set; }

        #region 附加字段 是否选中
        /// <summary>
        ///是否选中
        /// </summary>   
        [FieldMapAttribute(ClabName = "selected", MedName = "selected", WFName = "selected", DBColumn =false)]
        public Boolean Selected { get; set; }
        #endregion

        #region 附加字段 仪器子标题（报告单类型）
        /// <summary>
        /// 仪器子标题（报告单类型）
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_stitle", MedName = "itr_sub_title", WFName = "Ditr_sub_title", DBColumn = false)]
        public String DitrSubTitle { get; set; }
        #endregion

        /// <summary>
        ///仪器中文名
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public String ItrName { get; set; }

        public String PositiveFlag
        {
            get
            {
                if (ObrPositiveFlag == 1)
                {
                    return "阳性";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
