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
    [Export("wf.plugin.wf", typeof(IDaoElisaAnalyse))]
    public class DaoElisaAnalyse : IDaoElisaAnalyse
    {
        public List<EntityObrElisaResult> GetElisaResult(string ItrId, DateTime dtImmDate)
        {


            try
            {
                string where = string.Format("where Leres_Ditr_id = '{0}' and Leres_date >= '{1}' and Leres_date < '{2}'", ItrId, dtImmDate.ToString("yyyy-MM-dd"), dtImmDate.AddDays(1).ToString("yyyy-MM-dd"));
                string sqlSelect = @"
                    SELECT
Lis_Elisa_result.*,
Dict_itm.Ditm_ecode,
Dict_itm.Ditm_name,
Dict_itm.Ditm_rep_code
FROM Lis_Elisa_result WITH(NOLOCK)
left join Dict_itm on Lis_Elisa_result.Leres_Ditm_id = Dict_itm.Ditm_id " + where;

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sqlSelect);

                List<EntityObrElisaResult> list = EntityManager<EntityObrElisaResult>.ConvertToList(dt).OrderBy(i => i.ResId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityObrElisaResult>();
            }
        }

        public List<EntityDicQcConvert> GetQCConvert(string ItrId)
        {
            try
            {
                List<EntityDicQcConvert> list = new List<EntityDicQcConvert>();
                DaoDicQcConvert QcConvert = new DaoDicQcConvert();
                list = QcConvert.Search(ItrId);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcConvert>();
            }
        }

        public bool UpdateElisaResult(EntityObrElisaResult ElisaResult)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Leres_Ditr_id", ElisaResult.ResItrId);
                values.Add("Leres_date", ElisaResult.ResDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Leres_elsaplate", ElisaResult.ResElsaplate);
                values.Add("Leres_value", ElisaResult.ResValue);
                values.Add("Leres_flag", ElisaResult.ResFlag);
                values.Add("del_flag", ElisaResult.DelFlag);
                values.Add("Leres_Ditm_id", ElisaResult.ResItmId);
                values.Add("Leres_resulto_od", ElisaResult.ResResultoOd);
                values.Add("Leres_resulto_chr", ElisaResult.ResResultoChr);
                values.Add("Leres_sid", ElisaResult.ResSid);
                values.Add("Leres_start_sid", ElisaResult.ResStartSid);
                values.Add("Leres_end_sid", ElisaResult.ResEndSid);
                values.Add("Leres_reag_id", ElisaResult.ResReagId);
                values.Add("Leres_reag_date", ElisaResult.ResReagDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Leres_reag_manu", ElisaResult.ResReagManu);
                values.Add("Leres_pos_fmlu", ElisaResult.ResPosFmlu);
                values.Add("Leres_manu_date", ElisaResult.ResManuDate.ToString("yyyy-MM-dd HH:mm:ss"));

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Leres_id", ElisaResult.ResId);

                helper.UpdateOperation("Lis_Elisa_result", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateResValue(string ResId, string ResValue)
        {
            try
            {
                string sqlSelect = string.Format(@"update Lis_Elisa_result set Leres_value = '{0}' where Leres_id = '{1}'", ResValue, ResId);

                DBManager helper = new DBManager();

                helper.ExecCommand(sqlSelect);

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
