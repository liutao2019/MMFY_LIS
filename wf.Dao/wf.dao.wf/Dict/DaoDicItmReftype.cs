using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmReftype>))]
    public class DaoDicItmReftype : IDaoDic<EntityDicItmReftype>
    {
        public bool Save(EntityDicItmReftype refe)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itm_refervalue");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drefv_id", id);
                values.Add("Drefv_name", refe.RefName);
                values.Add("Drefv_age_h", refe.RefAgeHigh);
                values.Add("Drefv_age_l", refe.RefAgeLower);
                values.Add("py_code", refe.RefPyCode);
                values.Add("wb_code", refe.RefWbCode);
                values.Add("Drefv_age_h_unit", refe.RefAgeHighUnit);
                values.Add("Drefv_age_l_unit", refe.RefAgeLowerUnit);
                values.Add("sort_no", refe.RefSortNo);
                values.Add("del_flag", refe.RefDelFlag);
                helper.InsertOperation("Dict_itm_refervalue", values);
                refe.RefId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmReftype refe)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drefv_id", refe.RefId);
                values.Add("Drefv_name", refe.RefName);
                values.Add("Drefv_age_h", refe.RefAgeHigh);
                values.Add("Drefv_age_l", refe.RefAgeLower);
                values.Add("Drefv_age_h_unit", refe.RefAgeHighUnit);
                values.Add("Drefv_age_l_unit", refe.RefAgeLowerUnit);
                values.Add("py_code", refe.RefPyCode);
                values.Add("wb_code", refe.RefWbCode);
                values.Add("sort_no", refe.RefSortNo);
                values.Add("del_flag", refe.RefDelFlag);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drefv_id", refe.RefId);
                helper.UpdateOperation("Dict_itm_refervalue", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmReftype refe)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drefv_id", refe.RefId);

                helper.UpdateOperation("Dict_itm_refervalue", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmReftype> Search(Object obj)
        {

            try
            {
                String sql = @"select Drefv_id, Drefv_name, Drefv_age_h, Drefv_age_l, py_code, wb_code, 
Drefv_age_h_unit, Drefv_age_l_unit, sort_no, del_flag 
from Dict_itm_refervalue 
where del_flag='0' or del_flag is null ";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItmReftype>();
            }
        }

        public List<EntityDicItmReftype> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItmReftype> list = new List<EntityDicItmReftype>();
            foreach (DataRow item in dtSour.Rows)
            {

                EntityDicItmReftype refe = new EntityDicItmReftype();
                refe.RefId = item["Drefv_id"].ToString();
                refe.RefName = item["Drefv_name"].ToString();
                refe.RefPyCode = item["py_code"].ToString();
                refe.RefWbCode = item["wb_code"].ToString();
                refe.RefDelFlag = item["del_flag"].ToString();
                refe.RefAgeHighUnit = item["Drefv_age_h_unit"].ToString();
                refe.RefAgeLowerUnit = item["Drefv_age_l_unit"].ToString();
                int agehigh = 0, agelower = 0,sort=0;
                if (item["Drefv_age_h"] != null && item["Drefv_age_h"] != DBNull.Value)
                    int.TryParse(item["Drefv_age_h"].ToString(), out agehigh);
                refe.RefAgeHigh = agehigh;
                if (item["Drefv_age_l"] != null && item["Drefv_age_l"] != DBNull.Value)
                    int.TryParse(item["Drefv_age_l"].ToString(), out agelower);
                refe.RefAgeLower = agelower;
                if (item["Drefv_age_l"] != null && item["Drefv_age_l"] != DBNull.Value)
                    int.TryParse(item["Drefv_age_l"].ToString(), out sort);
                refe.RefSortNo = sort;
                list.Add(refe);
            }
            return list.OrderBy(i => i.RefId).ToList();
        }
    }
}
