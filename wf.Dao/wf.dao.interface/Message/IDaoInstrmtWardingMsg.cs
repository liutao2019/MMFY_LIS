using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoInstrmtWardingMsg
    {
        /// <summary>
        /// 根据病人仪器ID查询是否有警告信息
        /// </summary>
        /// <param name="patItrId"></param>
        /// <returns></returns>
        List<EntityInstrmtWarningMsg> CheckHasInstrmtWardMsgByPatItrId(string patItrId);

        /// <summary>
        /// 根据病人仪器ID删除警告信息
        /// </summary>
        /// <param name="patItrId"></param>
        /// <returns></returns>
        bool DeleteInstrmtWardMsgByPatItrId(string patItrId);
    }
}
