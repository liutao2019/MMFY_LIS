using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.common;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.users
{
    public partial class FrmChangePassword : FrmCommon
    {
        public FrmChangePassword()
            : this(false)
        {

        }

        public FrmChangePassword(bool showLoginID)
        {
            InitializeComponent();
            this.sysToolBar1.CheckPower = false;
            layitemLoginID.Visibility = showLoginID?DevExpress.XtraLayout.Utils.LayoutVisibility.Always: DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        /// <summary>
        /// 取消修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (layitemLoginID.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always && this.txtLoginID.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入登录账号", "提示");
                txtLoginID.Focus();
                return;
            }

            if (txtNewPassword.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.PASSWORD_NEW_NULL, PowerMessage.BASE_TITLE);
                txtNewPassword.Focus();
                return;
            }

            if (txtNewPassword.Text.Trim() != txtAgPassword.Text.Trim())
            {
                lis.client.control.MessageDialog.Show(PowerMessage.PASSWORD_NEW_ERROR, PowerMessage.BASE_TITLE);
                txtNewPassword.Focus();
                return;
            }

            string newPassword = EncryptClass.Encrypt(txtNewPassword.Text.Trim());
            string oldPassword = EncryptClass.Encrypt(txtPassword.Text.Trim());

            EntitySysUser sysUser = new EntitySysUser();

            if (this.txtLoginID.Visible)
            {
                sysUser.LoginId = this.txtLoginID.Text;
            }
            else
            {
                sysUser.LoginId = UserInfo.loginID;
            }

            sysUser.UserPassword = newPassword;
            sysUser.OldPassword = oldPassword;

            ProxyUserManage proxy = new ProxyUserManage();

            EntityResponse result = proxy.Service.ChangePassword(sysUser);

            if (!result.Scusess)
            {
                lis.client.control.MessageDialog.Show(result.ErroMsg, PowerMessage.BASE_TITLE);
            }

            if (result.Scusess)
            {
                UserInfo.password = newPassword;
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SAVE_SUCCESS, PowerMessage.BASE_TITLE);
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void FrmChangePassword_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnSave", "BtnClose" });


            if (layitemLoginID.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
            {
                this.txtLoginID.Focus();
            }
            else
            {
                this.txtPassword.Focus();
            }
        }

    }
}
