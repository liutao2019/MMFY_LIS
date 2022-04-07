using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;
using lis.client.control;
using dcl.common;
using dcl.client.common;

namespace wf.client.reagent
{
    public partial class FrmReaCheckPassword : FrmCommon
    {
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }

        //操作人工号(输入码)
        public string OperatorSftId { get; set; }
        public string FuncInfoID { get; set; }
        public string FuncCode { get; set; }
        public string ModuleName { get; set; }
        public bool Power { get; set; }

        public string PowerName { get; set; }
        public string Pat_i_code { get; set; }
        public string PassWord { get; set; }
        public string ReturnReason { get; set; }
        public FrmReaCheckPassword(string funcCode)
        {
            InitializeComponent();
            FuncCode = funcCode;
        }

        public FrmReaCheckPassword(string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public FrmReaCheckPassword(string title, string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            this.Text = title;
            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public FrmReaCheckPassword(string title, string funcInfoID, string funcCode, string moduleName, string powerName)
        {
            InitializeComponent();

            this.Text = title;
            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
            PowerName = powerName;
        }


        public FrmReaCheckPassword()
        {
            InitializeComponent();
        }

        public EnumOperationCode operationCode = EnumOperationCode.Unspecified;

        ProxySysUserInfo proxyUser = new ProxySysUserInfo(); 
        /// <summary>
        /// 确认是否有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoginid.Text.Trim()))
            {
                MessageDialog.Show("请输入账号！");
                return;
            }

            EntityUserQc userQc = new EntityUserQc();


            userQc.LoginId = txtLoginid.Text.Trim();
            userQc.Password = EncryptClass.Encrypt(txtPassword.Text.Trim());

            //使用admin账户时只验证用户名和密码 ??
            if (txtLoginid.Text.Trim().ToLower() != "admin")
            {
                if (FuncInfoID != "")
                {
                    userQc.FuncId = FuncInfoID;
                }

                if (FuncCode != "")
                {
                    userQc.FuncCode = FuncCode;
                }

                if (ModuleName != "")
                {
                    userQc.ModuleName = ModuleName;
                }
            }

            List<EntitySysUser> listUser = proxyUser.Service.SysUserQuery(userQc);


            if (listUser.Count > 0)
            {
                EntitySysUser sysUser = listUser[0];

                OperatorID = sysUser.UserLoginid;
                OperatorName = sysUser.UserName;
                OperatorSftId = sysUser.UserIncode;
                Pat_i_code = txtPat_i_code.valueMember;

                if (UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是" && PowerName != string.Empty)
                {
                    if (txtLoginid.Text.Trim().ToLower() == "admin")
                    {
                        Power = true;
                    }
                    else
                    {
                        EntityUserQc otherUserQc = new EntityUserQc();
                        otherUserQc.LoginId = txtLoginid.Text.Trim();
                        otherUserQc.ModuleName = PowerName;
                        List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);
                        if (listSelUser.Count > 0)
                            Power = true;
                    }
                }
               

                this.isActionSuccess = true;
                sysToolBar1.LogMessage = "使用账号[" + txtLoginid.Text.Trim() + "]通过验证";
                this.PassWord = this.txtPassword.Text.TrimEnd();
                this.ReturnReason = selectDicReaReturn1.displayMember;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageDialog.Show("账号错误或无权限或已停用");
                this.txtPassword.Text = string.Empty;
                Pat_i_code = string.Empty;
                this.ReturnReason = string.Empty;
                txtLoginid.Focus();
                return;
            }
        }

        public bool Valid(string Loginid,string Password)
        {
            txtPat_i_code.SetFilter(txtPat_i_code.getDataSource().FindAll(i => i.UserType == "检验组"));

            if (UserInfo.GetSysConfigValue("OperationVerifyLockOnCurrentUser") == "是")
                this.txtPat_i_code.SelectByID(UserInfo.loginID);

            EntityUserQc userQc = new EntityUserQc();

            userQc.LoginId = Loginid.Trim();
            userQc.Password = EncryptClass.Encrypt(Password.Trim());
            if (string.IsNullOrEmpty(userQc.LoginId))
            {
                MessageDialog.Show("请输入账号！");
                return false; 
            }

            //使用admin账户时只验证用户名和密码
            if (Loginid.Trim().ToLower() != "admin")
            {
                if (FuncInfoID != "")
                {
                    userQc.FuncId = FuncInfoID;
                }

                if (FuncCode != "")
                {
                    userQc.FuncCode = FuncCode;
                }

                if (ModuleName != "")
                {
                    userQc.ModuleName = ModuleName;
                }
            }

            List<EntitySysUser> listUser = proxyUser.Service.SysUserQuery(userQc);
            if (listUser.Count > 0)
            {
                EntitySysUser sysUser = listUser[0];
                                                 
                OperatorID = sysUser.UserLoginid;
                OperatorName = sysUser.UserName;
                OperatorSftId = sysUser.UserIncode;
                Pat_i_code = txtPat_i_code.valueMember;

                if (UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是" && PowerName != string.Empty)
                {
                    if (Loginid.Trim().ToLower() == "admin")
                    {
                        Power = true;
                    }
                    else
                    {
                        EntityUserQc otherUserQc = new EntityUserQc();
                        otherUserQc.LoginId = Loginid.Trim();
                        otherUserQc.ModuleName = PowerName;
                        List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);

                        if (listSelUser.Count > 0)
                            Power = true;
                    }
                }
                this.isActionSuccess = true;
                this.PassWord = Password;
                return true;
            }
            else
            {
                MessageDialog.Show("账号错误或无权限或已停用");
                Pat_i_code = string.Empty;
                return false;
            }
        }



        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReaCheckPassword_Load(object sender, EventArgs e)
        {
            labelControl1.Text = "身份帐号";
            labelControl2.Text = "身份密码";
            if (operationCode == EnumOperationCode.Report)
            {
                labelControl1.Text = LocalSetting.Current.Setting.ReportWord + "帐号";
                labelControl2.Text = LocalSetting.Current.Setting.ReportWord + "密码";
            }

            PassWord = string.Empty;
            UserInfo.SkipPower = true;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
            sysToolBar1.BtnConfirm.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            sysToolBar1.BtnClose.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            sysToolBar1.CheckPower = false;
            if (UserInfo.GetSysConfigValue("OperationVerifyLockOnCurrentUser") == "是")
            {
                this.txtPat_i_code.SelectByID(UserInfo.loginID);

                this.txtLoginid.Focus();
                this.ActiveControl = this.txtLoginid;
            }
            else
            {

            }

            //一审、取消一审
            if (operationCode == EnumOperationCode.Audit
                || operationCode == EnumOperationCode.UndoAudit)
            {
                string cfg = UserInfo.GetSysConfigValue("Audit_First_UserVerifyUseCurrentLoginName");

                if (cfg == "处理者为当前用户")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
                else if (cfg == "处理者为当前用户并锁定")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtLoginid.Enabled = false;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
            }
            //二审、取消二审
            else if (operationCode == EnumOperationCode.Report
                || operationCode == EnumOperationCode.UndoReport)
            {
                string cfg = UserInfo.GetSysConfigValue("Audit_Second_UserVerifyUseCurrentLoginName");

                if (cfg == "处理者为当前用户")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
                else if (cfg == "处理者为当前用户并锁定")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtLoginid.Enabled = false;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
            }

            //报告(二审)签名时，签名弹窗是否显示一审人
            if (ConfigHelper.GetSysConfigValueWithoutLogin("report_checkform_showlPat_i_code") == "是")
            {
                lblPat_i_code.Visible = true;
                txtPat_i_code.Visible = true;
            }
            else
            {
                lblPat_i_code.Visible = false;
                txtPat_i_code.Visible = false;
            }

            //报告(二审)时允许修改一审人(检验者)
            if (ConfigHelper.GetSysConfigValueWithoutLogin("report_Allowedit_auditercode") == "是")
            {
                txtPat_i_code.Readonly = false;
            }
            else
            {
                txtPat_i_code.Readonly = true;
            }

            UserInfo.SkipPower = false;
            txtPat_i_code.SetFilter(txtPat_i_code.getDataSource().FindAll(i => i.UserType == "检验组"));
        }

        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
