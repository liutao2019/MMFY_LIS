using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;

namespace dcl.client.result
{
    public partial class FrmPatientList : FrmCommon
    {
        public FrmPatientList()
        {
            InitializeComponent();
        }

        public BindingSource BindingSource
        {
            get
            {
                return bsPatientList;
            }
            set
            {
                bsPatientList = value;
            }
        }

        private void FrmPatientList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}