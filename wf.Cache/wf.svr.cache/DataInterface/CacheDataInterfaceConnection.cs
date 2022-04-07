using System.Collections.Generic;
using Lib.DataInterface.Implement;

namespace dcl.svr.cache
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
                    _instance = new CacheDataInterfaceConnection();
                }
                return _instance;
            }
        }

        CacheDataInterfaceConnection()
        {
            Refresh();
        }

        public EntityDictDataInterfaceConnection GetConnectionByCode(string conn_code)
        {
            EntityDictDataInterfaceConnection conn = CacheDirectDBDataInterface.Current.GetConnectionByCode(conn_code);
            return conn;
        }

        public List<EntityDictDataInterfaceConnection> GetAll()
        {
            return CacheDirectDBDataInterface.Current.GetConnections(null);
        }

        public void Refresh()
        {
            CacheDirectDBDataInterface.Current.RefreshCommand();
            CacheDirectDBDataInterface.Current.RefreshConnection();
            CacheDirectDBDataInterface.Current.RefreshConverter();
            CacheDirectDBDataInterface.Current.RefreshParameter();
        }
    }
}
