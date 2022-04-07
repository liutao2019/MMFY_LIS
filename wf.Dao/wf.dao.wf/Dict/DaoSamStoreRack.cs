using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntitySampStoreRack>))]
    public class DaoSamStoreRack:IDaoDic<EntitySampStoreRack>
    {
        public bool Delete(EntitySampStoreRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ssr_id", rack.SrId);

                helper.DeleteOperation("Sample_store_rack", key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntitySampStoreRack rack)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Sample_store_rack");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ssr_id", id);
                values.Add("Ssr_Drack_id", rack.SrRackId);
                values.Add("Ssr_status", rack.SrStatus);
                values.Add("Ssr_audit_user_name", rack.SrAuditUserName);
                values.Add("Ssr_audit_user_id", rack.SrAuditUserId);
                values.Add("Ssr_audit_date", rack.SrAuditDate);
                values.Add("Ssr_store_user_name", rack.SrStoreUserName);
                values.Add("Ssr_store_user_code", rack.SrStoreUserCode);
                values.Add("Ssr_store_date", rack.SrStoreDate);
                values.Add("Ssr_Dpos_id", rack.SrPlace);
                values.Add("Ssr_Dstore_id", rack.SrStoreId);
                values.Add("Ssr_amount", rack.SrAmount);
                values.Add("Ssr_Destroy_name", rack.SrDestroyName);
                values.Add("Ssr_Destroy_code", rack.SrDestroyCode);
                values.Add("Ssr_Destroy_date", rack.SrDestroyDate.ToString("yyyy-MM-dd HH:mm:ss"));

                helper.InsertOperation("Sample_store_rack", values);

                rack.SrId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySampStoreRack> Search(object obj)
        {
            //string where = string.Empty;
            //if (obj != null)
            //{
            //    where = string.Format("WHERE samp_store_rack.rack_id={0}", obj.ToString());
            //}


            try
            {
                String sql = string.Format(@"","");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySampStoreRack>.ConvertToList(dt).OrderBy(i => i.SrId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySampStoreRack>();
            }
        }

        public bool Update(EntitySampStoreRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ssr_Drack_id", rack.SrRackId);
                values.Add("Ssr_status", rack.SrStatus);
                values.Add("Ssr_audit_user_name", rack.SrAuditUserName);
                values.Add("Ssr_audit_user_id", rack.SrAuditUserId);
                values.Add("Ssr_audit_date", rack.SrAuditDate);
                values.Add("Ssr_store_user_name", rack.SrStoreUserName);
                values.Add("Ssr_store_user_code", rack.SrStoreUserCode);
                values.Add("Ssr_store_date", rack.SrStoreDate);
                values.Add("Ssr_Dpos_id", rack.SrPlace);
                values.Add("Ssr_Dstore_id", rack.SrStoreId);
                values.Add("Ssr_amount", rack.SrAmount);
                values.Add("Ssr_Destroy_name", rack.SrDestroyName);
                values.Add("Ssr_Destroy_code", rack.SrDestroyCode);
                values.Add("Ssr_Destroy_date", rack.SrDestroyDate);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ssr_id", rack.SrId);

                helper.UpdateOperation("Sample_store_rack", values, key);
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
