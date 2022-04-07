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
    /// <summary>
    /// 酶标板孔位序号表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaSort>))]
    public class DaoDicElisaSort : IDaoDic<EntityDicElisaSort>
    {
        public bool Save(EntityDicElisaSort sort)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_Elisa_plate_sort");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Deps_id", id);
                values.Add("Deps_name", sort.SortName);
                values.Add("Deps_hole_sorting", sort.SortHoleSorting);
                values.Add("Deps_Dplate_id", sort.PlateId);
                values.Add("del_flag", sort.DelFlag);

                helper.InsertOperation("Dict_Elisa_plate_sort", values);

                sort.SortId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaSort sort)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Deps_name", sort.SortName);
                values.Add("Deps_hole_sorting", sort.SortHoleSorting);
                values.Add("Deps_Dplate_id", sort.PlateId);
                values.Add("del_flag", sort.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Deps_id", sort.SortId);

                helper.UpdateOperation("Dict_Elisa_plate_sort", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaSort sort)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", 1);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Deps_id", sort.SortId);

                helper.UpdateOperation("Dict_Elisa_plate_sort", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaSort> Search(Object obj)
        {
                try
                {
                 String sql = String.Format(@"SELECT * FROM Dict_Elisa_plate_sort WITH(NOLOCK) where del_flag = '0'");
                DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    return ConvertToEntitys(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return new List<EntityDicElisaSort>();
                }
        }

        public List<EntityDicElisaSort> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicElisaSort> list = EntityManager<EntityDicElisaSort>.ConvertToList(dtSour);
            return list.OrderBy(i => i.SortId).ToList();
        }
    }
}
