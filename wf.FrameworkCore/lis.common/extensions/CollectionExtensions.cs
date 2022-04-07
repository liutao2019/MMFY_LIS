using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using System.Collections;

namespace dcl.common.extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 是否空或null
        /// </summary>
        public static bool IsEmpty(DataSet source)
        {
            return source == null || source.Tables.Count == 0 ||(source.Tables.Count == 1 && source.Tables[0].Rows.Count == 0);
        }


        /// <summary>
        /// 是否非空
        /// </summary>
        public static bool IsNotEmpty(DataTable source)
        {
            return !IsEmpty(source);
        }

        /// <summary>
        /// 是否空或null
        /// </summary>
        public static bool IsEmpty(string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 是否非空
        /// </summary>
        public static bool IsNotEmpty(string source)
        {
            return !IsEmpty(source);
        }

        /// <summary>
        /// 是否空或null
        /// </summary>
        public static bool IsEmpty(DataTable source)
        {
            return source == null || source.Rows.Count <= 0;
        }

        /// <summary>
        /// 是否非空
        /// </summary>
        public static bool IsNotEmpty(DataSet source)
        {
            return !IsEmpty(source);
        }

        /// <summary>
        /// 是否空或null
        /// </summary>
        public static bool IsEmpty(Array source)
        {
            return source == null || source.Length <= 0;
        }

        /// <summary>
        /// 是否非空
        /// </summary>
        public static bool IsNotEmpty(Array source)
        {
            return !IsEmpty(source);
        }

        /// <summary>
        /// 联合各元素,加上指定字符,得到string 返回值,如:List{"a","b","c"},指定字符为",",返回 "a,b,c"
        /// </summary>
        public static string JoinString(ICollection source, string value)
        {
            string result = "";
            foreach (var item in source)
            {
                result += value + item.ToString();

            }
            if (!string.IsNullOrEmpty(result))
                result = result.Substring(1);

            return result;
        }

        public static string JoinString2(ICollection source, string value)
        {
            string result = "";
            foreach (var item in source)
            {
                result += value + "'" + item.ToString() + "'";

            }
            if (!string.IsNullOrEmpty(result))
                result = result.Substring(1);

            return "(" + result + ")";
        }

      
        /// <summary>
        /// 在DataTable中查找关键词
        /// </summary>     
        /// <param name="column">列名</param>
        /// <param name="key">关键词</param>
        /// <returns>查找到的行编号</returns>
        public static int IndexOf(DataTable source, string column, string key)
        {
            if (Extensions.IsEmpty(source) || !source.Columns.Contains(column))
                return -1;

            int result = -1;
            for (int i = 0; i < source.Rows.Count; i++)
            {
                if (source.Rows[i][column].ToString() == key)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

       
    }

    //public static class CollectionExtensions
    //{
      
    //    /// <summary>
    //    /// 是否空或null
    //    /// </summary>
    //    public static bool IsEmpty(this DataSet source)
    //    {
    //        return source == null || source.Tables.Count <= 0;
    //    }

    //    /// <summary>
    //    /// 是否非空
    //    /// </summary>
    //    public static bool IsNotEmpty(this DataTable source)
    //    {
    //        return !IsEmpty(source);
    //    }

    //    /// <summary>
    //    /// 是否空或null
    //    /// </summary>
    //    public static bool IsEmpty(this DataTable source)
    //    {
    //        return source == null || source.Rows.Count <= 0;
    //    }

    //    /// <summary>
    //    /// 是否非空
    //    /// </summary>
    //    public static bool IsNotEmpty(this DataSet source)
    //    {
    //        return !IsEmpty(source);
    //    }

    //    /// <summary>
    //    /// 是否空或null
    //    /// </summary>
    //    public static bool IsEmpty(this Array source)
    //    {
    //        return source == null || source.Length <= 0;
    //    }

    //    /// <summary>
    //    /// 是否非空
    //    /// </summary>
    //    public static bool IsNotEmpty(this Array source)
    //    {
    //        return !IsEmpty(source);
    //    }

    //    /// <summary>
    //    /// 联合各元素,加上指定字符,得到string 返回值,如:List{"a","b","c"},指定字符为",",返回 "a,b,c"
    //    /// </summary>
    //    public static string JoinString(this ICollection source, string value)
    //    {
    //        string result = "";
    //        foreach (var item in source)
    //        {
    //            result += value + item.ToString();

    //        }
    //        if (!string.IsNullOrEmpty(result))
    //            result = result.Substring(1);

    //        return result;
    //    }

    //    public static string JoinString2(this ICollection source, string value)
    //    {
    //        string result = "";
    //        foreach (var item in source)
    //        {
    //            result += value + "'" + item.ToString() + "'";

    //        }
    //        if (!string.IsNullOrEmpty(result))
    //            result = result.Substring(1);

    //        return "("+result+")";
    //    }

    //    ///// <summary>
    //    ///// 是否空或null
    //    ///// </summary>
    //    //public static bool IsEmpty<T>(this ICollection<T> source)
    //    //{
    //    //    return source == null || source.Count <= 0;
    //    //}

    //    ///// <summary>
    //    ///// 是否非空
    //    ///// </summary>
    //    //public static bool IsNotEmpty<T>(this ICollection<T> source)
    //    //{
    //    //    return !IsEmpty(source);
    //    //}

    //    /// <summary>
    //    /// 在DataTable中查找关键词
    //    /// </summary>     
    //    /// <param name="column">列名</param>
    //    /// <param name="key">关键词</param>
    //    /// <returns>查找到的行编号</returns>
    //    public static int IndexOf(this DataTable source, string column, string key)
    //    {
    //        if (source.IsEmpty() || !source.Columns.Contains(column))
    //            return -1;

    //        int result = -1;
    //        for (int i = 0; i < source.Rows.Count; i++)
    //        {
    //            if (source.Rows[i][column].ToString() == key)
    //            {
    //                result = i;
    //                break;
    //            }
    //        }

    //        return result;
    //    }
    //}
}
