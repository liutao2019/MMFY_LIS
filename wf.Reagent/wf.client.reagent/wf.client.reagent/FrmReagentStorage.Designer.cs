
using dcl.client.common;
using dcl.client.control;
using lis.client.control;


namespace wf.client.reagent
{
    partial class FrmReagentStorage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReagentStorage));
            this.bsReaSetting = new System.Windows.Forms.BindingSource(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gcReaStorage = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.面板设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bsReaStorage = new System.Windows.Forms.BindingSource(this.components);
            this.gvReaStorage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRsr_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_applier = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_auditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_applydate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_auditdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsr_printflag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcReaPurchase = new DevExpress.XtraGrid.GridControl();
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
            this.roundPanelGroup1 = new wf.client.reagent.RoundStorageGroup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.selectDicReaSupplier1 = new dcl.client.control.SelectDicReaSupplier();
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
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuDelItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bsReaStorageDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gvReadetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRsd_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReagentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit6 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRsd_reacount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsSup = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsGroup = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit2 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsPdt = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit3 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsUnit = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit4 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsPos = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit5 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsCon = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit7 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit8 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit9 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemTextEdit10 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.lookUpEdit3 = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEdit2 = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEdit1 = new DevExpress.XtraEditors.LookUpEdit();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            this.txtPurno = new DevExpress.XtraEditors.TextEdit();
            this.txtReason = new DevExpress.XtraEditors.TextEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.selectDicReaStoreCondition1 = new dcl.client.control.SelectDicReaStoreCondition();
            this.selectDicReaStorePosition1 = new dcl.client.control.SelectDicReaStorePosition();
            this.txtInvoice = new DevExpress.XtraEditors.TextEdit();
            this.txtPrice = new DevExpress.XtraEditors.TextEdit();
            this.selectDicReaUnit1 = new dcl.client.control.SelectDicReaUnit();
            this.selectDicReaSupplier2 = new dcl.client.control.SelectDicReaSupplier();
            this.selectDicReaProduct1 = new dcl.client.control.SelectDicReaProduct();
            this.txtBatchNo = new DevExpress.XtraEditors.TextEdit();
            this.txtCount = new DevExpress.XtraEditors.TextEdit();
            this.txtPackage = new DevExpress.XtraEditors.TextEdit();
            this.selectDicReaGroup2 = new dcl.client.control.SelectDicReaGroup();
            this.selectDicReaSetting1 = new dcl.client.control.SelectDicReaSetting();
            this.txtReaDate = new DevExpress.XtraEditors.DateEdit();
            this.txtReaSid = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem23 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem24 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem25 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem26 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem27 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem21 = new DevExpress.XtraLayout.LayoutControlItem();
            this.gvReaPuechase = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReaStorage)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaStorage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaStorage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcReaPurchase)).BeginInit();
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
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaStorageDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReadetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPdt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPurno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPackage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaSid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaPuechase)).BeginInit();
            this.SuspendLayout();
            // 
            // bsReaSetting
            // 
            this.bsReaSetting.DataSource = typeof(dcl.entity.EntityReaSetting);
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
            this.panelControl4.Controls.Add(this.gcReaStorage);
            this.panelControl4.Controls.Add(this.gcReaPurchase);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 134);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(391, 435);
            this.panelControl4.TabIndex = 2;
            // 
            // gcReaStorage
            // 
            this.gcReaStorage.ContextMenuStrip = this.contextMenuStrip1;
            this.gcReaStorage.DataSource = this.bsReaStorage;
            this.gcReaStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReaStorage.Location = new System.Drawing.Point(2, 2);
            this.gcReaStorage.MainView = this.gvReaStorage;
            this.gcReaStorage.Name = "gcReaStorage";
            this.gcReaStorage.Size = new System.Drawing.Size(387, 231);
            this.gcReaStorage.TabIndex = 0;
            this.gcReaStorage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReaStorage});
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
            // bsReaStorage
            // 
            this.bsReaStorage.DataSource = typeof(dcl.entity.EntityReaStorage);
            // 
            // gvReaStorage
            // 
            this.gvReaStorage.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRsr_no,
            this.colRsr_applier,
            this.colRsr_auditor,
            this.colRsr_applydate,
            this.colRsr_auditdate,
            this.colRsr_status,
            this.colRsr_printflag});
            this.gvReaStorage.GridControl = this.gcReaStorage;
            this.gvReaStorage.Name = "gvReaStorage";
            this.gvReaStorage.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gvReaStorage.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReaStorage.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvReaStorage.OptionsSelection.MultiSelect = true;
            this.gvReaStorage.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvReaStorage.OptionsView.ColumnAutoWidth = false;
            this.gvReaStorage.OptionsView.ShowDetailButtons = false;
            this.gvReaStorage.OptionsView.ShowGroupPanel = false;
            this.gvReaStorage.OptionsView.ShowIndicator = false;
            this.gvReaStorage.Click += new System.EventHandler(this.gvReaStorage_Click);
            // 
            // colRsr_no
            // 
            this.colRsr_no.Caption = "入库单号";
            this.colRsr_no.FieldName = "Rsr_no";
            this.colRsr_no.Name = "colRsr_no";
            this.colRsr_no.OptionsColumn.AllowEdit = false;
            this.colRsr_no.Visible = true;
            this.colRsr_no.VisibleIndex = 1;
            this.colRsr_no.Width = 174;
            // 
            // colRsr_applier
            // 
            this.colRsr_applier.Caption = "操作人";
            this.colRsr_applier.FieldName = "Rsr_operator";
            this.colRsr_applier.Name = "colRsr_applier";
            this.colRsr_applier.OptionsColumn.AllowEdit = false;
            this.colRsr_applier.Visible = true;
            this.colRsr_applier.VisibleIndex = 2;
            this.colRsr_applier.Width = 84;
            // 
            // colRsr_auditor
            // 
            this.colRsr_auditor.Caption = "审核人";
            this.colRsr_auditor.FieldName = "Rsr_auditor";
            this.colRsr_auditor.Name = "colRsr_auditor";
            this.colRsr_auditor.OptionsColumn.AllowEdit = false;
            this.colRsr_auditor.Visible = true;
            this.colRsr_auditor.VisibleIndex = 4;
            this.colRsr_auditor.Width = 96;
            // 
            // colRsr_applydate
            // 
            this.colRsr_applydate.Caption = "入库日期";
            this.colRsr_applydate.FieldName = "Rsr_date";
            this.colRsr_applydate.Name = "colRsr_applydate";
            this.colRsr_applydate.OptionsColumn.AllowEdit = false;
            this.colRsr_applydate.Visible = true;
            this.colRsr_applydate.VisibleIndex = 3;
            this.colRsr_applydate.Width = 146;
            // 
            // colRsr_auditdate
            // 
            this.colRsr_auditdate.Caption = "审核日期";
            this.colRsr_auditdate.FieldName = "Rsr_auditdate";
            this.colRsr_auditdate.Name = "colRsr_auditdate";
            this.colRsr_auditdate.OptionsColumn.AllowEdit = false;
            this.colRsr_auditdate.Visible = true;
            this.colRsr_auditdate.VisibleIndex = 5;
            this.colRsr_auditdate.Width = 169;
            // 
            // colRsr_status
            // 
            this.colRsr_status.FieldName = "Rsr_status";
            this.colRsr_status.Name = "colRsr_status";
            // 
            // colRsr_printflag
            // 
            this.colRsr_printflag.FieldName = "Rsr_printflag";
            this.colRsr_printflag.Name = "colRsr_printflag";
            // 
            // gcReaPurchase
            // 
            this.gcReaPurchase.DataSource = this.bsReaPurchase;
            this.gcReaPurchase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gcReaPurchase.Location = new System.Drawing.Point(2, 233);
            this.gcReaPurchase.MainView = this.gvReaPurchase;
            this.gcReaPurchase.Name = "gcReaPurchase";
            this.gcReaPurchase.Size = new System.Drawing.Size(387, 200);
            this.gcReaPurchase.TabIndex = 1;
            this.gcReaPurchase.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReaPurchase});
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
            this.gvReaPurchase.DoubleClick += new System.EventHandler(this.gvReaPurchase_DoubleClick);
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
            this.groupControl1.Controls.Add(this.roundPanelGroup1);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.selectDicReaSupplier1);
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
            this.groupControl1.Size = new System.Drawing.Size(391, 134);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "查询";
            // 
            // roundPanelGroup1
            // 
            this.roundPanelGroup1.Location = new System.Drawing.Point(5, 104);
            this.roundPanelGroup1.Name = "roundPanelGroup1";
            this.roundPanelGroup1.Size = new System.Drawing.Size(315, 23);
            this.roundPanelGroup1.TabIndex = 182;
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
            this.labelControl4.Location = new System.Drawing.Point(33, 28);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 169;
            this.labelControl4.Text = "日期";
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
            this.panelControl7.Location = new System.Drawing.Point(252, 44);
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(739, 556);
            this.panelControl7.TabIndex = 2;
            // 
            // gcReaDetail
            // 
            this.gcReaDetail.ContextMenuStrip = this.contextMenuStrip2;
            this.gcReaDetail.DataSource = this.bsReaStorageDetail;
            this.gcReaDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReaDetail.Location = new System.Drawing.Point(2, 2);
            this.gcReaDetail.MainView = this.gvReadetail;
            this.gcReaDetail.Name = "gcReaDetail";
            this.gcReaDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemLookUpEdit1,
            this.ctlRepositoryItemLookUpEdit1,
            this.repositoryItemTextEdit3,
            this.repositoryItemDateEdit1,
            this.ctlRepositoryItemLookUpEdit2,
            this.ctlRepositoryItemLookUpEdit3,
            this.repositoryItemTextEdit4,
            this.repositoryItemTextEdit5,
            this.repositoryItemTextEdit6,
            this.ctlRepositoryItemLookUpEdit4,
            this.ctlRepositoryItemLookUpEdit5,
            this.repositoryItemTextEdit7,
            this.repositoryItemTextEdit8,
            this.repositoryItemTextEdit9,
            this.repositoryItemTextEdit10,
            this.ctlRepositoryItemLookUpEdit6,
            this.ctlRepositoryItemLookUpEdit7,
            this.ctlRepositoryItemLookUpEdit8,
            this.ctlRepositoryItemLookUpEdit9});
            this.gcReaDetail.Size = new System.Drawing.Size(735, 552);
            this.gcReaDetail.TabIndex = 0;
            this.gcReaDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReadetail});
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDelItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(125, 26);
            this.contextMenuStrip2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip2_ItemClicked);
            // 
            // MenuDelItem
            // 
            this.MenuDelItem.Name = "MenuDelItem";
            this.MenuDelItem.Size = new System.Drawing.Size(124, 22);
            this.MenuDelItem.Text = "删除记录";
            // 
            // bsReaStorageDetail
            // 
            this.bsReaStorageDetail.DataSource = typeof(dcl.entity.EntityReaStorageDetail);
            // 
            // gvReadetail
            // 
            this.gvReadetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRsd_no,
            this.colReagentName,
            this.gridColumn1,
            this.colRsd_reacount,
            this.gridColumn3,
            this.gridColumn2,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn4,
            this.gridColumn9});
            this.gvReadetail.GridControl = this.gcReaDetail;
            this.gvReadetail.Name = "gvReadetail";
            this.gvReadetail.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gvReadetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReadetail.OptionsSelection.MultiSelect = true;
            this.gvReadetail.OptionsView.ColumnAutoWidth = false;
            this.gvReadetail.OptionsView.ShowDetailButtons = false;
            this.gvReadetail.OptionsView.ShowGroupPanel = false;
            this.gvReadetail.OptionsView.ShowIndicator = false;
            // 
            // colRsd_no
            // 
            this.colRsd_no.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colRsd_no.AppearanceHeader.Options.UseForeColor = true;
            this.colRsd_no.Caption = "入库单号";
            this.colRsd_no.FieldName = "Rsd_no";
            this.colRsd_no.Name = "colRsd_no";
            this.colRsd_no.OptionsColumn.AllowEdit = false;
            this.colRsd_no.Visible = true;
            this.colRsd_no.VisibleIndex = 0;
            // 
            // colReagentName
            // 
            this.colReagentName.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colReagentName.AppearanceHeader.Options.UseForeColor = true;
            this.colReagentName.Caption = "试剂名称";
            this.colReagentName.ColumnEdit = this.ctlRepositoryItemLookUpEdit6;
            this.colReagentName.FieldName = "Rsd_reaid";
            this.colReagentName.Name = "colReagentName";
            this.colReagentName.OptionsColumn.AllowEdit = false;
            this.colReagentName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colReagentName.Visible = true;
            this.colReagentName.VisibleIndex = 1;
            // 
            // ctlRepositoryItemLookUpEdit6
            // 
            this.ctlRepositoryItemLookUpEdit6.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit6.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit6.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit6.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Drea_id", "编码", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Drea_name", "名称", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rsupplier_name", 120, "供货商"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Drea_package", 60, "规格"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Runit_name", 50, "单位")});
            this.ctlRepositoryItemLookUpEdit6.DataSource = this.bsReaSetting;
            this.ctlRepositoryItemLookUpEdit6.DisplayMember = "Drea_name";
            this.ctlRepositoryItemLookUpEdit6.Name = "ctlRepositoryItemLookUpEdit6";
            this.ctlRepositoryItemLookUpEdit6.NullText = "";
            this.ctlRepositoryItemLookUpEdit6.PopupWidth = 500;
            this.ctlRepositoryItemLookUpEdit6.ValueMember = "Drea_id";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "试剂规格";
            this.gridColumn1.FieldName = "Rsd_package";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // colRsd_reacount
            // 
            this.colRsd_reacount.AppearanceCell.Options.UseTextOptions = true;
            this.colRsd_reacount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colRsd_reacount.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colRsd_reacount.AppearanceHeader.Options.UseForeColor = true;
            this.colRsd_reacount.Caption = "入库数量";
            this.colRsd_reacount.ColumnEdit = this.repositoryItemTextEdit1;
            this.colRsd_reacount.FieldName = "Rsd_reacount";
            this.colRsd_reacount.Name = "colRsd_reacount";
            this.colRsd_reacount.Visible = true;
            this.colRsd_reacount.VisibleIndex = 3;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemTextEdit1.UseReadOnlyAppearance = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.Caption = "供货商";
            this.gridColumn3.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.gridColumn3.FieldName = "Rsd_supid";
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
            // gridColumn2
            // 
            this.gridColumn2.Caption = "试剂编码";
            this.gridColumn2.FieldName = "Rsd_reaid";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn5.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn5.Caption = "组别";
            this.gridColumn5.ColumnEdit = this.ctlRepositoryItemLookUpEdit1;
            this.gridColumn5.FieldName = "Rsd_groupid";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
            // 
            // ctlRepositoryItemLookUpEdit1
            // 
            this.ctlRepositoryItemLookUpEdit1.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit1.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rea_group_id", "编码", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rea_group", "名称", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit1.DataSource = this.bsGroup;
            this.ctlRepositoryItemLookUpEdit1.DisplayMember = "Rea_group";
            this.ctlRepositoryItemLookUpEdit1.Name = "ctlRepositoryItemLookUpEdit1";
            this.ctlRepositoryItemLookUpEdit1.NullText = "";
            this.ctlRepositoryItemLookUpEdit1.ValueMember = "Rea_group_id";
            // 
            // bsGroup
            // 
            this.bsGroup.DataSource = typeof(dcl.entity.EntityDicReaGroup);
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn6.Caption = "批号";
            this.gridColumn6.ColumnEdit = this.repositoryItemTextEdit3;
            this.gridColumn6.FieldName = "Rsd_batchno";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn7.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn7.Caption = "有效期";
            this.gridColumn7.ColumnEdit = this.repositoryItemDateEdit1;
            this.gridColumn7.FieldName = "Rsd_validdate";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.MinValue = new System.DateTime(2005, 1, 1, 0, 0, 0, 0);
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "生产厂家";
            this.gridColumn8.ColumnEdit = this.ctlRepositoryItemLookUpEdit2;
            this.gridColumn8.FieldName = "Rsd_pdtid";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 8;
            // 
            // ctlRepositoryItemLookUpEdit2
            // 
            this.ctlRepositoryItemLookUpEdit2.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit2.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rpdt_id", "编码", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rpdt_name", "名称", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit2.DataSource = this.bsPdt;
            this.ctlRepositoryItemLookUpEdit2.DisplayMember = "Rpdt_name";
            this.ctlRepositoryItemLookUpEdit2.Name = "ctlRepositoryItemLookUpEdit2";
            this.ctlRepositoryItemLookUpEdit2.NullText = "";
            this.ctlRepositoryItemLookUpEdit2.ValueMember = "Rpdt_id";
            // 
            // bsPdt
            // 
            this.bsPdt.DataSource = typeof(dcl.entity.EntityDicReaProduct);
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn10.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn10.Caption = "单位";
            this.gridColumn10.ColumnEdit = this.ctlRepositoryItemLookUpEdit3;
            this.gridColumn10.FieldName = "Rsd_unitid";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 9;
            // 
            // ctlRepositoryItemLookUpEdit3
            // 
            this.ctlRepositoryItemLookUpEdit3.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit3.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit3.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Runit_id", "编码", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Runit_name", "名称", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit3.DataSource = this.bsUnit;
            this.ctlRepositoryItemLookUpEdit3.DisplayMember = "Runit_name";
            this.ctlRepositoryItemLookUpEdit3.Name = "ctlRepositoryItemLookUpEdit3";
            this.ctlRepositoryItemLookUpEdit3.NullText = "";
            this.ctlRepositoryItemLookUpEdit3.ValueMember = "Runit_id";
            // 
            // bsUnit
            // 
            this.bsUnit.DataSource = typeof(dcl.entity.EntityDicReaUnit);
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn11.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn11.Caption = "单价";
            this.gridColumn11.ColumnEdit = this.repositoryItemTextEdit4;
            this.gridColumn11.FieldName = "Rsd_price";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "发票号";
            this.gridColumn12.ColumnEdit = this.repositoryItemTextEdit5;
            this.gridColumn12.FieldName = "Rsd_invoiceno";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 11;
            // 
            // repositoryItemTextEdit5
            // 
            this.repositoryItemTextEdit5.AutoHeight = false;
            this.repositoryItemTextEdit5.Name = "repositoryItemTextEdit5";
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn13.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn13.Caption = "条码号";
            this.gridColumn13.ColumnEdit = this.repositoryItemTextEdit6;
            this.gridColumn13.FieldName = "Rsd_barcode";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 12;
            // 
            // repositoryItemTextEdit6
            // 
            this.repositoryItemTextEdit6.AutoHeight = false;
            this.repositoryItemTextEdit6.Name = "repositoryItemTextEdit6";
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "储存位置";
            this.gridColumn14.ColumnEdit = this.ctlRepositoryItemLookUpEdit4;
            this.gridColumn14.FieldName = "Rsd_posid";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 13;
            // 
            // ctlRepositoryItemLookUpEdit4
            // 
            this.ctlRepositoryItemLookUpEdit4.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit4.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit4.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rstr_position_id", "编码", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rstr_position", "名称", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit4.DataSource = this.bsPos;
            this.ctlRepositoryItemLookUpEdit4.DisplayMember = "Rstr_position";
            this.ctlRepositoryItemLookUpEdit4.Name = "ctlRepositoryItemLookUpEdit4";
            this.ctlRepositoryItemLookUpEdit4.NullText = "";
            this.ctlRepositoryItemLookUpEdit4.ValueMember = "Rstr_position_id";
            // 
            // bsPos
            // 
            this.bsPos.DataSource = typeof(dcl.entity.EntityDicReaStorePos);
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "储存条件";
            this.gridColumn15.ColumnEdit = this.ctlRepositoryItemLookUpEdit5;
            this.gridColumn15.FieldName = "Rsd_conid";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 14;
            // 
            // ctlRepositoryItemLookUpEdit5
            // 
            this.ctlRepositoryItemLookUpEdit5.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit5.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit5.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit5.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rstr_condition_id", "编码", 104, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Rstr_condition", "名称", 120, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit5.DataSource = this.bsCon;
            this.ctlRepositoryItemLookUpEdit5.DisplayMember = "Rstr_condition";
            this.ctlRepositoryItemLookUpEdit5.Name = "ctlRepositoryItemLookUpEdit5";
            this.ctlRepositoryItemLookUpEdit5.NullText = "";
            this.ctlRepositoryItemLookUpEdit5.ValueMember = "Rstr_condition_id";
            // 
            // bsCon
            // 
            this.bsCon.DataSource = typeof(dcl.entity.EntityDicReaStoreCondition);
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn16.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn16.Caption = "试剂外包装";
            this.gridColumn16.ColumnEdit = this.ctlRepositoryItemLookUpEdit7;
            this.gridColumn16.FieldName = "Rsd_outerpacking";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 15;
            // 
            // ctlRepositoryItemLookUpEdit7
            // 
            this.ctlRepositoryItemLookUpEdit7.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit7.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit7.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit7.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit7.Name = "ctlRepositoryItemLookUpEdit7";
            this.ctlRepositoryItemLookUpEdit7.NullText = "";
            this.ctlRepositoryItemLookUpEdit7.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit7.ValueMember = "EvaContent";
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn17.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn17.Caption = "到达温度";
            this.gridColumn17.ColumnEdit = this.ctlRepositoryItemLookUpEdit8;
            this.gridColumn17.FieldName = "Rsd_temperature";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 16;
            // 
            // ctlRepositoryItemLookUpEdit8
            // 
            this.ctlRepositoryItemLookUpEdit8.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit8.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit8.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit8.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit8.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit8.Name = "ctlRepositoryItemLookUpEdit8";
            this.ctlRepositoryItemLookUpEdit8.NullText = "";
            this.ctlRepositoryItemLookUpEdit8.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit8.ValueMember = "EvaContent";
            // 
            // gridColumn18
            // 
            this.gridColumn18.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.gridColumn18.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn18.Caption = "检验报告";
            this.gridColumn18.ColumnEdit = this.ctlRepositoryItemLookUpEdit9;
            this.gridColumn18.FieldName = "Rsd_report";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 17;
            // 
            // ctlRepositoryItemLookUpEdit9
            // 
            this.ctlRepositoryItemLookUpEdit9.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit9.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit9.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit9.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit9.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit9.Name = "ctlRepositoryItemLookUpEdit9";
            this.ctlRepositoryItemLookUpEdit9.NullText = "";
            this.ctlRepositoryItemLookUpEdit9.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit9.ValueMember = "EvaContent";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "gridColumn4";
            this.gridColumn4.FieldName = "Rsd_id";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "采购单号";
            this.gridColumn9.FieldName = "Rsd_purno";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 18;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemTextEdit7
            // 
            this.repositoryItemTextEdit7.AutoHeight = false;
            this.repositoryItemTextEdit7.Name = "repositoryItemTextEdit7";
            // 
            // repositoryItemTextEdit8
            // 
            this.repositoryItemTextEdit8.AutoHeight = false;
            this.repositoryItemTextEdit8.Name = "repositoryItemTextEdit8";
            // 
            // repositoryItemTextEdit9
            // 
            this.repositoryItemTextEdit9.AutoHeight = false;
            this.repositoryItemTextEdit9.Name = "repositoryItemTextEdit9";
            // 
            // repositoryItemTextEdit10
            // 
            this.repositoryItemTextEdit10.AutoHeight = false;
            this.repositoryItemTextEdit10.Name = "repositoryItemTextEdit10";
            // 
            // panelControl5
            // 
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl5.Location = new System.Drawing.Point(252, 0);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(739, 44);
            this.panelControl5.TabIndex = 0;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.layoutControl1);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl6.Location = new System.Drawing.Point(0, 0);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(252, 600);
            this.panelControl6.TabIndex = 1;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.lookUpEdit3);
            this.layoutControl1.Controls.Add(this.lookUpEdit2);
            this.layoutControl1.Controls.Add(this.lookUpEdit1);
            this.layoutControl1.Controls.Add(this.radioGroup2);
            this.layoutControl1.Controls.Add(this.txtPurno);
            this.layoutControl1.Controls.Add(this.txtReason);
            this.layoutControl1.Controls.Add(this.radioGroup1);
            this.layoutControl1.Controls.Add(this.dateEdit2);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.txtBarcode);
            this.layoutControl1.Controls.Add(this.selectDicReaStoreCondition1);
            this.layoutControl1.Controls.Add(this.selectDicReaStorePosition1);
            this.layoutControl1.Controls.Add(this.txtInvoice);
            this.layoutControl1.Controls.Add(this.txtPrice);
            this.layoutControl1.Controls.Add(this.selectDicReaUnit1);
            this.layoutControl1.Controls.Add(this.selectDicReaSupplier2);
            this.layoutControl1.Controls.Add(this.selectDicReaProduct1);
            this.layoutControl1.Controls.Add(this.txtBatchNo);
            this.layoutControl1.Controls.Add(this.txtCount);
            this.layoutControl1.Controls.Add(this.txtPackage);
            this.layoutControl1.Controls.Add(this.selectDicReaGroup2);
            this.layoutControl1.Controls.Add(this.selectDicReaSetting1);
            this.layoutControl1.Controls.Add(this.txtReaDate);
            this.layoutControl1.Controls.Add(this.txtReaSid);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(821, 230, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(248, 596);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // lookUpEdit3
            // 
            this.lookUpEdit3.Location = new System.Drawing.Point(63, 352);
            this.lookUpEdit3.Name = "lookUpEdit3";
            this.lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit3.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.lookUpEdit3.Properties.DisplayMember = "EvaContent";
            this.lookUpEdit3.Properties.NullText = "";
            this.lookUpEdit3.Properties.ValueMember = "EvaContent";
            this.lookUpEdit3.Size = new System.Drawing.Size(156, 20);
            this.lookUpEdit3.StyleController = this.layoutControl1;
            this.lookUpEdit3.TabIndex = 32;
            // 
            // lookUpEdit2
            // 
            this.lookUpEdit2.Location = new System.Drawing.Point(63, 328);
            this.lookUpEdit2.Name = "lookUpEdit2";
            this.lookUpEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit2.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.lookUpEdit2.Properties.DisplayMember = "EvaContent";
            this.lookUpEdit2.Properties.NullText = "";
            this.lookUpEdit2.Properties.ValueMember = "EvaContent";
            this.lookUpEdit2.Size = new System.Drawing.Size(156, 20);
            this.lookUpEdit2.StyleController = this.layoutControl1;
            this.lookUpEdit2.TabIndex = 31;
            // 
            // lookUpEdit1
            // 
            this.lookUpEdit1.Location = new System.Drawing.Point(63, 304);
            this.lookUpEdit1.Name = "lookUpEdit1";
            this.lookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit1.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.lookUpEdit1.Properties.DisplayMember = "EvaContent";
            this.lookUpEdit1.Properties.NullText = "";
            this.lookUpEdit1.Properties.ValueMember = "EvaContent";
            this.lookUpEdit1.Size = new System.Drawing.Size(156, 20);
            this.lookUpEdit1.StyleController = this.layoutControl1;
            this.lookUpEdit1.TabIndex = 30;
            // 
            // radioGroup2
            // 
            this.radioGroup2.EditValue = "完全入库";
            this.radioGroup2.Location = new System.Drawing.Point(63, 552);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup2.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup2.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("完全入库", "完全入库"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("部分入库", "部分入库")});
            this.radioGroup2.Size = new System.Drawing.Size(156, 25);
            this.radioGroup2.StyleController = this.layoutControl1;
            this.radioGroup2.TabIndex = 29;
            // 
            // txtPurno
            // 
            this.txtPurno.Location = new System.Drawing.Point(63, 60);
            this.txtPurno.Name = "txtPurno";
            this.txtPurno.Size = new System.Drawing.Size(156, 20);
            this.txtPurno.StyleController = this.layoutControl1;
            this.txtPurno.TabIndex = 28;
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(63, 528);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(156, 20);
            this.txtReason.StyleController = this.layoutControl1;
            this.txtReason.TabIndex = 27;
            this.txtReason.Visible = false;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(63, 475);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "仓储码"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "条形码")});
            this.radioGroup1.Size = new System.Drawing.Size(156, 25);
            this.radioGroup1.StyleController = this.layoutControl1;
            this.radioGroup1.TabIndex = 26;
            // 
            // dateEdit2
            // 
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(63, 206);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Size = new System.Drawing.Size(156, 20);
            this.dateEdit2.StyleController = this.layoutControl1;
            this.dateEdit2.TabIndex = 25;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(12, 581);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(207, 22);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 21;
            this.simpleButton1.Text = ">";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(63, 504);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.ReadOnly = true;
            this.txtBarcode.Size = new System.Drawing.Size(156, 20);
            this.txtBarcode.StyleController = this.layoutControl1;
            this.txtBarcode.TabIndex = 20;
            // 
            // selectDicReaStoreCondition1
            // 
            this.selectDicReaStoreCondition1.AddEmptyRow = true;
            this.selectDicReaStoreCondition1.BindByValue = true;
            this.selectDicReaStoreCondition1.colDisplay = "";
            this.selectDicReaStoreCondition1.colExtend1 = "";
            this.selectDicReaStoreCondition1.colInCode = "";
            this.selectDicReaStoreCondition1.colPY = "";
            this.selectDicReaStoreCondition1.colSeq = "";
            this.selectDicReaStoreCondition1.colValue = "";
            this.selectDicReaStoreCondition1.colWB = "";
            this.selectDicReaStoreCondition1.displayMember = "";
            this.selectDicReaStoreCondition1.EnterMoveNext = true;
            this.selectDicReaStoreCondition1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaStoreCondition1.KeyUpDownMoveNext = false;
            this.selectDicReaStoreCondition1.LoadDataOnDesignMode = true;
            this.selectDicReaStoreCondition1.Location = new System.Drawing.Point(63, 450);
            this.selectDicReaStoreCondition1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaStoreCondition1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaStoreCondition1.Name = "selectDicReaStoreCondition1";
            this.selectDicReaStoreCondition1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaStoreCondition1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaStoreCondition1.Readonly = false;
            this.selectDicReaStoreCondition1.SaveSourceID = false;
            this.selectDicReaStoreCondition1.SelectFilter = null;
            this.selectDicReaStoreCondition1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaStoreCondition1.SelectOnly = true;
            this.selectDicReaStoreCondition1.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaStoreCondition1.TabIndex = 19;
            this.selectDicReaStoreCondition1.UseExtend = false;
            this.selectDicReaStoreCondition1.valueMember = "";
            // 
            // selectDicReaStorePosition1
            // 
            this.selectDicReaStorePosition1.AddEmptyRow = true;
            this.selectDicReaStorePosition1.BindByValue = true;
            this.selectDicReaStorePosition1.colDisplay = "";
            this.selectDicReaStorePosition1.colExtend1 = "";
            this.selectDicReaStorePosition1.colInCode = "";
            this.selectDicReaStorePosition1.colPY = "";
            this.selectDicReaStorePosition1.colSeq = "";
            this.selectDicReaStorePosition1.colValue = "";
            this.selectDicReaStorePosition1.colWB = "";
            this.selectDicReaStorePosition1.displayMember = "";
            this.selectDicReaStorePosition1.EnterMoveNext = true;
            this.selectDicReaStorePosition1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaStorePosition1.KeyUpDownMoveNext = false;
            this.selectDicReaStorePosition1.LoadDataOnDesignMode = true;
            this.selectDicReaStorePosition1.Location = new System.Drawing.Point(63, 425);
            this.selectDicReaStorePosition1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaStorePosition1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaStorePosition1.Name = "selectDicReaStorePosition1";
            this.selectDicReaStorePosition1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaStorePosition1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaStorePosition1.Readonly = false;
            this.selectDicReaStorePosition1.SaveSourceID = false;
            this.selectDicReaStorePosition1.SelectFilter = null;
            this.selectDicReaStorePosition1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaStorePosition1.SelectOnly = true;
            this.selectDicReaStorePosition1.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaStorePosition1.TabIndex = 18;
            this.selectDicReaStorePosition1.UseExtend = false;
            this.selectDicReaStorePosition1.valueMember = "";
            // 
            // txtInvoice
            // 
            this.txtInvoice.Location = new System.Drawing.Point(63, 401);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(156, 20);
            this.txtInvoice.StyleController = this.layoutControl1;
            this.txtInvoice.TabIndex = 17;
            // 
            // txtPrice
            // 
            this.txtPrice.EditValue = "0";
            this.txtPrice.Location = new System.Drawing.Point(63, 280);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(156, 20);
            this.txtPrice.StyleController = this.layoutControl1;
            this.txtPrice.TabIndex = 16;
            // 
            // selectDicReaUnit1
            // 
            this.selectDicReaUnit1.AddEmptyRow = true;
            this.selectDicReaUnit1.BindByValue = true;
            this.selectDicReaUnit1.colDisplay = "";
            this.selectDicReaUnit1.colExtend1 = "";
            this.selectDicReaUnit1.colInCode = "";
            this.selectDicReaUnit1.colPY = "";
            this.selectDicReaUnit1.colSeq = "";
            this.selectDicReaUnit1.colValue = "";
            this.selectDicReaUnit1.colWB = "";
            this.selectDicReaUnit1.displayMember = "";
            this.selectDicReaUnit1.EnterMoveNext = true;
            this.selectDicReaUnit1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaUnit1.KeyUpDownMoveNext = false;
            this.selectDicReaUnit1.LoadDataOnDesignMode = true;
            this.selectDicReaUnit1.Location = new System.Drawing.Point(63, 255);
            this.selectDicReaUnit1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaUnit1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaUnit1.Name = "selectDicReaUnit1";
            this.selectDicReaUnit1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaUnit1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaUnit1.Readonly = true;
            this.selectDicReaUnit1.SaveSourceID = false;
            this.selectDicReaUnit1.SelectFilter = null;
            this.selectDicReaUnit1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaUnit1.SelectOnly = true;
            this.selectDicReaUnit1.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaUnit1.TabIndex = 15;
            this.selectDicReaUnit1.UseExtend = false;
            this.selectDicReaUnit1.valueMember = "";
            // 
            // selectDicReaSupplier2
            // 
            this.selectDicReaSupplier2.AddEmptyRow = true;
            this.selectDicReaSupplier2.BindByValue = true;
            this.selectDicReaSupplier2.colDisplay = "";
            this.selectDicReaSupplier2.colExtend1 = "";
            this.selectDicReaSupplier2.colInCode = "";
            this.selectDicReaSupplier2.colPY = "";
            this.selectDicReaSupplier2.colSeq = "";
            this.selectDicReaSupplier2.colValue = "";
            this.selectDicReaSupplier2.colWB = "";
            this.selectDicReaSupplier2.displayMember = "";
            this.selectDicReaSupplier2.EnterMoveNext = true;
            this.selectDicReaSupplier2.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaSupplier2.KeyUpDownMoveNext = false;
            this.selectDicReaSupplier2.LoadDataOnDesignMode = true;
            this.selectDicReaSupplier2.Location = new System.Drawing.Point(63, 230);
            this.selectDicReaSupplier2.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaSupplier2.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaSupplier2.Name = "selectDicReaSupplier2";
            this.selectDicReaSupplier2.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaSupplier2.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaSupplier2.Readonly = true;
            this.selectDicReaSupplier2.SaveSourceID = false;
            this.selectDicReaSupplier2.SelectFilter = null;
            this.selectDicReaSupplier2.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaSupplier2.SelectOnly = true;
            this.selectDicReaSupplier2.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaSupplier2.TabIndex = 14;
            this.selectDicReaSupplier2.UseExtend = false;
            this.selectDicReaSupplier2.valueMember = "";
            // 
            // selectDicReaProduct1
            // 
            this.selectDicReaProduct1.AddEmptyRow = true;
            this.selectDicReaProduct1.BindByValue = true;
            this.selectDicReaProduct1.colDisplay = "";
            this.selectDicReaProduct1.colExtend1 = "";
            this.selectDicReaProduct1.colInCode = "";
            this.selectDicReaProduct1.colPY = "";
            this.selectDicReaProduct1.colSeq = "";
            this.selectDicReaProduct1.colValue = "";
            this.selectDicReaProduct1.colWB = "";
            this.selectDicReaProduct1.displayMember = "";
            this.selectDicReaProduct1.EnterMoveNext = true;
            this.selectDicReaProduct1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaProduct1.KeyUpDownMoveNext = false;
            this.selectDicReaProduct1.LoadDataOnDesignMode = true;
            this.selectDicReaProduct1.Location = new System.Drawing.Point(63, 376);
            this.selectDicReaProduct1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaProduct1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaProduct1.Name = "selectDicReaProduct1";
            this.selectDicReaProduct1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaProduct1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaProduct1.Readonly = true;
            this.selectDicReaProduct1.SaveSourceID = false;
            this.selectDicReaProduct1.SelectFilter = null;
            this.selectDicReaProduct1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaProduct1.SelectOnly = true;
            this.selectDicReaProduct1.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaProduct1.TabIndex = 13;
            this.selectDicReaProduct1.UseExtend = false;
            this.selectDicReaProduct1.valueMember = "";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(63, 182);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(156, 20);
            this.txtBatchNo.StyleController = this.layoutControl1;
            this.txtBatchNo.TabIndex = 11;
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(63, 158);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(156, 20);
            this.txtCount.StyleController = this.layoutControl1;
            this.txtCount.TabIndex = 10;
            // 
            // txtPackage
            // 
            this.txtPackage.Location = new System.Drawing.Point(63, 134);
            this.txtPackage.Name = "txtPackage";
            this.txtPackage.Size = new System.Drawing.Size(156, 20);
            this.txtPackage.StyleController = this.layoutControl1;
            this.txtPackage.TabIndex = 9;
            // 
            // selectDicReaGroup2
            // 
            this.selectDicReaGroup2.AddEmptyRow = true;
            this.selectDicReaGroup2.BindByValue = true;
            this.selectDicReaGroup2.colDisplay = "";
            this.selectDicReaGroup2.colExtend1 = "";
            this.selectDicReaGroup2.colInCode = "";
            this.selectDicReaGroup2.colPY = "";
            this.selectDicReaGroup2.colSeq = "";
            this.selectDicReaGroup2.colValue = "";
            this.selectDicReaGroup2.colWB = "";
            this.selectDicReaGroup2.displayMember = "";
            this.selectDicReaGroup2.EnterMoveNext = true;
            this.selectDicReaGroup2.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaGroup2.KeyUpDownMoveNext = false;
            this.selectDicReaGroup2.LoadDataOnDesignMode = true;
            this.selectDicReaGroup2.Location = new System.Drawing.Point(63, 109);
            this.selectDicReaGroup2.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaGroup2.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaGroup2.Name = "selectDicReaGroup2";
            this.selectDicReaGroup2.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaGroup2.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaGroup2.Readonly = true;
            this.selectDicReaGroup2.SaveSourceID = false;
            this.selectDicReaGroup2.SelectFilter = null;
            this.selectDicReaGroup2.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaGroup2.SelectOnly = true;
            this.selectDicReaGroup2.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaGroup2.TabIndex = 8;
            this.selectDicReaGroup2.UseExtend = false;
            this.selectDicReaGroup2.valueMember = "";
            // 
            // selectDicReaSetting1
            // 
            this.selectDicReaSetting1.AddEmptyRow = true;
            this.selectDicReaSetting1.BindByValue = false;
            this.selectDicReaSetting1.colDisplay = "";
            this.selectDicReaSetting1.colExtend1 = "";
            this.selectDicReaSetting1.colInCode = "";
            this.selectDicReaSetting1.colPY = "";
            this.selectDicReaSetting1.colSeq = "";
            this.selectDicReaSetting1.colValue = "";
            this.selectDicReaSetting1.colWB = "";
            this.selectDicReaSetting1.displayMember = null;
            this.selectDicReaSetting1.EnterMoveNext = true;
            this.selectDicReaSetting1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaSetting1.KeyUpDownMoveNext = false;
            this.selectDicReaSetting1.LoadDataOnDesignMode = true;
            this.selectDicReaSetting1.Location = new System.Drawing.Point(63, 84);
            this.selectDicReaSetting1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaSetting1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaSetting1.Name = "selectDicReaSetting1";
            this.selectDicReaSetting1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaSetting1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaSetting1.Readonly = false;
            this.selectDicReaSetting1.SaveSourceID = false;
            this.selectDicReaSetting1.SelectFilter = null;
            this.selectDicReaSetting1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaSetting1.SelectOnly = true;
            this.selectDicReaSetting1.Size = new System.Drawing.Size(156, 21);
            this.selectDicReaSetting1.TabIndex = 7;
            this.selectDicReaSetting1.UseExtend = false;
            this.selectDicReaSetting1.valueMember = null;
            this.selectDicReaSetting1.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityReaSetting>.ValueChangedEventHandler(this.selectDicReaSetting1_ValueChanged);
            // 
            // txtReaDate
            // 
            this.txtReaDate.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaStorage, "Rsr_date", true));
            this.txtReaDate.EditValue = null;
            this.txtReaDate.Location = new System.Drawing.Point(63, 36);
            this.txtReaDate.Name = "txtReaDate";
            this.txtReaDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReaDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReaDate.Size = new System.Drawing.Size(156, 20);
            this.txtReaDate.StyleController = this.layoutControl1;
            this.txtReaDate.TabIndex = 5;
            // 
            // txtReaSid
            // 
            this.txtReaSid.Location = new System.Drawing.Point(63, 12);
            this.txtReaSid.Name = "txtReaSid";
            this.txtReaSid.Properties.ReadOnly = true;
            this.txtReaSid.Size = new System.Drawing.Size(156, 20);
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
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem15,
            this.layoutControlItem16,
            this.layoutControlItem17,
            this.layoutControlItem8,
            this.layoutControlItem23,
            this.layoutControlItem22,
            this.layoutControlItem24,
            this.layoutControlItem25,
            this.layoutControlItem26,
            this.layoutControlItem27,
            this.layoutControlItem9,
            this.layoutControlItem21});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(231, 615);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.BackColor = System.Drawing.Color.Transparent;
            this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtReaSid;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem1.Text = "入库单号";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem2.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.Control = this.txtReaDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem2.Text = "入库时间";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem4.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem4.Control = this.selectDicReaSetting1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem4.Text = "试剂信息";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem3.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.selectDicReaGroup2;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 97);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem3.Text = "试剂组别";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem5.Control = this.txtPackage;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 122);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem5.Text = "规格";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem6.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem6.Control = this.txtCount;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 146);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem6.Text = "数量";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem7.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem7.Control = this.txtBatchNo;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 170);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem7.Text = "批号";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem10.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem10.Control = this.selectDicReaSupplier2;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 218);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem10.Text = "供应商";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem11.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.selectDicReaUnit1;
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 243);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem11.Text = "单位";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem12.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem12.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem12.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem12.Control = this.txtPrice;
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 268);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem12.Text = "单价";
            this.layoutControlItem12.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem13.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem13.Control = this.txtInvoice;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 389);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem13.Text = "发票号";
            this.layoutControlItem13.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem14.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem14.Control = this.selectDicReaStorePosition1;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 413);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem14.Text = "储存位置";
            this.layoutControlItem14.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem15.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem15.Control = this.selectDicReaStoreCondition1;
            this.layoutControlItem15.Location = new System.Drawing.Point(0, 438);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem15.Text = "储存条件";
            this.layoutControlItem15.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem16.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem16.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem16.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem16.Control = this.txtBarcode;
            this.layoutControlItem16.Location = new System.Drawing.Point(0, 492);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem16.Text = "条形码";
            this.layoutControlItem16.TextSize = new System.Drawing.Size(48, 14);
            this.layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.simpleButton1;
            this.layoutControlItem17.Location = new System.Drawing.Point(0, 569);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(211, 26);
            this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem17.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.radioGroup1;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 463);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(211, 29);
            this.layoutControlItem8.Text = "规则";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(48, 14);
            this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem23
            // 
            this.layoutControlItem23.Control = this.txtPurno;
            this.layoutControlItem23.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem23.Name = "layoutControlItem23";
            this.layoutControlItem23.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem23.Text = "采购单号";
            this.layoutControlItem23.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this.txtReason;
            this.layoutControlItem22.Location = new System.Drawing.Point(0, 516);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem22.Text = "撤销原因";
            this.layoutControlItem22.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem24
            // 
            this.layoutControlItem24.Control = this.radioGroup2;
            this.layoutControlItem24.Location = new System.Drawing.Point(0, 540);
            this.layoutControlItem24.MaxSize = new System.Drawing.Size(0, 29);
            this.layoutControlItem24.MinSize = new System.Drawing.Size(105, 29);
            this.layoutControlItem24.Name = "layoutControlItem24";
            this.layoutControlItem24.Size = new System.Drawing.Size(211, 29);
            this.layoutControlItem24.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem24.Text = "入库规则";
            this.layoutControlItem24.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem25
            // 
            this.layoutControlItem25.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem25.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem25.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem25.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem25.Control = this.lookUpEdit1;
            this.layoutControlItem25.Location = new System.Drawing.Point(0, 292);
            this.layoutControlItem25.Name = "layoutControlItem25";
            this.layoutControlItem25.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem25.Text = "外包装";
            this.layoutControlItem25.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem26
            // 
            this.layoutControlItem26.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem26.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem26.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem26.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem26.Control = this.lookUpEdit2;
            this.layoutControlItem26.Location = new System.Drawing.Point(0, 316);
            this.layoutControlItem26.Name = "layoutControlItem26";
            this.layoutControlItem26.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem26.Text = "到达温度";
            this.layoutControlItem26.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem27
            // 
            this.layoutControlItem27.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem27.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem27.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem27.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem27.Control = this.lookUpEdit3;
            this.layoutControlItem27.Location = new System.Drawing.Point(0, 340);
            this.layoutControlItem27.Name = "layoutControlItem27";
            this.layoutControlItem27.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem27.Text = "检验报告";
            this.layoutControlItem27.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem9.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem9.Control = this.selectDicReaProduct1;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 364);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(211, 25);
            this.layoutControlItem9.Text = "生产厂家";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem21.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem21.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem21.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem21.Control = this.dateEdit2;
            this.layoutControlItem21.Location = new System.Drawing.Point(0, 194);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Size = new System.Drawing.Size(211, 24);
            this.layoutControlItem21.Text = "有效期";
            this.layoutControlItem21.TextSize = new System.Drawing.Size(48, 14);
            // 
            // gvReaPuechase
            // 
            this.gvReaPuechase.Name = "gvReaPuechase";
            // 
            // FrmReagentStorage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 673);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReagentStorage";
            this.Text = "试剂入库";
            this.Load += new System.EventHandler(this.FrmReagenStorage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsReaSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReaStorage)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsReaStorage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaStorage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcReaPurchase)).EndInit();
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
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsReaStorageDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReadetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPdt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPurno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPackage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReaSid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReaPuechase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
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
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Label lbRecordCount;
        private DevExpress.XtraEditors.ComboBoxEdit cbeFlag;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraGrid.GridControl gcReaStorage;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReaStorage;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtReaSid;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.BindingSource bsReaStorageDetail;
        private DevExpress.XtraEditors.DateEdit txtReaDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.BindingSource bsReaStorage;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_no;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_applier;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_auditor;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_applydate;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_auditdate;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_status;
        private DevExpress.XtraGrid.Columns.GridColumn colRsr_printflag;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 面板设置ToolStripMenuItem;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private SelectDicReaSupplier selectDicReaSupplier1;
        private System.Windows.Forms.BindingSource bsSup;
        private RoundStorageGroup roundPanelGroup1;
        private DevExpress.XtraEditors.TextEdit txtPackage;
        private SelectDicReaGroup selectDicReaGroup2;
        private SelectDicReaSetting selectDicReaSetting1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit txtBatchNo;
        private DevExpress.XtraEditors.TextEdit txtCount;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private SelectDicReaSupplier selectDicReaSupplier2;
        private SelectDicReaProduct selectDicReaProduct1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private SelectDicReaStoreCondition selectDicReaStoreCondition1;
        private SelectDicReaStorePosition selectDicReaStorePosition1;
        private DevExpress.XtraEditors.TextEdit txtInvoice;
        private DevExpress.XtraEditors.TextEdit txtPrice;
        private SelectDicReaUnit selectDicReaUnit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraEditors.TextEdit txtBarcode;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem16;
        private System.Windows.Forms.BindingSource bsGroup;
        private System.Windows.Forms.BindingSource bsPdt;
        private System.Windows.Forms.BindingSource bsUnit;
        private System.Windows.Forms.BindingSource bsPos;
        private System.Windows.Forms.BindingSource bsCon;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem17;
        private DevExpress.XtraGrid.GridControl gcReaPurchase;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReaPurchase;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_no;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_applier;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_auditor;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_applydate;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_auditdate;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_status;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_printflag;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_rejectreason;
        private DevExpress.XtraGrid.Columns.GridColumn colRpc_remark;
        private DevExpress.XtraEditors.DateEdit dateEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem21;
        private SysToolBar sysToolBar1;
        private System.Windows.Forms.BindingSource bsReaSetting;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem MenuDelItem;
        private System.Windows.Forms.BindingSource bsReaPurchase;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReaPuechase;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.TextEdit txtReason;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem22;
        private DevExpress.XtraEditors.TextEdit txtPurno;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem23;
        private DevExpress.XtraGrid.GridControl gcReaDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReadetail;
        private DevExpress.XtraGrid.Columns.GridColumn colRsd_no;
        private DevExpress.XtraGrid.Columns.GridColumn colReagentName;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colRsd_reacount;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private ctlRepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit10;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem24;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit7;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit8;
        private ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit9;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem25;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit3;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem26;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem27;
    }
}