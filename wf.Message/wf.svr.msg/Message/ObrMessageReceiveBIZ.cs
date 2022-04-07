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
    public class ObrMessageReceiveBIZ : IDicObrMessageReceive
    {
        /// <summary>
        /// 删除危急值处理数据
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        public bool DeleteObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            bool isDelete = false;
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                isDelete = dao.DeleteObrMessageReceive(msgReceive);
            }
            return isDelete;
        }

        public bool SaveObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            bool isSave = false;
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                isSave = dao.SaveObrMessageReceive(msgReceive);
            }
            return isSave;
        }

        public List<EntityDicObrMessageReceive> SearchObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            List<EntityDicObrMessageReceive> listOMReceive = new List<EntityDicObrMessageReceive>();
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                listOMReceive = dao.SearchObrMessageReceive(msgReceive);
            }
            return listOMReceive;
        }

        public bool UpdateObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            bool isUpdate = false;
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                isUpdate = dao.UpdateObrMessageReceive(msgReceive);
            }
            return isUpdate;
        }

        public ObrMessageReceiveCollection GetMessageByReceiverID(string receiverID, EnumObrMessageReceiveType receiverType, bool bGetDeleted, bool bGetReaded, bool bGetExpired)
        {
            ObrMessageReceiveCollection omrCollection = new ObrMessageReceiveCollection();
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if(dao!=null)
            {
                omrCollection = dao.GetMessageByReceiverID(receiverID,receiverType, bGetDeleted, bGetReaded, bGetExpired);
            }
            return omrCollection;
        }
        /// <summary>
        /// 更新确认时间,根据信息ID
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        public bool UpdateObrMsgReciveToDateByID(EntityDicObrMessageReceive msgReceive)
        {
            bool isUpdateDate = false;
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                isUpdateDate = dao.UpdateObrMsgReciveToDateByID(msgReceive);
            }
            return isUpdateDate;
        }

        /// <summary>
        /// 更新确认时间和删除标志,根据信息ID
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        public bool UpdateObrMsgReciveToDateDelFlagByID(EntityDicObrMessageReceive msgReceive)
        {
            bool isUpdateDateDelFlag = false;
            IDaoObrMessageReceive dao = DclDaoFactory.DaoHandler<IDaoObrMessageReceive>();
            if (dao != null)
            {
                isUpdateDateDelFlag = dao.UpdateObrMsgReciveToDateDelFlagByID(msgReceive);
            }
            return isUpdateDateDelFlag;
        }
    }
}
