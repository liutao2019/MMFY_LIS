using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using Lib.DataInterface.Implement;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.wcf
{
    public class CacheService : ICacheService
    {
        #region ICacheService 成员

        public List<EntityDictDataInterfaceConnection> GetAllDataInterfaceConnection()
        {
            List<EntityDictDataInterfaceConnection> list = CacheDataInterfaceConnection.Current.GetAll();
            return list;
        }

        #endregion

        #region ICacheService 成员


        public DateTime GetServerDateTime()
        {
            return ServerDateTime.GetDatabaseServerDateTime();
        }

        #endregion

        #region ICacheService 成员


        public List<EntityDicItemSample> GetAllDictItemSam()
        {
            return dcl.svr.cache.DictItemSamCache.Current.DclCache;
        }

        #endregion
    }
}
