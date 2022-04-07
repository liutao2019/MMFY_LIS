using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 组合TAT数据:接口
    /// </summary>
    [ServiceContract]
    public interface IDicCombineTATMsg
    {
        /// <summary>
        /// 获取条码组合TAT数据(cache)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcComTATMessage();

        /// <summary>
        /// 获取条码组合TAT数据(仅取24小时内)
        /// </summary>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcComTAtMsgToCacheDao();

        

        /// <summary>
        /// 获取条码组合检验中TAT数据(cache)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcComLabTATMessage();

        /// <summary>
        /// 获取条码组合检验中TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcComLabTAtMsgToCacheDao();

        /// <summary>
        /// 获取组合TAT数据(cache)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetComTATMessage();

        /// <summary>
        /// 获取组合TAT数据(仅取24小时内)
        /// </summary>
        /// <param name="isOutLink"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetCombineTATmsgToCacheDao(bool isOutLink);

        /// <summary>
        /// 获取条码(采集到签收)TAT数据(cache)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcBcSamplToReceiveTATMessage();

        /// <summary>
        /// 获取条码(采集到签收)TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMsgTAT> GetBcSamplToReceiveTAtMsgToCacheDao();
    }
}
