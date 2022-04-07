using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResultBakItm))]
    public class DaoObrResultBakItm : IDaoObrResultBakItm
    {
        public List<EntityObrResultBakItm> GetObrResultBakItm(string RepId, int whereType)
        {
            string where = string.Empty;
            List<EntityObrResultBakItm> listResult = new List<EntityObrResultBakItm>();
            if (whereType == 1)
            {
                where += string.Format(@" and res_id='{0}' and bak_id is not null and bak_date is not null", RepId);
            }
            else if (whereType == 2)
            {
                where += string.Format(@" and bak_id='{0}'", RepId);
            }
            if (!string.IsNullOrEmpty(where))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"select
bak_id,
Dict_itm.Ditm_name,
res_itm_ecd,
res_itm_id,
res_chr,
res_unit,
case when isnull(res_ref_l, '') <> '' and isnull(res_ref_h, '') <> '' then isnull(res_ref_l, '') + ' - ' + isnull(res_ref_h, '')
when isnull(res_ref_l, '') <> '' then isnull(res_ref_l, '')
when isnull(res_ref_h, '') <> '' then isnull(res_ref_h, '') else '' end as res_ref_range,
res_id,
res_key,
bak_date
from Obr_result_bakitm with(nolock)
left join Dict_itm on Obr_result_bakitm.res_itm_id = Dict_itm.Ditm_id
where 1=1 {0}", where);
                DataTable dt = helper.ExecuteDtSql(sql);
                return EntityManager<EntityObrResultBakItm>.ConvertToList(dt);
            }
            return listResult;
        }

        public String InsertObrResultBakItm(string resId, string bakId, DateTime bakDate, List<string> resItmIds, List<string> resKeys)
        {
            String result = string.Empty;
            #region 生成where条件
            string sqlappwhere = string.Empty;
            if (resItmIds.Count > 0)
            {
                string sqlappStr = "";
                foreach (string strtemp in resItmIds)
                {
                    sqlappStr += string.Format(",'{0}'", strtemp);
                }

                if (!string.IsNullOrEmpty(sqlappStr))
                {
                    sqlappStr = sqlappStr.Remove(0, 1);
                    sqlappwhere += string.Format(@" and Lis_result.Lres_Ditm_id in ({0})", sqlappStr);
                }
            }

            if (resKeys.Count > 0)
            {
                string sqladdStr = "";
                foreach (string strtemp in resKeys)
                {
                    sqladdStr += string.Format(",'{0}'", strtemp);
                }

                if (!string.IsNullOrEmpty(sqladdStr))
                {
                    sqladdStr = sqladdStr.Remove(0, 1);
                    sqlappwhere += string.Format(@" and Lis_result.Lres_id in ({0})", sqladdStr);
                }
            }

            #endregion

            #region 新增项目结果备份SQL语句

            string sqlInsertResultoBakitm = string.Format(@"INSERT INTO Obr_result_bakitm
           ([res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3]
           ,bak_id
           ,bak_date)
select [Lres_Pma_rep_id]
           ,[Lres_Ditr_id]
           ,[Lres_sid]
           ,[Lres_Ditm_id]
           ,[Lres_itm_ename]
           ,[Lres_value]
           ,[Lres_value2]
           ,[Lres_convert_value]
           ,[Lres_unit]
           ,[Lres_price]
           ,[Lres_lower_limit]
           ,[Lres_upper_limit]
           ,[Lres_more]
           ,[Lres_ref_flag]
           ,[Lres_itm_method]
           ,[Lres_date]
           ,[Lres_flag]
           ,[Lres_type]
           ,[Lres_itm_report_type]
           ,[Lres_itm_Dcom_id]
           ,[Lres_itm_report_code]
           ,[Lres_source_Ditr_id]
           ,[Lres_ref_type]
           ,[Lres_remark]
           ,[Lres_recheck_flag]
           ,[Lres_value3]
           ,[Lres_value4]
           ,'{1}'
           ,'{2}'
from Lis_result
where Lres_Pma_rep_id='{0}'
and Lres_flag=1
{3}
", resId, bakId, bakDate, sqlappwhere);
            #endregion

            DBManager helper = new DBManager();
            try
            {
                helper.ExecCommand(sqlInsertResultoBakitm);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = ex.Message;
            }
            return result;
        }

        public string InsertObrResultByBak(List<string> resKeys)
        {
            string result = string.Empty;
            #region 生成where条件
            string sqlappwhere = string.Empty;

            if (resKeys.Count > 0)
            {
                string sqladdStr = "";
                foreach (string strtemp in resKeys)
                {
                    sqladdStr += string.Format(",'{0}'", strtemp);
                }

                if (!string.IsNullOrEmpty(sqladdStr))
                {
                    sqladdStr = sqladdStr.Remove(0, 1);
                    sqlappwhere += string.Format(@" and Obr_result_bakitm.res_key in ({0})", sqladdStr);
                }
            }

            #endregion

            #region 还原备份的项目结果SQL语句

            string sqlInsertResultoBakitm = string.Format(@"INSERT INTO Lis_result
           ([Lres_Pma_rep_id]
           ,[Lres_Ditr_id]
           ,[Lres_sid]
           ,[Lres_Ditm_id]
           ,[Lres_itm_ename]
           ,[Lres_value]
           ,[Lres_value2]
           ,[Lres_convert_value]
           ,[Lres_unit]
           ,[Lres_price]
           ,[Lres_lower_limit]
           ,[Lres_upper_limit]
           ,[Lres_more]
           ,[Lres_ref_flag]
           ,[Lres_itm_method]
           ,[Lres_date]
           ,[Lres_flag]
           ,[Lres_type]
           ,[Lres_itm_report_type]
           ,[Lres_itm_Dcom_id]
           ,[Lres_itm_report_code]
           ,[Lres_source_Ditr_id]
           ,[Lres_ref_type]
           ,[Lres_remark]
           ,[Lres_recheck_flag]
           ,[Lres_value3]
           ,[Lres_value4])
select [res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3]
from Obr_result_bakitm
where 1=1 {0}", sqlappwhere);
            #endregion

            DBManager helper = new DBManager();
            try
            {
                helper.ExecCommand(sqlInsertResultoBakitm);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = ex.Message;
            }
            return result;
        }

    }
}
