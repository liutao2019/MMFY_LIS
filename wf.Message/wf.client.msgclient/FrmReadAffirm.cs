using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.wcf;
using System.Configuration;
using dcl.entity;
using DevExpress.XtraEditors;

namespace dcl.client.msgclient
{
    public partial class FrmReadAffirm : XtraForm
    {
        internal EntityAuditInfo m_userInfo = null;

        string Msg_ReadConfirmType = "通用";
        string Msg_Return = "";
        /// <summary>
        /// 是否只是身份验证
        /// </summary>
        public bool IsCheckPass = false;

        public FrmReadAffirm()
        {
            InitializeComponent();
            this.Text = "危急值确认";
            IstxtMsgVisible(false);// 是否显示编辑框
            this.Shown += new EventHandler(FrmReadAffirm_Shown);

        }

        /// <summary>
        /// 带参数-是否显示编辑框
        /// </summary>
        /// <param name="bln">是否显示编辑框</param>
        public FrmReadAffirm(bool blnText)
        {
            InitializeComponent();
            IstxtMsgVisible(blnText);// 是否显示编辑框
            this.Shown += new EventHandler(FrmReadAffirm_Shown);
        }

        public FrmReadAffirm(string str)
        {
            InitializeComponent();
            Msg_Return = str;
            this.Shown += new EventHandler(FrmReadAffirm_Shown);
        }

        void FrmReadAffirm_Shown(object sender, EventArgs e)
        {
            this.sbConfirm.Click += new System.EventHandler(this.sbConfirm_Click);
            this.sbClose.Click += new System.EventHandler(this.sbClose_Click);
            this.lbBscripe.Click += new System.EventHandler(this.lbBscripe_Click);
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);


            Msg_ReadConfirmType = new ProxyObrMessage().Service.GetConfigValue("Msg_ReadConfirmType");
            //验证时的备注信息
            string ReadAffirmLabelText = ConfigurationManager.AppSettings["ReadAffirmLabelText"];
            if (!string.IsNullOrEmpty(ReadAffirmLabelText))
            {
                this.groupControl2.Text = ReadAffirmLabelText;
            }

            if (Msg_Return == "回退标本")
            {
                this.Text = "回退标本确认";
                lbcDoc.Visible = false;
                fpat_doc_id.Visible = false;
                groupControl2.Visible = false;//编辑框-不可见
                lbBscripe.Visible = false;//模板-不可见
                this.groupControl1.Visible = false;
            }
            if (IsCheckPass)
            {
                this.Text = "身份验证";
            }

            this.txtUserId.Focus();
        }

        /// <summary>
        /// 是否显示编辑框
        /// </summary>
        /// <param name="blnIstrue"></param>
        private void IstxtMsgVisible(bool blnIstrue)
        {
            if (!blnIstrue)//编辑框-是否可见
            {
                groupControl2.Visible = false;//编辑框-不可见
                lbBscripe.Visible = false;//模板-不可见
            }
        }

