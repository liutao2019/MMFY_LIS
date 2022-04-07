using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using dcl.client.frame;
using dcl.client.sample;
using dcl.client.common;
using dcl.client.resultquery;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using System.Text;
using Lib.LogManager;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.LookAndFeel;
using wf.ClientEntrance;

namespace wf.Auxiliary
{
    public class Login
    {
        Dictionary<String, String> DllMapping = new Dictionary<String, String>();
        static string path = PathManager.SettingPath;
        static string pathForOutlink = PathManager.OutLinkPath;

        public Login()
        {
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly newAssembly = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;

                // Get the DLL name from the Name argument.
                string[] nameParts = args.Name.Split(',');
                string dllName = nameParts[0];
                string downloadGroupName = DllMapping[dllName];

                // 下载所需要的文件 
                try
                {
                    deploy.DownloadFileGroup(downloadGroupName);
                }
                catch (DeploymentException de)
                {
                    lis.client.control.MessageDialog.Show("Downloading file group failed. Group name: " + downloadGroupName + "; DLL name: " + args.Name);
                    throw (de);
                }

                try
                {
                    newAssembly = Assembly.LoadFile(Application.StartupPath + @"\" + dllName + ".dll");
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
            else
            {
                throw (new Exception("Cannot load assemblies dynamically - application is not deployed using ClickOnce."));
            }

            return (newAssembly);

        }


        #region 更改条码打印程序桌面启动方式图标
        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);
        private void ModifyBarcodePrintDesktopIcon()
        {
            try
            {
                StringBuilder sbPath = new StringBuilder(260);
                SHGetFolderPath(IntPtr.Zero, 0x0019, IntPtr.Zero, 0, sbPath);

                string allusersPath = sbPath.ToString();

                if (System.IO.File.Exists(allusersPath + "\\检验条码打印.lnk"))
                {
                    //生成打印条码桌面图标
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

                    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(allusersPath + "\\检验条码打印.lnk");

                    //shortcut.TargetPath = @"C:\Program Files\hope\his\检验条码打印.bat";
                    //shortcut.WorkingDirectory = @"C:\Program Files\hope\his\";
                    shortcut.TargetPath = PathManager.SettingPath + @"检验条码打印.bat";
                    shortcut.WorkingDirectory = PathManager.SettingPath;


                    shortcut.WindowStyle = 1;
                    //shortcut.Description = this.ServerConfigXml.GetNodeValue("AutoUpdater/Updater/Description");
                    //shortcut.IconLocation = this.InstallPath + "\\" + this.MainAppFile + ",0";// System.Environment.SystemDirectory + "\\" + "shell32.dll, 171";
                    shortcut.IconLocation = System.Environment.SystemDirectory + "\\" + "shell32.dll, 16";// +i.ToString();
                    shortcut.Save();
                }
            }
            catch (Exception)
            {
                //throw;
            }

        }
        #endregion

        #region Main入口


        internal void Run(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
             
                bool enableIisSwtich = ConfigurationManager.AppSettings["EnableMutliIISSwtich"] == "1";
                if (enableIisSwtich)
                {
                    new IISSwtichController().SwtichAddr(ClientType.BarCodeClient);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            Lib.LogManager.Logger.LogInfo("1:" + sw.ElapsedMilliseconds.ToString());
            Thread t = new Thread(ModifyBarcodePrintDesktopIcon);
            t.Start();

            try
            {
                if (args.Length > 0)
                {
                    bool allMultiBarCodeClient = ConfigHelper.GetSysConfigValueWithoutLogin("Allow_Multi_BarCodeClient") == "是";

                    Lib.LogManager.Logger.LogInfo("2:" + sw.ElapsedMilliseconds.ToString());
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (string.IsNullOrEmpty(args[0]))//第一个参数如果为空
                        return;

                    string[] strParameters = args[0].Split(';');
                    string type = strParameters[0].Trim().ToLower();
                    if (type == "report")
                    {
                        Lib.LogManager.Logger.LogInfo("3:" + sw.ElapsedMilliseconds.ToString());
                        KillProcess("cmd");
                        RunReportSearch(strParameters, allMultiBarCodeClient);
                        Lib.LogManager.Logger.LogInfo("4:" + sw.ElapsedMilliseconds.ToString());
                    }
                    else if (type == "barcode")
                    {
                        LocalSetting.Current.Setting.DeptID = strParameters[1];
                        string localPath = PathManager.SettingLisPath + @"\printXml";

                        if (!Directory.Exists(localPath))
                            Directory.CreateDirectory(localPath);

                        KillProcess("cmd");
                        RunBarcode(strParameters, allMultiBarCodeClient);
                    }
                    else if(type == "zy")
                    {
                        if(strParameters.Length > 5)
                        {
                            strParameters[0] = "barcode";
                            var patInNo = strParameters[1];
                            var Operator = strParameters[2];
                            var deptId = strParameters[4];
                            strParameters[1] = deptId;
                            strParameters[2] = patInNo;
                            strParameters[3] = Operator;
                            strParameters[4] = "1";
                            LocalSetting.Current.Setting.DeptID = strParameters[1];
                            string localPath = PathManager.SettingLisPath + @"\printXml";

                            if (!Directory.Exists(localPath))
                                Directory.CreateDirectory(localPath);
                            KillProcess("cmd");
                            RunBarcode(strParameters, allMultiBarCodeClient);
                        }
                    }
                    else if (type == "tjbarcode")
                    {
                        //FrmBCPrint TJprint = new FrmBCPrint(strParameters);
                        //Application.Run(TJprint);
                        KillProcess("cmd");
                        RunTJBarcode(strParameters);
                    }
                    else if (type == "mzbarcode")
                    {
                        //FrmBCPrint TJprint = new FrmBCPrint(strParameters);
                        //Application.Run(TJprint);
                        KillProcess("cmd");
                        RunMZBarcode(strParameters);
                    }
                    else
                    {
                        FrmBCPrint TJprint = new FrmBCPrint(true, "体检", args[0]);
                        Application.Run(TJprint);
                    }
                }
                else
                {
                    CreateDecktopIcon();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("ZY_Barcode", ex);
            }
            sw.Stop();
            Lib.LogManager.Logger.LogInfo("all:" + sw.ElapsedMilliseconds.ToString());
        }

        private static void KillProcess(string proName)
        {
            Process[] procs = Process.GetProcessesByName(proName);
            if (procs != null && procs.Length > 0)
                procs[0].Kill();

        }

        static FrmCombineModeSel fcm = null;

        /// <summary>
        /// 调用查询
        /// </summary>
        /// <param name="strParameters"></param>
        /// <param name="allMultiBarCodeClient">允许打开多个条码客户端</param>
        private static void RunReportSearch(string[] strParameters, bool allMultiBarCodeClient)
        {
            ReportDownloader downloader = new ReportDownloader(new string[] { });
            Thread report = new Thread(downloader.getReportsFile);//new ThreadStart()
            report.Start();

            if (strParameters == null || strParameters.Length == 0)
                return;
            string str = "";
            foreach (string item in strParameters)
            {
                str += item + " ";
            }
            str = str.Trim();
            System.IO.File.WriteAllText(path + "reportparam.ini", str);

            string strLoginID = "";
            //住院报告查询,是否需要登录
            string IsReportLogin = ConfigurationManager.AppSettings["IsReportLogin"];
            if (!string.IsNullOrEmpty(IsReportLogin) && (IsReportLogin.ToUpper() == "Y"))
            {
                lis.client.control.FrmCheckPassword frmpw = new lis.client.control.FrmCheckPassword();
                if (frmpw.ShowDialog() == DialogResult.OK)
                {
                    strLoginID = frmpw.OperatorID;
                }
                else
                {
                    return;
                }
            }

            Process ins = GetPreIns();

            if (ins == null || allMultiBarCodeClient)
            {
                string dep_incode = strParameters[1];
                string pat_no_id = strParameters[2];

                fcm = new FrmCombineModeSel(pat_no_id, dep_incode);

                if (!string.IsNullOrEmpty(strLoginID))
                {
                    fcm.setLoginID = strLoginID;
                }
                fcm.FormClosed += new FormClosedEventHandler(fcm_FormClosed);

                Application.Run(fcm);
            }
            else
            {
                RestorePreIns(ins);
            }
        }


        static void fcm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //关闭查询系统，同时关闭危急值程序
                string isAutoCloseUrgentClient = System.Configuration.ConfigurationManager.AppSettings["isAutoCloseUrgentClient"];
                //如果为null添加默认值NO
                if (string.IsNullOrEmpty(isAutoCloseUrgentClient))
                    isAutoCloseUrgentClient = "N";

                if (isAutoCloseUrgentClient.ToUpperInvariant() == "Y" || isAutoCloseUrgentClient.ToUpperInvariant() == "YES")
                {
                    KillProcess("dcl.client.msgclient");
                }
            }
            catch (Exception)
            {
            }

            //强制关闭查询系统
            KillProcess("dcl.client.sampleclient");
        }

        private void RunBarcode(string[] strParameters, bool allMultiBarCodeClient)
        {
            ReportDownloader downloader = new ReportDownloader(new string[] { "ZYBarcodeReport", "printBarCodeQD" });
            Thread report = new Thread(downloader.getReportsFile);//new ThreadStart()
            report.Start();
            //string deptID = "";
            //string patID = "";
            //string doctorName = "";

            /////////////////////////////////////
            if (strParameters == null || strParameters.Length == 0)
                return;
            string str = "";
            foreach (var item in strParameters)
            {
               str += item + " ";   
            }
            str = str.Trim();
            System.IO.File.WriteAllText(path + "barcodeparam.ini", str);

            Process ins = GetPreIns();
            if (ins == null || allMultiBarCodeClient) //
            {
                FrmBCPrint print = new FrmBCPrint(true, "住院", "");
                //MessageHelper helper = new MessageHelper(print.Handle);
                bool MinWinDownloadBar = ConfigurationManager.AppSettings["MinWinDownloadBar"] == "1";
                print.DownloadBarWinStatues(MinWinDownloadBar);
                //helper.SendMsg(MessageHelper.WM_START, 0, "");
                Application.Run(print);
            }
            else
            {
                RestorePreIns(ins);//激活前一实例   

                //MessageHelper helper = new MessageHelper(ins.MainWindowHandle);
                //helper.SendMsg(MessageHelper.WM_START, 0, null);
            }
            /////////////////////////////////////

            //if (strParameters.Length >= 3)
            //{
            //    deptID = strParameters[1];
            //    patID = strParameters[2];
            //    doctorName = strParameters[3];
            //    //住院条码
            // GetBCPrintInstance();

            //print.DeptCode = deptID;
            //print.PatID = patID;
            //print.DoctorName = doctorName;
            //Application.Run(print);
            //}
        }

        /// <summary>
        /// 体检条码传参调用
        /// </summary>
        /// <param name="strParameters">体检系统传进来的参数</param>
        private void RunTJBarcode(string[] strParameters)
        {
            /////////////////////////////////////
            if (strParameters == null || strParameters.Length == 0)
                return;

            ReportDownloader downloader = new ReportDownloader(new string[] { });
            Thread report = new Thread(downloader.getReportsFile);//new ThreadStart()
            report.Start();

            string str = "";
            foreach (string item in strParameters)
            {
                str += item + " ";
            }
            str = str.Trim();
            //将参数写入INI 记录,在界面调用时用这个查询
            System.IO.File.WriteAllText(path + "TJbarcodeparam.ini", str);


            Process ins = GetPreIns();
            if (ins == null)
            {

                FrmBCPrint print = new FrmBCPrint(strParameters, "体检");
                bool MinWinDownloadBar = ConfigurationManager.AppSettings["MinWinDownloadBar"] == "1";
                print.DownloadBarWinStatues(MinWinDownloadBar);
                Application.Run(print);
            }
            else
            {
                RestorePreIns(ins);//激活前一实例   


            }

        }


        /// <summary>
        /// 门诊条码传参调用
        /// </summary>
        /// <param name="strParameters">体检系统传进来的参数</param>
        private void RunMZBarcode(string[] strParameters)
        {
            
            if (strParameters == null || strParameters.Length == 0)
                return;

            ReportDownloader downloader = new ReportDownloader(new string[] { });
            Thread report = new Thread(downloader.getReportsFile);//new ThreadStart()
            report.Start();
            string str = "";
            foreach (string item in strParameters)
            {
                str += item + " ";
            }
            str = str.Trim();
            //将参数写入INI 记录,在界面调用时用这个查询
            System.IO.File.WriteAllText(path + "MZbarcodeparam.ini", str);


            Process ins = GetPreIns();
            if (ins == null)
            {

                //FrmBCPrint print = new FrmBCPrint(true, "门诊", "");//(strParameters, "门诊");
                FrmBCPrint print = new FrmBCPrint(strParameters, "门诊");
                bool MinWinDownloadBar = ConfigurationManager.AppSettings["MinWinDownloadBar"] == "1";
                print.DownloadBarWinStatues(MinWinDownloadBar);
                Application.Run(print);
            }
            else
            {
                RestorePreIns(ins);//激活前一实例   


            }

        }

        #endregion

        #region 单进程
        public static Process GetPreIns()
        {//获取当前进程句柄
            Process cur = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(cur.ProcessName);
            foreach (Process proc in procs)
            { //遍历，以获取前一实例的句柄                     
                if (proc.Id != cur.Id) //忽略现有的例程
                {
                    //确保例程从EXE文件运行        
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == cur.MainModule.FileName)
                    {
                        //返回另一个例程实例          
                        return proc;
                    }
                }
            }
            //没有其它的例程，返回Null    
            return null;
        }

