using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 计算公式字典
    /// 旧表名:Def_itm_calu 新表名:Rel_itm_calculaformula
    /// </summary>
    [Serializable]

    public class EntityDicItmCalu : EntityBase
    {
        public EntityDicItmCalu()
        {
            SortNo = 0;
            CalFlag = 0;
        }
        /// <summary>
        /// 编码
        /// </summary>            
        [FieldMapAttribute(ClabName = "cal_id", MedName = "cal_id",WFName = "Rcalfor_id")]
        public String CalId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_itm_ecd", MedName = "itm_id", WFName = "Rcalfor_Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public String ItmName { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode" , WFName = "Ditm_ecode")]
        public String ItmEcode { get; set; }


        /// <summary>
        /// 计算公式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_fmla", MedName = "cal_expression", WFName = "Rcalfor_expression")]
        public String CalExpression { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        /// 类别 1-计算公式 2效验公式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_flag", MedName = "cal_flag", WFName = "Rcalfor_flag")]
        public Int32 CalFlag { get; set; }

        /// <summary>
        /// 公式代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_variable", MedName = "cal_variable", WFName = "Rcalfor_variable")]
        public String CalVariable { get; set; }

        /// <summary>
        /// 公式名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_name", MedName = "cal_name", WFName = "Rcalfor_name")]
        public String CalName { get; set; }
        /// <summary>
        /// 仪器支持公式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_supportItrCal", MedName = "cal_supportItrCal", WFName = "Rcalfor_supportitrcal")]
        public String CalSupportItrCal { get; set; }

        /// <summary>
        /// 仪器编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_itr_id", MedName = "cal_itr_id", WFName = "Rcalfor_Ditr_id")]
        public String CalItrId { get; set; }

        /// <summary>
        /// 仪器名称
        /// </summary>
        public String CalItrName { get; set; }

        /// <summary>
        /// 仪器代码
        /// </summary>
        public String ItrEname { get; set; }

        /// <summary>
        /// 特殊公式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cal_sp_formula", MedName = "cal_sp_formula",WFName = "Rcalfor_special")]
        public String CalSpFormula { get; set; }

        /// <summary>
        /// 公式类型
        /// </summary>
        public string CalType
        {
            get
            {
                if (CalFlag == 1)
                    return "计算公式";
                else if (CalFlag == 2)
                    return "校验公式";
                else if (CalFlag == 3)
                    return "酶标公式";
                else
                    return string.Empty;
            }
        }
    }
}
