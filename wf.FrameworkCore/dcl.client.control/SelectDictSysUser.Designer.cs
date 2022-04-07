namespace dcl.client.control
{
  partial class SelectDictSysUser
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
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.bsSource = new System.Windows.Forms.BindingSource();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coluserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colloginId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colwb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colincode = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(130, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 222);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(433, 222);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntitySysUser);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coluserName,
            this.colloginId,
            this.colpy,
            this.colwb,
            this.colincode});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // coluserName
            // 
            this.coluserName.Caption = "姓名";
            this.coluserName.FieldName = "UserName";
            this.coluserName.Name = "coluserName";
            this.coluserName.OptionsColumn.AllowEdit = false;
            this.coluserName.OptionsColumn.AllowFocus = false;
            this.coluserName.OptionsColumn.ReadOnly = true;
            this.coluserName.Visible = true;
            this.coluserName.VisibleIndex = 0;
            // 
            // colloginId
            // 
            this.colloginId.Caption = "登录帐号";
            this.colloginId.FieldName = "UserLoginid";
            this.colloginId.Name = "colloginId";
            this.colloginId.OptionsColumn.AllowEdit = false;
            this.colloginId.OptionsColumn.AllowFocus = false;
            this.colloginId.OptionsColumn.ReadOnly = true;
            this.colloginId.Visible = true;
            this.colloginId.VisibleIndex = 1;
            // 
            // colpy
            // 
            this.colpy.Caption = "拼音";
            this.colpy.FieldName = "PyCode";
            this.colpy.Name = "colpy";
            this.colpy.OptionsColumn.AllowEdit = false;
            this.colpy.OptionsColumn.AllowFocus = false;
            this.colpy.OptionsColumn.ReadOnly = true;
            this.colpy.Visible = true;
            this.colpy.VisibleIndex = 2;
            // 
            // colwb
            // 
            this.colwb.Caption = "五笔";
            this.colwb.FieldName = "WbCode";
            this.colwb.Name = "colwb";
            this.colwb.OptionsColumn.AllowEdit = false;
            this.colwb.OptionsColumn.AllowFocus = false;
            this.colwb.OptionsColumn.ReadOnly = true;
            this.colwb.Visible = true;
            this.colwb.VisibleIndex = 3;
            // 
            // colincode
            // 
            this.colincode.Caption = "输入码";
            this.colincode.FieldName = "UserIncode";
            this.colincode.Name = "colincode";
            this.colincode.OptionsColumn.AllowEdit = false;
            this.colincode.OptionsColumn.AllowFocus = false;
            this.colincode.OptionsColumn.ReadOnly = true;
            this.colincode.Visible = true;
            this.colincode.VisibleIndex = 4;
            // 
            // SelectDictSysUser
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "WbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "SortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "PyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "UserIncode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "UserName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "UserLoginid", true));
            this.Name = "SelectDictSysUser";
            this.Size = new System.Drawing.Size(130, 21);
            this.Leave += new System.EventHandler(this.SelectDict_inspect_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    private System.Windows.Forms.BindingSource bsSource;

    private DevExpress.XtraGrid.Columns.GridColumn coluserName;
    private DevExpress.XtraGrid.Columns.GridColumn colloginId;
    private DevExpress.XtraGrid.Columns.GridColumn colpy;
    private DevExpress.XtraGrid.Columns.GridColumn colwb;
    private DevExpress.XtraGrid.Columns.GridColumn colincode;

  }
}
