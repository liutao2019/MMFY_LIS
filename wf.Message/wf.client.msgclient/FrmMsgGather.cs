using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using dcl.client.wcf;
using System.Net;
using System.Xml;
using dcl.client.report;
using Lib.LogManager;
using dcl.client.users;
using dcl.entity;
using System.Linq;


namespace dcl.client.msgclient
{
    public partial class FrmMsgGather : Form
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        internal string deptID;

        /// <summary>
        /// 病人来源
        /// </summary>
        internal string patOriID;

        /// <summary>
        /// 忽略科室(或病区)
        /// </summary>
        internal string patNeglectDep;

        /// <summary>
        /// 医生代码(参传才使用)
        /// </summary>
        internal string patDoctorCode;

        private bool IspanelJCGroupVisible = false;

        private bool IspanelHTGroupVisible = false;

        /// <summary>
        /// 是否有微生物危急值
        /// </summary>
        private bool IsHaveBacCritical = false;

        /// <summary>
        /// 危急值确认模式
        /// </summary>
        private string Verify_Role = string.Empty;

        /// <summary>
        /// 细菌组-物理组ID
        /// </summary>
        private string xijun_TypeIDs { get; set; }

        /// <summary>
        /// 病理组与血库组-物理组ID
        /// </summary>
        private string bingli_TypeIDs { get; set; }

        /// <summary>
        /// 定时器
        /// </summary>
        Timer readTimer = new Timer();

        //由于使用托盘程序不能自动关机，特此使用这个方法

        private bool isExitApp = false;
        private const int WM_QUERYENDSESSION = 17; //0x0011 

        public MessageReceiverCollection Messages { get; set; }


        #region 非托管动态链接库

        /// <summary>
        /// 窗口显示最前端
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 音响设备发声音
        /// </summary>
        /// <param name="pszSound"></param>
        /// <param name="hmod"></param>
        /// <param name="fdwSound"></param>
        /// <returns></returns>
        //  [DllImport("winmm.dll")]
        //    private static extern bool PlaySound(string pszSound, int hmod, int fdwSound);


        /// <summary>
        /// 主机发声音
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        //  [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        // private static extern bool Beep(int frequency, int duration);
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

        #endregion

        public FrmMsgGather()
        {
            InitializeComponent();
            this.btnStartSetting.Checked = ConfigurationManager.AppSettings["RunWhenStart"] == "1";
            this.btnStartSetting.CheckedChanged += new EventHandler(btnStartSetting_CheckedChanged);
            this.gcLookData.Click += new EventHandler(gcLookData_Click);
            this.gvLookData2.DoubleClick += new System.EventHandler(this.gvLookData2_DoubleClick);
            splitContainerControl1.Click += splitContainer1_Click;
            this.FormBorderStyle = FormBorderStyle.None;

            this.groupControl1.MouseMove += new MouseEventHandler(groupBox1_MouseMove);
            this.groupControl1.MouseDown += new MouseEventHandler(groupBox1_MouseDown);

        }

