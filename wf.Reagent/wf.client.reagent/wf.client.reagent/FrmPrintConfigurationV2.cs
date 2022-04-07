using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HQ.BPPrint;
using System.Xml;
using System.Diagnostics;
using dcl.client.frame;
using lis.client.control;

namespace wf.client.reagent
{
    public partial class FrmPrintConfigurationV2 : FrmCommon
    {
        BarcodePrintVersion printVersion;

        public FrmPrintConfigurationV2()
        {
            InitializeComponent();

            this.lblBPPrintInfo.Text = string.Empty;
            this.lblBPPrintInfo_old.Text = string.Empty;

            if (File.Exists("HQ.BPPrint.dll.config"))
            {
                printVersion = BarcodePrintVersion.BPPrint_Old;

                this.rdoBPPrint.Visible = false;
                this.btnBPPrintSetting.Visible = false;
            }
            else if (File.Exists(BPPrintSetting.Current.ConfigFileName))
            {
                printVersion = BarcodePrintVersion.New;

                this.rdoBPPrint_old.Visible = false;
                this.btnBPPrintSetting_old.Visible = false;
            }
            else
            {
                printVersion = BarcodePrintVersion.DriverVersion;
                rdoDriverPrint.Checked = true;
                this.rdoBPPrint.Visible = false;
                this.btnBPPrintSetting.Visible = false;

                this.rdoBPPrint_old.Visible = false;
                this.btnBPPrintSetting_old.Visible = false;
            }
            if (File.Exists("HQ.LabellerPrint.dll") || File.Exists("HQ.LabellerPrint.Client.exe"))
            {
                rdoLabellerPrint.Visible = true;
                label3.Visible = true;
                comboBoxWay.Visible = true;
                //this.rdoBPPrint.Visible = false;
                //this.btnBPPrintSetting.Visible = false;
                //this.rdoBPPrint_old.Visible = false;
                //this.btnBPPrintSetting_old.Visible = false;
            }
        }
        //DataTable dt = CommonClient.CreateDT(new string[] { "paper", "heigth", "width", 
        //            "left", "right", "top", "bottom", "printName", "landscape" }, "printSet");
        DataTable dt = CommonClient.CreateDT(new string[] { "paper", "heigth", "width", "printName", "landscape", "ReturnPrintName", "PrintMode", "PrintWay" }, "printSet");
        //  string xmlOldFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\barcodePrintConfig.xml";
        string xmlOldFile = dcl.client.common.PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";

        XmlDocument barcodePrintConfig = null;
        XmlDocument docNewBPPrint = null;
        XmlDocument docOldBPPrint = null;
        XmlNode nodeOldBpEnableNode = null;

