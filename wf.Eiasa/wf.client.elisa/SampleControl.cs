using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.Collections;
using dcl.common;
using dcl.common.extensions;
using DevExpress.XtraEditors;

namespace dcl.client.elisa
{
    public partial class SampleControl : UserControl
    {
        public SampleControl()
        {
            InitializeComponent();
            Init();
            this.ItemType = ControlType.TextBox;//默认初始化为单个text控件显示或录入
            LoadUI();
        }

        public SampleControl(ControlType itemType)
        {
            InitializeComponent();
            SetDisplayMode(ControlType.Complex);
        }

        public void SetDisplayMode(ControlType itemType)
        {
            topLabels.Clear();
            leftLabels.Clear();
            mainControls.Clear();

            Init();
            this.ItemType = itemType;
            if (itemType == ControlType.Complex) //复杂酶标控件的处理
            {
                this.HoleSize = new System.Drawing.Size(49, 49);
                MarginOfControls = 53;
            }

            this.HasCreateUI = false;
            this.panelMain.Controls.Clear();
            this.panelLeft.Controls.Clear();
            this.panel1.Controls.Clear();

            LoadUI();

            checkBox_converCol_CheckedChanged(this.checkBox_converCol, EventArgs.Empty);
            checkBox_converRow_CheckedChanged(this.checkBox_converRow, EventArgs.Empty);
        }

        List<Control> topLabels = new List<Control>();
        List<Control> leftLabels = new List<Control>();
        public List<Control> mainControls = new List<Control>();

        public string HoleMode { get; set; }
        public string HoleStatus { get; set; }


        [DefaultValue(12)]
        public int Columns { get; set; }
        [DefaultValue(8)]
        public int Rows { get; set; }
        [DefaultValue(40)]
        public int MarginOfControls { get; set; }
        public string DefaultValueOfHoles { get; set; }
        [DefaultValue(typeof(Size), "25,25")]
        public Size HoleSize { get; set; }

        public string ImmID { get; set; }

        [DefaultValue(ControlType.Complex)]
        public ControlType ItemType { get; set; }

        /// <summary>
        /// 最大孔序号
        /// </summary>
        private int intMaxSeq = 0;
        /// <summary>
        /// 最大孔序号中漏掉的序号
        /// </summary>
        private List<int> lstLostSeq = new List<int>();
        /// <summary>
        /// 是否单击控件时自动计算序号值，并填充
        /// </summary>
        public bool blnAoutoSetSeq = false;

        /// <summary>
        /// 孔位模式List
        /// </summary>
        public List<string> HoleModeList
        {
            get { return EiasaConvertor.FormatValueToList(HoleMode); }

        }

        /// <summary>
        /// 孔位状态List
        /// </summary>
        public List<string> HoleStatusList
        {
            get { return EiasaConvertor.FormatValueToList(HoleStatus); }
        }

