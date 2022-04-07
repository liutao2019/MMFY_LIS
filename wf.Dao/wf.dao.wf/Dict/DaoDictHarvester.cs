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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDictHarvester>))]
    public class DaoDictHarvester : IDaoDic<EntityDictHarvester>
    {
        public bool Delete(EntityDictHarvester harvester)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", 1);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("dh_id", harvester.DhId);

                helper.UpdateOperation("dict_harvester", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDictHarvester harvester)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("dict_harvester");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("dh_id", id);
                values.Add("dh_code", harvester.DhCode);
                values.Add("dh_name", harvester.DhName);
                values.Add("py_code", harvester.PyCode);
                values.Add("wb_code", harvester.WbCode);
                values.Add("del_flag", harvester.DelFlag);

                helper.InsertOperation("dict_harvester", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDictHarvester> Search(object obj)
        {
            try
            {
                string sql = "select * from dict_harvester where del_flag=0";

                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDictHarvester> list = EntityManager<EntityDictHarvester>.ConvertToList(dt);
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDictHarvester>();
            }
        }

        public bool Update(EntityDictHarvester harvester)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("dh_code", harvester.DhCode);
                values.Add("dh_name", harvester.DhName);
                values.Add("py_code", harvester.PyCode);
                values.Add("wb_code", harvester.WbCode);
                values.Add("del_flag", harvester.DelFlag);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("dh_id", harvester.DhId);

                helper.UpdateOperation("dict_harvester", values, key);
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
