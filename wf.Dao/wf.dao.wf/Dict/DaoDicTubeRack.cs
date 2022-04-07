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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicTubeRack>))]
    public class DaoDicTubeRack : IDaoDic<EntityDicTubeRack>
    {
        public bool Save(EntityDicTubeRack rack)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_tube_rack");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtrack_code", id);
                values.Add("Dtrack_name", rack.RackName);
                values.Add("Dtrack_type", rack.RackType);
                values.Add("Dtrack_x_amount", rack.RackXAmount);
                values.Add("Dtrack_y_amount", rack.RackYAmount);
                
                helper.InsertOperation("Dict_tube_rack", values);

                rack.RackCode = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicTubeRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtrack_name", rack.RackName);
                values.Add("Dtrack_type", rack.RackType);
                values.Add("Dtrack_x_amount", rack.RackXAmount);
                values.Add("Dtrack_y_amount", rack.RackYAmount);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtrack_code", rack.RackCode);

                helper.UpdateOperation("Dict_tube_rack", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicTubeRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                if(rack==null)
                {
                    return false;
                }
                string sql = string.Format(@"delete from Dict_tube_rack where Dtrack_code='{0}' ", rack.RackCode);

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicTubeRack> Search(Object obj)
        {
            try
            {
                EntityDicTubeRack rack = obj as EntityDicTubeRack;
                string sql = null;
                if (rack != null)
                {
                    sql = string.Format(@"select  * from Dict_tube_rack where Dtrack_code='{0}'", rack.RackCode);
                }
                else
                {
                    sql = @"select  * from Dict_tube_rack";
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicTubeRack>();
            }
        }

        public List<EntityDicTubeRack> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicTubeRack> list = EntityManager<EntityDicTubeRack>.ConvertToList(dtSour);
           
            return list.ToList();
        }
    }
}
