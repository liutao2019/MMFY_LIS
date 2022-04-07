using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace dcl.client.sample
{
    public partial class FrmBCSearch : DevExpress.XtraEditors.XtraForm
    {
        public FrmBCSearch()
        {
            InitializeComponent();
        }

        public string DeptCode
        {
            get { return this.bcSearchControl1.DeptCode; }
            set
            {
                bcSearchControl1.DeptCode = value;
            }
        }
    }
}