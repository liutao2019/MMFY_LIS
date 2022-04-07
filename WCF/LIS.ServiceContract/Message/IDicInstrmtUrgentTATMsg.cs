using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 仪器危急值数据:接口
    /// </summary>
    [ServiceContract]
    public interface IDicInstrmtUrgentTATMsg
    {
        /// <summary>
        /// 获取仪器危急值(cache)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetItrUrgentMessage();

        /// <summary>
        /// 获取仪器危急值数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetInstrmtUrgentMsgToCacheDao();

        /// <summary>
        /// 根据ID删除指定的仪器危急值数据
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteItrUrgentMsgDataByID(string msg_id);
    }
}
