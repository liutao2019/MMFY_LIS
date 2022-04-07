namespace lis.client.control
{
  partial class FrmCheckPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCheckPassword));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtPat_i_code = new dcl.client.control.SelectDictSysUser();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.lblPat_i_code = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtLoginid = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginid.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtPat_i_code);
            this.panelControl1.Controls.Add(this.sysToolBar1);
            this.panelControl1.Controls.Add(this.lblPat_i_code);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.txtPassword);
            this.panelControl1.Controls.Add(this.txtLoginid);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(399, 256);
            this.panelControl1.TabIndex = 4;
            // 
            // txtPat_i_code
            // 
            this.txtPat_i_code.AddEmptyRow = true;
            this.txtPat_i_code.BindByValue = false;
            this.txtPat_i_code.colDisplay = "";
            this.txtPat_i_code.colExtend1 = null;
            this.txtPat_i_code.colInCode = "";
            this.txtPat_i_code.colPY = "";
            this.txtPat_i_code.colSeq = "";
            this.txtPat_i_code.colValue = "";
            this.txtPat_i_code.colWB = "";
            this.txtPat_i_code.displayMember = null;
            this.txtPat_i_code.EnterMoveNext = true;
            this.txtPat_i_code.FilterMode = dcl.client.control.DclPopFilterMode.ExactMatching;
            this.txtPat_i_code.KeyUpDownMoveNext = false;
            this.txtPat_i_code.LoadDataOnDesignMode = true;
            this.txtPat_i_code.Location = new System.Drawing.Point(184, 26);
            this.txtPat_i_code.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPat_i_code.MaximumSize = new System.Drawing.Size(571, 27);
            this.txtPat_i_code.MinimumSize = new System.Drawing.Size(57, 27);
            this.txtPat_i_code.Name = "txtPat_i_code";
            this.txtPat_i_code.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPat_i_code.PFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPat_i_code.Readonly = false;
            this.txtPat_i_code.SaveSourceID = false;
            this.txtPat_i_code.SelectFilter = null;
            this.txtPat_i_code.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPat_i_code.SelectOnly = true;
            this.txtPat_i_code.Size = new System.Drawing.Size(133, 27);
            this.txtPat_i_code.TabIndex = 0;
            this.txtPat_i_code.UseExtend = false;
            this.txtPat_i_code.valueMember = null;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Location = new System.Drawing.Point(92, 152);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.QuickOption = false;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(228, 81);
            this.sysToolBar1.TabIndex = 10;
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.btnCancle_Click);
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.btnOk_Click);
            // 
            // lblPat_i_code
            // 
            this.lblPat_i_code.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPat_i_code.Location = new System.Drawing.Point(90, 26);
            this.lblPat_i_code.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblPat_i_code.Name = "lblPat_i_code";
            this.lblPat_i_code.Size = new System.Drawing.Size(72, 24);
            this.lblPat_i_code.TabIndex = 27;
            this.lblPat_i_code.Text = "检 验 者";
            this.lblPat_i_code.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(82, 64);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 24);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "身份帐号";
            // 
            // txtPassword
            // 
            this.txtPassword.EnterMoveNextControl = true;
            this.txtPassword.Location = new System.Drawing.Point(184, 103);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(134, 30);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Enter_KeyPress);
            // 
            // txtLoginid
            // 
            this.txtLoginid.EnterMoveNextControl = true;
            this.txtLoginid.Location = new System.Drawing.Point(184, 61);
            this.txtLoginid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginid.Name = "txtLoginid";
            this.txtLoginid.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginid.Properties.Appearance.Options.UseFont = true;
            this.txtLoginid.Size = new System.Drawing.Size(134, 30);
            this.txtLoginid.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(82, 105);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(80, 24);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "身份密码";
            // 
            // FrmCheckPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 256);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmCheckPassword";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "身份验证";
            this.Load += new System.EventHandler(this.FrmCheckPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginid.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.PanelControl panelControl1;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private dcl.client.common.SysToolBar sysToolBar1;
    public DevExpress.XtraEditors.TextEdit txtLoginid;
    public DevExpress.XtraEditors.TextEdit txtPassword;
    private dcl.client.control.SelectDictSysUser txtPat_i_code;
    private DevExpress.XtraEditors.LabelControl lblPat_i_code;

  }
}