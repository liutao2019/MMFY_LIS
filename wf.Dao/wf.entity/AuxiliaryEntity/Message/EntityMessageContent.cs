using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace dcl.entity
{
    [Serializable]
    public class EntityMessageContent : ICloneable
    {
        /// <summary>
        /// 消息ID，使用GUID存储，小写
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public EnumMessageType MessageType { get; set; }

        /// <summary>
        /// 消息发送者工号
        /// </summary>
        public string SenderID { get; set; }

        /// <summary>
        /// 消息发送者名称
        /// </summary>
        public string SenderName { get; set; }
        public string SenderIP { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 接收者字符串，仅作显示 如："用户A，用户B...."
        /// </summary>
        public string ReceiverString { get; set; }

        /// <summary>
        /// XML格式的正文内容
        /// </summary>
        public string XMLContent { get; set; }

        /// <summary>
        /// 父消息ID
        /// </summary>
        public string ParentMsgID { get; set; }

        /// <summary>
        /// 有效日期，为空则不限制
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string Ext1 { get; set; }
        public string Ext2 { get; set; }
        public string Ext3 { get; set; }
        public string Ext4 { get; set; }

        /// <summary>
        /// 接收者列表
        /// </summary>
        public MessageReceiverCollection ListMessageReceiver { get; set; }

        public EntityMessageContent()
        {
            this.Deleted = false;
            this.ListMessageReceiver = new MessageReceiverCollection();

            this.SenderID = string.Empty;
            this.SenderName = string.Empty;
            this.SenderIP = string.Empty;
            this.ParentMsgID = string.Empty;
        }

        #region database
        public static EntityMessageContent FromDataRow(DataRow row)
        {
            EntityMessageContent entity = new EntityMessageContent();

            entity.MessageID = row["msg_id"].ToString();

            if (row["msg_type"] != DBNull.Value)
            {
                entity.MessageType = (EnumMessageType)(Convert.ToInt32(row["msg_type"]));
            }

            entity.SenderID = row["msg_sender_id"].ToString();
            entity.SenderName = row["msg_sender_name"].ToString();
            entity.SenderIP = row["msg_sender_ip"].ToString();

            if (row["msg_create_time"] != DBNull.Value)
            {
                entity.CreateTime = Convert.ToDateTime(row["msg_create_time"]);
            }

            entity.Title = row["msg_title"].ToString();


            entity.ReceiverString = row["msg_receiver"].ToString();
            entity.XMLContent = row["msg_content"].ToString();
            entity.ParentMsgID = row["msg_parent_msgid"].ToString();

            if (row["msg_del_flag"] != DBNull.Value)
            {
                entity.Deleted = Convert.ToBoolean(row["msg_del_flag"]);
            }

            if (row["msg_expiretime"] != DBNull.Value)
            {
                entity.ExpireTime = Convert.ToDateTime(row["msg_expiretime"]);
            }

            entity.Ext1 = row["msg_ext1"].ToString();
            entity.Ext2 = row["msg_ext2"].ToString();
            entity.Ext3 = row["msg_ext3"].ToString();
            entity.Ext4 = row["msg_ext4"].ToString();

            return entity;
        }

        public static List<EntityMessageContent> FromDataTable(DataTable table)
        {
            List<EntityMessageContent> list = new List<EntityMessageContent>();

            foreach (DataRow item in table.Rows)
            {
                EntityMessageContent entity = FromDataRow(item);
                list.Add(entity);
            }

            return list;
        }

        public SqlCommand GenerateInsertCommand()
        {
            SqlCommand cmd = new SqlCommand();

            string sqlInsert = @"
insert into msg_content(msg_id, msg_type, msg_sender_id, msg_sender_name, msg_sender_ip, msg_create_time, msg_title, msg_receiver, msg_content, msg_parent_msgid, msg_del_flag, msg_expiretime,msg_ext1,msg_ext2,msg_ext3,msg_ext4)
values(
@msg_id, 
@msg_type, 
@msg_sender_id, 
@msg_sender_name, 
@msg_sender_ip,
--getdate(),--生成日期采用数据库getdate
@msg_create_time, 
@msg_title, 
@msg_receiver,
@msg_content, 
@msg_parent_msgid, 
@msg_del_flag, 
@msg_expiretime,
@msg_ext1,
@msg_ext2,
@msg_ext3,
@msg_ext4
)
";

            cmd.CommandText = sqlInsert;

            cmd.Parameters.AddWithValue("msg_id", this.MessageID);
            cmd.Parameters.AddWithValue("msg_type", (int)this.MessageType);
            cmd.Parameters.AddWithValue("msg_sender_id", this.SenderID);
            cmd.Parameters.AddWithValue("msg_sender_name", this.SenderName);
            cmd.Parameters.AddWithValue("msg_create_time", this.CreateTime);//取服务端时间

            if (this.SenderIP == null)
            {
                cmd.Parameters.AddWithValue("msg_sender_ip", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_sender_ip", this.SenderIP);
            }

            if (this.Title == null)
            {
                cmd.Parameters.AddWithValue("msg_title", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_title", this.Title);
            }


            if (this.ReceiverString == null)
            {
                cmd.Parameters.AddWithValue("msg_receiver", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_receiver", this.ReceiverString);
            }

            //cmd.Parameters.AddWithValue("msg_receiver", this.ReceiverString);
            cmd.Parameters.AddWithValue("msg_content", this.XMLContent);


            if (this.SenderIP == null)
            {
                cmd.Parameters.AddWithValue("msg_parent_msgid", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_parent_msgid", this.ParentMsgID);
            }
            //cmd.Parameters.AddWithValue("msg_parent_msgid", this.ParentMsgID);


            cmd.Parameters.AddWithValue("msg_del_flag", this.Deleted);

            if (this.ExpireTime != null)
            {
                cmd.Parameters.AddWithValue("msg_expiretime", this.ExpireTime.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_expiretime", DBNull.Value);
            }

            if (this.Ext1 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext1", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext1", this.Ext1);
            }

            if (this.Ext2 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext2", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext2", this.Ext2);
            }

            if (this.Ext3 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext3", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext3", this.Ext3);
            }

            if (this.Ext4 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext4", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext4", this.Ext4);
            }

            return cmd;
        }

        public SqlCommand GenerateUpdateCommand()
        {
            if (string.IsNullOrEmpty(this.MessageID))
            {
                throw new Exception("消息ID为空");
            }

            SqlCommand cmd = new SqlCommand();

            string sqlUpdate = @"
update msg_content set
    msg_type = @msg_type, 
    --msg_sender_id = @msg_sender_id, 
    --msg_sender_name = @msg_sender_name, 
    --msg_sender_ip = @msg_sender_ip,
    msg_title = @msg_title, 
    msg_receiver = @msg_receiver,
    msg_content = @msg_content, 
    msg_parent_msgid = @msg_parent_msgid, 
    msg_del_flag = @msg_del_flag, 
    msg_expiretime = @msg_expiretime,
    msg_ext1 = @msg_ext1,
    msg_ext2 = @msg_ext2,
    msg_ext3 = @msg_ext3,
    msg_ext4 = @msg_ext4
where msg_id = @msg_id
";
            cmd.CommandText = sqlUpdate;

            cmd.Parameters.AddWithValue("msg_id", this.MessageID);
            cmd.Parameters.AddWithValue("msg_type", (int)this.MessageType);

            cmd.Parameters.AddWithValue("msg_title", this.Title);
            cmd.Parameters.AddWithValue("msg_receiver", this.ReceiverString);
            cmd.Parameters.AddWithValue("msg_content", this.XMLContent);
            cmd.Parameters.AddWithValue("msg_parent_msgid", this.ParentMsgID);
            cmd.Parameters.AddWithValue("msg_del_flag", this.Deleted);

            if (this.ExpireTime != null)
            {
                cmd.Parameters.AddWithValue("msg_expiretime", this.ExpireTime.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_expiretime", DBNull.Value);
            }

            if (this.Ext1 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext1", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext1", this.Ext1);
            }

            if (this.Ext2 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext2", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext2", this.Ext2);
            }

            if (this.Ext3 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext3", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext3", this.Ext3);
            }

            if (this.Ext4 == null)
            {
                cmd.Parameters.AddWithValue("msg_ext4", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("msg_ext4", this.Ext4);
            }


            return cmd;
        }

        public SqlCommand GenerateDeleteCommand()
        {
            if (string.IsNullOrEmpty(this.MessageID))
            {
                throw new Exception("消息ID为空");
            }

            SqlCommand cmd = new SqlCommand();

            string sqlDelete = @"delete from msg_content where msg_id = @msg_id";

            cmd.CommandText = sqlDelete;

            cmd.Parameters.AddWithValue("msg_id", this.MessageID);

            return cmd;
        }
        #endregion

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
