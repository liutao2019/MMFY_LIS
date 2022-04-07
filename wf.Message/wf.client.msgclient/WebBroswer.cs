using System;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.msgclient
{
    public partial class WebBroswer : Form
    {
        public WebBroswer()
        {
            InitializeComponent();
        }

        public string URL { get; set; }

        /// <summary>
        /// 是否显示处理按钮
        /// </summary>
        public bool IsShowAudit { get; set; }

        /// <summary>
        ///  危急值ID
        /// </summary>
        public string msg_id { get; set; }

        public delegate void OndelegEvent(object sender, EventArgs e);

        public OndelegEvent dgEvent { get;set;}

        /// <summary>
        /// 病人pat_id
        /// </summary>
        public string pat_id { get; set; }

        private void WebBroswer_Load(object sender, EventArgs e)
        {
            changeBtnText(false);
            this.btnAudit.Enabled = false;
            if (IsShowAudit)
            {
                this.panel1.Visible = true;
            }
            else
            {
                this.panel1.Visible = false;//不显示
            }

            this.webBrowser1.Url = new Uri(URL);

            if (IsShowAudit)
            {
                this.btnAudit.Enabled =true;
            }
        }
        /// <summary>
        /// 点击处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                //验证用户,显示编辑框
                FrmReadAffirm frmRA = new FrmReadAffirm(true);

                if (frmRA.ShowDialog() != DialogResult.Yes)
                {
                    return;
                }

                //AuditInfo CheckerInfo = frmRA.m_userInfo;
                EntityAuditInfo CheckerInfo = frmRA.m_userInfo;

                if (!string.IsNullOrEmpty(pat_id))
                {
                    //ProxyMessage proxy = new ProxyMessage();
                    //proxy.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                    //proxy.Service.RefreshDeptMessage();
                    ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                    proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                    proxyObrMsg.Service.RefreshDeptMessage();

                    changeBtnText(true);//处理成功改变状态和名称

                    if (dgEvent != null)
                    {
                        dgEvent(null,null);
                    }
                }
                else
                {
                    MessageBox.Show("error_462:处理失败pat_id为NULL");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error_461:"+ex.Message);
            }
        }

        /// <summary>
        /// 改变按钮文本
        /// </summary>
        /// <param name="bln">是否处理成功</param>
        private void changeBtnText(bool bln)
        {
            if (bln)
            {
                this.btnAudit.Text = "已处理成功";
                this.btnAudit.Enabled = false;//只处理一次
            }
            else
            {
                this.btnAudit.Text = "点击处理";
            }
        }
    }
}
