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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaStorePos>))]
    public class DaoDicReaStorePosition : IDaoDic<EntityDicReaStorePos>
    {
        public bool Delete(EntityDicReaStorePos sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rstr_position_id", sample.Rstr_position_id);

                helper.UpdateOperation("Dict_rea_store_position", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaStorePos sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_store_position");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rstr_position_id", id);
                values.Add("Rstr_position", sample.Rstr_position);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                
                helper.InsertOperation("Dict_rea_store_position", values);

                sample.Rstr_position_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaStorePos> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_store_position where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaStorePos>();
            }
        }

        public bool Update(EntityDicReaStorePos sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rstr_position", sample.Rstr_position);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("del_flag", sample.del_flag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rstr_position_id", sample.Rstr_position_id);

                helper.UpdateOperation("Dict_rea_store_position", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaStorePos> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaStorePos> list = EntityManager<EntityDicReaStorePos>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
