using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoCdrHistroyResult))]
    public class DaoCdrHistoryResult: IDaoCdrHistroyResult
    {
        public List<EntityObrResult> GetCdrHistoryObrResult(List<string> listObrId)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            try
            {  //res.res_sid,
                string obrIds = string.Empty;
                if (listObrId != null && listObrId.Count > 0)
                {
                    foreach (string obrId in listObrId)
                    {
                        obrIds += string.Format(",'{0}'", obrId);
                    }
                    obrIds = obrIds.Remove(0, 1);
                }
                DBManager helper = new DBManager("CDRConnectionString");
                string sql = string.Format(@"SELECT obr_repno as obr_id,obr_result as obr_value,obr_item_id as itm_id,obr_item_ename as itm_ename,  obr_time as obr_date
                                          from    CDR_LIS_RESULT  where obr_repno in ({0}) ", obrIds);
                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntityObrResult>.ConvertToList(dt).OrderBy(i => i.ObrDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public List<EntityPidReportMain> GetCdrHistroyPatients(EntityPatientQC qc)
        {
            List<EntityPidReportMain> list = new List<EntityPidReportMain>();
            try
            {  
                DBManager helper = new DBManager("CDRConnectionString");
                string sql=
                    string.Format(@" SELECT data_sys_id_value as rep_id,obr_secondaudit_time as rep_report_date  from CDR_LIS_MAIN
                                            where orc_sam_id = '{0}'
                                         and pid_name = '{1}'
                                         and pid_hospitalno = '{2}'
                                            and data_sys_id_value <> '{3}'
                                            and obr_secondaudit_time <= '{4}'
                                            -- and pat_flag in (2, 4)
                                        and data_valid_flag = 1 ", qc.SamId, qc.PidName, qc.PidInNo,qc.RepId, qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.RepInDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }
    }
}
