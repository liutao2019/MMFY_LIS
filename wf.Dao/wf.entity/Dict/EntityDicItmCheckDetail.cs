using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 校验字典信息(明细表)
    /// 旧表名:dic_itm_check_detail 新表名:Dict_itm_verifi_detail
    /// </summary>
    [Serializable]
    public class EntityDicItmCheckDetail : EntityBase
    {
        /// <summary>
        /// 编码（明细表）
        /// </summary>
        [FieldMapAttribute(ClabName = "ei_id", MedName = "det_id", WFName = "Divd_id")]
        public Int64? DetId { get; set; }

        /// <summary>
        /// 校验字典组ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_group_id", MedName = "check_id", WFName = "Divd_Dverifi_id")]
        public String CheckIdDetial { get; set; }

        /// <summary>
        /// 警告提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_name", MedName = "check_name", WFName = "Divd_name")]
        public String CheckNameDetail { get; set; }

        /// <summary>
        /// 类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_type", MedName = "check_type", WFName = "Divd_type")]
        public String CheckTypeDetail { get; set; }

        /// <summary>
        /// 校验公式
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_fmla", MedName = "check_expression", WFName = "Divd_expression")]
        public String CheckExpression { get; set; }

        /// <summary>
        /// 不可变的
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_displayvariable", MedName = "check_display_variable", WFName = "Divd_display_variable")]
        public String CheckDisplayVariable { get; set; }

        /// <summary>
        /// 可变的
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_variable", MedName = "check_variable", WFName = "Divd_variable")]
        public String CheckVariable { get; set; }

        /// <summary>
        ///  序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_seq", MedName = "seq_no", WFName = "seq_no")]
        public Int32 SeqNoDetail { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_itm_id", MedName = "check_itm_id", WFName = "Divd_Ditm_id")]
        public String CheckItmId { get; set; }

        /// <summary>
        /// 自动审核标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "ei_audit_flag", MedName = "check_audit_flag", WFName = "Divd_audit_flag")]
        public String CheckAuditFlag { get; set; }

        #region 附加字段 项目校验明细表
        /// <summary>
        ///  是否新增
        /// </summary>   
        [FieldMapAttribute(ClabName = "isNew", MedName = "check_audit_flag", WFName = "isNew", DBColumn = false)]
        public int IsNew { get; set; }

        /// <summary>
        ///  实验组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "check_audit_flag", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion
        #region 附加字段 仪器编码 
        /// <summary>
        ///  仪器编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_id", MedName = "check_itr_id", WFName = "Dverifi_Ditr_id", DBColumn = false)]
        public String CheckItrId { get; set; }
        #endregion

        #region 附加字段 项目代码
        /// <summary>
        ///  项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode", DBColumn = false)]
        public String ItmEcode { get; set; }
        #endregion

        #region 附加字段 项目名称
        /// <summary>
        ///  项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String _ItmName { get; set; }
        #endregion
    }
}
