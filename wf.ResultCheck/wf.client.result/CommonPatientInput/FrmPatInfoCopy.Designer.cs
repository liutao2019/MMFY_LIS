namespace dcl.client.result.CommonPatientInput
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
            this.txtPatDate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudNwSid = new System.Windows.Forms.NumericUpDown();
            this.cbNewSid = new System.Windows.Forms.CheckBox();
            this.txtPatInstructment = new dcl.client.control.SelectDicInstrument();
            this.xtpMoreToMore = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.xtpOneToMore = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtPatDate1 = new DevExpress.XtraEditors.DateEdit();
            this.txtPatInstructment1 = new dcl.client.control.SelectDicInstrument();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numEnd = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numStart = new System.Windows.Forms.NumericUpDown();
            this.cbNewStartEnd = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNwSid)).BeginInit();
            this.xtpMoreToMore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.xtpOneToMore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate1.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 321);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(305, 77);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);
            // 
            // txtPatDate
            // 
            this.txtPatDate.EditValue = null;
            this.txtPatDate.EnterMoveNextControl = true;
            this.txtPatDate.Location = new System.Drawing.Point(85, 30);
            this.txtPatDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatDate.Name = "txtPatDate";
            this.txtPatDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtPatDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPatDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPatDate.Size = new System.Drawing.Size(176, 24);
            this.txtPatDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "仪器";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudNwSid);
            this.groupBox1.Controls.Add(this.cbNewSid);
            this.groupBox1.Location = new System.Drawing.Point(39, 144);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(222, 84);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // nudNwSid
            // 
            this.nudNwSid.Enabled = false;
            this.nudNwSid.Location = new System.Drawing.Point(46, 35);
            this.nudNwSid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudNwSid.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.nudNwSid.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNwSid.Name = "nudNwSid";
            this.nudNwSid.Size = new System.Drawing.Size(168, 26);
            this.nudNwSid.TabIndex = 1;
            this.nudNwSid.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbNewSid
            // 
            this.cbNewSid.AutoSize = true;
            this.cbNewSid.Location = new System.Drawing.Point(8, -1);
            this.cbNewSid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbNewSid.Name = "cbNewSid";
            this.cbNewSid.Size = new System.Drawing.Size(150, 22);
            this.cbNewSid.TabIndex = 0;
            this.cbNewSid.Text = "自定义起始样本号";
            this.cbNewSid.UseVisualStyleBackColor = true;
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
            this.txtPatInstructment.Location = new System.Drawing.Point(85, 92);
            this.txtPatInstructment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatInstructment.MaximumSize = new System.Drawing.Size(571, 27);
            this.txtPatInstructment.MinimumSize = new System.Drawing.Size(57, 27);
            this.txtPatInstructment.Name = "txtPatInstructment";
            this.txtPatInstructment.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatInstructment.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatInstructment.Readonly = false;
            this.txtPatInstructment.SaveSourceID = false;
            this.txtPatInstructment.SelectFilter = null;
            this.txtPatInstructment.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatInstructment.SelectOnly = true;
            this.txtPatInstructment.ShowAllInstrmt = false;
            this.txtPatInstructment.Size = new System.Drawing.Size(176, 27);
            this.txtPatInstructment.TabIndex = 7;
            this.txtPatInstructment.UseExtend = false;
            this.txtPatInstructment.valueMember = null;
            // 
            // xtpMoreToMore
            // 
            this.xtpMoreToMore.Controls.Add(this.panelControl2);
            this.xtpMoreToMore.Name = "xtpMoreToMore";
            this.xtpMoreToMore.Size = new System.Drawing.Size(298, 285);
            this.xtpMoreToMore.Text = "批量对照批量";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtPatDate);
            this.panelControl2.Controls.Add(this.txtPatInstructment);
            this.panelControl2.Controls.Add(this.label1);
            this.panelControl2.Controls.Add(this.groupBox1);
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(298, 285);
            this.panelControl2.TabIndex = 1;
            // 
            // xtpOneToMore
            // 
            this.xtpOneToMore.Controls.Add(this.panelControl1);
            this.xtpOneToMore.Name = "xtpOneToMore";
            this.xtpOneToMore.Size = new System.Drawing.Size(298, 285);
            this.xtpOneToMore.Text = "单个对照批量";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtPatDate1);
            this.panelControl1.Controls.Add(this.txtPatInstructment1);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.groupBox2);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(298, 285);
            this.panelControl1.TabIndex = 0;
            // 
            // txtPatDate1
            // 
            this.txtPatDate1.EditValue = null;
            this.txtPatDate1.EnterMoveNextControl = true;
            this.txtPatDate1.Location = new System.Drawing.Point(83, 25);
            this.txtPatDate1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatDate1.Name = "txtPatDate1";
            this.txtPatDate1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtPatDate1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPatDate1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPatDate1.Size = new System.Drawing.Size(176, 24);
            this.txtPatDate1.TabIndex = 8;
            // 
            // txtPatInstructment1
            // 
            this.txtPatInstructment1.AddEmptyRow = true;
            this.txtPatInstructment1.BindByValue = false;
            this.txtPatInstructment1.colDisplay = "";
            this.txtPatInstructment1.colExtend1 = null;
            this.txtPatInstructment1.colInCode = "";
            this.txtPatInstructment1.colPY = "";
            this.txtPatInstructment1.colSeq = "";
            this.txtPatInstructment1.colValue = "";
            this.txtPatInstructment1.colWB = "";
            this.txtPatInstructment1.displayMember = null;
            this.txtPatInstructment1.EnterMoveNext = true;
            this.txtPatInstructment1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtPatInstructment1.KeyUpDownMoveNext = false;
            this.txtPatInstructment1.LoadDataOnDesignMode = true;
            this.txtPatInstructment1.Location = new System.Drawing.Point(83, 89);
            this.txtPatInstructment1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatInstructment1.MaximumSize = new System.Drawing.Size(571, 27);
            this.txtPatInstructment1.MinimumSize = new System.Drawing.Size(57, 27);
            this.txtPatInstructment1.Name = "txtPatInstructment1";
            this.txtPatInstructment1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatInstructment1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatInstructment1.Readonly = false;
            this.txtPatInstructment1.SaveSourceID = false;
            this.txtPatInstructment1.SelectFilter = null;
            this.txtPatInstructment1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatInstructment1.SelectOnly = true;
            this.txtPatInstructment1.ShowAllInstrmt = false;
            this.txtPatInstructment1.Size = new System.Drawing.Size(176, 27);
            this.txtPatInstructment1.TabIndex = 12;
            this.txtPatInstructment1.UseExtend = false;
            this.txtPatInstructment1.valueMember = null;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "日期";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numEnd);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numStart);
            this.groupBox2.Controls.Add(this.cbNewStartEnd);
            this.groupBox2.Location = new System.Drawing.Point(37, 139);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(222, 126);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "末";
            // 
            // numEnd
            // 
            this.numEnd.Enabled = false;
            this.numEnd.Location = new System.Drawing.Point(46, 78);
            this.numEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numEnd.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.numEnd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size(168, 26);
            this.numEnd.TabIndex = 12;
            this.numEnd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "始";
            // 
            // numStart
            // 
            this.numStart.Enabled = false;
            this.numStart.Location = new System.Drawing.Point(46, 35);
            this.numStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numStart.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.numStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(168, 26);
            this.numStart.TabIndex = 1;
            this.numStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbNewStartEnd
            // 
            this.cbNewStartEnd.AutoSize = true;
            this.cbNewStartEnd.Location = new System.Drawing.Point(8, -1);
            this.cbNewStartEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbNewStartEnd.Name = "cbNewStartEnd";
            this.cbNewStartEnd.Size = new System.Drawing.Size(150, 22);
            this.cbNewStartEnd.TabIndex = 0;
            this.cbNewStartEnd.Text = "自定义起末样本号";
            this.cbNewStartEnd.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "仪器";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtpOneToMore;
            this.xtraTabControl1.Size = new System.Drawing.Size(305, 321);
            this.xtraTabControl1.TabIndex = 8;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtpOneToMore,
            this.xtpMoreToMore});
            // 
            // FrmPatInfoCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 398);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.sysToolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmPatInfoCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "资料批量复制";
            this.Load += new System.EventHandler(this.FrmPatInfoCopy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNwSid)).EndInit();
            this.xtpMoreToMore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.xtpOneToMore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate1.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar sysToolBar1;
        protected DevExpress.XtraEditors.DateEdit txtPatDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbNewSid;
        private System.Windows.Forms.NumericUpDown nudNwSid;
        private control.SelectDicInstrument txtPatInstructment;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabPage xtpOneToMore;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraTab.XtraTabPage xtpMoreToMore;
        protected DevExpress.XtraEditors.DateEdit txtPatDate1;
        private control.SelectDicInstrument txtPatInstructment1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbNewStartEnd;
        private System.Windows.Forms.NumericUpDown numStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numEnd;
        private System.Windows.Forms.Label label5;
    }
}