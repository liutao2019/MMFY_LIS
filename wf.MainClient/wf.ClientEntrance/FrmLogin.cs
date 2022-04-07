using dcl.client.ca;
using dcl.client.cache;
using dcl.client.common;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.common;
using dcl.entity;
using DevExpress.XtraReports.UI;
using Lib.LogManager;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
//using BJCASECCOMLib;

namespace wf.ClientEntrance
{
    public partial class FrmLogin : Form
    {
        public String action = "login";//reLogin,lock

        //lis系统目录,存放不可更改文件
        //string lisPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\";
        //string lisConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\config";
        //string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport";
        //string xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml";

        ProxyLogin proxy = new ProxyLogin();
        /// <summary>
        /// 是否允许移动
        /// </summary>

        string lisPath = PathManager.SettingLisPath;
        string lisConfigPath = PathManager.SettingLisPath + @"\config";
        string localPath = PathManager.ReportPath;
        string xmlFile = PathManager.SettingLisPath + @"\printXml";

        /// <summary>
        /// 是否CA电子签名验证模式
        /// </summary>
        public string strCASignMode { get; set; }

        /// <summary>
        /// 是否登录前插入了CA的Ukey
        /// </summary>
        private bool IsExistsCaUKey { get; set; }

        /// <summary>
        /// 登录时是否使用CA电子验证
        /// </summary>
        private bool IsCASignByLogin { get; set; }

        /// <summary>
        /// CA电子签名验证操作类，不是窗体
        /// </summary>
        public Lis.Client.CASign.FrmUserInfo CAUserInfo = null;

        /// <summary>
        /// 登录失败处理：1min内登录失败3次锁定账户
        /// </summary>
        public System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// 登录失败次数统计
        /// </summary>
        private Dictionary<string, int> dicLogTimes = new Dictionary<string, int>();

        /// <summary>
        /// CA签名认证类
        /// </summary>
        private dcl.client.ca.ICaPKI caPKI = null;

        FrmMainNew frmMainNew = null;

        public FrmLogin()
        {
            InitializeComponent();
            this.txtLoginId.Focus();
            UserInfo.entityUserInfo = null;

            this.Load += FrmLogin_Load;
            //this.KeyPress += Enter_KeyPress;

            this.Paint += FrmLogin_Paint;
            this.Shown += FrmLogin_Shown;
            this.FormClosing += FrmLogin_FormClosing;

            this.pnLogo.MouseDown += frmLogin_MouseDown;
            this.pnLogin.MouseDown += frmLogin_MouseDown;
      

            this.btnLogin.Click += btnLogin_Click;
            this.btnCancle.Click += btnCancle_Click;
            this.txtLoginId.KeyPress += txtLoginId_KeyPress;
            this.txtPassword.KeyPress += Enter_KeyPress;

            Animator.AnimationType = AnimatorNS.AnimationType.Transparent;
            Animator.Interval = 20;
            Animator.MaxAnimationTime = 1000;
            Animator.TimeStep = 0.02F;

            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dicLogTimes.Clear();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Region = new Region(GetRoundedRectPath(25));
                }
                

                lblVersion.Text = String.IsNullOrEmpty(AssemblyTrademark) ? "" : AssemblyTrademark;
                //添加桌面快捷方式_ClickOnce本身无此功能_但ClickOnce安装完成后会立刻启动程序
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\广州创惠医学检验信息系统.appref-ms";
                string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\程序\\广州创惠信息科技有限公司\\创惠医学检验信息系统.appref-ms";
                if (System.IO.File.Exists(desktop) == false && System.IO.File.Exists(startMenu))
                {
                    File.Copy(startMenu, desktop);
                }

                //没有lis系统目录时，创建一个
                if (!Directory.Exists(lisPath))
                    Directory.CreateDirectory(lisPath);

                if (!Directory.Exists(lisConfigPath))
                    Directory.CreateDirectory(lisConfigPath);

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                if (!Directory.Exists(xmlFile))
                    Directory.CreateDirectory(xmlFile);

                //如果有条码config文件则不copy
                string oldBarcodeConfig = lisPath + @"config\dcl.client.sample.dll.config";
                string newBarcodeConfig = Application.StartupPath + @"\dcl.client.sample.dll.config";
                if (!File.Exists(oldBarcodeConfig) && File.Exists(newBarcodeConfig))
                {
                    File.Copy(newBarcodeConfig, oldBarcodeConfig);
                }
                //删除临时文件
                string file = Application.StartupPath + "\\Temp";

