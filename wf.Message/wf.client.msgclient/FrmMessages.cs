using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using dcl.client.wcf;

using dcl.client.users;
using dcl.entity;

namespace dcl.client.msgclient
{
    public partial class FrmMessages : Form
    {
        //声明API  
        Icon iconDefault;
        Icon iconBlank;
        bool showBlankIcon = false;

        /// <summary>
        /// 窗口是否初次显示
        /// </summary>
        bool firstLocation = true;

        /// <summary>
        /// 科室ID
        /// </summary>
        internal string deptID;

        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszSound, int hmod, int fdwSound);

        [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        private static extern bool Beep(int frequency, int duration);

        private const int SND_FILENAME = 0x00020000; public const int SND_ASYNC = 0x0001;

        #region 控制系统音量成员

        #region 属性

        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);
        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

        #endregion

        #region 方法

        /// <summary>
        /// +2音量
        /// </summary>
        private static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// -2音量
        /// </summary>
        private static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 静音
        /// </summary>
        private static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        #endregion

        #endregion


        Timer readTimer = new Timer();

        //bool isPlay = false;

        public FrmMessages()
        {
            InitializeComponent();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMessages));
            iconDefault = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            iconBlank = ((System.Drawing.Icon)(resources.GetObject("blank")));
            string PlaySoundMode = System.Configuration.ConfigurationManager.AppSettings["PlaySoundMode"];
            int pos;
            if (int.TryParse(PlaySoundMode, out pos))
            {
                ChangeSoundOpt(pos);
            }
            else
            {
                ChangeSoundOpt(0);
            }
            deptID = ConfigurationManager.AppSettings["dep_code"];
            //&amp;
            if (deptID.Contains("&"))
            {
                //支持多病区
                string[] strspl = deptID.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                //if (strspl.Length > 0) deptID = strspl[0];
                deptID = "";
                for (int j = 0; j < strspl.Length; j++)
                {
                    if (string.IsNullOrEmpty(deptID))
                    {
                        deptID = strspl[j];
                    }
                    else
                    {
                        deptID += "," + strspl[j];
                    }
                }
            }
            if (ConfigurationManager.AppSettings["UserAuthType"] == "sjaudit")
            {
                this.WindowState = FormWindowState.Maximized;
                this.panel2.Visible = true;
            }
            else if (ConfigurationManager.AppSettings["UserAuthType"] == "fsaudit")//如果是佛山市一用户
            {
                try
                {
                    tsmWinMax_Click(null, null);//危急值提示窗口默认最大化

                    for (int temp_up = 1; temp_up <= 50; temp_up++)//系统音量默认最大
                    {
                        VolumeUp();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public MessageReceiverCollection Messages { get; set; }
        public ObrMessageReceiveCollection Messages { get; set; }



        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            readTimer.Interval = 15000;
            readTimer.Tick += new EventHandler(readTimer_Tick);
            readTimer.Start();
        }


        void readTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //MessageReceiverCollection messages = GetMessages(deptID);
                ObrMessageReceiveCollection messages = GetMessages(deptID);
                if (messages != null && messages.Count > 0)
                // && (Messages == null || (Messages != null && Messages.Count != messages.Count)))
                {
                    Messages = messages;
                    //FlashIcon();
                    ShowMessages(messages);
                }
                else
                {
                    if (Messages != null)
                    {
                        Messages.Clear();
                    }
                    listView1.Items.Clear();

                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            //string deptID = "0001";

        }
        Timer showIconTimer;
        /// <summary>
        /// 托盘图标闪动
        /// </summary>
        private void FlashIcon()
        {
            if (showIconTimer == null)
                showIconTimer = new Timer();
            showIconTimer.Interval = 500;
            showIconTimer.Tick += new EventHandler(showIconTimer_Tick);
            showIconTimer.Start();
        }

        void showIconTimer_Tick(object sender, EventArgs e)
        {
            if (!showBlankIcon)
            {
                notifyIcon1.Icon = iconBlank;
                showBlankIcon = true;
            }
            else
            {
                SetDefaultIcon();
            }
        }

        private void ShowMessages(ObrMessageReceiveCollection messages) // MessageReceiverCollection messages
        {
            listView1.Items.Clear();

            if (messages == null || messages.Count == 0)
            {
                this.Hide();
                return;
            }

            string strPatIdAll = "";
            messages.Reverse();//顺序翻转

            //foreach (EntityMessageReceiver item in messages)
            foreach (EntityDicObrMessageReceive item in messages)
            {
                //listView1.Items.Add(item.ToString());

                //string title = string.Format(string.Format("危急值报告：{0} 姓名：{1}", item.MessageContent.ReceiverString, item.MessageContent.Ext2));
                //listView1.Items.Add(title);

                //string title = item.MessageContent.XMLContent;// string.Format(string.Format("危急值报告：{0} 姓名：{1}", item.MessageContent.ReceiverString, item.MessageContent.Ext2));
                string title = item.ObrMessageContent.ObrContent;

                //ListViewItem lvitem = listView1.Items.Add(item.MessageContent.Ext1, title, 0);
                if (title.Trim() != string.Empty)
                {
                    ListViewItem lvitem = listView1.Items.Add(title);
                    if (item.ObrMessageContent.ObrType == 4096)//if (item.MessageContent.MessageType == EnumMessageType.URGENT_MESSAGE)
                    {
                        lvitem.ForeColor = System.Drawing.Color.Red;//急查为红色字体
                    }
                    //lvitem.EnsureVisible();
                    lvitem.Tag = item;
                }
                strPatIdAll += item.ObrMessageContent.ObrValueA + ";";
            }
            strPatIdAll = strPatIdAll.TrimEnd(';');
            if (ConfigurationManager.AppSettings["UserAuthType"] == "sjaudit")
            {

                this.txtLogin.Text = "";
                this.txtPwd.Text = "";

                if (!string.IsNullOrEmpty(strPatIdAll))
                {
                    this.listView1.Visible = false;
                    this.panel2.Visible = true;

                    this.webBrowser1.Url = new Uri(ConfigurationManager.AppSettings["WebSelectUrl"] + strPatIdAll + "&returnCriticalReport=true");
                    this.txtLogin.Focus();
                }
            }

            #region 声音提示

            //播放声音文件
            //System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/pm3.wav");
            //sndPlayer.PlayLooping();
            //isPlay = true;
            //this.axWindowsMediaPlayer1.URL = Application.StartupPath + @"/sound.mp3";

            //播放声音
            if (tsmiBugle.Checked)
            {
                PlaySound("WindowsNotify.wav", 0, SND_ASYNC | SND_FILENAME);//音响发声
            }
            else if (tsmiHost.Checked)
            {
                playBeep();//主机发声
            }

            #endregion

            #region 窗口位置及状态

            if (tsmWinMax.Checked) //最大化
            {
                firstLocation = false;//取消窗口初次出现位置

                int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width;
                int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height;

                if (this.Location.X != 0 || this.Location.Y != 0)//坐标
                    this.SetDesktopLocation(0, 0);

                if (this.Size.Width != x || this.Size.Height != y)//窗口大小
                    this.Size = new System.Drawing.Size(x, y);
            }
            else if (firstLocation)//窗口初次出现位置
            {
                int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
                int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
                this.SetDesktopLocation(x, y);
                firstLocation = false;
            }
            #endregion

            if (true)
            {
                AnimateWindow(this.Handle, 1000, AW_ACTIVATE | AW_BLEND);//从下到上且不占其它程序焦点   
                this.Focus();
            }

            if (listView1.Items.Count <= 0)
            {
                this.Visible = false;
            }
        }
        /// <summary>
        /// 改变声音选项
        /// </summary>
        /// <param name="opt">0-无 1-主机 2-音响</param>
        private void ChangeSoundOpt(int opt)
        {
            switch (opt)
            {
                case 0://无
                    tsmiWithout.Checked = true;
                    tsmiHost.Checked = false;
                    tsmiBugle.Checked = false;
                    break;
                case 1://主机发声
                    tsmiWithout.Checked = false;
                    tsmiHost.Checked = true;
                    tsmiBugle.Checked = false;
                    break;
                case 2://音响设备发声
                    tsmiWithout.Checked = false;
                    tsmiHost.Checked = false;
                    tsmiBugle.Checked = true;
                    break;
                default://默认
                    tsmiWithout.Checked = true;
                    tsmiHost.Checked = false;
                    tsmiBugle.Checked = false;
                    break;
            }
        }
        /// <summary>
        /// 窗口状态设置
        /// </summary>
        /// <param name="opt"></param>
        private void ChangeShowOpt(int opt)
        {
            switch (opt)
            {
                case 0://还原
                    tsmWinNor.Checked = true;
                    tsmWinMax.Checked = false;
                    this.WindowState = FormWindowState.Normal;
                    this.Size = new System.Drawing.Size(271, 212);
                    break;
                case 1://最大化
                    tsmWinNor.Checked = false;
                    tsmWinMax.Checked = true;
                    //this.WindowState = FormWindowState.Maximized;
                    break;
                default://默认
                    tsmWinNor.Checked = true;
                    tsmWinMax.Checked = false;
                    this.WindowState = FormWindowState.Normal;
                    this.Size = new System.Drawing.Size(271, 212);
                    break;
            }
        }
        /// <summary>
        /// 主机发声
        /// </summary>
        private bool playBeep()
        {
            try
            {
                //响的时长
                const int L0 = 1600, L1 = 800, L2 = 400, L3 = 200, L4 = 100;
                //音符-频率
                const int noteC = 264, noteD = 297, noteE = 330, noteF = 352, noteG = 396, noteA = 440, noteB = 495;

                //自定义声音
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);

                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        private ObrMessageReceiveCollection GetMessages(string dep_code)  // 原本的返回值类型MessageReceiverCollection
        {
            //List<EntityMessageReceiver> messages = new List<EntityMessageReceiver>();

            //messages.Add(new EntityMessageReceiver("AGT", "AGT超过危急值60"));
            //messages.Add(new EntityMessageReceiver("Ca", "Ca超过危急值51.5"));

            //MessageReceiverCollection msg = new lis.biz.message.MessageBiz().GetMessageByReceiverID(dep_code, EnumMessageReceiverType.Dept, false, false, false);

            //ProxyMessage proxy = new ProxyMessage();
            ProxyObrMessage proxyMsg = new ProxyObrMessage();
            //MessageReceiverCollection msg;
            ObrMessageReceiveCollection msg;
            if (dep_code.Contains(","))
            {
                //msg = proxy.Service.GetDeptListMessageFromCache(dep_code);//更改为GetMessageByDepts（dept_codes）
                msg = proxyMsg.Service.GetMessageByDepts(dep_code);
            }
            else
            {
                //msg = proxy.Service.GetDeptMessageFromCache(dep_code);//方法改为GetDeptMessageByDeptCode(dep_code)
                msg = proxyMsg.Service.GetDeptMessageByDeptCode(dep_code);
            }
            //return messages;

            return msg;
        }

        private void FrmMessages_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!isExitApp)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                this.notifyIcon1.Visible = false;//退出时隐藏任务栏
            }

        }

        private void CloseIconShowTimer()
        {
            if (showIconTimer != null && showIconTimer.Enabled == true)
            {
                showIconTimer.Stop();
                showIconTimer = null;
            }
        }

        private void SetDefaultIcon()
        {
            notifyIcon1.Icon = iconDefault;
            showBlankIcon = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ShowMessages(Messages);
            CloseIconShowTimer();
            SetDefaultIcon();
        }

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0x0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;

        private void listView1_DoubleClick(object sender, EventArgs e)
        {


            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView1.SelectedItems[0];

                //EntityMessageReceiver ettReceiver = (item.Tag as EntityMessageReceiver);
                EntityDicObrMessageReceive ettReceiver = (item.Tag as EntityDicObrMessageReceive);

                //if (ettReceiver.MessageContent.MessageType == EnumMessageType.CRITICAL_MESSAGE || ettReceiver.MessageContent.MessageType == EnumMessageType.TJ_CRITICAL_MESSAGE)
                if (ettReceiver.ObrMessageContent.ObrType == 1024 || ettReceiver.ObrMessageContent.ObrType == 3024)
                {
                    #region 处理危急值消息
                    //string pat_id = ettReceiver.MessageContent.Ext1;
                    string pat_id = ettReceiver.ObrMessageContent.ObrValueA;

                    #region 用户验证

                    //是否用户验证
                    string IsUserValidate = GetAppValueByKey("IsUserValidate", "Y");//如果为null添加默认值Y
                    //验证时是否显示文本框
                    string TextVisible = GetAppValueByKey("TextVisible", "NO");//如果为null添加默认值NO
                    bool DocVisible = GetAppValueByKey("DocVisible", "N") == "Y";
                    //AuditInfo CheckerInfo = null;
                    EntityAuditInfo CheckerInfo = null;
                    if (IsUserValidate.ToUpperInvariant() == "N" || IsUserValidate.ToUpperInvariant() == "NO")
                    {
                        //不验证用户
                        //CheckerInfo = new AuditInfo();
                        CheckerInfo = new EntityAuditInfo();
                        CheckerInfo.UserId = "-1";
                        CheckerInfo.UserName = "-1";
                        CheckerInfo.UserStfId = "-1";
                        CheckerInfo.Password = "-1";
                    }
                    else
                    {
                        //验证用户
                        FrmReadAffirm frmRA = null;

                        //验证时,是否显示编辑框
                        if (TextVisible.ToUpperInvariant() == "Y" || TextVisible.ToUpperInvariant() == "YES")
                        {
                            //frmRA = new FrmReadAffirm(true);
                            //因清远人民需求,弹出报告后再处理
                            string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;
                            WebBroswer web = new WebBroswer();
                            web.URL = url;
                            web.IsShowAudit = true;
                            web.pat_id = pat_id;
                            //web.msg_id = ettReceiver.MessageID;
                            web.msg_id = ettReceiver.ObrId;
                            web.dgEvent = readTimer_Tick;
                            web.Show();

                            readTimer.Stop();
                            this.Hide();
                            readTimer.Start();

                            return;
                        }
                        else
                        {
                            frmRA = new FrmReadAffirm(); //这个页面需要修改

                            frmRA.SetDocVisiable(DocVisible);
                        }

                        if (frmRA.ShowDialog() != DialogResult.Yes)
                        {
                            return;
                        }
                        CheckerInfo = frmRA.m_userInfo;
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(pat_id))
                    {
                        //删除危急值消息(标志删除)
                        if (item.Tag != null)
                        {
                            //string msg_id = ettReceiver.MessageID;
                            string msg_id = ettReceiver.ObrId;

                            //ProxyMessage proxy = new ProxyMessage();
                            ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();

                            proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            proxyDeptMsg.Service.RefreshDeptMessage();
                            this.Messages = GetMessages(deptID);
                            ShowMessages(this.Messages);
                        }

                        if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                        {
                            Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + pat_id);
                            readTimer.Stop();
                            this.Hide();
                            readTimer.Start();
                        }
                        else
                        {

                            //string path_autoupdate = Application.StartupPath + "\\AutoUpdate.exe";
                            //string path_barcodeclient = Application.StartupPath + "\\dcl.client.sampleclient.exe";

                            //if (!File.Exists(path_autoupdate) && !File.Exists(path_barcodeclient))
                            //{
                            //只用网页打开结果报告单
                            listView1.Items.Remove(item);
                            string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;
                            WebBroswer web = new WebBroswer();
                            web.URL = url;
                            web.IsShowAudit = false;//不显示处理按钮
                            web.Show();

                            readTimer.Stop();
                            this.Hide();
                            readTimer.Start();
                        }
                        //}
                        //else
                        //{


                        //    string clientUrl = string.Empty;// ConfigurationManager.AppSettings["ClientSelectAddress"];
                        //    if (File.Exists(path_autoupdate))
                        //        clientUrl = path_autoupdate;
                        //    else
                        //        clientUrl = path_barcodeclient;

                        //    Process.Start(clientUrl, " \"report;-1;-1;-1;-1;" + pat_id);
                        //}


                    }
                    #endregion
                }
                else if (ettReceiver.ObrMessageContent.ObrType == 64) //else if (ettReceiver.MessageContent.MessageType == EnumMessageType.SAMPLE_RETURN)
                {
                    //string barcode = ettReceiver.MessageContent.Ext3;
                    //string dep_code = ettReceiver.ReceiverID;
                    string barcode = ettReceiver.ObrMessageContent.ObrValueC;
                    string dep_code = ettReceiver.ObrUserId;

                    string path_autoupdate = Application.StartupPath + "\\AutoUpdate.exe";
                    string path_barcodeclient = Application.StartupPath + "\\dcl.client.sampleclient.exe";

                    string clientUrl = string.Empty;// ConfigurationManager.AppSettings["ClientSelectAddress"];
                    if (File.Exists(path_autoupdate))
                        clientUrl = path_autoupdate;
                    else
                        clientUrl = path_barcodeclient;

                    Process.Start(clientUrl, string.Format(" \"barcode;{0};-1;-1;", dep_code));

                    listView1.Items.Remove(item);

                    //string msg_id = ettReceiver.MessageID;
                    string msg_id = ettReceiver.ObrId;
                    //AuditInfo CheckerInfo = new AuditInfo();
                    EntityAuditInfo CheckerInfo = new EntityAuditInfo();
                    //ProxyMessage proxy = new ProxyMessage();
                    ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                    //proxy.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, null);
                    proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, null);
                    //proxy.Service.RefreshDeptMessage();
                    proxyDeptMsg.Service.RefreshDeptMessage();
                    this.Messages = GetMessages(deptID);
                    ShowMessages(this.Messages);
                }
                else if (ettReceiver.ObrMessageContent.ObrType == 4096) //else if (ettReceiver.MessageContent.MessageType == EnumMessageType.URGENT_MESSAGE)
                {
                    #region 急查
                    string pat_id = ettReceiver.ObrMessageContent.ObrValueA;

                    //AuditInfo CheckerInfo = new AuditInfo();
                    EntityAuditInfo CheckerInfo = new EntityAuditInfo();
                    CheckerInfo.UserId = "-1";
                    CheckerInfo.UserName = "-1";
                    CheckerInfo.UserStfId = "-1";
                    CheckerInfo.Password = "-1";

                    CheckerInfo.ExetMsg = "急查";

                    if (!string.IsNullOrEmpty(pat_id))
                    {
                        //删除急查消息(标志删除)
                        if (item.Tag != null)
                        {
                            //string msg_id = ettReceiver.MessageID;
                            string msg_id = ettReceiver.ObrId;

                            //ProxyMessage proxy = new ProxyMessage();
                            ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                            proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            proxyDeptMsg.Service.RefreshDeptMessage();
                            this.Messages = GetMessages(deptID);
                            ShowMessages(this.Messages);
                        }

                        if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                        {
                            Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + pat_id);
                            readTimer.Stop();
                            this.Hide();
                            readTimer.Start();
                        }
                        else
                        {
                            //只用网页打开结果报告单
                            listView1.Items.Remove(item);
                            string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;

                            WebBroswer web = new WebBroswer();//这个页面需要修改
                            web.URL = url;
                            web.IsShowAudit = false;//不显示处理按钮
                            web.Show();

                            readTimer.Stop();
                            this.Hide();
                            readTimer.Start();
                        }

                    }
                    #endregion
                }
                //else if (ettReceiver.MessageContent.MessageType == EnumMessageType.BULLETIN)
                else if (ettReceiver.ObrMessageContent.ObrType == 1)//公告--目前清远人医用于血库通知
                {
                    #region 公告

                    //string pat_id = ettReceiver.MessageContent.Ext1;
                    string pat_id = ettReceiver.ObrMessageContent.ObrValueA;
                    //string strTempXmlContent = ettReceiver.MessageContent.XMLContent;
                    string strTempXmlContent = ettReceiver.ObrMessageContent.ObrContent;

                    //AuditInfo CheckerInfo = new AuditInfo();//不验证
                    EntityAuditInfo CheckerInfo = new EntityAuditInfo();//不验证
                    CheckerInfo.UserId = "-1";
                    CheckerInfo.UserName = "-1";
                    CheckerInfo.UserStfId = "-1";
                    CheckerInfo.Password = "-1";

                    if (!string.IsNullOrEmpty(pat_id))
                    {
                        string strTempMsg = "是否取消当前消息提示？";

                        if (!string.IsNullOrEmpty(strTempXmlContent))
                        {
                            strTempMsg += "\r\n\r\n" + strTempXmlContent;
                        }

                        if (MessageBox.Show(strTempMsg, "处理提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                        {
                            return;
                        }


                        //删除急查消息(标志删除)
                        if (item.Tag != null)
                        {
                            //string msg_id = ettReceiver.MessageID;
                            string msg_id = ettReceiver.ObrId;

                            //ProxyMessage proxy = new ProxyMessage();
                            ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();

                            proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            proxyDeptMsg.Service.RefreshDeptMessage();
                            this.Messages = GetMessages(deptID);
                            ShowMessages(this.Messages);
                        }

                        listView1.Items.Remove(item);

                        readTimer.Stop();
                        this.Hide();
                        readTimer.Start();
                    }

                    #endregion
                }
            }
            if (this.listView1.Items.Count == 0)
            {
                this.Visible = false;
                //isPlay = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Dispose();
            this.Close();

        }

        private void btnMegClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FrmReadAffirm frmAffirm = new FrmReadAffirm();
            if (frmAffirm.ShowDialog() == DialogResult.Yes)//&& frmAffirm.m_userInfo.UserId.Trim() == "admin"
            {
                if (!File.Exists(Application.StartupPath + "\\BarcodeClientConfig.exe"))
                {
                    FrmSetting frmsetting = new FrmSetting(this);
                    frmsetting.ShowDialog();
                }
                else
                {
                    Process.Start(Application.StartupPath + "\\BarcodeClientConfig.exe");
                }
            }
        }


        //由于使用托盘程序不能自动关机，特此使用这个方法

        private bool isExitApp = false;
        private const int WM_QUERYENDSESSION = 17; //0x0011 


        /// <summary> 
        /// 截获消息  
        /// </summary> 
        /// <param name="message"></param> 
        protected override void WndProc(ref Message message)
        {
            if (message.Msg != 0xA3)
            {
                switch (message.Msg)
                {
                    case WM_QUERYENDSESSION:
                        this.notifyIcon1.Visible = false;
                        isExitApp = true;
                        message.Result = (IntPtr)1;

                        break;
                }
                base.WndProc(ref message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void FrmMessages_Load(object sender, EventArgs e)
        {
            string phone = System.Configuration.ConfigurationManager.AppSettings["Message_LabPhoneNum"]; //ConfigHelper.GetSysConfigValueWithoutLogin("Message_LabPhoneNum");
            if (!string.IsNullOrEmpty(phone))
            {
                label1.Text = string.Format("如有疑问，请及时联系检验科({0})", phone);
            }

            readTimer_Tick(sender, e);
        }

        private void tsmiWithout_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(0);//无
            PlaySoundMgr.Instance.SaveConfig("0");
            PlaySoundMgr.Instance.Refresh();
            //刷新配置文件参数
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void tsmiHost_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(1);//主机
            PlaySoundMgr.Instance.SaveConfig("1");
            PlaySoundMgr.Instance.Refresh();
            //刷新配置文件参数
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void tsmiBugle_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(2);//音响
            PlaySoundMgr.Instance.SaveConfig("2");
            PlaySoundMgr.Instance.Refresh();
            //刷新配置文件参数
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwd.Focus();
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConfirm_Click(null, null);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //查询HIS账号验证
            EntityAuditInfo userInfo = new EntityAuditInfo();
            userInfo.UserId = this.txtLogin.Text.Trim();
            userInfo.Password = this.txtPwd.Text.Trim();

            string strAuditType = ConfigurationManager.AppSettings["UserAuthType"];

            IAudit Audit = null;

            switch (strAuditType)
            {
                case "lis"://检验身份验证
                    Audit = new LisAudit();
                    break;
                default:
                    Audit = new LisAudit();
                    break;
            }
            if (Audit != null)
            {
                userInfo = Audit.AuditWhenPrintImpl(userInfo);

                if (userInfo != null)
                {
                    ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                    foreach (EntityDicObrMessageReceive ettReceiver in this.Messages)
                    {
                        #region 处理危急值消息
                        //string pat_id = ettReceiver.MessageContent.Ext1;
                        string pat_id = ettReceiver.ObrMessageContent.ObrValueA;

                        if (!string.IsNullOrEmpty(pat_id))
                        {
                            //删除危急值消息(标志删除)
                            if (ettReceiver != null)
                            {
                                //string msg_id = ettReceiver.MessageID;
                                string msg_id = ettReceiver.ObrId;
                                proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(userInfo, msg_id, pat_id);
                            }

                        }
                        #endregion
                    }
                    proxyDeptMsg.Service.RefreshDeptMessage();
                    this.Messages = GetMessages(deptID);
                    ShowMessages(this.Messages);
                }
                else
                {
                    MessageBox.Show("用户名密码输入不正确！");
                }
            }
            else
            {
                MessageBox.Show("用户名密码输入不正确！");
            }
        }

        /// <summary>
        /// 获取应用程序默认配置
        /// </summary>
        /// <param name="appKey">key值</param>
        /// <param name="defValue">默认value值(当为null)</param>
        /// <returns>value值</returns>
        private string GetAppValueByKey(string appKey, string defValue)
        {
            string kValue = null;

            try
            {
                kValue = ConfigurationManager.AppSettings[appKey];
            }
            catch
            {

            }
            //当默认值为空时,赋值-默认值
            if (string.IsNullOrEmpty(defValue))
            {
                defValue = "nullnull";
            }
            //当value为null时,赋值-默认值
            if (string.IsNullOrEmpty(kValue))
            {
                kValue = defValue;
            }

            return kValue;
        }
        /// <summary>
        /// 窗口状态--最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmWinMax_Click(object sender, EventArgs e)
        {
            ChangeShowOpt(1);//最大化

            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height;

            if (this.Location.X != 0 || this.Location.Y != 0)//坐标
                this.SetDesktopLocation(0, 0);

            if (this.Size.Width != x || this.Size.Height != y)//窗口大小
                this.Size = new System.Drawing.Size(x, y);
        }
        /// <summary>
        /// 窗口状态--还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmWinNor_Click(object sender, EventArgs e)
        {
            ChangeShowOpt(0);//还原
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);
        }

        /// <summary>
        /// 点击Esc键-还原窗口
        /// 点击Ctrl+Q-最大化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMessages_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (tsmWinMax.Checked)//如果目前是最大化,则还原
                {
                    tsmWinNor_Click(null, null);
                }
            }
            else if (e.KeyCode == Keys.Q && e.Control)
            {
                if (tsmWinNor.Checked)//如果目前是还原,则最大化
                {
                    tsmWinMax_Click(null, null);
                }
            }
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangePassword frmPass = new FrmChangePassword(true);//这个需要等用户登入之后进行修改或测试
            frmPass.ShowDialog();
        }


        //private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        //{
        //    if (this.axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped && isPlay)
        //    {

        //        this.axWindowsMediaPlayer1.Ctlcontrols.play();

        //    }
        //} 





        //        AW_HOR_POSITIVE  （    &H1    ）  '从左到右打开窗口  
        //AW_HOR_NEGATIVE  （    &H2    ）  '从右到左打开窗口  
        //AW_VER_POSITIVE  （    &H4    ）  '从上到下打开窗口  
        //AW_VER_NEGATIVE  （    &H8    ）  '从下到上打开窗口  
        //AW_CENTER  　　　（    &H10  ）  '看不出任何效果  
        //AW_HIDE  　　　　（&H10000）  '在窗体卸载时若想使用本函数就得加上此常量    
        //AW_ACTIVATE  　　（&H20000）  '在窗体通过本函数打开后，默认情况下会失去焦点，除非加上本常量    
        //AW_SLIDE  　　　  （&H40000）  '看不出任何效果  
        //AW_BLEND  　　　  （&H80000）  '淡入淡出效果  
    }

    //public class EntityMessageReceiver
    //{
    //    public string Text { get; set; }
    //    public string Title { get; set; }

    //    public EntityMessageReceiver(string title, string text)
    //    {
    //        this.Text = text;
    //        this.Title = title;
    //    }

    //    public override string ToString()
    //    {
    //        return Text;
    //    }
    //}

    /// <summary>
    /// 子类化ListView，在View属性是List的时候出垂直滚动条
    /// </summary>
    public class ListViewEx : ListView
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ShowScrollBar(IntPtr hWnd, int iBar, int bShow);
        const int SB_HORZ = 0;
        const int SB_VERT = 1;
        protected override void WndProc(ref Message m)
        {
            if (this.View == View.List)
            {
                ShowScrollBar(this.Handle, SB_VERT, 1);
                ShowScrollBar(this.Handle, SB_HORZ, 0);
            }
            base.WndProc(ref m);
        }
    }

}
