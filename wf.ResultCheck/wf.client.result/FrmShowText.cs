using System;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.result
{
    public partial class FrmShowText : FrmCommon
    {
        public DevExpress.XtraEditors.MemoEdit medFather = null;

        public event System.EventHandler OnbtnClick = null;

        public FrmShowText()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 传参调用
        /// </summary>
        /// <param name="med_out">文本控件</param>
        /// <param name="win_name">窗口名称</param>
        public FrmShowText(DevExpress.XtraEditors.MemoEdit med_out, string win_name)
        {
            InitializeComponent();

            if (med_out != null)
            {
                medFather = med_out;
                this.medShow.Text = med_out.Text;
            }

            if (!string.IsNullOrEmpty(win_name))
            {
                this.Text = win_name;
            }
        }

        private void FrmShowText_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (medFather != null)
            {
                medFather.Text = this.medShow.Text;
            }
        }

        private void FrmShowText_Load(object sender, EventArgs e)
        {
            if (OnbtnClick == null) panel1.Visible = false;
            this.medShow.Focus();
            this.medShow.Select(medShow.Text.Length, 0);
        }

        private void btnExample_Click(object sender, EventArgs e)
        {
            if (medFather != null)
            {
                medFather.Text = this.medShow.Text;
            }

            if (OnbtnClick != null)
            {
                OnbtnClick(sender, e);
            }

            if (medFather != null)
            {
                this.medShow.Text = medFather.Text;
            }
        }
    }
}
