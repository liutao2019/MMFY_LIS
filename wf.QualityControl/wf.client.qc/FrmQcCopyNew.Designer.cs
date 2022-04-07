namespace dcl.client.qc
{
    partial class FrmQcCopyNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQcCopyNew));
            this.sysCopy = new dcl.client.common.SysToolBar();
            this.dtEdate = new DevExpress.XtraEditors.DateEdit();
            this.txtLot = new DevExpress.XtraEditors.TextEdit();
            this.txtMark = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcLot = new DevExpress.XtraGrid.GridControl();
            this.gvCopy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lue_Ins = new dcl.client.control.SelectDicInstrument();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sysCopy
            // 
            this.sysCopy.AutoCloseButton = true;
            this.sysCopy.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysCopy.Location = new System.Drawing.Point(0, 0);
            this.sysCopy.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sysCopy.Name = "sysCopy";
            this.sysCopy.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysCopy.NotWriteLogButtonNameList")));
            this.sysCopy.ShowItemToolTips = false;
            this.sysCopy.Size = new System.Drawing.Size(782, 79);
            this.sysCopy.TabIndex = 0;
            this.sysCopy.BtnCopyClick += new System.EventHandler(this.sysCopy_BtnCopyClick);
            // 
            // dtEdate
            // 
            this.dtEdate.EditValue = null;
            this.dtEdate.EnterMoveNextControl = true;
            this.dtEdate.Location = new System.Drawing.Point(25, 274);
            this.dtEdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtEdate.Name = "dtEdate";
            this.dtEdate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.dtEdate.Properties.Appearance.Options.UseFont = true;
            this.dtEdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEdate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtEdate.Size = new System.Drawing.Size(191, 28);
            this.dtEdate.TabIndex = 4;
            // 
            // txtLot
            // 
            this.txtLot.EnterMoveNextControl = true;
            this.txtLot.Location = new System.Drawing.Point(25, 205);
            this.txtLot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLot.Name = "txtLot";
            this.txtLot.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtLot.Properties.Appearance.Options.UseFont = true;
            this.txtLot.Size = new System.Drawing.Size(191, 28);
            this.txtLot.TabIndex = 2;
            // 
            // txtMark
            // 
            this.txtMark.EnterMoveNextControl = true;
            this.txtMark.Location = new System.Drawing.Point(25, 130);
            this.txtMark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMark.Name = "txtMark";
            this.txtMark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtMark.Properties.Appearance.Options.UseFont = true;
            this.txtMark.Size = new System.Drawing.Size(191, 28);
            this.txtMark.TabIndex = 1;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl6.Location = new System.Drawing.Point(25, 31);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(54, 22);
            this.labelControl6.TabIndex = 8;
            this.labelControl6.Text = "仪器：";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl5.Location = new System.Drawing.Point(25, 101);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(54, 22);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "水平：";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl2.Location = new System.Drawing.Point(25, 173);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 22);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "批号：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcLot);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 79);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(548, 616);
            this.panelControl2.TabIndex = 1;
            // 
            // gcLot
            // 
            this.gcLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLot.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLot.Location = new System.Drawing.Point(2, 56);
            this.gcLot.MainView = this.gvCopy;
            this.gcLot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLot.Name = "gcLot";
            this.gcLot.Size = new System.Drawing.Size(544, 558);
            this.gcLot.TabIndex = 1;
            this.gcLot.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCopy});
            // 
            // gvCopy
            // 
            this.gvCopy.ColumnPanelRowHeight = 23;
            this.gvCopy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvCopy.GridControl = this.gcLot;
            this.gvCopy.Name = "gvCopy";
            this.gvCopy.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvCopy.OptionsView.ShowGroupPanel = false;
            this.gvCopy.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCopy_FocusedRowChanged);
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
            this.gridColumn1.Caption = "仪器";
            this.gridColumn1.FieldName = "ItrEname";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
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
            this.gridColumn2.Caption = "质控物标识";
            this.gridColumn2.FieldName = "MatLevel";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
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
            this.gridColumn3.Caption = "质控物批号";
            this.gridColumn3.FieldName = "MatBatchNo";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
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
            this.gridColumn4.Caption = "物理组";
            this.gridColumn4.FieldName = "ProName";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "有效日期";
            this.gridColumn5.FieldName = "MatDateEnd";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.labelControl1);
            this.panelControl3.Controls.Add(this.txtSearch);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(544, 54);
            this.panelControl3.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl1.Location = new System.Drawing.Point(46, 13);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "检索：";
            // 
            // txtSearch
            // 
            this.txtSearch.EnterMoveNextControl = true;
            this.txtSearch.Location = new System.Drawing.Point(102, 9);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtSearch.Properties.Appearance.Options.UseFont = true;
            this.txtSearch.Size = new System.Drawing.Size(297, 28);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.EditValueChanged += new System.EventHandler(this.txtSearch_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl4.Location = new System.Drawing.Point(25, 245);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 22);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "有效日期";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lue_Ins);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Controls.Add(this.labelControl6);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.txtMark);
            this.groupBox1.Controls.Add(this.labelControl5);
            this.groupBox1.Controls.Add(this.txtLot);
            this.groupBox1.Controls.Add(this.dtEdate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.groupBox1.Location = new System.Drawing.Point(548, 79);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(234, 616);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "复制到：";
            // 
            // lue_Ins
            // 
            this.lue_Ins.AddEmptyRow = true;
            this.lue_Ins.BindByValue = false;
            this.lue_Ins.colDisplay = "";
            this.lue_Ins.colExtend1 = null;
            this.lue_Ins.colInCode = "";
            this.lue_Ins.colPY = "";
            this.lue_Ins.colSeq = "";
            this.lue_Ins.colValue = "";
            this.lue_Ins.colWB = "";
            this.lue_Ins.displayMember = null;
            this.lue_Ins.EnterMoveNext = true;
            this.lue_Ins.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lue_Ins.KeyUpDownMoveNext = false;
            this.lue_Ins.LoadDataOnDesignMode = true;
            this.lue_Ins.Location = new System.Drawing.Point(25, 61);
            this.lue_Ins.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lue_Ins.MaximumSize = new System.Drawing.Size(571, 38);
            this.lue_Ins.MinimumSize = new System.Drawing.Size(57, 34);
            this.lue_Ins.Name = "lue_Ins";
            this.lue_Ins.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lue_Ins.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lue_Ins.Readonly = false;
            this.lue_Ins.SaveSourceID = false;
            this.lue_Ins.SelectFilter = null;
            this.lue_Ins.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lue_Ins.SelectOnly = true;
            this.lue_Ins.ShowAllInstrmt = false;
            this.lue_Ins.Size = new System.Drawing.Size(191, 34);
            this.lue_Ins.TabIndex = 11;
            this.lue_Ins.UseExtend = false;
            this.lue_Ins.valueMember = null;
            // 
            // FrmQcCopyNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 695);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sysCopy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQcCopyNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "质控物拷贝";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmQcCopy_FormClosed);
            this.Load += new System.EventHandler(this.FrmQcCopy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEdate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcLot;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCopy;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtSearch;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private dcl.client.common.SysToolBar sysCopy;
        private DevExpress.XtraEditors.TextEdit txtLot;
        private DevExpress.XtraEditors.TextEdit txtMark;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.DateEdit dtEdate;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private control.SelectDicInstrument lue_Ins;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}