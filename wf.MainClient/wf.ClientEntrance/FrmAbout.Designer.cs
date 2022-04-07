namespace wf.ClientEntrance
{
    partial class FrmAbout
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelProductName = new System.Windows.Forms.Label();
            this.txtCopyRight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUpdate
            // 
            this.txtUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.txtUpdate.Location = new System.Drawing.Point(20, 428);
            this.txtUpdate.Margin = new System.Windows.Forms.Padding(13, 7, 7, 7);
            this.txtUpdate.Multiline = true;
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.ReadOnly = true;
            this.txtUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdate.Size = new System.Drawing.Size(938, 644);
            this.txtUpdate.TabIndex = 24;
            this.txtUpdate.TabStop = false;
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.AutoSize = true;
            this.labelCompanyName.Location = new System.Drawing.Point(13, 174);
            this.labelCompanyName.Margin = new System.Windows.Forms.Padding(13, 0, 7, 0);
            this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 39);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(109, 29);
            this.labelCompanyName.TabIndex = 25;
            this.labelCompanyName.Text = "公司名称";
            this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(13, 97);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(13, 0, 7, 0);
            this.labelVersion.MaximumSize = new System.Drawing.Size(0, 39);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(61, 29);
            this.labelVersion.TabIndex = 26;
            this.labelVersion.Text = "版本";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Location = new System.Drawing.Point(13, 19);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(13, 0, 7, 0);
            this.labelProductName.MaximumSize = new System.Drawing.Size(0, 39);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(109, 29);
            this.labelProductName.TabIndex = 27;
            this.labelProductName.Text = "产品名称";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCopyRight
            // 
            this.txtCopyRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.txtCopyRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCopyRight.Location = new System.Drawing.Point(20, 232);
            this.txtCopyRight.Margin = new System.Windows.Forms.Padding(13, 7, 7, 7);
            this.txtCopyRight.Multiline = true;
            this.txtCopyRight.Name = "txtCopyRight";
            this.txtCopyRight.ReadOnly = true;
            this.txtCopyRight.Size = new System.Drawing.Size(943, 97);
            this.txtCopyRight.TabIndex = 28;
            this.txtCopyRight.TabStop = false;
            this.txtCopyRight.Text = "版权";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 365);
            this.label1.Margin = new System.Windows.Forms.Padding(13, 0, 7, 0);
            this.label1.MaximumSize = new System.Drawing.Size(0, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 29);
            this.label1.TabIndex = 29;
            this.label1.Text = "更新说明:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 1104);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCopyRight);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.txtUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.Padding = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "产品信息";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox txtCopyRight;
        private System.Windows.Forms.Label label1;

    }
}
