using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 表单内容
    /// 旧表名:oa_table_detail 新表名:Oa_detail
    /// </summary>
    [Serializable]
    public class EntityOaTableDetail: EntityBase
    {

        /// <summary>
        ///单证代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderCode", MedName = "det_id", WFName = "Oadet_id")]
        public String DetId { get; set; }

        /// <summary>
        ///表单类型代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderTypeCode", MedName = "tab_code", WFName = "Oadet_Dot_code")]
        public String TabCode { get; set; }

        /// <summary>
        ///最后修改时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderDate", MedName = "det_date", WFName = "Oadet_date")]
        public DateTime DetDate { get; set; }

        #region 用于拼sql语句
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        #endregion

        /// <summary>
        ///最后修改人
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderUser", MedName = "det_user_id", WFName = "Oadet_Buser_id")]
        public String DetUserId { get; set; }

        /// <summary>
        ///表单内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderDetail", MedName = "det_content", WFName = "Oadet_content")]
        public String DetContent { get; set; }

        /// <summary>
        ///预留字段,业务关联用
        /// </summary>   
        [FieldMapAttribute(ClabName = "S1", MedName = "det_char_a", WFName = "Oadet_char_a")]
        public String DetCharA { get; set; }

        /// <summary>
        ///预留字段,业务关联用
        /// </summary>   
        [FieldMapAttribute(ClabName = "S2", MedName = "det_char_b", WFName = "Oadet_char_b")]
        public String DetCharB { get; set; }

        /// <summary>
        ///预留字段,业务关联用
        /// </summary>   
        [FieldMapAttribute(ClabName = "S3", MedName = "det_char_c", WFName = "Oadet_char_c")]
        public String DetCharC { get; set; }

        /// <summary>
        ///预留字段,业务关联用
        /// </summary>   
        [FieldMapAttribute(ClabName = "S4", MedName = "det_char_d", WFName = "Oadet_char_d")]
        public String DetCharD { get; set; }

        /// <summary>
        ///预留字段,保留单证中可能会被引用的表单字段内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "S5", MedName = "det_char_e", WFName = "Oadet_char_e")]
        public String DetCharE { get; set; }

        /// <summary>
        ///预留字段,保留单证中可能会被引用的表单字段内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "S6", MedName = "det_char_f", WFName = "Oadet_char_f")]
        public String DetCharF { get; set; }

        /// <summary>
        ///预留字段,保留单证中可能会被引用的表单字段内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "S7", MedName = "det_char_g", WFName = "Oadet_char_g")]
        public String DetCharG { get; set; }

        /// <summary>
        ///预留字段,保留单证中可能会被引用的表单字段内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "S8", MedName = "det_char_h", WFName = "Oadet_char_h")]
        public String DetCharH { get; set; }


    }
}
