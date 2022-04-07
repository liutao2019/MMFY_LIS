using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using dcl.client.common;
using dcl.common;

namespace dcl.client.result.PatControl
{
    public class CalcItemResHelper
    {
        public string GetCalcRes(string formula, Hashtable ht, CalInfoEventArgs args)
        {

            if (string.IsNullOrEmpty(formula)) return string.Empty;

            switch (formula)
            {
                case "EPI-GRE(Scr)":
                    return CalcEPI_GRE_Scr(ht, args);
                case "EPI-GRE(SCysC)":
                    return CalcEPI_GRE_SCysC(ht, args);
                case "EPI-GRE(both)":
                    return CalcEPI_GRE(ht, args);
                case "eGFR":
                    return CalceGfr(ht, args);
                case "Ccr":
                    return CalceCcr(ht, args);
                case "eGFR-EPI":
                    return CalceeGFR_EPI(ht, args);
                case "B102":
                    return CalceB102(ht, args);
                default:
                    return string.Empty;

            }
        }

        private string CalceCcr(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue && !string.IsNullOrEmpty(args.pat_weight))
                {
                    string sex = args.Sex;

                    double age = args.Age.Value;


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCR_Ecd")].ToString();
                    // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue))
                    {
                        if (sex == "1")
                        {
                            mathall =
                                string.Format(
                                    "(((140-{0})*{1}*1.0)/72)*{2}", age, args.pat_weight, crevalue);
                        }
                        else
                        {
                            mathall = string.Format("(((140-{0})*{1}*1.0)/85)*{2}", age, args.pat_weight, crevalue);
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

        private string CalceGfr(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue)
                {
                    string sex = args.Sex;

                    double age = args.Age.Value;


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre =  ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCR_Ecd")].ToString();
                   // string csys = ht["CysC"].ToString();
                    double crevalue;
                    if (double.TryParse(cre, out crevalue) )
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

        private string CalcEPI_GRE(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue && args.Age>17)
                {
                    string sex = args.SampRem;

                    //double age = entityPatient.pat_age.Value;

                    double age = args.Age.Value;

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCR_Ecd")].ToString();
                    string csys = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCysC_Ecd")].ToString();
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

        private string CalcEPI_GRE_SCysC(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue && args.Age > 17)
                {
                    string sex = args.SampRem;

                    //double age = entityPatient.pat_age.Value;

                    double age = args.Age.Value;

                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCysC_Ecd")].ToString();
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

        private string CalcEPI_GRE_Scr(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue && args.Age > 17)
                {
                    string sex = args.SampRem;

                    //double age = entityPatient.pat_age.Value;

                    double age = args.Age.Value;

                    string mathall = string.Empty;


                    string cre = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCR_Ecd")].ToString();
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

        private string CalceeGFR_EPI(Hashtable ht, CalInfoEventArgs args)
        {
            try
            {
                if (args.Age.HasValue && !string.IsNullOrEmpty(args.Sex))
                {
                    string sex = args.Sex;

                    double age = args.Age.Value;


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht[ConfigHelper.GetSysConfigValueWithoutLogin("LAB_CalcSCR_Ecd")].ToString();
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
                                        " 144 * (Math.Pow(({0} * 0.01131) / 0.7, -{1})) * (Math.Pow(0.993, {2}))", crevalue, 0.329,age);
                            }
                            else {
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
                dcl.root.logon.Logger.WriteException("CalcItemResHelper", "eGFR_EPI", ex.ToString());
            }
            return string.Empty;
        }
        private string CalceB102(Hashtable ht, CalInfoEventArgs args)
        {
            string cal = string.Empty;
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
            string mathall = string.Empty;
            double calvalue;
            double b102value;
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
            return mathall;
        }
    }
}
