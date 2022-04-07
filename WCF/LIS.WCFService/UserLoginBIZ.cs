using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Configuration;

namespace dcl.svr.wcf
{
    public class UserLoginBIZ : ILogin
    {
        public static bool Register = true;

        public EntityLoginUserInfo CsLogin(EntityRequest request)
        {
            EntityLoginUserInfo userInfo = new EntityLoginUserInfo();
            EntityLogin login = request.GetRequestValue<EntityLogin>(); ;
            EntityResponse result = new EntityResponse();
            EntityLoginErrorInfo errorInfo = new EntityLoginErrorInfo();
            String loginid = login.LogInID.ToString();
            String password = login.PassWord;
            string ip = login.IP;
            string mac = login.Mac;
            string action = login.Action;

            if (Register == false)
            {
                errorInfo.LoginStatus = 0;
                LogLogin("登录", "用户登录", loginid, ip, mac, "登录失败，原因：产品注册已到期,请联系广州慧扬健康");
            }
            else
            {
                try
                {
                    LogLogin("登录", "用户登录", loginid, ip, mac, "尝试登录");
                    List<EntitySysUser> listUser = new List<EntitySysUser>();
                    IDaoSysUser daoUser = DclDaoFactory.DaoHandler<IDaoSysUser>();
                    if (daoUser != null)
                    {
                        listUser = daoUser.GetUserInfoByLoginId(loginid, "");

                        string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                        if (!string.IsNullOrEmpty(strHospitalId))
                        {
                            listUser = listUser.FindAll(w => w.UserOrgId == strHospitalId);
                        }
                        userInfo.UserInfo = listUser;
                    }
                    errorInfo.LoginStatus = 2;
                    if (listUser.Count > 0 && listUser[0].UserPassword.ToString().Equals(password))
                    {
                        List<EntitySysUser> listDepart = daoUser.GetDepartByLoginId(loginid);
                        userInfo.Depart = listDepart;

                        IDaoSysParameter daoPar = DclDaoFactory.DaoHandler<IDaoSysParameter>();
                        List<EntitySysParameter> listSysConfig = new List<EntitySysParameter>();
                        if (daoPar != null)
                        {
                            listSysConfig = daoPar.GetSysParaByConfigType();
                            userInfo.SysConfig = listSysConfig;
                            List<EntitySysParameter> listUserConfig = daoPar.GetSysParaByType("system");
                            userInfo.UserConfig = listUserConfig;
                        }
                        string whereSql = string.Empty;
                        List<EntitySysParameter> listConfigs = listSysConfig.Where(w => w.ParmCode == "Role_AllowSetCustomeRole").ToList();
                        if (listConfigs.Count > 0 && listConfigs[0].ParmFieldValue == "是")
                        {
                            whereSql = "1";
                        }
                        List<EntitySysFunction> listFunc = new List<EntitySysFunction>();
                        List<EntitySysFunction> listAllFunc = new List<EntitySysFunction>();
                        IDaoSysFunction daoFunc = DclDaoFactory.DaoHandler<IDaoSysFunction>();
                        if (daoFunc != null)
                        {
                            listFunc = daoFunc.GetFuncListByLogionId(loginid, whereSql);
                            listAllFunc = daoFunc.GetFuncList();
                        }
                        userInfo.Func = listFunc;
                        userInfo.AllFunc = listAllFunc;
                        IDaoUserManage daoManger = DclDaoFactory.DaoHandler<IDaoUserManage>();
                        List<EntityUserDept> listUserDepart = new List<EntityUserDept>();
                        if (daoManger != null)
                        {
                            listUserDepart = daoManger.GetUserDeptInfoByUserId(listUser[0].UserId);
                            userInfo.UserDepart = listUserDepart;
                            List<EntityUserInstrmt> listUserItrs = daoManger.GetUserItrInfoByUserId(listUser[0].UserId);
                            userInfo.UserItrs = listUserItrs;
                            List<EntityUserItrQuality> listUserItrsQc = daoManger.GetUserItrQInfoByUserId(listUser[0].UserId);
                            userInfo.UserItrsQc = listUserItrsQc;
                            List<EntityUserLabQuality> listUserTypeQc = daoManger.GetUserLabQInfoByUserId(listUser[0].UserId);
                            userInfo.UserQcLab = listUserTypeQc;
                            List<EntityUserKey> listUserkey = daoManger.GetUserkey();
                            userInfo.PowerUserKey = listUserkey;
                            List<EntityUserRole> listUserRole = daoManger.GetUserRoleInfoByUserId(listUser[0].UserId);
                            userInfo.PowerUserRole = listUserRole;
                            List<EntityUserLab> listLab = daoManger.GetLabIdByLoginId(loginid);
                            userInfo.UserLabInfo = listLab;
                        }
                        errorInfo.LoginStatus = 1;
                    }

                    //网证通
                    //在此添加一个判断，如果当前用需要数字签名，对用户的登录帐号和客户thumbPrint，与数据库中的thumbprint做对比.
                    if (listUser.Count > 0 && !listUser[0].UserPassword.Equals(password))
                    {
                        errorInfo.LoginStatus = 3;
                    }
                    if (errorInfo.LoginStatus == 2)
                    {
                        LogLogin("登录", "用户登录", loginid, ip, mac, "登录失败，原因：用户登录帐号不存在");
                    }
                    else if (errorInfo.LoginStatus == 3)
                    {
                        LogLogin("登录", "用户登录", loginid, ip, mac, "登录失败，原因：该登录帐号对应的密码错误");
                    }
                    // 开始中大肿瘤医院 网证通
                    // 是否需要CA
                    else if (
                        (!string.IsNullOrEmpty(login.CASignMode)
                        && String.Equals(login.CASignMode as string, "true", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        IDaoSysParameter daoPar = DclDaoFactory.DaoHandler<IDaoSysParameter>();
                        object obj = daoPar.GetSysParaByConfigCode("CASignatureMode")[0].ParmFieldValue;
                        if (obj != null && String.Equals(obj as string, "GDNETCA", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //客户端CA的姆印
                            string clientKey = "";
                            if (!string.IsNullOrEmpty(login.Thumbprint))
                            {
                                clientKey = login.Thumbprint as string;
                            }

                            if (String.IsNullOrEmpty(clientKey))
                            {
                                errorInfo.LoginStatus = 5;
                                LogLogin("登录", "用户登录", loginid, ip, mac, "登录失败，原因：该帐户登录时需要USBKEY！");
                            }
                            else
                            {
                                string serverKey = "";
                                if (listUser.Count > 0)
                                {
                                    serverKey = !string.IsNullOrEmpty(listUser[0].UserCerid) ?
                                            listUser[0].UserCerid as string : "";
                                }
                                if (!String.Equals(clientKey, serverKey, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    errorInfo.LoginStatus = 6;
                                    LogLogin("登录", "用户登录", loginid, ip, mac, "登录失败，原因：该登录帐号对应USBKEY错误！");
                                }
                            }
                        }

                    }
                    else
                    {
                        LogLogin("登录", "用户登录", loginid, ip, mac, "登录成功");
                    }
                }
                catch (Exception ex)
                {
                    errorInfo.LoginStatus = 7;
                    errorInfo.ErrorMsg = "获取信息出错";
                    errorInfo.ErrorDetail = ex.ToString();
                }
            }
            userInfo.ErrorInfo = errorInfo;
            return userInfo;
        }

        public EntityResponse ModeChange(EntityRequest request)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EntityLogin login = request.GetRequestValue<EntityLogin>(); ;
            EntityResponse result = new EntityResponse();
            String loginid = login.LogInID.ToString();

            try
            {
                List<EntitySysUser> listUser = new List<EntitySysUser>();
                IDaoSysUser daoUser = DclDaoFactory.DaoHandler<IDaoSysUser>();
                if (daoUser != null)
                {
                    listUser = daoUser.GetUserInfoByLoginId(loginid, "1");
                }

                if (listUser != null && listUser.Count > 0)
                {
                    login.LogInID = listUser[0].UserLoginid;
                    login.UserCaFlag = listUser[0].UserCaFlag;
                    login.CAEntityId = listUser[0].CaEntityId;
                }
                else
                {
                    //查无此人时用2标识
                    login.CASignMode = "2";
                }
            }
            catch (Exception Ex)
            {
            }
            result.SetResult(login.Clone());
            return result;
        }


        public void LogLogin(string type, string module, string loginID, string ip, string mac, string message)
        {
            IDaoSysLoginLog daoPar = DclDaoFactory.DaoHandler<IDaoSysLoginLog>();
            if (daoPar != null)
            {
                daoPar.LogLogin(type, module, loginID, ip, mac, message);
            }
        }

        /// <summary>
        /// 插入系统日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="module"></param>
        /// <param name="loginID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InsertSystemLog(string type, string module, string loginID, string ip, string mac, string message)
        {
            IDaoSysLoginLog daoPar = DclDaoFactory.DaoHandler<IDaoSysLoginLog>();
            if (daoPar != null)
            {
                daoPar.LogLogin(type, module, loginID, ip, mac, message);
            }
            return true;
        }
    }
}
