using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.users
{
    public partial class reportStateMonitor : UserControl
    {
        public reportStateMonitor()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            this.gridControlReport.DataSource = null;
            this.lbPat_id.Text = "Pat_id号：";
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="Pat_id"></param>
        public void LoadData(string Pat_id)
        {
            Reset();

            this.lbPat_id.Text = "Pat_id号：" + Pat_id;
            ProxySysOperationLog proxy = new ProxySysOperationLog();
            if (!string.IsNullOrEmpty(Pat_id) && Pat_id.Trim() != string.Empty)
            {
                List<EntitySampProcessDetail> list = proxy.Service.GetSampProcessDetail(Pat_id);
                this.gridControlReport.DataSource = list;
            }
        }

        public void setPatIdVisible(bool isView)
        {
            this.panel1.Visible = isView;
        }
    }
}
