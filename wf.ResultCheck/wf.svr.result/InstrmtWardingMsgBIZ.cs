using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    public class InstrmtWardingMsgBIZ : IInstrmtWardingMsg
    {
        public List<EntityInstrmtWarningMsg> CheckHasInstrmtWardMsgByPatItrId(string patItrId)
        {
            List<EntityInstrmtWarningMsg> listMsg = new List<EntityInstrmtWarningMsg>();
            IDaoInstrmtWardingMsg msgDao = DclDaoFactory.DaoHandler<IDaoInstrmtWardingMsg>();
            if (msgDao != null)
            {
                listMsg = msgDao.CheckHasInstrmtWardMsgByPatItrId(patItrId);
            }
            return listMsg;
        }

        public Boolean DeleteInstrmtWardMsgByPatItrId(string patId)
        {
            bool result = false;
            IDaoInstrmtWardingMsg msgDao = DclDaoFactory.DaoHandler<IDaoInstrmtWardingMsg>();
            if (msgDao != null)
            {
                result = msgDao.DeleteInstrmtWardMsgByPatItrId(patId);
            }
            return result;
        }
    }
}
