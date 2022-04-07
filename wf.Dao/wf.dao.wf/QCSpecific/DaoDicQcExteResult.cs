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

namespace dcl.dao.clab
{

    [Export("wf.plugin.wf", typeof(IDaoDic<EntityQcExteResult>))]
    public class DaoDicresult : IDaoDic<EntityQcExteResult>
    {
        public bool Save(EntityQcExteResult result)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Lis_qc_eqa");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Leqa_id", id);
                values.Add("Leqa_Dmat_id", result.QresSid);
                values.Add("Leqa_date", result.QresDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Leqa_Ditm_id", result.QresItmId);
                values.Add("Leqa_value", result.QresValue);
                values.Add("Leqa_itm_x", result.QresItmX);
                values.Add("Leqa_itm_sd", result.QresItmSd);
                values.Add("Leqa_itm_cv", result.QresItmCv);
                values.Add("Leqa_pt_lower_limit", result.QresPtLowerLimit);
                values.Add("Leqa_pt_upper_limit", result.QresPtUpperLimit);
                values.Add("Leqa_pt_fraction", result.QresPtFraction);
                values.Add("Leqa_reasons", result.QresReasons);
                values.Add("Leqa_process", result.QresProcess);
                
                helper.InsertOperation("Lis_qc_eqa", values);

                result.QresSn = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityQcExteResult result)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                
                values.Add("Leqa_Dmat_id", result.QresSid);
                values.Add("Leqa_date", result.QresDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Leqa_Ditm_id", result.QresItmId);
                values.Add("Leqa_value", result.QresValue);
                values.Add("Leqa_itm_x", result.QresItmX);
                values.Add("Leqa_itm_sd", result.QresItmSd);
                values.Add("Leqa_itm_cv", result.QresItmCv);
                values.Add("Leqa_pt_lower_limit", result.QresPtLowerLimit);
                values.Add("Leqa_pt_upper_limit", result.QresPtUpperLimit);
                values.Add("Leqa_pt_fraction", result.QresPtFraction);
                values.Add("Leqa_reasons", result.QresReasons);
                values.Add("Leqa_process", result.QresProcess);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Leqa_id", result.QresSn);

                helper.UpdateOperation("Lis_qc_eqa", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityQcExteResult result)
        {
            try
            {
                DBManager helper = new DBManager();
                string sqlStr = null;
                if (result != null)
                {
                    sqlStr = string.Format(@"delect from Lis_qc_eqa where Leqa_id='{0}' ", result.QresSn);

                }
                helper.ExecSql(sqlStr);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityQcExteResult> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Lis_qc_eqa where Leqa_id<>'-1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityQcExteResult>();
            }
        }

        public List<EntityQcExteResult> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityQcExteResult> list = EntityManager<EntityQcExteResult>.ConvertToList(dtSour);

            return list.ToList();
        }
    }
}
