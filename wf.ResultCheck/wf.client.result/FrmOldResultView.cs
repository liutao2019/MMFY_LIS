using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.result
{
    public partial class FrmOldResultView : FrmCommon
    {
        public object dtSource
        {
            set
            {
                this.bindingSource1.DataSource = value;
            }
        }

        public FrmOldResultView()
        {
            InitializeComponent();
        }

        private void FrmOldResultView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
