using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;
using System.Configuration;
using dcl.common.extensions;
using dcl.client.common;


using System.Collections;
using dcl.common;
using System.IO;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmManualQuery : FrmCommon
    {
        public FrmManualQuery()
        {
            InitializeComponent();
            this.Shown += FrmBCQuery_Shown;
        }

        private void FrmBCQuery_Shown(object sender, EventArgs e)
        {
            dateSearch.DateTime = ServerDateTime.GetServerDateTime().Date;
            dateEnd.DateTime = dateSearch.DateTime.Date.AddDays(1).AddSeconds(-1);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        public EntitySampQC SampQc { get; set; }
        private void btnInquiry_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(dateSearch.Text) || string.IsNullOrEmpty(dateEnd.Text))
            {
                lis.client.control.MessageDialog.Show("请输入时间!");
                return;
            }

            SampQc = new EntitySampQC();

            SampQc.SearchDateType = SearchDataType.标本下载时间;
            SampQc.StartDate = this.dateSearch.DateTime.ToString(CommonValue.DateTimeFormat);
            SampQc.EndDate = this.dateEnd.DateTime.ToString(CommonValue.DateTimeFormat);
            SampQc.SampInfo = "122";

            SampQc.PidName = txtName.Text.Trim();

            if (!string.IsNullOrEmpty(txtBarcode.Text.Trim()))
                SampQc.ListSampBarId.Add(txtBarcode.Text.Trim());

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
