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

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicQcConvert>))]
    public class DaoDicQcConvert : IDaoDic<EntityDicQcConvert>
    {
        public bool Save(EntityDicQcConvert convert)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_qc_convert");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rqcv_id", id);
                values.Add("Rqcv_Ditr_id", convert.ItrId);
                values.Add("Rqcv_Ditm_id", convert.ItmId);
                values.Add("Rqcv_value", convert.ItmValue);
                values.Add("Rqcv_convert_value", convert.ItmConvertValue);
                values.Add("sort_no", convert.SortNo);
                
                helper.InsertOperation("Rel_qc_convert", values);

                convert.CovSn = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicQcConvert convert)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                
                values.Add("Rqcv_Ditr_id", convert.ItrId);
                values.Add("Rqcv_Ditm_id", convert.ItmId);
                values.Add("Rqcv_value", convert.ItmValue);
                values.Add("Rqcv_convert_value", convert.ItmConvertValue);
                values.Add("sort_no", convert.SortNo);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rqcv_id", convert.CovSn);

                helper.UpdateOperation("Rel_qc_convert", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicQcConvert convert)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "";
                if(convert!=null)
                {
                    sql = string.Format(@"delete from Rel_qc_convert where Rqcv_id='{0}'", convert.CovSn);
                }
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcConvert> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Rel_qc_convert ";

                if (obj != null)
                {
                    sql += string.Format(@"where Rqcv_Ditr_id = '{0}'", obj.ToString());
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcConvert>();
            }
        }

        public List<EntityDicQcConvert> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicQcConvert> list = EntityManager<EntityDicQcConvert>.ConvertToList(dtSour);

            return list.OrderBy(i => i.SortNo).ToList(); 
        }
    }
}
