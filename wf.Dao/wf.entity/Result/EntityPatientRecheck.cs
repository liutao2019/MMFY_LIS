using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace dcl.entity
{
    /// <summary>
    /// 
    /// 旧表名:patients_recheck 新表名:Pat_recheck
    /// </summary>
    [Serializable]
    public class EntityPatientRecheck
    {
        /// <summary>
        ///编码，主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_id", MedName = "chk_id", WFName = "Pr_id", DBIdentity = true)]
        public Int64 ChkId { get; set; }

        /// <summary>
        ///病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_pat_id", MedName = "chk_pat_id", WFName = "Pr_pat_id")]
        public String ChkPatId { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_itm_id", MedName = "chk_itm_id", WFName = "Pr_Ditm_id")]
        public String ChkItmId { get; set; }

        /// <summary>
        ///检验结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_res_chr", MedName = "chk_res_chr", WFName = "Pr_res_chr")]
        public String ChkResChr { get; set; }

        /// <summary>
        ///报告状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_flag", MedName = "chk_flag", WFName = "Pr_flag")]
        public Int32 ChkFlag { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "chk_exp", MedName = "chk_exp", WFName = "Pr_exp")]
        public String ChkExp { get; set; }
    }
}
