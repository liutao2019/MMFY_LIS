using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataInterface.Implement;
using Lib.DataInterface;
using dcl.client.wcf;
using System.Threading;

namespace dcl.client.cache
{
    public class CacheDataInterfaceConnection
    {
        private static CacheDataInterfaceConnection _instance = null;
        public static CacheDataInterfaceConnection Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheDataInterfaceConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        private CacheDataInterfaceConnection()
        {
            Refresh();
        }

        List<EntityDictDataInterfaceConnection> _cache = null;

        public List<EntityDictDataInterfaceConnection> GetAll()
        {
            return _cache;
        }

        public void Refresh()
        {
            ThreadRefresh();
        }

        private static object padlock = new object();
        private void ThreadRefresh()
        {
            lock (padlock)
            {
                ProxyCacheService proxy = new ProxyCacheService();
                this._cache = proxy.Service.GetAllDataInterfaceConnection();
            }
        }
    }
}
