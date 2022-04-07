using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheUserInstrmtInfo
    {
        private static CacheUserInstrmtInfo _instance = null;
        private static object padlock = new object();

        public static CacheUserInstrmtInfo Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheUserInstrmtInfo();
                        }
                    }
                }
                return _instance;
            }
        }

        public DataTable Cache { get; private set; }

        public List<EntityUserInstrmt> DclCache { get; private set; }

        #region .ctor

        private CacheUserInstrmtInfo()
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
//                    SqlHelper helper = new SqlHelper();
//                    string sqlSelect = @"select 
//PowerUserInstrmt.userInfoId,
// PowerUserInstrmt.itrId,
//poweruserinfo.loginId,
//poweruserinfo.userName
// from PowerUserInstrmt
//left join dbo.poweruserinfo on PowerUserInstrmt.userInfoId=poweruserinfo.userInfoId";
//                    Cache = helper.GetTable(sqlSelect);

                    CacheDataBIZ cacheBIZ = new CacheDataBIZ();
                    EntityResponse respone = cacheBIZ.GetCacheData("EntityUserInstrmt");
                    DclCache = respone.GetResult() as List<EntityUserInstrmt>;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public bool CheckUserCanMgrIInstrmt(string userInfoID, string itrID)
        {
            return Cache.Select(string.Format("userinfoid='{0}' and itrid='{1}'", userInfoID, itrID)).Length > 0;
        }
        public bool CheckUserCanMgrIInstrmt2(string loginID, string itrID)
        {

            return DclCache.Any(a => a.UserLoginid == loginID && a.ItrId == itrID);
            //.Select(string.Format("loginId='{0}' and itrid='{1}'", loginID, itrID)).Length > 0;
        }
        /// <summary>
        /// 获取这台仪器所有有权限的用户
        /// </summary>
        /// <param name="itrID"></param>
        /// <returns></returns>
        public DataSet GetUserCanMgrIInstrmt(string itrID)
        {
            DataRow[] drArr = Cache.Select(string.Format("itrid='{0}'", itrID));
            DataTable dtb = new DataTable();
            if (drArr.Length>0)
            {
                dtb = drArr[0].Table.Clone();
                foreach (DataRow dr in drArr)
                {
                    dtb.Rows.Add(dr.ItemArray);
                }
            }
            
            
            DataSet ds = new DataSet();
            ds.Tables.Add(dtb);
            return ds;
        }

        /// <summary>
        /// 获取这台仪器所有有权限的用户
        /// </summary>
        /// <param name="itrID"></param>
        /// <returns></returns>
        public List<EntityUserInstrmt> GetDclUserCanMgrIInstrmt(string itrID)
        {
            List<EntityUserInstrmt> drArr = DclCache.Where(w => w.ItrId == itrID).ToList();
            List<EntityUserInstrmt> dtb = new List<EntityUserInstrmt>();
            if (drArr.Count > 0)
            {
                dtb = EntityManager<EntityUserInstrmt>.ListClone(drArr);
                foreach (EntityUserInstrmt dr in drArr)
                {
                    dtb.Add(dr);
                }
            }

            return dtb;
        }

    }
}
