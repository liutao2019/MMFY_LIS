using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResultDesc))]
    public class DaoObrResultDesc : DclDaoBase, IDaoObrResultDesc
    {
        public bool DeleteResultById(string obrId)
        {
            DBManager helper = GetDbManager();
            try
            {
                string sql = String.Format("delete from Lis_result_desc where Lrd_id='{0}'", obrId);
                helper.ExecCommand(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityObrResultDesc> GetDescResultById(string obrId = "", string repFlag = "1")
        {
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Empty;
                switch (repFlag)
                {
                    case "1":
                        sql = string.Format("select  *,cast(0 as bit) selected from Lis_result_desc where Lrd_id='{0}'", obrId);
                        break;
                    case "3":
                        sql = string.Format("select cast(0 as bit) selected,Lrd_value,Lrd_positive_flag from Lis_result_desc where Lrd_id='{0}'", obrId);
                        break;
                    case "4":
                        sql = string.Format("select cast(0 as bit) selected,Lrd_describe as 'obr_value',Lrd_positive_flag from Lis_result_desc where Lrd_id='{0}'", obrId);
                        break;
                }

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityObrResultDesc>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityObrResultDesc>();
            }
        }

        public List<EntityObrResultDesc> GetObrResultDescById(string obrId)
        {
            List<EntityObrResultDesc> list = new List<EntityObrResultDesc>();
            try
            {
                string sql = string.Format(@"select 
Lis_result_desc.* ,
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_sub_title
from Lis_result_desc with(nolock)
left join Dict_itr_instrument on Lis_result_desc.Lrd_Ditr_id = Dict_itr_instrument.Ditr_id
 where Lrd_id = '{0}'", obrId);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityObrResultDesc>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool InsertObrResultDesc(EntityObrResultDesc ObrResultDesc)
        {
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lrd_id", ObrResultDesc.ObrId);
                values.Add("Lrd_Ditr_id", ObrResultDesc.ObrItrId);
                values.Add("Lrd_date", ObrResultDesc.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lrd_sid", ObrResultDesc.ObrSid);
                values.Add("Lrd_value", ObrResultDesc.ObrValue);
                values.Add("Lrd_checkobj", ObrResultDesc.ObrCheckObj);
                values.Add("Lrd_describe", ObrResultDesc.ObrDescribe);
                values.Add("Lrd_flag", ObrResultDesc.ObrFlag);
                values.Add("sort_no", ObrResultDesc.SortNo);
                values.Add("Lrd_positive_flag", ObrResultDesc.ObrPositiveFlag);

                helper.InsertOperation("Lis_result_desc", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrResultDesc(EntityObrResultDesc ObrResultDesc)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lrd_Ditr_id", ObrResultDesc.ObrItrId);
                values.Add("Lrd_date", ObrResultDesc.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lrd_sid", ObrResultDesc.ObrSid);
                values.Add("Lrd_value", ObrResultDesc.ObrValue);
                values.Add("Lrd_describe", ObrResultDesc.ObrDescribe);
                values.Add("Lrd_flag", ObrResultDesc.ObrFlag);
                values.Add("sort_no", ObrResultDesc.SortNo);
                values.Add("Lrd_positive_flag", ObrResultDesc.ObrPositiveFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lrd_id", ObrResultDesc.ObrId);

                helper.UpdateOperation("Lis_result_desc", values, keys);

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
