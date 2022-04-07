using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using System.IO;
using System.Drawing.Printing;

namespace dcl.client.report
{
    public partial class FrmPrintConfiguration : FrmCommon
    {
        public FrmPrintConfiguration()
        {
            InitializeComponent();

            psdPrint.AllowPrinter = true;

        }
        //DataTable dt = CommonClient.CreateDT(new string[] { "paper", "heigth", "width", 
        //            "left", "right", "top", "bottom", "printName", "landscape" }, "printSet");
        DataTable dt = CommonClient.CreateDT(new string[] { "paper", "heigth", "width", "printName", "landscape", "printType", "ContinuousPrinting" }, "printSet");
        //  string xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\printConfig.xml";
        string xmlFile = dcl.client.common.PathManager.SettingLisPath + @"printXml\printConfig.xml";

        bool PrintSet_A4Status = UserInfo.GetSysConfigValue("PrintSet_A4Status") == "是";

        private void FrmPrintConfiguration_Load(object sender, EventArgs e)
        {
            BindingPrinterNameList();
            sysPrints.SetToolButtonStyle(new string[] { sysPrints.BtnPrintSet.Name, sysPrints.BtnConfirm.Name });
            if (File.Exists(xmlFile))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt != null)
                    {

                        DataRow dr = dt.Rows[0];

                        if (!dt.Columns.Contains("ContinuousPrinting"))
                        {
                            dt.Columns.Add("ContinuousPrinting");
                        }

                        if (dt.Columns.Count > 2)
                        {
                            setlbl(dr);

                            psdPrint.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize(dr["paper"].ToString(), Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["heigth"]));
                            //pdPrint.DefaultPageSettings.Margins.Left = Convert.ToInt32(dr["left"]);
                            //pdPrint.DefaultPageSettings.Margins.Right = Convert.ToInt32(dr["right"]);
                            //pdPrint.DefaultPageSettings.Margins.Top = Convert.ToInt32(dr["top"]);
                            //pdPrint.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(dr["bottom"]);


                            bool isHavePrint = false;

                            //PrinterSettings.StringCollection printNameAll = new PrinterSettings.StringCollection();
                            foreach (string printName in PrinterSettings.InstalledPrinters)
                            {
                                if (printName == dr["printName"].ToString())
                                {
                                    isHavePrint = true;
                                    break;
                                }
                            }

                            if (!isHavePrint)
                            {
                                PrintDocument pdc = new PrintDocument();
                                pdPrint.PrinterSettings.PrinterName = pdc.PrinterSettings.PrinterName;
                            }
                            else
                            {
                                pdPrint.PrinterSettings.PrinterName = dr["printName"].ToString();
                                comboBoxPrinter.Text = dr["printName"].ToString();
                            }

                            if (dr["landscape"].ToString() == "True")
                                pdPrint.DefaultPageSettings.Landscape = true;
                            else
                                pdPrint.DefaultPageSettings.Landscape = false;
                        }

