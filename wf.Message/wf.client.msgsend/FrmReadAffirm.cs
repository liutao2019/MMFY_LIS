﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.XtraEditors;

namespace dcl.client.msgsend
{
    public partial class FrmReadAffirm : XtraForm
    {
        internal EntityAuditInfo m_userInfo = null;

        public FrmReadAffirm()
        {
            InitializeComponent();
            this.Text = "危急值确认";

            this.Shown += new EventHandler(FrmReadAffirm_Shown);
            this.txtMsg.KeyDown += new KeyEventHandler(txtMsg_KeyDown);
        }

        void txtMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control && e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                btnAffirm_Click(null, null);

            }
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

            bool bUserCheckSuccess = true;// new lis.biz.message.MessageBiz().m_blnCheckPassWord(this.txtUserId.Text.Trim(), this.txtPwd.Text.Trim());

            //查询HIS账号验证
            //AuditInfo userInfo = new AuditInfo();
            EntityAuditInfo userInfo = new EntityAuditInfo();
            userInfo.UserId = this.txtUserId.Text.Trim();
            userInfo.Password = this.txtPwd.Text.Trim();
            userInfo.IsOnlyInsgin = false;

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
                userInfo = Audit.AuditWhenPrintImpl(userInfo);
                if (userInfo != null)
                {
                    bUserCheckSuccess = true;

                    //系统配置：危急值内部提醒处理后只取消本提醒
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_IsOnlyInsgin") == "是")
                    {
                        userInfo.IsOnlyInsgin = true;
                    }

                    m_userInfo = userInfo;

                    m_userInfo.IsSaveMsg = true;//标识保存编辑框信息

                    if (txtMsg.Text.Trim().Length > 0)//编辑信息是否不为空
                    {
                        m_userInfo.MsgContent = txtMsg.Text.Trim();//编辑框内容
                    }
                    else
                    {
                        bUserCheckSuccess = false;
                        MessageBox.Show("备注内容不能为空!");
                        txtMsg.Focus();
                        txtMsg.SelectAll();
                        return;
                    }
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
                this.txtMsg.Focus();
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
    }

    public interface IAudit
    {
        EntityAuditInfo AuditWhenPrintImpl(EntityAuditInfo userInfo);
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
                auditInfo.MsgAffirmType = "2";//1-自动确认 2-手工确认
                return auditInfo;
            }
            return null;
        }

        #endregion
    }

}
