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
    /// 项目与酶标板孔位的对照表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaItem>))]
    public class DaoDicElisaItem : IDaoDic<EntityDicElisaItem>
    {
        public bool Save(EntityDicElisaItem item)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_Elisa_item_plate");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Riplate_id", id);
                values.Add("Riplate_Ditm_id", item.IplateItmId);
                values.Add("Riplate_Deps_id", item.IplateSortId);
                values.Add("Riplate_Depsta_id", item.IplateStaId);
                values.Add("Riplate_Dplate_id", item.PlateId);
                values.Add("del_flag", item.DelFlag); 
                values.Add("Riplate_resulttype", item.IplateResulttype);

                helper.InsertOperation("Rel_Elisa_item_plate", values);

                item.IplateId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaItem item)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Riplate_Ditm_id", item.IplateItmId);
                values.Add("Riplate_Deps_id", item.IplateSortId);
                values.Add("Riplate_Depsta_id", item.IplateStaId);
                values.Add("Riplate_Dplate_id", item.PlateId);
                values.Add("del_flag", item.DelFlag);
                values.Add("Riplate_resulttype", item.IplateResulttype);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Riplate_id", item.IplateId);

                helper.UpdateOperation("Rel_Elisa_item_plate", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaItem item)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", 1);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Riplate_id", item.IplateId);

                helper.UpdateOperation("Rel_Elisa_item_plate", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaItem> Search(Object obj)
        {
                try
                {
                 String sql = String.Format(@"select * from Rel_Elisa_item_plate where del_flag = '0'");
                DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    return ConvertToEntitys(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return new List<EntityDicElisaItem>();
                }
        }

        public List<EntityDicElisaItem> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicElisaItem> list = EntityManager<EntityDicElisaItem>.ConvertToList(dtSour);
            return list.OrderBy(i => i.IplateId).ToList();
        }
    }
}
