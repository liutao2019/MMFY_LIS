using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampProcessDetail))]
    public class DaoSampProcessDetail : DclDaoBase, IDaoSampProcessDetail
    {
        public string GetDeletePatId(string patId, string patName, string timeFrom, string timeTo)
        {
            List<EntitySampProcessDetail> list = new List<EntitySampProcessDetail>();
            try
            {
                string sql = string.Format(@"select Sproc_rep_id from Sample_process_detial
                                                    where Sproc_status = 530 and Sproc_content is not null
                                                    and Sproc_content like '%姓名：{0}%' 
                                                    and Sproc_content like '%病人ID：{1}%'
                                                    and Sproc_date between '{2}" + " 00:00:00'" +
                                                    " and '{3}" + " 23:59:59'", patName, patId, timeFrom, timeTo);
                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntitySampProcessDetail>.ConvertToList(dt).OrderBy(i => i.ProcDate).ToList();
                return list[0].RepId;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }

        }

        public List<EntitySampProcessDetail> GetSampProcessDetail(string sampBarId)
        {
            List<EntitySampProcessDetail> list = new List<EntitySampProcessDetail>();
            try
            {
                string sql = string.Format(@"select
Sample_process_detial.Sproc_id, Sproc_date, Sproc_user_id, Sample_process_detial.Sproc_user_name, Sproc_type, Sproc_status, Sproc_Sma_bar_id, 
Sproc_Sma_bar_code, Sproc_place, Sproc_times, Sproc_content, Sproc_rep_id,Dict_status.Dsta_content proc_status_name
from Sample_process_detial
Left join Dict_status on Sample_process_detial.Sproc_status = Dict_status.Dsta_name
where Sproc_Sma_bar_id='{0}'", sampBarId);

                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(sql);



                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampProcessDetail>.ConvertToList(dt).OrderBy(i => i.ProcDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取检验报告流程状态记录
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        public List<EntitySampProcessDetail> GetSamprocessDetailByRepId(string repId)
        {
            List<EntitySampProcessDetail> list = new List<EntitySampProcessDetail>();
            try
            {

                string sql = string.Format(@"select Sample_process_detial.*,Dict_status.Dsta_content as proc_status_name
from Sample_process_detial WITH(NOLOCK) inner join Dict_status on Dict_status.Dsta_name = Sample_process_detial.Sproc_status
where Sample_process_detial.Sproc_rep_id = '{0}'
union
select Sample_process_detial.*,Dict_status.Dsta_content as proc_status_name
from Sample_process_detial WITH(NOLOCK)
inner join Dict_status on Dict_status.Dsta_name = Sample_process_detial.Sproc_status
where Sample_process_detial.Sproc_Sma_bar_code=(select CASE WHEN  Pma_bar_code is null 
or Pma_bar_code ='' then '0x0xa' ELSE Pma_bar_code end 
from Pat_lis_main nolock  where Pma_rep_id='{0}')", repId);
                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampProcessDetail>.ConvertToList(dt).OrderBy(i => i.ProcDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public Boolean SaveSampProcessDetail(EntitySampProcessDetail sampProcessDetial)
        {
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Sproc_date", sampProcessDetial.ProcDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Sproc_user_id", sampProcessDetial.ProcUsercode);
                values.Add("Sproc_user_name", sampProcessDetial.ProcUsername);
                values.Add("Sproc_status", sampProcessDetial.ProcStatus);
                values.Add("Sproc_Sma_bar_id", sampProcessDetial.ProcBarno);
                values.Add("Sproc_Sma_bar_code", sampProcessDetial.ProcBarcode);
                values.Add("Sproc_place", sampProcessDetial.ProcPlace);
                values.Add("Sproc_times", sampProcessDetial.ProcTimes);
                values.Add("Sproc_content", sampProcessDetial.ProcContent);
                values.Add("Sproc_rep_id", sampProcessDetial.RepId);
                helper.InsertOperation("Sample_process_detial", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }


        public List<String> BatchSaveSampProcessDetail(List<EntitySampProcessDetail> listSampProcessDetial, DBManager helper)
        {
            List<String> result = new List<string>();

            PropertyInfo[] propertys = listSampProcessDetial[0].GetType().GetProperties();

            foreach (EntitySampProcessDetail sampProcess in listSampProcessDetial)
            {
                try
                {
                    result.Add(helper.ConverToDBSQL(sampProcess, propertys, "Sample_process_detial"));
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }

            return result;
        }
    }
}
