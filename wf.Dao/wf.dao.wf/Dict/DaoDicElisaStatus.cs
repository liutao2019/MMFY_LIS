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
    /// 酶标板孔位状态表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaStatus>))]
    public class DaoDicElisaStatus : IDaoDic<EntityDicElisaStatus>
    {
        public bool Save(EntityDicElisaStatus status)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_Elisa_plate_status");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Depsta_id", id);
                values.Add("Depsta_name", status.StaName);
                values.Add("Depsta_hole_staus", status.StaHoleStaus);
                values.Add("Depsta_Dplate_id", status.PlateId);
                values.Add("del_flag", status.DelFlag);

                helper.InsertOperation("Dict_Elisa_plate_status", values);

                status.StaId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaStatus status)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Depsta_name", status.StaName);
                values.Add("Depsta_hole_staus", status.StaHoleStaus);
                values.Add("Depsta_Dplate_id", status.PlateId);
                values.Add("del_flag", status.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Depsta_id", status.StaId);

                helper.UpdateOperation("Dict_Elisa_plate_status", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaStatus status)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag",1);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Depsta_id", status.StaId);

                helper.UpdateOperation("Dict_Elisa_plate_status", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaStatus> Search(Object obj)
        {
                try
                {
                 String sql = String.Format(@"SELECT * FROM Dict_Elisa_plate_status WITH(NOLOCK) where del_flag = '0'");
                DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    return ConvertToEntitys(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return new List<EntityDicElisaStatus>();
                }
        }

        public List<EntityDicElisaStatus> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicElisaStatus> list = EntityManager<EntityDicElisaStatus>.ConvertToList(dtSour);
            return list.OrderBy(i => i.StaId).ToList();
        }
    }
}
