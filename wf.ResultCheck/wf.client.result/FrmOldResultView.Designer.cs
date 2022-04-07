namespace dcl.client.result
{
    partial class FrmOldResultView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOldResultView));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_res_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_a = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bindingSource1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(676, 795);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(dcl.entity.EntityDicObrResultOriginal);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.OddRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.gridView1.Appearance.OddRow.Options.UseForeColor = true;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.Blue;
            this.gridView1.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_res_sid,
            this.col_itm_ecd,
            this.col_res_chr_a,
            this.col_res_date,
            this.col_itr_mid});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.ImmediateUpdateRowPosition = false;
            this.gridView1.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridView1.OptionsView.AllowCellMerge = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // col_res_sid
            // 
            this.col_res_sid.AppearanceCell.Options.UseTextOptions = true;
            this.col_res_sid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_res_sid.Caption = "样本号";
            this.col_res_sid.FieldName = "ObrSid";
            this.col_res_sid.Name = "col_res_sid";
            this.col_res_sid.OptionsColumn.AllowEdit = false;
            this.col_res_sid.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_sid.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_sid.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_sid.OptionsColumn.FixedWidth = true;
            this.col_res_sid.OptionsColumn.ReadOnly = true;
            this.col_res_sid.Width = 77;
            // 
            // col_itm_ecd
            // 
            this.col_itm_ecd.Caption = "项目代码";
            this.col_itm_ecd.FieldName = "ItmEcd";
            this.col_itm_ecd.Name = "col_itm_ecd";
            this.col_itm_ecd.OptionsColumn.AllowEdit = false;
            this.col_itm_ecd.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_itm_ecd.OptionsFilter.AllowAutoFilter = false;
            this.col_itm_ecd.OptionsFilter.AllowFilter = false;
            this.col_itm_ecd.Visible = true;
            this.col_itm_ecd.VisibleIndex = 0;
            // 
            // col_res_chr_a
            // 
            this.col_res_chr_a.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.col_res_chr_a.AppearanceCell.Options.UseFont = true;
            this.col_res_chr_a.Caption = "结果";
            this.col_res_chr_a.FieldName = "ObrValue";
            this.col_res_chr_a.Name = "col_res_chr_a";
            this.col_res_chr_a.OptionsColumn.AllowEdit = false;
            this.col_res_chr_a.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_a.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_a.OptionsColumn.FixedWidth = true;
            this.col_res_chr_a.OptionsColumn.ReadOnly = true;
            this.col_res_chr_a.OptionsFilter.AllowAutoFilter = false;
            this.col_res_chr_a.OptionsFilter.AllowFilter = false;
            this.col_res_chr_a.Visible = true;
            this.col_res_chr_a.VisibleIndex = 1;
            this.col_res_chr_a.Width = 90;
            // 
            // col_res_date
            // 
            this.col_res_date.Caption = "结果日期";
            this.col_res_date.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.col_res_date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.col_res_date.FieldName = "ObrDate";
            this.col_res_date.Name = "col_res_date";
            this.col_res_date.OptionsColumn.AllowEdit = false;
            this.col_res_date.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_date.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_date.OptionsColumn.FixedWidth = true;
            this.col_res_date.OptionsColumn.ReadOnly = true;
            this.col_res_date.OptionsFilter.AllowAutoFilter = false;
            this.col_res_date.OptionsFilter.AllowFilter = false;
            this.col_res_date.Visible = true;
            this.col_res_date.VisibleIndex = 2;
            this.col_res_date.Width = 132;
            // 
            // col_itr_mid
            // 
            this.col_itr_mid.Caption = "仪器代码";
            this.col_itr_mid.FieldName = "ObrItrId";
            this.col_itr_mid.Name = "col_itr_mid";
            this.col_itr_mid.OptionsColumn.AllowEdit = false;
            this.col_itr_mid.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_itr_mid.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_itr_mid.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_itr_mid.OptionsColumn.FixedWidth = true;
            this.col_itr_mid.OptionsColumn.ReadOnly = true;
            this.col_itr_mid.OptionsFilter.AllowAutoFilter = false;
            this.col_itr_mid.OptionsFilter.AllowFilter = false;
            this.col_itr_mid.Width = 87;
            // 
            // FrmOldResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 795);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOldResultView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查看复查原结果";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOldResultView_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_a;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_date;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_mid;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}