using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.report
{
    public partial class FrmStatisticsTemplate : FrmCommon
    {
        public FrmStatisticsTemplate()
        {
            InitializeComponent();
        }

        List<EntityTpTemplate> dtStati = null;
        public FrmStatisticsTemplate(List<EntityTpTemplate> dtSt)
        {
            InitializeComponent();
            dtStati = dtSt;
        }

        private void FrmStatisticsTemplate_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] {sysToolBar1.BtnSave.Name });
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (txtName.EditValue != null && txtName.EditValue.ToString() != "")
            {
                if (dtStati!=null)
                {
                    foreach (EntityTpTemplate dr in dtStati)
                    {
                        dr.StName = txtName.EditValue.ToString();
                    }
                    ProxyStatTemp proxy = new ProxyStatTemp();
                    bool isSuccess = proxy.Service.InsertTpTemplate(dtStati);
                    if (isSuccess)
                    {
                        this.Close();
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("请输入模板名称！", "提示");
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                sysToolBar1_OnBtnSaveClicked(null, null);
            }
        }

    }
}
