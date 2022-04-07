using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户权限实体集合
    /// </summary>
    [Serializable]
    public class EntityPowerList
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<EntitySysRole> SysRole { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<EntitySysUser> SysUser { get; set; }
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<EntityDicPubOrganize> DicType { get; set; }
        /// <summary>
        /// 科室列表
        /// </summary>
        public List<EntityDicPubDept> DicDept { get; set; }
        /// <summary>
        /// 用户与权限的关系列表
        /// </summary>
        public List<EntityUserRole> UserRole { get; set; }
        /// <summary>
        /// 用户与科室的关系列表
        /// </summary>
        public List<EntityUserDept> UserDept { get; set; }
        /// <summary>
        /// 用户与医院的关系列表
        /// </summary>
        public List<EntityUserHospital> UserHospital { get; set; }
        /// <summary>
        /// 用户与医院质控的关系列表
        /// </summary>
        public List<EntityUserHosQuality> UserHosQuality { get; set; }
        /// <summary>
        /// 用户与仪器的关系列表
        /// </summary>
        public List<EntityUserInstrmt> UserItr { get; set; }
        /// <summary>
        /// 用户与仪器质控的关系列表
        /// </summary>
        public List<EntityUserItrQuality> UserItrQuality { get; set; }
        /// <summary>
        /// 用户与实验室的关系列表
        /// </summary>
        public List<EntityUserLab> UserLab { get; set; }
        /// <summary>
        /// 用户与实验室质控的关系列表
        /// </summary>
        public List<EntityUserLabQuality> UserLabQuality { get; set; }
    }
}
