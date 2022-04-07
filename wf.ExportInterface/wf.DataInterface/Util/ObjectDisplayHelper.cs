using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    class ObjectDisplayHelper
    {
        public static string ToString(object obj)
        {
            string val = "";
            if (obj == DBNull.Value)
                val = "<DBNull>";
            else if (obj == null)
                val = "<Null>";
            else
            {
                if (obj.GetType() == typeof(string))
                {
                    val = "\"" + obj.ToString() + "\"";
                }
                else if (obj.GetType() == typeof(DateTime))
                {
                    val = "\"" + ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss") + "\"";
                }
                else
                {
                    val = obj.ToString();
                }
            }
            return val;
        }
    }
}
