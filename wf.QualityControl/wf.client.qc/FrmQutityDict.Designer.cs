namespace dcl.client.qc
{
    partial class FrmQutityDict
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQutityDict));
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.navBar = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.FrmParameter = new DevExpress.XtraNavBar.NavBarItem();
            this.FrmCriterion = new DevExpress.XtraNavBar.NavBarItem();
            this.FrmDiversion = new DevExpress.XtraNavBar.NavBarItem();
            this.FrmQcRuleInst = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBar)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.groupControl1.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.groupControl1.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(239, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1138, 1028);
            this.groupControl1.TabIndex = 3;
            // 
            // navBar
            // 
            this.navBar.ActiveGroup = this.navBarGroup1;
            this.navBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.navBar.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBar.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.FrmParameter,
            this.FrmCriterion,
            this.FrmDiversion,
            this.FrmQcRuleInst});
            this.navBar.Location = new System.Drawing.Point(0, 0);
            this.navBar.Name = "navBar";
            this.navBar.OptionsNavPane.ExpandedWidth = 239;
            this.navBar.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.navBar.Size = new System.Drawing.Size(239, 1028);
            this.navBar.TabIndex = 0;
            this.navBar.Text = "质控参数";
            this.navBar.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "质控参数";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.FrmParameter),
            new DevExpress.XtraNavBar.NavBarItemLink(this.FrmCriterion),
            new DevExpress.XtraNavBar.NavBarItemLink(this.FrmDiversion),
            new DevExpress.XtraNavBar.NavBarItemLink(this.FrmQcRuleInst)});
            this.navBarGroup1.LargeImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup1.LargeImage")));
            this.navBarGroup1.Name = "navBarGroup1";
            this.navBarGroup1.SmallImage = ((System.Drawing.Image)(resources.GetObject("navBarGroup1.SmallImage")));
            // 
            // FrmParameter
            // 
            this.FrmParameter.Caption = "项目参数";
            this.FrmParameter.LargeImage = ((System.Drawing.Image)(resources.GetObject("FrmParameter.LargeImage")));
            this.FrmParameter.Name = "FrmParameter";
            this.FrmParameter.SmallImage = ((System.Drawing.Image)(resources.GetObject("FrmParameter.SmallImage")));
            this.FrmParameter.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Nav_LinkClick);
            // 
            // FrmCriterion
            // 
            this.FrmCriterion.Caption = "质控规则";
            this.FrmCriterion.LargeImage = ((System.Drawing.Image)(resources.GetObject("FrmCriterion.LargeImage")));
            this.FrmCriterion.Name = "FrmCriterion";
            this.FrmCriterion.SmallImage = ((System.Drawing.Image)(resources.GetObject("FrmCriterion.SmallImage")));
            this.FrmCriterion.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Nav_LinkClick);
            // 
            // FrmDiversion
            // 
            this.FrmDiversion.Caption = "半定量设定";
            this.FrmDiversion.LargeImage = ((System.Drawing.Image)(resources.GetObject("FrmDiversion.LargeImage")));
            this.FrmDiversion.Name = "FrmDiversion";
            this.FrmDiversion.SmallImage = ((System.Drawing.Image)(resources.GetObject("FrmDiversion.SmallImage")));
            this.FrmDiversion.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Nav_LinkClick);
            // 
            // FrmQcRuleInst
            // 
            this.FrmQcRuleInst.Caption = "质控通道";
            this.FrmQcRuleInst.LargeImage = ((System.Drawing.Image)(resources.GetObject("FrmQcRuleInst.LargeImage")));
            this.FrmQcRuleInst.Name = "FrmQcRuleInst";
            this.FrmQcRuleInst.SmallImage = ((System.Drawing.Image)(resources.GetObject("FrmQcRuleInst.SmallImage")));
            this.FrmQcRuleInst.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Nav_LinkClick);
            // 
            // FrmQutityDict
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 1028);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.navBar);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmQutityDict";
            this.Text = "FrmQutityDict";
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraNavBar.NavBarControl navBar;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem FrmParameter;
        private DevExpress.XtraNavBar.NavBarItem FrmCriterion;
        private DevExpress.XtraNavBar.NavBarItem FrmDiversion;
        private DevExpress.XtraNavBar.NavBarItem FrmQcRuleInst;
    }
}