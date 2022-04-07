using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.common;
using System.Diagnostics;
using lis.client.control;
using dcl.client.common;
using Lib.LogManager;
using DevExpress.XtraBars.Navigation;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using dcl.entity;
using System.IO;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using dcl.client.cache;

namespace wf.ClientEntrance
{
    public partial class FrmMainNew : FrmCommon
    {
        private MenuStrip mainMenu = new MenuStrip();
        public FrmLogin login;
        private System.Windows.Forms.Timer reductionTimer;

        Font maxFont = new System.Drawing.Font("微软雅黑", 22.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        Font minFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        UsageRecordsCache ur_cache = null;
        SynchronizationContext m_SyncContext;
        DevExpress.XtraEditors.TileGroup tileGroup2 = new DevExpress.XtraEditors.TileGroup();
        DevExpress.XtraEditors.TileItem ClearTiltleItem = null;

        public FrmMainNew()
        {
            InitializeComponent();

            this.KeyPreview = true;
            base.ShowSucessMessage = false;
            this.MouseDown += FrmMainNew_MouseDown;

            menuStrip.DoubleClick += MenuStrip_DoubleClick;
            menuStrip.MouseDown += MenuStrip_MouseDown;
            menuStrip.MouseMove += MenuStrip_MouseMove;

            this.titleMin.ButtonClick += TitleMin_Click;
            this.titleMax.ButtonClick += TitleMax_Click;
            this.titleClose.ButtonClick += TitleClose_Click;

            barManager1.StatusBar.Visible = false;
            mBtnLogo.ButtonClick += MBtnLogo_Click;

            #region Timer

            //reductionTimer = new System.Windows.Forms.Timer();
            //reductionTimer.Interval = 5000;
            //reductionTimer.Tick += ReductionTimer_Tick;
            //reductionTimer.Stop();

            #endregion
        }

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //panelDock.Visible = false;
            if (DesignMode)
                return;

            string style = UserInfo.GetUserConfigValue("MainStyle");
            if (style != "")
                UserLookAndFeel.Default.SetSkinStyle(style);
            else
                UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");
            barCurSkin.Caption = UserLookAndFeel.Default.SkinName;
            UserLookAndFeel.Default.StyleChanged += Default_StyleChanged;

            //有背景图的情况下背景色无效,必须用此方法设置
            SetBackColor();
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

            this.fUserName1.Caption = string.Format("当前用户：{0}    ", UserInfo.userName);
            DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
            this.fLoginTime1.Caption = string.Format("登录时间：{0}    ", dtitServer.ToString("yyyy-MM-dd HH:mm:ss"));
            this.fVersion1.Caption = String.IsNullOrEmpty(AssemblyTrademark) ? "" : AssemblyTrademark;
            string strApp = ConfigurationManager.AppSettings["wcfAddr"].ToString();
            this.fServer1.Caption = string.Format("    服务器:{0}     ", strApp.Split('/')[2]);

            //创建菜单项
            CreateMenu();

            CheckFunc();

            timer.Enabled = true;

            this.Text = this.Text; //+ "   当前用户：" + UserInfo.userName;

            //检查该用户是否用文档未阅读（参数控制）
            //CheckUnreadDocument();

            //护士用户登录判断是否打开危急消息客户端
            if (UserInfo.entityUserInfo.PowerUserRole != null && UserInfo.entityUserInfo.PowerUserRole.Count > 0)
            {
                if (UserInfo.entityUserInfo.PowerUserRole.Where(w => w.RoleId == "10021").Count() > 0)
                {
                    RunMessageClient();
                }
            }

            //***********************************************************************************//
            //判断用户密码是否是默认密码，如果是的话，提示用户修改密码
            if (UserInfo.password.ToString() == "12345")
            {
                MessageDialog.Show(string.Format("尊敬的用户:{0}，您当前的密码是默认密码，为了您的安全使用，\n请及时修改密码！", UserInfo.userName));
            }

            //***********************************************************************************//
            ////检验是否启动危急值内部提醒
            ////检验是否启动仪器危急值提醒
            ////检验是否启动组合TAT提醒
            ////检验是否启动条码TAT提醒
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_In_IsNotify") == "是" ||
                dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_Instrmt_IsNotify") == "是"
                || dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Combine_TAT_IsNotify") == "是"
                || dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCCombine_TAT_IsNotify") == "是"
                || dcl.client.common.LocalSetting.Current.Setting.IsQCNotify == "1")
            {
                dcl.client.notifyclient.UrgentNotifyHelper.Current.start(); //危急值内部提醒--启动
            }
            else
            {
                dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
            }

            dcl.client.notifyclient.LEDNotifyHelper.Current.start(); //LED内部通知--启动

            dcl.client.notifyclient.ReagentNotifyHelper.Current.start(); //LED内部通知--启动

            //if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Gobal_IsRemoveComLogo") == "是")
            //{
            // BackgroundImage = Properties.Resources.BackGround;
            //}

            //Thread cache = new Thread(CacheLabForm);
            //cache.Start();

            m_SyncContext = SynchronizationContext.Current;

            #region UsageRecordsCache 初始化

            InitUsageRecordsCache();
            
            #endregion
        }

        //默认的皮肤样式改变事件
        private void Default_StyleChanged(object sender, EventArgs e)
        {
            List<EntitySysParameter> updateList = new List<EntitySysParameter>();
            EntitySysParameter sysp = new EntitySysParameter();
            sysp.ParmId = 9;
            sysp.ParmCode = "MainStyle";
            sysp.ParmFieldValue = UserLookAndFeel.Default.SkinName;
            updateList.Add(sysp);
            bool updateFlag =  new ProxySystemConfig().Service.UpdateSysPara(updateList);

            if(updateFlag)
                barCurSkin.Caption = UserLookAndFeel.Default.SkinName;
        }


