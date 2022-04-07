using dcl.client.common;
using dcl.client.wcf;
using dcl.common;
using dcl.entity;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace dcl.client.report
{
    /// <summary>
    /// 打印报告类
    /// </summary>
    public class DCLTouchReportPrint
    {
        public static string ReportPath { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        private static String PrinterName { get; set; }

        /// <summary>
        /// 报表自定义后缀
        /// </summary>
        private static String CustomReportSuffix { get; set; }

        /// <summary>
        /// 条码打印
        /// </summary>
        /// <param name="reportCode"></param>
        /// <param name="listBarCode"></param>
        public static void PrintBarCode(String reportCode, List<String> listSampSn, string printName = "")
        {
            if (!string.IsNullOrEmpty(reportCode) && listSampSn.Count > 0)
            {
                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                printPara.ReportCode = reportCode;
                printPara.listSampSn = listSampSn;
                XtraReport xr = CreateReport(printPara, printName);
                xr.Print();
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="parameter"></param>
        public static void Print(EntityDCLPrintParameter parameter, String printName = "")
        {
            XtraReport xr = CreateReport(parameter, printName);
            if (xr != null)
                xr.Print();
        }


        /// <summary>
        /// 根据打印（直接传入数据打印）
        /// </summary>
        /// <param name="parameter"></param>
        public static void PrintByData(EntityDCLPrintData printData, String printName = "")
        {
            if (!string.IsNullOrEmpty(printData.ReportName))
            {
                XtraReport xr = CreateReport(printData, printName);
                if (xr != null)
                {
                    xr.Print();
                }
            }
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// <param name="parameter"></param>
        public static void BatchPrint(List<EntityDCLPrintParameter> lisParameter, String printName = "")
        {
            List<EntityDCLPrintParameter> listPrintParameter = lisParameter.OrderBy(o => o.Sequence).ToList();

            XtraReport mainReport = new XtraReport();
            mainReport.PrintingSystem.ShowMarginsWarning = false;
            foreach (EntityDCLPrintParameter item in lisParameter)
            {
                XtraReport xr = CreateReport(item, printName);
                if (xr != null)
                {
                    mainReport.Pages.AddRange(xr.Pages);
                    mainReport.PaperKind = xr.PaperKind;
                    mainReport.PrintingSystem.PageSettings.PrinterName = xr.PrintingSystem.PageSettings.PrinterName;
                    SetXtraReportWaterMark(mainReport, xr);
                }
            }

            if (mainReport.Pages.Count > 0)
            {
           
                mainReport.PrintingSystem.ShowMarginsWarning = false;
                mainReport.Print();
            }
        }

        /// <summary>
        /// A4连续打印
        /// </summary>
        /// <param name="lisParameter"></param>
        /// <param name="printName"></param>
        public static void ContinuousPrint(List<EntityDCLPrintParameter> lisParameter, String printName = "")
        {
            List<EntityDCLPrintParameter> listPrintParameter = lisParameter.OrderBy(o => o.Sequence).ToList();
            Dictionary<string, List<EntityDCLPrintParameter>> dict = new Dictionary<string, List<EntityDCLPrintParameter>>();
            string reportPath = ReportPath + "ContinuousPrinting.repx";
            XtraReport mainReport = new XtraReport();
            mainReport.PaperKind = PaperKind.A4;
            mainReport.ReportUnit = ReportUnit.HundredthsOfAnInch;
            mainReport.PrintingSystem.ShowMarginsWarning = false;
            bool IsBatchPrintByPatDepName = ConfigHelper.GetSysConfigValueWithoutLogin("RepSel_batchPrintByPatDepName") == "是";
            //系统配置:批量套打按病历号分批打印


            bool IsbatchPrintByPatInNo = ConfigHelper.GetSysConfigValueWithoutLogin("RepSel_batchPrintByPatInNo") == "是";
            //是否按照科室分组
            if (IsBatchPrintByPatDepName)
            {
                for (int i = 0; i < lisParameter.Count; i++)
                {
                    if (!dict.ContainsKey(lisParameter[i].PatName) && !dict.ContainsKey(lisParameter[i].PatDepName))
                    {
                        dict.Add(lisParameter[i].PatDepName, new List<EntityDCLPrintParameter>());
                    }
                    dict[lisParameter[i].PatDepName].Add(lisParameter[i]);
                }

            }
            else
            {
                for (int i = 0; i < lisParameter.Count; i++)
                {
                    if (!dict.ContainsKey(lisParameter[i].PatName))
                    {
                        dict.Add(lisParameter[i].PatName, new List<EntityDCLPrintParameter>());
                    }
                    dict[lisParameter[i].PatName].Add(lisParameter[i]);
                }

            }
            foreach (string item in dict.Keys)
            {
                List<EntityDCLPrintParameter> listPrintData = dict[item];
                XtraReport xr = new XtraReport();
                //合并同一分组报告单
                foreach (EntityDCLPrintParameter par in listPrintData)
                {
                    XtraReport xtrRep = CreateReport(par, printName);
                    xr.Pages.AddRange(xtrRep.Pages);
                }
                if (xr != null)
                {
                    mainReport.PrintingSystem.PageSettings.PrinterName = xr.PrintingSystem.PageSettings.PrinterName;
                    DataTable source = new DataTable();
                    source.Columns.Add("img", typeof(byte[]));
                    source.Columns.Add("name", typeof(string));
                    source.Columns.Add("seq", typeof(int));
                    source.Columns.Add("total", typeof(int));
                    int temp = 0;
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
                            options.Resolution = 96;
                            options.Format = ImageFormat.Png;
                            options.PageRange = (i + 1).ToString();
                            MemoryStream stream = new MemoryStream();
                            xr.ExportToImage(stream, options);
                            if (IsBatchPrintByPatDepName)
                            {
                                source.Rows.Add(new object[] { stream.ToArray(), item, (temp + 1), xr.Pages.Count });
                                temp++;
                            }
                            else
                            {
                                source.Rows.Add(new object[] { stream.ToArray(), item, (i + 1), xr.Pages.Count });
                            }

                        }
                    }
                    if (source.Rows.Count > 0)
                    {
                        XtraReport imgReport = new XtraReport();
                        if (IsbatchPrintByPatInNo)
                        {
                            //用于清空上一次打印过的报表
                            mainReport.Pages.Clear();
                        }
                        imgReport.LoadLayout(reportPath);
                        imgReport.DataSource = source;
                        imgReport.CreateDocument();

                        //报告大小越界不提示
                        imgReport.PrintingSystem.ShowMarginsWarning = false;

                        mainReport.Pages.AddRange(imgReport.Pages);

                    }
                }
            }
            if (mainReport.Pages.Count > 0)
                mainReport.Print();

        }


        /// <summary>
        /// 批量打印预览
        /// </summary>
        /// <param name="lisParameter"></param>
        /// <param name="printName"></param>
        public static void BatchPrintPreview(List<EntityDCLPrintParameter> lisParameter, String printName = "")
        {
            List<EntityDCLPrintParameter> listPrintParameter = lisParameter.OrderBy(o => o.Sequence).ToList();
            XtraReport mainReport = new XtraReport();
            mainReport.PrintingSystem.ShowMarginsWarning = false;
            foreach (EntityDCLPrintParameter item in lisParameter)
            {
                XtraReport xr = CreateReport(item, printName);
                if (xr != null)
                {
                    mainReport.Pages.AddRange(xr.Pages);
                    mainReport.PrintingSystem.PageSettings.PrinterName = xr.PrintingSystem.PageSettings.PrinterName;
                    SetXtraReportWaterMark(mainReport, xr);
                }
            }
           
            if (mainReport.Pages.Count > 0)
                mainReport.ShowPreviewDialog();
        }


        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="parameter"></param>
        public static void PrintPreview(EntityDCLPrintParameter parameter)
        {
            XtraReport xr = CreateReport(parameter);
            if (xr != null)
                xr.ShowPreviewDialog();
        }

        /// <summary>
        /// 预览（直接传入数据预览）
        /// </summary>
        /// <param name="printData"></param>
        public static void PrintPreviewByData(EntityDCLPrintData printData)
        {
            if (!string.IsNullOrEmpty(printData.ReportName))
            {
                XtraReport xr = CreateReport(printData);
                if (xr != null)
                {
                    xr.ShowPreviewDialog();
                }
            }
        }

        /// <summary>
        /// 生成报表对象
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static XtraReport CreateReport(EntityDCLPrintParameter parameter, String printName = "")
        {
            if (parameter.ReportCode.StartsWith("Picture_pat"))
            {
                return CreatePictureReport(parameter, printName);
            }
            else if (parameter.ReportCode.StartsWith("bacilli"))
            {
                ProxyReportPrint proxyPrint = new ProxyReportPrint();
                EntityDCLPrintData printData = proxyPrint.Service.GetBacReportSource(parameter);
                return CreateReport(printData, printName);
            }
            else if (parameter.ReportCode.StartsWith("NestReport"))
            {
                return CreateNestReport(parameter, printName);
            }
            else
            {
                ProxyReportPrint proxyPrint = new ProxyReportPrint();
                EntityDCLPrintData printData = proxyPrint.Service.GetReportSource(parameter);
                return CreateReport(printData, printName);
            }
        }

        /// <summary>
        /// 获取用EntityDCLPrintData生成的报表对象
        /// </summary>
        /// <param name="printData"></param>
        /// <param name="printName"></param>
        /// <returns></returns>
        public static XtraReport GetXtraReportByPrintData(EntityDCLPrintData printData, String printName = "")
        {
            return CreateReport(printData, printName);
        }

        /// <summary>
        /// 获取用EntityDCLPrintParameter生成的报表对象
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="printName"></param>
        /// <returns></returns>
        public static XtraReport GetXtraReportByPrintData(EntityDCLPrintParameter parameter, String printName = "")
        {
            return CreateReport(parameter, printName);
        }

        private static XtraReport CreateReport(EntityDCLPrintData printData, String printName = "")
        {
            XtraReport xr = null;
            if (!string.IsNullOrEmpty(printData.ReportName))
            {
                if (printName == string.Empty)
                    GetPrintSetting();
                else
                    PrinterName = printName;
                string reportPath = ReportPath + printData.ReportName + CustomReportSuffix + printData.ReportSuffix;
                if (File.Exists(reportPath))
                {
                    try
                    {
                        xr = new XtraReport();
                        xr.LoadLayout(reportPath);
                        xr.DataSource = printData.ReportData;
                        if (PrinterName != string.Empty)
                            xr.PrinterName = PrinterName;
                        xr.CreateDocument(false);
                        xr.PrintingSystem.ShowMarginsWarning = false;

                    }
                    catch (Exception ex)
                    {
                        xr = null;
                        Lib.LogManager.Logger.LogException("报表读取错误！", ex);
                        lis.client.control.MessageDialog.Show("报表读取错误！", "提示");
                    }
                }
                else
                    lis.client.control.MessageDialog.Show("该报表不存在！", "提示");
            }
            else
                lis.client.control.MessageDialog.Show("该报表不存在！", "提示");
            return xr;
        }

        private static XtraReport CreatePictureReport(EntityDCLPrintParameter parameter, String printName)
        {
            XtraReport xr = null;
            ProxyReportPrint proxyPrint = new ProxyReportPrint();
            List<EntityDCLPrintData> listPrintData = proxyPrint.Service.GetPictureReportSource(parameter);
            int i = listPrintData.FindIndex(r => r.ReportCode == parameter.ReportCode);
            if (i > -1)
            {
                xtrRepPatients xtrPat = new xtrRepPatients();
                string reportPath = ReportPath + listPrintData[i].ReportName + CustomReportSuffix + listPrintData[i].ReportSuffix;
                xtrPat.LoadLayout(reportPath);
                SetHospitalName.setName(xtrPat.Bands);//设置医院名称
                xtrPat.DataSource = listPrintData[i].ReportData;//赋予报表数据源

                int resIndex = listPrintData.FindIndex(r => r.ReportCode.Contains("Picture_res"));
                if (resIndex > -1)
                {
                    xtrRepResulto xrRepRig = new xtrRepResulto();
                    string resAddress = ReportPath + listPrintData[resIndex].ReportName + CustomReportSuffix + listPrintData[resIndex].ReportSuffix;
                    xrRepRig.LoadLayout(resAddress);
                    xrRepRig.DataSource = listPrintData[resIndex].ReportData;
                    xtrPat.xtrResu_Rig.ReportSource = xrRepRig;
                }

                int picIndex = listPrintData.FindIndex(r => r.ReportCode.Contains("Picture_pic"));
                if (picIndex > -1)
                {
                    xtrRepResulto_p xrPic = new xtrRepResulto_p();
                    string resAddress = ReportPath + listPrintData[picIndex].ReportName + CustomReportSuffix + listPrintData[picIndex].ReportSuffix;
                    xrPic.LoadLayout(resAddress);
                    xrPic.DataSource = listPrintData[picIndex].ReportData;
                    xtrPat.xtrRepResp.ReportSource = xrPic;
                }

                xr = new XtraReport();
                if (PrinterName != string.Empty)
                    xr.PrinterName = PrinterName;
                xtrPat.CreateDocument(false);
                xr.PrintingSystem.ShowMarginsWarning = false;
                xr.Pages.AddRange(xtrPat.Pages);
            }

            return xr;
        }


        private static XtraReport CreateNestReport(EntityDCLPrintParameter parameter, String printName)
        {
            XtraReport mainReport = new XtraReport();

            EntityResponse response = new ProxyReportMain().Service.GetReport(new EntityRequest());
            List<EntitySysReport> listReport = response.GetResult() as List<EntitySysReport>;

            //拿出Next所有的报表
            List<EntitySysReport> listNextReport = listReport.FindAll(w => w.RepCode.Contains(parameter.ReportCode) && w.RepCode != parameter.ReportCode);

            ProxyReportPrint proxyPrint = new ProxyReportPrint();

            //主报表数据
            EntityDCLPrintData printData = proxyPrint.Service.GetReportSource(parameter);

            string reportPath = ReportPath + printData.ReportName + CustomReportSuffix + printData.ReportSuffix;
            mainReport.LoadLayout(reportPath);
            mainReport.DataSource = printData.ReportData;

            foreach (EntitySysReport report in listNextReport)
            {
                XRControl subControl = mainReport.FindControl(report.RepCode, true);
                if (subControl != null)
                {
                    XRSubreport subReport = subControl as XRSubreport;

                    //子报告数据
                    parameter.ReportCode = report.RepCode;
                    EntityDCLPrintData subPrintData = proxyPrint.Service.GetReportSource(parameter);

                    string subReportPath = ReportPath + subPrintData.ReportName + CustomReportSuffix + subPrintData.ReportSuffix;

                    XtraReport subReportSour = new XtraReport();
                    subReportSour.LoadLayout(subReportPath);
                    subReportSour.DataSource = subPrintData.ReportData;

                    subReport.ReportSource = subReportSour;
                }
            }


            if (PrinterName != string.Empty)
                mainReport.PrinterName = PrinterName;
            mainReport.CreateDocument(false);
            mainReport.PrintingSystem.ShowMarginsWarning = false;

            return mainReport;
        }

        /// <summary>
        /// 获取本地打印设置
        /// </summary>
        private static void GetPrintSetting()
        {
            PrinterName = string.Empty;
            CustomReportSuffix = string.Empty;

            PrinterName = ReportSetting.PrintName;

            if (ReportSetting.PrintType == "1")
            {
                CustomReportSuffix = "_套打";
            }
            else if (ReportSetting.PrintType == "2")
            {
                CustomReportSuffix = "_自助";
            }
        }


        /// <summary>
        /// 设置水印
        /// </summary>
        /// <param name="mainReport"></param>
        /// <param name="xr"></param>
        private static void SetXtraReportWaterMark(XtraReport mainReport, XtraReport xr)
        {
            if (xr.Watermark != null && !string.IsNullOrEmpty(xr.Watermark.Text))
            {
                PageWatermark page = new PageWatermark();
                page.Text = xr.Watermark.Text;
                page.TextDirection = xr.Watermark.TextDirection;
                page.TextTransparency = xr.Watermark.TextTransparency;
                page.Font = xr.Watermark.Font;
                page.ForeColor = xr.Watermark.ForeColor;
                page.Image = xr.Watermark.Image;
                for (int i = 0; i < mainReport.Pages.Count; i++)
                {
                    mainReport.Pages[i].AssignWatermark(page);
                }

            }
        }



    }
}
