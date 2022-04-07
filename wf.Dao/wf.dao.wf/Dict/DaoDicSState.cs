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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSState>))]
    public class DaoDicSState : IDaoDic<EntityDicSState>
    {
        public bool Save(EntityDicSState state)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_status");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dstau_id", id);
                values.Add("Dstau_name", state.StauName);
                values.Add("Dstau_c_code", state.CCode);
                values.Add("Dstau_Dpro_id", state.ProId);
                values.Add("sort_no", state.SortNo);
                values.Add("py_code", state.PyCode);
                values.Add("wb_code", state.WbCode);
                values.Add("del_flag", state.DelFlag);

                helper.InsertOperation("Dict_sample_status", values);

                state.StauId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSState state)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dstau_name", state.StauName);
                values.Add("Dstau_c_code", state.CCode);
                values.Add("Dstau_Dpro_id", state.ProId);
                values.Add("sort_no", state.SortNo);
                values.Add("py_code", state.PyCode);
                values.Add("wb_code", state.WbCode);
                values.Add("del_flag", state.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dstau_id", state.StauId);

                helper.UpdateOperation("Dict_sample_status", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSState state)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dstau_id", state.StauId);

                helper.UpdateOperation("Dict_sample_status", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSState> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT  Dict_sample_status.Dstau_id,Dict_sample_status.Dstau_name,Dict_sample_status.Dstau_c_code,
Dict_sample_status.Dstau_Dpro_id,Dict_sample_status.sort_no,Dict_sample_status.py_code,
Dict_sample_status.wb_code,Dict_sample_status.del_flag,Dict_profession.Dpro_name
FROM  Dict_sample_status 
LEFT OUTER JOIN Dict_profession ON Dict_sample_status.Dstau_Dpro_id=Dict_profession.Dpro_id
WHERE Dict_sample_status.del_flag='0' AND Dict_sample_status.Dstau_id<>'-1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSState>();
            }
        }

        public List<EntityDicSState> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicSState> list = new List<EntityDicSState>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicSState state = new EntityDicSState();

                state.StauId = item["Dstau_id"].ToString();
                state.StauName = item["Dstau_name"].ToString();
                state.CCode = item["Dstau_c_code"].ToString();
                state.ProId = item["Dstau_Dpro_id"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                state.SortNo = sort;

                state.PyCode = item["py_code"].ToString();
                state.WbCode = item["wb_code"].ToString();
                state.DelFlag = item["del_flag"].ToString();
                state.ProName = item["Dpro_name"].ToString();

                list.Add(state);
            }
            return list.OrderBy(i => i.SortNo).ToList();
        }
    }
}
