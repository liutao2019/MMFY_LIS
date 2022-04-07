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
    [Export("wf.plugin.wf", typeof(IDaoOaHoRecord))]
    public class DaoOaHoRecord : IDaoOaHoRecord
    {

        public List<EntityHoRecord> GetDtNullResData(string ctypeID)
        {
            DataTable dt = new DataTable();
            List<EntityHoRecord> list = new List<EntityHoRecord>();
            try
            {
                string sql = string.Format(@" select top 1 * from Lis_ho_record with(nolock)
where Lhr_hand_time>(getdate()-1)
and Lhr_Dpro_id='{0}'
and Lhr_ext9 is not null and Lhr_ext9<>'' 
order by Lhr_hand_time desc
", ctypeID);


                DBManager helper = new DBManager();

                DataTable table = helper.ExecuteDtSql(sql);

                if (table != null && table.Rows.Count > 0 && table.Rows[0]["Lhr_ext9"].ToString().Length > 0)
                {
                    if (ConvertXmlToDatatable(table.Rows[0]["Lhr_ext9"].ToString(), "dtNullRes", out dt))
                    {
                        list = EntityManager<EntityHoRecord>.ConvertToList(dt).OrderBy(i => i.HrId).ToList();
                        return list;
                    }
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityHoRecord>();
            }
            list = EntityManager<EntityHoRecord>.ConvertToList(dt).OrderBy(i => i.HrId).ToList();
            return list;
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


        public EntityHoRecord GetHandoverStat(DateTime dtFrom, DateTime dtTo, string ctypeID)
        {
            string sql = string.Format(@"SELECT SUM(Lhr_totalRecv_count) AS Lhr_totalRecv_count,
SUM(Lhr_noqutity_count) AS Lhr_noqutity_count,
SUM(Lhr_report_count) AS Lhr_report_count,
SUM(Lhr_unreport_count) AS Lhr_unreport_count,
SUM(Lhr_urgent_count) AS Lhr_urgent_count FROM (
SELECT COUNT(1) AS Lhr_totalRecv_count,0 AS Lhr_noqutity_count 
,0 AS Lhr_report_count,0 AS Lhr_unreport_count,0 AS Lhr_urgent_count
FROM Sample_main with(NOLOCK) WHERE Sma_status_id NOT IN ('0','1','2','3','4','9') 
AND Sample_main.Sma_date >='{0}' AND Sample_main.Sma_date<='{1}'
AND Sma_type='{2}'
UNION ALL 
SELECT 0 AS Lhr_totalRecv_count,COUNT(1) AS Lhr_noqutity_count 
,0 AS Lhr_report_count,0 AS Lhr_unreport_count,0 AS Lhr_urgent_count
FROM Sample_main with(NOLOCK) WHERE Sma_status_id  IN ('9') 
AND Sample_main.Sma_date >='{0}' AND Sample_main.Sma_date<='{1}'
AND Sma_type='{2}'
UNION ALL 
SELECT 0 AS Lhr_totalRecv_count,0 AS Lhr_noqutity_count 
,COUNT(1) AS Lhr_report_count,0 AS Lhr_unreport_count,0 AS Lhr_urgent_count
FROM Pat_lis_main with(NOLOCK)
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
WHERE Pma_status  IN (2,4) 
AND Pma_in_date >='{0}' AND Pma_in_date<='{1}'
AND Dict_itr_instrument.Ditr_lab_id='{2}'
UNION ALL 
SELECT 0 AS Lhr_totalRecv_count,0 AS Lhr_noqutity_count 
,0 AS Lhr_report_count,COUNT(1) AS Lhr_unreport_count,0 AS Lhr_urgent_count
FROM Pat_lis_main with(NOLOCK)
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
WHERE Pma_status not IN (2,4) 
AND Pma_in_date >='{0}' AND Pma_in_date<='{1}'
AND Dict_itr_instrument.Ditr_lab_id='{2}'
UNION ALL 
SELECT 0 AS Lhr_totalRecv_count,0 AS Lhr_noqutity_count 
,0 AS Lhr_report_count, 0 AS Lhr_unreport_count,COUNT(1) AS Lhr_urgent_count
FROM Pat_lis_main with(NOLOCK)
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
WHERE Pma_urgent_flag IS NOT NULL AND Pma_urgent_flag<>0 
AND Pma_in_date >='{0}' AND Pma_in_date<='{1}'
AND Dict_itr_instrument.Ditr_lab_id='{2}')a", dtFrom.Date.ToString("yyyy-MM-dd HH:mm:ss")
                                  , dtTo.ToString("yyyy-MM-dd HH:mm:ss"), ctypeID);
            DBManager helper = new DBManager();

            DataTable table = helper.ExecuteDtSql(sql);



            //接收后未审核的标本
            string sql2 = string.Format(@"select Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Dict_itm_combine.Dcom_name,
Rel_itm_combine_item.Rici_Dcom_id,
Rel_itm_combine_item.Rici_Ditm_id,
Rel_itm_combine_item.Rici_Ditm_ecode,
'' as reason,
'' as opinion,
 1 as msg_flag,
Dict_itr_instrument.Ditr_ename
from 
Pat_lis_main with(nolock)
inner join Pat_lis_detail with(nolock) on Pat_lis_detail.Pdet_id=Pat_lis_main.Pma_rep_id
inner join Rel_itm_combine_item with(nolock) on Pat_lis_detail.Pdet_Dcom_id=Rel_itm_combine_item.Rici_Dcom_id
inner join Dict_itm_combine with(nolock) on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id
inner join Dict_itr_instrument with(nolock) on Dict_itr_instrument.Ditr_id=Pat_lis_main.Pma_Ditr_id
where 
Pat_lis_main.Pma_in_date >='{0}' AND Pat_lis_main.Pma_in_date<='{1}'
and Pat_lis_main.Pma_status=0
AND Dict_itr_instrument.Ditr_lab_id='{2}'
and Dict_itr_instrument.Ditr_report_type in('1','2')
and not exists(select top 1 1 from Lis_result with(nolock) where Lis_result.Lres_Pma_rep_id=Pat_lis_detail.Pdet_id 
and Lis_result.Lres_Ditm_id=Rel_itm_combine_item.Rici_Ditm_id
and Lis_result.Lres_flag=1 and Lis_result.Lres_value is not null and Lis_result.Lres_value<>'')", dtFrom.Date.ToString("yyyy-MM-dd HH:mm:ss")
                                  , dtTo.ToString("yyyy-MM-dd HH:mm:ss"), ctypeID);

            DataTable table2 = helper.ExecuteDtSql(sql2);//未完成的标本

            string hr_ext9 = "";

            if (table2 != null && table2.Rows.Count > 0)
            {
                table2.TableName = "dtNullRes";
                hr_ext9 = dtDataToXml(table2);
            }

            EntityHoRecord info = new EntityHoRecord();

            if (table.Rows.Count > 0)
            {
                info.HrNoqutityCount = table.Rows[0]["Lhr_noqutity_count"].ToString();
                info.HrReportCount = table.Rows[0]["Lhr_report_count"].ToString();
                info.HrTotalRecvCount = table.Rows[0]["Lhr_totalRecv_count"].ToString();
                info.HrUnreportCount = table.Rows[0]["Lhr_unreport_count"].ToString();
                info.HrUrgentCount = table.Rows[0]["Lhr_urgent_count"].ToString();
                info.HrExt9 = hr_ext9;
            }

            return info;
        }

        public List<EntityHoRecord> GetHandoverList(DateTime dtFrom, DateTime dtTo)
        {
            try
            {
                string sql = string.Format(@"
SELECT Lis_ho_record.*,Dict_profession.Dpro_name AS typename,
p.Buser_name as hr_hand_name, p2.Buser_name as hr_recvconfirm_name
FROM Lis_ho_record 
LEFT JOIN Dict_profession ON Lis_ho_record.Lhr_Dpro_id=Dict_profession.Dpro_id
left join Base_user p on p.Buser_loginid=Lhr_hand_Buser_id
left join Base_user p2 on p2.Buser_loginid=Lhr_recvconfirm_Buser_id
WHERE Lhr_hand_time >='{0}' AND Lhr_hand_time<='{1}'", dtFrom.Date.ToString("yyyy-MM-dd HH:mm:ss")
                                      , dtTo.ToString("yyyy-MM-dd HH:mm:ss"));
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityHoRecord> list = EntityManager<EntityHoRecord>.ConvertToList(dt).OrderBy(i => i.HrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityHoRecord>();
            }
        }
        


        /// <summary>
        /// 将DataTable对象转换成XML字符串
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>XML字符串</returns>
        private string dtDataToXml(DataTable dt)
        {
            string strRvXML = "";

            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    XmlWt = new XmlTextWriter(ms, Encoding.UTF8);
                    XmlWt.Formatting = Formatting.None;
                    //获取ds中的数据
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    //UnicodeEncoding ucode = new UnicodeEncoding();
                    //string returnValue = ucode.GetString(temp).Trim();
                    UTF8Encoding utfcode = new UTF8Encoding();
                    string returnValue = utfcode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (System.Exception ex)
                {
                    Lib.LogManager.Logger.LogException("dtDataToXml", ex);
                }
                finally
                {
                    //释放资源
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return "";
            }

            return strRvXML;
        }


        public bool SaveHandoverInfo(EntityHoRecord info)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lhr_id", info.HrId);
                values.Add("Lhr_Dpro_id", info.HrTypeId);
                values.Add("Lhr_hand_Buser_id", info.HrHandCode);
                values.Add("Lhr_receive_Buser_id", info.HrReceiveCode);
                values.Add("Lhr_hand_time", info.HrHandTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lhr_totalRecv_count", info.HrTotalRecvCount);
                values.Add("Lhr_noqutity_count", info.HrNoqutityCount);
                values.Add("Lhr_report_count", info.HrReportCount);
                values.Add("Lhr_unreport_count", info.HrUnreportCount);
                values.Add("Lhr_urgent_count", info.HrUrgentCount);
                values.Add("Lhr_qc_flag", info.HrQcFlag);
                values.Add("Lhr_qc_reason", info.HrQcReason);
                values.Add("Lhr_itr_mflag", info.HrItrMflag);
                values.Add("Lhr_fault_Ditr_id", info.HrItrFaultId);
                values.Add("Lhr_fault_reason", info.HrItrFaultReason);
                values.Add("Lhr_fault_time", info.HrItrFaultTime?.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lhr_itr_judge", info.HrItrJudge);
                values.Add("Lhr_san_room", info.HrSanRoom);
                values.Add("Lhr_san_microbe", info.HrSanMicrobe);
                values.Add("Lhr_sp_complain", info.HrSpComplain);
                values.Add("Lhr_sp_hydro", info.HrSpHydro);
                values.Add("Lhr_sp_machine", info.HrSpMachine);
                values.Add("Lhr_sp_ifsam", info.HrSpIfsam);
                values.Add("Lhr_handcomfirm_Buser_id", info.HrHandcomfirmCode);
                values.Add("Lhr_recvconfirm_Buser_id", info.HrRecvconfirmCode);
                values.Add("Lhr_ext1", info.HrExt1);
                values.Add("Lhr_ext2", info.HrExt2);
                values.Add("Lhr_ext3", info.HrExt3);
                values.Add("Lhr_ext4", info.HrExt4);
                values.Add("Lhr_ext5", info.HrExt5);
                values.Add("Lhr_ext6", info.HrExt6);
                values.Add("Lhr_ext7", info.HrExt6);
                values.Add("Lhr_ext8", info.HrExt8);
                values.Add("Lhr_ext9", info.HrExt9);
                helper.InsertOperation("Lis_ho_record", values);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteHandover(string ho_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql = string.Format(@"delete from Lis_ho_record where Lhr_id='{0}'", ho_id);
                helper.ExecSql(strSql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
    
}
