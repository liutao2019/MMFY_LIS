using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampStoreDetail))]
    public class DaoSampStoreDetail : IDaoSampStoreDetail
    {

        public int ModifySamDetail(EntitySampStoreDetail entity)
        {
            int intRet = -1; 
            try
            {
                string strSql = string.Format(@"update Sample_store_detail
                                set
	                                Ssdt_status = {0}
                                where Ssdt_Ssr_id = '{1}' and Ssdt_bar_code = '{2}' "
                              , entity.DetStatus, entity.DetId, entity.DetBarCode);

                DBManager helper = new DBManager();

                helper.ExecSql(strSql);
                intRet = 1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        public List<EntitySampStoreDetail> GetRackDetail(string strSsid)
        {
            try
            {
                String sql = @" select distinct 0 as isselected,
Sample_store_detail.*,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_com_name,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '男*' end) Pma_pat_sex,
(case isnull(dbo.getAge(Pat_lis_main.Pma_pat_age_exp),'') when '' then '20*' else dbo.getAge(Pat_lis_main.Pma_pat_age_exp) end ) pid_age,
Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_pat_dept_name, 
Pat_lis_main.Pma_Dsam_id,
Pat_lis_main.Pma_status,

Sample_store_rack.Ssr_Dstore_id,

case when Pat_lis_main.Pma_serial_num is null or Pat_lis_main.Pma_serial_num='' then Pma_sid else Pat_lis_main.Pma_serial_num end  as rep_sid2,

Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_status,
Sample_store_rack.Ssr_Drack_id

from   Sample_store_detail
left join Pat_lis_main
on  Pat_lis_main.Pma_bar_code = Sample_store_detail.Ssdt_bar_code
left join Sample_store_rack on Sample_store_rack.Ssr_id = Sample_store_detail.Ssdt_Ssr_id
where  Sample_store_detail.Ssdt_status <> 20 and Sample_store_detail.Ssdt_bar_code is not null 
and Sample_store_detail.Ssdt_bar_code != ''  ";
                if (!string.IsNullOrEmpty(strSsid))
                {
                    sql += string.Format(@"and Sample_store_detail.Ssdt_Ssr_id  = '{0}' ", strSsid);
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySampStoreDetail> list = EntityManager<EntitySampStoreDetail>.ConvertToList(dt).OrderBy(i => i.DetId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySampStoreDetail>();
            }
        }

        public string GetWhere(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex)
        {
            string strWhere = string.Format(
    "  Pat_lis_main.Pma_in_date >= DateAdd(d,0,'{0}') and Pat_lis_main.Pma_in_date < DateAdd(d,1,'{0}') ",
    date);

            strWhere += string.Format(" and Pma_Ditr_id = '{0}' ", BatchItr);
            if (selectIndex == 0)
            {
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) >= " +  SamFrom + " ";
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) <= " + SamTo + " ";
            }
            if (selectIndex == 1)
            {
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) >= " + SamFrom +  " ";
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) <= " +  SamTo + " ";
            }

            return strWhere;
        }
        public int UpdateSamDetail(EntitySampStoreDetail entity)
        {
            int intRet = -1;
            string strSql = string.Format(@"update Sample_store_detail
                                set
	                                Ssdt_status = {0}
                                where Ssdt_Ssr_id = '{1}' and Ssdt_bar_code = '{2}'", entity.DetStatus, entity.DetId, entity.DetBarCode);
            DBManager helper = new DBManager();

            intRet = helper.ExecCommand(strSql);

            return intRet;

        }

        public List<EntitySampStoreDetail> GetSamDetail(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex)
        {
            string strWhere = GetWhere(date, BatchItr, SamFrom, SamTo, selectIndex);
            try
            {
                String sql = @" select distinct
cast(Pat_lis_main.Pma_sid as int) as rep_sid_len,
Sample_store_detail.*,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_pat_sex,
Pat_lis_main.Pma_com_name,
(case isnull(dbo.getAge(Pat_lis_main.Pma_pat_age_exp),'') when '' then '20*' 
else dbo.getAge(Pat_lis_main.Pma_pat_age_exp) end ) pid_age,

Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_pat_dept_name, 
Pat_lis_main.Pma_Dsam_id,
Pat_lis_main.Pma_status,

Pat_lis_main.Pma_serial_num,Pat_lis_main.Pma_sid,len(Pat_lis_main.Pma_sid),len(Pat_lis_main.Pma_serial_num),
Sample_store_rack.Ssr_Dstore_id,
case when Pat_lis_main.Pma_serial_num is null or Pat_lis_main.Pma_serial_num='' then Pat_lis_main.Pma_sid 
else Pat_lis_main.Pma_serial_num end  as rep_sid2,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_status,
Sample_store_rack.Ssr_Drack_id,
CAST(Sample_store_detail.Ssdt_x as VARCHAR)+'*'+CAST(Sample_store_detail.Ssdt_y as VARCHAR) AS det_xy

from   Pat_lis_main
left join Sample_store_detail
on  Pat_lis_main.Pma_bar_code = Sample_store_detail.Ssdt_bar_code
left join Sample_store_rack on Sample_store_rack.Ssr_id = Sample_store_detail.Ssdt_Ssr_id
where   " + strWhere + " order by rep_sid_len asc,Pat_lis_main.Pma_pat_name,Pat_lis_main.Pma_status,len(Pat_lis_main.Pma_serial_num),len(Pat_lis_main.Pma_sid),Pat_lis_main.Pma_sid desc, Pma_bar_code, Pat_lis_main.Pma_serial_num  ";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySampStoreDetail> list = EntityManager<EntitySampStoreDetail>.ConvertToList(dt).OrderBy(i => i.DetId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySampStoreDetail>();
            }
        }

        public List<EntitySampStoreDetail> GetSimpleRackDetail()
        {
            try
            {
                string sql = string.Format(@"select Ssdt_Ssr_id,
Ssdt_bar_code,
Ssdt_x,
Ssdt_y,
Ssdt_seqno,
Ssdt_status 
from  Sample_store_detail ");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySampStoreDetail> list = EntityManager<EntitySampStoreDetail>.ConvertToList(dt).OrderBy(i => i.DetId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySampStoreDetail>();
            }
        }

        public bool AddSampStoreDetail(EntitySampStoreDetail SampDetail)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ssdt_Ssr_id", SampDetail.DetId);
                values.Add("Ssdt_bar_code", SampDetail.DetBarCode);
                values.Add("Ssdt_x", SampDetail.DetX);
                values.Add("Ssdt_y", SampDetail.DetY);
                values.Add("Ssdt_seqno", SampDetail.DetSeqno);
                values.Add("Ssdt_status", SampDetail.DetStatus);
                values.Add("Ssdt_date", DateTime.Now);

                helper.InsertOperation("Sample_store_detail", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public int DeleteSampStoreDetail(string strSsid, string barcode)
        {
            int intRet = -1;
            string strSql = string.Format(@"Delete from  Sample_store_detail where  Ssdt_Ssr_id = '{0}' and Ssdt_bar_code = '{1}' and Ssdt_status=5 ", strSsid, barcode);
            DBManager helper = new DBManager();

            intRet = helper.ExecCommand(strSql);

            return intRet;
        }

       public int GetSampStoreDetailCount(string strSsid)
        {
            int intRet = -1;
            string strSql = string.Format(@"select ISNULL(count(1),0)  from Sample_store_detail where  Ssdt_Ssr_id = '{0}' and Ssdt_status<>20  ", strSsid);
            DBManager helper = new DBManager();

            intRet = helper.ExecCommand(strSql);

            return intRet;
        }
    }
}
