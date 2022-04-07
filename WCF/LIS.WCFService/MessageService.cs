using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.msg;
using dcl.pub.entities.Message;
using dcl.pub.entities;
using Lib.DAC;
using dcl.svr.result;
using dcl.entity;

namespace dcl.svr.wcf
{
    public class MessageService : IMessageContract
    {
        #region IMessageContract 成员

        public EntityOperationResult InsertMessage(dcl.pub.entities.Message.EntityMessageContent message)
        {
            return new MessageBiz().SendMessage(message);
        }

        public dcl.pub.entities.Message.EntityMessageContent GetMessageContentByMsgID(string messageID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            return new MessageBiz().GetMessageContentByMsgID(messageID, bGetReceiver, bGetDeleted, bGetExpired);
        }

        public List<dcl.pub.entities.Message.EntityMessageContent> GetMessageByType(dcl.pub.entities.Message.EnumMessageType messageType, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            return new MessageBiz().GetMessageByType(messageType, bGetReceiver, bGetDeleted, bGetExpired);
        }


        public List<dcl.pub.entities.Message.EntityMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired)
        {
            return new MessageBiz().GetLEDMessage(bGetDeleted, bGetExpired);
        }

        public MessageReceiverCollection GetMessageByReceiverID(string receiverID, dcl.pub.entities.Message.EnumMessageReceiverType receiverType, bool bGetDeleted, bool bGetReaded, bool bGetExpired)
        {
            return new MessageBiz().GetMessageByReceiverID(receiverID, receiverType, bGetDeleted, bGetReaded, bGetExpired);
        }
        public DataTable GetUrgentflagAndPatlookcodeByPatid(string pat_id)
        {
            return new MessageBiz().GetUrgentflagAndPatlookcodeByPatid(pat_id);
        }

        public EntityOperationResult DeleteMessageByID(string messageID, bool bPhiDelete)
        {
            return new MessageBiz().DeleteMessageByID(messageID, bPhiDelete);
        }

        public EntityOperationResult DeleteMessageByIDAndUpdateCriticalChecker(lis.dto.Entity.AuditInfo objAuditInfo, string messageID, string pat_id)
        {
            EntityOperationResult result = null;
            try
            {
                string msg = objAuditInfo.ExetMsg;
                if (objAuditInfo.IsSaveMsg)//是否保存危急值编辑内容
                {
                    //删除消息-并保持危急值编辑内容
                    result = new MessageBiz().DeleteMessageByID(objAuditInfo, messageID, false, pat_id);
                }
                else
                {
                    //删除消息
                    result = new MessageBiz().DeleteMessageByID(objAuditInfo, messageID, false);
                }

                try
                {
                    //更新病人表危急值查看标志，查看人

                    string sql = "update patients set pat_urgent_flag = 2,pat_look_code = ?, pat_look_date = getdate() where pat_id = ?";
                    if (msg == "急查")//暂时用来急查用
                    {
                        sql = "update patients set pat_look_code = ?, pat_look_date = getdate() where pat_id = ?";

                    }
                    SqlHelper helper = new SqlHelper();

                    DbCommandEx cmd = helper.CreateCommandEx(sql);
                    cmd.AddParameterValue(objAuditInfo.UserName, System.Data.DbType.AnsiString);
                    cmd.AddParameterValue(pat_id, System.Data.DbType.AnsiString);
                    helper.ExecuteNonQuery(cmd);

                    //多重耐药菌
                    try
                    {
                        sql = "update patients set pat_drugfast=2 where pat_id = ? and pat_drugfast=1";
                        cmd = helper.CreateCommandEx(sql);
                        cmd.AddParameterValue(pat_id, System.Data.DbType.AnsiString);
                        helper.ExecuteNonQuery(cmd);
                    }
                    catch
                    {

                    }

                    try
                    {
                        sql = "update PatientsReportIndex set pat_critical_flag = 2,pat_look_code = ?,pat_look_name=?, pat_look_time = getdate() where pat_id = ?";
                        cmd = helper.CreateCommandEx(sql);
                        cmd.AddParameterValue(objAuditInfo.UserId, System.Data.DbType.AnsiString);
                        cmd.AddParameterValue(objAuditInfo.UserName, System.Data.DbType.AnsiString);
                        cmd.AddParameterValue(pat_id, System.Data.DbType.AnsiString);
                        helper.ExecuteNonQuery(cmd);
                    }
                    catch
                    {

                    }



                    //DeptMessageCache.Current.RemoveMessage(messageID);
                }
                catch (Exception ex)
                {
                    //dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex.Message);
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    throw;
                }
                try
                {

                    if (objAuditInfo.SendMsgFlag)
                    {
                        new PatientEnterService().SendCriticalMsg(pat_id);
                    }
                    string sql = string.Format(@"
declare @barcode varchar(20)
set @barcode=(select top 1 pat_bar_code from patients with(nolock) where pat_id='{5}')

insert into bc_sign( bc_date, bc_login_id, bc_name, bc_status, bc_bar_no, bc_bar_code, bc_place, bc_flow, bc_remark, pat_id)
values(getdate(),'{1}','{2}','1000',@barcode,@barcode,'{3}',1,'{4}','{5}')

", "", objAuditInfo.UserId, objAuditInfo.UserName, objAuditInfo.Place, objAuditInfo.msg_content, pat_id);
                    SqlHelper helper = new SqlHelper();
                    helper.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {

                    dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex.Message);
                }
            }
            catch (Exception ex1)
            {

                dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex1.Message);
            }
            return result;
        }

    
        public EntityOperationResult DeleteMessageReceiver(string messageID, EnumMessageReceiverType receiverType, string receiverID, bool bPhiDelete)
        {
            return new MessageBiz().DeleteMessageReceiver(messageID, receiverType, receiverID, bPhiDelete);
        }

