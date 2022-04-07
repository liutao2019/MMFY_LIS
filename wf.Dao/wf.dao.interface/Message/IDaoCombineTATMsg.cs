using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoCombineTATMsg
    {
        /// <summary>
        /// 获取条码组合TAT数据(仅取24小时内)
        /// </summary>
        List<EntityDicMsgTAT> GetBcComTAtMsgToCacheDao();

        /// <summary>
        /// 获取条码组合检验中TAT数据(仅取24小时内)
        /// </summary>
        List<EntityDicMsgTAT> GetBcComLabTAtMsgToCacheDao();

        /// <summary>
        /// 获取组合TAT数据(仅取24小时内)
        /// </summary>
        /// <param name="isOutLink"></param>
        /// <returns></returns>
        List<EntityDicMsgTAT> GetCombineTATmsgToCacheDao(bool isOutLink);

        /// <summary>
        /// 获取条码(采集到签收)TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        List<EntityDicMsgTAT> GetBcSamplToReceiveTAtMsgToCacheDao();
    }
}
