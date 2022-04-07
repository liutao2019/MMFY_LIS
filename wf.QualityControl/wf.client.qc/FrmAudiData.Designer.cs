namespace dcl.client.qc
{
    partial class FrmAudiData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAudiData));
            this.gcLot = new DevExpress.XtraGrid.GridControl();
            this.gvLot = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcIsEff = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ckEffective = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.qcm_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_meas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_x = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_c_sd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_ns = new DevExpress.XtraGrid.Columns.GridColumn();
            this.userName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.qcm_reson = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.qcm_fun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.qcm_rem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcLot
            // 
            this.gcLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLot.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLot.Location = new System.Drawing.Point(0, 80);
            this.gcLot.MainView = this.gvLot;
            this.gcLot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLot.Name = "gcLot";
            this.gcLot.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ckEffective,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2});
            this.gcLot.Size = new System.Drawing.Size(1048, 707);
            this.gcLot.TabIndex = 8;
            this.gcLot.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLot});
            this.gcLot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseDown);
            // 
            // gvLot
            // 
            this.gvLot.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvLot.Appearance.Row.Options.UseBackColor = true;
            this.gvLot.ColumnPanelRowHeight = 23;
            this.gvLot.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcIsEff,
            this.qcm_date,
            this.itm_ecd,
            this.qcm_c_no,
            this.qcm_meas,
            this.qcm_c_x,
            this.qcm_c_sd,
            this.qcm_ns,
            this.userName,
            this.gridColumn1,
            this.qcm_reson,
            this.qcm_fun,
            this.qcm_rem});
            this.gvLot.GridControl = this.gcLot;
            this.gvLot.GroupCount = 1;
            this.gvLot.Name = "gvLot";
            this.gvLot.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvLot.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLot.OptionsView.ColumnAutoWidth = false;
            this.gvLot.OptionsView.ShowGroupPanel = false;
            this.gvLot.OptionsView.ShowIndicator = false;
            this.gvLot.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.qcm_c_no, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvLot.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridViewPatientList_CustomDrawColumnHeader);
            // 
            // gcIsEff
            // 
            this.gcIsEff.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gcIsEff.AppearanceCell.Options.UseFont = true;
            this.gcIsEff.AppearanceCell.Options.UseTextOptions = true;
            this.gcIsEff.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcIsEff.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gcIsEff.AppearanceHeader.Options.UseFont = true;
            this.gcIsEff.AppearanceHeader.Options.UseTextOptions = true;
            this.gcIsEff.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcIsEff.ColumnEdit = this.ckEffective;
            this.gcIsEff.FieldName = "QresDisplay";
            this.gcIsEff.Name = "gcIsEff";
            this.gcIsEff.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gcIsEff.OptionsColumn.ShowCaption = false;
            this.gcIsEff.OptionsFilter.AllowAutoFilter = false;
            this.gcIsEff.OptionsFilter.AllowFilter = false;
            this.gcIsEff.Visible = true;
            this.gcIsEff.VisibleIndex = 0;
            this.gcIsEff.Width = 41;
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
            this.qcm_date.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_date.AppearanceCell.Options.UseFont = true;
            this.qcm_date.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_date.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_date.AppearanceHeader.Options.UseFont = true;
            this.qcm_date.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_date.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_date.Caption = "测定日期";
            this.qcm_date.FieldName = "QresDate";
            this.qcm_date.Name = "qcm_date";
            this.qcm_date.OptionsColumn.AllowEdit = false;
            this.qcm_date.OptionsFilter.AllowFilter = false;
            this.qcm_date.Visible = true;
            this.qcm_date.VisibleIndex = 1;
            this.qcm_date.Width = 98;
            // 
            // itm_ecd
            // 
            this.itm_ecd.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.itm_ecd.AppearanceCell.Options.UseFont = true;
            this.itm_ecd.AppearanceCell.Options.UseTextOptions = true;
            this.itm_ecd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.itm_ecd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.itm_ecd.AppearanceHeader.Options.UseFont = true;
            this.itm_ecd.AppearanceHeader.Options.UseTextOptions = true;
            this.itm_ecd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
            this.qcm_c_no.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_no.AppearanceCell.Options.UseFont = true;
            this.qcm_c_no.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_c_no.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_no.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_no.AppearanceHeader.Options.UseFont = true;
            this.qcm_c_no.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_c_no.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_no.Caption = "水平";
            this.qcm_c_no.FieldName = "QcShowLevel";
            this.qcm_c_no.Name = "qcm_c_no";
            this.qcm_c_no.OptionsColumn.AllowEdit = false;
            this.qcm_c_no.OptionsFilter.AllowFilter = false;
            this.qcm_c_no.Width = 40;
            // 
            // qcm_meas
            // 
            this.qcm_meas.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_meas.AppearanceCell.Options.UseFont = true;
            this.qcm_meas.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_meas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_meas.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_meas.AppearanceHeader.Options.UseFont = true;
            this.qcm_meas.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_meas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_meas.Caption = "结果";
            this.qcm_meas.FieldName = "QresValue";
            this.qcm_meas.Name = "qcm_meas";
            this.qcm_meas.OptionsColumn.AllowEdit = false;
            this.qcm_meas.OptionsFilter.AllowFilter = false;
            this.qcm_meas.Visible = true;
            this.qcm_meas.VisibleIndex = 3;
            this.qcm_meas.Width = 45;
            // 
            // qcm_c_x
            // 
            this.qcm_c_x.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_x.AppearanceCell.Options.UseFont = true;
            this.qcm_c_x.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_c_x.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_x.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_x.AppearanceHeader.Options.UseFont = true;
            this.qcm_c_x.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_c_x.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_x.Caption = "靶值";
            this.qcm_c_x.FieldName = "QresItmX";
            this.qcm_c_x.Name = "qcm_c_x";
            this.qcm_c_x.OptionsColumn.AllowEdit = false;
            this.qcm_c_x.OptionsFilter.AllowFilter = false;
            this.qcm_c_x.Width = 34;
            // 
            // qcm_c_sd
            // 
            this.qcm_c_sd.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_sd.AppearanceCell.Options.UseFont = true;
            this.qcm_c_sd.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_c_sd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_sd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_c_sd.AppearanceHeader.Options.UseFont = true;
            this.qcm_c_sd.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_c_sd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_c_sd.Caption = "标准差";
            this.qcm_c_sd.FieldName = "QresItmSd";
            this.qcm_c_sd.Name = "qcm_c_sd";
            this.qcm_c_sd.OptionsColumn.AllowEdit = false;
            this.qcm_c_sd.OptionsFilter.AllowFilter = false;
            this.qcm_c_sd.Width = 42;
            // 
            // qcm_ns
            // 
            this.qcm_ns.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_ns.AppearanceCell.Options.UseFont = true;
            this.qcm_ns.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_ns.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_ns.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_ns.AppearanceHeader.Options.UseFont = true;
            this.qcm_ns.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_ns.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_ns.Caption = "规则";
            this.qcm_ns.FieldName = "QresRunawayRule";
            this.qcm_ns.Name = "qcm_ns";
            this.qcm_ns.OptionsColumn.AllowEdit = false;
            this.qcm_ns.OptionsFilter.AllowFilter = false;
            this.qcm_ns.Visible = true;
            this.qcm_ns.VisibleIndex = 4;
            this.qcm_ns.Width = 150;
            // 
            // userName
            // 
            this.userName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.userName.AppearanceCell.Options.UseFont = true;
            this.userName.AppearanceCell.Options.UseTextOptions = true;
            this.userName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.userName.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.userName.AppearanceHeader.Options.UseFont = true;
            this.userName.AppearanceHeader.Options.UseTextOptions = true;
            this.userName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.userName.Caption = "审核者";
            this.userName.FieldName = "QresAuditUserName";
            this.userName.Name = "userName";
            this.userName.OptionsColumn.AllowEdit = false;
            this.userName.Visible = true;
            this.userName.VisibleIndex = 8;
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
            this.gridColumn1.Caption = "时间";
            this.gridColumn1.DisplayFormat.FormatString = "HH\':\'mm\':\'ss";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn1.FieldName = "QresDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
            // 
            // qcm_reson
            // 
            this.qcm_reson.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_reson.AppearanceCell.Options.UseFont = true;
            this.qcm_reson.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_reson.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_reson.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_reson.AppearanceHeader.Options.UseFont = true;
            this.qcm_reson.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_reson.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_reson.Caption = "失控原因";
            this.qcm_reson.ColumnEdit = this.repositoryItemComboBox1;
            this.qcm_reson.FieldName = "QresReasons";
            this.qcm_reson.Name = "qcm_reson";
            this.qcm_reson.OptionsColumn.AllowEdit = false;
            this.qcm_reson.Visible = true;
            this.qcm_reson.VisibleIndex = 6;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // qcm_fun
            // 
            this.qcm_fun.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_fun.AppearanceCell.Options.UseFont = true;
            this.qcm_fun.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_fun.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_fun.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_fun.AppearanceHeader.Options.UseFont = true;
            this.qcm_fun.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_fun.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_fun.Caption = "处理方式";
            this.qcm_fun.ColumnEdit = this.repositoryItemComboBox2;
            this.qcm_fun.FieldName = "QresProcess";
            this.qcm_fun.Name = "qcm_fun";
            this.qcm_fun.OptionsColumn.AllowEdit = false;
            this.qcm_fun.Visible = true;
            this.qcm_fun.VisibleIndex = 7;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // qcm_rem
            // 
            this.qcm_rem.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_rem.AppearanceCell.Options.UseFont = true;
            this.qcm_rem.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_rem.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_rem.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_rem.AppearanceHeader.Options.UseFont = true;
            this.qcm_rem.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_rem.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_rem.Caption = "备注";
            this.qcm_rem.FieldName = "QresRemark";
            this.qcm_rem.Name = "qcm_rem";
            this.qcm_rem.Visible = true;
            this.qcm_rem.VisibleIndex = 9;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.sysToolBar1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1048, 80);
            this.panelControl1.TabIndex = 9;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(2, 2);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1044, 76);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnQualityRuleClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityRuleClicked);
            this.sysToolBar1.OnBtnQualityAuditClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityAuditClicked);
            this.sysToolBar1.OnBtnQualityDataClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityDataClicked);
            // 
            // FrmAudiData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 787);
            this.Controls.Add(this.gcLot);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAudiData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据审核";
            this.Load += new System.EventHandler(this.FrmAudiData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEffective)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcLot;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLot;
        private DevExpress.XtraGrid.Columns.GridColumn gcIsEff;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ckEffective;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_date;
        private DevExpress.XtraGrid.Columns.GridColumn itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_no;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_meas;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_x;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_c_sd;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_ns;
        private DevExpress.XtraGrid.Columns.GridColumn userName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_reson;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_fun;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_rem;
    }
}