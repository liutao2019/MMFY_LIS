using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaAnnouncement))]
    public class DaoOaAnnouncement : IDaoOaAnnouncement
    {
        public int DeleteAnnouncement(List<EntityOaAnnouncement> listentity)
        {
            int intRet = -1;
            try
            {
                DBManager helper = new DBManager();
                string sql;
                foreach (EntityOaAnnouncement ann in listentity)
                {
                    if (ann.AnnGoroup == "已收取")
                    {
                        sql = " delete from Oa_announcement_receive where Oanctr_Buser_id='" + ann.ArReceiverId + "' and Oanctr_Oanct_id =" + ann.AnctId;
                    }
                    else
                    {
                        sql = " update Oa_announcement set del_flag=1 where Oanct_id=" + ann.AnctId;
                    }
                    intRet = helper.ExecCommand(sql);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return intRet;
        }

        public List<EntityOaAnnouncement> GetAnnouncementData(string userInfoId, string subject, string publisherName, DateTime dateFrom, DateTime dateTo)
        {
            string strSqlSend =string.Format(@"SELECT *,null as receiver_user_id,null as receiver_date,'已发送' as annGoroup,null as ReadFlag,0 as isselected 
FROM Oa_announcement 
where del_flag=0 AND Oanct_publish_user_id='{0}' AND Oanct_publish_date between '{1}' and '{2}'  ", userInfoId, 
dateFrom.Date.ToString("yyyy-MM-dd HH:mm:ss"), 
dateTo.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));

            string strSqlReceive =string.Format(@"select AA.*,AR.Oanctr_Buser_id,AR.Oanctr_date,'已收取' as annGoroup
,(case when ar.Oanctr_date is null then '未读' else '已读' end) as ReadFlag,0 as isselected 
from Oa_announcement_receive AR
inner JOIN Oa_announcement AA ON AR.Oanctr_Oanct_id=AA.Oanct_id
WHERE AR.Oanctr_Buser_id='{0}' and AA.Oanct_publish_date between '{1}' and '{2}' ", userInfoId, 
dateFrom.Date.ToString("yyyy-MM-dd HH:mm:ss"), 
dateTo.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));

            if (!string.IsNullOrEmpty(subject))
            {
                strSqlSend += " and Oanct_title like '%" + subject + "%' ";
                strSqlReceive += " and AA.Oanct_title like '%" + subject + "%' ";
            }
            if (!string.IsNullOrEmpty(publisherName))
            {
                strSqlSend += " and Oanct_publish_user_name  like '%" + publisherName + "%' ";
                strSqlReceive += " and AA.Oanct_publish_user_name  like '%" + publisherName + "%' ";
            }

            string strSql = "select p.* from(" + strSqlSend + " union all " + strSqlReceive + ")p order by p.Oanct_publish_date desc ";

            try
            {
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(strSql);
                List<EntityOaAnnouncement> list = EntityManager<EntityOaAnnouncement>.ConvertToList(dt).OrderBy(i => i.AnctId).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaAnnouncement>();
            }
        }


        public List<EntityOaAnnouncement> GetLastUnReadAnnouncement(string userInfoId)
        {
            DataTable dt = new DataTable();

            string strSql =string.Format(@" select TOP 1 AA.*,AR.Oanctr_Buser_id,AR.Oanctr_date,'已收取' as annGoroup 
from Oa_announcement_receive AR
inner JOIN Oa_announcement AA ON AR.Oanctr_Oanct_id=AA.Oanct_id
WHERE AR.Oanctr_Buser_id='{0}' and AR.Oanctr_date is null ORDER BY AA.Oanct_publish_date DESC  ", userInfoId);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                List<EntityOaAnnouncement> list = EntityManager<EntityOaAnnouncement>.ConvertToList(dt).OrderBy(i => i.AnctId).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaAnnouncement>();
            }
        }

        public List<EntityOaAnnouncement> GetSingleAnnouncementData(string userInfoId, int annID)
        {
            DataTable dt = new DataTable();
            string strSql =string.Format(@"SELECT *,null as receiver_user_id,null as receiver_date,'已发送' as annGoroup,null as ReadFlag,0 as isselected 
FROM Oa_announcement 
where Oanct_id='{0}' AND Oanct_publish_user_id='{1}'   
union all 
select AA.*,AR.Oanctr_Buser_id,AR.Oanctr_date,'已收取' as annGoroup
,(case when ar.Oanctr_date is null then '未读' else '已读' end) as ReadFlag,0 as isselected 
from Oa_announcement_receive AR
inner JOIN Oa_announcement AA ON AR.Oanctr_Oanct_id=AA.Oanct_id
WHERE AR.Oanctr_Oanct_id='{0}' and AR.Oanctr_Buser_id='{1}' ", annID,userInfoId);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                List<EntityOaAnnouncement> list = EntityManager<EntityOaAnnouncement>.ConvertToList(dt).OrderBy(i => i.AnctId).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaAnnouncement>();
            }
        }

        public List<EntityOaAnnouncement> GetUnReadAnnouncement(string userInfoId)
        {
            DataTable dt = new DataTable();

            string strSql =string.Format(@" select AA.*,AR.Oanctr_Buser_id,AR.Oanctr_date,'已收取' as annGoroup 
from Oa_announcement_receive AR
inner JOIN Oa_announcement AA ON AR.Oanctr_Oanct_id=AA.Oanct_id
WHERE AR.Oanctr_Buser_id='{0}' and AR.Oanctr_date is not null ", userInfoId);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                List<EntityOaAnnouncement> list = EntityManager<EntityOaAnnouncement>.ConvertToList(dt).OrderBy(i => i.AnctId).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaAnnouncement>();
            }
        }

        public int[] GetUnReadAnnouncementCount(List<EntityOaAnnouncementReceive> listReceive, string userInfoId)
        {
            int[] intArry = new int[2];
            intArry[0] = 0;
            intArry[1] = 0;
            try
            {
                if (listReceive != null && listReceive.Count > 0)
                {
                    List<EntityOaAnnouncementReceive> list = listReceive.Where(w => w.ReceiverUserId == userInfoId && w.ReceiverType == 1).ToList();

                    if (list.Count > 0)
                    {
                        intArry[0] = Convert.ToInt32(list[0].ReceiverNum);
                    }
                    list = listReceive.Where(w => w.ReceiverUserId == userInfoId && w.ReceiverType == 2).ToList();

                    if (list.Count > 0)
                    {
                        intArry[1] = Convert.ToInt32(list[0].ReceiverNum);
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return intArry;
        }

        public string IsIISAvailable()
        {
            return "HelloWorlds";
        }

        public bool IsNeedShowAnnouncement(string userInfoId, int minutes)
        {
            try
            {
                string sql = string.Format(@"Select ISNULL(count(1),0) 
from Oa_announcement_receive AR
inner JOIN Oa_announcement AA ON AR.Oanctr_Oanct_id=AA.Oanct_id
WHERE AR.Oanctr_Buser_id='{0}' AND AR.Oanctr_date is  null and DATEDIFF(MI,Oanct_publish_date,GETDATE())<'{1}'   ", userInfoId,minutes);
                DBManager helper = new DBManager();
                object obj = helper.ExecCommand(sql);
                return Convert.ToInt32(obj) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public int SaveAnnouncementData(EntityOaAnnouncement entityAnnouncement, List<string> reveiverList)
        {
            DBManager helper = new DBManager();
            int annID = GetMaxAnnID();
            string strSql =string.Format(@"insert into Oa_announcement
                                        (
	                                        Oanct_id,
	                                        Oanct_title,
	                                        Oanct_content,
                                            Oanct_publish_user_id,
                                            Oanct_publish_user_name,
                                            Oanct_publish_date,
                                            Oanct_reciver_name,
                                            Oanct_type,
                                            del_flag
                                        )
                                        values
                                        ( '{0}', '{1}','{2}','{3}','{4}','{5}', '{6}', '{7}','{8}')",
                                        annID, entityAnnouncement.AnctTitle,entityAnnouncement.AnctContent, entityAnnouncement.AnctPublishUserId,entityAnnouncement.AnctPublishUserName,
                                        entityAnnouncement.AnctPublishDate.ToString("yyyy-MM-dd HH:mm:ss"), entityAnnouncement.AnctReciverName, entityAnnouncement.AnctType,0);


            try
            {
           

                int intRet = helper.ExecCommand(strSql);

                if (intRet == 1)
                {
                    foreach (string receiveID in reveiverList)
                    {
                        strSql = string.Format(@"insert into Oa_announcement_receive
                                        (Oanctr_Oanct_id,Oanctr_Buser_id)
                                        values
                                        ('{0}','{1}')", annID, receiveID);
                        helper.ExecCommand(strSql);
                    }
                    //AnnuncemenCache.Current.Refresh();
                }
                return annID;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return -1;
        }

        public void SetReadFlag(string userInfoId, int annID)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql =string.Format("update Oa_announcement_receive set Oanctr_date=getdate() where Oanctr_Oanct_id='{0}' and Oanctr_Buser_id='{1}' ", annID, userInfoId);
                helper.ExecCommand(sql);
               // AnnuncemenCache.Current.Refresh();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 把xml内容转成指定Datatable
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ConvertXmlToDatatable(string strXml, string tableName, out DataTable dt)
        {
            bool bln = false;
            dt = null;
            try
            {
                if (strXml != null && strXml.Length > 0)
                {
                    strXml = strXml.Trim();
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXml);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains(tableName) && ds.Tables[tableName] != null)
                    {
                        dt = ds.Tables[tableName].Copy();
                        bln = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(GetType().FullName, new Exception("把xml内容转成指定Datatable,遇到错误！\r\n" + ex.ToString()));
            }
            return bln;
        }

        /// <summary>
        /// 获取当前最大的ID值
        /// </summary>
        /// <returns></returns>
        public int GetMaxAnnID()
        {
            int strMaxID;

            string strSql = @"select max(dd.Oanct_id) ancOanct_idt_id from Oa_announcement dd";

            DBManager helper = new DBManager();
            object obj = helper.ExecCommand(strSql);
            DataTable table = helper.ExecuteDtSql(strSql);
            if (string.IsNullOrEmpty(table.Rows[0]["ancOanct_idt_id"].ToString()))
            {
                strMaxID = 1;
            }
            else
            {
                strMaxID = Convert.ToInt32(table.Rows[0]["ancOanct_idt_id"]) + 1;
            }
            return strMaxID;
        }

       

        public bool IsNeedShowHo(string ctypeID)
        {
            try
            {
                DateTime dtNow = DateTime.Now;
                DBManager helper = new DBManager();

                string sql = string.Format(@"select * from dict_hand_over where ho_type_id='{0}' ", ctypeID);


                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicHandOver> info = EntityManager<EntityDicHandOver>.ConvertToList(dt).OrderBy(i => i.HoId).ToList();

                if (info != null && info.Count>0)
                {
                    string inter = info[0].HoTimeInter;

                    int mins;

                    if (!int.TryParse(info[0].HoTimeInter, out mins))
                    {
                        mins = 10;
                    }
                    if (!string.IsNullOrEmpty(info[0].HoTime1))
                    {
                        DateTime dt1 = Convert.ToDateTime(info[0].HoTime1);

                        if (Math.Abs(dt1.Subtract(dtNow).TotalMinutes) <= mins)
                        {
                            return true;
                        }
                    }

                    if (!string.IsNullOrEmpty(info[0].HoTime2))
                    {
                        DateTime dt1 = Convert.ToDateTime(info[0].HoTime2);

                        if (Math.Abs(dt1.Subtract(dtNow).TotalMinutes) <= mins)
                        {
                            return true;
                        }
                    }

                    if (!string.IsNullOrEmpty(info[3].HoTime3))
                    {
                        DateTime dt1 = Convert.ToDateTime(info[3].HoTime3);

                        if (Math.Abs(dt1.Subtract(dtNow).TotalMinutes) <= mins)
                        {
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
            return false;
        }

        //public List<EntitySysUser> GetAllUser(string hosID)
        //{
        //    DataTable dt = new DataTable();
        //    string sql = BuildHospitalSqlWhere("p.user_org_id", hosID);
        //    string strSql = string.Format(@"select 0 as isselected,p.user_id,
        //                               p.user_name,
        //                               p.user_loginid,
        //                               p.user_type,
        //                               p.wb_code,
        //                               p.py_code
        //                        from   Sys_user p       
        //                        where  p.del_flag = 0  {0}", sql);
        //    try
        //    {
        //        DBManager helper = new DBManager();
        //        dt = helper.ExecuteDtSql(strSql);
        //        List<EntitySysUser> list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.UserId).ToList();
        //        return list;

        //    }
        //    catch (Exception ex)
        //    {
        //        Lib.LogManager.Logger.LogException(ex);
        //        return new List<EntitySysUser>();
        //    }
        //}


        public List<EntityDicPubDept> GetPowerUserDepart(string hosID)
        {
            DataTable dt = new DataTable();
            string sql = BuildHospitalSqlWhere("Ddept_Dorg_id", hosID);
            string strSql = String.Format(@"select Ddept_id userId,Ddept_id,Ddept_name, -1 user_id,'科室：'+Ddept_name userName
from Dict_dept where 1=1 {0} union select Base_user_dept.Bud_Ddept_id +'^'+ Base_user.Buser_Id userId,
Dict_dept.Ddept_id,Ddept_name,Base_user.Buser_Id user_id,Buser_Name userName from Dict_dept 
join Base_user_dept on Dict_dept.Ddept_id =Base_user_dept.Bud_Ddept_id join Base_user on Base_user_dept.Bud_Buser_id =Base_user.Buser_Id where 1=1 {0}", sql);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                return ConvertToEntitys(dt);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubDept>();
            }
        }

        public List<EntityDicPubDept> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubDept> list = new List<EntityDicPubDept>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicPubDept depart = new EntityDicPubDept();
                depart.DeptId = item["Ddept_id"].ToString();
                depart.DeptName = item["Ddept_name"].ToString();
                depart.UserId = item["userId"].ToString();
                int infoid = -1;
                if (item["user_id"] != null && item["user_id"] != DBNull.Value)
                    int.TryParse(item["user_id"].ToString(), out infoid);
                depart.UserInfoId = infoid;
                depart.UserName = item["userName"].ToString();
                list.Add(depart);
            }
            return list.OrderBy(i => i.UserId).ToList();
        }

        public static string BuildHospitalSqlWhere(string column,string hosID)
        {
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }


        public List<EntityOaAnnouncementReceive> GetAnnouncementCache()
        {
            List<EntityOaAnnouncementReceive> list = new List<EntityOaAnnouncementReceive>();
            try
            {
                DBManager helper = new DBManager();
                string sql= @"Select count(Oanctr_Buser_id) num,Oanctr_Buser_id,1 AS messagetype 
from Oa_announcement_receive 
where  Oanctr_date is  NULL GROUP BY Oanctr_Buser_id
UNION all
select count(MessageOwer) num,MessageOwer userInfoId,2 AS messagetype 
from sysmessage WHERE
MessageOwerType='-2' AND ReadDate IS null  GROUP BY MessageOwer ";
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityOaAnnouncementReceive>.ConvertToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

    }
}
