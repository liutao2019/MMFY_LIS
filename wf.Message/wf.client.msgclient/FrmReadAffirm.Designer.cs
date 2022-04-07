namespace dcl.client.msgclient
{
    partial class FrmReadAffirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReadAffirm));
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.txtMsg2 = new System.Windows.Forms.TextBox();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.fpat_doc_id = new dcl.client.msgclient.SelectDicPubDoctor();
            this.lbcDoc = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.lbBscripe = new DevExpress.XtraEditors.LabelControl();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.sbClose = new DevExpress.XtraEditors.SimpleButton();
            this.sbConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPwd.Location = new System.Drawing.Point(165, 52);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(208, 29);
            this.txtPwd.TabIndex = 16;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmReadAffirm_KeyDown);
            // 
            // txtUserId
            // 
            this.txtUserId.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserId.Location = new System.Drawing.Point(165, 14);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(208, 29);
            this.txtUserId.TabIndex = 15;
            this.txtUserId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserId_KeyDown);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.panelControl5);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.panelControl4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(513, 555);
            this.panelControl1.TabIndex = 12;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panelControl3);
            this.groupControl1.Controls.Add(this.panelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 127);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(509, 210);
            this.groupControl1.TabIndex = 32;
            this.groupControl1.Text = "处理意见";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.txtMsg2);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 57);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(505, 151);
            this.panelControl3.TabIndex = 1;
            // 
            // txtMsg2
            // 
            this.txtMsg2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMsg2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMsg2.Location = new System.Drawing.Point(2, 2);
            this.txtMsg2.Multiline = true;
            this.txtMsg2.Name = "txtMsg2";
            this.txtMsg2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg2.Size = new System.Drawing.Size(501, 147);
            this.txtMsg2.TabIndex = 18;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.radioButton1);
            this.panelControl2.Controls.Add(this.radioButton3);
            this.panelControl2.Controls.Add(this.radioButton2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(2, 27);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(505, 30);
            this.panelControl2.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(22, 6);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(58, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "复查";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton3.Location = new System.Drawing.Point(236, 6);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(66, 19);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "其他:";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.Location = new System.Drawing.Point(118, 6);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(88, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "继续观察";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.labelControl1);
            this.panelControl5.Controls.Add(this.txtPwd);
            this.panelControl5.Controls.Add(this.txtUserId);
            this.panelControl5.Controls.Add(this.fpat_doc_id);
            this.panelControl5.Controls.Add(this.lbcDoc);
            this.panelControl5.Controls.Add(this.labelControl2);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl5.Location = new System.Drawing.Point(2, 2);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(509, 125);
            this.panelControl5.TabIndex = 31;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(120, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(38, 23);
            this.labelControl1.TabIndex = 23;
            this.labelControl1.Text = "用户";
            // 
            // fpat_doc_id
            // 
            this.fpat_doc_id.AddEmptyRow = true;
            this.fpat_doc_id.BindByValue = false;
            this.fpat_doc_id.colDisplay = "";
            this.fpat_doc_id.colExtend1 = null;
            this.fpat_doc_id.colInCode = "";
            this.fpat_doc_id.colPY = "";
            this.fpat_doc_id.colSeq = "";
            this.fpat_doc_id.colValue = "";
            this.fpat_doc_id.colWB = "";
            this.fpat_doc_id.displayMember = null;
            this.fpat_doc_id.EnterMoveNext = true;
            this.fpat_doc_id.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.fpat_doc_id.KeyUpDownMoveNext = false;
            this.fpat_doc_id.LoadDataOnDesignMode = true;
            this.fpat_doc_id.Location = new System.Drawing.Point(165, 90);
            this.fpat_doc_id.MaximumSize = new System.Drawing.Size(500, 21);
            this.fpat_doc_id.MinimumSize = new System.Drawing.Size(50, 21);
            this.fpat_doc_id.Name = "fpat_doc_id";
            this.fpat_doc_id.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.fpat_doc_id.PFont = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpat_doc_id.Readonly = false;
            this.fpat_doc_id.SaveSourceID = true;
            this.fpat_doc_id.SelectFilter = null;
            this.fpat_doc_id.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.fpat_doc_id.SelectOnly = true;
            this.fpat_doc_id.Size = new System.Drawing.Size(208, 21);
            this.fpat_doc_id.TabIndex = 20;
            this.fpat_doc_id.UseExtend = false;
            this.fpat_doc_id.valueMember = null;
            this.fpat_doc_id.Visible = false;
            // 
            // lbcDoc
            // 
            this.lbcDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbcDoc.Location = new System.Drawing.Point(120, 86);
            this.lbcDoc.Name = "lbcDoc";
            this.lbcDoc.Size = new System.Drawing.Size(38, 23);
            this.lbcDoc.TabIndex = 25;
            this.lbcDoc.Text = "医生";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(120, 49);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(38, 23);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "密码";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.lbBscripe);
            this.groupControl2.Controls.Add(this.txtMsg);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(2, 337);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(509, 167);
            this.groupControl2.TabIndex = 30;
            this.groupControl2.Text = "备注";
            // 
            // lbBscripe
            // 
            this.lbBscripe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBscripe.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbBscripe.Location = new System.Drawing.Point(469, 4);
            this.lbBscripe.Name = "lbBscripe";
            this.lbBscripe.Size = new System.Drawing.Size(30, 18);
            this.lbBscripe.TabIndex = 18;
            this.lbBscripe.Text = "模版";
            // 
            // txtMsg
            // 
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMsg.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMsg.Location = new System.Drawing.Point(2, 27);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(505, 138);
            this.txtMsg.TabIndex = 17;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.sbClose);
            this.panelControl4.Controls.Add(this.sbConfirm);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(2, 504);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(509, 49);
            this.panelControl4.TabIndex = 29;
            // 
            // sbClose
            // 
            this.sbClose.Location = new System.Drawing.Point(281, 5);
            this.sbClose.Name = "sbClose";
            this.sbClose.Size = new System.Drawing.Size(108, 38);
            this.sbClose.TabIndex = 1;
            this.sbClose.Text = "关闭";
            // 
            // sbConfirm
            // 
            this.sbConfirm.Location = new System.Drawing.Point(100, 5);
            this.sbConfirm.Name = "sbConfirm";
            this.sbConfirm.Size = new System.Drawing.Size(108, 38);
            this.sbConfirm.TabIndex = 0;
            this.sbConfirm.Text = "确认";
            // 
            // FrmReadAffirm
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 555);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 180);
            this.Name = "FrmReadAffirm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "危急值确认";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            this.panelControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtUserId;
        //private SelectDict_Doctor fpat_doc_id;
        private SelectDicPubDoctor fpat_doc_id;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl lbcDoc;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton sbConfirm;
        private DevExpress.XtraEditors.SimpleButton sbClose;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private System.Windows.Forms.TextBox txtMsg;
        private DevExpress.XtraEditors.LabelControl lbBscripe;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.TextBox txtMsg2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}