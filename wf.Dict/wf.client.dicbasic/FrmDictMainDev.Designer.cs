namespace dcl.client.dicbasic
{
    partial class FrmDictMainDev
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDictMainDev));
            this.nBCMenus = new DevExpress.XtraNavBar.NavBarControl();
            this.gcChildFrm = new DevExpress.XtraEditors.GroupControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.plTop = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.nBCMenus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcChildFrm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.plTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nBCMenus
            // 
            this.nBCMenus.ActiveGroup = null;
            this.nBCMenus.Dock = System.Windows.Forms.DockStyle.Left;
            this.nBCMenus.DragDropFlags = DevExpress.XtraNavBar.NavBarDragDrop.None;
            this.nBCMenus.LinkSelectionMode = DevExpress.XtraNavBar.LinkSelectionModeType.OneInControl;
            this.nBCMenus.Location = new System.Drawing.Point(3, 3);
            this.nBCMenus.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.nBCMenus.Name = "nBCMenus";
            this.nBCMenus.OptionsNavPane.ExpandedWidth = 403;
            this.nBCMenus.OptionsNavPane.ShowOverflowButton = false;
            this.nBCMenus.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.nBCMenus.Size = new System.Drawing.Size(403, 749);
            this.nBCMenus.SkinExplorerBarViewScrollStyle = DevExpress.XtraNavBar.SkinExplorerBarViewScrollStyle.ScrollBar;
            this.nBCMenus.TabIndex = 5;
            this.nBCMenus.Text = "字典中心";
            this.nBCMenus.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();
            // 
            // gcChildFrm
            // 
            this.gcChildFrm.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.gcChildFrm.Appearance.Options.UseBackColor = true;
            this.gcChildFrm.AppearanceCaption.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.gcChildFrm.AppearanceCaption.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcChildFrm.AppearanceCaption.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.gcChildFrm.AppearanceCaption.Options.UseBackColor = true;
            this.gcChildFrm.AppearanceCaption.Options.UseFont = true;
            this.gcChildFrm.AppearanceCaption.Options.UseForeColor = true;
            this.gcChildFrm.AppearanceCaption.Options.UseTextOptions = true;
            this.gcChildFrm.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcChildFrm.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gcChildFrm.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.gcChildFrm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcChildFrm.Location = new System.Drawing.Point(406, 3);
            this.gcChildFrm.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gcChildFrm.Name = "gcChildFrm";
            this.gcChildFrm.Size = new System.Drawing.Size(1193, 749);
            this.gcChildFrm.TabIndex = 6;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "1-公用字典.png");
            this.imageCollection1.Images.SetKeyName(1, "2-仪器字典.png");
            this.imageCollection1.Images.SetKeyName(2, "3-标本字典.png");
            this.imageCollection1.Images.SetKeyName(3, "4-项目字典.png");
            this.imageCollection1.Images.SetKeyName(4, "5-细菌字典.png");
            this.imageCollection1.Images.SetKeyName(5, "6-条码字典.png");
            this.imageCollection1.Images.SetKeyName(6, "7-归档字典.png");
            this.imageCollection1.Images.SetKeyName(7, "8-温控字典.png");
            this.imageCollection1.Images.SetKeyName(8, "9-基础字典.png");
            this.imageCollection1.Images.SetKeyName(9, "10-院感字典.png");
            this.imageCollection1.Images.SetKeyName(10, "11-生物化学字典.png");
            this.imageCollection1.Images.SetKeyName(11, "12-标本培养基字典.png");
            this.imageCollection1.Images.SetKeyName(12, "13-质控字典.png");
            this.imageCollection1.Images.SetKeyName(13, "14-结核字典.png");
            this.imageCollection1.Images.SetKeyName(14, "15-存储字典.png");
            // 
            // plTop
            // 
            this.plTop.Controls.Add(this.sysToolBar1);
            this.plTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.plTop.Location = new System.Drawing.Point(0, 0);
            this.plTop.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.plTop.Name = "plTop";
            this.plTop.Size = new System.Drawing.Size(1602, 131);
            this.plTop.TabIndex = 7;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(8, 12, 8, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1602, 131);
            this.sysToolBar1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcChildFrm);
            this.panelControl1.Controls.Add(this.nBCMenus);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 131);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1602, 755);
            this.panelControl1.TabIndex = 8;
            // 
            // FrmDictMainDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1602, 886);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.plTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDictMainDev";
            this.Text = "字典中心--主窗体";
            ((System.ComponentModel.ISupportInitialize)(this.nBCMenus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcChildFrm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.plTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraNavBar.NavBarControl nBCMenus;
        private DevExpress.XtraEditors.GroupControl gcChildFrm;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel plTop;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}