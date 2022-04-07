using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.msg
{
    public class ObrMessageContentBIZ : IDicObrMessageContent
    {
        /// <summary>
        /// 删除危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool DeleteObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            bool IsDelMsgCon = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsDelMsgCon = dao.DeleteObrMessageContent(msgContent);
            }
            return IsDelMsgCon;
        }
        /// <summary>
        /// 获取LED消息
        /// </summary>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired)
        {
            List<EntityDicObrMessageContent> listMsgContent = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                listMsgContent = dao.GetLEDMessage(bGetDeleted, bGetExpired);
            }
            return listMsgContent;
        }

        /// <summary>
        /// 保存危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool SaveObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            bool IsSaveMsgCon = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsSaveMsgCon = dao.SaveObrMessageContent(msgContent);
            }
            return IsSaveMsgCon;
        }
        /// <summary>
        /// 查询所有危急值消息数据
        /// </summary>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> SearchAllMessageContent()
        {
            List<EntityDicObrMessageContent> list = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                list = dao.SearchAllMessageContent();
            }
            return list;
        }
        /// <summary>
        /// 根据报告ID获取病人组合和危急值信息
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> GetBacPatientsMsg(string RepId)
        {
            List<EntityDicObrMessageContent> list = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                list = dao.GetBacPatientsMsg(RepId);
            }
            return list;
        }

        /// <summary>
        /// 更新危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool UpdateObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            bool IsUpdateMsgCon = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsUpdateMsgCon = dao.UpdateObrMessageContent(msgContent);
            }
            return IsUpdateMsgCon;
        }
        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段,含标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool UpdateObrMessageContentHaveIn(EntityDicObrMessageContent msgContent)
        {
            bool IsUpdateMsgConHaveIn = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsUpdateMsgConHaveIn = dao.UpdateObrMessageContentHaveIn(msgContent);
            }
            return IsUpdateMsgConHaveIn;
        }
        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段，含删除标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool UpdateObrMessageContentHaveInDelFlag(EntityDicObrMessageContent msgContent)
        {
            bool IsUpdateMsgConDelFlag = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsUpdateMsgConDelFlag = dao.UpdateObrMessageContentHaveInDelFlag(msgContent);
            }
            return IsUpdateMsgConDelFlag;
        }
        /// <summary>
        /// 根据信息ID更新删除标志,以及查看者ID姓名和类型
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool UpdateObrMsgConToDelFlagByID(EntityDicObrMessageContent msgContent)
        {
            bool IsUpdateMsgConDelFlag = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsUpdateMsgConDelFlag = dao.UpdateObrMsgConToDelFlagByID(msgContent);
            }
            return IsUpdateMsgConDelFlag;
        }

        /// <summary>
        /// 根据信息ID更新查看者ID姓名和类型及标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public bool UpdateObrMsgConToInsignByID(EntityDicObrMessageContent msgContent)
        {
            bool IsUpdateMsgConInsign = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                IsUpdateMsgConInsign = dao.UpdateObrMsgConToInsignByID(msgContent);
            }
            return IsUpdateMsgConInsign;
        }

        /// <summary>
        /// 更新删除标志，根据扩展字段A
        /// </summary>
        /// <param name="obrValueA"></param>
        /// <returns></returns>
        public bool UPdateMessageDelFlagByObrValueA(string obrValueA)
        {
            bool isDelFlag = false;
            if (!string.IsNullOrEmpty(obrValueA))
            {
                IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
                if (dao != null)
                {
                    isDelFlag = dao.UPdateMessageDelFlagByObrValueA(obrValueA);
                }
            }
            return isDelFlag;
        }

        /// <summary>
        /// 根据条件来查危急值信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> GetMessageByCondition(EntityDicObrMessageContent content)
        {
            List<EntityDicObrMessageContent> listOBrMsgContent = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                listOBrMsgContent = dao.GetMessageByCondition(content);
            }
            return listOBrMsgContent;
        }
        /// <summary>
        /// 判断是否节假日
        /// </summary>
        /// <returns></returns>
        public bool IsDateHoliday()
        {
            bool isHoliday = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                isHoliday = dao.IsDateHoliday();
            }
            return isHoliday;
        }
        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已超时</param>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> GetAllMessage(bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            List<EntityDicObrMessageContent> list = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                List<EntityDicObrMessageContent> listContent = dao.GetAllMessage(bGetDeleted, bGetExpired);
                if (listContent.Count > 0)
                {
                    foreach (EntityDicObrMessageContent msgContent in listContent)
                    {
                        if (bGetReceiver)
                        {
                            msgContent.ListObrMessageReceiver = new UserMessageBIZ().GetMessageReceiverByMessageID(msgContent.ObrId);
                        }
                        list.Add(msgContent);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取自编危急值
        /// </summary>
        /// <param name="coondition"></param>
        /// <returns></returns>
        public List<EntityDicObrMessageContent> GetDIYCriticalMsg(EntityDicObrMessageContent coondition)
        {
            List<EntityDicObrMessageContent> list = new List<EntityDicObrMessageContent>();
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                list = dao.GetDIYCriticalMsg(coondition);
            }
            return list;
        }
        /// <summary>
        /// 添加自编危急值信息
        /// </summary>
        /// <param name="entityReceive"></param>
        /// <returns></returns>
        public bool AddDIYCriticalMsg(EntityDicObrMessageReceive entityReceive)
        {
            bool result = false;
            string flag = entityReceive.ObrMessageContent.ObrSendFlag;
            result = SaveObrMessageContent(entityReceive.ObrMessageContent);
            result = new ObrMessageReceiveBIZ().SaveObrMessageReceive(entityReceive);
            if (result)
            {
                if (flag == "1")
                {
                    //之后需要修改，现在还是引用的旧的,暂时先注释
                    //new PatientEnterService().SendCriticalMsg(dtNw);
                }
            }
            return result;
        }
        /// <summary>
        ///根据危急值消息和确认角色更新医生或者护士确认标志
        /// </summary>
        /// <param name="userRole">角色</param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public bool UpdateReadFlag(string userRole, string obrId)
        {
            bool result = false;
            IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
            if (dao != null)
            {
                result = dao.UpdateReadFlag(userRole, obrId);
            }
            return result;
        }
    }
}
