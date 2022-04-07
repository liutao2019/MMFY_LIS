using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.common
{
    public class Compare
    {
        public static bool IsNullOrDBNull(object obj)
        {
            bool b = false;
            if (obj == null || obj == DBNull.Value)
            {
                b = true;
            }
            return b;
        }

        /// <summary>
        /// DBNull或Null或""时为真
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty(object obj)
        {
            return IsNullOrDBNull(obj) || obj.ToString() == "";
        }
    }
}
