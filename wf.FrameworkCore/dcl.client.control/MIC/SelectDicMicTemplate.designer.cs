﻿namespace dcl.client.control
{
    partial class SelectDicMicTemplate
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
            this.bsSource = new System.Windows.Forms.BindingSource();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTmpType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTmpContent = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(130, 24);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(308, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicMicTemplate);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(308, 222);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTmpType,
            this.colTmpContent});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colTmpType
            // 
            this.colTmpType.Caption = "模板类型";
            this.colTmpType.FieldName = "TmpType";
            this.colTmpType.Name = "colTmpType";
            this.colTmpType.OptionsColumn.AllowEdit = false;
            this.colTmpType.OptionsColumn.AllowFocus = false;
            this.colTmpType.OptionsColumn.ReadOnly = true;
            // 
            // colTmpContent
            // 
            this.colTmpContent.Caption = "模板内容";
            this.colTmpContent.FieldName = "TmpContent";
            this.colTmpContent.Name = "colTmpContent";
            this.colTmpContent.OptionsColumn.AllowEdit = false;
            this.colTmpContent.OptionsColumn.AllowFocus = false;
            this.colTmpContent.OptionsColumn.ReadOnly = true;
            this.colTmpContent.Visible = true;
            this.colTmpContent.VisibleIndex = 0;
            // 
            // SelectDicMicTemplate
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "TmpContent", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "TmpContent", true));
            this.Name = "SelectDicMicTemplate";
            this.Size = new System.Drawing.Size(130, 21);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource bsSource;
    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    //private lis.client.control.dsBasicTableAdapters.dict_departTableAdapter dict_departTableAdapter;
    private DevExpress.XtraGrid.Columns.GridColumn colTmpContent;
    private DevExpress.XtraGrid.Columns.GridColumn colTmpType;
    //private lis.client.control.dsBasicTableAdapters.dict_checkbTableAdapter dict_checkbTableAdapter;

  }
}
