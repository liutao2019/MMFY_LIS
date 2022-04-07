using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoNoticeManage
    {
        /// <summary>
        /// 查询管理信息数据/系统用户数据/用户角色数据/用户部门数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        EntityResponse SearchMessUserRoleDepart(EntitySysUser user);

        /// <summary>
        /// 保存通知管理数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SaveSysMessage(List<EntitySysMessage> message);

        /// <summary>
        /// 删除通知管理数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool DeleteSysMessage(List<EntitySysMessage> message);

        /// <summary>
        /// 更新后查询通知管理数据
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
        List<EntitySysMessage> UpdateAndSearchSysMessage(int MessageId);
    }
}
