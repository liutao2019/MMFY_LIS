namespace dcl.client.oa
{
  partial class FrmDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDialog));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lueDuty = new DevExpress.XtraEditors.LookUpEdit();
            this.labDutyName = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtLogin = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueDuty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.sysToolBar1);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(728, 452);
            this.panelControl1.TabIndex = 4;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Location = new System.Drawing.Point(223, 365);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.QuickOption = false;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(262, 145);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked_1);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lueDuty);
            this.panelControl2.Controls.Add(this.labDutyName);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Controls.Add(this.txtLogin);
            this.panelControl2.Controls.Add(this.txtPassword);
            this.panelControl2.Controls.Add(this.labelControl2);
            this.panelControl2.Location = new System.Drawing.Point(26, 29);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(674, 314);
            this.panelControl2.TabIndex = 9;
            // 
            // lueDuty
            // 
            this.lueDuty.EnterMoveNextControl = true;
            this.lueDuty.Location = new System.Drawing.Point(288, 128);
            this.lueDuty.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.lueDuty.Name = "lueDuty";
            this.lueDuty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueDuty.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("duty_id", "班次ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("duty_name", "班次名称")});
            this.lueDuty.Properties.DisplayMember = "duty_id";
            this.lueDuty.Properties.NullText = "";
            this.lueDuty.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lueDuty.Properties.ValueMember = "duty_name";
            this.lueDuty.Size = new System.Drawing.Size(217, 36);
            this.lueDuty.TabIndex = 1;
            this.lueDuty.Visible = false;
            // 
            // labDutyName
            // 
            this.labDutyName.Location = new System.Drawing.Point(139, 128);
            this.labDutyName.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labDutyName.Name = "labDutyName";
            this.labDutyName.Size = new System.Drawing.Size(96, 29);
            this.labDutyName.TabIndex = 4;
            this.labDutyName.Text = "班次名称";
            this.labDutyName.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(139, 60);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 29);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "身份帐号";
            // 
            // txtLogin
            // 
            this.txtLogin.EnterMoveNextControl = true;
            this.txtLogin.Location = new System.Drawing.Point(288, 53);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(217, 36);
            this.txtLogin.TabIndex = 0;
            this.txtLogin.EditValueChanged += new System.EventHandler(this.txtLogin_EditValueChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.EnterMoveNextControl = true;
            this.txtPassword.Location = new System.Drawing.Point(288, 193);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(217, 36);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(139, 201);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 29);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "身份密码";
            // 
            // FrmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 452);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "FrmDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "身份验证";
            this.Load += new System.EventHandler(this.FrmDialog_Load);
            this.Shown += new System.EventHandler(this.FrmDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueDuty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.PanelControl panelControl1;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.PanelControl panelControl2;
    private dcl.client.common.SysToolBar sysToolBar1;
    public DevExpress.XtraEditors.TextEdit txtPassword;
    private DevExpress.XtraEditors.LookUpEdit lueDuty;
    private DevExpress.XtraEditors.LabelControl labDutyName;
    public DevExpress.XtraEditors.TextEdit txtLogin;

  }
}