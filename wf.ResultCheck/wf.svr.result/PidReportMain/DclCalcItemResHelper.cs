using dcl.common;
using dcl.entity;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    internal class DclCalcItemResHelper
    {

        public string GetCalcRes(string formula, Hashtable ht, EntityPidReportMain entityPatient)
        {

            if (string.IsNullOrEmpty(formula)) return string.Empty;

            switch (formula)
            {
                case "EPI-GRE(Scr)":
                    return CalcEPI_GRE_Scr(ht, entityPatient);
                case "EPI-GRE(SCysC)":
                    return CalcEPI_GRE_SCysC(ht, entityPatient);
                case "EPI-GRE(both)":
                    return CalcEPI_GRE(ht, entityPatient);
                case "eGFR":
                    return CalceGfr(ht, entityPatient);
                case "Ccr":
                    return CalceCcr(ht, entityPatient);
                case "eGFR-EPI":
                    return CalceeGFR_EPI(ht, entityPatient);
                case "B102":
                    return CalceB102(ht, entityPatient);
                default:
                    return string.Empty;

            }
        }


        private string CalceCcr(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                if (entityPatient.PidAge.HasValue && !string.IsNullOrEmpty(entityPatient.PidWeight))
                {
                    string sex = entityPatient.PidSex;

                    double age = 1;
                    int minute = Convert.ToInt32(entityPatient.PidAge.Value % 60);

                    int hour = Convert.ToInt32(entityPatient.PidAge.Value / 60);

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCR_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "1")
                        {
                            mathall =
                                string.Format(
                                    "(((140-{0})*{1}*1.0)/72)*{2}", age, entityPatient.PidWeight, crevalue);
                        }
                        else
                        {
                            mathall = string.Format("(((140-{0})*{1}*1.0)/85)*{2}", age, entityPatient.PidWeight, crevalue);
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                return decVal.ToString("0.00");
                            }
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "CalceCcr", ex.ToString());
            }
            return string.Empty;
        }

        private string CalceGfr(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                if (entityPatient.PidAge.HasValue)
                {
                    string sex = entityPatient.PidSex;

                    double age = 1;
                    int minute = Convert.ToInt32(entityPatient.PidAge.Value % 60);

                    int hour = Convert.ToInt32(entityPatient.PidAge.Value / 60);

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCR_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "1")
                        {
                            mathall =
                                string.Format(
                                    "186* (Math.Pow({0},{1}))*(Math.Pow({2},{3}))",
                                    Math.Round(crevalue / 88.4, 2), "-1.154", age, "-0.203");
                        }
                        else
                        {

                            mathall =
                                string.Format(
                                    "186* (Math.Pow({0},{1}))*(Math.Pow({2},{3}))*0.742",
                                    Math.Round(crevalue / 88.4, 2), "-1.154", age, "-0.203");
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                return decVal.ToString("0.00");
                            }
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "CalceGfr", ex.ToString());
            }
            return string.Empty;
        }

        private string CalcEPI_GRE(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                if (entityPatient.PidAge.HasValue)
                {
                    string sex = entityPatient.PidSex;

                    //double age = entityPatient.pat_age.Value;

                    double age = 1;
                    int minute = Convert.ToInt32(entityPatient.PidAge.Value % 60);

                    int hour = Convert.ToInt32(entityPatient.PidAge.Value / 60);

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }

                    if (age < 18) return string.Empty;

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCR_Ecd")].ToString();
                    string csys = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCysC_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    double csysvalue;
                    if (double.TryParse(cre, out crevalue) && double.TryParse(csys, out csysvalue))
                    {
                        if (sex == "1")
                        {
                            if ((crevalue / 88.4) <= 0.9)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                        else
                        {
                            if ((crevalue / 88.4) <= 0.7)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                return decVal.ToString("0.00");
                            }
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "CalcEPI_GRE_Scr", ex.ToString());
            }
            return string.Empty;
        }

        private string CalcEPI_GRE_SCysC(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                if (entityPatient.PidAge.HasValue)
                {
                    string sex = entityPatient.PidSex;

                    //double age = entityPatient.pat_age.Value;

                    double age = 1;
                    int minute = Convert.ToInt32(entityPatient.PidAge.Value % 60);

                    int hour = Convert.ToInt32(entityPatient.PidAge.Value / 60);

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }

                    if (age < 18) return string.Empty;

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCysC_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "1")
                        {
                            if (crevalue <= 0.8)
                            {
                                mathall =
                                    string.Format(
                                        "133* (Math.Pow(({0}/0.8),-{1}))*(Math.Pow(0.996,{2}))",
                                        crevalue, "0.499", age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                       "133* (Math.Pow(({0}/0.8),-{1}))*(Math.Pow(0.996,{2}))",
                                       crevalue, "1.328", age);
                            }

                        }
                        else
                        {

                            if (crevalue <= 0.8)
                            {
                                mathall =
                                    string.Format(
                                        "133* (Math.Pow(({0}/0.8),-{1}))*(Math.Pow(0.996,{2}))*0.932",
                                        crevalue, "0.499", age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                       "133* (Math.Pow(({0}/0.8),-{1}))*(Math.Pow(0.996,{2}))*0.932",
                                       crevalue, "1.328", age);
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                return decVal.ToString("0.00");
                            }
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "CalcEPI_GRE_SCysC", ex.ToString());
            }
            return string.Empty;
        }

        private string CalcEPI_GRE_Scr(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                if (entityPatient.PidAge.HasValue)
                {
                    string sex = entityPatient.PidSex;

                    //double age = entityPatient.pat_age.Value;

                    double age = 1;
                    int minute = Convert.ToInt32(entityPatient.PidAge.Value % 60);

                    int hour = Convert.ToInt32(entityPatient.PidAge.Value / 60);

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }

                    if (age < 18) return string.Empty;

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCR_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "1")
                        {
                            if ((crevalue / 88.4) <= 0.9)
                            {
                                mathall =
                                    string.Format(
                                        "141* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(0.993,{2}))",
                                        Math.Round(crevalue / 88.4, 2), "0.411", age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                       "141* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(0.993,{2}))",
                                       Math.Round(crevalue / 88.4, 2), "1.209", age);
                            }

                        }
                        else
                        {

                            if ((crevalue / 88.4) <= 0.7)
                            {
                                mathall =
                                    string.Format(
                                        "144* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(0.993,{2}))",
                                        Math.Round(crevalue / 88.4, 2), "0.329", age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                       "144* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(0.993,{2}))",
                                       Math.Round(crevalue / 88.4, 2), "1.209", age);
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                return decVal.ToString("0.00");
                            }
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "CalcEPI_GRE_Scr", ex.ToString());
            }
            return string.Empty;
        }


        private string CalceeGFR_EPI(Hashtable ht, EntityPidReportMain entityPatient)
        {
            try
            {
                //标本必须是血清才执行
                if (entityPatient.SamName != "血清")
                {
                    return "";
                }
                if (entityPatient.PidAge.HasValue && !string.IsNullOrEmpty(entityPatient.PidSex))
                {
                    string sex = entityPatient.PidSex;

                    string[] spage = entityPatient.PidAgeExp.Split('Y');
                    double age = Convert.ToDouble(spage[0]);

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_CalcSCR_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "2")
                        {
                            if (crevalue <= 62)
                            {
                                mathall =
                                    string.Format(
                                        " 144 * (Math.Pow(({0} * 0.01131) / 0.7, -{1})) * (Math.Pow(0.993, {2}))", crevalue, 0.329, age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                       "144 * (Math.Pow(({0} * 0.01131) / 0.7, -{1})) * (Math.Pow(0.993, {2}))", crevalue, 1.209, age);
                            }
                        }
                        else
                        {
                            if (crevalue <= 80)
                            {

                                mathall =
                                    string.Format(
                                        "141 * (Math.Pow(({0} * 0.01131) / 0.9, -{1})) * (Math.Pow(0.993, {2}))", crevalue, 0.411, age);
                            }
                            else
                            {
                                mathall =
                                   string.Format(
                                      "141 * (Math.Pow(({0} * 0.01131) / 0.9, -{1})) * (Math.Pow(0.993, {2}))", crevalue, 1.209, age);
                            }
                        }
                    
                    if (!string.IsNullOrEmpty(mathall))
                        {
                            //pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                            object objValue = ExpressionCompute.CalExpression(mathall);
                            if (objValue != null)
                            {
                                decimal decVal = 0;

                                if (decimal.TryParse(objValue.ToString(), out decVal))
                                {

                                    decVal = decimal.Round(decVal, 4);
                                    return decVal.ToString("0.00");
                                }
                                return string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "eGFR_EPI", ex.ToString());
            }
            return string.Empty;
        }
        private string CalceB102(Hashtable ht, EntityPidReportMain entityPatient)
        {
            string cal = string.Empty;
            string mathall = string.Empty;
            double calvalue;
            double b102value;
            try
            {
                if (ht.Contains("UA4"))
                {
                    cal = ht["UA4"].ToString();
                }
                else if (ht.Contains("UA5"))
                {
                    cal = ht["UA5"].ToString();
                }
                else if (ht.Contains("UB24"))
                {
                    cal = ht["UB24"].ToString();
                }
                string B102 = ht["B102"].ToString();

                if (B102 != null && double.TryParse(B102, out b102value) && b102value < 0.3)
                {
                    return mathall;
                }
                else
                {
                    if (double.TryParse(cal, out calvalue) && double.TryParse(B102, out b102value))
                    {
                        mathall = (calvalue / b102value).ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
         
            return mathall;
        }
    }
}
