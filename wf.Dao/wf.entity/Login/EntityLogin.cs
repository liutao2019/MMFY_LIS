using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统登录
    /// </summary>
    [Serializable]
    public class EntityLogin : EntityBase
    {
        /// <summary>
        ///登录ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "loginID", MedName = "loginID", WFName = "loginID")]
        public string LogInID { get; set; }

        /// <summary>
        ///密码
        /// </summary>   
        [FieldMapAttribute(ClabName = "password", MedName = "password", WFName = "password")]
        public String PassWord { get; set; }

        /// <summary>
        ///ip
        /// </summary>   
        [FieldMapAttribute(ClabName = "ip", MedName = "ip", WFName = "ip")]
        public String IP { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mac", MedName = "mac", WFName = "mac")]
        public String Mac { get; set; }

        /// <summary>
        ///状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "action", MedName = "action", WFName = "action")]
        public String Action { get; set; }

        /// <summary>
        ///CA登录码
        /// </summary>   
        [FieldMapAttribute(ClabName = "CASignMode", MedName = "CASignMode", WFName = "CASignMode")]
        public string CASignMode { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "Thumbprint", MedName = "Thumbprint", WFName = "Thumbprint")]
        public String Thumbprint { get; set; }
        /// <summary>
        /// 是否使用电子验证
        /// </summary>
        public bool UserCaFlag { get; set; }

        /// <summary>
        /// CA唯一标识
        /// </summary>
        public string CAEntityId { get; set; }

    }
}
