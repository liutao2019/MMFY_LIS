namespace dcl.client.report
{
    partial class FrmSelectTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectTemplate));
            this.gcTemplate = new DevExpress.XtraGrid.GridControl();
            this.bsTpTemplate = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.gcTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTpTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcTemplate
            // 
            this.gcTemplate.DataSource = this.bsTpTemplate;
            this.gcTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTemplate.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcTemplate.Location = new System.Drawing.Point(0, 0);
            this.gcTemplate.MainView = this.gridView1;
            this.gcTemplate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcTemplate.Name = "gcTemplate";
            this.gcTemplate.Size = new System.Drawing.Size(815, 692);
            this.gcTemplate.TabIndex = 0;
            this.gcTemplate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsTpTemplate
            // 
            this.bsTpTemplate.DataSource = typeof(dcl.entity.EntityTpTemplate);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gridView1.GridControl = this.gcTemplate;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "模板名称";
            this.gridColumn1.FieldName = "StName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 692);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(815, 145);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            // 
            // FrmSelectTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 837);
            this.Controls.Add(this.gcTemplate);
            this.Controls.Add(this.sysToolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择模板(可双击选择)";
            this.Load += new System.EventHandler(this.FrmSelectTemplate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTpTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcTemplate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.BindingSource bsTpTemplate;
    }
}