/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReaStorageDetail))]
    public class DaoReaStorageDetail : DclDaoBase, IDaoReaStorageDetail
    {
        
        public bool CancelReaStorageDetail(EntityReaStorageDetail detail)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsd_no", detail.Rsd_no);

                helper.UpdateOperation("Rea_storage_detail", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteReaStorageDetail(string storageId, string rea_id)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsd_no", storageId);
                if (!string.IsNullOrEmpty(rea_id))
                {
                    keys.Add("Rsd_reaid", rea_id);
                }


                helper.DeleteOperation("Rea_storage_detail", keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return result;
            }
            return true;
        }

        public bool InsertNewReaStorageDetail(EntityReaStorageDetail storage)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (storage != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(storage);

                    helper.InsertOperation("Rea_storage_detail", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public List<EntityReaStorageDetail> GetReaStorageDetail(EntityReaQC reaQC)
        {
            List<EntityReaStorageDetail> detailList = new List<EntityReaStorageDetail>();
            DataTable dtDetail = new DataTable();
            DBManager helper = new DBManager();
            string sql = @"
SELECT 
Dict_rea_setting.Drea_name as ReagentName,
Dict_rea_supplier.Rsupplier_name as SupName,
Dict_rea_product.Rpdt_name as PdtName,
Dict_rea_group.Rea_group as GroupName,
Dict_rea_store_position.Rstr_position as PosName,
Dict_rea_strcondition.Rstr_condition as ConName,
Dict_rea_unit.Runit_name as UnitName,
Rea_storage_detail.*
FROM Rea_storage_detail 
left JOIN Dict_rea_setting ON Rea_storage_detail.Rsd_reaid = Dict_rea_setting.Drea_id
left JOIN Dict_rea_product ON Rea_storage_detail.Rsd_pdtid = Dict_rea_product.Rpdt_id
left JOIN Dict_rea_supplier ON Dict_rea_supplier.Rsupplier_id = Rea_storage_detail.Rsd_supid
left JOIN Dict_rea_unit ON Dict_rea_unit.Runit_id = Rea_storage_detail.Rsd_unitid
left JOIN Dict_rea_store_position ON Dict_rea_store_position.Rstr_position_id = Rea_storage_detail.Rsd_posid
left JOIN Dict_rea_strcondition ON Dict_rea_strcondition.Rstr_condition_id = Rea_storage_detail.Rsd_conid
left JOIN Dict_rea_group ON Dict_rea_group.Rea_group_id = Rea_storage_detail.Rsd_groupid
left JOIN Rea_storage ON Rea_storage.Rsr_no = Rea_storage_detail.Rsd_no
WHERE 1=1  and Rea_storage_detail.del_flag = '0' {0}
";
            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(reaQC.ObrSn))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_id='{0}' ", reaQC.ObrSn);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaNo))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_no='{0}' ", reaQC.ReaNo);
            }
            if (!string.IsNullOrEmpty(reaQC.PurNo))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_purno='{0}' ", reaQC.PurNo);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaId))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_reaid='{0}' ", reaQC.ReaId);
            }
            if (!string.IsNullOrEmpty(reaQC.Barcode))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_barcode='{0}' ", reaQC.Barcode);
            }
            if (!string.IsNullOrEmpty(reaQC.BatchNo))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_batchno='{0}' ", reaQC.BatchNo);
            }
            if (!string.IsNullOrEmpty(reaQC.SupId))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_supid='{0}' ", reaQC.SupId);
            }
            if (!string.IsNullOrEmpty(reaQC.GrpId))
            {
                sqlWhere += string.Format(@" and Rea_storage_detail.Rsd_groupid='{0}' ", reaQC.GrpId);
            }
            if (reaQC.JudgeCount)
            {
                sqlWhere += @" and Rea_storage_detail.Rsd_count > 0 ";
            }
            if (reaQC.JudgeValidtime)
            {
                sqlWhere += @" and Rea_storage_detail.Rsd_validdate > GETDATE() ";
            }
            if (reaQC.WithTime)
            {
                if (!string.IsNullOrEmpty(reaQC.DateStart.ToString()))
                {
                    sqlWhere += " and Rea_storage.Rsr_date" + " >= '" + reaQC.DateStart + "'";
                }
                if (!string.IsNullOrEmpty(reaQC.DateEnd.ToString()))
                {
                    sqlWhere += " and Rea_storage.Rsr_date" + " <'" + reaQC.DateEnd?.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }
            try
            {
                dtDetail = helper.ExecuteDtSql(string.Format(sql, sqlWhere));
                detailList = EntityManager<EntityReaStorageDetail>.ConvertToList(dtDetail);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return detailList;
        }

        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                string sql = string.Format("delete from Rea_storage_detail where Rsd_id = '{0}'", obrSn);
                DBManager helper = GetDbManager();
                try
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public List<EntityReaStorageDetail> GetNotdelivered()
        {
            List<EntityReaStorageDetail> detailList = new List<EntityReaStorageDetail>();
            DBManager helper = new DBManager();
            DataTable dtDetail = new DataTable();
            string sql = "select * from Rea_storage_detail where Rsd_count > 0";
            try
            {
                dtDetail = helper.ExecuteDtSql(sql);
                detailList = EntityManager<EntityReaStorageDetail>.ConvertToList(dtDetail);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return detailList;
        }

        public List<EntityReaStorageDetail> QueryListByDate(DateTime date, string reaid)
        {
            List<EntityReaStorageDetail> detailList = new List<EntityReaStorageDetail>();
            DBManager helper = new DBManager();
            DataTable dtDetail = new DataTable();
            string sql = @"select Dict_rea_setting.Drea_name as ReagentName,
Dict_rea_supplier.Rsupplier_name as SupName,
Dict_rea_product.Rpdt_name as PdtName,
Dict_rea_group.Rea_group as GroupName,
Dict_rea_store_position.Rstr_position as PosName,
Dict_rea_strcondition.Rstr_condition as ConName,
Dict_rea_unit.Runit_name as UnitName,
Rea_storage_detail.* from Rea_storage_detail
left JOIN Dict_rea_setting ON Rea_storage_detail.Rsd_reaid = Dict_rea_setting.Drea_id
left JOIN Dict_rea_product ON Rea_storage_detail.Rsd_pdtid = Dict_rea_product.Rpdt_id
left JOIN Dict_rea_supplier ON Dict_rea_supplier.Rsupplier_id = Rea_storage_detail.Rsd_supid
left JOIN Dict_rea_unit ON Dict_rea_unit.Runit_id = Rea_storage_detail.Rsd_unitid
left JOIN Dict_rea_store_position ON Dict_rea_store_position.Rstr_position_id = Rea_storage_detail.Rsd_posid
left JOIN Dict_rea_strcondition ON Dict_rea_strcondition.Rstr_condition_id = Rea_storage_detail.Rsd_conid
left JOIN Dict_rea_group ON Dict_rea_group.Rea_group_id = Rea_storage_detail.Rsd_groupid
where Rsd_status = '0' and Rsd_validdate<='{0}' and Rsd_reaid = '{1}'";
            try
            {
                sql = string.Format(sql, date, reaid);
                dtDetail = helper.ExecuteDtSql(sql);
                detailList = EntityManager<EntityReaStorageDetail>.ConvertToList(dtDetail);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return detailList;
        }

        public List<EntityReaStorageDetail> GetNotdeliveredByID(string reaid, int num)
        {
            List<EntityReaStorageDetail> detailList = new List<EntityReaStorageDetail>();
            DBManager helper = new DBManager();
            DataTable dtDetail = new DataTable();
            string sql = "select top {1} * from Rea_storage_detail where Rsd_count > 0 and Rsd_reaid='{0}' order by sort_no,Rsd_barcode asc";
            sql = string.Format(sql, reaid, num);
            try
            {
                dtDetail = helper.ExecuteDtSql(sql);
                detailList = EntityManager<EntityReaStorageDetail>.ConvertToList(dtDetail);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return detailList;
        }

        public void UpdateDetailStatus(string barcode, int status,int count, string rsdno)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Rsd_status", status);
                    values.Add("Rsd_count", count);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Rsd_barcode", barcode);
                    keys.Add("Rsd_no", rsdno);

                    helper.UpdateOperation("Rea_storage_detail", values, keys);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }
    }
}