        #region 用户自定义菜单事件代码

        /// <summary>
        /// 用户自定义菜单事件
        /// </summary>
        /// <param name="menuText"></param>
        private void UserFunction(string menuText)
        {
            if (menuText.IndexOf(".") != -1)
            {
                menuText = menuText.Substring(menuText.IndexOf(".") + 1);
            }

            switch (menuText)
            {
                case "退出":
                    {
                        dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
                        dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.ReagentNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭
                        Application.Exit();
                        break;
                    }
                case "注销用户":
                    {
                        login.action = "reLogin";
                        dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
                        dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.ReagentNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭

                        Application.Exit();
                        Process process = new Process();
                        process.StartInfo.FileName = Application.ExecutablePath;
                        process.Start();
                        break;
                    }
                case "锁定系统":
                    {
                        this.WindowState = FormWindowState.Maximized;
                        login.action = "lock";
                        dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
                        dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.ReagentNotifyHelper.Current.stop(); //LED内部通知--关闭
                        dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭
                        this.Hide();
                        login.LockSystem();
                        login.Show();
                        break;
                    }
                case "层叠":
                    {
                        this.LayoutMdi(MdiLayout.Cascade);
                        break;
                    }
                case "垂直排列":
                    {
                        this.LayoutMdi(MdiLayout.TileVertical);
                        break;
                    }
                case "水平排列":
                    {
                        this.LayoutMdi(MdiLayout.TileHorizontal);
                        break;
                    }
                case "全部关闭":
                    {
                        foreach (System.Windows.Forms.Form form in this.MdiChildren)
                        {
                            form.Close();
                        }
                        break;
                    }
                case "刷新字典":
                    {
                        try
                        {
                            LocalSetting.Current.Refresh();
                            dcl.client.cache.ClientCacheManager.RefreshAll();
                            CacheClient.ClearCache();

                            lis.client.control.MessageDialog.ShowAutoCloseDialog("字典缓存已清空");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("刷新字典失败");
                        }

                        break;
                    }
                case "修改密码":
                    {
                        //虽然修改密码也对应一个窗体,但因为弹出模式不同,所以单独写
                        ShowSingleDialog("dcl.client.users.FrmChangePassword");
                        break;
                    }
                case "关于检验系统":
                    {
                        FrmAbout frm = new FrmAbout();
                        frm.ShowDialog(this);
                        break;
                    }
                case "WEB报告查询":
                    {
                        string strRequest = "web/frmreportselect.aspx";

                        //系统配置：WEB报告查询须身份验证
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("web_reportselect_AddLogin") == "是")
                        {
                            strRequest = "web/FrmReportSelectNw.aspx";
                        }
                        string wcfAddress = ConfigurationManager.AppSettings["wcfAddr"] + strRequest;
                        Process.Start("IEXPLORE.EXE", wcfAddress);
                        break;
                    }
                case "WEB危急报告登记查询":
                    {
                        if (string.IsNullOrEmpty(UserInfo.departId))
                        {
                            break;
                        }
                        string wcfAddress = ConfigurationManager.AppSettings["wcfAddr"] +
                                            "web/frmCriticalCheckReport.aspx?dep_id=" + UserInfo.departId;
                        Process.Start("IEXPLORE.EXE", wcfAddress);
                        break;
                    }
                case "打印设置":
                    {
                        ShowSingleDialog("dcl.client.report.FrmPrintConfiguration");
                        break;
                    }
                case "本地设置":
                    {
                        ShowSingleDialog("dcl.client.users.frmLocalSetting");
                        break;
                    }
                //case "WHONET导出":
                //    {
                //        Exec("Lis.Utils.Whonet.exe");
                //        break;
                //    }
                case "实验室管理":
                    {
                        try
                        {
                            Process.Start(@"F:\LISAPP\LQCS\client\LQCS.UI.Win.MainFrame.exe", "1 1");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        break;
                    }
                case "帮助":
                    {
                        Exec("帮助文档.doc");
                        break;
                    }
                case "资料导入":
                    {
                        ShowSingleDialog("dcl.client.tools.FrmImportLisResult");
                        return;
                    }
                case "数据重传":
                    {
                        ShowSingleDialog("dcl.client.tools.FrmLisDataReUpLoad");
                        return;
                    }
                default:
                    break;
            }
        }


        #endregion

        /// <summary>
        /// 弹出对话框窗体
        /// </summary>
        /// <param name="funcCode">窗体路径</param>
        private void ShowSingleDialog(string funcCode)
        {
            int index = funcCode.LastIndexOf(".");
            string nameSpaceName = funcCode.Substring(0, index);
            string className = funcCode.Substring(index + 1);
            string path = Application.StartupPath + "\\" + nameSpaceName + ".dll";
            Assembly assembly = Assembly.GetEntryAssembly();
            assembly = Assembly.LoadFrom(path);
            Type type = assembly.GetType(nameSpaceName + "." + className);
            ConstructorInfo conn = type.GetConstructor(new Type[0]);
            System.Windows.Forms.Form frm = (System.Windows.Forms.Form)conn.Invoke(new object[0]);
            frm.ShowDialog(this);
        }


        #region 菜单生成及事件控制


        /// <summary>
        /// 启动外部程序
        /// </summary>
        /// <param name="menuText"></param>
        private void Exec(string menuText)
        {
            if (string.IsNullOrEmpty(menuText) || !File.Exists(menuText))
                return;
            Process.Start(menuText);
        }

