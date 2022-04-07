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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaDept>))]
    public class DaoDicReaDept : IDaoDic<EntityDicReaDept>
    {
        public bool Delete(EntityDicReaDept sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rdept_id", sample.Rdept_id);

                helper.UpdateOperation("Dict_rea_dept", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaDept sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_dept");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rdept_id", id);
                values.Add("Rclaim_Dept", sample.Rclaim_Dept);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                
                helper.InsertOperation("Dict_rea_dept", values);

                sample.Rdept_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaDept> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_dept where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaDept>();
            }
        }

        public bool Update(EntityDicReaDept sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rclaim_Dept", sample.Rclaim_Dept);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("del_flag", sample.del_flag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rdept_id", sample.Rdept_id);

                helper.UpdateOperation("Dict_rea_dept", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaDept> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaDept> list = EntityManager<EntityDicReaDept>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
