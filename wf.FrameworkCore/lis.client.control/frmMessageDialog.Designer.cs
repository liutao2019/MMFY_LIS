namespace lis.client.control
{
    partial class frmMessageDialog
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
            this.pnlMain = new DevExpress.XtraEditors.PanelControl();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlButtonContainer = new DevExpress.XtraEditors.PanelControl();
            this.pnl_okcancel = new DevExpress.XtraEditors.PanelControl();
            this.btn_okcancel_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_okcancel_ok = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_yesnocancel = new DevExpress.XtraEditors.PanelControl();
            this.btn_yesnocancel_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_yesnocancel_no = new DevExpress.XtraEditors.SimpleButton();
            this.btn_yesnocancel_yes = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_yesno = new DevExpress.XtraEditors.PanelControl();
            this.btn_yesno_no = new DevExpress.XtraEditors.SimpleButton();
            this.btn_yesno_yes = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_ok = new DevExpress.XtraEditors.PanelControl();
            this.btn_ok_ok = new DevExpress.XtraEditors.SimpleButton();
            this.pnlForm = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).BeginInit();
            this.pnlButtonContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_okcancel)).BeginInit();
            this.pnl_okcancel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_yesnocancel)).BeginInit();
            this.pnl_yesnocancel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_yesno)).BeginInit();
            this.pnl_yesno.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ok)).BeginInit();
            this.pnl_ok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlForm)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Appearance.Options.UseBackColor = true;
            this.pnlMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlMain.Controls.Add(this.lblMsg);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(296, 78);
            this.pnlMain.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("宋体", 10.5F);
            this.lblMsg.Location = new System.Drawing.Point(36, 29);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 14);
            this.lblMsg.TabIndex = 0;
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtonContainer.Appearance.Options.UseBackColor = true;
            this.pnlButtonContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlButtonContainer.Controls.Add(this.pnl_okcancel);
            this.pnlButtonContainer.Controls.Add(this.pnl_yesnocancel);
            this.pnlButtonContainer.Controls.Add(this.pnl_yesno);
            this.pnlButtonContainer.Controls.Add(this.pnl_ok);
            this.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonContainer.Location = new System.Drawing.Point(2, 80);
            this.pnlButtonContainer.Name = "pnlButtonContainer";
            this.pnlButtonContainer.Size = new System.Drawing.Size(296, 34);
            this.pnlButtonContainer.TabIndex = 1;
            // 
            // pnl_okcancel
            // 
            this.pnl_okcancel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_okcancel.Controls.Add(this.btn_okcancel_cancel);
            this.pnl_okcancel.Controls.Add(this.btn_okcancel_ok);
            this.pnl_okcancel.Location = new System.Drawing.Point(0, 51);
            this.pnl_okcancel.Name = "pnl_okcancel";
            this.pnl_okcancel.Size = new System.Drawing.Size(296, 28);
            this.pnl_okcancel.TabIndex = 5;
            this.pnl_okcancel.Visible = false;
            // 
            // btn_okcancel_cancel
            // 
            this.btn_okcancel_cancel.Location = new System.Drawing.Point(160, 2);
            this.btn_okcancel_cancel.Name = "btn_okcancel_cancel";
            this.btn_okcancel_cancel.Size = new System.Drawing.Size(80, 25);
            this.btn_okcancel_cancel.TabIndex = 1;
            this.btn_okcancel_cancel.Text = "取消";
            this.btn_okcancel_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_okcancel_ok
            // 
            this.btn_okcancel_ok.Location = new System.Drawing.Point(70, 2);
            this.btn_okcancel_ok.Name = "btn_okcancel_ok";
            this.btn_okcancel_ok.Size = new System.Drawing.Size(80, 25);
            this.btn_okcancel_ok.TabIndex = 0;
            this.btn_okcancel_ok.Text = "确定";
            this.btn_okcancel_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // pnl_yesnocancel
            // 
            this.pnl_yesnocancel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_yesnocancel.Controls.Add(this.btn_yesnocancel_cancel);
            this.pnl_yesnocancel.Controls.Add(this.btn_yesnocancel_no);
            this.pnl_yesnocancel.Controls.Add(this.btn_yesnocancel_yes);
            this.pnl_yesnocancel.Location = new System.Drawing.Point(183, 45);
            this.pnl_yesnocancel.Name = "pnl_yesnocancel";
            this.pnl_yesnocancel.Size = new System.Drawing.Size(296, 28);
            this.pnl_yesnocancel.TabIndex = 4;
            this.pnl_yesnocancel.Visible = false;
            // 
            // btn_yesnocancel_cancel
            // 
            this.btn_yesnocancel_cancel.Location = new System.Drawing.Point(194, 3);
            this.btn_yesnocancel_cancel.Name = "btn_yesnocancel_cancel";
            this.btn_yesnocancel_cancel.Size = new System.Drawing.Size(80, 25);
            this.btn_yesnocancel_cancel.TabIndex = 2;
            this.btn_yesnocancel_cancel.Text = "取消";
            this.btn_yesnocancel_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_yesnocancel_no
            // 
            this.btn_yesnocancel_no.Location = new System.Drawing.Point(113, 3);
            this.btn_yesnocancel_no.Name = "btn_yesnocancel_no";
            this.btn_yesnocancel_no.Size = new System.Drawing.Size(80, 25);
            this.btn_yesnocancel_no.TabIndex = 1;
            this.btn_yesnocancel_no.Text = "否";
            this.btn_yesnocancel_no.Click += new System.EventHandler(this.btn_no_Click);
            // 
            // btn_yesnocancel_yes
            // 
            this.btn_yesnocancel_yes.Location = new System.Drawing.Point(32, 3);
            this.btn_yesnocancel_yes.Name = "btn_yesnocancel_yes";
            this.btn_yesnocancel_yes.Size = new System.Drawing.Size(80, 25);
            this.btn_yesnocancel_yes.TabIndex = 0;
            this.btn_yesnocancel_yes.Text = "是";
            this.btn_yesnocancel_yes.Click += new System.EventHandler(this.btn_yes_Click);
            // 
            // pnl_yesno
            // 
            this.pnl_yesno.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_yesno.Controls.Add(this.btn_yesno_no);
            this.pnl_yesno.Controls.Add(this.btn_yesno_yes);
            this.pnl_yesno.Location = new System.Drawing.Point(10, 50);
            this.pnl_yesno.Name = "pnl_yesno";
            this.pnl_yesno.Size = new System.Drawing.Size(296, 28);
            this.pnl_yesno.TabIndex = 3;
            this.pnl_yesno.Visible = false;
            // 
            // btn_yesno_no
            // 
            this.btn_yesno_no.Location = new System.Drawing.Point(150, 3);
            this.btn_yesno_no.Name = "btn_yesno_no";
            this.btn_yesno_no.Size = new System.Drawing.Size(80, 25);
            this.btn_yesno_no.TabIndex = 1;
            this.btn_yesno_no.Text = "否";
            this.btn_yesno_no.Click += new System.EventHandler(this.btn_no_Click);
            // 
            // btn_yesno_yes
            // 
            this.btn_yesno_yes.Location = new System.Drawing.Point(69, 3);
            this.btn_yesno_yes.Name = "btn_yesno_yes";
            this.btn_yesno_yes.Size = new System.Drawing.Size(80, 25);
            this.btn_yesno_yes.TabIndex = 0;
            this.btn_yesno_yes.Text = "是";
            this.btn_yesno_yes.Click += new System.EventHandler(this.btn_yes_Click);
            // 
            // pnl_ok
            // 
            this.pnl_ok.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_ok.Controls.Add(this.btn_ok_ok);
            this.pnl_ok.Location = new System.Drawing.Point(228, 36);
            this.pnl_ok.Name = "pnl_ok";
            this.pnl_ok.Size = new System.Drawing.Size(296, 28);
            this.pnl_ok.TabIndex = 2;
            this.pnl_ok.Visible = false;
            // 
            // btn_ok_ok
            // 
            this.btn_ok_ok.Location = new System.Drawing.Point(107, 2);
            this.btn_ok_ok.Name = "btn_ok_ok";
            this.btn_ok_ok.Size = new System.Drawing.Size(80, 25);
            this.btn_ok_ok.TabIndex = 0;
            this.btn_ok_ok.Text = "确定";
            this.btn_ok_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // pnlForm
            // 
            this.pnlForm.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pnlForm.Appearance.Options.UseBackColor = true;
            this.pnlForm.Controls.Add(this.pnlMain);
            this.pnlForm.Controls.Add(this.pnlButtonContainer);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(300, 116);
            this.pnlForm.TabIndex = 2;
            // 
            // frmMessageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 116);
            this.Controls.Add(this.pnlForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMessageDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            this.Load += new System.EventHandler(this.MessageDialog_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMessageDialog_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMain)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlButtonContainer)).EndInit();
            this.pnlButtonContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_okcancel)).EndInit();
            this.pnl_okcancel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_yesnocancel)).EndInit();
            this.pnl_yesnocancel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_yesno)).EndInit();
            this.pnl_yesno.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_ok)).EndInit();
            this.pnl_ok.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlForm)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMain;
        private DevExpress.XtraEditors.PanelControl pnlButtonContainer;
        private System.Windows.Forms.Label lblMsg;
        private DevExpress.XtraEditors.PanelControl pnlForm;
        private DevExpress.XtraEditors.PanelControl pnl_ok;
        private DevExpress.XtraEditors.SimpleButton btn_ok_ok;
        private DevExpress.XtraEditors.PanelControl pnl_yesno;
        private DevExpress.XtraEditors.SimpleButton btn_yesno_yes;
        private DevExpress.XtraEditors.SimpleButton btn_yesno_no;
        private DevExpress.XtraEditors.PanelControl pnl_yesnocancel;
        private DevExpress.XtraEditors.SimpleButton btn_yesnocancel_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_yesnocancel_no;
        private DevExpress.XtraEditors.SimpleButton btn_yesnocancel_yes;
        private DevExpress.XtraEditors.PanelControl pnl_okcancel;
        private DevExpress.XtraEditors.SimpleButton btn_okcancel_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_okcancel_ok;
    }
}