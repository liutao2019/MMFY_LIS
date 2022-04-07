using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using System.Data;
using dcl.dao.core;
using Lib.LogManager;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoTouchPrint))]
    public class DaoTouchPrint : IDaoTouchPrint
    {
        public List<EntityPidReportMain> PatientPrintQuery(string pidInNo, string pidSrcId)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            try
            {
                string strSql = string.Format(@"select 
                Pma_rep_id,
                Pma_pat_name,
                Pma_collection_date,
                Pma_com_name,
                Pma_report_date,
                Pma_status,
                Pma_bar_code,
                Pma_Ditr_id,
                Dict_sample.Dsam_name,
                Dict_itr_instrument.Ditr_report_id
                from Pat_lis_main
                left join Dict_sample on Pat_lis_main.Pma_Dsam_id=Dict_sample.Dsam_id
                Left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
                where (Pma_pat_in_no='{0}' or Pma_social_no='{0}') 
                and Pma_pat_Dsorc_id in ({1})
                and Pma_status = 2", pidInNo, pidSrcId);

                DBManager helper = new DBManager();
                //Logger.LogInfo(strSql);
                DataTable dt = helper.ExecuteDtSql(strSql);
                listPat = EntityManager<EntityPidReportMain>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return listPat;

        }

        public List<EntitySampMain> SampMainPrintQuery(string pidInNo, string pidSrcId)
        {
            List<EntitySampMain> listSampMain = new List<EntitySampMain>();
            try
            {
                string strSql = string.Format(@"select
Sample_main.Sma_pat_name,
Sma_bar_code,
Sma_date,
Sma_com_name,
Sma_collection_date,
Sma_lastoper_date,
Sma_status_id,
Dict_status.Dsta_content,
Dict_sample.Dsam_name
from Sample_main 
left join Dict_status on Dict_status.Dsta_name=Sample_main.Sma_status_id
left join Dict_sample on Dict_sample.Dsam_id=Sample_main.Sma_Dsam_id
where Sma_pat_in_no='{0}' 
and Sma_pat_idt_id in ({1})
and Sma_status_id<20 
and sample_main.del_flag = '0'
order by Sma_date desc", pidInNo, pidSrcId);

                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(strSql);
                //Logger.LogInfo(strSql);
                listSampMain = EntityManager<EntitySampMain>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return listSampMain;
        }
    }
}
