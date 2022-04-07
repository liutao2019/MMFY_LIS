using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.common
{
    public class AgeConverter
    {
        /// <summary>
        /// 年月日时分 转换成 分钟
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static int ToMinute(int year, int month, int day, int hour, int minute)
        {
            int d = year * 365;
            d += month * 30;
            d += day;

            hour += d * 24;

            minute += hour * 60;
            //month = month + year * 12;
            //day = day + 30 * month;
            //hour = hour + day * 24;
            //minute = minute + hour * 60;
            return minute;
        }

        /// <summary>
        /// 年龄转换成数据库显示格式
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string ToDBDisplayFormat(int year, int month, int day, int hour, int minute)
        {
            //return string.Format("{0}Y{1}M{2}D{3}H{4}m", year, month, day, hour, minute);
            return string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, hour, minute);
        }

        /// <summary>
        /// 岁转换成分钟
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int YearToMinute(int year)
        {
            return ToMinute(year, 0, 0, 0, 0);
        }

        /// <summary>
        /// xxx岁xx月xx天xx时xx分 转换成分钟
        /// </summary>
        /// <param name="ageText"></param>
        /// <returns></returns>
        public static int AgeDisplayTextToMinute(string ageText)
        {

            string ageValue = string.Empty;
            if (ageText != null && ageText.Length > 0)
            {
                ageValue = ageText.Replace('年', 'Y')
                                  .Replace('岁', 'Y')
                                  .Replace('月', 'M')
                                  .Replace('天', 'D')
                                  .Replace('日', 'D')
                                  .Replace('时', 'H')
                                  .Replace('分', 'I');
            }
            return AgeValueTextToMinute(ageValue);
        }


        /// <summary>
        /// YMDHm格式转换成分钟
        /// </summary>
        /// <param name="ageValue"></param>
        /// <returns></returns>
        public static int AgeValueTextToMinute(string ageValue)
        {
            string ageValue2 = ageValue;
            ageValue2 = ageValue2.Trim(null);
            if (ageValue2 != null && ageValue2.Length > 0)
            {
                string strYear = ageValue2.Split('Y')[0];
                string right = ageValue2.Split('Y')[1];

                string strMonth = right.Split('M')[0];
                right = ageValue2.Split('M')[1];

                string strDay = right.Split('D')[0];
                right = ageValue2.Split('D')[1];

                string strHour = right.Split('H')[0];
                right = ageValue2.Split('H')[1];

                string strMinute = string.Empty;

                if (right.Contains("m"))
                {
                    strMinute = right.Split('m')[0];
                }
                else
                {
                    strMinute = right.Split('I')[0];
                }

                int intYear = 0;
                int.TryParse(strYear, out intYear);

                int intMonth = 0;
                int.TryParse(strMonth, out intMonth);

                int intDay = 0;
                int.TryParse(strDay, out intDay);

                int intHour = 0;
                int.TryParse(strHour, out intHour);

                int intMinute = 0;
                int.TryParse(strMinute, out intMinute);


                return ToMinute(intYear, intMonth, intDay, intHour, intMinute);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 分钟转换成年月日时分
        /// </summary>
        /// <param name="minuteValue"></param>
        /// <returns></returns>
        public static PatAge MinuteToAge(int minuteValue)
        {
            PatAge age = new PatAge();

            if (minuteValue == 0)
            {
                return age;
            }

            int minute = minuteValue % 60;

            int hour = minuteValue / 60;

            int day = hour / 24;
            hour = hour % 24;

            int month = day / 30;
            day = day % 30;

            int year = month / 12;
            month = month % 12;


            age.Minute = minute;
            age.Hour = hour;
            age.Day = day;
            age.Month = month;
            age.Year = year;

            return age;
        }

        /// <summary>
        /// 去掉0单位
        /// 20Y0M0D0H0m -> 20Y
        /// </summary>
        /// <param name="age_value"></param>
        /// <returns></returns>
        public static string TrimZeroValue(string ageValue)
        {
            if (ageValue == null || ageValue == string.Empty)
            {
                return ageValue;
            }

            if (ageValue == "0Y0M0D0H0m" || ageValue == "0Y0M0D0H0I")
            {
                return "0Y";
            }

            string newValue = string.Empty;
            string ageValue2 = ageValue;

            ageValue2 = ageValue2.Trim(null);

            string strYear = ageValue2.Split('Y')[0];
            string right = ageValue2.Split('Y')[1];

            string strMonth = right.Split('M')[0];
            right = ageValue2.Split('M')[1];

            string strDay = right.Split('D')[0];
            right = ageValue2.Split('D')[1];

            string strHour = right.Split('H')[0];
            right = ageValue2.Split('H')[1];

            //string strMinute = right.Split('m')[0];

            string strMinute = string.Empty;

            if (right.Contains("m"))
            {
                strMinute = right.Split('m')[0];
            }
            else
            {
                strMinute = right.Split('I')[0];
            }

            if (strYear != "0")
            {
                newValue = strYear + "Y";
            }

            if (strMonth != "0")
            {
                newValue += strMonth + "M";
            }

            if (strDay != "0")
            {
                newValue += strDay + "D";
            }

            if (strHour != "0")
            {
                newValue += strHour + "H";
            }

            if (strMinute != "0")
            {
                newValue += strMinute + "I";
            }

            return newValue;
        }

        /// <summary>
        /// 存储格式转为显示格式
        /// 20Y->20岁
        /// </summary>
        /// <param name="ageValue"></param>
        /// <returns></returns>
        public static string ValueToText(string ageValue)
        {
            string ageText = ageValue;
            ageText = ageText.Replace('Y', '岁').Replace('M', '月').Replace('D', '天').Replace('H', '时').Replace('m', '分').Replace('I', '分');
            return ageText;
        }
    }

    public class PatAge
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        public PatAge()
        {
            Year = 0;
            Month = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
        }
    }
}
