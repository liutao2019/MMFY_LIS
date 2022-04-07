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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaMeaning>))]
    public class DaoDicElisaMeaning : IDaoDic<EntityDicElisaMeaning>
    {
        public bool Save(EntityDicElisaMeaning mean)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_elisa_meaning");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Delisa_id", id);
                values.Add("Delisa_item_a", mean.MeagItemA);
                values.Add("Delisa_item_b", mean.MeagItemB);
                values.Add("Delisa_item_c", mean.MeagItemC);
                values.Add("Delisa_item_d", mean.MeagItemD);
                values.Add("Delisa_item_e", mean.MeagItemE);
                values.Add("Delisa_exp", mean.MeagExp);
                values.Add("Delisa_probability", mean.MeagProbability);
                values.Add("Delisa_conclusion", mean.MeagConclusion);
                values.Add("Delisa_remark", mean.MeagRemark);

                helper.InsertOperation("Dict_elisa_meaning", values);

                mean.MeagId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaMeaning mean)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Delisa_id", mean.MeagId);
                values.Add("Delisa_item_a", mean.MeagItemA);
                values.Add("Delisa_item_b", mean.MeagItemB);
                values.Add("Delisa_item_c", mean.MeagItemC);
                values.Add("Delisa_item_d", mean.MeagItemD);
                values.Add("Delisa_item_e", mean.MeagItemE);
                values.Add("Delisa_exp", mean.MeagExp);
                values.Add("Delisa_probability", mean.MeagProbability);
                values.Add("Delisa_conclusion", mean.MeagConclusion);
                values.Add("Delisa_remark", mean.MeagRemark);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Delisa_id", mean.MeagId);

                helper.UpdateOperation("Dict_elisa_meaning", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaMeaning mean)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Delisa_id", mean.MeagId);

                helper.DeleteOperation("Dict_elisa_meaning", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaMeaning> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Dict_elisa_meaning ";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicElisaMeaning> list = EntityManager<EntityDicElisaMeaning>.ConvertToList(dt).OrderBy(i => i.MeagId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicElisaMeaning>();
            }
        }
    }
}
