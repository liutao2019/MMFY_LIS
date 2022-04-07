namespace dcl.client.qc
{
    partial class FrmQcDataModify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQcDataModify));
            this.txtQcData = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.lblErro = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtModReason = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.labItemEcd = new System.Windows.Forms.Label();
            this.pnlOutOfControl = new System.Windows.Forms.Panel();
            this.txtFun = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtReson = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtQcData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtModReason.Properties)).BeginInit();
            this.pnlOutOfControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReson.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtQcData
            // 
            this.txtQcData.Location = new System.Drawing.Point(136, 62);
            this.txtQcData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtQcData.Name = "txtQcData";
            this.txtQcData.Properties.Mask.EditMask = "f0";
            this.txtQcData.Size = new System.Drawing.Size(249, 36);
            this.txtQcData.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 29);
            this.label4.TabIndex = 12;
            this.label4.Text = "测定值：";
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(3, 329);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(425, 124);
            this.sysToolBar1.TabIndex = 13;
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            // 
            // lblErro
            // 
            this.lblErro.AutoSize = true;
            this.lblErro.ForeColor = System.Drawing.Color.Red;
            this.lblErro.Location = new System.Drawing.Point(180, 64);
            this.lblErro.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblErro.Name = "lblErro";
            this.lblErro.Size = new System.Drawing.Size(0, 29);
            this.lblErro.TabIndex = 14;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtModReason);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.labItemEcd);
            this.panelControl1.Controls.Add(this.pnlOutOfControl);
            this.panelControl1.Controls.Add(this.txtQcData);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.sysToolBar1);
            this.panelControl1.Controls.Add(this.lblErro);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(431, 456);
            this.panelControl1.TabIndex = 15;
            // 
            // txtModReason
            // 
            this.txtModReason.Location = new System.Drawing.Point(136, 122);
            this.txtModReason.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtModReason.Name = "txtModReason";
            this.txtModReason.Properties.Mask.EditMask = "f0";
            this.txtModReason.Size = new System.Drawing.Size(249, 36);
            this.txtModReason.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 29);
            this.label3.TabIndex = 17;
            this.label3.Text = "修改原因：";
            // 
            // labItemEcd
            // 
            this.labItemEcd.AutoSize = true;
            this.labItemEcd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labItemEcd.Location = new System.Drawing.Point(48, 31);
            this.labItemEcd.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labItemEcd.Name = "labItemEcd";
            this.labItemEcd.Size = new System.Drawing.Size(0, 29);
            this.labItemEcd.TabIndex = 16;
            // 
            // pnlOutOfControl
            // 
            this.pnlOutOfControl.Controls.Add(this.txtFun);
            this.pnlOutOfControl.Controls.Add(this.txtReson);
            this.pnlOutOfControl.Controls.Add(this.label2);
            this.pnlOutOfControl.Controls.Add(this.label1);
            this.pnlOutOfControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOutOfControl.Location = new System.Drawing.Point(3, 205);
            this.pnlOutOfControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlOutOfControl.Name = "pnlOutOfControl";
            this.pnlOutOfControl.Size = new System.Drawing.Size(425, 124);
            this.pnlOutOfControl.TabIndex = 15;
            this.pnlOutOfControl.Visible = false;
            // 
            // txtFun
            // 
            this.txtFun.EditValue = "";
            this.txtFun.EnterMoveNextControl = true;
            this.txtFun.Location = new System.Drawing.Point(132, 75);
            this.txtFun.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtFun.Name = "txtFun";
            this.txtFun.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtFun.Size = new System.Drawing.Size(249, 36);
            this.txtFun.TabIndex = 18;
            // 
            // txtReson
            // 
            this.txtReson.EditValue = "";
            this.txtReson.EnterMoveNextControl = true;
            this.txtReson.Location = new System.Drawing.Point(132, 14);
            this.txtReson.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtReson.Name = "txtReson";
            this.txtReson.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReson.Size = new System.Drawing.Size(249, 36);
            this.txtReson.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 29);
            this.label2.TabIndex = 16;
            this.label2.Text = "处理方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 29);
            this.label1.TabIndex = 14;
            this.label1.Text = "失控原因：";
            // 
            // FrmQcDataModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 456);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQcDataModify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "质控数据修改";
            this.Deactivate += new System.EventHandler(this.FrmQcDataModify_Deactivate);
            this.Load += new System.EventHandler(this.FrmQcDataModify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtQcData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtModReason.Properties)).EndInit();
            this.pnlOutOfControl.ResumeLayout(false);
            this.pnlOutOfControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReson.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.TextEdit txtQcData;
        private System.Windows.Forms.Label label4;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Label lblErro;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Panel pnlOutOfControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labItemEcd;
        public DevExpress.XtraEditors.ComboBoxEdit txtFun;
        public DevExpress.XtraEditors.ComboBoxEdit txtReson;
        protected DevExpress.XtraEditors.TextEdit txtModReason;
        private System.Windows.Forms.Label label3;
    }
}