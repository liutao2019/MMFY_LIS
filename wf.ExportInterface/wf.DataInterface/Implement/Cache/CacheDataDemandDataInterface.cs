using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    public class CacheDataDemandDataInterface : AbstractDICache
    {
        #region singleton
        private static CacheDataDemandDataInterface _instance = null;
        private static object instancelock = new object();

        /// <summary>
        /// 单体实例
        /// </summary>
        public static CacheDataDemandDataInterface Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instancelock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheDataDemandDataInterface();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        protected override EnumDataAccessMode DataAccessMode
        {
            get { return EnumDataAccessMode.Custom; }
        }
    }
}
