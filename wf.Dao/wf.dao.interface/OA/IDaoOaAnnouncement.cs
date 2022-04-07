using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaAnnouncement
    {
        /// <summary>
        /// 获取公告数据
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="subject"></param>
        /// <param name="publisherName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        List<EntityOaAnnouncement> GetAnnouncementData(string userInfoId, string subject, string publisherName, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// 获取某个人某几条公告数据
        /// </summary>
        /// <returns></returns>
        List<EntityOaAnnouncement> GetSingleAnnouncementData(string userInfoId, int annID);

        /// <summary>
        /// 发送保存公告
        /// </summary>
        /// <param name="entityAnnouncement"></param>
        /// <param name="reveiverList"></param>
        /// <returns></returns>
        int SaveAnnouncementData(EntityOaAnnouncement entityAnnouncement, List<string> reveiverList);


        /// <summary>
        /// 删除公告
        /// </summary>
        /// <returns></returns>
        int DeleteAnnouncement(List<EntityOaAnnouncement> entity);

        /// <summary>
        /// 获取未读数据数量
        /// </summary>
        /// <param name="listReceive">未读公告集合</param>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        int[] GetUnReadAnnouncementCount(List<EntityOaAnnouncementReceive> listReceive,string userInfoId);

        /// <summary>
        /// 获取未读
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        List<EntityOaAnnouncement> GetUnReadAnnouncement(string userInfoId);

        /// <summary>
        /// 获取最新一条未读公告
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        List<EntityOaAnnouncement> GetLastUnReadAnnouncement(string receiverId);

        /// <summary>
        /// 获得公告缓存
        /// </summary>
        /// <returns></returns>
        List<EntityOaAnnouncementReceive> GetAnnouncementCache();
        /// <summary>
        /// 是否需要登录时弹出窗体
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="minutes">分钟</param>
        /// <returns></returns>
        bool IsNeedShowAnnouncement(string userInfoId, int minutes);

        /// <summary>
        /// 更新已读状态
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="annID"></param>
        void SetReadFlag(string userInfoId, int annID);

        /// <summary>
        /// iis是否连通可用
        /// </summary>
        /// <returns></returns>
        string IsIISAvailable();






        bool IsNeedShowHo(string ctypeID);


        /// <summary>
        /// 获取科室权限
        /// </summary>
        /// <param name="hosID"></param>
        /// <returns></returns>
        List<EntityDicPubDept> GetPowerUserDepart(string hosID);


    }
}
