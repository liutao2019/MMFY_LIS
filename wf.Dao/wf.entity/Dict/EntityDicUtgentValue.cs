using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目危急值字典
    /// </summary>
    [Serializable]
    public class EntityDicUtgentValue : EntityBase
    {
        public int? AgeHInt
        {
            get
            {
                return GetAgeInt(this.UgtAgeH, this.UgtAgeHunit);
            }
        }

        public int? AgeLInt
        {
            get
            {
                return GetAgeInt(this.UgtAgeL, this.UgtAgeLunit);
            }
        }

        private int? GetAgeInt(string age, string ageUnit)
        {
            int _age;
            if (int.TryParse(age, out _age))
            {
                if (ageUnit == "岁")
                {
                    return _age * 365 * 24 * 60;
                }
                else if (ageUnit == "月")
                {
                    return _age * 30 * 24 * 60;
                }
                else if (ageUnit == "天")
                {
                    return _age * 24 * 60;
                }
                else if (ageUnit == "时")
                {
                    return _age * 60;
                }
                else
                {
                    return _age * 365 * 24 * 60;
                }
            }
            else
            {
                return null;
            }
        }

        public EntityDicUtgentValue()
        {
            this.UgtKey = -1;
            this.UgtAgeHunit = "岁";
            this.UgtAgeLunit = "岁";
            this.UgtSamId = "-1";
            this.UgtDepCode = "-1";
        }

        /// <summary>
        /// 主键
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_key", MedName = "ugt_key", WFName = "Diuv_id")]
        public System.Int32 UgtKey { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_itm_id", MedName = "ugt_itm_id", WFName = "Diuv_Ditm_id")]
        public System.String UgtItmId { get; set; }
        
        

        /// <summary>
        /// 样本号
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_sam_id", MedName = "ugt_sam_id", WFName = "Diuv_Dsam_id")]
        public System.String UgtSamId { get; set; }

        
        /// <summary>
        /// 科室
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_dep_code", MedName = "ugt_dep_code", WFName = "Diuv_Ddept_code")]
        public System.String UgtDepCode { get; set; }

       

        /// <summary>
        /// 性别
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_sex", MedName = "ugt_sex", WFName = "Diuv_sex")]
        public System.String UgtSex { get; set; }

        /// <summary>
        /// 年龄上限单位
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_age_h", MedName = "ugt_age_h", WFName = "Diuv_age_h")]
        public System.String UgtAgeH { get; set; }

        /// <summary>
        /// 年龄上限单位
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_age_hunit", MedName = "ugt_age_hunit", WFName = "Diuv_age_hunit")]
        public System.String UgtAgeHunit { get; set; }


        /// <summary>
        /// 年龄下限
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_age_l", MedName = "ugt_age_l", WFName = "Diuv_age_l")]
        public System.String UgtAgeL { get; set; }

        /// <summary>
        /// 年龄下限单位
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_age_lunit", MedName = "ugt_age_lunit", WFName = "Diuv_age_lunit")]
        public System.String UgtAgeLunit { get; set; }

        /// <summary>
        /// 极大阈值
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_pan_h", MedName = "ugt_pan_h", WFName = "Diuv_pan_h")]
        public System.String UgtPanH { get; set; }

        /// <summary>
        /// 极小阈值
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_pan_l", MedName = "ugt_pan_l", WFName = "Diuv_pan_l")]
        public System.String UgtPanL { get; set; }

        /// <summary>
        /// 危急值上限
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_max", MedName = "ugt_max", WFName = "Diuv_max")]
        public System.String UgtMax { get; set; }

        /// <summary>
        /// 危急值下限
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_min", MedName = "ugt_min", WFName = "Diuv_min")]
        public System.String UgtMin { get; set; }

        /// <summary>
        /// 极大扩展值
        /// </summary>
        [FieldMapAttribute(ClabName = "ext_ugt_max", MedName = "ext_ugt_max", WFName = "Diuv_ext_max")]
        public System.String ExtUgtMax { get; set; }
        

        /// <summary>
        /// 极小扩展值
        /// </summary>
        [FieldMapAttribute(ClabName = "ext_ugt_min", MedName = "ext_ugt_min", WFName = "Diuv_ext_min")]
        public System.String ExtUgtMin { get; set; }
        /// <summary>
        /// 临床诊断
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_icd_name", MedName = "ugt_icd_name", WFName = "Diuv_icd_name")]
        public System.String UgtIcdName { get; set; }

        /// <summary>
        /// 危急值结果
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_desc", MedName = "ugt_desc", WFName = "Diuv_desc")]
        public System.String UgtDesc { get; set; }

        #region 附加字段

        /// <summary>
        /// 项目代码
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_itm_ecd", MedName = "ugt_itm_ecd", WFName = "ugt_itm_ecd", DBColumn = false )]
        public System.String UgtItmEcd { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_sam_name", MedName = "ugt_sam_name", WFName = "ugt_sam_name", DBColumn = false)]
        public System.String UgtSamName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [FieldMapAttribute(ClabName = "ugt_dep_name", MedName = "ugt_dep_name", WFName = "ugt_dep_name", DBColumn = false)]
        public System.String UgtDepName { get; set; }

        #endregion
    }
}
