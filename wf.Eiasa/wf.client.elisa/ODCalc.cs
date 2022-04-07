using System;
using System.Collections.Generic;

using System.Text;
using dcl.common.extensions;
using dcl.common;
using System.Windows.Forms;

namespace dcl.client.elisa
{
    /// <summary>
    /// OD值判定Model类
    /// </summary>
    public class ODCalc
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="negMin">阴性最小值</param>
        /// <param name="negMax">阴性最大值</param>
        /// <param name="posMin">阳性最小值</param>
        /// <param name="posMax">阳性最大值</param>
        public ODCalc(string expression, string negMin, string negMax, string posMin, string posMax, bool p_blnMinusnull)
        {
            this.Expression = expression;
            ContrastList = new Dictionary<string, Contrast>();
            blnMinusnull = p_blnMinusnull;

            ContrastList.Add("2", new Contrast("2", "[空白]"));
            Contrast neg = new Contrast("3", "[阴性]");
            neg.blnMinusnull = blnMinusnull;
            decimal NegMinTemp;
            if (decimal.TryParse(negMin, out NegMinTemp))
                neg.MinValue = NegMin = NegMinTemp;
            decimal NegMaxTemp;
            if (decimal.TryParse(negMax, out  NegMaxTemp))
                neg.MaxValue = NegMax = NegMaxTemp;
            ContrastList.Add("3", neg);

            Contrast pos = new Contrast("4", "[阳性]");
            pos.blnMinusnull = blnMinusnull;
            decimal PosMinTemp;
            if (decimal.TryParse(posMin, out  PosMinTemp))
                pos.MinValue = PosMin = PosMinTemp;
            decimal PosMaxTemp;
            if (decimal.TryParse(posMax, out PosMaxTemp))
                pos.MaxValue = PosMax = PosMaxTemp;
            ContrastList.Add("4", pos);
            ContrastList.Add("5", new Contrast("5", "[质控H]"));
            ContrastList.Add("6", new Contrast("6", "[质控M]"));
            ContrastList.Add("7", new Contrast("7", "[质控L]"));
            //   ContrastList.Add("7", new Contrast("8", "[样本]"));
            //NegList = new List<string>();
            //PosList = new List<string>();
            //HasNegAverageValue = false;
            //HasPosAverageValue = false;
        }

        public Dictionary<string, Contrast> ContrastList;
        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 阴性最小值(小于此值取此值)
        /// </summary>
        public decimal NegMin { get; set; }
        /// <summary>
        /// 阴性最大值(大于此值取此值)
        /// </summary>
        public decimal NegMax { get; set; }
        /// <summary>
        /// 阳性最小值
        /// </summary>
        public decimal PosMin { get; set; }
        /// <summary>
        /// 阳性最大值
        /// </summary>
        public decimal PosMax { get; set; }

        /// <summary>
        /// 阴阳性对照物是否默认减去空白值计算
        /// </summary>
        public bool blnMinusnull { get; set; }

        ///// <summary>
        ///// 阴性对照
        ///// </summary>
        //public List<string> NegList { get; set; }
        ///// <summary>
        ///// 阴性对照平均值
        ///// </summary>
        //public decimal NegAverageValue { get; set; }
        ///// <summary>
        ///// 阳性对照
        ///// </summary>
        //public List<string> PosList { get; set; }
        ///// <summary>
        ///// 阳性对照平均值
        ///// </summary>
        //public decimal PosAverageValue { get; set; }

        //public bool HasNegAverageValue { get; set; }
        //public bool HasPosAverageValue { get; set; }

        /// <summary>
        /// 获取阴阳孔位列表
        /// </summary>
        /// <param name="HoleStatusList"></param>
        /// <param name="mainControls"></param>
        public void GetValueList(List<string> HoleStatusList, List<System.Windows.Forms.Control> mainControls)
        {

            for (int i = 0; i < HoleStatusList.Count; i++)
            {
                if (ContrastList.ContainsKey(HoleStatusList[i]))
                {
                    ContrastList[HoleStatusList[i]].ValueList.Add(mainControls[i].Text);
                    ContrastList[HoleStatusList[i]].HasAverageValue = false;
                }
                //Contrast item = ContrastList[HoleStatusList[i]];
                //if (item != null)
                //item.ValueList.Add(mainControls[i].Text);               
            }
        }

        /// <summary>
        /// 计算平均值
        /// </summary>
        public void CalcAverageValue()
        {

            foreach (Contrast item in ContrastList.Values)
            {
                item.ContrastList = ContrastList;
                item.CalcAverageValue();
            }
        }

