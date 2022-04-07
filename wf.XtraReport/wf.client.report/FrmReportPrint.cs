using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Drawing;
using dcl.client.frame;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.IO;
using DevExpress.XtraPrinting;
using dcl.client.common;
using lis.client.control;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using dcl.common;
using System.Drawing.Imaging;
using System.ServiceProcess;
using Lib.LogManager;
using dcl.entity;

namespace dcl.client.report
{
    public partial class FrmReportPrint : FrmCommon
    {
        MessageHelper helper;

        private string printTypeName = "";

        public FrmReportPrint()
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            InitializeComponent();
            base.ShowSucessMessage = false;
            HasShowPreview = false;
            isNotAllowCreateDoc = false;
            if (!Directory.Exists(localPath))
            {
                try
                {
                    Directory.CreateDirectory(localPath);
                }
                catch (Exception)
                { }
            }
            localPath += @"\";
        }
        // string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport";
        //  string xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\printConfig.xml";
        //  string barXmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\barcodePrintConfig.xml";
        string localPath = PathManager.ReportPath;
        string xmlFile = PathManager.SettingLisPath + @"\printXml\printConfig.xml";
        string barXmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";



        private bool batchPrint = false;

        public bool BatchPrint
        {
            get { return batchPrint; }
            set { batchPrint = value; }
        }
     

        Hashtable printParam;



        /// <summary>
        /// 显示打印按钮
        /// </summary>
        public bool ShowPrint = true;
        public bool HasShowPreview { get; set; }


        /// <summary>
        /// 打印(实体)
        /// </summary>
        /// <param name="listPrintData"></param>
        public void Print3(List<EntityPrintData> listPrintData)
        {
            Print3(listPrintData, string.Empty);
        }

        /// <summary>
        /// 打印(实体)
        /// </summary>
        /// <param name="listPrintData"></param>
        public void Print3(List<EntityPrintData> listPrintData, string printerName)
        {
            //系统配置:批量套打按病历号分批打印
            bool IsbatchPrintByPatInNo = ConfigHelper.GetSysConfigValueWithoutLogin("RepSel_batchPrintByPatInNo") == "是";

            if (!this.batchPrint) IsbatchPrintByPatInNo = false;

            if (!IsbatchPrintByPatInNo)
            {
                ShowPrint = true;
                currentPrintData = listPrintData;
                FrmReportPrint frp = new FrmReportPrint();
                XtraReport xr = new XtraReport();
                xr.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);
                frp.batchPrint = this.batchPrint;
                frp.Report3(true, listPrintData, xr, printerName);

                //批量打印预览
                if (this.batchPrint && HasShowPreview)
                {
                    //显示打印按钮
                    if (ShowPrint == false)
                        xr.PrintingSystem.SetCommandVisibility(new PrintingSystemCommand[] { PrintingSystemCommand.Print, PrintingSystemCommand.PrintDirect }, CommandVisibility.None);

                    xr.ShowPreviewDialog(this.LookAndFeel);

                    HasShowPreview = false;
                }
                else//正常打印
                {
                    xr.ExportToHtml(@"C:\test.html");
                    string ensurePrint = GetPrintName();
                    if (string.IsNullOrEmpty(printerName) && !string.IsNullOrEmpty(ensurePrint))
                    {
                        xr.Print(ensurePrint);
                    }
                    else
                    {
                        xr.Print();
                    }
                }
            }
            else
            {
                #region 套打分批打印

                ShowPrint = true;
                currentPrintData = listPrintData;

                List<XtraReport> list_xt = new List<XtraReport>();
                List<string> list_PatInNo = new List<string>();//记录不同病人的病历号
                int bCountAll = 1;//分多少批打印,默认1次

                //如果为套打则根据病人姓名分批
                if (this.batchPrint && listPrintData != null && listPrintData.Count > 0)
                {
                    //分组记录不同病人的病历号
                    foreach (EntityPrintData tempPData in listPrintData)
                    {
                        if (!list_PatInNo.Contains(tempPData.pat_in_no))
                        {
                            list_PatInNo.Add(tempPData.pat_in_no);
                        }
                    }
                    //如果不同病历号人数大于1,则分批处理
                    if (list_PatInNo.Count > 1)
                    {
                        bCountAll = list_PatInNo.Count;
                    }
                }

                for (int bCount = 0; bCount < bCountAll; bCount++)
                {
                    //取符合条件的listPrintData
                    List<EntityPrintData> temp_listPrintData = new List<EntityPrintData>();
                    if (bCountAll > 1)
                    {
                        foreach (EntityPrintData tempPData in listPrintData)
                        {
                            //筛选相同病历号的病人报告
                            if (list_PatInNo[bCount] == tempPData.pat_in_no)
                            {
                                temp_listPrintData.Add(tempPData);
                            }
                        }
                    }
                    else
                    {
                        temp_listPrintData = listPrintData;
                    }

                    if (temp_listPrintData.Count <= 0) continue;//如果没有符合条件的temp_listPrintData,则跳过

                    //生成打印控件
                    FrmReportPrint frp = new FrmReportPrint();
                    XtraReport xr = new XtraReport();
                    if ((bCount + 1) == bCountAll)
                    {
                        xr.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);
                    }
                    frp.batchPrint = this.batchPrint;
                    frp.Report3(true, temp_listPrintData, xr, printerName);
                    //
                    list_xt.Add(xr);
                }

                //开始分批打印
                if (list_xt.Count > 0)
                {
                    foreach (XtraReport xr in list_xt)
                    {
                        //批量打印预览
                        if (this.batchPrint && HasShowPreview)
                        {
                            //显示打印按钮
                            if (ShowPrint == false)
                                xr.PrintingSystem.SetCommandVisibility(new PrintingSystemCommand[] { PrintingSystemCommand.Print, PrintingSystemCommand.PrintDirect }, CommandVisibility.None);

                            xr.ShowPreviewDialog(this.LookAndFeel);
                        }
                        else
                        {
                            string ensurePrint = GetPrintName();
                            if (string.IsNullOrEmpty(printerName) && !string.IsNullOrEmpty(ensurePrint))
                            {
                                xr.Print(ensurePrint);
                            }
                            else
                            {
                                xr.Print();
                            }
                        }
                    }
                    HasShowPreview = false;
                }

