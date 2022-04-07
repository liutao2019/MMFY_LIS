using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.dicbasic
{
    public partial class frmItemModifyHistory : FrmCommon
    {
        DataTable _datasource;
        public frmItemModifyHistory(DataTable datasource)
        {
            InitializeComponent();
            this._datasource = datasource;
            this.dateTimePicker1.Value = DateTime.Now.Date.AddDays(-30);
            this.dateTimePicker2.Value = DateTime.Now.Date;
        }

        private void frmItemModifyHistory_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = this._datasource;
            Filter();
        }

        private void Filter()
        {
            string filter = string.Format("OperationTime >= '{0}' and OperationTime < '{1}'"
                , this.dateTimePicker1.Value.ToString("yyyy-MM-dd")
                , this.dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd")
                );

            if (this.textBox1.Text.Trim() != string.Empty)
            {
                filter += string.Format(" and OperationKey = '{0}'", this.textBox1.Text);
            }
            this.bindingSource1.Filter = filter;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Filter();
        }
    }
}
