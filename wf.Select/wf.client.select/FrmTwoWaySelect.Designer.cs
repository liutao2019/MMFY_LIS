namespace dcl.client.resultquery
{
    partial class FrmTwoWaySelect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTwoWaySelect));
            this.gcBar = new DevExpress.XtraGrid.GridControl();
            this.bsSampDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gvBar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkSet = new DevExpress.XtraEditors.CheckEdit();
            this.ckClear = new DevExpress.XtraEditors.CheckEdit();
            this.gcTW = new DevExpress.XtraGrid.GridControl();
            this.bsTwoWay = new System.Windows.Forms.BindingSource(this.components);
            this.gvTW = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sysItem = new dcl.client.common.SysToolBar();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.splitContainerControl6 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.lueInstrmt = new dcl.client.control.SelectDicInstrument();
            this.txtBarCode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSampDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckClear.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTwoWay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl6)).BeginInit();
            this.splitContainerControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcBar
            // 
            this.gcBar.DataSource = this.bsSampDetail;
            this.gcBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBar.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcBar.Location = new System.Drawing.Point(2, 27);
            this.gcBar.MainView = this.gvBar;
            this.gcBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcBar.Name = "gcBar";
            this.gcBar.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcBar.Size = new System.Drawing.Size(303, 360);
            this.gcBar.TabIndex = 6;
            this.gcBar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBar,
            this.gridView4});
            // 
            // bsSampDetail
            // 
            this.bsSampDetail.DataSource = typeof(dcl.entity.EntitySampDetail);
            // 
            // gvBar
            // 
            this.gvBar.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn10});
            this.gvBar.GridControl = this.gcBar;
            this.gvBar.Name = "gvBar";
            this.gvBar.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvBar.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "条码号";
            this.gridColumn1.FieldName = "SampBarCode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 78;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "姓名";
            this.gridColumn2.FieldName = "PidName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 62;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "组合项目";
            this.gridColumn3.FieldName = "OrderName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 74;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "HIS编码";
            this.gridColumn4.FieldName = "OrderCode";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 72;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "上机标志";
            this.gridColumn10.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn10.FieldName = "CommFlag";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 4;
            this.gridColumn10.Width = 71;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueChecked = 1;
            this.repositoryItemCheckEdit1.ValueUnchecked = 0;
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.gcBar;
            this.gridView4.Name = "gridView4";
            // 
            // chkSet
            // 
            this.chkSet.Location = new System.Drawing.Point(132, 37);
            this.chkSet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSet.Name = "chkSet";
            this.chkSet.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSet.Properties.Appearance.Options.UseFont = true;
            this.chkSet.Properties.Caption = "设置上机";
            this.chkSet.Size = new System.Drawing.Size(116, 28);
            this.chkSet.TabIndex = 6;
            this.chkSet.CheckedChanged += new System.EventHandler(this.chkSet_CheckedChanged);
            // 
            // ckClear
            // 
            this.ckClear.Location = new System.Drawing.Point(14, 37);
            this.ckClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckClear.Name = "ckClear";
            this.ckClear.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckClear.Properties.Appearance.Options.UseFont = true;
            this.ckClear.Properties.Caption = "清除上机";
            this.ckClear.Size = new System.Drawing.Size(110, 28);
            this.ckClear.TabIndex = 4;
            this.ckClear.CheckedChanged += new System.EventHandler(this.ckClear_CheckedChanged);
            // 
            // gcTW
            // 
            this.gcTW.DataSource = this.bsTwoWay;
            this.gcTW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTW.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcTW.Location = new System.Drawing.Point(2, 27);
            this.gcTW.MainView = this.gvTW;
            this.gcTW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcTW.Name = "gcTW";
            this.gcTW.Size = new System.Drawing.Size(867, 546);
            this.gcTW.TabIndex = 0;
            this.gcTW.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTW,
            this.gridView2});
            // 
            // bsTwoWay
            // 
            this.bsTwoWay.DataSource = typeof(dcl.entity.EntitySampDetailMachineCode);
            // 
            // gvTW
            // 
            this.gvTW.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
            this.gvTW.GridControl = this.gcTW;
            this.gvTW.Name = "gvTW";
            this.gvTW.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvTW.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "组合项目";
            this.gridColumn5.FieldName = "OrderName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "项目明细";
            this.gridColumn6.FieldName = "ComItmEname";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "项目编码";
            this.gridColumn7.FieldName = "ComItmId";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "组合编码";
            this.gridColumn8.FieldName = "ComId";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "通道码";
            this.gridColumn9.FieldName = "MacCode";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 4;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcTW;
            this.gridView2.Name = "gridView2";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.sysItem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1184, 81);
            this.panel4.TabIndex = 1;
            // 
            // sysItem
            // 
            this.sysItem.AutoCloseButton = true;
            this.sysItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysItem.Location = new System.Drawing.Point(0, 0);
            this.sysItem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sysItem.Name = "sysItem";
            this.sysItem.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysItem.NotWriteLogButtonNameList")));
            this.sysItem.ShowItemToolTips = false;
            this.sysItem.Size = new System.Drawing.Size(1184, 81);
            this.sysItem.TabIndex = 0;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.gcBar);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 186);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(307, 389);
            this.groupControl3.TabIndex = 2;
            this.groupControl3.Text = "条码";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.chkSet);
            this.groupControl1.Controls.Add(this.ckClear);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(307, 77);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "设置";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.gcTW);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl4.Location = new System.Drawing.Point(0, 0);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(871, 575);
            this.groupControl4.TabIndex = 3;
            this.groupControl4.Text = "条码项目明细";
            // 
            // splitContainerControl6
            // 
            this.splitContainerControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl6.Location = new System.Drawing.Point(0, 81);
            this.splitContainerControl6.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.splitContainerControl6.Name = "splitContainerControl6";
            this.splitContainerControl6.Panel1.Controls.Add(this.groupControl3);
            this.splitContainerControl6.Panel1.Controls.Add(this.groupControl2);
            this.splitContainerControl6.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl6.Panel1.Text = "Panel1";
            this.splitContainerControl6.Panel2.Controls.Add(this.groupControl4);
            this.splitContainerControl6.Panel2.Text = "Panel2";
            this.splitContainerControl6.Size = new System.Drawing.Size(1184, 575);
            this.splitContainerControl6.SplitterPosition = 499;
            this.splitContainerControl6.TabIndex = 4;
            this.splitContainerControl6.Text = "splitContainerControl6";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.lueInstrmt);
            this.groupControl2.Controls.Add(this.txtBarCode);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 77);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(307, 109);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "条件";
            // 
            // lueInstrmt
            // 
            this.lueInstrmt.AddEmptyRow = true;
            this.lueInstrmt.AutoScroll = true;
            this.lueInstrmt.BindByValue = false;
            this.lueInstrmt.colDisplay = "";
            this.lueInstrmt.colExtend1 = null;
            this.lueInstrmt.colInCode = "";
            this.lueInstrmt.colPY = "";
            this.lueInstrmt.colSeq = "";
            this.lueInstrmt.colValue = "";
            this.lueInstrmt.colWB = "";
            this.lueInstrmt.displayMember = null;
            this.lueInstrmt.EnterMoveNext = true;
            this.lueInstrmt.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueInstrmt.KeyUpDownMoveNext = false;
            this.lueInstrmt.LoadDataOnDesignMode = true;
            this.lueInstrmt.Location = new System.Drawing.Point(65, 35);
            this.lueInstrmt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lueInstrmt.MaximumSize = new System.Drawing.Size(572, 27);
            this.lueInstrmt.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueInstrmt.Name = "lueInstrmt";
            this.lueInstrmt.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueInstrmt.PFont = new System.Drawing.Font("Tahoma", 10.5F);
            this.lueInstrmt.Readonly = false;
            this.lueInstrmt.SaveSourceID = false;
            this.lueInstrmt.SelectFilter = null;
            this.lueInstrmt.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueInstrmt.SelectOnly = true;
            this.lueInstrmt.ShowAllInstrmt = false;
            this.lueInstrmt.Size = new System.Drawing.Size(230, 27);
            this.lueInstrmt.TabIndex = 0;
            this.lueInstrmt.UseExtend = false;
            this.lueInstrmt.valueMember = null;
            // 
            // txtBarCode
            // 
            this.txtBarCode.Location = new System.Drawing.Point(65, 73);
            this.txtBarCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtBarCode.Properties.Appearance.Options.UseFont = true;
            this.txtBarCode.Size = new System.Drawing.Size(230, 28);
            this.txtBarCode.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl1.Location = new System.Drawing.Point(23, 35);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 22);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "仪器";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl2.Location = new System.Drawing.Point(4, 76);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 22);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "条码号";
            // 
            // FrmTwoWaySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 656);
            this.Controls.Add(this.splitContainerControl6);
            this.Controls.Add(this.panel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmTwoWaySelect";
            this.Text = "双向上机项目查询";
            this.Load += new System.EventHandler(this.FrmTwoWaySelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSampDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckClear.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTwoWay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl6)).EndInit();
            this.splitContainerControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarCode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gcBar;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBar;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.GridControl gcTW;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTW;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.CheckEdit ckClear;
        private DevExpress.XtraEditors.CheckEdit chkSet;
        private System.Windows.Forms.Panel panel4;
        private dcl.client.common.SysToolBar sysItem;
        private System.Windows.Forms.BindingSource bsSampDetail;
        private System.Windows.Forms.BindingSource bsTwoWay;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl6;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtBarCode;
        private control.SelectDicInstrument lueInstrmt;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}