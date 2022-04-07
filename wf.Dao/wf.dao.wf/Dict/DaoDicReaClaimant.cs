using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.dao.core;
using dcl.common;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaClaimant>))]
    public class DaoDicReaClaimant : IDaoDic<EntityDicReaClaimant>
    {
        public bool Delete(EntityDicReaClaimant sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rclaimant_id", sample.Rclaimant_id);

                helper.UpdateOperation("Dict_rea_claimant", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaClaimant sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_claimant");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rclaimant_id", id);
                values.Add("Rclaimant", sample.Rclaimant);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                
                helper.InsertOperation("Dict_rea_claimant", values);

                sample.Rclaimant_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaClaimant> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_claimant where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaClaimant>();
            }
        }

        public bool Update(EntityDicReaClaimant sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rclaimant", sample.Rclaimant);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("del_flag", sample.del_flag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rclaimant_id", sample.Rclaimant_id);

                helper.UpdateOperation("Dict_rea_claimant", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaClaimant> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaClaimant> list = EntityManager<EntityDicReaClaimant>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
