using dcl.entity;

namespace dcl.client.qc
{
    partial class FrmRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRoom));
            this.ckMB = new DevExpress.XtraEditors.CheckEdit();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.gcPatients = new DevExpress.XtraGrid.GridControl();
            this.gvPatients = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gcDate = new DevExpress.XtraGrid.GridControl();
            this.gvDate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lue_Item = new dcl.client.control.SelectDicItmItem();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSid = new DevExpress.XtraEditors.TextEdit();
            this.deStart = new DevExpress.XtraEditors.DateEdit();
            this.lue_Instrmt = new dcl.client.control.SelectDicInstrument();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ckMB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckMB
            // 
            this.ckMB.Location = new System.Drawing.Point(26, 40);
            this.ckMB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckMB.Name = "ckMB";
            this.ckMB.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.ckMB.Properties.Appearance.Options.UseFont = true;
            this.ckMB.Properties.Caption = "酶标质控";
            this.ckMB.Size = new System.Drawing.Size(151, 26);
            this.ckMB.TabIndex = 47;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1237, 81);
            this.sysToolBar1.TabIndex = 11;
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnImportClicked += new System.EventHandler(this.sysToolBar1_OnBtnImportClicked);
            this.sysToolBar1.BtnCalculationClick += new System.EventHandler(this.sysToolBar1_BtnCalculationClick);
            // 
            // gcPatients
            // 
            this.gcPatients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPatients.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcPatients.Location = new System.Drawing.Point(0, 0);
            this.gcPatients.MainView = this.gvPatients;
            this.gcPatients.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcPatients.Name = "gcPatients";
            this.gcPatients.Size = new System.Drawing.Size(974, 662);
            this.gcPatients.TabIndex = 12;
            this.gcPatients.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPatients});
            // 
            // gvPatients
            // 
            this.gvPatients.ColumnPanelRowHeight = 23;
            this.gvPatients.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvPatients.GridControl = this.gcPatients;
            this.gvPatients.Name = "gvPatients";
            this.gvPatients.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvPatients.OptionsView.ShowGroupPanel = false;
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
            this.gridColumn6.Caption = "样本号";
            this.gridColumn6.FieldName = "ObrSid";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
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
            this.gridColumn3.Caption = "检验项目";
            this.gridColumn3.FieldName = "ItmEname";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
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
            this.gridColumn4.Caption = "结果";
            this.gridColumn4.FieldName = "ObrValue";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
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
            this.gridColumn5.Caption = "时间";
            this.gridColumn5.FieldName = "ObrDate";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl3);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gcPatients);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1233, 662);
            this.splitContainerControl1.SplitterPosition = 253;
            this.splitContainerControl1.TabIndex = 13;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gcDate);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 324);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(253, 338);
            this.groupControl2.TabIndex = 50;
            this.groupControl2.Text = "目标批号";
            // 
            // gcDate
            // 
            this.gcDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDate.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDate.Location = new System.Drawing.Point(2, 27);
            this.gcDate.MainView = this.gvDate;
            this.gcDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDate.Name = "gcDate";
            this.gcDate.Size = new System.Drawing.Size(249, 309);
            this.gcDate.TabIndex = 13;
            this.gcDate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDate});
            // 
            // gvDate
            // 
            this.gvDate.ColumnPanelRowHeight = 23;
            this.gvDate.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvDate.GridControl = this.gcDate;
            this.gvDate.Name = "gvDate";
            this.gvDate.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDate.OptionsView.ShowGroupPanel = false;
            this.gvDate.OptionsView.ShowIndicator = false;
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
            this.gridColumn1.Caption = "水平";
            this.gridColumn1.FieldName = "MatLevel";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
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
            this.gridColumn2.Caption = "批号";
            this.gridColumn2.FieldName = "MatBatchNo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label5);
            this.groupControl1.Controls.Add(this.label6);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.lue_Item);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.txtSid);
            this.groupControl1.Controls.Add(this.deStart);
            this.groupControl1.Controls.Add(this.lue_Instrmt);
            this.groupControl1.Controls.Add(this.deEnd);
            this.groupControl1.Controls.Add(this.lueType);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 78);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(253, 246);
            this.groupControl1.TabIndex = 49;
            this.groupControl1.Text = "查询";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 18);
            this.label5.TabIndex = 54;
            this.label5.Text = "检验项目";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 18);
            this.label6.TabIndex = 53;
            this.label6.Text = "样本号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 52;
            this.label3.Text = "测定仪器";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 18);
            this.label4.TabIndex = 51;
            this.label4.Text = "实验组";
            // 
            // lue_Item
            // 
            this.lue_Item.AddEmptyRow = true;
            this.lue_Item.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lue_Item.BindByValue = false;
            this.lue_Item.colDisplay = "";
            this.lue_Item.colExtend1 = null;
            this.lue_Item.colInCode = "";
            this.lue_Item.colPY = "";
            this.lue_Item.colSeq = "ItmSortNo";
            this.lue_Item.colValue = "";
            this.lue_Item.colWB = "";
            this.lue_Item.displayMember = null;
            this.lue_Item.EnterMoveNext = true;
            this.lue_Item.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Item.KeyUpDownMoveNext = false;
            this.lue_Item.LoadDataOnDesignMode = true;
            this.lue_Item.Location = new System.Drawing.Point(88, 210);
            this.lue_Item.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lue_Item.MaximumSize = new System.Drawing.Size(571, 27);
            this.lue_Item.MinimumSize = new System.Drawing.Size(57, 27);
            this.lue_Item.Name = "lue_Item";
            this.lue_Item.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Item.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Item.Readonly = false;
            this.lue_Item.SaveSourceID = false;
            this.lue_Item.SelectFilter = null;
            this.lue_Item.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Item.SelectOnly = true;
            this.lue_Item.Size = new System.Drawing.Size(145, 27);
            this.lue_Item.TabIndex = 46;
            this.lue_Item.UseExtend = false;
            this.lue_Item.valueMember = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 50;
            this.label2.Text = "结束时间";
            // 
            // txtSid
            // 
            this.txtSid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSid.Location = new System.Drawing.Point(88, 176);
            this.txtSid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSid.Name = "txtSid";
            this.txtSid.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtSid.Properties.Appearance.Options.UseFont = true;
            this.txtSid.Size = new System.Drawing.Size(145, 28);
            this.txtSid.TabIndex = 44;
            // 
            // deStart
            // 
            this.deStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deStart.EditValue = null;
            this.deStart.EnterMoveNextControl = true;
            this.deStart.Location = new System.Drawing.Point(88, 42);
            this.deStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deStart.Name = "deStart";
            this.deStart.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deStart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.deStart.Properties.Appearance.Options.UseFont = true;
            this.deStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStart.Size = new System.Drawing.Size(145, 28);
            this.deStart.TabIndex = 1;
            // 
            // lue_Instrmt
            // 
            this.lue_Instrmt.AddEmptyRow = true;
            this.lue_Instrmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lue_Instrmt.BindByValue = false;
            this.lue_Instrmt.colDisplay = "";
            this.lue_Instrmt.colExtend1 = null;
            this.lue_Instrmt.colInCode = "";
            this.lue_Instrmt.colPY = "";
            this.lue_Instrmt.colSeq = "";
            this.lue_Instrmt.colValue = "";
            this.lue_Instrmt.colWB = "";
            this.lue_Instrmt.displayMember = null;
            this.lue_Instrmt.EnterMoveNext = true;
            this.lue_Instrmt.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Instrmt.KeyUpDownMoveNext = false;
            this.lue_Instrmt.LoadDataOnDesignMode = true;
            this.lue_Instrmt.Location = new System.Drawing.Point(88, 143);
            this.lue_Instrmt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lue_Instrmt.MaximumSize = new System.Drawing.Size(571, 27);
            this.lue_Instrmt.MinimumSize = new System.Drawing.Size(57, 27);
            this.lue_Instrmt.Name = "lue_Instrmt";
            this.lue_Instrmt.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Instrmt.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Instrmt.Readonly = false;
            this.lue_Instrmt.SaveSourceID = false;
            this.lue_Instrmt.SelectFilter = null;
            this.lue_Instrmt.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Instrmt.SelectOnly = true;
            this.lue_Instrmt.ShowAllInstrmt = true;
            this.lue_Instrmt.Size = new System.Drawing.Size(145, 27);
            this.lue_Instrmt.TabIndex = 41;
            this.lue_Instrmt.UseExtend = false;
            this.lue_Instrmt.valueMember = null;
            // 
            // deEnd
            // 
            this.deEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(88, 76);
            this.deEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deEnd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.deEnd.Properties.Appearance.Options.UseFont = true;
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Size = new System.Drawing.Size(145, 28);
            this.deEnd.TabIndex = 2;
            // 
            // lueType
            // 
            this.lueType.AddEmptyRow = true;
            this.lueType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.lueType.Location = new System.Drawing.Point(88, 110);
            this.lueType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueType.MaximumSize = new System.Drawing.Size(571, 27);
            this.lueType.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = true;
            this.lueType.Size = new System.Drawing.Size(145, 27);
            this.lueType.TabIndex = 40;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 49;
            this.label1.Text = "开始时间";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.ckMB);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(253, 78);
            this.groupControl3.TabIndex = 13;
            this.groupControl3.Text = "设置";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 81);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1237, 666);
            this.panelControl1.TabIndex = 14;
            // 
            // FrmRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 747);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.sysToolBar1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "室间质控";
            this.Load += new System.EventHandler(this.FrmRoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ckMB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraGrid.GridControl gcPatients;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPatients;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.CheckEdit ckMB;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private System.Windows.Forms.Label label1;
        private control.SelectDicLabProfession lueType;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private control.SelectDicInstrument lue_Instrmt;
        private DevExpress.XtraEditors.DateEdit deStart;
        private DevExpress.XtraEditors.TextEdit txtSid;
        private System.Windows.Forms.Label label2;
        private control.SelectDicItmItem lue_Item;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDate;
        private DevExpress.XtraGrid.GridControl gcDate;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}