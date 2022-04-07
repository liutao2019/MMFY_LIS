namespace dcl.client.users
{
    partial class FrmNetCASignRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNetCASignRecord));
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.txtPassword2 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtCAUser = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCAUser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.EnterMoveNextControl = true;
            this.txtPassword.Location = new System.Drawing.Point(178, 91);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(325, 36);
            this.txtPassword.TabIndex = 40;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(63, 99);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 29);
            this.labelControl9.TabIndex = 41;
            this.labelControl9.Text = "密码";
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl13.Location = new System.Drawing.Point(154, 108);
            this.labelControl13.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(13, 29);
            this.labelControl13.TabIndex = 42;
            this.labelControl13.Text = "*";
            // 
            // txtPassword2
            // 
            this.txtPassword2.EnterMoveNextControl = true;
            this.txtPassword2.Location = new System.Drawing.Point(178, 170);
            this.txtPassword2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.Properties.PasswordChar = '*';
            this.txtPassword2.Size = new System.Drawing.Size(325, 36);
            this.txtPassword2.TabIndex = 43;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 176);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 29);
            this.labelControl1.TabIndex = 44;
            this.labelControl1.Text = "确认密码";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(154, 186);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(13, 29);
            this.labelControl2.TabIndex = 45;
            this.labelControl2.Text = "*";
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 267);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(550, 145);
            this.sysToolBar1.TabIndex = 47;
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 29);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 29);
            this.labelControl3.TabIndex = 48;
            this.labelControl3.Text = "证书用户";
            // 
            // txtCAUser
            // 
            this.txtCAUser.EnterMoveNextControl = true;
            this.txtCAUser.Location = new System.Drawing.Point(178, 21);
            this.txtCAUser.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCAUser.Name = "txtCAUser";
            this.txtCAUser.Properties.ReadOnly = true;
            this.txtCAUser.Size = new System.Drawing.Size(325, 36);
            this.txtCAUser.TabIndex = 49;
            // 
            // FrmNetCASignRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 412);
            this.Controls.Add(this.txtCAUser);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.sysToolBar1);
            this.Controls.Add(this.txtPassword2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl13);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNetCASignRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CA信息";
            this.Load += new System.EventHandler(this.FrmNetCASignRecord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCAUser.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.TextEdit txtPassword2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtCAUser;
    }
}