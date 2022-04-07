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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampStoreArea>))]
    public class DaoDicSampStoreArea: IDaoDic<EntityDicSampStoreArea>
    {
        public bool Save(EntityDicSampStoreArea area)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_store_position");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpos_id", id);
                values.Add("Dpos_code", area.AreaCode);
                values.Add("Dpos_name", area.AreaName);
                values.Add("Dpos_capacity", area.AreaCapacity);
                values.Add("Dpos_status", area.AreaStatus);
                values.Add("Dpos_remark", area.AreaRemark);
                values.Add("Dpos_Dstore_id", area.StoreId);

                helper.InsertOperation("Dict_sample_store_position", values);

                area.StoreId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSampStoreArea area)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpos_code", area.AreaCode);
                values.Add("Dpos_name", area.AreaName);
                values.Add("Dpos_capacity", area.AreaCapacity);
                values.Add("Dpos_status", area.AreaStatus);
                values.Add("Dpos_remark", area.AreaRemark);
                values.Add("Dpos_Dstore_id", area.StoreId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpos_id", area.AreaId);

                helper.UpdateOperation("Dict_sample_store_position", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSampStoreArea area)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpos_id", area.AreaId);

                helper.DeleteOperation("Dict_sample_store_position", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampStoreArea> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Dict_sample_store_position";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicSampStoreArea>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampStoreArea>();
            }
        }
    }
}
