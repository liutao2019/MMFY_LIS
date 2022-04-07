using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 细菌无菌涂片字典
    /// 旧表名:Dic_mic_smear 新表名:Dict_mic_smear
    /// </summary>
    [Serializable]
    public class EntityDicMicSmear : EntityBase
    {
        public EntityDicMicSmear()
        {
            SmeSortNo = 0;
            SmeComFlag = 0;
            SmePositiveFlag = 0;
            SmeClass = 0;
            SmePublic = false;
        }
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_id", MedName = "sme_id",WFName = "Dsme_id")]
        public String SmeId { get; set; }

        /// <summary>
        ///涂片结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_cname", MedName = "sme_name", WFName = "Dsme_name")]
        public String SmeName { get; set; }

        /// <summary>
        ///组别
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_type", MedName = "sme_type", WFName = "Dsme_Dpro_id")]
        public String SmeType { get; set; }

        /// <summary>
        ///输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_incode", MedName = "c_code", WFName = "Dsme_c_code")]
        public String SmeCCode { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_prop", MedName = "sme_prop", WFName = "Dsme_prop")]
        public String SmeProp { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_aen", MedName = "sme_aen", WFName = "Dsme_aen")]
        public String SmeAen { get; set; }

        /// <summary>
        ///默认检查目的
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_chk_id", MedName = "purp_id", WFName = "Dsme_Dpurp_id")]
        public String SmePurpId { get; set; }

        /// <summary>
        ///标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_com_flag", MedName = "sme_com_flag", WFName = "Dsme_com_flag")]
        public Int32 SmeComFlag { get; set; }

        /// <summary>
        ///阳性标志 0-正常 1-阳性
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_reg_flag", MedName = "sme_positive_flag", WFName = "Dsme_positive_flag")]
        public Int32 SmePositiveFlag { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SmeSortNo { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_py", MedName = "py_code", WFName = "py_code")]
        public String SmePyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_wb", MedName = "wb_code", WFName = "wb_code")]
        public String SmeWbCode { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_del", MedName = "del_flag", WFName = "del_flag")]
        public String SmeDelFlag { get; set; }

        /// <summary>
        ///类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_class", MedName = "sme_class", WFName = "Dsme_class")]
        public Int32 SmeClass { get; set; }

        /// <summary>
        ///是否共用
        /// </summary>   
        [FieldMapAttribute(ClabName = "nob_pulbic", MedName = "sme_public", WFName = "Dsme_public")]
        public Boolean SmePublic { get; set; }

        #region 附加码 组别
        /// <summary>
        /// 组别
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String SmeProName { get; set; }
        #endregion

        #region 附加码 检查目的
        [FieldMapAttribute(ClabName = "chk_cname", MedName = "purp_name", WFName = "Dpurp_name", DBColumn = false)]
        public String SmePurpName { get; set; }
        #endregion

        public int isselected { get; set; }
    }
}
