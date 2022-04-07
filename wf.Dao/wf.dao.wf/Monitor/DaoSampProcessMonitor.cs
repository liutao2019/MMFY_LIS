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
    [Export("wf.plugin.wf", typeof(IDaoSampProcessMonitor))]
    public class DaoSampProcessMonitor : IDaoSampProcessMonitor
    {
        public List<EntitySampProcessMonitor> GetBCPatients(string valueOutTime)
        {
            List<EntitySampProcessMonitor> listSampProMonitor = new List<EntitySampProcessMonitor>();

            if (valueOutTime != null)
            {
                try
                {
                    DBManager helper = new DBManager();

                    DateTime dtSelect = DateTime.Now.AddHours(-24);

                    string sqlStr = string.Format(@"select Sample_main.Sma_pat_src_id,
Sample_main.Sma_id,Sample_main.Sma_bar_code,Sample_main.Sma_pat_name,
Sample_main.Sma_pat_sex,Sample_main.Sma_pat_bed_no,Sample_main.Sma_com_name,
Sample_main.Sma_Dsam_id,Sample_main.Sma_status_id,Sample_main.Sma_pat_unique_id,
(case when isnull(Sample_main.Sma_urgent_flag,'0')='1' then '急' else '' end) samp_urgent_flagStr,
(case when isnull(Sample_main.Sma_urgent_flag,'0')='1' then 1 else 0 end) urgent_flag,
Sample_main.Sma_pat_age,Sample_main.Sma_pat_in_no,Sample_main.Sma_doctor_name,
Sample_main.Sma_occ_date,Sample_main.Sma_pat_diag,Sample_main.Sma_pat_admiss_times,
Sample_main.Sma_pat_dept_name,Sample_main.Sma_remark,Sample_main.Sma_tub_code,
Dict_test_tube.Dtub_name,Dict_sample.Dsam_name,
isnull(Dict_profession.Dpro_name,'未指定') Dpro_name,
Sample_main.Sma_type,
datediff(n,Sample_main.Sma_lastoper_date,GETDATE()) bc_time_difference,
(case when datediff(n,Sample_main.Sma_lastoper_date,GETDATE())>{9} then 1 else 0 end ) bc_isTimeout,
Sample_main.Sma_lastoper_date,Pat_lis_main.Pma_serial_num,
(select top 1 Sproc_place from Sample_process_detial 
where Sample_process_detial.Sproc_Sma_bar_code =Sample_main.Sma_bar_code  
and Sample_process_detial.Sproc_status=Sample_main.Sma_status_id
order by Sample_process_detial.Sproc_date desc ) Sproc_place,
Base_user.Buser_name
from Sample_main with(nolock)
left join Dict_test_tube with(nolock) on Sample_main.Sma_tub_code=Dict_test_tube.Dtub_code
left join Dict_sample with(nolock) on Sample_main.Sma_Dsam_id=Dict_sample.Dsam_id
left join Dict_profession with(nolock) on Sample_main.Sma_type=Dict_profession.Dpro_id 
left join Pat_lis_main with(nolock) on Sample_main.Sma_bar_code=Pat_lis_main.Pma_bar_code 
left join Base_user on Base_user.Buser_loginid= Sample_main.Sma_lastoper_Buser_id 
where Sample_main.Sma_lastoper_date>='{0}' and Sample_main.del_flag='0'
and Sample_main.Sma_status_id in ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')  
                            ",
                    dtSelect.ToString("yyyy-MM-dd HH:mm:ss"),
                    EnumBarcodeOperationCodeNew.SampleCollect,
                    EnumBarcodeOperationCodeNew.SampleSend,
                    EnumBarcodeOperationCodeNew.SampleReach,
                    EnumBarcodeOperationCodeNew.SampleSecondSend,
                    EnumBarcodeOperationCodeNew.SampleReceive,
                    EnumBarcodeOperationCodeNew.SampleRegister,
                    EnumBarcodeOperationCodeNew.UndoAudit,
                    EnumBarcodeOperationCodeNew.Audit,
                    valueOutTime
                    );

                    DataTable dtProMonitor = helper.ExecuteDtSql(sqlStr);

                    listSampProMonitor = EntityManager<EntitySampProcessMonitor>.ConvertToList(dtProMonitor).OrderBy(i => i.PidSrcId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listSampProMonitor;
        }

        public List<EntitySampProcessMonitor> GetSampCount(int OperationCode)
        {
            List<EntitySampProcessMonitor> listSampProMonitor = new List<EntitySampProcessMonitor>();
            try
            {
                DBManager helper = new DBManager();
                DateTime dtSelect = DateTime.Now.AddHours(-24);
                string sql = string.Format(@"select COUNT(*) as count,Sma_pat_dept_name ,Sma_pat_src_id from Sample_main where Sma_lastoper_date >= '{0}' and Sample_main.del_flag = '0' and Sample_main.Sma_status_id = '{1}' and Sma_urgent_flag<>'1' group by Sma_pat_dept_name,Sma_pat_src_id", dtSelect.ToString("yyyy-MM-dd HH:mm:ss"), OperationCode);
                DataTable dtProMonitor = helper.ExecuteDtSql(sql);
                listSampProMonitor = EntityManager<EntitySampProcessMonitor>.ConvertToList(dtProMonitor).OrderBy(i => i.PidSrcId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            } 
            return listSampProMonitor;
        }
    }
}
