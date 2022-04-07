namespace wf.ClientEntrance
{
    partial class FrmMainNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainNew));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            this.miFrmDictGroupAssortManage = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerAnnuncement = new System.Windows.Forms.Timer(this.components);
            this.timerLockSystem = new System.Windows.Forms.Timer(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.fUserName1 = new DevExpress.XtraBars.BarStaticItem();
            this.fLoginTime1 = new DevExpress.XtraBars.BarStaticItem();
            this.fVersion1 = new DevExpress.XtraBars.BarStaticItem();
            this.fServer1 = new DevExpress.XtraBars.BarStaticItem();
            this.barCurSkin = new DevExpress.XtraBars.BarStaticItem();
            this.skinBarSubItem = new DevExpress.XtraBars.SkinBarSubItem();
            this.barMeun = new DevExpress.XtraBars.BarLargeButtonItem();
            this.brResize = new DevExpress.XtraBars.BarLargeButtonItem();
            this.brClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.fAnnuncement = new DevExpress.XtraBars.BarStaticItem();
            this.fmessage = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.tileGroup6 = new DevExpress.XtraEditors.TileGroup();
            this.tileNavCategory1 = new DevExpress.XtraBars.Navigation.TileNavCategory();
            this.titleBar = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.检验申请ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标本流转ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检中管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检后管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.微生物管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实验管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.微生物实验ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.院感管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.mBtnLogo = new Lis.CustomControls.MetrolButtonEx();
            this.panel3 = new System.Windows.Forms.Panel();
            this.titleMin = new Lis.CustomControls.MetrolButtonEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.titleMax = new Lis.CustomControls.MetrolButtonEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.titleClose = new Lis.CustomControls.MetrolButtonEx();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::wf.ClientEntrance.CustomWaitForm), true, true);
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup4 = new DevExpress.XtraEditors.TileGroup();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.titleBar.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // miFrmDictGroupAssortManage
            // 
            this.miFrmDictGroupAssortManage.Name = "miFrmDictGroupAssortManage";
            this.miFrmDictGroupAssortManage.Size = new System.Drawing.Size(32, 19);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipTitle = "提示";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // timerAnnuncement
            // 
            this.timerAnnuncement.Enabled = true;
            this.timerAnnuncement.Interval = 10000;
            this.timerAnnuncement.Tick += new System.EventHandler(this.timerAnnuncement_Tick);
            // 
            // timerLockSystem
            // 
            this.timerLockSystem.Enabled = true;
            this.timerLockSystem.Interval = 60000;
            this.timerLockSystem.Tick += new System.EventHandler(this.timerLockSystem_Tick);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.fUserName1,
            this.fLoginTime1,
            this.fVersion1,
            this.fServer1,
            this.skinBarSubItem,
            this.barStaticItem2,
            this.barMeun,
            this.brResize,
            this.brClose,
            this.fAnnuncement,
            this.fmessage,
            this.barCurSkin});
            this.barManager1.MaxItemId = 13;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.fUserName1),
            new DevExpress.XtraBars.LinkPersistInfo(this.fLoginTime1),
            new DevExpress.XtraBars.LinkPersistInfo(this.fVersion1),
            new DevExpress.XtraBars.LinkPersistInfo(this.fServer1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCurSkin),
            new DevExpress.XtraBars.LinkPersistInfo(this.skinBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.barMeun),
            new DevExpress.XtraBars.LinkPersistInfo(this.brResize),
            new DevExpress.XtraBars.LinkPersistInfo(this.brClose),
            new DevExpress.XtraBars.LinkPersistInfo(this.fAnnuncement),
            new DevExpress.XtraBars.LinkPersistInfo(this.fmessage)});
            this.bar3.OptionsBar.AllowCollapse = true;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DisableCustomization = true;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // fUserName1
            // 
            this.fUserName1.Caption = "用户名";
            this.fUserName1.Id = 0;
            this.fUserName1.Name = "fUserName1";
            this.fUserName1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // fLoginTime1
            // 
            this.fLoginTime1.Caption = "登陆时间";
            this.fLoginTime1.Id = 1;
            this.fLoginTime1.Name = "fLoginTime1";
            this.fLoginTime1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // fVersion1
            // 
            this.fVersion1.Caption = "版本号";
            this.fVersion1.Id = 2;
            this.fVersion1.Name = "fVersion1";
            this.fVersion1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // fServer1
            // 
            this.fServer1.Caption = "中间层服务器:";
            this.fServer1.Id = 3;
            this.fServer1.Name = "fServer1";
            this.fServer1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barCurSkin
            // 
            this.barCurSkin.Caption = "当前主题";
            this.barCurSkin.Id = 12;
            this.barCurSkin.Name = "barCurSkin";
            this.barCurSkin.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // skinBarSubItem
            // 
            this.skinBarSubItem.Caption = "皮肤";
            this.skinBarSubItem.Id = 5;
            this.skinBarSubItem.Name = "skinBarSubItem";
            // 
            // barMeun
            // 
            this.barMeun.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barMeun.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.barMeun.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            this.barMeun.Id = 7;
            this.barMeun.LargeGlyph = global::wf.ClientEntrance.Properties.Resources.主菜单;
            this.barMeun.Name = "barMeun";
            toolTipTitleItem1.Text = "主菜单";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.barMeun.SuperTip = superToolTip1;
            this.barMeun.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barMeun.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMeun_ItemClick);
            // 
            // brResize
            // 
            this.brResize.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.brResize.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.brResize.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Left;
            this.brResize.Glyph = ((System.Drawing.Image)(resources.GetObject("brResize.Glyph")));
            this.brResize.Id = 8;
            this.brResize.Name = "brResize";
            this.brResize.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brResize_ItemClick);
            // 
            // brClose
            // 
            this.brClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.brClose.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.brClose.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Left;
            this.brClose.Id = 9;
            this.brClose.LargeGlyph = global::wf.ClientEntrance.Properties.Resources.关闭;
            this.brClose.Name = "brClose";
            this.brClose.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.brClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.brClose_ItemClick);
            // 
            // fAnnuncement
            // 
            this.fAnnuncement.Caption = "公告";
            this.fAnnuncement.Description = "您有0条未读公告";
            this.fAnnuncement.Id = 10;
            this.fAnnuncement.Name = "fAnnuncement";
            this.fAnnuncement.TextAlignment = System.Drawing.StringAlignment.Near;
            this.fAnnuncement.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.fAnnuncement.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.fAnnuncement_Click);
            // 
            // fmessage
            // 
            this.fmessage.Caption = "通知";
            this.fmessage.Description = "您有0条未读通知";
            this.fmessage.Id = 11;
            this.fmessage.Name = "fmessage";
            this.fmessage.TextAlignment = System.Drawing.StringAlignment.Near;
            this.fmessage.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.fmessage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.fmessage_Click);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1018, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 491);
            this.barDockControlBottom.Size = new System.Drawing.Size(1018, 29);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 491);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1018, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 491);
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.barStaticItem2.Caption = "菜单";
            this.barStaticItem2.Id = 6;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            this.xtraTabbedMdiManager1.PageAdded += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageAdded);
            this.xtraTabbedMdiManager1.PageRemoved += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageRemoved);
            // 
            // tileGroup6
            // 
            this.tileGroup6.Name = "tileGroup6";
            // 
            // tileNavCategory1
            // 
            this.tileNavCategory1.Name = "tileNavCategory1";
            this.tileNavCategory1.OptionsDropDown.BackColor = System.Drawing.Color.Empty;
            this.tileNavCategory1.OwnerCollection = null;
            // 
            // 
            // 
            this.tileNavCategory1.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileNavCategory1.Tile.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Default;
            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.MediumTurquoise;
            this.titleBar.Controls.Add(this.menuStrip);
            this.titleBar.Controls.Add(this.panel4);
            this.titleBar.Controls.Add(this.panel3);
            this.titleBar.Controls.Add(this.panel2);
            this.titleBar.Controls.Add(this.panel1);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(1018, 27);
            this.titleBar.TabIndex = 19;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.检中管理ToolStripMenuItem,
            this.检后管理ToolStripMenuItem,
            this.微生物管理ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(35, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(878, 29);
            this.menuStrip.TabIndex = 10;
            this.menuStrip.Text = "menuStrip2";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检验申请ToolStripMenuItem,
            this.标本流转ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(86, 25);
            this.toolStripMenuItem1.Tag = "";
            this.toolStripMenuItem1.Text = "检前管理";
            // 
            // 检验申请ToolStripMenuItem
            // 
            this.检验申请ToolStripMenuItem.Name = "检验申请ToolStripMenuItem";
            this.检验申请ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.检验申请ToolStripMenuItem.Text = "检验申请";
            // 
            // 标本流转ToolStripMenuItem
            // 
            this.标本流转ToolStripMenuItem.Name = "标本流转ToolStripMenuItem";
            this.标本流转ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.标本流转ToolStripMenuItem.Text = "标本流转";
            // 
            // 检中管理ToolStripMenuItem
            // 
            this.检中管理ToolStripMenuItem.Name = "检中管理ToolStripMenuItem";
            this.检中管理ToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.检中管理ToolStripMenuItem.Text = "检中管理";
            // 
            // 检后管理ToolStripMenuItem
            // 
            this.检后管理ToolStripMenuItem.Name = "检后管理ToolStripMenuItem";
            this.检后管理ToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.检后管理ToolStripMenuItem.Text = "检后管理";
            // 
            // 微生物管理ToolStripMenuItem
            // 
            this.微生物管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.实验管理ToolStripMenuItem});
            this.微生物管理ToolStripMenuItem.Name = "微生物管理ToolStripMenuItem";
            this.微生物管理ToolStripMenuItem.Size = new System.Drawing.Size(102, 25);
            this.微生物管理ToolStripMenuItem.Text = "微生物管理";
            // 
            // 实验管理ToolStripMenuItem
            // 
            this.实验管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.微生物实验ToolStripMenuItem,
            this.院感管理ToolStripMenuItem});
            this.实验管理ToolStripMenuItem.Name = "实验管理ToolStripMenuItem";
            this.实验管理ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.实验管理ToolStripMenuItem.Text = "实验管理";
            // 
            // 微生物实验ToolStripMenuItem
            // 
            this.微生物实验ToolStripMenuItem.Name = "微生物实验ToolStripMenuItem";
            this.微生物实验ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.微生物实验ToolStripMenuItem.Text = "微生物实验";
            // 
            // 院感管理ToolStripMenuItem
            // 
            this.院感管理ToolStripMenuItem.Name = "院感管理ToolStripMenuItem";
            this.院感管理ToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.院感管理ToolStripMenuItem.Text = "院感管理";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.mBtnLogo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(35, 27);
            this.panel4.TabIndex = 9;
            // 
            // mBtnLogo
            // 
            this.mBtnLogo.AutoSize = true;
            this.mBtnLogo.BackColor = System.Drawing.Color.Transparent;
            this.mBtnLogo.BackColorLeave = System.Drawing.Color.Transparent;
            this.mBtnLogo.BackColorM = System.Drawing.Color.Transparent;
            this.mBtnLogo.BackColorMove = System.Drawing.Color.Transparent;
            this.mBtnLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mBtnLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mBtnLogo.FontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mBtnLogo.ImageLeave = global::wf.ClientEntrance.Properties.Resources.mainlogo;
            this.mBtnLogo.ImageM = global::wf.ClientEntrance.Properties.Resources.mainlogo;
            this.mBtnLogo.ImageMove = global::wf.ClientEntrance.Properties.Resources.mainlogo_;
            this.mBtnLogo.Location = new System.Drawing.Point(0, 0);
            this.mBtnLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mBtnLogo.Name = "mBtnLogo";
            this.mBtnLogo.Size = new System.Drawing.Size(35, 27);
            this.mBtnLogo.TabIndex = 1;
            this.mBtnLogo.TextColor = System.Drawing.Color.Black;
            this.mBtnLogo.TextM = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.titleMin);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(913, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(35, 27);
            this.panel3.TabIndex = 8;
            // 
            // titleMin
            // 
            this.titleMin.AutoSize = true;
            this.titleMin.BackColor = System.Drawing.Color.Transparent;
            this.titleMin.BackColorLeave = System.Drawing.Color.Transparent;
            this.titleMin.BackColorM = System.Drawing.Color.Transparent;
            this.titleMin.BackColorMove = System.Drawing.Color.Transparent;
            this.titleMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.titleMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleMin.FontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleMin.ImageLeave = global::wf.ClientEntrance.Properties.Resources.title_bar_min1;
            this.titleMin.ImageM = global::wf.ClientEntrance.Properties.Resources.title_bar_min1;
            this.titleMin.ImageMove = global::wf.ClientEntrance.Properties.Resources.title_bar_min2;
            this.titleMin.Location = new System.Drawing.Point(0, 0);
            this.titleMin.Margin = new System.Windows.Forms.Padding(4);
            this.titleMin.Name = "titleMin";
            this.titleMin.Size = new System.Drawing.Size(35, 27);
            this.titleMin.TabIndex = 0;
            this.titleMin.TextColor = System.Drawing.Color.Black;
            this.titleMin.TextM = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.titleMax);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(948, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(35, 27);
            this.panel2.TabIndex = 7;
            // 
            // titleMax
            // 
            this.titleMax.AutoSize = true;
            this.titleMax.BackColor = System.Drawing.Color.Transparent;
            this.titleMax.BackColorLeave = System.Drawing.Color.Transparent;
            this.titleMax.BackColorM = System.Drawing.Color.Transparent;
            this.titleMax.BackColorMove = System.Drawing.Color.Transparent;
            this.titleMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.titleMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleMax.FontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleMax.ImageLeave = global::wf.ClientEntrance.Properties.Resources.title_bar_max3;
            this.titleMax.ImageM = global::wf.ClientEntrance.Properties.Resources.title_bar_max3;
            this.titleMax.ImageMove = global::wf.ClientEntrance.Properties.Resources.title_bar_max4;
            this.titleMax.Location = new System.Drawing.Point(0, 0);
            this.titleMax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleMax.Name = "titleMax";
            this.titleMax.Size = new System.Drawing.Size(35, 27);
            this.titleMax.TabIndex = 1;
            this.titleMax.TextColor = System.Drawing.Color.Black;
            this.titleMax.TextM = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.titleClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(983, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(35, 27);
            this.panel1.TabIndex = 6;
            // 
            // titleClose
            // 
            this.titleClose.AutoSize = true;
            this.titleClose.BackColor = System.Drawing.Color.Transparent;
            this.titleClose.BackColorLeave = System.Drawing.Color.Transparent;
            this.titleClose.BackColorM = System.Drawing.Color.Transparent;
            this.titleClose.BackColorMove = System.Drawing.Color.Transparent;
            this.titleClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.titleClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleClose.FontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleClose.ImageLeave = global::wf.ClientEntrance.Properties.Resources.title_bar_close1;
            this.titleClose.ImageM = global::wf.ClientEntrance.Properties.Resources.title_bar_close1;
            this.titleClose.ImageMove = global::wf.ClientEntrance.Properties.Resources.title_bar_close2;
            this.titleClose.Location = new System.Drawing.Point(0, 0);
            this.titleClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleClose.Name = "titleClose";
            this.titleClose.Size = new System.Drawing.Size(35, 27);
            this.titleClose.TabIndex = 1;
            this.titleClose.TextColor = System.Drawing.Color.Black;
            this.titleClose.TextM = "";
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // tileControl1
            // 
            this.tileControl1.AppearanceItem.Normal.BackColor = System.Drawing.Color.Transparent;
            this.tileControl1.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.Transparent;
            this.tileControl1.AppearanceItem.Normal.BorderColor = System.Drawing.Color.Transparent;
            this.tileControl1.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Black;
            this.tileControl1.AppearanceItem.Normal.Image = global::wf.ClientEntrance.Properties.Resources.TileItemBackImg;
            this.tileControl1.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseBorderColor = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseForeColor = true;
            this.tileControl1.AppearanceItem.Normal.Options.UseImage = true;
            this.tileControl1.AppearanceItem.Pressed.BackColor = System.Drawing.Color.Gray;
            this.tileControl1.AppearanceItem.Pressed.Options.UseBackColor = true;
            this.tileControl1.BackColor = System.Drawing.Color.White;
            this.tileControl1.BackgroundImage = global::wf.ClientEntrance.Properties.Resources.临床实验室底图;
            this.tileControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tileControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControl1.DragSize = new System.Drawing.Size(0, 0);
            this.tileControl1.IndentBetweenGroups = 20;
            this.tileControl1.ItemSize = 110;
            this.tileControl1.Location = new System.Drawing.Point(0, 27);
            this.tileControl1.MaxId = 37;
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.Padding = new System.Windows.Forms.Padding(0);
            this.tileControl1.Size = new System.Drawing.Size(1018, 464);
            this.tileControl1.TabIndex = 24;
            this.tileControl1.Text = "tileControl1";
            // 
            // tileGroup4
            // 
            this.tileGroup4.Name = "tileGroup4";
            // 
            // FrmMainNew
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 520);
            this.Controls.Add(this.tileControl1);
            this.Controls.Add(this.titleBar);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.IsMdiContainer = true;
            this.Name = "FrmMainNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检验系统集成平台";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.titleBar.ResumeLayout(false);
            this.titleBar.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem miFrmDictGroupAssortManage;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timerAnnuncement;
        private System.Windows.Forms.Timer timerLockSystem;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraEditors.TileGroup tileGroup6;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarStaticItem fUserName1;
        private DevExpress.XtraBars.BarStaticItem fLoginTime1;
        private DevExpress.XtraBars.BarStaticItem fVersion1;
        private DevExpress.XtraBars.BarStaticItem fServer1;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarLargeButtonItem barMeun;
        private DevExpress.XtraBars.BarLargeButtonItem brResize;
        private DevExpress.XtraBars.BarLargeButtonItem brClose;
        private DevExpress.XtraBars.BarStaticItem fAnnuncement;
        private DevExpress.XtraBars.BarStaticItem fmessage;
        private DevExpress.XtraBars.Navigation.TileNavCategory tileNavCategory1;
        public System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private Lis.CustomControls.MetrolButtonEx mBtnLogo;
        private Lis.CustomControls.MetrolButtonEx titleMin;
        private Lis.CustomControls.MetrolButtonEx titleMax;
        private Lis.CustomControls.MetrolButtonEx titleClose;
        public System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 检验申请ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标本流转ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检中管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检后管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 微生物管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实验管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 微生物实验ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 院感管理ToolStripMenuItem;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraBars.BarStaticItem barCurSkin;
        private DevExpress.XtraEditors.TileGroup tileGroup4;
    }
}
