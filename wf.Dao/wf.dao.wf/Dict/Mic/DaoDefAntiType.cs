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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDefAntiType>))]
    public class DaoDefAntiType : IDaoDic<EntityDefAntiType>
    {
        public List<EntityDefAntiType> Search(Object obj)
        {

            try
            {
                string sql = @"SELECT distinct Rel_anti_type.*,Dict_mic_bacteria.Dbact_cname,Dict_mic_antibio.Dant_cname FROM Rel_anti_type
left join Dict_mic_bacteria on Rel_anti_type.Rat_Dbact_id = Dict_mic_bacteria.Dbact_id
left join Dict_mic_antibio on Rel_anti_type.Rat_Dant_id = Dict_mic_antibio.Dant_id
--WHERE del_flag = 0";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDefAntiType> list = EntityManager<EntityDefAntiType>.ToList(dt);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDefAntiType>();
            }
        }

        public bool Save(EntityDefAntiType antiType)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_anti_type");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rat_id", id);
                values.Add("Rat_Dant_id", antiType.DtAntiID);
                values.Add("Rat_Dbact_id", antiType.DtBtID);
                values.Add("Rat_flag", antiType.DtFlag);
                values.Add("del_flag", antiType.DelFlag);


                helper.InsertOperation("Rel_anti_type", values);

                antiType.DtID = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDefAntiType antiType)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rat_Dant_id", antiType.DtAntiID);
                values.Add("Rat_Dbact_id", antiType.DtBtID);
                values.Add("Rat_flag", antiType.DtFlag);
                values.Add("del_flag", antiType.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rat_id", antiType.DtID);

                helper.UpdateOperation("Rel_anti_type", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDefAntiType sample)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rat_id", sample.DtID);

                helper.UpdateOperation("Rel_anti_type", values, keys);
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
