using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Serializable]
    public class EntityLogLogin : EntityBase
    {
        public EntityLogLogin()
        {

        }

        public EntityLogLogin(string type, string module, string loginID, string ip, string mac, string message, DateTime time)
        {
            LogLoginID = loginID;
            LogIP = ip;
            LogMAC = mac;
            LogType = type;
            LogMessage = message;
            LogModule = module;
            LogTime = time;
        }
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "ID", MedName = "ID", WFName = "ID")]
        public Int32 LogID { get; set; }

        /// <summary>
        ///操作模块
        /// </summary>   
        [FieldMapAttribute(ClabName = "Module", MedName = "Module", WFName = "Module")]
        public String LogModule { get; set; }

        /// <summary>
        ///时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "Time", MedName = "Time", WFName = "Time")]
        public DateTime LogTime { get; set; }

        /// <summary>
        ///登录ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "LoginID", MedName = "LoginID", WFName = "LoginID")]
        public String LogLoginID { get; set; }

        /// <summary>
        ///IP地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "IP", MedName = "IP", WFName = "IP")]
        public String LogIP { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "MAC", MedName = "MAC", WFName = "MAC")]
        public String LogMAC { get; set; }

        /// <summary>
        ///操作类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "Type", MedName = "Type", WFName = "Type")]
        public String LogType { get; set; }

        /// <summary>
        ///详细
        /// </summary>   
        [FieldMapAttribute(ClabName = "Message", MedName = "Message", WFName = "Message")]
        public String LogMessage { get; set; }
    }
}
