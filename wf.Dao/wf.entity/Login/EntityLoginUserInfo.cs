using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统登录权限信息
    /// </summary>
    [Serializable]
    public class EntityLoginUserInfo : EntityBase
    {
       /// <summary>
       /// 用户登录信息
       /// </summary>
        public List<EntitySysUser> UserInfo { get; set; }

        /// <summary>
        /// 用户实验组信息
        /// </summary>
        public List<EntityUserLab> UserLabInfo { get; set; }

        /// <summary>
        ///科室信息
        /// </summary>   
        public List<EntitySysUser> Depart { get; set; }

        /// <summary>
        /// 用户科室信息
        /// </summary>
        public List<EntityUserDept> UserDepart { get; set; }

        /// <summary>
        ///系统配置
        /// </summary>   
        public List<EntitySysParameter> SysConfig { get; set; }

        /// <summary>
        ///用户配置
        /// </summary>   
        public List<EntitySysParameter> UserConfig { get; set; }

        /// <summary>
        ///用户功能
        /// </summary>   
        public List<EntitySysFunction> Func { get; set; }

        /// <summary>
        ///全部功能
        /// </summary>   
        public List<EntitySysFunction> AllFunc { get; set; }

        /// <summary>
        ///用户仪器
        /// </summary>   
        public List<EntityUserInstrmt> UserItrs { get; set; }

        /// <summary>
        ///用户质控仪器
        /// </summary>   
        public List<EntityUserItrQuality> UserItrsQc { get; set; }

        /// <summary>
        ///用户质控实验组
        /// </summary>   
        public List<EntityUserLabQuality> UserQcLab { get; set; }

        /// <summary>
        ///用户权限码
        /// </summary>   
        public List<EntityUserKey> PowerUserKey { get; set; }

        /// <summary>
        ///用户角色关系
        /// </summary>   
        public List<EntityUserRole> PowerUserRole { get; set; }

        /// <summary>
        /// 用户登录错误信息
        /// </summary>
        public EntityLoginErrorInfo ErrorInfo { get; set; }
    }
}
