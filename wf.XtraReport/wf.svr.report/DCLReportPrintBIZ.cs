using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace dcl.svr.report
{
    public class DCLReportPrintBIZ : IReportPrint
    {
        /// <summary>
        /// 获取报表数据源
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public EntityDCLPrintData GetReportSource(EntityDCLPrintParameter parameter)
        {
            EntityDCLPrintData printData = new EntityDCLPrintData();

            IDaoReportPrint dao = DclDaoFactory.DaoHandler<IDaoReportPrint>();
            if (dao != null)
            {
                printData = dao.GetReportData(parameter);
            }

            return printData;
        }

        /// <summary>
        /// 细菌报表特殊处理
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public EntityDCLPrintData GetBacReportSource(EntityDCLPrintParameter parameter)
        {
            EntityDCLPrintData printData = new EntityDCLPrintData();
            IDaoReportPrint dao = DclDaoFactory.DaoHandler<IDaoReportPrint>();
            if (dao != null)
            {
                parameter.ReportCode = dao.GetBacReportCode(parameter.RepId);
                printData = dao.GetReportData(parameter);
            }
            return printData;
        }

        /// <summary>
        /// 多图像报表 Picture_pat特殊前缀报表
        /// </summary>
        public List<EntityDCLPrintData> GetPictureReportSource(EntityDCLPrintParameter parameter)
        {
            List<EntityDCLPrintData> listPrintData = new List<EntityDCLPrintData>();

            IDaoReportPrint dao = DclDaoFactory.DaoHandler<IDaoReportPrint>();
            if (dao != null)
            {
                List<string> listReportCode = new List<string>();
                string reportSuffix = parameter.ReportCode.Replace("Picture_pat", "");
                listReportCode.Add(parameter.ReportCode);
                listReportCode.Add("Picture_res" + reportSuffix);
                listReportCode.Add("Picture_other" + reportSuffix);
                listReportCode.Add("Picture_pic" + reportSuffix);

                foreach (string reportCode in listReportCode)
                {
                    EntityDCLPrintParameter subParameter = EntityManager<EntityDCLPrintParameter>.EntityClone(parameter);
                    subParameter.ReportCode = reportCode;
                    EntityDCLPrintData printData = dao.GetReportData(subParameter);
                    if (!string.IsNullOrEmpty(printData.ReportCode))
                    {
                        listPrintData.Add(printData);
                    }
                }
            }

            return listPrintData;
        }

        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private XtraReport CreateReport(EntityDCLPrintParameter parameter)
        {
            XtraReport xr = null;
            EntityDCLPrintData printData = new EntityDCLPrintData();
            if (parameter.ReportCode.StartsWith("bacilli"))
                printData = GetBacReportSource(parameter);
            else if (parameter.ReportCode.StartsWith("NestReport"))
            {
                return CreateNestReport(parameter);
            }
            //else if (parameter.ReportCode.StartsWith("Picture_pat"))
            //{
            //    return CreatePictureReport(parameter);
            //}
            else
            {
                printData = GetReportSource(parameter);
            }

            string reportPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "xtraReport\\" + printData.ReportName + printData.ReportSuffix;
            if (File.Exists(reportPath))
            {
                try
                {
                    xr = new XtraReport();
                    xr.LoadLayout(reportPath);
                    xr.DataSource = printData.ReportData;
                    xr.CreateDocument(false);
                }
                catch (Exception ex)
                {
                    xr = null;
                    Lib.LogManager.Logger.LogException("报表读取错误！", ex);
                }
            }
            return xr;
        }

        private  XtraReport CreateNestReport(EntityDCLPrintParameter parameter)
        {
            XtraReport mainReport = new XtraReport();

            EntityResponse response = new ReporMainBIZ().GetReport(new EntityRequest());
            List<EntitySysReport> listReport = response.GetResult() as List<EntitySysReport>;

            //拿出Next所有的报表
            List<EntitySysReport> listNextReport = listReport.FindAll(w => w.RepCode.Contains(parameter.ReportCode) && w.RepCode != parameter.ReportCode);

            //主报表数据
            EntityDCLPrintData printData = GetReportSource(parameter);

            string reportPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "xtraReport\\" + printData.ReportName + printData.ReportSuffix;
            //Lib.LogManager.Logger.LogInfo(reportPath);
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
                    EntityDCLPrintData subPrintData = GetReportSource(parameter);

                    string subReportPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "xtraReport\\" + subPrintData.ReportName + printData.ReportSuffix;

                    XtraReport subReportSour = new XtraReport();
                    subReportSour.LoadLayout(subReportPath);
                    subReportSour.DataSource = subPrintData.ReportData;

                    subReport.ReportSource = subReportSour;
                }
            }

            mainReport.CreateDocument(false);
            mainReport.PrintingSystem.ShowMarginsWarning = false;
            return mainReport;
        }


        /// <summary>
        /// A4连续打印
        /// </summary>
        /// <param name="lisParameter"></param>
        /// <param name="printName"></param>
        private XtraReport ContinuousPrint(List<EntityDCLPrintParameter> lisParameter)
        {
            List<EntityDCLPrintParameter> listPrintParameter = lisParameter.OrderBy(o => o.Sequence).ToList();
            Dictionary<string, List<EntityDCLPrintParameter>> dict = new Dictionary<string, List<EntityDCLPrintParameter>>();
            string reportPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "xtraReport\\ContinuousPrinting.repx";

            if (!File.Exists(reportPath))
                return null;

            XtraReport mainReport = new XtraReport();
            mainReport.PaperKind = PaperKind.A4;
            mainReport.ReportUnit = ReportUnit.HundredthsOfAnInch;
            mainReport.PrintingSystem.ShowMarginsWarning = false;


            for (int i = 0; i < lisParameter.Count; i++)
            {
                if (!dict.ContainsKey(lisParameter[i].PatName))
                {

                    dict.Add(lisParameter[i].PatName, new List<EntityDCLPrintParameter>());
                }
                dict[lisParameter[i].PatName].Add(lisParameter[i]);
            }


            foreach (string item in dict.Keys)
            {
                List<EntityDCLPrintParameter> listPrintData = dict[item];
                XtraReport xr = new XtraReport();
                //合并同一分组报告单
                foreach (EntityDCLPrintParameter par in listPrintData)
                {
                    XtraReport xtrRep = CreateReport(par);
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

                            source.Rows.Add(new object[] { stream.ToArray(), item, (i + 1), xr.Pages.Count });
                        }
                    }
                    if (source.Rows.Count > 0)
                    {
                        XtraReport imgReport = new XtraReport();

                        imgReport.LoadLayout(reportPath);
                        imgReport.DataSource = source;
                        imgReport.CreateDocument();

                        mainReport.Pages.AddRange(imgReport.Pages);
                    }
                }
            }


            return mainReport;
        }


        /// <summary>
        /// 获取PDF报告
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public String GetReportPDF(EntityDCLPrintParameter parameter)
        {
            string strPDF = string.Empty;

            XtraReport xr = CreateReport(parameter);
            if (xr != null)
            {
                MemoryStream ms = new MemoryStream();
                xr.ExportToPdf(ms);
                if (ms != null)
                {
                    byte[] by = ms.ToArray();
                    strPDF = Convert.ToBase64String(by);
                }
                ms.Close();
            }

            return strPDF;
        }


        /// <summary>
        /// 获取PDF报告
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public String GetA4ContinuousReportPDF(List<EntityDCLPrintParameter> lisParameter)
        {
            string strPDF = string.Empty;

            XtraReport xr = ContinuousPrint(lisParameter);
            if (xr != null)
            {
                MemoryStream ms = new MemoryStream();
                xr.ExportToPdf(ms);
                if (ms != null)
                {
                    byte[] by = ms.ToArray();
                    strPDF = Convert.ToBase64String(by);
                }
                ms.Close();
            }

            return strPDF;
        }
    }
}
