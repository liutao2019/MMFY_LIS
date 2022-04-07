using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSamSearch))]
    public class DaoSamSearch : IDaoSamSearch
    {
        public List<EntitySampStoreDetail> GetRackQueryData(EntityDicSamSearchParamter samSearchParam)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql = string.Format(
                    @"select DISTINCT Sample_store_detail.Ssdt_Ssr_id,
Sample_store_detail.Ssdt_bar_code,
Sample_store_detail.Ssdt_x,
Sample_store_detail.Ssdt_y,
Sample_store_detail.Ssdt_seqno,
Sample_store_detail.Ssdt_status,   
Sample_store_detail.Ssdt_date,
Sample_store_rack.Ssr_Drack_id,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '男*' end) Pma_pat_sex,
(case isnull(dbo.getAge(Pat_lis_main.Pma_pat_age_exp),'') when '' then '20*' else dbo.getAge(Pat_lis_main.Pma_pat_age_exp) end ) pid_age,
Pat_lis_main.Pma_com_name,
Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_pat_dept_name, 
Pat_lis_main.Pma_Dsam_id,
Pat_lis_main.Pma_sid,--样本号
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_status,
CAST(Sample_store_detail.Ssdt_x as VARCHAR)+'*'+CAST(Sample_store_detail.Ssdt_y as VARCHAR) AS det_xy,
Dict_profession.Dpro_name,
Dict_sample.Dsam_name,
Dict_sample_tube_rack.Drack_name,Dict_sample_tube_rack.Drack_barcode,Dict_sample_tube_rack.Drack_Dtrack_code,Dict_sample_tube_rack.Drack_colour,
Dict_sample_store.Dstore_name,Dict_sample_store_position.Dpos_name

from   Sample_store_detail
left join Pat_lis_main on  Pat_lis_main.Pma_bar_code = Sample_store_detail.Ssdt_bar_code
left join Sample_store_rack on Sample_store_rack.Ssr_id = Sample_store_detail.Ssdt_Ssr_id
left join Dict_sample_store on  Dict_sample_store.Dstore_id = Sample_store_rack.Ssr_Dstore_id
left join Dict_sample_store_position on  Dict_sample_store_position.Dpos_id = Sample_store_rack.Ssr_Dpos_id
left join Dict_sample_tube_rack on   Dict_sample_tube_rack.Drack_id=Sample_store_rack.Ssr_Drack_id 
left join Dict_profession on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
left join Dict_sample on  Dict_sample.Dsam_id = Pat_lis_main.Pma_Dsam_id
where   Sample_store_detail.Ssdt_date between '{0}' and '{1}'  "
                                , samSearchParam.DateTimeFrom.Value.Date.ToString("yyyy-MM-dd HH:mm:ss")
                                , samSearchParam.DateTimeTo.Value.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")
                                );
                ;
                if (!string.IsNullOrEmpty(samSearchParam.RackCtype))
                {
                    strSql += " and Dict_sample_tube_rack.Drack_Dpro_id = @rackCtype ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.IceID))
                {
                    strSql += " and Sample_store_rack.Ssr_Dstore_id = @iceID ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.CupID))
                {
                    strSql += " and Sample_store_rack.Ssr_Dpos_id = @cupID ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.RackBarcode))
                {
                    strSql += " and Dict_sample_tube_rack.Drack_barcode like @rackBarcode ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.SamBarcode))
                {
                    strSql += " and Sample_store_detail.Ssdt_bar_code like @samBarcode ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.PatInNo))
                {
                    strSql += " and Pat_lis_main.Pma_pat_in_no like @patInNo ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.PatName))
                {
                    strSql += " and Pat_lis_main.Pma_pat_name like @patName ";
                }
                if (!string.IsNullOrEmpty(samSearchParam.StoreMan))
                {
                    strSql += " and Sample_store_rack.Ssr_store_user_name like @storeMan ";
                }

                List<DbParameter> paramHt = new List<DbParameter>();

                if (!string.IsNullOrEmpty(samSearchParam.RackCtype))
                {
                    paramHt.Add(new SqlParameter("@rackCtype", samSearchParam.RackCtype));
                }
                if (!string.IsNullOrEmpty(samSearchParam.IceID))
                {
                    paramHt.Add(new SqlParameter("@iceID", samSearchParam.IceID));
                }
                if (!string.IsNullOrEmpty(samSearchParam.CupID))
                {
                    paramHt.Add(new SqlParameter("@cupID", samSearchParam.CupID));
                }
                if (!string.IsNullOrEmpty(samSearchParam.RackBarcode))
                {
                    paramHt.Add(new SqlParameter("@rackBarcode", samSearchParam.RackBarcode));
                }
                if (!string.IsNullOrEmpty(samSearchParam.SamBarcode))
                {
                    paramHt.Add(new SqlParameter("@samBarcode", samSearchParam.SamBarcode));
                }
                if (!string.IsNullOrEmpty(samSearchParam.PatInNo))
                {
                    paramHt.Add(new SqlParameter("@patInNo", samSearchParam.PatInNo));
                }
                if (!string.IsNullOrEmpty(samSearchParam.PatName))
                {
                    paramHt.Add(new SqlParameter("@patName", samSearchParam.PatName));
                }
                if (!string.IsNullOrEmpty(samSearchParam.StoreMan))
                {
                    paramHt.Add(new SqlParameter("@storeMan", samSearchParam.StoreMan));
                }

                DataTable dtSSDetail = helper.ExecuteDtSql(strSql, paramHt);

                List<EntitySampStoreDetail> listSSDetail = EntityManager<EntitySampStoreDetail>.ConvertToList(dtSSDetail).OrderBy(i => i.DetId).ToList();

                return listSSDetail;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetRackQueryData", ex);
                return new List<EntitySampStoreDetail>();
            }
        }

        public List<EntitySampStoreDetail> GetRackQueryDataByBarcode(string rackBarcode, string samBarcode)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql =string.Format(
                    @"select Sample_store_detail.Ssdt_Ssr_id,
Sample_store_detail.Ssdt_bar_code,
Sample_store_detail.Ssdt_x,
Sample_store_detail.Ssdt_y,
Sample_store_detail.Ssdt_seqno,
Sample_store_detail.Ssdt_status,   
Sample_store_detail.Ssdt_date,

Sample_store_rack.Ssr_Drack_id,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '男*' end) Pma_pat_sex,
(case isnull(dbo.getAge(Pat_lis_main.Pma_pat_age_exp),'') when '' then '20*' else dbo.getAge(Pat_lis_main.Pma_pat_age_exp) end ) pid_age,
Pat_lis_main.Pma_Ditr_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_com_name,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_check_date,
Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_pat_dept_name, 
Pat_lis_main.Pma_pat_Dsorc_id,
Pat_lis_main.Pma_Dsam_id,
Pat_lis_main.Pma_pat_in_no,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_status,

CAST(Sample_store_detail.Ssdt_x as VARCHAR)+'*'+CAST(Sample_store_detail.Ssdt_y as VARCHAR) AS det_xy,

Dict_profession.Dpro_name,
Dict_sample.Dsam_name,
Dict_sample_tube_rack.Drack_name,Dict_sample_tube_rack.Drack_barcode,Dict_sample_tube_rack.Drack_Dtrack_code,
Dict_sample_store.Dstore_name,Dict_sample_store_position.Dpos_name

from   Sample_store_detail
left join Pat_lis_main on  Pat_lis_main.Pma_bar_code = Sample_store_detail.Ssdt_bar_code
left join Sample_store_rack on Sample_store_rack.Ssr_id = Sample_store_detail.Ssdt_Ssr_id
left join Dict_sample_store on  Dict_sample_store.Dstore_id = Sample_store_rack.Ssr_Dstore_id
left join Dict_sample_store_position on  Dict_sample_store_position.Dpos_id = Sample_store_rack.Ssr_Dpos_id
left join Dict_sample_tube_rack on   Dict_sample_tube_rack.Drack_id=Sample_store_rack.Ssr_Drack_id 
left join Dict_profession on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
left join Dict_sample on  Dict_sample.Dsam_id = Pat_lis_main.Pma_Dsam_id
where   1=1 ");

                if (!string.IsNullOrEmpty(rackBarcode))
                {
                    strSql += " and Dict_sample_tube_rack.Drack_barcode = @rackBarcode ";
                }
                if (!string.IsNullOrEmpty(samBarcode))
                {
                    strSql += " and Sample_store_detail.Ssdt_bar_code = @samBarcode ";
                }
                
                List<DbParameter> paramHt = new List<DbParameter>();

                if (!string.IsNullOrEmpty(rackBarcode))
                {
                    paramHt.Add(new SqlParameter("@rackBarcode", rackBarcode));
                }
                if (!string.IsNullOrEmpty(samBarcode))
                {
                    paramHt.Add(new SqlParameter("@samBarcode", samBarcode));
                }

                DataTable dtSSR = helper.ExecuteDtSql(strSql,paramHt);

                List<EntitySampStoreDetail> listSSR = EntityManager<EntitySampStoreDetail>.ConvertToList(dtSSR).OrderBy(i => i.DetId).ToList();

                return listSSR;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetRackQueryDataByBarcode", ex);
                return new List<EntitySampStoreDetail>();
            }
        }

    }
}
