using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 科室公告接收表
    /// 旧表名:Oa_announcement_receive 新表名:Oa_announcement_receive
    /// </summary>
    [Serializable]
    public class EntityOaAnnouncementReceive: EntityBase
    {

        /// <summary>
        ///关联Announcement.ann_Id
        /// </summary>   
        [FieldMapAttribute(ClabName = "ar_ann_Id", MedName = "anct_id", WFName = "Oanctr_Oanct_id")]
        public Int32 AnctId { get; set; }

        /// <summary>
        ///接收id
        /// </summary>   
        [FieldMapAttribute(ClabName = "ar_ReceiverId", MedName = "receiver_user_id", WFName = "Oanctr_Buser_id")]
        public String ReceiverUserId { get; set; }

        /// <summary>
        ///接收时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "ar_ReadDate", MedName = "receiver_date", WFName = "Oanctr_date")]
        public DateTime ReceiverDate { get; set; }

        #region 附加字段 公告总数
        /// <summary>
        ///公告总数
        /// </summary>   
        [FieldMapAttribute(ClabName = "num", MedName = "num", WFName = "num")]
        public string ReceiverNum { get; set; }
        #endregion

        #region 附加字段 公告类型
        /// <summary>
        ///公告类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "messagetype", MedName = "messagetype", WFName = "messagetype")]
        public Int32 ReceiverType { get; set; }
        #endregion

    }
}
