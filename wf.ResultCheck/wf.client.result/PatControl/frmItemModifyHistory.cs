using System;
using System.Data;
using dcl.client.frame;
using dcl.entity;
using System.Collections.Generic;

namespace dcl.client.result.PatControl
{
    public partial class frmItemModifyHistory : FrmCommon
    {
        List<EntitySysOperationLog> _datasource;
        public frmItemModifyHistory(List<EntitySysOperationLog> datasource)
        {
            InitializeComponent();
            this._datasource = datasource;
        }

        private void frmItemModifyHistory_Load(object sender, EventArgs e)
        {
            this.bsOperationLog.DataSource = this._datasource;
        }

    }
}
