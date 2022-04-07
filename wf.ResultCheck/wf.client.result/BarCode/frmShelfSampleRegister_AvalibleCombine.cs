using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.entity;

namespace dcl.client.result
{
    public partial class frmShelfSampleRegister_AvalibleCombine : XtraForm
    {
        frmShelfSampleRegister parentForm;
        public frmShelfSampleRegister_AvalibleCombine(frmShelfSampleRegister parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        public void LoadCombine(List<EntityDicCombine> listAvalibleCombine)
        {
            this.gridControlLeft.DataSource = listAvalibleCombine;
        }

        private void frmShelfSampleRegister_AvalibleCombine_Load(object sender, EventArgs e)
        {

        }

        private void frmShelfSampleRegister_AvalibleCombine_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void frmShelfSampleRegister_AvalibleCombine_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void gridViewLeft_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = this.gridViewLeft.GetFocusedDataRow();
            if (dr != null)
            {
                string com_id = dr["com_id"].ToString();
                string com_name = dr["com_name"].ToString();

                Dictionary<string, string> selectCombine = null;
                if (this.parentForm.txtCombineEdit.Tag == null)
                {
                    selectCombine = new Dictionary<string, string>();
                    this.parentForm.txtCombineEdit.Tag = selectCombine;
                }
                else
                {
                    selectCombine = this.parentForm.txtCombineEdit.Tag as Dictionary<string, string>;
                }

                selectCombine.Add(com_id, com_name);
                if (this.parentForm.txtCombineEdit.Text == string.Empty)
                {
                    this.parentForm.txtCombineEdit.Text = com_name;
                }
                else
                {
                    this.parentForm.txtCombineEdit.Text += " + " + com_name;
                }

                List<EntityDicCombine> listAvaCombine = this.parentForm.GetAvalibleCombine();
                this.gridControlLeft.DataSource = listAvaCombine;

            }
        }
    }
}
