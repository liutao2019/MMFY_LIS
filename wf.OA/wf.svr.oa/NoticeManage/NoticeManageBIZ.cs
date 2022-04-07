using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.oa
{
    public class NoticeManageBIZ: IDicNoticeManage
    {
        /// <summary>
        /// 查询管理信息数据/系统用户数据/用户角色数据/用户部门数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public EntityResponse SearchMessUserRoleDepart(EntitySysUser user)
        {
            EntityResponse result = new EntityResponse();

            IDaoNoticeManage dao = DclDaoFactory.DaoHandler<IDaoNoticeManage>();
            if (dao != null)
            {
                result = dao.SearchMessUserRoleDepart(user);
            }
            return result;
        }

        /// <summary>
        /// 保存通知管理数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SaveSysMessage(List<EntitySysMessage> message)
        {
            IDaoNoticeManage dao = DclDaoFactory.DaoHandler<IDaoNoticeManage>();
            bool isSaveTrue = false;
            if (dao != null)
            {
                isSaveTrue = dao.SaveSysMessage(message);
                AnnuncemenCache.Current.Refresh();//刷新缓存，实时显示多少条未读通知
            }
            return isSaveTrue;
        }

        /// <summary>
        /// 删除通知管理数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DeleteSysMessage(List<EntitySysMessage> message)
        {
            IDaoNoticeManage dao = DclDaoFactory.DaoHandler<IDaoNoticeManage>();
            bool isDeleteTrue = false;
            if (dao != null)
            {
                isDeleteTrue = dao.DeleteSysMessage(message);
            }
            return isDeleteTrue;
        }

        /// <summary>
        /// 更新后查询通知管理数据
        /// </summary>
        /// <param name="MessageId"></param>
        /// <returns></returns>
       public  List<EntitySysMessage> UpdateAndSearchSysMessage(int MessageId)
        {
            IDaoNoticeManage dao = DclDaoFactory.DaoHandler<IDaoNoticeManage>();
            List<EntitySysMessage> listMess = new List<EntitySysMessage>();
            if (dao != null)
            {
                listMess = dao.UpdateAndSearchSysMessage(MessageId);
                AnnuncemenCache.Current.Refresh();//刷新缓存，实时显示多少条未读通知
            }
            return listMess;
        }
    }
}
