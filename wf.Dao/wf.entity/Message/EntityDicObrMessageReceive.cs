using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 危急值处理表
    /// 旧表名:obr_message_receive 新表名:Lis_message_receive
    /// </summary>
    [Serializable]
    public class EntityDicObrMessageReceive : EntityBase
    {
        /// <summary>
        /// 信息ID（关联危急值信息表）
        /// </summary>
        [FieldMapAttribute(ClabName = "receiver_msg_id", MedName = "obr_id", WFName = "Lmsgrec_Lmsg_id")]
        public String ObrId { get; set; }

        /// <summary>
        /// 危急值处理ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_id", MedName = "obr_user_id", WFName = "Lmsgrec_user_id")]
        public String ObrUserId { get; set; }

        /// <summary>
        /// 危急值类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_type", MedName = "obr_type", WFName = "Lmsgrec_type")]
        public Int32 ObrType { get; set; }

        /// <summary>
        /// 科室
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_name", MedName = "obr_user_name", WFName = "Lmsgrec_user_name")]
        public String ObrUserName { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_read_time", MedName = "obr_read_time", WFName = "Lmsgrec_read_time")]
        public DateTime? ObrReadTime { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_del_flag", MedName = "del_flag", WFName = "del_flag")]
        public Boolean DelFlag { get; set; }

        #region 附加字段 危急值消息实体
        public EntityDicObrMessageContent ObrMessageContent { get; set; }
        #endregion

        #region 附加字段 逻辑删除标志(true: 是；false：否)
        public Boolean LogicalDelete { get; set; }
        #endregion

    }
}
