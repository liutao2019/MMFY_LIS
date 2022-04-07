namespace dcl.client.oa
{
    partial class FrmOfficePlanCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOfficePlanCopy));
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.sTo = new DevExpress.XtraEditors.DateEdit();
            this.sFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dTo = new DevExpress.XtraEditors.DateEdit();
            this.dFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.sTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dFrom.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 140);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(702, 145);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // sTo
            // 
            this.sTo.EditValue = null;
            this.sTo.Location = new System.Drawing.Point(429, 31);
            this.sTo.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.sTo.Name = "sTo";
            this.sTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sTo.Size = new System.Drawing.Size(217, 36);
            this.sTo.TabIndex = 1;
            this.sTo.TextChanged += new System.EventHandler(this.sFrom_TextChanged);
            // 
            // sFrom
            // 
            this.sFrom.EditValue = null;
            this.sFrom.Location = new System.Drawing.Point(178, 31);
            this.sFrom.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.sFrom.Name = "sFrom";
            this.sFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.sFrom.Size = new System.Drawing.Size(217, 36);
            this.sFrom.TabIndex = 0;
            this.sFrom.TextChanged += new System.EventHandler(this.sFrom_TextChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(407, 44);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(9, 29);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "-";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(61, 44);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 29);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "源时间段";
            // 
            // dTo
            // 
            this.dTo.EditValue = null;
            this.dTo.Location = new System.Drawing.Point(429, 118);
            this.dTo.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.dTo.Name = "dTo";
            this.dTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dTo.Properties.ReadOnly = true;
            this.dTo.Size = new System.Drawing.Size(217, 36);
            this.dTo.TabIndex = 3;
            // 
            // dFrom
            // 
            this.dFrom.EditValue = null;
            this.dFrom.Location = new System.Drawing.Point(178, 118);
            this.dFrom.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.dFrom.Name = "dFrom";
            this.dFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dFrom.Size = new System.Drawing.Size(217, 36);
            this.dFrom.TabIndex = 2;
            this.dFrom.TextChanged += new System.EventHandler(this.sFrom_TextChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(407, 131);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(9, 29);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "-";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(35, 131);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 29);
            this.labelControl2.TabIndex = 20;
            this.labelControl2.Text = "目的时间段";
            // 
            // FrmOfficePlanCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 285);
            this.Controls.Add(this.dTo);
            this.Controls.Add(this.dFrom);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.sTo);
            this.Controls.Add(this.sFrom);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.sysToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOfficePlanCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "复制排班计划";
            this.Load += new System.EventHandler(this.FrmOfficePlanCopy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dFrom.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.DateEdit sTo;
        public DevExpress.XtraEditors.DateEdit sFrom;
        public DevExpress.XtraEditors.DateEdit dTo;
        public DevExpress.XtraEditors.DateEdit dFrom;
    }
}