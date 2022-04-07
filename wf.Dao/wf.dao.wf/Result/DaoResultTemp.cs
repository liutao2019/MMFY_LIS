using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoResultTemp))]
    public class DaoResultTemp : IDaoResultTemp
    {
        
        public List<string> GetPatientsSid(EntityAnanlyseQC query)
        {
            string where = GetWhere(query);
            string sql = string.Format(@"SELECT  p.Pma_sid FROM Pat_lis_main p LEFT JOIN Pat_lis_detail pm ON pm.Pdet_id=p.Pma_rep_id
LEFT JOIN Rel_itm_combine_item dcm ON dcm.Rici_Dcom_id=pm.Pdet_Dcom_id where 1=1  {0}", where);
            try
            {
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);

                return (from x in (EntityManager<EntityPidReportMain>.ConvertToList(dt)) select x.RepSid).ToList();
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }
        }

        private string GetWhere(EntityAnanlyseQC query)
        {
            string where = string.Empty;
            if (query.DateStart != null)
            {
                where += string.Format(" AND p.Pma_in_date> '{0}'", query.DateStart);
            }
            if (query.DateEnd != null)
            {
                where += string.Format(" AND p.Pma_in_date<='{0}'", query.DateEnd);
            }
            if (!string.IsNullOrEmpty(query.ItrId))
            {
                where += string.Format(" And p.Pma_Ditr_id='{0}'", query.ItrId);
            }
            if (!string.IsNullOrEmpty(query.ItmId))
            {
                where += string.Format(" AND dcm.Rici_Ditm_id='{0}'", query.ItmId);
            }
            return where;
        }
    }
}
