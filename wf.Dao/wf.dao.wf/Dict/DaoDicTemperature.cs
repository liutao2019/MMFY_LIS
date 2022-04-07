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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicTemperature>))]
    public class DaoDicTemperature : IDaoDic<EntityDicTemperature>
    {
        public bool Delete(EntityDicTemperature temp)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("dt_id", temp.DtId);

                helper.UpdateOperation("dict_temperature", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicTemperature temp)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("dict_temperature");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("dt_id", id);
                values.Add("dt_code", temp.DtCode);
                values.Add("dt_name", temp.DtName);
                values.Add("dt_h_limit", temp.DtHLimit);
                values.Add("dt_l_limit", temp.DtLLimit);
                values.Add("wb_code", temp.WbCode);
                values.Add("py_code", temp.PyCode);
                values.Add("del_flag", temp.DelFlag);

                helper.InsertOperation("dict_temperature", values);
                temp.DtId = id.ToString();
                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicTemperature> Search(object obj)
        {
            try
            {
                string sql = "select * from dict_temperature where del_flag='0'";

                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicTemperature>();
            }
        }

        private List<EntityDicTemperature> ConvertToEntitys(DataTable dt)
        {
            List<EntityDicTemperature> list = EntityManager<EntityDicTemperature>.ConvertToList(dt);

            //使用foreach就会出现两行相同的数据
            //foreach (DataRow item in dt.Rows)
            //{
            //    EntityDicTemperature temp = new EntityDicTemperature();
            //    temp.DtId = item["dt_id"].ToString();
            //    temp.DtCode = item["dt_code"].ToString();
            //    temp.DtName = item["dt_name"].ToString();
            //    temp.DtHLimit = item["dt_h_limit"].ToString();
            //    temp.DtLLimit = item["dt_l_limit"].ToString();
            //    temp.WbCode = item["wb_code"].ToString();
            //    temp.PyCode = item["py_code"].ToString();
            //    temp.DelFlag = item["del_flag"].ToString();
            //    list.Add(temp);
            //}
            return list;
        }

        public bool Update(EntityDicTemperature temp)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("dt_code", temp.DtCode);
                values.Add("dt_name", temp.DtName);
                values.Add("dt_h_limit", temp.DtHLimit);
                values.Add("dt_l_limit", temp.DtLLimit);
                values.Add("wb_code", temp.WbCode);
                values.Add("py_code", temp.PyCode);
                values.Add("del_flag", temp.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("dt_id", temp.DtId);

                helper.UpdateOperation("dict_temperature", values, keys);
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
