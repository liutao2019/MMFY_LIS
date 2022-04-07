using System;

namespace dcl.entity
{
    /// <summary>
    /// 标本类别
    /// 旧表名:Dic_sample_type 新表名:Dict_sample
    /// </summary>
    [Serializable]
    public class EntityDicSample : EntityBase
    {
        public EntityDicSample()
        {
            SamSortNo = 0;
            Checked = false;
        }

        /// <summary>
        /// 编码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_id", MedName = "sam_id", WFName = "Dsam_id")]
        public string SamId { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name")]
        public string SamName { get; set; }

        /// <summary>
        /// 标本代码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_code", MedName = "sam_code", WFName = "Dsam_code")]
        public string SamCode { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_incode", MedName = "c_code", WFName = "Dsam_c_code")]
        public string SamCCode { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_type", MedName = "pro_id", WFName = "Dsam_Dpro_id")]
        public string SamProId { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_py", MedName = "py_code", WFName = "py_code")]
        public string SamPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_wb", MedName = "wb_code", WFName = "wb_code")]
        public string SamWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int SamSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_del", MedName = "del_flag", WFName = "del_flag")]
        public string SamDelFlag { get; set; }

        /// <summary>
        /// 自定义类别(标本组)
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_custom_type", MedName = "sam_custom_type", WFName = "Dsam_custom_type")]
        public string SamCustomType { get; set; }

        /// <summary>
        /// 平板类型(whonet编码)
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_trans_code", MedName = "sam_trans_code", WFName = "Dsam_trans_code")]
        public string SamTransType { get; set; }

        /// <summary>
        /// 0-无菌体液  1-有菌体液(微生物)
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_bac_flag", MedName = "sam_bac_flag", WFName = "Dsam_bac_flag")]
        public string SamBacFlag { get; set; }

        #region 附加字段

        /// <summary>
        /// 物理组
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public string TypeName { get; set; }

        #endregion 附加字段

        #region 附加字段 是否选中

        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }

        #endregion 附加字段 是否选中

        #region 附加字段 统一ID

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return SamId;
            }
        }

        #endregion 附加字段 统一ID

        /// <summary>
        /// 体液显示
        /// </summary>
        public String SamBacFlagText
        {
            get
            {
                if (!string.IsNullOrEmpty(SamBacFlag))
                {
                    if (SamBacFlag == "0")
                    {
                        return "无菌体液";
                    }
                    else
                    {
                        return "有菌体液";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 标本组显示
        /// </summary>
        public String SamCustomTypeText
        {
            get
            {
                if (!string.IsNullOrEmpty(SamCustomType))
                {
                    if (SamCustomType == "0")
                    {
                        return "BLD";
                    }
                    else if (SamCustomType == "1")
                    {
                        return "MISC";
                    }
                    else if (SamCustomType == "2")
                    {
                        return "UR";
                    }
                    else if (SamCustomType == "3")
                    {
                        return "RES";
                    }
                    else if (SamCustomType == "4")
                    {
                        return "CSF";
                    }
                    else if (SamCustomType == "5")
                    {
                        return "SBF";
                    }
                    else if (SamCustomType == "6")
                    {
                        return "STL";
                    }
                    else
                    {
                        return "SV";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
    }
}