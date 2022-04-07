using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityUserQc : EntityBase
    {
        public EntityUserQc()
        {
            NotEqual = false;
        }
        /// <summary>
        /// 登录ID
        /// </summary>
        public String LoginId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public String RoleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public String ModuleName { get; set; }

        /// <summary>
        /// 验证ID
        /// </summary>
        public String UserCerId { get; set; }

        /// <summary>
        /// 验证医院
        /// </summary>
        public String UserCaMode { get; set; }

        /// <summary>
        /// 验证密码
        /// </summary>
        public String CaPasswordMode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public String FuncId { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public String FuncCode { get; set; }

        /// <summary>
        /// 不等于
        /// </summary>
        public bool NotEqual { get; set; }
    }
}
