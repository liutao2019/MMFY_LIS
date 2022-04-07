using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityRemoteCallClientInfo
    {
        /// <summary>
        /// 用户登录ID
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 当期操作的Ip地址
        /// </summary>
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string CryptedPassword { get; set; }

        /// <summary>
        /// 当前操作名称
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 操作人工号 (输入码)
        /// </summary>
        public string OperatorSftId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 物理组名称
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 是否使用审核规则
        /// </summary>
        public bool UseAuditRule { get; set; }

        /// <summary>
        /// 新生儿筛查
        /// </summary>
        public bool BabyFilterFlag { get; set; }

        /// <summary>
        /// 不发送危急值消息的pat_id
        /// </summary
        [System.ComponentModel.Description("不发送危急值消息的pat_id")]
        public List<string> UnSendCriticalMessagePatIDs { get; set; }

        public override string ToString()
        {
            return string.Format("user:{0} operation:{1}", LoginID, OperationName);
        }

        /// <summary>
        /// 调用时间
        /// </summary>
        public DateTime Time { get;  set; }

        public EntityRemoteCallClientInfo()
        {
            LoginID = string.Empty;
            IPAddress = string.Empty;
            MACAddress = string.Empty;
            CryptedPassword = string.Empty;
            OperationName = string.Empty;
            Time = DateTime.Now;
            UnSendCriticalMessagePatIDs = new List<string>();
            BabyFilterFlag = false;
        }

    }
}