        /// <summary>
        /// 标本范围
        /// </summary>
        public SampleRange SampleRange { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        public bool HasCreateUI = false;


        public string StrNesPosValues = string.Empty;

        /// <summary>
        /// 不能产生结果的孔索引
        /// </summary>
        private List<int> lstNoResult = new List<int>();

        /// <summary>
        /// 加载UI界面
        /// </summary>
        private void LoadUI()
        {
            if (HasCreateUI)
                return;

            BuildLables(panel1, topLabels, 20, 20, ControlType.Label);
            BuildLables(panelLeft, leftLabels, 20, 0, ControlType.Label, Position.Vertical);

            for (int i = 0; i < Rows; i++)
            {
                BuildLables(panelMain, mainControls, 13, i * MarginOfControls, ItemType);
            }

            HasCreateUI = true;


            if (this.ItemType == ControlType.TextBox)
            {
                //遍历所有控件加上单击事件
                foreach (Control contl in mainControls)
                {
                    contl.Click += new System.EventHandler(Control_MouseClick);
                }
            }

            //竖排LABEL显示为字母默认
            this.checkBox_converRow.Checked = true;

        }

        void sam(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成显示标签
        /// </summary>
        /// <param name="container"></param>
        /// <param name="controls"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="controlType"></param>
        private void BuildLables(Control container, List<Control> controls, int x, int y, ControlType controlType)
        {
            BuildLables(container, controls, x, y, controlType, Position.Horizontal);
        }

        /// <summary>
        /// 生成显示标签
        /// </summary>
        /// <param name="container"></param>
        /// <param name="controls"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="controlType"></param>
        /// <param name="position"></param>
        private void BuildLables(Control container, List<Control> controls, int x, int y, ControlType controlType, Position position)
        {
            int vertical = 0;
            int horizontal = 0;
            int controlsCount = 0;
            int startIndex = controls.Count;
            switch (position)
            {
                case Position.Vertical:
                    controlsCount = Rows;
                    break;
                case Position.Horizontal:
                    controlsCount = Columns;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < controlsCount; i++)
            {
                switch (position)
                {
                    case Position.Vertical:
                        vertical = x;
                        horizontal = y + i * MarginOfControls;
                        break;
                    case Position.Horizontal:
                        vertical = x + i * MarginOfControls;
                        horizontal = y;
                        break;
                    default:
                        break;
                }

                switch (controlType)
                {
                    case ControlType.Label:
                        controls.Add(new LabelControl());
                        break;
                    case ControlType.TextBox:
                        controls.Add(CreateNewTextEdit());
                        break;
                    case ControlType.Complex:
                        controls.Add(new ComplexResultsControl());
                        break;
                    default:
                        break;
                }
                int index = startIndex + i;
                Control temp = controls[index];
                temp.Name = controlType.ToString() + (index + 1).ToString();
                temp.Tag = (index + 1).ToString();

                if (!string.IsNullOrEmpty(DefaultValueOfHoles) && temp is TextBox)
                    temp.Text = DefaultValueOfHoles;
                else if (temp is LabelControl)
                    temp.Text = (index + 1).ToString();

                temp.Size = HoleSize;
                //if (temp is TextBox)
                //    (temp as TextBox).TextAlign = HorizontalAlignment.Center;
                temp.Location = new Point(vertical, horizontal);

                container.Controls.Add(temp);
            }
        }

        /// <summary>
        /// 生成TextEdit控件
        /// </summary>
        /// <returns></returns>
        private static TextEdit CreateNewTextEdit()
        {
            TextEdit edit = new TextEdit();
            edit.EnterMoveNextControl = true;
            return edit;
        }

        /// <summary>
        /// 格式化孔值－用string设置或返回string
        /// </summary>
        public string FormatHoleValues
        {
            get
            { return GetHoleValues(); }
            set
            {
                SetHoleValues(value);
            }
        }

        /// <summary>
        /// 格式化的OD值
        /// </summary>
        public string FormatODValues
        {
            //get
            //{ return GetHoleValues(); }
            set
            {
                SetHoleValues(ControlType.Complex, value);
            }
        }

        /// <summary>
        /// 格式化阴阳定性值
        /// </summary>
        public string FormatNesPosValues
        {

            set
            {
                SetHoleNegPosValues(value.Split(','));
            }
        }

        /// <summary>
        /// 设置孔数据
        /// </summary>
        /// <param name="value"></param>
        private void SetHoleValues(string value)
        {
            SetHoleValues(ControlType.TextBox, value);
        }

        /// <summary>
        /// 设置孔数据
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="value"></param>
        private void SetHoleValues(ControlType controlType, string value)
        {
            if (ItemType == ControlType.Complex)
                Clear();

            if (string.IsNullOrEmpty(value))
                return;


            if (value[0] == ',')
            {
                value = value.Substring(1);
            }

            SetHoleValues(controlType, value.Split(','));
        }


        /// <summary>
        /// 设置孔数据
        /// </summary>
        /// <param name="values"></param>
        private void SetHoleValues(string[] values)
        {
            SetHoleValues(ControlType.TextBox, values);
        }

        /// <summary>
        /// 设置孔数据
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="values"></param>
        private void SetHoleValues(ControlType controlType, string[] values)
        {
            int count = (values.Length > mainControls.Count) ? mainControls.Count : values.Length;
            for (int i = 0; i < count; i++)
            {
                string strValue = values[i].Trim();

                if (controlType == ControlType.Complex)
                {
                    (mainControls[i] as ComplexResultsControl).ODText = strValue;
                }
                else
                {
                    mainControls[i].Text = strValue;
                    if (!string.IsNullOrEmpty(StrNesPosValues) && StrNesPosValues.Split(',').Length == count&&StrNesPosValues.Split(',')[i].Contains("+"))
                    {
                        mainControls[i].ForeColor = Color.Red;
                    }
                    else
                    {
                        mainControls[i].ForeColor = Color.Black;
                    }
                }
            }
        }

        /// <summary>
        /// 设置孔的阴阳数据
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="values"></param>
        private void SetHoleNegPosValues(string[] values)
        {
            int count = (values.Length > mainControls.Count) ? mainControls.Count : values.Length;
            for (int i = 0; i < count; i++)
            {
                string strValue = values[i].Trim();


                (mainControls[i] as ComplexResultsControl).NegOrPosText = strValue;

            }
        }

        /// <summary>
        /// 清空孔数据
        /// </summary>
        public void Clear()
        {
            foreach (Control item in mainControls)
            {
                item.Refresh();
            }
        }

        /// <summary>
        /// OD值List
        /// </summary>
        /// <returns></returns>
        public List<string> GetODList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < mainControls.Count; i++)
            {
                if (ItemType == ControlType.Complex)
                {
                    result.Add((mainControls[i] as ComplexResultsControl).ODText);
                }
            }

            return result;
        }


