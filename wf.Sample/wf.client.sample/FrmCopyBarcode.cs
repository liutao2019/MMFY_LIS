using dcl.client.frame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.sample
{
    public partial class FrmCopyBarcode : FrmCommon
    {
        public string text = "";
        public FrmCopyBarcode(string strtext)
        {
            InitializeComponent();
            text = strtext;
        }

        private void FrmCopyBarcode_Load(object sender, EventArgs e)
        {
            txtMessageContent.Text = text;
        }
    }
}
