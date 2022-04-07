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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaCalu>))]
    public class DaoDicElisaCalu : IDaoDic<EntityDicElisaCalu>
    {
        public bool Save(EntityDicElisaCalu calu)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_Elisa_calculaformula");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Recal_id", id);
                values.Add("Recal_Ditm_id", calu.CalItmId);
                values.Add("Recal_expression", calu.CalExpression);
                values.Add("del_flag", calu.CalDelFlag);

                helper.InsertOperation("Rel_Elisa_calculaformula", values);

                calu.CalId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaCalu calu)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Recal_id", calu.CalId);
                values.Add("Recal_Ditm_id", calu.CalItmId);
                values.Add("Recal_expression", calu.CalExpression);
                values.Add("del_flag", calu.CalDelFlag);
    
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Recal_id", calu.CalId);

                helper.UpdateOperation("Rel_Elisa_calculaformula", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaCalu calu)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Recal_id", calu.CalId);

                helper.DeleteOperation("Rel_Elisa_calculaformula", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaCalu> Search(Object obj)
        {
            try
            {
                String sql = @"select Recal_id, Recal_Ditm_id, Recal_expression, del_flag from Rel_Elisa_calculaformula";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicElisaCalu> list = EntityManager<EntityDicElisaCalu>.ConvertToList(dt).OrderBy(i => i.CalId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicElisaCalu>();
            }
        }
    }
}
