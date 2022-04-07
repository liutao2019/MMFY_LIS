
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.common;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicCombineTimeruleRelated>))]
    public class DaoDicCombineTimeruleRelated : IDaoDic<EntityDicCombineTimeruleRelated>
    {
        public bool Save(EntityDicCombineTimeruleRelated com)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtrr_Dcom_id", com.ComId);
                values.Add("Dtrr_Dtr_id", com.ComTimeId);
            
                helper.InsertOperation("Dict_itm_combine_timerule_related", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicCombineTimeruleRelated com)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtrr_Dcom_id", com.ComId);
                values.Add("Dtrr_Dtr_id", com.ComTimeId);
        
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtrr_Dcom_id", com.ComId);

                helper.UpdateOperation("Dict_itm_combine_timerule_related", values, keys);
              
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicCombineTimeruleRelated com)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtrr_Dcom_id", com.ComId);

                helper.DeleteOperation("Dict_itm_combine_timerule_related", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicCombineTimeruleRelated> Search(Object obj)
        {
            List<EntityDicCombineTimeruleRelated> list = new List<EntityDicCombineTimeruleRelated>();
            try
            {
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                {
                    String sql = string.Format(@"select Dtrr_Dtr_id from Dict_itm_combine_timerule_related where Dtrr_Dcom_id={0}", obj);

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityDicCombineTimeruleRelated>.ConvertToList(dt).OrderBy(i => i.ComId).ToList();
                }
                
            }
            catch (Exception ex) 
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicCombineTimeruleRelated> ConvertToEntitys(DataTable dtSour)
        {
            throw new NotImplementedException();
        }

    }
}
