using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoTempHandle))]
    public class DaoTempHandle : IDaoTempHandle
    {
        public List<EntityTemHarvester> GetHarvesterByDhId(string dhId)
        {
            List<EntityTemHarvester> listHar = new List<EntityTemHarvester>();
            if (!string.IsNullOrEmpty(dhId))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"select dict_harvester.dh_name,tem_harvester.*,dict_temperature.dt_l_limit,dict_temperature.dt_h_limit
from tem_harvester
left join dict_harvester on tem_harvester.th_h_id=dict_harvester.dh_id
left join Dict_sample_store on Dict_sample_store.Dstore_dh_id=dict_harvester.dh_id
left join dict_temperature on Dict_sample_store.Dstore_rg_id=dict_temperature.dt_id
where tem_harvester.th_h_id='{0}'", dhId);

                DataTable dt = helper.ExecuteDtSql(sql);
                listHar = EntityManager<EntityTemHarvester>.ConvertToList(dt);

            }
            return listHar;

        }

        public List<EntityTemHarvester> GetTempHarvesterByProId(string proId, DateTime? datetime = null)
        {
            List<EntityTemHarvester> listTemHar = new List<EntityTemHarvester>();

            if (!string.IsNullOrEmpty(proId))
            {
                try
                {
                    DBManager helper = new DBManager();

                    DateTime dateTime;
                    if (datetime != null)
                    {
                        dateTime = datetime.Value;
                    }
                    else
                    {
                        dateTime = DateTime.Now.Date;
                    }
                    string sql = string.Format(@"select h.*,dict_harvester.dh_name,Dict_sample_store.Dstore_name,
dict_temperature.dt_h_limit,dict_temperature.dt_l_limit,Dict_profession.Dpro_name
from tem_harvester h
left join dict_harvester on h.th_h_id = dict_harvester.dh_id
left join Dict_sample_store on Dict_sample_store.Dstore_dh_id = dict_harvester.dh_id
left join Dict_profession on Dict_sample_store.Dstore_lab_id = Dict_profession.Dpro_id
left join dict_temperature on Dict_sample_store.Dstore_rg_id =  dict_temperature.dt_id
where Dict_profession.Dpro_id = '{0}' and th_date = (select max(th_date) from tem_harvester 
where th_h_id= h.th_h_id) and th_date>= '{1}'", proId, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    DataTable dt = helper.ExecuteDtSql(sql);

                    listTemHar = EntityManager<EntityTemHarvester>.ConvertToList(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            return listTemHar;
        }
    }
}
