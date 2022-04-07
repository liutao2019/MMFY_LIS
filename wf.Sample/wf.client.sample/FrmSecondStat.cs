using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraReports.UI;
using dcl.client.report;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmSecondStat : FrmCommon
    {
        public FrmSecondStat()
        {
            InitializeComponent();
        }

        public FrmSecondStat(string typeId, string typeName)
        {
            InitializeComponent();
            lueType.valueMember = typeId;
            lueType.displayMember = typeName;
        }

        private void FrmSecondStat_Load(object sender, EventArgs e)
        {
            deStrat.EditValue = ServerDateTime.GetServerDateTime().Date;
            deEnd.EditValue = ServerDateTime.GetServerDateTime().Date.AddDays(1).AddMilliseconds(-1);
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name, sysToolBar1.BtnSinglePrint.Name, sysToolBar1.BtnClose.Name });
        }

        string localPath = dcl.client.common.PathManager.ReportPath;
  
        XtraReport xr;
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            EntityStatisticsQC statQC = new EntityStatisticsQC();
            if (lueType.valueMember != null && lueType.valueMember.Trim() != "")
            {
                statQC.BacilliType = lueType.valueMember;
            }

            if (deStrat.EditValue != null)
            {
                statQC.DateEditStart = deStrat.EditValue.ToString();
            }
            if (deEnd.EditValue != null)
            {
                statQC.DateEditEnd = deEnd.EditValue.ToString();
            }
            if (txtNoStrat.Text.Trim() != "")
            {
                statQC.EditYBStart = txtNoStrat.Text.Trim();
            }
            ProxySecondSign proxy = new ProxySecondSign();
            EntityDCLPrintData ds = proxy.Service.GetReportData(statQC);

            if (ds.ReportData != null && ds.ReportData.Tables.Count > 0)
            {
                string pathStr = "";
                xr = new XtraReport();
                pathStr = localPath + ds.ReportName + ds.ReportSuffix;
                xr.LoadLayout(pathStr);
                SetHospitalName.setName(xr.Bands);
                DataTable dt = new DataTable();
                xr.DataSource = ds.ReportData;
                prpGene.printControl1.PrintingSystem = xr.PrintingSystem;
                xr.CreateDocument();
                prpGene.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing);
                prpGene.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous);
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("找不到数据", 1);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (xr != null)
            {
                xr.Print();
            }
        }

    }
}
