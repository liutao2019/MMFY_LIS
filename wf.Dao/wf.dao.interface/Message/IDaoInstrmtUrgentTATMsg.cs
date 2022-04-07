using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoInstrmtUrgentTATMsg
    {
        /// <summary>
        /// 获取仪器危急值数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        List<EntityDicMsgTAT> GetInstrmtUrgentMsgToCacheDao();

        /// <summary>
        /// 根据ID删除指定的仪器危急值数据
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        bool DeleteItrUrgentMsgDataByID(string msg_id);
    }
}
