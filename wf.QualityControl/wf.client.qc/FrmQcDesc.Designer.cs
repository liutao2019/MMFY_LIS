using dcl.entity;

namespace dcl.client.qc
{
    partial class FrmQcDesc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQcDesc));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.cmbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lue_Items = new dcl.client.control.SelectDicItmItem();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcQcAnalysis = new DevExpress.XtraGrid.GridControl();
            this.bsQcAnalysis = new System.Windows.Forms.BindingSource(this.components);
            this.gvQcAnalysis = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coledate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.collevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colanalysis = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colaudit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lblAuditName = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this.rbAnalysis = new System.Windows.Forms.RichTextBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dtEdate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dtSdate = new DevExpress.XtraEditors.DateEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcQcAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQcAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSdate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSdate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.cmbType);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.lue_Items);
            this.panelControl3.Controls.Add(this.labelControl8);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(875, 44);
            this.panelControl3.TabIndex = 34;
            // 
            // cmbType
            // 
            this.cmbType.EnterMoveNextControl = true;
            this.cmbType.Location = new System.Drawing.Point(89, 12);
            this.cmbType.Name = "cmbType";
            this.cmbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbType.Properties.Items.AddRange(new object[] {
            "H",
            "M",
            "L"});
            this.cmbType.Size = new System.Drawing.Size(141, 20);
            this.cmbType.TabIndex = 23;
            this.cmbType.EditValueChanged += new System.EventHandler(this.cmbType_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl3.Location = new System.Drawing.Point(13, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(70, 17);
            this.labelControl3.TabIndex = 22;
            this.labelControl3.Text = "质控物水平";
            // 
            // lue_Items
            // 
            this.lue_Items.AddEmptyRow = true;
            this.lue_Items.BindByValue = false;
            this.lue_Items.colDisplay = "";
            this.lue_Items.colExtend1 = null;
            this.lue_Items.colInCode = "";
            this.lue_Items.colPY = "";
            this.lue_Items.colSeq = "ItmSortNo";
            this.lue_Items.colValue = "";
            this.lue_Items.colWB = "";
            this.lue_Items.displayMember = "";
            this.lue_Items.EnterMoveNext = true;
            this.lue_Items.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Items.KeyUpDownMoveNext = false;
            this.lue_Items.LoadDataOnDesignMode = true;
            this.lue_Items.Location = new System.Drawing.Point(340, 12);
            this.lue_Items.MaximumSize = new System.Drawing.Size(500, 21);
            this.lue_Items.MinimumSize = new System.Drawing.Size(50, 21);
            this.lue_Items.Name = "lue_Items";
            this.lue_Items.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Items.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Items.Readonly = false;
            this.lue_Items.SaveSourceID = false;
            this.lue_Items.SelectFilter = null;
            this.lue_Items.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Items.SelectOnly = true;
            this.lue_Items.Size = new System.Drawing.Size(125, 21);
            this.lue_Items.TabIndex = 4;
            this.lue_Items.UseExtend = false;
            this.lue_Items.valueMember = "";
            this.lue_Items.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicItmItem>.ValueChangedEventHandler(this.lue_Items_ValueChanged);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl8.Location = new System.Drawing.Point(274, 13);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(56, 17);
            this.labelControl8.TabIndex = 21;
            this.labelControl8.Text = "测定项目";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 59);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(879, 337);
            this.panelControl2.TabIndex = 4;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 46);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gcQcAnalysis);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl5);
            this.splitContainerControl1.Panel2.Controls.Add(this.lblAuditName);
            this.splitContainerControl1.Panel2.Controls.Add(this.label1);
            this.splitContainerControl1.Panel2.Controls.Add(this.rbAnalysis);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl4);
            this.splitContainerControl1.Panel2.Controls.Add(this.dtEdate);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.dtSdate);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl17);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(875, 289);
            this.splitContainerControl1.SplitterPosition = 307;
            this.splitContainerControl1.TabIndex = 35;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcQcAnalysis
            // 
            this.gcQcAnalysis.DataSource = this.bsQcAnalysis;
            this.gcQcAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQcAnalysis.Location = new System.Drawing.Point(0, 0);
            this.gcQcAnalysis.MainView = this.gvQcAnalysis;
            this.gcQcAnalysis.Name = "gcQcAnalysis";
            this.gcQcAnalysis.Size = new System.Drawing.Size(307, 289);
            this.gcQcAnalysis.TabIndex = 51;
            this.gcQcAnalysis.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQcAnalysis});
            // 
            // bsQcAnalysis
            // 
            this.bsQcAnalysis.DataSource = typeof(dcl.entity.EntityObrQcAnalysis);
            // 
            // gvQcAnalysis
            // 
            this.gvQcAnalysis.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_id,
            this.colitm_code,
            this.colsdate,
            this.coledate,
            this.collevel,
            this.colanalysis,
            this.colaudit,
            this.gridColumn1,
            this.gridColumn2});
            this.gvQcAnalysis.GridControl = this.gcQcAnalysis;
            this.gvQcAnalysis.Name = "gvQcAnalysis";
            this.gvQcAnalysis.OptionsBehavior.Editable = false;
            this.gvQcAnalysis.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvQcAnalysis.OptionsView.ColumnAutoWidth = false;
            this.gvQcAnalysis.OptionsView.ShowGroupPanel = false;
            // 
            // colitr_id
            // 
            this.colitr_id.Caption = "仪器名称";
            this.colitr_id.FieldName = "QanItrName";
            this.colitr_id.Name = "colitr_id";
            this.colitr_id.OptionsColumn.AllowEdit = false;
            this.colitr_id.Visible = true;
            this.colitr_id.VisibleIndex = 0;
            this.colitr_id.Width = 64;
            // 
            // colitm_code
            // 
            this.colitm_code.Caption = "项目名称";
            this.colitm_code.FieldName = "QanItmName";
            this.colitm_code.Name = "colitm_code";
            this.colitm_code.OptionsColumn.AllowEdit = false;
            this.colitm_code.Visible = true;
            this.colitm_code.VisibleIndex = 1;
            // 
            // colsdate
            // 
            this.colsdate.Caption = "开始日期";
            this.colsdate.FieldName = "QanDateStart";
            this.colsdate.Name = "colsdate";
            this.colsdate.OptionsColumn.AllowEdit = false;
            this.colsdate.Visible = true;
            this.colsdate.VisibleIndex = 2;
            this.colsdate.Width = 89;
            // 
            // coledate
            // 
            this.coledate.AppearanceHeader.Options.UseTextOptions = true;
            this.coledate.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.coledate.Caption = "结束日期";
            this.coledate.FieldName = "QanDateEnd";
            this.coledate.Name = "coledate";
            this.coledate.OptionsColumn.AllowEdit = false;
            this.coledate.Visible = true;
            this.coledate.VisibleIndex = 3;
            this.coledate.Width = 66;
            // 
            // collevel
            // 
            this.collevel.Caption = "质控水平";
            this.collevel.FieldName = "QanLevel";
            this.collevel.Name = "collevel";
            this.collevel.OptionsColumn.AllowEdit = false;
            this.collevel.Visible = true;
            this.collevel.VisibleIndex = 4;
            // 
            // colanalysis
            // 
            this.colanalysis.Caption = "质控分析内容";
            this.colanalysis.FieldName = "QanAnanlysis";
            this.colanalysis.Name = "colanalysis";
            this.colanalysis.OptionsColumn.AllowEdit = false;
            this.colanalysis.Visible = true;
            this.colanalysis.VisibleIndex = 5;
            this.colanalysis.Width = 87;
            // 
            // colaudit
            // 
            this.colaudit.AppearanceCell.Options.UseTextOptions = true;
            this.colaudit.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colaudit.Caption = "审核者";
            this.colaudit.FieldName = "QanAuditUserName";
            this.colaudit.Name = "colaudit";
            this.colaudit.OptionsColumn.AllowEdit = false;
            this.colaudit.Visible = true;
            this.colaudit.VisibleIndex = 6;
            this.colaudit.Width = 45;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "审核标志";
            this.gridColumn1.FieldName = "QanAuditFlag";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 7;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "审核时间";
            this.gridColumn2.FieldName = "QanAuditDate";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsQcAnalysis, "QanAuditDate", true));
            this.labelControl1.Location = new System.Drawing.Point(344, 212);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 14);
            this.labelControl1.TabIndex = 181;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(288, 212);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(52, 14);
            this.labelControl5.TabIndex = 180;
            this.labelControl5.Text = "审核时间:";
            // 
            // lblAuditName
            // 
            this.lblAuditName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsQcAnalysis, "QanAuditUserName", true));
            this.lblAuditName.Location = new System.Drawing.Point(107, 213);
            this.lblAuditName.Name = "lblAuditName";
            this.lblAuditName.Size = new System.Drawing.Size(0, 14);
            this.lblAuditName.TabIndex = 179;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(62, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 14);
            this.label1.TabIndex = 178;
            this.label1.Text = "审核者:";
            // 
            // rbAnalysis
            // 
            this.rbAnalysis.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsQcAnalysis, "QanAnanlysis", true));
            this.rbAnalysis.Location = new System.Drawing.Point(113, 77);
            this.rbAnalysis.Name = "rbAnalysis";
            this.rbAnalysis.Size = new System.Drawing.Size(361, 102);
            this.rbAnalysis.TabIndex = 72;
            this.rbAnalysis.Text = "";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(23, 77);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(84, 14);
            this.labelControl4.TabIndex = 71;
            this.labelControl4.Text = "质控分析内容：";
            // 
            // dtEdate
            // 
            this.dtEdate.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsQcAnalysis, "QanDateEnd", true));
            this.dtEdate.EditValue = null;
            this.dtEdate.EnterMoveNextControl = true;
            this.dtEdate.Location = new System.Drawing.Point(347, 26);
            this.dtEdate.Name = "dtEdate";
            this.dtEdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEdate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtEdate.Size = new System.Drawing.Size(127, 20);
            this.dtEdate.TabIndex = 68;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(280, 28);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 69;
            this.labelControl2.Text = "结束时间：";
            // 
            // dtSdate
            // 
            this.dtSdate.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsQcAnalysis, "QanDateStart", true));
            this.dtSdate.EditValue = null;
            this.dtSdate.EnterMoveNextControl = true;
            this.dtSdate.Location = new System.Drawing.Point(111, 26);
            this.dtSdate.Name = "dtSdate";
            this.dtSdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtSdate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtSdate.Size = new System.Drawing.Size(127, 20);
            this.dtSdate.TabIndex = 65;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(47, 28);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(60, 14);
            this.labelControl17.TabIndex = 66;
            this.labelControl17.Text = "开始时间：";
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(879, 59);
            this.sysToolBar1.TabIndex = 5;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.btnAdd_Click);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.btnDel_Click);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.btnSave_Click);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.btnReferesh_Click);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.btnClose_Click);
            this.sysToolBar1.OnBtnQualityRuleClicked += new System.EventHandler(this.sysToolBar1_BtnQualityAuditClicked);
            this.sysToolBar1.OnBtnSinglePrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnSinglePrintClicked);
            // 
            // FrmQcDesc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 396);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.sysToolBar1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "FrmQcDesc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "质控评价";
            this.Load += new System.EventHandler(this.FrmMensurate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcQcAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQcAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSdate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSdate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.BindingSource bsQcAnalysis;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private dcl.client.control.SelectDicItmItem lue_Items;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbType;
        private System.Windows.Forms.RichTextBox rbAnalysis;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit dtEdate;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dtSdate;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraGrid.GridControl gcQcAnalysis;
        public DevExpress.XtraGrid.Views.Grid.GridView gvQcAnalysis;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colitm_code;
        private DevExpress.XtraGrid.Columns.GridColumn colsdate;
        private DevExpress.XtraGrid.Columns.GridColumn coledate;
        private DevExpress.XtraGrid.Columns.GridColumn collevel;
        private DevExpress.XtraGrid.Columns.GridColumn colanalysis;
        private DevExpress.XtraGrid.Columns.GridColumn colaudit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        protected DevExpress.XtraEditors.LabelControl labelControl1;
        protected DevExpress.XtraEditors.LabelControl labelControl5;
        protected DevExpress.XtraEditors.LabelControl lblAuditName;
        protected DevExpress.XtraEditors.LabelControl label1;
    }
}