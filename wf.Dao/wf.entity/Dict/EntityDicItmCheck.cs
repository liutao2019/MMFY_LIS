using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 校验字典组信息(主表)
    /// 旧表名:dic_itm_check 新表名:Dict_itm_verifi
    /// </summary>
    [Serializable]
    public class EntityDicItmCheck : EntityBase
    {
        /// <summary>
        /// 编码
        /// </summary>
        [FieldMapAttribute(ClabName = "group_id", MedName = "check_id", WFName = "Dverifi_id")]
        public String CheckId { get; set; }

        /// <summary>
        /// 仪器编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_id", MedName = "check_itr_id", WFName = "Dverifi_Ditr_id")]
        public String CheckItrId { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "group_name", MedName = "check_name", WFName = "Dverifi_name")]
        public String CheckName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>  
        [FieldMapAttribute(ClabName = "group_seq", MedName = "seq_no", WFName = "seq_no")]
        public Int32 SeqNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "group_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }

        /// <summary>
        ///  备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "group_remark", MedName = "check_remark", WFName = "Dverifi_remark")]
        public String CheckRemark { get; set; }

        #region 附加字段
        /// <summary>
        /// 显示编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "display_group_id", MedName = "display_group_id", WFName = "display_group_id", DBColumn = false)]
        public String DisplayGroupId { get; set; }

        /// <summary>
        /// (明细列表)
        /// </summary>
        public List<EntityDicItmCheckDetail> DetailList { get; set; }
        #endregion
    }
}
