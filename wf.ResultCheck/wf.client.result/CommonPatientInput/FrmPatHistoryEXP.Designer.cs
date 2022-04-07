namespace dcl.client.result.CommonPatientInput
{
    partial class FrmPatHistoryEXP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPatHistoryEXP));
            this.gcHistoryExp = new DevExpress.XtraGrid.GridControl();
            this.gvHistoryExp = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtExp = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHistoryExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHistoryExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExp)).BeginInit();
            this.SuspendLayout();
            // 
            // gcHistoryExp
            // 
            this.gcHistoryExp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcHistoryExp.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcHistoryExp.Location = new System.Drawing.Point(0, 0);
            this.gcHistoryExp.MainView = this.gvHistoryExp;
            this.gcHistoryExp.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcHistoryExp.Name = "gcHistoryExp";
            this.gcHistoryExp.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txtExp});
            this.gcHistoryExp.Size = new System.Drawing.Size(728, 597);
            this.gcHistoryExp.TabIndex = 1;
            this.gcHistoryExp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHistoryExp});
            // 
            // gvHistoryExp
            // 
            this.gvHistoryExp.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2});
            this.gvHistoryExp.GridControl = this.gcHistoryExp;
            this.gvHistoryExp.Name = "gvHistoryExp";
            this.gvHistoryExp.OptionsBehavior.Editable = false;
            this.gvHistoryExp.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvHistoryExp.OptionsView.RowAutoHeight = true;
            this.gvHistoryExp.OptionsView.ShowDetailButtons = false;
            this.gvHistoryExp.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "时间";
            this.gridColumn1.FieldName = "RepInDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 65;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "项目";
            this.gridColumn3.FieldName = "PidComName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 69;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn2.Caption = "评价";
            this.gridColumn2.ColumnEdit = this.txtExp;
            this.gridColumn2.FieldName = "RepRemark";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 181;
            // 
            // txtExp
            // 
            this.txtExp.Name = "txtExp";
            // 
            // FrmPatHistoryEXP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 597);
            this.Controls.Add(this.gcHistoryExp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.TouchUIMode = DevExpress.Utils.DefaultBoolean.False;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPatHistoryEXP";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "历史评价";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPatHistoryEXP_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gcHistoryExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHistoryExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcHistoryExp;
        public DevExpress.XtraGrid.Views.Grid.GridView gvHistoryExp;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit txtExp;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}