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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSample>))]
    public class DaoDicSample : IDaoDic<EntityDicSample>
    {
        public bool Save(EntityDicSample sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsam_id", id);
                values.Add("Dsam_name", sample.SamName);
                values.Add("Dsam_code", sample.SamCode);
                values.Add("Dsam_c_code", sample.SamCCode);
                values.Add("Dsam_Dpro_id", sample.SamProId);
                values.Add("py_code", sample.SamPyCode);
                values.Add("wb_code", sample.SamWbCode);
                values.Add("sort_no", sample.SamSortNo);
                values.Add("del_flag", sample.SamDelFlag);
                values.Add("Dsam_custom_type", sample.SamCustomType);
                values.Add("Dsam_trans_code", sample.SamTransType);
                values.Add("Dsam_bac_flag", sample.SamBacFlag);

                helper.InsertOperation("Dict_sample", values);

                sample.SamId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSample sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsam_name", sample.SamName);
                values.Add("Dsam_code", sample.SamCode);
                values.Add("Dsam_c_code", sample.SamCCode);
                values.Add("Dsam_Dpro_id", sample.SamProId);
                values.Add("py_code", sample.SamPyCode);
                values.Add("wb_code", sample.SamWbCode);
                values.Add("sort_no", sample.SamSortNo);
                values.Add("del_flag", sample.SamDelFlag);
                values.Add("Dsam_custom_type", sample.SamCustomType);
                values.Add("Dsam_trans_code", sample.SamTransType);
                values.Add("Dsam_bac_flag", sample.SamBacFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dsam_id", sample.SamId);

                helper.UpdateOperation("Dict_sample", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSample sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dsam_id", sample.SamId);

                helper.UpdateOperation("Dict_sample", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSample> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT  Dict_sample.Dsam_id,Dict_sample.Dsam_name, Dict_sample.Dsam_code, 
Dict_sample.Dsam_c_code, Dict_sample.Dsam_Dpro_id, Dict_sample.py_code, 
Dict_sample.wb_code, Dict_sample.sort_no, Dict_sample.del_flag,0 sp_select,
Dict_profession.Dpro_name, Dict_sample.Dsam_custom_type, Dict_sample.Dsam_trans_code,Dict_sample.Dsam_bac_flag
FROM Dict_sample 
LEFT OUTER JOIN Dict_profession ON Dict_sample.Dsam_Dpro_id = Dict_profession.Dpro_id  
where Dict_sample.del_flag=0 and Dsam_id<>'-1'";
                if (obj != null && obj.ToString() == "cache")
                {
                    sql = "select * from Dict_sample where del_flag=0";
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSample>();
            }
        }

        public List<EntityDicSample> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicSample> list = EntityManager<EntityDicSample>.ConvertToList(dtSour);
            //foreach (DataRow item in dtSour.Rows)
            //{
            //    EntityDicSample sample = new EntityDicSample();

            //    sample.SamId = item["sam_id"].ToString();
            //    sample.SamName = item["sam_name"].ToString();
            //    sample.SamCode = item["sam_code"].ToString();
            //    sample.SamCCode = item["c_code"].ToString();
            //    sample.SamProId = item["pro_id"].ToString();
            //    sample.SamPyCode = item["py_code"].ToString();
            //    sample.SamWbCode = item["wb_code"].ToString();

            //    int sort = 0;
            //    if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
            //        int.TryParse(item["sort_no"].ToString(), out sort);
            //    sample.SamSortNo = sort;

            //    sample.SamDelFlag = item["del_flag"].ToString();
            //    sample.SamCustomType = item["sam_custom_type"].ToString();
            //    sample.SamTransType = item["sam_trans_code"].ToString();

            //    sample.TypeName = item["pro_name"].ToString();

            //    list.Add(sample);
            //}
            return list.OrderBy(i => i.SamSortNo).ToList();
        }
    }
}
