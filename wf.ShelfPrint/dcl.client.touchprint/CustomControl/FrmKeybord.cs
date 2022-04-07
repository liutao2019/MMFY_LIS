using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wf.ShelfPrint.CustomControl
{
    public partial class FrmKeybord : Form
    {

        //定义委托
        public delegate void NumKeyBordClickHandle(object sender);
        //定义事件
        public event NumKeyBordClickHandle NumKeyBordClick;


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
        string ImagePath = Application.StartupPath + @"\JPG\";

        public FrmKeybord()
        {
            InitializeComponent();
            this.Load += FrmKeybord_Load;
            pbUpper.Click += PbUpper_Click;
        }
        private void FrmKeybord_Load(object sender, EventArgs e)
        {
            GenEvent();
        }



        bool Upper = true;
        private void PbUpper_Click(object sender, EventArgs e)
        {
            Upper = !Upper;

            foreach (Control cl in plEnglish.Controls)
            {
                if (cl is Button)
                {
                    if (Upper)
                        cl.Text = cl.Text.ToUpper();
                    else
                        cl.Text = cl.Text.ToLower();
                }
            }
        }

        private void GenEvent()
        {
            foreach (Control cl in plEnglish.Controls)
            {
                if (cl.Name == "pbUpper")
                    continue;

                cl.Click += Cl_Click;
            }

            foreach (Control cl in plNumber.Controls)
            {
                cl.Click += Cl_Click;
            }
        }


        private void Cl_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                if ((sender as PictureBox).Tag.ToString().Contains("Delete"))
                {
                    if (this._targetControl.Text.Length > 0)
                        _targetControl.Text = _targetControl.Text.Substring(0, _targetControl.Text.Length - 1);
                }
            }

            if (sender is Button)
            {
                Button btn = sender as Button;
                if (sender == null)
                    return;

                if (btn.Tag.ToString().Contains("Empty"))
                {
                    _targetControl.Text = string.Empty;
                }
                else if (btn.Tag.ToString().Contains("Confirm"))
                {
                    NumKeyBordClick?.Invoke(btn);
                    this.Hide();
                }
                else
                {
                    _targetControl.Text += btn.Text.ToString();
                }
            }
            (_targetControl as TextBox)?.Select(_targetControl.Text.Length, 0);
        }



        /// <summary>
        /// 是否是英文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNatural(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za]+$");
            return reg1.IsMatch(str);
        }

    }
}
