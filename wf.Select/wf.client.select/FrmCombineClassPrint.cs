using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.resultquery
{
    public partial class FrmCombineClassPrint : FrmCommon
    {
        public FrmCombineClassPrint()
        {
            InitializeComponent();
        }

        public string strClass = string.Empty;

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ckA.Checked)
                strClass += ",'" + ckA.Text + "'";
            if (ckB.Checked)
                strClass += ",'" + ckB.Text + "'";
            if (ckC.Checked)
                strClass += ",'" + ckC.Text + "'";
            if (ckD.Checked)
                strClass += ",'" + ckD.Text + "'";
            if (ckE.Checked)
                strClass += ",'" + ckE.Text + "'";
            if (ckF.Checked)
                strClass += ",'" + ckF.Text + "'";
            if (ckOther.Checked)
                strClass += ",'" + ckOther.Text + "'";
            if (!string.IsNullOrEmpty(strClass))
            {
                strClass = strClass.Substring(1, strClass.Length - 1);
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("请选择组合分类！");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
