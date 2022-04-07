using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampStoreRack))]
    public class DaoSampStoreRack : IDaoSampStoreRack
    {
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
                values.Add("Ssr_Destroy_date", entity.SrDestroyDate.Year == 1 ? (DateTime?)null : entity.SrDestroyDate);

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

        public int UpdateSrAmountById(string SrId,string qc)
        {
            int intRet = -1;
            string strSql = string.Format(@"update Sample_store_rack set Ssr_amount= (case when Ssr_amount=0 then 0 else Ssr_amount-1 end)
                                       where Ssr_id = '{0}'  and Ssr_status<>20 ", SrId);
            if(qc == "SrStatus")
            {
                strSql = string.Format(@"update Sample_store_rack set Ssr_status= 0
                                       where Ssr_id = '{0}'  and Ssr_status<>20 ", SrId);
            }
            DBManager helper = new DBManager();
             
            intRet = helper.ExecCommand(strSql);

            return intRet;
        }

    }
}
