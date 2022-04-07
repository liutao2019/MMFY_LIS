using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 管理通知：sysmessage 
    /// </summary>
    [Serializable]
    public class EntitySysMessage : EntityBase
    {
        /// <summary>
        /// 消息ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageId", MedName = "MessageId", WFName = "MessageId")]
        public Int32 MessageId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageType", MedName = "MessageType", WFName = "MessageType")]
        public String MessageType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageTitle", MedName = "MessageTitle", WFName = "MessageTitle")]
        public String MessageTitle { get; set; }

        /// <summary>
        /// 消息正文
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageContent", MedName = "MessageContent", WFName = "MessageContent")]
        public String MessageContent { get; set; }

        /// <summary>
        /// 信息来源
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageOwer", MedName = "MessageOwer", WFName = "MessageOwer")]
        public String MessageOwer { get; set; }

        /// <summary>
        /// 信息来源类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageOwerType", MedName = "MessageOwerType", WFName = "MessageOwerType")]
        //public String MessageOwerType { get; set; }
        public Int32 MessageOwerType { get; set; } //数据表中是varchar类型，但是为了跟MessageId一样才改的，目的是让TreeList可以按其实现分组

        /// <summary>
        /// 发件人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageFromId", MedName = "MessageFromId", WFName = "MessageFromId")]
        public String MessageFromId { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageFrom", MedName = "MessageFrom", WFName = "MessageFrom")]
        public String MessageFrom { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>   
        [FieldMapAttribute(ClabName = "MessageTo", MedName = "MessageTo", WFName = "MessageTo")]
        public String MessageTo { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "CreateDate", MedName = "CreateDate", WFName = "CreateDate")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 已读
        /// </summary>   
        [FieldMapAttribute(ClabName = "ReadDate", MedName = "ReadDate", WFName = "ReadDate")]
        public DateTime? ReadDate { get; set; }

        #region 附加字段 是否已读 
        /// <summary>
        /// 是否已读 
        /// </summary>
        [FieldMapAttribute(ClabName = "ReadDateYN", MedName = "ReadDateYN", WFName = "ReadDateYN")]
        public String ReadDateYN { get; set; }
        #endregion

    }
}
