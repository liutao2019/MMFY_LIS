using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysRole
    {
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool DeleteRoleInfo(EntitySysRole func);

        /// <summary>
        /// 新增一个角色
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool InsertRoleInfo(EntitySysRole func);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>

        bool UpdateRoleInfo(EntitySysRole func);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        List<EntitySysRole> GetAllInfo();

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        List<EntitySysRole> GetPowerRoleUser();
        
    }
}