                if (Directory.Exists(file))
                {
                    Directory.Delete(file, true);
                }
                Directory.CreateDirectory(file);

                CASignRead();
                
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                lis.client.control.MessageDialog.Show("系统初始化失败", "提示");
            }
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            try
            {
                string SSOINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SSOCONTENT.INI";
                if (System.IO.File.Exists(SSOINIPath))
                {
                    string strssodata = System.IO.File.ReadAllText(SSOINIPath);
                    if (!string.IsNullOrEmpty(strssodata) && strssodata.ToUpper().StartsWith("SSO") && strssodata.Contains("|"))
                    {
                        string[] splStrSso = strssodata.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (splStrSso.Length >= 4)
                        {
                            if (!string.IsNullOrEmpty(splStrSso[3]))
                            {
                                string strTempPw = new ProxySysUserInfo().Service.Getuserpwinfo(splStrSso[3]);
                                if (string.IsNullOrEmpty(strTempPw))
                                {
                                    strTempPw = splStrSso[3];
                                }
                                else
                                {
                                    strTempPw = EncryptClass.Decrypt(strTempPw);
                                }

                                txtLoginId.Text = splStrSso[3];
                                txtPassword.Text = strTempPw;
                                btnLogin_Click(sender, e);
                            }
                        }
                    }
                }
                else
                {
                    this.TopLevel = true;
                    this.Activate();
                    this.ActiveControl = this.txtLoginId;
                    this.txtLoginId.Focus();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("FrmLogin_Shown", ex);
            }
        }

        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (Char)Keys.Enter) && this.Enabled == true && !string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                btnLogin.PerformClick();
            }
        }

        private void txtLoginId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (Char)Keys.Enter) && this.Enabled == true)
            {
                txtPassword.Focus();
            }
        }

        private void ShowTips()
        {
            Animator.Show(xRails_LabelError, true, AnimatorNS.Animation.Transparent);
            Animator.Show(pnSubmitBtn, true, AnimatorNS.Animation.HorizSlide);
        }


        bool loginClick = false;
        private bool isGoLogin = false;
        /// <summary>
        /// 登录按钮,数据查询后的初始化事件在afterSearch中执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                labLoading.Visible = true;

                if (loginClick || this.Enabled == false)//避免重复提交
                    return;

                Animator.Hide(pnSubmitBtn, true, AnimatorNS.Animation.HorizSlide);
                Animator.Hide(xRails_LabelError, true, AnimatorNS.Animation.Transparent);
                Animator.WaitAllAnimations();

                isGoLogin = false;
                loginClick = true;

                //Application.DoEvents();

                labLoading.Text = "    " + ".....正在验证用户名和密码.......";

                if (action == "lock" && this.txtLoginId.Text != UserInfo.loginID)
                {
                    string strmes = "本系统已被" + UserInfo.loginID + "锁定";
                    ShowTips();
                    MessageDialog.Show(string.Format("本系统已被用户【{0}】锁定，不能使用其它用户登录！", UserInfo.loginID),"警告");
                    this.txtLoginId.Text = UserInfo.loginID;
                    this.txtPassword.Focus();
                }

                if(string.IsNullOrEmpty(txtLoginId.Text.Trim()))
                {
                    xRails_LabelError.Text = "请输入用户名！";
                    ShowTips();
                    loginClick = false;
                    txtLoginId.Focus();
                    return;
                }

                if(string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    xRails_LabelError.Text = "请输入密码！";
                    ShowTips();
                    loginClick = false;
                    txtPassword.Focus();
                    return;
                }

                EntityLogin LoginInfo = SearchEntity();

                #region 旧CA认证
                ////CA认证登录
                //if ((strCASignMode == "深圳沙井医院" || strCASignMode == "广州中医大" || strCASignMode == "河池市人民医院" || strCASignMode == "广东医学院附属医院") && IsCASignByLogin)
                //{
                //    if (LoginInfo != null)
                //    {
                //        EntityLogin dtbLoginInfo = LoginInfo;

                //        //需要进行电子签名验证
                //        if (dtbLoginInfo.CASignMode.ToString() == "True")
                //        {
                //            if (!CAUserInfo.UserLoginCherk(this.txtLoginId.Text.Trim(), this.txtPassword.Text.Trim()))
                //            {
                //                xRails_LabelError.Text = "    " + "电子签名验证不成功";
                //                ShowTips();
                //                loginClick = false;
                //                return;
                //            }
                //        }

                //        //不需要进行电子签名验证
                //        else if (dtbLoginInfo.CASignMode.ToString() == "False"
                //            || dtbLoginInfo.CASignMode.ToString() == ""
                //            || this.txtLoginId.Text.ToLower() == "admin"
                //            )
                //        {

                //        }
                //        else//没有查无此人
                //        {
                //            xRails_LabelError.Text = "    " + "用户登录帐号不存在";
                //            ShowTips();
                //            loginClick = false;
                //            return;
                //        }
                //    }
                //}
                ////开始  网证通CA
                //else if (strCASignMode == "中大肿瘤医院" || strCASignMode == "广州十二医院" && IsCASignByLogin)
                //{
                //    if (LoginInfo != null)
                //    {
                //        EntityLogin dtbLoginInfo = LoginInfo;
                //        //需要进行电子签名验证
                //        if (dtbLoginInfo.CASignMode.ToString() == "True")
                //        {
                //            string result = string.Empty;
                //            if (strCASignMode == "广州十二医院")
                //            {
                //                result = GDNetCA.CheckCert(ConfigurationManager.AppSettings["GDCAUrl"], ConfigurationManager.AppSettings["DefaultServerCert"]);
                //            }
                //            else
                //            {
                //                result = GDNetCA.CheckCert(ConfigurationManager.AppSettings["GDCAUrl"]);
                //            }
                //            if (!String.IsNullOrEmpty(result) && !result.Contains("证书有效"))
                //            {
                //                xRails_LabelError.Text = "    " + result; //"电子签名验证不成功";
                //                ShowTips();
                //                loginClick = false;
                //                return;
                //            }

                //            if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                //            {
                //                // <!--netca web service地址-->
                //                string NetCa_WSAddress = System.Configuration.ConfigurationManager.AppSettings["NetCa_WSAddress"];
                //                //医院业务系统使用的CA厂商批复码
                //                string NetCa_hispital_code = System.Configuration.ConfigurationManager.AppSettings["NetCa_hispital_code"];

                //                //上传ca信息给netca
                //                if (!string.IsNullOrEmpty(NetCa_WSAddress) && !string.IsNullOrEmpty(NetCa_hispital_code))
                //                {
                //                    DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
                //                }
                //            }
                //        }

                //        //不需要进行电子签名验证
                //        else if (dtbLoginInfo.CASignMode.ToString() == "False"
                //            || dtbLoginInfo.CASignMode.ToString() == ""
                //            || this.txtLoginId.Text.ToLower() == "admin"
                //            )
                //        {

                //        }
                //        else//没有查无此人
                //        {
                //            xRails_LabelError.Text = "    " + "用户登录帐号不存在";
                //            ShowTips();
                //            loginClick = false;
                //            return;
                //        }
                //    }
                //}

                #endregion

                if (IsCASignByLogin && LoginInfo.UserCaFlag)
                {
                    caPKI = CaPKIFactory.CreateCASignature(strCASignMode);

                    if (caPKI == null)
                    {
                        xRails_LabelError.Text = "电子签名验证失败: " + CaPKIFactory.errorInfo;
                        ShowTips();
                        loginClick = false;
                        return;
                    }
                    caPKI.UserId = LoginInfo.LogInID;
                    if (!caPKI.LoginWithCA(LoginInfo))
                    {
                        xRails_LabelError.Text = "电子签名验证失败: " + caPKI.ErrorInfo;
                        ShowTips();
                        loginClick = false;
                        return;
                    }
                }


                EntityRequest request = new EntityRequest();
                request.SetRequestValue(LoginInfo);
                EntityLoginUserInfo userInfo = proxy.Service.CsLogin(request);
                LoadUserInfo(userInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// 获得登录提交服务端的验证Dataset
        /// </summary>
        /// <returns></returns>
        private EntityLogin SearchEntity()
        {
            EntityLogin login = new EntityLogin();
            string ip = IPUtility.GetIP();
            string mac = IPUtility.GetMAC();

            UserInfo.ip = ip;
            UserInfo.mac = mac;
            login.LogInID = txtLoginId.Text;
            login.PassWord = EncryptClass.Encrypt(txtPassword.Text);
            login.IP = ip;
            login.Mac = mac;
            login.Action = this.action;

            if (IsCASignByLogin)
            {
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(login);
                EntityResponse respone = new EntityResponse();
                //查询用户表，检查该用户是否需要验证电子签名
                respone = proxy.Service.ModeChange(request);
                login = respone.GetResult() as EntityLogin;
            }

            return login;
        }

        /// <summary>
        /// 登录后获得用户信息
        /// </summary>
        /// <param name="returnData"></param>
        private void LoadUserInfo(EntityLoginUserInfo userInfo)
        {
            try
            {
                if (userInfo != null)
                {
                    if (userInfo.ErrorInfo.LoginStatus == 1)
                    {
                        labLoading.Text = "    " + "......验证成功,正在初始化用户信息......";

                        UserInfo.entityUserInfo = new EntityLoginUserInfo();

                        try
                        {
                            UserInfo.entityUserInfo.UserInfo = userInfo.UserInfo;
                            UserInfo.entityUserInfo.Func = userInfo.Func;
                            UserInfo.entityUserInfo.AllFunc = userInfo.AllFunc;
                            UserInfo.entityUserInfo.SysConfig = userInfo.SysConfig; ;
                            UserInfo.entityUserInfo.UserConfig = userInfo.UserConfig;
                            UserInfo.entityUserInfo.UserItrsQc = userInfo.UserItrsQc;
                            UserInfo.entityUserInfo.UserQcLab = userInfo.UserQcLab;
                            UserInfo.entityUserInfo.PowerUserKey = userInfo.PowerUserKey;
                            if (userInfo.PowerUserRole != null)
                            {
                                UserInfo.entityUserInfo.PowerUserRole = userInfo.PowerUserRole;
                            }
                            UserInfo.listUserLab = userInfo.UserLabInfo;
                            UserInfo.types = "";
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            throw;
                        }


                        foreach (EntityUserLab lab in UserInfo.listUserLab)
                        {
                            UserInfo.types += lab.LabId + ",";
                        }
                        UserInfo.types = UserInfo.types.TrimEnd(',');

                        if (!string.IsNullOrEmpty(userInfo.UserInfo[0].ItrId))
                        {
                            UserInfo.defaultItr = userInfo.UserInfo[0].ItrId;
                        }

                        if (!string.IsNullOrEmpty(userInfo.UserInfo[0].UserDefaultLabId))
                        {
                            UserInfo.defaultType = userInfo.UserInfo[0].UserDefaultLabId;
                        }

                        UserInfo.userInfoId = userInfo.UserInfo[0].UserId;
                        UserInfo.loginID = userInfo.UserInfo[0].UserLoginid;
                        UserInfo.userName = userInfo.UserInfo[0].UserName;
                        UserInfo.isAdmin = userInfo.UserInfo[0].UserId.Equals("-1");
                        UserInfo.password = EncryptClass.Decrypt(userInfo.UserInfo[0].UserPassword);
                        if (!string.IsNullOrEmpty(userInfo.UserInfo[0].UserCaFlag.ToString()))
                        {
                            UserInfo.CASignMode = userInfo.UserInfo[0].UserCaFlag;
                        }

                        if (string.IsNullOrEmpty(userInfo.UserInfo[0].DefaultLabName))
                        {
                            UserInfo.defaultTypeName = userInfo.UserInfo[0].DefaultLabName;
                        }

                        if (!string.IsNullOrEmpty(userInfo.UserInfo[0].ItrEname))
                        {
                            UserInfo.defaultItrName = userInfo.UserInfo[0].ItrEname;
                        }

                        List<EntityUserInstrmt> listUserItrs = userInfo.UserItrs;
                        if (listUserItrs.Count > 0)
                        {
                            string[] strUserItrs = new string[listUserItrs.Count];

                            for (int i = 0; i < listUserItrs.Count; i++)
                            {
                                strUserItrs[i] = listUserItrs[i].ItrId;
                            }
                            UserInfo.UserItrs = strUserItrs;
                        }

                        List<EntitySysUser> listDepart = userInfo.Depart;
                        if (listDepart.Count > 0)
                        {
                            EntitySysUser entityDepart = listDepart[0];
                            UserInfo.departName = entityDepart.UserDepartName;
                            UserInfo.oriName = entityDepart.UserOrgName;
                            UserInfo.departId = entityDepart.UserDepartId;
                        }

                        UserInfo.listUserDepart = userInfo.UserDepart;

                        try
                        {
                            UserInfo.userKey = true;

                            //if (UserInfo.GetSysConfigValue("UserKey") == "是" && !UserInfo.isAdmin && UserInfo.entityUserInfo.UserLabInfo[0].UserType == "检验组")
                            //{
                            //    UserInfo.userKey = false;
                            //    List<EntityUserKey> listUserKey = userInfo.PowerUserKey;
                            //    if (listUserKey.Where(w=>w.UserLoginId==UserInfo.loginID).Count() > 0)//有密钥判定密钥是否对，无密钥信息通过登录登录密钥。
                            //    {
                            //        NetcaSign signer = new NetcaSign();

                            //        string strKey = string.Empty;

                            //        if (signer.login(this.txtLoginId.Text, ref strKey) && listUserKey.Where(w=>w.Userkey== strKey && w.UserLoginId== this.txtLoginId.Text.Trim()).Count()>0 )
                            //        {
                            //            UserInfo.userKey = true;
                            //        }
                            //        else
                            //        {
                            //            labLoading.Text = "    " +  "密钥验证不通过";

                            //        }
                            //    }
                            //    else
                            //    {
                            //        lis.client.control.MessageDialog.ShowAutoCloseDialog("请联系计算机管理中心追加密钥！");
                            //        labLoading.Text = "    " +  "请联系计算机管理中心追加密钥！";

                            //    }
                            //    if (!UserInfo.userKey)
                            //    {
                            //        this.Enabled = true;
                            //        loginClick = false;
                            //        return;
                            //    }

                            //}
                            //else
                            //{
                            //    UserInfo.userKey = true;
                            //}

                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            throw;
                        }

                        if (action == "lock" && frmMainNew != null && isGoLogin)
                        {
                            action = "lockNoTip";
                            frmMainNew.Close();
                            frmMainNew = null;
                            action = "lock";
                        }

                        if (action == "lock" && frmMainNew != null)
                        {
                            this.frmMainNew.Show();
                            dcl.client.notifyclient.UrgentNotifyHelper.Current.start(); //危急值内部提醒--启动
                            dcl.client.notifyclient.LEDNotifyHelper.Current.start(); //LED内部通知--启动
                            dcl.client.notifyclient.ReagentNotifyHelper.Current.start(); //试剂管理内部通知--启动
                            this.Hide();
                        }
                        else
                        {
                            //Thread t = new Thread(CacheForm);
                            //t.Start();

                            labLoading.Text = "    " + "......正在加载数据字典缓存......";
                            if (frmMainNew == null)
                            {
                                labLoading.Text = "    " + "......正在初始化工作环境......";
                                frmMainNew = new FrmMainNew();

                                Thread report = new Thread(getReportsFile);
                                report.IsBackground = true;
                                report.Start();
                            }

                            labLoading.Text = "    " + "......登录完成……";
                            txtPassword.Text = "";

                            this.Hide();
                            this.Visible = false;

                            frmMainNew.login = this;
                            this.btnLogin.Click += this.frmMainNew.LoadNewForm;
                            frmMainNew.Show();

                            Thread th = new Thread(LoadDict);
                            th.IsBackground = true;
                            th.Start();
                        }
                        action = "login";
                        LockSystem(false);
                    }

                    else
                    {
                        this.Show();

                        string errorInfo = userInfo.ErrorInfo.LoginStatus.ToString();

                        if (errorInfo == "0")
                        {
                            xRails_LabelError.Text = "    " + "产品注册已到期,请联系管理员";
                        }
                        if (errorInfo == "2")
                        {
                            xRails_LabelError.Text = "    " + "用户登录帐号不存在";
                        }
                        if (errorInfo == "3")
                        {
                            int times;
                            bool t =  dicLogTimes.TryGetValue(userInfo.UserInfo[0].UserLoginid, out times);
                            if (t && times > 4)
                            {
                                dicLogTimes.Remove(userInfo.UserInfo[0].UserLoginid);

                                ProxyUserManage proxy = new ProxyUserManage();

                                proxy.Service.UpdateUserFlag(userInfo.UserInfo[0].UserLoginid);

                                this.action = "lock";
                                
                                xRails_LabelError.Text = "操作异常，该账户已被锁定，请联系管理员解锁。";
                            }
                            else if (t)
                            {
                                xRails_LabelError.Text = "    " + "该登录帐号对应的密码错误";

                                dicLogTimes[userInfo.UserInfo[0].UserLoginid] = ++times;
                            }
                            else
                            {
                                xRails_LabelError.Text = "    " + "该登录帐号对应的密码错误";

                                dicLogTimes.Add(userInfo.UserInfo[0].UserLoginid, 1);
                            }
                            //xRails_LabelError.Text = "    " + "该登录帐号对应的密码错误";
                        }
                        //网证通
                        if (errorInfo == "5")
                        {
                            xRails_LabelError.Text = "    " + "该登录帐号需要USBKey！";
                        }
                        if (errorInfo == "6")
                        {
                            xRails_LabelError.Text = "    " + "该登录帐号所使用USBKey错误！";
                        }
                        if (errorInfo == "7")
                        {
                            xRails_LabelError.Text = "    " + "网络连接失败！";
                        }
                        ShowTips();
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("网络连接失败!", "错误");
                    xRails_LabelError.Text = "    " + "网络连接失败!";
                    Lib.LogManager.Logger.LogInfo(userInfo.ErrorInfo.ErrorMsg);
                    ShowTips();
                }

                loginClick = false;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        private void LoadDict()
        {
            try
            {
                CacheClient.GetCache<EntityDefItmProperty>();
                CacheClient.GetCache<EntityDicInstrument>();
                CacheClient.GetCache<EntityDicPubProfession>();
                CacheClient.GetCache<EntityDicSample>();
                CacheClient.GetCache<EntityDicSampRemark>();
                CacheClient.GetCache<EntityDicOrigin>();
                CacheClient.GetCache<EntityDicCombine>();
                CacheClient.GetCache<EntityDicCheckPurpose>();
                CacheClient.GetCache<EntityDicPubDept>();
                CacheClient.GetCache<EntityDicResultTips>();
                CacheClient.GetCache<EntityDicPubIcd>();
                CacheClient.GetCache<EntityDicSState>();
                CacheClient.GetCache<EntitySysUser>();
                CacheClient.GetCache<EntityDicPubIdent>();
                CacheClient.GetCache<EntityDicPubInsurance>();
                CacheClient.GetCache<EntityDicDoctor>();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        #region 测试代码
        public void CacheForm()
        {
            try
            {

                //dcl.client.result.FrmPatEnter.PreLoad();
                //frm = new dcl.client.result.FrmPatEnter(true);
                //frm.Dispose();
                //frm = null;
                //dcl.client.result.FrmPatEnter.cached = true;
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion

        /// <summary>
        /// 从服务器获得报表文件到本地
        /// </summary>
        /// <param name="localPath">写入地址</param>
        public void getReportsFile()
        {
            try
            {
                if (ConfigurationManager.AppSettings["wcfAddr"].Contains("localhost"))
                    return;

                string user = UserInfo.GetSysConfigValue("1");//得到系统配置

                #region 改造后代码 查询报表数据
                ProxyReportMain proxyRepMain = new ProxyReportMain();
                EntityRequest request = new EntityRequest();
                EntityResponse response = proxyRepMain.Service.GetReport(request); //获取报表数据
                List<EntitySysReport> listSysReport = new List<EntitySysReport>();
                listSysReport = response.GetResult() as List<EntitySysReport>;

                EntitySysReport eySysReport = new EntitySysReport();
                eySysReport.RepLocation = "报表样本.repx";
                listSysReport.Add(eySysReport);
                #endregion

                string serverPath = ConfigurationManager.AppSettings["wcfAddr"].ToString() + @"xtraReport/";//服务器目录

                localPath += @"\";//本地目录

                string strEX = "";

                System.Net.WebClient client = new System.Net.WebClient();
                client.Proxy = null;
                foreach (var infoRep in listSysReport)
                {
                    try
                    {
                        string strRepPath = infoRep.RepLocation;
                        string writePath = localPath + strRepPath;
                        string readPath = serverPath + strRepPath;
                        if (user == "使用本地模版")
                        {
                            if (!File.Exists(writePath))
                                client.DownloadFile(readPath, writePath);
                        }
                        else
                            client.DownloadFile(readPath, writePath);

                    }
                    catch (Exception e)
                    {
                        strEX += "服务器无:" + infoRep.RepLocation + "报表！\r\n";
                    }
                }

                if (strEX != "")
                {
                    // Logger.LogInfo(strEX);
                }

                XtraReport xtrTest = new XtraReport();
                xtrTest.LoadLayout(localPath + "报表样本.repx");
                xtrTest.Dispose();
            }
            catch (Exception exc)//一般是因为服务器MIME类型没有添加.repx post/get
            {
                Logger.LogException("下载报表文件遇到错误", exc);
            }
        }


        /// <summary>
        /// 取消登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public string AssemblyTrademark
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }

        /// <summary>
        /// 窗体关闭前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (action == "lock")
            {
                if (lis.client.control.MessageDialog.Show(string.Format("当前系统被锁定，退出系统可能导致用户【{0}】未保存的工作消失，是否继续？", UserInfo.loginID), MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }



        /// <summary>
        /// 清空密码
        /// </summary>
        internal void LockSystem(bool IsLock=true)
        {
            if(IsLock)
            {
                xRails_LabelError.Text = "本系统已被" + UserInfo.loginID + "锁定";
                ShowTips();
                btnLogin.Text = "解锁";
                this.txtLoginId.Enabled = false;
                txtPassword.Text = "";
                txtPassword.Focus();
            }
            else
            {
                btnLogin.Text = "登录";
                this.txtLoginId.Enabled = true;
                
            }
            
        }


        /// <summary>
        /// 读取BJCAUKEY信息
        /// </summary>
        private void CASignRead()
        {
            strCASignMode = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("CASignMode");
            IsCASignByLogin = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("CASignByLogin") != "否";

            //判断是否为电子签名验证模式|| strCASignMode == "河池市人民医院"
            //添加省妇幼的CA登录。|| strCASignMode=="省妇幼"
            if (strCASignMode == "深圳沙井医院" || strCASignMode == "广州中医大" || strCASignMode == "河池市人民医院" || strCASignMode == "广东医学院附属医院")
            {

                CAUserInfo = new Lis.Client.CASign.FrmUserInfo();


                if (!string.IsNullOrEmpty(CAUserInfo.strUserName) && IsCASignByLogin)
                {
                    IsExistsCaUKey = true;//标志登录前插入了ukey
                    this.txtLoginId.Text = CAUserInfo.strUserName;
                    this.txtPassword.Focus();
                }
                else
                {
                    IsExistsCaUKey = false;
                }
            }
            // * 网证通CA
            //else if (strCASignMode == "中大肿瘤医院")
            //{
            //    //设置登录框中帐号姆印
            //    //SecuInter.X509Certificate oCert = NetCAPKI.getX509Certificate(
            //    //     NetCAPKI.SECUINTER_CURRENT_USER_STORE, NetCAPKI.SECUINTER_MY_STORE,1,1);
            //    //this.txtLoginId.Text = NetCAPKI.getX509CertificateThumbprint(oCert);
            //    //this.txtPassword.Focus();
            //}
            else
            {
                this.txtLoginId.Focus();
            }
        }

        #region 任意位置拖动

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #endregion

        #region 圆角

        private void FrmLogin_Paint(object sender, PaintEventArgs e)
        {
            //this.Region = new Region(GetRoundedRectPath(25));
        }

        //圆角
        private GraphicsPath GetRoundedRectPath(int diameter)
        {
            Rectangle rect = new Rectangle(-1, -1, this.Width + 1, this.Height);
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            //   左上角   
            path.AddArc(arcRect, 181, 90);
            //   右上角   
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 271, 90);
            //   右下角   
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 359, 90);
            //   左下角   
            arcRect.X = rect.Left;
            arcRect.Width += 2;
            arcRect.Height += 2;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }


        #endregion

        #region 重写CreateParams

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 0x20000;
                return createParams;
            }
        }


        #endregion
    }
}
