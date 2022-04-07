using dcl.dao.interfaces;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaStoreCondition>))]
    public class DaoDicReaStoreCondition : IDaoDic<EntityDicReaStoreCondition>
    {
        public bool Delete(EntityDicReaStoreCondition sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rstr_condition_id", sample.Rstr_condition_id);

                helper.UpdateOperation("Dict_rea_strcondition", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaStoreCondition sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_strcondition");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rstr_condition_id", id);
                values.Add("Rstr_condition", sample.Rstr_condition);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                helper.InsertOperation("Dict_rea_strcondition", values);

                sample.Rstr_condition_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaStoreCondition> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_strcondition where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaStoreCondition>();
            }
        }

        public bool Update(EntityDicReaStoreCondition sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rstr_condition", sample.Rstr_condition);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("del_flag", sample.del_flag);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rstr_condition_id", sample.Rstr_condition_id);

                helper.UpdateOperation("Dict_rea_strcondition", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaStoreCondition> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaStoreCondition> list = EntityManager<EntityDicReaStoreCondition>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
