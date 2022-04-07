namespace dcl.client.dicbasic
{
    partial class FrmMitmOrAdjustCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMitmOrAdjustCopy));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lue_Instrmt = new dcl.client.control.SelectDicInstrument();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(78, 68);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 29);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "目标仪器";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(382, 161);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(205, 56);
            this.btnClose.TabIndex = 174;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(102, 161);
            this.btnOk.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(205, 56);
            this.btnOk.TabIndex = 173;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(184, 74);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(13, 29);
            this.labelControl2.TabIndex = 176;
            this.labelControl2.Text = "*";
            // 
            // lue_Instrmt
            // 
            this.lue_Instrmt.AddEmptyRow = true;
            this.lue_Instrmt.BindByValue = false;
            this.lue_Instrmt.colDisplay = "";
            this.lue_Instrmt.colExtend1 = null;
            this.lue_Instrmt.colInCode = "";
            this.lue_Instrmt.colPY = "";
            this.lue_Instrmt.colSeq = "";
            this.lue_Instrmt.colValue = "";
            this.lue_Instrmt.colWB = "";
            this.lue_Instrmt.displayMember = null;
            this.lue_Instrmt.EnterMoveNext = true;
            this.lue_Instrmt.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Instrmt.KeyUpDownMoveNext = false;
            this.lue_Instrmt.LoadDataOnDesignMode = true;
            this.lue_Instrmt.Location = new System.Drawing.Point(210, 71);
            this.lue_Instrmt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lue_Instrmt.MaximumSize = new System.Drawing.Size(928, 42);
            this.lue_Instrmt.MinimumSize = new System.Drawing.Size(93, 42);
            this.lue_Instrmt.Name = "lue_Instrmt";
            this.lue_Instrmt.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Instrmt.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Instrmt.Readonly = false;
            this.lue_Instrmt.SaveSourceID = false;
            this.lue_Instrmt.SelectFilter = null;
            this.lue_Instrmt.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Instrmt.SelectOnly = true;
            this.lue_Instrmt.ShowAllInstrmt = false;
            this.lue_Instrmt.Size = new System.Drawing.Size(377, 42);
            this.lue_Instrmt.TabIndex = 177;
            this.lue_Instrmt.UseExtend = false;
            this.lue_Instrmt.valueMember = null;
            // 
            // FrmMitmOrAdjustCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 263);
            this.Controls.Add(this.lue_Instrmt);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmMitmOrAdjustCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "仪器通道复制";
            this.Load += new System.EventHandler(this.FrmMitmOrAdjustCopy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private control.SelectDicInstrument lue_Instrmt;
    }
}