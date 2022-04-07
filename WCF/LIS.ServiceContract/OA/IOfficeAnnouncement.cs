using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.ServiceModel;

using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IOfficeAnnouncement
    {
        /// <summary>
        /// 获取所有用户相关绑定数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetAllUserBindingData();

        /// <summary>
        /// 获取公告数据
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="subject"></param>
        /// <param name="publisherName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaAnnouncement> GetAnnouncementData(string userInfoId, string subject, string publisherName, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// 获取某个人某几条公告数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaAnnouncement> GetSingleAnnouncementData(string userInfoId, int annID);

        /// <summary>
        /// 发送保存公告
        /// </summary>
        /// <param name="entityAnnouncement"></param>
        /// <param name="reveiverList"></param>
        /// <returns></returns>
        [OperationContract]
        int SaveAnnouncementData(EntityOaAnnouncement entityAnnouncement, List<string> reveiverList);

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int DeleteAnnouncement(List<EntityOaAnnouncement> entity);

        /// <summary>
        /// 获取未读数据数量
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        [OperationContract]
        int[] GetUnReadAnnouncementCount(string userInfoId);

        /// <summary>
        /// 获取未读
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaAnnouncement> GetUnReadAnnouncement(string userInfoId);

        /// <summary>
        /// 获取最新一条未读公告
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaAnnouncement> GetLastUnReadAnnouncement(string userInfoId);

        /// <summary>
        /// 获取公告缓存
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaAnnouncementReceive> GetAnnouncementCache();
        /// <summary>
        /// 是否需要登录时弹出窗体
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="minutes">分钟</param>
        /// <returns></returns>
        [OperationContract]
        bool IsNeedShowAnnouncement(string userInfoId, int minutes);

        /// <summary>
        /// 更新已读状态
        /// </summary>
        /// <param name="userInfoId"></param>
        /// <param name="annID"></param>
        [OperationContract]
        void SetReadFlag(string userInfoId, int annID);

        /// <summary>
        /// iis是否连通可用
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string IsIISAvailable();


        /// <summary>
        /// 是否关闭提示
        /// </summary>
        /// <param name="ctypeID"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsNeedShowHo(string ctypeID);

    }
}
