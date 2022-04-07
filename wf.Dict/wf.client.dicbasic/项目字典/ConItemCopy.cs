using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.dicbasic
{
    public partial class ConItemCopy : FrmCommon
    {
        string itm_Id;

        public ConItemCopy()
        {
            InitializeComponent();
        }

        public ConItemCopy(string itm_id, string itm_name, string itm_ecd, string itm_rep_ecd)
        {
            InitializeComponent();
            itm_Id = itm_id;
            this.txtItmName.EditValue = itm_name;
            this.txtItmEcd.EditValue = itm_ecd;
            this.txtItmRepEcd.EditValue = itm_rep_ecd;
        }

        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void txtItmName_TextChanged(object sender, EventArgs e)
        {
            this.txtPY.Text = tookit.GetSpellCode(this.txtItmName.Text);
            this.txtWB.Text = tookit.GetWBCode(this.txtItmName.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.txtItmName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("项目名称不能为空", "提示");
                this.txtItmName.Focus();
                return;
            }

            if (this.txtItmEcd.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("项目代码不能为空", "提示");
                this.txtItmEcd.Focus();
                return;
            }

            if (this.txtItmRepEcd.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("显示代码不能为空", "提示");
                this.txtItmRepEcd.Focus();
                return;
            }

            DataTable dtItem = CommonClient.CreateDT(new string[] {"itm_id","itm_name", "itm_ecd", "itm_rep_ecd", "itm_meams", "itm_py", "itm_wb" }, "dict_item");
            DataRow drItem = dtItem.NewRow();

            drItem["itm_id"] = itm_Id;
            drItem["itm_name"] = txtItmName.EditValue;
            drItem["itm_ecd"] = txtItmEcd.EditValue;
            drItem["itm_rep_ecd"] = txtItmRepEcd.EditValue;

            if (txtItmMeams.EditValue != null && txtItmMeams.EditValue.ToString().Trim() != "")
                drItem["itm_meams"] = txtItmMeams.EditValue;
            drItem["itm_py"] = txtPY.EditValue;
            drItem["itm_wb"] = txtWB.EditValue;

            dtItem.Rows.Add(drItem);

            base.doNew(dtItem);

            if (base.isActionSuccess)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtItmEcd_TextChanged(object sender, EventArgs e)
        {
            this.txtItmRepEcd.EditValue = txtItmEcd.EditValue;
        }

    }
}
