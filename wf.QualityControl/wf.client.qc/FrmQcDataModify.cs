using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmQcDataModify : FrmCommon
    {
        public FrmQcDataModify()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 上一次操作ID
        /// </summary>
        public string lastOperationID = string.Empty;

        /// <summary>
        /// 上一次操作人密码
        /// </summary>
        public string lastOperationPw = string.Empty;

        public string QcId { get; set; }

        public string QcData { get; set; }

        public bool QcIsAudit { get; set; }

        /// <summary>
        /// 是否失控
        /// </summary>
        public bool QcOutOfControl { get; set; }

        public string QcDetailId { get; set; }

        public string QcItemEcd { get; set; }

        private void FrmQcDataModify_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnModify.Name, sysToolBar1.BtnDelete.Name });

            List<EntityDicPubEvaluate> listEva = CacheClient.GetCache<EntityDicPubEvaluate>();

            foreach (EntityDicPubEvaluate drBscripe in listEva)
            {
                if (drBscripe.EvaFlag == "6")
                    txtFun.Properties.Items.Add(drBscripe.EvaContent.Trim());
                if (drBscripe.EvaFlag == "5")
                    txtReson.Properties.Items.Add(drBscripe.EvaContent.Trim());
            }
        }

        private void FrmQcDataModify_Deactivate(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Visible = false;
        }


        public void setQcData()
        {
            txtQcData.Properties.ReadOnly = !QcIsAudit;
            txtQcData.Text = QcData;
            labItemEcd.Text = QcItemEcd;
            lblErro.Text = !QcIsAudit ? "该数据已审核" : string.Empty;
            sysToolBar1.BtnModify.Enabled = QcIsAudit;
            sysToolBar1.BtnDelete.Enabled = QcIsAudit;
            if (QcOutOfControl)
            {
                pnlOutOfControl.Visible = true;
                this.Height = 200;
            }
            else
            {
                pnlOutOfControl.Visible = false;
                this.Height = 200 - pnlOutOfControl.Height;
            }
        }

        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (QcId != null && QcId != string.Empty && txtQcData.Text.Trim() != string.Empty)
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 修改", "", "", "");

                DialogResult dig = DialogResult.Cancel;

                if (lastOperationID != string.Empty && lastOperationPw != string.Empty)
                {
                    bool flag = frmCheck.Valid(lastOperationID, lastOperationPw);
                    if (!flag)
                        dig = frmCheck.ShowDialog();
                    else
                        dig = DialogResult.OK;
                }
                else
                {
                    //frmCheck.txtLoginid.Text = LastOperationID;
                    frmCheck.ActiveControl = frmCheck.txtLoginid;
                    dig = frmCheck.ShowDialog();
                }

                if (dig == DialogResult.OK)
                {
                    EntityObrQcResult dtQcValue = new EntityObrQcResult();
                    dtQcValue.QresSn = Convert.ToInt32(QcId);
                    dtQcValue.QresValue = txtQcData.Text.Trim();
                    dtQcValue.QresType = "1";
                    dtQcValue.QresReasons = txtReson.Text.Trim();
                    dtQcValue.QresProcess = txtFun.Text.Trim();

                    ProxyObrQcResult proxyQc = new ProxyObrQcResult();

                    if (proxyQc.Service.UpdateQcResult(dtQcValue))
                    {
                        RefreshData();
                        this.Visible = false;
                    }

                    lastOperationID = frmCheck.txtLoginid.Text == string.Empty ? lastOperationID : frmCheck.txtLoginid.Text;
                    lastOperationPw = frmCheck.txtPassword.Text == string.Empty ? lastOperationPw : frmCheck.txtPassword.Text;
                }
            }
            else
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请输入测定值");
        }

        public delegate void RefreshQcData();
        public event RefreshQcData RefreshData;

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (QcId != null && QcId != string.Empty)
            {
                if (lis.client.control.MessageDialog.Show("是否删除测定数据?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<string> listQresSn = new List<string>();
                    listQresSn.Add(QcId);

                    ProxyObrQcResult proxyQc = new ProxyObrQcResult();

                    if (proxyQc.Service.DeleteQcResult(listQresSn))
                    {
                        RefreshData();
                        this.Visible = false;
                    }
                }
            }
        }
    }
}