        /// <summary>
        /// 缓存文本为"窗口"的菜单
        /// </summary>
        private ToolStripMenuItemEnlarge menuWiondows = null;


        void CheckFunc()
        {
            //读取菜单
            List<EntitySysFunction> dtMenu = new List<EntitySysFunction>();
            if (UserInfo.isAdmin == true)
            {
                dtMenu = UserInfo.entityUserInfo.AllFunc;
            }
            else
            {
                dtMenu = UserInfo.entityUserInfo.Func;
            }     

        }



        /// <summary>
        /// 生成菜单
        /// </summary>
        private void CreateMenu()
        {
            new MeumBuilder().GenSysFuncMenu(menuStrip, StripItem_Click);

            foreach (ToolStripMenuItem menuItem in menuStrip.Items)
            {
                //menuItem.MouseEnter += toolStripMenuItem_MouseEnter;
                //menuItem.MouseHover += toolStripMenuItem_MouseHover;
                //menuItem.DropDownClosed += toolStripMenuItem_DropDownClosed;
                //menuItem.MouseMove += toolStripMenuItem_MouseMove;
            }
        }


        ToolStripMenuItem parent;
        private void toolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            reductionTimer.Stop();
            reductionTimer.Start();
            titleBar.Height = menuStrip.Height;
            ToolStripMenuItem Sender = sender as ToolStripMenuItem;
            if (parent == null)
                parent = Sender;

            if (parent.Name == Sender.Name)
                return;

            if (parent != null)
                parent.Font = minFont;

