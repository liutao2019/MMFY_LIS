
using dcl.client.common;
using dcl.client.control;
using lis.client.control;

namespace wf.client.reagent
{
    partial class FrmReagentPurchase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReagentPurchase));
            this.colRpcd_reacount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colReagentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gcReaPurchase = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.面板设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bsReaPurchase = new System.Windows.Forms.BindingSource(this.components);
            this.gvReaPurchase = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRpc_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_applier = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_auditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_applydate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_auditdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_printflag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_rejectreason = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpc_remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lbRecordCount = new System.Windows.Forms.Label();
            this.cbeFlag = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.selectDicReaSupplier1 = new dcl.client.control.SelectDicReaSupplier();
            this.roundPanelGroup1 = new wf.client.reagent.RoundPanelGroup();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.txtBarSearchCondition = new DevExpress.XtraEditors.TextEdit();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBarSearchPatType = new DevExpress.XtraEditors.LookUpEdit();
            this.selectDicReaGroup1 = new dcl.client.control.SelectDicReaGroup();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dtEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.dtBegin = new DevExpress.XtraEditors.DateEdit();
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this.gcReaDetail = new DevExpress.XtraGrid.GridControl();
            this.bsPurchaseDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gvReadetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRpcd_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsSup = new System.Windows.Forms.BindingSource(this.components);
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.reagentEditor1 = new wf.client.reagent.ReaControl.ReagentPurchaseEditor();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.txtReaDate = new DevExpress.XtraEditors.DateEdit();
            this.txtReaSid = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReaPurchase)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaPurchase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaPurchase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarSearchCondition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBarSearchPatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReaDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPurchaseDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReadetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaSid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // colRpcd_reacount
            // 
            this.colRpcd_reacount.AppearanceCell.Options.UseTextOptions = true;
            this.colRpcd_reacount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colRpcd_reacount.Caption = "采购数量";
            this.colRpcd_reacount.ColumnEdit = this.repositoryItemTextEdit1;
            this.colRpcd_reacount.FieldName = "Rpcd_reacount";
            this.colRpcd_reacount.Name = "colRpcd_reacount";
            this.colRpcd_reacount.Visible = true;
            this.colRpcd_reacount.VisibleIndex = 3;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // colReagentName
            // 
            this.colReagentName.Caption = "试剂名称";
            this.colReagentName.FieldName = "ReagentName";
            this.colReagentName.Name = "colReagentName";
            this.colReagentName.OptionsColumn.AllowEdit = false;
            this.colReagentName.Visible = true;
            this.colReagentName.VisibleIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.sysToolBar1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1391, 69);
            this.panelControl1.TabIndex = 0;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(2, 2);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1387, 65);
            this.sysToolBar1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 69);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1391, 604);
            this.panelControl2.TabIndex = 1;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl4);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl3);
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl7);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl5);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl6);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1387, 600);
            this.splitContainerControl1.SplitterPosition = 391;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.gcReaPurchase);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 140);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(391, 429);
            this.panelControl4.TabIndex = 2;
            // 
            // gcReaPurchase
            // 
            this.gcReaPurchase.ContextMenuStrip = this.contextMenuStrip1;
            this.gcReaPurchase.DataSource = this.bsReaPurchase;
            this.gcReaPurchase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReaPurchase.Location = new System.Drawing.Point(2, 2);
            this.gcReaPurchase.MainView = this.gvReaPurchase;
            this.gcReaPurchase.Name = "gcReaPurchase";
            this.gcReaPurchase.Size = new System.Drawing.Size(387, 425);
            this.gcReaPurchase.TabIndex = 0;
            this.gcReaPurchase.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReaPurchase});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.面板设置ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 面板设置ToolStripMenuItem
            // 
            this.面板设置ToolStripMenuItem.Name = "面板设置ToolStripMenuItem";
            this.面板设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.面板设置ToolStripMenuItem.Text = "面板设置";
            this.面板设置ToolStripMenuItem.Click += new System.EventHandler(this.面板设置ToolStripMenuItem_Click);
            // 
            // bsReaPurchase
            // 
            this.bsReaPurchase.DataSource = typeof(dcl.entity.EntityReaPurchase);
            // 
            // gvReaPurchase
            // 
            this.gvReaPurchase.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRpc_no,
            this.colRpc_applier,
            this.colRpc_auditor,
            this.colRpc_applydate,
            this.colRpc_auditdate,
            this.colRpc_status,
            this.colRpc_printflag,
            this.colRpc_rejectreason,
            this.colRpc_remark});
            this.gvReaPurchase.GridControl = this.gcReaPurchase;
            this.gvReaPurchase.Name = "gvReaPurchase";
            this.gvReaPurchase.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gvReaPurchase.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReaPurchase.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvReaPurchase.OptionsSelection.MultiSelect = true;
            this.gvReaPurchase.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvReaPurchase.OptionsView.ColumnAutoWidth = false;
            this.gvReaPurchase.OptionsView.ShowDetailButtons = false;
            this.gvReaPurchase.OptionsView.ShowGroupPanel = false;
            this.gvReaPurchase.OptionsView.ShowIndicator = false;
            this.gvReaPurchase.Click += new System.EventHandler(this.gvReaPurchase_Click);
            // 
            // colRpc_no
            // 
            this.colRpc_no.Caption = "采购单号";
            this.colRpc_no.FieldName = "Rpc_no";
            this.colRpc_no.Name = "colRpc_no";
            this.colRpc_no.OptionsColumn.AllowEdit = false;
            this.colRpc_no.Visible = true;
            this.colRpc_no.VisibleIndex = 1;
            this.colRpc_no.Width = 174;
            // 
            // colRpc_applier
            // 
            this.colRpc_applier.Caption = "采购人";
            this.colRpc_applier.FieldName = "Rpc_applier";
            this.colRpc_applier.Name = "colRpc_applier";
            this.colRpc_applier.OptionsColumn.AllowEdit = false;
            this.colRpc_applier.Visible = true;
            this.colRpc_applier.VisibleIndex = 2;
            this.colRpc_applier.Width = 84;
            // 
            // colRpc_auditor
            // 
            this.colRpc_auditor.Caption = "审核人";
            this.colRpc_auditor.FieldName = "Rpc_auditor";
            this.colRpc_auditor.Name = "colRpc_auditor";
            this.colRpc_auditor.OptionsColumn.AllowEdit = false;
            this.colRpc_auditor.Visible = true;
            this.colRpc_auditor.VisibleIndex = 4;
            this.colRpc_auditor.Width = 96;
            // 
            // colRpc_applydate
            // 
            this.colRpc_applydate.Caption = "采购日期";
            this.colRpc_applydate.FieldName = "Rpc_applydate";
            this.colRpc_applydate.Name = "colRpc_applydate";
            this.colRpc_applydate.OptionsColumn.AllowEdit = false;
            this.colRpc_applydate.Visible = true;
            this.colRpc_applydate.VisibleIndex = 3;
            this.colRpc_applydate.Width = 146;
            // 
            // colRpc_auditdate
            // 
            this.colRpc_auditdate.Caption = "审核日期";
            this.colRpc_auditdate.FieldName = "Rpc_auditdate";
            this.colRpc_auditdate.Name = "colRpc_auditdate";
            this.colRpc_auditdate.OptionsColumn.AllowEdit = false;
            this.colRpc_auditdate.Visible = true;
            this.colRpc_auditdate.VisibleIndex = 5;
            this.colRpc_auditdate.Width = 169;
            // 
            // colRpc_status
            // 
            this.colRpc_status.FieldName = "Rpc_status";
            this.colRpc_status.Name = "colRpc_status";
            // 
            // colRpc_printflag
            // 
            this.colRpc_printflag.FieldName = "Rpc_printflag";
            this.colRpc_printflag.Name = "colRpc_printflag";
            // 
            // colRpc_rejectreason
            // 
            this.colRpc_rejectreason.Caption = "回退原因";
            this.colRpc_rejectreason.FieldName = "Rpc_returnreason";
            this.colRpc_rejectreason.Name = "colRpc_rejectreason";
            this.colRpc_rejectreason.OptionsColumn.AllowEdit = false;
            this.colRpc_rejectreason.Visible = true;
            this.colRpc_rejectreason.VisibleIndex = 6;
            this.colRpc_rejectreason.Width = 186;
            // 
            // colRpc_remark
            // 
            this.colRpc_remark.Caption = "备注";
            this.colRpc_remark.FieldName = "Rpc_remark";
            this.colRpc_remark.Name = "colRpc_remark";
            this.colRpc_remark.OptionsColumn.AllowEdit = false;
            this.colRpc_remark.Visible = true;
            this.colRpc_remark.VisibleIndex = 7;
            this.colRpc_remark.Width = 366;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.lbRecordCount);
            this.panelControl3.Controls.Add(this.cbeFlag);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 569);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(391, 31);
            this.panelControl3.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(185, 7);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 164;
            this.labelControl3.Text = "状态：";
            this.labelControl3.Visible = false;
            // 
            // lbRecordCount
            // 
            this.lbRecordCount.AutoSize = true;
            this.lbRecordCount.Location = new System.Drawing.Point(52, 7);
            this.lbRecordCount.Name = "lbRecordCount";
            this.lbRecordCount.Size = new System.Drawing.Size(62, 14);
            this.lbRecordCount.TabIndex = 162;
            this.lbRecordCount.Text = "记录数：0";
            // 
            // cbeFlag
            // 
            this.cbeFlag.EditValue = "全部";
            this.cbeFlag.Location = new System.Drawing.Point(237, 3);
            this.cbeFlag.Name = "cbeFlag";
            this.cbeFlag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeFlag.Properties.Items.AddRange(new object[] {
            "全部",
            "未审核",
            "未报告",
            "未打印",
            "已打印"});
            this.cbeFlag.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeFlag.Size = new System.Drawing.Size(92, 20);
            this.cbeFlag.TabIndex = 163;
            this.cbeFlag.Visible = false;
            this.cbeFlag.EditValueChanged += new System.EventHandler(this.cbeAge_EditValueChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.selectDicReaSupplier1);
            this.groupControl1.Controls.Add(this.roundPanelGroup1);
            this.groupControl1.Controls.Add(this.labelControl17);
            this.groupControl1.Controls.Add(this.txtBarSearchCondition);
            this.groupControl1.Controls.Add(this.labelControl18);
            this.groupControl1.Controls.Add(this.cmbBarSearchPatType);
            this.groupControl1.Controls.Add(this.selectDicReaGroup1);
            this.groupControl1.Controls.Add(this.labelControl22);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.dtEnd);
            this.groupControl1.Controls.Add(this.labelControl21);
            this.groupControl1.Controls.Add(this.dtBegin);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(391, 140);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "查询";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(210, 56);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 181;
            this.labelControl1.Text = "供货商";
            // 
            // selectDicReaSupplier1
            // 
            this.selectDicReaSupplier1.AddEmptyRow = true;
            this.selectDicReaSupplier1.BindByValue = false;
            this.selectDicReaSupplier1.colDisplay = "";
            this.selectDicReaSupplier1.colExtend1 = "";
            this.selectDicReaSupplier1.colInCode = "";
            this.selectDicReaSupplier1.colPY = "";
            this.selectDicReaSupplier1.colSeq = "";
            this.selectDicReaSupplier1.colValue = "";
            this.selectDicReaSupplier1.colWB = "";
            this.selectDicReaSupplier1.displayMember = null;
            this.selectDicReaSupplier1.EnterMoveNext = true;
            this.selectDicReaSupplier1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaSupplier1.KeyUpDownMoveNext = false;
            this.selectDicReaSupplier1.LoadDataOnDesignMode = true;
            this.selectDicReaSupplier1.Location = new System.Drawing.Point(252, 52);
            this.selectDicReaSupplier1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaSupplier1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaSupplier1.Name = "selectDicReaSupplier1";
            this.selectDicReaSupplier1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaSupplier1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaSupplier1.Readonly = false;
            this.selectDicReaSupplier1.SaveSourceID = false;
            this.selectDicReaSupplier1.SelectFilter = null;
            this.selectDicReaSupplier1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaSupplier1.SelectOnly = true;
            this.selectDicReaSupplier1.Size = new System.Drawing.Size(124, 21);
            this.selectDicReaSupplier1.TabIndex = 180;
            this.selectDicReaSupplier1.UseExtend = false;
            this.selectDicReaSupplier1.valueMember = null;
            // 
            // roundPanelGroup1
            // 
            this.roundPanelGroup1.Location = new System.Drawing.Point(9, 111);
            this.roundPanelGroup1.Name = "roundPanelGroup1";
            this.roundPanelGroup1.Size = new System.Drawing.Size(367, 23);
            this.roundPanelGroup1.TabIndex = 179;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(218, 84);
            this.labelControl17.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(24, 14);
            this.labelControl17.TabIndex = 178;
            this.labelControl17.Text = "键值";
            // 
            // txtBarSearchCondition
            // 
            this.txtBarSearchCondition.Location = new System.Drawing.Point(252, 80);
            this.txtBarSearchCondition.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtBarSearchCondition.Name = "txtBarSearchCondition";
            this.txtBarSearchCondition.Size = new System.Drawing.Size(124, 20);
            this.txtBarSearchCondition.TabIndex = 176;
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(9, 84);
            this.labelControl18.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(48, 14);
            this.labelControl18.TabIndex = 177;
            this.labelControl18.Text = "关键查询";
            // 
            // cmbBarSearchPatType
            // 
            this.cmbBarSearchPatType.EditValue = "rea_sid_int";
            this.cmbBarSearchPatType.Location = new System.Drawing.Point(75, 80);
            this.cmbBarSearchPatType.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.cmbBarSearchPatType.Name = "cmbBarSearchPatType";
            this.cmbBarSearchPatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBarSearchPatType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "类型"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("value", "value", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.NullText = "";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";
            this.cmbBarSearchPatType.Size = new System.Drawing.Size(122, 20);
            this.cmbBarSearchPatType.TabIndex = 175;
            // 
            // selectDicReaGroup1
            // 
            this.selectDicReaGroup1.AddEmptyRow = true;
            this.selectDicReaGroup1.BindByValue = false;
            this.selectDicReaGroup1.colDisplay = "";
            this.selectDicReaGroup1.colExtend1 = "";
            this.selectDicReaGroup1.colInCode = "";
            this.selectDicReaGroup1.colPY = "";
            this.selectDicReaGroup1.colSeq = "";
            this.selectDicReaGroup1.colValue = "";
            this.selectDicReaGroup1.colWB = "";
            this.selectDicReaGroup1.displayMember = null;
            this.selectDicReaGroup1.EnterMoveNext = true;
            this.selectDicReaGroup1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaGroup1.KeyUpDownMoveNext = false;
            this.selectDicReaGroup1.LoadDataOnDesignMode = true;
            this.selectDicReaGroup1.Location = new System.Drawing.Point(75, 52);
            this.selectDicReaGroup1.MaximumSize = new System.Drawing.Size(583, 24);
            this.selectDicReaGroup1.MinimumSize = new System.Drawing.Size(58, 24);
            this.selectDicReaGroup1.Name = "selectDicReaGroup1";
            this.selectDicReaGroup1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaGroup1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaGroup1.Readonly = false;
            this.selectDicReaGroup1.SaveSourceID = false;
            this.selectDicReaGroup1.SelectFilter = null;
            this.selectDicReaGroup1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaGroup1.SelectOnly = true;
            this.selectDicReaGroup1.Size = new System.Drawing.Size(122, 24);
            this.selectDicReaGroup1.TabIndex = 174;
            this.selectDicReaGroup1.UseExtend = false;
            this.selectDicReaGroup1.valueMember = null;
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(9, 56);
            this.labelControl22.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(48, 14);
            this.labelControl22.TabIndex = 173;
            this.labelControl22.Text = "试剂组别";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(9, 28);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 169;
            this.labelControl4.Text = "采购日期";
            // 
            // dtEnd
            // 
            this.dtEnd.EditValue = null;
            this.dtEnd.Location = new System.Drawing.Point(252, 25);
            this.dtEnd.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Size = new System.Drawing.Size(124, 20);
            this.dtEnd.TabIndex = 172;
            this.dtEnd.EditValueChanged += new System.EventHandler(this.dtEnd_EditValueChanged);
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(218, 28);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(12, 14);
            this.labelControl21.TabIndex = 171;
            this.labelControl21.Text = "—";
            // 
            // dtBegin
            // 
            this.dtBegin.EditValue = null;
            this.dtBegin.Location = new System.Drawing.Point(75, 26);
            this.dtBegin.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Size = new System.Drawing.Size(122, 20);
            this.dtBegin.TabIndex = 170;
            this.dtBegin.EditValueChanged += new System.EventHandler(this.dtBegin_EditValueChanged);
            // 
            // panelControl7
            // 
            this.panelControl7.Controls.Add(this.gcReaDetail);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl7.Location = new System.Drawing.Point(195, 44);
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(796, 556);
            this.panelControl7.TabIndex = 2;
            // 
            // gcReaDetail
            // 
            this.gcReaDetail.DataSource = this.bsPurchaseDetail;
            this.gcReaDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReaDetail.Location = new System.Drawing.Point(2, 2);
            this.gcReaDetail.MainView = this.gvReadetail;
            this.gcReaDetail.Name = "gcReaDetail";
            this.gcReaDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemLookUpEdit1});
            this.gcReaDetail.Size = new System.Drawing.Size(792, 552);
            this.gcReaDetail.TabIndex = 0;
            this.gcReaDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReadetail});
            // 
            // bsPurchaseDetail
            // 
            this.bsPurchaseDetail.DataSource = typeof(dcl.entity.EntityReaPurchaseDetail);
            // 
            // gvReadetail
            // 
            this.gvReadetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRpcd_no,
            this.colReagentName,
            this.gridColumn1,
            this.colRpcd_reacount,
            this.gridColumn2,
            this.gridColumn3});
            this.gvReadetail.GridControl = this.gcReaDetail;
            this.gvReadetail.Name = "gvReadetail";
            this.gvReadetail.OptionsView.ShowGroupPanel = false;
            // 
            // colRpcd_no
            // 
            this.colRpcd_no.Caption = "采购单号";
            this.colRpcd_no.FieldName = "Rpcd_no";
            this.colRpcd_no.Name = "colRpcd_no";
            this.colRpcd_no.OptionsColumn.AllowEdit = false;
            this.colRpcd_no.Visible = true;
            this.colRpcd_no.VisibleIndex = 0;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "试剂规格";
            this.gridColumn1.FieldName = "ReagentPackage";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "试剂编码";
            this.gridColumn2.FieldName = "Rpcd_reaid";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "供货商";
            this.gridColumn3.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.gridColumn3.FieldName = "sup_id";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.ActionButtonIndex = 1;
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.CaseSensitiveSearch = true;
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rsupplier_id", "编码", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rsupplier_name", "名称", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repositoryItemLookUpEdit1.DataSource = this.bsSup;
            this.repositoryItemLookUpEdit1.DisplayMember = "Rsupplier_name";
            this.repositoryItemLookUpEdit1.DropDownRows = 15;
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repositoryItemLookUpEdit1.ValueMember = "Rsupplier_id";
            // 
            // bsSup
            // 
            this.bsSup.DataSource = typeof(dcl.entity.EntityDicReaSupplier);
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.reagentEditor1);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl5.Location = new System.Drawing.Point(195, 0);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(796, 44);
            this.panelControl5.TabIndex = 0;
            // 
            // reagentEditor1
            // 
            this.reagentEditor1.BackColor = System.Drawing.Color.Transparent;
            this.reagentEditor1.Caption = "试剂信息";
            this.reagentEditor1.GroupID = null;
            this.reagentEditor1.GroupName = null;
            this.reagentEditor1.listReaDetail = null;
            this.reagentEditor1.Location = new System.Drawing.Point(16, 13);
            this.reagentEditor1.Name = "reagentEditor1";
            this.reagentEditor1.PatientsMi = null;
            this.reagentEditor1.PopupFormHeight = 0;
            this.reagentEditor1.PopupFormWidth = 0;
            this.reagentEditor1.ShowCaption = true;
            this.reagentEditor1.Size = new System.Drawing.Size(398, 21);
            this.reagentEditor1.SupID = null;
            this.reagentEditor1.SupName = null;
            this.reagentEditor1.TabIndex = 0;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.layoutControl1);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl6.Location = new System.Drawing.Point(0, 0);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(195, 600);
            this.panelControl6.TabIndex = 1;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.txtReaDate);
            this.layoutControl1.Controls.Add(this.txtReaSid);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(191, 596);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaPurchase, "Rpc_returnreason", true));
            this.textEdit1.Location = new System.Drawing.Point(63, 60);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(116, 20);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 6;
            this.textEdit1.Visible = false;
            // 
            // txtReaDate
            // 
            this.txtReaDate.EditValue = null;
            this.txtReaDate.Location = new System.Drawing.Point(63, 36);
            this.txtReaDate.Name = "txtReaDate";
            this.txtReaDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReaDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReaDate.Size = new System.Drawing.Size(116, 20);
            this.txtReaDate.StyleController = this.layoutControl1;
            this.txtReaDate.TabIndex = 5;
            // 
            // txtReaSid
            // 
            this.txtReaSid.Location = new System.Drawing.Point(63, 12);
            this.txtReaSid.Name = "txtReaSid";
            this.txtReaSid.Properties.ReadOnly = true;
            this.txtReaSid.Size = new System.Drawing.Size(116, 20);
            this.txtReaSid.StyleController = this.layoutControl1;
            this.txtReaSid.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(191, 596);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtReaSid;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(171, 24);
            this.layoutControlItem1.Text = "采购单号";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtReaDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(171, 24);
            this.layoutControlItem2.Text = "采购时间";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.textEdit1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(171, 528);
            this.layoutControlItem3.Text = "回退原因";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            // 
            // FrmReagentPurchase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 673);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReagentPurchase";
            this.Text = "试剂申领";
            this.Load += new System.EventHandler(this.FrmReagenPurchase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReaPurchase)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsReaPurchase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaPurchase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarSearchCondition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBarSearchPatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReaDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPurchaseDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReadetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaSid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        public DevExpress.XtraEditors.DateEdit dtEnd;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        public DevExpress.XtraEditors.DateEdit dtBegin;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private SelectDicReaGroup selectDicReaGroup1;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        public DevExpress.XtraEditors.TextEdit txtBarSearchCondition;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        public DevExpress.XtraEditors.LookUpEdit cmbBarSearchPatType;
        private RoundPanelGroup roundPanelGroup1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Label lbRecordCount;
        private DevExpress.XtraEditors.ComboBoxEdit cbeFlag;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraGrid.GridControl gcReaPurchase;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReaPurchase;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtReaSid;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.GridControl gcReaDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReadetail;
        private System.Windows.Forms.BindingSource bsPurchaseDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colRpcd_no;
        private DevExpress.XtraGrid.Columns.GridColumn colRpcd_reacount;
        private DevExpress.XtraGrid.Columns.GridColumn colReagentName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.DateEdit txtReaDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.BindingSource bsReaPurchase;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_no;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_applier;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_auditor;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_applydate;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_auditdate;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_status;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_printflag;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_rejectreason;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_remark;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 面板设置ToolStripMenuItem;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private SelectDicReaSupplier selectDicReaSupplier1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private ReaControl.ReagentPurchaseEditor reagentEditor1;
        private ctlRepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private System.Windows.Forms.BindingSource bsSup;
    }
}