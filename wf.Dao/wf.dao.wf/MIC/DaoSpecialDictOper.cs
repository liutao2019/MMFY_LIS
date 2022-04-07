using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(ISpecialDictOperDao))]
    public class DaoSpecialDictOper : ISpecialDictOperDao
    {
        #region 获取模板信息


        string GetTemplate_SQL = @"select Dict_mic_template.* 
from Dict_mic_template 
left join pid_mic_report_detail 
on pid_mic_report_detail.com_id = Dict_mic_template.Dtmp_Dcom_id
where Dict_mic_template.del_flag = 0
and pid_mic_report_detail.rep_pat_key = @rep_pat_key
and Dtmp_type = '8'
union all
select Dict_mic_template.* 
from Dict_mic_template
where Dtmp_type = '8' and (Dtmp_Dcom_id is null or Dtmp_Dcom_id = '')
order by Dict_mic_template.sort_no";

        string GetTempalteByType_SQL = @"                 
                select * from Dict_mic_template 
                where Dtmp_type = @tmp_type";

        public List<EntityDicMicTemplate> GetDicMicTemplate(string tempType, string patKey)
        {
            try
            {
                DBManager helper = new DBManager();
                List<DbParameter> list = new List<DbParameter>();
                list.Add(new SqlParameter("@rep_pat_key", patKey == null? string.Empty: patKey));
                list.Add(new SqlParameter("@tmp_type", tempType));
                DataTable dt = helper.ExecuteDtSql(GetTemplate_SQL, list);



                if (dt == null || dt.Rows.Count <= 0)
                {
                    list.Clear();
                    list.Add(new SqlParameter("@tmp_type", tempType));
                    dt = helper.ExecuteDtSql(GetTempalteByType_SQL, list);
                }

                List<EntityDicMicTemplate> tmpList = EntityManager<EntityDicMicTemplate>.ToList(dt);
                return tmpList;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取模板信息", ex);
                return null;
            }
        }

        #endregion


    }
}
