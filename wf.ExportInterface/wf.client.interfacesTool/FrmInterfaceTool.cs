using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.interfacesTool
{
    public partial class FrmInterfaceTool : Form
    {
        public FrmInterfaceTool()
        {
            InitializeComponent();
        }

        private void FrmInterfaceTool_Load(object sender, EventArgs e)
        {
            txtFrequency.Text = ConfigurationManager.AppSettings["Frequency"];
            txtNumber.Text = ConfigurationManager.AppSettings["Number"];
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (btnUpload.Text == "停止")
            {
                txtFrequency.Enabled = true;
                txtNumber.Enabled = true;
                btnUpload.Enabled = true;
                btnUpload.Text = "开始";
                tmUpload.Enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(txtFrequency.Text))
            {
                MessageBox.Show("请输入频率");
                return;
            }

            if (string.IsNullOrEmpty(txtNumber.Text))
            {
                MessageBox.Show("请输入数量");
                return;
            }

            int frequency = 0;

            if (!Int32.TryParse(txtFrequency.Text, out frequency))
            {
                MessageBox.Show("请输入正确的频率");
                return;
            }

            int number = 0;

            if (!Int32.TryParse(txtNumber.Text, out number))
            {
                MessageBox.Show("请输入正确的数量");
                return;
            }

            txtFrequency.Enabled = false;
            txtNumber.Enabled = false;
            btnUpload.Enabled = false;
            btnUpload.Text = "停止";

            tmUpload.Interval = frequency * 1000;
            tmUpload.Enabled = true;
            tmUpload_Tick(sender, e);
        }

        private void tmUpload_Tick(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtNumber.Text);

            ProxyDCLInterfacesTool proxy = new ProxyDCLInterfacesTool();
            EntityResponse value = proxy.Service.UploadDCLReport(number);
            if (value.Scusess)
            {
                NameValueCollection result = value.GetResult<NameValueCollection>();
                foreach (string item in result.AllKeys)
                {
                    txtInfo.Text += string.Format("病人ID：{0} 上传{1}\r\n", item, result[item]);
                }
            }
            else
            {
                txtInfo.Text += string.Format("上传失败，消息：{0} \r\n", value.ErroMsg);
            }

        }
    }
}
