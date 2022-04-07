using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using System.Collections;
using DevExpress.XtraTreeList;
using System.Data.SqlClient;
using dcl.client.common;

namespace dcl.client.result
{
    public partial class FrmUndoRepotReson : FrmCommon
    {
        public FrmUndoRepotReson()
        {
            InitializeComponent();
        }

        public string strRemark = "";

        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Reson.Text))
            {
                lis.client.control.MessageDialog.Show("反审原因不能为空！", "提示");
                return;
            }
            strRemark = txt_Reson.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void FrmUndoRepotReson_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
        }

    }


}
