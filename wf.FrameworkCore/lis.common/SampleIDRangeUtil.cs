using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.common
{
    /// <summary>
    /// 样本范围工具
    /// </summary>
    public class SampleIDRangeUtil
    {
        /// <summary>
        /// 整数型数值区间输入转换成sql语句
        /// 例:输入"1-5,8,9,10-15",输出对应的sql where语句
        /// </summary>
        /// <param name="fieldName">要过滤的字段名，必须为整型</param>
        /// <param name="inupt">输入串</param>
        /// <returns></returns>
        public static string IntSampleIDToSQL(string fieldName, string inupt)
        {
            if (inupt == null || inupt.Trim(null) == string.Empty)
            {
                return string.Empty;
            }

            string output = string.Empty;

            string[] strs = inupt.Split(',');

            if (strs.Length > 0)
            {
                output = " 1=2  ";

                foreach (string str in strs)
                {
                    if (str.IndexOf('-')!=-1)
                    {
                        string[] str2 = str.Split('-');

                        if (str2.Length == 2)
                        {
                            int from = 0;
                            int to = 0;

                            if (int.TryParse(str2[0], out from) && int.TryParse(str2[1], out to))
                            {
                                output += string.Format(" or ({0} >={1} and {0} <={2}) ", fieldName, from, to);
                            }
                            else
                            {
                                output += " or (1=2) ";
                            }
                        }
                        else
                        {
                            output += string.Format(" and {0} < 0 ", fieldName);
                        }
                    }
                    else
                    {
                        string str2 = str.Trim(null);

                        int i = -1;

                        if (int.TryParse(str2, out i))
                        {
                            output += string.Format(" or {0} = {1} ", fieldName, i);
                        }
                        else
                        {
                            if (str2 != string.Empty)
                            {
                                output += "or 1=2 ";
                            }
                        }
                    }
                }
            }
            else
            {
                output = " 1=2 ";
            }
            return output;
        }


        /// <summary>
        /// 整数型数值区间输入转换成<c>List int</c>
        /// 例:输入"1-5,8,9,10-15",输入对应的sql where语句
        /// </summary>
        public static List<long> ToList(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<long>();

            List<long> results = new List<long>();

            string[] numbers = input.Split(',');
            foreach (string num in numbers)
            {
                int result;

                if (num.IndexOf('-') >= 0) //解析"-"号
                {
                    string[] range = num.Split('-');
                    if (range.Length > 0) //添加范围内数字
                    {
                        int from, to;
                        if (int.TryParse(range[0], out from) && int.TryParse(range[range.Length - 1], out to))
                        {
                            for (int i = from; i <= to; i++)
                                results.Add(i);
                        }
                        else
                            continue;
                    }
                }
                else if (int.TryParse(num, out result))
                    results.Add(result);
            }

            return results;
        }


        public static List<int> ToList(char split,string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<int>();

            List<int> results = new List<int>();

            string[] numbers = input.Split(split);
            foreach (string num in numbers)
            {
                int result;

                if (num.IndexOf('-') >= 0) //解析"-"号
                {
                    string[] range = num.Split('-');
                    if (range.Length > 0) //添加范围内数字
                    {
                        int from, to;
                        if (int.TryParse(range[0], out from) && int.TryParse(range[range.Length - 1], out to))
                        {
                            for (int i = from; i <= to; i++)
                                results.Add(i);
                        }
                        else
                            continue;
                    }
                }
                else if (int.TryParse(num, out result))
                    results.Add(result);
            }

            return results;
        }
    }
}
