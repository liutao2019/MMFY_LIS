using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System.Data;
using System.Configuration;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysRole))]
    public class DaoSysRole : IDaoSysRole
    {
        public bool DeleteRoleInfo(EntitySysRole role)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Brole_id", role.RoleId);

                helper.DeleteOperation("Base_role", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertRoleInfo(EntitySysRole role)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Base_role");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Brole_id", id);
                values.Add("Brole_name", role.RoleName);
                values.Add("Brole_remark", role.RoleRemark);

                helper.InsertOperation("Base_role", values);

                role.RoleId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateRoleInfo(EntitySysRole role)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Brole_id", role.RoleId);

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Brole_name", role.RoleName);
                values.Add("Brole_remark", role.RoleRemark);

                helper.UpdateOperation("Base_role", values, key);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysRole> GetAllInfo()
        {
            try
            {
                String sql = @"select * from Base_role";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysRole>.ConvertToList(dt).OrderBy(i => i.RoleName).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysRole>();
            }
        }

        public List<EntitySysRole> GetPowerRoleUser()
        {
            DataTable dt = new DataTable();
            string sql = BuildHospitalSqlWhere("Base_user.Buser_Dorg_id");
            string strSql = String.Format(@"select cast(Brole_id as varchar)  userId, cast(Brole_id as varchar) Brole_id,
Brole_name,-1 user_id,'角色: '+ Brole_name user_name from Base_role where 1=1  
union select cast(Base_role.Brole_id as varchar) +'^'+ Base_user.Buser_Id userId,
cast(Base_role.Brole_id as varchar) role_id,Brole_name role_name,Base_user.Buser_Id,
Buser_name userName from Base_role join Base_user_role on Base_role.Brole_id =Base_user_role.Bur_Brole_id join Base_user 
on Base_user_role.Bur_Buser_id =Base_user.Buser_Id where 1=1 {0} ", sql);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                List<EntitySysRole> list = EntityManager<EntitySysRole>.ConvertToList(dt).OrderBy(i => i.UserId).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysRole>();
            }
        }

        public static string BuildHospitalSqlWhere(string column)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }
    }
}
