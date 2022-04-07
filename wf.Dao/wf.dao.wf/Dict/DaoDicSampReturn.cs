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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampReturn>))]
    public class DaoDicSampReturn : IDaoDic<EntityDicSampReturn>
    {
        public bool Save(EntityDicSampReturn message)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_return");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dreturn_id", id);
                values.Add("Dreturn_content", message.ReturnContent);
                values.Add("Dreturn_Buser_id", message.ReturnUserId);

                helper.InsertOperation("Dict_sample_return", values);

                message.ReturnId = Convert.ToInt32(id.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSampReturn message)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dreturn_content", message.ReturnContent);
                values.Add("Dreturn_Buser_id", message.ReturnUserId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dreturn_id", message.ReturnId);

                helper.UpdateOperation("Dict_sample_return", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSampReturn message)
        {
            try
            {
                DBManager helper = new DBManager();

                //Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("del_flag", "1");

                //Dictionary<string, object> keys = new Dictionary<string, object>();
                //keys.Add("sam_id", message.SamId);

                //helper.UpdateOperation("dic_samp_return", values, keys);

                string sql = string.Format(@"delete from Dict_sample_return where Dreturn_id={0} ", message.ReturnId);

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampReturn> Search(Object obj)
        {
            try
            {
                EntityDicSampReturn info = obj as EntityDicSampReturn;
                String sql = null;
                if (info != null)
                {
                    sql = string.Format(@"select * from Dict_sample_return  where Dreturn_id={0}" , info.ReturnId);
                }
                else
                {
                    sql = string.Format(@"select * from Dict_sample_return ");
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampReturn>();
            }
        }

        public List<EntityDicSampReturn> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicSampReturn> list = EntityManager<EntityDicSampReturn>.ConvertToList(dtSour);
            
            return list.ToList();
        }
    }
}
