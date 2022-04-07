namespace dcl.client.users
{
    partial class FrmCaSign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCaSign));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcom_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colctype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colptype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_his_fee_code = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gvMain;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1567, 1351);
            this.gridControl1.TabIndex = 51;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcom_id,
            this.colcom_code,
            this.colcom_name,
            this.colctype_name,
            this.colptype_name,
            this.colcom_his_fee_code});
            this.gvMain.GridControl = this.gridControl1;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            this.gvMain.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // colcom_id
            // 
            this.colcom_id.Caption = "使用时间";
            this.colcom_id.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colcom_id.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colcom_id.FieldName = "CaDate";
            this.colcom_id.Name = "colcom_id";
            this.colcom_id.Visible = true;
            this.colcom_id.VisibleIndex = 0;
            this.colcom_id.Width = 130;
            // 
            // colcom_code
            // 
            this.colcom_code.Caption = "KEY代码";
            this.colcom_code.FieldName = "CaCerId";
            this.colcom_code.Name = "colcom_code";
            this.colcom_code.Visible = true;
            this.colcom_code.VisibleIndex = 5;
            this.colcom_code.Width = 76;
            // 
            // colcom_name
            // 
            this.colcom_name.Caption = "操作人工号";
            this.colcom_name.FieldName = "CaLoginId";
            this.colcom_name.Name = "colcom_name";
            this.colcom_name.Visible = true;
            this.colcom_name.VisibleIndex = 1;
            this.colcom_name.Width = 89;
            // 
            // colctype_name
            // 
            this.colctype_name.AppearanceHeader.Options.UseTextOptions = true;
            this.colctype_name.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colctype_name.Caption = "操作人名称";
            this.colctype_name.FieldName = "CaName";
            this.colctype_name.Name = "colctype_name";
            this.colctype_name.Visible = true;
            this.colctype_name.VisibleIndex = 2;
            this.colctype_name.Width = 81;
            // 
            // colptype_name
            // 
            this.colptype_name.Caption = "事件";
            this.colptype_name.FieldName = "CaEvent";
            this.colptype_name.Name = "colptype_name";
            this.colptype_name.Visible = true;
            this.colptype_name.VisibleIndex = 3;
            this.colptype_name.Width = 87;
            // 
            // colcom_his_fee_code
            // 
            this.colcom_his_fee_code.Caption = "事件描述";
            this.colcom_his_fee_code.FieldName = "CaRemark";
            this.colcom_his_fee_code.Name = "colcom_his_fee_code";
            this.colcom_his_fee_code.OptionsColumn.AllowEdit = false;
            this.colcom_his_fee_code.Visible = true;
            this.colcom_his_fee_code.VisibleIndex = 4;
            this.colcom_his_fee_code.Width = 264;
            // 
            // FrmCaSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1567, 1351);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "FrmCaSign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "密钥使用情况";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_id;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_code;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_name;
        private DevExpress.XtraGrid.Columns.GridColumn colctype_name;
        private DevExpress.XtraGrid.Columns.GridColumn colptype_name;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_his_fee_code;

    }
}