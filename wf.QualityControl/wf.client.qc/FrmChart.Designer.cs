using dcl.entity;

namespace dcl.client.qc
{
    partial class FrmChart
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
            DevExpress.XtraCharts.RadarDiagram radarDiagram1 = new DevExpress.XtraCharts.RadarDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.RadarLineSeriesView radarLineSeriesView1 = new DevExpress.XtraCharts.RadarLineSeriesView();
            DevExpress.XtraCharts.RadarLineSeriesView radarLineSeriesView2 = new DevExpress.XtraCharts.RadarLineSeriesView();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            this.lue_Apparatus = new dcl.client.control.SelectDicInstrument();
            this.dtEnd = new DevExpress.XtraEditors.DateEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.tabSub = new DevExpress.XtraTab.XtraTabControl();
            this.xtItem = new DevExpress.XtraTab.XtraTabPage();
            this.checkEditAll = new DevExpress.XtraEditors.CheckEdit();
            this.tlQcItem = new DevExpress.XtraTreeList.TreeList();
            this.tlcItem = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ckShowType_I = new DevExpress.XtraEditors.CheckEdit();
            this.ckShowType_S = new DevExpress.XtraEditors.CheckEdit();
            this.xtProcess = new DevExpress.XtraTab.XtraTabPage();
            this.tlQcAuditItem = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnDataAudit = new DevExpress.XtraEditors.SimpleButton();
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.dtBegin = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.plData = new DevExpress.XtraEditors.PanelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gcLot = new DevExpress.XtraGrid.GridControl();
            this.bsGcData = new System.Windows.Forms.BindingSource(this.components);
            this.gvLot = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcIsEff = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ckEffective = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.qcm_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_meas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_x = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_sd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_ns = new DevExpress.XtraGrid.Columns.GridColumn();
            this.userName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_reson = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_fun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_next_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.qcm_two_audit_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_two_audit_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_rem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_info_group = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.ckDataAudit = new DevExpress.XtraEditors.CheckEdit();
            this.ckDataAll = new DevExpress.XtraEditors.CheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gcInfo = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcItem = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcAVG = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcSD = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcCV = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcN = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcActualAVG = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcActualSD = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcActualCV = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.txtSet = new DevExpress.XtraEditors.TextEdit();
            this.updata_SimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.ceSet_con = new DevExpress.XtraEditors.CheckEdit();
            this.ceSet_Sd = new DevExpress.XtraEditors.CheckEdit();
            this.plPic = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.pnlChat = new System.Windows.Forms.Panel();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.ceType_UD = new DevExpress.XtraEditors.CheckEdit();
            this.ceType_Radar = new DevExpress.XtraEditors.CheckEdit();
            this.ceType_LJ = new DevExpress.XtraEditors.CheckEdit();
            this.ceType_MC = new DevExpress.XtraEditors.CheckEdit();
            this.ceType_BDL = new DevExpress.XtraEditors.CheckEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbQcPointData = new System.Windows.Forms.GroupBox();
            this.ceData_Rep = new DevExpress.XtraEditors.CheckEdit();
            this.ceData_con = new DevExpress.XtraEditors.CheckEdit();
            this.ceData_All = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ceShow_Fal = new DevExpress.XtraEditors.CheckEdit();
            this.ceShow_All = new DevExpress.XtraEditors.CheckEdit();
            this.gbShowType = new System.Windows.Forms.GroupBox();
            this.cePointLast = new DevExpress.XtraEditors.CheckEdit();
            this.ceLevel = new DevExpress.XtraEditors.CheckEdit();
            this.ceTransverse = new DevExpress.XtraEditors.CheckEdit();
            this.ceSerieType = new DevExpress.XtraEditors.CheckEdit();
            this.cePointType = new DevExpress.XtraEditors.CheckEdit();
            this.gbQcType = new System.Windows.Forms.GroupBox();
            this.cmbOth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceRes_Day = new DevExpress.XtraEditors.CheckEdit();
            this.ceRes_Ave = new DevExpress.XtraEditors.CheckEdit();
            this.ceRes_All = new DevExpress.XtraEditors.CheckEdit();
            this.ceRes_Con = new DevExpress.XtraEditors.CheckEdit();
            this.plItem = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.bdqcvalue = new System.Windows.Forms.BindingSource(this.components);
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.toolTipController2 = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSub)).BeginInit();
            this.tabSub.SuspendLayout();
            this.xtItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlQcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckShowType_I.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckShowType_S.Properties)).BeginInit();
            this.xtProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlQcAuditItem)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plData)).BeginInit();
            this.plData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckDataAudit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckDataAll.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSet_con.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSet_Sd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plPic)).BeginInit();
            this.plPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.pnlChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(radarDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(radarLineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(radarLineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_UD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_Radar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_LJ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_MC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_BDL.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.gbQcPointData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceData_Rep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceData_con.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceData_All.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceShow_Fal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceShow_All.Properties)).BeginInit();
            this.gbShowType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cePointLast.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceLevel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTransverse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSerieType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePointType.Properties)).BeginInit();
            this.gbQcType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Day.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Ave.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_All.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Con.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plItem)).BeginInit();
            this.plItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalue)).BeginInit();
            this.SuspendLayout();
            // 
            // lue_Apparatus
            // 
            this.lue_Apparatus.AddEmptyRow = true;
            this.lue_Apparatus.AutoScroll = true;
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
            this.lue_Apparatus.Location = new System.Drawing.Point(89, 85);
            this.lue_Apparatus.MaximumSize = new System.Drawing.Size(500, 21);
            this.lue_Apparatus.MinimumSize = new System.Drawing.Size(50, 21);
            this.lue_Apparatus.Name = "lue_Apparatus";
            this.lue_Apparatus.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Apparatus.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Apparatus.Readonly = false;
            this.lue_Apparatus.SaveSourceID = false;
            this.lue_Apparatus.SelectFilter = null;
            this.lue_Apparatus.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Apparatus.SelectOnly = true;
            this.lue_Apparatus.ShowAllInstrmt = true;
            this.lue_Apparatus.Size = new System.Drawing.Size(165, 21);
            this.lue_Apparatus.TabIndex = 1;
            this.lue_Apparatus.UseExtend = false;
            this.lue_Apparatus.valueMember = null;
            this.lue_Apparatus.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.lue_Apparatus_ValueChanged);
            // 
            // dtEnd
            // 
            this.dtEnd.EditValue = null;
            this.dtEnd.EnterMoveNextControl = true;
            this.dtEnd.Location = new System.Drawing.Point(89, 36);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtEnd.Size = new System.Drawing.Size(165, 20);
            this.dtEnd.StyleController = this.layoutControl1;
            this.dtEnd.TabIndex = 3;
            this.dtEnd.EditValueChanged += new System.EventHandler(this.dtEnd_EditValueChanged);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.tabSub);
            this.layoutControl1.Controls.Add(this.lue_Apparatus);
            this.layoutControl1.Controls.Add(this.lueType);
            this.layoutControl1.Controls.Add(this.dtBegin);
            this.layoutControl1.Controls.Add(this.dtEnd);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(266, 582);
            this.layoutControl1.TabIndex = 40;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // tabSub
            // 
            this.tabSub.Location = new System.Drawing.Point(12, 110);
            this.tabSub.Name = "tabSub";
            this.tabSub.SelectedTabPage = this.xtItem;
            this.tabSub.Size = new System.Drawing.Size(242, 460);
            this.tabSub.TabIndex = 2;
            this.tabSub.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtItem,
            this.xtProcess});
            this.tabSub.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabSub_SelectedPageChanged);
            // 
            // xtItem
            // 
            this.xtItem.Controls.Add(this.checkEditAll);
            this.xtItem.Controls.Add(this.tlQcItem);
            this.xtItem.Controls.Add(this.panelControl1);
            this.xtItem.Name = "xtItem";
            this.xtItem.Size = new System.Drawing.Size(236, 431);
            this.xtItem.Text = "浏览";
            // 
            // checkEditAll
            // 
            this.checkEditAll.Location = new System.Drawing.Point(60, 25);
            this.checkEditAll.Name = "checkEditAll";
            this.checkEditAll.Properties.Caption = "";
            this.checkEditAll.Size = new System.Drawing.Size(19, 19);
            this.checkEditAll.TabIndex = 17;
            this.checkEditAll.TabStop = false;
            // 
            // tlQcItem
            // 
            this.tlQcItem.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.tlcItem});
            this.tlQcItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlQcItem.KeyFieldName = "TvId";
            this.tlQcItem.Location = new System.Drawing.Point(0, 23);
            this.tlQcItem.Name = "tlQcItem";
            this.tlQcItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tlQcItem.OptionsView.ShowCheckBoxes = true;
            this.tlQcItem.OptionsView.ShowIndicator = false;
            this.tlQcItem.ParentFieldName = "TvParentId";
            this.tlQcItem.Size = new System.Drawing.Size(236, 408);
            this.tlQcItem.TabIndex = 16;
            // 
            // tlcItem
            // 
            this.tlcItem.Caption = "质控项目";
            this.tlcItem.FieldName = "TvName";
            this.tlcItem.MinWidth = 48;
            this.tlcItem.Name = "tlcItem";
            this.tlcItem.OptionsColumn.AllowEdit = false;
            this.tlcItem.OptionsColumn.AllowSort = false;
            this.tlcItem.Visible = true;
            this.tlcItem.VisibleIndex = 0;
            this.tlcItem.Width = 121;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ckShowType_I);
            this.panelControl1.Controls.Add(this.ckShowType_S);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(236, 23);
            this.panelControl1.TabIndex = 15;
            // 
            // ckShowType_I
            // 
            this.ckShowType_I.Location = new System.Drawing.Point(153, 2);
            this.ckShowType_I.Name = "ckShowType_I";
            this.ckShowType_I.Properties.Caption = "多项目";
            this.ckShowType_I.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckShowType_I.Properties.RadioGroupIndex = 2;
            this.ckShowType_I.Size = new System.Drawing.Size(66, 19);
            this.ckShowType_I.TabIndex = 40;
            this.ckShowType_I.TabStop = false;
            this.ckShowType_I.CheckedChanged += new System.EventHandler(this.ckShowType_CheckedChanged);
            // 
            // ckShowType_S
            // 
            this.ckShowType_S.EditValue = true;
            this.ckShowType_S.Location = new System.Drawing.Point(21, 2);
            this.ckShowType_S.Name = "ckShowType_S";
            this.ckShowType_S.Properties.Caption = "多水平";
            this.ckShowType_S.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckShowType_S.Properties.RadioGroupIndex = 2;
            this.ckShowType_S.Size = new System.Drawing.Size(66, 19);
            this.ckShowType_S.TabIndex = 39;
            this.ckShowType_S.CheckedChanged += new System.EventHandler(this.ckShowType_CheckedChanged);
            // 
            // xtProcess
            // 
            this.xtProcess.Controls.Add(this.tlQcAuditItem);
            this.xtProcess.Controls.Add(this.panel6);
            this.xtProcess.Name = "xtProcess";
            this.xtProcess.Size = new System.Drawing.Size(237, 422);
            this.xtProcess.Text = "审核";
            // 
            // tlQcAuditItem
            // 
            this.tlQcAuditItem.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.tlQcAuditItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlQcAuditItem.KeyFieldName = "TvId";
            this.tlQcAuditItem.Location = new System.Drawing.Point(0, 0);
            this.tlQcAuditItem.Name = "tlQcAuditItem";
            this.tlQcAuditItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tlQcAuditItem.OptionsView.ShowCheckBoxes = true;
            this.tlQcAuditItem.OptionsView.ShowIndicator = false;
            this.tlQcAuditItem.ParentFieldName = "TvParentId";
            this.tlQcAuditItem.Size = new System.Drawing.Size(237, 382);
            this.tlQcAuditItem.TabIndex = 1;
            this.tlQcAuditItem.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tlQcAuditItem_AfterCheckNode);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "质控项目";
            this.treeListColumn1.FieldName = "TvName";
            this.treeListColumn1.MinWidth = 48;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.OptionsColumn.AllowSort = false;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 121;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnDataAudit);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 382);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(237, 40);
            this.panel6.TabIndex = 2;
            this.panel6.Visible = false;
            // 
            // btnDataAudit
            // 
            this.btnDataAudit.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnDataAudit.Appearance.Options.UseFont = true;
            this.btnDataAudit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDataAudit.Location = new System.Drawing.Point(9, 7);
            this.btnDataAudit.Name = "btnDataAudit";
            this.btnDataAudit.Size = new System.Drawing.Size(141, 24);
            this.btnDataAudit.TabIndex = 2;
            this.btnDataAudit.Text = "审核";
            this.btnDataAudit.Click += new System.EventHandler(this.btnDataAudit_Click);
            // 
            // lueType
            // 
            this.lueType.AddEmptyRow = true;
            this.lueType.AutoScroll = true;
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
            this.lueType.Location = new System.Drawing.Point(89, 60);
            this.lueType.MaximumSize = new System.Drawing.Size(500, 21);
            this.lueType.MinimumSize = new System.Drawing.Size(50, 21);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = true;
            this.lueType.Size = new System.Drawing.Size(165, 21);
            this.lueType.TabIndex = 0;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = null;
            this.lueType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.lueType_ValueChanged);
            // 
            // dtBegin
            // 
            this.dtBegin.EditValue = null;
            this.dtBegin.EnterMoveNextControl = true;
            this.dtBegin.Location = new System.Drawing.Point(89, 12);
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtBegin.Size = new System.Drawing.Size(165, 20);
            this.dtBegin.StyleController = this.layoutControl1;
            this.dtBegin.TabIndex = 2;
            this.dtBegin.EditValueChanged += new System.EventHandler(this.dtBegin_EditValueChanged);
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
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Size = new System.Drawing.Size(266, 582);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.dtBegin;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(246, 24);
            this.layoutControlItem1.Text = "查询起始日期";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(72, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dtEnd;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(246, 24);
            this.layoutControlItem2.Text = "查询结束日期";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(72, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lueType;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(246, 25);
            this.layoutControlItem3.Text = "实验组";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(72, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lue_Apparatus;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 73);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(246, 25);
            this.layoutControlItem4.Text = "测定仪器";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(72, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.tabSub;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 98);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(246, 464);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.plData);
            this.panelControl2.Controls.Add(this.plItem);
            this.panelControl2.Controls.Add(this.sysToolBar1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1264, 654);
            this.panelControl2.TabIndex = 4;
            // 
            // plData
            // 
            this.plData.Controls.Add(this.splitterControl1);
            this.plData.Controls.Add(this.xtraTabControl1);
            this.plData.Controls.Add(this.plPic);
            this.plData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plData.Location = new System.Drawing.Point(276, 62);
            this.plData.Name = "plData";
            this.plData.Size = new System.Drawing.Size(986, 590);
            this.plData.TabIndex = 34;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(751, 2);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 586);
            this.splitterControl1.TabIndex = 36;
            this.splitterControl1.TabStop = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(751, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(233, 586);
            this.xtraTabControl1.TabIndex = 9;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gcLot);
            this.xtraTabPage1.Controls.Add(this.panelControl5);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(227, 557);
            this.xtraTabPage1.Text = "结果";
            // 
            // gcLot
            // 
            this.gcLot.DataSource = this.bsGcData;
            this.gcLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLot.Location = new System.Drawing.Point(0, 33);
            this.gcLot.MainView = this.gvLot;
            this.gcLot.Name = "gcLot";
            this.gcLot.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ckEffective,
            this.repositoryItemLookUpEdit1});
            this.gcLot.Size = new System.Drawing.Size(227, 524);
            this.gcLot.TabIndex = 10;
            this.gcLot.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLot});
            // 
            // gvLot
            // 
            this.gvLot.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvLot.Appearance.Row.Options.UseBackColor = true;
            this.gvLot.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcIsEff,
            this.qcm_date,
            this.itm_ecd,
            this.qcm_c_no,
            this.qcm_meas,
            this.qcm_c_x,
            this.qcm_c_sd,
            this.gridColumn1,
            this.qcm_ns,
            this.userName,
            this.qcm_reson,
            this.qcm_fun,
            this.qcm_next_time,
            this.gridColumn2,
            this.qcm_two_audit_date,
            this.qcm_two_audit_name,
            this.qcm_rem,
            this.qcm_info_group,
            this.gridColumn3});
            this.gvLot.GridControl = this.gcLot;
            this.gvLot.Name = "gvLot";
            this.gvLot.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvLot.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLot.OptionsView.ColumnAutoWidth = false;
            this.gvLot.OptionsView.ShowGroupPanel = false;
            this.gvLot.OptionsView.ShowIndicator = false;
            this.gvLot.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.qcm_date, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gcIsEff
            // 
            this.gcIsEff.Caption = "显示";
            this.gcIsEff.ColumnEdit = this.ckEffective;
            this.gcIsEff.FieldName = "QresDisplay";
            this.gcIsEff.Name = "gcIsEff";
            this.gcIsEff.OptionsFilter.AllowFilter = false;
            this.gcIsEff.Visible = true;
            this.gcIsEff.VisibleIndex = 0;
            this.gcIsEff.Width = 38;
            // 
            // ckEffective
            // 
            this.ckEffective.AutoHeight = false;
            this.ckEffective.Caption = "Check";
            this.ckEffective.Name = "ckEffective";
            this.ckEffective.ValueChecked = 0;
            this.ckEffective.ValueUnchecked = 1;
            // 
            // qcm_date
            // 
            this.qcm_date.Caption = "测定时间";
            this.qcm_date.DisplayFormat.FormatString = "yyyy-MM-dd HH\':\'mm\':\'ss";
            this.qcm_date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.qcm_date.FieldName = "QresDate";
            this.qcm_date.Name = "qcm_date";
            this.qcm_date.OptionsColumn.AllowEdit = false;
            this.qcm_date.OptionsFilter.AllowFilter = false;
            this.qcm_date.Visible = true;
            this.qcm_date.VisibleIndex = 1;
            this.qcm_date.Width = 124;
            // 
            // itm_ecd
            // 
            this.itm_ecd.Caption = "项目";
            this.itm_ecd.FieldName = "ItmEcode";
            this.itm_ecd.Name = "itm_ecd";
            this.itm_ecd.OptionsColumn.AllowEdit = false;
            this.itm_ecd.OptionsFilter.AllowFilter = false;
            this.itm_ecd.Visible = true;
            this.itm_ecd.VisibleIndex = 2;
            this.itm_ecd.Width = 57;
            // 
            // qcm_c_no
            // 
            this.qcm_c_no.Caption = "水平";
            this.qcm_c_no.FieldName = "QcShowLevel";
            this.qcm_c_no.Name = "qcm_c_no";
            this.qcm_c_no.OptionsColumn.AllowEdit = false;
            this.qcm_c_no.OptionsFilter.AllowFilter = false;
            this.qcm_c_no.Visible = true;
            this.qcm_c_no.VisibleIndex = 3;
            this.qcm_c_no.Width = 40;
            // 
            // qcm_meas
            // 
            this.qcm_meas.Caption = "结果";
            this.qcm_meas.FieldName = "QresValue";
            this.qcm_meas.Name = "qcm_meas";
            this.qcm_meas.OptionsColumn.AllowEdit = false;
            this.qcm_meas.OptionsFilter.AllowFilter = false;
            this.qcm_meas.Visible = true;
            this.qcm_meas.VisibleIndex = 4;
            this.qcm_meas.Width = 45;
            // 
            // qcm_c_x
            // 
            this.qcm_c_x.Caption = "靶值";
            this.qcm_c_x.FieldName = "QresItmX";
            this.qcm_c_x.Name = "qcm_c_x";
            this.qcm_c_x.OptionsColumn.AllowEdit = false;
            this.qcm_c_x.OptionsFilter.AllowFilter = false;
            this.qcm_c_x.Width = 34;
            // 
            // qcm_c_sd
            // 
            this.qcm_c_sd.Caption = "标准差";
            this.qcm_c_sd.FieldName = "QresItmSd";
            this.qcm_c_sd.Name = "qcm_c_sd";
            this.qcm_c_sd.OptionsColumn.AllowEdit = false;
            this.qcm_c_sd.OptionsFilter.AllowFilter = false;
            this.qcm_c_sd.Width = 42;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "时间";
            this.gridColumn1.DisplayFormat.FormatString = "HH\':\'mm\':\'ss";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn1.FieldName = "QresDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // qcm_ns
            // 
            this.qcm_ns.Caption = "规则";
            this.qcm_ns.FieldName = "QresRunawayRule";
            this.qcm_ns.Name = "qcm_ns";
            this.qcm_ns.OptionsColumn.AllowEdit = false;
            this.qcm_ns.OptionsFilter.AllowFilter = false;
            this.qcm_ns.Visible = true;
            this.qcm_ns.VisibleIndex = 5;
            this.qcm_ns.Width = 121;
            // 
            // userName
            // 
            this.userName.Caption = "审核者";
            this.userName.FieldName = "QresAuditUserName";
            this.userName.Name = "userName";
            this.userName.OptionsColumn.AllowEdit = false;
            this.userName.Visible = true;
            this.userName.VisibleIndex = 8;
            // 
            // qcm_reson
            // 
            this.qcm_reson.Caption = "失控原因";
            this.qcm_reson.FieldName = "QresReasons";
            this.qcm_reson.Name = "qcm_reson";
            this.qcm_reson.OptionsColumn.AllowEdit = false;
            this.qcm_reson.Visible = true;
            this.qcm_reson.VisibleIndex = 6;
            // 
            // qcm_fun
            // 
            this.qcm_fun.Caption = "处理方式";
            this.qcm_fun.FieldName = "QresProcess";
            this.qcm_fun.Name = "qcm_fun";
            this.qcm_fun.OptionsColumn.AllowEdit = false;
            this.qcm_fun.Visible = true;
            this.qcm_fun.VisibleIndex = 7;
            // 
            // qcm_next_time
            // 
            this.qcm_next_time.Caption = "下一次审核时间";
            this.qcm_next_time.DisplayFormat.FormatString = "yyyy-MM-dd HH\':\'mm\':\'ss";
            this.qcm_next_time.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.qcm_next_time.FieldName = "NextAuditTime";
            this.qcm_next_time.Name = "qcm_next_time";
            this.qcm_next_time.OptionsColumn.AllowEdit = false;
            this.qcm_next_time.OptionsFilter.AllowAutoFilter = false;
            this.qcm_next_time.OptionsFilter.AllowFilter = false;
            this.qcm_next_time.Visible = true;
            this.qcm_next_time.VisibleIndex = 10;
            this.qcm_next_time.Width = 150;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "结果类型";
            this.gridColumn2.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.gridColumn2.FieldName = "QresType";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
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
            // qcm_two_audit_date
            // 
            this.qcm_two_audit_date.Caption = "二审时间";
            this.qcm_two_audit_date.FieldName = "QresSecondauditDate";
            this.qcm_two_audit_date.Name = "qcm_two_audit_date";
            this.qcm_two_audit_date.OptionsColumn.AllowEdit = false;
            this.qcm_two_audit_date.OptionsFilter.AllowAutoFilter = false;
            this.qcm_two_audit_date.OptionsFilter.AllowFilter = false;
            this.qcm_two_audit_date.Visible = true;
            this.qcm_two_audit_date.VisibleIndex = 11;
            // 
            // qcm_two_audit_name
            // 
            this.qcm_two_audit_name.Caption = "二审人";
            this.qcm_two_audit_name.FieldName = "QresSecondauditUserName";
            this.qcm_two_audit_name.Name = "qcm_two_audit_name";
            this.qcm_two_audit_name.OptionsColumn.AllowEdit = false;
            this.qcm_two_audit_name.OptionsFilter.AllowAutoFilter = false;
            this.qcm_two_audit_name.OptionsFilter.AllowFilter = false;
            this.qcm_two_audit_name.Visible = true;
            this.qcm_two_audit_name.VisibleIndex = 12;
            // 
            // qcm_rem
            // 
            this.qcm_rem.Caption = "备注";
            this.qcm_rem.FieldName = "QresRemark";
            this.qcm_rem.Name = "qcm_rem";
            this.qcm_rem.OptionsColumn.AllowEdit = false;
            this.qcm_rem.OptionsFilter.AllowAutoFilter = false;
            this.qcm_rem.OptionsFilter.AllowFilter = false;
            this.qcm_rem.Visible = true;
            this.qcm_rem.VisibleIndex = 13;
            // 
            // qcm_info_group
            // 
            this.qcm_info_group.Caption = "信息";
            this.qcm_info_group.FieldName = "GroupName";
            this.qcm_info_group.Name = "qcm_info_group";
            this.qcm_info_group.Visible = true;
            this.qcm_info_group.VisibleIndex = 14;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "审核时间";
            this.gridColumn3.DisplayFormat.FormatString = "yyyy-MM-dd HH\':\'mm\':\'ss";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "QresAuditDate";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 9;
            this.gridColumn3.Width = 150;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.ckDataAudit);
            this.panelControl5.Controls.Add(this.ckDataAll);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl5.Location = new System.Drawing.Point(0, 0);
            this.panelControl5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(227, 33);
            this.panelControl5.TabIndex = 9;
            // 
            // ckDataAudit
            // 
            this.ckDataAudit.Location = new System.Drawing.Point(122, 9);
            this.ckDataAudit.Name = "ckDataAudit";
            this.ckDataAudit.Properties.Caption = "显示已审结果";
            this.ckDataAudit.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckDataAudit.Properties.RadioGroupIndex = 9;
            this.ckDataAudit.Size = new System.Drawing.Size(107, 19);
            this.ckDataAudit.TabIndex = 7;
            this.ckDataAudit.TabStop = false;
            this.ckDataAudit.CheckedChanged += new System.EventHandler(this.ckData_CheckedChanged);
            // 
            // ckDataAll
            // 
            this.ckDataAll.EditValue = true;
            this.ckDataAll.Location = new System.Drawing.Point(10, 9);
            this.ckDataAll.Name = "ckDataAll";
            this.ckDataAll.Properties.Caption = "显示所有结果";
            this.ckDataAll.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckDataAll.Properties.RadioGroupIndex = 9;
            this.ckDataAll.Size = new System.Drawing.Size(122, 19);
            this.ckDataAll.TabIndex = 6;
            this.ckDataAll.CheckedChanged += new System.EventHandler(this.ckData_CheckedChanged);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gcInfo);
            this.xtraTabPage2.Controls.Add(this.panelControl6);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(227, 557);
            this.xtraTabPage2.Text = "统计";
            // 
            // gcInfo
            // 
            this.gcInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcInfo.Location = new System.Drawing.Point(0, 84);
            this.gcInfo.MainView = this.bandedGridView1;
            this.gcInfo.Name = "gcInfo";
            this.gcInfo.Size = new System.Drawing.Size(227, 473);
            this.gcInfo.TabIndex = 45;
            this.gcInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2,
            this.gridBand1,
            this.gridBand3});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gcAVG,
            this.gcSD,
            this.gcCV,
            this.gcItem,
            this.gcActualAVG,
            this.gcActualSD,
            this.gcActualCV,
            this.gcN});
            this.bandedGridView1.GridControl = this.gcInfo;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            this.bandedGridView1.OptionsView.ShowIndicator = false;
            // 
            // gridBand2
            // 
            this.gridBand2.Columns.Add(this.gcItem);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 0;
            this.gridBand2.Width = 108;
            // 
            // gcItem
            // 
            this.gcItem.Caption = "项目";
            this.gcItem.FieldName = "stItem";
            this.gcItem.Name = "gcItem";
            this.gcItem.OptionsColumn.AllowEdit = false;
            this.gcItem.Visible = true;
            this.gcItem.Width = 108;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "设定";
            this.gridBand1.Columns.Add(this.gcAVG);
            this.gridBand1.Columns.Add(this.gcSD);
            this.gridBand1.Columns.Add(this.gcCV);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 1;
            this.gridBand1.Width = 159;
            // 
            // gcAVG
            // 
            this.gcAVG.Caption = "平均值";
            this.gcAVG.FieldName = "stAve";
            this.gcAVG.Name = "gcAVG";
            this.gcAVG.OptionsColumn.AllowEdit = false;
            this.gcAVG.Visible = true;
            this.gcAVG.Width = 56;
            // 
            // gcSD
            // 
            this.gcSD.Caption = "标准差";
            this.gcSD.FieldName = "stSD";
            this.gcSD.Name = "gcSD";
            this.gcSD.OptionsColumn.AllowEdit = false;
            this.gcSD.Visible = true;
            this.gcSD.Width = 49;
            // 
            // gcCV
            // 
            this.gcCV.Caption = "CV(%)";
            this.gcCV.FieldName = "stCV";
            this.gcCV.Name = "gcCV";
            this.gcCV.OptionsColumn.AllowEdit = false;
            this.gcCV.Visible = true;
            this.gcCV.Width = 54;
            // 
            // gridBand3
            // 
            this.gridBand3.Caption = "本月实际";
            this.gridBand3.Columns.Add(this.gcN);
            this.gridBand3.Columns.Add(this.gcActualAVG);
            this.gridBand3.Columns.Add(this.gcActualSD);
            this.gridBand3.Columns.Add(this.gcActualCV);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 230;
            // 
            // gcN
            // 
            this.gcN.Caption = "N";
            this.gcN.FieldName = "stActualN";
            this.gcN.Name = "gcN";
            this.gcN.OptionsColumn.AllowEdit = false;
            this.gcN.Visible = true;
            this.gcN.Width = 42;
            // 
            // gcActualAVG
            // 
            this.gcActualAVG.Caption = "平均值";
            this.gcActualAVG.FieldName = "stActualAve";
            this.gcActualAVG.Name = "gcActualAVG";
            this.gcActualAVG.OptionsColumn.AllowEdit = false;
            this.gcActualAVG.Visible = true;
            this.gcActualAVG.Width = 66;
            // 
            // gcActualSD
            // 
            this.gcActualSD.Caption = "标准差";
            this.gcActualSD.FieldName = "stActualSD";
            this.gcActualSD.Name = "gcActualSD";
            this.gcActualSD.OptionsColumn.AllowEdit = false;
            this.gcActualSD.Visible = true;
            this.gcActualSD.Width = 70;
            // 
            // gcActualCV
            // 
            this.gcActualCV.Caption = "CV(%)";
            this.gcActualCV.FieldName = "stActualCV";
            this.gcActualCV.Name = "gcActualCV";
            this.gcActualCV.OptionsColumn.AllowEdit = false;
            this.gcActualCV.Visible = true;
            this.gcActualCV.Width = 52;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.labelControl20);
            this.panelControl6.Controls.Add(this.txtSet);
            this.panelControl6.Controls.Add(this.updata_SimpleButton);
            this.panelControl6.Controls.Add(this.ceSet_con);
            this.panelControl6.Controls.Add(this.ceSet_Sd);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl6.Location = new System.Drawing.Point(0, 0);
            this.panelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(227, 84);
            this.panelControl6.TabIndex = 44;
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(109, 58);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(63, 14);
            this.labelControl20.TabIndex = 5;
            this.labelControl20.Text = "个SD的结果";
            // 
            // txtSet
            // 
            this.txtSet.Location = new System.Drawing.Point(67, 56);
            this.txtSet.Name = "txtSet";
            this.txtSet.Size = new System.Drawing.Size(37, 20);
            this.txtSet.TabIndex = 6;
            // 
            // updata_SimpleButton
            // 
            this.updata_SimpleButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.updata_SimpleButton.Location = new System.Drawing.Point(2, 2);
            this.updata_SimpleButton.Name = "updata_SimpleButton";
            this.updata_SimpleButton.Size = new System.Drawing.Size(223, 25);
            this.updata_SimpleButton.TabIndex = 4;
            this.updata_SimpleButton.Text = "将统计数据更新到该质控参数";
            this.updata_SimpleButton.Visible = false;
            this.updata_SimpleButton.Click += new System.EventHandler(this.updataSimpleButton_Click);
            // 
            // ceSet_con
            // 
            this.ceSet_con.Location = new System.Drawing.Point(21, 33);
            this.ceSet_con.Name = "ceSet_con";
            this.ceSet_con.Properties.Caption = "去除失控点";
            this.ceSet_con.Size = new System.Drawing.Size(96, 19);
            this.ceSet_con.TabIndex = 1;
            // 
            // ceSet_Sd
            // 
            this.ceSet_Sd.Location = new System.Drawing.Point(21, 58);
            this.ceSet_Sd.Name = "ceSet_Sd";
            this.ceSet_Sd.Properties.Caption = "去除";
            this.ceSet_Sd.Size = new System.Drawing.Size(51, 19);
            this.ceSet_Sd.TabIndex = 2;
            this.ceSet_Sd.TabStop = false;
            // 
            // plPic
            // 
            this.plPic.Controls.Add(this.splitContainerControl1);
            this.plPic.Dock = System.Windows.Forms.DockStyle.Left;
            this.plPic.Location = new System.Drawing.Point(2, 2);
            this.plPic.Name = "plPic";
            this.plPic.Size = new System.Drawing.Size(749, 586);
            this.plPic.TabIndex = 35;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Collapsed = true;
            this.splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.pnlChat);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panel2);
            this.splitContainerControl1.Panel2.Controls.Add(this.gbShowType);
            this.splitContainerControl1.Panel2.Controls.Add(this.gbQcType);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(745, 582);
            this.splitContainerControl1.SplitterPosition = 147;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // pnlChat
            // 
            this.pnlChat.AutoScroll = true;
            this.pnlChat.BackColor = System.Drawing.Color.White;
            this.pnlChat.Controls.Add(this.chartControl1);
            this.pnlChat.Controls.Add(this.panelControl4);
            this.pnlChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChat.Location = new System.Drawing.Point(0, 0);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(745, 577);
            this.pnlChat.TabIndex = 36;
            // 
            // chartControl1
            // 
            this.chartControl1.AppearanceNameSerializable = "Light";
            this.chartControl1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl1.CrosshairOptions.ShowArgumentLine = false;
            this.chartControl1.CrosshairOptions.ShowCrosshairLabels = false;
            this.chartControl1.CrosshairOptions.ShowGroupHeaders = false;
            this.chartControl1.CrosshairOptions.ShowOnlyInFocusedPane = false;
            radarDiagram1.AxisY.DateTimeScaleOptions.AutoGrid = false;
            radarDiagram1.AxisY.NumericScaleOptions.AutoGrid = false;
            radarDiagram1.AxisY.Visible = false;
            radarDiagram1.AxisY.VisualRange.Auto = false;
            radarDiagram1.AxisY.VisualRange.MaxValueSerializable = "3";
            radarDiagram1.AxisY.VisualRange.MinValueSerializable = "0";
            radarDiagram1.AxisY.WholeRange.Auto = false;
            radarDiagram1.AxisY.WholeRange.MaxValueSerializable = "3";
            radarDiagram1.AxisY.WholeRange.MinValueSerializable = "0";
            this.chartControl1.Diagram = radarDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Left;
            this.chartControl1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.chartControl1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartControl1.Location = new System.Drawing.Point(0, 25);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;
            this.chartControl1.PaletteName = "调色板1";
            this.chartControl1.PaletteRepository.Add("调色板1", new DevExpress.XtraCharts.Palette("调色板1", DevExpress.XtraCharts.PaletteScaleMode.Repeat, new DevExpress.XtraCharts.PaletteEntry[] {
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128))))), System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.Green, System.Drawing.Color.Green),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(64))))), System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.Olive, System.Drawing.Color.Olive),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.Navy, System.Drawing.Color.Navy),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))), System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128))))), System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128))))), System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(192))))), System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(192)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(128))))), System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))))),
                new DevExpress.XtraCharts.PaletteEntry(System.Drawing.Color.Blue, System.Drawing.Color.Blue)}));
            series1.Name = "系列1";
            series1.View = radarLineSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            radarLineSeriesView2.Closed = false;
            this.chartControl1.SeriesTemplate.View = radarLineSeriesView2;
            this.chartControl1.Size = new System.Drawing.Size(745, 552);
            this.chartControl1.TabIndex = 8;
            this.chartControl1.Visible = false;
            this.chartControl1.ObjectHotTracked += new DevExpress.XtraCharts.HotTrackEventHandler(this.chartControl1_ObjectHotTracked);
            this.chartControl1.DoubleClick += new System.EventHandler(this.chartControl1_DoubleClick);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.ceType_UD);
            this.panelControl4.Controls.Add(this.ceType_Radar);
            this.panelControl4.Controls.Add(this.ceType_LJ);
            this.panelControl4.Controls.Add(this.ceType_MC);
            this.panelControl4.Controls.Add(this.ceType_BDL);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(745, 25);
            this.panelControl4.TabIndex = 9;
            // 
            // ceType_UD
            // 
            this.ceType_UD.Location = new System.Drawing.Point(392, 4);
            this.ceType_UD.Name = "ceType_UD";
            this.ceType_UD.Properties.Caption = "优顿图";
            this.ceType_UD.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceType_UD.Properties.RadioGroupIndex = 0;
            this.ceType_UD.Size = new System.Drawing.Size(68, 19);
            this.ceType_UD.TabIndex = 5;
            this.ceType_UD.TabStop = false;
            this.ceType_UD.Visible = false;
            // 
            // ceType_Radar
            // 
            this.ceType_Radar.Location = new System.Drawing.Point(89, 4);
            this.ceType_Radar.Name = "ceType_Radar";
            this.ceType_Radar.Properties.Caption = "雷达图";
            this.ceType_Radar.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceType_Radar.Properties.RadioGroupIndex = 0;
            this.ceType_Radar.Size = new System.Drawing.Size(69, 19);
            this.ceType_Radar.TabIndex = 6;
            this.ceType_Radar.TabStop = false;
            this.ceType_Radar.CheckedChanged += new System.EventHandler(this.ceType_Radar_CheckedChanged);
            // 
            // ceType_LJ
            // 
            this.ceType_LJ.EditValue = true;
            this.ceType_LJ.Location = new System.Drawing.Point(9, 4);
            this.ceType_LJ.Name = "ceType_LJ";
            this.ceType_LJ.Properties.Caption = "L-J图";
            this.ceType_LJ.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceType_LJ.Properties.RadioGroupIndex = 0;
            this.ceType_LJ.Size = new System.Drawing.Size(55, 19);
            this.ceType_LJ.TabIndex = 3;
            // 
            // ceType_MC
            // 
            this.ceType_MC.Location = new System.Drawing.Point(285, 4);
            this.ceType_MC.Name = "ceType_MC";
            this.ceType_MC.Properties.Caption = "Monica图";
            this.ceType_MC.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceType_MC.Properties.RadioGroupIndex = 0;
            this.ceType_MC.Size = new System.Drawing.Size(79, 19);
            this.ceType_MC.TabIndex = 4;
            this.ceType_MC.TabStop = false;
            this.ceType_MC.CheckedChanged += new System.EventHandler(this.ceType_MC_CheckedChanged);
            // 
            // ceType_BDL
            // 
            this.ceType_BDL.Location = new System.Drawing.Point(183, 4);
            this.ceType_BDL.Name = "ceType_BDL";
            this.ceType_BDL.Properties.Caption = "半定量图";
            this.ceType_BDL.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceType_BDL.Properties.RadioGroupIndex = 0;
            this.ceType_BDL.Size = new System.Drawing.Size(77, 19);
            this.ceType_BDL.TabIndex = 5;
            this.ceType_BDL.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbQcPointData);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, -52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 52);
            this.panel2.TabIndex = 44;
            // 
            // gbQcPointData
            // 
            this.gbQcPointData.Controls.Add(this.ceData_Rep);
            this.gbQcPointData.Controls.Add(this.ceData_con);
            this.gbQcPointData.Controls.Add(this.ceData_All);
            this.gbQcPointData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQcPointData.Enabled = false;
            this.gbQcPointData.ForeColor = System.Drawing.Color.Red;
            this.gbQcPointData.Location = new System.Drawing.Point(259, 0);
            this.gbQcPointData.Name = "gbQcPointData";
            this.gbQcPointData.Size = new System.Drawing.Size(0, 52);
            this.gbQcPointData.TabIndex = 40;
            this.gbQcPointData.TabStop = false;
            this.gbQcPointData.Text = "质控点数据类型";
            // 
            // ceData_Rep
            // 
            this.ceData_Rep.Location = new System.Drawing.Point(237, 21);
            this.ceData_Rep.Name = "ceData_Rep";
            this.ceData_Rep.Properties.Caption = "只显示平均值";
            this.ceData_Rep.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceData_Rep.Properties.RadioGroupIndex = 0;
            this.ceData_Rep.Size = new System.Drawing.Size(111, 19);
            this.ceData_Rep.TabIndex = 8;
            this.ceData_Rep.TabStop = false;
            // 
            // ceData_con
            // 
            this.ceData_con.Location = new System.Drawing.Point(120, 21);
            this.ceData_con.Name = "ceData_con";
            this.ceData_con.Properties.Caption = "去除失控结果";
            this.ceData_con.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceData_con.Properties.RadioGroupIndex = 0;
            this.ceData_con.Size = new System.Drawing.Size(111, 19);
            this.ceData_con.TabIndex = 7;
            this.ceData_con.TabStop = false;
            // 
            // ceData_All
            // 
            this.ceData_All.EditValue = true;
            this.ceData_All.Location = new System.Drawing.Point(7, 21);
            this.ceData_All.Name = "ceData_All";
            this.ceData_All.Properties.Caption = "所有质控结果";
            this.ceData_All.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceData_All.Properties.RadioGroupIndex = 0;
            this.ceData_All.Size = new System.Drawing.Size(107, 19);
            this.ceData_All.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ceShow_Fal);
            this.groupBox2.Controls.Add(this.ceShow_All);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.ForeColor = System.Drawing.Color.Red;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 52);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "横向点显示类型";
            // 
            // ceShow_Fal
            // 
            this.ceShow_Fal.EditValue = true;
            this.ceShow_Fal.Location = new System.Drawing.Point(129, 21);
            this.ceShow_Fal.Name = "ceShow_Fal";
            this.ceShow_Fal.Properties.Caption = "只显示有效结果";
            this.ceShow_Fal.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceShow_Fal.Properties.RadioGroupIndex = 0;
            this.ceShow_Fal.Size = new System.Drawing.Size(125, 19);
            this.ceShow_Fal.TabIndex = 5;
            // 
            // ceShow_All
            // 
            this.ceShow_All.Location = new System.Drawing.Point(9, 21);
            this.ceShow_All.Name = "ceShow_All";
            this.ceShow_All.Properties.Caption = "显示所有结果";
            this.ceShow_All.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ceShow_All.Properties.RadioGroupIndex = 0;
            this.ceShow_All.Size = new System.Drawing.Size(122, 19);
            this.ceShow_All.TabIndex = 4;
            this.ceShow_All.TabStop = false;
            // 
            // gbShowType
            // 
            this.gbShowType.Controls.Add(this.cePointLast);
            this.gbShowType.Controls.Add(this.ceLevel);
            this.gbShowType.Controls.Add(this.ceTransverse);
            this.gbShowType.Controls.Add(this.ceSerieType);
            this.gbShowType.Controls.Add(this.cePointType);
            this.gbShowType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbShowType.ForeColor = System.Drawing.Color.Red;
            this.gbShowType.Location = new System.Drawing.Point(0, 54);
            this.gbShowType.Name = "gbShowType";
            this.gbShowType.Size = new System.Drawing.Size(0, 0);
            this.gbShowType.TabIndex = 10;
            this.gbShowType.TabStop = false;
            this.gbShowType.Text = "显示方式";
            // 
            // cePointLast
            // 
            this.cePointLast.Location = new System.Drawing.Point(289, 16);
            this.cePointLast.Name = "cePointLast";
            this.cePointLast.Properties.Caption = "每天只连最后一点";
            this.cePointLast.Size = new System.Drawing.Size(121, 19);
            this.cePointLast.TabIndex = 14;
            this.cePointLast.TabStop = false;
            // 
            // ceLevel
            // 
            this.ceLevel.Location = new System.Drawing.Point(613, 18);
            this.ceLevel.Name = "ceLevel";
            this.ceLevel.Properties.Caption = "水平分开作图";
            this.ceLevel.Size = new System.Drawing.Size(114, 19);
            this.ceLevel.TabIndex = 13;
            this.ceLevel.TabStop = false;
            this.ceLevel.Visible = false;
            // 
            // ceTransverse
            // 
            this.ceTransverse.Location = new System.Drawing.Point(525, 18);
            this.ceTransverse.Name = "ceTransverse";
            this.ceTransverse.Properties.Caption = "横向作图";
            this.ceTransverse.Size = new System.Drawing.Size(80, 19);
            this.ceTransverse.TabIndex = 12;
            this.ceTransverse.TabStop = false;
            this.ceTransverse.Visible = false;
            // 
            // ceSerieType
            // 
            this.ceSerieType.Location = new System.Drawing.Point(161, 16);
            this.ceSerieType.Name = "ceSerieType";
            this.ceSerieType.Properties.Caption = "失控点不连线";
            this.ceSerieType.Size = new System.Drawing.Size(109, 19);
            this.ceSerieType.TabIndex = 11;
            this.ceSerieType.TabStop = false;
            // 
            // cePointType
            // 
            this.cePointType.EditValue = true;
            this.cePointType.Location = new System.Drawing.Point(7, 19);
            this.cePointType.Name = "cePointType";
            this.cePointType.Properties.Caption = "以不同的点区分";
            this.cePointType.Size = new System.Drawing.Size(125, 19);
            this.cePointType.TabIndex = 10;
            this.cePointType.TabStop = false;
            // 
            // gbQcType
            // 
            this.gbQcType.Controls.Add(this.cmbOth);
            this.gbQcType.Controls.Add(this.ceRes_Day);
            this.gbQcType.Controls.Add(this.ceRes_Ave);
            this.gbQcType.Controls.Add(this.ceRes_All);
            this.gbQcType.Controls.Add(this.ceRes_Con);
            this.gbQcType.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbQcType.ForeColor = System.Drawing.Color.Red;
            this.gbQcType.Location = new System.Drawing.Point(0, 0);
            this.gbQcType.Name = "gbQcType";
            this.gbQcType.Size = new System.Drawing.Size(0, 54);
            this.gbQcType.TabIndex = 35;
            this.gbQcType.TabStop = false;
            this.gbQcType.Text = "质控图类型";
            // 
            // cmbOth
            // 
            this.cmbOth.EditValue = "--------更多------";
            this.cmbOth.Location = new System.Drawing.Point(488, 21);
            this.cmbOth.Name = "cmbOth";
            this.cmbOth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOth.Properties.Items.AddRange(new object[] {
            "--------更多------"});
            this.cmbOth.Properties.ReadOnly = true;
            this.cmbOth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbOth.Size = new System.Drawing.Size(117, 20);
            this.cmbOth.TabIndex = 4;
            // 
            // ceRes_Day
            // 
            this.ceRes_Day.Location = new System.Drawing.Point(461, 23);
            this.ceRes_Day.Name = "ceRes_Day";
            this.ceRes_Day.Properties.Caption = "";
            this.ceRes_Day.Properties.RadioGroupIndex = 0;
            this.ceRes_Day.Properties.ReadOnly = true;
            this.ceRes_Day.Size = new System.Drawing.Size(20, 19);
            this.ceRes_Day.TabIndex = 3;
            this.ceRes_Day.TabStop = false;
            // 
            // ceRes_Ave
            // 
            this.ceRes_Ave.Location = new System.Drawing.Point(289, 23);
            this.ceRes_Ave.Name = "ceRes_Ave";
            this.ceRes_Ave.Properties.Caption = "每天结果平均值画一点";
            this.ceRes_Ave.Properties.RadioGroupIndex = 0;
            this.ceRes_Ave.Size = new System.Drawing.Size(164, 19);
            this.ceRes_Ave.TabIndex = 2;
            this.ceRes_Ave.TabStop = false;
            this.ceRes_Ave.CheckedChanged += new System.EventHandler(this.ceRes_CheckedChanged);
            // 
            // ceRes_All
            // 
            this.ceRes_All.EditValue = true;
            this.ceRes_All.Location = new System.Drawing.Point(161, 23);
            this.ceRes_All.Name = "ceRes_All";
            this.ceRes_All.Properties.Caption = "每个结果画一点";
            this.ceRes_All.Properties.RadioGroupIndex = 0;
            this.ceRes_All.Size = new System.Drawing.Size(125, 19);
            this.ceRes_All.TabIndex = 1;
            // 
            // ceRes_Con
            // 
            this.ceRes_Con.Location = new System.Drawing.Point(9, 23);
            this.ceRes_Con.Name = "ceRes_Con";
            this.ceRes_Con.Properties.Caption = "每个在控结果画一点";
            this.ceRes_Con.Properties.RadioGroupIndex = 0;
            this.ceRes_Con.Size = new System.Drawing.Size(148, 19);
            this.ceRes_Con.TabIndex = 0;
            this.ceRes_Con.TabStop = false;
            // 
            // plItem
            // 
            this.plItem.Controls.Add(this.panelControl3);
            this.plItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.plItem.Location = new System.Drawing.Point(2, 62);
            this.plItem.Name = "plItem";
            this.plItem.Size = new System.Drawing.Size(274, 590);
            this.plItem.TabIndex = 33;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.layoutControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(270, 586);
            this.panelControl3.TabIndex = 3;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(2, 2);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.QuickOption = false;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1260, 60);
            this.sysToolBar1.TabIndex = 36;
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            this.sysToolBar1.OnBtnPageUpClicked += new System.EventHandler(this.sysToolBar1_OnBtnPageUpClicked);
            this.sysToolBar1.OnBtnPageDownClicked += new System.EventHandler(this.sysToolBar1_OnBtnPageDownClicked);
            this.sysToolBar1.OnBtnReturnClicked += new System.EventHandler(this.sysToolBar1_BtnReturnClick);
            this.sysToolBar1.OnResultViewClicked += new System.EventHandler(this.sysToolBar1_OnResultViewClicked);
            this.sysToolBar1.OnBtnPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintClicked);
            this.sysToolBar1.OnPrintPreviewClicked += new System.EventHandler(this.sysToolBar1_OnPrintPreviewClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            this.sysToolBar1.OnBtnQualityRuleClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityRuleClicked);
            this.sysToolBar1.OnBtnQualityTestClicked += new System.EventHandler(this.testData_Click);
            this.sysToolBar1.OnBtnQualityAuditClicked += new System.EventHandler(this.dataAudit_Click);
            this.sysToolBar1.OnBtnQualityDataClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityDataClicked);
            this.sysToolBar1.OnBtnQualityImageClicked += new System.EventHandler(this.simpleButton1_Click);
            this.sysToolBar1.BtnSaveDefaultClick += new System.EventHandler(this.sysToolBar1_BtnSaveDefaultClick);
            // 
            // toolTipController1
            // 
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.BottomLeft;
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 654);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "FrmChart";
            this.Text = "质控";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSub)).EndInit();
            this.tabSub.ResumeLayout(false);
            this.xtItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlQcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckShowType_I.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckShowType_S.Properties)).EndInit();
            this.xtProcess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlQcAuditItem)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plData)).EndInit();
            this.plData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckDataAudit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckDataAll.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            this.panelControl6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSet_con.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSet_Sd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plPic)).EndInit();
            this.plPic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.pnlChat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(radarDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(radarLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(radarLineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceType_UD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_Radar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_LJ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_MC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceType_BDL.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.gbQcPointData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceData_Rep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceData_con.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceData_All.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceShow_Fal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceShow_All.Properties)).EndInit();
            this.gbShowType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cePointLast.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceLevel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTransverse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSerieType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePointType.Properties)).EndInit();
            this.gbQcType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Day.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Ave.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_All.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRes_Con.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plItem)).EndInit();
            this.plItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.BindingSource bdqcvalue;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.DateEdit dtEnd;
        private DevExpress.XtraEditors.DateEdit dtBegin;
        private dcl.client.control.SelectDicInstrument lue_Apparatus;
        private DevExpress.Utils.ToolTipController toolTipController1;
        private DevExpress.XtraEditors.PanelControl plData;
        private DevExpress.XtraEditors.PanelControl plItem;
        private DevExpress.XtraEditors.PanelControl plPic;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.GroupBox gbQcType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gbQcPointData;
        private DevExpress.XtraEditors.CheckEdit ceSet_con;
        private DevExpress.XtraEditors.CheckEdit ceSet_Sd;
        private DevExpress.XtraEditors.CheckEdit ceRes_Day;
        private DevExpress.XtraEditors.CheckEdit ceRes_Ave;
        private DevExpress.XtraEditors.CheckEdit ceRes_All;
        private DevExpress.XtraEditors.CheckEdit ceRes_Con;
        private DevExpress.XtraEditors.CheckEdit ceShow_All;
        private DevExpress.XtraEditors.CheckEdit ceData_Rep;
        private DevExpress.XtraEditors.CheckEdit ceData_con;
        private DevExpress.XtraEditors.CheckEdit ceData_All;
        private DevExpress.XtraEditors.CheckEdit ceShow_Fal;
        private DevExpress.XtraEditors.CheckEdit ceType_MC;
        private DevExpress.XtraEditors.CheckEdit ceType_LJ;
        private DevExpress.XtraEditors.CheckEdit ceType_UD;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbOth;
        private DevExpress.XtraEditors.CheckEdit ckShowType_I;
        private DevExpress.XtraEditors.CheckEdit ckShowType_S;
        private dcl.client.control.SelectDicLabProfession lueType;
        private DevExpress.XtraEditors.CheckEdit ceType_BDL;
        private System.Windows.Forms.GroupBox gbShowType;
        private DevExpress.XtraEditors.CheckEdit ceTransverse;
        private DevExpress.XtraEditors.CheckEdit ceSerieType;
        private DevExpress.XtraEditors.CheckEdit cePointType;
        private DevExpress.XtraEditors.CheckEdit ceLevel;
        private DevExpress.XtraEditors.CheckEdit ceType_Radar;
        private System.Windows.Forms.Panel pnlChat;
        private DevExpress.Utils.ToolTipController toolTipController2;
        private DevExpress.XtraTab.XtraTabControl tabSub;
        private DevExpress.XtraTab.XtraTabPage xtItem;
        private DevExpress.XtraTab.XtraTabPage xtProcess;
        private DevExpress.XtraTreeList.TreeList tlQcAuditItem;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private System.Windows.Forms.Panel panel6;
        private DevExpress.XtraEditors.SimpleButton btnDataAudit;
        private DevExpress.XtraEditors.CheckEdit ckDataAudit;
        private DevExpress.XtraEditors.CheckEdit ckDataAll;
        private System.Windows.Forms.BindingSource bsGcData;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.SimpleButton updata_SimpleButton;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.CheckEdit cePointLast;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn tlcItem;
        private DevExpress.XtraTreeList.TreeList tlQcItem;
        private DevExpress.XtraEditors.CheckEdit checkEditAll;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_info_group;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_rem;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_two_audit_name;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_two_audit_date;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_next_time;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_fun;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_reson;
        private DevExpress.XtraGrid.Columns.GridColumn userName;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_ns;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_sd;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_x;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_meas;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_no;
        private DevExpress.XtraGrid.Columns.GridColumn itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_date;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ckEffective;
        private DevExpress.XtraGrid.Columns.GridColumn gcIsEff;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLot;
        private DevExpress.XtraGrid.GridControl gcLot;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcActualCV;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcActualSD;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcActualAVG;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcN;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcCV;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcSD;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcAVG;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcItem;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.GridControl gcInfo;
        private DevExpress.XtraEditors.TextEdit txtSet;
        private DevExpress.XtraEditors.LabelControl labelControl20;
    }
}