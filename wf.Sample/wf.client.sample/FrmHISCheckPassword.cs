using System;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.sample
{
    public partial class FrmHISCheckPassword : FrmCommon
    {
        public string OperatorID = "";
        public string OperatorName = "";
        public string OperatorSftId = "";
        public string FuncInfoID = "";
        public string FuncCode = "";
        public string ModuleName = "";
        public FrmHISCheckPassword(string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public FrmHISCheckPassword(string title, string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            this.Text = title;
            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public IAudit Audit { get; set; }

        private Timer carTimer;
 
        public FrmHISCheckPassword(IAudit audit)
        {
            Audit = audit;
            InitializeComponent();
            this.txtLoginid.KeyDown += new KeyEventHandler(txtLoginid_KeyDown);

            if (ConfirmByCard.Enable)
            {
                carTimer=new Timer();
                carTimer.Enabled = true;
                carTimer.Interval = 3000;
                carTimer.Tick += carTimer_Tick;
                carTimer.Start();
            }
        }

        void carTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                carTimer.Stop();
                if (string.IsNullOrEmpty(this.txtLoginid.Text))
                {
                    ConfirmByCard confirmByCard = new ConfirmByCard();
                    if (confirmByCard.AutoReadCard()
                        && !string.IsNullOrEmpty(confirmByCard.CardData))
                    {
                        this.txtLoginid.Text = confirmByCard.CardData;
                        if (confirmByCard.FillPwdUseCardID)
                        {
                            this.txtPassword.Text = confirmByCard.CardData;
                            this.sysToolBar1.BtnConfirm.PerformClick();
                        }
                    }
                    else
                    {
                        carTimer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                carTimer.Stop();
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        //读卡数据
        void txtLoginid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.txtLoginid.Text))
                {
                    ConfirmByCard confirmByCard = new ConfirmByCard();
                    if (confirmByCard.ReadCard()
                        && !string.IsNullOrEmpty(confirmByCard.CardData))
                    {
                        this.txtLoginid.Text = confirmByCard.CardData;
                        if (confirmByCard.FillPwdUseCardID)
                        {
                            this.txtPassword.Text = confirmByCard.CardData;
                            this.sysToolBar1.BtnConfirm.PerformClick();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 确认是否有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            AuditInfo userInfo = new AuditInfo();
            userInfo.UserId = txtLoginid.Text.Trim();
            userInfo.Password = txtPassword.Text.Trim();
            //佛山市一：住院调outlink验证，门诊用系统验证 
            Audit.ShouldAuditWhenPrint = true;

            AuditInfo result = Audit.AuditWhenPrint(userInfo);

            if (result != null)
            {
                OperatorID = result.UserId;
                OperatorName = result.UserName;
                OperatorSftId = result.UserStfId;
                //暂时注释 2017-12-12
                //OperatorSftId = listUser[0].UserStfId;
                // sysToolBar1.LogMessage = "使用账号[" + txtLoginid.Text.Trim() + "]通过验证";
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                lis.client.control.MessageDialog.Show("账号错误或无权限或已停用");
                txtLoginid.Focus();
                return;
            }

        }

        private void FrmCheckPassword_Load(object sender, EventArgs e)
        {
            UserInfo.SkipPower = true;
            sysToolBar1.CheckPower = false;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
            txtLoginid.Text = "";
            txtLoginid.Cursor = Cursor.Current;
            txtLoginid.Focus();

            sysToolBar1.BtnConfirm.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            sysToolBar1.BtnClose.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
        }

        //public string LoginID { get { return txtLoginid.Text;} }

        //public string LoginName { get { return this.. .Text; } }

        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
