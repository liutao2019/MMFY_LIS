using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampStock))]
    public class DaoSamRack : IDaoSampStock
    {
        public List<EntityDicSampTubeRack> GetDictRackList(DateTime dateFrom, DateTime dateEditTo)
        {
            try
            {
                String sql = string.Format(@"select  Dict_sample_tube_rack.*,
Sample_store_rack.Ssr_id,
Sample_store_rack.Ssr_status,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_audit_date,
Sample_store_rack.Ssr_amount,
Dict_tube_rack.Dtrack_code as cus_code,
Dict_tube_rack.Dtrack_name,
Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_profession.Dpro_name,
CAST(isnull(Sample_store_rack.Ssr_amount,0) AS VARCHAR)+'/'+CAST((Dict_tube_rack.Dtrack_x_amount*Dict_tube_rack.Dtrack_y_amount)AS VARCHAR) AS usestatus,
0 as isselected
from   Dict_sample_tube_rack
left join Sample_store_rack on Sample_store_rack.Ssr_Drack_id = Dict_sample_tube_rack.Drack_id
inner join Dict_tube_rack on  Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
where  Sample_store_rack.Ssr_status <> 20 and  Sample_store_rack.Ssr_status <> 15 and  
Sample_store_rack.Ssr_status <> 10  and isnull(Dict_sample_tube_rack.del_flag,'0')<>'1'
and Dict_sample_tube_rack.Drack_createtime  between '{0}' and '{1}' ", dateFrom.ToString("yyyy-MM-dd HH:mm:ss"), dateEditTo.ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicSampTubeRack> list = EntityManager<EntityDicSampTubeRack>.ConvertToList(dt).OrderBy(i => i.RackId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampTubeRack>();
            }
        }

        public List<EntityDicSampStoreStatus> GetSamManageStatus()
        {
            try
            {
                String sql = string.Format(@"select Dstau_id,
                                   Dstau_name
                            from   Dict_samp_store_status");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicSampStoreStatus> list = EntityManager<EntityDicSampStoreStatus>.ConvertToList(dt).OrderBy(i => i.StauId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampStoreStatus>();
            }
        }

        public List<EntityPidReportMain> GetPatientsInfo(string barcode)
        {
            try
            {
                String sql = string.Format(@"select 
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_bar_code,
Pat_lis_main.Pma_status,
Pma_com_name
from   Pat_lis_main
where  Pat_lis_main.Pma_bar_code = '{0}' ", barcode);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityPidReportMain> list = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

       

        /////////*****************************下面还没有改过来对应med库的新表和新字段*************************/////////////////////////////////////////////////////
        public string GetWhere(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex)
        {
            string strWhere = string.Format(
    "  Pat_lis_main.Pma_in_date >= DateAdd(d,0,'{0}') and Pat_lis_main.Pma_in_date < DateAdd(d,1,'{0}') ",
    date);

            strWhere += string.Format(" and Pma_Ditr_id = '{0}' ", BatchItr);
            if (selectIndex == 0)
            {
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) >= " + SamFrom + " ";
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) <= " + SamTo + " ";
            }
            if (selectIndex == 1)
            {
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) >= " + SamFrom + " ";
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) <= " + SamTo + " ";
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
                String sql = @" select distinct  0 as isselected,
cast(Pat_lis_main.Pma_sid as int) as rep_sid_len,
Sample_store_detail.*,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
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

        public string GetMaxRack()
        {
            try
            {
                String sql = @"select max(Drack_id) from Dict_sample_tube_rack";

                DBManager helper = new DBManager();

                string MaxId = helper.ExecuteDtSql(sql).Rows[0][0].ToString();

                return MaxId;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return "";
            }
        }

        public string GetMaxRackCode()
        {
            try
            {
                String sql = @"select max(Drack_code) from Dict_sample_tube_rack where ISNUMERIC(Drack_code)=1";

                DBManager helper = new DBManager();

                string MaxId = helper.ExecuteDtSql(sql).Rows[0][0].ToString();

                return MaxId;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return "";
            }
        }

        public string GetNextMaxBarCode()
        {
            DBManager helper = new DBManager();
            string sql;

            sql = "update Base_barcode_generator set Bbargen_no=Bbargen_no+1 where  Bbargen_id='5' ";
            helper.ExecSql(sql);

            sql = "select Bbargen_no from Base_barcode_generator  where Bbargen_id='5' ";
            DataTable obj = helper.ExecSel(sql);
            var intSeq = Int64.Parse(obj.Rows[0][0].ToString());
            string seq;
            if (intSeq > 999999)
            {
                seq = intSeq.ToString("00000000");
            }
            else
            {
                seq = intSeq.ToString("000000");
            }
            int jy = 0;
            foreach (char c in seq.ToArray())
            {
                jy += int.Parse(c.ToString());
            }
            return jy.ToString("00") + seq;
        }

        public bool IsRaclBarCodeExists(string barCode, bool isExists)
        {
            try
            {
                string sql = string.Format(@"Select ISNULL(count(1),0) from Dict_sample_tube_rack 
                                    where Drack_barcode='{0}'", barCode);
                if (isExists)
                {
                    sql += "and Drack_print_flag=1";
                }

                DBManager helper = new DBManager();

                string obj = helper.ExecuteDtSql(sql).Rows[0][0].ToString();
                return Convert.ToInt32(obj) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool IsBarCodeUsing(string barCode)
        {
            try
            {
                string sql = string.Format(@"Select ISNULL(count(1),0) from Sample_store_detail 
                                    where Ssdt_bar_code='{0}' ", barCode);

                DBManager helper = new DBManager();

                string obj = helper.ExecuteDtSql(sql).Rows[0][0].ToString();
                return Convert.ToInt32(obj) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityPidReportDetail> GetAppendBarCode(string barCode, string patId)
        {
            try
            {
                string sql = string.Format(@"select isnull(Pdet_bar_code, '') Pdet_bar_code
                            from Pat_lis_detail
                            where Pat_lis_detail.Pdet_id ='{1}' and Pat_lis_detail.Pdet_bar_code<>'{0}' ", barCode, patId);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityPidReportDetail> list = EntityManager<EntityPidReportDetail>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportDetail>();
            }
        }

        public List<EntitySampStoreDetail> GetSimpleRackDetail()
        {
            try
            {
                string sql = string.Format(@"select Sample_store_detail.Ssdt_Ssr_id,
Sample_store_detail.Ssdt_bar_code,
Sample_store_detail.Ssdt_x,
Sample_store_detail.Ssdt_y,
Sample_store_detail.Ssdt_seqno,
Sample_store_detail.Ssdt_status 
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
                int id = IdentityHelper.GetMedIdentity("Sample_store_detail");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ssdt_Ssr_id", id);
                values.Add("Ssdt_bar_code", SampDetail.DetBarCode);
                values.Add("Ssdt_x", SampDetail.DetX);
                values.Add("Ssdt_y", SampDetail.DetY);
                values.Add("Ssdt_seqno", SampDetail.DetSeqno);
                values.Add("Ssdt_status", SampDetail.DetStatus);
                values.Add("Ssdt_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                helper.InsertOperation("Sample_store_detail", values);

                SampDetail.DetId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public string GetMaxSamRackID()
        {
            try
            {
                String sql = @"select max(Ssr_id) from Sample_store_rack";

                DBManager helper = new DBManager();

                string MaxId = helper.ExecuteDtSql(sql).Rows[0][0].ToString();

                return MaxId;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return "";
            }
        }

        public bool AddSampProcessDetail(EntitySampProcessDetail SampDetail)
        {
            try
            {
                //int id = IdentityHelper.GetMedIdentity("Samp_process_detial");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("proc_no", id);
                values.Add("Sproc_date", SampDetail.ProcDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Sproc_user_id", SampDetail.ProcUsercode);
                values.Add("Sproc_user_name", SampDetail.ProcUsername);
                values.Add("Sproc_status", SampDetail.ProcStatus);
                values.Add("Sproc_Sma_bar_id", SampDetail.ProcBarno);
                values.Add("Sproc_Sma_bar_code", SampDetail.ProcBarcode);
                values.Add("Sproc_place", SampDetail.ProcPlace);
                values.Add("Sproc_content", SampDetail.ProcContent);

                helper.InsertOperation("Sample_process_detial", values);

                //SampDetail.ProcNo = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteSampProcessDetail(string barCode, string bcStatus)
        {
            try
            {
                DBManager helper = new DBManager();


                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Sproc_Sma_bar_code", barCode);
                keys.Add("Sproc_status", bcStatus);

                helper.DeleteOperation("Sample_process_detial", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

       
        public bool ModifySamStoreRack(EntitySampStoreRack entity)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ssr_status", entity.SrStatus);
                values.Add("Ssr_audit_user_name", entity.SrAuditUserName);
                values.Add("Ssr_audit_user_id", entity.SrAuditUserId);
                values.Add("Ssr_audit_date", entity.SrAuditDate.Year == 1 ? (DateTime?)null : entity.SrAuditDate);
                values.Add("Ssr_store_user_name", entity.SrStoreUserName);
                values.Add("Ssr_store_user_code", entity.SrStoreUserCode);
                values.Add("Ssr_store_date", entity.SrStoreDate.Year == 1 ? (DateTime?)null : entity.SrStoreDate);
                values.Add("Ssr_Dpos_id", entity.SrPlace);
                values.Add("Ssr_Dstore_id", entity.SrStoreId);
                values.Add("Ssr_amount", entity.SrAmount);
                values.Add("Ssr_Destroy_name", entity.SrDestroyName);
                values.Add("Ssr_Destroy_code", entity.SrDestroyCode);
                values.Add("sr_Destroy_date", entity.SrDestroyDate.Year == 1 ? (DateTime?)null : entity.SrDestroyDate);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ssr_id", entity.SrId);
                keys.Add("Ssr_Drack_id", entity.SrRackId);

                helper.UpdateOperation("Sample_store_rack", values, keys);

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
