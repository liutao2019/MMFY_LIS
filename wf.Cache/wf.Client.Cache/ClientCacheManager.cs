using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.client.cache
{
    public class ClientCacheManager
    {
        public static void RefreshAll()
        {
            CacheDataInterfaceConnection.Current.Refresh();
            CacheItemSam.Current.Refresh();
            CacheSysconfig.Current.Refresh();
        }
    }
}
