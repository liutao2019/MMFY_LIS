using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.pub.entities.Message;

namespace dcl.svr.msg
{
    /// <summary>
    /// 危急值消息
    /// </summary>
    public class DeptMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static DeptMessageCache _instance = null;

        public static DeptMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DeptMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private DeptMessageCache()
        {
            this.cache = new MessageReceiverCollection();
        }
        #endregion

        private MessageReceiverCollection cache = null;

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = new MessageBiz().GetMessageByReceiverID(null, EnumMessageReceiverType.Dept, false, true, false);
        }

        /// <summary>
        /// 根据科室代码获取科室消息
        /// </summary>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public MessageReceiverCollection GetMessage(string dept_code)
        {
            if (dept_code == null)
            {
                return cache;
            }
            else
            {
                SqlHelper db = new SqlHelper();

                int Int_AddDays = -1;//默认24小时内

                //系统配置：[旧危急值]提醒几小时内的消息
                string Str_AddDays = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Old_CacheHour");

                if (!string.IsNullOrEmpty(Str_AddDays))
                {
                    if (Str_AddDays == "48")
                    {
                        Int_AddDays = -2;
                    }
                    else if (Str_AddDays == "72")
                    {
                        Int_AddDays = -3;
                    }
                }

                string sql = string.Format(@"select * from dict_depart where dep_ward =
(select top 1 dep_ward from dbo.dict_depart where dep_code='{0}' )", dept_code);

                DataTable dtDep = db.GetTable(sql);

                List<string> depIDs = new List<string>();

                IEnumerable<EntityMessageReceiver> query;
                if (dtDep.Rows.Count > 0)
                {
                    foreach (DataRow drDep in dtDep.Rows)
                    {
                        depIDs.Add(drDep["dep_code"].ToString());
                    }

                    query = from item in this.cache
                            where depIDs.Contains(item.ReceiverID.ToString().Trim()) && item.Deleted == false && item.MessageContent.CreateTime >= DateTime.Now.AddDays(Int_AddDays) && item.MessageContent.MessageType != EnumMessageType.URGENT_MESSAGE
                            orderby item.MessageContent.CreateTime descending
                            select item;
                }
                else
                {
                    query = from item in this.cache
                            where item.ReceiverID.ToString().Trim() == dept_code && item.Deleted == false && item.MessageContent.CreateTime >= DateTime.Now.AddDays(Int_AddDays) && item.MessageContent.MessageType != EnumMessageType.URGENT_MESSAGE
                            orderby item.MessageContent.CreateTime descending
                            select item;
                }
                MessageReceiverCollection list = new MessageReceiverCollection(query);
                return list;
            }
        }

        /// <summary>
        /// 根据多科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        public MessageReceiverCollection GetMessageByDepts(string dept_codes)
        {
            if (dept_codes == null)
            {
                return cache;
            }
            else
            {
                SqlHelper db = new SqlHelper();

                int Int_AddDays = -1;//默认24小时内

                //系统配置：[旧危急值]提醒几小时内的消息
                string Str_AddDays = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Old_CacheHour");

                if (!string.IsNullOrEmpty(Str_AddDays))
                {
                    if (Str_AddDays == "48")
                    {
                        Int_AddDays = -2;
                    }
                    else if (Str_AddDays == "72")
                    {
                        Int_AddDays = -3;
                    }
                }

                string dept_code = "";//如果没病区,默认用此科室查询

                string dept_codeIn = "";//多病区条件

                if (dept_codes.Contains(","))
                {
                    foreach (string strTemp in dept_codes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        //随机默认取第一个为默认病区
                        if (string.IsNullOrEmpty(dept_code))
                        {
                            dept_code = strTemp;
                        }

                        if (string.IsNullOrEmpty(dept_codeIn))
                        {
                            dept_codeIn = "'" + strTemp + "'";
                        }
                        else
                        {
                            dept_codeIn += ",'" + strTemp + "'";
                        }
                    }
                }
                else
                {
                    dept_code = dept_codes;
                }

                string sql = string.Format(@"select * from dict_depart where dep_ward in
(select dep_ward from dbo.dict_depart where dep_code in({0}) )", dept_codeIn);

                DataTable dtDep = db.GetTable(sql);

                List<string> depIDs = new List<string>();

                IEnumerable<EntityMessageReceiver> query;
                if (dtDep.Rows.Count > 0)
                {
                    foreach (DataRow drDep in dtDep.Rows)
                    {
                        depIDs.Add(drDep["dep_code"].ToString());
                    }

                    query = from item in this.cache
                            where depIDs.Contains(item.ReceiverID.ToString().Trim()) && item.Deleted == false && item.MessageContent.CreateTime >= DateTime.Now.AddDays(Int_AddDays) && item.MessageContent.MessageType != EnumMessageType.URGENT_MESSAGE
                            orderby item.MessageContent.CreateTime descending
                            select item;
                }
                else
                {
                    query = from item in this.cache
                            where item.ReceiverID.ToString().Trim() == dept_code && item.Deleted == false && item.MessageContent.CreateTime >= DateTime.Now.AddDays(Int_AddDays) && item.MessageContent.MessageType != EnumMessageType.URGENT_MESSAGE
                            orderby item.MessageContent.CreateTime descending
                            select item;
                }
                MessageReceiverCollection list = new MessageReceiverCollection(query);
                return list;
            }
        }

        public MessageReceiverCollection GetMessage(string dept_code, EnumMessageType message_type)
        {
            if (dept_code == null)
            {
                var query = from item in this.cache
                            where item.MessageContent.MessageType == message_type
                            orderby item.MessageContent.CreateTime descending
                            select item;
                MessageReceiverCollection list = new MessageReceiverCollection(query);
                return list;
            }
            else
            {
                var query = from item in this.cache
                            where item.ReceiverID == dept_code && item.MessageContent.MessageType == message_type
                            orderby item.MessageContent.CreateTime descending
                            select item;

                MessageReceiverCollection list = new MessageReceiverCollection(query);
                return list;
            }
        }

        ///// <summary>
        ///// 在缓存在移除消息
        ///// </summary>
        ///// <param name="msg_id"></param>
        //public void RemoveMessage(string msg_id)
        //{
        //    for (int i = this.cache.Count - 1; i >= 0; i--)
        //    {
        //        if (this.cache[i].MessageID == msg_id)
        //        {
        //            this.cache.Remove(this.cache[i]);
        //        }
        //    }
        //}
    }
}
