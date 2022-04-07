using dcl.entity;

namespace dcl.client.qc
{
    partial class FrmReagentsCompare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReagentsCompare));
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lue_Apparatus = new dcl.client.control.SelectDicInstrument();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dtBegin = new DevExpress.XtraEditors.DateEdit();
            this.dtEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.tlQcItem = new DevExpress.XtraTreeList.TreeList();
            this.tlcItem = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.dtCompareDate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtSID = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcCompareData = new DevExpress.XtraGrid.GridControl();
            this.gvCompareData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ckEffective = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlQcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtCompareDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCompareDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCompareData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompareData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            this.SuspendLayout();
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
            this.lueType.Location = new System.Drawing.Point(498, 40);
            this.lueType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueType.MaximumSize = new System.Drawing.Size(571, 27);
            this.lueType.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = true;
            this.lueType.Size = new System.Drawing.Size(143, 27);
            this.lueType.TabIndex = 40;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = null;
            this.lueType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.lueType_ValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl2.Location = new System.Drawing.Point(438, 41);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 22);
            this.labelControl2.TabIndex = 47;
            this.labelControl2.Text = "实验组";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl3.Location = new System.Drawing.Point(661, 41);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 22);
            this.labelControl3.TabIndex = 44;
            this.labelControl3.Text = "测定仪器";
            // 
            // lue_Apparatus
            // 
            this.lue_Apparatus.AddEmptyRow = true;
            this.lue_Apparatus.BindByValue = false;
            this.lue_Apparatus.colDisplay = "";
            this.lue_Apparatus.colExtend1 = null;
            this.lue_Apparatus.colInCode = "";
            this.lue_Apparatus.colPY = "";
            this.lue_Apparatus.colSeq = "";
            this.lue_Apparatus.colValue = "";
            this.lue_Apparatus.colWB = "";
            this.lue_Apparatus.displayMember = null;
            this.lue_Apparatus.EnterMoveNext = true;
            this.lue_Apparatus.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Apparatus.KeyUpDownMoveNext = false;
            this.lue_Apparatus.LoadDataOnDesignMode = true;
            this.lue_Apparatus.Location = new System.Drawing.Point(739, 40);
            this.lue_Apparatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lue_Apparatus.MaximumSize = new System.Drawing.Size(571, 27);
            this.lue_Apparatus.MinimumSize = new System.Drawing.Size(57, 27);
            this.lue_Apparatus.Name = "lue_Apparatus";
            this.lue_Apparatus.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Apparatus.PFont = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lue_Apparatus.Readonly = false;
            this.lue_Apparatus.SaveSourceID = false;
            this.lue_Apparatus.SelectFilter = null;
            this.lue_Apparatus.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Apparatus.SelectOnly = true;
            this.lue_Apparatus.ShowAllInstrmt = true;
            this.lue_Apparatus.Size = new System.Drawing.Size(143, 27);
            this.lue_Apparatus.TabIndex = 41;
            this.lue_Apparatus.UseExtend = false;
            this.lue_Apparatus.valueMember = null;
            this.lue_Apparatus.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.lue_Apparatus_ValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl1.Location = new System.Drawing.Point(29, 41);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 22);
            this.labelControl1.TabIndex = 51;
            this.labelControl1.Text = "日期范围";
            // 
            // dtBegin
            // 
            this.dtBegin.EditValue = null;
            this.dtBegin.EnterMoveNextControl = true;
            this.dtBegin.Location = new System.Drawing.Point(107, 40);
            this.dtBegin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtBegin.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.dtBegin.Properties.Appearance.Options.UseFont = true;
            this.dtBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtBegin.Size = new System.Drawing.Size(143, 28);
            this.dtBegin.TabIndex = 48;
            // 
            // dtEnd
            // 
            this.dtEnd.EditValue = null;
            this.dtEnd.EnterMoveNextControl = true;
            this.dtEnd.Location = new System.Drawing.Point(268, 40);
            this.dtEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtEnd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.dtEnd.Properties.Appearance.Options.UseFont = true;
            this.dtEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtEnd.Size = new System.Drawing.Size(143, 28);
            this.dtEnd.TabIndex = 49;
            // 
            // labelControl21
            // 
            this.labelControl21.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl21.Location = new System.Drawing.Point(256, 43);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(6, 21);
            this.labelControl21.TabIndex = 50;
            this.labelControl21.Text = "-";
            // 
            // tlQcItem
            // 
            this.tlQcItem.ColumnPanelRowHeight = 23;
            this.tlQcItem.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tlcItem});
            this.tlQcItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.tlQcItem.KeyFieldName = "type_id";
            this.tlQcItem.Location = new System.Drawing.Point(2, 2);
            this.tlQcItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tlQcItem.Name = "tlQcItem";
            this.tlQcItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tlQcItem.OptionsView.ShowCheckBoxes = true;
            this.tlQcItem.OptionsView.ShowIndicator = false;
            this.tlQcItem.ParentFieldName = "parentId";
            this.tlQcItem.Size = new System.Drawing.Size(177, 674);
            this.tlQcItem.TabIndex = 0;
            // 
            // tlcItem
            // 
            this.tlcItem.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.tlcItem.AppearanceCell.Options.UseFont = true;
            this.tlcItem.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.tlcItem.AppearanceHeader.Options.UseFont = true;
            this.tlcItem.AppearanceHeader.Options.UseTextOptions = true;
            this.tlcItem.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tlcItem.Caption = "质控项目";
            this.tlcItem.FieldName = "type_name";
            this.tlcItem.MinWidth = 48;
            this.tlcItem.Name = "tlcItem";
            this.tlcItem.OptionsColumn.AllowEdit = false;
            this.tlcItem.OptionsColumn.AllowSort = false;
            this.tlcItem.Visible = true;
            this.tlcItem.VisibleIndex = 0;
            this.tlcItem.Width = 121;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.QuickOption = false;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1199, 81);
            this.sysToolBar1.TabIndex = 52;
            this.sysToolBar1.BtnCalculationClick += new System.EventHandler(this.sysToolBar1_BtnCalculationClick);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.dtBegin);
            this.groupControl1.Controls.Add(this.lueType);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.lue_Apparatus);
            this.groupControl1.Controls.Add(this.dtEnd);
            this.groupControl1.Controls.Add(this.labelControl21);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 81);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1199, 82);
            this.groupControl1.TabIndex = 54;
            this.groupControl1.Text = "查询条件";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.tlQcItem);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 163);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1199, 678);
            this.panelControl1.TabIndex = 55;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gcCompareData);
            this.groupControl2.Controls.Add(this.panelControl2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(179, 2);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1018, 674);
            this.groupControl2.TabIndex = 52;
            this.groupControl2.Text = "数据对比";
            // 
            // dtCompareDate
            // 
            this.dtCompareDate.EditValue = null;
            this.dtCompareDate.EnterMoveNextControl = true;
            this.dtCompareDate.Location = new System.Drawing.Point(57, 8);
            this.dtCompareDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtCompareDate.Name = "dtCompareDate";
            this.dtCompareDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtCompareDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.dtCompareDate.Properties.Appearance.Options.UseFont = true;
            this.dtCompareDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtCompareDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtCompareDate.Size = new System.Drawing.Size(143, 28);
            this.dtCompareDate.TabIndex = 54;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl5.Location = new System.Drawing.Point(17, 9);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 22);
            this.labelControl5.TabIndex = 53;
            this.labelControl5.Text = "日期";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl4.Location = new System.Drawing.Point(210, 10);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(54, 22);
            this.labelControl4.TabIndex = 48;
            this.labelControl4.Text = "样本号";
            // 
            // txtSID
            // 
            this.txtSID.EnterMoveNextControl = true;
            this.txtSID.Location = new System.Drawing.Point(266, 8);
            this.txtSID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSID.Name = "txtSID";
            this.txtSID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtSID.Properties.Appearance.Options.UseFont = true;
            this.txtSID.Properties.NullText = "例：1-5.7.9.10-15";
            this.txtSID.Size = new System.Drawing.Size(344, 28);
            this.txtSID.TabIndex = 50;
            this.txtSID.ToolTip = "例：1-5.7.9.10-15";
            this.txtSID.ToolTipTitle = "例：1-5.7.9.10-15";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.dtCompareDate);
            this.panelControl2.Controls.Add(this.labelControl5);
            this.panelControl2.Controls.Add(this.txtSID);
            this.panelControl2.Controls.Add(this.labelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(2, 27);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1014, 42);
            this.panelControl2.TabIndex = 53;
            // 
            // gcCompareData
            // 
            this.gcCompareData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCompareData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCompareData.Location = new System.Drawing.Point(2, 69);
            this.gcCompareData.MainView = this.gvCompareData;
            this.gcCompareData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCompareData.Name = "gcCompareData";
            this.gcCompareData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ckEffective,
            this.repositoryItemLookUpEdit1});
            this.gcCompareData.Size = new System.Drawing.Size(1014, 603);
            this.gcCompareData.TabIndex = 54;
            this.gcCompareData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCompareData});
            // 
            // gvCompareData
            // 
            this.gvCompareData.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCompareData.Appearance.Row.Options.UseBackColor = true;
            this.gvCompareData.ColumnPanelRowHeight = 23;
            this.gvCompareData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn2,
            this.gridColumn6,
            this.gridColumn3,
            this.gridColumn4});
            this.gvCompareData.GridControl = this.gcCompareData;
            this.gvCompareData.Name = "gvCompareData";
            this.gvCompareData.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCompareData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvCompareData.OptionsView.ColumnAutoWidth = false;
            this.gvCompareData.OptionsView.ShowGroupPanel = false;
            this.gvCompareData.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.FieldName = "com_seq";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 88;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "质控日期";
            this.gridColumn5.FieldName = "com_qc_date";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            this.gridColumn5.Width = 150;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "质控数据";
            this.gridColumn2.FieldName = "com_qc_data";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 94;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "对比样本号";
            this.gridColumn6.FieldName = "com_compare_sid";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 87;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "对比数据";
            this.gridColumn3.FieldName = "com_compare_data";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 96;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "对比系数";
            this.gridColumn4.FieldName = "com_coefficients";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 102;
            // 
            // ckEffective
            // 
            this.ckEffective.AutoHeight = false;
            this.ckEffective.Name = "ckEffective";
            this.ckEffective.ValueChecked = 0;
            this.ckEffective.ValueUnchecked = 1;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.DisplayMember = "dtName";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.ValueMember = "dtId";
            // 
            // FrmReagentsCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 841);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.sysToolBar1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmReagentsCompare";
            this.Text = "质控试剂对比";
            this.Load += new System.EventHandler(this.FrmReagentsCompare_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlQcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtCompareDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCompareDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCompareData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompareData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.control.SelectDicLabProfession lueType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private dcl.client.control.SelectDicInstrument lue_Apparatus;
        private DevExpress.XtraTreeList.TreeList tlQcItem;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcItem;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtBegin;
        private DevExpress.XtraEditors.DateEdit dtEnd;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txtSID;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit dtCompareDate;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ckEffective;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCompareData;
        private DevExpress.XtraGrid.GridControl gcCompareData;
    }
}