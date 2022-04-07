namespace lis.client.control
{
    partial class FrmFilter
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
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.txtF = new DevExpress.XtraEditors.TextEdit();
            this.filterControl = new lis.client.control.FilterControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtF.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoEnableButtons = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 368);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.QuickOption = true;
            this.sysToolBar1.Size = new System.Drawing.Size(404, 34);
            this.sysToolBar1.TabIndex = 11;
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            // 
            // txtF
            // 
            this.txtF.Location = new System.Drawing.Point(0, 0);
            this.txtF.Name = "txtF";
            this.txtF.Size = new System.Drawing.Size(0, 21);
            this.txtF.TabIndex = 13;
            // 
            // filterControl
            // 
            this.filterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterControl.Location = new System.Drawing.Point(0, 0);
            this.filterControl.Name = "filterControl";
            this.filterControl.Size = new System.Drawing.Size(404, 368);
            this.filterControl.SortFilterColumns = false;
            this.filterControl.TabIndex = 12;
            this.filterControl.Text = "filterControl1";
            // 
            // FrmFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 402);
            this.Controls.Add(this.txtF);
            this.Controls.Add(this.filterControl);
            this.Controls.Add(this.sysToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查询条件";
            this.Load += new System.EventHandler(this.FrmFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtF.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar sysToolBar1;
        private FilterControl filterControl;
        private DevExpress.XtraEditors.TextEdit txtF;
    }
}