        /// <summary>
        /// 阴阳值List
        /// </summary>
        /// <returns></returns>
        public List<string> GetNegOrPosList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < mainControls.Count; i++)
            {
                if (ItemType == ControlType.Complex)
                {
                    result.Add((mainControls[i] as ComplexResultsControl).NegOrPosText);
                }
            }

            return result;
        }

        /// <summary>
        /// 原始值List
        /// </summary>
        /// <returns></returns>
        public List<string> GetValuesList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < mainControls.Count; i++)
            {
                if (ItemType == ControlType.Complex)
                {
                    result.Add((mainControls[i] as ComplexResultsControl).Text);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取孔数据，以String类型返回
        /// </summary>
        /// <returns></returns>
        private string GetHoleValues()
        {
            Dictionary<int, string> results = ValuesOfHoles();
            string result = "";
            foreach (string item in results.Values)
            {
                result += "," + item;
            }

            return result;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Init()
        {
            Columns = 12;
            Rows = 8;
            MarginOfControls = 38;
            HoleSize = new Size(25, 25);
        }

        /// <summary>
        /// 孔值
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> ValuesOfHoles()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (Control ctl in mainControls)
            {
                result.Add(Convert.ToInt32(ctl.Tag.ToString()), ctl.Text);
            }

            return result;
        }

        /// <summary>
        /// 是否所有的仪器原始值都存在
        /// </summary>
        public bool NotAllHolesHaveValues
        {
            get
            {
                Dictionary<int, string> sourceValues = ValuesOfHoles();
                foreach (string item in sourceValues.Values)
                {
                    if (string.IsNullOrEmpty(item))
                        return true;
                }

                return false;
            }
        }


        /// <summary>
        /// 显示控件
        /// ShowControl(this.panelTop.Controls, "Label10");
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="controlName"></param>
        private void ShowControl(ControlCollection controls, string controlName)
        {
            foreach (Control ctl in controls)
            {
                if (ctl.Name == controlName)
                {
                    lis.client.control.MessageDialog.Show(ctl.Text);
                }
            }
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            lis.client.control.MessageDialog.Show((mainControls[2] as TextEdit).Text);
        }

        /// <summary>
        /// 判断阴阳性
        /// </summary>
        /// <param name="judgor"></param>
        internal void JudgeNegOrPos(Judgor judgor)
        {
            List<string> manyExpress = new List<string>();
            List<string> lstFeebPosMin = new List<string>();//弱阳性最小值判断值
            List<string> lstFeebPosMax = new List<string>();//弱阳性最大值判断值
            for (int i = 0; i < mainControls.Count; i++)
            {
                // double result = judgor.ExtremePoint((mainControls[i] as ComplexResultsControl).ODText);
                if ((HoleStatusList[i] == "1" || HoleStatusList[i] == "5" ||
                    HoleStatusList[i] == "6" || HoleStatusList[i] == "7") && !lstNoResult.Contains(i))//用孔序号匹配不匹配的话默认不判断阴阳性
                {
                    double result = Convert.ToDouble((mainControls[i] as ComplexResultsControl).ODText);
                    manyExpress.Add(string.Format("{0} {1} {2}", result, judgor.Expression, judgor.Value));

                    lstFeebPosMin.Add(string.Format("{0} {1} {2}", result, ">=", judgor.feebposMin));
                    lstFeebPosMax.Add(string.Format("{0} {1} {2}", result, "<=", judgor.feebposMax));
                }
                else if (string.IsNullOrEmpty((mainControls[i] as ComplexResultsControl).ODText))
                {
                    manyExpress.Add("1+1==3"); //默认为false
                    lstFeebPosMin.Add("1+1==3");
                    lstFeebPosMax.Add("1+1==3");
                }
            }

            List<bool> results = Evaluator.EvaluateMany<bool>(manyExpress);

            List<bool> FeebPosMinRes = new List<bool>();// Evaluator.EvaluateMany<bool>(lstFeebPosMin);
            List<bool> FeebPosMaxRes = new List<bool>();// Evaluator.EvaluateMany<bool>(lstFeebPosMax);

            //如果没有弱阳性值则不判断
            if (!string.IsNullOrEmpty(judgor.feebposMax) && !string.IsNullOrEmpty(judgor.feebposMin))
            {
                FeebPosMinRes = Evaluator.EvaluateMany<bool>(lstFeebPosMin);
                FeebPosMaxRes = Evaluator.EvaluateMany<bool>(lstFeebPosMax);
            }

            for (int i = 0; i < mainControls.Count; i++)
            {
                if (HoleStatusList[i] == "1" || HoleStatusList[i] == "5" ||
                    HoleStatusList[i] == "6" || HoleStatusList[i] == "7")
                {
                    if (FeebPosMaxRes.Count > 0 && FeebPosMinRes.Count > 0)
                    {
                        if (FeebPosMinRes[i] == true && FeebPosMaxRes[i] == true)
                        {
                            //此时为弱阳性
                            Nature Nature = Nature.feedPos;
                            (mainControls[i] as ComplexResultsControl).SetNegPos(Nature, true);
                        }
                        else
                        {
                            (mainControls[i] as ComplexResultsControl).SetNegPos(judgor.Nature, results[i]);
                        }
                    }

                    else
                    {
                        (mainControls[i] as ComplexResultsControl).SetNegPos(judgor.Nature, results[i]);
                    }




                }
                else if (HoleStatusList[i]=="4")//将阳性对照物定性设置为阳性。
                {
                    (mainControls[i] as ComplexResultsControl).SetNegPos(judgor.Nature, true);
                }
            }
        }

        /// <summary>
        /// 判断OD值
        /// </summary>
        /// <param name="express"></param>
        internal void JudgeOD(ODCalc calc)
        {
            if (string.IsNullOrEmpty(calc.Expression))
                return;

            List<string> manyExpress = new List<string>();

            calc.GetValueList(HoleStatusList, mainControls);
            // 计算平均值
            calc.CalcAverageValue();
            calc.FormatFormula();
            decimal decResultPrse = 0;
            lstNoResult.Clear();
            for (int i = 0; i < mainControls.Count; i++)
            {
                //如果原始值不为数字不能计算时能加入LIST记录着孔位序号
                if (!decimal.TryParse(mainControls[i].Text, out decResultPrse))
                {
                    lstNoResult.Add(i);
                    continue;
                }
                if (HoleStatusIs("1", i))
                {
                    manyExpress.Add(calc.Expression.Replace("[样本]", "(" + mainControls[i].Text + ")"));
                }
                else if (HoleStatusIs("5", i))
                {
                    manyExpress.Add(calc.Expression.Replace("[样本]", "(" + mainControls[i].Text + ")"));
                    //manyExpress.Add(calc.Expression.Replace("[质控H]", mainControls[i].Text));
                }
                else if (HoleStatusIs("6", i))
                {
                    manyExpress.Add(calc.Expression.Replace("[样本]", "(" + mainControls[i].Text + ")"));
                    //manyExpress.Add(calc.Expression.Replace("[质控M]", mainControls[i].Text));
                }
                else if (HoleStatusIs("7", i))
                {
                    manyExpress.Add(calc.Expression.Replace("[样本]", "(" + mainControls[i].Text + ")"));
                    //manyExpress.Add(calc.Expression.Replace("[质控L]", mainControls[i].Text));
                }
                else //非样本不计算
                    //非范围内不计算
                    manyExpress.Add(mainControls[i].Text);
            }

            List<double> results = Evaluator.EvaluateMany<double>(manyExpress);
            //将不能计算的孔值为空赋值进去显示
            for (int i2 = 0; i2 < lstNoResult.Count; i2++)
            {
                results.Insert(lstNoResult[i2], 0);
            }

            string strPoint = "F4";//默认保留四位小数
            string p_pointconfig = dcl.client.frame.UserInfo.GetSysConfigValue("EiasaResultPoint");
            switch (p_pointconfig)
            {
                case "四位": strPoint = "F4"; break;
                case "三位": strPoint = "F3"; break;
                case "两位": strPoint = "F2"; break;
                default: strPoint = "F4"; break;
            }

            string result = "";
            for (int i = 0; i < results.Count; i++)
            {
                if ((HoleStatusList[i] == "1" || HoleStatusList[i] == "5" || HoleStatusList[i] == "6" || HoleStatusList[i] == "7")
                    && !lstNoResult.Contains(i))//如果与没结果LIST记录索引匹配，则跳空
                {
                    result += "," + results[i].ToString(strPoint);//默认保留四位小数strPoint
                }
                else
                    result += ",";
            }

            FormatODValues = result;
        }


        /// <summary>
        /// 判断孔状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool HoleStatusIs(string code, int i)
        {
            return HoleStatusList[i] == code;
        }

        /// <summary>
        /// 自动累加设置单个孔数据
        /// </summary>
        /// <param name="p_strContorlName">控件名字</param>
        public void m_mthSetOneSep(Control control)
        {
            int intIndex = this.mainControls.IndexOf(control);
            this.m_intGetMaxSeq();
            if (intIndex > 0)
            {
                string strSeq = "";
                if (lstLostSeq.Count > 0)
                {
                    strSeq = lstLostSeq[0].ToString();

                }
                else
                {
                    strSeq = (intMaxSeq + 1).ToString();
                }
                this.mainControls[intIndex].Text = strSeq;

            }



        }


        /// <summary>
        /// 获取最大孔序号,以及检查有没有中间漏掉的序号
        /// </summary>
        /// <returns></returns>
        public int m_intGetMaxSeq()
        {
            int intControlCount = this.mainControls.Count;
            int intTemp = 0;
            List<int> lstAllSeq = new List<int>();
            for (int i = 0; i < intControlCount; i++)
            {
                if (!string.IsNullOrEmpty(this.mainControls[i].Text.Trim()) && Int32.TryParse(this.mainControls[i].Text.Trim(), out intTemp))
                {
                    lstAllSeq.Add(int.Parse(this.mainControls[i].Text.Trim()));//添加序号记录到所有序号LIST当中

                    if (Int32.Parse(this.mainControls[i].Text.Trim()) > intMaxSeq)
                    {
                        intMaxSeq = Convert.ToInt32(this.mainControls[i].Text.Trim());
                    }
                }


            }

            //先清空旧数据漏掉的LIST
            lstLostSeq.Clear();
            //从最大的序号里找出没有用到的序号

            for (int i2 = 1; i2 < intMaxSeq; i2++)
            {
                if (!lstAllSeq.Contains(i2))
                {
                    lstLostSeq.Add(i2);//如果找不到匹配的，则记录到漏掉的序号LISt里
                }
            }
            if (lstLostSeq.Count > 0)
            {
                lstLostSeq.Sort();
            }

            return intMaxSeq;
        }

        /// <summary>
        /// 单击文本控件事件填充事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_MouseClick(object sender, EventArgs e)
        {
            if (blnAoutoSetSeq)
            {
                this.m_mthSetOneSep((Control)sender);
            }

        }

        /// <summary>
        /// 横向自动填充数据
        /// </summary>
        public void m_mthLaterAllFill(int p_intRow, int p_intCol)
        {



            if (p_intRow > this.Rows || p_intCol > this.Columns)
            {
                lis.client.control.MessageDialog.Show("请输入小于面板的行数与列数！", "输入了错误信息！");
            }

            //如果参数行和列为0则默认全部行和列填充
            if (p_intRow == 0)
            {
                p_intRow = this.Rows;
            }
            if (p_intCol == 0)
            {
                p_intCol = this.Columns;
            }

            string[,] strDataArr = new string[this.Rows, this.Columns];
            int intSeq = 1;
            //组建纵向填充数据到二维数组里

            for (int row = 0; row < this.Rows; row++)
            {
                if (p_intRow > row)
                {
                    for (int col = 0; col < this.Columns; col++)
                    {
                        if (p_intCol > col)
                        {
                            strDataArr[row, col] = intSeq.ToString();
                            intSeq++;
                        }
                        else
                        {
                            strDataArr[row, col] = "";
                        }

                    }
                }

            }
            intSeq = 0;
            //二维数据赋值到控件里
            for (int row2 = 0; row2 < this.Rows; row2++)
            {
                for (int col2 = 0; col2 < this.Columns; col2++)
                {
                    if (!string.IsNullOrEmpty(strDataArr[row2, col2]))
                    {
                        this.mainControls[intSeq].Text = strDataArr[row2, col2];

                    }
                    else
                    {
                        this.mainControls[intSeq].Text = "";
                    }
                    intSeq++;
                }
            }

        }

        /// <summary>
        /// 纵向自动填充数据
        /// <param name="p_intRow">真实需要填充数据的行数</param>
        /// <param name="p_intCol">真实填充数据的列数</param>
        /// </summary>
        public void m_mthLongAllFill(int p_intRow, int p_intCol)
        {
            if (p_intRow > this.Rows || p_intCol > this.Columns)
            {
                lis.client.control.MessageDialog.Show("请输入小于面板的行数与列数！", "输入了错误信息！");
            }

            //如果参数行和列为0则默认全部行和列填充
            if (p_intRow == 0)
            {
                p_intRow = this.Rows;
            }
            if (p_intCol == 0)
            {
                p_intCol = this.Columns;
            }

            string[,] strDataArr = new string[this.Rows, this.Columns];
            int intSeq = 1;
            //组建纵向填充数据到二维数组里
            for (int col = 0; col < this.Columns; col++)
            {
                if (p_intCol > col)
                {
                    for (int row = 0; row < this.Rows; row++)
                    {
                        if (p_intRow > row)
                        {
                            strDataArr[row, col] = intSeq.ToString();
                            intSeq++;
                        }
                        else
                        {
                            strDataArr[row, col] = "";
                        }

                    }
                }

            }
            intSeq = 0;
            //二维数据赋值到控件里
            for (int row2 = 0; row2 < this.Rows; row2++)
            {
                for (int col2 = 0; col2 < this.Columns; col2++)
                {
                    if (!string.IsNullOrEmpty(strDataArr[row2, col2]))
                    {
                        this.mainControls[intSeq].Text = strDataArr[row2, col2];

                    }
                    else
                    {
                        this.mainControls[intSeq].Text = "";
                    }
                    intSeq++;
                }
            }

        }


        /// <summary>
        /// 将行的显示lable为字母表示
        /// <param name="p_blnConver">是否转换为字母</param>
        /// </summary>
        public void m_mthConverRowLable(bool p_blnConver)
        {
            int intContorls = this.leftLabels.Count;
            if (p_blnConver)
            {
                //顺序转换成字母
                for (int i = 0; i < intContorls; i++)
                {
                    this.leftLabels[i].Text = ((char)(65 + i)).ToString();
                }

            }
            else
            {
                for (int i = 0; i < intContorls; i++)
                {
                    this.leftLabels[i].Text = (i + 1).ToString();
                }
            }



        }

        /// <summary>
        /// 将列的显示lable为字母表示
        /// <param name="p_blnConver">是否转换为字母</param>
        /// </summary>
        public void m_mthConverColLable(bool p_blnConver)
        {
            int intContorls = this.topLabels.Count;
            if (p_blnConver)
            {
                //顺序转换成字母
                for (int i = 0; i < intContorls; i++)
                {
                    this.topLabels[i].Text = ((char)(65 + i)).ToString();
                }
            }
            else
            {
                for (int i = 0; i < intContorls; i++)
                {
                    this.topLabels[i].Text = (i + 1).ToString();
                }
            }


        }

        /// <summary>
        /// 选中时转换行显示为字母
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_converRow_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_converRow.Checked)
            {
                this.m_mthConverRowLable(true);
            }
            else
            {
                this.m_mthConverRowLable(false);
            }
        }

        /// <summary>
        /// 选中时转换列显示为字母
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_converCol_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_converCol.Checked)
            {
                this.m_mthConverColLable(true);
            }
            else
            {
                this.m_mthConverColLable(false);
            }
        }

    }

    /// <summary>
    /// 内嵌控件
    /// </summary>
    public enum ControlType
    {
        TextBox,
        Label,
        Complex
    }

    /// <summary>
    /// 布局
    /// </summary>
    enum Position
    {
        /// <summary>
        /// 垂直的
        /// </summary>
        Vertical,
        /// <summary>
        /// 水平的
        /// </summary>
        Horizontal
    }
}