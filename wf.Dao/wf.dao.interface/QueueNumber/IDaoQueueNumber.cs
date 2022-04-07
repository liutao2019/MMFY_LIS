using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoQueueNumber
    {
        /// <summary>
        /// 保存病人排队信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean SaveQueueInfo(EntityQueueNumber queue);

        /// <summary>
        /// 更新病人排队号
        /// </summary>
        /// <param name="queueNo"></param>
        /// <param name="queueDate"></param>
        /// <param name="pidInNo"></param>
        /// <returns></returns>
        Boolean UpdateQueueNo(string queueNo, DateTime queueDate, string pidInNo);
        /// <summary>
        /// 获取当天时间最大排队号
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Int32 GetMaxQueueNo();

        /// <summary>
        /// 获取排队信息
        /// </summary>
        /// <param name="dateSatrt">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="windowsName">窗口号</param>
        /// <param name="windowsArea">采血区域</param>
        /// <returns></returns>
        List<EntityQueueNumber> GetQueueNumber(string dateSatrt, string dateEnd, string windowsName, string windowsArea);


        /// <summary>
        /// 更新排队状态
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <param name="queueNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Boolean UpdateQueueStatus(string pidInNo,string queueNo, string status);


        /// <summary>
        /// 更新排队状态
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <param name="queueNo"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        Boolean UpdateQueueWindow(string pidInNo, string queueNo, string window);

        /// <summary>
        /// 获取单个排队信息
        /// </summary>
        /// <param name="pidInNo">开始时间</param>
        /// <returns></returns>
        EntityQueueNumber GetQueueNumberByNo(string pidInNo, string queueStaus, string startDate, string endDate);
    }
}
