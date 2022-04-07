using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysUser))]
    public class DaoSysUser : IDaoSysUser
    {
        public List<EntitySysUser> GetDepartByLoginId(string loginid)
        {
            List<EntitySysUser> list = new List<EntitySysUser>();

            if (!string.IsNullOrEmpty(loginid))
            {
                try
                {
                    string strSql = string.Format(@"SELECT Ddept_id depart_id,Ddept_code, Ddept_name dep_name,Ddept_Dsorc_id dep_ori_id,
Dsorc_name ori_name FROM Base_user JOIN Dict_dept 
on Base_user.Buser_Ddept_id = Dict_dept.Ddept_id JOIN Dict_source 
on Dict_dept.Ddept_Dsorc_id = Dict_source.Dsorc_id WHERE (Base_user.Buser_loginid = '{0}')", loginid);
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(strSql);

                    list = EntityManager<EntitySysUser>.ConvertToList(dt).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            return list;
        }

        public List<EntitySysUser> GetLoginId()
        {
            List<EntitySysUser> list = new List<EntitySysUser>();
            try
            {
                String sql = String.Format(@"select Buser_loginid from Base_user");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.UserLoginid).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public List<EntitySysUser> GetSourceIdByLoginId(string loginid)
        {
            List<EntitySysUser> list = new List<EntitySysUser>();
            if (!string.IsNullOrEmpty(loginid))
            {
                try
                {
                    string strSql = string.Format(@"SELECT Base_user_lab.Bul_lab_id
FROM Base_user INNER JOIN
Base_user_lab ON Base_user.Buser_id = Base_user_lab.Bul_Buser_id
WHERE (Base_user.Buser_loginid = '{0}')", loginid);
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(strSql);

                    list = EntityManager<EntitySysUser>.ConvertToList(dt).ToList();


                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }

        public List<EntitySysUser> GetUserInfoByLoginId(string loginid, string mark)
        {
            List<EntitySysUser> list = new List<EntitySysUser>();

            if (!string.IsNullOrEmpty(loginid))
            {
                try
                {
                    string strSql = string.Format(@" select
Base_user.*,
Dict_profession.Dpro_name,
Dict_itr_instrument.Ditr_ename as itr_mid
from Base_user
left join Dict_profession on Base_user.Buser_default_lab_id = Dict_profession.Dpro_id
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Base_user.Buser_Ditr_id 
where Base_user.Buser_loginid='{0}' and Base_user.del_flag <> '1'", loginid);
                    if (!string.IsNullOrEmpty(mark) && mark == "1")
                    {
                        strSql = String.Format(@"select
Base_user.*,
Dict_profession.Dpro_name as defaule_pro_name,
Dict_itr_instrument.Ditr_ename as itr_mid
from Base_user
left join Dict_profession on Base_user.Buser_default_lab_id = Dict_profession.Dpro_id
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Base_user.Buser_Ditr_id 
where (Base_user.Buser_cerid='{0}' or Base_user.Buser_loginid='{0}') and Base_user.del_flag <> '1'", loginid);
                    }
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(strSql);
                    list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.SortNo).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            return list;
        }

        public List<EntitySysUser> Search(object obj)
        {
            try
            {
                string strSql = "";
                if (obj != null)
                {
                    strSql = string.Format(@"select 0 as isselected,p.Buser_id as Buser_id,
p.Buser_name ,
p.Buser_loginid ,
p.Buser_type ,
p.wb_code ,
p.py_code ,
p.Buser_password,
p.Buser_identity,
p2.Bul_lab_id
from   Base_user p       
left join Base_user_lab p2 on  p2.Bul_Buser_id = p.Buser_id
where  p.del_flag = 0 and p.Buser_id<>'-1' {0}
                                ", BuildHospitalSqlWhere("p.Buser_Dorg_id"));
                    if (obj.ToString() != "-1" && obj.ToString() != "All")
                    {
                        strSql += " and p2.Bul_lab_id = '{0}'";
                        strSql = string.Format(strSql, obj.ToString());
                    }
                    strSql +=
                        " group by p.Buser_id,p.Buser_name,p.Buser_loginid,p.Buser_type,p.Buser_password,p.wb_code,p.py_code,p2.Bul_lab_id,p.sort_no,p.Buser_identity  ";
                }
                else
                {
                    strSql = string.Format("select Buser_id,Buser_name,Buser_loginid,py_code,wb_code,Buser_type,0 sp_select,Buser_identity,Buser_Dorg_id, Buser_incode from dbo.Base_user where 1=1 and del_flag=0 {0}", BuildHospitalSqlWhere("Buser_Dorg_id"));
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntitySysUser> list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.UserId).ThenBy(w => w.UserName).ThenBy(w => w.UserLoginid).ThenBy(w => w.UserType).ThenBy(w => w.SortNo).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysUser>();
            }
        }
        public static string BuildHospitalSqlWhere(string column)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }

        public List<EntitySysUser> GetPowerUserInfo()
        {
            DataTable dt = new DataTable();
            string sql = BuildTypeHospitalSqlWithAnd();
            string strSql = string.Format(@"select Dpro_id  userId,Dpro_id,Dpro_name,-1 user_id,'物理组: '+ Dpro_name user_name 
from Dict_profession where Dpro_type=1 and del_flag=0  union select Dpro_id+'^'+
Base_user.Buser_id  userId,Dict_profession.Dpro_id,Dict_profession.Dpro_name,
Base_user.Buser_id,Base_user.Buser_name from Base_user
join Base_user_lab on Base_user.Buser_id = Base_user_lab.Bul_Buser_id
join Dict_profession on Base_user_lab.Bul_lab_id = Dict_profession.Dpro_id 
where Dpro_type=1 and Dict_profession.del_flag=0", sql);
            try
            {
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(strSql);
                List<EntitySysUser> list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.ProName).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysUser>();
            }
        }

        public static string BuildTypeHospitalSqlWithAnd()
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return
                string.Format(
                    " and (Dict_profession.Dorg_id='{0}' or Dict_profession.Dorg_id='' or Dict_profession.Dorg_id is null)  ", hosID);
        }

        public List<EntitySysUser> SysUserQuery(EntityUserQc userQc)
        {
            List<EntitySysUser> listSysUser = new List<EntitySysUser>();
            if (userQc != null)
            {
                string strWhere = GetQueryWhere(userQc);
                try
                {
                    //老代码查询有distinct，去掉，不然会报错
                    string sqlStr = string.Format(@"select Base_user.Buser_id ,Base_user.Buser_id,Base_user.Buser_incode,
Base_user.Buser_loginid,Base_user.Buser_ca_mode,Base_user.Buser_cerid, base_user.Buser_caentity_id,Base_user.Buser_name,Base_function.Bfunc_name,
Base_function.Bfunc_code
from Base_user
left join Base_user_role on Base_user.Buser_id=Base_user_role.Bur_Buser_id
left join Base_role_function on Base_role_function.Brf_Brole_id=Base_user_role.Bur_Brole_id
left join Base_function on Base_function.Bfunc_id=Base_role_function.Brf_Bfunc_id 
where 1=1 {0} and Base_user.del_flag='0'", strWhere);
                    //追加秘钥查询语句
                    if (userQc.NotEqual && !string.IsNullOrEmpty(userQc.UserCerId))
                    {
                        sqlStr = string.Format(" select * from Base_user where Base_user.Buser_loginid<>'{0}' and Base_user.Buser_cerid='{1}'", userQc.LoginId, userQc.UserCerId);
                    }
                    DBManager helper = new DBManager();
                    DataTable dtSysUser = helper.ExecuteDtSql(sqlStr);

                    listSysUser = EntityManager<EntitySysUser>.ConvertToList(dtSysUser).OrderBy(i => i.UserId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("根据角色ID获取用户数据GetSysUserByRoleID", ex);
                }
            }
            return listSysUser;
        }
        public string GetQueryWhere(EntityUserQc userQc)
        {
            string strWhere = string.Empty;

            //如果是ca用户,并且采用:密码独立验证,则使用当前密码
            //系统配置：CA密码验证方式
            if (userQc.UserCaMode == "深圳沙井医院" && !string.IsNullOrEmpty(userQc.UserCerId))
            {
                strWhere += string.Format(" and 1=2 or Base_user.Buser_cerid='{0}'", userQc.UserCerId);
                return strWhere;
            }

            //登录ID
            if (!string.IsNullOrEmpty(userQc.LoginId))
            {
                string cerId = string.Empty;
                //验证ID
                if (!string.IsNullOrEmpty(userQc.UserCerId))
                {
                    cerId += string.Format(" or Base_user.Buser_cerid='{0}'", userQc.UserCerId);
                }
                strWhere += string.Format(" and (Base_user.Buser_loginid='{0}' {1})", userQc.LoginId, cerId);
            }

            //密码
            if (!string.IsNullOrEmpty(userQc.Password))
            {
                strWhere += string.Format(" and Base_user.Buser_password='{0}'", userQc.Password);
            }

            //角色ID
            if (!string.IsNullOrEmpty(userQc.RoleId))
            {
                strWhere += string.Format(" and Base_user_role.Bur_Brole_id='{0}'", userQc.RoleId);
            }

            //模块名称
            if (!string.IsNullOrEmpty(userQc.ModuleName))
            {
                strWhere += string.Format(" and Base_function.Bfunc_child_name='{0}'", userQc.ModuleName);
            }

            //模块ID
            if (!string.IsNullOrEmpty(userQc.FuncId))
            {
                strWhere += string.Format(" and Base_function.Bfunc_id='{0}'", userQc.FuncId);
            }

            //模块代码
            if (!string.IsNullOrEmpty(userQc.FuncCode))
            {
                strWhere += string.Format(" and Base_function.Bfunc_code='{0}'", userQc.FuncCode);
            }

            return strWhere;
        }



        #region IPowerUser 成员

        public ArrayList FindUser(string departmentId)
        {
            if (string.IsNullOrEmpty(departmentId))
                return new ArrayList();

            ArrayList result = new ArrayList();

            DBManager helper = new DBManager();
            DataTable dtResult = helper.ExecuteDtSql(String.Format("Select * from poweruserdepart where departId = '{0}'", departmentId));
            foreach (DataRow row in dtResult.Rows)
            {
                if (row["userInfoId"] != null && !string.IsNullOrEmpty(row["userInfoId"].ToString()))
                    result.Add(row["userInfoId"].ToString());
            }

            return result;
        }

        public ArrayList FindDepartments(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new ArrayList();

            ArrayList result = new ArrayList();
            DBManager helper = new DBManager();
            DataTable dtResult = helper.ExecuteDtSql(String.Format("Select * from poweruserdepart where userInfoId = '{0}'", userId));
            foreach (DataRow row in dtResult.Rows)
            {
                if (row["departId"] != null && !string.IsNullOrEmpty(row["departId"].ToString()))
                    result.Add(row["departId"].ToString());
            }

            return result;
        }

        public ArrayList FindDepartments_Code(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new ArrayList();

            ArrayList result = new ArrayList();
            DBManager helper = new DBManager();

            DataTable dtResult = helper.ExecuteDtSql(String.Format(@"Select 
poweruserdepart.userInfoId,
poweruserdepart.departId, 
dict_depart.dep_id,
dict_depart.dep_code
from poweruserdepart 
left join dict_depart on dep_id=departId where userInfoId = '{0}'", userId));
            foreach (DataRow row in dtResult.Rows)
            {
                if (row["dep_code"] != null && !string.IsNullOrEmpty(row["dep_code"].ToString()))
                    result.Add(row["dep_code"].ToString());
            }

            return result;
        }

        /// <summary>
        /// 获取指定角色名称的用户ID
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public List<string> GetUserIDForRoleName(string RoleName)
        {
            if (string.IsNullOrEmpty(RoleName))
                return new List<string>();

            List<string> result = new List<string>();
            DBManager helper = new DBManager();

            DataTable dtResult = helper.ExecuteDtSql(String.Format(@"select PowerUserRole.userInfoId, PowerUserRole.roleInfoId, 
                powerroleinfo.roleName
                from PowerUserRole
                inner join powerroleinfo on 
                powerroleinfo.roleInfoId=PowerUserRole.roleInfoId
                and powerroleinfo.roleName='{0}'", RoleName));
            foreach (DataRow row in dtResult.Rows)
            {
                if (row["userInfoId"] != null && !string.IsNullOrEmpty(row["userInfoId"].ToString()))
                    result.Add(row["userInfoId"].ToString());
            }

            return result;
        }


        public int AddUserinfoKey(string loginId, string keyCode, byte[] certinfo, string password)
        {
            DBManager helper = new DBManager();
            string sqlDel = string.Format("delete poweruserkey where userLoginId='{0}' ", loginId);
            int delReuslt =  helper.ExecCommand(sqlDel);

            string insertSql = @"insert into poweruserkey (userLoginId,userKey,user_certinfo,user_certpassword)
                                values (@userLoginId,@userKey,@user_certinfo,@user_certpassword)";
            List<DbParameter> list = new List<DbParameter>();
            list.Add(new SqlParameter("@userLoginId", loginId));
            list.Add(new SqlParameter("@userKey", keyCode));
            list.Add(new SqlParameter("@user_certinfo", certinfo));
            list.Add(new SqlParameter("@user_certpassword", password));
            int result = helper.ExecCommand(insertSql, list);
            return result;
        }

        public string Getuserpwinfo(string loginId)
        {
            string strRv = "";
            try
            {
                if (string.IsNullOrEmpty(loginId))
                {
                    throw new Exception("参数不能为空值");
                }

                DBManager helper = new DBManager();

                DataTable dtResult = helper.ExecuteDtSql(String.Format(@"select userInfoId,userName,loginId,password 
from poweruserinfo WITH (NOLOCK)
where loginId='{0}'", loginId));

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    strRv = dtResult.Rows[0]["password"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("Getuserpwinfo", ex);
            }
            return strRv;
        }

        #endregion
    }
}
