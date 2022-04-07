using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wf.ShelfPrint
{
    public partial class FrmSystem : Form
    {
        public FrmSystem()
        {
            InitializeComponent();
        }

        public string PaperQuantity { get; private set; }

        private void FrmSystem_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.97;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            if (pb.Tag.ToString().Contains("Delete"))
                SendKeys.Send("{BACKSPACE}");
            else if (pb.Tag.ToString().Contains("Empty"))
                txtInput.Text = string.Empty;
            else if (pb.Tag.ToString().Contains("Quit"))
            {
                if (MessageBox.Show("是否退出程序？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Application.Exit();
            }
            else if (pb.Tag.ToString().Contains("Confirm"))
            {
                if (string.IsNullOrEmpty(txtInput.Text))
                {
                    MessageBox.Show("请输入重置的纸张数量！");
                }
                else
                {
                    PaperQuantity = txtInput.Text;
                    this.DialogResult = DialogResult.Yes;
                }
            }
            else if (pb.Tag.ToString().Contains("Return"))
            {
                this.Close();
            }
            else
                txtInput.Text += pb.Tag.ToString();

            txtInput.Select(txtInput.Text.Length, 0);
        }
    }
}
