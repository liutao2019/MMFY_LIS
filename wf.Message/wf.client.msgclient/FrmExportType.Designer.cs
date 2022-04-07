namespace dcl.client.msgclient
{
    partial class FrmExportType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExportType));
            this.rbtA = new System.Windows.Forms.RadioButton();
            this.rbtB = new System.Windows.Forms.RadioButton();
            this.rbtC = new System.Windows.Forms.RadioButton();
            this.sbtnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // rbtA
            // 
            this.rbtA.AutoSize = true;
            this.rbtA.Checked = true;
            this.rbtA.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtA.Location = new System.Drawing.Point(158, 40);
            this.rbtA.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbtA.Name = "rbtA";
            this.rbtA.Size = new System.Drawing.Size(208, 43);
            this.rbtA.TabIndex = 0;
            this.rbtA.TabStop = true;
            this.rbtA.Tag = "1";
            this.rbtA.Text = "危机值记录";
            this.rbtA.UseVisualStyleBackColor = true;
            // 
            // rbtB
            // 
            this.rbtB.AutoSize = true;
            this.rbtB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtB.Location = new System.Drawing.Point(158, 89);
            this.rbtB.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbtB.Name = "rbtB";
            this.rbtB.Size = new System.Drawing.Size(243, 43);
            this.rbtB.TabIndex = 1;
            this.rbtB.Tag = "1";
            this.rbtB.Text = "MDR预警记录";
            this.rbtB.UseVisualStyleBackColor = true;
            // 
            // rbtC
            // 
            this.rbtC.AutoSize = true;
            this.rbtC.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtC.Location = new System.Drawing.Point(158, 137);
            this.rbtC.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbtC.Name = "rbtC";
            this.rbtC.Size = new System.Drawing.Size(240, 43);
            this.rbtC.TabIndex = 2;
            this.rbtC.Tag = "1";
            this.rbtC.Text = "其他预警记录";
            this.rbtC.UseVisualStyleBackColor = true;
            // 
            // sbtnConfirm
            // 
            this.sbtnConfirm.Location = new System.Drawing.Point(117, 193);
            this.sbtnConfirm.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sbtnConfirm.Name = "sbtnConfirm";
            this.sbtnConfirm.Size = new System.Drawing.Size(150, 55);
            this.sbtnConfirm.TabIndex = 5;
            this.sbtnConfirm.Text = "确定";
            // 
            // sbtnCancel
            // 
            this.sbtnCancel.Location = new System.Drawing.Point(276, 193);
            this.sbtnCancel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sbtnCancel.Name = "sbtnCancel";
            this.sbtnCancel.Size = new System.Drawing.Size(150, 55);
            this.sbtnCancel.TabIndex = 6;
            this.sbtnCancel.Text = "取消";
            // 
            // FrmExportType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 279);
            this.Controls.Add(this.sbtnCancel);
            this.Controls.Add(this.sbtnConfirm);
            this.Controls.Add(this.rbtC);
            this.Controls.Add(this.rbtB);
            this.Controls.Add(this.rbtA);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExportType";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出类型选择";
            this.Load += new System.EventHandler(this.FrmExportType_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtA;
        private System.Windows.Forms.RadioButton rbtB;
        private System.Windows.Forms.RadioButton rbtC;
        private DevExpress.XtraEditors.SimpleButton sbtnConfirm;
        private DevExpress.XtraEditors.SimpleButton sbtnCancel;
    }
}