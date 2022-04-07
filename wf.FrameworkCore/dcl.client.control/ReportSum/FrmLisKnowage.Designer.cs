using System;
using DevExpress.XtraGrid.Views.Base;

namespace dcl.client.control
{
    partial class FrmLisKnowage
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcCombine = new DevExpress.XtraGrid.GridControl();
            this.gvCombine = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc = new DevExpress.XtraEditors.GroupControl();
            this.gcItem = new DevExpress.XtraGrid.GridControl();
            this.gvItem = new DevExpress.XtraGrid.Views.Card.CardView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCombine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCombine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).BeginInit();
            this.gc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gcCombine);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gc);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(982, 638);
            this.splitContainerControl1.SplitterPosition = 322;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcCombine
            // 
            this.gcCombine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCombine.Location = new System.Drawing.Point(0, 0);
            this.gcCombine.MainView = this.gvCombine;
            this.gcCombine.Name = "gcCombine";
            this.gcCombine.Size = new System.Drawing.Size(322, 638);
            this.gcCombine.TabIndex = 0;
            this.gcCombine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCombine});
            // 
            // gvCombine
            // 
            this.gvCombine.ColumnPanelRowHeight = 30;
            this.gvCombine.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvCombine.GridControl = this.gcCombine;
            this.gvCombine.GroupCount = 1;
            this.gvCombine.GroupFormat = "[#image]{1} {2}";
            this.gvCombine.Name = "gvCombine";
            this.gvCombine.OptionsBehavior.Editable = false;
            this.gvCombine.OptionsCustomization.AllowRowSizing = true;
            this.gvCombine.OptionsFind.AlwaysVisible = true;
            this.gvCombine.OptionsFind.ShowClearButton = false;
            this.gvCombine.OptionsFind.ShowFindButton = false;
            this.gvCombine.OptionsView.ShowGroupPanel = false;
            this.gvCombine.OptionsView.ShowIndicator = false;
            this.gvCombine.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvCombine.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCombine_FocusedRowChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "  ";
            this.gridColumn1.FieldName = "ComName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowSize = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "专业组";
            this.gridColumn2.FieldName = "PTypeName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsColumn.AllowMove = false;
            this.gridColumn2.OptionsColumn.AllowSize = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gc
            // 
            this.gc.Controls.Add(this.gcItem);
            this.gc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc.Location = new System.Drawing.Point(0, 0);
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(655, 638);
            this.gc.TabIndex = 0;
            // 
            // gcItem
            // 
            this.gcItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcItem.Location = new System.Drawing.Point(2, 21);
            this.gcItem.MainView = this.gvItem;
            this.gcItem.Name = "gcItem";
            this.gcItem.Size = new System.Drawing.Size(651, 615);
            this.gcItem.TabIndex = 0;
            this.gcItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItem,
            this.gridView2});
            this.gcItem.DoubleClick += new System.EventHandler(this.gcItem_DoubleClick);
            // 
            // gvItem
            // 
            this.gvItem.CardCaptionFormat = "{4}({5})";
            this.gvItem.CardInterval = 20;
            this.gvItem.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvItem.DetailHeight = 450;
            this.gvItem.FocusedCardTopFieldIndex = 0;
            this.gvItem.GridControl = this.gcItem;
            this.gvItem.MaximumCardColumns = 1;
            this.gvItem.MaximumCardRows = 6;
            this.gvItem.Name = "gvItem";
            this.gvItem.OptionsBehavior.AutoHorzWidth = true;
            this.gvItem.OptionsView.ShowQuickCustomizeButton = false;
            this.gvItem.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvItem.ColumnFilterChanged += new System.EventHandler(this.gvItem_ColumnFilterChanged);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "参考范围";
            this.gridColumn3.FieldName = "ItmRef";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.ToolTip = "双击查看明细";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "项目意义";
            this.gridColumn4.FieldName = "ItmMeaning";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsColumn.ReadOnly = true;
            this.gridColumn4.ToolTip = "双击查看明细";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "检查目的";
            this.gridColumn5.FieldName = "ItmContent";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsColumn.ReadOnly = true;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcItem;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // FrmLisKnowage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 638);
            this.Controls.Add(this.splitContainerControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmLisKnowage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLisKnowage";
            this.Load += new System.EventHandler(this.FrmLisKnowage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCombine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCombine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).EndInit();
            this.gc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gcCombine;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCombine;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.GroupControl gc;
        private DevExpress.XtraGrid.GridControl gcItem;
        private DevExpress.XtraGrid.Views.Card.CardView gvItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    }
}
