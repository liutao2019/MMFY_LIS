using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器质控类实体表
    /// 旧表名:qc_rule_mes 新表名:Dict_qc_rule_mes 
    /// </summary>
    [Serializable]
    public class EntityDicQcRuleMes : EntityBase
    {
        /// <summary>
        ///主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_id", MedName = "qrm_id", WFName = "Dqrm_id")]
        public Int32 QrmId { get; set; }

        /// <summary>
        ///质控项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_item_id", MedName = "qrm_item_id", WFName = "Dqrm_Ditm_id")]
        public String QrmItemId { get; set; }

        /// <summary>
        ///(暂时不知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_node_id", MedName = "qrm_node_id", WFName = "Dqrm_node_id")]
        public Int32 QrmNodeId { get; set; }

        /// <summary>
        ///(暂时不知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_rootNode_id", MedName = "qrm_rootNode_id", WFName = "Dqrm_rootNode_id")]
        public Int32 QrmRootNodeId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_start_time", MedName = "qrm_start_time", WFName = "Dqrm_start_time")]
        public DateTime QrmStartTime { get; set; }

        /// <summary>
        ///结束日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrm_end_time", MedName = "qrm_end_time", WFName = "Dqrm_end_time")]
        public DateTime QrmEndTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type", MedName = "qcm_type", WFName = "Dqrm_type")]
        public String QcmType { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itr_id", MedName = "qcm_itr_id", WFName = "Dqrm_Ditr_id")]
        public String QcmItrId { get; set; }

        #region 附加字段 项目代码
        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "itm_ecode", DBColumn =false)]
        public String ItmEcode { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段 组别名称
        /// <summary>
        ///组别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 实验组ID
        /// <summary>
        ///实验组ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_type", MedName = "itr_lab_id", WFName = "Ditr_lab_id", DBColumn = false)]
        public String ItrLabId { get; set; }
        #endregion

        #region 附加字段 保养内容
        /// <summary>
        /// 保养内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_content", MedName = "mai_content", WFName = "Dim_content", DBColumn = false)]
        public String MaiContent { get; set; }
        #endregion
    }
}
