using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoQueueNumber))]
    public class DaoQueueNumber : IDaoQueueNumber
    {

        public bool SaveQueueInfo(EntityQueueNumber queue)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("pid_in_no", queue.PidInNo);
                values.Add("pid_social_no", queue.PidSocialNo);
                values.Add("queue_no", queue.QueueNo);
                values.Add("queue_windows_name", queue.QueueWindowsName);
                values.Add("queue_date", queue.QueueDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("queue_status", queue.QueueStatus);
                values.Add("queue_windows_area", queue.QueueWindowsArea);
                values.Add("queue_priority", queue.QueuePriority);
                values.Add("pid_name", queue.PidName);
                helper.InsertOperation("pid_queue", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public int GetMaxQueueNo()
        {
            int queueNo = 0;
            try
            {
                DBManager helper = new DBManager();
                string sql = @"select top 1 queue_no from pid_queue where queue_date >= convert(varchar(10), getdate(), 120)+' 00:00:00' 
                                        and queue_date <= convert(varchar(10), getdate(), 120)+' 23:59:59' order by queue_date desc";
                DataTable dt = helper.ExecSel(sql);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["queue_no"] != null)
                {
                    queueNo = Convert.ToInt32(dt.Rows[0]["queue_no"].ToString());
                }
                else
                {
                    queueNo = 0;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return queueNo;
        }

        public List<EntityQueueNumber> GetQueueNumber(string dateSatrt, string dateEnd, string windowsName, string windowsArea)
        {
            List<EntityQueueNumber> list = new List<EntityQueueNumber>();
            try
            {
                string sqlWhere = string.Empty;
                if (!string.IsNullOrEmpty(dateSatrt))
                {
                    sqlWhere += string.Format("and queue_date>='{0}'", dateSatrt);
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    sqlWhere += string.Format("and queue_date<='{0}'", dateEnd);
                }
                if (!string.IsNullOrEmpty(windowsName))
                {
                    sqlWhere += string.Format("and queue_windows_name='{0}'", windowsName);
                }
                if (!string.IsNullOrEmpty(windowsArea))
                {
                    sqlWhere += string.Format("and queue_windows_area='{0}'", windowsArea);
                }
                string sql = string.Format(@"select queue_sn, pid_in_no, pid_social_no, queue_no, queue_windows_name, queue_date,pid_name,Datediff(mi,pid_queue.queue_date,getdate()) as queue_time_min,
                                                              (select top 1 Sample_main.Sma_collection_date from Sample_main where Sample_main.Sma_pat_in_no=pid_queue.pid_in_no order by Sma_collection_date  desc)as collection_date,queue_status, queue_windows_area, queue_priority from pid_queue where 1=1{0}", sqlWhere);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityQueueNumber>.ConvertToList(dt).OrderBy(w => w.QueueNo.Length).ThenBy(w=>w.QueueNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool UpdateQueueStatus(string pidInNo,string queueNo, string status)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(pidInNo) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(status))
                {
                    string sql = string.Format("update pid_queue set queue_status='{0}' where pid_in_no='{1}' and queue_no='{2}'", status, pidInNo, queueNo);
                    DBManager helper = new DBManager();
                    result = helper.ExecCommand(sql) > 0;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
        public bool UpdateQueueWindow(string pidInNo,string queueNo, string window)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(pidInNo) && !string.IsNullOrEmpty(window) && !string.IsNullOrEmpty(window))
                {
                    string sql = string.Format("update pid_queue set queue_windows_name='{0}' where pid_in_no='{1}' and queue_no='{2}'", window, pidInNo,queueNo);
                    DBManager helper = new DBManager();
                    result = helper.ExecCommand(sql) > 0;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
        public EntityQueueNumber GetQueueNumberByNo(string pidInNo, string queueStatus, string dateSatrt, string dateEnd)
        {
            EntityQueueNumber queue = new EntityQueueNumber();
            try
            {
                string sqlWhere = string.Empty;
                if (!string.IsNullOrEmpty(dateSatrt))
                {
                    sqlWhere += string.Format("and queue_date>='{0}'", dateSatrt);
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    sqlWhere += string.Format("and queue_date<='{0}'", dateEnd);
                }
                if (!string.IsNullOrEmpty(pidInNo))
                {
                    sqlWhere += string.Format("and pid_in_no='{0}'", pidInNo);
                }
                if (!string.IsNullOrEmpty(queueStatus))
                {
                    sqlWhere += string.Format("and queue_status='{0}'", queueStatus);
                }

                string sql = string.Format(@"select queue_sn, pid_in_no, pid_social_no, queue_no, queue_windows_name,queue_status,
                                                             queue_date,pid_name from pid_queue where 1=1 {0} ", sqlWhere);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityQueueNumber> list = EntityManager<EntityQueueNumber>.ConvertToList(dt).OrderBy(w=>w.QueueDate).ToList();
                if (list != null && list.Count > 0)
                    queue = list[0];
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return queue;
        }

        public bool UpdateQueueNo(string queueNo, DateTime queueDate, string pidInNo)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(queueNo) && !string.IsNullOrEmpty(pidInNo))
                {
                    string sql = string.Format("update pid_queue set queue_no='{0}', queue_date='{1}' where pid_in_no='{2}'", queueNo, queueDate.ToString(), pidInNo);
                    DBManager helper = new DBManager();
                    result = helper.ExecCommand(sql) > 0;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
