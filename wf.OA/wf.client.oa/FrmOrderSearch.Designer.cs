namespace dcl.client.oa
{
    partial class FrmOrderSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOrderSearch));
            this.tvOrderDetail = new DevExpress.XtraTreeList.TreeList();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.timeTo = new DevExpress.XtraEditors.DateEdit();
            this.timeFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboOrderType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOrderType.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvOrderDetail
            // 
            this.tvOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrderDetail.KeyFieldName = "OrderCode";
            this.tvOrderDetail.Location = new System.Drawing.Point(325, 2);
            this.tvOrderDetail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvOrderDetail.Name = "tvOrderDetail";
            this.tvOrderDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvOrderDetail.OptionsView.AutoWidth = false;
            this.tvOrderDetail.OptionsView.ShowRoot = false;
            this.tvOrderDetail.ParentFieldName = "";
            this.tvOrderDetail.Size = new System.Drawing.Size(864, 756);
            this.tvOrderDetail.TabIndex = 7;
            this.tvOrderDetail.TreeLevelWidth = 12;
            this.tvOrderDetail.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            // 
            // pnlDetail
            // 
            this.pnlDetail.AutoScroll = true;
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(12, 37);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(295, 564);
            this.pnlDetail.TabIndex = 7;
            // 
            // timeTo
            // 
            this.timeTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTo.EditValue = null;
            this.timeTo.Location = new System.Drawing.Point(97, 101);
            this.timeTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeTo.Name = "timeTo";
            this.timeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeTo.Size = new System.Drawing.Size(210, 24);
            this.timeTo.TabIndex = 15;
            this.timeTo.TextChanged += new System.EventHandler(this.timeFrom_TextChanged);
            // 
            // timeFrom
            // 
            this.timeFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeFrom.EditValue = null;
            this.timeFrom.Location = new System.Drawing.Point(97, 69);
            this.timeFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeFrom.Size = new System.Drawing.Size(210, 24);
            this.timeFrom.TabIndex = 14;
            this.timeFrom.TextChanged += new System.EventHandler(this.timeFrom_TextChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(77, 103);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(15, 18);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "到";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 71);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(80, 18);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "保存时间 从";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(62, 39);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "分类";
            // 
            // cboOrderType
            // 
            this.cboOrderType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOrderType.Location = new System.Drawing.Point(97, 37);
            this.cboOrderType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboOrderType.Name = "cboOrderType";
            this.cboOrderType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboOrderType.Size = new System.Drawing.Size(210, 24);
            this.cboOrderType.TabIndex = 1;
            this.cboOrderType.SelectedIndexChanged += new System.EventHandler(this.cboOrderType_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.sysToolBar1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1191, 81);
            this.panel4.TabIndex = 4;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1191, 81);
            this.sysToolBar1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tvOrderDetail);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 81);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1191, 760);
            this.panelControl1.TabIndex = 5;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl2);
            this.panelControl2.Controls.Add(this.groupControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(323, 756);
            this.panelControl2.TabIndex = 5;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.timeTo);
            this.groupControl1.Controls.Add(this.cboOrderType);
            this.groupControl1.Controls.Add(this.timeFrom);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(319, 139);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "统计条件";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.pnlDetail);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(2, 141);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(10);
            this.groupControl2.Size = new System.Drawing.Size(319, 613);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "查询条件";
            // 
            // FrmOrderSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 841);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel4);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmOrderSearch";
            this.Text = "科室事务统计";
            this.Load += new System.EventHandler(this.FrmOrderSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOrderType.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.TreeList tvOrderDetail;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboOrderType;
        private System.Windows.Forms.Panel pnlDetail;
        private DevExpress.XtraEditors.DateEdit timeTo;
        private DevExpress.XtraEditors.DateEdit timeFrom;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Panel panel4;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}