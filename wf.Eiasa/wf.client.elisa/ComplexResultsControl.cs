using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.common;

namespace dcl.client.elisa
{
    public partial class ComplexResultsControl : UserControl
    {
        public ComplexResultsControl()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return txtOriginal.Text;
            }
            set
            {
                //原始数值：+0001 -0000 -1.583
                if (String.IsNullOrEmpty(value))
                    return;
                if (value.Substring(1) == "0000")
                {
                    txtOriginal.Text = "0";
                    return;
                }
                else if (value[0] == '+'&&value.IndexOf('.')==-1)
                {
                    //去掉+号，将+0001变成0.001
                    value = value.Substring(1);
                    value = value.Insert(1, ".");
                }
                    //如果有-号为-0001，就变成-0.001
                else if (value[0] == '-'&&value.IndexOf('.')==-1)
                    value = value.Insert(2, ".");

                txtOriginal.Text = value;
            }
        }

        /// <summary>
        /// 阴阳值
        /// </summary>
        public string NegOrPosText
        {
            get { return cbRes.Text; }
            set { cbRes.Text = value; }
        }

        /// <summary>
        /// OD值
        /// </summary>
        public string ODText
        {
            get { return txtOD.Text; }
            set { txtOD.Text = value; }
        }

        /// <summary>
        /// 判断阴阳性
        /// </summary>
        /// <param name="strExpress"> > >= < <= </param>
        /// <param name="value">判定值</param>
        /// <param name="nature"></param>
        public void JudgeNegOrPos(string strExpress, double value, Nature nature)
        {
            string express = String.Format(" {0} {1} {2} ", txtOriginal.Text, strExpress, value);
            bool result = Evaluator.EvaluateToBool(express);
            SetNegPos(nature, result);
        }

        /// <summary>
        /// 设置阴阳性
        /// </summary>
        /// <param name="nature"></param>
        /// <param name="result"></param>
        public void SetNegPos(Nature nature, bool result)
        {
            if (result)
                SetNegPos(nature);
            else
                SetNegPos(nature == Nature.Neg ? Nature.Pos : Nature.Neg);
        }

        /// <summary>
        /// 判断OD值
        /// </summary>
        /// <param name="express">不带参数的算术表达式</param>
        public void JudgeOD(string express)
        {
            express = express.Replace("[样本]", txtOriginal.Text);
            txtOD.Text = Evaluator.EvaluateToDouble(express).ToString();
        }

        private void SetNegPos(Nature nature)
        {
            if (nature == Nature.Pos)
            {
                txtOriginal.ForeColor = Color.Red;
                cbRes.Text = "+"; 
            }
            else if (nature==Nature.Neg)
            {
                cbRes.Text = "-";
            }
            else if (nature==Nature.feedPos)
            {
                cbRes.Text = "±";
            }
               
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            txtOD.Text = string.Empty;
            txtOriginal.ForeColor = System.Drawing.SystemColors.WindowText;
            cbRes.SelectedIndex = 0; //cbRes.Text = "-";
        }

        private void cbRes_EditValueChanged(object sender, EventArgs e)
        {
            //快捷输入
            if (cbRes.Text == "1" || cbRes.Text == "１" || cbRes.Text == "－")
            {
                cbRes.Text = "-";
            }
            else if (cbRes.Text == "2" || cbRes.Text == "２")
            {
                cbRes.Text = "+";
            }
            else if (cbRes.Text == "3" || cbRes.Text == "３")
            {
                cbRes.Text = "±";
            }
            // 阳性时改变字体颜色
            ChangeForeColorWhenPositive();
        }

        /// <summary>
        /// 阳性时改变字体颜色
        /// </summary>
        private void ChangeForeColorWhenPositive()
        {
            if (cbRes.Text == "+"||cbRes.Text=="±")
            {
               txtOD.ForeColor = txtOriginal.ForeColor = cbRes.ForeColor = Color.Red;
            }
            else
            {
                txtOD.ResetForeColor();
                txtOriginal.ResetForeColor();
                cbRes.ResetForeColor();
            }
        }
    }

    /// <summary>
    /// 定性
    /// </summary>
    public enum Nature
    {
        Unkown,
        /// <summary> 阳性 </summary>
        Pos,
        /// <summary> 阴性 </summary>
        Neg,
        /// <summary>
        /// 弱阳性
        /// </summary>
        feedPos
    }
}