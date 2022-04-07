using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace lis.client.control
{
    public partial class frmMessageDialog : DevExpress.XtraEditors.XtraForm
    {
        Timer timer = null;

        private bool autoclose = false;
        MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;

        public frmMessageDialog(string message, string caption, MessageBoxButtons buttons, decimal timeClose)
        {
            InitializeComponent();

            this.Text = caption;

            int initLblHeight = this.lblMsg.Height;

            this.lblMsg.Text = message;

            int widthIncrease = this.lblMsg.Width - (300 - 75);

            if (widthIncrease > 0)
            {
                this.Width += widthIncrease;
            }

            int heightIncrease = this.lblMsg.Height - initLblHeight;
            this.Height += heightIncrease;

            this.btns = buttons;

            VisibleDelay = timeClose;
        }

        public bool AutoClose
        {
            get
            {
                return AutoClose;
            }
            set
            {
                autoclose = value;
                if (value == true)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.pnlMain.LookAndFeel.UseDefaultLookAndFeel = false;
                    this.pnlMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                    this.pnlMain.BackColor = Color.FromArgb(255, 255, 192);
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                    this.pnlMain.LookAndFeel.UseDefaultLookAndFeel = true;
                    this.pnlMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
                }
            }
        }


        public decimal VisibleDelay = 1.2m;
        MessageBoxButtons btns;

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        public frmMessageDialog(string message, string caption, MessageBoxButtons buttons)
            : this(message, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        {

        }

        public frmMessageDialog(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            string[] strMessage = message.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (strMessage.Length > 20)
            {
                message = string.Empty;
                for (int i = 0; i < 20; i++)
                {
                    message += strMessage[i] + "\r\n";
                }
                message += "......\r\n\r\n";
                message += strMessage[strMessage.Length - 1];
            }

            //if (message.Length > 100)
            //{
            //    message = "...." + message.Substring(message.Length - 100, message.Length);
            //}
            InitializeComponent();

            this.Text = caption;

            int initLblHeight = this.lblMsg.Height;

            this.lblMsg.Text = message;

            int widthIncrease = this.lblMsg.Width - (300 - 75);

            if (widthIncrease > 0)
            {
                this.Width += widthIncrease;
            }

            int heightIncrease = this.lblMsg.Height - initLblHeight;
            this.Height += heightIncrease;

            this.btns = buttons;
            this.defaultButton = defButton;
            //MessageBox.Show("", "", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);
        }

        public void MessageDialog_Load(object sender, EventArgs e)
        {
            //pnlMain.HorizontalScroll.Visible = true;
            if (this.autoclose)
            {
                this.pnlButtonContainer.Visible = false;
                this.Height = this.Height - this.pnlButtonContainer.Height;
                AnimateWindow(base.Handle, 100, 0x20010);
            }
            else
            {
                this.SuspendLayout();
                SetButtons();
                //this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.ResumeLayout();
            }
        }

        private void SetButtons()
        {

            if (this.btns == MessageBoxButtons.OK)
            {
                this.pnl_ok.Dock = DockStyle.Fill;
                this.pnl_ok.Visible = true;
                AlignPanelControl(this.pnl_ok);
                //MessageBoxButtons如果是OK并且默认按钮是Button3则不获取焦点（标本登记提示未签收不获取焦点）
                if (this.defaultButton == MessageBoxDefaultButton.Button3)
                {
                    this.ActiveControl = pnlMain;
                }
            }
            else if (this.btns == MessageBoxButtons.YesNo)
            {
                this.pnl_yesno.Dock = DockStyle.Fill;
                this.pnl_yesno.Visible = true;
                AlignPanelControl(this.pnl_yesno);

                if (this.defaultButton == MessageBoxDefaultButton.Button2)
                {
                    this.ActiveControl = this.btn_yesno_no;
                    this.btn_yesno_no.Focus();
                }
            }
            else if (this.btns == MessageBoxButtons.YesNoCancel)
            {
                this.pnl_yesnocancel.Dock = DockStyle.Fill;
                this.pnl_yesnocancel.Visible = true;
                AlignPanelControl(this.pnl_yesnocancel);

                if (this.defaultButton == MessageBoxDefaultButton.Button2)
                {
                    this.ActiveControl = this.btn_yesnocancel_no;
                    this.btn_yesnocancel_no.Focus();
                }
                else if (this.defaultButton == MessageBoxDefaultButton.Button3)
                {
                    this.ActiveControl = this.btn_yesnocancel_cancel;
                    this.btn_yesnocancel_cancel.Focus();
                }
            }
            else if (this.btns == MessageBoxButtons.OKCancel)
            {
                this.pnl_okcancel.Dock = DockStyle.Fill;
                this.pnl_okcancel.Visible = true;
                AlignPanelControl(this.pnl_okcancel);

                if (this.defaultButton == MessageBoxDefaultButton.Button2)
                {
                    this.ActiveControl = this.btn_okcancel_cancel;
                    this.btn_okcancel_cancel.Focus();
                }
            }
        }

        /// <summary>
        /// 对齐界面控件
        /// </summary>
        /// <param name="parent"></param>
        private void AlignPanelControl(Control parent)
        {
            int subCtrlCount = parent.Controls.Count;

            int margin = 10;

            int subControlTotalWidth = 0;

            bool bNeedSpace = false;
            foreach (Control subControl in parent.Controls)
            {
                subControlTotalWidth += subControl.Width;
                if (bNeedSpace)
                {
                    subControlTotalWidth += margin;
                }
                bNeedSpace = true;
            }

            int currentX = (parent.Width - subControlTotalWidth) / 2;

            bNeedSpace = false;
            for (int i = subCtrlCount - 1; i >= 0; i--)
            {
                Control subControl = parent.Controls[i];

                if (bNeedSpace)
                {
                    currentX += margin;
                }

                subControl.Left = currentX;
                currentX += subControl.Width;

                bNeedSpace = true;
            }

            //foreach (Control subControl in parent.Controls)
            //{
            //    subControl.Left = currentX;
            //    currentX += subControl.Width;
            //    if (bNeedSpace)
            //    {
            //        currentX += margin;
            //    }
            //    bNeedSpace = true;
            //}
        }

        public new DialogResult ShowDialog()
        {
            DialogResult result;
            if (this.autoclose)
            {
                timer = new Timer();
                timer.Interval = (int)(this.VisibleDelay * 1000);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                result = base.ShowDialog();
            }
            else
            {
                result = base.ShowDialog();
            }
            return result;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void frmMessageDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Dispose();
            }
        }

        private void btn_yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btn_no_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
