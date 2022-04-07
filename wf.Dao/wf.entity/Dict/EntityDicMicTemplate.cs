using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 模板信息
    /// 旧表名:Dic_mic_template 新表名:Dict_mic_template
    /// </summary>
    [Serializable]
    public class EntityDicMicTemplate
    {
        public EntityDicMicTemplate()
        {
            PatSelect = false;
        }

        /// <summary>
        ///主键id无特殊意义。程序触发生成的自增型主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_id", MedName = "tmp_id", WFName = "Dtmp_id")]
        public String TmpId { get; set; }

        /// <summary>
        ///模板类型：0=预报告描述模板；
        ///1=报告描述模板；
        /// 2=危急值模板；
        /// 3=培养条件；
        /// 4=标本说明； 
        /// 5=染色方法；
        /// 6=镜下形态；
        /// 7=图片项目；
        /// 8=图片结果；
        /// 9=球杆比值；
        /// 10=标本性装；
        /// 11=上皮/白细胞；
        /// 12=实验室评语；
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_type", MedName = "tmp_type", WFName = "Dtmp_type")]
        public Int32 TmpType { get; set; }

        /// <summary>
        ///模板内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_content", MedName = "tmp_content", WFName = "Dtmp_content")]
        public String TmpContent { get; set; }

        /// <summary>
        ///排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "sort_no", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }

        /// <summary>
        ///阴性标志：0-阴性  1-阳性
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_negative_flag", MedName = "tmp_negative_flag", WFName = "Dtmp_negative_flag")]
        public Int32 TmpNegativeFlag { get; set; }

        /// <summary>
        ///阴性标志内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_remark", MedName = "tmp_remark", WFName = "Dtmp_remark")]
        public String TmpRemark { get; set; }

        /// <summary>
        ///检验组合 关联 Dic_itm_combine.com_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "tmp_com_id", MedName = "tmp_com_id", WFName = "Dtmp_Dcom_id")]
        public String TmpComId { get; set; }

        #region 附加字段 是否选中

        /// <summary>
        ///是否选中
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_select", MedName = "pat_select", WFName = "pat_select", DBColumn = false)]
        public Boolean PatSelect { get; set; }

        #endregion 附加字段 是否选中
    }
}
