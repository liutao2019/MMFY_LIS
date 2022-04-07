using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 科室公告相关表
    /// 旧表名:Oa_announcement 新表名:Oa_announcement
    /// </summary>
    [Serializable]
    public class EntityOaAnnouncement: EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_Id", MedName = "anct_id", WFName = "Oanct_id")]
        public Int32 AnctId { get; set; }

        /// <summary>
        ///公告标题
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_Subject", MedName = "anct_title", WFName = "Oanct_title")]
        public String AnctTitle { get; set; }

        /// <summary>
        ///公告内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_Body", MedName = "anct_content", WFName = "Oanct_content")]
        public String AnctContent { get; set; }

        /// <summary>
        ///发布人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_PublisherId", MedName = "anct_publish_user_id", WFName = "Oanct_publish_user_id")]
        public String AnctPublishUserId { get; set; }

        /// <summary>
        ///发布人
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_PublisherName", MedName = "anct_publish_user_name", WFName = "Oanct_publish_user_name")]
        public String AnctPublishUserName { get; set; }

        /// <summary>
        ///发布日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_PublishDate", MedName = "anct_publish_date", WFName = "Oanct_publish_date")]
        public DateTime AnctPublishDate { get; set; }

        /// <summary>
        ///接收人
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_ReceiverNames", MedName = "anct_reciver_name", WFName = "Oanct_reciver_name")]
        public String AnctReciverName { get; set; }

        /// <summary>
        ///公告类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_Type", MedName = "anct_type", WFName = "Oanct_type")]
        public String AnctType { get; set; }

        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "ann_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }

        #region 附加字段 收取状态
        /// <summary>
        ///收取状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "annGoroup", MedName = "annGoroup", WFName = "annGoroup", DBColumn = false)]
        public string AnnGoroup { get; set; }
        #endregion


        #region 附加字段 收取人id
        /// <summary>
        ///收取人id
        /// </summary>   
        [FieldMapAttribute(ClabName = "ar_ReceiverId", MedName = "ar_ReceiverId", WFName = "ar_ReceiverId", DBColumn = false)]
        public string ArReceiverId { get; set; }
        #endregion

        #region 附加字段 是否被选中
        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "isselected", MedName = "isselected", WFName = "isselected", DBColumn = false)]
        public Int32 IsSelected { get; set; }
        #endregion

        #region 附加字段  状态
        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "ReadFlag", MedName = "ReadFlag", WFName = "ReadFlag", DBColumn = false)]
        public String  ReadFlag { get; set; }
        #endregion
    }
}
