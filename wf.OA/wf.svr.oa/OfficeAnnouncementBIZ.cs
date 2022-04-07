using System;
using System.Collections.Generic;
using dcl.servececontract;
using Lib.LogManager;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Configuration;
using dcl.svr.users;

namespace dcl.svr.oa
{
    public class OfficeAnnouncementBIZ : IOfficeAnnouncement
    {


        public EntityResponse GetAllUserBindingData()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EntityResponse response = new EntityResponse();
            try
            {
                IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
                string hosID = ConfigurationManager.AppSettings["HospitalID"];
                //获取用户
                List<EntitySysUser> allUser =new SysUserInfoBIZ().GetAllUsers(null);
                dict.Add("AllUser", allUser);
                //获取物理组
                List<EntitySysUser> listPowerUserInfo = new SysUserInfoBIZ().GetPowerUserInfo();
                dict.Add("PowerUserInfo", listPowerUserInfo);
                //获取角色
                List<EntitySysRole> listPowerRoleUser = new RoleManageProBIZ().GetPowerRoleUser();
                dict.Add("PowerRoleUser", listPowerRoleUser);
                //获取科室
                List<EntityDicPubDept> listPowerUserDepart = dao.GetPowerUserDepart(hosID);
                dict.Add("PowerUserDepart", listPowerUserDepart);

                response.SetResult(dict);
                return response;
            }
            catch (Exception ex)
            {
                Logger.LogException("科室公告 GetAllUserBindingData", ex);
            }
            return response;
        }

        public List<EntityOaAnnouncement> GetAnnouncementData(string userInfoId, string subject, string publisherName, DateTime dateFrom, DateTime dateTo)
        {
            List<EntityOaAnnouncement> list = new List<EntityOaAnnouncement>();
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return list = dao.GetAnnouncementData(userInfoId, subject, publisherName, dateFrom, dateTo);
            }
            else
            {
                return new List<EntityOaAnnouncement>();
            }
        }

        public List<EntityOaAnnouncement> GetSingleAnnouncementData(string userInfoId, int annID)
        {
            List<EntityOaAnnouncement> list = new List<EntityOaAnnouncement>();
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return list = dao.GetSingleAnnouncementData(userInfoId, annID);
            }
            else
            {
                return new List<EntityOaAnnouncement>();
            }
        }

        public int SaveAnnouncementData(EntityOaAnnouncement entityAnnouncement, List<string> reveiverList)
        {
            int data = -1;
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {

                data = dao.SaveAnnouncementData(entityAnnouncement, reveiverList);
                AnnuncemenCache.Current.Refresh();

            }
            return data;
        }

        public int DeleteAnnouncement(List<EntityOaAnnouncement> entity)
        {
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return (dao.DeleteAnnouncement(entity));
            }
            else return -1;
        }

        public List<EntityOaAnnouncement> GetUnReadAnnouncement(string userInfoId)
        {
            List<EntityOaAnnouncement> list = new List<EntityOaAnnouncement>();
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return list = dao.GetUnReadAnnouncement(userInfoId);
            }
            else
            {
                return list;
            }

        }

        public List<EntityOaAnnouncement> GetLastUnReadAnnouncement(string userInfoId)
        {
            List<EntityOaAnnouncement> list = new List<EntityOaAnnouncement>();
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return list = dao.GetLastUnReadAnnouncement(userInfoId);
            }
            else
            {
                return list;
            }
        }

    


        public int[] GetUnReadAnnouncementCount(string userInfoId)
        {
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            List<EntityOaAnnouncementReceive> listReceive = AnnuncemenCache.Current.DclCache;
            if (dao != null)
            {
                return dao.GetUnReadAnnouncementCount(listReceive, userInfoId);
            }
            else
            {
                return new int[2];
            }
        }

        public bool IsNeedShowAnnouncement(string userInfoId, int minutes)
        {
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return (dao.IsNeedShowAnnouncement(userInfoId, minutes));
            }
            else
            {
                return false;
            }
        }

        public void SetReadFlag(string userInfoId, int annID)
        {
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                dao.SetReadFlag(userInfoId, annID);
                AnnuncemenCache.Current.Refresh();
            }
        }

        public string IsIISAvailable()
        {
            throw new NotImplementedException();
        }

  

        public bool IsNeedShowHo(string ctypeID)
        {
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return (dao.IsNeedShowHo(ctypeID));
            }
            else
            {
                return false;
            }
        }

        public List<EntityOaAnnouncementReceive> GetAnnouncementCache()
        {
            List<EntityOaAnnouncementReceive> list = new List<EntityOaAnnouncementReceive>();
            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao != null)
            {
                return (dao.GetAnnouncementCache());
            }
            return list;

        }
    }
}
