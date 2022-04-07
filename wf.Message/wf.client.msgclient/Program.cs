using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Data;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.LookAndFeel;

namespace dcl.client.msgclient
{
    static class Program
    {
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
            UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Blue");

            Addbinding(null);

            #region 添加Endpoint节点

            AddEndpoint("svc.basic", "basicHttpBinding", "bigdata", "svc.basic", "");
            AddEndpoint("runtimesetting", "basicHttpBinding", "bigdata", "IRunTimeSettingService", "");
            #endregion

            #region 更新config内容

            try
            {
                //更新配置文件内容的文件路径Update_messageclientConfig.txt
                string updateConfigPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Update_messageclientConfig.txt";
                if (System.IO.File.Exists(updateConfigPath))
                {
                    string strUpConfig = System.IO.File.ReadAllText(updateConfigPath, System.Text.Encoding.Default);
                    if (!string.IsNullOrEmpty(strUpConfig))
                    {
                        //内容格式：key名1=key值1;key名2=key值2;key名3=key值3;
                        string[] strComSplit = strUpConfig.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strCom in strComSplit)
                        {
                            string[] strItmSplit = strCom.Split(new char[] { '=' }, StringSplitOptions.None);
                            if (strItmSplit.Length >= 2)
                            {
                                if (!string.IsNullOrEmpty(strItmSplit[0]))
                                {
                                    UpdateAppSettings(strItmSplit[0], string.Format("{0}", strItmSplit[1]));
                                }
                            }
                        }
                    }
                    System.IO.File.WriteAllText(updateConfigPath, "");//清空内容
                }
            }
            catch
            {

            }

            #endregion

            //启动设置
            if (args.Length > 0 && args[0] == "setting")
            {
                FrmSetting frmsetting = new FrmSetting();
                frmsetting.ShowDialog();
                return;
            }

