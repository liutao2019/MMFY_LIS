using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    public class CacheDirectDBDataInterface : AbstractDICache
    {
        #region singleton
        private static CacheDirectDBDataInterface _instance = null;
        private static object instancelock = new object();

        /// <summary>
        /// 单体实例
        /// </summary>
        public static CacheDirectDBDataInterface Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instancelock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheDirectDBDataInterface();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        protected override EnumDataAccessMode DataAccessMode
        {
            get { return EnumDataAccessMode.DirectToDB; }
        }
    }
}
