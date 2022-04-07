using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDeptMessage))]
    public class DaoDeptMessage : IDaoDeptMessage
    {
        public bool DeleteMessageByIDPatId(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id)
        {
            bool isDelMsgByID = false;
            bool isSecond = false;
            try
            {
                string sqlDeleteContent = string.Empty;
                string sqlDeleteReceiver = string.Empty;

                //作为危急值处理更新操作的实体参数
                EntityDicObrMessageContent obrMsgContent = new EntityDicObrMessageContent();
                obrMsgContent.ObrAuditUserId = objAuditInfo.UserId;
                obrMsgContent.ObrAuditUserName = objAuditInfo.UserName;
                obrMsgContent.ObrConfirmType = objAuditInfo.MsgAffirmType;
                obrMsgContent.ObrValueA = pat_id;

                bool isThree = false;
                if (bPhiDelete)//物理删除
                {
                    //sqlDeleteContent = @"delete from msg_content where msg_id = @msg_id";
                    EntityDicObrMessageContent eyMsgContent = new EntityDicObrMessageContent();
                    eyMsgContent.ObrId = messageID;
                    isDelMsgByID = new DaoObrMessageContent().DeleteObrMessageContent(eyMsgContent);

                    //sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id";
                    EntityDicObrMessageReceive eyMsgReceive = new EntityDicObrMessageReceive();
                    eyMsgReceive.ObrId = messageID;
                    isSecond = new DaoObrMessageReceive().DeleteObrMessageReceive(eyMsgReceive);

                }
                else
                {
                    //不取消其他提醒,只取消内部提醒
                    if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                    {
                        //sqlDeleteContent = @"update msg_content set msf_insgin_flag ='1',msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_type in ('1024','4096','2024','3024') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                        isDelMsgByID = new DaoObrMessageContent().UpdateObrMessageContentHaveIn(obrMsgContent);

                        //sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                        EntityDicObrMessageReceive obrMsgReceive = new EntityDicObrMessageReceive();
                        obrMsgReceive.ObrId = messageID;
                        isSecond = new DaoObrMessageReceive().UpdateObrMsgReciveToDateByID(obrMsgReceive);
                    }
                    else
                    {
                        //sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_type in ('1024','4096','2024','3024') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                        isDelMsgByID = new DaoObrMessageContent().UpdateObrMessageContentHaveInDelFlag(obrMsgContent);

                        //sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                        EntityDicObrMessageReceive obrMsgReceive = new EntityDicObrMessageReceive();
                        obrMsgReceive.ObrId = messageID;
                        isSecond = new DaoObrMessageReceive().UpdateObrMsgReciveToDateDelFlagByID(obrMsgReceive);
                    }
                    isThree = AddPatExtEditInfo(objAuditInfo, pat_id);//在扩展表[保存]危急值编辑内容
                }
                if (isSecond && isDelMsgByID && isThree)
                    return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public bool DeleteMessageByID(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete)
        {
            bool isDelMsgByID = false;
            string sqlDeleteContent = string.Empty;
            string sqlDeleteReceiver = string.Empty;
            try
            {
                //作为危急值处理更新操作的实体参数
                EntityDicObrMessageContent obrMsgContent = new EntityDicObrMessageContent();
                obrMsgContent.ObrAuditUserId = objAuditInfo.UserId;
                obrMsgContent.ObrAuditUserName = objAuditInfo.UserName;
                obrMsgContent.ObrConfirmType = objAuditInfo.MsgAffirmType;
                obrMsgContent.ObrId = messageID;

                if (bPhiDelete)//物理删除
                {
                    //sqlDeleteContent = @"delete from msg_content where msg_id = @msg_id";
                    EntityDicObrMessageContent eyMsgContent = new EntityDicObrMessageContent();
                    eyMsgContent.ObrId = messageID;
                    isDelMsgByID = new DaoObrMessageContent().DeleteObrMessageContent(eyMsgContent);

                    //sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id";
                    EntityDicObrMessageReceive eyMsgReceive = new EntityDicObrMessageReceive();
                    eyMsgReceive.ObrId = messageID;
                    isDelMsgByID = new DaoObrMessageReceive().DeleteObrMessageReceive(eyMsgReceive);
                }
                else
                {
                    //只标记内部提醒已处理
                    if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                    {
                        //sqlDeleteContent = @"update msg_content set msf_insgin_flag ='1',msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                        isDelMsgByID = new DaoObrMessageContent().UpdateObrMsgConToInsignByID(obrMsgContent);

                        //sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                        EntityDicObrMessageReceive eyObrMsgReceive = new EntityDicObrMessageReceive();
                        eyObrMsgReceive.ObrId = messageID;
                        isDelMsgByID = new DaoObrMessageReceive().UpdateObrMsgReciveToDateByID(eyObrMsgReceive);
                    }
                    else
                    {
                        //sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                        isDelMsgByID = new DaoObrMessageContent().UpdateObrMsgConToDelFlagByID(obrMsgContent);

                        //sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                        EntityDicObrMessageReceive eyObrMsgReceive = new EntityDicObrMessageReceive();
                        eyObrMsgReceive.ObrId = messageID;
                        isDelMsgByID = new DaoObrMessageReceive().UpdateObrMsgReciveToDateDelFlagByID(eyObrMsgReceive);
                    }
                }
                return isDelMsgByID;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 在扩展表-增加危急值编辑信息
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public bool AddPatExtEditInfo(EntityAuditInfo objAuditInfo, string pat_id)
        {
            bool isPatExt = false;
            if (string.IsNullOrEmpty(objAuditInfo.MsgContent))
            {
                objAuditInfo.MsgContent = string.Format("{0}", objAuditInfo.Place);

            }
            else if (!string.IsNullOrEmpty(objAuditInfo.Place))
            {
                objAuditInfo.MsgContent = objAuditInfo.MsgContent + "\r\n" + objAuditInfo.Place;
            }

            try
            {
                bool patExtIsExist = new DaoPidReportMainExt().SearchPatExtExistByID(pat_id);////记录扩展表是否存已存在此ID

                if (patExtIsExist)//若扩展表存在此ID，则Update
                {
                    isPatExt = new DaoPidReportMainExt().UpdatePidReportMainExt(objAuditInfo, pat_id);
                }
                else
                {
                    isPatExt = new DaoPidReportMainExt().SavePidReportMainExt(objAuditInfo, pat_id);
                }
                return isPatExt;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteMessageByIDAndUpdateCriticalChecker(EntityAuditInfo objAuditInfo, string messageID, string pat_id)
        {
            bool IsTrue = false;
            try
            {
                DBManager helper = new DBManager();

                string msg = objAuditInfo.ExetMsg;
                if (objAuditInfo.IsSaveMsg)//是否保存危急值编辑内容
                {
                    //删除消息-并保持危急值编辑内容
                    IsTrue = DeleteMessageByIDPatId(objAuditInfo, messageID, false, pat_id);
                }
                else
                {
                    //删除消息
                    IsTrue = DeleteMessageByID(objAuditInfo, messageID, false);
                }

                try
                {
                    //更新病人表危急值查看标志，查看人
                    string sql = "update Pat_lis_main set Pma_urgent_flag = 2,Pma_read_Buser_id =@rep_read_user_id, Pma_read_date = getdate() where Pma_rep_id =@rep_id ";
                    if (msg == "急查")//暂时用来急查用
                    {
                        sql = "update Pat_lis_main set Pma_read_Buser_id = @rep_read_user_id, Pma_read_date = getdate() where Pma_rep_id = @rep_id";

                    }

                    List<DbParameter> paramHt = new List<DbParameter>();
                    paramHt.Add(new SqlParameter("@rep_read_user_id", objAuditInfo.UserName));
                    paramHt.Add(new SqlParameter("@rep_id", pat_id));

                    IsTrue = helper.ExecCommand(sql, paramHt) > 0;

                    //多重耐药菌
                    sql = string.Format(@"update Pat_lis_main set Pma_drugfast_flag=2 where Pma_rep_id = '{0}' and Pma_drugfast_flag=1", pat_id);
                    helper.ExecSql(sql);

                    sql = "update PatientsReportIndex set pat_critical_flag = 2,pat_look_code = @pat_look_code,pat_look_name=@pat_look_name, pat_look_time = getdate() where pat_id = @pat_id ";
                    List<DbParameter> paramHt2 = new List<DbParameter>();
                    paramHt2.Add(new SqlParameter("@pat_look_code", objAuditInfo.UserId));
                    paramHt2.Add(new SqlParameter("@pat_look_name", objAuditInfo.UserName));
                    paramHt2.Add(new SqlParameter("@pat_id", pat_id));

                    IsTrue = helper.ExecCommand(sql, paramHt2) > 0;
                } 
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }

                try
                {
                    if (objAuditInfo.SendMsgFlag)
                    {
                        //之后需要修改，现在还是引用的旧的(暂时先屏蔽)
                        //new PatientEnterService().SendCriticalMsg(pat_id);
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

                return IsTrue;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return IsTrue;
        }

        public void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark)
        {
            try
            {
                DBManager helper = new DBManager();

                List<string> strList = new List<string>();
                string sqlMessage = string.Format(@" update Sample_return set Sret_proc_flag = 1 where Sret_Smain_bar_code='{0}'", barcode);
                string sqlPatients = string.Format(@" update Sample_main set Sma_return_flag = 0 where Sma_bar_code='{0}'", barcode);
                string sqlSign = string.Format(@" insert into Sample_process_detial(Sproc_date, Sproc_status, Sproc_user_id, Sproc_user_name, Sproc_Sma_bar_id, Sproc_Sma_bar_code ,Sproc_content ,Sproc_times) 
                            values ('{0}', '{1}' , '{2}' ,'{3}', '{4}', '{5}' ,'{6}' ,{7})"
                                , currentServerTime, "9", strOperatorID, strOperatorName, barcode, barcode, bc_remark, "1");

                strList.Add(sqlMessage);
                strList.Add(sqlPatients);
                strList.Add(sqlSign);

                foreach (string s in strList)
                {
                    helper.ExecSql(s);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

    }
}