        private void FrmPrintConfiguration_Load(object sender, EventArgs e)
        {
            this.sysPrints.CheckPower = false;
            sysPrints.SetToolButtonStyle(new string[] { sysPrints.BtnSave.Name });
            SetPrintSelectVisiable();
            BindingPrinterNameList();
            LoadSetting();
            if (IsWin7())
            {
                ReadPrinterConfig();
            }
            else
            {
                if (File.Exists(xmlOldFile))
                {
                    try
                    {
                        DataSet dsPrint = new DataSet();
                        dsPrint.ReadXml(xmlOldFile);
                        if (dsPrint.Tables.Count > 0)
                        {
                            DataTable dt = dsPrint.Tables[0];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("printName")) //打印机名称
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[0]["printName"].ToString()))
                                    {
                                        string p_PrinterName = pdPrint.PrinterSettings.PrinterName; //临时记录默认打印机
                                        pdPrint.PrinterSettings.PrinterName = dt.Rows[0]["printName"].ToString();
                                        if (string.IsNullOrEmpty(comboBoxPrinter.Text))
                                        {
                                            comboBoxPrinter.Text = dt.Rows[0]["printName"].ToString();
                                        }
                                        if (!pdPrint.PrinterSettings.IsValid) //当前打印机是否有效,无效则用默认
                                        {
                                            pdPrint.PrinterSettings.PrinterName = p_PrinterName;
                                        }
                                    }
                                }

                                if (dt.Columns.Contains("ReturnPrintName")) //打印机名称
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[0]["ReturnPrintName"].ToString()))
                                    {
                                        //string p_PrinterName = pdPrint.PrinterSettings.PrinterName; //临时记录默认打印机
                                        //pdPrint.PrinterSettings.PrinterName = dt.Rows[0]["ReturnPrintName"].ToString();
                                        if (string.IsNullOrEmpty(comboBoxReturnPrint.Text))
                                        {
                                            comboBoxReturnPrint.Text = dt.Rows[0]["ReturnPrintName"].ToString();
                                        }
                                        //if (!pdPrint.PrinterSettings.IsValid) //当前打印机是否有效,无效则用默认
                                        //{
                                        //    pdPrint.PrinterSettings.PrinterName = p_PrinterName;
                                        //}
                                    }
                                }
                                if (dt.Columns.Contains("PrintMode"))
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[0]["PrintMode"].ToString())
                                        && dt.Rows[0]["PrintMode"].ToString() == "False")
                                    {
                                        rdoLabellerPrint.Checked = true;
                                    }
                                    //else
                                    //    rdoDriverPrint.Checked = true;
                                }
                                if (dt.Columns.Contains("PrintWay"))
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[0]["PrintWay"].ToString()))
                                    {
                                        if (string.IsNullOrEmpty(comboBoxWay.Text))
                                        {
                                            comboBoxWay.Text = dt.Rows[0]["PrintWay"].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            //comboBoxWay.SelectedIndex = 0;


            //if (bUserNewBPPrint)
            //{
            //    this.lblBPPrintInfo.Text = string.Format("端口：{0}   波特率：{1}", BPPrintSetting.Current.Port, BPPrintSetting.Current.Baudrate);
            //    docNewBPPrint = new XmlDocument();
            //    docNewBPPrint.Load(BPPrintSetting.Current.ConfigFileName);
            //}
            //else
            //{

            //}

            //sysPrints.SetToolButtonStyle(new string[] { sysPrints.BtnPrintSet.Name });
            //if (File.Exists(xmlFile))
            //{
            //    DataSet ds = new DataSet();
            //    ds.ReadXml(xmlFile);
            //    if (ds.Tables.Count > 0)
            //    {
            //        dt = ds.Tables[0];
            //        if (dt != null)
            //        {
            //            DataRow dr = dt.Rows[0];
            //            setlbl(dr);

            //            psdPrint.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize(dr["paper"].ToString(), Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["heigth"]));
            //            //pdPrint.DefaultPageSettings.Margins.Left = Convert.ToInt32(dr["left"]);
            //            //pdPrint.DefaultPageSettings.Margins.Right = Convert.ToInt32(dr["right"]);
            //            //pdPrint.DefaultPageSettings.Margins.Top = Convert.ToInt32(dr["top"]);
            //            //pdPrint.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(dr["bottom"]);
            //            pdPrint.PrinterSettings.PrinterName = dr["printName"].ToString();
            //            if (dr["landscape"].ToString() == "True")
            //                pdPrint.DefaultPageSettings.Landscape = true;
            //            else
            //                pdPrint.DefaultPageSettings.Landscape = false;

            //        }

            //    }
            //}
        }

        private bool IsWin7()
        {
            try
            {
                OperatingSystem os = Environment.OSVersion;
                if (os.Version.Major < 6)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void BindingPrinterNameList()
        {
            try
            {
                PrinterSettings.StringCollection printers = PrinterSettings.InstalledPrinters;
                foreach (string printer in printers)
                {
                    comboBoxPrinter.Items.Add(printer);
                    comboBoxReturnPrint.Items.Add(printer);
                }

                var ps = new PrinterSettings();

                comboBoxPrinter.SelectedValue = ps.PrinterName;
                comboBoxReturnPrint.SelectedValue = ps.PrinterName;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 加载设定
        /// </summary>
        private void LoadSetting()
        {
            //if (UserInfo.GetSysConfigValue("BarCode_EnableLabellerPrint") == "是")
            //{
            //    return;
            //}
            //加载设定
            if (this.printVersion == BarcodePrintVersion.DriverVersion)
            {
                //PrintMachineSet_old();

                if (File.Exists(BPPrintSetting.Current.ConfigFileName))
                {
                    docNewBPPrint = new XmlDocument();
                    docNewBPPrint.Load(BPPrintSetting.Current.ConfigFileName);
                }
            }
            else if (this.printVersion == BarcodePrintVersion.BPPrint_Old)
            {
                docOldBPPrint = new XmlDocument();
                docOldBPPrint.Load("HQ.BPPrint.dll.config");

                foreach (XmlNode node in docOldBPPrint.SelectSingleNode("/configuration/appSettings").ChildNodes)
                {
                    if (node.Attributes != null && node.Attributes["key"].Value == "UseCustomPrintMode")
                    {
                        nodeOldBpEnableNode = node;
                        if (node.Attributes["value"].Value.ToLower() == "1"
                            || node.Attributes["value"].Value.ToLower() == "true")
                        {
                            this.rdoBPPrint_old.Checked = true;
                        }
                        else
                        {
                            this.rdoDriverPrint.Checked = true;
                        }
                        break;
                    }
                }

                if (File.Exists(BPPrintSetting.Current.ConfigFileName))
                {
                    docNewBPPrint = new XmlDocument();
                    docNewBPPrint.Load(BPPrintSetting.Current.ConfigFileName);
                }
            }
            else if (this.printVersion == BarcodePrintVersion.New)
            {
                this.lblBPPrintInfo.Text = string.Format("端口：{0}   波特率：{1}", BPPrintSetting.Current.Port, BPPrintSetting.Current.Baudrate);

                docNewBPPrint = new XmlDocument();
                docNewBPPrint.Load(BPPrintSetting.Current.ConfigFileName);

                string printType = docNewBPPrint.SelectSingleNode("/BarcodePrint/PrintMode").InnerText;
                if (printType == "0")//0=驱动 1=北洋串口
                {
                    this.rdoDriverPrint.Checked = true;
                }
                else if (printType == "1")
                {
                    this.rdoBPPrint.Checked = true;
                }
            }
        }

        private void sysPrints_BtnPrintSetClick(object sender, EventArgs e)
        {
            PrintMachineSet_old();
        }

        /// <summary>
        /// 旧版驱动方式打印设置
        /// </summary>
        private void PrintMachineSet_old()
        {
            #region 读取打印机配置

            ReadPrinterConfig();

            #endregion

            //psdPrint.AllowMargins = false;
            DialogResult dr = psdPrint.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string paper = pdPrint.DefaultPageSettings.PaperSize.PaperName;
                int heigth = pdPrint.DefaultPageSettings.PaperSize.Height;
                int width = pdPrint.DefaultPageSettings.PaperSize.Width;
                //int left= pdPrint.DefaultPageSettings.Margins.Left;//*0.25;
                //int right = pdPrint.DefaultPageSettings.Margins.Right; //* 0.25;
                //int top = pdPrint.DefaultPageSettings.Margins.Top; //* 0.25;
                //int bottom = pdPrint.DefaultPageSettings.Margins.Bottom;//* 0.25;
                string printName = pdPrint.PrinterSettings.PrinterName;
                string printName2 = comboBoxReturnPrint.Text;


                printName = comboBoxPrinter.Text;


                bool landscape = pdPrint.DefaultPageSettings.Landscape;

                dt.Clear();
                dt.Rows.Add(paper, heigth, width, printName, landscape, printName2);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                ds.WriteXml(xmlOldFile);
                //setlbl(dt.Rows[0]);
            }
        }

        private void ReadPrinterConfig()
        {
            if (File.Exists(xmlOldFile))
            {
                try
                {
                    DataSet dsPrint = new DataSet();
                    dsPrint.ReadXml(xmlOldFile);
                    if (dsPrint.Tables.Count > 0)
                    {
                        DataTable dt = dsPrint.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains("printName")) //打印机名称
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[0]["printName"].ToString()))
                                {
                                    string p_PrinterName = pdPrint.PrinterSettings.PrinterName; //临时记录默认打印机
                                    pdPrint.PrinterSettings.PrinterName = dt.Rows[0]["printName"].ToString();
                                    if (string.IsNullOrEmpty(comboBoxPrinter.Text))
                                    {
                                        comboBoxPrinter.Text = dt.Rows[0]["printName"].ToString();
                                    }
                                    if (!pdPrint.PrinterSettings.IsValid) //当前打印机是否有效,无效则用默认
                                    {
                                        pdPrint.PrinterSettings.PrinterName = p_PrinterName;
                                    }
                                }
                            }
                            if (dt.Columns.Contains("ReturnPrintName")) //打印机名称
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[0]["ReturnPrintName"].ToString()))
                                {
                                    //string p_PrinterName = pdPrint.PrinterSettings.PrinterName; //临时记录默认打印机
                                    //pdPrint.PrinterSettings.PrinterName = dt.Rows[0]["ReturnPrintName"].ToString();
                                    if (string.IsNullOrEmpty(comboBoxReturnPrint.Text))
                                    {
                                        comboBoxReturnPrint.Text = dt.Rows[0]["ReturnPrintName"].ToString();
                                    }
                                    //if (!pdPrint.PrinterSettings.IsValid) //当前打印机是否有效,无效则用默认
                                    //{
                                    //    pdPrint.PrinterSettings.PrinterName = p_PrinterName;
                                    //}
                                }
                            }
                            if (dt.Columns.Contains("paper")) //纸张
                            {
                                //if (!string.IsNullOrEmpty(dt.Rows[0]["paper"].ToString()))
                                //    pdPrint.DefaultPageSettings.PaperSize.PaperName = dt.Rows[0]["paper"].ToString();
                            }
                            if (dt.Columns.Contains("heigth")) //高度
                            {
                                //int p_Height = 1169;
                                //if (!string.IsNullOrEmpty(dt.Rows[0]["heigth"].ToString()))
                                //    if (int.TryParse(dt.Rows[0]["heigth"].ToString(), out p_Height))
                                //        pdPrint.DefaultPageSettings.PaperSize.Height = p_Height;
                            }
                            if (dt.Columns.Contains("width")) //宽度
                            {
                                //int p_Width = 827;
                                //if (!string.IsNullOrEmpty(dt.Rows[0]["width"].ToString()))
                                //    if (int.TryParse(dt.Rows[0]["width"].ToString(), out p_Width))
                                //        pdPrint.DefaultPageSettings.PaperSize.Width = p_Width;
                            }
                            if (dt.Columns.Contains("landscape")) //横或纵
                            {
                                //if (!string.IsNullOrEmpty(dt.Rows[0]["landscape"].ToString()))
                                //{
                                //    if (dt.Rows[0]["landscape"].ToString() == "False")
                                //        pdPrint.DefaultPageSettings.Landscape = false;
                                //    else
                                //        pdPrint.DefaultPageSettings.Landscape = true;
                                //}
                            }
                            if (dt.Columns.Contains("PrintMode"))
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[0]["PrintMode"].ToString())
                                    && dt.Rows[0]["PrintMode"].ToString() == "False")
                                {
                                    rdoLabellerPrint.Checked = true;
                                }
                                //else
                                //    rdoDriverPrint.Checked = true;
                            }
                            if (dt.Columns.Contains("PrintWay"))
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[0]["PrintWay"].ToString()))
                                {
                                    if (string.IsNullOrEmpty(comboBoxWay.Text))
                                    {
                                        comboBoxWay.Text = dt.Rows[0]["PrintWay"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void setPrint()
        {
            if (File.Exists(xmlOldFile))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlOldFile);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        //setlbl(dr);
                        pdPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(dr["paper"].ToString(), Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["heigth"]));
                        //pdPrint.DefaultPageSettings.PaperSize.Height=Convert.ToInt32(dr["heigth"]);
                        //pdPrint.DefaultPageSettings.PaperSize.Width = Convert.ToInt32(dr["width"]);
                        pdPrint.DefaultPageSettings.Margins.Left = Convert.ToInt32(dr["left"]);
                        pdPrint.DefaultPageSettings.Margins.Right = Convert.ToInt32(dr["right"]);
                        pdPrint.DefaultPageSettings.Margins.Top = Convert.ToInt32(dr["top"]);
                        pdPrint.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(dr["bottom"]);
                        pdPrint.PrinterSettings.PrinterName = dr["printName"].ToString();
                        if (dr["landscape"].ToString() == "True")
                            pdPrint.DefaultPageSettings.Landscape = true;
                        else
                            pdPrint.DefaultPageSettings.Landscape = false;
                    }
                }
            }
        }

        internal void ShowDialogQuick()
        {
            PrintMachineSet_old();
            ShowDialog();
        }

        private void btnDriverPrintSetting_Click(object sender, EventArgs e)
        {
            //if (this.printVersion == BarcodePrintVersion.DriverVersion
            //    || this.printVersion == BarcodePrintVersion.BPPrint_Old)
            //{
            PrintMachineSet_old();
            //}
            //else
            //{
            //}
        }

        /// <summary>
        /// 新版驱动方式打印设置
        /// </summary>
        private void PrintMachineSet_New()
        {
        }

        private void btnBPPrintSetting_Click(object sender, EventArgs e)
        {
            if (docNewBPPrint != null)
            {
                HQ.BPPrint.frmBPConfig frm = new frmBPConfig(docNewBPPrint);
                frm.ShowDialog();
            }
        }

        private void btnBPPrintSetting_old_Click(object sender, EventArgs e)
        {
            Process.Start("BP条码打印配置.exe");
        }

        private enum BarcodePrintVersion
        {
            DriverVersion,
            BPPrint_Old,
            New,
        }

        private void sysPrints_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (File.Exists(xmlOldFile))
            {
                barcodePrintConfig = new XmlDocument();
                barcodePrintConfig.Load(xmlOldFile);
                XmlNode nodeBarcode = this.barcodePrintConfig.SelectSingleNode("/NewDataSet/printSet/PrintMode");
                if (nodeBarcode != null)
                {
                    nodeBarcode.InnerText = "True";
                    this.barcodePrintConfig.Save(xmlOldFile);
                }
            }
            if (this.rdoBPPrint.Checked)
            {
                if (this.printVersion == BarcodePrintVersion.New)
                {
                    XmlNode node = this.docNewBPPrint.SelectSingleNode("/BarcodePrint/PrintMode");
                    node.InnerText = "1";
                    this.docNewBPPrint.Save(BPPrintSetting.Current.ConfigFileName);

                    BPTemplateSetting.Current.Refresh();
                    BPPrintSetting.Current.Refresh();
                    MessageDialog.ShowAutoCloseDialog("保存成功");
                }
                else if (this.printVersion == BarcodePrintVersion.BPPrint_Old)
                {
                    if (this.nodeOldBpEnableNode != null)
                    {
                        this.nodeOldBpEnableNode.Attributes["value"].Value = "false";
                        this.docOldBPPrint.Save("HQ.BPPrint.dll.config");
                        MessageDialog.ShowAutoCloseDialog("保存成功");
                    }
                }
            }
            else if (this.rdoDriverPrint.Checked)
            {
                if (this.printVersion == BarcodePrintVersion.New)
                {
                    XmlNode node = this.docNewBPPrint.SelectSingleNode("/BarcodePrint/PrintMode");
                    node.InnerText = "0";
                    this.docNewBPPrint.Save(BPPrintSetting.Current.ConfigFileName);

                    BPTemplateSetting.Current.Refresh();
                    BPPrintSetting.Current.Refresh();
                    MessageDialog.ShowAutoCloseDialog("保存成功");
                }
                else if (this.printVersion == BarcodePrintVersion.BPPrint_Old)
                {
                    //if (UserInfo.GetSysConfigValue("BarCode_EnableLabellerPrint") == "是"
                    //    && File.Exists(xmlOldFile))
                    //{
                    //    barcodePrintConfig = new XmlDocument();
                    //    barcodePrintConfig.Load(xmlOldFile);
                    //    XmlNode nodeBarcode = this.barcodePrintConfig.SelectSingleNode("/NewDataSet/printSet/PrintMode");
                    //    nodeBarcode.InnerText = "True";
                    //    this.barcodePrintConfig.Save(xmlOldFile);
                    //    wf.auxiliary.control.MessageDialog.ShowAutoCloseDialog("保存成功");
                    //    return;
                    //}
                    if (nodeOldBpEnableNode != null)
                    {
                        this.nodeOldBpEnableNode.Attributes["value"].Value = "false";
                        this.docOldBPPrint.Save("HQ.BPPrint.dll.config");
                        MessageDialog.ShowAutoCloseDialog("保存成功");
                    }
                }
            }
            else if (this.rdoBPPrint_old.Checked)
            {
                if (this.printVersion == BarcodePrintVersion.New)
                {
                    XmlNode node = this.docNewBPPrint.SelectSingleNode("/BarcodePrint/PrintMode");
                    node.InnerText = "0";
                    this.docNewBPPrint.Save(BPPrintSetting.Current.ConfigFileName);

                    BPTemplateSetting.Current.Refresh();
                    BPPrintSetting.Current.Refresh();
                    MessageDialog.ShowAutoCloseDialog("保存成功");
                }
                else if (this.printVersion == BarcodePrintVersion.BPPrint_Old)
                {
                    if (nodeOldBpEnableNode != null)
                    {
                        this.nodeOldBpEnableNode.Attributes["value"].Value = "true";
                        this.docOldBPPrint.Save("HQ.BPPrint.dll.config");
                        MessageDialog.ShowAutoCloseDialog("保存成功");
                    }
                }
            }
            else if (this.rdoLabellerPrint.Checked)
            {
                if (File.Exists(xmlOldFile))
                {
                    barcodePrintConfig = new XmlDocument();
                    barcodePrintConfig.Load(xmlOldFile);
                    XmlNode node = this.barcodePrintConfig.SelectSingleNode("/NewDataSet/printSet/PrintMode");
                    if (node != null)
                    {
                        node.InnerText = "False";
                        this.barcodePrintConfig.Save(xmlOldFile);

                        MessageDialog.ShowAutoCloseDialog("保存成功");
                    }

                }
            }
        }

        private void rdoDriverPrint_CheckedChanged(object sender, EventArgs e)
        {
            SetPrintSelectVisiable();
        }

        private void SetPrintSelectVisiable()
        {
            //if (IsWin7())
            //{
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["BPPrintReturn"])
                && System.Configuration.ConfigurationManager.AppSettings["BPPrintReturn"].ToUpper() == "Y")
            {
                label1.Visible = true;
                comboBoxReturnPrint.Visible = true;
            }
            else
            {
                label1.Visible = rdoDriverPrint.Checked;
                comboBoxReturnPrint.Visible = rdoDriverPrint.Checked;
            }
            lblPrinter.Visible = rdoDriverPrint.Checked;
            comboBoxPrinter.Visible = rdoDriverPrint.Checked;
            //}
        }

        private void comboBoxPrinter_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxPrinter.Text) && pdPrint != null && pdPrint.DefaultPageSettings != null)
            {
                string paper = pdPrint.DefaultPageSettings.PaperSize.PaperName;
                int heigth = pdPrint.DefaultPageSettings.PaperSize.Height;
                int width = pdPrint.DefaultPageSettings.PaperSize.Width;
                string printName = comboBoxPrinter.Text;
                string printName2 = comboBoxReturnPrint.Text;
                bool landscape = pdPrint.DefaultPageSettings.Landscape;
                string printMode = "";
                if (this.rdoLabellerPrint.Checked)
                    printMode = "False";
                else
                    printMode = "True";
                string printWay = comboBoxWay.Text;

                dt.Clear();
                dt.Rows.Add(paper, heigth, width, printName, landscape, printName2, printMode, printWay);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                ds.WriteXml(xmlOldFile);
            }
        }

        private void comboBoxReturnPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxReturnPrint.Text) && pdPrint != null && pdPrint.DefaultPageSettings != null)
            {
                string paper = pdPrint.DefaultPageSettings.PaperSize.PaperName;
                int heigth = pdPrint.DefaultPageSettings.PaperSize.Height;
                int width = pdPrint.DefaultPageSettings.PaperSize.Width;
                string printName = comboBoxPrinter.Text;
                string printName2 = comboBoxReturnPrint.Text;
                bool landscape = pdPrint.DefaultPageSettings.Landscape;
                string printMode = "";
                if (this.rdoLabellerPrint.Checked)
                    printMode = "False";
                else
                    printMode = "True";
                string printWay = comboBoxWay.Text;

                dt.Clear();
                dt.Rows.Add(paper, heigth, width, printName, landscape, printName2, printMode, printWay);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                ds.WriteXml(xmlOldFile);
            }
        }

        private void comboBoxWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxWay.Text) && pdPrint != null && pdPrint.DefaultPageSettings != null)
            {
                string paper = pdPrint.DefaultPageSettings.PaperSize.PaperName;
                int heigth = pdPrint.DefaultPageSettings.PaperSize.Height;
                int width = pdPrint.DefaultPageSettings.PaperSize.Width;
                string printName = comboBoxPrinter.Text;
                string printName2 = comboBoxReturnPrint.Text;
                bool landscape = pdPrint.DefaultPageSettings.Landscape;
                string printMode = "";
                if (this.rdoLabellerPrint.Checked)
                    printMode = "False";
                else
                    printMode = "True";
                string printWay = comboBoxWay.Text;

                dt.Clear();
                dt.Rows.Add(paper, heigth, width, printName, landscape, printName2, printMode, printWay);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                ds.WriteXml(xmlOldFile);
            }
        }
    }


}
