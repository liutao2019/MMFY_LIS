namespace dcl.client.result.CommonPatientInput
{
    partial class FrmItemInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemInfo));
            this.txtItemLInfo = new DevExpress.XtraEditors.MemoEdit();
            this.txtItemHInfo = new DevExpress.XtraEditors.MemoEdit();
            this.txtItemSign = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl36 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl35 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl28 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemLInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemHInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSign.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtItemLInfo
            // 
            this.txtItemLInfo.Location = new System.Drawing.Point(143, 575);
            this.txtItemLInfo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtItemLInfo.Name = "txtItemLInfo";
            this.txtItemLInfo.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtItemLInfo.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtItemLInfo.Properties.ReadOnly = true;
            this.txtItemLInfo.Size = new System.Drawing.Size(1224, 258);
            this.txtItemLInfo.TabIndex = 17;
            // 
            // txtItemHInfo
            // 
            this.txtItemHInfo.Location = new System.Drawing.Point(143, 303);
            this.txtItemHInfo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtItemHInfo.Name = "txtItemHInfo";
            this.txtItemHInfo.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtItemHInfo.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtItemHInfo.Properties.ReadOnly = true;
            this.txtItemHInfo.Size = new System.Drawing.Size(1224, 258);
            this.txtItemHInfo.TabIndex = 16;
            // 
            // txtItemSign
            // 
            this.txtItemSign.Location = new System.Drawing.Point(143, 29);
            this.txtItemSign.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtItemSign.Name = "txtItemSign";
            this.txtItemSign.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtItemSign.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtItemSign.Properties.ReadOnly = true;
            this.txtItemSign.Size = new System.Drawing.Size(1224, 258);
            this.txtItemSign.TabIndex = 15;
            // 
            // labelControl36
            // 
            this.labelControl36.Location = new System.Drawing.Point(26, 580);
            this.labelControl36.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl36.Name = "labelControl36";
            this.labelControl36.Size = new System.Drawing.Size(96, 29);
            this.labelControl36.TabIndex = 14;
            this.labelControl36.Text = "偏低提示";
            // 
            // labelControl35
            // 
            this.labelControl35.Location = new System.Drawing.Point(26, 306);
            this.labelControl35.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl35.Name = "labelControl35";
            this.labelControl35.Size = new System.Drawing.Size(96, 29);
            this.labelControl35.TabIndex = 13;
            this.labelControl35.Text = "偏高提示";
            // 
            // labelControl28
            // 
            this.labelControl28.Location = new System.Drawing.Point(26, 29);
            this.labelControl28.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl28.Name = "labelControl28";
            this.labelControl28.Size = new System.Drawing.Size(96, 29);
            this.labelControl28.TabIndex = 12;
            this.labelControl28.Text = "临床意义";
            // 
            // FrmItemInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 873);
            this.Controls.Add(this.txtItemLInfo);
            this.Controls.Add(this.txtItemHInfo);
            this.Controls.Add(this.txtItemSign);
            this.Controls.Add(this.labelControl36);
            this.Controls.Add(this.labelControl35);
            this.Controls.Add(this.labelControl28);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmItemInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目信息";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmItemInfo_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmItemInfo_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtItemLInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemHInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSign.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit txtItemLInfo;
        private DevExpress.XtraEditors.MemoEdit txtItemHInfo;
        private DevExpress.XtraEditors.MemoEdit txtItemSign;
        private DevExpress.XtraEditors.LabelControl labelControl36;
        private DevExpress.XtraEditors.LabelControl labelControl35;
        private DevExpress.XtraEditors.LabelControl labelControl28;
    }
}