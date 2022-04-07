using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.users
{
    public partial class FrmNetCASignRecord :   FrmCommon
    {
        public FrmNetCASignRecord(string userName)
        {
            InitializeComponent();
            this.txtCAUser.Text = userName;
        }

        public string Password { get; set; }

        private void FrmNetCASignRecord_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnSave", "BtnClose" });

            this.txtPassword.Focus();
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                lis.client.control.MessageDialog.Show("密码不能为空");
                txtPassword.Focus();
                return;
            }
            if ( string.IsNullOrEmpty(txtPassword2.Text))
            {
                lis.client.control.MessageDialog.Show("确认密码不能为空");
                txtPassword2.Focus();
                return;
            }
            if ( txtPassword.Text != txtPassword2.Text)
            {
                lis.client.control.MessageDialog.Show("两次输入的密码必须一致");
                txtPassword2.Focus();
                return;
            }
            Password = txtPassword.Text;
            DialogResult=DialogResult.Yes;
            Close();
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            Password = string.Empty;
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
