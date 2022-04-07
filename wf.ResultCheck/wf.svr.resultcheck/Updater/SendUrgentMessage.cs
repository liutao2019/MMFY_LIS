using System;
using System.Collections.Generic;

using System.Threading;
using dcl.root.logon;
using dcl.entity;
using dcl.svr.msg;

namespace dcl.svr.resultcheck.Updater
{
    public class SendUrgentMessage : AbstractAuditClass, IAuditUpdater
    {

        public SendUrgentMessage(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report)//只有报告(二审)时才操作
            {
                Thread t = new Thread(ThreadSendUrgentMessage);
                t.Start();
            }
            else if (this.auditType == EnumOperationCode.UndoReport)
            {
                Thread t = new Thread(ThreadClearUrgentMessage);
                t.Start();
            }
        }

        #endregion


        void ThreadSendUrgentMessage()
        {
            try
            {

                if (this.pat_info.RepCtype == "2")
                {



                    //插入到用户消息表
                    EntityMessageContent msg_content = new EntityMessageContent();
                    msg_content.Deleted = false;
                    msg_content.Ext1 = this.pat_info.RepId;
                    msg_content.Ext2 = this.pat_info.PidName;
                    msg_content.ExpireTime = null;
                    msg_content.MessageType = EnumMessageType.URGENT_MESSAGE;
                    msg_content.ReceiverString = this.pat_info.PidDeptName;
                    msg_content.Title = "急查报告";

                    string message = string.Format("急查报告\r\n病区：{0} 住院号：{1} 姓名：{2}", this.pat_info.PidDeptName
                                                                        , this.pat_info.PidInNo
                                                                        , this.pat_info.PidName);

                    if (config.bSecondAuditUrgentContainCom)//发送危急值含[组合]
                    {
                        //组合
                        string temp_pat_comName = string.IsNullOrEmpty(this.pat_info.PidComName) ? "NULL" : this.pat_info.PidComName;

                        message = string.Format("急查报告 病区：{0} 住院号：{1} 姓名：{2} 组合：{3}", this.pat_info.PidDeptName
                                                                        , this.pat_info.PidInNo
                                                                        , this.pat_info.PidName
                                                                        , temp_pat_comName);
                    }

                    msg_content.XMLContent = message;

                    EntityMessageReceiver rec = new EntityMessageReceiver();
                    rec.Deleted = false;
                    rec.ReceiverType = EnumMessageReceiverType.Dept;
                    rec.ReceiverID = this.pat_info.PidDeptId;
                    rec.ReceiverName = this.pat_info.PidDeptName;

                    msg_content.ListMessageReceiver.Add(rec);

                    //new dcl.svr.msg.MessageBiz().SendMessage(msg_content);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ThreadSendUrgentMessage", ex.ToString());
            }
        }

        void ThreadClearUrgentMessage()
        {
            ThreadClearUrgentMessage(this.pat_info.RepId);
        }

        void ThreadClearUrgentMessage(string p_strPat_id)
        {
            try
            {
                //删除危急值处理表数据 msg_receiver
                EntityDicObrMessageContent conMsg = new EntityDicObrMessageContent();
                conMsg.ObrValueA = p_strPat_id;
                List<EntityDicObrMessageContent> listMsgCon = new List<EntityDicObrMessageContent>();
                listMsgCon = new ObrMessageContentBIZ().GetMessageByCondition(conMsg);
                foreach (var info in listMsgCon)
                {
                    EntityDicObrMessageReceive msgReceive = new EntityDicObrMessageReceive();
                    msgReceive.ObrId = info.ObrId;
                    new ObrMessageReceiveBIZ().DeleteObrMessageReceive(msgReceive); //删除
                }

                //删除危急值消息表数据 msg_content
                new ObrMessageContentBIZ().UPdateMessageDelFlagByObrValueA(p_strPat_id);

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ThreadClearUrgentMessage", ex.ToString());

            }
        }
    }
}
