using System;
using System.Windows.Forms;
using dcl.entity;

namespace dcl.client.msgsend
{
    public partial class FrmReadAffirm2 : Form
    {

        internal EntityAuditInfo m_userInfo = null;

        public FrmReadAffirm2()
        {
            InitializeComponent();
            this.Text = "危急值确认";

            this.Shown += new EventHandler(FrmReadAffirm_Shown);
        }

        void FrmReadAffirm_Shown(object sender, EventArgs e)
        {
            this.txtUserId.Focus();
        }

        /// <summary>
        /// 点击确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAffirm_Click(object sender, EventArgs e)
        {

            bool bUserCheckSuccess = true;

            //查询HIS账号验证
            EntityAuditInfo userInfo = new EntityAuditInfo();

            userInfo.UserId = this.txtUserId.Text.Trim();
            userInfo.Password = this.txtPwd.Text.Trim();

            userInfo.MsgDocNum = this.txtdoc_num.Text.Trim();//临床工号
            userInfo.MsgDocName = this.txtdoc_name.Text.Trim();//临床姓名
            userInfo.MsgDepTel = this.txtdep_tel.Text.Trim();//临床电话

            userInfo.IsOnlyInsgin = false;

            //if (string.IsNullOrEmpty(userInfo.msg_doc_num) && string.IsNullOrEmpty(userInfo.msg_doc_name) && string.IsNullOrEmpty(userInfo.msg_dep_tel))
            if (string.IsNullOrEmpty(userInfo.MsgDocNum) && string.IsNullOrEmpty(userInfo.MsgDocName) && string.IsNullOrEmpty(userInfo.MsgDepTel))
            {
                MessageBox.Show("临床信息不能为空,请至少填写一项!");
                this.txtdoc_num.Focus();
                return;
            }

            if (string.IsNullOrEmpty(userInfo.UserId))
            {
                MessageBox.Show("请输入用户名!");
                this.txtUserId.Focus();
                return;
            }

            //危急值内部提醒验证方式
            string AuditType = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType");

            string strAuditType = "lis";

            if (AuditType == "OutLink")
                strAuditType = "fsaudit";

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
                string temp_doc_num = userInfo.MsgDocNum;//临床工号
                string temp_doc_name = userInfo.MsgDocName;//临床姓名
                string temp_dep_tel = userInfo.MsgDepTel;//临床电话

                userInfo = Audit.AuditWhenPrintImpl(userInfo);
                if (userInfo != null)
                {
                    bUserCheckSuccess = true;

                    //系统配置：危急值内部提醒处理后只取消本提醒
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_IsOnlyInsgin") == "是")
                    {
                        userInfo.IsOnlyInsgin = true;
                    }

                    userInfo.MsgDocNum = temp_doc_num;
                    userInfo.MsgDocName = temp_doc_name;
                    userInfo.MsgDepTel = temp_dep_tel;

                    m_userInfo = userInfo;
               
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

        private void FrmReadAffirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAffirm_Click(null, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPwd.Focus();
            }
        }

        private void txtdoc_num_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtdoc_name.Focus();
            }
        }

        private void txtdoc_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtdoc_num.Focus();
            }
        }

        private void txtdep_tel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtUserId.Focus();
            }
        }
    }
}