        public List<dcl.pub.entities.Message.EntityMessageContent> GetAllMessage(bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            return new MessageBiz().GetAllMessage(bGetReceiver, bGetDeleted, bGetExpired);
        }

        public System.Data.DataSet GetRoleAndUser()
        {
            return new MessageBiz().GetRoleAndUser();
        }

        public List<dcl.pub.entities.Message.EntityMessageContent> GetMessageBySenderID(string senderID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            return new MessageBiz().GetMessageBySenderID(senderID, bGetReceiver, bGetDeleted, bGetExpired);
        }

        public MessageReceiverCollection Cache_GetUserMessage(string userLoginID)
        {
            return UserMessageCache.Current.GetUserMessage(userLoginID);
        }


        public EntityOperationResult UpdateMessageContent(EntityMessageContent message)
        {
            return new MessageBiz().UpdateMessageContent(message);
        }


        public System.Data.DataTable GetDeptInfo()
        {
            return new MessageBiz().m_dtbGetDeptInfo();
        }

        public DataTable GetDocInfo()
        {
            return new MessageBiz().GetDocInfo();
        }

        public string GetConfigValue(string confiigcode)
        {
            return dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig(confiigcode);
        }


        public MessageReceiverCollection GetDeptMessageFromCacheWithMsgType(string dept_code, EnumMessageType message_type)
        {
            return DeptMessageCache.Current.GetMessage(dept_code, message_type);
        }


        public void RefreshDeptMessage()
        {
            DeptMessageCache.Current.Refresh();
        }

        /// <summary>
        /// 刷新危急值缓存信息
        /// </summary>
        public void RefreshUrgentMessage()
        {
            UrgentMessageCache.Current.Refresh();
        }

        public System.Data.DataSet SJHisCheckPassWord(System.Data.DataSet ds)
        {
            return new MessageBiz().SJHisCheckPassWord(ds);
        }


        public MessageReceiverCollection GetDeptMessageFromCache(string dept_code)
        {
            return DeptMessageCache.Current.GetMessage(dept_code);
        }

        /// <summary>
        /// 根据多科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        public MessageReceiverCollection GetDeptListMessageFromCache(string dept_codes)
        {
            return DeptMessageCache.Current.GetMessageByDepts(dept_codes);
        }


