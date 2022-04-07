using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoPidReportDetail))]
    public class DaoPidReportDetail : DclDaoBase, IDaoPidReportDetail
    {
        public bool DeleteReportDetail(string repId)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Pdet_id", repId);

                helper.DeleteOperation("Pat_lis_detail", keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return result;
            }
            return true;
        }

        public List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId)
        {
            List<EntityPidReportDetail> detailList = new List<EntityPidReportDetail>();
            DataTable dtDetail = new DataTable();
            if (!string.IsNullOrEmpty(repId))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"
SELECT 
Dict_itm_combine.Dcom_name as pat_com_name,
Dict_itm_combine.sort_no as com_seq,
Pat_lis_detail.*
FROM Pat_lis_detail 
INNER JOIN Dict_itm_combine ON Pat_lis_detail.Pdet_Dcom_id = Dict_itm_combine.Dcom_id
WHERE (Pat_lis_detail.Pdet_id = '{0}')
", repId);
                try
                {
                    dtDetail = helper.ExecuteDtSql(sql);
                    detailList = EntityManager<EntityPidReportDetail>.ConvertToList(dtDetail).OrderBy(i => i.SortNo).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return detailList;
        }

        public List<string> GetPidReportDetailByBarcodeAndStatus(string barcode, string patFlag)
        {
            List<string> list = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(barcode) && !string.IsNullOrEmpty(patFlag))
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"SELECT Pat_lis_detail.Pdet_Dcom_id
FROM Pat_lis_detail WITH(NOLOCK) 
LEFT JOIN Pat_lis_main WITH(NOLOCK) ON Pat_lis_main.Pma_rep_id = Pat_lis_detail.Pdet_id
WHERE(Pat_lis_main.Pma_bar_code = '{0}' AND Pat_lis_main.Pma_status >={1}) ", barcode, patFlag);
                    DataTable dtPidRepDetail = helper.ExecuteDtSql(sql);
                    if (dtPidRepDetail != null && dtPidRepDetail.Rows.Count > 0)
                    {
                        list.Add(dtPidRepDetail.Rows[0]["Pdet_Dcom_id"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;

        }
        public bool InsertNewPidReportDetail(EntityPidReportDetail detail)
        {
            bool result = false;

            DBManager helper = GetDbManager();
            try
            {

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Pdet_id", detail.RepId);
                values.Add("Pdet_Dcom_id", detail.ComId);
                values.Add("Pdet_com_code", detail.OrderCode);
                values.Add("Pdet_price", detail.OrderPrice);
                values.Add("Pdet_order_sn", detail.OrderSn);
                if (detail.SortNo != null)
                    values.Add("sort_no", detail.SortNo.Value);
                values.Add("Pdet_bar_code", detail.RepBarCode);
                values.Add("Pdet_applyid", detail.ApplyID);

                helper.InsertOperation("Pat_lis_detail", values);

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }
        public bool UploadNewPidReportDetail(EntityPidReportDetail detail)
        {
            bool result = false;

            DBManager helper = new DBManager("GHConnectionString");
            try
            {

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Pdet_id", detail.RepId);
                values.Add("Pdet_Dcom_id", detail.ComId);
                values.Add("Pdet_com_code", detail.OrderCode);
                values.Add("Pdet_price", detail.OrderPrice);
                values.Add("Pdet_order_sn", detail.OrderSn);
                if (detail.SortNo != null)
                    values.Add("sort_no", detail.SortNo.Value);
                values.Add("Pdet_bar_code", detail.RepBarCode);

                helper.InsertOperation("Pat_lis_detail", values);

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }
        public List<EntityPidReportDetail> SearchPidReportDetailByMulitRepId(string mulitRepId)
        {
            List<EntityPidReportDetail> listPidRepDetail = new List<EntityPidReportDetail>();
            if (!string.IsNullOrEmpty(mulitRepId))
            {
                try
                {
                    DBManager helper = new DBManager();

                    string sqlStr = string.Format("Select * from Pat_lis_detail where Pdet_id in ({0})", mulitRepId);
                    DataTable dtPidRepDetail = helper.ExecuteDtSql(sqlStr);

                    listPidRepDetail = EntityManager<EntityPidReportDetail>.ConvertToList(dtPidRepDetail).OrderBy(i => i.RepId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("根据多个病人标识ID查询病人组合明细数据SearchPidReportDetailByMulitRepId", ex);
                }
            }
            return listPidRepDetail;
        }

        public bool UpdateDetailRepIdByOldRedId(string newRepId, string oldRepId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(newRepId) && !string.IsNullOrEmpty(oldRepId))
            {
                try
                {
                    string sql = string.Format(@"update Pat_lis_detail set Pdet_id = '{0}' where Pdet_id = '{1}' ", newRepId, oldRepId);
                    DBManager helper = new DBManager();
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }


        public bool UpdatePidReportDetailInfo(EntityPidReportDetail detail)
        {
            bool result = false;

            DBManager helper = GetDbManager();
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Pdet_Dcom_id", detail.ComId);
                values.Add("Pdet_com_code", detail.OrderCode);
                values.Add("Pdet_price", detail.OrderPrice);
                values.Add("Pdet_order_sn", detail.OrderSn);
                if (detail.SortNo != null)
                    values.Add("sort_no", detail.SortNo.Value);
                values.Add("Pdet_bar_code", detail.RepBarCode);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Pdet_id", detail.RepId);
                keys.Add("Pdet_Dcom_id", detail.ComId);
                helper.UpdateOperation("Pat_lis_detail", values, keys);

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            return result;
        }

        public bool IsExistedOrder(string OrderSn)
        {
            try
            {
                DBManager helper = GetDbManager();
                string sql = string.Format("select top 1 1 from Pat_lis_detail where Pdet_order_sn = '{0}'",OrderSn);
                DataTable dt = helper.ExecSel(sql);
                if (dt.Rows.Count > 0)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException("查询数据库报告明细记录失败",ex);
                throw ex;
            }
        }
    }
}
