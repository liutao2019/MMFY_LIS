using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace dcl.svr.touchprint
{
    public class TouchPrintBIZ : ITouchPrint
    {
        /// <summary>
        /// 查询病人打印信息
        /// </summary>
        /// <param name="pidInNo">病历号</param>
        /// <param name="pidSrcId">来源</param>
        public List<EntityTouchPrintData> PatientPrintQuery(string pidInNo, string pidSrcId, string PrintType = "0")
        {
            List<EntityTouchPrintData> listPrintData = new List<EntityTouchPrintData>();

            IDaoTouchPrint printDao = DclDaoFactory.DaoHandler<IDaoTouchPrint>();
            if (printDao != null)
            {
                //未传来源，默认为门诊
                if (string.IsNullOrEmpty(pidSrcId))
                    pidSrcId = "107";

                List<EntityPidReportMain> listPat = printDao.PatientPrintQuery(pidInNo, pidSrcId);
                listPat = listPat.OrderBy(o => o.RepStatus).ToList();

                DCLReportPrintBIZ printBiz = new DCLReportPrintBIZ();

                foreach (EntityPidReportMain pat in listPat)
                {
                    EntityTouchPrintData printData = new EntityTouchPrintData();
                    printData.RepId = pat.RepId;
                    printData.PidComName = pat.PidComName;
                    printData.PidName = pat.PidName;
                    printData.RepReportDate = pat.RepReportDate != null ? pat.RepReportDate.Value.ToString("MM-dd HH:mm") : "";
                    printData.RepSamName = pat.SamName;
                    printData.RepBarCode = pat.RepBarCode;
                    printData.RepItrId = pat.RepItrId;

                    if (pat.RepStatus != null)
                    {
                        if (pat.RepStatus.Value == 4)
                            printData.RepStatus = "已打印";
                        else if (pat.RepStatus.Value == 2)
                        {
                            printData.RepStatus = "打印中";

                            EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                            printParameter.RepId = pat.RepId;
                            printParameter.ReportCode = pat.ItrReportId;
                            if(PrintType == "0")
                                printData.RepPDF = printBiz.GetReportPDF(printParameter); //CreatePDF(pat.RepId);
                        }
                        else
                            printData.RepStatus = "未出结果";
                    }
                    printData.SampCollectionDate = pat.SampCollectionDate != null ? pat.SampCollectionDate.Value.ToString("MM-dd HH:mm") : "";

                    listPrintData.Add(printData);
                }

                List<EntitySampMain> listSampMain = printDao.SampMainPrintQuery(pidInNo, pidSrcId);

                foreach (EntitySampMain sampMain in listSampMain)
                {
                    EntityTouchPrintData printData = new EntityTouchPrintData();
                    printData.PidComName = sampMain.SampComName;
                    printData.PidName = sampMain.PidName;
                    printData.RepSamName = sampMain.SamName;
                    printData.RepBarCode = sampMain.SampBarCode;

                    //if (sampMain.SampStatusId == "9")
                    //    printData.RepStatus = "标本回退";
                    //else
                    //    printData.RepStatus = "送检中";
                    //printData.SampCollectionDate = sampMain.CollectionDate != null ?
                    //                               sampMain.CollectionDate.Value.ToString("MM-dd HH:mm") :
                    //                               sampMain.SampLastactionDate.ToString("MM-dd HH:mm");

                    ///0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,
                    ///7-已检验,8-二次送检
                    if (sampMain.SampStatusId == "0")
                        printData.RepStatus = "未采样";
                    if (sampMain.SampStatusId == "1")
                        printData.RepStatus = "未采样";
                    if (sampMain.SampStatusId == "2")
                        printData.RepStatus = "未采样";
                    if (sampMain.SampStatusId == "3")
                        printData.RepStatus = "已采样";
                    if (sampMain.SampStatusId == "4")
                        printData.RepStatus = "已送检";
                    if (sampMain.SampStatusId == "5")
                        printData.RepStatus = "已接收";
                    if (sampMain.SampStatusId == "6")
                        printData.RepStatus = "检验中";
                    if (sampMain.SampStatusId == "7")
                        printData.RepStatus = "已检验";
                    if (sampMain.SampStatusId == "8")
                        printData.RepStatus = "二次送检";
                    if (sampMain.SampStatusId == "9")
                        printData.RepStatus = "标本回退";
                    else
                        printData.RepStatus = "*****";
                    printData.SampCollectionDate = sampMain.CollectionDate != null ?
                                                   sampMain.CollectionDate.Value.ToString("MM-dd HH:mm") :
                                                   sampMain.SampLastactionDate.ToString("MM-dd HH:mm");

                    listPrintData.Add(printData);
                }
            }

            return listPrintData;
        }

        /// <summary>
        /// 生成PDF报告
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        private string CreatePDF(string RepId)
        {
            Lib.LogManager.Logger.LogInfo("pat_id:" + RepId);
            //测试，先固定ID，正式发布后去除
            //RepId = "1023120180412691";
            string pdf = string.Empty;
            try
            {
                string strXml = string.Format(@"<?xml version=""1.0"" encoding =""utf-8"" ?>
                                                <DocumentElement>
                                                <AccessKey>ED6EB64FCF30D0718F70B0B07191939F60A063</AccessKey>
                                                <MethodName>lis_downloadpdfresult</MethodName>
                                                <DataTable>
                                                   <data_org_id>1001</data_org_id>
                                                   <data_sys_id>1002</data_sys_id>
                                                   <data_sys_id_value>{0}</data_sys_id_value>
                                                   <rep_no>{0}</rep_no>
                                                </DataTable>
                                                </DocumentElement>", RepId);

                Lib.LogManager.Logger.LogInfo("strXml:" + strXml);
                string strResponse = string.Empty;//LisCDRInterfaceClient.CDRServiceHelper.GetProxy().CreatePostHttpResponse(strXml);
                Lib.LogManager.Logger.LogInfo("strResponse:" + strResponse);

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strResponse);

                XmlNode pdfNode = xmldoc.SelectSingleNode("DocumentElement/DataTable/pdf_blob");
                if (pdfNode != null &&
                   !string.IsNullOrEmpty(pdfNode.InnerText))
                {
                    pdf = pdfNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return pdf;
        }


    }
}
