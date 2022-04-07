using System;
using System.Collections.Generic;

using System.Text;

using dcl.common.extensions;

namespace dcl.client.elisa
{
    public struct Judge
    {
        public const string ID = "imj_id";
        public const string ItemID = "imj_itm_id";
        public const string Value = "imj_value";
        public const string Expression = "imj_express";
        public const string Res = "imj_res";
        public const string PosMin = "imj_pos_min";
        public const string PosMax = "imj_pos_max";
        public const string RegMax = "imj_neg_max";
        public const string RegMin = "imj_neg_min";
        public const string Pos = "pos";
        public const string Neg = "neg";
        public const string ValueExpress = "imj_valueexpress";
        public const string imj_feebpos_min = "imj_feebpos_min";
        public const string imj_feebpos_max = "imj_feebpos_max";
        public const string imj_Minusnull = "imj_minusnull";
        public const String TableName = "imm_judge";
    }
    /// <summary>
    /// 酶标阴阳判断Model类
    /// </summary>
    public class Judgor
    {
        public Judgor(string expression, string value, string nature)
            : this(expression, value, nature, DefaultOfNullValue, DefaultOfNullValue, DefaultOfNullValue, DefaultOfNullValue, strDefaultOfNullValue, strDefaultOfNullValue)
        {
           
        }


        public Judgor(string expression, string value, string nature, string feebposMin, string feebposMax)
            : this(expression, value, nature, DefaultOfNullValue, DefaultOfNullValue, DefaultOfNullValue, DefaultOfNullValue, feebposMin, feebposMax)
        {

        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="value">判定值</param>
        /// <param name="nature">阴阳性</param>
       /// <param name="posMin">阳性最小值</param>
       /// <param name="posMax">阳性最大值</param>
       /// <param name="regMin">阴性最小值</param>
       /// <param name="regMax">阴性最大值</param>
       /// <param name="feebposMin">弱阳性最小值</param>
       /// <param name="feebposMax">弱阳性最大值</param>
        public Judgor(string expression, string value, string nature, double posMin, double posMax, double regMin, double regMax, string p_feebposMin, string p_feebposMax)
        {
            this.Expression = expression;
            double dou=0;
            double.TryParse(value, out dou);
            this.Value = dou;
            this.Nature = Nature.Unkown;
            if (nature == Judge.Pos)
                Nature = Nature.Pos;
            else if (nature == Judge.Neg)
                Nature = Nature.Neg;

            this.PosMax = posMax;
            this.PosMin = posMin;
            this.NegMax = regMax;
            this.NegMin = regMin;
            this.feebposMin = p_feebposMin;
            this.feebposMax = p_feebposMax;

        }

        public static double DefaultOfNullValue  = -1;

        public static string strDefaultOfNullValue = "";

        /// <summary>
        /// 判定值
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// 阴阳性
        /// </summary>
        public Nature Nature { get; set; }
        /// <summary>
        /// 阴性最小值
        /// </summary>
        public double NegMin { get; set; }
        /// <summary>
        /// 阴性最大值
        /// </summary>
        public double NegMax { get; set; }
        /// <summary>
        /// 阳性最小值
        /// </summary>
        public double PosMin { get; set; }
        /// <summary>
        /// 阳性最大值
        /// </summary>
        public double PosMax { get; set; }

        /// <summary>
        /// 弱阳性最大值
        /// </summary>
        public string feebposMax { get; set; }

        /// <summary>
        /// 弱阳性最小值
        /// </summary>
        public string feebposMin { get; set; }

        ///// <summary>
        ///// 极值保护,当超过极值时,取极值
        ///// </summary>
        ///// <param name="source"></param>
        ///// <returns></returns>
        //internal double ExtremePoint(string source)
        //{
        //    if (string.IsNullOrEmpty( source))
        //        return 0;
        //    double valueOfSource = Convert.ToDouble(source);
        //    switch (Nature)
        //    {
        //        case Nature.Unkown:                
        //            break;
        //        case Nature.Pos:
        //            valueOfSource = ProtectScope(valueOfSource, PosMax, PosMin);
        //            break;
        //        case Nature.Neg:
        //            valueOfSource = ProtectScope(valueOfSource, NegMax, NegMin);           
        //            break;
        //        default:
        //            break;
        //    }

        //    return valueOfSource;
        //}

        //private double ProtectScope(double valueOfSource,double max, double min)
        //{

        //    if (valueOfSource > max && max != DefaultOfNullValue)
        //        valueOfSource = max;
        //    if (valueOfSource < min && min != DefaultOfNullValue)
        //        valueOfSource = min;
        //    return valueOfSource;
        //}


    }
}
