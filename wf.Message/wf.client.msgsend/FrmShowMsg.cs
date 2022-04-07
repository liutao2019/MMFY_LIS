using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.msgsend
{
    public partial class FrmShowMsg : Form
    {
        public FrmShowMsg()
        {
            InitializeComponent();
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);
            this.Hide();
        }

        public void startShowFrm()
        {
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);

            this.Hide();
        }

        public void HideThisMsg()
        {
            if (this.Visible)
            {
                this.Hide();
            }
        }

        public void ShowThisMsg()
        {
            if (!this.Visible)
            {
                this.TopMost = true;
                this.Show();
            }
        }

        private void FrmShowMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (ReturnOKEvent != null)
            {
                ReturnOKEvent(null);
            }
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ReturnOKEvent(null);
            this.Hide();
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="dt"></param>
        public delegate void ReturnOKHandler(object sender);

        /// <summary>
        /// 事件
        /// </summary>
        public event ReturnOKHandler ReturnOKEvent;


    }
}
