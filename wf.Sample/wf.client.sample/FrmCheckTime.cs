using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;

namespace dcl.client.sample
{
    public partial class FrmCheckTime : FrmCommon
    {
        public DateTime SignTime { get; set; }
        public bool Cannel { get; set; }
        public FrmCheckTime(DateTime defaultTime)
        {
            InitializeComponent();
            SetTime(defaultTime);
        }
  
        /// <summary>
        /// 确认是否有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            SignTime = this.dateEdit1.DateTime;
            this.Hide();
            Cannel = false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cannel = true;
        }

        private void FrmCheckPassword_Load(object sender, EventArgs e)
        {
            sysToolBar1.CheckPower = false;
            UserInfo.SkipPower = true;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
           // SetTime(DateTime.Now);
        }

        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        internal void SetTime(DateTime dateTime)
        {
            this.dateEdit1.EditValue = dateTime;
        }

     

        private void dateEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
