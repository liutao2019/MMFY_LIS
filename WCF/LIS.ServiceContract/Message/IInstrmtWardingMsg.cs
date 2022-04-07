using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IInstrmtWardingMsg
    {
        /// <summary>
        /// 根据病人仪器ID查询是否有警告信息
        /// </summary>
        /// <param name="patItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityInstrmtWarningMsg> CheckHasInstrmtWardMsgByPatItrId(string patItrId);

        /// <summary>
        /// 根据病人仪器ID删除警告信息
        /// </summary>
        /// <param name="patItrId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteInstrmtWardMsgByPatItrId(string patItrId);
    }
}