                #endregion
            }
        }


        public Dictionary<string, MemoryStream> ExportToPDF(List<EntityPrintData> listPrintData)
        {
            FrmReportPrint frp = new FrmReportPrint();
            Dictionary<string, XtraReport> xtraReports = frp.Report4(listPrintData);
            return new PdfReportController().GenPdfReport(xtraReports);
        }

        public void ContinuousPrinting(List<EntityPrintData> listPrintDataSource)
        {
            string path = Application.StartupPath + "\\ContinuousPrinting.repx";
            if (!File.Exists(path))
            {
                MessageDialog.Show(string.Format("无法找到报表 {0}", path));
                return;
            }
            this.BatchPrint = false;


            Dictionary<string, List<EntityPrintData>> dict = new Dictionary<string, List<EntityPrintData>>();
            for (int i = 0; i < listPrintDataSource.Count; i++)
            {


                if (!dict.ContainsKey(listPrintDataSource[i].pat_name))
                {

                    dict.Add(listPrintDataSource[i].pat_name, new List<EntityPrintData>());
                }
                dict[listPrintDataSource[i].pat_name].Add(listPrintDataSource[i]);
            }
            XtraReport mainReport = new XtraReport();
            mainReport.PaperKind = PaperKind.A4;
            mainReport.ReportUnit = ReportUnit.HundredthsOfAnInch;
            mainReport.PrintingSystem.ShowMarginsWarning = false;
            printTypeName = "";
            string printerName = "";
            //设置打印机
            if (printerName == "")
                printTypeName = setPrintParameter(mainReport);
            else
                mainReport.PrintingSystem.PageSettings.PrinterName = printerName;

            mainReport.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);



            foreach (string item in dict.Keys)
            {
                List<EntityPrintData> listPrintData = dict[item];
                FrmReportPrint frp = new FrmReportPrint();
                //frp.BatchPrint=true;
                XtraReport xr = new XtraReport();
                xr.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);
                frp.Report3(true, listPrintData, xr, "");


                DataTable source = new DataTable();
                source.Columns.Add("img", typeof(byte[]));
                source.Columns.Add("name", typeof(string));
                source.Columns.Add("seq", typeof(int));

                for (int i = 0; i < xr.Pages.Count; i++)
                {
                    if (xr.Pages[i].PageSize.Height > 1900)
                    {

                        mainReport.Pages.Add(xr.Pages[i]);
                    }
                    else
                    {
                        ImageExportOptions options = new ImageExportOptions();
                        options.ExportMode = ImageExportMode.SingleFilePageByPage;

                        options.Format = ImageFormat.Png;
                        options.PageRange = (i + 1).ToString();
                        MemoryStream stream = new MemoryStream();
                        xr.ExportToImage(stream, options);
                        source.Rows.Add(new object[] { stream.ToArray(), item, i + 1 });
                    }

                }
                if (source.Rows.Count > 0)
                {
                    XtraReport imgReport = new XtraReport();

                    imgReport.LoadLayout(path);

                    imgReport.DataSource = source;
                    imgReport.CreateDocument();
                    mainReport.Pages.AddRange(imgReport.Pages);
                }

            }



            mainReport.Print();

        }

        public bool IsPreviewShowPrint = false;//预览时是否显示打印按钮
        public void PrintPreview3(List<EntityPrintData> listPrintData)
        {
            ShowPrint = false;
            currentPrintData = listPrintData;
            FrmReportPrint frp = new FrmReportPrint();
            XtraReport xr = new XtraReport();
            xr.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);

            frp.Report3(true, listPrintData, xr, "");

            //显示打印按钮
            if (ShowPrint == false)
                xr.PrintingSystem.SetCommandVisibility(new PrintingSystemCommand[] { PrintingSystemCommand.Print, PrintingSystemCommand.PrintDirect }, CommandVisibility.None);

            if (IsPreviewShowPrint)
                xr.PrintingSystem.SetCommandVisibility(new PrintingSystemCommand[] { PrintingSystemCommand.Print, PrintingSystemCommand.PrintDirect }, CommandVisibility.All);

            HasShowPreview = true;
            xr.ShowPreviewDialog(this.LookAndFeel);


            HasShowPreview = false;
        }

        private List<EntityPrintData> currentPrintData = null;


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="par">打印参数</param>
        /// <param name="printerName">打印机名称</param>
        public void Print2(Hashtable par, string printerName)
        {
            ShowPrint = true;
            printParam = par;
            FrmReportPrint frp = new FrmReportPrint();
            XtraReport xr = new XtraReport();
            xr.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(xr_PrintProgress);

            try
            {
                ServiceController cs = new ServiceController();
                cs.ServiceName = "Spooler";
                if (cs.Status != ServiceControllerStatus.Running)
                {
                    cs.Start();
                }
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("启动打印服务失败！");
            }

            PrintDocument pdc = new PrintDocument();
            string defaultName = pdc.PrinterSettings.PrinterName;

            if (defaultName.Trim() != printerName.Trim())
                SetDefaultPrinter(printerName);

            frp.Reports(true, par, xr, printerName);

            xr.Print();


            if (defaultName.Trim() != printerName.Trim())
                SetDefaultPrinter(defaultName);

        }

        #region 事件
        public delegate void PrintEventHandler(object sender, PrintEventArgs arg);
        public event PrintEventHandler PrintStart;
        public void OnPrintStart(Hashtable p)
        {
            if (PrintStart != null)
            {
                PrintStart(this, new PrintEventArgs(p));
            }
        }

        public delegate void PrintEventHandler2(object sender, PrintEventArgs arg);
        public event PrintEventHandler2 PrintStart2;
        public void OnPrintStart2(List<EntityPrintData> p)
        {
            PrintEventArgs eventArgs = new PrintEventArgs(null);
            eventArgs.listPrintData = p;

            if (PrintStart2 != null)
            {
                PrintStart2(this, eventArgs);
            }
        }
        #endregion

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDefaultPrinter(string Name);


        protected void xr_PrintProgress(object sender, DevExpress.XtraPrinting.PrintProgressEventArgs e)
        {
            if (e.PageIndex == 0 && e.PrintAction != System.Drawing.Printing.PrintAction.PrintToPreview)
            {
                OnPrintStart(printParam);
                OnPrintStart2(this.currentPrintData);
            }
        }

        /// <summary>
        /// 不同报表混合打印
        /// </summary>
        /// <param name="isTrue">开关，是否使用本地/服务器版本</param>
        /// <param name="par">参数</param>
        /// <param name="xr">打印对象</param>
        private void Reports(bool isTrue, Hashtable par, XtraReport xr, string printerName)
        {
            xr.PageHeight = 827;
            xr.PageWidth = 1169;
            xr.ReportUnit = ReportUnit.HundredthsOfAnInch;
            xr.PrintingSystem.ShowMarginsWarning = false;
            printTypeName = "";

            if (printerName == "")
                printTypeName = setPrintParameter(xr);
            else
                xr.PrintingSystem.PageSettings.PrinterName = printerName;

            List<string> lis = new List<string>();

            foreach (DictionaryEntry deRep in par)
            {
                DataTable dtRep = CommonClient.CreateDT(new string[] { "code" }, "rep");
                string[] str = deRep.Key.ToString().Split('@');
                dtRep.Rows.Add(str[0]);
                DataTable dtReport = base.doSearch(dtRep).Tables["report"];
                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    DataRow drReports = dtReport.Rows[0];


                    string path = "";//报表地址
                    string reportPath = drReports["repAddress"].ToString();
                    string conn_code = drReports["repConnCode"].ToString();
                    path = localPath + reportPath.Split('.')[0] + printTypeName + "." + reportPath.Split('.')[1];

                    if (File.Exists(path))//报表存在
                    {
                        Hashtable htRep = (Hashtable)deRep.Value;//得到条件

                        ReportParameter repPar = new ReportParameter(htRep, true, path, drReports["repSql"].ToString(), drReports["repCode"].ToString(), conn_code);

                        XtraReport xrRpe = getSubreport(repPar);

                        if (xrRpe != null)
                        {
                            xr.Pages.AddRange(xrRpe.Pages);
                        }
                    }
                    else
                    {
                        lis.Add(drReports["repName"].ToString() + "报表不存在");
                    }
                }
                else
                {
                    if (deRep.Key.ToString() != "&parameter&")
                    {
                        lis.Add("不存在报表代码为：" + deRep.Key.ToString() + "的报表！");
                    }
                }


            }

            if (lis.Count > 0)
            {
                string erro = "";
                foreach (string st in lis)
                {
                    erro += st + "\r\n";
                }
                ReportNotFoundException exRep = new ReportNotFoundException();
                exRep.MSG = erro;
                throw exRep;
            }
        }

        bool isNotAllowCreateDoc = false;

        /// <summary>
        /// this:输出打印者
        /// </summary>
        private string thisOutputPrintPerson { get; set; }
        /// <summary>
        /// this:输出打印时间
        /// </summary>
        private string thisOutputPrintTime { get; set; }

        private void Report3(bool useServerReoprt, List<EntityPrintData> listPrintData, XtraReport xr, string printerName)
        {

            xr.PaperKind = PaperKind.A4;
            xr.ReportUnit = ReportUnit.HundredthsOfAnInch;
            xr.PrintingSystem.ShowMarginsWarning = false;
            xr.Landscape = GetpageSettingsLandscape();//获取纵横向打印
            printTypeName = "";

            //设置打印机
            if (printerName == "")
                printTypeName = setPrintParameter(xr);
            else
                xr.PrintingSystem.PageSettings.PrinterName = printerName;

            List<string> lis = new List<string>();

            DataSet dsReportSour = null;
            DetailBand Detail = new DetailBand();
            int y = 0;
            if (batchPrint)
            {
                xr.LoadLayout(localPath + "临床批量打印报表.repx");
                Detail = xr.FindControl("Detail", true) as DetailBand;
                printTypeName = "_套打";
                #region 合并报告单到A4纸
                //ImageExportOptions options = new ImageExportOptions();

                //options.ExportMode = ImageExportMode.SingleFilePageByPage;

                //options.Format = ImageFormat.Jpeg;

                //xr.Margins.Bottom = 50;
                //xr.Margins.Top = 50;
                //xr.Margins.Right = 0;
                //xr.Margins.Left = 0;

                //XRPictureBox pic = new XRPictureBox();
                //pic.Location = new Point(0, 0);
                //pic.Height = Convert.ToInt32(xr.PageHeight / 2);
                //pic.Width = xr.PageWidth;
                //pic.Sizing = ImageSizeMode.StretchImage;
                //pic.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                //            new DevExpress.XtraReports.UI.XRBinding("Image", null, "可设计字段.test", "")});

                ////DetailBand Detail = new DetailBand();
                //Detail.Controls.Add(pic);

                //xr.Bands.Add(Detail);
                //DataSet dataset = new DataSet();
                //DataTable dtTest = new DataTable("可设计字段");
                //dtTest.Columns.Add("test", typeof(System.Drawing.Image));
                #endregion
            }

            foreach (EntityPrintData entityPrintData in listPrintData)
            {
                DataTable dtRep = CommonClient.CreateDT(new string[] { "code" }, "rep");

                dtRep.Rows.Add(entityPrintData.ReportCode);
                DataTable dtReport = base.doSearch(dtRep).Tables["report"];

                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    DataRow drReports = dtReport.Rows[0];
                    string path = "";

                    #region 得到报表地址
                    string reportPath = drReports["repAddress"].ToString();
                    path = localPath + reportPath.Split('.')[0] + printTypeName + "." + reportPath.Split('.')[1];
                    #endregion

                    if (File.Exists(path))//报表存在
                    {
                        XtraReport xrRpe;

                        Hashtable htRep = new Hashtable();

                        foreach (EntityPrintParameter printParam in entityPrintData.Parameters)
                        {
                            if (!htRep.Contains(printParam.Name))
                            {
                                htRep.Add(printParam.Name, printParam.Value);
                            }
                        }

                        #region 赋值：传入打印者与打印时间

                        if (!string.IsNullOrEmpty(entityPrintData.outputPrintPerson)
                                            && !string.IsNullOrEmpty(entityPrintData.outputPrintTime))
                        {
                            thisOutputPrintPerson = entityPrintData.outputPrintPerson;
                            thisOutputPrintTime = entityPrintData.outputPrintTime;
                        }
                        else
                        {
                            thisOutputPrintPerson = null;
                            thisOutputPrintTime = null;
                        }

                        #endregion

                        if (drReports["repCode"].ToString() == "InspectionReport_pat")
                        {
                            xrRpe = getImgSubReport(htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("Picture_pat")) // 2010-6-10 支持多图像报表
                        {
                            xrRpe = getImgReport(drReports["repCode"].ToString(), htRep, true);// 2010-6-10 支持多图像报表  // xrRpe = getImgReport(htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("NestReport"))
                        {
                            xrRpe = getNestReport(drReports["repCode"].ToString(), htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("bacilli"))
                        {
                            xrRpe = getBacReport(htRep, batchPrint ? false : true);
                        }
                        else
                        {
                            xrRpe = new XtraReport();
                            string sql = EncryptClass.Decrypt(drReports["repSql"].ToString());//得到sql并解密

                            foreach (EntityPrintParameter printParam in entityPrintData.Parameters)//把条件填入
                            {
                                sql = sql.Replace(printParam.Name, printParam.Value);
                            }
                            DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                            dtSql.Rows.Add(sql, drReports["repConnCode"].ToString());
                            DataSet dsReport = base.doOther(dtSql);//得到数据源


                            try
                            {
                                if (!batchPrint)
                                {
                                    #region 传入打印者与打印时间

                                    if (!string.IsNullOrEmpty(entityPrintData.outputPrintPerson)
                                        && !string.IsNullOrEmpty(entityPrintData.outputPrintTime))
                                        if (dsReport != null)
                                        {
                                            DataSet dstempAddCol = dsReport;

                                            if (dstempAddCol != null && dstempAddCol.Tables.Count > 0
                                                && dstempAddCol.Tables[0] != null
                                                && dstempAddCol.Tables[0].Rows.Count > 0
                                                && dstempAddCol.Tables[0].Columns.Contains("传入打印者")
                                                && dstempAddCol.Tables[0].Columns.Contains("传入打印时间"))
                                            {
                                                foreach (DataRow drtempAddCol in dstempAddCol.Tables[0].Rows)
                                                {
                                                    drtempAddCol["传入打印者"] = entityPrintData.outputPrintPerson;
                                                    drtempAddCol["传入打印时间"] = entityPrintData.outputPrintTime;
                                                }
                                            }
                                        }

                                    #endregion
                                }
                                xrRpe.LoadLayout(path);//加载样式
                                if (GetpageSettingsLandscape())//获取纵横向打印
                                {
                                    xrRpe.Landscape = true;
                                    int tempwidth = xrRpe.PageWidth;
                                    xrRpe.PageWidth = xrRpe.PageHeight;
                                    xrRpe.PageHeight = tempwidth;
                                }

                                SetHospitalName.setName(xrRpe.Bands);//设置医院名称
                                xrRpe.DataSource = dsReport;//赋予报表数据源
                                xrRpe.CreateDocument();
                            }
                            catch (Exception ex)
                            {
                                lis.Add(drReports["repName"].ToString() + "报表读取错误！(报表)");
                                //Logger.WriteException("FrmReportPrint", "报表读取错误！", ex.ToString());
                                Logger.LogException("报表读取错误！", ex);
                            }
                        }

                        if (xrRpe != null)
                        {
                            if (batchPrint)
                            {
                                #region 传入打印者与打印时间

                                if (!string.IsNullOrEmpty(entityPrintData.outputPrintPerson)
                                    && !string.IsNullOrEmpty(entityPrintData.outputPrintTime))
                                    if (xrRpe.DataSource != null && xrRpe.DataSource is DataSet)
                                    {
                                        DataSet dstempAddCol = (DataSet)xrRpe.DataSource;

                                        if (dstempAddCol != null && dstempAddCol.Tables.Count > 0
                                            && dstempAddCol.Tables[0] != null
                                            && dstempAddCol.Tables[0].Rows.Count > 0
                                            && dstempAddCol.Tables[0].Columns.Contains("传入打印者")
                                            && dstempAddCol.Tables[0].Columns.Contains("传入打印时间"))
                                        {
                                            foreach (DataRow drtempAddCol in dstempAddCol.Tables[0].Rows)
                                            {
                                                drtempAddCol["传入打印者"] = entityPrintData.outputPrintPerson;
                                                drtempAddCol["传入打印时间"] = entityPrintData.outputPrintTime;
                                            }
                                        }
                                    }

                                #endregion

                                if (dsReportSour == null)
                                    dsReportSour = (DataSet)xrRpe.DataSource;

                                XRSubreport sbRep = new XRSubreport();
                                sbRep.Size = new Size(827, 100);
                                sbRep.ReportSource = xrRpe;
                                xrRpe.CreateDocument();
                                sbRep.Location = new Point(0, y);
                                Detail.Controls.Add(sbRep);
                                y += 100;
                            }
                            else
                            {
                                xr.Pages.AddRange(xrRpe.Pages);
                                if (xrRpe.Watermark != null && !string.IsNullOrEmpty(xrRpe.Watermark.Text))
                                {
                                    PageWatermark page = new PageWatermark();
                                    page.Text = xrRpe.Watermark.Text;
                                    page.TextDirection = xrRpe.Watermark.TextDirection;
                                    page.TextTransparency = xrRpe.Watermark.TextTransparency;
                                    page.Font = xrRpe.Watermark.Font;
                                    page.ForeColor = xrRpe.Watermark.ForeColor;
                                    page.Image = xrRpe.Watermark.Image;
                                    xr.Pages[xr.Pages.Count - 1].AssignWatermark(page);
                                }
                            }

                            #region 合并报告单到A4纸
                            //for (int i = 0; i < xrRpe.Pages.Count; i++)
                            //{
                            //    options.PageRange = (i + 1).ToString();
                            //    MemoryStream stream = new MemoryStream();
                            //    xrRpe.ExportToImage(stream, options);
                            //    dtTest.Rows.Add(Bitmap.FromStream(stream));
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        lis.Add(drReports["repName"].ToString() + "报表不存在");
                    }
                }
            }
            if (batchPrint)
            {
                xr.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                if (dsReportSour != null && dsReportSour.Tables.Count > 0)
                {
                    DataSet dsXR = new DataSet();
                    DataTable dtXR = dsReportSour.Tables[0].Clone();
                    dtXR.TableName = "可设计字段";
                    dtXR.Rows.Add(dsReportSour.Tables[0].Rows[0].ItemArray);
                    dsXR.Tables.Add(dtXR);
                    xr.DataSource = dsXR;
                }
            }

            if (lis.Count > 0)
            {
                string erro = "";
                foreach (string st in lis)
                {
                    erro += st + "\r\n";
                }
                ReportNotFoundException exRep = new ReportNotFoundException();
                exRep.MSG = erro;
                throw exRep;
            }
        }

        private Dictionary<string, XtraReport> Report4(List<EntityPrintData> listPrintData)
        {
            List<string> lis = new List<string>();
            Dictionary<string, XtraReport> lisXtraReport = new Dictionary<string, XtraReport>();

            DataSet dsReportSour = null;
            DetailBand Detail = new DetailBand();
            int y = 0;

            foreach (EntityPrintData entityPrintData in listPrintData)
            {

                DataTable dtRep = CommonClient.CreateDT(new string[] { "code" }, "rep");

                dtRep.Rows.Add(entityPrintData.ReportCode);
                DataTable dtReport = base.doSearch(dtRep).Tables["report"];

                if (dtReport != null && dtReport.Rows.Count > 0)
                {
                    DataRow drReports = dtReport.Rows[0];
                    string path = "";

                    #region 得到报表地址
                    string reportPath = drReports["repAddress"].ToString();
                    path = localPath + reportPath.Split('.')[0] + printTypeName + "." + reportPath.Split('.')[1];
                    #endregion

                    if (File.Exists(path))//报表存在
                    {
                        XtraReport xrRpe;

                        Hashtable htRep = new Hashtable();

                        foreach (EntityPrintParameter printParam in entityPrintData.Parameters)
                        {
                            if (!htRep.Contains(printParam.Name))
                            {
                                htRep.Add(printParam.Name, printParam.Value);
                            }
                        }

                        if (drReports["repCode"].ToString() == "InspectionReport_pat")
                        {
                            xrRpe = getImgSubReport(htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("Picture_pat")) // 2010-6-10 支持多图像报表
                        {
                            xrRpe = getImgReport(drReports["repCode"].ToString(), htRep, true);// 2010-6-10 支持多图像报表  // xrRpe = getImgReport(htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("NestReport"))
                        {
                            xrRpe = getNestReport(drReports["repCode"].ToString(), htRep, true);
                        }
                        else if (drReports["repCode"].ToString().StartsWith("bacilli"))
                        {
                            xrRpe = getBacReport(htRep, batchPrint ? false : true);
                        }
                        else
                        {
                            xrRpe = new XtraReport();
                            string sql = EncryptClass.Decrypt(drReports["repSql"].ToString());//得到sql并解密

                            foreach (EntityPrintParameter printParam in entityPrintData.Parameters)//把条件填入
                            {
                                sql = sql.Replace(printParam.Name, printParam.Value);
                            }
                            DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                            dtSql.Rows.Add(sql, drReports["repConnCode"].ToString());
                            DataSet dsReport = base.doOther(dtSql);//得到数据源


                            try
                            {
                                xrRpe.LoadLayout(path);//加载样式
                                SetHospitalName.setName(xrRpe.Bands);//设置医院名称
                                xrRpe.DataSource = dsReport;//赋予报表数据源
                                xrRpe.CreateDocument();
                            }
                            catch (Exception ex)
                            {
                                lis.Add(drReports["repName"].ToString() + "报表读取错误！(报表)");
                                //Logger.WriteException("FrmReportPrint", "报表读取错误！", ex.ToString());
                                Logger.LogException("报表读取错误！", ex);
                            }
                        }

                        if (xrRpe != null)
                        {
                            if (!lisXtraReport.ContainsKey(entityPrintData.pat_id))
                                lisXtraReport.Add(entityPrintData.pat_id, xrRpe);
                        }
                    }
                    else
                    {
                        lis.Add(drReports["repName"].ToString() + "报表不存在");
                    }
                }
            }



            if (lis.Count > 0)
            {
                string erro = "";
                foreach (string st in lis)
                {
                    erro += st + "\r\n";
                }
                ReportNotFoundException exRep = new ReportNotFoundException();
                exRep.MSG = erro;
                throw exRep;
            }
            return lisXtraReport;
        }

        private void Reports(bool istrue, DataTable par, string reportName, XtraReport xrRpe)
        {
            xrRpe.PrintingSystem.ShowMarginsWarning = false;
            string path = localPath + reportName.Split('.')[0] + printTypeName + "." + reportName.Split('.')[1];
            if (File.Exists(path))
            {
                try
                {
                    par.TableName = "可设计字段";
                    xrRpe.LoadLayout(path);
                    SetHospitalName.setName(xrRpe.Bands);
                    xrRpe.DataSource = par;
                }
                catch (Exception ex)
                {
                    //Logger.WriteException("FrmReportPrint", "报表读取错误！", ex.ToString());
                    Logger.LogException("报表读取错误！", ex);
                    lis.client.control.MessageDialog.Show("报表读取错误！", "提示");
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("该报表不存在！", "提示");
            }
        }

        private void Reports(bool istrue, DataSet par, string reportName, XtraReport xrRpe)
        {
            xrRpe.PrintingSystem.ShowMarginsWarning = false;
            string path = localPath + reportName.Split('.')[0] + printTypeName + "." + reportName.Split('.')[1];
            if (File.Exists(path))
            {
                try
                {
                    xrRpe.LoadLayout(path);
                    SetHospitalName.setName(xrRpe.Bands);
                    xrRpe.DataSource = par;
                }
                catch (Exception ex)
                {
                    //Logger.WriteException("FrmReportPrint", "报表读取错误！", ex.ToString());
                    Logger.LogException("报表读取错误！", ex);
                    lis.client.control.MessageDialog.Show("报表读取错误！", "提示");
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("该报表不存在！", "提示");
            }
        }

        /// <summary>
        /// 得到子报表
        /// </summary>
        /// <param name="repPar"></param>
        /// <returns></returns>
        private XtraReport getSubreport(ReportParameter repPar)
        {
            if (repPar.ReportType == "InspectionReport_pat")
            {
                return getImgSubReport(repPar.SubReportParameter, true);
            }
            else if (repPar.ReportType.StartsWith("Picture_pat")) // 2010-6-10 支持多图像报表
            {
                return getImgReport(repPar.ReportType, repPar.SubReportParameter, true);// 2010-6-10 支持多图像报表  // xrRpe = getImgReport(htRep, true);
            }
            else if (repPar.ReportType.StartsWith("NestReport"))
            {
                return getNestReport(repPar.ReportType, repPar.SubReportParameter, true);
            }
            else if (repPar.ReportType.StartsWith("bacilli"))
            {
                return getBacReport(repPar.SubReportParameter, true);
            }
            else
            {
                return getGeneralReport(repPar.SubReportParameter, repPar.Sql, repPar.Path, repPar.ConnectionCode);
            }
        }

        #region 各种子报表设计方法

        /// <summary>
        /// 细菌报表处理
        /// </summary>
        /// <param name="htRep"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private XtraReport getBacReport(Hashtable htRep, bool isTrue)
        {
            DataTable dtBacId = CommonClient.CreateDT(new string[] { "pat_id" }, "patients");
            foreach (DictionaryEntry de in htRep)
            {
                if (de.Key.ToString().Trim() == "&where&")
                    dtBacId.Rows.Add(de.Value);
            }

            DataSet dsBac = base.doUpdate(dtBacId);
            DataTable dtPat = dsBac.Tables["patients"];
            XtraReport xr = new XtraReport();
            if (dtPat.Rows.Count > 0)
            {
                foreach (DataRow drPat in dtPat.Rows)
                {
                    XtraReport xtrPat = new XtraReport();
                    string reportPath = drPat["repAddress"].ToString();
                    xtrPat.LoadLayout(localPath + reportPath.Split('.')[0] + printTypeName + "." + reportPath.Split('.')[1]);
                    string sql = EncryptClass.Decrypt(drPat["repSql"].ToString());//得到sql并解密
                    sql = sql.Replace("&where&", drPat["repWhere"].ToString());
                    DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                    dtSql.Rows.Add(sql, drPat["repConnCode"].ToString());
                    DataSet dsReport = base.doOther(dtSql);

                    #region this:传入打印者与打印时间

                    if (!string.IsNullOrEmpty(thisOutputPrintPerson)
                        && !string.IsNullOrEmpty(thisOutputPrintTime))
                    {
                        if (dsReport != null)
                        {
                            DataSet dstempAddCol = dsReport;

                            if (dstempAddCol != null && dstempAddCol.Tables.Count > 0
                                && dstempAddCol.Tables[0] != null
                                && dstempAddCol.Tables[0].Rows.Count > 0
                                && dstempAddCol.Tables[0].Columns.Contains("传入打印者")
                                && dstempAddCol.Tables[0].Columns.Contains("传入打印时间"))
                            {
                                foreach (DataRow drtempAddCol in dstempAddCol.Tables[0].Rows)
                                {
                                    drtempAddCol["传入打印者"] = thisOutputPrintPerson;
                                    drtempAddCol["传入打印时间"] = thisOutputPrintTime;
                                }
                            }
                        }
                    }

                    #endregion

                    xtrPat.DataSource = dsReport;
                    SetHospitalName.setName(xtrPat.Bands);//设置医院名称
                    if (isTrue)
                    {
                        if (!isNotAllowCreateDoc)
                            xtrPat.CreateDocument();
                        xr.Pages.AddRange(xtrPat.Pages);
                    }
                    else
                        xr = xtrPat;
                }
            }


            return xr;
        }

        /// <summary>
        /// 普通报表
        /// </summary>
        /// <param name="htRep"></param>
        /// <param name="strSql"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private XtraReport getGeneralReport(Hashtable htRep, string strSql, string path, string conn_code)
        {
            XtraReport xrRpe = new XtraReport();
            string sql = EncryptClass.Decrypt(strSql);//得到sql并解密
            foreach (DictionaryEntry de in htRep)//把条件填入
            {
                sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
            }
            DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
            dtSql.Rows.Add(sql, conn_code);
            DataSet dsReport = base.doOther(dtSql);//得到数据源
            try
            {
                xrRpe.LoadLayout(path);//加载样式
                SetHospitalName.setName(xrRpe.Bands);//设置医院名称
                xrRpe.DataSource = dsReport;//赋予报表数据源
                if (!isNotAllowCreateDoc)
                    xrRpe.CreateDocument();
            }
            catch (Exception ex)
            {
                Logger.LogException("报表读取错误！", ex);
            }

            return xrRpe;



        }

        /// <summary>
        /// 特殊图形报表(自动双列带转换图片)
        /// </summary>
        /// <param name="xr"></param>
        /// <returns></returns>
        public XtraReport getImgSubReport(Hashtable hsSr, bool isTrue)
        {
            DataTable dtRepCode = CommonClient.CreateDT(new string[] { "repCode" }, "report");
            dtRepCode.Rows.Add("where repCode in('InspectionReport_pat','InspectionReport_res','InspectionReport_pic')");
            DataTable dtSr = base.doView(dtRepCode).Tables["SubReport"];//获得子报表数据源
            XtraReport xr = new XtraReport();
            DataRow[] drPats = dtSr.Select("repCode='InspectionReport_pat'");//得到病人报表
            DataRow[] drRes = dtSr.Select("repCode='InspectionReport_res'");//得到结果报表
            DataRow[] drPic = dtSr.Select("repCode='InspectionReport_pic'");//得到图片报表
            //string rootPath = Application.StartupPath.ToString() + @"\xtraReport\";//得到根路径
            string patAddress = localPath + drPats[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drPats[0]["repAddress"].ToString().Split('.')[1];
            string resAddress = localPath + drRes[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drRes[0]["repAddress"].ToString().Split('.')[1];
            string picAddress = localPath + drPic[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drPic[0]["repAddress"].ToString().Split('.')[1];
            if (drRes.Length <= 0 || !File.Exists(resAddress))
                resAddress = null;
            if (drPic.Length <= 0 || !File.Exists(picAddress))
                picAddress = null;
            if (drPats.Length > 0)
            {
                string sql = EncryptClass.Decrypt(drPats[0]["repSql"].ToString());
                foreach (DictionaryEntry de in hsSr)
                {
                    sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
                }
                DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                dtSql.Rows.Add(sql, drPats[0]["repConnCode"].ToString());
                DataTable dtPat = base.doOther(dtSql).Tables["可设计字段"];
                if (dtPat.Rows.Count > 0)
                {
                    foreach (DataRow drPat in dtPat.Rows)
                    {
                        #region  读取总设计报表
                        xtrRepPatients xtrPat = new xtrRepPatients();
                        DataTable dtXtPar = dtPat.Clone();
                        dtXtPar.Rows.Add(drPat.ItemArray);
                        dtXtPar.TableName = "可设计字段";
                        xtrPat.LoadLayout(patAddress);
                        SetHospitalName.setName(xtrPat.Bands);//设置医院名称
                        xtrPat.DataSource = dtXtPar;//赋予报表数据源
                        #endregion

                        #region 读取左列报表
                        xtrRepResulto xrRep = new xtrRepResulto();
                        DataTable dtXtRes = dtSql.Clone();
                        if (resAddress != null)
                            xrRep.LoadLayout(resAddress);
                        string resSql = EncryptClass.Decrypt(drRes[0]["repSql"].ToString());
                        resSql = resSql.Replace("&where&", " and res_id='" + drPat["标识ID"] + "'");
                        dtXtRes.Rows.Add(resSql, drRes[0]["repConnCode"].ToString());
                        DataSet dsRepSour = base.doOther(dtXtRes);


                        DataSet dsLeft = new DataSet();
                        DataTable dtResLeft = dsRepSour.Tables[0].Clone();

                        foreach (DataRow drRepLeft in dsRepSour.Tables[0].Rows)
                        {
                            if (drRepLeft["结果显示位置"].ToString() == "0")
                                dtResLeft.Rows.Add(drRepLeft.ItemArray);
                        }

                        dsLeft.Tables.Add(dtResLeft);
                        xrRep.DataSource = dsLeft;
                        xtrPat.xtrResu.ReportSource = xrRep;
                        #endregion

                        #region 读取右列结果报表
                        xtrRepResulto xrRepRig = new xtrRepResulto();
                        DataTable dtRepRig = dtResLeft.Clone();
                        if (resAddress != null)
                            xrRepRig.LoadLayout(resAddress);
                        DataSet ds = new DataSet();
                        foreach (DataRow drRep in dsRepSour.Tables[0].Rows)
                        {
                            if (drRep["结果显示位置"].ToString() == "1")
                                dtRepRig.Rows.Add(drRep.ItemArray);
                        }
                        ds.Tables.Add(dtRepRig);
                        xrRepRig.DataSource = ds;
                        xtrPat.xtrResu_Rig.ReportSource = xrRepRig;
                        #endregion

                        #region  处理图片数据源以及报表
                        xtrRepResulto_p xrPic = new xtrRepResulto_p();
                        DataTable dtXtPic = dtSql.Clone();
                        if (picAddress != null)
                            xrPic.LoadLayout(picAddress);
                        string picSql = EncryptClass.Decrypt(drPic[0]["repSql"].ToString());
                        picSql = picSql.Replace("&where&", " and pres_id='" + drPat["标识ID"] + "'");
                        dtXtPic.Rows.Add(picSql, drPic[0]["repConnCode"].ToString());
                        DataSet dsPic = base.doOther(dtXtPic);
                        DataTable dtPic = dsPic.Tables["可设计字段"];
                        foreach (DataRow drImg in dtPic.Rows)
                        {
                            drImg["图片"] = Convert.FromBase64String(drImg["图像源"].ToString());
                        }
                        xrPic.DataSource = dsPic;
                        #endregion

                        xtrPat.xtrRepResp.ReportSource = xrPic;
                        if (isTrue)
                        {
                            if (!isNotAllowCreateDoc)
                                xtrPat.CreateDocument();
                            xr.Pages.AddRange(xtrPat.Pages);
                        }
                        else
                            xr = xtrPat;
                    }
                }
            }

            return xr;
        }


        /// <summary>
        /// 特殊图形报表(单列不转换图片)
        /// </summary>
        /// <param name="hsSr"></param>
        /// <param name="isTrue"></param>
        /// <returns></returns>
        public XtraReport getImgReport(string reportName, Hashtable hsSr, bool isTrue) // 2010-6-30
        {
            DataTable dtRepCode = CommonClient.CreateDT(new string[] { "repCode" }, "report");
            dtRepCode.Rows.Add("where repCode like 'Picture_pat%' or repCode like 'Picture_res%' or repCode like 'Picture_pic%' or repCode like 'Picture_other%'");
            DataTable dtSr = base.doView(dtRepCode).Tables["SubReport"];//获得子报表数据源
            XtraReport xr = new XtraReport();
            string suffix = reportName.Replace("Picture_pat", "");
            string resultReport = "Picture_res" + suffix;
            string resultOtherReport = "Picture_other" + suffix;
            string picturetReport = "Picture_pic" + suffix;
            DataRow[] drPats = dtSr.Select("repCode like '" + reportName + "'");//得到病人报表
            DataRow[] drRes = dtSr.Select("repCode like '" + resultReport + "'");//得到结果报表
            DataRow[] drPic = dtSr.Select("repCode like '" + picturetReport + "'");//得到图片报表
            DataRow[] drOther = dtSr.Select("repCode like '" + resultOtherReport + "'");//得到结果报表
            string patAddress = localPath + drPats[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drPats[0]["repAddress"].ToString().Split('.')[1];
            string resAddress = localPath + drRes[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drRes[0]["repAddress"].ToString().Split('.')[1];
            string otherAddress = "";
            if (drOther != null && drOther.Length > 0)
                otherAddress = localPath + drOther[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drOther[0]["repAddress"].ToString().Split('.')[1];
            string picAddress = localPath + drPic[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drPic[0]["repAddress"].ToString().Split('.')[1];
            if (drRes.Length <= 0 || !File.Exists(resAddress))
                resAddress = null;

            if (drOther.Length <= 0 || !File.Exists(otherAddress))
                otherAddress = null;
            if (drPic.Length <= 0 || !File.Exists(picAddress))
                picAddress = null;
            if (drPats.Length > 0)
            {
                string sql = EncryptClass.Decrypt(drPats[0]["repSql"].ToString());
                foreach (DictionaryEntry de in hsSr)
                {
                    sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
                }
                DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                dtSql.Rows.Add(sql, drPats[0]["repConnCode"].ToString());
                DataTable dtPat = base.doOther(dtSql).Tables["可设计字段"];
                if (dtPat.Rows.Count > 0)
                {
                    foreach (DataRow drPat in dtPat.Rows)
                    {
                        #region  读取总设计报表
                        xtrRepPatients xtrPat = new xtrRepPatients();
                        DataTable dtXtPar = dtPat.Clone();
                        dtXtPar.Rows.Add(drPat.ItemArray);
                        dtXtPar.TableName = "可设计字段";
                        xtrPat.LoadLayout(patAddress);
                        SetHospitalName.setName(xtrPat.Bands);//设置医院名称
                        xtrPat.DataSource = dtXtPar;//赋予报表数据源
                        #endregion

                        bool twoColumn = drPats[0]["repName"].ToString().Contains("双列");
                        if (twoColumn)
                        {
                            #region 读取左列报表
                            xtrRepResulto xrRep = new xtrRepResulto();
                            DataTable dtXtRes = dtSql.Clone();
                            if (resAddress != null)
                                xrRep.LoadLayout(resAddress);
                            string resSql = EncryptClass.Decrypt(drRes[0]["repSql"].ToString());
                            resSql = resSql.Replace("&where&", " and res_id='" + drPat["标识ID"] + "'");
                            dtXtRes.Rows.Add(resSql, drRes[0]["repConnCode"].ToString());
                            DataSet dsRepSour = base.doOther(dtXtRes);


                            DataSet dsLeft = new DataSet();
                            DataTable dtResLeft = dsRepSour.Tables[0].Clone();

                            foreach (DataRow drRepLeft in dsRepSour.Tables[0].Rows)
                            {

                                if (drRepLeft["结果显示位置"].ToString() == "0" && (string.IsNullOrEmpty(otherAddress) || (!string.IsNullOrEmpty(otherAddress)
                                    && drRepLeft["镜检标志"].ToString() != "1")))
                                {
                                    dtResLeft.Rows.Add(drRepLeft.ItemArray);
                                }

                            }

                            dsLeft.Tables.Add(dtResLeft);
                            xrRep.DataSource = dsLeft;
                            xtrPat.xtrResu.ReportSource = xrRep;
                            #endregion

                            #region 读取右列结果报表
                            xtrRepResulto xrRepRig = new xtrRepResulto();
                            DataTable dtRepRig = dtResLeft.Clone();
                            if (resAddress != null)
                                xrRepRig.LoadLayout(resAddress);
                            DataSet ds = new DataSet();
                            foreach (DataRow drRep in dsRepSour.Tables[0].Rows)
                            {
                                if (drRep["结果显示位置"].ToString() == "1" && (string.IsNullOrEmpty(otherAddress) || (!string.IsNullOrEmpty(otherAddress)
                                    && drRep["镜检标志"].ToString() != "1")))
                                    dtRepRig.Rows.Add(drRep.ItemArray);
                            }
                            ds.Tables.Add(dtRepRig);
                            xrRepRig.DataSource = ds;
                            xtrPat.xtrResu_Rig.ReportSource = xrRepRig;
                            #endregion

                            #region 读取其他列结果报表
                            xtrRepResulto xrRepOther = new xtrRepResulto();
                            DataTable dtRepOther = dtResLeft.Clone();
                            if (!string.IsNullOrEmpty(otherAddress)) //没有镜检的不用获取
                            {
                                xrRepOther.LoadLayout(otherAddress);
                                DataSet dsOther = new DataSet();
                                foreach (DataRow drRep in dsRepSour.Tables[0].Rows)
                                {
                                    if (drRep["镜检标志"].ToString() == "1")
                                        dtRepOther.Rows.Add(drRep.ItemArray);
                                }
                                dsOther.Tables.Add(dtRepOther);
                                xrRepOther.DataSource = dsOther;
                                xtrPat.xtrResu_Other.ReportSource = xrRepOther;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 读取结果报表
                            DataTable dtXtRes = dtSql.Clone();
                            string resSql = EncryptClass.Decrypt(drRes[0]["repSql"].ToString());
                            resSql = resSql.Replace("&where&", " and res_id='" + drPat["标识ID"] + "'");
                            dtXtRes.Rows.Add(resSql, drRes[0]["repConnCode"].ToString());
                            DataSet dsRepSour = base.doOther(dtXtRes);


                            xtrRepResulto xrRepRig = new xtrRepResulto();
                            DataTable dtRepRig = dsRepSour.Tables[0];
                            if (resAddress != null)
                                xrRepRig.LoadLayout(resAddress);
                            xrRepRig.DataSource = dsRepSour;
                            xtrPat.xtrResu_Rig.ReportSource = xrRepRig;
                            #endregion
                        }

                        #region  处理图片数据源以及报表
                        xtrRepResulto_p xrPic = new xtrRepResulto_p();
                        DataTable dtXtPic = dtSql.Clone();
                        if (picAddress != null)
                            xrPic.LoadLayout(picAddress);
                        string picSql = EncryptClass.Decrypt(drPic[0]["repSql"].ToString());
                        picSql = picSql.Replace("&where&", " and pres_id='" + drPat["标识ID"] + "'");
                        dtXtPic.Rows.Add(picSql, drPic[0]["repConnCode"].ToString());
                        DataSet dsPic = base.doOther(dtXtPic);
                        DataTable dtPic = dsPic.Tables["可设计字段"];
                        xrPic.DataSource = dsPic;
                        #endregion

                        xtrPat.xtrRepResp.ReportSource = xrPic;
                        if (isTrue)
                        {
                            if (!isNotAllowCreateDoc)
                                xtrPat.CreateDocument();
                            xr.Pages.AddRange(xtrPat.Pages);
                        }
                        else
                            xr = xtrPat;
                    }
                }
            }

            return xr;
        }

        /// <summary>
        /// nest嵌套报表
        /// </summary>
        /// <param name="hsSr"></param>
        /// <param name="isTrue"></param>
        /// <returns></returns>
        public XtraReport getNestReport(string reportName, Hashtable hsSr, bool isTrue) // 2010-6-30
        {
            DataTable dtRepCode = CommonClient.CreateDT(new string[] { "repCode" }, "report");
            dtRepCode.Rows.Add(string.Format("where repCode like '{0}%'", reportName));
            DataTable dtSr = base.doView(dtRepCode).Tables["SubReport"];//获得子报表数据源
            XtraReport mainRep = new XtraReport();

            DataRow[] drsMain = dtSr.Select(string.Format("repCode = '{0}'", reportName));
            if (drsMain.Length > 0)
            {
                string main_rep_addr = localPath + drsMain[0]["repAddress"].ToString().Split('.')[0] + printTypeName + "." + drsMain[0]["repAddress"].ToString().Split('.')[1];
                mainRep.LoadLayout(main_rep_addr);
                string sql = EncryptClass.Decrypt(drsMain[0]["repSql"].ToString());
                foreach (DictionaryEntry de in hsSr)
                {
                    sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
                }
                DataTable dtSql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                dtSql.Rows.Add(sql, drsMain[0]["repConnCode"].ToString());
                DataSet datasource = base.doOther(dtSql);//.Tables["可设计字段"];
                mainRep.DataSource = datasource;

                DataRow[] drPats = dtSr.Select(string.Format("repCode like '{0}_%'", reportName));

                bool exceedCount = false;

                foreach (DataRow row in drPats)
                {
                    string rep_code = row["repCode"].ToString();
                    string rep_addr = localPath + row["repAddress"].ToString().Split('.')[0] + printTypeName + "." + row["repAddress"].ToString().Split('.')[1];

                    XRSubreport subReport = mainRep.FindControl(rep_code, true) as XRSubreport;

                    if (File.Exists(rep_addr) && subReport != null)
                    {
                        string subsql = EncryptClass.Decrypt(row["repSql"].ToString());
                        foreach (DictionaryEntry de in hsSr)
                        {
                            subsql = subsql.Replace(de.Key.ToString(), de.Value.ToString());
                        }
                        DataTable sub_query_sql = CommonClient.CreateDT(new string[] { "sql", "conn_code" }, "Data");
                        sub_query_sql.Rows.Add(subsql, row["repConnCode"].ToString());
                        DataSet sub_datasource = base.doOther(sub_query_sql);
                        if (sub_datasource != null && sub_datasource.Tables.Count > 0 && sub_datasource.Tables[0].Rows.Count > 24)
                            exceedCount = true;

                        XtraReport rep = new XtraReport();
                        rep.LoadLayout(rep_addr);

                        #region this:传入打印者与打印时间

                        if (!string.IsNullOrEmpty(thisOutputPrintPerson)
                            && !string.IsNullOrEmpty(thisOutputPrintTime))
                        {
                            if (sub_datasource != null)
                            {
                                DataSet dstempAddCol = sub_datasource;

                                if (dstempAddCol != null && dstempAddCol.Tables.Count > 0
                                    && dstempAddCol.Tables[0] != null
                                    && dstempAddCol.Tables[0].Rows.Count > 0
                                    && dstempAddCol.Tables[0].Columns.Contains("传入打印者")
                                    && dstempAddCol.Tables[0].Columns.Contains("传入打印时间"))
                                {
                                    foreach (DataRow drtempAddCol in dstempAddCol.Tables[0].Rows)
                                    {
                                        drtempAddCol["传入打印者"] = thisOutputPrintPerson;
                                        drtempAddCol["传入打印时间"] = thisOutputPrintTime;
                                    }
                                }
                            }
                        }

                        #endregion

                        rep.DataSource = sub_datasource;

                        subReport.ReportSource = rep;
                    }
                }
                if (reportName == "NestReport_XS800" && exceedCount)
                    mainRep.PaperKind = PaperKind.A4;
            }
            if (!isNotAllowCreateDoc)
                mainRep.CreateDocument();
            return mainRep;
        }
        #endregion

        private string setPrintParameter(XtraReport xr)
        {
            xr.PrintingSystem.ShowMarginsWarning = false;
            xr.PrintingSystem.ContinuousPageNumbering = false;
            xr.PrintingSystem.ShowPrintStatusDialog = false;
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        DataRow drPrintConfig = dt.Rows[0];
                        if (dt.Columns.Count > 2)
                            xr.PrintingSystem.PageSettings.PrinterName = drPrintConfig["printName"].ToString();
                        if (drPrintConfig["printType"].ToString() == "0")
                        {
                            return "";
                        }
                        else if (drPrintConfig["printType"].ToString() == "1")
                        {
                            return "_套打";
                        }
                        else if (drPrintConfig["printType"].ToString() == "2")
                        {
                            return "_自助";
                        }

                        if (!ShowPrint)
                            return "";

                        return "_套打";
                    }
                }
            }
            return string.Empty;
        }

        private string GetPrintName()
        {
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        DataRow drPrintConfig = dt.Rows[0];
                        if (dt.Columns.Count > 2
                            && dt.Columns.Contains("printName") && drPrintConfig["printName"] != null &&
                            !string.IsNullOrEmpty(drPrintConfig["printName"].ToString()))
                            return drPrintConfig["printName"].ToString();
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取横向或纵向打印
        /// </summary>
        /// <returns></returns>
        private bool GetpageSettingsLandscape()
        {
            if (File.Exists(xmlFile))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];

                        if (!dt.Columns.Contains("ContinuousPrinting"))
                        {
                            dt.Columns.Add("ContinuousPrinting");
                        }

                        if (dt.Columns.Count > 2 && dr != null && dr.Table.Columns.Contains("landscape"))
                        {
                            if (dr["landscape"].ToString() == "True")
                                return true;
                            else
                                return false;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">文件夹的路径</param>
        private void addFile(string path)
        {
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
        }

        public class PrintEventArgs : EventArgs
        {
            public PrintEventArgs(Hashtable p)
            {
                param = p;
                listPrintData = new List<EntityPrintData>(); ;
            }
            public Hashtable param { get; set; }


            public List<EntityPrintData> listPrintData { get; set; }
        }

    }

    public class ReportNotFoundException : Exception
    {
        public string MSG { get; set; }
    }

    public class ReportParameter
    {
        public ReportParameter(Hashtable _htRep, bool _isCreate, string _path, string _sql, string _reportType, string conn_code)
        {
            subReportParameter = _htRep;
            isCreate = _isCreate;
            path = _path;
            sql = _sql;
            reportType = _reportType;
            ConnectionCode = conn_code;
        }

        public string ConnectionCode { get; set; }

        private Hashtable subReportParameter = null;

        public Hashtable SubReportParameter
        {
            get { return subReportParameter; }
            set { subReportParameter = value; }
        }

        private bool isCreate = true;

        public bool IsCreate
        {
            get { return isCreate; }
            set { isCreate = value; }
        }

        private string path = string.Empty;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string sql = string.Empty;

        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }

        private string reportType = string.Empty;

        public string ReportType
        {
            get { return reportType; }
            set { reportType = value; }
        }

    }
}




