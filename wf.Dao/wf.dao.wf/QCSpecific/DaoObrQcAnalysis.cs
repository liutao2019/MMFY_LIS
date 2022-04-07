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
    [Export("wf.plugin.wf", typeof(IDaoObrQcAnalysis))]
    public class DaoObrQcAnalysis : IDaoObrQcAnalysis
    {
        public bool DeleteQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lqa_id", qcAnalysis.QanId);

                helper.DeleteOperation("Lis_qc_analysis", keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            bool result = false;
            try
            {
                int id = IdentityHelper.GetMedIdentity("Lis_qc_analysis");
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                qcAnalysis.QanId = id.ToString();
                values = helper.ConverToDBSaveParameter(qcAnalysis);
                helper.InsertOperation("Lis_qc_analysis", values);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityObrQcAnalysis> SearchQcAnalysis(EntityObrQcResultQC qc)
        {
            List<EntityObrQcAnalysis> listAnalysis = new List<EntityObrQcAnalysis>();
            try
            {

                DBManager helper = new DBManager();
                string strWhere = string.Empty;
                if (!string.IsNullOrEmpty(qc.ItrId))
                {
                    strWhere += string.Format(" and Lqa_Ditr_id='{0}'", qc.ItrId);
                }
                if (!string.IsNullOrEmpty(qc.ItemId))
                {
                    strWhere += string.Format(" and Lqa_Ditm_id='{0}' ", qc.ItemId);
                }
                if (!string.IsNullOrEmpty(qc.QanLevel))
                {
                    strWhere += string.Format(" and Lqa_level='{0}' ", qc.QanLevel);
                }
                string sql = string.Format(@"select Lqa_id, Lqa_Ditr_id, Lqa_Ditm_id, Lqa_date_start, Lqa_date_end, Lqa_ananlysis, Lqa_audit_Buser_id, Lqa_audit_flag,Lqa_level,
Dict_itm.Ditm_name, Lqa_audit_date,Base_user.Buser_name,Dict_itr_instrument.Ditr_name from Lis_qc_analysis 
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id=Lqa_Ditr_id
left join Base_user on Base_user.Buser_loginid=Lqa_audit_Buser_id 
left join Dict_itm on Dict_itm.Ditm_id=Lqa_Ditm_id
where 1=1 {0}", strWhere);
                DataTable dt = helper.ExecuteDtSql(sql);
                listAnalysis = EntityManager<EntityObrQcAnalysis>.ConvertToList(dt).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listAnalysis;

        }

        public bool UpdateQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values = helper.ConverToDBSaveParameter(qcAnalysis);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lqa_id", qcAnalysis.QanId);

                helper.UpdateOperation("Lis_qc_analysis", values, keys);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
