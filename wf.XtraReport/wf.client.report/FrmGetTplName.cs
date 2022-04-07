using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace dcl.client.report
{
    public partial class FrmGetTplName : Form
    {
        public string sBox;
        public FrmGetTplName()
        {
            InitializeComponent();           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sBox = textEdit1.Text;
            this.Close();
        }
    }
}
