namespace dcl.client.sample
{
    partial class FrmVerifOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVerifOrder));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gcBarcode = new DevExpress.XtraGrid.GridControl();
            this.bsPatient = new System.Windows.Forms.BindingSource(this.components);
            this.gvBarcode = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn52 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn45 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ilookupItem = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lueSex = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtPatientName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.deStart = new DevExpress.XtraEditors.DateEdit();
            this.txtPatientID = new DevExpress.XtraEditors.TextEdit();
            this.lcID = new DevExpress.XtraEditors.LabelControl();
            this.selectPatSource = new dcl.client.control.SelectDicPubSource();
            this.labelControl481 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pcBase)).BeginInit();
            this.pcBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBarcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilookupItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBase
            // 
            this.pcBase.Controls.Add(this.panelControl1);
            this.pcBase.Location = new System.Drawing.Point(0, 38);
            this.pcBase.Margin = new System.Windows.Forms.Padding(4);
            this.pcBase.Size = new System.Drawing.Size(1162, 548);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1158, 544);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gcBarcode);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(2, 143);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1154, 399);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "医嘱";
            // 
            // gcBarcode
            // 
            this.gcBarcode.DataSource = this.bsPatient;
            this.gcBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBarcode.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcBarcode.Location = new System.Drawing.Point(2, 27);
            this.gcBarcode.MainView = this.gvBarcode;
            this.gcBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcBarcode.Name = "gcBarcode";
            this.gcBarcode.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ilookupItem,
            this.lueSex});
            this.gcBarcode.Size = new System.Drawing.Size(1150, 370);
            this.gcBarcode.TabIndex = 59;
            this.gcBarcode.TabStop = false;
            this.gcBarcode.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBarcode});
            // 
            // bsPatient
            // 
            this.bsPatient.DataSource = typeof(dcl.entity.EntitySampMain);
            // 
            // gvBarcode
            // 
            this.gvBarcode.Appearance.FocusedRow.BackColor = System.Drawing.Color.Transparent;
            this.gvBarcode.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvBarcode.Appearance.GroupRow.BackColor = System.Drawing.Color.Transparent;
            this.gvBarcode.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvBarcode.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvBarcode.Appearance.OddRow.Options.UseBackColor = true;
            this.gvBarcode.Appearance.Row.Options.UseBackColor = true;
            this.gvBarcode.Appearance.SelectedRow.BackColor = System.Drawing.Color.Red;
            this.gvBarcode.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvBarcode.ColumnPanelRowHeight = 25;
            this.gvBarcode.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn10,
            this.gridColumn12,
            this.gridColumn14,
            this.gridColumn5,
            this.gridColumn52,
            this.gridColumn13,
            this.gridColumn45});
            this.gvBarcode.CustomizationFormBounds = new System.Drawing.Rectangle(592, 334, 208, 194);
            this.gvBarcode.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvBarcode.GridControl = this.gcBarcode;
            this.gvBarcode.Name = "gvBarcode";
            this.gvBarcode.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvBarcode.OptionsDetail.AllowZoomDetail = false;
            this.gvBarcode.OptionsDetail.EnableMasterViewMode = false;
            this.gvBarcode.OptionsDetail.ShowDetailTabs = false;
            this.gvBarcode.OptionsDetail.SmartDetailExpand = false;
            this.gvBarcode.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvBarcode.OptionsSelection.MultiSelect = true;
            this.gvBarcode.OptionsView.ColumnAutoWidth = false;
            this.gvBarcode.OptionsView.ShowGroupPanel = false;
            this.gvBarcode.OptionsView.ShowIndicator = false;
            this.gvBarcode.RowHeight = 25;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "姓名";
            this.gridColumn10.FieldName = "PidName";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.FixedWidth = true;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 121;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn12.AppearanceCell.Options.UseFont = true;
            this.gridColumn12.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn12.AppearanceHeader.Options.UseFont = true;
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "病人ID";
            this.gridColumn12.FieldName = "PidInNo";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 1;
            this.gridColumn12.Width = 123;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn14.AppearanceCell.Options.UseFont = true;
            this.gridColumn14.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn14.AppearanceHeader.Options.UseFont = true;
            this.gridColumn14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "申请时间";
            this.gridColumn14.DisplayFormat.FormatString = "yy\'-\'MM\'-\'dd\' \'HH\':\'mm\':\'ss";
            this.gridColumn14.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn14.FieldName = "SampOccDate";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowEdit = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 6;
            this.gridColumn14.Width = 203;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "项目";
            this.gridColumn5.FieldName = "SampComName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 191;
            // 
            // gridColumn52
            // 
            this.gridColumn52.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn52.AppearanceCell.Options.UseFont = true;
            this.gridColumn52.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn52.AppearanceHeader.Options.UseFont = true;
            this.gridColumn52.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn52.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn52.Caption = "费用类别";
            this.gridColumn52.FieldName = "PidInsuId";
            this.gridColumn52.Name = "gridColumn52";
            this.gridColumn52.Visible = true;
            this.gridColumn52.VisibleIndex = 3;
            this.gridColumn52.Width = 117;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn13.AppearanceCell.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn13.AppearanceHeader.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "规格";
            this.gridColumn13.FieldName = "SampCapcityUnit";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 4;
            this.gridColumn13.Width = 135;
            // 
            // gridColumn45
            // 
            this.gridColumn45.AppearanceCell.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn45.AppearanceCell.Options.UseFont = true;
            this.gridColumn45.AppearanceHeader.Font = new System.Drawing.Font("宋体", 10.5F);
            this.gridColumn45.AppearanceHeader.Options.UseFont = true;
            this.gridColumn45.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn45.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn45.Caption = "总价";
            this.gridColumn45.FieldName = "SampPrice";
            this.gridColumn45.Name = "gridColumn45";
            this.gridColumn45.Visible = true;
            this.gridColumn45.VisibleIndex = 5;
            this.gridColumn45.Width = 129;
            // 
            // ilookupItem
            // 
            this.ilookupItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ilookupItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sam_id", 50, "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sam_name", 100, "名称")});
            this.ilookupItem.DisplayMember = "SamName";
            this.ilookupItem.Name = "ilookupItem";
            this.ilookupItem.NullText = "";
            this.ilookupItem.ValueMember = "SamId";
            // 
            // lueSex
            // 
            this.lueSex.AutoHeight = false;
            this.lueSex.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSex.DisplayMember = "value";
            this.lueSex.Name = "lueSex";
            this.lueSex.ValueMember = "id";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtPatientName);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.deEnd);
            this.groupControl1.Controls.Add(this.deStart);
            this.groupControl1.Controls.Add(this.txtPatientID);
            this.groupControl1.Controls.Add(this.lcID);
            this.groupControl1.Controls.Add(this.selectPatSource);
            this.groupControl1.Controls.Add(this.labelControl481);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1154, 141);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "查询条件";
            // 
            // txtPatientName
            // 
            this.txtPatientName.Location = new System.Drawing.Point(108, 91);
            this.txtPatientName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatientName.Properties.Appearance.Options.UseFont = true;
            this.txtPatientName.Size = new System.Drawing.Size(207, 28);
            this.txtPatientName.TabIndex = 272;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl4.Location = new System.Drawing.Point(26, 91);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(76, 23);
            this.labelControl4.TabIndex = 271;
            this.labelControl4.Text = "患者姓名";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl3.Location = new System.Drawing.Point(336, 46);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(76, 23);
            this.labelControl3.TabIndex = 270;
            this.labelControl3.Text = "结束日期";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl2.Location = new System.Drawing.Point(26, 46);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(76, 23);
            this.labelControl2.TabIndex = 269;
            this.labelControl2.Text = "开始日期";
            // 
            // deEnd
            // 
            this.deEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(418, 46);
            this.deEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deEnd.Properties.Appearance.Options.UseFont = true;
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Properties.DisplayFormat.FormatString = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.EditFormat.FormatString = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.Mask.EditMask = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deEnd.Size = new System.Drawing.Size(207, 28);
            this.deEnd.TabIndex = 268;
            // 
            // deStart
            // 
            this.deStart.EditValue = null;
            this.deStart.EnterMoveNextControl = true;
            this.deStart.Location = new System.Drawing.Point(108, 46);
            this.deStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deStart.Name = "deStart";
            this.deStart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deStart.Properties.Appearance.Options.UseFont = true;
            this.deStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStart.Properties.DisplayFormat.FormatString = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.EditFormat.FormatString = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.Mask.EditMask = "yyyy\'-\'MM\'-\'dd\' \'HH\'-\'mm\'-\'ss";
            this.deStart.Size = new System.Drawing.Size(207, 28);
            this.deStart.TabIndex = 267;
            // 
            // txtPatientID
            // 
            this.txtPatientID.Location = new System.Drawing.Point(418, 91);
            this.txtPatientID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatientID.Name = "txtPatientID";
            this.txtPatientID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatientID.Properties.Appearance.Options.UseFont = true;
            this.txtPatientID.Size = new System.Drawing.Size(207, 28);
            this.txtPatientID.TabIndex = 266;
            // 
            // lcID
            // 
            this.lcID.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lcID.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lcID.Location = new System.Drawing.Point(354, 91);
            this.lcID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcID.Name = "lcID";
            this.lcID.Size = new System.Drawing.Size(58, 23);
            this.lcID.TabIndex = 265;
            this.lcID.Text = "门诊ID";
            // 
            // selectPatSource
            // 
            this.selectPatSource.AddEmptyRow = true;
            this.selectPatSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPatSource.AutoScroll = true;
            this.selectPatSource.BindByValue = true;
            this.selectPatSource.colDisplay = "";
            this.selectPatSource.colExtend1 = null;
            this.selectPatSource.colInCode = "";
            this.selectPatSource.colPY = "";
            this.selectPatSource.colSeq = "";
            this.selectPatSource.colValue = "";
            this.selectPatSource.colWB = "";
            this.selectPatSource.displayMember = "";
            this.selectPatSource.EnterMoveNext = true;
            this.selectPatSource.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectPatSource.KeyUpDownMoveNext = true;
            this.selectPatSource.LoadDataOnDesignMode = true;
            this.selectPatSource.Location = new System.Drawing.Point(729, 46);
            this.selectPatSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectPatSource.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectPatSource.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectPatSource.Name = "selectPatSource";
            this.selectPatSource.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectPatSource.PFont = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectPatSource.Readonly = false;
            this.selectPatSource.SaveSourceID = false;
            this.selectPatSource.SelectFilter = null;
            this.selectPatSource.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectPatSource.SelectOnly = true;
            this.selectPatSource.Size = new System.Drawing.Size(207, 27);
            this.selectPatSource.TabIndex = 263;
            this.selectPatSource.UseExtend = false;
            this.selectPatSource.valueMember = "";
            // 
            // labelControl481
            // 
            this.labelControl481.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl481.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl481.Location = new System.Drawing.Point(647, 46);
            this.labelControl481.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl481.Name = "labelControl481";
            this.labelControl481.Size = new System.Drawing.Size(76, 23);
            this.labelControl481.TabIndex = 264;
            this.labelControl481.Text = "病人来源";
            // 
            // FrmVerifOrder
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 586);
            this.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "FrmVerifOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医嘱查询";
            ((System.ComponentModel.ISupportInitialize)(this.pcBase)).EndInit();
            this.pcBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBarcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBarcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilookupItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gcBarcode;
        public DevExpress.XtraGrid.Views.Grid.GridView gvBarcode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn52;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lueSex;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ilookupItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn45;
        public System.Windows.Forms.BindingSource bsPatient;
        protected control.SelectDicPubSource selectPatSource;
        protected DevExpress.XtraEditors.LabelControl labelControl481;
        protected DevExpress.XtraEditors.LabelControl lcID;
        private DevExpress.XtraEditors.TextEdit txtPatientID;
        protected DevExpress.XtraEditors.LabelControl labelControl3;
        protected DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.DateEdit deStart;
        private DevExpress.XtraEditors.TextEdit txtPatientName;
        protected DevExpress.XtraEditors.LabelControl labelControl4;
    }
}