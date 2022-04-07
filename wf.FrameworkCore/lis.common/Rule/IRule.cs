using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
//using Lis.Entities;

namespace dcl.common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IRule
    {
        public abstract string ConvertRule(string src);

        public static IRule CreateRule(CommonValue.RuleType rule)
        {
            IRule RuleConverter = null;
            switch (rule)
            {
                case CommonValue.RuleType.Age:
                    RuleConverter = new AgeRule();
                    break;

                case CommonValue.RuleType.Sex:
                    RuleConverter = new SexRule();
                    break;
            }
            return RuleConverter;
        }

        /// <summary>
        /// 规则生成
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static IRule CreateRule(string rule)
        {
            IRule RuleConverter = new NoneRule();
            switch (rule)
            {
                #region 年龄
                case "Age":
                    RuleConverter = new AgeRule();
                    break;
                case "AgeInt":
                    RuleConverter = new AgeIntRule();
                    break;

                case "年龄_出生日期_转YMDHI":
                    RuleConverter = new Age_BirthdayToYMDHI();
                    break;
                case "年龄_出生日期_转YMDHI_不满6岁显月份":
                    RuleConverter = new Age_BirthdayToYMDHI_ShowMonthForUnder6();
                    break;
                case "年龄_中文年月日出生日期_转YMDHI_不满4岁显月份":
                    RuleConverter = new Age_BirthdayToYMDHI_ShowMonthForUnder4();
                    break;
                case "年龄_岁_转YMDHI":
                    RuleConverter = new Age_YearToYMDHI();
                    break;
                case "年龄_Y岁或M月_转YMDHI":
                    RuleConverter = new YorMAge_YearToYMDHI();
                    break;
                case "年龄_简单YMDHI转YMDHI_单位前后缀_取整":
                    RuleConverter = new Age_xYxMxDxHxI_YMDHI_IntType();
                    break;
                case "年龄_岁年月个月天日时小时分分钟转YMDHI_单位前后缀_取整":
                    RuleConverter = new Age_YorMorDorHorI_YMDHI_IntType();
                    break;
                case "年龄_中文转YMDHI":
                    RuleConverter = new Age_ChineseToYMDHI();
                    break;
                case "年龄_出生日期_or_中文_转YMDHI":
                    RuleConverter = new Age_BirthdayOrChineseToYMDHI();
                    break;

                #endregion

                #region 性别

                case "Sex":
                    RuleConverter = new SexRule();
                    break;

                case "性别_1男2女":
                    RuleConverter = new SexRule2();
                    break;

                case "性别_0男1女":
                    RuleConverter = new SexRule3();
                    break;

                case "性别_男女":
                    RuleConverter = new SexRule4();
                    break;
                case "条码_性别_1男2女":
                    RuleConverter = new SexRuleForbarcode();
                    break;
                #endregion

                #region 布尔
                case "布尔_1true_0false":
                    RuleConverter = new Bool1true0false();
                    break;

                case "布尔_是true_否false":
                    RuleConverter = new BoolTrueFalse();
                    break;
                #endregion

                case "SimpleAge":
                    RuleConverter = new SimpleAgeRule();
                    break;
            }
            return RuleConverter;
        }
    }

    public class NoneRule : IRule
    {
        public override string ConvertRule(string src)
        {
            return src;
        }
    }

    #region 年龄转换类
    /// <summary>
    /// 年龄转换规则
    /// </summary>
    public class AgeRule : IRule
    {
        public override string ConvertRule(string src)
        {
            string output = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                DateTime birthdate = new DateTime();
                if (DateTime.TryParse(src, out birthdate))
                    output = FromBirthdate(birthdate);
                else
                    return "0Y0M0D0H0I";
            }
            return output;
        }


        public virtual string FromBirthdate(DateTime date)
        {
            DateTime dtToday = DateTime.Now;
            TimeSpan ts = dtToday - date;

            int totalMinutes = (int)ts.TotalMinutes;

            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;

            year = totalMinutes / (365 * 24 * 60);

            if (dtToday.Year > date.Year && dtToday.Month > date.Month)
            {
                year = dtToday.Year - date.Year;
            }
            else if (dtToday.Year > date.Year && dtToday.Month == date.Month && dtToday.Day >= date.Day)
            {
                year = dtToday.Year - date.Year;
            }
            else if (dtToday.Year > date.Year)
            {
                year = dtToday.Year - date.Year - 1;
            }

            if (year == 0 || dtToday.Year == date.Year)
            {
                month = totalMinutes / (30 * 24 * 60);

                if (dtToday.Month > date.Month)
                {
                    if (dtToday.Month > date.Month && dtToday.Day >= date.Day)
                    {
                        month = dtToday.Month - date.Month;
                    }
                    else
                    {
                        month = dtToday.Month - date.Month - 1;
                    }
                }

                if (month == 0 || (dtToday.Year == date.Year && dtToday.Month == date.Month))
                {
                    day = totalMinutes / (24 * 60);

                    if (day == 0)
                    {
                        hour = totalMinutes / 60;

                        if (hour == 0)
                        {
                            minute = totalMinutes;
                        }
                    }
                }
            }

            //if (month >= 0 && year == 0) //一岁
            //{
            //    year = 1;

            //}
            ////大于一岁

            //minute = hour = day = month = 0;

            string output = string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, hour, minute);
            return output;
        }
    }

    public class AgeIntRule : IRule
    {
        public override string ConvertRule(string src)
        {
            string output = string.Empty;
            if (!string.IsNullOrEmpty(src))
            {
                DateTime birthdate = new DateTime();
                if (DateTime.TryParse(src, out birthdate))
                    output = FromBirthdate(birthdate);
                else
                    return "0";
            }
            return output;
        }


        public string FromBirthdate(DateTime date)
        {
            DateTime dtToday = DateTime.Now;
            TimeSpan ts = dtToday - date;

            int totalMinutes = (int)ts.TotalMinutes;

            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;

            year = totalMinutes / (365 * 24 * 60);

            if (year == 0)
            {
                month = totalMinutes / (30 * 24 * 60);

                if (month == 0)
                {
                    day = totalMinutes / (24 * 60);

                    if (day == 0)
                    {
                        hour = totalMinutes / 60;

                        if (hour == 0)
                        {
                            minute = totalMinutes;
                        }
                    }
                }
            }
            //if (month >= 0 && year == 0) //一岁
            //{
            //    year = 1;

            //}
            ////大于一岁

            //minute = hour = day = month = 0;

            return year.ToString();

        }
    }

    public class SimpleAgeRule : AgeRule
    {
        public override string ConvertRule(string src)
        {
            string complexAge = base.ConvertRule(src);
            if (string.IsNullOrEmpty(complexAge))
                return "0";
            else
            {
                int index = complexAge.IndexOf('Y');
                return complexAge.Substring(0, index);
            }
        }
    }

    /// <summary>
    /// 出生日期转YMDHI
    /// </summary>
    public class Age_BirthdayToYMDHI : AgeRule
    {
        public override string ConvertRule(string src)
        {
            string output = null;
            if (!string.IsNullOrEmpty(src))
            {
                DateTime birthdate = new DateTime();
                if (DateTime.TryParse(src, out birthdate))
                {
                    output = FromBirthdate(birthdate);
                }
                else { }
                //return "0Y0M0D0H0I";
            }
            return output;
        }
    }

    public class Age_BirthdayToYMDHI_ShowMonthForUnder6 : Age_BirthdayToYMDHI
    {
        public override string FromBirthdate(DateTime date)
        {
            //DateTime dtToday = DateTime.Now;
            //TimeSpan ts = dtToday - date;

            //int totalMinutes = (int)ts.TotalMinutes;

            //int year = 0;
            //int month = 0;
            //int day = 0;
            //int hour = 0;
            //int minute = 0;

            //year = totalMinutes / (365 * 24 * 60);

            //if (year == 0)
            //{
            //    month = totalMinutes / (30 * 24 * 60);

            //    if (month == 0)
            //    {
            //        day = totalMinutes / (24 * 60);

            //        if (day == 0)
            //        {
            //            hour = totalMinutes / 60;

            //            if (hour == 0)
            //            {
            //                minute = totalMinutes;
            //            }
            //        }
            //    }
            //}
            //else if (year < 6) //六岁以下显月份
            //{
            //    month = (totalMinutes % (365 * 24 * 60)) / (30 * 24 * 60);
            //}


            //string output = string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, hour, minute);
            //return output;
            string output = string.Empty;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;


            try
            {
                PhyIntervalDateTime info = new PhyIntervalDateTime();
                info.CalcuPhyDateTime(date, DateTime.Now);
                year = info.PhyYears;


                if (year == 0)
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, info.PhyMonths, info.PhyDays, info.PhyHours, 0);
                }
                else if (year < 6)
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, info.PhyMonths, info.PhyDays, 0, 0);

                }
                else
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, 0, 0, 0, 0);

                }
            }
            catch
            {
                DateTime dtToday = DateTime.Now;
                TimeSpan ts = dtToday - date;




                int totalMinutes = (int)ts.TotalMinutes;
                year = totalMinutes / (365 * 24 * 60);

                if (year == 0)
                {
                    month = totalMinutes / (30 * 24 * 60);

                    if (month == 0)
                    {
                        day = totalMinutes / (24 * 60);

                        if (day == 0)
                        {
                            hour = totalMinutes / 60;

                            if (hour == 0)
                            {
                                minute = totalMinutes;
                            }
                        }
                    }
                }
                else if (year < 6) //六岁以下显月份
                {
                    month = (totalMinutes % (365 * 24 * 60)) / (30 * 24 * 60);

                }
                output = string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, hour, minute);

            }



            return output;
        }
    }
    /// <summary>
    /// 出生日期转YMDHI
    /// </summary>
    public class Age_CHSBirthdayToYMDHI : AgeRule
    {
        public override string ConvertRule(string src)
        {
            string output = null;
            if (!string.IsNullOrEmpty(src))
            {
                string age = src.ToUpper().Replace('年', 'Y')
                                       .Replace('岁', 'Y')
                                       .Replace("个月", "M")
                                       .Replace('月', 'M')
                                       .Replace('日', 'D')
                                       .Replace('天', 'D')
                                       .Replace("小时", "H")
                                       .Replace('时', 'H')
                                       .Replace("分钟", "I")
                                       .Replace('分', 'I');

                string patten = "(Y|d|M|H|I)";
                string[] tmp = System.Text.RegularExpressions.Regex.Split(age, patten);
                string[] tmp2 = new string[tmp.Length];
                int count = 0;
                for (int i = 0; i < tmp.Length; i = i + 2)
                {
                    if (i + 1 >= tmp.Length)
                        continue;
                    tmp2[count] = tmp[i] + tmp[i + 1];
                    count++;
                }
                string year = null;
                string month = null;
                string day = null;
                string hour = null;
                string minute = null;
                foreach (string s in tmp2)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    if (s.Contains("Y") && year == null)
                        year = s;

                    if (s.Contains("M") && month == null)
                        month = s;

                    if (s.Contains("D") && day == null)
                        day = s;

                    if (s.Contains("H") && hour == null)
                        hour = s;

                    if (s.Contains("I") && minute == null)
                        minute = s;
                }
                if (year == null)
                {
                    year = DateTime.Now.Year+"Y";
                }
                if (month == null) month = DateTime.Now.Month + "M";
                if (day == null) day = "01D";
              
                src= year + month + day ;
                src = src.Replace("Y", "-").Replace("M", "-").Replace("D", "");
                DateTime birthdate = new DateTime();
                if (DateTime.TryParse(src, out birthdate))
                {
                    output = FromBirthdate(birthdate);
                }
                else { }
                //return "0Y0M0D0H0I";
            }
            return output;
        }
    }
    public class Age_BirthdayToYMDHI_ShowMonthForUnder4 : Age_CHSBirthdayToYMDHI
    {
        public override string FromBirthdate(DateTime date)
        {
            string output = string.Empty;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int minute = 0;


            try
            {
                PhyIntervalDateTime info = new PhyIntervalDateTime();
                info.CalcuPhyDateTime(date, DateTime.Now);
                year = info.PhyYears;


                if (year == 0)
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, info.PhyMonths, info.PhyDays, info.PhyHours, 0);
                }
                else if (year < 4)
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, info.PhyMonths, info.PhyDays, 0, 0);

                }
                else
                {
                    output = string.Format("{0}Y{1}M{2}D{3}H{4}I", info.PhyYears, 0, 0, 0, 0);

                }
            }
            catch
            {
                DateTime dtToday = DateTime.Now;
                TimeSpan ts = dtToday - date;




                int totalMinutes = (int)ts.TotalMinutes;
                year = totalMinutes / (365 * 24 * 60);

                if (year == 0)
                {
                    month = totalMinutes / (30 * 24 * 60);

                    if (month == 0)
                    {
                        day = totalMinutes / (24 * 60);

                        if (day == 0)
                        {
                            hour = totalMinutes / 60;

                            if (hour == 0)
                            {
                                minute = totalMinutes;
                            }
                        }
                    }
                }
                else if (year < 4) //六岁以下显月份
                {
                    month = (totalMinutes % (365 * 24 * 60)) / (30 * 24 * 60);

                }
                output = string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, hour, minute);

            }



            return output;
        }
    }
    class PhyIntervalDateTime
    {
        private int phyYears = 0;
        private int phyMonths = 0;
        private int phyDays = 0;
        private int phyHours = 0;
        private int phyMinutes = 0;
        private int phySeconds = 0;

        #region  属性
        /// <summary>
        /// 物理年差
        /// </summary>
        public int PhyYears
        {
            get
            {
                return phyYears;
            }
        }

        /// <summary>
        /// 物理月差
        /// </summary>
        public int PhyMonths
        {
            get
            {
                return phyMonths;
            }
        }

        /// <summary>
        /// 物理天差
        /// </summary>
        public int PhyDays
        {
            get
            {
                return phyDays;
            }
        }

        /// <summary>
        /// 物理时差
        /// </summary>
        public int PhyHours
        {
            get
            {
                return phyHours;
            }
        }

        /// <summary>
        /// 物理分差
        /// </summary>
        public int PhyMinutes
        {
            get
            {
                return phyMinutes;
            }
        }

        /// <summary>
        /// 物理秒差
        /// </summary>
        public int PhySeconds
        {
            get
            {
                return phySeconds;
            }
        }
        #endregion  属性

        #region  函数
        /// <summary>
        /// 计算两个日期的物年月日时分秒差
        /// </summary>
        /// <param name="sDate1"></param>
        /// <param name="sDate2"></param>
        public void CalcuPhyDateTime(DateTime Date1, DateTime Date2)
        {
            DateTime outDateTime;





            if (Date2 < Date1)
            {
                //throw new Exception("计算日期必须大于给定的参照日期！");
                return;
            }

            int Years, Months, Days, Hours, Minutes, Seconds;  //需要的结果
            int Year1, Month1, Day1, Hour1, Minute1, Second1;  //参照日期的相关数值
            int Year2, Month2, Day2, Hour2, Minute2, Second2;  //计算日期的相关数值

            Year1 = Date1.Year;
            Month1 = Date1.Month;
            Day1 = Date1.Day;
            Hour1 = Date1.Hour;
            Minute1 = Date1.Minute;
            Second1 = Date1.Second;

            Year2 = Date2.Year;
            Month2 = Date2.Month;
            Day2 = Date2.Day;
            Hour2 = Date2.Hour;
            Minute2 = Date2.Minute;
            Second2 = Date2.Second;

            ////年
            //Years = Year2 - Year1;

            ////月
            //if (Month2 - Month1 < 0)
            //{
            //    Months = 12 + Month2 - Month1;
            //    Years--;
            //}
            //else
            //{
            //    Months = Month2 - Month1;
            //}

            ////天
            //if (Day2 - Day1 < 0)
            //{
            //    Days = Day2 + DateTime.DaysInMonth(Date1.Year, Date1.Month) - Day1;
            //    Months--;
            //}
            //else
            //{
            //    Days = Day2 - Day1;
            //}

            //年
            Years = Year2 - Year1;

            //月
            if (Month2 - Month1 < 0)
            {
                Months = 12 + Month2 - Month1;
                Years--;
            }
            else
            {
                Months = Month2 - Month1;
                if (Day2 < Day1 && Month2 == Month1)
                {
                    Months = 12 + Month2 - Month1;
                    Years--;
                }
            }

            //天
            if (Day2 - Day1 < 0)
            {
                Date1 = Date2.AddMonths(-1);
                Days = Day2 + DateTime.DaysInMonth(Date1.Year, Date1.Month) - Day1;
                Months--;
            }
            else
            {
                Days = Day2 - Day1;
            }


            //时
            if (Hour2 - Hour1 < 0)
            {
                Hours = 24 + Hour2 - Hour1;
                Days--;
            }
            else
            {
                Hours = Hour2 - Hour1;
            }

            //分
            if (Minute2 - Minute1 < 0)
            {
                Minutes = 60 + Minute2 - Minute1;
                Hours--;
            }
            else
            {
                Minutes = Minute2 - Minute1;
            }

            //秒
            if (Second2 - Second1 < 0)
            {
                Seconds = 60 + Second2 - Second1;
                Minutes--;
            }
            else
            {
                Seconds = Second2 - Second1;
            }

            phyYears = Years;
            phyMonths = Months;
            phyDays = Days;
            phyHours = Hours;
            phyMinutes = Minutes;
            phySeconds = Seconds;
        }
        #endregion  函数
    }

    /// <summary>
    /// 岁转YMDHI
    /// </summary>
    public class Age_YearToYMDHI : IRule
    {
        public override string ConvertRule(string src)
        {
            string output = null;

            decimal year = 0;

            if (decimal.TryParse(src, out year))
            {
                output = Convert.ToInt32(year).ToString() + "Y0M0D0H0I";
            }

            return output;
        }
    }

    /// <summary>
    /// Y岁/M月转YMDHI
    /// </summary>
    public class YorMAge_YearToYMDHI : IRule
    {
        public override string ConvertRule(string src)
        {
            string output = string.Empty;

            decimal year = 0;


            if (src.IndexOf('M') > -1 || src.IndexOf('m') > -1)
            {
                src = src.Replace("M", "").Replace("m", "");
                if (decimal.TryParse(src, out year))
                {
                    //小于2个月转换为天
                    if (year < 2)
                    {
                        year = year * 30;

                        output = "0Y0M" + Convert.ToInt32(year).ToString() + "D0H0I";
                        //decimal.round(sum, 0, midpointrounding.awayfromzero);
                    }
                    else//四舍五入取月
                    {
                        year = decimal.Round(year, 0, MidpointRounding.AwayFromZero);
                        output = "0Y" + Convert.ToInt32(year).ToString() + "M0D0H0I";
                    }
                }
            }
            else if (src.IndexOf('D') > -1 || src.IndexOf('d') > -1)
            {
                src = src.Replace("D", "").Replace("d", "");
                if (decimal.TryParse(src, out year))
                {
                    output = "0Y0M" + Convert.ToInt32(year).ToString() + "D0H0I";
                }
            }
            else if (src.IndexOf('Y') > -1 || src.IndexOf('y') > -1)
            {
                src = src.Replace("Y", "").Replace("y", "");
                if (decimal.TryParse(src, out year))
                {
                    year = decimal.Round(year, 0, MidpointRounding.AwayFromZero);
                    output = Convert.ToInt32(year).ToString() + "Y0M0D0H0I";
                }
            }
            else
            {
                if (decimal.TryParse(src, out year))
                {
                    year = decimal.Round(year, 0, MidpointRounding.AwayFromZero);
                    output = Convert.ToInt32(year).ToString() + "Y0M0D0H0I";
                }
            }

            return output;
        }
    }

    /// <summary>
    /// 年龄_简单YMDHI转YMDHI_单位前后缀_取整
    /// </summary>
    public class Age_xYxMxDxHxI_YMDHI_IntType : IRule
    {
        public override string ConvertRule(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return "";
            }

            decimal intTest;

            if (decimal.TryParse(src, out intTest))
            {
                return Convert.ToInt32(intTest).ToString() + "Y0M0D0H0I";
            }


            string strAge = src.ToLower();


            string strY = "0";
            string strM = "0";
            string strD = "0";
            string strH = "0";
            string strI = "0";

            if (strAge.IndexOf("y") > 0)
            {
                #region surfix y
                strY = strAge.Substring(0, strAge.IndexOf("y"));
                if (strAge.IndexOf("m") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (strAge.IndexOf("y") == 0 && strAge.Length > 0)
            {
                #region prefix y
                if (strAge.IndexOf("m") > 0)
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                            }
                            else
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                            }
                        }
                        else
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                        }
                    }
                    else
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                    }
                }
                else
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.Length - strAge.IndexOf("y") - 1);
                }
                #endregion

            }
            else if (strAge.IndexOf("m") > 0)
            {
                strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                if (strAge.IndexOf("d") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        }
                    }
                }
            }
            else if (strAge.IndexOf("m") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("d") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                        }
                        else
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                        }
                    }
                    else
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                    }
                }
                else
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                }
            }
            else if (strAge.IndexOf("d") > 0)
            {
                strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                if (strAge.IndexOf("h") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    }
                }
            }
            else if (strAge.IndexOf("d") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("h") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                    }
                    else
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                    }
                }
                else
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                }
            }
            else if (strAge.IndexOf("h") > 0)
            {
                strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                if (strAge.IndexOf("i") > 0)
                {
                    strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("h") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("i") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                }
                else
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("i") > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
            }
            else if (strAge.IndexOf("i") == 0 && strAge.Length > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
            }
            else
            {
                return string.Empty;
            }

            string strOutAge = string.Empty;
            try
            {
                strY = Convert.ToInt32(Convert.ToDecimal(strY)).ToString();
                strM = Convert.ToInt32(Convert.ToDecimal(strM)).ToString();
                strD = Convert.ToInt32(Convert.ToDecimal(strD)).ToString();
                strH = Convert.ToInt32(Convert.ToDecimal(strH)).ToString();
                strI = Convert.ToInt32(Convert.ToDecimal(strI)).ToString();
                strOutAge = strY + "Y" + strM + "M" + strD + "D" + strH + "H" + strI + "I";
            }
            catch
            {


            }




            return strOutAge;
        }
    }

    /// <summary>
    /// 年龄_岁年月个月天日时小时分分钟转YMDHI_单位前后缀_取整
    /// </summary>
    public class Age_YorMorDorHorI_YMDHI_IntType : IRule
    {
        public override string ConvertRule(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return "";
            }

            decimal intTest;

            if (decimal.TryParse(src, out intTest))
            {
                return Convert.ToInt32(intTest).ToString() + "Y0M0D0H0I";
            }


            string strAge = src.ToLower();


            string strY = "0";
            string strM = "0";
            string strD = "0";
            string strH = "0";
            string strI = "0";

            if (strAge.IndexOf("岁") > 0 || strAge.IndexOf("年") > 0)
            {
                #region surfix y
                string strConverY = "";
                if (strAge.IndexOf("岁") > 0)
                {
                    strConverY = "岁";
                    strY = strAge.Substring(0, strAge.IndexOf("岁"));
                }
                else if (strAge.IndexOf("年") > 0)
                {
                    strY = strAge.Substring(0, strAge.IndexOf("年"));
                    strConverY = "年";
                }

                if (strAge.IndexOf("月") > 0 || strAge.IndexOf("个月") > 0)
                {
                    string strConverM = "";
                    if (strAge.IndexOf("个月") > 0)
                    {
                        strConverM = "个月";

                    }
                    else if (strAge.IndexOf("月") > 0)
                    {

                        strConverM = "月";

                    }

                    strM = strAge.Substring(strAge.IndexOf(strConverY) + strConverY.Length, strAge.IndexOf(strConverM) - strAge.IndexOf(strConverY) - strConverY.Length);

                    if (strAge.IndexOf("日") > 0 || strAge.IndexOf("天") > 0)
                    {
                        string strConverD = "";
                        if (strAge.IndexOf("日") > 0)
                        {
                            strConverD = "日";

                        }
                        else if (strAge.IndexOf("天") > 0)
                        {

                            strConverD = "天";

                        }

                        strD = strAge.Substring(strAge.IndexOf(strConverM) + strConverM.Length, strAge.IndexOf(strConverD) - strAge.IndexOf(strConverM) - strConverM.Length);
                        if (strAge.IndexOf("时") > 0 || strAge.IndexOf("小时") > 0)
                        {
                            string strConverH = "";
                            if (strAge.IndexOf("小时") > 0)
                            {
                                strConverH = "小时";

                            }
                            else if (strAge.IndexOf("时") > 0)
                            {

                                strConverH = "时";

                            }

                            strH = strAge.Substring(strAge.IndexOf(strConverD) + 1, strAge.IndexOf(strConverH) - strAge.IndexOf(strConverD) - 1);
                            if (strAge.IndexOf("分") > 0 || strAge.IndexOf("分钟") > 0)
                            {
                                string strConverI = "";
                                if (strAge.IndexOf("分钟") > 0)
                                {
                                    strConverI = "分钟";

                                }
                                else if (strAge.IndexOf("分") > 0)
                                {

                                    strConverI = "分";

                                }
                                strI = strAge.Substring(strAge.IndexOf(strConverH) + strConverH.Length, strAge.IndexOf(strConverI) - strAge.IndexOf(strConverH) - strConverH.Length);
                            }
                        }
                    }
                }
                #endregion
            }

            else if (strAge.IndexOf("月") > 0 || strAge.IndexOf("个月") > 0)
            {
                string strConverM = "";
                if (strAge.IndexOf("个月") > 0)
                {
                    strConverM = "个月";

                }
                else if (strAge.IndexOf("月") > 0)
                {

                    strConverM = "月";

                }

                strM = strAge.Substring(0, strAge.IndexOf(strConverM));

                if (strAge.IndexOf("日") > 0 || strAge.IndexOf("天") > 0)
                {
                    string strConverD = "";
                    if (strAge.IndexOf("日") > 0)
                    {
                        strConverD = "日";

                    }
                    else if (strAge.IndexOf("天") > 0)
                    {

                        strConverD = "天";

                    }

                    strD = strAge.Substring(strAge.IndexOf(strConverM) + strConverM.Length, strAge.IndexOf(strConverD) - strAge.IndexOf(strConverM) - strConverM.Length);
                    if (strAge.IndexOf("时") > 0 || strAge.IndexOf("小时") > 0)
                    {
                        string strConverH = "";
                        if (strAge.IndexOf("小时") > 0)
                        {
                            strConverH = "小时";

                        }
                        else if (strAge.IndexOf("时") > 0)
                        {

                            strConverH = "时";

                        }

                        strH = strAge.Substring(strAge.IndexOf(strConverD) + strConverD.Length, strAge.IndexOf(strConverH) - strAge.IndexOf(strConverD) - strConverD.Length);
                        if (strAge.IndexOf("分") > 0 || strAge.IndexOf("分钟") > 0)
                        {
                            string strConverI = "";
                            if (strAge.IndexOf("分钟") > 0)
                            {
                                strConverI = "分钟";

                            }
                            else if (strAge.IndexOf("分") > 0)
                            {

                                strConverI = "分";

                            }
                            strI = strAge.Substring(strAge.IndexOf(strConverH) + strConverH.Length, strAge.IndexOf(strConverI) - strAge.IndexOf(strConverH) - strConverH.Length);
                        }
                    }
                }
            }
            else if (strAge.IndexOf("日") > 0 || strAge.IndexOf("天") > 0)
            {
                string strConverD = "";
                if (strAge.IndexOf("日") > 0)
                {
                    strConverD = "日";

                }
                else if (strAge.IndexOf("天") > 0)
                {

                    strConverD = "天";

                }

                strD = strAge.Substring(0, strAge.IndexOf(strConverD));
                if (strAge.IndexOf("时") > 0 || strAge.IndexOf("小时") > 0)
                {
                    string strConverH = "";
                    if (strAge.IndexOf("小时") > 0)
                    {
                        strConverH = "小时";

                    }
                    else if (strAge.IndexOf("时") > 0)
                    {

                        strConverH = "时";

                    }

                    strH = strAge.Substring(strAge.IndexOf(strConverD) + strConverD.Length, strAge.IndexOf(strConverH) - strAge.IndexOf(strConverD) - strConverD.Length);
                    if (strAge.IndexOf("分") > 0 || strAge.IndexOf("分钟") > 0)
                    {
                        string strConverI = "";
                        if (strAge.IndexOf("分钟") > 0)
                        {
                            strConverI = "分钟";

                        }
                        else if (strAge.IndexOf("分") > 0)
                        {

                            strConverI = "分";

                        }
                        strI = strAge.Substring(strAge.IndexOf(strConverH) + strConverH.Length, strAge.IndexOf(strConverI) - strAge.IndexOf(strConverH) - strConverH.Length);
                    }
                }
            }
            else if (strAge.IndexOf("时") > 0 || strAge.IndexOf("小时") > 0)
            {
                string strConverH = "";
                if (strAge.IndexOf("小时") > 0)
                {
                    strConverH = "小时";

                }
                else if (strAge.IndexOf("时") > 0)
                {

                    strConverH = "时";

                }

                strH = strAge.Substring(0, strAge.IndexOf(strConverH));
                if (strAge.IndexOf("分") > 0 || strAge.IndexOf("分钟") > 0)
                {
                    string strConverI = "";
                    if (strAge.IndexOf("分钟") > 0)
                    {
                        strConverI = "分钟";

                    }
                    else if (strAge.IndexOf("分") > 0)
                    {

                        strConverI = "分";

                    }
                    strI = strAge.Substring(strAge.IndexOf(strConverH) + strConverH.Length, strAge.IndexOf(strConverI) - strAge.IndexOf(strConverH) - strConverH.Length);
                }
            }
            else if (strAge.IndexOf("分") > 0 || strAge.IndexOf("分钟") > 0)
            {
                string strConverI = "";
                if (strAge.IndexOf("分钟") > 0)
                {
                    strConverI = "分钟";

                }
                else if (strAge.IndexOf("分") > 0)
                {

                    strConverI = "分";

                }
                strI = strAge.Substring(0, strAge.IndexOf(strConverI));
            }
            else
            {
                return string.Empty;
            }

            string strOutAge = string.Empty;
            try
            {
                strY = Convert.ToInt32(Convert.ToDecimal(strY)).ToString();
                strM = Convert.ToInt32(Convert.ToDecimal(strM)).ToString();
                strD = Convert.ToInt32(Convert.ToDecimal(strD)).ToString();
                strH = Convert.ToInt32(Convert.ToDecimal(strH)).ToString();
                strI = Convert.ToInt32(Convert.ToDecimal(strI)).ToString();

                if (strY != "0")
                {
                    strOutAge = strY + "Y0M0D0H0I";
                }
                else if (strM != "0")
                {
                    strOutAge = "0Y" + strM + "M0D0H0I";
                }
                else if (strD != "0")
                {
                    strOutAge = "0Y0M" + strD + "D0H0I";
                }
                else if (strH != "0")
                {
                    strOutAge = "0Y0M0D" + strH + "H0I";
                }
                else
                {
                    strOutAge = "0Y0M0D0H" + strI + "I";
                }
            }
            catch
            {


            }




            return strOutAge;
        }
    }

    /// <summary>
    /// 年龄_中文装YMDHI
    /// </summary>
    public class Age_ChineseToYMDHI : IRule
    {

        public override string ConvertRule(string src)
        {
            string age = src.ToUpper().Replace('年', 'Y')
                                       .Replace('岁', 'Y')
                                       .Replace("个月", "M")
                                       .Replace('月', 'M')
                                       .Replace('日', 'D')
                                       .Replace('天', 'D')
                                       .Replace("小时", "H")
                                       .Replace('时', 'H')
                                       .Replace("分钟", "I")
                                       .Replace('分', 'I');

            string patten = "(Y|D|M|H|I)";
            string[] tmp = System.Text.RegularExpressions.Regex.Split(age, patten);
            string[] tmp2 = new string[tmp.Length];
            int count = 0;
            for (int i = 0; i < tmp.Length; i = i + 2)
            {
                if (i + 1 >= tmp.Length)
                    continue;
                tmp2[count] = tmp[i] + tmp[i + 1];
                count++;
            }
            string year = null;
            string month = null;
            string day = null;
            string hour = null;
            string minute = null;
            foreach (string s in tmp2)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                if (s.Contains("Y") && year == null)
                    year = s;

                if (s.Contains("M") && month == null)
                    month = s;

                if (s.Contains("D") && day == null)
                    day = s;

                if (s.Contains("H") && hour == null)
                    hour = s;

                if (s.Contains("I") && minute == null)
                    minute = s;
            }
            if (year == null) year = "0Y";
            if (month == null) month = "0M";
            if (day == null) day = "0D";
            if (hour == null) hour = "0H";
            if (minute == null) minute = "0I";
            return year + month + day + hour + minute;
        }
    }

    /// <summary>
    /// 年龄_出生日期_or_中文_转YMDHI
    /// </summary>
    public class Age_BirthdayOrChineseToYMDHI : IRule
    {
        public override string ConvertRule(string src)
        {
            IRule RuleConverterTemp = new NoneRule();

            DateTime dti = DateTime.Now;
            if (DateTime.TryParse(src, out dti))
            {
                RuleConverterTemp = new Age_BirthdayToYMDHI();
                return RuleConverterTemp.ConvertRule(src);
            }
            else
            {
                RuleConverterTemp = new Age_ChineseToYMDHI();
                return RuleConverterTemp.ConvertRule(src);
            }
        }
    }

    #endregion

    #region 性别转换类
    /// <summary>
    /// 性别转换规则 "男_女"
    /// </summary>
    public class SexRule : IRule
    {
        public override string ConvertRule(string src)
        {
            string sex = "0";
            if (src.Trim() == "男")
            {
                sex = "1";
            }
            else if (src.Trim() == "女")
            {
                sex = "2";
            }
            return sex;
        }
    }

    /// <summary>
    /// 性别转换规则 1男 2女
    /// </summary>
    public class SexRule2 : IRule
    {
        public override string ConvertRule(string src)
        {
            string sex = "0";

            if (src.Trim() == "1")
            {
                sex = "1";
            }
            else if (src.Trim() == "2")
            {
                sex = "2";
            }
            return sex;
        }
    }

    /// <summary>
    /// 性别转换规则 0男 1女
    /// </summary>
    public class SexRule3 : IRule
    {
        public override string ConvertRule(string src)
        {
            string sex = "0";

            if (src.Trim() == "0")
            {
                sex = "1";//男
            }
            else if (src.Trim() == "1")
            {
                sex = "2";//女
            }
            return sex;
        }
    }

    /// <summary>
    /// 性别转换规则 男 女
    /// </summary>
    public class SexRule4 : IRule
    {
        public override string ConvertRule(string src)
        {
            string sex = "0";

            if (src.Trim() == "男")
            {
                sex = "1";
            }
            else if (src.Trim() == "女")
            {
                sex = "2";
            }
            return sex;
        }
    }

    /// <summary>
    /// 条码性别转换规则 男 女
    /// </summary>
    public class SexRuleForbarcode : IRule
    {
        public override string ConvertRule(string src)
        {
            string sex = "0";

            if (src.Trim() == "1")
            {
                sex = "男";
            }
            else if (src.Trim() == "2")
            {
                sex = "女";
            }
            return sex;
        }
    }
    #endregion

    #region 布尔类型转换
    public class Bool1true0false : IRule
    {
        public override string ConvertRule(string src)
        {
            string ret = "false";

            if (src == "1")
            {
                ret = "true";
            }
            return ret;
        }
    }

    public class BoolTrueFalse : IRule
    {
        public override string ConvertRule(string src)
        {
            string ret = "false";

            if (src == "是")
            {
                ret = "true";
            }
            return ret;
        }
    }
    #endregion
}
