using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoUserManage))]
    public class DaoUserManage : IDaoUserManage
    {
        public List<EntitySysUser> GetUserInfo(bool isPower)
        {
            try
            {
                string strUser = "";
                if (!isPower)
                {
                    strUser = " and Buser_type='护工组'";
                }
                String sql = string.Format(@"SELECT Base_user.*,Sys_user_key.user_loginid,
                case when Sys_user_key.user_loginid is null then '1' else  '0' end [poweruserkey]
                FROM Base_user
                left join Sys_user_key
                on Base_user.Buser_loginid=Sys_user_key.user_loginid
                 WHERE (Buser_id > '-1') {0} {1} ORDER BY Buser_name", strUser, "");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySysUser> list = EntityManager<EntitySysUser>.ConvertToList(dt).OrderBy(i => i.UserId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysUser>();
            }
        }

        public List<EntitySysRole> GetRoleInfo(bool isPower)
        {
            try
            {
                string strRole = "";
                if (!isPower)
                {
                    strRole = " where Brole_remark like '%护工组%'";
                }
                String sql = string.Format(@"select * from Base_role {0} order by Brole_name", strRole);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySysRole> list = EntityManager<EntitySysRole>.ConvertToList(dt).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysRole>();
            }
        }

        public List<EntityDicPubOrganize> GetTypeInfo()
        {
            try
            {
                String sql = string.Format(@"select Dorg_id type_id,Dorg_name type_name,'-1' parentId,'' itr_id,'' Itr_name,'0' sortId,Dorg_id tyep_hospitalName from Dict_organize 
union
select  Dpro_id+'^'+isnull(Dpro_Dorg_id,''),'实验组: '+ Dpro_name type_name,
Dpro_Dorg_id parentId,Dpro_id itr_id,'' Itr_name,'1' sortId ,Dpro_Dorg_id tyep_hospitalName
from Dict_profession where Dict_profession.del_flag=0 and Dpro_type = 1 
union 
select  Dict_profession.Dpro_id+'^'+isnull(Dpro_Dorg_id,'')+'^'+isnull(Ditr_id,'') ,
'仪器: ' + isnull(Ditr_ename,'') +' '+ isnull(Ditr_name,'')  type_name,
Dpro_id+'^'+isnull(Dpro_Dorg_id,'') parentId, Ditr_id as itr_id,isnull(Ditr_name,'') Itr_name,'2' sortId ,Dpro_Dorg_id tyep_hospitalName
from Dict_profession left join Dict_itr_instrument on Dict_profession.Dpro_id=Dict_itr_instrument.Ditr_lab_id 
where Dict_profession.del_flag=0 and Dpro_type = 1 and Dict_itr_instrument.del_flag=0 and Ditr_id is not null");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicPubOrganize> list = new List<EntityDicPubOrganize>();
                foreach (DataRow item in dt.Rows)
                {
                    EntityDicPubOrganize pubOrganize = new EntityDicPubOrganize();

                    pubOrganize.OrgId = item["type_id"].ToString();
                    pubOrganize.OrgName = item["type_name"].ToString();
                    pubOrganize.OrgCode = item["tyep_hospitalName"].ToString();
                    pubOrganize.ParentId = item["parentId"].ToString();
                    pubOrganize.ItrId = item["itr_id"].ToString();
                    pubOrganize.ItrName = item["Itr_name"].ToString();
                    pubOrganize.SortId = item["sortId"].ToString();

                    list.Add(pubOrganize);
                }
                return list.OrderBy(i => i.OrgSortNo).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubOrganize>();
            }
        }

        public List<EntityUserLab> GetUserTypeInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_lab where Bul_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserLab> list = EntityManager<EntityUserLab>.ConvertToList(dt).OrderBy(i => i.LabId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserLab>();
            }
        }

        public List<EntityUserInstrmt> GetUserItrInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_itr where Buitr_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserInstrmt> list = EntityManager<EntityUserInstrmt>.ConvertToList(dt).OrderBy(i => i.ItrId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserInstrmt>();
            }
        }

        public List<EntityUserHospital> GetUserHosInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_org where Buorg_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserHospital> list = EntityManager<EntityUserHospital>.ConvertToList(dt).OrderBy(i => i.OrgId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserHospital>();
            }
        }

        public List<EntityUserDept> GetUserDeptInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_dept where Bud_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserDept> list = EntityManager<EntityUserDept>.ConvertToList(dt).OrderBy(i => i.DeptId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserDept>();
            }
        }

        public List<EntityUserRole> GetUserRoleInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_role where Bur_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserRole> list = EntityManager<EntityUserRole>.ConvertToList(dt).OrderBy(i => i.RoleId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserRole>();
            }
        }

        public List<EntityUserLabQuality> GetUserLabQInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_qclab where Buqc_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserLabQuality> list = EntityManager<EntityUserLabQuality>.ConvertToList(dt).OrderBy(i => i.LabId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserLabQuality>();
            }
        }

        public List<EntityUserItrQuality> GetUserItrQInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_qcitr where Buqcitr_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserItrQuality> list = EntityManager<EntityUserItrQuality>.ConvertToList(dt).OrderBy(i => i.ItrId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserItrQuality>();
            }
        }

        public List<EntityUserHosQuality> GetUserHosQInfoByUserId(string userId)
        {
            try
            {
                String sql = string.Format(@"select * from Base_user_qcorg where Buorg_Buser_id = '{0}'", userId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserHosQuality> list = EntityManager<EntityUserHosQuality>.ConvertToList(dt).OrderBy(i => i.OrgId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserHosQuality>();
            }
        }

        public int[] DeleteUserRelate(string userInfoID)
        {
            ArrayList arr = new ArrayList();
            arr.Add(String.Format("delete from Base_user_lab where Bul_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_role where Bur_Buser_id={0}", userInfoID));
            //arr.Add(String.Format("delete from sys_power_user where user_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_itr where Buitr_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_org where Buorg_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qclab where Buqc_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qcitr where Buqcitr_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qcorg where Buorg_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_dept where Bud_Buser_id={0}", userInfoID));
            return this.DoTran(arr);
        }

        public int[] DeleteUserInfo(string userInfoID)
        {
            ArrayList arr = new ArrayList();
            arr.Add(String.Format("delete from Base_user_lab where Bul_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_role where Bur_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_User where Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_itr where Buitr_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_org where Buorg_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qclab where Buqc_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qcitr where Buqcitr_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_qcorg where Buorg_Buser_id={0}", userInfoID));
            arr.Add(String.Format("delete from Base_user_dept where Bud_Buser_id={0}", userInfoID));
            return this.DoTran(arr);
        }

        public int[] DoTran(ArrayList StrList)
        {
            string[] strSql = new string[StrList.Count];
            for (int i = 0; i < StrList.Count; i++)
            {
                strSql[i] = StrList[i].ToString();
            }
            return this.DoTran(strSql);
        }

        public int[] DoTran(string[] strSql)
        {
            SqlTransaction transaction = null;
            int[] num = new int[strSql.Length];
            try
            {
                try
                {
                    for (int i = 0; i < strSql.Length; i++)
                    {
                        DBManager helper = new DBManager();
                        DataTable dt = helper.ExecuteDtSql(strSql[i]);
                    }
                }
                catch (Exception exception)
                {
                    try
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        throw exception;
                    }
                    catch (Exception exception2)
                    {
                        if (transaction == null)
                        {
                            throw exception;
                        }
                        if (transaction.Connection != null)
                        {
                            num[0] = -2;
                            throw exception2;
                        }
                        num[0] = -1;
                        throw exception;
                    }
                }
            }
            finally
            {
            }
            return num;
        }

        public bool UpdateUserSign(byte[] userSign, string userId)
        {
            try
            {
                String sql = string.Format(@"update Base_user set Buser_image = {0} where Buser_id = '{1}'", userSign.ToString(), userId);

                DBManager helper = new DBManager();

                //DataTable dt = helper.ExecuteDtSql(sql);

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Buser_image", userSign);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Buser_id", userId);

                helper.UpdateOperation("Base_user", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertUserList(EntityPowerList userList)
        {
            try
            {
                String sql = string.Empty;
                List<EntityUserRole> userRole = userList.UserRole;
                if (userRole != null)
                {
                    foreach (var item in userRole)
                    {
                        sql += string.Format(@"Insert into Base_user_role (Bur_Buser_id,Bur_Brole_id ) values ('{0}','{1}'); ", item.UserId, item.RoleId);
                    }
                }
                if (userList.UserDept != null)
                {
                    foreach (var item in userList.UserDept)
                    {
                        sql += string.Format(@"Insert into Base_user_dept (Bud_Buser_id,Bud_Ddept_id ) values ('{0}','{1}'); ", item.UserId, item.DeptId);
                    }
                }
                if (userList.UserLab != null)
                {
                    foreach (var item in userList.UserLab)
                    {
                        sql += string.Format(@"Insert into Base_user_lab (Bul_Buser_id,Bul_lab_id ) values ('{0}','{1}'); ", item.UserId, item.LabId);
                    }
                }
                if (userList.UserItr != null)
                {
                    foreach (var item in userList.UserItr)
                    {
                        sql += string.Format(@"Insert into Base_user_itr (Buitr_Buser_id,Buitr_Ditr_id ) values ('{0}','{1}');", item.UserId, item.ItrId);
                    }
                }
                if (userList.UserHospital != null)
                {
                    foreach (var item in userList.UserHospital)
                    {
                        sql += string.Format(@"Insert into Base_user_org (Buorg_Buser_id,Buser_Dorg_id ) values ('{0}','{1}'); ", item.UserId, item.OrgId);
                    }
                }
                if (userList.UserLabQuality != null)
                {
                    foreach (var item in userList.UserLabQuality)
                    {
                        sql += string.Format(@"Insert into Base_user_qclab (Buqc_Buser_id,Buqc_lab_id ) values ('{0}','{1}'); ", item.UserId, item.LabId);
                    }
                }
                if (userList.UserItrQuality != null)
                {
                    foreach (var item in userList.UserItrQuality)
                    {
                        sql += string.Format(@"Insert into Base_user_qcitr (Buqcitr_Buser_id,Buqcitr_Ditr_id ) values ('{0}','{1}'); ", item.UserId, item.ItrId);
                    }
                }
                if (userList.UserHosQuality != null)
                {
                    foreach (var item in userList.UserHosQuality)
                    {
                        sql += string.Format(@"Insert into Base_user_qcorg (Buorg_Buser_id,Buser_Dorg_id ) values ('{0}','{1}'); ", item.UserId, item.OrgId);
                    }
                }
                DBManager helper = new DBManager();
                if (!string.IsNullOrEmpty(sql))
                {
                    int dt = helper.ExecCommand(sql);
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateUserList(List<EntitySysUser> userList)
        {
            try
            {
                DBManager helper = new DBManager();
                foreach (var userInfo in userList)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Buser_name", userInfo.UserName);
                    //values.Add("Buser_loginid", userInfo.UserLoginid);
                    values.Add("Buser_password", userInfo.UserPassword);
                    values.Add("Buser_Ditr_id", userInfo.ItrId);
                    values.Add("sort_no", userInfo.SortNo);
                    values.Add("py_code", userInfo.PyCode);
                    values.Add("wb_code", userInfo.WbCode);
                    values.Add("del_flag", userInfo.DelFlag);
                    values.Add("Buser_incode", userInfo.UserIncode);
                    values.Add("Buser_source_id", userInfo.UserSourceId);
                    values.Add("Buser_default_lab_id", userInfo.UserDefaultLabId);
                    values.Add("Buser_Ddept_id", userInfo.UserDepartId);
                    values.Add("Buser_type", userInfo.UserType);
                    if (userInfo.UserSigninamge != null)
                    {
                        values.Add("Buser_image", userInfo.UserSigninamge);
                    }
                    values.Add("Buser_image_file", userInfo.UserSignname);
                    values.Add("Buser_cerid", userInfo.UserCerid);
                    values.Add("Buser_ca_mode", userInfo.UserCaFlag);
                    values.Add("Buser_Dorg_id", userInfo.UserOrgId);
                    values.Add("Buser_identity", userInfo.Identity);
                    values.Add("Buser_caentity_id", userInfo.CaEntityId);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Buser_id", userInfo.UserId);

                    helper.UpdateOperation("Base_user", values, keys);
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public string AddUserList(List<EntitySysUser> userList)
        {
            try
            {
                DBManager helper = new DBManager();
                int id = IdentityHelper.GetMedIdentity("Base_user");
                foreach (var userInfo in userList)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Buser_id", id);
                    values.Add("Buser_name", userInfo.UserName);
                    values.Add("Buser_loginid", userInfo.UserLoginid);
                    values.Add("Buser_password", userInfo.UserPassword);
                    values.Add("Buser_Ditr_id", userInfo.ItrId);
                    values.Add("sort_no", userInfo.SortNo);
                    values.Add("py_code", userInfo.PyCode);
                    values.Add("wb_code", userInfo.WbCode);
                    values.Add("del_flag", userInfo.DelFlag);
                    values.Add("Buser_incode", userInfo.UserIncode);
                    values.Add("Buser_source_id", userInfo.UserSourceId);
                    values.Add("Buser_default_lab_id", userInfo.UserDefaultLabId);
                    values.Add("Buser_Ddept_id", userInfo.UserDepartId);
                    values.Add("Buser_type", userInfo.UserType);
                    if (userInfo.UserSigninamge != null)
                    {
                        values.Add("Buser_image", userInfo.UserSigninamge);
                    }
                    values.Add("Buser_image_file", userInfo.UserSignname);
                    values.Add("Buser_cerid", userInfo.UserCerid);
                    values.Add("Buser_ca_mode", userInfo.UserCaFlag);
                    values.Add("Buser_Dorg_id", userInfo.UserOrgId);

                    helper.InsertOperation("Base_user", values);
                }
                return id.ToString();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return "";
            }
        }

        public bool UpdateUserPassword(string loginId, string newPassword)
        {
            try
            {
                String sql = string.Format(@"update Base_user set Buser_password = '{0}' where Buser_loginid = '{1}'", newPassword, loginId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 停用用户账号
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool UpdateUserFlag(string loginId)
        {
            try
            {
                string sql = string.Format(@"update Base_user set del_flag = '1' where Buser_loginid = '{0}'", loginId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityUserDept> GetUserDeptInfoByLoginID(string loginId)
        {
            try
            {
                String sql = String.Format(@"select poweruserdepart.* from dbo.poweruserdepart
                        left join Base_user on Base_user.Buser_id=poweruserdepart.userinfoid
                        where (Base_user.Buser_loginid = '{0}')", loginId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserDept> list = EntityManager<EntityUserDept>.ConvertToList(dt).OrderBy(i => i.UserId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserDept>();
            }
        }

        public List<EntityUserKey> GetUserkey()
        {
            try
            {
                String sql = string.Format(@"select * from poweruserkey");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityUserKey> list = EntityManager<EntityUserKey>.ConvertToList(dt).OrderBy(i => i.UserId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityUserKey>();
            }
        }

        public List<EntityUserLab> GetLabIdByLoginId(string loginid)
        {
            List<EntityUserLab> list = new List<EntityUserLab>();
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

                    list = EntityManager<EntityUserLab>.ConvertToList(dt).ToList();


                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }

        public List<EntityUserInstrmt> GetUserInstrmtCache()
        {
            List<EntityUserInstrmt> list = new List<EntityUserInstrmt>();
            try
            {
                string sql = @"select 
                                Base_user_itr.Buitr_Buser_id,
                                 Base_user_itr.Buitr_Ditr_id,
                                Base_user.Buser_loginid,
                                Base_user.Buser_name
                                 from Base_user_itr
                                left join dbo.Base_user on Base_user_itr.Buitr_Buser_id=Base_user.Buser_id";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                list = EntityManager<EntityUserInstrmt>.ConvertToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool DelCerid(string CerId, string EntityId)
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = string.Format("UPDATE Base_user SET Buser_cerid = '',Buser_caentity_id='' WHERE Buser_cerid='{0}' or Buser_caentity_id = '{1}'", CerId, EntityId);
                helper.ExecuteDtSql(strSql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool SetCerid(string LoginId, string CerId, string EntityId)
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = string.Format("UPDATE Base_user SET Buser_cerid = '{0}',Buser_caentity_id='{2}' WHERE Buser_loginid='{1}'", CerId, LoginId, EntityId);
                helper.ExecuteDtSql(strSql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
