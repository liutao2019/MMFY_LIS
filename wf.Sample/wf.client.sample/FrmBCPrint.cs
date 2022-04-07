using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using dcl.client.common;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmBCPrint : FrmCommonExt
    {
        string path = PathManager.SettingPath + @"barcodeparam.ini";
        string m_strTJPath = PathManager.SettingPath + @"TJbarcodeparam.ini";
        string m_strMZPath = PathManager.SettingPath + @"MZbarcodeparam.ini";
        public FrmBCPrint(): this(false, "", "")
        { }

        public FrmBCPrint(bool isAlone, string strType, string strTjIds)
        {
            InitializeComponent();
            if (DesignMode)
                return;
            this.isAlone = isAlone;
            this.strType = strType;
            this.strTjIds = strTjIds;
            if (isAlone)
            {
                bcPrintControl.IsAlone = true;
                tabMain.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                if (strType == "住院")
                {
                    bcPrintControl.CheckPower = false;
                    if (LocalSetting.Current.Setting.DeptID != null && LocalSetting.Current.Setting.DeptID.Trim() != "")
                    {
                        List<EntityDicPubDept> listDept = CacheClient.GetCache<EntityDicPubDept>();

                        int depIndex = listDept.FindIndex(w => w.DeptId == LocalSetting.Current.Setting.DeptID);

                        if (depIndex > -1)
                            LocalSetting.Current.Setting.DeptName = listDept[depIndex].DeptParentId;
                    }
                }
                else
                {
                    bcPrintControl2.CheckPower = false;
                    bcPrintControl2.IsFormatTJCode = true;
                }
            }

            InitBarcodePower();


        }

        /// <summary>
        /// 体检条码调用
        /// </summary>
        /// <param name="strTjIds"></param>
        public FrmBCPrint(string[] strTjIds, string type)
        {
            InitializeComponent();
            if (DesignMode)
                return;
            this.isAlone = true;
            this.strType = type;
            this.strTjIds = strTjIds[3];

            InitBarcodePower();

            BCPrintControl bcPrintCon = bcPrintControl1;

            if (type == "体检")
            {
                bcPrintCon = bcPrintControl2;
                bcPrintControl2.txtOutPatientsEnd.Visible = true;
                bcPrintControl2.lblOutPatients.Visible = true;
            }

            bcPrintCon.CheckPower = false;
            bcPrintCon.DoctorId = strTjIds[1];
            bcPrintCon.DoctorCode = strTjIds[1];
            bcPrintCon.DoctorName = strTjIds[2];
            bcPrintCon.IsAlone = true;
            bcPrintCon.IsFormatTJCode = false;
        }



        private void InitBarcodePower()
        {
            List<IStep> stepList = new List<IStep>();
            stepList.Add(new PrintStep());
            stepList.Add(new SamplingStep());
            stepList.Add(new SendStep());
            stepList.Add(new ReachStep());

            stepList.Add(new ReceiveStep());
            StepFactory.StepList = stepList;

        }
        bool MinDownloadBarWin = false;
        /// <summary>
        /// 单独条码系统时传参下载条码时窗体是否最小化
        /// </summary>
        public void DownloadBarWinStatues(bool min)
        {
            MinDownloadBarWin = min;
            if (min)
            {
                //this.Size = new Size(150, 50);
                this.MinimizeBox = false;
                System.Drawing.Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(this);//实例化一个当前窗口的对象
                Rectangle rect2 = new System.Drawing.Rectangle(rect.Right - 150 - 1, rect.Bottom - 30 - 1, 150, 30);//为实例化的对象创建工作区域
                this.SetBounds(rect2.X, rect2.Y, rect2.Width, rect2.Height);//设置当前窗体的边界
                this.WindowState = FormWindowState.Normal;
            }
        }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string DeptCode
        {
            get { return this.bcPrintControl.DeptCode; }
            set
            {
                bcPrintControl.DeptCode = value;
                bcPrintControl.RefreshReturnBarcodeMessage();//科室改变后,马上刷新回退条码信息
                bcPrintControl1.DeptCode = value;
            }
        }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatID
        {
            get { return this.bcPrintControl.PatID; }
            set
            {
                bcPrintControl.PatID = value;
                bcPrintControl1.PatID = value;
            }
        }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName
        {
            get { return this.bcPrintControl.DoctorName; }
            set
            {
                bcPrintControl.DoctorName = value;
                bcPrintControl1.DoctorName = value;
                bcPrintControl2.DoctorName = value;
            }
        }

        /// <summary>
        /// 医生工号
        /// </summary>
        public string DoctorCode
        {
            get { return this.bcPrintControl.DoctorCode; }
            set
            {
                bcPrintControl.DoctorCode = value;
                bcPrintControl1.DoctorCode = value;
                bcPrintControl2.DoctorCode = value;
            }
        }

        private bool isAlone = false;

        private string strType = string.Empty;

        private string strTjIds = string.Empty;

        private void FrmBCPrint_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            if (isAlone)//住院条码不分权限
            {
                if (strType == "住院")
                {
                    xtraTabPage2.PageVisible = xtraTabPage3.PageVisible = xtraTabPage4.PageVisible = false; //TO-DO:住院条码

                    if (!MinDownloadBarWin)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    HideCloseButton();
                    DownLoadZYBarcode();

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_DisinfectBarCode") == "开")
                    {
                        tabMain.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
                        xtraTabPage3.PageVisible = true;
                        xtraTabPage3.Text = "消毒效果试验条码";
                        bcManual1.IsAlone = true;
                        bcManual1.DeptCode = this.DeptCode;
                    }

                    return;
                }
                else if (strType == "体检")
                {
                    xtraTabPage1.PageVisible = xtraTabPage2.PageVisible = xtraTabPage3.PageVisible = false; //TO-DO:体验条码
                    if (!MinDownloadBarWin)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    HideCloseButton();
                    DownLoadTJBarcode();//下载体检条码数据
                    return;
                }
                else if (strType == "门诊")
                {
                    xtraTabPage1.PageVisible = xtraTabPage4.PageVisible = xtraTabPage3.PageVisible = false; //TO-DO:体验条码
                    if (!MinDownloadBarWin)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    HideCloseButton();

                    DownLoadMZBarcode();//下载体检条码数据
                    return;
                }
            }
            if (strType == "手工条码")
            {
                xtraTabPage1.PageVisible = xtraTabPage4.PageVisible = xtraTabPage2.PageVisible = false;
                if (!MinDownloadBarWin)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                HideCloseButton();
                return;
            }

            string result = "";
            if (File.Exists(path))
                result = File.ReadAllText(path);
            string[] parm = result.Split(' ');

            if (parm.Length > 3)
            {
                this.DeptCode = parm[1];
            }

            bool needSkipPower = UserInfo.SkipPower;
            UserInfo.SkipPower = false;

            bool inPatientPower = UserInfo.HaveFunction(216);
            bool outPatientPower = UserInfo.HaveFunction(217);
            bool manualPower = UserInfo.HaveFunction(218);
            bool tjPower = UserInfo.HaveFunction(string.Empty, "TJBarcode");
            bool queueNumberPower = UserInfo.HaveFunction(string.Empty, "QueueNumber");
            UserInfo.SkipPower = needSkipPower;

            if (!inPatientPower)
                xtraTabPage1.PageVisible = false;
            if (!outPatientPower)
                xtraTabPage2.PageVisible = false;
            if (!manualPower)
                xtraTabPage3.PageVisible = false;

            if (!tjPower)
                xtraTabPage4.PageVisible = false;
            if (!queueNumberPower)
                xtraTabPage5.PageVisible = false;
            selectTab = tabMain.SelectedTabPage;

            tabMain.SelectedPageChanged += tabMain_SelectedPageChanged;
        }

        [DllImport("User32.dll")]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll")]
        public static extern UInt32 SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        public const UInt32 SHDB_SHOW = 0x0001;
        public const UInt32 SHDB_HIDE = 0x0002;
        public const int GWL_STYLE = -16;
        public const UInt32 WS_NONAVDONEBUTTON = 0x00010000;

        private void HideCloseButton()
        {
            // this.ControlBox = false; //ControlBox
            //UInt32 dwStyle = GetWindowLong(Handle, GWL_STYLE);
            //if ((dwStyle & WS_NONAVDONEBUTTON) == 0)
            //    SetWindowLong(Handle, GWL_STYLE, dwStyle | WS_NONAVDONEBUTTON);
        }

        //protected override void WndProc(ref Message msg)
        //{

        //    if (msg.Msg == MessageHelper.WM_START)
        //    {
        //        //DownLoadZYBarcode();

        //    }
        //    base.WndProc(ref msg);

        //}

        private void DownLoadZYBarcode()
        {
            string result = "";
            if (File.Exists(path))
                result = File.ReadAllText(path);

            if (string.IsNullOrEmpty(result))
            {
                MessageDialog.Show("参数传递出错");
                return;
            }

            string[] parm = result.Split(' ');
            //住院条码
            if (parm == null || parm.Length < 3)
            {
                MessageDialog.Show("参数传递出错");
                return;
            }

            if (parm[1].Trim() == "-1" && parm[2].Trim() == "-1")//住院号与部门代码不可同时为-1
            {
                MessageDialog.Show("参数传递出错");
                return;
            }
            if (parm[1].Trim() == "-1")
                parm[1] = "";

            if (parm[2].Trim() == "-1")
                parm[2] = "";

            string docID = string.Empty;

            if (parm.Length > 4)
            {
                docID = parm[4].Trim();
            }


            //if (DeptCode != parm[1] || PatID != parm[2] || DoctorName != parm[3] || docID != DoctorCode)
            //{

            this.DeptCode = parm[1].Trim();
            this.PatID = parm[2].Trim();
            this.DoctorName = parm[3].Trim();
            DoctorCode = docID;
            if (!(ConfigHelper.GetSysConfigValueWithoutLogin("AutoDownloadBarCode") == "否"))
            {
                bcPrintControl.DownloadAdvice(PatID);
            }
        }


        /// <summary>
        /// 下载体检条码
        /// </summary>
        private void DownLoadTJBarcode()
        {
            string result = "";
            if (File.Exists(m_strTJPath))
            {
                result = File.ReadAllText(m_strTJPath);
            }

            if (string.IsNullOrEmpty(result))
            {
                MessageDialog.Show("参数传递出错");
                return;
            }

            string[] parm = result.Split(' ');
            //体检条码
            if (parm == null || parm.Length < 3)
            {
                MessageDialog.Show("参数传递出错！");
                return;
            }

            //判断每次去读取INI文件里的参数，是不是还和上一次读取的一样;
            //如果一样，则返回
            //上次读取的参数信息会保留在界面
            if (PatID != parm[3] || DoctorName != parm[2])
            {
                this.DoctorCode = parm[1].Trim();
                this.DoctorName = parm[2].Trim();
                this.PatID = parm[3].Trim();

                bcPrintControl2.cbTypeID.SelectedIndex = 0;

                bcPrintControl2.DownloadTjAdvice(this.PatID);
            }

        }

        /// <summary>
        /// 下载门诊条码
        /// </summary>
        private void DownLoadMZBarcode()
        {
            string result = "";
            if (File.Exists(m_strMZPath))
            {
                result = File.ReadAllText(m_strMZPath);
            }

            if (string.IsNullOrEmpty(result))
            {
                MessageDialog.Show("参数传递出错");
                return;
            }

            string[] parm = result.Split(' ');
            //门诊条码
            if (parm == null || parm.Length < 3)
            {
                MessageDialog.Show("参数传递出错！");
                return;
            }

            //判断每次去读取INI文件里的参数，是不是还和上一次读取的一样;
            //如果一样，则返回
            //上次读取的参数信息会保留在界面
            if (PatID != parm[3] || DoctorName != parm[2])
            {

                this.DoctorCode = parm[1].Trim();
                this.DoctorName = parm[2].Trim();
                this.PatID = parm[3].Trim();

                int temp_selIndex = 0;
                //如果有第五个参数,则选择下载类型
                if (parm.Length >= 5 && !string.IsNullOrEmpty(parm[4].Trim()) && int.TryParse(parm[4].Trim(), out temp_selIndex))
                {
                    bcPrintControl1.cbTypeID.SelectedIndex = temp_selIndex;//选择门诊下载类型(如果,发票号,病人号等)
                }
                else
                {
                    bcPrintControl1.cbTypeID.SelectedIndex = 0;
                }

                bcPrintControl1.DownloadTjAdvice(this.PatID);
            }

        }


        private void FrmBCPrint_Paint(object sender, PaintEventArgs e)
        {
            if (MinDownloadBarWin)
            {
                if (isAlone)
                {
                    if (strType == "住院")
                    {
                        DownLoadZYBarcode();
                    }
                    else if (strType == "体检")
                    {
                        DownLoadTJBarcode();
                    }
                    else if (strType == "门诊")
                    {
                        DownLoadMZBarcode();
                    }
                }
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                {

                    if (isAlone)
                    {
                        if (strType == "住院" && ConfigHelper.GetSysConfigValueWithoutLogin("Allow_Multi_BarCodeClient") != "是")
                        {
                            DownLoadZYBarcode();
                        }
                        else if (strType == "体检")
                        {
                            DownLoadTJBarcode();
                        }
                        else if (strType == "门诊")
                        {
                            DownLoadMZBarcode();
                        }
                    }
                }
            }

        }

        private void FrmBCPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (isAlone)
            //{
            //    string autoupdatePath = Application.StartupPath + "\\autoupdate.exe";

            //    if (File.Exists(autoupdatePath))
            //    {
            //        Process.Start(autoupdatePath);
            //    }

            //    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\程序\\广州创惠计算机科技有限公司\\创惠医学检验信息系统-条码检验查询.appref-ms";

            //    if (File.Exists(filePath))
            //    {
            //        Process.Start(filePath);
            //    }
            //}

        }

        DevExpress.XtraTab.XtraTabPage selectTab = new DevExpress.XtraTab.XtraTabPage();


        private void tabMain_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (!isAlone)
            {
                if (tabMain.SelectedTabPage.Text == "手工条码")
                {
                    List<EntitySysFunction> dtUserFunc = UserInfo.entityUserInfo.Func;
                    if (UserInfo.isAdmin == true)
                    {
                        dtUserFunc = UserInfo.entityUserInfo.AllFunc;
                    }

                    if (dtUserFunc != null && dtUserFunc.Count > 0)
                    {
                        List<EntitySysFunction> listUserFunc = dtUserFunc.Where(w => w.FuncId == 218).ToList();
                        if (listUserFunc.Count > 0)
                        {
                            if (listUserFunc[0].FuncValiuser.Trim() == "1")
                            {
                                FrmCheckPassword fcp = new FrmCheckPassword();
                                if (fcp.ShowDialog() == DialogResult.OK)
                                {
                                    if (fcp.OperatorID != UserInfo.loginID)
                                    {
                                        lis.client.control.MessageDialog.Show("非当前用户！", "提示");
                                        tabMain.SelectedTabPage = selectTab;
                                        return;
                                    }
                                }
                                else
                                {
                                    tabMain.SelectedTabPage = selectTab;
                                    return;
                                }
                            }
                        }
                    }

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("BCManual_allow_getMzPatInfo") != "否")
                    {
                        if (bcPrintControl1.GetFocusDataRow() != null)
                        {
                            bcManual1.setControlValue(bcPrintControl1.GetFocusDataRow());
                            bcManual1.m_mthSetSoureControl(bcPrintControl1);//门诊条码打印控制传进去控制
                        }
                    }
                }
                else if (tabMain.SelectedTabPage.Text == "体检条码")
                {
                    bcPrintControl2.txtOutPatientsEnd.Visible = true;
                    bcPrintControl2.lblOutPatients.Visible = true;
                }
                else
                {
                    bcManual1.m_mthClearSoureControl();
                }
                selectTab = tabMain.SelectedTabPage;
            }
        }

        private void FrmBCPrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isAlone)
            {
                try
                {
                    dcl.client.notifyclient.BcSamplToReceiveTATNotifyHelper.Current.stop();
                    //关闭条码系统，同时关闭危急值程序
                    string isAutoCloseUrgentClient = System.Configuration.ConfigurationManager.AppSettings["isAutoCloseUrgentClient"];

                    //如果为null添加默认值NO
                    if (string.IsNullOrEmpty(isAutoCloseUrgentClient))
                        isAutoCloseUrgentClient = "N";

                    if (isAutoCloseUrgentClient.ToUpperInvariant() == "Y" || isAutoCloseUrgentClient.ToUpperInvariant() == "YES")
                    {
                        Process[] processes = Process.GetProcessesByName("dcl.client.msgclient");
                        for (int p = 0; processes.Length > p; p++)
                        {
                            processes[p].Kill();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void SetMaunaText()
        {
            xtraTabPage3.Text = "院感条码";
            bcManual1.SetMaunaText();
        }

        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }
    }
}