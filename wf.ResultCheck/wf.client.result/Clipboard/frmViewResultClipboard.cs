using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.result.PatControl
{
    public partial class frmViewResultClipboard : FrmCommon
    {
        public frmViewResultClipboard()
        {
            InitializeComponent();
        }

        private void frmViewResultClipboard_Load(object sender, EventArgs e)
        {
            this.gridControl1.DataSource = ResultClipboard.Current.resulto;
        }
    }
}
