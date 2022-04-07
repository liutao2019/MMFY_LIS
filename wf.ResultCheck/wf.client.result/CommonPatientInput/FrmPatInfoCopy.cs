using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;

using dcl.entity;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmPatInfoCopy : FrmCommon
    {
        public FrmPatInfoCopy()
        {
            InitializeComponent();
        }

        public FrmPatInfoCopy(List<EntityPidReportMain> listPats)
        {
            ListPatients = listPats;
            InitializeComponent();
            txtPatDate.DateTime = DateTime.Now;
            txtPatDate1.DateTime = DateTime.Now;
        }

        List<EntityPidReportMain> ListPatients = new List<EntityPidReportMain>();

        private void FrmPatInfoCopy_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnCopy.Name, sysToolBar1.BtnClose.Name });

            this.cbNewSid.CheckedChanged += new System.EventHandler(this.cbNwSid_CheckedChanged);
            cbNewStartEnd.CheckedChanged += CbNewStartEnd_CheckedChanged;
        }


        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            if (this.xtraTabControl1.SelectedTabPage.Name == "xtpOneToMore")
                OneToMore();

            if (this.xtraTabControl1.SelectedTabPage.Name == "xtpMoreToMore")
                MoreToMore();
        }


        #region 单个对照批量

        private void CbNewStartEnd_CheckedChanged(object sender, EventArgs e)
        {
            numStart.Enabled = cbNewStartEnd.Checked;
            numEnd.Enabled = cbNewStartEnd.Checked;
        }

        private void OneToMore()
        {
            if (txtPatInstructment1.valueMember == null || txtPatInstructment1.valueMember.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要复制的仪器!");
                return;
            }

            if (ListPatients.Count > 1)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("单个对照批量-只能选择一条记录进行批量复制!");
                return;
            }

            if (ListPatients.Count <=0 )
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择需要复制的模版!");
                return;
            }

            int startSid = (int)numStart.Value;//自定义起始样本号
            int endSid = (int)numEnd.Value;//自定义起始样本号
            if (endSid < startSid)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("末样本号不能小于始样本号!");
                return;
            }

            List<string> lisPat = new List<string>();
            String operationResult = string.Empty;
            ProxyReportCopy proxyReportCopy = new ProxyReportCopy();
            List<decimal> newSid = new List<decimal>();

            for (; startSid <= endSid; startSid++)
            {
                lisPat.Clear();
                lisPat.Add(ListPatients[0].RepId);

                newSid.Clear();
                newSid.Add(startSid);
                //复制患者信息(可自定义样本号)
                operationResult = proxyReportCopy.Service.CopyPatientsInfoCustomSid(lisPat, txtPatDate1.DateTime, txtPatInstructment1.valueMember.Trim(), newSid);

                if (operationResult != string.Empty)
                {
                    lis.client.control.MessageDialog.Show(operationResult+ "复制失败，终止操作!");
                    return;
                }
            }
            if (operationResult == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("复制成功！");
                this.Close();
            }
        }
        #endregion

        #region 批量对照批量

        private void cbNwSid_CheckedChanged(object sender, EventArgs e)
        {
            nudNwSid.Enabled = cbNewSid.Checked;
        }


        private void MoreToMore()
        {
            if (txtPatInstructment.valueMember == null || txtPatInstructment.valueMember.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要复制的仪器");
                return;
            }
            List<string> lisPat = new List<string>();
            List<decimal> newSid = new List<decimal>();//自定义样本号(队列)
            decimal startSid = nudNwSid.Value;//自定义起始样本号

            foreach (var drPat in ListPatients)
            {
                lisPat.Add(drPat.RepId);
                newSid.Add(startSid);
                startSid++;
            }
            String operationResult = string.Empty;
            ProxyReportCopy proxyReportCopy = new ProxyReportCopy();
            if (cbNewSid.Checked)
            {
                //复制患者信息(可自定义样本号)
                operationResult = proxyReportCopy.Service.CopyPatientsInfoCustomSid(lisPat, txtPatDate.DateTime, txtPatInstructment.valueMember.Trim(), newSid);
            }
            else
            {
                operationResult = proxyReportCopy.Service.CopyPatientsInfo(lisPat, txtPatDate.DateTime, txtPatInstructment.valueMember.Trim());
            }

            if (operationResult != string.Empty)
            {
                lis.client.control.MessageDialog.Show(operationResult);
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("复制成功！");
                this.Close();
            }
        }

        #endregion
    }
}
