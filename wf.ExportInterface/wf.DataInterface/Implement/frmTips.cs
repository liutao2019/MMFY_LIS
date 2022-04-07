using System;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    partial class frmTips : Form
    {
        public frmTips()
        {
            InitializeComponent();
            //this.richTextBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string rtf = this.richTextBox1.Rtf;
            //this.richTextBox1.Rtf = Resources.strHelpCommand;
            this.richTextBox1.ReadOnly = true;
        }
    }
}
