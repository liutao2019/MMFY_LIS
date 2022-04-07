using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaOrderTable))]
    public class DaoOaOrderTable : IDaoOaOrderTable
    {
        public bool DeleteOrderTable(EntityOaTable sample)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dot_code", sample.TabCode);
                helper.DeleteOperation("Dict_oa_type", key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityOaTable> GetOrderTable(object obj)
        {
            try
            {
                String sql = String.Format(@"select Dot_code,Dot_name from Dict_oa_type order by Len(Dot_code),Dot_code");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityOaTable> list = EntityManager<EntityOaTable>.ConvertToList(dt).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaTable>();
            }
        }


        public bool SaveOrderTable(EntityOaTable sample)
        {
            try
            {

                DBManager helper = new DBManager();
                int id = IdentityHelper.GetMedIdentity("Dict_oa_type");
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dot_code", id);
                values.Add("Dot_name", sample.TabName);
                helper.InsertOperation("Dict_oa_type", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateOrderTable(EntityOaTable sample)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dot_name", sample.TabName);
                Dictionary<string, object> keys = new Dictionary<string, object>(); 
                keys.Add("Dot_code", sample.TabCode);
                helper.UpdateOperation("Dict_oa_type", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

  
    }
}
