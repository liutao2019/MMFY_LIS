using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统发声信息（电视机系统用）（bl_sys_message_speech）
    /// </summary>
    [Serializable]
    public class EntitySysMessageSpeech : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "sn", MedName = "sn", WFName = "sn")]
        public Int32 Sn { get; set; }

        /// <summary>
        ///发声内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "speech_text", MedName = "speech_text", WFName = "speech_text")]
        public String SpeechText { get; set; }

        /// <summary>
        ///发声状态（0未读，10已读）
        /// </summary>   
        [FieldMapAttribute(ClabName = "status", MedName = "status", WFName = "status")]
        public Int32 Status { get; set; }

        /// <summary>
        ///数据 (暂无使用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "data", MedName = "data", WFName = "data")]
        public String Data { get; set; }

        /// <summary>
        ///生成时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "create_date", MedName = "create_date", WFName = "create_date")]
        public DateTime CreateDate { get; set; }
      
    }
}
