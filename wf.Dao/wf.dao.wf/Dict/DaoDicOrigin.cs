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

    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicOrigin>))]
    public class DaoDicOrigin : IDaoDic<EntityDicOrigin>
    {
        public bool Save(EntityDicOrigin origin)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_source");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsorc_id", id);
                values.Add("Dsorc_name", origin.SrcName);
                values.Add("Dsorc_c_code", origin.CCode);
                values.Add("py_code", origin.PyCode);
                values.Add("wb_code", origin.WbCode);
                values.Add("sort_no", origin.SortNo);
                values.Add("del_flag", origin.DelFlag);

                helper.InsertOperation("Dict_source", values);

                origin.SrcId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicOrigin origin)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsorc_name", origin.SrcName);
                values.Add("Dsorc_c_code", origin.CCode);
                values.Add("py_code", origin.PyCode);
                values.Add("wb_code", origin.WbCode);
                values.Add("sort_no", origin.SortNo);
                values.Add("del_flag", origin.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dsorc_id", origin.SrcId);

                helper.UpdateOperation("Dict_source", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicOrigin origin)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dsorc_id", origin.SrcId);

                helper.UpdateOperation("Dict_source", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicOrigin> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT * FROM Dict_source WHERE del_flag='0' and Dsorc_id<>'-1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicOrigin>();
            }
        }

        public List<EntityDicOrigin> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicOrigin> list = new List<EntityDicOrigin>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicOrigin origin = new EntityDicOrigin();

                origin.SrcId = item["Dsorc_id"].ToString();
                origin.SrcName = item["Dsorc_name"].ToString();
                origin.CCode = item["Dsorc_c_code"].ToString();
                origin.PyCode = item["py_code"].ToString();
                origin.WbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                origin.SortNo = sort;

                origin.DelFlag = item["del_flag"].ToString();
                origin.Checked = false;
                

                list.Add(origin);
            }
            return list.OrderBy(i => i.SortNo).ToList();
        }
    }
}