                        if (dr["printType"].ToString() == "0")
                        {
                            rbMachinePlay.Checked = true;
                        }
                        else if (dr["printType"].ToString() == "1")
                        {
                            rbPrintWith.Checked = true;
                        }
                        else if (dr["printType"].ToString() == "2")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            rbMachinePlay.Checked = true;
                        }
                        if (dr["ContinuousPrinting"] != null && dr["ContinuousPrinting"].ToString() == "1")
                        {
                            cbContinuousPrint.Checked = true;
                        }
                        else
                        {
                            cbContinuousPrint.Checked = false;

                        }
                        if (PrintSet_A4Status)
                        {
                            if (rbMachinePlay.Checked == true)
                            {
                                //cbContinuousPrint.Checked = true;
                                cbContinuousPrint.Enabled = true;
                            }
                            if (rbPrintWith.Checked == true)
                            {
                                //cbContinuousPrint.Checked = false;
                                cbContinuousPrint.Enabled = false;
                            }
                        }
                    }
                }
            }
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
                }

                var ps = new PrinterSettings();

                comboBoxPrinter.SelectedValue = ps.PrinterName;
            }
            catch (Exception)
            {

            }
        }

        private void sysPrints_BtnPrintSetClick(object sender, EventArgs e)
        {
            psdPrint.AllowMargins = false;

            DialogResult dr = DialogResult.No;
            dr = psdPrint.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (dt.Columns.Count == 2)
                    dt = CommonClient.CreateDT(new string[] { "paper", "heigth", "width", "printName", "landscape", "printType", "ContinuousPrinting" }, "printSet");

                string paper = pdPrint.DefaultPageSettings.PaperSize.PaperName;
                int heigth = pdPrint.DefaultPageSettings.PaperSize.Height;
                int width = pdPrint.DefaultPageSettings.PaperSize.Width;
                //int left= pdPrint.DefaultPageSettings.Margins.Left;//*0.25;
                //int right = pdPrint.DefaultPageSettings.Margins.Right; //* 0.25;
                //int top = pdPrint.DefaultPageSettings.Margins.Top; //* 0.25;
                //int bottom = pdPrint.DefaultPageSettings.Margins.Bottom;//* 0.25;
                string printName = pdPrint.PrinterSettings.PrinterName;
                //comboBoxPrinter.Text = printName;
                bool landscape = pdPrint.DefaultPageSettings.Landscape;

                rbHorizontal.Checked = landscape;

                string printType;// = "1";
                if (rbMachinePlay.Checked)
                {
                    printType = "0";
                }
                else if (rbPrintWith.Checked)
                {
                    printType = "1";
                }
                else if (radioButton1.Checked)
                {
                    printType = "2";
                }
                else
                {
                    printType = "0";
                }

                dt.Clear();
                DataRow row = dt.NewRow();
                row["printType"] = printType;
                row["paper"] = paper;
                row["heigth"] = heigth;
                row["width"] = width;
                row["printName"] = printName;
                row["landscape"] = landscape;
                row["ContinuousPrinting"] = this.cbContinuousPrint.Checked ? "1" : "0";
                dt.Rows.Add(row);
                //dt.Rows.Add(paper, heigth, width, printName, landscape, printType, this.cbContinuousPrint.Checked ? "1" : "0");
                setlbl(dt.Rows[0]);
            }
        }

        private void setlbl(DataRow dr)
        {
            //lblSize.Text = dr["paper"].ToString();
            //lbl_Left.Text = dr["left"].ToString();
            //lbl_Right.Text = dr["right"].ToString();
            //lbl_Top.Text = dr["top"].ToString();
            //lbl_Next.Text = dr["bottom"].ToString();
            comboBoxPrinter.Text = dr["printName"].ToString();
            string ld = dr["landscape"].ToString();
            string printType = dr["printType"].ToString();
            if (ld == "True")
                rbHorizontal.Checked = true;
            else
                rbVertical.Checked = true;


            if (printType == "0")
            {
                rbMachinePlay.Checked = true;
            }
            else if (printType == "1")
            {
                rbPrintWith.Checked = true;
            }
            else if (printType == "2")
            {
                radioButton1.Checked = true;
            }
            else
            {
                rbMachinePlay.Checked = true;
            }
        }

        private void setPrint()
        {
            if (File.Exists(xmlFile))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        setlbl(dr);
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


                        if (dr["printType"].ToString() == "0")
                        {
                            rbMachinePlay.Checked = true;
                        }
                        else if (dr["printType"].ToString() == "1")
                        {
                            rbPrintWith.Checked = true;
                        }
                        else if (dr["printType"].ToString() == "2")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            rbMachinePlay.Checked = true;
                        }
                    }
                }
            }
        }

        private void sysPrints_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if (dt == null || dt.Rows.Count == 0)
            {
                DataRow drPrint = dt.NewRow();
                if (rbMachinePlay.Checked)
                {
                    drPrint["printType"] = "0";
                }
                else if (rbPrintWith.Checked)
                {
                    drPrint["printType"] = "1";
                }
                else if (radioButton1.Checked)
                {
                    drPrint["printType"] = "2";
                }
                else
                {
                    drPrint["printType"] = "0";
                }
                drPrint["ContinuousPrinting"] = this.cbContinuousPrint.Checked ? "1" : "0";
                if (!string.IsNullOrEmpty(comboBoxPrinter.Text))
                    drPrint["printName"] = comboBoxPrinter.Text;
                else
                {
                    drPrint["printName"] = string.Empty;
                }
                drPrint["paper"] = pdPrint.DefaultPageSettings.PaperSize.PaperName; ;
                drPrint["heigth"] = pdPrint.DefaultPageSettings.PaperSize.Height;
                drPrint["width"] = pdPrint.DefaultPageSettings.PaperSize.Width;
                drPrint["landscape"] = rbHorizontal.Checked;//是否横向
                dt.Rows.Add(drPrint);
            }
            else
            {
                DataRow drPrint = dt.Rows[0];
                if (rbMachinePlay.Checked)
                {
                    drPrint["printType"] = "0";
                }
                else if (rbPrintWith.Checked)
                {
                    drPrint["printType"] = "1";
                }
                else if (radioButton1.Checked)
                {
                    drPrint["printType"] = "2";
                }
                else
                {
                    drPrint["printType"] = "0";
                }
                drPrint["landscape"] = rbHorizontal.Checked;//是否横向
                drPrint["ContinuousPrinting"] = this.cbContinuousPrint.Checked ? "1" : "0";
                if (!string.IsNullOrEmpty(comboBoxPrinter.Text))
                    drPrint["printName"] = comboBoxPrinter.Text;
            }

            ds.Tables.Add(dt.Copy());
            //  string lisConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml";
            string lisConfigPath = dcl.client.common.PathManager.SettingLisPath + @"\printXml";

            if (!Directory.Exists(lisConfigPath))
                Directory.CreateDirectory(lisConfigPath);
            ds.WriteXml(xmlFile);
            //setlbl(dt.Rows[0]);
            ReportSetting.Refresh();
            this.Close();
        }

        private void comboBoxPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rbVertical_CheckedChanged(object sender, EventArgs e)
        {
            if (rbVertical.Checked)
            {
                pdPrint.DefaultPageSettings.Landscape = false;
            }
        }

        private void rbHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHorizontal.Checked)
            {
                pdPrint.DefaultPageSettings.Landscape = true;
            }
        }

        private void rbPrintWith_CheckedChanged(object sender, EventArgs e)
        {
            if (PrintSet_A4Status)
            {
                cbContinuousPrint.Checked = false;
                cbContinuousPrint.Enabled = false;
            }
        }

        private void rbMachinePlay_CheckedChanged(object sender, EventArgs e)
        {
            if (PrintSet_A4Status)
            {
                cbContinuousPrint.Checked = true;
                cbContinuousPrint.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (PrintSet_A4Status)
            {
                cbContinuousPrint.Enabled = true;
            }
        }
    }
}
