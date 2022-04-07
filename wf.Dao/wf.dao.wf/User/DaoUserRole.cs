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
    [Export("wf.plugin.wf", typeof(IDaoUserRole))]
    public class DaoUserRole : IDaoUserRole
    {
        public bool DeleteUserByRoleId(string roleId)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Bur_Brole_id", roleId);

                helper.DeleteOperation("Base_user_role", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertUserRole(List<EntityUserRole> userRoles)
        {
            try
            {
                DBManager helper = new DBManager();
                foreach (EntityUserRole item in userRoles)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Bur_Buser_id", item.UserId);
                    values.Add("Bur_Brole_id", item.RoleId);

                    helper.InsertOperation("Base_user_role", values);
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityUserRole> GetUsersByRoleId(string roleId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_role where Bur_Brole_id = '{0}' and Bur_Buser_id in (select Buser_id from Base_User)", roleId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityUserRole>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserRole>();
            }
        }

        public List<EntityUserRole> GetAllUserRole()
        {
            List<EntityUserRole> listUserRole = new List<EntityUserRole>();
            string sql = @"select Base_user.Buser_id,Buser_loginid,Base_role.Brole_remark,Base_user_role.Bur_Brole_id from Base_user 
left join Base_user_role on Base_user.Buser_id=Base_user_role.Bur_Buser_id
left join Base_role on Base_user_role.Bur_Brole_id=Base_role.Brole_id";
            DBManager helper = new DBManager();
            DataTable dt = helper.ExecuteDtSql(sql);
            if (dt != null)
            {
                listUserRole = EntityManager<EntityUserRole>.ConvertToList(dt);
            }
            return listUserRole;
        }
    }
}
