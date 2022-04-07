namespace dcl.client.oa
{
    partial class FrmDutySelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDutySelect));
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.lblSearch = new DevExpress.XtraEditors.LabelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcDutys = new DevExpress.XtraGrid.GridControl();
            this.bsDutyDict = new System.Windows.Forms.BindingSource();
            this.gvdutys = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colwb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypePy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeWb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDutys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDutyDict)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvdutys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(485, 17);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSearch.Size = new System.Drawing.Size(251, 36);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtSearch_EditValueChanging);
            // 
            // lblSearch
            // 
            this.lblSearch.Appearance.Image = global::dcl.client.oa.Properties.Resources.Find;
            this.lblSearch.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSearch.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblSearch.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.lblSearch.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lblSearch.Location = new System.Drawing.Point(314, 2);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(108, 33);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "查询：";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.gcDutys);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitContainerControl1.Size = new System.Drawing.Size(1168, 891);
            this.splitContainerControl1.SplitterPosition = 512;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblSearch);
            this.panelControl1.Controls.Add(this.txtSearch);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1168, 89);
            this.panelControl1.TabIndex = 3;
            // 
            // gcDutys
            // 
            this.gcDutys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDutys.DataSource = this.bsDutyDict;
            this.gcDutys.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcDutys.Location = new System.Drawing.Point(6, 104);
            this.gcDutys.MainView = this.gvdutys;
            this.gcDutys.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcDutys.Name = "gcDutys";
            this.gcDutys.Size = new System.Drawing.Size(1163, 783);
            this.gcDutys.TabIndex = 2;
            this.gcDutys.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvdutys});
            // 
            // bsDutyDict
            // 
            this.bsDutyDict.DataSource = typeof(dcl.entity.EntityOaDicShift);
            // 
            // gvdutys
            // 
            this.gvdutys.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDID,
            this.colDName,
            this.colpy,
            this.colwb,
            this.colType,
            this.colSdate,
            this.colEdate,
            this.colTypeName,
            this.colTypePy,
            this.colTypeWb});
            this.gvdutys.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvdutys.GridControl = this.gcDutys;
            this.gvdutys.Name = "gvdutys";
            this.gvdutys.OptionsBehavior.Editable = false;
            this.gvdutys.OptionsCustomization.AllowColumnMoving = false;
            this.gvdutys.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvdutys.OptionsView.ShowGroupPanel = false;
            this.gvdutys.DoubleClick += new System.EventHandler(this.gvdutys_DoubleClick);
            // 
            // colDID
            // 
            this.colDID.Caption = "编号";
            this.colDID.FieldName = "ShiftId";
            this.colDID.Name = "colDID";
            this.colDID.Visible = true;
            this.colDID.VisibleIndex = 0;
            // 
            // colDName
            // 
            this.colDName.Caption = "名称";
            this.colDName.FieldName = "ShiftName";
            this.colDName.Name = "colDName";
            this.colDName.Visible = true;
            this.colDName.VisibleIndex = 1;
            // 
            // colpy
            // 
            this.colpy.Caption = "拼音简码";
            this.colpy.FieldName = "PyCode";
            this.colpy.Name = "colpy";
            // 
            // colwb
            // 
            this.colwb.Caption = "五笔简码";
            this.colwb.FieldName = "WbCode";
            this.colwb.Name = "colwb";
            // 
            // colType
            // 
            this.colType.Caption = "所属科室";
            this.colType.FieldName = "ShiftDeptId";
            this.colType.Name = "colType";
            // 
            // colSdate
            // 
            this.colSdate.Caption = "开始时间";
            this.colSdate.FieldName = "ShiftStartDate";
            this.colSdate.Name = "colSdate";
            this.colSdate.Visible = true;
            this.colSdate.VisibleIndex = 2;
            // 
            // colEdate
            // 
            this.colEdate.Caption = "截止时间";
            this.colEdate.FieldName = "ShiftEndDate";
            this.colEdate.Name = "colEdate";
            this.colEdate.Visible = true;
            this.colEdate.VisibleIndex = 3;
            // 
            // colTypeName
            // 
            this.colTypeName.Caption = "物理组别";
            this.colTypeName.FieldName = "TypeName";
            this.colTypeName.Name = "colTypeName";
            this.colTypeName.Visible = true;
            this.colTypeName.VisibleIndex = 4;
            // 
            // colTypePy
            // 
            this.colTypePy.Caption = "拼音简码";
            this.colTypePy.FieldName = "TypePy";
            this.colTypePy.Name = "colTypePy";
            // 
            // colTypeWb
            // 
            this.colTypeWb.Caption = "五笔简码";
            this.colTypeWb.FieldName = "WbCode";
            this.colTypeWb.Name = "colTypeWb";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.sysToolBar1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 901);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1168, 83);
            this.panelControl2.TabIndex = 2;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(3, 3);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1162, 77);
            this.sysToolBar1.TabIndex = 6;
            this.sysToolBar1.BtnSelectTemplateClick += new System.EventHandler(this.sysToolBar1_BtnSelectTemplateClick);
            // 
            // FrmDutySelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 984);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.splitContainerControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmDutySelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmDutyDict";
            this.Load += new System.EventHandler(this.FrmDutyDict_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDutys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDutyDict)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvdutys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtSearch;
        private DevExpress.XtraEditors.LabelControl lblSearch;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gcDutys;
        private DevExpress.XtraGrid.Views.Grid.GridView gvdutys;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraGrid.Columns.GridColumn colDID;
        private DevExpress.XtraGrid.Columns.GridColumn colDName;
        private DevExpress.XtraGrid.Columns.GridColumn colpy;
        private DevExpress.XtraGrid.Columns.GridColumn colwb;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
        private DevExpress.XtraGrid.Columns.GridColumn colSdate;
        private DevExpress.XtraGrid.Columns.GridColumn colEdate;
        private DevExpress.XtraGrid.Columns.GridColumn colTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn colTypePy;
        private DevExpress.XtraGrid.Columns.GridColumn colTypeWb;
        private System.Windows.Forms.BindingSource bsDutyDict;
    }
}