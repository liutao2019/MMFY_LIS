using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.common
{
    public class SQLFormater
    {
        /// <summary>
        /// 防止Filter或DataTable.SelectAll的SQL注入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Format(string input)
        {
            string temp = input.Trim();

            return temp.Replace("'", "''").Replace("[", "[[]").Replace("*", "[*]");//.Replace("]", "[]]");
        }

        /// <summary>
        /// 防止数据库SQL注入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatSQL(string input)
        {
            string temp = input.Trim();

            return temp.Replace("'", "''");
        }
    }
}
