namespace dcl.client.sample
{
    partial class FrmSecondStat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSecondStat));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.deStrat = new DevExpress.XtraEditors.DateEdit();
            this.txtNoStrat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.prpGene = new dcl.client.common.PrintPreview();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStrat.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStrat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoStrat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.lueType);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.deEnd);
            this.panelControl1.Controls.Add(this.deStrat);
            this.panelControl1.Controls.Add(this.txtNoStrat);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1792, 174);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(1246, 101);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(191, 29);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "例：1-5.7.9.10-15";
            // 
            // lueType
            // 
            this.lueType.AddEmptyRow = true;
            this.lueType.BindByValue = false;
            this.lueType.colDisplay = "";
            this.lueType.colExtend1 = null;
            this.lueType.colInCode = "";
            this.lueType.colPY = "";
            this.lueType.colSeq = "";
            this.lueType.colValue = "";
            this.lueType.colWB = "";
            this.lueType.displayMember = null;
            this.lueType.EnterMoveNext = true;
            this.lueType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueType.KeyUpDownMoveNext = false;
            this.lueType.LoadDataOnDesignMode = true;
            this.lueType.Location = new System.Drawing.Point(143, 21);
            this.lueType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lueType.MaximumSize = new System.Drawing.Size(1083, 48);
            this.lueType.MinimumSize = new System.Drawing.Size(108, 48);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = true;
            this.lueType.Size = new System.Drawing.Size(282, 48);
            this.lueType.TabIndex = 0;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = null;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(438, 97);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(17, 29);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "~";
            // 
            // deEnd
            // 
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(468, 91);
            this.deEnd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Size = new System.Drawing.Size(282, 36);
            this.deEnd.TabIndex = 2;
            // 
            // deStrat
            // 
            this.deStrat.EditValue = null;
            this.deStrat.EnterMoveNextControl = true;
            this.deStrat.Location = new System.Drawing.Point(143, 91);
            this.deStrat.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.deStrat.Name = "deStrat";
            this.deStrat.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deStrat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStrat.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStrat.Size = new System.Drawing.Size(282, 36);
            this.deStrat.TabIndex = 1;
            // 
            // txtNoStrat
            // 
            this.txtNoStrat.EditValue = "";
            this.txtNoStrat.EnterMoveNextControl = true;
            this.txtNoStrat.Location = new System.Drawing.Point(947, 93);
            this.txtNoStrat.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtNoStrat.Name = "txtNoStrat";
            this.txtNoStrat.Size = new System.Drawing.Size(282, 36);
            this.txtNoStrat.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(799, 101);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(120, 29);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "编号范围：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(26, 29);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 29);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "接收室：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(52, 101);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 29);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "时间：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.prpGene);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 174);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1792, 1508);
            this.panelControl2.TabIndex = 1;
            // 
            // prpGene
            // 
            this.prpGene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prpGene.Location = new System.Drawing.Point(3, 3);
            this.prpGene.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.prpGene.Name = "prpGene";
            this.prpGene.Size = new System.Drawing.Size(1786, 1413);
            this.prpGene.TabIndex = 2;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.sysToolBar1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(3, 1416);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1786, 89);
            this.panelControl3.TabIndex = 1;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(3, 3);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1780, 83);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnStatClicked += new System.EventHandler(this.sysToolBar1_OnBtnStatClicked);
            this.sysToolBar1.OnBtnSinglePrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnSinglePrintClicked);
            // 
            // FrmSecondStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1792, 1682);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmSecondStat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标本登记统计";
            this.Load += new System.EventHandler(this.FrmSecondStat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStrat.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStrat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoStrat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private dcl.client.control.SelectDicLabProfession lueType;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.DateEdit deStrat;
        private DevExpress.XtraEditors.TextEdit txtNoStrat;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private dcl.client.common.SysToolBar sysToolBar1;
        private dcl.client.common.PrintPreview prpGene;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}