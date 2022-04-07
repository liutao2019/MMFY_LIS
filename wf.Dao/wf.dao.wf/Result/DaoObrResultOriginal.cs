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
    [Export("wf.plugin.wf", typeof(IDaoResultOriginal))]
    public class DaoObrResultOriginal : IDaoResultOriginal
    {
        public List<EntityObrResult> GetAllObrResult(DateTime date, string itr_id, int result_type, string filter)
        {
            DateTime dtSearchDate = date.Date;

            if (string.IsNullOrEmpty(filter))
            {
                filter = " 1=1 ";
            }

            string sql = string.Format(@"select Lres_id,Lres_Ditr_id,Lres_source_Ditr_id,Lres_sid,obr_mac_code = '',
Lres_value,Lres_value2,obr_value_c = '',obr_value_d = '',Lres_date,Lres_Pma_rep_id,
Lres_Ditm_id as itm_id,Lres_itm_ename as itm_ecode ,msg = ''
from Lis_result
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
where
--obr_flag = 1 and
((Lres_date >='{2}' and Lres_date <'{3}') 
or (Pma_in_date >= '{2}' and Pma_in_date < '{3}'))
and Lres_Ditr_id = '{0}'
and {1}", itr_id, filter, date.Date, date.Date.AddDays(1));

            DBManager helper = new DBManager();

            DataTable dtResult = helper.ExecuteDtSql(sql);

            dtResult = GetSidInt(dtResult);
            List<EntityObrResult> list = EntityManager<EntityObrResult>.ConvertToList(dtResult).OrderBy(i => i.ObrDate).ToList();
            return list;
        }

        public List<EntityObrResultOriginal> GetObrResult(DateTime date, string itr_id, int result_type, string filter)
        {
            try
            {
                DateTime dtSearchDate = date.Date;

                if (string.IsNullOrEmpty(filter))
                {
                    filter = " 1=1 ";
                }

                string sql = string.Format(@" select Lis_result_original.*,Rel_itr_channel_code.Ricc_Ditm_id as itm_id,
Dict_itm.Ditm_ecode as itm_ecode
from Lis_result_original
left outer join Rel_itr_channel_code on (Rel_itr_channel_code.Ricc_code = Lis_result_original.Lro_Ricc_code 
and Lis_result_original.Lro_Ditr_id = Rel_itr_channel_code.Ricc_Ditr_id)
left outer join Dict_itm on Rel_itr_channel_code.Ricc_Ditm_id = Dict_itm.Ditm_id
where (Lro_date >='{2}' and Lro_date<'{3}') and Lro_Ditr_id = '{0}'
and {1}", itr_id, filter, date.Date.ToString("yyyy-MM-dd HH:mm:ss"), date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                DataTable dtResult = helper.ExecuteDtSql(sql);

                dtResult = GetSidInt(dtResult);
                List<EntityObrResultOriginal> list = EntityManager<EntityObrResultOriginal>.ConvertToList(dtResult).OrderByDescending(i => i.ObrDate).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityObrResultOriginal>();
            }
        }


        public List<EntityObrResultOriginalEx> GetOrignObrResult(EntityPidReportMain pat)
        {
            try
            {
                string sql = string.Format(@" select Lis_result_originalex.*,Lis_result_originalex.Lro_Ricc_code as itm_id,
                    Dict_itm.Ditm_ecode as itm_ecode
                    from Lis_result_originalex (nolock)
                    left outer join Dict_itm(nolock) on Dict_itm.Ditm_id = Lis_result_originalex.Lro_Ricc_code
                    where Lro_Lresdesc_id = '{0}' ", pat.RepId);

                DBManager helper = new DBManager();
                DataTable dtResult = helper.ExecuteDtSql(sql);
                List<EntityObrResultOriginalEx> list = EntityManager<EntityObrResultOriginalEx>.ConvertToList(dtResult).OrderByDescending(i => i.ObrDate).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityObrResultOriginalEx>();
            }
        }


        public DataTable GetSidInt(DataTable dtResult)
        {
            if (!dtResult.Columns.Contains("obr_sid_int"))
            {
                dtResult.Columns.Add("obr_sid_int", typeof(double));
            }
            foreach (DataRow item in dtResult.Rows)
            {
                if (item["Lro_sid"] != null
                    && item["Lro_sid"] != DBNull.Value
                   && !string.IsNullOrEmpty(item["Lro_sid"].ToString())
                    )
                {
                    string sid = item["Lro_sid"].ToString();
                    double pat_sid_int = 0;
                    if (double.TryParse(sid, out pat_sid_int))
                    {
                        item["obr_sid_int"] = pat_sid_int;
                    }
                }
            }
            return dtResult;
        }
    }
}
