using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;

using lis.client.control;
using dcl.entity;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.instrmt
{
    public partial class FrmInstrmtServicing : FrmCommon
    {
        ShowType showType = new ShowType();
        string itrId = string.Empty;
        int servicingId = 0;

        public FrmInstrmtServicing(string strItrId, string strItrName)
        {
            InitializeComponent();
            plHandle.Visible = false;
            plChk.Visible = false;
            txtInstrmt.Text = strItrName;
            this.Height = 255;
            showType = ShowType.Content;
            toolServicing.BtnDeSpe.Caption = "维修上报";
            itrId = strItrId;
        }

        public FrmInstrmtServicing(string strItrId, string strItrName, int strServicingId, string strSerContent)
        {
            InitializeComponent();
            plChk.Visible = false;
            txtSerContent.Properties.ReadOnly = true;
            txtInstrmt.Text = strItrName;
            txtSerContent.Text = strSerContent;
            this.Height = 480;
            showType = ShowType.Handler;
            toolServicing.BtnResultJudge.Caption = "处理";
            string text = "";
            List<EntityDicPubEvaluate> listPubEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>();
            foreach (EntityDicPubEvaluate pubEvaluate in listPubEvaluate)
            {
                if (pubEvaluate.EvaFlag == "17")
                {
                    text = pubEvaluate.EvaContent;
                }
            }
            txtSerChkContent.Text = text;
            itrId = strItrId;
            servicingId = strServicingId;
        }


        public FrmInstrmtServicing(string strItrId, string strItrName, int strServicingId, string strSerContent, string strHandleResult, decimal? strPrice, int? strInterval)
        {
            InitializeComponent();
            txtSerContent.Properties.ReadOnly = true;
            txtSerPrice.Properties.ReadOnly = true;
            txtSerInterval.Properties.ReadOnly = true;
            txtInstrmt.Text = strItrName;
            txtSerContent.Text = strSerContent;
            txtSerHandleResult.Properties.ReadOnly = true;
            txtSerInterval.EditValue = strInterval == null ? 0 : strPrice.Value;
            txtSerPrice.EditValue = strPrice == null ? 0 : strPrice.Value;
            txtSerHandleResult.Text = strHandleResult;
            showType = ShowType.Audit;
            toolServicing.BtnSingleAudit.Caption = "审核";
            string text = "";
            List<EntityDicPubEvaluate> listPubEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>(); 
            foreach (EntityDicPubEvaluate pubEvaluate in listPubEvaluate)
            {
                if (pubEvaluate.EvaFlag == "18")
                {
                    text = pubEvaluate.EvaContent;
                }
            }
            txtSerChkContent.Text = text;
            itrId = strItrId;
            servicingId = strServicingId;
        }

        private void FrmInstrmtServicing_Load(object sender, EventArgs e)
        {
            switch (showType)
            {
                case ShowType.Content:
                    toolServicing.SetToolButtonStyle(new string[] { toolServicing.BtnDeSpe.Name });
                    this.ActiveControl = txtSerContent;
                    break;
                case ShowType.Handler:
                    toolServicing.SetToolButtonStyle(new string[] { toolServicing.BtnResultJudge.Name });
                    this.ActiveControl = txtSerHandleResult;
                    break;
                case ShowType.Audit:
                    toolServicing.SetToolButtonStyle(new string[] { toolServicing.BtnSingleAudit.Name });
                    this.ActiveControl = txtSerChkContent;
                    break;
                default:
                    break;
            }
        }

        private void toolServicing_BtnDeSpeClick(object sender, EventArgs e)
        {
            if (txtSerContent.Text != null && txtSerContent.Text.Trim() != "")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    EntityDicItrInstrumentServicing Servicing = new EntityDicItrInstrumentServicing();
                    Servicing.SerItrId = itrId;
                    Servicing.SerPutinCode = frmCheck.OperatorID;
                    Servicing.SerContent = txtSerContent.Text.Trim();
                    
                    ProxyItrInstrumentServicing proxyServicing = new ProxyItrInstrumentServicing();

                    if (proxyServicing.Service.ServicingPutIn(Servicing)) 
                    {
                        lis.client.control.MessageDialog.Show("操作成功！");
                        this.DialogResult = DialogResult.OK;
                        return;
                    }

                }
                this.DialogResult = DialogResult.No;
            }
            else
                lis.client.control.MessageDialog.Show("请输入故障内容！");
        }

        private void toolServicing_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            if (txtSerContent.Text != null && txtSerContent.Text.Trim() != "")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    EntityDicItrInstrumentServicing Servicing = new EntityDicItrInstrumentServicing();
                    Servicing.SerId = servicingId;
                    Servicing.SerItrId = itrId;

                    Servicing.SerHandleDate = DateTime.Now; //新增加的

                    Servicing.SerHandlerCode = frmCheck.OperatorID;
                    Servicing.SerHandleResult = txtSerHandleResult.Text.Trim();
                    Servicing.SerPrice = Convert.ToDecimal(txtSerPrice.EditValue);
                    Servicing.SerInterval = Convert.ToInt32(txtSerInterval.EditValue);

                    ProxyItrInstrumentServicing proxyServicing = new ProxyItrInstrumentServicing();

                    if (proxyServicing.Service.ServingHandle(Servicing)) 
                    {
                        lis.client.control.MessageDialog.Show("操作成功！");
                        this.DialogResult = DialogResult.OK;
                        return;
                    }
                }
                this.DialogResult = DialogResult.No;
            }
            else
                lis.client.control.MessageDialog.Show("请输入处理结果！");
        }

        private void toolServicing_OnBtnSingleAuditClicked(object sender, EventArgs e)
        {
            if (txtSerChkContent.Text != null && txtSerChkContent.Text.Trim() != "")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    EntityDicItrInstrumentServicing Servicing = new EntityDicItrInstrumentServicing();
                    Servicing.SerId = servicingId;
                    Servicing.SerItrId = itrId;
                    Servicing.SerChkCode = frmCheck.OperatorID;
                    Servicing.SerChkContent = txtSerChkContent.Text.Trim();
                    
                    ProxyItrInstrumentServicing proxyServicing = new ProxyItrInstrumentServicing();

                    if (proxyServicing.Service.ServingAudit(Servicing))
                    {
                        lis.client.control.MessageDialog.Show("操作成功！");
                        this.DialogResult = DialogResult.OK;
                        return;
                    }
                }
                this.DialogResult = DialogResult.No;
            }
            else
                lis.client.control.MessageDialog.Show("请输入审核结果！");
        }
    }

    public enum ShowType
    {
        Content,
        Handler,
        Audit
    }
}
