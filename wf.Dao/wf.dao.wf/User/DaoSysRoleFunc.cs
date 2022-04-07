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
    [Export("wf.plugin.wf", typeof(IDaoSysRoleFunc))]
    public class DaoSysRoleFunc : IDaoSysRoleFunc
    {
        public bool DeleteFuncByRoleId(string roleId)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Brf_Brole_id", roleId);

                helper.DeleteOperation("Base_role_function", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertRoleFunc(List<EntitySysRoleFunction> roleFuncs)
        {
            try
            {
                DBManager helper = new DBManager();
                foreach (EntitySysRoleFunction item in roleFuncs)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Brf_Brole_id", item.RoleId);
                    values.Add("Brf_Bfunc_id", item.FuncId);

                    helper.InsertOperation("Base_role_function", values);
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysRoleFunction> GetFuncsByRoleId(string roleId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_role_function where 1=1");
                if (!string.IsNullOrEmpty(roleId))
                {
                    sql += string.Format(@" and Brf_Brole_id = '{0}'", roleId);
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysRoleFunction>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysRoleFunction>();
            }
        }
    }
}
