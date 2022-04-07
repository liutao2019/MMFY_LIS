using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheUserInfo
    {
        private static CacheUserInfo _instance = null;
        private static object padlock = new object();

        public static CacheUserInfo Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheUserInfo();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntitySysUser> Cache { get; private set; }

        #region .ctor

        private CacheUserInfo()
        {
            ThreadRefresh();
        }

        #endregion
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void Refresh()
        {
            Thread t = new Thread(ThreadRefresh);
            t.Start();
        }

        private void ThreadRefresh()
        {
            lock (padlock)
            {
                try
                {
                    EntityResponse response = new CacheDataBIZ().GetCacheData("EntitySysUser");
                    Cache = response.GetResult() as List<EntitySysUser>;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public string GetUserNameByLoginID(string loginid)
        {
            if (this.Cache == null)
            {
                return null;
            }
            var query = from item in this.Cache
                        where item.UserLoginid == loginid
                        select item.UserName;

            if (query.Count() > 0)
            {
                return query.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取与制定用户相同物理组的随人用户
        /// </summary>
        /// <param name="loginid"></param>
        /// <returns></returns>
        public string GetRandomUserCodeWithSamGroup(string loginid)
        {
            var query = from item in this.Cache
                        where item.UserLoginid == loginid
                        select item.UserDefaultLabId;

            string defaultType = "-1";
            if (query.Count() > 0)
            {
                defaultType = query.First();
            }

            if (defaultType == "-1")
            {
                return null;
            }

            var query2 = from item in this.Cache
                         where item.UserDefaultLabId == defaultType && item.UserLoginid != loginid && item.UserLoginid.ToLower() != "admin"
                         select item.UserLoginid;

            //List<EntityUserInfo> userInfoList = new List<EntityUserInfo>();
            //foreach (EntityUserInfo userInfo in this.Cache)
            //{
            //    if (userInfo.default_type == defaultType
            //        && userInfo.loginId != loginid
            //        && userInfo.loginId.ToLower() != "admin"
            //        )
            //    {
            //        userInfoList.Add(userInfo);
            //    }
            //}
            if (query2.Count() > 0)
            {
                List<string> listUsersCode = new List<string>(query2);

                return listUsersCode[new Random().Next(0, listUsersCode.Count)];
            }
            else
            {
                return null;
            }
        }

        public bool HasFunctionByLoginID(string loginID, string funcCode)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(loginID) && loginID.ToLower() == "admin")
                return true;

            var query1 = from user in this.Cache
                         join userrole in CacheUserRole.Current.Cache on user.UserId equals userrole.UserId
                         join rolefunc in CacheRoleFunc.Current.Cache on userrole.RoleId equals rolefunc.RoleId.ToString()
                         join func in CacheFunctionInfo.Current.Cache on rolefunc.FuncId equals func.FuncId
                         where user.UserLoginid == loginID && func.FuncCode == funcCode
                         select userrole;

            if (query1.Count() > 0)
            {
                ret = true;
            }

            return ret;
        }

        public bool HasFunctionByLoginID(string loginID, int funcId)
        {
            bool ret = false;

            if (!string.IsNullOrEmpty(loginID) && loginID.ToLower() == "admin")
                return true;

            var query1 = from user in this.Cache
                         join userrole in CacheUserRole.Current.Cache on user.UserId equals userrole.UserId
                         join rolefunc in CacheRoleFunc.Current.Cache on userrole.RoleId equals rolefunc.RoleId.ToString()
                         join func in CacheFunctionInfo.Current.Cache on rolefunc.FuncId equals func.FuncId
                         where user.UserLoginid == loginID && func.FuncId == funcId
                         select userrole;

            if (query1.Count() > 0)
            {
                ret = true;
            }

            return ret;
        }
    }
}
