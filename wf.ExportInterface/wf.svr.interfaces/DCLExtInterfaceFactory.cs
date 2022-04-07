namespace dcl.svr.interfaces
{
    public static class DCLExtInterfaceFactory
    {
        /// <summary>
        /// 接口服务类
        /// </summary>
        public static DCLExtInterfaceBase DCLExtInterface
        {
            get
            {
                string strInterfaceMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");

                switch (strInterfaceMode)
                {
                    case "茂名妇幼保健院":
                        return new DCLExtInterface_MMFY();
                    default:
                        return new DCLExtInterface_Default();
                }
            }
        }
    }
}