        public static void RestorePreIns(Process ins)
        {
            //激活前一实例  
            bool MinWinDownloadBar = ConfigurationManager.AppSettings["MinWinDownloadBar"] == "1";
            if (MinWinDownloadBar)
            {

                ShowWindowAsync(ins.MainWindowHandle, 8);
            }
            else
            {
                ShowWindowAsync(ins.MainWindowHandle, WS_SHOWNORMAL); //确保窗口没有被最小化或最大化     

            }


            SetForegroundWindow(ins.MainWindowHandle); //设置真实例程为foreground window    
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 3;
        #endregion


        private static void CreateDecktopIcon()
        {
            string strPath = PathManager.SettingPath;
            try
            {
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string executPath = string.Empty;
                executPath = Application.ExecutablePath;
                if (!System.IO.File.Exists(strPath + "\\run.bat")) //run.bat
                {
                    CreateFilte(strPath + "\\run.bat", "@echo off \r\n lis.bat \"report;805;-1;");
                }
                if (!System.IO.File.Exists(strPath + "\\signin.bat")) //signin.bat
                {
                    CreateFilte(strPath + "\\signin.bat", "@echo off \r\n lis.bat \"bcsignin;");
                }

                //是否生成新检验报告查询快捷方式在桌面
                string IsCreateDeckIconReport = ConfigurationManager.AppSettings["CreateDeckIconReport"];
                if (!string.IsNullOrEmpty(IsCreateDeckIconReport) && (IsCreateDeckIconReport.ToLower()=="n"
                    || IsCreateDeckIconReport.ToLower() == "no"))
                {
                    //不生成快捷方式在桌面
                    try
                    {
                        string pathlink_p = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\报告查询.lnk";
                        if (System.IO.File.Exists(pathlink_p))
                        {
                            //如果图标存在,则删除
                            System.IO.File.Delete(pathlink_p);
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    CreateDecktopLink(strPath + "\\", "run.bat", "报告查询");
                }

                CreateFilte(strPath + "\\lis.ini", executPath);
                CreateFilte(strPath + "\\lis2.ini", Application.StartupPath + "\\AutoUpdate.exe");
                CreateFilte(strPath + "\\肿瘤医院请双击.bat", string.Format("copy /y \"{0}\\Outlink2.ini\"  \"{0}\\Outlink.ini\"", Application.StartupPath));
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\慧扬条码检验查询更新.appref-ms";
                string bacType = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\标本采集.lnk";

                if (System.IO.File.Exists(bacType))
                {
                    System.IO.File.Delete(bacType);
                }

                if (System.IO.File.Exists(desktop))
                {
                    System.IO.File.Delete(desktop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

        }

        private static void CreateDecktopLink(string path, string fileName, string description)
        {
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + description + ".lnk");
                shortcut.TargetPath = path + fileName;
                shortcut.WorkingDirectory = path;
                shortcut.WindowStyle = 1;
                shortcut.Description = description;
                shortcut.IconLocation = System.Environment.SystemDirectory + "\\" + "shell32.dll, 171";
                shortcut.Save();
            }
            catch (Exception ex)
            {
                //throw;
                //MessageBox.Show("创建桌面图标失败" + ex.ToString());
            }
        }

        private static void CreateFilte(string creaPath, string content)
        {
            FileInfo fi = new FileInfo(creaPath);
            if (fi.Exists && fi.IsReadOnly)
                fi.IsReadOnly = false;

            TextWriter writer = System.IO.File.CreateText(creaPath);
            writer.WriteLine(content);
            writer.Close();
        }


        internal void CheckUpdate()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                bool hasNewVesion = ApplicationDeployment.CurrentDeployment.CheckForUpdate();
                if (hasNewVesion)
                {
                    NotifyIcon notifyIcon = new NotifyIcon();
                    notifyIcon.ShowBalloonTip(10000, "更新新检验条码", "正在更新新检验条码。。。。需要几分钟，请稍候。", ToolTipIcon.Info);

                    if (ApplicationDeployment.CurrentDeployment.Update())
                        notifyIcon.ShowBalloonTip(2000, "更新成功", "更新新检验条码成功。", ToolTipIcon.Info);
                }
            }


        }
    }

    static class Program
    {
        /// <summary>
        /// 运行消息客户端
        /// </summary>
        public static void RunMessageClient()
        {
            string msgclient_name = "dcl.client.msgclient.exe";

            string path = Application.StartupPath + "\\" + msgclient_name;
            if (System.IO.File.Exists(path))
            {
                bool isExistence = false;
                Process[] allProcess = Process.GetProcesses();

                List<Process> processRunning = new List<Process>();

                //查找进程中是否已经运行了危急值客户端
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
                    string lis_message_name = PathManager.MessageClientPath + msgclient_name;

                    if (System.IO.File.Exists(lis_message_name)
                        && System.IO.File.Exists(lis_message_name + ".config"))
                    {
                        //只调用相同路径的危急值客户端
                        string UseUrgentClientForSamePath = ConfigurationManager.AppSettings["UseUrgentClientForSamePath"];

                        if (!string.IsNullOrEmpty(UseUrgentClientForSamePath)
                            && UseUrgentClientForSamePath.ToLower() == "y")
                        {
                            Process.Start(path);
                        }
                        else
                        {
                            Process.Start(lis_message_name);
                        }
                    }
                    else
                    {
                        Process.Start(path);
                    }
                }
            }
        }

        /// <summary>
        /// 运行消息客户端(传科室代码)
        /// </summary>
        /// <param name="DepCode"></param>
        public static void RunMessageClient(string DepCode)
        {
            string msgclient_name = "dcl.client.msgclient.exe";

            string path = Application.StartupPath + "\\" + msgclient_name;
            if (System.IO.File.Exists(path))
            {
                bool isExistence = false;
                Process[] allProcess = Process.GetProcesses();

                List<Process> processRunning = new List<Process>();

                //查找进程中是否已经运行了危急值客户端
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
                    string lis_message_name = PathManager.MessageClientPath + msgclient_name;

                    if (System.IO.File.Exists(lis_message_name)
                        && System.IO.File.Exists(lis_message_name + ".config")
                        )
                    {
                        //只调用相同路径的危急值客户端
                        string UseUrgentClientForSamePath = ConfigurationManager.AppSettings["UseUrgentClientForSamePath"];

                        if (!string.IsNullOrEmpty(UseUrgentClientForSamePath)
                            && UseUrgentClientForSamePath.ToLower() == "y")
                        {
                            Process.Start(path, DepCode);
                        }
                        else
                        {
                            Process.Start(lis_message_name, DepCode);
                        }
                    }
                    else
                    {
                        Process.Start(path, DepCode);
                    }
                }
            }
        }

