using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrMessageReceive))]
    public class DaoObrMessageReceive : IDaoObrMessageReceive
    {
        public bool DeleteObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            //物理删除 一般不会用到
            try
            {
                DBManager helper = new DBManager();

                if (!string.IsNullOrEmpty(msgReceive.ObrId))
                {
                    //物理删除
                    string sqlStr = string.Format(@"delete from Lis_message_receive where Lmsgrec_Lmsg_id = '{0}' ", msgReceive.ObrId);
                    helper.ExecSql(sqlStr);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public bool SaveObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lmsgrec_Lmsg_id", msgReceive.ObrId);//客户端生成唯一主键
                values.Add("Lmsgrec_user_id", msgReceive.ObrUserId);
                values.Add("Lmsgrec_type", msgReceive.ObrType);
                values.Add("Lmsgrec_user_name", msgReceive.ObrUserName);
                values.Add("Lmsgrec_read_time", msgReceive.ObrReadTime);
                values.Add("del_flag", msgReceive.DelFlag);

                helper.InsertOperation("Lis_message_receive", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicObrMessageReceive> SearchObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = string.Format(@"select Lmsgrec_Lmsg_id,
                                                       Lmsgrec_user_id,
                                                       Lmsgrec_type,
                                                       Lmsgrec_user_name,
                                                       Lmsgrec_read_time,
                                                       del_flag
                                                from Lis_message_receive
                                                     where Lmsgrec_Lmsg_id = '{0}' ", msgReceive.ObrId);

                DataTable dtMsgReceive = helper.ExecuteDtSql(sqlStr);
                List<EntityDicObrMessageReceive> list = EntityManager<EntityDicObrMessageReceive>.ConvertToList(dtMsgReceive).OrderBy(i => i.ObrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicObrMessageReceive>();
            }
        }

        public bool UpdateObrMessageReceive(EntityDicObrMessageReceive msgReceive)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Lmsgrec_user_id", msgReceive.ObrUserId);
                values.Add("Lmsgrec_type", msgReceive.ObrType);
                values.Add("Lmsgrec_user_name", msgReceive.ObrUserName);
                values.Add("Lmsgrec_read_time", msgReceive.ObrReadTime);
                values.Add("del_flag", msgReceive.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lmsgrec_Lmsg_id", msgReceive.ObrId);

                helper.UpdateOperation("Lis_message_receive", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public ObrMessageReceiveCollection GetMessageByReceiverID(string receiverID, EnumObrMessageReceiveType receiverType, bool bGetDeleted, bool bGetReaded, bool bGetExpired)
        {
            ObrMessageReceiveCollection list = new ObrMessageReceiveCollection();
            try
            {
                string sql_content = @" select
Lis_message_content.*,
Lis_message_receive.*
from Lis_message_receive
inner join Lis_message_content on Lis_message_content.Lmsg_id = Lis_message_receive.Lmsgrec_Lmsg_id
where (Lis_message_content.del_flag = 0 or Lis_message_content.del_flag is null) 
and Lis_message_receive.Lmsgrec_type = @obr_type ";

                if (receiverID != null)
                {
                    sql_content += " and rtrim(Lis_message_receive.Lmsgrec_user_id) = @obr_user_id ";
                }

                if (!bGetDeleted)
                {
                    sql_content += " and Lis_message_receive.del_flag = 0";//接收者未删除
                }

                if (!bGetReaded)
                {
                    sql_content += " and Lis_message_receive.Lmsgrec_read_time is null";//接收者未阅读
                }

                if (!bGetExpired)
                {
                    sql_content += @" and Lis_message_content.Lmsg_create_time>getdate()-7
                                  and (Lis_message_content.Lmsg_expiration_date is null or getdate() <= Lis_message_content.Lmsg_expiration_date)";
                }

                List<DbParameter> paramHt = new List<DbParameter>();

                paramHt.Add(new SqlParameter("@obr_type", (int)receiverType));

                if (receiverID != null)
                {
                    paramHt.Add(new SqlParameter("@obr_user_id", receiverID));
                }

                DBManager helper = new DBManager();

                DataTable tableMessage = helper.ExecuteDtSql(sql_content, paramHt);

                foreach (DataRow row in tableMessage.Rows)
                {
                    EntityDicObrMessageReceive entityRec = EntityManager<EntityDicObrMessageReceive>.ConvertToEntity(row);
                    EntityDicObrMessageContent entityContent = EntityManager<EntityDicObrMessageContent>.ConvertToEntity(row);
                    entityRec.ObrMessageContent = entityContent;

                    list.Add(entityRec);
                }

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return list;
            }
        }

        public bool UpdateObrMsgReciveToDateByID(EntityDicObrMessageReceive msgReceive)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = string.Format(@"update Lis_message_receive 
                                                 set Lmsgrec_read_time=getdate()  
                                                where Lmsgrec_Lmsg_id = @obr_id ");

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_id", msgReceive.ObrId));

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrMsgReciveToDateDelFlagByID(EntityDicObrMessageReceive msgReceive)
        {
            try
            {
                DBManager helper = new DBManager();
                //这里更新了标志msf_insgin_flag
                string sqlStr = string.Format(@"update Lis_message_receive 
                                                 set  del_flag = 1,
                                                      Lmsgrec_read_time=getdate()  
                                                where Lmsgrec_Lmsg_id = @obr_id ");

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_id", msgReceive.ObrId));

                if (msgReceive.LogicalDelete)
                {
                    sqlStr += string.Format(@" and Lmsgrec_type = @obr_type and Lmsgrec_user_id = @obr_user_id");

                    paramHt.Add(new SqlParameter("@obr_type", msgReceive.ObrType));
                    paramHt.Add(new SqlParameter("@obr_user_id", msgReceive.ObrUserId));
                }

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            } 
        }
    }
}
