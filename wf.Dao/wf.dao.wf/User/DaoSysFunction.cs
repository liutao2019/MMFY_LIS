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
    [Export("wf.plugin.wf", typeof(IDaoSysFunction))]
    public class DaoSysFunction : IDaoSysFunction
    {
        public bool DeleteFunc(EntitySysFunction func)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Bfunc_id", func.FuncId);

                helper.DeleteOperation("Base_function", key);

                return true;
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
            
        }

        public bool InsertAFunc(EntitySysFunction func)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Base_function");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bfunc_id", id);
                values.Add("Bfunc_code", func.FuncCode);
                values.Add("Bfunc_name", func.FuncName);
                values.Add("Bfunc_type", func.FuncType);
                values.Add("Bfunc_parent_Bfunc_id", func.FuncParentId);
                values.Add("Bfunc_child_name", func.FuncChildName);
                values.Add("sort_no", func.FuncSortNo);
                values.Add("Bfunc_quick_flag", func.FuncQuickFlag);
                values.Add("Bfunc_image", func.FuncImage);
                values.Add("Bfunc_dictcache", func.FuncDictcache);
                values.Add("Bfunc_valiuser", func.FuncValiuser);
                values.Add("Bfunc_shortcut", func.FuncShortcut);
                values.Add("Bfunc_module", func.FuncModule);

                helper.InsertOperation("Base_function", values);

                func.FuncId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysFunction> GetFuncList(string whereSql="")
        {
            try
            {
                string where = string.Empty;
                if (whereSql == "1")
                {
                    where = "where Bfunc_type='窗体' or Bfunc_type = '功能' or Bfunc_type = '外部程序'  or Bfunc_type = '自定义'";
                }
                String sql = string.Format(@"SELECT * from Base_function {0}", where);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                if (whereSql == "1")
                {
                    return (from x in EntityManager<EntitySysFunction>.ConvertToList(dt)
                            orderby x.FuncParentId, x.FuncSortNo
                            select x).ToList();
                }

                return EntityManager<EntitySysFunction>.ConvertToList(dt).OrderBy(i => i.FuncSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysFunction>();
            }
        }

        public bool UpdateAFunc(EntitySysFunction func)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bfunc_code", func.FuncCode);
                values.Add("Bfunc_name", func.FuncName);
                values.Add("Bfunc_type", func.FuncType);
                values.Add("Bfunc_parent_Bfunc_id", func.FuncParentId);
                values.Add("Bfunc_child_name", func.FuncChildName);
                values.Add("sort_no", func.FuncSortNo);
                values.Add("Bfunc_quick_flag", func.FuncQuickFlag);
                values.Add("Bfunc_image", func.FuncImage);
                values.Add("Bfunc_dictcache", func.FuncDictcache);
                values.Add("Bfunc_valiuser", func.FuncValiuser);
                values.Add("Bfunc_shortcut", func.FuncShortcut);
                values.Add("Bfunc_module", func.FuncModule);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Bfunc_id", func.FuncId);

                helper.UpdateOperation("Base_function", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysFunction> GetFuncListByLogionId(string logionid, string whereSql)
        {
            try
            {
                string otherFunctype = "or Bfunc_type='自定义'";
                if (whereSql == "1")
                {
                    otherFunctype = " or  (Bfunc_parent_Bfunc_id =140 and Bfunc_type='自定义') ";
                }
                String sql = string.Format(@"SELECT distinct T.* from (SELECT  Base_function.*
FROM Base_user INNER JOIN
Base_user_role ON 
Base_user.Buser_id = Base_user_role.Bur_Buser_id INNER JOIN
Base_function INNER JOIN
Base_role_function ON Base_function.Bfunc_id = Base_role_function.Brf_Bfunc_id ON 
Base_user_role.Bur_Brole_id = Base_role_function.Brf_Brole_id 
WHERE (Base_user.Buser_loginid = '{0}')  Union all SELECT distinct * FROM Base_function where Bfunc_type='分隔符' {1}  ) T ", logionid, otherFunctype);

                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                return EntityManager<EntitySysFunction>.ConvertToList(dt).OrderBy(i => i.FuncSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysFunction>();
            }
        }

        public List<EntitySysFunction> GetFuncName()
        {
            List<EntitySysFunction> list = new List<EntitySysFunction>();
            try
            {
                String sql = String.Format(@"select distinct Bfunc_name from Base_function where Bfunc_type='窗体' or Bfunc_type='管理员窗体'");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntitySysFunction>.ConvertToList(dt).OrderBy(i => i.FuncName).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
