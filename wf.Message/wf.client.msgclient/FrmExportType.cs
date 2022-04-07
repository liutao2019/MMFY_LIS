using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.msgclient
{
    public partial class FrmExportType : XtraForm
    {
        public string strType { get; set; }

        public FrmExportType()
        {
            InitializeComponent();
            this.sbtnConfirm.Click += btnOk_Click;
            this.sbtnCancel.Click += btnClose_Click;
        }

        private void FrmExportType_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rbtA.Checked)
            {
                strType = "0";//危机值记录
            }
            else if (rbtB.Checked)
            {
                strType = "1";//MDR预警记录
            }
            else if (rbtC.Checked)
            {
                strType = "2";//其他预警记录
            }

            this.Close();
        }

        
    }
}