        private void FrmReadAffirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (groupControl2.Visible)
                {
                    txtMsg.Focus();
                }
                else
                {
                    sbConfirm_Click(null, null);
                }
            }
        }

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPwd.Focus();
            }
        }

        public void SetDocVisiable(bool docVisible)
        {
            lbcDoc.Visible = docVisible;
            fpat_doc_id.Visible = docVisible;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMsg2.Text = "复查";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMsg2.Text = "继续观察";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMsg2.Text = "其他";
        }

        public void getBscripe(String str, string type)
        {
            if (type == "16")
            {
                txtMsg.Text = str;
            }

        }

        /// <summary>
        /// 备注模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbBscripe_Click(object sender, EventArgs e)
        {
            FrmBscripe fb = new FrmBscripe(this, "16");
            fb.ShowDialog();
        }

        private void sbConfirm_Click(object sender, EventArgs e)
        {
            bool bUserCheckSuccess = true;

            EntityAuditInfo userInfo = new EntityAuditInfo();
            userInfo.UserId = this.txtUserId.Text.Trim();
            userInfo.Password = this.txtPwd.Text.Trim();

            if (string.IsNullOrEmpty(userInfo.UserId))
            {
                MessageBox.Show("请输入用户名!");
                this.txtUserId.Focus();
                return;

            }
            if (string.IsNullOrEmpty(userInfo.Password))
            {
                MessageBox.Show("请输入密码!");
                this.txtPwd.Focus();
                return;

            }
            //判断文本框是否有输入内容
            if (txtMsg.Visible && (string.IsNullOrEmpty(txtMsg.Text)))
            {
                MessageBox.Show("请输入编辑框内容!");
                this.txtMsg.Focus();
                return;
            }

            string strAuditType = ConfigurationManager.AppSettings["UserAuthType"];
            //确认角色  医生or护士
            string Verify_Role = ConfigurationManager.AppSettings["Verify_Role"];
            userInfo.UserRole = Verify_Role;
            IAudit Audit = null;

            switch (strAuditType)
            {
                case "lis"://检验身份验证
                    Audit = new LisAudit();
                    break;
                default:
                    Audit = new LisAudit();
                    break;
            }
            if (Audit != null)
            {
                userInfo = Audit.AuditWhenPrintImpl(userInfo);
                if (userInfo != null)
                {
                    bUserCheckSuccess = true;
                    m_userInfo = userInfo;

                    if (fpat_doc_id.Visible)
                    {
                        m_userInfo.IsSaveMsg = true;//标识保存编辑框信息
                        m_userInfo.MsgContent = fpat_doc_id.displayMember;
                    }

                    /*********是否保存编辑框信息********/

                    if (groupControl2.Visible)//编辑框-是否可见,若可见即保存编辑的信息
                    {
                        m_userInfo.IsSaveMsg = true;//标识保存编辑框信息

                        if (txtMsg.Text.Trim().Length > 0)//编辑信息是否不为空
                        {
                            m_userInfo.MsgContent = txtMsg.Text.Trim();//编辑框内容
                        }
                    }


                    if (groupControl1.Visible)//编辑框-是否可见,若可见即保存编辑的信息
                    {
                        m_userInfo.IsSaveMsg = true;//标识保存编辑框信息

                        if (txtMsg2.Text.Trim().Length > 0)//编辑信息是否不为空
                        {
                            m_userInfo.MsgContent = txtMsg2.Text.Trim();//编辑框内容
                        }
                    }
                    /**************End***************/
                }
                else
                {
                    bUserCheckSuccess = false;
                }
            }
            else
                bUserCheckSuccess = false;


            if (bUserCheckSuccess)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                MessageBox.Show("账户或密码错误，请重新输入!");
                this.txtUserId.Clear();
                this.txtPwd.Clear();
                this.txtUserId.Focus();
            }

        }

        private void sbClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

    }

    public interface IAudit
    {
        EntityAuditInfo AuditWhenPrintImpl(EntityAuditInfo userInfo);
        string strError { get; }
    }


    public class LisAudit : IAudit
    {
        public LisAudit()
        { }
        #region IAudit 成员

        public EntityAuditInfo AuditWhenPrintImpl(EntityAuditInfo userInfo) 
        {
            EntitySysUser eyUser = new EntitySysUser();
            eyUser.UserLoginid = userInfo.UserId;
            eyUser.UserPassword = userInfo.Password;

            ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
            List<EntitySysUser> listResult = proxyObrMsg.Service.LisCheckPassWord(eyUser);

            if (listResult.Count > 0)
            {
                EntityAuditInfo auditInfo = new EntityAuditInfo();
                auditInfo.UserId = listResult[0].UserLoginid;
                auditInfo.UserName = listResult[0].UserName;
                auditInfo.UserRole = userInfo.UserRole;
                return auditInfo;
            }
            return null;
        }

        #endregion

        #region IAudit 成员

        private string _strError = null;
        public string strError
        {
            get
            {
                return _strError;
            }
        }

        #endregion
    }
}
