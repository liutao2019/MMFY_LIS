using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;


namespace dcl.client.dicbasic
{
    public partial class FrmPatInfoCopy : FrmCommon
    {
        public FrmPatInfoCopy()
        {
            InitializeComponent();
        }

        public string itrID = string.Empty;

        public bool isSeccess = false;

        private void FrmPatInfoCopy_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnCopy.Name, sysToolBar1.BtnClose.Name });
        }

        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.displayMember))
            {
                lis.client.control.MessageDialog.Show("请选择需要复制的仪器", "提示");
                return;
            }
            itrID = txtPatInstructment.valueMember;
            isSeccess = true;
            this.Close();
        }
    }
}