        public System.Data.DataSet LisCheckPassWord(System.Data.DataSet ds)
        {
            return new MessageBiz().m_blnCheckPassWord(ds);
        }

        /// <summary>
        /// 获取危急值历史信息
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public System.Data.DataSet GetUrgentHistoryMsg(System.Data.DataSet ds)
        {
            return new MessageBiz().GetUrgentHistoryMsg(ds);
        }
        #endregion

        #region IMessageContract 成员


        public System.Data.DataTable isBackForCheck(string pat_id, string msg_type)
        {
            return new MessageBiz().isBackForCheck(pat_id, msg_type);
        }

        #endregion

        #region IMessageContract 成员

        /// <summary>
        /// 根据id删除指定的仪器危急值信息
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public bool DeleteItrUrgentMsgDataByID(string msg_id)
        {
            return dcl.svr.msg.InstrmtUrgentMsgCache.Current.DeleteItrUrgentMsgDataByID(msg_id);
        }

        /// <summary>
        /// 获取仪器危急值信息(缓存)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetItrUrgentMessage(string strWhere)
        {
            return dcl.svr.msg.InstrmtUrgentMsgCache.Current.GetItrUrgentMessage(strWhere);
        }

        /// <summary>
        /// 获取仪器质控信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetItrQcMessage(string strWhere)
        {
            return new dcl.svr.msg.MessageBiz().GetItrQcMessage(strWhere);
        }

        /// <summary>
        /// 获取组合TAT信息(缓存)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetComTATMessage(string strWhere)
        {
            return dcl.svr.msg.CombineTATmsgCache.Current.GetComTATMessage(strWhere);
        }

        /// <summary>
        /// 根据筛选条件获取条码组合TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBcComTATMessage(string strWhere)
        {
            return dcl.svr.msg.CombineTATmsgCache.Current.GetBcComTATMessage(strWhere);
        }

        /// <summary>
        /// 根据筛选条件获取条码组合检验中TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBcComLabTATMessage(string strWhere)
        {
            return dcl.svr.msg.CombineTATmsgCache.Current.GetBcComLabTATMessage(strWhere);
        }

        /// <summary>
        /// 查询组合TAT信息
        /// </summary>
        /// <param name="receiver_date">签收开始日期</param>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public System.Data.DataTable SearchCombineTATmsg(DateTime receiver_date, string strWhere)
        {
            return dcl.svr.msg.CombineTATmsgCache.Current.SearchCombineTATmsg(receiver_date, strWhere);
        }

        /// <summary>
        /// 根据筛选条件获取条码(采集到签收)TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBcBcSamplToReceiveTATMessage(string strWhere)
        {
            return dcl.svr.msg.CombineTATmsgCache.Current.GetBcBcSamplToReceiveTATMessage(strWhere);
        }

        /// <summary>
        /// 获取自编危急值信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDIYCriticalMsg(string sqlWhere)
        {
            return new dcl.svr.msg.MessageBiz().GetDIYCriticalMsg(sqlWhere);
        }

        /// <summary>
        /// 添加自编危急值信息
        /// </summary>
        /// <param name="dtNw"></param>
        /// <returns></returns>
        public int AddDIYCriticalMsg(System.Data.DataTable dtNw)
        {
            int rvInt = 0;

            
            if (dtNw == null && dtNw.Rows.Count == 0)
            {
                return rvInt;
            }
            string flag = dtNw.Rows[0]["msg_send_flag"].ToString();
            rvInt = new dcl.svr.msg.MessageBiz().AddDIYCriticalMsg(dtNw);

            if (rvInt > 0)
            {
                if (flag == "1")
                {
                    new PatientEnterService().SendCriticalMsg(dtNw);
                }
            }

            return rvInt;
        }

        /// <summary>
        /// 处理回退标本
        /// </summary>
        /// <param name="barcode"></param>
        public void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark)
        {
            new MessageBiz().HandleReturnMessage(barcode, strOperatorID, strOperatorName, currentServerTime, bc_remark);
        }
        #endregion
    }
}
