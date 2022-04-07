using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using lis.dto.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using dcl.root.dac;
using dcl.svr.framedic;
using dcl.pub.entities;
using dcl.entity;
using dcl.pub.entities.Message;

namespace dcl.svr.msg
{
    public class MessageBiz
    {
        #region 增
        /// <summary>
        /// 插入消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public EntityOperationResult SendMessage(EntityMessageContent message)
        {
            EntityOperationResult result = new EntityOperationResult();
            message.MessageID = MessageHelper.GetNewMessageID();//生成新的消息ID
            message.CreateTime = ServerDateTime.GetDatabaseServerDateTime();

            message.ListMessageReceiver = GenerateMessageReceiver(message);//生成消息接收者

            SqlCommand cmdMessageContent = message.GenerateInsertCommand();

            List<SqlCommand> cmdMessageReceiver = new List<SqlCommand>();
            foreach (EntityMessageReceiver item in message.ListMessageReceiver)//遍历接收者生成消息接收者记录
            {
                item.MessageID = message.MessageID;
                cmdMessageReceiver.Add(item.GenerateInsertCommand());
            }

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    helper.ExecuteNonQuery(cmdMessageContent);

                    foreach (SqlCommand cmd in cmdMessageReceiver)
                    {
                        helper.ExecuteNonQuery(cmd);
                    }

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return result;
        }

