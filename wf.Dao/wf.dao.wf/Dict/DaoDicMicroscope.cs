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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicroscope>))]
    public class DaoDicMicroscope : IDaoDic<EntityDicMicroscope>
    {
        public bool Save(EntityDicMicroscope Microscope)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itm_microscopy");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dimc_id", id);
                values.Add("Dimc_name", Microscope.MicroName);
                values.Add("sort_no", Microscope.MicroSortNo);
                values.Add("py_code", Microscope.MicroPyCode);
                values.Add("wb_code", Microscope.MicroWbCode);
                values.Add("del_flag", Microscope.MicroDelFlag);

                helper.InsertOperation("Dict_itm_microscopy", values);

                Microscope.MicroId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicMicroscope Microscope)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dimc_name", Microscope.MicroName);
                values.Add("sort_no", Microscope.MicroSortNo);
                values.Add("py_code", Microscope.MicroPyCode);
                values.Add("wb_code", Microscope.MicroWbCode);
                values.Add("del_flag", Microscope.MicroDelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dimc_id", Microscope.MicroId);

                helper.UpdateOperation("Dict_itm_microscopy", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicMicroscope Microscope)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dimc_id", Microscope.MicroId);

                helper.UpdateOperation("Dict_itm_microscopy", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicroscope> Search(Object obj)
        {
            try
            {
                String sql = @"select Dimc_id,Dimc_name,sort_no,py_code,wb_code,del_flag from Dict_itm_microscopy where del_flag=0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicroscope>();
            }
        }

        public List<EntityDicMicroscope> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicMicroscope> list = new List<EntityDicMicroscope>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicMicroscope microscope = new EntityDicMicroscope();

                microscope.MicroId = item["Dimc_id"].ToString();
                microscope.MicroName = item["Dimc_name"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                microscope.MicroSortNo = sort;

                microscope.MicroPyCode = item["py_code"].ToString();
                microscope.MicroWbCode = item["wb_code"].ToString();
                microscope.MicroDelFlag = item["del_flag"].ToString();

                list.Add(microscope);
            }
            return list.OrderBy(i => i.MicroSortNo).ToList();
        }
    }
}
