namespace dcl.client.dicbasic
{
    partial class FrmPatInfoCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPatInfoCopy));
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPatInstructment = new dcl.client.control.SelectDicInstrument();
            this.SuspendLayout();
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 157);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(596, 131);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "仪器";
            // 
            // txtPatInstructment
            // 
            this.txtPatInstructment.AddEmptyRow = true;
            this.txtPatInstructment.BindByValue = false;
            this.txtPatInstructment.colDisplay = "";
            this.txtPatInstructment.colExtend1 = null;
            this.txtPatInstructment.colInCode = "";
            this.txtPatInstructment.colPY = "";
            this.txtPatInstructment.colSeq = "";
            this.txtPatInstructment.colValue = "";
            this.txtPatInstructment.colWB = "";
            this.txtPatInstructment.displayMember = null;
            this.txtPatInstructment.EnterMoveNext = true;
            this.txtPatInstructment.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtPatInstructment.KeyUpDownMoveNext = false;
            this.txtPatInstructment.LoadDataOnDesignMode = true;
            this.txtPatInstructment.Location = new System.Drawing.Point(161, 52);
            this.txtPatInstructment.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtPatInstructment.MaximumSize = new System.Drawing.Size(928, 42);
            this.txtPatInstructment.MinimumSize = new System.Drawing.Size(93, 42);
            this.txtPatInstructment.Name = "txtPatInstructment";
            this.txtPatInstructment.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatInstructment.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatInstructment.Readonly = false;
            this.txtPatInstructment.SaveSourceID = false;
            this.txtPatInstructment.SelectFilter = null;
            this.txtPatInstructment.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatInstructment.SelectOnly = true;
            this.txtPatInstructment.ShowAllInstrmt = false;
            this.txtPatInstructment.Size = new System.Drawing.Size(328, 42);
            this.txtPatInstructment.TabIndex = 6;
            this.txtPatInstructment.UseExtend = false;
            this.txtPatInstructment.valueMember = null;
            // 
            // FrmPatInfoCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 288);
            this.Controls.Add(this.txtPatInstructment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sysToolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmPatInfoCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "资料复制";
            this.Load += new System.EventHandler(this.FrmPatInfoCopy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Label label2;
        private control.SelectDicInstrument txtPatInstructment;
    }
}