            parent = Sender;
        }

        private void toolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem Sender = sender as ToolStripMenuItem;
            Sender.Font = maxFont;
            //titleBar.Height = menuStrip.Height;
            foreach (ToolStripMenuItem SubMenu in Sender.DropDownItems)
            {
                SubMenu.Font = maxFont;
            }
        }

        private void toolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if (parent == null)
                return;

            parent.Font = minFont;
            titleBar.Height = menuStrip.Height;
            reductionTimer.Stop();
        }

        private void toolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem Sender = sender as ToolStripMenuItem;
            Sender.ShowDropDown();
        }


        private void StripItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ClickSender = sender as ToolStripMenuItem;
            if (ClickSender.Tag == null || string.IsNullOrEmpty(ClickSender.Name)) return;

            //将界面菜单点击操作加入缓存记录
            string[] strs = ClickSender.Text.Split('.');
            string str = strs.Length > 1 ? strs[1] : strs[0];
            ur_cache.AddCache(str,ClickSender.Name, ClickSender.Text, ClickSender.Tag.ToString(), UserInfo.userName
                );

            StripItemClick(ClickSender.Name, ClickSender.Tag.ToString(), ClickSender.Text);
        }

        private void StripItemClick(string SenderName,string SenderTag,string SenderText)
        {
            if (SenderTag == "自定义")
            {
                UserFunction(SenderText);
                return;
            }
            switch (SenderName)
            {
                case "eWebQuery":
                    {
                        string strRequest = "web/frmreportselect.aspx";
                        //系统配置：WEB报告查询须身份验证
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("web_reportselect_AddLogin") == "是")
                        {
                            strRequest = "web/FrmReportSelectNw.aspx";
                        }
                        string wcfAddress = ConfigurationManager.AppSettings["wcfAddr"] + strRequest;
                        Process.Start("IEXPLORE.EXE", wcfAddress);
                        return;
                    }
                case "acFrmPrintConfiguration":
                    {
                        ShowSingleDialog("dcl.client.report.FrmPrintConfiguration");
                        return;
                    }
                case "acfrmLocalSetting":
                    {
                        ShowSingleDialog("dcl.client.users.frmLocalSetting");
                        return;
                    }
               
            }
            LoadForm(SenderText, SenderTag);
        }

        /// <summary>
        /// 载入子窗体
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="funcCode"></param>
        public void LoadForm(string funcName, string funcCode)
        {
            try
            {
                if (string.IsNullOrEmpty(funcCode))
                    return;

                if (CheckExist(funcCode) == true)
                {
                    tileControl1.Visible = false;
                    return;
                }

                if (funcCode == "dcl.client.dicbasic.FrmDictMainDev" || ConvertToDataBaseName.isNeedChecked(funcCode))
                {
                    FrmCheckPassword fcp = new FrmCheckPassword();
                    if (fcp.ShowDialog() == DialogResult.OK)
                    {
                        if (fcp.OperatorID != UserInfo.loginID)
                        {
                            lis.client.control.MessageDialog.Show("非当前用户！", "提示");
                            return;
                        }
                        if (!splashScreenManager.IsSplashFormVisible)
                        {
                            splashScreenManager.ShowWaitForm();
                            splashScreenManager.SetWaitFormCaption("请稍候");
                            splashScreenManager.SetWaitFormDescription("正在加载中...");
                            Thread.Sleep(500);
                        }

                    }
                    else
                        return;
                }
                else
                {
                    if (!splashScreenManager.IsSplashFormVisible)
                    {
                        splashScreenManager.ShowWaitForm();
                        splashScreenManager.SetWaitFormCaption("请稍候");
                        splashScreenManager.SetWaitFormDescription("正在加载中...");
                    }
                }
                



                tileControl1.Visible = false;

                //带参数的窗体对象，使用 ; 分割窗体对象与参数组
                //如 dcl.client.result.FrmPatEnter;a&b&b
                //窗体对象为dcl.client.result.FrmPatEnter，参数组为a&b&b，分割参数后得到a b c三个参数，目前参数只支持字符串
                string[] funcCode_s = funcCode.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                funcCode = funcCode_s[0];

                //传递到窗体的参数，每个使用 & 符号分割
                string[] formParams = null;
                if (funcCode_s.Length > 1)
                {
                    formParams = funcCode_s[1].Split(new char[] { '&' }, StringSplitOptions.None);
                }

                int index = funcCode.LastIndexOf(".");
                string nameSpaceName = funcCode.Substring(0, index);
                string className = funcCode.Substring(index + 1);

                string path = Application.StartupPath + "\\" + nameSpaceName + ".dll";
                Assembly assembly = Assembly.GetEntryAssembly();
                assembly = Assembly.LoadFrom(path);
                Type type = assembly.GetType(nameSpaceName + "." + className);

                ConstructorInfo conn;
                if (formParams != null && formParams.Length > 0)
                {
                    //带参数的窗体
                    List<Type> ctorParamTypeList = new List<Type>();
                    for (int i = 0; i < formParams.Length; i++)
                    {
                        ctorParamTypeList.Add(typeof(string));
                    }
                    conn = type.GetConstructor(ctorParamTypeList.ToArray());
                }
                else
                {
                    //不带参数
                    conn = type.GetConstructor(new Type[0]);
                }

                System.Windows.Forms.Form frm;
                if (formParams != null && formParams.Length > 0)
                {
                    //带参数的窗体
                    frm = (System.Windows.Forms.Form)conn.Invoke(formParams);
                }
                else
                {
                    //不带参数
                    frm = (System.Windows.Forms.Form)conn.Invoke(new object[0]);
                }

                if (funcCode == "Lis.Client.DocumentManage.AboutFileForm"
                    || funcCode == "Lis.Client.DocumentManage.AboutStandardForm"
                    || funcCode == "Lis.Client.DocumentManage.AboutLawForm"
                    || funcCode == "Lis.Client.DocumentManage.AboutSuperiorfileForm"
                    || funcCode == "Lis.Client.DocumentManage.AboutHospitalfileForm")
                {
                    bool blnOperat = UserInfo.HaveFunction(funcCode, "OperatingAuthority");
                    conn = type.GetConstructor(new Type[2] { typeof(string), typeof(bool) });
                    frm = (System.Windows.Forms.Form)conn.Invoke(new object[2] { UserInfo.loginID, blnOperat });
                }
                else
                {
                    if (!CheckUnreadDocument())
                    {
                        return;
                    }
                }
                frm.Tag = funcCode;
                frm.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 60);
                frm.WindowState = FormWindowState.Maximized;

                if (funcName.IndexOf(".") != -1)
                {
                    funcName = funcName.Substring(funcName.IndexOf(".") + 1);
                }
                frm.Text = funcName;
                frm.MdiParent = this;
                frm.Activated += new EventHandler(frm_Activated);
                frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                frm.Show();
                xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
                this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;

                if (splashScreenManager.IsSplashFormVisible)
                    splashScreenManager.CloseWaitForm();
            }
            catch (Exception ex)
            {
                if(splashScreenManager.IsSplashFormVisible)
                    splashScreenManager.CloseWaitForm();
                Logger.LogException(ex);
                if (xtraTabbedMdiManager1.Pages.Count <= 0)
                {
                    tileControl1.Visible = true;
                }
                lis.client.control.MessageDialog.Show("模块加载失败", "提示");
            }
            finally
            {
            }
        }

        /// <summary>
        /// 子窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender != null && sender is System.Windows.Forms.Form)
            {
                System.Windows.Forms.Form form = sender as System.Windows.Forms.Form;
                string formTag = form.Tag.ToString();
                if (menuWiondows != null)
                {
                    for (int i = 0; i < menuWiondows.DropDownItems.Count; i++)
                    {
                        if (menuWiondows.DropDownItems[i] is ToolStripMenuItemEnlarge)
                        {
                            ToolStripMenuItemEnlarge thisMenu = (ToolStripMenuItemEnlarge)menuWiondows.DropDownItems[i];
                            if (thisMenu.Tag.ToString() == formTag)
                            {
                                menuWiondows.DropDownItems.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 子窗体激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void frm_Activated(object sender, EventArgs e)
        {
            System.Windows.Forms.Form form = sender as System.Windows.Forms.Form;

            SetWiondowsCheck(form.Text, form.Tag.ToString());
        }

        /// <summary>
        /// 窗口下的菜单激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowMenu_Click(object sender, EventArgs e)
        {
            string formTag = "";
            string formText = "";

            if (sender is ToolStripMenuItemEnlarge)
            {
                ToolStripMenuItemEnlarge menu = sender as ToolStripMenuItemEnlarge;
                formTag = menu.Tag.ToString();
                formText = menu.Text;
            }

            if (sender is ToolStripItem)
            {
                ToolStripItem menu = sender as ToolStripItem;
                formTag = menu.Tag.ToString();
                formText = menu.Text;
            }

            //设置选择效果
            SetWiondowsCheck(formText, formTag);

            CheckExist(formTag);
        }

        /// <summary>
        /// 检查子窗体是否存在,如果存在则激活
        /// </summary>
        /// <param name="formTag"></param>
        /// <returns></returns>
        private bool CheckExist(string formTag)
        {
            foreach (System.Windows.Forms.Form form in this.MdiChildren)
            {
                if (form.Tag.ToString() == formTag)
                {
                    form.Activate();
                    return true;
                }
            }
            return false;
        }


        private bool tabOpiton = false;
        /// <summary>
        /// 设置窗口菜单下的条目选择情况,新打开窗体时增加条目
        /// </summary>
        /// <param name="formTag"></param>
        private void SetWiondowsCheck(string formText, string formTag)
        {
            tabOpiton = true;
            bool checkValue = false;
            if (menuWiondows != null)
            {
                for (int i = 0; i < menuWiondows.DropDownItems.Count; i++)
                {
                    if (menuWiondows.DropDownItems[i] is ToolStripMenuItemEnlarge)
                    {
                        ToolStripMenuItemEnlarge thisMenu = (ToolStripMenuItemEnlarge)menuWiondows.DropDownItems[i];
                        if (thisMenu.Tag.ToString() == formTag)
                        {
                            thisMenu.Checked = true;
                            checkValue = true;
                        }
                        else
                        {
                            thisMenu.Checked = false;
                        }
                    }
                }
            }
            tabOpiton = false;
        }


        #endregion

        #region 窗体事件控制

        /// <summary>
        /// 关闭提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (login.action != "reLogin")
            {
                if (UserInfo.GetSysConfigValue("Enable_HandOver") == "是")
                {
                    if (new ProxyOfficeAnnouncement().Service.IsNeedShowHo(LocalSetting.Current.Setting.CType_id))
                    {
                        dcl.client.oa.FrmHandOverInfo info = new dcl.client.oa.FrmHandOverInfo();
                        info.ShowDialog();
                    }
                }
                if (login.action == "lockNoTip" || lis.client.control.MessageDialog.Show("您确定要退出实验室系统吗？", "确认", MessageBoxButtons.YesNo) !=
                    DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.DialogResult = DialogResult.No;
                }
                else
                {
                    dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
                    dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
                    dcl.client.notifyclient.ReagentNotifyHelper.Current.stop(); //LED内部通知--关闭
                    dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭

                    this.DialogResult = DialogResult.Yes;

                    //loglogut
                    LogLogout();

                    //保证关闭所有子线程
                    notifyIcon.Visible = false;
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }

        /// <summary>
        /// 退出日志
        /// </summary>
        private void LogLogout()
        {
            DataSet data;
            data = CommonClient.CreateDS(new String[] { "module", "loginID", "password", "ip", "mac", "action" }, "logout");
            DataTable dtAction = CommonClient.CreateDT(new String[] { "action" }, "action");
            dtAction.Rows.Add(new Object[] { "logout" });
            data.Tables.Add(dtAction);

            string ip = UserInfo.ip;
            string mac = UserInfo.mac;


            data.Tables["logout"].Rows.Add(new Object[] { "用户登录", UserInfo.loginID, string.Empty, ip, mac, "退出" });

            DataSet dsResult = this.doOther(data);
        }

        //外部调用刷新窗体方法
        public void LoadNewForm(object sender, EventArgs e)
        {
            FrmMain_Load(sender, e);
        }



        private void CacheLabForm()
        {
            string nameSpaceName = "dcl.client.result";
            string path = Application.StartupPath + "\\" + nameSpaceName + ".dll";
            Assembly assembly = Assembly.GetEntryAssembly();
            assembly = Assembly.LoadFrom(path);
            Type type = assembly.GetType(nameSpaceName + ".FrmPatEnterNew");
            ConstructorInfo conn = type.GetConstructor(new Type[0]);
            System.Windows.Forms.Form frm = (System.Windows.Forms.Form)conn.Invoke(new object[0]);
            frm.Visible = false;
            frm.Show();
            frm.Close();
        }


        /// <summary>
        /// 运行消息客户端
        /// </summary>
        public void RunMessageClient()
        {
            string msgclient_name = "dcl.client.msgclient.exe";

            string path = Application.StartupPath + "\\" + msgclient_name;
            if (System.IO.File.Exists(path))
            {
                bool isExistence = false;
                Process[] allProcess = Process.GetProcesses();

                List<Process> processRunning = new List<Process>();
                foreach (Process p in allProcess)
                {
                    if (p.ProcessName.ToLower() + ".exe" == msgclient_name.ToLower())
                    {
                        isExistence = true;
                        break;
                    }
                }
                if (!isExistence)
                {
                    Process.Start(path);
                }
            }
        }

        public string AssemblyTrademark
        {
            get
            {
                object[] attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }

        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor()
        {
            int iCnt = this.Controls.Count;
            for (int i = 0; i < iCnt; i++)
            {
                if (this.Controls[i].GetType().ToString() == "System.Windows.Forms.MdiClient")
                {
                    thisMdi = (System.Windows.Forms.MdiClient)this.Controls[i];
                    thisMdi.BackColor = Color.White;
                    thisMdi.BackgroundImage = this.BackgroundImage;
                    break;
                }
            }
        }

        private MdiClient thisMdi = null;

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (thisMdi != null)
            {
                thisMdi.Refresh();
            }
        }

        #endregion

        #region 消息提示


        private void timer_Tick(object sender, EventArgs e)
        {
            DoTimer();
        }

        /// <summary>
        /// 定时执行时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoTimer()
        {
            try
            {
                int span = int.Parse(UserInfo.GetSysConfigValue("MainTimer"));
                if (this.Visible)
                {
                    DataSet dsView = new DataSet();
                    DataTable dt = new DataTable("PowerUserInfo");
                    dt.Columns.Add("userInfoId");
                    DataRow dr = dt.NewRow();
                    dr["userInfoId"] = UserInfo.userInfoId;
                    dt.Rows.Add(dr);
                    dsView.Tables.Add(dt);

                    DataTable dtMessage = this.doView(dsView).Tables["SysMessage"];
                    string messageCount = dtMessage.Rows[0][0].ToString();
                    if (int.Parse(messageCount) > 0)
                    {
                        notifyIcon.Visible = true;
                        notifyIcon.BalloonTipText = "您有 " + messageCount + " 条未读消息";
                    }
                    else
                    {
                        notifyIcon.BalloonTipText = "";
                        notifyIcon.Visible = false;
                    }

                    //提示用户
                    if (notifyIcon.BalloonTipText != "")
                    {
                        notifyIcon.ShowBalloonTip(3000);

                    }
                }
                timer.Interval = span * 1000;
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                Logger.LogException(ex);
            }
        }

        #endregion

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            LoadForm("科室通知管理", "dcl.client.oa.FrmOfficeMessage");
        }

        /// <summary>
        /// 为了保险再次隐藏登录窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            login.Visible = false;
            bool unReadAnnouncement = ConfigHelper.GetSysConfigValueWithoutLogin("Check_UnReadAnnouncement") == "是";
            if (unReadAnnouncement || ConfigHelper.GetSysConfigValueWithoutLogin("Enable_OfficeMessageTip") == "是")
            {
                try
                {
                    mUpdateSessionWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
                    mUpdateSessionWorker.DoWork += m_UpdateSessionWorkerDoWork;
                    mUpdateSessionWorker.RunWorkerCompleted += m_UpdateSessionWorkerRunWorkerCompleted;
                    timerAnnuncement_Tick(null, null);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            //登陆检验系统的时候提醒报告超时信息

            #region 提醒报告超时信息

            //系统配置：登陆时提醒报告超时仪器IDs(ID1,ID2)
            string strLogin_OvertimeMessageForItrIDs = UserInfo.GetSysConfigValue("Login_OvertimeMessageForItrIDs");
            if (!string.IsNullOrEmpty(strLogin_OvertimeMessageForItrIDs))
            {
                string temp_itr_ids = "";
                if (!dcl.client.frame.UserInfo.isAdmin)
                {
                    #region 非admin
                    //如果为非admin,用户,则用有权限的仪器提醒
                    if (dcl.client.frame.UserInfo.UserItrs != null && dcl.client.frame.UserInfo.UserItrs.Length > 0)
                    {
                        foreach (string strTemp in dcl.client.frame.UserInfo.UserItrs)
                        {
                            if (strLogin_OvertimeMessageForItrIDs.Contains(strTemp))
                            {
                                if (string.IsNullOrEmpty(temp_itr_ids))
                                {
                                    temp_itr_ids = "'" + strTemp + "'";
                                }
                                else
                                {
                                    temp_itr_ids += ",'" + strTemp + "'";
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(temp_itr_ids))
                        {
                            dcl.client.notifyclient.OverTimeWarningHelper.Current.startCheckOverTime(DateTime.Now, temp_itr_ids);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region admin
                    foreach (string strTempSpl in strLogin_OvertimeMessageForItrIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(temp_itr_ids))
                        {
                            temp_itr_ids = "'" + strTempSpl + "'";
                        }
                        else
                        {
                            temp_itr_ids += ",'" + strTempSpl + "'";
                        }
                    }
                    //如果为admin,则全部仪器提醒
                    dcl.client.notifyclient.OverTimeWarningHelper.Current.startCheckOverTime(DateTime.Now, temp_itr_ids);
                    #endregion
                }
            }
            #endregion

            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
        }

        private void m_UpdateSessionWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerAnnuncement.Start();
        }

        private void m_UpdateSessionWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string userInfoId = e.Argument as string;

                ProxyOfficeAnnouncement proxy = new ProxyOfficeAnnouncement();
                int[] annCount = proxy.Service.GetUnReadAnnouncementCount(userInfoId);

                if (annCount[0] > 0)
                {
                    fAnnuncement.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    fAnnuncement.Caption = "您有 " + annCount[0] + " 条未读公告";
                }
                else
                {
                    fAnnuncement.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }

                if (annCount[1] > 0)
                {
                    fmessage.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    fmessage.Caption = "您有 " + annCount[1] + " 条未读通知";
                }
                else
                {
                    fmessage.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }

            }
            catch (Exception ex)
            {
                timerAnnuncement.Stop();
                fAnnuncement.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                fmessage.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                Logger.LogException(ex);
            }
        }


        private void fAnnuncement_Click(object sender, EventArgs e)
        {
            LoadForm("科室公告管理", "lis.client.office.FrmAnnuncementMgr");
        }

        private void fmessage_Click(object sender, EventArgs e)
        {
            LoadForm("科室通知管理", "lis.client.office.FrmOfficeMessage");
        }

        private bool CheckUnreadDocument()
        {
            //if (UserInfo.GetSysConfigValue("CheckUnreadDocument") == "是")
            //{
            //    string strUnreadDoc =
            //        Lis.Client.DocumentManage.OuterQuery.getInstance().GetUnreadDocTreeNode(UserInfo.loginID);
            //    if (!string.IsNullOrEmpty(strUnreadDoc))
            //    {
            //        lis.client.control.MessageDialog.Show(strUnreadDoc, "有未阅读文档，请打开菜单：");
            //        return false;
            //    }
            //}
            return true;
        }


        private BackgroundWorker mUpdateSessionWorker;

        private void timerAnnuncement_Tick(object sender, EventArgs e)
        {
            try
            {
                timerAnnuncement.Stop();

                if (mUpdateSessionWorker != null)
                {
                    mUpdateSessionWorker.RunWorkerAsync(UserInfo.userInfoId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void timerLockSystem_Tick(object sender, EventArgs e)
        {
            try
            {
                string configvalue = UserInfo.GetSysConfigValue("Sys_LockSystemInter");
                decimal minutes;
                if (string.IsNullOrEmpty(configvalue) || !decimal.TryParse(configvalue, out minutes))
                {
                    timerLockSystem.Enabled = false;
                    return;
                }
                if (LastInputChecker.GetLastInputTime() > minutes * 60 * 1000 && minutes > 0)
                {
                    this.WindowState = FormWindowState.Maximized;
                    login.action = "lock";
                    dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
                    dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
                    dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭
                    dcl.client.notifyclient.ReagentNotifyHelper.Current.stop();
                    this.Hide();
                    login.LockSystem();
                    login.Show();
                }
            }
            catch (Exception ex)
            {
                timerLockSystem.Enabled = false;
                Logger.LogException(ex);
            }


        }

        private void xtraTabbedMdiManager1_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager1.Pages.Count > 0)
            {
                tileControl1.Visible = false;
            }
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager1.Pages.Count <= 0)
            {
                tileControl1.Visible = true;
            }
        }

        //自动缩小菜单栏
        private void ReductionTimer_Tick(object sender, EventArgs e)
        {
            if (menuStrip.IsDropDown)
                return;

            if (menuStrip.Bounds.Contains(MousePosition.X, MousePosition.Y))
                return;

            parent.Font = minFont;
            titleBar.Height = menuStrip.Height;
            reductionTimer.Stop();

            ToolStripMenuItem Sender = parent as ToolStripMenuItem;
            Sender.HideDropDown();
        }


        private void MBtnLogo_Click(object sender, EventArgs e)
        {
            barManager1.StatusBar.Visible = !barManager1.StatusBar.Visible;
        }

        //最小化
        private void TitleMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //放大、缩小
        private void TitleMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                SetFormSize();
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        //关闭
        private void TitleClose_Click(object sender, EventArgs e)
        {
            dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
            dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
            dcl.client.notifyclient.ReagentNotifyHelper.Current.stop();
            dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭
            Application.Exit();
        }

        //设置主窗体的样式
        private void SetFormSize()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximumSize = new Size(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * 0.9));
            this.WindowState = FormWindowState.Normal;
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;//高度
            this.Location = new Point(Convert.ToInt32(0), Convert.ToInt32(yHeight * 0.05));
        }


        private Point myPoint;
        private void MenuStrip_MouseDown(object sender, MouseEventArgs e)
        {
            myPoint.X = e.X;
            myPoint.Y = e.Y;
        }

        private void MenuStrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point oriPoint = MousePosition;
                oriPoint.Offset(-myPoint.X, -myPoint.Y);
                Location = oriPoint;
            }
        }

        private void MenuStrip_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                SetFormSize();
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void barMeun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        //回到桌面
        private void brResize_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!tileControl1.Visible)
            {
                tileControl1.Visible = true;
            }
            else
            {
                if (xtraTabbedMdiManager1.Pages.Count > 0)
                {
                    tileControl1.Visible = false;
                }
            }
        }

        private void brClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dcl.client.notifyclient.UrgentNotifyHelper.Current.stop(); //危急值内部提醒--关闭
            dcl.client.notifyclient.LEDNotifyHelper.Current.stop(); //LED内部通知--关闭
            dcl.client.notifyclient.ReagentNotifyHelper.Current.stop();
            dcl.client.notifyclient.OverTimeWarningHelper.Current.stop();//报告超时提醒--关闭
            Application.Exit();
        }

        #region 窗体移动，窗体缩放

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private void FrmMainNew_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)(0x0084)://移动鼠标，按住或释放鼠标时发生 
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                    {
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    }
                    else if (vPoint.X >= ClientSize.Width - 5)
                    {
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    }
                    else if (vPoint.Y <= 5)
                    {
                        m.Result = (IntPtr)Guying_HTTOP;
                    }
                    else if (vPoint.Y >= ClientSize.Height - 5)
                    {
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region UsageRecordsCache相关代码
        /// <summary>
        /// 初始化UsageRecordsCache
        /// </summary>
        private void InitUsageRecordsCache()
        {
            ur_cache = new UsageRecordsCache();
            ur_cache.InitCache(true);
            string IsShowUsageRecordsCache = ConfigurationManager.AppSettings["IsShowUsageRecordsCache"];
            if(IsShowUsageRecordsCache?.Trim().ToLower()=="y")
            {
                ///根据配置信息设置是否将缓存信息显示在界面上
                UsageRecordsCache.LoadUsageRecord += LoadUsageRecord;
                ur_cache.LoadHistory += LoadUsageRecordsHistory;
            }
            
        }

        /// <summary>
        /// 添加UsageRecord界面Group
        /// </summary>
        /// <param name="record"></param>
        private void AddOperRecordGroup(UsageRecord record)
        {
            if (!this.tileControl1.Groups.Contains(tileGroup2))
            {
                //首次添加// 同时添加清除记录的Group
                this.tileControl1.Groups.Add(tileGroup2);
                tileControl1.RowCount = 2;
                tileControl1.ShowGroupText = true;
                tileGroup2.Text = "历史记录";
                DevExpress.XtraEditors.TileItemElement ClrarEle = new DevExpress.XtraEditors.TileItemElement();
                DevExpress.XtraEditors.TileItem ClearItm = new DevExpress.XtraEditors.TileItem();
                ClearItm.Elements.Add(ClrarEle);
                tileGroup2.Items.Add(ClearItm);
                ClrarEle.Text = "清除记录";
                ClrarEle.Appearance.Normal.ForeColor = Color.Red;
                ClrarEle.Appearance.Normal.FontSizeDelta = 18;
                ClrarEle.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
                ClearItm.Id = 999;
                ClearItm.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
                ClearItm.ItemClick += ClearItm_ItemClick;
                ClearTiltleItem = ClearItm;
            }
            if(tileGroup2.Items.Count == ur_cache.MaxCount+1)
            {
                //子项目数量已达到最大值，去除最开始加的一个数据，空出一个位置
                tileGroup2.Items.RemoveAt(tileGroup2.Items.Count-2);
            }
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();          
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();           
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();

            tileItemElement1.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.MaxWidth = 160;
            tileItemElement1.Text = record.EventModule;
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement1.TextLocation = new System.Drawing.Point(40, 0);


            tileItemElement2.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement2.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement2.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement2.MaxWidth = 500;
            tileItemElement2.Text = string.Format("操作时间:{0}", record.EventDate.ToString("yyyy-MM-dd HH:mm:ss"));
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement2.TextLocation = new System.Drawing.Point(40, 40);

            tileItemElement4.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement4.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement4.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI", 9F);
            tileItemElement4.MaxWidth = 170;
            tileItemElement4.Text = string.Format("操作者:{0}", record.EventDesc);
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement4.TextLocation = new System.Drawing.Point(40, 60);

            tileItemElement3.Image = ChangeColor(wf.ClientEntrance.Properties.Resources.检验查询, Color.Black);
            tileItemElement3.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement3.ImageLocation = new System.Drawing.Point(1, 5);
            tileItemElement3.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            tileItemElement3.ImageSize = new System.Drawing.Size(35, 35);


            DevExpress.XtraEditors.TileItem tileItem1 = new DevExpress.XtraEditors.TileItem();           
            tileItem1.Elements.Add(tileItemElement1);
            tileItem1.Elements.Add(tileItemElement2);
            tileItem1.Elements.Add(tileItemElement4);
            tileItem1.Elements.Add(tileItemElement3);
            tileItem1.Tag = record;
            tileItem1.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            tileItem1.ItemClick += UsageRecord_Click;
            tileGroup2.Items.Insert(0, tileItem1);

        }

        private Bitmap ChangeColor(Bitmap bmp, Color c)
        {
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    Color newColor = Color.FromArgb(pixel.A, (int)(pixel.R / 255.0 * c.R), (int)(pixel.G / 255.0 * c.G), (int)(pixel.B / 255.0 * c.B));
                    bmp.SetPixel(i, j, newColor);
                }
            return bmp;
        }

        /// <summary>
        /// UsageRecord界面Group点击处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsageRecord_Click(object sender, TileItemEventArgs e)
        {
            TileItem itm = sender as TileItem;
            UsageRecord record = itm.Tag as UsageRecord;
            ur_cache.AddCache(record.EventModule,record.SenderName,record.SenderText,record.SenderTag,UserInfo.userName);

            if (!HasResource(record.SenderTag))
            {
                lis.client.control.MessageDialog.Show("你没有权限使用此功能！", "提示");
                return;
            }

            StripItemClick(record.SenderName,record.SenderTag,record.SenderText);
        }

        public bool HasResource(string funcId)
        {
            if (string.IsNullOrEmpty(funcId))
                return false;

            if (UserInfo.HaveFunctionByCode(ConvertToDataBaseName.ConvertToDBName(funcId)))
                return true;
            return false;
        }

        /// <summary>
        /// 清除记录Group事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearItm_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            tileGroup2.Items.Clear();
            this.tileControl1.Groups.Remove(tileGroup2);
            ur_cache.ClearCache();
        }

        /// <summary>
        /// 加载UsageRecord历史记录
        /// </summary>
        /// <param name="res"></param>
        private void LoadUsageRecordsHistory(UsageRecords res)
        {
            if (res == null || res.Records?.Count <= ur_cache.MaxCount)
            {
                foreach (UsageRecord re in res.Records)
                {
                    Thread th = new Thread(new ParameterizedThreadStart(threadloadusagerecord));
                    th.IsBackground = true;
                    th.Start(re);
                }
            }
            else
            {
                for (int i = res.Records.Count - ur_cache.MaxCount; i < res.Records.Count; i++)
                {
                    Thread th = new Thread(new ParameterizedThreadStart(threadloadusagerecord));
                    th.IsBackground = true;
                    th.Start(res.Records[i]);
                }
            }
        }

        /// <summary>
        /// 加载UsageRecord缓存信息
        /// </summary>
        /// <param name="record"></param>
        private void LoadUsageRecord(UsageRecord record)
        {
            if (record == null)
            {
                return;
            }
            Thread th = new Thread(new ParameterizedThreadStart(threadloadusagerecord));
            th.IsBackground = true;
            th.Start(record);
            
        }

        private void threadloadusagerecord(object obj)
        {
            UsageRecord record = obj as UsageRecord;
            if (record == null)
                return;
            m_SyncContext?.Post(LoadUsageRecordGroup, record);
        }

        private void LoadUsageRecordGroup(object state)
        {
            UsageRecord record = state as UsageRecord;
            if (record == null)
                return;
            AddOperRecordGroup(record);
        }

        #endregion

    }

    #region ToolStripMenuItemEnlarge


    public partial class ToolStripMenuItemEnlarge : ToolStripMenuItem
    {
        private bool isValidateUser = false;

        public bool IsValidateUser
        {
            get { return isValidateUser; }
            set { isValidateUser = value; }
        }
    }

    public partial class ToolStripButtonEnlarge : ToolStripButton
    {
        private bool isValidateUser = false;

        public bool IsValidateUser
        {
            get { return isValidateUser; }
            set { isValidateUser = value; }
        }
    }


    #endregion
    
}
