using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using dcl.client.report;
using System.Data;
using dcl.common.extensions;
using System.Windows.Forms;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.sample
{
    public class ZeBraPrinter : IPrintMachine
    {
        string InstrumentName = "条码机";

        /// <summary>
        /// 打印 
        /// </summary>    
        public override bool Print(string machineName, string prtTemplate)
        {
            //lis.client.control.MessageDialog.Show("打印机名称：" + machineName, "提示");
            if (PrintInfo == null || this.PrintInfo.ID == null || PrintInfo.ID.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有选择条码", "提示");
                return false;
            }

            IList<string> listBCID = this.PrintInfo.ID;

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_PrintType").Trim() == "单输出任务")
            {
                foreach (string bcID in listBCID)
                {
                    List<string> listSampSn = new List<string>();
                    listSampSn.Add(bcID);
                    DCLReportPrint.PrintBarCode(prtTemplate, listSampSn, machineName);
                }
            }
            else
            {
                List<string> listSampSn = new List<string>(listBCID);
                DCLReportPrint.PrintBarCode(prtTemplate, listSampSn, machineName);
            }
            return true;
        }

        /// <summary>
        /// 门诊回执打印
        /// </summary>    
        public override bool Print(string machineName, string prtTemplate, string column, string allCombinesName)
        {
            if (PrintInfo == null || this.PrintInfo.ID == null || PrintInfo.ID.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有选择条码", "提示");
                return false;
            }

            if (prtTemplate == string.Empty)
            {
                lis.client.control.MessageDialog.Show("找不到条码机的打印模版", "提示");
                return false;
            }
            string MachineName = GetReturnPrintMachineName();
            if (string.IsNullOrEmpty(MachineName))
                MachineName = machineName;


            List<string> listBarId = new List<string>(this.PrintInfo.ID);

            EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
            foreach (string item in listBarId)
            {
                printPara.ReportCode = prtTemplate;
                printPara.ListBarId = new List<string> { item };
                DCLReportPrint.Print(printPara, MachineName); //调用基础打印方法 2018-03-12 SJC
            }


            //DCLReportPrint.PrintBarCode(prtTemplate, listSampSn, machineName);

            return true;
        }

        private string GetReturnPrintMachineName()
        {
            string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";
            if (System.IO.File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null && dt.Columns.Contains("ReturnPrintName")
                        && !string.IsNullOrEmpty(dt.Rows[0]["ReturnPrintName"].ToString()))
                    {
                        return dt.Rows[0]["ReturnPrintName"].ToString();
                    }
                    if (dt != null)
                    {
                        return dt.Rows[0]["printName"].ToString();
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// 体检检查打印条码
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="prtTemplate"></param>
        /// <returns></returns>
        public override bool PrintByTJPace(string machineName, string prtTemplate)
        {
            //if (PrintInfo == null || this.PrintInfo.lstRows == null || PrintInfo.lstRows.Count == 0)
            //{
            //    lis.client.control.MessageDialog.Show("没有选择条码", "提示");
            //    return false;
            //}

            ////string prtTemplate = "BarcodeReport";// GetItrPrtTemplate(InstrumentName);
            //if (prtTemplate == string.Empty)
            //{
            //    lis.client.control.MessageDialog.Show("找不到条码机的打印模版", "提示");
            //    return false;
            //}

            //#region 生成表格打印条码
            //DataTable dtbBarcode = new DataTable();
            //dtbBarcode.Columns.Add("条码号");
            //dtbBarcode.Columns.Add("姓名");
            //dtbBarcode.Columns.Add("性别");
            //dtbBarcode.Columns.Add("组合名称");

            //dtbBarcode.Columns.Add("科室");
            //dtbBarcode.Columns.Add("住院号");
            //dtbBarcode.Columns.Add("床号");
            //dtbBarcode.Columns.Add("年龄");
            //dtbBarcode.Columns.Add("标本");
            //dtbBarcode.Columns.Add("采集容器");
            //dtbBarcode.Columns.Add("执行科室");
            //dtbBarcode.Columns.Add("标本备注");
            //dtbBarcode.Columns.Add("急查标志");
            //dtbBarcode.Columns.Add("之夫标志");

            //#endregion

            //IList<DataRow> lstPrintInfo = this.PrintInfo.lstRows;

            ////生成条码信息表
            //foreach (DataRow drPrintInfo in lstPrintInfo)
            //{
            //    DataRow dr = dtbBarcode.NewRow();
            //    dr["条码号"] = drPrintInfo[BarcodeTable.Patient.EMPID].ToString();
            //    dr["姓名"] = drPrintInfo[BarcodeTable.Patient.Name].ToString();
            //    dr["性别"] = drPrintInfo[BarcodeTable.Patient.Sex].ToString();
            //    dr["年龄"] = drPrintInfo[BarcodeTable.Patient.Age].ToString();
            //    dr["组合名称"] = drPrintInfo[BarcodeTable.Patient.Item].ToString();
            //    dr["科室"] = drPrintInfo[BarcodeTable.Patient.Department].ToString();
            //    if (drPrintInfo[BarcodeTable.Patient.Urgent].ToString().Trim() == "1")
            //    {
            //        dr["急查标志"] = "急";
            //    }





            //    dtbBarcode.Rows.Add(dr);
            //    FrmReportPrint.Print(dtbBarcode, prtTemplate, machineName);
            //    dtbBarcode.Clear();
            //}











            return true;
        }


        /// <summary>
        /// 门诊打印回执（带合并取报告时间）
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="prtTemplate"></param>
        /// <param name="column"></param>
        /// <param name="allCombinesName"></param>
        /// <returns></returns>
        public override bool PrintExReturnReport(List<List<string>> printexp ,string machineName, string prtTemplate, string column, string allCombinesName)
        {
            if (printexp == null || printexp.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有选择条码", "提示");
                return false;
            }

            if (prtTemplate == string.Empty)
            {
                lis.client.control.MessageDialog.Show("找不到条码机的打印模版", "提示");
                return false;
            }
            
            EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
            foreach (List<string> item in printexp)
            {
                printPara.ReportCode = prtTemplate;
                printPara.ListBarId = item;
                DCLReportPrint.Print(printPara, machineName); //调用基础打印方法 2018-03-12 SJC
            }
            
            return true;
        }
    }
}
