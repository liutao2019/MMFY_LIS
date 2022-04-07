using System;
using System.Data;

namespace dcl.common
{
    public class ValueConvertHelper
    {

        /// <summary>  
        /// 将字符型类型转换为整型值  
        /// </summary>
        /// <param name="objValue">字符型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>整型值</returns>  
        public static int IntParse(string objValue, int defaultValue)
        {
            int returnValue = defaultValue;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    returnValue = int.Parse(objValue);
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }

        /// <summary>  
        /// 将对象类型转换为整型值  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>整型值</returns>  
        public static int IntParse(object objValue, int defaultValue)
        {
            int returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = int.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值  
            return returnValue;
        }

        /// <summary>  
        /// 将对象类型转换为整型值  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <returns>整型值</returns>  
        public static int IntParse(object objValue)
        {
            return IntParse(objValue, 0);
        }
        /// <summary>  
        /// 将对象类型转换为日期值  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>日期值</returns>  
        public static DateTime DateTimeParse(object objValue, DateTime defaultValue)
        {
            DateTime returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = DateTime.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值  
            return returnValue;
        }


        /// <summary>  
        /// 将对象类型转换为字符型  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>字符型</returns>  
        public static string StringParse(object objValue, string defaultValue)
        {
            string returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = objValue.ToString();
                }
                catch
                {
                    returnValue = defaultValue; ;
                }

            }

            //返回值  
            return returnValue;
        }

        /// <summary>  
        /// 将对象类型转换为字符型  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <returns>字符型</returns>  
        public static string StringParse(object objValue)
        {
            return StringParse(objValue, string.Empty);
        }


        /// <summary>  
        /// 将对象类型转换为GUID  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>GUID</returns>  
        public static Guid GuidParse(object objValue, Guid defaultValue)
        {
            Guid returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = new Guid(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue; ;
                }

            }

            //返回值  
            return returnValue;
        }


        /// <summary>  
        /// 将对象类型转换为GUID  
        /// </summary>  
        /// <param name="objValue">对象类型</param>  
        /// <returns>GUID</returns>  
        public static Guid GuidParse(object objValue)
        {
            return GuidParse(objValue, Guid.Empty);
        }

        /// <summary>
        /// 字符串转换为计算公式（自定义公式的计算）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ComputeValue(string value)
        {
            try
            {
                DataTable dt = new DataTable();
                string result = dt.Compute(value, string.Empty).ToString();
                return result;
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>  
        /// C#类型转换函数  
        /// </summary>  
        /// <typeparam name="T">目标类型值</typeparam>  
        /// <param name="objValue">对象类型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>目标类型值</returns>  
        public static T Parse<T>(object objValue, T defaultValue)
        {
            T returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = (T)objValue;
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值  
            return returnValue;
        }

        /// <summary>  
        /// C#类型转换函数 
        /// </summary>  
        /// <typeparam name="T">目标类型值</typeparam>  
        /// <param name="objValue">对象类型</param>  
        /// <returns>目标类型值</returns>  
        public static T Parse<T>(object objValue)
        {
            return Parse<T>(objValue, default(T));
        }



        public static string Convert2NotNullString(string value)
        {
            if (value == null)
                return "";

            return value;
        }

        public static string Convert2NotNullString(int? value, string deaultValue = "")
        {
            if (value.HasValue)
                return value.Value.ToString();

            return deaultValue;
        }

        public static int Convert2NotNullInt(int? value, int defaultValue = -1)
        {
            if (value.HasValue)
                return value.Value;

            return defaultValue;
        }

        public static int Convert2Int(string value, int defaultValue = -1)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double Conver2Double(object value, double defaultValue = 0)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// win8中tostring()为有星期信息显示
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="haveDate">是否有日期</param>
        /// <param name="haveTime">是否有时间</param>
        /// <returns></returns>
        public static string DateTime2String(DateTime time,
            bool haveDate = true, bool haveTime = true)
        {
            string s = "";
            if (haveDate)
                s += "yyyy-MM-dd ";
            if (haveTime)
                s += "HH:mm:ss";
            if (string.IsNullOrEmpty(s.Trim()))
                return time.ToString();
            return time.ToString(s);
        }
    }
}
