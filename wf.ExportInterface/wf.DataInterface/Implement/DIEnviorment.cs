using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    public class DIEnviorment
    {
        private static IDataInterfaceServiceContract _proxy = null;

        public static IDataInterfaceServiceContract Proxy
        {
            get
            {
                return _proxy;
            }
        }

        public static void RegistDACProxy(IDataInterfaceServiceContract proxy)
        {
            _proxy = proxy;
        }

        public static void InitCache(EnumDataAccessMode access_mode)
        {
            if (access_mode == EnumDataAccessMode.Custom)
                CacheDataDemandDataInterface.Current.RefreshAll();
            else if (access_mode == EnumDataAccessMode.DirectToDB)
                CacheDirectDBDataInterface.Current.RefreshAll();
        }
    }
}
