using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.msg
{
    /// <summary>
    /// 仪器危急值数据:BIZ
    /// </summary>
    public class InstrmtUrgentTATMsgBIZ : IDicInstrmtUrgentTATMsg
    {
        public bool DeleteItrUrgentMsgDataByID(string msg_id)
        {
            bool isDelItrMsg = false;
            IDaoInstrmtUrgentTATMsg dao = DclDaoFactory.DaoHandler<IDaoInstrmtUrgentTATMsg>();
            if (dao != null)
            {
                isDelItrMsg = dao.DeleteItrUrgentMsgDataByID(msg_id);
            }
            return isDelItrMsg;
        }

        public List<EntityDicMsgTAT> GetItrUrgentMessage()
        {
            return dcl.svr.msg.InstrmtUrgentTATMsgCache.Current.cache;
        }

        public List<EntityDicMsgTAT> GetInstrmtUrgentMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listInstrmtMsg = new List<EntityDicMsgTAT>();
            IDaoInstrmtUrgentTATMsg dao = DclDaoFactory.DaoHandler<IDaoInstrmtUrgentTATMsg>();
            if(dao!=null)
            {
                listInstrmtMsg = dao.GetInstrmtUrgentMsgToCacheDao();
            }
            return listInstrmtMsg;
        }

        
    }
}
