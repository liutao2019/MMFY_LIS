using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraCharts;
using lis.client.control;
using dcl.entity;
using dcl.client.common;
using dcl.client.wcf;
using dcl.client.report;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmQcDesc : FrmCommon
    {
        string strItrId = string.Empty;
        public FrmQcDesc(string itrId)
        {
            strItrId = itrId;
            InitializeComponent();
        }
        private void FrmMensurate_Load(object sender, EventArgs e)
        {
            sysToolBar1.BtnQualityRule.Caption = "审核";
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnRefresh", "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", sysToolBar1.BtnQualityRule.Name, "BtnCancel", sysToolBar1.BtnSinglePrint.Name, "BtnClose" });
            lue_ItemsFilter();
            OnRefresh();
            sysToolBar1_Enabled(true);
        }
        private void sysToolBar1_Enabled(bool isTrue)
        {
            sysToolBar1.BtnAdd.Enabled = isTrue;
            sysToolBar1.BtnModify.Enabled = isTrue;
            sysToolBar1.BtnDelete.Enabled = isTrue;
            sysToolBar1.BtnSave.Enabled = !isTrue;
            sysToolBar1.BtnCancel.Enabled = !isTrue;
            dtSdate.ReadOnly = isTrue;
            dtEdate.ReadOnly = isTrue;
            rbAnalysis.ReadOnly = isTrue;
        }
        private void OnRefresh()
        {
            EntityObrQcResultQC qc = new EntityObrQcResultQC();
            qc.ItrId = strItrId;
            qc.QanLevel = cmbType.Text;
            qc.ItemId = lue_Items.valueMember;
            ProxyObrQcAnalysis proxyQc = new ProxyObrQcAnalysis();
            List<EntityObrQcAnalysis> list = proxyQc.Service.SearchQcAnalysis(qc);
            bsQcAnalysis.DataSource = list;
        }
        private void btnReferesh_Click(object sender, EventArgs e)
        {
            OnRefresh();
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.bsQcAnalysis.EndEdit();
            if (bsQcAnalysis.Current == null)
            {
                return;
            }
            EntityObrQcAnalysis qcAnalysis = (EntityObrQcAnalysis)bsQcAnalysis.Current;
            if (qcAnalysis.QanAuditFlag == 1)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("已审核不允许修改！");
                return;
            }
            ProxyObrQcAnalysis proxyQc = new ProxyObrQcAnalysis();
            bool success = false;
            if (string.IsNullOrEmpty(qcAnalysis.QanId))
            {
                success = proxyQc.Service.InsertQcAnalysis(qcAnalysis);
            }
            else {
                success = proxyQc.Service.UpdateQcAnalysis(qcAnalysis);
            }
            if (success)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功！");
                OnRefresh();
            }
            sysToolBar1_Enabled(true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            sysToolBar1_Enabled(false);
            EntityObrQcAnalysis qcAnalysis = (EntityObrQcAnalysis)bsQcAnalysis.AddNew();
            dtSdate.EditValue = DateTime.Now;
            dtEdate.EditValue = DateTime.Now.AddDays(1);
            qcAnalysis.QanDateStart = dtSdate.DateTime;
            qcAnalysis.QanDateEnd = dtEdate.DateTime;
            qcAnalysis.QanItmId = lue_Items.valueMember;
            qcAnalysis.QanItrId = strItrId;
            qcAnalysis.QanLevel =cmbType.Text;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            EntityObrQcAnalysis qcAnalysis = gvQcAnalysis.GetFocusedRow() as EntityObrQcAnalysis;
            if (qcAnalysis==null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据", "提示");
                return;
            }
            if (qcAnalysis.QanAuditFlag==1)
            {
                lis.client.control.MessageDialog.Show("已经审核不允许删除！", "提示");
                return;
            }
            DialogResult dresult = MessageBox.Show("确定要删除该条数据吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult != DialogResult.OK)
                return;
            ProxyObrQcAnalysis proxy = new ProxyObrQcAnalysis();
            bool success = proxy.Service.DeleteQcAnalysis(qcAnalysis);

            if (success)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("删除成功！");
                OnRefresh();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 项目过滤
        /// </summary>
        private void lue_ItemsFilter()
        {
            if (!string.IsNullOrEmpty(strItrId))
            {
                PoxyFrmParameter proxy = new PoxyFrmParameter();
                List<EntityDicMachineCode> mitmNoList = proxy.Service.SearchMitmNo(strItrId);//对应 SearchMitmNo(itrId) 

                if ( mitmNoList.Count > 0)
                {
                    StringBuilder sbMtId = new StringBuilder();
                    foreach (var drMitm in mitmNoList)
                    {
                        sbMtId.Append(string.Format(",'{0}'", drMitm.MacItmId));
                    }
                    sbMtId.Remove(0, 1);

                    //根据仪器所含的专业组过滤项目，并过滤掉停用的项目
                    lue_Items.SetFilter(lue_Items.getDataSource().FindAll(w => sbMtId.ToString().Contains(w.ItmId) && w.ItmDelFlag == "0"));
                }
            }

        }
        private void lue_Items_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            OnRefresh();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            sysToolBar1_Enabled(false);
        }

        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            sysToolBar1_Enabled(true);
            OnRefresh();
        }
        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            string repCode= ConfigHelper.GetSysConfigValueWithoutLogin("QC_Analysis_RepCode");
            if (!string.IsNullOrEmpty(repCode))
            {
                EntityDCLPrintParameter par = new EntityDCLPrintParameter();
                par.ReportCode = repCode;
                par.CustomParameter.Add("ItrId", strItrId);
                if (!string.IsNullOrEmpty(lue_Items.valueMember))
                {
                    par.CustomParameter.Add("ItmId", lue_Items.valueMember);
                }
               if(!string.IsNullOrEmpty(cmbType.Text))
                {
                    par.CustomParameter.Add("QanLevel", cmbType.Text);
                }
                DCLReportPrint.Print(par);
            }
            else {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请配置报表代码！");
            }
        }
         /// <summary>
         /// 审核
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void sysToolBar1_BtnQualityAuditClicked(object sender, EventArgs e)
        {
            EntityObrQcAnalysis qcAnalysis = gvQcAnalysis.GetFocusedRow() as EntityObrQcAnalysis;
            if (qcAnalysis == null)
            {
                lis.client.control.MessageDialog.Show("请选择要审核的数据", "提示");
                return;
            }
            if (qcAnalysis.QanAuditFlag==1)
            {
                lis.client.control.MessageDialog.Show("该条数据已被审核", "提示");
                return;
            }
            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", "", "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                qcAnalysis.QanAuditUserId = frmCheck.OperatorID;
                qcAnalysis.QanAuditFlag = 1;
                qcAnalysis.QanAuditDate = ServerDateTime.GetServerDateTime();
                ProxyObrQcAnalysis proxyQc = new ProxyObrQcAnalysis();
                bool result = proxyQc.Service.UpdateQcAnalysis(qcAnalysis);
                if (result)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("审核成功!");
                    OnRefresh();
                }
            }
        }

        private void cmbType_EditValueChanged(object sender, EventArgs e)
        {
            OnRefresh();
        }
    }
}