        void groupBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //StopMonitor();
                spanPoint = new Point(e.X, e.Y);
            }
        }

        Point spanPoint = new Point();
        Point centerPoint = new Point();
        void groupBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + (e.X - spanPoint.X), this.Location.Y + (e.Y - spanPoint.Y));
            }
        }
        //停止监控
        void StopMonitor()
        {
            this.timerColor.Stop();
            gvLookData.Appearance.Empty.BackColor = Color.White;
            webBrowser1.Visible = true;
            readTimer.Stop();
        }

        void splitContainer1_Click(object sender, EventArgs e)
        {
            StopMonitor();
        }

        void gcLookData_Click(object sender, EventArgs e)
        {
            StopMonitor();
        }

        void btnStartSetting_CheckedChanged(object sender, EventArgs e)
        {
            CommonTool.UpdateAppSettings("RunWhenStart", this.btnStartSetting.Checked ? "1" : "0");
            CommonTool.runWhenStart(this.btnStartSetting.Checked);
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                RefreshData(true);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        void RefreshData(bool changeStatus)
        {
            #region  实时刷新获取科室代码配置信息，满足节假日功能需求 2018-03-14 SJC
            deptID = GetDepCodeValue();//获取科室代码
            //获取确认角色 是医生确认还是护士确认
            Verify_Role = ConfigurationManager.AppSettings["Verify_Role"];
            if (deptID.Contains("&"))
            {
                string[] strspl = deptID.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                if (strspl.Length > 0)
                {
                    //deptID = strspl[0];
                    deptID = "";
                    for (int i = 0; i < strspl.Length; i++)
                    {
                        deptID += "'" + strspl[i] + "',";
                    }
                    if (!string.IsNullOrEmpty(deptID))
                    {
                        deptID = deptID.TrimEnd(new char[] { ',' });
                    }
                }
            }
            #endregion

            List<EntityPidReportMain> listMessage = GetDtUrgentMsg(deptID);
            List<EntityPidReportMain> msgData = new List<EntityPidReportMain>();
            List<EntityPidReportMain> urgentData = new List<EntityPidReportMain>();
            List<EntityPidReportMain> returnData = new List<EntityPidReportMain>();
            if (listMessage != null && listMessage.Count > 0)
            {
                //情况数据
                this.gcLookData.DataSource = null;
                this.gcLookData2.DataSource = null;
                this.gcLookData3.DataSource = null;

                this.button1.BringToFront();
                this.tsmBatchAudit2.Enabled = true;

                if (IspanelJCGroupVisible)
                {
                    panelJCGroup.Visible = true;
                    msgData = SetMsgTypeTXT(getDtMSGByType(listMessage, "危急"));
                    //增加过滤 
                    if (!string.IsNullOrEmpty(Verify_Role))
                    {
                        if (Verify_Role == "nurse")
                        {
                            msgData = msgData.FindAll(w => w.ObrNurseReadFlag != "1");
                        }
                        else if (Verify_Role == "doctor")
                        {
                            msgData = msgData.FindAll(w => w.ObrDoctorReadFlag != "1");
                        }
                    }
                    this.gcLookData.DataSource = msgData;
                    urgentData= getDtMSGByType(listMessage, "急查");
                    this.gcLookData2.DataSource = urgentData;
                }
                else
                {
                    panelJCGroup.Height = 0;
                    panelJCGroup.Visible = false;
                    //赋值GV
                    this.gcLookData.DataSource = listMessage;
                }

                //判断是否有微生物信息
                if (this.gcLookData.DataSource != null && this.gcLookData.DataSource is List<EntityPidReportMain>
                    && ((List<EntityPidReportMain>)this.gcLookData.DataSource).Where(w => w.ObrValueD == "微生物危急值").ToList().Count > 0)
                {
                    IsHaveBacCritical = true;
                }
                else
                {
                    IsHaveBacCritical = false;
                }


                if (IspanelHTGroupVisible)
                {
                    returnData= getDtMSGByType(listMessage, "回退标本");
                    this.gcLookData3.DataSource = returnData;
                }
                else
                {
                    panelHTGroup.Visible = false;
                }
                bool HaveData = false;
                if (msgData.Count > 0 || urgentData.Count > 0 || returnData.Count > 0)
                {
                    HaveData = true;
                }
             
                //存在数据才弹窗 有声音提示
                if (changeStatus && HaveData)
                {
                    #region 声音提示
                    PlaySoundMgr.Instance.PlaySound();
                    #endregion

                    AnimateWindow(this.Handle, 1000, AW_ACTIVATE | AW_BLEND);//从下到上且不占其它程序焦点   
                    //调整再次弹窗弹在屏幕中间
                    int height = System.Windows.Forms.SystemInformation.WorkingArea.Height;
                    int width = System.Windows.Forms.SystemInformation.WorkingArea.Width;
                    int formheight = this.Size.Height;
                    int formwidth = this.Size.Width;
                    int newformx = width / 2 - formwidth / 2;
                    int newformy = height / 2 - formheight / 2;
                    this.SetDesktopLocation(newformx, newformy);
                    this.Focus();
                    this.gcLookData.RefreshDataSource();
                    if (IspanelJCGroupVisible)
                    {
                        panelJCGroup.Visible = true;
                        this.gcLookData2.RefreshDataSource();
                    }
                    else
                    {
                        panelJCGroup.Visible = false;
                    }

                    if (IspanelHTGroupVisible)
                    {
                        this.gcLookData3.RefreshDataSource();
                    }

                    this.timerColor.Start();
                    webBrowser1.Visible = false;
                }
            }
            else
            {
                IsHaveBacCritical = false;
                gcLookData.DataSource = null;
                gcLookData2.DataSource = null;
                gcLookData3.DataSource = null;
                gvLookData.Appearance.Empty.BackColor = Color.White;
                this.tsmBatchAudit2.Enabled = false;

                this.timerColor.Stop();
                gvLookData.Appearance.Empty.BackColor = Color.White;
                webBrowser1.Visible = true;
                if (changeStatus)
                {
                    this.webBrowser1.Url = null;
                    this.Hide();
                }
            }
        }

        private List<EntityPidReportMain> getDtMSGByType(List<EntityPidReportMain> listObj, string msg_type_txt)
        {
            List<EntityPidReportMain> listReturn = new List<EntityPidReportMain>();
            string OutPatientShowReturn = ConfigurationManager.AppSettings["OutPatientShowReturn"];
            if (listObj.Count > 0 && !string.IsNullOrEmpty(msg_type_txt))
            {
                if (msg_type_txt == "危急")
                {
                    listReturn = listObj.Where(w => w.MsgTypeTxt == msg_type_txt || w.MsgTypeTxt == "自编危急" || w.MsgTypeTxt == "传染病").ToList();
                }
                else if (msg_type_txt == "急查")
                {
                    listReturn = listObj.Where(w => w.MsgTypeTxt == msg_type_txt || w.MsgTypeTxt == "多重耐药").ToList();
                }
                else
                {
                    //门诊不显示回退条码信息
                    if (!string.IsNullOrEmpty(OutPatientShowReturn) && OutPatientShowReturn.ToUpper() == "N")
                    {
                        listReturn = listObj.Where(w => w.MsgTypeTxt == msg_type_txt && w.PidSrcId != "107").ToList();
                    }
                    else
                    {
                        listReturn = listObj.Where(w => w.MsgTypeTxt == msg_type_txt).ToList();
                    }
                }
                return listReturn;
            }
            else
            {
                return listObj;
            }
        }

        private List<EntityPidReportMain> SetMsgTypeTXT(List<EntityPidReportMain> listObj)
        {
            if (listObj != null && listObj.Count > 0)
            {
                for (int i = 0; i < listObj.Count; i++)
                {
                    EntityPidReportMain eyTempSel = listObj[i];

                    //如果细菌组,则显示 MDR预警报告
                    if (!string.IsNullOrEmpty(xijun_TypeIDs) && !string.IsNullOrEmpty(eyTempSel.ItrLabId)
                        && xijun_TypeIDs.Contains(eyTempSel.ItrLabId))
                    {
                        eyTempSel.MsgTypeTxt = "MDR预警报告";
                    }
                    else if (!string.IsNullOrEmpty(bingli_TypeIDs) && !string.IsNullOrEmpty(eyTempSel.ItrLabId)
                        && bingli_TypeIDs.Contains(eyTempSel.ItrLabId))
                    {
                        //如果病理组与血库组,则显示 其他预警报告
                        eyTempSel.MsgTypeTxt = "其他预警报告";
                    }
                }
            }
            return listObj;
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        private ObrMessageReceiveCollection GetMessages(string dep_code)
        {
            if (string.IsNullOrEmpty(dep_code))//如果科室代码为空则返回null
            {
                return null;
            }
            ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
            ObrMessageReceiveCollection msg = proxyObrMsg.Service.GetDeptMessageByDeptCode(dep_code);
            return msg;
        }


        /// <summary>
        /// 读取危急信息(DataTable)
        /// </summary>
        /// <param name="dep_code"></param>
        /// <returns></returns>
        private List<EntityPidReportMain> GetDtUrgentMsg(string dep_code)
        {
            try
            {
                string p_receiver_id = dep_code;//科室ID

                EntityUrgentHistoryUseParame eyWhereParam = new EntityUrgentHistoryUseParame();
                eyWhereParam.ReceiveID = p_receiver_id;
                eyWhereParam.IsNeglectDep = patNeglectDep;
                eyWhereParam.PatOriConfig = patOriID;
                eyWhereParam.PatDocId = patDoctorCode;
                eyWhereParam.MsgType = (ConfigurationManager.AppSettings["msg_type"] ?? "");

                ProxyUrgentObrMessage proxyUrgentObrMsg = new ProxyUrgentObrMessage();
                //获取危急值历史信息(getCacheByDep)
                List<EntityPidReportMain> listUrgentMsg = proxyUrgentObrMsg.Service.GetUrgentHistoryMsgByDep(eyWhereParam);
                return listUrgentMsg;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }

        }

        /// <summary>
        /// 点击关闭时隐藏窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMsgGather_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!isExitApp)
            {
                this.webBrowser1.Url = null;

                e.Cancel = true;
                this.Hide();
                readTimer.Start();
            }
            else
            {
                this.notifyIcon1.Visible = false;//退出时隐藏任务栏
                readTimer.Stop();
            }
        }

        private void FrmMsgGather_Load(object sender, EventArgs e)
        {
            //nw危急值标题
            string NwAppTile = ConfigurationManager.AppSettings["NwAppTile"];
            if (!string.IsNullOrEmpty(NwAppTile))
            {
                xtraTabPage1.Text = NwAppTile;

                if (NwAppTile != "危急值报告提示")
                {
                    colpat_result.Caption = "危急值,MDR及其他预警报告";
                }
            }

            xijun_TypeIDs = ConfigurationManager.AppSettings["xijun_typeids"];
            bingli_TypeIDs = ConfigurationManager.AppSettings["bingli_typeids"];

            panelPrint.Visible = ConfigurationManager.AppSettings["Enable_Print"] == "Y";
            IspanelHTGroupVisible = new ProxyObrMessage().Service.GetConfigValue("ReturnMessages_IsNotify") == "是";

            if (ConfigurationManager.AppSettings["UserAuthType"] == "fsaudit")//如果是佛山市一用户
            {
                try
                {
                    //佛山市一不显示病人电话
                    colpat_tel.Visible = false;
                    colpat_tel2.Visible = false;
                    coldoc_tel.Visible = false;
                    coldoc_tel2.Visible = false;

                    button2.Visible = false;
                    IspanelJCGroupVisible = false;

                    //窗口最大化
                    if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Minimized)
                        this.WindowState = FormWindowState.Maximized;

                    if (SoundControl.IsMuted())
                    {
                        Mute();
                    }
                    for (int temp_up = 1; temp_up <= 50; temp_up++)//系统音量默认最大
                    {
                        VolumeUp();
                    }

                    btnSetting.Visible = false;//隐藏菜单项--设置
                    btnSoundOption.Visible = false;//隐藏菜单项--声音设置
                    toolStripSeparator1.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载时," + ex.Message);
                }
            }
            else
            {
                //危急值提示窗口是否最大化
                string IsMaximizedWindowState = GetAppValueByKey("IsMaximizedWindowState", "N");//如果为null添加默认值N

                if (IsMaximizedWindowState.ToUpper() == "Y" || IsMaximizedWindowState.ToUpper() == "YES")
                {
                    //窗口最大化
                    if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                }

                xtraTabPage1.Text = "消息提示";
                IspanelJCGroupVisible = true;
            }
            
            patNeglectDep = readMSGXml("NEGLECTDEP");//获取xml配置--忽略科室
            patOriID = readMSGXml("ORIID");//获取xml配置--病人来源

            try
            {
                patDoctorCode = "";
                //用于临时存储医生代码信息
                string MSGTEMPDOCCODEINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGTEMPDOCCODE.INI";
                if (System.IO.File.Exists(MSGTEMPDOCCODEINIPath))
                {
                    patDoctorCode = System.IO.File.ReadAllText(MSGTEMPDOCCODEINIPath);

                    if (!string.IsNullOrEmpty(patDoctorCode))//如果有医生代码,则忽略科室
                    {
                        patNeglectDep = "1";
                    }

                    System.IO.File.Delete(MSGTEMPDOCCODEINIPath);//读取完,删除
                }
            }
            catch
            {
            }

            readTimer_Tick(sender, e);
            readTimer.Interval = 15000;
            readTimer.Tick += new EventHandler(readTimer_Tick);
            readTimer.Start();

        }

        /// <summary>
        /// 根据当天是否节假日来获取门诊或是急诊的科室代码(中山三院)
        /// </summary>
        /// <returns></returns>
        private string GetDepCodeValue()
        {
            string return_DeptCode = string.Empty;

            bool ZSSY_HIS = ConfigurationManager.AppSettings["UserAuthType"] == "ZSSY_HIS";

            string Work_Time = ConfigurationManager.AppSettings["Work_Time"];

            bool isWorkTime = IsRetWorkTime(Work_Time);//判断是否上班时间

            #region 中山三院:如果是工作时间且不是节假日，则取门诊(dep_code)的科室代码；其他时间取急诊(holiday_dep_code)的科室代码

            if (ZSSY_HIS)
            {

                if (isWorkTime)
                {
                    ProxyObrMessageContent proxyMsgContent = new ProxyObrMessageContent();
                    bool isHoliday = proxyMsgContent.Service.IsDateHoliday();//判断当天是否是节假日

                    if (isHoliday)
                    {
                        return_DeptCode = ConfigurationManager.AppSettings["holiday_dep_code"];//急诊的科室代码
                    }
                    else
                    {
                        return_DeptCode = ConfigurationManager.AppSettings["dep_code"];//门诊科室代码
                    }

                }
                else
                {
                    return_DeptCode = ConfigurationManager.AppSettings["holiday_dep_code"];//急诊的科室代码
                }
            }
            else
            {
                return_DeptCode = ConfigurationManager.AppSettings["dep_code"];//非中山三院则取门诊的科室代码
            }
            #endregion

            return return_DeptCode;
        }

        private bool IsRetWorkTime(string Work_Time)
        {
            bool isWork_Time = false;
            if (!string.IsNullOrEmpty(Work_Time) && Work_Time == "LG_Hospital")
            {
                //罗岗分院上班时间
                if (
                    (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 12:00:00") && DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 08:30:00")) ||
                    (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 17:00:00") && DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 14:00:00"))
                    )
                {
                    isWork_Time = true;
                }
            }
            else
            {
                //总院上班时间
                if (
                    (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 12:00:00") && DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 08:00:00")) ||
                    (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 17:00:00") && DateTime.Now > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 14:00:00"))
                    )
                {
                    isWork_Time = true;
                }
            }
            return isWork_Time;
        }

        /// <summary>
        /// 菜单--点击关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 菜单--点击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void tsmiWithout_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(0);//无
        }

        private void tsmiHost_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(1);//主机
        }

        private void tsmiBugle_Click(object sender, EventArgs e)
        {
            ChangeSoundOpt(2);//音响
        }

        private void btnMegClose_Click(object sender, EventArgs e)
        {
            this.Hide();
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

        private void btnLookHistory_Click(object sender, EventArgs e)
        {
            FrmSelectHistory selHistory = new FrmSelectHistory(deptID);
            selHistory.patNeglectDep = this.patNeglectDep;//忽视科室
            selHistory.patOriID = this.patOriID;//病人来源
            selHistory.patDoctorcode = this.patDoctorCode;//医生代码
            selHistory.ShowDialog();
        }

        private void timerColor_Tick(object sender, EventArgs e)
        {
            if (panelPrint.BackColor == Color.White || panelPrint.BackColor == Color.Blue)
            {
                panelPrint.BackColor = Color.Red;
            }
            else
            {
                //如果有微生物危急值,则红蓝闪烁
                if (IsHaveBacCritical)
                {
                    panelPrint.BackColor = Color.Blue;
                }
                else
                {
                    panelPrint.BackColor = Color.White;
                }
            }
        }


        /// <summary>
        /// GV菜单-历史查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmLookHistory2_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSelectHistory selHistory = new FrmSelectHistory(deptID);
                selHistory.patNeglectDep = this.patNeglectDep;//忽视科室
                selHistory.patOriID = this.patOriID;//病人来源
                selHistory.ShowDialog();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        /// <summary>
        /// 双击gvLookData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData.GetFocusedRow() == null) return;
            try
            {
                StopMonitor();
                int rowIndex = gvLookData.FocusedRowHandle;
                EntityPidReportMain eyPatData = gvLookData.GetFocusedRow() as EntityPidReportMain;
                string patid = eyPatData.RepId;

                ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                List<EntityPidReportMain> listUrgentInfo = new List<EntityPidReportMain>();
                listUrgentInfo = proxyObrMsg.Service.GetUrgentflagAndPatlookcodeByPatid(patid);

                int paturgentflag = 0;
                paturgentflag = listUrgentInfo[0].RepUrgentFlag.Value;
                string patlookcode = listUrgentInfo[0].RepReadUserId;
                string patlookdate = string.Empty;
                if (listUrgentInfo[0].RepReadDate != null)
                    patlookdate = listUrgentInfo[0].RepReadDate.Value.ToString();

                #region 注释 (暂定)只要内部还未确认，双击就还是可以确认危急值的 2018-03-13
                //if (paturgentflag >= 2 && !string.IsNullOrEmpty(patlookcode) && !string.IsNullOrEmpty(patlookdate))
                //{
                //    if (eyPatData.ObrType.ToString() == "1024" || eyPatData.ObrType.ToString() == "3024")
                //    {
                //        #region 显示报告单 (暂时先注释)
                //        //        if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                //        //        {
                //        //            Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + patid);
                //        //            readTimer.Stop();
                //        //            this.Hide();
                //        //            readTimer.Start();
                //        //        }
                //        //        else
                //        //        {
                //        //            //只用网页打开结果报告单
                //        //            string url = ConfigurationManager.AppSettings["WebSelectUrl"] + patid;
                //        //            string IsOuterwebBrowse = ConfigurationManager.AppSettings["IsOuterwebBrowse"];

                //        //            if (!string.IsNullOrEmpty(IsOuterwebBrowse) && (IsOuterwebBrowse.ToUpper() == "Y" || IsOuterwebBrowse.ToUpper() == "YES"))
                //        //            {
                //        //                //外部浏览报告
                //        //                WebBroswer web = new WebBroswer();
                //        //                web.URL = url;
                //        //                web.IsShowAudit = false;
                //        //                web.ShowDialog();
                //        //            }
                //        //            else
                //        //            {
                //        //                this.webBrowser1.Url = new Uri(url);
                //        //            }
                //        //        }
                //        #endregion

                //        //proxy.Service.RefreshUrgentMessage();
                //        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                //        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                //        RefreshData(false);
                //        return;
                //    }
                //}
                #endregion

                if (eyPatData != null)
                {
                    if (eyPatData.ObrType.ToString() == "1024" || eyPatData.ObrType.ToString() == "3024"
                        || eyPatData.ObrType.ToString() == "2024")
                    {
                        #region 处理危急值消息
                        string pat_id = eyPatData.RepId.ToString();

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
                            CheckerInfo = new EntityAuditInfo();
                            CheckerInfo.UserId = "-1";
                            CheckerInfo.UserName = "-1";
                            CheckerInfo.UserStfId = "-1";
                            CheckerInfo.Password = "-1";
                            CheckerInfo.IsSaveMsg = true;
                            CheckerInfo.MsgAffirmType = "1";
                            CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());

                        }
                        else
                        {
                            //验证用户
                            FrmReadAffirm frmRA = null;

                            //验证时,是否显示编辑框
                            if (TextVisible.ToUpperInvariant() == "Y" || TextVisible.ToUpperInvariant() == "YES")
                            {
                                frmRA = new FrmReadAffirm(true);
                            }
                            else
                            {
                                frmRA = new FrmReadAffirm();
                            }
                            frmRA.SetDocVisiable(DocVisible);
                            if (frmRA.ShowDialog() != DialogResult.Yes)
                            {
                                return;
                            }
                            CheckerInfo = frmRA.m_userInfo;
                            if (!string.IsNullOrEmpty(Verify_Role) && !string.IsNullOrEmpty(CheckerInfo.UserRole) && Verify_Role != CheckerInfo.UserRole)
                            {
                                MessageBox.Show("角色不正确！");
                                return;
                            }
                            if (CheckerInfo != null)
                            {
                                CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());
                                CheckerInfo.IsSaveMsg = true;
                                CheckerInfo.MsgAffirmType = "1";
                            }
                        }

                        #endregion
                        #region 可能存在危急值已被确认 还会再次确认问题
                        EntityDicObrMessageContent content = new EntityDicObrMessageContent();
                        content.ObrValueA = pat_id;
                        ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                        List<EntityDicObrMessageContent> listMessageContent = proxyMsg.Service.GetMessageByCondition(content);
                        if (listMessageContent.Count > 0 && listMessageContent[0].DelFlag)
                        {
                            MessageBox.Show("该条数据已被确认！");
                            RefreshData(false);
                            this.readTimer.Start();
                            return;
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(pat_id))
                        {
                            try
                            {
                                string msg_id = eyPatData.ObrIdMsg;
                                //ProxyMessage proxy = new ProxyMessage();
                                proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                                //proxy.Service.RefreshUrgentMessage();
                                ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                                proxyUrgObrMsg.Service.RefreshUrgentMessage();

                                RefreshData(false);

                            }
                            catch (Exception ex)
                            {
                                Lib.LogManager.Logger.LogException(ex);
                                //throw;
                            }

                            if (eyPatData.ObrType.ToString() == "1024" || eyPatData.ObrType.ToString() == "3024")
                            {
                                #region 显示报告单 (暂时先注释)
                                //if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                                //{
                                //    Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + pat_id);
                                //    readTimer.Stop();
                                //    this.Hide();
                                //    readTimer.Start();
                                //}
                                //else
                                //{
                                //    //只用网页打开结果报告单
                                //    string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;
                                //    string IsOuterwebBrowse = ConfigurationManager.AppSettings["IsOuterwebBrowse"];

                                //    if (!string.IsNullOrEmpty(IsOuterwebBrowse) && (IsOuterwebBrowse.ToUpper() == "Y" || IsOuterwebBrowse.ToUpper() == "YES"))
                                //    {
                                //        //外部浏览报告
                                //        WebBroswer web = new WebBroswer();
                                //        web.URL = url;
                                //        web.IsShowAudit = false;
                                //        web.ShowDialog();
                                //    }
                                //    else
                                //    {
                                //        this.webBrowser1.Url = new Uri(url);
                                //    }
                                //}
                                #endregion
                            }

                            this.readTimer.Start();
                        }
                        #endregion
                    }
                    else if (eyPatData.ObrType.ToString() == "4096")
                    {
                        #region 急查
                        string pat_id = eyPatData.RepId;

                        //AuditInfo CheckerInfo = new AuditInfo();
                        EntityAuditInfo CheckerInfo = new EntityAuditInfo();
                        CheckerInfo.UserId = "-1";
                        CheckerInfo.UserName = "-1";
                        CheckerInfo.UserStfId = "-1";
                        CheckerInfo.Password = "-1";
                        CheckerInfo.IsSaveMsg = true;
                        CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());
                        CheckerInfo.ExetMsg = "急查";
                        if (!string.IsNullOrEmpty(pat_id))
                        {
                            string msg_id = eyPatData.ObrIdMsg;
                            //ProxyMessage proxy = new ProxyMessage();

                            proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);

                            //proxy.Service.RefreshUrgentMessage();
                            ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                            proxyUrgObrMsg.Service.RefreshUrgentMessage();

                            RefreshData(false);

                            #region 显示报告单（暂时先注释)
                            //if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                            //{
                            //    Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + pat_id);
                            //    readTimer.Stop();
                            //    this.Hide();
                            //    readTimer.Start();
                            //}
                            //else
                            //{
                            //    //只用网页打开结果报告单
                            //    //gvLookData.DeleteRow(rowIndex);
                            //    string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;
                            //    this.webBrowser1.Url = new Uri(url);
                            //}
                            #endregion
                            this.readTimer.Start();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        
        }


        public string GetClientIPv4()
        {
            string ipv4 = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = IPA.ToString();
                    break;
                }
            }
            return ipv4;
        }

        private void gvLookData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            EntityPidReportMain eyPat = this.gvLookData.GetRow(e.RowHandle) as EntityPidReportMain;
            if (eyPat != null)
            {
                if (eyPat.ObrType.ToString() == "4096")//急查报告为红色
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 批量审核GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmBatchAudit2_Click(object sender, EventArgs e)
        {
            StopMonitor();

            if (this.gvLookData.RowCount > 0)
            {
                List<EntityPidReportMain> listAudit = gcLookData.DataSource as List<EntityPidReportMain>;
                if (listAudit != null && listAudit.Count > 0)
                {

                    #region 用户验证

                    //是否用户验证
                    string IsUserValidate = GetAppValueByKey("IsUserValidate", "Y");//如果为null添加默认值Y
                                                                                    //验证时是否显示文本框
                    string TextVisible = GetAppValueByKey("TextVisible", "NO");//如果为null添加默认值NO

                    EntityAuditInfo CheckerInfo = null;

                    //验证用户
                    FrmReadAffirm frmRA = null;

                    //验证时,是否显示编辑框
                    if (TextVisible.ToUpperInvariant() == "Y" || TextVisible.ToUpperInvariant() == "YES")
                    {
                        frmRA = new FrmReadAffirm(true);
                    }
                    else
                    {
                        frmRA = new FrmReadAffirm();
                    }

                    if (frmRA.ShowDialog() != DialogResult.Yes)
                    {
                        readTimer_Tick(null, null);
                        readTimer.Start();//批量审核结束后,恢复接收新信息
                        return;
                    }
                    CheckerInfo = frmRA.m_userInfo;

                    #endregion

                    try
                    {
                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();

                        //删除危急值消息(标志删除)
                        foreach (var infoAudit in listAudit)
                        {
                            if (infoAudit.ObrType.ToString() == "1024" || infoAudit.ObrType.ToString() == "2024" || infoAudit.ObrType.ToString() == "3024")//只审核危急报告
                            {
                                string msg_id = infoAudit.ObrIdMsg;

                                string pat_id = infoAudit.RepId;

                                proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            }
                        }
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                    readTimer_Tick(null, null);

                    readTimer.Start();//批量审核结束后,恢复接收新信息
                }
            }

            readTimer.Start();//批量审核结束后,恢复接收新信息
        }

        /// <summary>
        /// 操作系统关闭时，关闭应用程序
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != 0xA3)
            {
                switch (m.Msg)
                {
                    case WM_QUERYENDSESSION:
                        this.notifyIcon1.Visible = false;
                        isExitApp = true;
                        m.Result = (IntPtr)1;

                        break;
                }
                base.WndProc(ref m);
            }
        }

        private void FrmMsgGather_Shown(object sender, EventArgs e)
        {
            //定时检测自动更新
            timerAutoUpdate.Start();

            panelJCGroup.Visible = IspanelJCGroupVisible;
            panelHTGroup.Visible = IspanelHTGroupVisible;
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelJCGroup.Visible = IspanelJCGroupVisible;
            panelHTGroup.Visible = IspanelHTGroupVisible;
            this.webBrowser1.Url = null;
            this.timerColor.Stop();

            this.Hide();
            readTimer.Start();
        }

        /// <summary>
        /// 读取xml配置根据键名获取键值
        /// </summary>
        /// <param name="tKey">键名</param>
        /// <returns>为空时则为默认值</returns>
        private string readMSGXml(string tKey)
        {
            string tValue = "";

            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGSETTING.XML";

            try
            {
                if (!File.Exists(filepath))
                {
                    return tValue;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(tKey))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tValue = row[tKey].ToString();
                        ds.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return tValue;
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangePassword frmPass = new FrmChangePassword(true);
            frmPass.ShowDialog();
        }

        /// <summary>
        /// 双击处理急查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLookData2_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData2.GetFocusedRow() == null)
                return;
            StopMonitor();
            int rowIndex = gvLookData2.FocusedRowHandle;
            EntityPidReportMain eyPatsData = gvLookData2.GetFocusedRow() as EntityPidReportMain;

            if (eyPatsData != null)
            {
                if (eyPatsData.ObrType.ToString() == "1024")
                {

                }
                else if (eyPatsData.ObrType.ToString() == "4096")
                {
                    #region 急查
                    string pat_id = eyPatsData.RepId;

                    EntityAuditInfo CheckerInfo = new EntityAuditInfo();
                    CheckerInfo.UserId = "-1";
                    CheckerInfo.UserName = "-1";
                    CheckerInfo.UserStfId = "-1";
                    CheckerInfo.Password = "-1";
                    CheckerInfo.IsSaveMsg = true;
                    CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());
                    CheckerInfo.ExetMsg = "急查";
                    if (!string.IsNullOrEmpty(pat_id))
                    {
                        string msg_id = eyPatsData.ObrIdMsg;

                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();

                        proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);

                        //proxy.Service.RefreshUrgentMessage();
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                        RefreshData(false);

                        #region 网页端打开报告单（暂时注释 2018-01-09）
                        //if (File.Exists(Application.StartupPath + "\\Lis.ReportQuery.exe"))
                        //{
                        //    Process.Start(Application.StartupPath + "\\Lis.ReportQuery.exe", "pat_id;" + pat_id);
                        //    readTimer.Stop();
                        //    this.Hide();
                        //    readTimer.Start();
                        //}
                        //else
                        //{
                        //    //只用网页打开结果报告单
                        //    //gvLookData.DeleteRow(rowIndex);
                        //    string url = ConfigurationManager.AppSettings["WebSelectUrl"] + pat_id;
                        //    this.webBrowser1.Url = new Uri(url);
                        //}
                        #endregion

                        this.readTimer.Start();
                    }
                    #endregion
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!isMaxWindows)
            {
                ShowWindow(this.Handle, (int)ShowWindowOption.SW_MAXIMIZE);
                isMaxWindows = true;

            }
            else
            {
                ShowWindow(this.Handle, (int)ShowWindowOption.SW_SHOWNORMAL);
                isMaxWindows = false;
            }
        }


        bool isMaxWindows = false;

        ///<summary>向指定窗体句柄发送显示状态</summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="nCmdShow">窗体显示参数(如下参数例表)</param>
        /// <returns>是否成功</returns>
        ///<remarks>
        ///<para> nCmdShow:为以下其参数例表</para>
        ///</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// ShowWindow函数枚举值
        /// </summary>
        public enum ShowWindowOption
        {
            /// <summary>
            /// 隐藏窗口，活动状态给令一个窗口
            /// </summary>
            SW_HIDE = 0,
            /// <summary>
            /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_SHOWNORMAL = 1,
            /// <summary>
            /// 最小化窗口，并将其激活
            /// </summary>
            SW_SHOWMINIMIZED = 2,
            /// <summary>
            /// 最大化窗口，并将其激活
            /// </summary>
            SW_SHOWMAXIMIZED = 3,
            /// <summary>
            /// 最大化窗口，并将其激活
            /// </summary>
            SW_MAXIMIZE = 3,
            /// <summary>
            /// 用最近的大小和位置显示一个窗口，同时不改变活动窗口
            /// </summary>
            SW_SHOWNOACTIVATE = 4,
            /// <summary>
            /// 用当前的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_SHOW = 5,
            /// <summary>
            /// 最小化窗口，活动状态给令一个窗口
            /// </summary>
            SW_MINIMIZE = 6,
            /// <summary>
            /// 最小化一个窗口，同时不改变活动窗口
            /// </summary>
            SW_SHOWMINNOACTIVE = 7,
            /// <summary>
            /// 用当前的大小和位置显示一个窗口，不改变活动窗口
            /// </summary>
            SW_SHOWNA = 8,
            /// <summary>
            /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_RESTORE = 9


        }

        /// <summary>
        /// 定时检测是否有更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAutoUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                //本地配置文件路径
                string AutoUpdateCheckFilePath_str = Application.StartupPath + "\\" + "AutoUpdateCheck.exe";
                if (System.IO.File.Exists(AutoUpdateCheckFilePath_str))
                {
                    Process.Start(AutoUpdateCheckFilePath_str);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gvLookData3_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData3.GetFocusedRow() == null) return;
            StopMonitor();
            int rowIndex = gvLookData3.FocusedRowHandle;

            EntityPidReportMain eyPatData = gvLookData3.GetFocusedRow() as EntityPidReportMain;

            if (eyPatData != null)
            {
                #region 处理回退标本
                string pat_bar_code = eyPatData.RepBarCode;

                if (!string.IsNullOrEmpty(pat_bar_code))
                {
                    FrmReadAffirm frmRA = new FrmReadAffirm("回退标本");
                    if (frmRA.ShowDialog() != DialogResult.Yes)
                    {
                        return;
                    }
                    EntityAuditInfo CheckerInfo = frmRA.m_userInfo;
                    string strOperatorID = CheckerInfo.UserId;
                    string strOperatorName = CheckerInfo.UserName;
                    string currentServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string bc_remark = "处理回退条码_" + pat_bar_code;

                    ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                    proxyObrMsg.Service.HandleReturnMessage(pat_bar_code, strOperatorID, strOperatorName, currentServerTime, bc_remark);

                    //proxy.Service.RefreshUrgentMessage();
                    ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                    proxyUrgObrMsg.Service.RefreshUrgentMessage();

                    RefreshData(false);
                    this.readTimer.Start();
                }
                #endregion
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLookData.GetFocusedRow() == null) return;
                int rowIndex = gvLookData.FocusedRowHandle;

                EntityPidReportMain eyPatData = gvLookData.GetFocusedRow() as EntityPidReportMain;

                string pat_id = eyPatData.RepId;
                string itr_rep_flag = eyPatData.ItrReportType;
                string itr_rep_id = eyPatData.ItrReportId;

                if (string.IsNullOrEmpty(pat_id) || string.IsNullOrEmpty(itr_rep_id))
                {
                    lis.client.control.MessageDialog.Show("找不到当前仪器的打印模版", "提示");
                    return;
                }

                try
                {
                    //打印清单报表 新代码  
                    EntityDCLPrintParameter paramter = new EntityDCLPrintParameter();
                    paramter.ReportCode = itr_rep_id;
                    paramter.RepId = pat_id;

                    Dictionary<String, Object> keyValue = new Dictionary<String, Object>();
                    keyValue.Add("ItrRepFlag", itr_rep_flag);
                    paramter.CustomParameter = keyValue;

                    DCLReportPrint.Print(paramter);//单打

                }
                catch (ReportNotFoundException ex1)
                {
                    lis.client.control.MessageDialog.Show(ex1.MSG);
                }
                catch (Exception ex2)
                {
                    Logger.LogException(ex2);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void gvLookData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            EntityPidReportMain eyPat = this.gvLookData.GetRow(e.RowHandle) as EntityPidReportMain;
            if (eyPat != null && e.Column != null && e.Column.FieldName != null)
            {
                if (eyPat.MsgTypeTxt == "传染病")//传染病为单元格底色为绿色
                {
                    e.Appearance.BackColor = Color.Green;
                }
            }
        }
    }
}
