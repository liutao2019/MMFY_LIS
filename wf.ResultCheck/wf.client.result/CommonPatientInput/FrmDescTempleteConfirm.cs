using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmDescTempleteConfirm : FrmCommon
    {
        public FrmDescTempleteConfirm()
        {
            InitializeComponent();
            
            SaveAsPublic = false;
        }

        /// <summary>
        /// form_load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDescTempleteConfirm_Load(object sender, EventArgs e)
        {
            sysToolBar1.OrderCustomer = true;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnClose", "BtnConfirm" });
        }

        public bool SaveAsPublic;

        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            this.SaveAsPublic = (bool)this.radioGroup1.EditValue;
            this.DialogResult = DialogResult.OK;
        }
    }
}
