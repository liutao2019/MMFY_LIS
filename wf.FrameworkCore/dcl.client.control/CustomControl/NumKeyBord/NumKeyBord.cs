using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.control
{
    public partial class NumKeyBord : UserControl
    {
        //定义委托
        public delegate void NumKeyBordClickHandle(object sender, EventArgs e);
        //定义事件
        public event NumKeyBordClickHandle NumKeyBordClick;

        #region Property


        private Color _keyBordBackColor = Color.LightCyan;
        [Browsable(true)]
        [Description("控件背景颜色")]
        public Color KeyBordBackColor
        {
            get { return _keyBordBackColor; }
            set
            {
                _keyBordBackColor = value;
                this.tableLayoutPanel1.BackColor = _keyBordBackColor;
                base.Invalidate();
            }
        }

        private Color _keyForeColor = Color.Black;
        [Browsable(true)]
        [Description("数字字体颜色")]
        public Color KeyForeColor
        {
            get { return _keyForeColor; }
            set
            {
                _keyForeColor = value;
                foreach (Control ctl in tableLayoutPanel1.Controls)
                {
                    if (ctl is Button)
                    {
                        (ctl as Button).ForeColor = _keyForeColor;
                    }
                }
                base.Invalidate();
            }
        }

        private Control _targetControl;
        [Browsable(true)]
        [Description("接收数字的控件")]
        public Control TargetControl
        {
            get { return _targetControl; }
            set
            {
                _targetControl = value;
                base.Invalidate();
            }
        }

        #endregion

        #region Constructer

        public NumKeyBord()
        {
            InitializeComponent();
            InitKeyboard();
        }

        #endregion

        private void InitKeyboard()
        {
            foreach (Control ctl in tableLayoutPanel1.Controls)
            {
                if (ctl is Button)
                {
                    (ctl as Button).ForeColor = Color.Black;
                    (ctl as Button).Click += btnNum_Click;
                }
            }
        }

        private void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            string str = btn.Text;
            if (str == "打印")
            {
                NumKeyBordClick?.Invoke(btn, e);

                this.Hide();
                this.Dispose();
                return;
            }
            else if (str == "<")
            {
                if (this._targetControl.Text.Length > 0)
                    _targetControl.Text = _targetControl.Text.Substring(0, _targetControl.Text.Length - 1);
            }
            else
            {
                this.TargetControl.Text += str;
            }
        }


        private void TopLevelControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.Bounds.Contains(e.X, e.Y))
            {
                this.Hide();
                this.Dispose();
            }
        }


    }
}
