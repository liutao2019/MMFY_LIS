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
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSamSave))]
    public class DaoSamSave : IDaoSamSave
    {
        public int AuditSamStoreRack(EntitySampStoreRack entity)
        {
            EntityResponse response = new EntityResponse();
            IDaoDic<EntitySampStoreRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntitySampStoreRack>>();
            int k = 0;

            entity.SrAuditDate = DateTime.Now; // ss_chk_date
            entity.SrStoreDate = DateTime.Now; //ss_store_date

            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                bool rack = dao.Update(entity);
                if (rack)
                {
                    k = 1;
                }
            }

            return k;
        }

        public int DeleteBcSign(string barCode, string bcStatus)
        {
            DBManager helper = new DBManager();

            int intRet = -1;

            string strSql = string.Format(@"delete from  Sample_process_detial  where Sproc_Sma_bar_code='{0}' and Sproc_status='{1}' ", barCode, bcStatus);

            try
            {
                helper.ExecSql(strSql);
                intRet = 1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 DeleteBcSign", ex);
            }

            return intRet;
        }

        public List<EntityDicSampStoreArea> GetCups()
        {
            EntityResponse response = new EntityResponse();
            IDaoDic<EntityDicSampStoreArea> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreArea>>();
            try
            {
                Object obj = null;
                List<EntityDicSampStoreArea> list = dao.Search(obj);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetCups", ex);
                return new List<EntityDicSampStoreArea>();
            }
        }

        public List<EntityDicSampTubeRack> GetDictRackInfo(string rackbarcode)
        {
            try
            {
                string strSql = string.Format(@"select  Dict_sample_tube_rack.*,
Sample_store_rack.Ssr_id,
isnull(Sample_store_rack.Ssr_status,'') sr_status,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_audit_date,
Sample_store_rack.Ssr_amount,
Dict_tube_rack.Dtrack_name cus_name, --赋予别名
Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_profession.Dpro_name,
--usestatus_xy,
0 as isselected
from   Dict_sample_tube_rack
left join Sample_store_rack
on  Sample_store_rack.Ssr_Drack_id = Dict_sample_tube_rack.Drack_id
inner join Dict_tube_rack
on  Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession
on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
where  Dict_sample_tube_rack.Drack_barcode='{0}' and isnull(Dict_sample_tube_rack.del_flag,'0')<>'1' "
                                   , rackbarcode);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicSampTubeRack> list = EntityManager<EntityDicSampTubeRack>.ConvertToList(dt).OrderBy(i => i.RackId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampTubeRack>();
            }
        }

        public List<EntityDicSampTubeRack> GetDictRackListForSave(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            try
            {
                string strSql = string.Format(@"select  Dict_sample_tube_rack.*,
Sample_store_rack.Ssr_id,
Sample_store_rack.Ssr_status,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_audit_date,
Sample_store_rack.Ssr_amount,

Dict_tube_rack.Dtrack_name cus_name,  --赋予别名
Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_profession.Dpro_name,
--usestatus_xy,
0 as isselected
from   Dict_sample_tube_rack
left join Sample_store_rack
on  Sample_store_rack.Ssr_Drack_id = Dict_sample_tube_rack.Drack_id
inner join Dict_tube_rack
on  Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession
on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
where  Sample_store_rack.Ssr_status = 5  and isnull(Dict_sample_tube_rack.del_flag,'0')<>'1'   
union all 
select  Dict_sample_tube_rack.*,
Sample_store_rack.Ssr_id,
Sample_store_rack.Ssr_status,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_audit_date,
Sample_store_rack.Ssr_amount,
Dict_tube_rack.Dtrack_name cus_name,--赋予别名
Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_profession.Dpro_name,
--usestatus_xy,
0 as isselected
from   Dict_sample_tube_rack
left join Sample_store_rack
on  Sample_store_rack.Ssr_Drack_id = Dict_sample_tube_rack.Drack_id
inner join Dict_tube_rack
on  Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession
on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
where  Sample_store_rack.Ssr_status != 20 and Sample_store_rack.Ssr_status != 5   
and isnull(Dict_sample_tube_rack.del_flag,'0')<>'1' 
and (Dict_sample_tube_rack.Drack_createtime  between '{0}' and '{1}' or Sample_store_rack.Ssr_audit_date between '{2}' and '{3}') "
                            , dateTimeFrom.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                              dateTimeTo.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"),
                              dateTimeFrom.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                              dateTimeTo.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicSampTubeRack> list = EntityManager<EntityDicSampTubeRack>.ConvertToList(dt).OrderBy(i => i.RackId).ToList();

                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetDictRackList", ex);
                return new List<EntityDicSampTubeRack>();
            }
        }

        public List<EntityDicSampStore> GetIceBox()
        {
            IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
            try
            {
                Object obj = null;
                List<EntityDicSampStore> list = dao.Search(obj);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetIceBox", ex);
                return new List<EntityDicSampStore>();
            }
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
where  Sample_store_detail.Ssdt_status <> 20  
and isnull(Sample_store_detail.Ssdt_bar_code,'')<>''    ";
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

        public int GetSamRackStatus(string strSsid)
        {
            try
            {
                string sql = string.Format(@"Select Ssr_status from Sample_store_rack where Ssr_id=@sr_id ");

                DBManager helper = new DBManager();

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@sr_id ", strSsid));

                object obj = helper.SelOne(sql, paramHt);

                if (obj == null || obj == DBNull.Value)
                {
                    return -1;
                }
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetSamRackStatus", ex);
            }
            return -1;
        }

        public int ModifyRackStatus(EntityDicSampTubeRack entity)
        {
            int intRet = -1;
            try
            {

                string strSql = string.Format(@"update Dict_sample_tube_rack
                                set
                                    Drack_status = {0}
                                where Drack_id = '{1}' "
                                    , entity.RackStatus, entity.RackId);

                DBManager helper = new DBManager();

                helper.ExecSql(strSql);
                intRet = 1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 ModifyRackStatus", ex);
            }

            return intRet;
        }

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
    }
}