            int pCount = 0;
            bool bExistProcess = false;
            bool IsShowfrmMsgGather = false;//是否显示frmMsgGather窗口

            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.ToLower() + ".exe" == "dcl.client.msgclient.exe")
                {
                    pCount++;

                    if (pCount >= 2)
                    {
                        bExistProcess = true;
                        break;
                    }
                }
            }

            try
            {
                //用于临时存储医生代码信息
                string MSGTEMPDOCCODEINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGTEMPDOCCODE.INI";
                if (System.IO.File.Exists(MSGTEMPDOCCODEINIPath))
                {
                    System.IO.File.Delete(MSGTEMPDOCCODEINIPath);
                }
            }
            catch
            {

            }

            #region 传参处理

            //如果有参数,则更新科室(病区)信息
            if (args.Length > 0)
            {
                if (!string.IsNullOrEmpty(args[0]))//第一个参数如果为空
                {
                    if (args[0].Contains(";"))//如果有两个参数（病区号;医生代码）
                    {
                        string[] argssplit = args[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        if (argssplit.Length >= 1 && !string.IsNullOrEmpty(argssplit[0]))
                        {
                            string args_dep_code = argssplit[0];
                            UpdateAppSettings("dep_code", args_dep_code);
                        }
                        if (argssplit.Length >= 2 && !string.IsNullOrEmpty(argssplit[1]))
                        {
                            string args_doc_code = argssplit[1];//医生代码
                            try
                            {
                                //用于临时存储医生代码信息
                                string MSGTEMPDOCCODEINIPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGTEMPDOCCODE.INI";
                                System.IO.File.WriteAllText(MSGTEMPDOCCODEINIPath, args_doc_code);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        string args_dep_code = args[0];
                        UpdateAppSettings("dep_code", args_dep_code);
                    }
                }
            }
            #endregion

            //是否与报告查询采用相同科室
            string IsReportSameDep = ConfigurationManager.AppSettings["IsReportSameDep"];

            //nw危急值标题
            string NwAppTile = ConfigurationManager.AppSettings["NwAppTile"];

            //细菌组-物理组ID
            string xijun_typeids = ConfigurationManager.AppSettings["xijun_typeids"];
            //病理组与血库组-物理组ID
            string bingli_typeids = ConfigurationManager.AppSettings["bingli_typeids"];


            //fsaudit 佛山市一验证
            string strAuditType2 = ConfigurationManager.AppSettings["UserAuthType"];
            //如果为佛山市一,读取ini
            if (File.Exists(@"C:\Program Files\medchange\his\reportparam.ini") && (!(!string.IsNullOrEmpty(strAuditType2) && strAuditType2 == "fsaudit")))
            {
                if (!string.IsNullOrEmpty(IsReportSameDep) && IsReportSameDep.ToUpper() == "Y")
                {
                    string result = File.ReadAllText(@"C:\Program Files\medchange\his\reportparam.ini");
                    string[] parm = result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parm.Length > 2)
                    {
                        if (ConfigurationManager.AppSettings["dep_code"].ToString() != parm[1])
                            UpdateAppSettings("dep_code", parm[1]);
                    }
                }
            }


            if (ConfigurationManager.AppSettings["dep_code"] == string.Empty)
            {
                string path = Application.StartupPath + "\\BarcodeClientConfig.exe";
                if (File.Exists(path))
                {
                    Process.Start(path);
                }
                else
                {
                    FrmSetting frmsetting = new FrmSetting();
                    if (frmsetting.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            #region 添加某些配置--清远人医需求

            //检查-是否没有配置-是否用户验证
            string IsUserValidate = ConfigurationManager.AppSettings["IsUserValidate"];
            if (string.IsNullOrEmpty(IsUserValidate))//如果为null添加默认值Y
            {
                UpdateAppSettings("IsUserValidate", "Y");
            }

            //检查-是否没有配置-验证时是否显示文本框
            string TextVisible = ConfigurationManager.AppSettings["TextVisible"];
            if (string.IsNullOrEmpty(TextVisible))//如果没有则添加,并默认NO
            {
                UpdateAppSettings("TextVisible", "NO");
            }

            //检查-危急值提示窗口是否最大化
            string IsMaximizedWindowState = ConfigurationManager.AppSettings["IsMaximizedWindowState"];
            if (string.IsNullOrEmpty(IsMaximizedWindowState))//如果没有则添加,并默认N
            {
                UpdateAppSettings("IsMaximizedWindowState", "N");
            }

            //检查-是否没有配置-外部浏览报告
            string IsOuterwebBrowse = ConfigurationManager.AppSettings["IsOuterwebBrowse"];
            if (string.IsNullOrEmpty(IsOuterwebBrowse))//如果没有则添加,并默认NO
            {
                UpdateAppSettings("IsOuterwebBrowse", "NO");
            }

            //检查-是否没有配置-是否与报告查询采用相同科室
            if (string.IsNullOrEmpty(IsReportSameDep))//如果没有则添加,并默认N
            {
                UpdateAppSettings("IsReportSameDep", "N");
            }

            //检查-是否没有配置-nw危急值标题
            if (string.IsNullOrEmpty(NwAppTile))//如果没有则添加
            {
                UpdateAppSettings("NwAppTile", "危急值报告提示");
            }

            if (string.IsNullOrEmpty(xijun_typeids))
            {
                UpdateAppSettings("xijun_typeids", "null");
            }

            if (string.IsNullOrEmpty(bingli_typeids))
            {
                UpdateAppSettings("bingli_typeids", "null");
            }
            //验证时的备注信息
            string ReadAffirmLabelText = ConfigurationManager.AppSettings["ReadAffirmLabelText"];
            //检查-是否没有配置-验证时的备注信息
            if (string.IsNullOrEmpty(ReadAffirmLabelText))//如果没有则添加
            {
                UpdateAppSettings("ReadAffirmLabelText", "以下请填写处理意见");
            }
            #endregion

            #region 佛山市一
            string strAuditType = ConfigurationManager.AppSettings["UserAuthType"];
            string RunWhenStart = ConfigurationManager.AppSettings["RunWhenStart"];
            if (strAuditType == "fsaudit" && string.IsNullOrEmpty(RunWhenStart))
            {
                UpdateAppSettings("RunWhenStart", "1");
                CommonTool.runWhenStart(true);
            }
            if (false)//佛山市一专用,主要身份验证OutLink fsaudit
            {
                UpdateAppSettings("UserAuthType", "fsaudit");
            }
            if (false)//佛山市一专用,主要身份验证OutLink
            {
                UpdateAppSettings("showWindow", "NEW");
            }
            #endregion

            string strshowWindow = ConfigurationManager.AppSettings["showWindow"];
            if ((!string.IsNullOrEmpty(strshowWindow)) && strshowWindow.ToUpperInvariant() == "NEW")
            {
                IsShowfrmMsgGather = true;
            }
            else
            {
                if (string.IsNullOrEmpty(strshowWindow)) UpdateAppSettings("showWindow", "old");//如果为空,则添加配置
                IsShowfrmMsgGather = false;
            }

            //下载报表
            if (ConfigurationManager.AppSettings["Enable_Print"] == "Y")
            {
                ReportDownloader downloader = new ReportDownloader(new string[] { });
                Thread report = new Thread(downloader.getReportsFile);//new ThreadStart()
                report.Start();
            }

            if (!bExistProcess)
            {
                if (IsShowfrmMsgGather)
                {
                    try
                    {
                        Application.Run(new FrmMsgGather());
                    }
                    catch (Exception ex)
                    {

                        Lib.LogManager.Logger.LogException(ex);
                    }

                }
                else
                {
                    Application.Run(new FrmMessages());
                }
            }
            else
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// 修改或增加配置项
        /// </summary>
        /// <param name="newKey">键</param>
        /// <param name="newValue">值</param>
        private static void UpdateAppSettings(string newKey, string newValue)
        {
            try
            {
                // Get the configuration file.
                System.Configuration.Configuration config =
                  ConfigurationManager.OpenExeConfiguration(
                  ConfigurationUserLevel.None);

                //添加
                config.AppSettings.Settings.Add(newKey, newValue);

                //修改
                config.AppSettings.Settings[newKey].Value = newValue;

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of the changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch
            {
                //throw;
            }

        }

        /// <summary>
        /// 为system.serviceModel/client添加节点endpoint
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="endpointBinding"></param>
        /// <param name="endpointBindingConfiguration"></param>
        /// <param name="endpointContract"></param>
        /// <param name="endpointAddress"></param>
        private static void AddEndpoint(string endpointName, string endpointBinding, string endpointBindingConfiguration, string endpointContract, string endpointAddress)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                System.ServiceModel.Configuration.ClientSection clientSection = config.GetSection("system.serviceModel/client") as System.ServiceModel.Configuration.ClientSection;
                foreach (System.ServiceModel.Configuration.ChannelEndpointElement item in clientSection.Endpoints)
                {
                    //如果存在,则不添加
                    if (item.Name == endpointName)
                        return;
                }

                System.ServiceModel.Configuration.ChannelEndpointElement itemAdd = new System.ServiceModel.Configuration.ChannelEndpointElement();
                itemAdd.Name = endpointName;
                itemAdd.Binding = endpointBinding;
                itemAdd.BindingConfiguration = endpointBindingConfiguration;
                itemAdd.Contract = endpointContract;
                if (!string.IsNullOrEmpty(endpointAddress))
                {
                    itemAdd.Address = new Uri(endpointAddress);
                }

                clientSection.Endpoints.Add(itemAdd);

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("system.serviceModel");
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 添加system.serviceModel/bindings
        /// </summary>
        /// <param name="bindingName"></param>
        private static void Addbinding(string bindingName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                System.ServiceModel.Configuration.BindingsSection clientSection = config.GetSection("system.serviceModel/bindings") as System.ServiceModel.Configuration.BindingsSection;

                if (clientSection.BasicHttpBinding.Bindings.Count <= 0)
                {
                    //<binding name="bigdata" closeTimeout="00:05:10" openTimeout="00:05:10" receiveTimeout="00:05:10" sendTimeout="00:05:10" 
                    //allowCookies="false" bypassProxyOnLocal="false" hostNameComparisonMode="StrongWildcard" 
                    //maxBufferSize="2147483647" maxBufferPoolSize="2147483647" maxReceivedMessageSize="2147483647" 
                    //messageEncoding="Text" textEncoding="utf-8" transferMode="Buffered" useDefaultWebProxy="true">
                    System.ServiceModel.Configuration.BasicHttpBindingElement elem = new System.ServiceModel.Configuration.BasicHttpBindingElement();
                    elem.Name = "bigdata";
                    elem.CloseTimeout = new TimeSpan(0, 5, 10);
                    elem.OpenTimeout = new TimeSpan(0, 5, 10);
                    elem.ReceiveTimeout = new TimeSpan(0, 5, 10);
                    elem.SendTimeout = new TimeSpan(0, 5, 10);
                    elem.AllowCookies = false;
                    elem.BypassProxyOnLocal = false;
                    elem.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                    elem.MaxBufferSize = 2147483647;
                    elem.MaxBufferPoolSize = 2147483647;
                    elem.MaxReceivedMessageSize = 2147483647;
                    elem.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                    elem.TextEncoding = System.Text.Encoding.UTF8;
                    elem.TransferMode = System.ServiceModel.TransferMode.Buffered;
                    elem.UseDefaultWebProxy = true;

                    //<readerQuotas maxDepth="2147483647" maxStringContentLength="2147483647" maxArrayLength="2147483647" 
                    //maxBytesPerRead="2147483647" maxNameTableCharCount="2147483647" />
                    elem.ReaderQuotas.MaxDepth = 2147483647;
                    elem.ReaderQuotas.MaxStringContentLength = 2147483647;
                    elem.ReaderQuotas.MaxArrayLength = 2147483647;
                    elem.ReaderQuotas.MaxBytesPerRead = 2147483647;
                    elem.ReaderQuotas.MaxNameTableCharCount = 2147483647;

                    //<security mode="None">
                    //  <transport clientCredentialType="None" proxyCredentialType="None" realm="" />
                    //  <message clientCredentialType="UserName" algorithmSuite="Default" />
                    //</security>
                    elem.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                    elem.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
                    elem.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                    elem.Security.Transport.Realm = "";
                    elem.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;
                    elem.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

                    clientSection.BasicHttpBinding.Bindings.Add(elem);
                }
                else
                {
                    if (clientSection.BasicHttpBinding.ConfiguredBindings.Count > 0)
                    {
                    }

                    return;
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("system.serviceModel");
            }
            catch
            {
                return;
            }
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
            ProxyReportMain proxyReport = new ProxyReportMain();
            EntityResponse result = proxyReport.Service.GetReport(new EntityRequest());//获得所有报表
            List<EntitySysReport> listSysReport = new List<EntitySysReport>();
            listSysReport = result.GetResult() as List<EntitySysReport>;

            List<EntitySysReport> filterSysReport = new List<EntitySysReport>();
            if (ReportCodes.Length > 0)
            {
                for (int i = 0; i < ReportCodes.Length; i++)
                {
                    foreach (var info in listSysReport)
                    {
                        if (info.RepCode.Equals(ReportCodes[i]))
                        {
                            filterSysReport.Add(info);
                        }
                    }
                }
            }
            else
            {
                filterSysReport = listSysReport;
            }

            EntitySysReport eyReport = new EntitySysReport();
            eyReport.RepLocation = "报表样本.repx";

            filterSysReport.Add(eyReport);

            //  string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport";
            string localPath = PathManager.ReportPath;
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            //string user = UserInfo.GetSysConfigValue("1");//得到系统配置

            string serverPath = ConfigurationManager.AppSettings["wcfAddr"].ToString() + @"xtraReport/";//服务器目录

            localPath += @"\";//本地目录

            string ex = "";

            System.Net.WebClient client = new System.Net.WebClient();
            client.Proxy = null;
            foreach (var infoRep in filterSysReport)
            {
                try
                {
                    string strRepPath = infoRep.RepLocation;//drRep["repAddress"].ToString();
                    string writePath = localPath + strRepPath;
                    string readPath = serverPath + strRepPath;
                    //if (user == "使用本地模版")
                    //{
                    //    if (!System.IO.File.Exists(writePath))
                    //        client.DownloadFile(readPath, writePath);
                    //}
                    //else
                    client.DownloadFile(readPath, writePath);

                }
                catch (Exception e)
                {
                    ex += "服务器无:" + infoRep.RepLocation + "报表！\r\n";
                }
            }

            if (ex != "")
            {
                //Logger.WriteException("barcodeclient", "getServerReportsFile", ex);
            }

            //XtraReport xtrTest = new XtraReport();
            //xtrTest.LoadLayout(localPath + "报表样本.repx");
            //xtrTest.Dispose();
        }
    }

}
