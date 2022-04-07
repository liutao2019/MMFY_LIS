using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IQueueNumber
    {
        /// <summary>
        /// 保存病人信息到排队表
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveQueueInfo(EntityQueueNumber queue);

        /// <summary>
        /// 更新病人排队号
        /// </summary>
        /// <param name="queueNo"></param>
        /// <param name="queueDate"></param>
        /// <param name="pidInNo"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQueueNo(string queueNo, DateTime queueDate,string pidInNo);

        /// <summary>
        /// 获取当天时间最大排队号
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Int32 GetMaxQueueNo();

        /// <summary>
        /// 获取排队信息
        /// </summary>
        /// <param name="dateSatrt">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="windowsName">窗口号</param>
        /// <param name="windowsArea">采血区域</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityQueueNumber> GetQueueNumber(string dateSatrt, string dateEnd,string windowsName,string windowsArea);

       /// <summary>
       /// 获取该病人的排队信息
       /// </summary>
       /// <param name="pidInNo"></param>
       /// <param name="startDate"></param>
       /// <param name="endDate"></param>
       /// <returns></returns>
        [OperationContract]
        EntityQueueNumber GetQueueNumberByNo(string pidInNo,string queueStaus,string startDate,string endDate);

        /// <summary>
        /// 更新排队状态
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <param name="queueNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQueueStatus(string  pidInNo,string queueNo,string status);

        /// <summary>
        /// 更新采血窗口
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <param name="queueNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQueueWindow(string pidInNo, string queueNo,string window);
        /// <summary>
        /// /保存发声信息
        /// </summary>
        /// <param name="speech"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveMessageSpeech(EntitySysMessageSpeech speech);
    }
}
