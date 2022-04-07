using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.touchprint
{
    public partial class frmSign : Form
    {
        string SystemPassword;
        public frmSign()
        {
            InitializeComponent();
            SystemPassword = ConfigurationManager.AppSettings["SystemPassword"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请输入密码！");
                return;
            }


            if (textBox1.Text.Trim() == SystemPassword)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("密码错误，请联系管理员！");
        }
    }
}
