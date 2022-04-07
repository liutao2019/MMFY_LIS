namespace dcl.client.dicbasic
{
    partial class ConDictHarvester
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnWbCode = new DevExpress.XtraEditors.ButtonEdit();
            this.bsHarvester = new System.Windows.Forms.BindingSource(this.components);
            this.btnPyCode = new DevExpress.XtraEditors.ButtonEdit();
            this.btnHdName = new DevExpress.XtraEditors.ButtonEdit();
            this.btnHdCode = new DevExpress.XtraEditors.ButtonEdit();
            this.btnHdId = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDhId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDhCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDhName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWbCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnWbCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHarvester)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPyCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1668, 1100);
            this.splitContainerControl1.SplitterPosition = 889;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnWbCode);
            this.layoutControl1.Controls.Add(this.btnPyCode);
            this.layoutControl1.Controls.Add(this.btnHdName);
            this.layoutControl1.Controls.Add(this.btnHdCode);
            this.layoutControl1.Controls.Add(this.btnHdId);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(3, 33);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(543, 1064);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnWbCode
            // 
            this.btnWbCode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsHarvester, "WbCode", true));
            this.btnWbCode.Enabled = false;
            this.btnWbCode.Location = new System.Drawing.Point(112, 154);
            this.btnWbCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnWbCode.Name = "btnWbCode";
            this.btnWbCode.Size = new System.Drawing.Size(413, 28);
            this.btnWbCode.StyleController = this.layoutControl1;
            this.btnWbCode.TabIndex = 8;
            // 
            // bsHarvester
            // 
            this.bsHarvester.DataSource = typeof(dcl.entity.EntityDictHarvester);
            // 
            // btnPyCode
            // 
            this.btnPyCode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsHarvester, "PyCode", true));
            this.btnPyCode.Enabled = false;
            this.btnPyCode.Location = new System.Drawing.Point(112, 120);
            this.btnPyCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPyCode.Name = "btnPyCode";
            this.btnPyCode.Size = new System.Drawing.Size(413, 28);
            this.btnPyCode.StyleController = this.layoutControl1;
            this.btnPyCode.TabIndex = 7;
            // 
            // btnHdName
            // 
            this.btnHdName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsHarvester, "DhName", true));
            this.btnHdName.Location = new System.Drawing.Point(112, 86);
            this.btnHdName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHdName.Name = "btnHdName";
            this.btnHdName.Size = new System.Drawing.Size(413, 28);
            this.btnHdName.StyleController = this.layoutControl1;
            this.btnHdName.TabIndex = 6;
            this.btnHdName.Leave += new System.EventHandler(this.btnHdName_Leave);
            // 
            // btnHdCode
            // 
            this.btnHdCode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsHarvester, "DhCode", true));
            this.btnHdCode.Location = new System.Drawing.Point(112, 52);
            this.btnHdCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHdCode.Name = "btnHdCode";
            this.btnHdCode.Size = new System.Drawing.Size(413, 28);
            this.btnHdCode.StyleController = this.layoutControl1;
            this.btnHdCode.TabIndex = 5;
            // 
            // btnHdId
            // 
            this.btnHdId.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsHarvester, "DhId", true));
            this.btnHdId.Enabled = false;
            this.btnHdId.Location = new System.Drawing.Point(112, 18);
            this.btnHdId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHdId.Name = "btnHdId";
            this.btnHdId.Size = new System.Drawing.Size(413, 28);
            this.btnHdId.StyleController = this.layoutControl1;
            this.btnHdId.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlGroup1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 4;
            this.layoutControlGroup1.Size = new System.Drawing.Size(543, 1064);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnHdId;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(513, 34);
            this.layoutControlItem1.Text = "标识ID";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 22);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnHdCode;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(513, 34);
            this.layoutControlItem2.Text = "采集器编码";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(90, 22);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnHdName;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 68);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(513, 34);
            this.layoutControlItem3.Text = "采集器名称";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(90, 22);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnPyCode;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 102);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(513, 34);
            this.layoutControlItem4.Text = "拼音码";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(90, 22);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnWbCode;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 136);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(513, 898);
            this.layoutControlItem5.Text = "五笔码";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(90, 22);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsHarvester;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1111, 1100);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDhId,
            this.colDhCode,
            this.colDhName,
            this.colPyCode,
            this.colWbCode});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colDhId
            // 
            this.colDhId.Caption = "标识ID";
            this.colDhId.FieldName = "DhId";
            this.colDhId.Name = "colDhId";
            this.colDhId.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "DhId", "记录总数:{0:0.##}")});
            this.colDhId.Visible = true;
            this.colDhId.VisibleIndex = 0;
            // 
            // colDhCode
            // 
            this.colDhCode.Caption = "采集器编码";
            this.colDhCode.FieldName = "DhCode";
            this.colDhCode.Name = "colDhCode";
            this.colDhCode.Visible = true;
            this.colDhCode.VisibleIndex = 1;
            // 
            // colDhName
            // 
            this.colDhName.Caption = "采集器名称";
            this.colDhName.FieldName = "DhName";
            this.colDhName.Name = "colDhName";
            this.colDhName.Visible = true;
            this.colDhName.VisibleIndex = 2;
            // 
            // colPyCode
            // 
            this.colPyCode.Caption = "拼音码";
            this.colPyCode.FieldName = "PyCode";
            this.colPyCode.Name = "colPyCode";
            this.colPyCode.Visible = true;
            this.colPyCode.VisibleIndex = 3;
            // 
            // colWbCode
            // 
            this.colWbCode.Caption = "五笔码";
            this.colWbCode.FieldName = "WbCode";
            this.colWbCode.Name = "colWbCode";
            this.colWbCode.Visible = true;
            this.colWbCode.VisibleIndex = 4;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(549, 1100);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "基本信息";
            // 
            // ConDictHarvester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "ConDictHarvester";
            this.Size = new System.Drawing.Size(1668, 1100);
            this.Load += new System.EventHandler(this.ConDictHarvester_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnWbCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHarvester)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPyCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHdId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ButtonEdit btnWbCode;
        private DevExpress.XtraEditors.ButtonEdit btnPyCode;
        private DevExpress.XtraEditors.ButtonEdit btnHdName;
        private DevExpress.XtraEditors.ButtonEdit btnHdCode;
        private DevExpress.XtraEditors.ButtonEdit btnHdId;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.BindingSource bsHarvester;
        private DevExpress.XtraGrid.Columns.GridColumn colDhId;
        private DevExpress.XtraGrid.Columns.GridColumn colDhCode;
        private DevExpress.XtraGrid.Columns.GridColumn colDhName;
        private DevExpress.XtraGrid.Columns.GridColumn colPyCode;
        private DevExpress.XtraGrid.Columns.GridColumn colWbCode;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}
