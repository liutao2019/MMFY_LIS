namespace dcl.client.oa
{
    partial class OrderDetail
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDetail));
            this.tvOrderDetail = new DevExpress.XtraTreeList.TreeList();
            this.dateEditEnd = new DevExpress.XtraEditors.DateEdit();
            this.dateEditStart = new DevExpress.XtraEditors.DateEdit();
            this.pnlSearch = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcDetail = new DevExpress.XtraEditors.GroupControl();
            this.pnlDetail = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.barOrderDetail = new dcl.client.common.SysToolBar();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).BeginInit();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            this.gcDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvOrderDetail
            // 
            this.tvOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrderDetail.KeyFieldName = "OrderCode";
            this.tvOrderDetail.Location = new System.Drawing.Point(0, 38);
            this.tvOrderDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tvOrderDetail.Name = "tvOrderDetail";
            this.tvOrderDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvOrderDetail.OptionsView.AutoWidth = false;
            this.tvOrderDetail.OptionsView.ShowRoot = false;
            this.tvOrderDetail.ParentFieldName = "";
            this.tvOrderDetail.Size = new System.Drawing.Size(487, 416);
            this.tvOrderDetail.TabIndex = 6;
            this.tvOrderDetail.TreeLevelWidth = 12;
            this.tvOrderDetail.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tvOrderDetail.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.tvOrderDetail_BeforeFocusNode);
            this.tvOrderDetail.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tvOrderDetail_FocusedNodeChanged);
            // 
            // dateEditEnd
            // 
            this.dateEditEnd.EditValue = null;
            this.dateEditEnd.Location = new System.Drawing.Point(291, 9);
            this.dateEditEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateEditEnd.Name = "dateEditEnd";
            this.dateEditEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditEnd.Size = new System.Drawing.Size(111, 26);
            this.dateEditEnd.TabIndex = 4;
            // 
            // dateEditStart
            // 
            this.dateEditStart.EditValue = null;
            this.dateEditStart.Location = new System.Drawing.Point(87, 9);
            this.dateEditStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateEditStart.Name = "dateEditStart";
            this.dateEditStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditStart.Size = new System.Drawing.Size(119, 26);
            this.dateEditStart.TabIndex = 3;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.labelControl2);
            this.pnlSearch.Controls.Add(this.labelControl1);
            this.pnlSearch.Controls.Add(this.dateEditEnd);
            this.pnlSearch.Controls.Add(this.dateEditStart);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(487, 38);
            this.pnlSearch.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(218, 11);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "创建结束日期";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 11);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "创建开始日期";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 3);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.tvOrderDetail);
            this.splitContainerControl1.Panel1.Controls.Add(this.pnlSearch);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gcDetail);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(963, 454);
            this.splitContainerControl1.SplitterPosition = 649;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcDetail
            // 
            this.gcDetail.Controls.Add(this.pnlDetail);
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.Location = new System.Drawing.Point(0, 0);
            this.gcDetail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(468, 454);
            this.gcDetail.TabIndex = 0;
            this.gcDetail.Text = "详细信息";
            // 
            // pnlDetail
            // 
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(3, 30);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(462, 421);
            this.pnlDetail.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.barOrderDetail);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(969, 56);
            this.panelControl1.TabIndex = 5;
            // 
            // barOrderDetail
            // 
            this.barOrderDetail.AutoCloseButton = true;
            this.barOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barOrderDetail.Location = new System.Drawing.Point(3, 3);
            this.barOrderDetail.Margin = new System.Windows.Forms.Padding(0);
            this.barOrderDetail.Name = "barOrderDetail";
            this.barOrderDetail.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("barOrderDetail.NotWriteLogButtonNameList")));
            this.barOrderDetail.ShowItemToolTips = false;
            this.barOrderDetail.Size = new System.Drawing.Size(963, 50);
            this.barOrderDetail.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 56);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(969, 460);
            this.panelControl2.TabIndex = 6;
            // 
            // OrderDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "OrderDetail";
            this.Size = new System.Drawing.Size(969, 516);
            this.Load += new System.EventHandler(this.OrderDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            this.gcDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.TreeList tvOrderDetail;
        private DevExpress.XtraEditors.DateEdit dateEditEnd;
        private DevExpress.XtraEditors.DateEdit dateEditStart;
        private DevExpress.XtraEditors.PanelControl pnlSearch;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl gcDetail;
        private DevExpress.XtraEditors.PanelControl pnlDetail;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private common.SysToolBar barOrderDetail;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}
