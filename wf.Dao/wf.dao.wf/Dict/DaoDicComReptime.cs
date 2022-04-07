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

namespace dcl.dao.wf.Dict
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicComReptime>))]
    public class DaoDicComReptime : IDaoDic<EntityDicComReptime>
    {
        public bool Save(EntityDicComReptime reptime)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_reptime");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dret_id", id);
                values.Add("Dret_code", reptime.RetCode);
                values.Add("Dret_name", reptime.RetName);
                values.Add("Dstart_time", reptime.StartTime);
                values.Add("Dend_time", reptime.EndTime);
                values.Add("Dret_type", reptime.RetType);
                values.Add("Dret_day", reptime.RetDay);
                values.Add("Dret_time", reptime.RetTime);

                helper.InsertOperation("Dict_reptime", values);

                reptime.RetId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicComReptime reptime)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dret_code", reptime.RetCode);
                values.Add("Dret_name", reptime.RetName);
                values.Add("Dstart_time", reptime.StartTime);
                values.Add("Dend_time", reptime.EndTime);
                values.Add("Dret_type", reptime.RetType);
                values.Add("Dret_day", reptime.RetDay);
                values.Add("Dret_time", reptime.RetTime);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dret_id", reptime.RetId);

                helper.UpdateOperation("Dict_reptime", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicComReptime reptime)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"delete from Dict_reptime where Dret_id='{0}'"
                                           , reptime.RetId);
                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicComReptime> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT  Dret_id,Dret_code,Dret_name,Dstart_time,
                                       Dend_time, Dret_type, Dret_day, Dret_time
                                FROM Dict_reptime";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicComReptime>();
            }
        }

        public List<EntityDicComReptime> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicComReptime> list = EntityManager<EntityDicComReptime>.ConvertToList(dtSour);

            return list.ToList();
        }
    }
}
