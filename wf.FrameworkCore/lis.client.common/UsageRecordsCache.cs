using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace dcl.client.common
{
    /// <summary>
    /// 操作记录缓存
    /// </summary>
    public class UsageRecordsCache
    {
        /// <summary>
        /// 本地缓存记录（最大记录数为20）
        /// </summary>
        static UsageRecords records = new UsageRecords();

        static string path = Application.StartupPath + @"\LocalHistoryOperLog.txt";

        public delegate void DelLoadUsageRecord(UsageRecord record);

        public static DelLoadUsageRecord LoadUsageRecord;

        public delegate void DelInitHistory(UsageRecords res);

        public DelInitHistory LoadHistory;

        public int MaxCount = 5;

        /// <summary>
        /// 初始化记录缓存   
        /// </summary>
        /// <param name="IsLoadLocalHistory">是否从本地加载历史记录,默认不加载</param>
        public void InitCache(bool IsLoadLocalHistory = false)
        {
            Thread th = new Thread(() =>
            {

                if (records.Records == null)
                    records.Records = new List<UsageRecord>();

                if (!IsLoadLocalHistory)
                    ClearTxt();
                else
                    LoadLocalHistory();

                //lock (records)
                //{
                //    //if (records.Records.Count == 20)
                //    //    records.Records.RemoveAt(0);//移除最早的记录
                //    //UsageRecord record = new UsageRecord { EventDate = DateTime.Now, EventModule = "ROOT", EventDesc = "操作缓存开始初始化。。。。" };                    
                //    //records.Records.Add(record);
                //    //if (LoadUsageRecord != null)
                //    //    LoadUsageRecord(record);
                //    UpdateLocal();
                //}
            });
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 获取本地保存的操作历史纪录
        /// </summary>
        private void LoadLocalHistory()
        {
            try
            {
                if (!File.Exists(path))
                    return;
                UsageRecords re = UsageRecordsSerializer.DeserializeFromXml<UsageRecords>(path);
                if (re == null)
                    return;
                records = re;
                if (LoadHistory != null)
                    LoadHistory(records);
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 向缓存写入新记录
        /// </summary>
        /// <param name="ModuleName">操作模块名称</param>
        /// <param name="Desc">描述</param>
        public void AddCache(string ModuleName, string Desc = "")
        {
            if (records.Records == null)
                throw new Exception("操作缓存未初始化！");
            Thread th = new Thread(() =>
            {
                lock (records)
                {
                    if (records.Records.Count >= 20)
                        records.Records.RemoveAt(0);
                    UsageRecord record = new UsageRecord { EventDate = DateTime.Now, EventModule = ModuleName, EventDesc = Desc };
                    records.Records.Add(record);
                    if (LoadUsageRecord != null)
                        LoadUsageRecord(record);
                    UpdateLocal();
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        public void AddCache(string ModuleName,string SenderName,string SenderText, string SenderTag,string EventDesc)
        {
            if (records.Records == null)
                throw new Exception("操作缓存未初始化！");
            Thread th = new Thread(() =>
            {
                lock (records)
                {
                    if (records.Records.Count >= 20)
                        records.Records.RemoveAt(0);
                    UsageRecord record = new UsageRecord { EventDate = DateTime.Now, EventModule = ModuleName, SenderName= SenderName,
                    SenderTag = SenderTag, SenderText = SenderText,EventDesc = EventDesc
                    };
                    records.Records.Add(record);
                    if (LoadUsageRecord != null)
                        LoadUsageRecord(record);
                    UpdateLocal();
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="record"></param>
        private void UpdateLocal()
        {
            try
            {
                UsageRecordsSerializer.SerializeToXml(path, records);
            }
            catch (Exception)
            {

            }
        }

        #region 记录重置
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            if (records.Records == null)
                throw new Exception("操作缓存未初始化！");
            Thread th = new Thread(() =>
            {
                lock (records)
                {
                    records.Records.Clear();
                    ClearTxt();
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 清空本地文本内容
        /// </summary>
        private void ClearTxt()
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }

    /// <summary>
    /// 操作记录实体类
    /// </summary>
    [XmlType(TypeName = "Record")]
    public class UsageRecord
    {

        /// <summary>
        /// 发生日期
        /// </summary>
        [XmlAttribute("EventDate")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        [XmlAttribute("EventModule")]
        public string EventModule { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        [XmlAttribute("EventDesc")]
        public string EventDesc { get; set; }

        /// <summary>
        /// SenderTag
        /// </summary>
        [XmlAttribute("SenderTag")]
        public string SenderTag { get; set; }

        /// <summary>
        /// SenderName
        /// </summary>
        [XmlAttribute("SenderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// SenderText
        /// </summary>
        [XmlAttribute("SenderText")]
        public string SenderText { get; set; }


    }


    /// <summary>
    /// 操作记录列表
    /// </summary>
    [XmlType(TypeName = "Document")]
    public class UsageRecords
    {
        [XmlArray("Records")]
        public List<UsageRecord> Records { get; set; }
    }

    public class UsageRecordsSerializer
    {
        public static void SerializeToXml<T>(string filePath, T obj)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath)) { System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T)); xs.Serialize(writer, obj); }
            }
            catch (Exception ex) { }
        }

        /// <summary>     
        /// 从某一XML文件反序列化到某一类型   
        /// </summary>    
        /// <param name="filePath">待反序列化的XML文件名称</param>  
        /// <param name="type">反序列化出的</param>  
        /// <returns></returns>    
        public static T DeserializeFromXml<T>(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                    throw new ArgumentNullException(filePath + " not Exists");
                using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    T ret = (T)xs.Deserialize(reader);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
