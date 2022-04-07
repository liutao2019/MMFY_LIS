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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampStore>))]
    public class DaoDicSampStore : IDaoDic<EntityDicSampStore>
    {
        public bool Save(EntityDicSampStore store)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_store");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dstore_id", id);
                values.Add("Dstore_code", store.StoreCode);
                values.Add("Dstore_name", store.StoreName);
                values.Add("Dstore_lab_id", store.StoreLabId);
                values.Add("Dstore_amount", store.StoreAmount);
                values.Add("Dstore_Dstau_id", store.StoreStatus);
                values.Add("py_code", store.StorePyCode);
                values.Add("wb_code", store.StoreWbCode);
                values.Add("Dstore_rg_id", store.StoreRgId);
                values.Add("Dstore_dh_id", store.StoreDhId);

                helper.InsertOperation("Dict_sample_store", values);

                store.StoreId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSampStore store)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dstore_code", store.StoreCode);
                values.Add("Dstore_name", store.StoreName);
                values.Add("Dstore_lab_id", store.StoreLabId);
                values.Add("Dstore_amount", store.StoreAmount);
                values.Add("Dstore_Dstau_id", store.StoreStatus);
                values.Add("py_code", store.StorePyCode);
                values.Add("wb_code", store.StoreWbCode);
                values.Add("Dstore_rg_id", store.StoreRgId);
                values.Add("Dstore_dh_id", store.StoreDhId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dstore_id", store.StoreId);

                helper.UpdateOperation("Dict_sample_store", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSampStore store)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dstore_id", store.StoreId);

                helper.DeleteOperation("Dict_sample_store", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampStore> Search(Object obj)
        {
            try
            {
                String sql = @"select Dict_sample_store.*,Dict_profession.Dpro_name,Dict_samp_store_status.Dstau_name,dict_temperature.dt_name,dict_harvester.dh_name 
from Dict_sample_store 
left join Dict_profession on Dict_profession.Dpro_id = Dict_sample_store.Dstore_lab_id
left join Dict_samp_store_status on Dict_samp_store_status.Dstau_id = Dict_sample_store.Dstore_Dstau_id
left join dict_harvester on dict_harvester.dh_id=Dict_sample_store.Dstore_dh_id
left join dict_temperature on dict_temperature.dt_id=Dict_sample_store.Dstore_rg_id";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicSampStore>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampStore>();
            }
        }
    }
}