        public static void RunConfig(bool isNotOpen)
        {
            if (!isNotOpen&&System.IO.File.Exists(Application.StartupPath + "\\BarcodeClientConfig.exe")
                && !System.IO.File.Exists(PathManager.SettingPath + @"检验条码打印.bat"))
            {
                Process.Start(Application.StartupPath + "\\BarcodeClientConfig.exe");
            }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.SetCompatibleTextRenderingDefault(false);
            UserLookAndFeel.Default.UseDefaultLookAndFeel = true;
            UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");


            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-hans");
            //args = new string[] { "barcode;3005&9998;-1;-1" };
            //args = new string[] { "report;3005&9998;-1;" };
            //args = new string[] { "report;0240;222;" };
            //args = new string[] { "barcode;0145;1234567;卢昌凤;40342;" };
            //args = new string[] { "mzbarcode;40342;卢昌凤;1234567;1;" };

            //条码系统打开危急值程序时是否带科室代码参数
            string IsOpenUrgentClientWithDept = ConfigurationManager.AppSettings["IsOpenUrgentClientWithDept"];

            //打开条码系统时是否打开危急值程序
            string IsOpenUrgentClient = ConfigurationManager.AppSettings["isOpenUrgentClient"];
            //运行危急值消息客户端
            if ((!string.IsNullOrEmpty(IsOpenUrgentClient)) && (IsOpenUrgentClient.ToUpperInvariant() == "NO"
                || IsOpenUrgentClient.ToUpperInvariant() == "N"))
            {
                //如果为NO或N,则打开条码系统时不打开危急值程序
            }
            else
            {
                if ((!string.IsNullOrEmpty(IsOpenUrgentClientWithDept)) && (IsOpenUrgentClientWithDept.ToUpperInvariant() == "YES"
                || IsOpenUrgentClientWithDept.ToUpperInvariant() == "Y"))
                {
                    bool okOpenWithValue = false;//传参打开成功
                    if (args.Length > 0)
                    {
                        //只有独立一个科室代码参数时才采用传参调用危急值程序
                        if (!args[0].ToString().Contains("report") 
                            && !args[0].ToString().Contains("tjbarcode")
                            && !args[0].ToString().Contains("barcode")
                            && !args[0].ToString().Contains("mzbarcode")
                            && !args[0].ToString().Contains(";"))
                        {
                            string dep_incode = args[0];
                            if (!string.IsNullOrEmpty(dep_incode))
                            {
                                RunMessageClient(dep_incode);
                                okOpenWithValue = true;
                            }
                        }
                    }

                    if (!okOpenWithValue)//传参打开失败时,则采取默认打开方式
                    {
                        RunMessageClient();
                    }
                }
                else
                {
                    RunMessageClient();
                }
            }

            bool isgyey = ConfigHelper.GetSysConfigValueWithoutLogin("BC_ShowPrintSettingBtn") == "是";
            bool isNotOpen = isgyey && (args[0].ToString().Contains("barcode") || args[0].ToString().Contains("tjbarcode") );
            RunConfig(isNotOpen);

            Login login = new Login();
            login.Run(args);
            login.CheckUpdate();
            //else
            //{
            //    FrmLogin login = new FrmLogin();
            //    Application.Run(login);
            //}
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logger.LogException("barcodeclient", e.Exception);
        }
    }

