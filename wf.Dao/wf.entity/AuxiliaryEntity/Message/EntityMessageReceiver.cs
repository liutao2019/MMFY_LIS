using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace dcl.entity
{
    [Serializable]
    public class EntityMessageReceiver
    {
        /// <summary>
        /// 消息ID，使用GUID存储，小写
        /// </summary>
        public string MessageID { get; set; }


        /// <summary>
        /// 接收者ID
        /// </summary>
        public string ReceiverID { get; set; }

        /// <summary>
        /// 接收者类型
        /// </summary>
        public EnumMessageReceiverType ReceiverType { get; set; }

        /// <summary>
        /// 接收者名称
        /// </summary>
        public string ReceiverName { get; set; }


        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// 消息主体
        /// </summary>
        public EntityMessageContent MessageContent { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool Deleted { get; set; }

        public EntityMessageReceiver()
        {
            this.Deleted = false;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.ReceiverType, this.ReceiverID, this.ReceiverName);
            //return base.ToString();
        }

        #region database
        public static EntityMessageReceiver FromDataRow(DataRow row)
        {
            EntityMessageReceiver entity = new EntityMessageReceiver();

            entity.MessageID = row["receiver_msg_id"].ToString();
            entity.ReceiverID = row["receiver_id"].ToString();
            entity.ReceiverType = (EnumMessageReceiverType)Convert.ToInt32(row["receiver_type"]);

            entity.ReceiverName = row["receiver_name"].ToString();

            if (row["receiver_read_time"] != DBNull.Value)
            {
                entity.ReadTime = Convert.ToDateTime(row["receiver_read_time"]);
            }

            entity.Deleted = Convert.ToBoolean(row["receiver_del_flag"]);

            return entity;
        }

        public static MessageReceiverCollection FromDataTable(DataTable table)
        {
            MessageReceiverCollection list = new MessageReceiverCollection();

            foreach (DataRow row in table.Rows)
            {
                EntityMessageReceiver entity = FromDataRow(row);
                list.Add(entity);
            }

            return list;
        }

        public SqlCommand GenerateInsertCommand()
        {
            SqlCommand cmd = new SqlCommand();

            string sqlInsert = @"
insert into msg_receiver(receiver_msg_id, receiver_id, receiver_type, receiver_name, receiver_read_time, receiver_del_flag)
values(@receiver_msg_id, @receiver_id, @receiver_type, @receiver_name, @receiver_read_time, @receiver_del_flag)
";

            cmd.CommandText = sqlInsert;

            cmd.Parameters.AddWithValue("receiver_msg_id", this.MessageID);
            cmd.Parameters.AddWithValue("receiver_id", this.ReceiverID);
            cmd.Parameters.AddWithValue("receiver_name", this.ReceiverName);

            cmd.Parameters.AddWithValue("receiver_type", (int)this.ReceiverType);

            if (this.ReadTime != null)
            {
                cmd.Parameters.AddWithValue("receiver_read_time", this.ReadTime);
            }
            else
            {
                cmd.Parameters.AddWithValue("receiver_read_time", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("receiver_del_flag", this.Deleted);

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
update msg_receiver set
    receiver_name = @receiver_name,
    receiver_read_time = @receiver_read_time,
    receiver_del_flag = @receiver_del_flag
where receiver_msg_id = @receiver_msg_id and receiver_id = @receiver_id and receiver_type = @receiver_type
";
            cmd.CommandText = sqlUpdate;

            cmd.Parameters.AddWithValue("receiver_msg_id", this.MessageID);
            cmd.Parameters.AddWithValue("receiver_id", this.ReceiverID);
            cmd.Parameters.AddWithValue("receiver_type", (int)this.ReceiverType);
            cmd.Parameters.AddWithValue("receiver_name", this.ReceiverName);

            if (this.ReadTime != null)
            {
                cmd.Parameters.AddWithValue("receiver_read_time", this.ReadTime);
            }
            else
            {
                cmd.Parameters.AddWithValue("receiver_read_time", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("receiver_del_flag", this.Deleted);

            return cmd;
        }

        public SqlCommand GenerateDeleteCommand()
        {
            if (string.IsNullOrEmpty(this.MessageID))
            {
                throw new Exception("消息ID为空");
            }

            SqlCommand cmd = new SqlCommand();

            string sqlDelete = @"delete from msg_receiver where receiver_msg_id = @receiver_msg_id and receiver_id = @receiver_id and receiver_type = @receiver_type";

            cmd.CommandText = sqlDelete;
            cmd.Parameters.AddWithValue("receiver_msg_id", this.MessageID);
            cmd.Parameters.AddWithValue("receiver_id", this.ReceiverID);
            cmd.Parameters.AddWithValue("receiver_type", (int)this.ReceiverType);

            return cmd;
        }
        #endregion
    }
}
