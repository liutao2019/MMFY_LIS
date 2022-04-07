using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.qc
{
    public partial class frmQcDataModifyHistory : FrmCommon
    {
        public frmQcDataModifyHistory()
        {
            InitializeComponent();
            this.dateTimePicker1.Value = DateTime.Now.Date.AddDays(-30);
            this.dateTimePicker2.Value = DateTime.Now.Date;
        }

        private void frmItemModifyHistory_Load(object sender, EventArgs e)
        {
        }

        private void LoadData()
        {
            EntityLogQc qc = new EntityLogQc();
            qc.DateStart = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            qc.DateEnd = this.dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd");
            qc.OperatModule = "质量控制图表";
            qc.OperatGroup = "测定结果";
            qc.OperatObject = this.lue_Item.displayMember;
            ProxySysOperationLog proxy = new ProxySysOperationLog();
            bindingSource1.DataSource = proxy.Service.GetQcOperationLog(qc);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
