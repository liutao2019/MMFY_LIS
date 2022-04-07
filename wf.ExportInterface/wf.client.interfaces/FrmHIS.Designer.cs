namespace dcl.client.interfaces
{
    partial class FrmHIS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHIS));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup3 = new DevExpress.XtraNavBar.NavBarGroup();
            this.barConHISInterfaces = new DevExpress.XtraNavBar.NavBarItem();
            this.barConContrastDefine = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.btnConnectionNew = new DevExpress.XtraNavBar.NavBarItem();
            this.btnCommandNew = new DevExpress.XtraNavBar.NavBarItem();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colanti_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colanti_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colanti_ename = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colanti_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colanti_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.navBarControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(891, 728);
            this.splitContainerControl1.SplitterPosition = 308;
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup3;
            this.navBarControl1.AllowGlyphSkinning = true;
            this.navBarControl1.Appearance.Background.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navBarControl1.Appearance.Background.Options.UseBackColor = true;
            this.navBarControl1.Appearance.GroupBackground.BackColor = System.Drawing.Color.Lavender;
            this.navBarControl1.Appearance.GroupBackground.Options.UseBackColor = true;
            this.navBarControl1.ContentButtonHint = null;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup3,
            this.navBarGroup1});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.barConContrastDefine,
            this.barConHISInterfaces,
            this.btnConnectionNew,
            this.btnCommandNew});
            this.navBarControl1.LargeImages = this.imageCollection1;
            this.navBarControl1.LinkSelectionMode = DevExpress.XtraNavBar.LinkSelectionModeType.OneInGroup;
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.NavigationPaneGroupClientHeight = 400;
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 308;
            this.navBarControl1.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.navBarControl1.Size = new System.Drawing.Size(308, 728);
            this.navBarControl1.TabIndex = 2;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();
            this.navBarControl1.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarControl1_LinkClicked);
            // 
            // navBarGroup3
            // 
            this.navBarGroup3.Caption = "系统接口";
            this.navBarGroup3.Expanded = true;
            this.navBarGroup3.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.barConHISInterfaces),
            new DevExpress.XtraNavBar.NavBarItemLink(this.barConContrastDefine)});
            this.navBarGroup3.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup3.LargeImage")));
            this.navBarGroup3.Name = "navBarGroup3";
            this.navBarGroup3.SelectedLinkIndex = 0;
            this.navBarGroup3.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup3.SmallImage")));
            // 
            // barConHISInterfaces
            // 
            this.barConHISInterfaces.Appearance.ForeColor = System.Drawing.Color.Black;
            this.barConHISInterfaces.Appearance.Options.UseForeColor = true;
            this.barConHISInterfaces.Caption = "定义接口";
            this.barConHISInterfaces.LargeImage = ((System.Drawing.Image)(resources.GetObject("barConHISInterfaces.LargeImage")));
            this.barConHISInterfaces.Name = "barConHISInterfaces";
            this.barConHISInterfaces.SmallImage = ((System.Drawing.Image)(resources.GetObject("barConHISInterfaces.SmallImage")));
            // 
            // barConContrastDefine
            // 
            this.barConContrastDefine.Appearance.ForeColor = System.Drawing.Color.Black;
            this.barConContrastDefine.Appearance.Options.UseForeColor = true;
            this.barConContrastDefine.Caption = "数据对照";
            this.barConContrastDefine.LargeImage = ((System.Drawing.Image)(resources.GetObject("barConContrastDefine.LargeImage")));
            this.barConContrastDefine.Name = "barConContrastDefine";
            this.barConContrastDefine.SmallImage = ((System.Drawing.Image)(resources.GetObject("barConContrastDefine.SmallImage")));
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "新接口";
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.btnConnectionNew),
            new DevExpress.XtraNavBar.NavBarItemLink(this.btnCommandNew)});
            this.navBarGroup1.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup1.LargeImage")));
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.SelectedLinkIndex = 0;
            this.navBarGroup1.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup1.SmallImage")));
            // 
            // btnConnectionNew
            // 
            this.btnConnectionNew.Caption = "连接参数";
            this.btnConnectionNew.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnConnectionNew.LargeImage")));
            this.btnConnectionNew.Name = "btnConnectionNew";
            this.btnConnectionNew.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnConnectionNew.SmallImage")));
            this.btnConnectionNew.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.btnConnectionNew_LinkClicked);
            // 
            // btnCommandNew
            // 
            this.btnCommandNew.Caption = "接口参数";
            this.btnCommandNew.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCommandNew.LargeImage")));
            this.btnCommandNew.Name = "btnCommandNew";
            this.btnCommandNew.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnCommandNew.SmallImage")));
            this.btnCommandNew.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.btnCommandNew_LinkClicked);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Blue;
            this.groupControl1.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(577, 728);
            this.groupControl1.TabIndex = 1;
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colanti_id,
            this.colanti_cname,
            this.colanti_ename,
            this.colanti_py,
            this.colanti_wb});
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowIndicator = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colanti_id, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colanti_id
            // 
            this.colanti_id.Caption = "编码";
            this.colanti_id.FieldName = "anti_id";
            this.colanti_id.Name = "colanti_id";
            this.colanti_id.Visible = true;
            this.colanti_id.VisibleIndex = 0;
            this.colanti_id.Width = 92;
            // 
            // colanti_cname
            // 
            this.colanti_cname.Caption = "中文名称";
            this.colanti_cname.FieldName = "anti_cname";
            this.colanti_cname.Name = "colanti_cname";
            this.colanti_cname.Visible = true;
            this.colanti_cname.VisibleIndex = 1;
            this.colanti_cname.Width = 125;
            // 
            // colanti_ename
            // 
            this.colanti_ename.Caption = "英文名称";
            this.colanti_ename.FieldName = "anti_ename";
            this.colanti_ename.Name = "colanti_ename";
            this.colanti_ename.Visible = true;
            this.colanti_ename.VisibleIndex = 2;
            this.colanti_ename.Width = 80;
            // 
            // colanti_py
            // 
            this.colanti_py.Caption = "拼音码";
            this.colanti_py.FieldName = "anti_py";
            this.colanti_py.Name = "colanti_py";
            this.colanti_py.Visible = true;
            this.colanti_py.VisibleIndex = 3;
            this.colanti_py.Width = 80;
            // 
            // colanti_wb
            // 
            this.colanti_wb.Caption = "五笔码";
            this.colanti_wb.FieldName = "anti_wb";
            this.colanti_wb.Name = "colanti_wb";
            this.colanti_wb.Visible = true;
            this.colanti_wb.VisibleIndex = 4;
            this.colanti_wb.Width = 86;
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "接口定义";
            this.navBarItem2.LargeImageIndex = 12;
            this.navBarItem2.Name = "navBarItem2";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(895, 732);
            this.panelControl1.TabIndex = 4;
            // 
            // FrmHIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(895, 732);
            this.Controls.Add(this.panelControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "FrmHIS";
            this.Text = "系统接口";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_id;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_cname;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_ename;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_py;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_wb;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup3;

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraNavBar.NavBarItem barConContrastDefine;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private DevExpress.XtraNavBar.NavBarItem barConHISInterfaces;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem btnConnectionNew;
        private DevExpress.XtraNavBar.NavBarItem btnCommandNew;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}