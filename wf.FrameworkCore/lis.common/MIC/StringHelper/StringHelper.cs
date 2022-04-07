using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.common
{  
    /// <summary>
    /// 字符串帮助类（含一些扩展方法)
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 将字符串按逗号分割成数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] ConvertbyComma(this string str) 
        {
            str= str.Replace("，", ",");
            if (str.EndsWith(","))
                str = str.Substring(0, str.Length - 1);
            return str.Split(new char[] { ',' });
        }

        public static char GetLastCharValue(this string str)
        {
            if (str.Length > 0)
            {
                char result = str.Substring(str.Length - 1, 1).ToCharArray()[0];
                return result;
            }
            return 'a';
        }
    }
}