        /// <summary>
        /// 根据对照物的平均值格式化计算公式
        /// </summary>
        public void FormatFormula()
        {
            foreach (Contrast item in ContrastList.Values)
            {
                if (item.HasAverageValue)
                    Expression = Expression.Replace(item.Name, "("+item.AverageValue.ToString()+")");
            }
        }

        /// <summary>
        ///  根据表达式来计算阴阳性判定值
        /// </summary>
        /// <param name="HoleStatusList">每个孔的状态值，</param>
        /// <param name="mainControls">每个孔的控件信息</param>
        /// <returns></returns>
        public double GetExpressJudgePosNegVlaue(List<string> HoleStatusList, List<Control> mainControls)
        {
            this.GetValueList(HoleStatusList, mainControls);
            this.CalcAverageValue();
            this.FormatFormula();
            return Evaluator.EvaluateToDouble(this.Expression);
        }
    }

    /// <summary>
    /// 对照物(阳性，阴性，质控等等）
    /// </summary>
    public class Contrast
    {
        public Contrast(string holeStatus, string name)
        {
            this.HoleStatus = holeStatus;
            this.Name = name;
            HasAverageValue = false;
            ValueList = new List<string>();
        }
        /// <summary>
        /// 对照物原始值集合
        /// </summary>
        public Dictionary<string,Contrast> ContrastList { get; set; }
        /// <summary>
        /// 对照值列表
        /// </summary>
        public List<string> ValueList { get; set; }

        /// <summary>
        /// 对照平均值
        /// </summary>
        public decimal AverageValue { get; set; }

        /// <summary>
        /// 对照物孔位状态
        /// </summary>
        public string HoleStatus { get; set; }

        /// <summary>
        /// 简称,如"[阴性]"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 有平均值
        /// </summary>
        public bool HasAverageValue { get; set; }

        /// <summary>
        /// 是否是阳性对照
        /// </summary>
        public bool IsPos
        {
            get { return HoleStatus == "4"; }
        }

        /// <summary>
        /// 是否是阴性对照
        /// </summary>
        public bool IsNeg
        {
            get { return HoleStatus == "3"; }
        }
        /// <summary>
        /// 是否有最小值
        /// </summary>
        public bool HasMin { get; set; }

        /// <summary>
        /// 是否有最大值
        /// </summary>
        public bool HasMax { get; set; }

        /// <summary>
        /// 最小值(小于此值取此值)
        /// </summary>
        public decimal MinValue { get; set; }

        /// <summary>
        /// 最大值(大于此值取此值)
        /// </summary>
        public decimal MaxValue { get; set; }

        /// <summary>
        /// 阴阳性对照物是否默认减去空白值计算
        /// </summary>
        public bool blnMinusnull { get; set; }

        /// <summary>
        /// 是否存在对照值列表
        /// </summary>
        internal bool HasValueList
        {
            get { return ValueList != null && ValueList.Count > 0; }
        }

        /// <summary>
        /// 计算平均值 
        /// </summary>
        internal void CalcAverageValue()
        {
            if (!HasValueList)
                return;
            decimal result = 0;
            
            decimal deckongbai = 0;

            if (IsPos||IsNeg)//如果是阴阳对照物则进行空白对照加减
            {
                if (blnMinusnull)
                {
                    Contrast kongbai = ContrastList["2"];

                    if (kongbai.HasValueList)
                    {

                        if (kongbai.HasAverageValue)
                        {
                            deckongbai = kongbai.AverageValue;
                        }
                        else
                        {

                            kongbai.CalcAverageValue();
                            deckongbai = kongbai.AverageValue;
                        }

                    }
                }
            }
            

            foreach (string item in ValueList)
            {
                string strItemValue = item;
                if (deckongbai>0)//进行空白对照计算阴阳性值
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        strItemValue= (Convert.ToDecimal(item) - deckongbai).ToString();
                    }
                }

                result += ExtremePoint(strItemValue);
            }

            AverageValue = result / ValueList.Count;
            HasAverageValue = true;
        }

        /// <summary>
        /// 极值保护,当超过极值时,取极值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal decimal ExtremePoint(string source)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                    return 0;
                decimal valueOfSource = Convert.ToDecimal(source);
                if (IsPos || IsNeg)
                    valueOfSource = ProtectScope(valueOfSource, MaxValue, MinValue);



                return valueOfSource;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private decimal ProtectScope(decimal valueOfSource, decimal max, decimal min)
        {
            if (valueOfSource > max && max > 0)
                valueOfSource = max;
            if (valueOfSource < min && min > 0)
                valueOfSource = min;
            return valueOfSource;
        }
    }
}
