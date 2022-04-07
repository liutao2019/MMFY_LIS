namespace dcl.client.tools
{
    partial class FrmTempHandle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTempHandle));
            this.selectDicLabProfession1 = new dcl.client.control.SelectDicLabProfession();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.JKDate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.picPanel = new DevExpress.XtraEditors.XtraScrollableControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JKDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JKDate.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectDicLabProfession1
            // 
            this.selectDicLabProfession1.AddEmptyRow = true;
            this.selectDicLabProfession1.BindByValue = false;
            this.selectDicLabProfession1.colDisplay = "";
            this.selectDicLabProfession1.colExtend1 = null;
            this.selectDicLabProfession1.colInCode = "";
            this.selectDicLabProfession1.colPY = "";
            this.selectDicLabProfession1.colSeq = "";
            this.selectDicLabProfession1.colValue = "";
            this.selectDicLabProfession1.colWB = "";
            this.selectDicLabProfession1.displayMember = null;
            this.selectDicLabProfession1.EnterMoveNext = true;
            this.selectDicLabProfession1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicLabProfession1.KeyUpDownMoveNext = false;
            this.selectDicLabProfession1.LoadDataOnDesignMode = true;
            this.selectDicLabProfession1.Location = new System.Drawing.Point(132, 63);
            this.selectDicLabProfession1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.selectDicLabProfession1.MaximumSize = new System.Drawing.Size(928, 44);
            this.selectDicLabProfession1.MinimumSize = new System.Drawing.Size(93, 44);
            this.selectDicLabProfession1.Name = "selectDicLabProfession1";
            this.selectDicLabProfession1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicLabProfession1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicLabProfession1.Readonly = false;
            this.selectDicLabProfession1.SaveSourceID = false;
            this.selectDicLabProfession1.SelectFilter = null;
            this.selectDicLabProfession1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicLabProfession1.SelectOnly = true;
            this.selectDicLabProfession1.Size = new System.Drawing.Size(294, 44);
            this.selectDicLabProfession1.TabIndex = 5;
            this.selectDicLabProfession1.UseExtend = false;
            this.selectDicLabProfession1.valueMember = null;
            this.selectDicLabProfession1.onAfterChange += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.afterChange(this.selectDicLabProfession1_onAfterChange);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "实验组";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(983, 63);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(101, 36);
            this.numericUpDown1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(871, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "监控跨度";
            // 
            // JKDate
            // 
            this.JKDate.EditValue = null;
            this.JKDate.Location = new System.Drawing.Point(585, 63);
            this.JKDate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.JKDate.Name = "JKDate";
            this.JKDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.JKDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.JKDate.Size = new System.Drawing.Size(236, 36);
            this.JKDate.TabIndex = 1;
            this.JKDate.EditValueChanged += new System.EventHandler(this.JKDate_EditValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "监控日期";
            // 
            // picPanel
            // 
            this.picPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picPanel.Appearance.Options.UseBackColor = true;
            this.picPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPanel.Location = new System.Drawing.Point(3, 130);
            this.picPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.picPanel.Name = "picPanel";
            this.picPanel.Size = new System.Drawing.Size(1567, 650);
            this.picPanel.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sysToolBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1573, 131);
            this.panel3.TabIndex = 3;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(8, 11, 8, 11);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1573, 131);
            this.sysToolBar1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.picPanel);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 131);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1573, 783);
            this.panelControl1.TabIndex = 4;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.selectDicLabProfession1);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.numericUpDown1);
            this.groupControl1.Controls.Add(this.JKDate);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1567, 127);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "监控条件";
            // 
            // FrmTempHandle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1573, 914);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmTempHandle";
            this.Text = "温控";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTempHandle_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JKDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JKDate.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.DateEdit JKDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private control.SelectDicLabProfession selectDicLabProfession1;
        private DevExpress.XtraEditors.XtraScrollableControl picPanel;
        private System.Windows.Forms.Panel panel3;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}