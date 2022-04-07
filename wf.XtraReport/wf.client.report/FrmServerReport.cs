using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.report
{
    public partial class FrmServerReport : FrmCommon
    {
        string reportName;
        public FrmServerReport()
        {
            InitializeComponent();
        }

        public FrmServerReport(string reportName)
        {
            InitializeComponent();
            this.reportName = reportName;
        }

        private void FrmServerReport_Load(object sender, EventArgs e)
        {
            sysReport.SetToolButtonStyle(new string[] {sysReport.BtnConfirm.Name });
            DataTable dt = CommonClient.CreateDT(new string[] {"repAddress" }, "report");
            dt.Rows.Add(reportName);
            gcServerReport.DataSource = base.doSearch(dt).Tables["report"];
        }

        private void sysReport_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            DataRow drRep= gvServerReport.GetFocusedDataRow();
            if (drRep != null)
            {
                DataTable dt = CommonClient.CreateDT(new string[] { "repBacAddress","repResAddress" }, "report");
                dt.Rows.Add(reportName, drRep["reportName"]);
                base.doUpdate(dt);
                if (base.isActionSuccess)
                {
                    this.Close();
                }
            }
            else
                lis.client.control.MessageDialog.Show("请选择要还原的文件!", "提示");
        }
    }
}
