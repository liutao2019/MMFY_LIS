using System;
using System.Collections.Generic;
using System.Text;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.cache
{
    public class CacheItemSam : AbstractClientCache<EntityDicItemSample>
    {
        private static CacheItemSam _instance = null;
        private static object padlock = new object();

        public static CacheItemSam Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheItemSam();
                        }
                    }
                }
                return _instance;
            }
        }
        private CacheItemSam()
        {
            Refresh();
        }

        public override void Refresh()
        {
            ProxyCacheService proxy = new ProxyCacheService();
            this._cache = proxy.Service.GetAllDictItemSam();
        }

        public override List<EntityDicItemSample> SelectAll()
        {
            return this._cache;
        }
    }
}
