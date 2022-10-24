using dcl.client.common;
using dcl.client.control;
using System;

namespace wf.client.reagent
{
    partial class FrmReagentSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReagentSetting));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.bsReaSetting = new System.Windows.Forms.BindingSource();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.selectDicReaSupplier1 = new dcl.client.control.SelectDicReaSupplier();
            this.txtId = new DevExpress.XtraEditors.TextEdit();
            this.ceDelFlag = new DevExpress.XtraEditors.CheckEdit();
            this.cePrintFlag = new DevExpress.XtraEditors.CheckEdit();
            this.txtRemark = new DevExpress.XtraEditors.TextEdit();
            this.txtwbcode = new DevExpress.XtraEditors.TextEdit();
            this.txtpycode = new DevExpress.XtraEditors.TextEdit();
            this.txtMethod = new DevExpress.XtraEditors.TextEdit();
            this.txtUpper = new DevExpress.XtraEditors.TextEdit();
            this.txtLower = new DevExpress.XtraEditors.TextEdit();
            this.txtTender = new DevExpress.XtraEditors.TextEdit();
            this.txtCertificate = new DevExpress.XtraEditors.TextEdit();
            this.selectDicReaStoreCondition1 = new dcl.client.control.SelectDicReaStoreCondition();
            this.selectDicReaStorePosition1 = new dcl.client.control.SelectDicReaStorePosition();
            this.selectDicReaGroup1 = new dcl.client.control.SelectDicReaGroup();
            this.selectDicReaProduct1 = new dcl.client.control.SelectDicReaProduct();
            this.selectDicReaUnit1 = new dcl.client.control.SelectDicReaUnit();
            this.txtPackage = new DevExpress.XtraEditors.TextEdit();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem18 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem21 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem20 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem22 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem23 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcReagentSetting = new DevExpress.XtraGrid.GridControl();
            this.gvReagentSetting = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDrea_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBarcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_package = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_unit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_product = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_group = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_position = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_condition = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_lower_limit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_upper_limit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_certificate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_tender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_method = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colwb_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_provincialno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.txtSort = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem19 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDelFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePrintFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtwbcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtpycode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMethod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLower.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTender.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCertificate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPackage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReagentSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReagentSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.splitContainerControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 66);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1192, 536);
            this.panelControl3.TabIndex = 8;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerControl1.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.splitContainerControl1.Appearance.Options.UseBackColor = true;
            this.splitContainerControl1.Appearance.Options.UseBorderColor = true;
            this.splitContainerControl1.AppearanceCaption.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerControl1.AppearanceCaption.BorderColor = System.Drawing.Color.Transparent;
            this.splitContainerControl1.AppearanceCaption.Options.UseBackColor = true;
            this.splitContainerControl1.AppearanceCaption.Options.UseBorderColor = true;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1188, 532);
            this.splitContainerControl1.SplitterPosition = 318;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(318, 532);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "试剂库设置";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEdit2);
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.selectDicReaSupplier1);
            this.layoutControl1.Controls.Add(this.txtId);
            this.layoutControl1.Controls.Add(this.ceDelFlag);
            this.layoutControl1.Controls.Add(this.cePrintFlag);
            this.layoutControl1.Controls.Add(this.txtRemark);
            this.layoutControl1.Controls.Add(this.txtwbcode);
            this.layoutControl1.Controls.Add(this.txtpycode);
            this.layoutControl1.Controls.Add(this.txtMethod);
            this.layoutControl1.Controls.Add(this.txtUpper);
            this.layoutControl1.Controls.Add(this.txtLower);
            this.layoutControl1.Controls.Add(this.txtTender);
            this.layoutControl1.Controls.Add(this.txtCertificate);
            this.layoutControl1.Controls.Add(this.selectDicReaStoreCondition1);
            this.layoutControl1.Controls.Add(this.selectDicReaStorePosition1);
            this.layoutControl1.Controls.Add(this.selectDicReaGroup1);
            this.layoutControl1.Controls.Add(this.selectDicReaProduct1);
            this.layoutControl1.Controls.Add(this.selectDicReaUnit1);
            this.layoutControl1.Controls.Add(this.txtPackage);
            this.layoutControl1.Controls.Add(this.txtBarcode);
            this.layoutControl1.Controls.Add(this.txtName);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 21);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(314, 509);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEdit2
            // 
            this.textEdit2.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_provincialno", true));
            this.textEdit2.Location = new System.Drawing.Point(70, 36);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(215, 20);
            this.textEdit2.StyleController = this.layoutControl1;
            this.textEdit2.TabIndex = 25;
            // 
            // bsReaSetting
            // 
            this.bsReaSetting.DataSource = typeof(dcl.entity.EntityReaSetting);
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_alarmdays", true));
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_alarmdays", true));
            this.textEdit1.Location = new System.Drawing.Point(70, 330);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(215, 20);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 24;
            // 
            // selectDicReaSupplier1
            // 
            this.selectDicReaSupplier1.AddEmptyRow = true;
            this.selectDicReaSupplier1.BindByValue = true;
            this.selectDicReaSupplier1.colDisplay = "";
            this.selectDicReaSupplier1.colExtend1 = "";
            this.selectDicReaSupplier1.colInCode = "";
            this.selectDicReaSupplier1.colPY = "";
            this.selectDicReaSupplier1.colSeq = "";
            this.selectDicReaSupplier1.colValue = "";
            this.selectDicReaSupplier1.colWB = "";
            this.selectDicReaSupplier1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_supplier", true));
            this.selectDicReaSupplier1.displayMember = "";
            this.selectDicReaSupplier1.EnterMoveNext = true;
            this.selectDicReaSupplier1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaSupplier1.KeyUpDownMoveNext = false;
            this.selectDicReaSupplier1.LoadDataOnDesignMode = true;
            this.selectDicReaSupplier1.Location = new System.Drawing.Point(70, 182);
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
            this.selectDicReaSupplier1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaSupplier1.TabIndex = 23;
            this.selectDicReaSupplier1.UseExtend = false;
            this.selectDicReaSupplier1.valueMember = "";
            // 
            // txtId
            // 
            this.txtId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_id", true));
            this.txtId.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_id", true));
            this.txtId.Enabled = false;
            this.txtId.Location = new System.Drawing.Point(70, 12);
            this.txtId.Name = "txtId";
            this.txtId.Properties.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(215, 20);
            this.txtId.StyleController = this.layoutControl1;
            this.txtId.TabIndex = 22;
            // 
            // ceDelFlag
            // 
            this.ceDelFlag.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "del_flag", true));
            this.ceDelFlag.Location = new System.Drawing.Point(150, 498);
            this.ceDelFlag.Name = "ceDelFlag";
            this.ceDelFlag.Properties.Caption = "停用";
            this.ceDelFlag.Properties.ValueChecked = 1;
            this.ceDelFlag.Properties.ValueGrayed = "-1";
            this.ceDelFlag.Properties.ValueUnchecked = 0;
            this.ceDelFlag.Size = new System.Drawing.Size(135, 19);
            this.ceDelFlag.StyleController = this.layoutControl1;
            this.ceDelFlag.TabIndex = 21;
            // 
            // cePrintFlag
            // 
            this.cePrintFlag.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_printflag", true));
            this.cePrintFlag.Location = new System.Drawing.Point(12, 498);
            this.cePrintFlag.Name = "cePrintFlag";
            this.cePrintFlag.Properties.Caption = "打印条形码";
            this.cePrintFlag.Properties.ValueChecked = 1;
            this.cePrintFlag.Properties.ValueGrayed = "-1";
            this.cePrintFlag.Properties.ValueUnchecked = 0;
            this.cePrintFlag.Size = new System.Drawing.Size(134, 19);
            this.cePrintFlag.StyleController = this.layoutControl1;
            this.cePrintFlag.TabIndex = 20;
            this.cePrintFlag.Visible = false;
            // 
            // txtRemark
            // 
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_remark", true));
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_remark", true));
            this.txtRemark.Location = new System.Drawing.Point(70, 474);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(215, 20);
            this.txtRemark.StyleController = this.layoutControl1;
            this.txtRemark.TabIndex = 19;
            // 
            // txtwbcode
            // 
            this.txtwbcode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "wb_code", true));
            this.txtwbcode.Location = new System.Drawing.Point(70, 450);
            this.txtwbcode.Name = "txtwbcode";
            this.txtwbcode.Size = new System.Drawing.Size(215, 20);
            this.txtwbcode.StyleController = this.layoutControl1;
            this.txtwbcode.TabIndex = 18;
            // 
            // txtpycode
            // 
            this.txtpycode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "py_code", true));
            this.txtpycode.Location = new System.Drawing.Point(70, 426);
            this.txtpycode.Name = "txtpycode";
            this.txtpycode.Size = new System.Drawing.Size(215, 20);
            this.txtpycode.StyleController = this.layoutControl1;
            this.txtpycode.TabIndex = 17;
            // 
            // txtMethod
            // 
            this.txtMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_method", true));
            this.txtMethod.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_method", true));
            this.txtMethod.Location = new System.Drawing.Point(70, 402);
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.Size = new System.Drawing.Size(215, 20);
            this.txtMethod.StyleController = this.layoutControl1;
            this.txtMethod.TabIndex = 16;
            // 
            // txtUpper
            // 
            this.txtUpper.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_upper_limit", true));
            this.txtUpper.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_upper_limit", true));
            this.txtUpper.Location = new System.Drawing.Point(70, 306);
            this.txtUpper.Name = "txtUpper";
            this.txtUpper.Size = new System.Drawing.Size(215, 20);
            this.txtUpper.StyleController = this.layoutControl1;
            this.txtUpper.TabIndex = 15;
            // 
            // txtLower
            // 
            this.txtLower.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_lower_limit", true));
            this.txtLower.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_lower_limit", true));
            this.txtLower.Location = new System.Drawing.Point(70, 282);
            this.txtLower.Name = "txtLower";
            this.txtLower.Size = new System.Drawing.Size(215, 20);
            this.txtLower.StyleController = this.layoutControl1;
            this.txtLower.TabIndex = 14;
            // 
            // txtTender
            // 
            this.txtTender.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_tender", true));
            this.txtTender.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_tender", true));
            this.txtTender.Location = new System.Drawing.Point(70, 378);
            this.txtTender.Name = "txtTender";
            this.txtTender.Size = new System.Drawing.Size(215, 20);
            this.txtTender.StyleController = this.layoutControl1;
            this.txtTender.TabIndex = 13;
            // 
            // txtCertificate
            // 
            this.txtCertificate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_certificate", true));
            this.txtCertificate.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_certificate", true));
            this.txtCertificate.Location = new System.Drawing.Point(70, 354);
            this.txtCertificate.Name = "txtCertificate";
            this.txtCertificate.Size = new System.Drawing.Size(215, 20);
            this.txtCertificate.StyleController = this.layoutControl1;
            this.txtCertificate.TabIndex = 12;
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
            this.selectDicReaStoreCondition1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_condition", true));
            this.selectDicReaStoreCondition1.displayMember = "";
            this.selectDicReaStoreCondition1.EnterMoveNext = true;
            this.selectDicReaStoreCondition1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaStoreCondition1.KeyUpDownMoveNext = false;
            this.selectDicReaStoreCondition1.LoadDataOnDesignMode = true;
            this.selectDicReaStoreCondition1.Location = new System.Drawing.Point(70, 257);
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
            this.selectDicReaStoreCondition1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaStoreCondition1.TabIndex = 11;
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
            this.selectDicReaStorePosition1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_position", true));
            this.selectDicReaStorePosition1.displayMember = "";
            this.selectDicReaStorePosition1.EnterMoveNext = true;
            this.selectDicReaStorePosition1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaStorePosition1.KeyUpDownMoveNext = false;
            this.selectDicReaStorePosition1.LoadDataOnDesignMode = true;
            this.selectDicReaStorePosition1.Location = new System.Drawing.Point(70, 232);
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
            this.selectDicReaStorePosition1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaStorePosition1.TabIndex = 10;
            this.selectDicReaStorePosition1.UseExtend = false;
            this.selectDicReaStorePosition1.valueMember = "";
            // 
            // selectDicReaGroup1
            // 
            this.selectDicReaGroup1.AddEmptyRow = true;
            this.selectDicReaGroup1.BindByValue = true;
            this.selectDicReaGroup1.colDisplay = "";
            this.selectDicReaGroup1.colExtend1 = "";
            this.selectDicReaGroup1.colInCode = "";
            this.selectDicReaGroup1.colPY = "";
            this.selectDicReaGroup1.colSeq = "";
            this.selectDicReaGroup1.colValue = "";
            this.selectDicReaGroup1.colWB = "";
            this.selectDicReaGroup1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_group", true));
            this.selectDicReaGroup1.displayMember = "";
            this.selectDicReaGroup1.EnterMoveNext = true;
            this.selectDicReaGroup1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaGroup1.KeyUpDownMoveNext = false;
            this.selectDicReaGroup1.LoadDataOnDesignMode = true;
            this.selectDicReaGroup1.Location = new System.Drawing.Point(70, 207);
            this.selectDicReaGroup1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaGroup1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaGroup1.Name = "selectDicReaGroup1";
            this.selectDicReaGroup1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaGroup1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaGroup1.Readonly = false;
            this.selectDicReaGroup1.SaveSourceID = false;
            this.selectDicReaGroup1.SelectFilter = null;
            this.selectDicReaGroup1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaGroup1.SelectOnly = true;
            this.selectDicReaGroup1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaGroup1.TabIndex = 9;
            this.selectDicReaGroup1.UseExtend = false;
            this.selectDicReaGroup1.valueMember = "";
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
            this.selectDicReaProduct1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_product", true));
            this.selectDicReaProduct1.displayMember = "";
            this.selectDicReaProduct1.EnterMoveNext = true;
            this.selectDicReaProduct1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaProduct1.KeyUpDownMoveNext = false;
            this.selectDicReaProduct1.LoadDataOnDesignMode = true;
            this.selectDicReaProduct1.Location = new System.Drawing.Point(70, 157);
            this.selectDicReaProduct1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaProduct1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaProduct1.Name = "selectDicReaProduct1";
            this.selectDicReaProduct1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaProduct1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaProduct1.Readonly = false;
            this.selectDicReaProduct1.SaveSourceID = false;
            this.selectDicReaProduct1.SelectFilter = null;
            this.selectDicReaProduct1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaProduct1.SelectOnly = true;
            this.selectDicReaProduct1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaProduct1.TabIndex = 8;
            this.selectDicReaProduct1.UseExtend = false;
            this.selectDicReaProduct1.valueMember = "";
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
            this.selectDicReaUnit1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsReaSetting, "Drea_unit", true));
            this.selectDicReaUnit1.displayMember = "";
            this.selectDicReaUnit1.EnterMoveNext = true;
            this.selectDicReaUnit1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicReaUnit1.KeyUpDownMoveNext = false;
            this.selectDicReaUnit1.LoadDataOnDesignMode = true;
            this.selectDicReaUnit1.Location = new System.Drawing.Point(70, 132);
            this.selectDicReaUnit1.MaximumSize = new System.Drawing.Size(500, 21);
            this.selectDicReaUnit1.MinimumSize = new System.Drawing.Size(50, 21);
            this.selectDicReaUnit1.Name = "selectDicReaUnit1";
            this.selectDicReaUnit1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicReaUnit1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicReaUnit1.Readonly = false;
            this.selectDicReaUnit1.SaveSourceID = false;
            this.selectDicReaUnit1.SelectFilter = null;
            this.selectDicReaUnit1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicReaUnit1.SelectOnly = true;
            this.selectDicReaUnit1.Size = new System.Drawing.Size(215, 21);
            this.selectDicReaUnit1.TabIndex = 7;
            this.selectDicReaUnit1.UseExtend = false;
            this.selectDicReaUnit1.valueMember = "";
            // 
            // txtPackage
            // 
            this.txtPackage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Drea_package", true));
            this.txtPackage.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_package", true));
            this.txtPackage.Location = new System.Drawing.Point(70, 108);
            this.txtPackage.Name = "txtPackage";
            this.txtPackage.Size = new System.Drawing.Size(215, 20);
            this.txtPackage.StyleController = this.layoutControl1;
            this.txtPackage.TabIndex = 6;
            // 
            // txtBarcode
            // 
            this.txtBarcode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsReaSetting, "Barcode", true));
            this.txtBarcode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Barcode", true));
            this.txtBarcode.Location = new System.Drawing.Point(70, 84);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(215, 20);
            this.txtBarcode.StyleController = this.layoutControl1;
            this.txtBarcode.TabIndex = 5;
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsReaSetting, "Drea_name", true));
            this.txtName.Location = new System.Drawing.Point(70, 60);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(215, 20);
            this.txtName.StyleController = this.layoutControl1;
            this.txtName.TabIndex = 4;
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
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem15,
            this.layoutControlItem16,
            this.layoutControlItem17,
            this.layoutControlItem18,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem21,
            this.layoutControlItem20,
            this.layoutControlItem22,
            this.layoutControlItem23});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(297, 529);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem1.Control = this.txtName;
            this.layoutControlItem1.CustomizationFormText = "试剂名称";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem1.Text = "*试剂名称";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtBarcode;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem2.Text = "条形码";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtPackage;
            this.layoutControlItem3.CustomizationFormText = "包装规格";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem3.Text = "包装规格";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem4.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem4.Control = this.selectDicReaUnit1;
            this.layoutControlItem4.CustomizationFormText = "单位";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem4.Text = "*单位";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem5.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem5.Control = this.selectDicReaProduct1;
            this.layoutControlItem5.CustomizationFormText = "生产厂商";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 145);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem5.Text = "*生产厂商";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem6.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem6.Control = this.selectDicReaGroup1;
            this.layoutControlItem6.CustomizationFormText = "试剂组别";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 195);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem6.Text = "*试剂组别";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.selectDicReaStorePosition1;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 220);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem7.Text = "存储位置";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.selectDicReaStoreCondition1;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 245);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem8.Text = "存储条件";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.txtCertificate;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 342);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem9.Text = "注册证号";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.txtTender;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 366);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem10.Text = "招标编号";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.txtMethod;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 390);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem13.Text = "方法学";
            this.layoutControlItem13.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.txtpycode;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 414);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem14.Text = "拼音码";
            this.layoutControlItem14.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.txtwbcode;
            this.layoutControlItem15.Location = new System.Drawing.Point(0, 438);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem15.Text = "五笔码";
            this.layoutControlItem15.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this.txtRemark;
            this.layoutControlItem16.Location = new System.Drawing.Point(0, 462);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem16.Text = "备注";
            this.layoutControlItem16.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.cePrintFlag;
            this.layoutControlItem17.Location = new System.Drawing.Point(0, 486);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(138, 23);
            this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem17.TextVisible = false;
            // 
            // layoutControlItem18
            // 
            this.layoutControlItem18.Control = this.ceDelFlag;
            this.layoutControlItem18.Location = new System.Drawing.Point(138, 486);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.Size = new System.Drawing.Size(139, 23);
            this.layoutControlItem18.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem18.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.txtLower;
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 270);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem11.Text = "库存低限";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.txtUpper;
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 294);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem12.Text = "库存高限";
            this.layoutControlItem12.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.Control = this.txtId;
            this.layoutControlItem21.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem21.Text = "试剂编码";
            this.layoutControlItem21.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            this.layoutControlItem20.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem20.Control = this.selectDicReaSupplier1;
            this.layoutControlItem20.Location = new System.Drawing.Point(0, 170);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.Size = new System.Drawing.Size(277, 25);
            this.layoutControlItem20.Text = "*供货商";
            this.layoutControlItem20.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this.textEdit1;
            this.layoutControlItem22.Location = new System.Drawing.Point(0, 318);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem22.Text = "报警天数";
            this.layoutControlItem22.TextSize = new System.Drawing.Size(55, 14);
            // 
            // layoutControlItem23
            // 
            this.layoutControlItem23.Control = this.textEdit2;
            this.layoutControlItem23.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem23.Name = "layoutControlItem23";
            this.layoutControlItem23.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem23.Text = "省标编号";
            this.layoutControlItem23.TextSize = new System.Drawing.Size(55, 14);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcReagentSetting);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 53);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(865, 479);
            this.panelControl2.TabIndex = 1;
            // 
            // gcReagentSetting
            // 
            this.gcReagentSetting.DataSource = this.bsReaSetting;
            this.gcReagentSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReagentSetting.Location = new System.Drawing.Point(2, 2);
            this.gcReagentSetting.MainView = this.gvReagentSetting;
            this.gcReagentSetting.Name = "gcReagentSetting";
            this.gcReagentSetting.Size = new System.Drawing.Size(861, 475);
            this.gcReagentSetting.TabIndex = 0;
            this.gcReagentSetting.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReagentSetting});
            // 
            // gvReagentSetting
            // 
            this.gvReagentSetting.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDrea_name,
            this.colBarcode,
            this.colDrea_id,
            this.colDrea_package,
            this.colDrea_unit,
            this.colDrea_product,
            this.colDrea_group,
            this.colDrea_position,
            this.colDrea_condition,
            this.colDrea_lower_limit,
            this.colDrea_upper_limit,
            this.gridColumn2,
            this.colDrea_certificate,
            this.colDrea_tender,
            this.colDrea_method,
            this.colDrea_remark,
            this.colpy_code,
            this.colwb_code,
            this.gridColumn1,
            this.colDrea_provincialno});
            this.gvReagentSetting.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvReagentSetting.GridControl = this.gcReagentSetting;
            this.gvReagentSetting.Name = "gvReagentSetting";
            this.gvReagentSetting.OptionsBehavior.Editable = false;
            this.gvReagentSetting.OptionsFilter.ShowAllTableValuesInFilterPopup = true;
            this.gvReagentSetting.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReagentSetting.OptionsView.ColumnAutoWidth = false;
            this.gvReagentSetting.OptionsView.ShowFooter = true;
            this.gvReagentSetting.OptionsView.ShowGroupPanel = false;
            this.gvReagentSetting.OptionsView.ShowIndicator = false;
            this.gvReagentSetting.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colDrea_id, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colDrea_name
            // 
            this.colDrea_name.Caption = "名称";
            this.colDrea_name.FieldName = "Drea_name";
            this.colDrea_name.Name = "colDrea_name";
            this.colDrea_name.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Drea_name", "记录总数:{0:0.##}")});
            this.colDrea_name.Visible = true;
            this.colDrea_name.VisibleIndex = 1;
            this.colDrea_name.Width = 100;
            // 
            // colBarcode
            // 
            this.colBarcode.Caption = "条码号";
            this.colBarcode.FieldName = "Barcode";
            this.colBarcode.Name = "colBarcode";
            // 
            // colDrea_id
            // 
            this.colDrea_id.Caption = "编码";
            this.colDrea_id.FieldName = "Drea_id";
            this.colDrea_id.Name = "colDrea_id";
            this.colDrea_id.Visible = true;
            this.colDrea_id.VisibleIndex = 0;
            this.colDrea_id.Width = 48;
            // 
            // colDrea_package
            // 
            this.colDrea_package.Caption = "包装规格";
            this.colDrea_package.FieldName = "Drea_package";
            this.colDrea_package.Name = "colDrea_package";
            this.colDrea_package.Visible = true;
            this.colDrea_package.VisibleIndex = 3;
            this.colDrea_package.Width = 62;
            // 
            // colDrea_unit
            // 
            this.colDrea_unit.Caption = "单位";
            this.colDrea_unit.FieldName = "Runit_name";
            this.colDrea_unit.Name = "colDrea_unit";
            this.colDrea_unit.Visible = true;
            this.colDrea_unit.VisibleIndex = 4;
            this.colDrea_unit.Width = 40;
            // 
            // colDrea_product
            // 
            this.colDrea_product.Caption = "生产厂商";
            this.colDrea_product.FieldName = "Rpdt_name";
            this.colDrea_product.Name = "colDrea_product";
            this.colDrea_product.Visible = true;
            this.colDrea_product.VisibleIndex = 5;
            this.colDrea_product.Width = 119;
            // 
            // colDrea_group
            // 
            this.colDrea_group.Caption = "组别";
            this.colDrea_group.FieldName = "Rea_group";
            this.colDrea_group.Name = "colDrea_group";
            this.colDrea_group.Visible = true;
            this.colDrea_group.VisibleIndex = 7;
            // 
            // colDrea_position
            // 
            this.colDrea_position.Caption = "存储位置";
            this.colDrea_position.FieldName = "Rstr_position";
            this.colDrea_position.Name = "colDrea_position";
            this.colDrea_position.Visible = true;
            this.colDrea_position.VisibleIndex = 8;
            // 
            // colDrea_condition
            // 
            this.colDrea_condition.Caption = "存储条件";
            this.colDrea_condition.FieldName = "Rstr_condition";
            this.colDrea_condition.Name = "colDrea_condition";
            this.colDrea_condition.Visible = true;
            this.colDrea_condition.VisibleIndex = 9;
            // 
            // colDrea_lower_limit
            // 
            this.colDrea_lower_limit.Caption = "库存低限";
            this.colDrea_lower_limit.FieldName = "Drea_lower_limit";
            this.colDrea_lower_limit.Name = "colDrea_lower_limit";
            this.colDrea_lower_limit.Visible = true;
            this.colDrea_lower_limit.VisibleIndex = 10;
            // 
            // colDrea_upper_limit
            // 
            this.colDrea_upper_limit.Caption = "库存高限";
            this.colDrea_upper_limit.FieldName = "Drea_upper_limit";
            this.colDrea_upper_limit.Name = "colDrea_upper_limit";
            this.colDrea_upper_limit.Visible = true;
            this.colDrea_upper_limit.VisibleIndex = 11;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "报警天数";
            this.gridColumn2.FieldName = "Drea_alarmdays";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 18;
            // 
            // colDrea_certificate
            // 
            this.colDrea_certificate.Caption = "注册证号";
            this.colDrea_certificate.FieldName = "Drea_certificate";
            this.colDrea_certificate.Name = "colDrea_certificate";
            this.colDrea_certificate.Visible = true;
            this.colDrea_certificate.VisibleIndex = 12;
            // 
            // colDrea_tender
            // 
            this.colDrea_tender.Caption = "招标编号";
            this.colDrea_tender.FieldName = "Drea_tender";
            this.colDrea_tender.Name = "colDrea_tender";
            this.colDrea_tender.Visible = true;
            this.colDrea_tender.VisibleIndex = 13;
            // 
            // colDrea_method
            // 
            this.colDrea_method.Caption = "方法学";
            this.colDrea_method.FieldName = "Drea_method";
            this.colDrea_method.Name = "colDrea_method";
            this.colDrea_method.Visible = true;
            this.colDrea_method.VisibleIndex = 14;
            // 
            // colDrea_remark
            // 
            this.colDrea_remark.Caption = "备注";
            this.colDrea_remark.FieldName = "Drea_remark";
            this.colDrea_remark.Name = "colDrea_remark";
            this.colDrea_remark.Visible = true;
            this.colDrea_remark.VisibleIndex = 15;
            // 
            // colpy_code
            // 
            this.colpy_code.Caption = "拼音码";
            this.colpy_code.FieldName = "py_code";
            this.colpy_code.Name = "colpy_code";
            this.colpy_code.Visible = true;
            this.colpy_code.VisibleIndex = 16;
            // 
            // colwb_code
            // 
            this.colwb_code.Caption = "五笔码";
            this.colwb_code.FieldName = "wb_code";
            this.colwb_code.Name = "colwb_code";
            this.colwb_code.Visible = true;
            this.colwb_code.VisibleIndex = 17;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "供货商";
            this.gridColumn1.FieldName = "Rsupplier_name";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 6;
            // 
            // colDrea_provincialno
            // 
            this.colDrea_provincialno.Caption = "省标编号";
            this.colDrea_provincialno.FieldName = "Drea_provincialno";
            this.colDrea_provincialno.Name = "colDrea_provincialno";
            this.colDrea_provincialno.Visible = true;
            this.colDrea_provincialno.VisibleIndex = 2;
            this.colDrea_provincialno.Width = 97;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(865, 53);
            this.panelControl1.TabIndex = 0;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.txtSort);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(2, 2);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(861, 49);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // txtSort
            // 
            this.txtSort.Location = new System.Drawing.Point(75, 12);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(774, 20);
            this.txtSort.StyleController = this.layoutControl2;
            this.txtSort.TabIndex = 4;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem19});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(861, 49);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Control = this.txtSort;
            this.layoutControlItem19.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Size = new System.Drawing.Size(841, 29);
            this.layoutControlItem19.Text = "关键字搜索";
            this.layoutControlItem19.TextSize = new System.Drawing.Size(60, 14);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sysToolBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1192, 66);
            this.panel1.TabIndex = 2;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1192, 66);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            // 
            // FrmReagentSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 602);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReagentSetting";
            this.Text = "试剂库设置";
            this.Load += new System.EventHandler(this.FrmReagentSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsReaSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDelFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePrintFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtwbcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtpycode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMethod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLower.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTender.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCertificate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPackage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReagentSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReagentSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.TextEdit txtBarcode;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit txtPackage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private SelectDicReaUnit selectDicReaUnit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Panel panel1;
        private SysToolBar sysToolBar1;
        private SelectDicReaProduct selectDicReaProduct1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private SelectDicReaGroup selectDicReaGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private SelectDicReaStorePosition selectDicReaStorePosition1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private SelectDicReaStoreCondition selectDicReaStoreCondition1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.TextEdit txtCertificate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.TextEdit txtUpper;
        private DevExpress.XtraEditors.TextEdit txtLower;
        private DevExpress.XtraEditors.TextEdit txtTender;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.TextEdit txtMethod;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.TextEdit txtRemark;
        private DevExpress.XtraEditors.TextEdit txtwbcode;
        private DevExpress.XtraEditors.TextEdit txtpycode;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem16;
        private DevExpress.XtraEditors.CheckEdit ceDelFlag;
        private DevExpress.XtraEditors.CheckEdit cePrintFlag;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem17;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem18;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.TextEdit txtSort;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem19;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gcReagentSetting;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReagentSetting;
        private System.Windows.Forms.BindingSource bsReaSetting;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_name;
        private DevExpress.XtraGrid.Columns.GridColumn colBarcode;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_id;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_package;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_unit;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_product;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_group;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_position;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_condition;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_lower_limit;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_upper_limit;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_certificate;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_tender;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_method;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_remark;
        private DevExpress.XtraGrid.Columns.GridColumn colpy_code;
        private DevExpress.XtraGrid.Columns.GridColumn colwb_code;
        private DevExpress.XtraEditors.TextEdit txtId;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem21;
        private SelectDicReaSupplier selectDicReaSupplier1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem23;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_provincialno;
    }
}