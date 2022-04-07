namespace dcl.client.tools
{
    partial class FrmSource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSource));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.dsLabBindingSource = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colpat_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_itr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_hostorder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_sex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_age = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_flag = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsLabBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.dsLabBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1625, 1156);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // dsLabBindingSource
            // 
            this.dsLabBindingSource.DataSource = typeof(dcl.entity.EntityPidReportMain);
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 23;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colpat_id,
            this.colpat_itr_id,
            this.colpat_sid,
            this.colpat_hostorder,
            this.colpat_date,
            this.colpat_name,
            this.colpat_sex,
            this.colpat_age,
            this.colpat_flag});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsDetail.AllowZoomDetail = false;
            this.gridView1.OptionsDetail.EnableMasterViewMode = false;
            this.gridView1.OptionsDetail.ShowDetailTabs = false;
            this.gridView1.OptionsDetail.SmartDetailExpand = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colpat_id
            // 
            this.colpat_id.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_id.AppearanceCell.Options.UseFont = true;
            this.colpat_id.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_id.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_id.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_id.AppearanceHeader.Options.UseFont = true;
            this.colpat_id.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_id.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_id.Caption = "标志ID";
            this.colpat_id.FieldName = "RepId";
            this.colpat_id.Name = "colpat_id";
            this.colpat_id.Visible = true;
            this.colpat_id.VisibleIndex = 7;
            this.colpat_id.Width = 118;
            // 
            // colpat_itr_id
            // 
            this.colpat_itr_id.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_itr_id.AppearanceCell.Options.UseFont = true;
            this.colpat_itr_id.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_itr_id.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_itr_id.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_itr_id.AppearanceHeader.Options.UseFont = true;
            this.colpat_itr_id.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_itr_id.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_itr_id.Caption = "仪器代码";
            this.colpat_itr_id.FieldName = "RepItrId";
            this.colpat_itr_id.Name = "colpat_itr_id";
            this.colpat_itr_id.Visible = true;
            this.colpat_itr_id.VisibleIndex = 0;
            // 
            // colpat_sid
            // 
            this.colpat_sid.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_sid.AppearanceCell.Options.UseFont = true;
            this.colpat_sid.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_sid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_sid.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_sid.AppearanceHeader.Options.UseFont = true;
            this.colpat_sid.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_sid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_sid.Caption = "样本号";
            this.colpat_sid.FieldName = "RepSid";
            this.colpat_sid.Name = "colpat_sid";
            this.colpat_sid.Visible = true;
            this.colpat_sid.VisibleIndex = 1;
            this.colpat_sid.Width = 121;
            // 
            // colpat_hostorder
            // 
            this.colpat_hostorder.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_hostorder.AppearanceCell.Options.UseFont = true;
            this.colpat_hostorder.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_hostorder.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_hostorder.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_hostorder.AppearanceHeader.Options.UseFont = true;
            this.colpat_hostorder.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_hostorder.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_hostorder.Caption = "序号";
            this.colpat_hostorder.FieldName = "RepSerialNum";
            this.colpat_hostorder.Name = "colpat_hostorder";
            this.colpat_hostorder.Visible = true;
            this.colpat_hostorder.VisibleIndex = 2;
            this.colpat_hostorder.Width = 47;
            // 
            // colpat_date
            // 
            this.colpat_date.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_date.AppearanceCell.Options.UseFont = true;
            this.colpat_date.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_date.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_date.AppearanceHeader.Options.UseFont = true;
            this.colpat_date.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_date.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_date.Caption = "日期";
            this.colpat_date.FieldName = "RepInDate";
            this.colpat_date.Name = "colpat_date";
            this.colpat_date.Visible = true;
            this.colpat_date.VisibleIndex = 3;
            this.colpat_date.Width = 92;
            // 
            // colpat_name
            // 
            this.colpat_name.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_name.AppearanceCell.Options.UseFont = true;
            this.colpat_name.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_name.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_name.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_name.AppearanceHeader.Options.UseFont = true;
            this.colpat_name.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_name.Caption = "姓名";
            this.colpat_name.FieldName = "PidName";
            this.colpat_name.Name = "colpat_name";
            this.colpat_name.Visible = true;
            this.colpat_name.VisibleIndex = 4;
            this.colpat_name.Width = 92;
            // 
            // colpat_sex
            // 
            this.colpat_sex.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_sex.AppearanceCell.Options.UseFont = true;
            this.colpat_sex.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_sex.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_sex.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_sex.AppearanceHeader.Options.UseFont = true;
            this.colpat_sex.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_sex.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_sex.Caption = "性别";
            this.colpat_sex.FieldName = "PidSexName";
            this.colpat_sex.Name = "colpat_sex";
            this.colpat_sex.Visible = true;
            this.colpat_sex.VisibleIndex = 5;
            this.colpat_sex.Width = 92;
            // 
            // colpat_age
            // 
            this.colpat_age.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_age.AppearanceCell.Options.UseFont = true;
            this.colpat_age.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_age.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_age.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_age.AppearanceHeader.Options.UseFont = true;
            this.colpat_age.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_age.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_age.Caption = "年龄";
            this.colpat_age.FieldName = "PidAgeExp";
            this.colpat_age.Name = "colpat_age";
            this.colpat_age.Visible = true;
            this.colpat_age.VisibleIndex = 6;
            this.colpat_age.Width = 92;
            // 
            // colpat_flag
            // 
            this.colpat_flag.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_flag.AppearanceCell.Options.UseFont = true;
            this.colpat_flag.AppearanceCell.Options.UseTextOptions = true;
            this.colpat_flag.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_flag.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colpat_flag.AppearanceHeader.Options.UseFont = true;
            this.colpat_flag.AppearanceHeader.Options.UseTextOptions = true;
            this.colpat_flag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colpat_flag.Caption = "诊断 ";
            this.colpat_flag.FieldName = "PidDiag";
            this.colpat_flag.Name = "colpat_flag";
            // 
            // FrmSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1625, 1156);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmSource";
            this.Text = "病人资料浏览";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsLabBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource dsLabBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_itr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_date;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_sex;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_age;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_flag;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_id;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_hostorder;
    }
}