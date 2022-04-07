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
    [Export("wf.plugin.wf", typeof(IDaoQcMateriaDetail))]
    public class DaoDicQcMateriaDetail : IDaoQcMateriaDetail
    {
        public EntityResponse SaveQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_qc_materia_detail");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rmatdet_id", id);
                values.Add("Rmatdet_Ditr_id", QMDetail.MatItrId);
                values.Add("Rmatdet_Ditm_id", QMDetail.MatItmId);
                values.Add("Rmatdet_Ditm_name", QMDetail.MatItmName);
                values.Add("Rmatdet_amount", QMDetail.MatAmount);

                values.Add("Rmatdet_rule", QMDetail.MatRuleId);
                values.Add("Rmatdet_itm_unit", QMDetail.MatItmUnit);
                values.Add("Rmatdet_Dmat_id", QMDetail.MatId);
                values.Add("Rmatdet_itm_x", QMDetail.MatItmX);
                values.Add("Rmatdet_itm_sd", QMDetail.MatItmSd);

                values.Add("Rmatdet_itm_ccv", QMDetail.MatItmCcv);
                values.Add("Rmatdet_reag_manufacturer", QMDetail.MatReagManufacturer);
                values.Add("Rmatdet_reag_batchno", QMDetail.MatReagBatchno);
                values.Add("Rmatdet_read_valid_date", QMDetail.MatReadValidDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rmatdet_m_pro", QMDetail.MatMPro);

                values.Add("Rmatdet_itm_cv", QMDetail.MatItmCv);
                values.Add("Rmatdet_max_value", QMDetail.MatMaxValue);
                values.Add("Rmatdet_min_value", QMDetail.MatMinValue);
                values.Add("Rmatdet_value_type", QMDetail.MatValueType);
                values.Add("Rmatdet_allow_cv", QMDetail.MatAllowCv);

                helper.InsertOperation("Rel_qc_materia_detail", values);

                QMDetail.MatDetId = id.ToString();

                result.Scusess = true;
                result.SetResult(QMDetail);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }

        public bool UpdateQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Rmatdet_Ditr_id", QMDetail.MatItrId);
                values.Add("Rmatdet_Ditm_id", QMDetail.MatItmId);
                values.Add("Rmatdet_Ditm_name", QMDetail.MatItmName);
                values.Add("Rmatdet_amount", QMDetail.MatAmount);

                values.Add("Rmatdet_rule", QMDetail.MatRuleId);
                values.Add("Rmatdet_itm_unit", QMDetail.MatItmUnit);
                values.Add("Rmatdet_Dmat_id", QMDetail.MatId);
                values.Add("Rmatdet_itm_x", QMDetail.MatItmX);
                values.Add("Rmatdet_itm_sd", QMDetail.MatItmSd);

                values.Add("Rmatdet_itm_ccv", QMDetail.MatItmCcv);
                values.Add("Rmatdet_reag_manufacturer", QMDetail.MatReagManufacturer);
                values.Add("Rmatdet_reag_batchno", QMDetail.MatReagBatchno);
                values.Add("Rmatdet_read_valid_date", QMDetail.MatReadValidDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rmatdet_m_pro", QMDetail.MatMPro);

                values.Add("Rmatdet_itm_cv", QMDetail.MatItmCv);
                values.Add("Rmatdet_max_value", QMDetail.MatMaxValue);
                values.Add("Rmatdet_min_value", QMDetail.MatMinValue);
                values.Add("Rmatdet_value_type", QMDetail.MatValueType);
                values.Add("Rmatdet_allow_cv", QMDetail.MatAllowCv);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rmatdet_id", QMDetail.MatDetId);

                helper.UpdateOperation("Rel_qc_materia_detail", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcMateriaDetail> SearchQcMateriaDetail(string mat_id)
        {
            try
            {
                string strSql = string.Format(@"SELECT dbo.Rel_qc_materia_detail.*,Dict_itm.Ditm_ecode 
FROM dbo.Rel_qc_materia_detail 
LEFT JOIN dbo.Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id=Dict_itm.Ditm_id 
WHERE Rel_qc_materia_detail.Rmatdet_Dmat_id='{0}' ", mat_id);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateriaDetail> list = EntityManager<EntityDicQcMateriaDetail>.ConvertToList(dt).OrderBy(i => i.MatDetId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateriaDetail>();
            }
        }

        public bool DeleteQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = "";

                if (!string.IsNullOrEmpty(QMDetail.MatDetId))
                {
                    sqlStr = string.Format(@"delete from Rel_qc_materia_detail  where Rmatdet_id='{0}' ", QMDetail.MatDetId);
                }
                if (!string.IsNullOrEmpty(QMDetail.MatId))
                {
                    sqlStr = string.Format(@"DELETE Rel_qc_materia_detail WHERE Rmatdet_Dmat_id='{0}' ", QMDetail.MatId);
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

        public List<EntityDicQcMateriaDetail> GetQcMateriaDetailItmId(string strItrId)
        {
            try
            {
                string strSql = string.Format(@"SELECT Rmatdet_Ditm_id
FROM  Rel_qc_materia_detail 
WHERE Rmatdet_Ditr_id='{0}' 
group by Rmatdet_Ditm_id", strItrId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateriaDetail> list = EntityManager<EntityDicQcMateriaDetail>.ConvertToList(dt).OrderBy(i => i.MatDetId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateriaDetail>();
            }
        }

        public bool UpdateQcMateriaDetailCondition(EntityDicQcMateriaDetail QMDetail)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                string sql = @"update Rel_qc_materia_detail set Rel_qc_materia_detail.Rmatdet_itm_x=@mat_itm_x,
Rel_qc_materia_detail.Rmatdet_itm_sd=@mat_itm_sd,Rel_qc_materia_detail.Rmatdet_itm_cv=@mat_itm_cv 
where Rel_qc_materia_detail.Rmatdet_Ditm_id=@mat_itm_id and Rel_qc_materia_detail.Rmatdet_Ditr_id=@mat_itr_id 
and Rel_qc_materia_detail.Rmatdet_Dmat_id=@mat_id";
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@mat_itm_x", QMDetail.MatItmX));
                paramHt.Add(new SqlParameter("@mat_itm_sd", QMDetail.MatItmSd));
                paramHt.Add(new SqlParameter("@mat_itm_cv", QMDetail.MatItmCv));
                paramHt.Add(new SqlParameter("@mat_itm_id", QMDetail.MatItmId));
                paramHt.Add(new SqlParameter("@mat_itr_id", QMDetail.MatItrId));
                paramHt.Add(new SqlParameter("@mat_id", QMDetail.MatId));
                result = helper.ExecCommand(sql, paramHt)>0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
