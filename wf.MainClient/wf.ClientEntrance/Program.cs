using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using dcl.client.resultquery;
using System.Diagnostics;
using System.Data;
using System.IO;
using dcl.client.frame;
using dcl.client.common;
using System.Configuration;
using System.Threading;
using DevExpress.XtraReports.UI;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using System.Text;

using Lib.LogManager;
using System.ComponentModel.Composition.Hosting;
using DevExpress.LookAndFeel;
using dcl.entity;
using dcl.client.wcf;

namespace wf.ClientEntrance
{
    static class Program
    {
        public static CompositionContainer Container = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            try
            {
                AggregateCatalog catalog = new AggregateCatalog(
                               new DirectoryCatalog(@".\", string.Format("mod_{0}.dll",
                               ConfigurationManager.AppSettings["MicrobeSystem.ExtDataInterface"]??"*")),
                               new DirectoryCatalog(@".\", "dcl.client.micManagement.dll")
                               //new DirectoryCatalog(@".\", "*.dll")
                               );
                Container = new CompositionContainer(catalog);
                UserInfo.Container = Container;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.SetCompatibleTextRenderingDefault(false);
            UserLookAndFeel.Default.UseDefaultLookAndFeel = true;

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-hans");
            try
            {
                bool enableIisSwtich = ConfigurationManager.AppSettings["EnableMutliIISSwtich"] == "1";
                if (enableIisSwtich)
                {
                    new IISSwtichController().SwtichAddr(ClientType.LisClient);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            try
            {
                Lib.DataInterface.Implement.DIEnviorment.RegistDACProxy(new dcl.client.wcf.ProxyDataInterface());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                //MessageBox.Show("注册数据接口文件失败!");
                return;
                //throw;
            }

            try
            {
                string SSOINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SSOCONTENT.INI";
                if (System.IO.File.Exists(SSOINIPath)) System.IO.File.Delete(SSOINIPath);
            }
            catch (Exception ex1)
            {
            }

            //MessageBox.Show("3");
            if (args.Length > 0)
            {
                string loginUseJhSessionID = System.Configuration.ConfigurationManager.AppSettings["loginUseJhSessionID"];
                string outkeyStr = string.Empty;
                if (string.IsNullOrEmpty(args[0]))//第一个参数如果为空
                    return;
                string[] strParameters = args[0].Split(';');
                string type = strParameters[0].Trim().ToLower();
                string typeOld = strParameters[0].Trim();
                if (type == "lis")
                {
                    Login();
                }
                else if (type == "zy")
                {
                    CreateDecktopIcon();
                }

                else if (type == "report")
                {
                    RunConfig();
                    KillProcess("cmd");
                    if (System.IO.File.Exists(Application.StartupPath + @"/Lis.ReportQuery.exe"))
                    {
                        Process.Start(Application.StartupPath + @"/Lis.ReportQuery.exe", args[0]);
                    }
                    else
                    {
                        RunReportSearch(strParameters);
                    }

                }
                else if (type == "barcode")
                {
                    CreateDecktopIcon();
                    RunMessageClient();
                    CreateMessageStartup();
                    RunConfig();

                    if (strParameters != null && strParameters.Length > 0)
                    {
                        string str = "";
                        foreach (string item in strParameters)
                        {
                            str += item + " ";
                        }
                        str = str.Trim();
                        System.IO.File.WriteAllText(path + "barcodeparam.ini", str);
                    }
                    Login();
                }
                else if (type != null && type.ToUpper().StartsWith("SSO"))
                {
                    try
                    {
                        string SSOINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SSOCONTENT.INI";
                        System.IO.File.WriteAllText(SSOINIPath, type);
                    }
                    catch(Exception ex1)
                    {
                    }

                    Login();
                }
                else if (typeOld != null && (typeOld.StartsWith("sessionID") || typeOld.StartsWith("jhsessionid") || loginUseJhSessionID != null && loginUseJhSessionID.ToUpper() == "Y"))
                {
                    if (getUserCodeInfoByJH(typeOld, out outkeyStr))
                    {
                        try
                        {
                            //传入参数格式为：sessionID=B078E5BA-F71D-4D09-9B8D-6216092330AA
                            string SSOEMUINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SSOEMUCONTENT.INI";
                            System.IO.File.WriteAllText(SSOEMUINIPath, outkeyStr);
                        }
                        catch (Exception ex1)
                        {
                        }

                        Login();
                    }
                }
            }
            else
            {
                Login();
            }

            UpdateIcon();
        }


        public static void Login()
        {
            FrmLogin login = new FrmLogin();
            login.timer.Interval = 60000;
            login.timer.Start();
            try
            {
                Application.Run(login);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
#if DEBUG
                lis.client.control.MessageDialog.Show("当前操作失败，请重试或与管理人员联系！" + ex.ToString());
#else
                lis.client.control.MessageDialog.Show("当前操作失败，请重试或与管理人员联系！" + ex.Message);
#endif
            }
        }

        private static void KillProcess(string proName)
        {
            Process[] procs = Process.GetProcessesByName(proName);
            if (procs != null && procs.Length > 0)
                procs[0].Kill();

        }


        static FrmCombineModeSel fcm = null;
        static string path = PathManager.SettingPath;
        static string pathForOutlink = PathManager.OutLinkPath;
        /// <summary>
        /// 调用查询
        /// </summary>
        /// <param name="strParameters"></param>
        private static void RunReportSearch(string[] strParameters)
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

            Process ins = GetPreIns();

            if (ins == null)
            {
                string dep_incode = strParameters[1];
                string pat_no_id = strParameters[2];

                fcm = new FrmCombineModeSel(pat_no_id, dep_incode);

                fcm.FormClosed += new FormClosedEventHandler(fcm_FormClosed);

                Application.Run(fcm);
            }
            else
            {
                RestorePreIns(ins);
            }
        }


        private static void UpdateIcon()
        {
            string bacType = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\标本采集.lnk";
            if (System.IO.File.Exists(bacType))
            {
                System.IO.File.Delete(bacType);
            }
        }

        static void fcm_FormClosed(object sender, FormClosedEventArgs e)
        {
            KillProcess("dcl.client.sampleclient");
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
        {//激活前一实例              
            ShowWindowAsync(ins.MainWindowHandle, WS_SHOWNORMAL); //确保窗口没有被最小化或最大化            
            SetForegroundWindow(ins.MainWindowHandle); //设置真实例程为foreground window    
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 3;


        private static void CreateDecktopIcon()
        {
            RunMessageClient();
            RunConfig();
            string strPath = PathManager.SettingPath;
            try
            {

                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string executPath = string.Empty;

                executPath = Application.StartupPath + @"\AutoUpdate.exe";

                CreateFilte(strPath + "\\lis.bat", string.Format("\"{0}\" %1", executPath));

                if (!System.IO.File.Exists(strPath + "\\run.bat")) //run.bat
                {
                    CreateFilte(strPath + "\\run.bat", "@echo off \r\n lis.bat \"report;805;-1;");
                }

                if (!System.IO.File.Exists(strPath + "\\signin.bat")) //signin.bat
                {
                    CreateFilte(strPath + "\\signin.bat", "@echo off \r\n lis.bat \"bcsignin;");
                }

                CreateDesktopLink(strPath + "\\", "run.bat", "新检验报告查询");

                CreateFilte(strPath + "\\lis.ini", executPath);

                CreateFilte(strPath + "\\lis2.ini", Application.StartupPath + "\\AutoUpdate.exe");

                CreateFilte(strPath + "\\肿瘤医院请双击.bat", string.Format("copy /y \"{0}\\Outlink2.ini\"  \"{0}\\Outlink.ini\"", Application.StartupPath));

                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\检验系统集成平台.appref-ms";
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
                foreach (Process p in allProcess)
                {
                    if (p.ProcessName.ToLower() + ".exe" == msgclient_name.ToLower())
                    {
                        isExistence = true;
                        break;
                    }
                }

                if (!isExistence)
                    Process.Start(path);

            }
        }

        private static void CreateMessageStartup()
        {
            string msgclient_name = "dcl.client.msgclient.exe";
            string path = Application.StartupPath + "\\" + msgclient_name;
            if (System.IO.File.Exists(path))
            {
                //生成到开机启动
                StringBuilder sbPath = new StringBuilder(260);
                SHGetFolderPath(IntPtr.Zero, 0x0018, IntPtr.Zero, 0, sbPath);

                WshShell shell = new WshShell();

                string desktopname = "检验消息程序";

                string allusersPath = sbPath.ToString();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(allusersPath + "\\" + desktopname + ".lnk");

                shortcut.TargetPath = Application.StartupPath + "\\" + msgclient_name;
                shortcut.WorkingDirectory = Application.StartupPath;
                shortcut.WindowStyle = 1;
                shortcut.Description = "检验消息程序";
                shortcut.IconLocation = Application.StartupPath + "\\" + msgclient_name + ",0";
                shortcut.Save();
            }
        }

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        public static void RunConfig()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\BarcodeClientConfig.exe")
                  && !System.IO.File.Exists(PathManager.SettingPath + @"检验条码打印.bat")
                )
            {
                bool isExistence = false;
                Process[] allProcess = Process.GetProcesses();

                List<Process> processRunning = new List<Process>();
                foreach (Process p in allProcess)
                {
                    if (p.ProcessName.ToLower() + ".exe" == "BarcodeClientConfig.exe".ToLower())
                    {
                        isExistence = true;
                        break;
                    }
                }

                if (!isExistence)
                    Process.Start(Application.StartupPath + "\\BarcodeClientConfig.exe");
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

        private static void CreateDesktopLink(string path, string fileName, string description)
        {
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
        "\\" + description + ".lnk");
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
        /// <summary>
        /// 嘉和单点登录验证
        /// </summary>
        /// <param name="keyStr"></param>
        /// <param name="outkeyStr"></param>
        /// <returns></returns>
        private static bool getUserCodeInfoByJH(string strJHSESSIONID, out string outkeyStr)
        {
            outkeyStr = string.Empty;
            try
            {
                string[] strsp = strJHSESSIONID.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (strsp.Length >= 2)
                {
                    Lis.SendDataByHl7v3.DataSendHelper lsh = new Lis.SendDataByHl7v3.DataSendHelper();
                    outkeyStr = lsh.getUserDetailInfo(strsp[1]);
                    if (string.IsNullOrEmpty(outkeyStr))
                    {
                        throw new Exception("解密失败");
                    }
                    return true;
                }
                else if (strJHSESSIONID != null && !strJHSESSIONID.Contains("="))
                {
                    Lis.SendDataByHl7v3.DataSendHelper lsh = new Lis.SendDataByHl7v3.DataSendHelper();
                    outkeyStr = lsh.getUserDetailInfo(strJHSESSIONID);
                    if (string.IsNullOrEmpty(outkeyStr))
                    {
                        throw new Exception("解密失败");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("嘉和单点登录验证", ex);
            }
            return false;
        }
    }
}
