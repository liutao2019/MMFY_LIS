using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using lis.client.control;
using System.Collections;
using dcl.client.report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using dcl.common;
using System.Text.RegularExpressions;
using dcl.entity;
using dcl.client.wcf;
using System.IO;

namespace dcl.client.ananlyse
{
    public partial class FrmSummaryPrint : FrmCommon
    {
        public FrmSummaryPrint()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmSummaryPrint_Shown);
        }

        void FrmSummaryPrint_Shown(object sender, EventArgs e)
        {
            //解决 报告查询，统计分析等具有时间选择需要单击进行编辑； 
            prpBacilli.Visible = true;
            if (this.MdiParent != null)
            {
                this.MdiParent.Activate();
            }
            this.Activate();
            this.deStart.Focus();
        }

        private void FrmSummaryPrint_Load(object sender, EventArgs e)
        {
            //txtWhere.Properties.ReadOnly = true;
            this.deStart.EditValue = DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
            this.deEnd.EditValue = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day).Date;
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name, sysToolBar1.BtnSinglePrint.Name, sysToolBar1.BtnExport.Name, sysToolBar1.BtnClose.Name });

        }


        //封装查询条件
        private EntityAnanlyseQC getWhere()
        {
            EntityAnanlyseQC query = new EntityAnanlyseQC();

            if (lue_Instrmt.valueMember != null && lue_Instrmt.valueMember != "")
            {
                query.ItrId = lue_Instrmt.valueMember;
            }

            if (this.deStart.EditValue != null)
            {
                query.DateStart = Convert.ToDateTime(deStart.EditValue);
            }
            if (this.deEnd.EditValue != null)
            {
                query.DateEnd = Convert.ToDateTime(deEnd.DateTime.Date).AddDays(1);
            }

            //判断是否为字母，为字母的话就是登录管理员密码进入管理界面
            string pattern = @"^\d+(\.)?\d*$";
            if (txtSid.EditValue != null && !string.IsNullOrEmpty(txtSid.EditValue.ToString()))
            {
                if (!Regex.IsMatch(txtSid.EditValue.ToString().Trim().Replace(",", "").Replace("-", ""), pattern))
                {
                    MessageBox.Show("请正确输入数字范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtSid.SelectAll();
                    this.txtSid.Focus();
                    return null;

                }
                else
                {
                    if (radioGroup1.EditValue.ToString() == "0")
                    {
                        query.listSid = ConvertStringSidToListSid.GetListSid(txtSid.EditValue.ToString());
                    }
                    else
                    {
                        query.listSort = ConvertStringSidToListSid.GetListSortNo(txtSid.EditValue.ToString());
                    }
                }
            }
            //if (this.lue_Instrmt.valueMember != null && lue_Instrmt.valueMember != "")
            //{
            //    where += " and patients.pat_itr_id='" + lue_Instrmt.valueMember + "'";
            //}
            if (this.lue_Sample.valueMember != null && lue_Sample.valueMember != "")
            {
                query.SamId = lue_Sample.valueMember;
            }
            string strType = rgType.EditValue.ToString();
            switch (strType)
            {
                case "0":
                    query.reportType = ReportType.SHCDJG;
                    break;
                case "1":
                    query.reportType = ReportType.YXCDJG;
                    break;
                case "2":
                    query.reportType = ReportType.WSCDJG;
                    break;
                default:
                    break;
            }
            return query;
        }
        XtraReport xr;
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            if (this.deStart.EditValue == null || this.deEnd.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入起始日期！", "提示");
                return;
            }
            TimeSpan ts = Convert.ToDateTime(deEnd.EditValue).Subtract(Convert.ToDateTime(deStart.EditValue));
            int day = ts.Days;
            if (day < 0)
            {
                lis.client.control.MessageDialog.Show("结束日期大于开始日期！", "提示");
                return;
            }
            if (day > 61)
            {
                lis.client.control.MessageDialog.Show("日期范围不能超过两个月！", "提示");
                return;
            }


            //if (lue_Instrmt.valueMember == null || (lue_Instrmt.valueMember != null && lue_Instrmt.valueMember == ""))
            //{
            //    lis.client.control.MessageDialog.Show("请选择仪器！", "提示");
            //    return;
            //}
            EntityAnanlyseQC query = getWhere();
            if (query == null)
                return;

            //为空是查询条件错误，返回
            if (string.IsNullOrEmpty(query.DateStart.ToString()))
            {
                return;
            }
            query._repCode = UserInfo.GetSysConfigValue("SummaryPrintReport");
            ProxySummaryPrint proxy = new ProxySummaryPrint();

            DataSet ds = new DataSet();
            printer = proxy.Service.GetReportData(query);
            if (string.IsNullOrEmpty(printer.ReportName))
            {
                MessageDialog.Show(string.Format("数据表中未找到报表"));
                return;
            }
            string localPath = PathManager.ReportPath;
            string pathStr = localPath + printer.ReportName + printer.ReportSuffix;
            if (!File.Exists(pathStr))
            {
                lis.client.control.MessageDialog.Show(string.Format("报表不存在!"));
                return;
            }
            ds = printer.ReportData;
            //   string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport\";
            if (ds != null && ds.Tables.Count > 0)
            {
                try
                {
                    if (ds.Tables.Contains("可设计字段"))
                    {
                        gcPar.DataSource = ds.Tables["可设计字段"];
                    }

                    xr = new XtraReport();
                    xr.LoadLayout(pathStr);
                    SetHospitalName.setName(xr.Bands);
                    DataTable dt = new DataTable();
                    xr.DataSource = ds;
                    prpBacilli.printControl1.PrintingSystem = xr.PrintingSystem;
                    //xr.PrintingSystem = this.printingSystem1;
                    xr.CreateDocument();
                }
                catch (Exception ex)
                {
                    lis.client.control.MessageDialog.Show(ex.ToString());
                }
            }

        }
        EntityDCLPrintData printer = new EntityDCLPrintData();
        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (xr != null)
            {
                xr.Print();
            }
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcPar.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("文件名不能为空！");
                        return;
                    }

                    try
                    {
                        //gcPar.ExportToExcelOld(ofd.FileName.Trim());
                        gcPar.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        // Lib.LogManager.Logger.LogException("导出", ex);
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出失败！");
                    }
                }

            }
        }
    }
}
