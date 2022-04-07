using dcl.dao.interfaces;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaReturn>))]
    public class DaoDicReaReturn : IDaoDic<EntityDicReaReturn>
    {
        public bool Save(EntityDicReaReturn message)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_return");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dreturn_id", id);
                values.Add("Dreturn_content", message.ReturnContent);
                values.Add("Dreturn_Buser_id", message.ReturnUserId);

                helper.InsertOperation("Dict_rea_return", values);

                message.ReturnId = Convert.ToInt32(id.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicReaReturn message)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dreturn_content", message.ReturnContent);
                values.Add("Dreturn_Buser_id", message.ReturnUserId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dreturn_id", message.ReturnId);

                helper.UpdateOperation("Dict_rea_return", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicReaReturn message)
        {
            try
            {
                DBManager helper = new DBManager();

                //Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("del_flag", "1");

                //Dictionary<string, object> keys = new Dictionary<string, object>();
                //keys.Add("sam_id", message.SamId);

                //helper.UpdateOperation("dic_samp_return", values, keys);

                string sql = string.Format(@"delete from Dict_rea_return where Dreturn_id={0} ", message.ReturnId);

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaReturn> Search(Object obj)
        {
            try
            {
                EntityDicReaReturn info = obj as EntityDicReaReturn;
                String sql = null;
                if (info != null)
                {
                    sql = string.Format(@"select * from Dict_rea_return  where Dreturn_id={0}" , info.ReturnId);
                }
                else
                {
                    sql = string.Format(@"select * from Dict_rea_return ");
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaReturn>();
            }
        }

        public List<EntityDicReaReturn> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaReturn> list = EntityManager<EntityDicReaReturn>.ConvertToList(dtSour);
            
            return list.ToList();
        }
    }
}
