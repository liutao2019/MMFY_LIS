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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaUnit>))]
    public class DaoDicReaUnit : IDaoDic<EntityDicReaUnit>
    {
        public bool Delete(EntityDicReaUnit sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Runit_id", sample.Runit_id);

                helper.UpdateOperation("Dict_rea_unit", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaUnit sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_unit");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Runit_id", id);
                values.Add("Runit_name", sample.Runit_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                
                helper.InsertOperation("Dict_rea_unit", values);

                sample.Runit_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaUnit> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_unit where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaUnit>();
            }
        }

        public bool Update(EntityDicReaUnit sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Runit_name", sample.Runit_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("del_flag", sample.del_flag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Runit_id", sample.Runit_id);

                helper.UpdateOperation("Dict_rea_unit", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaUnit> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaUnit> list = EntityManager<EntityDicReaUnit>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
