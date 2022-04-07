using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResultBact))]
    public class DaoObrResultBact : DclDaoBase, IDaoObrResultBact
    {
        public bool DeleteResultById(string obrId)
        {
            DBManager helper = GetDbManager();
            try
            {
                string sql = String.Format("delete from Lis_result_bact where Lbac_id='{0}'", obrId);
                helper.ExecCommand(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityObrResultBact> GetBactResultById(string obrId = "")
        {
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Format(@"select Lis_result_bact.*,
Dict_mic_bacteria.Dbact_Dbactt_id as btype_id,
Dict_mic_bacteria.Dbact_cname,
Dict_mic_bacteria.Dbact_ename,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_sub_title
from Lis_result_bact 
left join Dict_mic_bacteria on Lis_result_bact.Lbac_Dbact_id = Dict_mic_bacteria.Dbact_id
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Lis_result_bact.Lbac_Ditr_id
WHERE Lbac_id ='{0}'", obrId);
                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityObrResultBact>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityObrResultBact>();
            }
        }

        public bool SaveResultBact(EntityObrResultBact bactResult)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (bactResult != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(bactResult);

                    helper.InsertOperation("Lis_result_bact", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
    }
}