    public class ReportDownloader
    {
        public ReportDownloader(string[] repCodes)
        {
            ReportCodes = repCodes;
        }

        private string[] ReportCodes { get; set; }

        public void getReportsFile()
        {
            List<string> listCode = new List<string>();
            if (ReportCodes.Length > 0)
            {

                for (int i = 0; i < ReportCodes.Length; i++)
                {
                    listCode.Add(ReportCodes[i]);
                }
            }

            // string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport";
            string localPath = PathManager.ReportPath;

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            string user = UserInfo.GetSysConfigValue("1");//得到系统配置


            ProxyReportMain proxyRepMain = new ProxyReportMain();
            List<EntitySysReport> listSysReport = proxyRepMain.Service.GetRepLocationByListCode(listCode); //获取报表数据
            EntitySysReport eySysReport = new EntitySysReport();
            eySysReport.RepLocation = "报表样本.repx";
            listSysReport.Add(eySysReport);
            string serverPath = ConfigurationManager.AppSettings["wcfAddr"].ToString() + @"xtraReport/";//服务器目录

            localPath += @"\";//本地目录

            string ex = "";

            System.Net.WebClient client = new System.Net.WebClient();
            client.Proxy = null;
            foreach (EntitySysReport drRep in listSysReport)
            {
                try
                {
                    string strRepPath = drRep.RepLocation;
                    string writePath = localPath + strRepPath;
                    string readPath = serverPath + strRepPath;
                    if (user == "使用本地模版")
                    {
                        if (!System.IO.File.Exists(writePath))
                            client.DownloadFile(readPath, writePath);
                    }
                    else
                        client.DownloadFile(readPath, writePath);

                }
                catch (Exception)
                {
                    ex += "服务器无:" + drRep.RepLocation + "报表！\r\n";
                }
            }

            if (ex != "")
            {
                Logger.LogInfo("getServerReportsFile", ex);
            }
            try
            {
                XtraReport xtrTest = new XtraReport();
                xtrTest.LoadLayout(localPath + "报表样本.repx");
                xtrTest.Dispose();
            }
            catch (Exception ex1)
            {
                Logger.LogInfo("XtraReport.LoadLayout(localPath+报表样本.repx)", ex1.Message);
            }
        }
    }

}
