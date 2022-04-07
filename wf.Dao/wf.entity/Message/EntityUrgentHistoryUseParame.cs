using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 获取危急值信息时,所使用的参数作为实体成员
    /// </summary>
    public class EntityUrgentHistoryUseParame
    {
        /// <summary>
        /// 科室ID(receiver_id)
        /// </summary>
        public String ReceiveID { get; set; }

        /// <summary>
        /// 忽略科室(或病区)(is_neglect_dep)
        /// </summary>
        public String IsNeglectDep { get; set; }

        /// <summary>
        /// 病人来源配置(pat_ori_config)
        /// </summary>
        public String PatOriConfig { get; set; }

        /// <summary>
        /// 医生代码(pat_doc_id)
        /// </summary>
        public String PatDocId { get; set; }

        /// <summary>
        /// 消息类型(msg_type)
        /// </summary>
        public String MsgType { get; set; }

        /// <summary>
        /// 创建时间开始(create_time_start)
        /// </summary>
        public String CreateTimeStart { get; set; }

        /// <summary>
        /// 创建时间结束（create_time_end）
        /// </summary>
        public String CreateTimeEnd { get; set; }

        /// <summary>
        /// 科室ID数（DepIDs）
        /// </summary>
        public String DepIDs { get; set; }

        /// <summary>
        /// 获取推迟多少分钟的危急值信息（filterTime）
        /// </summary>
        public String FilterTime { get; set; }

        /// <summary>
        /// 获取推迟多少分钟的急查信息（filterTime2）
        /// </summary>
        public String FilterTime2 { get; set; }

        /// <summary>
        /// 只接收自编危急信息 2024（isOnlyDIY）
        /// </summary>
        public String IsOnlyDIY { get; set; }
        
    }
}
