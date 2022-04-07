namespace wf.ClientEntrance
{
    partial class FrmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            AnimatorNS.Animation animation1 = new AnimatorNS.Animation();
            this.pnLogo = new System.Windows.Forms.Panel();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.xRails_LabelError = new Lis.CustomControls.XRails_Label();
            this.lblVersion = new Lis.CustomControls.XRails_Label();
            this.pnSubmitBtn = new System.Windows.Forms.Panel();
            this.btnCancle = new Lis.CustomControls.MetrolButton();
            this.btnLogin = new Lis.CustomControls.MetrolButton();
            this.labLoading = new Lis.CustomControls.XRails_Label();
            this.txtPassword = new Lis.CustomControls.XRails_TextBox();
            this.txtLoginId = new Lis.CustomControls.XRails_TextBox();
            this.xRails_TitleLabel1 = new XLis.CustomControls.XRails_TitleLabel();
            this.Animator = new AnimatorNS.Animator(this.components);
            this.pnLogin.SuspendLayout();
            this.pnSubmitBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnLogo
            // 
            this.pnLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnLogo.BackgroundImage")));
            this.pnLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Animator.SetDecoration(this.pnLogo, AnimatorNS.DecorationType.None);
            this.pnLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLogo.Location = new System.Drawing.Point(0, 0);
            this.pnLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnLogo.Name = "pnLogo";
            this.pnLogo.Size = new System.Drawing.Size(484, 618);
            this.pnLogo.TabIndex = 11;
            // 
            // pnLogin
            // 
            this.pnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(61)))));
            this.pnLogin.Controls.Add(this.xRails_LabelError);
            this.pnLogin.Controls.Add(this.lblVersion);
            this.pnLogin.Controls.Add(this.pnSubmitBtn);
            this.pnLogin.Controls.Add(this.labLoading);
            this.pnLogin.Controls.Add(this.txtPassword);
            this.pnLogin.Controls.Add(this.txtLoginId);
            this.pnLogin.Controls.Add(this.xRails_TitleLabel1);
            this.Animator.SetDecoration(this.pnLogin, AnimatorNS.DecorationType.None);
            this.pnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLogin.Location = new System.Drawing.Point(484, 0);
            this.pnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(484, 618);
            this.pnLogin.TabIndex = 12;
            // 
            // xRails_LabelError
            // 
            this.xRails_LabelError.AutoSize = true;
            this.xRails_LabelError.BackColor = System.Drawing.Color.Transparent;
            this.xRails_LabelError.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Animator.SetDecoration(this.xRails_LabelError, AnimatorNS.DecorationType.None);
            this.xRails_LabelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.xRails_LabelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(118)))), ((int)(((byte)(127)))));
            this.xRails_LabelError.Location = new System.Drawing.Point(57, 454);
            this.xRails_LabelError.Name = "xRails_LabelError";
            this.xRails_LabelError.Size = new System.Drawing.Size(12, 15);
            this.xRails_LabelError.TabIndex = 22;
            this.xRails_LabelError.Text = "_";
            this.xRails_LabelError.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Animator.SetDecoration(this.lblVersion, AnimatorNS.DecorationType.None);
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(118)))), ((int)(((byte)(127)))));
            this.lblVersion.Location = new System.Drawing.Point(281, 572);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(90, 15);
            this.lblVersion.TabIndex = 19;
            this.lblVersion.Text = "版本 2018.10.11";
            // 
            // pnSubmitBtn
            // 
            this.pnSubmitBtn.BackColor = System.Drawing.Color.Transparent;
            this.pnSubmitBtn.Controls.Add(this.btnCancle);
            this.pnSubmitBtn.Controls.Add(this.btnLogin);
            this.Animator.SetDecoration(this.pnSubmitBtn, AnimatorNS.DecorationType.None);
            this.pnSubmitBtn.Location = new System.Drawing.Point(61, 378);
            this.pnSubmitBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnSubmitBtn.Name = "pnSubmitBtn";
            this.pnSubmitBtn.Size = new System.Drawing.Size(371, 73);
            this.pnSubmitBtn.TabIndex = 21;
            // 
            // btnCancle
            // 
            this.btnCancle.Active1 = System.Drawing.Color.LightCoral;
            this.btnCancle.Active2 = System.Drawing.Color.LightCoral;
            this.btnCancle.BackColor = System.Drawing.Color.Transparent;
            this.Animator.SetDecoration(this.btnCancle, AnimatorNS.DecorationType.None);
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancle.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancle.ForeColor = System.Drawing.Color.White;
            this.btnCancle.Inactive1 = System.Drawing.Color.RosyBrown;
            this.btnCancle.Inactive2 = System.Drawing.Color.RosyBrown;
            this.btnCancle.Location = new System.Drawing.Point(210, 10);
            this.btnCancle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Radius = 5;
            this.btnCancle.Size = new System.Drawing.Size(158, 54);
            this.btnCancle.Stroke = false;
            this.btnCancle.StrokeColor = System.Drawing.Color.Gray;
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "退出";
            this.btnCancle.Transparency = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Active1 = System.Drawing.Color.DarkRed;
            this.btnLogin.Active2 = System.Drawing.Color.DarkRed;
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.Animator.SetDecoration(this.btnLogin, AnimatorNS.DecorationType.None);
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Inactive1 = System.Drawing.Color.Brown;
            this.btnLogin.Inactive2 = System.Drawing.Color.Brown;
            this.btnLogin.Location = new System.Drawing.Point(3, 10);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Radius = 5;
            this.btnLogin.Size = new System.Drawing.Size(158, 54);
            this.btnLogin.Stroke = false;
            this.btnLogin.StrokeColor = System.Drawing.Color.Gray;
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登录";
            this.btnLogin.Transparency = false;
            // 
            // labLoading
            // 
            this.labLoading.AutoSize = true;
            this.labLoading.BackColor = System.Drawing.Color.Transparent;
            this.labLoading.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Animator.SetDecoration(this.labLoading, AnimatorNS.DecorationType.None);
            this.labLoading.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(118)))), ((int)(((byte)(127)))));
            this.labLoading.Image = ((System.Drawing.Image)(resources.GetObject("labLoading.Image")));
            this.labLoading.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labLoading.Location = new System.Drawing.Point(60, 404);
            this.labLoading.Name = "labLoading";
            this.labLoading.Size = new System.Drawing.Size(132, 15);
            this.labLoading.TabIndex = 20;
            this.labLoading.Text = "    正在连接，请稍后...";
            this.labLoading.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labLoading.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(48)))), ((int)(((byte)(67)))));
            this.txtPassword.ColorBordersOnEnter = true;
            this.Animator.SetDecoration(this.txtPassword, AnimatorNS.DecorationType.None);
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(131)))), ((int)(((byte)(140)))));
            this.txtPassword.Image = ((System.Drawing.Image)(resources.GetObject("txtPassword.Image")));
            this.txtPassword.Location = new System.Drawing.Point(61, 298);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.MaxLength = 32767;
            this.txtPassword.Multiline = false;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ReadOnly = false;
            this.txtPassword.ShortcutsEnabled = true;
            this.txtPassword.ShowBottomBorder = true;
            this.txtPassword.ShowTopBorder = true;
            this.txtPassword.Size = new System.Drawing.Size(371, 54);
            this.txtPassword.TabIndex = 17;
            this.txtPassword.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Watermark = "密码";
            this.txtPassword.WatermarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(120)))), ((int)(((byte)(129)))));
            // 
            // txtLoginId
            // 
            this.txtLoginId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(48)))), ((int)(((byte)(67)))));
            this.txtLoginId.ColorBordersOnEnter = true;
            this.Animator.SetDecoration(this.txtLoginId, AnimatorNS.DecorationType.None);
            this.txtLoginId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(131)))), ((int)(((byte)(140)))));
            this.txtLoginId.Image = ((System.Drawing.Image)(resources.GetObject("txtLoginId.Image")));
            this.txtLoginId.Location = new System.Drawing.Point(61, 212);
            this.txtLoginId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginId.MaxLength = 32767;
            this.txtLoginId.Multiline = false;
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.ReadOnly = false;
            this.txtLoginId.ShortcutsEnabled = true;
            this.txtLoginId.ShowBottomBorder = true;
            this.txtLoginId.ShowTopBorder = true;
            this.txtLoginId.Size = new System.Drawing.Size(371, 54);
            this.txtLoginId.TabIndex = 16;
            this.txtLoginId.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtLoginId.UseSystemPasswordChar = false;
            this.txtLoginId.Watermark = "帐号";
            this.txtLoginId.WatermarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(120)))), ((int)(((byte)(129)))));
            // 
            // xRails_TitleLabel1
            // 
            this.xRails_TitleLabel1.AutoSize = true;
            this.xRails_TitleLabel1.BackColor = System.Drawing.Color.Transparent;
            this.xRails_TitleLabel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Animator.SetDecoration(this.xRails_TitleLabel1, AnimatorNS.DecorationType.None);
            this.xRails_TitleLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F);
            this.xRails_TitleLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.xRails_TitleLabel1.Location = new System.Drawing.Point(61, 138);
            this.xRails_TitleLabel1.Name = "xRails_TitleLabel1";
            this.xRails_TitleLabel1.Side = XLis.CustomControls.XRails_TitleLabel.PanelSide.LeftPanel;
            this.xRails_TitleLabel1.Size = new System.Drawing.Size(110, 40);
            this.xRails_TitleLabel1.TabIndex = 15;
            this.xRails_TitleLabel1.Text = "请登录";
            this.xRails_TitleLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.xRails_TitleLabel1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            this.xRails_TitleLabel1.UseCompatibleTextRendering = true;
            // 
            // Animator
            // 
            this.Animator.AnimationType = AnimatorNS.AnimationType.Custom;
            this.Animator.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.Animator.DefaultAnimation = animation1;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(968, 618);
            this.Controls.Add(this.pnLogin);
            this.Controls.Add(this.pnLogo);
            this.Animator.SetDecoration(this, AnimatorNS.DecorationType.None);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎";
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            this.pnSubmitBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnLogo;
        private System.Windows.Forms.Panel pnLogin;
        private Lis.CustomControls.XRails_Label lblVersion;
        private System.Windows.Forms.Panel pnSubmitBtn;
        private Lis.CustomControls.MetrolButton btnCancle;
        private Lis.CustomControls.MetrolButton btnLogin;
        private Lis.CustomControls.XRails_Label labLoading;
        private Lis.CustomControls.XRails_TextBox txtPassword;
        private Lis.CustomControls.XRails_TextBox txtLoginId;
        private XLis.CustomControls.XRails_TitleLabel xRails_TitleLabel1;
        private AnimatorNS.Animator Animator;
        private Lis.CustomControls.XRails_Label xRails_LabelError;
    }
}