        /// <summary>
        /// 在扩展表-增加危急值编辑信息
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult AddPatExtEditInfo(AuditInfo objAuditInfo, string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            if (string.IsNullOrEmpty(objAuditInfo.msg_content))
            {
                objAuditInfo.msg_content = string.Format("{0}", objAuditInfo.Place);

            }
            else if (!string.IsNullOrEmpty(objAuditInfo.Place))
            {
                objAuditInfo.msg_content = objAuditInfo.msg_content + "\r\n" + objAuditInfo.Place;
            }

            string sqlInsertPatExt = string.Empty;

            sqlInsertPatExt = @"INSERT INTO patients_ext(msg_content,msg_checker_id,msg_checker_name,pat_id)
            VALUES (@msg_content,@msg_checker_id,@msg_checker_name,@pat_id)";

            SqlCommand cmdPatExtEditInfo = new SqlCommand();

            try
            {
                bool patExtIsExist = this.PatExtExistByID(pat_id);//记录扩展表是否存已存在此ID

                if (patExtIsExist)//若扩展表存在此ID，则Update
                {
                    sqlInsertPatExt = @"UPDATE patients_ext
   SET msg_content = @msg_content,msg_checker_id = @msg_checker_id,msg_checker_name = @msg_checker_name
 WHERE pat_id=@pat_id";
                }

                cmdPatExtEditInfo.CommandText = sqlInsertPatExt;

                cmdPatExtEditInfo.Parameters.AddWithValue("msg_content", objAuditInfo.msg_content);//编辑内容
                cmdPatExtEditInfo.Parameters.AddWithValue("msg_checker_id", objAuditInfo.UserId);//验证者id
                cmdPatExtEditInfo.Parameters.AddWithValue("msg_checker_name", objAuditInfo.UserName);//验证者名称
                cmdPatExtEditInfo.Parameters.AddWithValue("pat_id", pat_id);//ID

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    helper.ExecuteNonQuery(cmdPatExtEditInfo);

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return result;
        }
        #endregion

        #region 删
        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageID">消息ID</param>
        /// <param name="bPhiDelete">是否为物理删除：数据库中删除/置删除标志</param>
        /// <returns></returns>
        public EntityOperationResult DeleteMessageByID(string messageID, bool bPhiDelete)
        {
            EntityOperationResult result = new EntityOperationResult();

            string sqlDeleteContent = string.Empty;
            string sqlDeleteReceiver = string.Empty;
            if (bPhiDelete)//物理删除
            {
                sqlDeleteContent = @"delete from msg_content where msg_id = @msg_id";
                sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id";
            }
            else
            {
                sqlDeleteContent = @"update msg_content set msg_del_flag = 1 where msg_id = @msg_id";
                sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
            }
            dcl.root.logon.Logger.WriteException("MessageService", messageID, sqlDeleteContent);
            SqlCommand cmdDelContent = new SqlCommand(sqlDeleteContent);
            cmdDelContent.Parameters.AddWithValue("msg_id", messageID);


            SqlCommand cmdDelReceiver = new SqlCommand(sqlDeleteReceiver);
            cmdDelReceiver.Parameters.AddWithValue("receiver_msg_id", messageID);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int i = helper.ExecuteNonQuery(cmdDelContent);
                    int n = helper.ExecuteNonQuery(cmdDelReceiver);

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex.Message);
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return result;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageID">消息ID</param>
        /// <param name="bPhiDelete">是否为物理删除：数据库中删除/置删除标志</param>
        /// <returns></returns>
        public EntityOperationResult DeleteMessageByID(AuditInfo objAuditInfo, string messageID, bool bPhiDelete)
        {
            EntityOperationResult result = new EntityOperationResult();

            string sqlDeleteContent = string.Empty;
            string sqlDeleteReceiver = string.Empty;
            if (bPhiDelete)//物理删除
            {
                sqlDeleteContent = @"delete from msg_content where msg_id = @msg_id";
                sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id";
            }
            else
            {
                sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                //只标记内部提醒已处理
                if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                {
                    sqlDeleteContent = @"update msg_content set msf_insgin_flag ='1',msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                    sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                }
                else
                {
                    sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                    sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                }
            }

            //dcl.root.logon.Logger.WriteException("MessageService", messageID, sqlDeleteContent);

            SqlCommand cmdDelContent = new SqlCommand(sqlDeleteContent);
            cmdDelContent.Parameters.AddWithValue("msg_checker_id", objAuditInfo.UserId);
            cmdDelContent.Parameters.AddWithValue("msg_checker_name", objAuditInfo.UserName);
            cmdDelContent.Parameters.AddWithValue("msg_affirm_type", objAuditInfo.msg_affirm_type);//1-自动确认 2-手工确认
            cmdDelContent.Parameters.AddWithValue("msg_id", messageID);

            SqlCommand cmdDelReceiver = new SqlCommand(sqlDeleteReceiver);
            cmdDelReceiver.Parameters.AddWithValue("receiver_msg_id", messageID);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int i = helper.ExecuteNonQuery(cmdDelContent);
                    int n = helper.ExecuteNonQuery(cmdDelReceiver);

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex.Message);
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return result;
        }

        /// <summary>
        /// 删除消息-并保存危急值编辑框内容
        /// </summary>
        /// <param name="messageID">消息ID</param>
        /// <param name="bPhiDelete">是否为物理删除：数据库中删除/置删除标志</param>
        /// <param name="pat_id">病人表主索引ID</param>
        /// <returns></returns>
        public EntityOperationResult DeleteMessageByID(AuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();

            string sqlDeleteContent = string.Empty;
            string sqlDeleteReceiver = string.Empty;
            if (bPhiDelete)//物理删除
            {
                sqlDeleteContent = @"delete from msg_content where msg_id = @msg_id";

                sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id";
            }
            else
            {
                //  sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";
                //不取消其他提醒,只取消内部提醒
                if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                {
                    sqlDeleteContent = @"update msg_content set msf_insgin_flag ='1',msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_type in ('1024','4096','2024','3024') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                    sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                }
                else
                {
                    sqlDeleteContent = @"update msg_content set msg_del_flag = 1,msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_type in ('1024','4096','2024','3024') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                    sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate()  where receiver_msg_id = @receiver_msg_id";
                }

                //if (objAuditInfo.IsSaveMsg)//是否保存危急值编辑内容
                //{
                //if (!string.IsNullOrEmpty(objAuditInfo.msg_content))//危急值编辑内容是否不为空
                //{
                result = AddPatExtEditInfo(objAuditInfo, pat_id);//在扩展表[保存]危急值编辑内容

                //if (!result.Success)//若编辑信息保存失败,即不删除状态
                //{
                //  sqlDeleteContent = @"update msg_content set msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_id = @msg_id";

                //        sqlDeleteContent = @"update msg_content set msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where  msg_type in ('1024','4096') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                //      sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=@receiver_read_time  where receiver_msg_id = @receiver_msg_id";
                //     }
                //}
                //else//若编辑内容为空,则不删除状态
                //{
                //    // sqlDeleteContent = @"update msg_content set msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_id = @msg_id";

                //    sqlDeleteContent = @"update msg_content set msg_checker_id=@msg_checker_id,msg_checker_name=@msg_checker_name,msg_affirm_type=@msg_affirm_type  where msg_type in ('1024','4096') and msg_del_flag = 0 and msg_ext1 =@msg_ext1";
                //    sqlDeleteReceiver = @"update msg_receiver set receiver_read_time=@receiver_read_time  where receiver_msg_id = @receiver_msg_id";
                //}
                //  }
            }
            //dcl.root.logon.Logger.WriteException("MessageService", messageID, sqlDeleteContent);
            SqlCommand cmdDelContent = new SqlCommand(sqlDeleteContent);
            cmdDelContent.Parameters.AddWithValue("msg_checker_id", objAuditInfo.UserId);
            cmdDelContent.Parameters.AddWithValue("msg_checker_name", objAuditInfo.UserName);
            cmdDelContent.Parameters.AddWithValue("msg_affirm_type", objAuditInfo.msg_affirm_type);//1-自动确认 2-手工确认
            // cmdDelContent.Parameters.AddWithValue("msg_id", messageID);
            cmdDelContent.Parameters.AddWithValue("msg_ext1", pat_id);



            SqlCommand cmdDelReceiver = new SqlCommand(sqlDeleteReceiver);
            cmdDelReceiver.Parameters.AddWithValue("receiver_msg_id", messageID);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int i = helper.ExecuteNonQuery(cmdDelContent);
                    int n = helper.ExecuteNonQuery(cmdDelReceiver);

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("MessageService", "DeleteMessageByIDAndUpdateCriticalChecker", ex.Message);
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return result;
        }

        /// <summary>
        /// 删除消息接收者
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="receiverType"></param>
        /// <param name="receiverID"></param>
        /// <param name="bPhiDelete"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteMessageReceiver(string messageID, EnumMessageReceiverType receiverType, string receiverID, bool bPhiDelete)
        {
            EntityOperationResult result = new EntityOperationResult();

            string sqlDeleteReceiver = string.Empty;
            if (bPhiDelete)//物理删除
            {
                sqlDeleteReceiver = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id and receiver_type = @receiver_type and receiver_id = @receiver_id";
            }
            else
            {
                sqlDeleteReceiver = @"update msg_receiver set receiver_del_flag = 1,receiver_read_time=getdate() where receiver_msg_id = @receiver_msg_id and receiver_type = @receiver_type and receiver_id = @receiver_id";
            }

            SqlCommand cmd = new SqlCommand(sqlDeleteReceiver);
            cmd.Parameters.AddWithValue("receiver_msg_id", messageID);
            cmd.Parameters.AddWithValue("receiver_type", (int)receiverType);
            cmd.Parameters.AddWithValue("receiver_id", receiverID);

            DBHelper helper = new DBHelper();

            try
            {
                helper.ExecuteNonQuery(cmd);

                UserMessageCache.Current.DeleteReceivedMessage(messageID, receiverID, bPhiDelete);
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return result;
        }
        #endregion

        #region 改


        public EntityOperationResult UpdateMessageContent(EntityMessageContent message)
        {
            EntityOperationResult result = new EntityOperationResult();

            SqlCommand cmd = message.GenerateUpdateCommand();

            DBHelper helper = new DBHelper();

            try
            {
                helper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }
            return result;
        }


        #endregion

        #region 查
        /// <summary>
        /// 根据消息ID获取消息
        /// </summary>
        /// <param name="messageID">消息ID</param>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        public EntityMessageContent GetMessageContentByMsgID(string messageID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            EntityMessageContent entity = null;

            string sql_content = string.Empty;

            if (bGetDeleted)
            {
                sql_content = @"select * from msg_content where msg_id = @msg_id ";
            }
            else
            {
                sql_content = @"select * from msg_content where msg_id = @msg_id and msg_del_flag = 0";
            }

            if (bGetExpired)
            {

            }
            else
            {
                sql_content += @" and (msg_expiretime is null or getdate() <= msg_expiretime)";
            }


            SqlCommand cmdMsgContent = new SqlCommand(sql_content);
            cmdMsgContent.Parameters.AddWithValue("msg_id", messageID);

            DBHelper helper = new DBHelper();

            DataTable tableContent = helper.GetTable(cmdMsgContent);

            if (tableContent.Rows.Count > 0)//找不到制定的消息
            {
                entity = EntityMessageContent.FromDataRow(tableContent.Rows[0]);

                if (bGetReceiver)//获取接收者
                {
                    entity.ListMessageReceiver = GetMessageReceiverByMessageID(messageID);
                }
            }

            return entity;
        }

        /// <summary>
        /// 根据消息类别获取消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        public List<EntityMessageContent> GetMessageByType(EnumMessageType messageType, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            List<EntityMessageContent> ret = new List<EntityMessageContent>();

            string sql_content = string.Empty;

            if (bGetDeleted)
            {
                sql_content = @"select * from msg_content where msg_type = @msg_type ";
            }
            else
            {
                sql_content = @"select * from msg_content where msg_type = @msg_type and msg_del_flag = 0";
            }

            if (bGetExpired)
            {

            }
            else
            {
                sql_content += @" and (msg_expiretime is null or getdate() <= msg_expiretime)";
            }


            SqlCommand cmdMsgContent = new SqlCommand(sql_content);
            cmdMsgContent.Parameters.AddWithValue("msg_type", (int)messageType);

            DBHelper helper = new DBHelper();

            DataTable tableContent = helper.GetTable(cmdMsgContent);

            foreach (DataRow row in tableContent.Rows)
            {
                EntityMessageContent entity = EntityMessageContent.FromDataRow(tableContent.Rows[0]);

                if (bGetReceiver)//获取接收者
                {
                    entity.ListMessageReceiver = GetMessageReceiverByMessageID(entity.MessageID);

                }

                ret.Add(entity);
            }

            return ret;
        }



        /// <summary>
        /// 获取LED消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        public List<EntityMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired)
        {
            List<EntityMessageContent> ret;// = new List<EntityMessageContent>();

            string sql_content = string.Empty;

            if (bGetDeleted)
            {
                sql_content = string.Format(@"select * from msg_content where msg_type in ({0},{1},{2})", (int)EnumMessageType.LED1, (int)EnumMessageType.LED2, (int)EnumMessageType.LED3);
            }
            else
            {
                sql_content = string.Format(@"select * from msg_content where msg_type in ({0},{1},{2}) and msg_del_flag = 0", (int)EnumMessageType.LED1, (int)EnumMessageType.LED2, (int)EnumMessageType.LED3);
                //sql_content = @"select * from msg_content where msg_type = @msg_type and msg_del_flag = 0";
            }

            if (bGetExpired)
            {

            }
            else
            {
                sql_content += @" and (msg_expiretime is null or getdate() <= msg_expiretime)";
            }

            sql_content += " order by msg_create_time desc";

            SqlCommand cmdMsgContent = new SqlCommand(sql_content);
            //cmdMsgContent.Parameters.AddWithValue("msg_type", (int)messageType);

            DBHelper helper = new DBHelper();

            DataTable tableContent = helper.GetTable(cmdMsgContent);

            ret = EntityMessageContent.FromDataTable(tableContent);

            return ret;

            //foreach (DataRow row in tableContent.Rows)
            //{
            //    EntityMessageContent entity = EntityMessageContent.FromDataRow(tableContent.Rows[0]);

            //    if (bGetReceiver)//获取接收者
            //    {
            //        entity.ListMessageReceiver = GetMessageReceiverByMessageID(entity.MessageID);

            //    }

            //    ret.Add(entity);
            //}

            //return ret;
        }


        /// <summary>
        /// 根据接收者ID获取消息
        /// </summary>
        /// <param name="receiverID">接收者ID,如果为null则获取所有</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetReaded">是否获取已阅消息</param>
        /// <param name="bGetExpired">是否获取已超时消息</param>
        /// <returns></returns>
        public MessageReceiverCollection GetMessageByReceiverID(string receiverID, EnumMessageReceiverType receiverType, bool bGetDeleted, bool bGetReaded, bool bGetExpired)
        {
            string sql_content = @"
select
	msg_content.*,
	msg_receiver.*
from msg_receiver
inner join msg_content on msg_content.msg_id = msg_receiver.receiver_msg_id
where (msg_content.msg_del_flag = 0 or msg_content.msg_del_flag is null) and receiver_type = @receiver_type
";

            if (receiverID != null)
            {
                sql_content += " and rtrim(msg_receiver.receiver_id) = @receiver_id ";
            }

            if (!bGetDeleted)
            {
                sql_content += " and msg_receiver.receiver_del_flag = 0";//接收者未删除
            }

            if (!bGetReaded)
            {
                sql_content += " and msg_receiver.receiver_read_time is null";//接收者未阅读
            }

            if (!bGetExpired)
            {
                sql_content += " and msg_create_time>getdate()-7 and (msg_content.msg_expiretime is null or getdate() <= msg_content.msg_expiretime)";
            }

            SqlCommand cmdMessage = new SqlCommand(sql_content);

            if (receiverID != null)
            {
                cmdMessage.Parameters.AddWithValue("receiver_id", receiverID);
            }

            cmdMessage.Parameters.AddWithValue("receiver_type", (int)receiverType);

            DBHelper helper = new DBHelper();

            DataTable tableMessage = helper.GetTable(cmdMessage);

            MessageReceiverCollection list = new MessageReceiverCollection();
            foreach (DataRow row in tableMessage.Rows)
            {
                EntityMessageReceiver entityRec = EntityMessageReceiver.FromDataRow(row);
                EntityMessageContent entityContent = EntityMessageContent.FromDataRow(row);

                entityRec.MessageContent = entityContent;
                list.Add(entityRec);
            }

            return list;
        }

        /// <summary>
        /// 根据病人信息来获取危急值处理信息
        /// </summary>
        /// <param name="pat_id">病人ID</param>
        /// <returns></returns>
        public DataTable GetUrgentflagAndPatlookcodeByPatid(string pat_id)
        {
            string sql_content = "select pat_urgent_flag,pat_look_code,pat_look_date from patients where pat_id = @pat_id";

            SqlCommand cmdInfo = new SqlCommand(sql_content);

            if (pat_id != null)
            {
                cmdInfo.Parameters.AddWithValue("pat_id", pat_id);
            }

            DBHelper helper = new DBHelper();
            DataTable dtInfo = helper.GetTable(cmdInfo);
            dtInfo.TableName = "UrgentInfo";
            return dtInfo;
           
        }

        /// <summary>
        /// 根据消息ID获取消息接收者
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public MessageReceiverCollection GetMessageReceiverByMessageID(string messageID)
        {
            DBHelper helper = new DBHelper();

            string sql_receiver = @"select * from msg_receiver where msg_id = @msg_id";
            SqlCommand cmdMsgReceiver = new SqlCommand(sql_receiver);
            cmdMsgReceiver.Parameters.AddWithValue("msg_id", messageID);

            DataTable tableReceiver = helper.GetTable(cmdMsgReceiver);

            MessageReceiverCollection ret = EntityMessageReceiver.FromDataTable(tableReceiver);

            return ret;
        }



        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已超时</param>
        /// <returns></returns>
        public List<EntityMessageContent> GetAllMessage(bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            string sql = string.Empty;

            if (bGetDeleted)
            {
                sql = @"select * from msg_content where 1=1 ";
            }
            else
            {
                sql = @"select * from msg_content where msg_del_flag = 0";
            }

            if (bGetExpired)
            {

            }
            else
            {
                sql += @" and (msg_expiretime is null or getdate() <= msg_expiretime)";
            }

            SqlCommand cmd = new SqlCommand(sql);

            DBHelper helper = new DBHelper();
            DataTable table = helper.GetTable(cmd);

            List<EntityMessageContent> ret = new List<EntityMessageContent>();
            foreach (DataRow row in table.Rows)
            {
                EntityMessageContent entity = EntityMessageContent.FromDataRow(row);

                if (bGetReceiver)
                {
                    entity.ListMessageReceiver = this.GetMessageReceiverByMessageID(entity.MessageID);
                }

                ret.Add(entity);
            }
            return ret;
        }

        /// <summary>
        /// 生成消息接收者
        /// </summary>
        /// <param name="entityMessage"></param>
        /// <returns></returns>
        private MessageReceiverCollection GenerateMessageReceiver(EntityMessageContent entityMessage)
        {
            MessageReceiverCollection list = new MessageReceiverCollection();

            foreach (var item in entityMessage.ListMessageReceiver)
            {
                if (item.ReceiverType == EnumMessageReceiverType.Role)//如果接收者为角色，则转换为用户
                {
                    string role_id = item.ReceiverID;

                    DataTable tableUser = DictUser.NewInstance.GetRoleUsers(role_id);

                    foreach (DataRow rowUser in tableUser.Rows)
                    {
                        EntityMessageReceiver entityReceiver = new EntityMessageReceiver();
                        entityReceiver.MessageID = entityMessage.MessageID;
                        entityReceiver.ReceiverType = EnumMessageReceiverType.User;
                        entityReceiver.ReceiverID = rowUser["userInfoId"].ToString();
                        entityReceiver.ReceiverName = rowUser["userName"].ToString();

                        if (!list.Exists(i => i.ReceiverID == entityReceiver.ReceiverID && i.ReceiverType == entityReceiver.ReceiverType))
                        {
                            list.Add(entityReceiver);
                        }
                    }
                }
                else
                {
                    item.MessageID = entityMessage.MessageID;
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 根据消息发送者ID获取消息
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="bGetReceiver"></param>
        /// <param name="bGetDeleted"></param>
        /// <param name="bGetExpired"></param>
        /// <returns></returns>
        public List<EntityMessageContent> GetMessageBySenderID(string senderID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            List<EntityMessageContent> ret = new List<EntityMessageContent>();

            string sql_content = string.Empty;

            if (bGetDeleted)
            {
                sql_content = @"select * from msg_content where msg_sender_id = @msg_sender_id ";
            }
            else
            {
                sql_content = @"select * from msg_content where msg_sender_id = @msg_sender_id and msg_del_flag = 0";
            }

            if (bGetExpired)
            {

            }
            else
            {
                sql_content += @" and (msg_expiretime is null or getdate() <= msg_expiretime)";
            }

            SqlCommand cmdMsgContent = new SqlCommand(sql_content);
            cmdMsgContent.Parameters.AddWithValue("msg_sender_id", senderID);

            DBHelper helper = new DBHelper();

            DataTable tableContent = helper.GetTable(cmdMsgContent);

            foreach (DataRow row in tableContent.Rows)
            {
                EntityMessageContent entity = EntityMessageContent.FromDataRow(row);

                if (bGetReceiver)//获取接收者
                {
                    entity.ListMessageReceiver = GetMessageReceiverByMessageID(entity.MessageID);
                }

                ret.Add(entity);
            }

            return ret;
        }

        /// <summary>
        /// 获取用户与角色
        /// </summary>
        /// <returns></returns>
        public DataSet GetRoleAndUser()
        {
            DataSet result = new DataSet();

            DBHelper helper = new DBHelper();

            //DataTable dtPowerUserInfo = helper.GetTable(String.Format(@"select type_id  userId,type_id,type_name,-1 userInfoId,'物理组: '+ type_name userName from dict_type where type_flag=1 and type_del=0 union select type_id+'^'+poweruserinfo.userInfoId  userId,dict_type.type_id,dict_type.type_name,poweruserinfo.userInfoId,poweruserinfo.userName from poweruserinfo join powerusertype on poweruserinfo.userinfoid = powerusertype.userinfoid join dict_type on powerusertype.typeSourceId = dict_type.type_id where type_flag=1 and type_del=0  order by dict_type.type_name,poweruserinfo.userName"));

            DataTable dtPowerUserInfo = helper.GetTable(@"
select
type_id  userId,
type_id,
type_name,
'-1' userInfoId,
'物理组: '+ type_name userName 
from dict_type where type_flag=1 and type_del=0 
union 
select type_id+'^'+poweruserinfo.loginId  userId,
dict_type.type_id,
dict_type.type_name,
poweruserinfo.loginId userInfoId,
poweruserinfo.userName 
from poweruserinfo 
join powerusertype on poweruserinfo.userinfoid = powerusertype.userinfoid 
join dict_type on powerusertype.typeSourceId = dict_type.type_id where type_flag=1 and type_del=0
order by dict_type.type_name,poweruserinfo.userName 
");

            dtPowerUserInfo.TableName = "PowerUserInfo";
            result.Tables.Add(dtPowerUserInfo);

            DataTable dtPowerRoleUser = helper.GetTable(String.Format(@"select cast(roleInfoId as varchar)  userId, cast(roleInfoId as varchar) roleInfoId,roleName,-1 userInfoId,'角色: '+ roleName userName from powerroleinfo union select cast(powerroleinfo.roleInfoId as varchar) +'^'+ poweruserinfo.userInfoId userId,cast(powerroleinfo.roleInfoId as varchar) roleInfoId,roleName,poweruserinfo.userInfoId,userName from powerroleinfo join poweruserrole on powerroleinfo.roleInfoId =poweruserrole.roleInfoId join poweruserinfo on poweruserrole.userInfoId =poweruserinfo.userInfoId"));
            dtPowerRoleUser.TableName = "PowerRoleUser";
            result.Tables.Add(dtPowerRoleUser);

            DataTable dtPowerUserDepart = helper.GetTable(String.Format(@"select dep_id userId,dep_id,dep_name, -1 userInfoId,'科室：'+dep_name userName  from dict_depart union select dep_id +'^'+ poweruserinfo.userInfoId userId,dep_id,dep_name,poweruserinfo.userInfoId,userName from dict_depart join poweruserdepart on dict_depart.dep_id =poweruserdepart.departId join poweruserinfo on poweruserdepart.userInfoId =poweruserinfo.userInfoId"));
            dtPowerUserDepart.TableName = "PowerUserDepart";
            result.Tables.Add(dtPowerUserDepart);

            return result;
        }


        /// <summary>
        /// 获取科室信息
        /// </summary>
        /// <returns></returns>
        public DataTable m_dtbGetDeptInfo()
        {
            string strSQL = @"SELECT

	dep_name,
	dep_code

FROM
	dict_depart 
	WHERE dep_del=0";

            DBHelper objHelper = new DBHelper();

            DataTable dtbResult = objHelper.GetTable(strSQL);
            dtbResult.TableName = "dict_depart";

            return dtbResult;
        }

        /// <summary>
        /// 获取仪器质控信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetItrQcMessage(string strWhere)
        {
            try
            {
                string days = CacheSysConfig.Current.GetSystemConfig("QC_ItrQcMessageQueryDays");
                int day;
                if (string.IsNullOrEmpty(days) || !int.TryParse(days, out day))
                {
                    days = "90";
                }
                string sqlGetItrQcMsg = string.Format(@"
select qc_rule_mes.*,dict_item.itm_ecd,dict_instrmt.itr_mid,dict_type.type_name 
from qc_rule_mes WITH (NOLOCK)
left join dict_item on qc_rule_mes.qrm_item_id=dict_item.itm_id
left join dict_instrmt on qc_rule_mes.qcm_itr_id=dict_instrmt.itr_id
left join dict_type on dict_instrmt.itr_type=dict_type.type_id
where qrm_start_time>(getdate()-{1}) and (qcm_type='失控' or qcm_type='新增')
{0}
", strWhere, days);

                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                Lib.DAC.DbCommandEx cmd = helper.CreateCommandEx(sqlGetItrQcMsg);

                DataTable dt = helper.GetTable(cmd);
                if (dt != null) { dt.TableName = "dtItrQcMsg"; }

                return dt;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取仪器质控信息", ex);
            }

            return null;
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="p_strUserId">用户名</param>
        /// <param name="p_strPwd">密码</param>
        /// <returns>true为验证通过</returns>
        public DataSet m_blnCheckPassWord(DataSet ds)
        {
            //加密
            string p_strPwd = dcl.common.EncryptClass.Encrypt(ds.Tables[0].Rows[0]["pw"].ToString());
            string p_strUserId = ds.Tables[0].Rows[0]["userid"].ToString();
            string strSQL = string.Format(@"
SELECT 
	userInfoId,
	userName,
	loginId,
	password
	
FROM
	poweruserinfo
	WHERE del=0 AND  loginId ='{0}'
	 AND password='{1}'", p_strUserId, p_strPwd);
            DataSet dtbResult = null;
            try
            {
                DBHelper objHelper = new DBHelper();


                dtbResult = objHelper.GetDataSet(strSQL);
                dtbResult.Tables[0].TableName = "PowerUserInfo";
            }
            catch (Exception objEx)
            {

                throw;
            }


            return dtbResult;
        }

        /// <summary>
        /// 沙井HIS账号验证
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet SJHisCheckPassWord(DataSet ds)
        {
            DataSet result = new DataSet();
            OracleConnection conn = new OracleConnection("Data Source=HISRUN;user=bslis;password=bslis;");
            try
            {
                string sql = "";

                if (ds.Tables["Dtnhyy"] != null && ds.Tables["Dtnhyy"].Rows.Count > 0)//添加了南海oracle调用验证
                {
                    #region HIS提供的函数

                    /**
                 
create or replace function nhyy_login_chk(s1 varchar2,s2 varchar2) return varchar2 is
  Result varchar2(20);
  m varchar2(256);
  xm varchar2(20);
  d varchar2(50);
  c varchar2(256);
  s varchar2(256);
  i Integer;
  t Integer;
begin
  select p0193_3,p0193_4 into m,xm from hospital.p0193 where p0193_1=s1 and left(p0193_2,1)<>'*' and rownum<=1;
  d:= '67531824098463720159';
  c:= '';
  s:= '';
  for i in 1..len(m) loop
    c:=c||substr(d,to_number(Chr(ascii(substr(m,i,1))+32))+1,1);
  end loop;
  
  for i in 1..trunc(Len(c)/3) loop
    t:=to_number(substr(c,(i-1)*3+1,3));
    if t>255 then
      t:=0; 
    end if;
    s:=s||chr(t);
  end loop;
  if s2=s then
    Result:=xm;
  else
    Result:=null;
  end if;
    
  return(Result);
end nhyy_login_chk;


--插入参数是医生工号和密码，返回的是姓名

                ***/

                    #endregion

                    conn = new OracleConnection("Data Source=EHISSERVER;user=system;password=manager;");

                    sql = string.Format("select nhyy_login_chk('{0}','{1}') FROM dual", ds.Tables["Dtnhyy"].Rows[0]["userid"].ToString(), ds.Tables["Dtnhyy"].Rows[0]["pw"].ToString());
                }
                else if (ds.Tables["ZDLY"] != null && ds.Tables["ZDLY"].Rows.Count > 0)//添加了中大六院oracle调用验证
                {
                    conn = new OracleConnection("Data Source=zdlyhis;user=zdlylis;password=lis6yhis510655;");
                    string pass = Neusoft.HisCrypto.DESCryptoService.DESEncrypt(ds.Tables["ZDLY"].Rows[0]["pw"].ToString(), Neusoft.FrameWork.Management.Connection.DESKey);

                    sql = string.Format("select username,account from zdlyhis.view_lis_login where account='{0}' and password='{1}' ", ds.Tables["ZDLY"].Rows[0]["userid"], pass);
                }
                else
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["pw"].ToString()))
                    {
                        sql = string.Format("select yhdm, yhkl,yhmc from v_hszyh where   yhkl IS null and yhdm='{0}'", ds.Tables[0].Rows[0]["userid"].ToString());
                    }
                    else
                    {
                        sql = string.Format("select yhdm, yhkl,yhmc from v_hszyh where  yhdm='{0}' and yhkl='{1}'", ds.Tables[0].Rows[0]["userid"].ToString(), ds.Tables[0].Rows[0]["pw"].ToString());
                    }
                }

                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = sql;
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(result, "PowerUserInfo");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;


        }

        /// <summary>
        /// 获取危急值历史信息
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet GetUrgentHistoryMsg(DataSet ds)
        {
            DataSet dtbResult = null;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["sqlWhere"] != null)
            {
                string p_msg_type = ds.Tables["sqlWhere"].Rows[0]["msg_type"].ToString();//消息类型,默认1024,危急信息
                string p_time_start = ds.Tables["sqlWhere"].Rows[0]["create_time_start"].ToString();//开始时间
                string p_time_end = ds.Tables["sqlWhere"].Rows[0]["create_time_end"].ToString();//结束时间
                string p_receiver_id = ds.Tables["sqlWhere"].Rows[0]["receiver_id"].ToString();//科室ID


                string strWhere_receiver_id_In = "";
                if (!string.IsNullOrEmpty(p_receiver_id))//过滤科室(或病区)
                {
                    if (p_receiver_id.Contains(",") && p_receiver_id.Contains("'"))
                    {
                        strWhere_receiver_id_In = string.Format(@" and (msg_receiver.receiver_id in(select dep_code from dict_depart where dep_ward in
(select  dep_ward from dbo.dict_depart where dep_code in ({0}))) or msg_receiver.receiver_id in(
select  dep_ward from dbo.dict_depart where dep_code in ({0}) and dep_ward is not null  )) ", p_receiver_id);
                    }
                    else
                    {
                        strWhere_receiver_id_In = string.Format(@" and (msg_receiver.receiver_id in(select dep_code from dict_depart where dep_ward =
(select top 1 dep_ward from dbo.dict_depart where dep_code='{0}')) or msg_receiver.receiver_id in(
select  dep_ward from dbo.dict_depart where dep_code='{0}' and dep_ward is not null  )) ", p_receiver_id);
                    }
                }

                string strWhere_ori_id = "";//病人来源查询条件

                if (ds.Tables["sqlWhere"].Columns.Contains("is_neglect_dep"))
                {
                    string p_neg_dep = ds.Tables["sqlWhere"].Rows[0]["is_neglect_dep"].ToString();//忽略科室(或病区)

                    if ((!string.IsNullOrEmpty(p_neg_dep)) && p_neg_dep == "1")//1代表忽略
                    {
                        strWhere_receiver_id_In = "";
                    }
                }

                if (ds.Tables["sqlWhere"].Columns.Contains("pat_ori_config"))//是否有病人来源配置
                {
                    string p_ori_id = ds.Tables["sqlWhere"].Rows[0]["pat_ori_config"].ToString();//病人来源配置

                    if (!string.IsNullOrEmpty(p_ori_id))
                    {
                        //值为1111 分别代表 住院,门诊,体检,其他
                        char[] chr = p_ori_id.ToCharArray();

                        for (int i = 0; i < chr.Length; i++)
                        {
                            if (i == 0)
                            {
                                if (chr[i] == '0') strWhere_ori_id += " and patients.pat_ori_id<>'108' ";
                            }
                            if (i == 1)
                            {
                                if (chr[i] == '0') strWhere_ori_id += " and patients.pat_ori_id<>'107' ";
                            }
                            if (i == 2)
                            {
                                if (chr[i] == '0') strWhere_ori_id += " and patients.pat_ori_id<>'109' ";
                            }
                            if (i == 3)
                            {
                                if (chr[i] == '0') strWhere_ori_id += " and (patients.pat_ori_id='107' or patients.pat_ori_id='108' or patients.pat_ori_id='109') ";
                            }
                        }
                    }
                }

                string strWhere_doc_id = "";//医生代码查询条件

                if (ds.Tables["sqlWhere"].Columns.Contains("pat_doc_id"))//是否有医生代码
                {
                    string p_doc_id = ds.Tables["sqlWhere"].Rows[0]["pat_doc_id"].ToString();//医生代码
                    if (!string.IsNullOrEmpty(p_doc_id))
                    {
                        strWhere_doc_id = string.Format(" and patients.pat_doc_id=(select top 1 doc_id from dict_doctor as doc where doc.doc_code='{0}') ", p_doc_id);
                    }
                }

                //新查询出字段：审核人工号
                string strSQL = string.Format(@"select 
 patients.pat_id,
 patients.pat_in_no,
(case when isnull(dict_depart.dep_name,'')<>'' then dict_depart.dep_name 
when isnull(patients.pat_dep_name,'')<>'' then patients.pat_dep_name else '未知' end) as 'dep_name',--科室名称,
 patients.pat_bed_no ,--床号,
 patients.pat_name ,--姓名, 
patients.pat_doc_name as doc_name,--医生姓名,
(case patients.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,--性别,
 dbo.getAge(patients.pat_age_exp) pat_age,--年龄,
msg_ext3 as  pat_result,--危急值结果,
PowerUserInfo2.loginId pat_chk_number,--审核人工号
PowerUserInfo2.Username pat_chk_name,--审核人,
patients.pat_chk_date ,--一审时间,
patients.pat_report_date ,--二审时间,
dict_instrmt.itr_type, --物理组
patients.pat_tel,
patients.pat_bar_code,
msg_content.msg_ext4,
--PowerUserInfo1.Username pat_look_name,--确认人,
patients.pat_look_code pat_look_name,--确认人,
patients.pat_look_date ,--确认时间,
'' pat_datediff,--确认时间差,
dict_sample.sam_name,
(case when patients.pat_look_code is null then '未确认'  when msg_content.msg_affirm_type='2' then '检验科通知确认' else '临床确认' end) as 'affirm_mode',--确认方式,
patients_ext.msg_content pat_res,--备注
patients_ext.msg_content2 pat_res2--备注
from patients with(nolock)
      left join 
      dict_doctor on patients.pat_doc_id=dict_doctor.doc_id and dict_doctor.doc_del='0'
      left join
      PowerUserInfo PowerUserInfo1 on patients.pat_look_code=PowerUserInfo1.loginId
      left join
      PowerUserInfo PowerUserInfo2 on patients.pat_report_code=PowerUserInfo2.loginId
      left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
      left join dict_sample on dict_sample.sam_id=patients.pat_sam_id
      left join dict_depart on patients.pat_dep_id = dict_depart.dep_code and dict_depart.dep_del='0'
      left join msg_content on patients.pat_id=msg_content.msg_ext1
left join patients_ext with(nolock) on patients_ext.pat_id=msg_content.msg_ext1
where patients.pat_id in(
select msg_content.msg_ext1 from msg_content with(nolock)
left join  msg_receiver with(nolock) on msg_content.msg_id=msg_receiver.receiver_msg_id
where -- msg_content.msg_del_flag=1 and 
(msg_content.msg_type=3024 or msg_content.msg_type=2024 or msg_content.msg_type={0}) --类型
and msg_content.msg_create_time>='{1}' --日期
and msg_content.msg_create_time<'{2}' --日期
{3}
)
{4}
{5}
--and (patients.pat_dep_id is not null and patients.pat_dep_id<>'')
--order by patients.pat_chk_date", p_msg_type, p_time_start, p_time_end, strWhere_receiver_id_In, strWhere_ori_id, strWhere_doc_id);

                #region 自编危急值信息

                string strSQLDIY = string.Format(@"SELECT 
  msg_content.msg_id as pat_id,
 msg_content.pat_in_no,
 msg_receiver.receiver_name as 'dep_name',
 msg_content.pat_bed_no,
 msg_content.pat_name, 
 msg_content.msg_doc_name as 'doc_name',
 (case msg_content.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,
 msg_content.pat_age_str as pat_age,
 msg_content.msg_ext3 as  pat_result,
 '' as pat_chk_name,
 msg_content.msg_create_time as pat_chk_date,
 msg_content.msg_create_time as pat_report_date,
msg_content.msg_pat_tel as pat_tel,
 '' as pat_bar_code,
msg_content.msg_ext4,
msg_content.msg_checker_name as pat_look_name,--确认人,
msg_receiver.receiver_read_time  as pat_look_date,--确认时间,
'' pat_datediff,--确认时间差,
'' as sam_name,
(case when isnull(msg_content.msg_checker_id,'')='' then '未确认'  when msg_content.msg_affirm_type='2' then '检验科通知确认' else '临床确认' end) as 'affirm_mode',--确认方式,
patients_ext.msg_content pat_res--备注
FROM  msg_content with(nolock) INNER JOIN
msg_receiver with(nolock) ON msg_content.msg_id = msg_receiver.receiver_msg_id
left join dict_origin on dict_origin.ori_id=msg_content.pat_ori_id
left join patients_ext with(nolock) on patients_ext.pat_id=msg_content.msg_ext1
where msg_content.msg_type=2024 
and msg_content.msg_create_time>='{0}' --日期
and msg_content.msg_create_time<'{1}' --日期
{2}
", p_time_start, p_time_end, strWhere_receiver_id_In);

                #endregion

                try
                {
                    DBHelper objHelper = new DBHelper();
                    dtbResult = objHelper.GetDataSet(strSQL);
                    dtbResult.Tables[0].TableName = "UrgentHistory";

                    if (true)//自编危急
                    {
                        DataTable dtbResultDIY = objHelper.GetTable(strSQLDIY);
                        if (dtbResultDIY != null && dtbResultDIY.Rows.Count > 0)
                        {
                            dtbResult.Tables[0].Merge(dtbResultDIY);
                        }
                    }

                    //排序
                    if (dtbResult != null && dtbResult.Tables[0].Rows.Count > 0
                        && dtbResult.Tables[0].Columns.Contains("pat_chk_date"))
                    {
                        DataView dvTempSort = dtbResult.Tables[0].DefaultView.ToTable().DefaultView;
                        dvTempSort.Sort = "pat_chk_date desc";
                        dtbResult.Clear();
                        dtbResult.Tables.Clear();
                        dtbResult.AcceptChanges();
                        dtbResult.Tables.Add(dvTempSort.ToTable());
                        dtbResult.Tables[0].TableName = "UrgentHistory";
                    }

                    //if (dtbResult.Tables["UrgentHistory"] != null && dtbResult.Tables["UrgentHistory"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow drUH in dtbResult.Tables["UrgentHistory"].Rows)
                    //    {
                    //        drUH["pat_result"] = GetResult(drUH["pat_id"].ToString());
                    //    }
                    //}
                }
                catch (Exception objEx)
                {
                    Lib.LogManager.Logger.LogException("获取危急值历史信息", objEx);
                    throw;
                }
            }
            else if (ds != null && ds.Tables.Count > 0 && ds.Tables["getUrgent"] != null)
            {
                dtbResult = GetUrgentMsg(ds);//实时查看危急值信息,此方法暂时停用 13-1-31
            }
            else if (ds != null && ds.Tables.Count > 0 && ds.Tables["dict_bscripe"] != null)//获取临床危急值备注
            {
                string strSQL = "select * from dict_bscripe";

                try
                {
                    DBHelper objHelper = new DBHelper();
                    dtbResult = objHelper.GetDataSet(strSQL);
                    dtbResult.Tables[0].TableName = "dict_bscripe";
                }
                catch (Exception objEx)
                {
                    Lib.LogManager.Logger.LogException("获取dict_bscripe信息", objEx);
                    //throw;
                }
            }
            else if (ds != null && ds.Tables.Count > 0 && ds.Tables["getAll"] != null)//目前危急值内部提醒调用
            {
                int temp_fMinutes = 0;

                int temp_fMinutes2 = 0;

                if (ds.Tables["getAll"].Rows.Count > 0 && ds.Tables["getAll"].Columns.Contains("filterTime"))
                {
                    if (int.TryParse(ds.Tables["getAll"].Rows[0]["filterTime"].ToString(), out temp_fMinutes))
                    {
                        // temp_fMinutes = temp_fMinutes > 0 ? temp_fMinutes : -temp_fMinutes;
                    }
                }

                if (ds.Tables["getAll"].Rows.Count > 0 && ds.Tables["getAll"].Columns.Contains("filterTime2"))
                {
                    if (int.TryParse(ds.Tables["getAll"].Rows[0]["filterTime2"].ToString(), out temp_fMinutes2))
                    {
                        //temp_fMinutes2 = temp_fMinutes2 > 0 ? temp_fMinutes2 : -temp_fMinutes2;
                    }
                }

                bool isDIYCritical = false;//是否只接收自编危急信息 2024

                if (ds.Tables["getAll"].Rows.Count > 0 && ds.Tables["getAll"].Columns.Contains("isOnlyDIY"))
                {
                    if (ds.Tables["getAll"].Rows[0]["isOnlyDIY"].ToString() == "1")
                    {
                        isDIYCritical = true;
                    }
                }

                string filterTime = DateTime.Now.AddMinutes(-temp_fMinutes).ToString();//获取推迟多少分钟的危急值信息

                // string strWhere = string.Format(" msg_create_time<='{0}' ", filterTime);


                string filterTime2 = DateTime.Now.AddMinutes(-temp_fMinutes2).ToString();//获取推迟多少分钟的急查信息

                string strWhere = string.Empty;
                if (temp_fMinutes > 0)
                {
                    if (isDIYCritical)
                    {
                        strWhere = string.Format(" ((msg_type=2024) and msg_create_time<='{0}') ", filterTime);
                    }
                    else
                    {
                        strWhere = string.Format(" ((msg_type=1024 or msg_type=3024) and msg_create_time<='{0}') ", filterTime);
                    }
                }
                if (temp_fMinutes2 > 0)
                {
                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += " or ";
                    }
                    strWhere += string.Format("(msg_type=4096 and msg_create_time<='{0}')", filterTime2);
                }
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strWhere = string.Format("({0})", strWhere);
                }
                else
                {
                    strWhere = "msg_type=0";
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    strWhere += " and (pat_dep_id is not null and pat_dep_id<>'') ";
                }
                else
                {
                    strWhere = " (pat_dep_id is not null and pat_dep_id<>'') ";
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    strWhere += " and (msf_insgin_flag is null or msf_insgin_flag='' or msf_insgin_flag<>'1') ";
                }
                else
                {
                    strWhere = " (msf_insgin_flag is null or msf_insgin_flag='' or msf_insgin_flag<>'1') ";
                }


                if (ds.Tables["getAll"].Rows.Count > 0 && ds.Tables["getAll"].Columns.Contains("DepIDs")
                    && !string.IsNullOrEmpty(ds.Tables["getAll"].Rows[0]["DepIDs"].ToString()))
                {
                    string depids = ds.Tables["getAll"].Rows[0]["DepIDs"].ToString();
                    string DepStr = "";

                    string[] darry = depids.Split(',');



                    foreach (string drDep in darry)
                    {
                        if (string.IsNullOrEmpty(DepStr))
                        {
                            DepStr = "'" + drDep + "'";
                        }
                        else
                        {
                            DepStr += ",'" + drDep + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += string.Format(" and (msg_send_depcode in ({0})) ", DepStr);
                    }
                    else
                    {
                        strWhere = string.Format(" and (msg_send_depcode in ({0})) ", DepStr);
                    }
                }

                //string strWhere = string.Format(" ((msg_type=1024 and msg_create_time<='{0}') or (msg_type=4096 and msg_create_time<='{1}'))", filterTime, filterTime2);
                DataTable dtUrgAll = UrgentMessageCache.Current.GetDTUrgentMessage(strWhere);
                if (dtUrgAll != null && dtUrgAll.Rows.Count > 0)
                {
                    dtUrgAll.TableName = "UrgentMsg";
                    dtbResult = new DataSet();
                    dtbResult.Tables.Add(dtUrgAll.Copy());
                }
            }
            else if (ds != null && ds.Tables.Count > 0 && ds.Tables["getCacheByDep"] != null)//目前新住院危急值调用
            {
                string p_receiver_id = ds.Tables["getCacheByDep"].Rows[0]["receiver_id"].ToString();//科室ID

                //获取一个病区的代码
                p_receiver_id = GetDepInwhereStr(p_receiver_id);

                if (string.IsNullOrEmpty(p_receiver_id))//如果为空,转空字符串
                    p_receiver_id = "''";

                string strWhere = string.Format(" pat_dep_id in ({0}) ", p_receiver_id);//筛选条件

                if (ds.Tables["getCacheByDep"].Columns.Contains("is_neglect_dep"))//是否忽略科室(或病区)
                {
                    string p_neg_dep = ds.Tables["getCacheByDep"].Rows[0]["is_neglect_dep"].ToString();//忽略科室(或病区)

                    if ((!string.IsNullOrEmpty(p_neg_dep)) && p_neg_dep == "1")//1代表忽略
                    {
                        strWhere = " 1=1 and (pat_dep_id is not null and pat_dep_id<>'') ";
                    }
                }

                if (ds.Tables["getCacheByDep"].Columns.Contains("pat_doc_id"))//医生代码
                {
                    string p_doc_code = ds.Tables["getCacheByDep"].Rows[0]["pat_doc_id"].ToString();//医生代码

                    if ((!string.IsNullOrEmpty(p_doc_code)))
                    {
                        strWhere += string.Format(" and pat_doc_id='{0}' ", p_doc_code);
                    }
                }

                if (ds.Tables["getCacheByDep"].Columns.Contains("pat_ori_config"))//是否有病人来源配置
                {
                    string p_ori_id = ds.Tables["getCacheByDep"].Rows[0]["pat_ori_config"].ToString();//病人来源配置

                    if (!string.IsNullOrEmpty(p_ori_id))
                    {
                        //值为1111 分别代表 住院,门诊,体检,其他
                        char[] chr = p_ori_id.ToCharArray();

                        for (int i = 0; i < chr.Length; i++)
                        {
                            if (i == 0)
                            {
                                if (chr[i] == '0') strWhere += " and pat_ori_id<>'108' ";
                            }
                            if (i == 1)
                            {
                                if (chr[i] == '0') strWhere += " and pat_ori_id<>'107' ";
                            }
                            if (i == 2)
                            {
                                if (chr[i] == '0') strWhere += " and pat_ori_id<>'109' ";
                            }
                            if (i == 3)
                            {
                                if (chr[i] == '0') strWhere += " and (pat_ori_id='107' or pat_ori_id='108' or pat_ori_id='109') ";
                            }
                        }
                    }
                }

                if (ds.Tables["getCacheByDep"].Columns.Contains("msg_type"))//消息类型
                {
                    string msg_type = ds.Tables["getCacheByDep"].Rows[0]["msg_type"].ToString();

                    if ((!string.IsNullOrEmpty(msg_type)))
                    {
                        strWhere += string.Format(" and msg_type={0} ", msg_type);
                    }
                    else
                    {
                        strWhere += string.Format(" and msg_type<>3024 ");
                    }
                }


                DataTable dtUrgAll = UrgentMessageCache.Current.GetDTUrgentMessage(strWhere);
                if (dtUrgAll != null && dtUrgAll.Rows.Count > 0)
                {
                    dtUrgAll.TableName = "UrgentMsg";
                    dtbResult = new DataSet();
                    dtbResult.Tables.Add(dtUrgAll.Copy());
                }
            }

            return dtbResult;

        }

        /// <summary>
        /// 获取一个病区的科室代码
        /// </summary>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        private string GetDepInwhereStr(string dept_code)
        {
            string DepStr = "";

            if (dept_code == null) return DepStr;  //空科室代码,返回空    

            try
            {
                DBHelper db = new DBHelper();

                string sql = "";

                if (!string.IsNullOrEmpty(dept_code) && dept_code.Contains(",") && dept_code.Contains("'"))
                {
                    sql = string.Format(@"select dep_code from dict_depart where dep_ward in
(select dep_ward from dbo.dict_depart where dep_code in({0}) )", dept_code);
                }
                else
                {
                    sql = string.Format(@"select dep_code from dict_depart where dep_ward =
(select top 1 dep_ward from dbo.dict_depart where dep_code='{0}' )", dept_code);
                }

                DataTable dtDep = db.GetTable(sql);

                if (dtDep != null && dtDep.Rows.Count > 0)
                {
                    foreach (DataRow drDep in dtDep.Rows)
                    {
                        if (string.IsNullOrEmpty(DepStr))
                        {
                            DepStr = "'" + drDep["dep_code"].ToString() + "'";
                        }
                        else
                        {
                            DepStr += ",'" + drDep["dep_code"].ToString() + "'";
                        }
                    }
                }
                else
                {
                    DepStr = string.Format("'{0}'", dept_code);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取病区信息", ex);
            }
            return DepStr;
        }

        /// <summary>
        /// 获取危急值信息(实时获取)
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet GetUrgentMsg(DataSet ds)
        {
            DataSet dtbResult = null;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["getUrgent"] != null)
            {
                string p_receiver_id = ds.Tables["getUrgent"].Rows[0]["receiver_id"].ToString();//科室ID

                string strSQL = string.Format(@"select 
 patients.pat_id,
msg_content.msg_id,
msg_content.msg_type,
 patients.pat_in_no,
(case when isnull(dict_depart.dep_name,'')<>'' then dict_depart.dep_name 
when isnull(patients.pat_dep_name,'')<>'' then patients.pat_dep_name else '未知' end) as 'dep_name',--科室名称,
 patients.pat_bed_no ,--床号,
 patients.pat_name ,--姓名, 
 --dict_doctor.doc_name ,--医生姓名,
patients.pat_doc_name as doc_name,--医生姓名,
(case patients.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,--性别,
 dbo.getAge(patients.pat_age_exp) pat_age,--年龄,
patients.pat_c_name,--组合
patients.pat_tel,
patients.pat_bar_code,
msg_ext3 as pat_result,--危急值结果,
PowerUserInfo2.Username pat_chk_name,--审核人,
patients.pat_chk_date ,--一审时间,
patients.pat_report_date ,--二审时间,
--PowerUserInfo1.Username pat_look_name,--确认人,
patients.pat_look_code pat_look_name,--确认人,
patients.pat_look_date ,--确认时间,
msg_content.msg_ext4,
'' pat_datediff,--确认时间差,
'' affirm_mode,--确认方式,
'' pat_res--备注
from patients with(nolock)
      left join 
      dict_doctor on patients.pat_doc_id=dict_doctor.doc_code and dict_doctor.doc_del='0'
      left join
      PowerUserInfo PowerUserInfo1 on patients.pat_look_code=PowerUserInfo1.loginId
      left join
      PowerUserInfo PowerUserInfo2 on patients.pat_report_code=PowerUserInfo2.loginId
      left join dict_depart on patients.pat_dep_id=dict_depart.dep_code and dict_depart.dep_del='0'
      left join 
      msg_content on patients.pat_id=msg_content.msg_ext1
where 
pat_flag in ('2','4') and
pat_id in(
select msg_content.msg_ext1 from msg_content
left join  msg_receiver on msg_content.msg_id=msg_receiver.receiver_msg_id
where msg_content.msg_del_flag=0
and msg_receiver.receiver_del_flag=0
and msg_content.msg_type in(1024,4096) --类型
--and msg_content.msg_create_time>='2012-01-01 0:00:00' --日期
--and msg_content.msg_create_time<'2013-01-25 0:00:00' --日期
and msg_receiver.receiver_id in(select dep_code from dict_depart where dep_ward =
(select top 1 dep_ward from dbo.dict_depart where dep_code='{0}')) 
)
order by patients.pat_chk_date", p_receiver_id);

                try
                {
                    DBHelper objHelper = new DBHelper();
                    dtbResult = objHelper.GetDataSet(strSQL);
                    dtbResult.Tables[0].TableName = "UrgentMsg";

                    //if (dtbResult.Tables["UrgentMsg"] != null && dtbResult.Tables["UrgentMsg"].Rows.Count > 0)
                    //{
                    //    foreach (DataRow drUH in dtbResult.Tables["UrgentMsg"].Rows)
                    //    {
                    //        drUH["pat_result"] = GetResult(drUH["pat_id"].ToString());
                    //    }
                    //}
                }
                catch (Exception objEx)
                {
                    Lib.LogManager.Logger.LogException("实时获取危急值信息", objEx);
                    //throw;
                }
            }

            return dtbResult;


        }
        /// <summary>
        /// 获取所有未确认的危机值信息(默认只取最近五天数据)
        /// </summary>
        /// <returns></returns>
        public DataTable GetUrgentMsgToCache()
        {
            bool UrgentMessage_ShowPrintedRepMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("UrgentMessage_ShowPrintedRepMsg") != "否";

            //系统配置：允许服务端缓存危急值信息
            bool UrgentMessage_AllowWCFCache = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_AllowWCFCache") != "否";

            if (!UrgentMessage_AllowWCFCache)
            {
                //如果不缓存,则返回空
                return null;
            }

            string pat_flag_sql = string.Empty;
            if (UrgentMessage_ShowPrintedRepMsg)
            {
                pat_flag_sql = "2,4";
            }
            else
            {
                pat_flag_sql = "2";
            }
            DataTable dtbResult = null;

            #region 危急与急查信息
            string strSQL = string.Format(@"select 
 patients.pat_id,
msg_content.msg_id,
msg_content.msg_type,
msg_content.msf_insgin_flag,
(case when msg_content.msg_type=1024 and msg_content.msg_ext4='传染病' then '传染病' when msg_content.msg_type=1024 or msg_content.msg_type=3024 then '危急' when msg_content.msg_type=4096 and msg_content.msg_ext4='多重耐药菌' then '多重耐药' when msg_content.msg_type=4096 then '急查' else '' end) as msg_type_txt,
msg_content.msg_create_time,
(case when isnull(patients.pat_dep_id,'')='' then patients.pat_ward_id else patients.pat_dep_id end) as 'pat_dep_id',
 patients.pat_in_no,
patients.pat_itr_id,
(case when isnull(dict_depart.dep_name,'')<>'' then dict_depart.dep_name 
when isnull(patients.pat_dep_name,'')<>'' then patients.pat_dep_name else '未知' end) as 'dep_name',--科室名称,
 patients.pat_bed_no ,--床号,
 patients.pat_name ,--姓名, 
(case when isnull(dict_doctor.doc_name,'')<>'' then dict_doctor.doc_name else patients.pat_doc_name end) as 'doc_name',--医生姓名,
dict_doctor.doc_code as pat_doc_id,
(case patients.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,--性别,
 dbo.getAge(patients.pat_age_exp) pat_age,--年龄,
patients.pat_c_name,--组合
patients.pat_ori_id,--病人来源
dict_doctor.doc_tel,
patients.pat_tel,
patients.pat_bar_code,
msg_content.msg_ext3 as  pat_result,--危急值结果,
PowerUserInfo2.Username pat_chk_name,--审核人,
patients.pat_chk_date ,--一审时间,
patients.pat_report_date ,--二审时间,
dict_instrmt.itr_type, --物理组
getdate() as 'server_date',--服务器时间
cast(0 as bit) as 'pat_select',
dict_instrmt.itr_name,
dict_instrmt.itr_rep_flag,
isnull(dict_instrmt.itr_rep_id,'') itr_rep_id,
patients.pat_sid,
patients.pat_host_order,
dict_sample.sam_name,
dict_depart.dep_tel,
msg_content.msg_ext4,
msg_content.msg_send_depcode
from patients with(nolock)
      left join 
      dict_doctor on patients.pat_doc_id=dict_doctor.doc_id and dict_doctor.doc_del='0'
      left join
      PowerUserInfo PowerUserInfo2 on patients.pat_report_code=PowerUserInfo2.loginId
      left join dict_depart on patients.pat_dep_id = dict_depart.dep_code and dict_depart.dep_del='0'
      left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
      left join dict_sample on dict_sample.sam_id=patients.pat_sam_id
      left join 
      msg_content with(nolock) on patients.pat_id=msg_content.msg_ext1
where 
msg_content.msg_del_flag=0 and
pat_flag in ({0}) and
pat_id in(
select msg_content.msg_ext1 from msg_content with(nolock)
left join  msg_receiver with(nolock) on msg_content.msg_id=msg_receiver.receiver_msg_id
where msg_content.msg_del_flag=0
and msg_receiver.receiver_del_flag=0
and msg_content.msg_type in(1024,4096,3024) --类型
and msg_content.msg_create_time>=Dateadd(day,-5,getdate()) --日期

)
and msg_content.msg_type in(1024,4096,3024) 
--order by patients.pat_chk_date,msg_content.msg_type desc", pat_flag_sql);
            #endregion

            #region 内部提醒危急值(msg_content无数据时,来源为住院)

            string strSQLInner = @"select 
 patients.pat_id,
'-1$' as msg_id,
1024 as msg_type,
cast(null as char(1)) as msf_insgin_flag,
'危急' as msg_type_txt,
patients.pat_report_date as msg_create_time,
(case when isnull(patients.pat_dep_id,'')='' then patients.pat_ward_id else patients.pat_dep_id end) as 'pat_dep_id',
 patients.pat_in_no,
patients.pat_itr_id,
(case when isnull(dict_depart.dep_name,'')<>'' then dict_depart.dep_name 
when isnull(patients.pat_dep_name,'')<>'' then patients.pat_dep_name else '未知' end) as 'dep_name',--科室名称,
 patients.pat_bed_no ,--床号,
 patients.pat_name ,--姓名, 
(case when isnull(dict_doctor.doc_name,'')<>'' then dict_doctor.doc_name else patients.pat_doc_name end) as 'doc_name',--医生姓名,
dict_doctor.doc_code as pat_doc_id,
(case patients.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,--性别,
 dbo.getAge(patients.pat_age_exp) pat_age,--年龄,
patients.pat_c_name,--组合
patients.pat_ori_id,--病人来源
dict_doctor.doc_tel,
patients.pat_tel,
patients.pat_bar_code,
'异常危急值信息' as  pat_result,--危急值结果,
PowerUserInfo2.Username pat_chk_name,--审核人,
patients.pat_chk_date ,--一审时间,
patients.pat_report_date ,--二审时间,
dict_instrmt.itr_type, --物理组
getdate() as 'server_date',--服务器时间
cast(0 as bit) as 'pat_select',
dict_instrmt.itr_name,
dict_instrmt.itr_rep_flag,
isnull(dict_instrmt.itr_rep_id,'') itr_rep_id,
patients.pat_sid,
patients.pat_host_order,
dict_sample.sam_name,
dict_depart.dep_tel,
cast(null as varchar(200)) as msg_ext4,
cast(null as varchar(12)) as msg_send_depcode,
Datediff(mi,pat_report_date,getdate()) as msg_add_time,
'' as msg_ext5,
'检验科' as msg_send_depname
from patients with(nolock)
      left join 
      dict_doctor on patients.pat_doc_id=dict_doctor.doc_id and dict_doctor.doc_del='0'
      left join
      PowerUserInfo PowerUserInfo2 on patients.pat_report_code=PowerUserInfo2.loginId
      left join dict_depart on patients.pat_dep_id = dict_depart.dep_code and dict_depart.dep_del='0'
      left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
      left join dict_sample on dict_sample.sam_id=patients.pat_sam_id
where 
patients.pat_report_date>=Dateadd(day,-1,getdate())
and pat_flag in (2,4)
and (patients.pat_urgent_flag=1 or patients.pat_urgent_flag=2)
and (patients.pat_look_code is null or patients.pat_look_code='')
and patients.pat_look_date is null
and exists(select top 1 1 from resulto with(nolock) where res_id=patients.pat_id)
and not exists(select top 1 1 from msg_content with(nolock) where msg_content.msg_ext1=patients.pat_id)
and not exists(select top 1 1 from patients_ext with(nolock) where patients_ext.pat_id=patients.pat_id and patients_ext.msg_doc_name is not null and patients_ext.msg_doc_name<>'')
and patients.pat_ori_id='108'
";

            #endregion

            #region 自编危急信息
            string strSQLDIY = @"SELECT 
  msg_content.msg_id as pat_id,
 msg_content.msg_id,
 msg_content.msg_type,
 msg_content.msf_insgin_flag,
 '自编危急' as msg_type_txt,
 msg_content.msg_create_time,
 msg_receiver.receiver_id as 'pat_dep_id', 
 msg_content.pat_in_no,
 '' as pat_itr_id,
 msg_receiver.receiver_name as 'dep_name',
 msg_content.pat_bed_no,
 msg_content.pat_name, 
 msg_content.msg_doc_name as 'doc_name',
 msg_content.msg_doc_code as 'pat_doc_id',
 (case msg_content.pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,
 msg_content.pat_age_str as pat_age,
 '' as pat_c_name,
 msg_content.pat_ori_id,
dict_doctor.doc_tel,
msg_content.msg_pat_tel as pat_tel,
 '' as pat_bar_code,
 msg_content.msg_ext3 as  pat_result,
 '' as pat_chk_name,
 msg_content.msg_create_time as pat_chk_date,
 msg_content.msg_create_time as pat_report_date,
 '' as itr_type,
 getdate() as 'server_date',--服务器时间
 cast(0 as bit) as 'pat_select',
 '' as itr_name,
'' as itr_rep_flag,
'' as itr_rep_id,
 '' as pat_sid,
 '' as pat_host_order,
'' as sam_name,
'' as dep_tel,
msg_content.msg_ext4,msg_content.msg_send_depcode
FROM  msg_content with(nolock) INNER JOIN
msg_receiver with(nolock) ON msg_content.msg_id = msg_receiver.receiver_msg_id
left join dict_origin on dict_origin.ori_id=msg_content.pat_ori_id
left join dict_doctor on msg_content.msg_doc_code=dict_doctor.doc_id and dict_doctor.doc_del='0'
where msg_content.msg_type=2024 and msg_content.msg_del_flag=0
and msg_receiver.receiver_del_flag=0
and msg_content.msg_create_time>=Dateadd(day,-5,getdate())";
            #endregion

            #region 回退标本信息
            string strReturn = @"SELECT 
 '' as pat_id,
 '' as msg_id,
 16 as msg_type,
 '' as msf_insgin_flag,
 '回退标本' as msg_type_txt,
 bc_return_messages.bc_time as msg_create_time,
 bc_return_messages.bc_d_code as 'pat_dep_id', 
 bc_patients.bc_in_no as pat_in_no,
 '' as pat_itr_id,
 bc_return_messages.bc_d_name as 'dep_name',
 bc_patients.bc_bed_no as pat_bed_no,
 bc_patients.bc_name as pat_name, 
 bc_patients.bc_doct_name as 'doc_name',
 bc_patients.bc_doct_code as 'pat_doc_id',
 bc_patients.bc_sex as pat_sex,
 bc_patients.bc_age as pat_age,
 bc_patients.bc_his_name as pat_c_name,
 bc_return_messages.bc_ori_id as pat_ori_id,
 dict_doctor.doc_tel,
 '' as pat_tel,
 bc_return_messages.bc_bar_code as pat_bar_code,
 bc_return_messages.bc_message as  pat_result,
 bc_return_messages.bc_sender_name as pat_chk_name,
 getdate() as pat_chk_date,
 getdate() as pat_report_date,
 '' as itr_type,
 getdate() as 'server_date',--服务器时间
 cast(0 as bit) as 'pat_select',
 '' as itr_name,
'' as itr_rep_flag,
'' as itr_rep_id,
 '' as pat_sid,
 '' as pat_host_order,
'' as sam_name,
'' as dep_tel,
'' as msg_ext4,bc_return_messages.bc_d_name as msg_send_depcode
FROM  bc_return_messages with(nolock) 
left join dict_origin on dict_origin.ori_id=bc_return_messages.bc_ori_id
left join dict_doctor on bc_return_messages.bc_sender_code=dict_doctor.doc_id and dict_doctor.doc_del='0'
left join bc_patients on bc_return_messages.bc_bar_code=bc_patients.bc_bar_code
where bc_return_messages.bc_time>=DATEADD(d,-300,GETDATE()) and bc_return_messages.bc_del = 0 and  bc_return_messages.bc_handle_flag = 0";
            #endregion

            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "UrgentMsg";

                //当二审时可以不发送危急，则不查询
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditCheckResultViwer_showCol_UnSendUrg") != "是")//内部提醒危急值(msg_content无数据时,来源为住院)
                {
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CannotSendClinic") != "是")
                    {
                        DataTable dtbResultInner = objHelper.GetTable(strSQLInner);
                        if (dtbResultInner != null && dtbResultInner.Rows.Count > 0)
                        {
                            System.Diagnostics.Debug.WriteLine("危急值缓存，内部提醒危急信息：" + dtbResultInner.Rows.Count.ToString());
                            dtbResult.Merge(dtbResultInner);
                        }
                    }

                }

                //排序
                if (dtbResult != null && dtbResult.Rows.Count > 0
                    && dtbResult.Columns.Contains("pat_chk_date") && dtbResult.Columns.Contains("msg_type"))
                {
                    DataView dvTempSort = dtbResult.DefaultView.ToTable().DefaultView;
                    dvTempSort.Sort = "pat_chk_date desc,msg_type desc";
                    dsResult.Clear();
                    dsResult.Tables.Clear();
                    dsResult.AcceptChanges();
                    dsResult.Tables.Add(dvTempSort.ToTable());
                    dtbResult = dsResult.Tables[0];
                    dtbResult.TableName = "UrgentMsg";
                }

                if (true)//自编危急
                {
                    DataTable dtbResultDIY = objHelper.GetTable(strSQLDIY);
                    if (dtbResultDIY != null && dtbResultDIY.Rows.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("危急值缓存，自编危急信息：" + dtbResultDIY.Rows.Count.ToString());
                        dtbResult.Merge(dtbResultDIY);
                    }
                }

                DataView view = dtbResult.DefaultView;

                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    string temppatInNoZy = "";//住院病人住院号
                    List<string> temppatInNoZyList = new List<string>();//住院病人住院号集
                    List<string> patIDList = new List<string>();
                    for (int i = dtbResult.Rows.Count - 1; i >= 0; i--)
                    {
                        string pat_id = dtbResult.Rows[i]["pat_id"].ToString();
                        if (patIDList.Contains(pat_id))//过滤重复的信息(如,既是急查又是危急值的信息)
                        {
                            dtbResult.Rows.Remove(dtbResult.Rows[i]);
                        }
                        else
                        {
                            patIDList.Add(pat_id);
                            //   dtbResult.Rows[i]["pat_result"] = GetResult(pat_id);
                        }

                        if (dtbResult.Rows[i]["pat_ori_id"].ToString() == "108" && dtbResult.Rows[i]["pat_in_no"].ToString().Length > 0)
                        {
                            if (!temppatInNoZyList.Contains(dtbResult.Rows[i]["pat_in_no"].ToString()))
                            {
                                temppatInNoZyList.Add(dtbResult.Rows[i]["pat_in_no"].ToString());
                                temppatInNoZy += "'" + dtbResult.Rows[i]["pat_in_no"].ToString() + "',";
                            }
                        }
                    }

                    #region 陈星海his连接字符串,检查转科室信息
                    //陈星海his连接字符串,检查转科室信息
                    string CXHYYhisConnectionString = System.Configuration.ConfigurationManager.AppSettings["CXHYYhisConnectionString"];
                    if (!string.IsNullOrEmpty(CXHYYhisConnectionString) && temppatInNoZyList.Count > 0)
                    {
                        try
                        {
                            temppatInNoZy = temppatInNoZy.TrimEnd(new char[] { ',' });
                            DBHelper objHelper2 = new DBHelper(CXHYYhisConnectionString);
                            string sqlhisview = string.Format("select BAHM,BRKS,KSMC from Lis_brks where BAHM in({0})", temppatInNoZy);
                            //BAHM  --住院号
                            //BRKS  --科室代码
                            //KSMC  --科室名称
                            DataTable dthisview = objHelper2.GetTable(sqlhisview);

                            if (dthisview != null && dthisview.Rows.Count > 0)
                            {
                                //pat_dep_id dep_name
                                for (int j = 0; j < dthisview.Rows.Count; j++)
                                {
                                    DataRow[] drtempsel = dtbResult.Select("pat_ori_id='108' and pat_in_no='" + dthisview.Rows[j]["BAHM"].ToString() + "' and pat_dep_id<>'" + dthisview.Rows[j]["BRKS"].ToString() + "'");
                                    if (drtempsel.Length > 0)
                                    {
                                        for (int k = 0; k < drtempsel.Length; k++)
                                        {
                                            drtempsel[k]["pat_dep_id"] = dthisview.Rows[j]["BRKS"].ToString();
                                            drtempsel[k]["dep_name"] = dthisview.Rows[j]["KSMC"].ToString();
                                        }
                                    }
                                }
                                dtbResult.AcceptChanges();
                            }

                        }
                        catch (Exception objEx2)
                        {
                            Lib.LogManager.Logger.LogException("获取陈星海HIS库数据", objEx2);
                        }
                    }
                    #endregion
                }
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReturnMessages_IsNotify") == "是")
                {
                    DataTable dtbResultReturn = objHelper.GetTable(strReturn);
                    if (dtbResultReturn != null && dtbResultReturn.Rows.Count > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine("危急值缓存，自编危急信息：" + dtbResultDIY.Rows.Count.ToString());
                        dtbResult.Merge(dtbResultReturn);
                    }
                }
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取危急值数据", objEx);
            }

            return dtbResult;

        }

        /// <summary>
        /// 判断是否异常或危机
        /// </summary>
        /// <param name="res_ref_flag"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool dict_res_ref_flag(string res_ref_flag, out string description)
        {
            bool Isref = false;//是否异常或危急
            description = "";

            switch (res_ref_flag)
            {
                case "-1":
                    description = "无判定提示";
                    Isref = false;
                    break;
                case "0":
                    description = "正常";
                    Isref = false;
                    break;
                case "1":
                    description = "↑";
                    Isref = false;
                    break;
                case "2":
                    description = "↓";
                    Isref = false;
                    break;
                case "3":
                    description = "阳性";
                    Isref = true;
                    break;
                case "4":
                    description = "弱阳性";
                    Isref = true;
                    break;
                case "6":
                    description = "超出异常值";//危急值
                    Isref = true;
                    break;
                case "8":
                    description = "高于参考值";
                    Isref = false;
                    break;
                case "16":
                    description = "超出异常值";//高于危急值
                    Isref = true;
                    break;
                case "24":
                    description = "超出异常值";//高于危急值
                    Isref = true;
                    break;
                case "32":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "40":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "48":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "56":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "128":
                    description = "低于参考值";
                    Isref = false;
                    break;
                case "256":
                    description = "超出异常值";//低于危急值
                    Isref = true;
                    break;
                case "384":
                    description = "超出异常值";//低于危急值
                    Isref = true;
                    break;
                case "512":
                    description = "超出危急值";//低于阈值下限
                    Isref = true;
                    break;
                case "640":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                case "768":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                case "896":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                default:
                    description = "";
                    Isref = false;
                    break;
            }

            return Isref;

        }
        /*
        /// <summary>
        /// 根据res_id获取危机值信息
        /// </summary>
        /// <param name="res_id"></param>
        /// <returns></returns>
        private string GetResult(string res_id)
        {
            string res_value = "";

            if (!string.IsNullOrEmpty(res_id))
            {
                string strSQL = string.Format(@"select resulto.res_itm_id,--项目ID
dict_item.itm_rep_ecd,-- 项目显示名称
resulto.res_chr,--结果
resulto.res_unit,--单位
resulto.res_ref_flag --异常标识
from resulto 
left join dict_item on dict_item.itm_id=resulto.res_itm_id
where res_id='{0}'", res_id);

                try
                {
                    DataSet dtbResult = new DataSet();
                    DBHelper objHelper = new DBHelper();
                    dtbResult = objHelper.GetDataSet(strSQL);
                    dtbResult.Tables[0].TableName = "UrgentResult";


                    if (dtbResult.Tables["UrgentResult"] != null && dtbResult.Tables["UrgentResult"].Rows.Count > 0)
                    {
                        foreach (DataRow drUR in dtbResult.Tables["UrgentResult"].Rows)
                        {
                            string res_description = "";

                            if (dict_res_ref_flag(drUR["res_ref_flag"].ToString(), out res_description))
                            {
                                if (string.IsNullOrEmpty(res_value))
                                {
                                    res_value = string.Format("{0}  {1} {2}   {3}", drUR["itm_rep_ecd"].ToString()
                                        , drUR["res_chr"].ToString()
                                        , drUR["res_unit"].ToString()
                                        , res_description);
                                }
                                else
                                {
                                    res_value += string.Format("\r\n{0}  {1} {2}   {3}", drUR["itm_rep_ecd"].ToString()
                                        , drUR["res_chr"].ToString()
                                        , drUR["res_unit"].ToString()
                                        , res_description);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(res_value))
                        {
                            res_value += "\r\n";
                        }
                    }
                }
                catch (Exception objEx)
                {
                    res_value = "遇到错误！\r\n获取结果失败";
                }
            }

            return res_value;
        }
        */
        /// <summary>
        /// 扩展表是否已存在此ID
        /// </summary>
        /// <returns></returns>
        public bool PatExtExistByID(string pat_id)
        {
            string strSQL = string.Format("select pat_id from patients_ext where pat_id='{0}'", pat_id);

            DBHelper objHelper = new DBHelper();
            try
            {
                DataTable dtbResult = objHelper.GetTable(strSQL);
                dtbResult.TableName = "patients_ext";

                if (dtbResult != null)
                    if (dtbResult.Rows.Count > 0)
                    {
                        return true;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        /// <summary>
        /// 根據pat_id獲取病人是否召回
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="msg_type"></param>
        /// <returns></returns>
        public DataTable isBackForCheck(string pat_id, string msg_type)
        {
            string sql = string.Format("select * from msg_content where msg_type = '{1}' and msg_ext1 = '{0}'", pat_id, msg_type);
            DataTable result = new DataTable();
            result.TableName = "backpatient";

            try
            {
                result = new DBHelper().GetTable(sql);
                result.TableName = "backpatient";
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取自编危急值信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public DataTable GetDIYCriticalMsg(string sqlWhere)
        {
            DataTable result = new DataTable();
            result.TableName = "dt";
            try
            {
                string sql = string.Format(@"SELECT msg_content.msg_id, msg_content.msg_type,
 msg_content.msg_sender_id, msg_content.msg_sender_name,
 msg_content.msg_sender_ip, 
 msg_content.msg_create_time,
 msg_content.msg_title,
 msg_content.msg_content,
 msg_content.msg_del_flag,
 (case when msg_content.msg_del_flag=1 then '已查看' else '未查看' end) as msg_status,
 (case when msg_content.msf_insgin_flag='1' then '是' else '' end) as msf_insgin_flag_str,
 msg_content.msg_ext1,
 msg_content.msg_ext3,                    
 msg_content.msg_checker_id,
 msg_content.msg_checker_name,
 msg_content.pat_name, 
 msg_content.pat_sex,
(case msg_content.pat_sex when 1 then '男' when 2 then '女' else '未知' end) as pat_sex_str,
 msg_content.pat_age_str,
 msg_content.pat_bed_no,
 msg_content.msg_pat_tel,
 msg_content.pat_in_no,
 msg_content.pat_ori_id,
 msg_content.msg_doc_code,
 msg_content.msg_doc_name,
 dict_origin.ori_name,
 msg_receiver.receiver_id,
msg_receiver.receiver_read_time as msg_date_look, 
 msg_receiver.receiver_name
FROM  msg_content with(nolock) INNER JOIN
msg_receiver with(nolock) ON msg_content.msg_id = msg_receiver.receiver_msg_id
left join dict_origin on dict_origin.ori_id=msg_content.pat_ori_id
left join patients_ext with(nolock) on msg_content.msg_id=patients_ext.pat_id
where 1=1
{0}", sqlWhere);
                result = new DBHelper().GetTable(sql);
                result.TableName = "dt";
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 添加自编危急值信息
        /// </summary>
        /// <param name="dtNw"></param>
        /// <returns></returns>
        public int AddDIYCriticalMsg(DataTable dtNw)
        {
            int rvInt = 0;
            try
            {
                if (dtNw != null && dtNw.Rows.Count > 0)
                {
                    DataRow dr = dtNw.Rows[0];
                    string msg_id = Guid.NewGuid().ToString().Replace("-", "");
                    string dep_code = dr["dep_code"].ToString();
                    string dep_name = dr["dep_name"].ToString();
                    string msg_sender_id = dr["msg_sender_id"].ToString();
                    string msg_sender_name = dr["msg_sender_name"].ToString();
                    string msg_ext3 = dr["msg_ext3"].ToString();
                    string pat_name = dr["pat_name"].ToString();
                    string pat_sex = dr["pat_sex"].ToString();
                    string pat_age_str = dr["pat_age_str"].ToString();
                    string pat_bed_no = dr["pat_bed_no"].ToString();
                    string pat_in_no = dr["pat_in_no"].ToString();
                    string pat_ori_id = dr["pat_ori_id"].ToString();
                    string msg_doc_code = "";
                    string msg_doc_name = "";
                    string msg_send_depcode = "";
                    string pat_tel = "";

                    if (dr.Table.Columns.Contains("msg_doc_code"))
                    {
                        msg_doc_code = dr["msg_doc_code"].ToString();
                    }

                    if (dr.Table.Columns.Contains("msg_doc_name"))
                    {
                        msg_doc_name = dr["msg_doc_name"].ToString();
                    }

                    if (dr.Table.Columns.Contains("msg_send_depcode"))
                    {
                        msg_send_depcode = dr["msg_send_depcode"].ToString();
                    }

                    if (dr.Table.Columns.Contains("msg_pat_tel"))
                    {
                        pat_tel = dr["msg_pat_tel"].ToString();
                    }

                    INSERTSQLSTRING temp1 = new INSERTSQLSTRING("msg_content");
                    temp1.AddColValueOjb("msg_id", msg_id);
                    temp1.AddColValueOjb("msg_type", 2024);
                    temp1.AddColValueOjb("msg_sender_id", msg_sender_id);
                    temp1.AddColValueOjb("msg_sender_name", msg_sender_name);
                    temp1.AddColValue("msg_create_time", "getdate()");
                    temp1.AddColValueOjb("msg_title", "自编消息");
                    temp1.AddColValue("msg_content", "''");
                    temp1.AddColValueOjb("msg_del_flag", 0);
                    temp1.AddColValueOjb("msg_ext1", msg_id);
                    temp1.AddColValueOjb("msg_ext3", msg_ext3);
                    temp1.AddColValueOjb("pat_name", pat_name);
                    temp1.AddColValueOjb("pat_sex", pat_sex);
                    temp1.AddColValueOjb("pat_age_str", pat_age_str);
                    temp1.AddColValueOjb("pat_bed_no", pat_bed_no);
                    temp1.AddColValueOjb("pat_in_no", pat_in_no);
                    temp1.AddColValueOjb("pat_ori_id", pat_ori_id);
                    temp1.AddColValueOjb("msg_doc_code", msg_doc_code);
                    temp1.AddColValueOjb("msg_doc_name", msg_doc_name);
                    temp1.AddColValueOjb("msg_send_depcode", msg_send_depcode);
                    temp1.AddColValueOjb("msg_pat_tel", pat_tel);


                    INSERTSQLSTRING temp2 = new INSERTSQLSTRING("msg_receiver");
                    temp2.AddColValueOjb("receiver_msg_id", msg_id);
                    temp2.AddColValueOjb("receiver_id", dep_code);
                    temp2.AddColValueOjb("receiver_type", 3);
                    temp2.AddColValueOjb("receiver_name", dep_name);
                    //temp2.AddColValue("receiver_read_time", "getdate()");
                    temp2.AddColValueOjb("receiver_del_flag", 0);

                    Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                    rvInt = helper.ExecuteNonQuery(temp1.GetInsertSQL());
                    rvInt += helper.ExecuteNonQuery(temp2.GetInsertSQL());


                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            return rvInt;
        }

        #endregion

        public DataTable GetDocInfo()
        {
            string sql = @"select * ,dict_depart.dep_name,dict_depart.dep_code from dict_doctor with(nolock) LEFT OUTER JOIN
      dict_depart ON dict_doctor.doc_dep_id = dict_depart.dep_id where doc_del ='0'  order by doc_seq asc";

            DBHelper objHelper = new DBHelper();

            DataTable dtbResult = objHelper.GetTable(sql);
            dtbResult.TableName = "dict_doctor";

            return dtbResult;
        }

        /// <summary>
        /// 处理回退标本
        /// </summary>
        /// <param name="barcode"></param>
        public void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark)
        {
            List<string> strList = new List<string>();
            string sqlMessage = string.Format(@"update bc_return_messages set bc_handle_flag = 1 where bc_bar_code='{0}'", barcode);
            string sqlPatients = string.Format(@"update bc_patients set bc_return_flag = 0 where bc_bar_code='{0}'", barcode);
            string sqlSign = string.Format(@"insert into bc_sign(bc_date, bc_status, bc_login_id, bc_name, bc_bar_no, bc_bar_code ,bc_remark ,bc_flow) 
                            values ('{0}', '{1}' , '{2}' ,'{3}', '{4}', '{5}' ,'{6}' ,{7})"
                            , currentServerTime, "9", strOperatorID, strOperatorName, barcode, barcode, bc_remark, "1");
            strList.Add(sqlMessage);
            strList.Add(sqlPatients);
            strList.Add(sqlSign);

            Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
            foreach (string s in strList)
            {
                helper.ExecuteNonQuery(s);
            }

        }
    }
}
