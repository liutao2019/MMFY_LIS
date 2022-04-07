using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmControl : FrmCommon
    {
        List<EntityObrQcResult> ListQcResult = new List<EntityObrQcResult>();

        public FrmControl(List<EntityObrQcResult> dtQc_value)
        {
            InitializeComponent();

            ListQcResult = dtQc_value.FindAll(w => w.QresRunawayFlag != "2");

            dtQc_value = dtQc_value.FindAll(w => w.QresRunawayFlag == "2");

            gridControl1.DataSource = dtQc_value;
        }

        public bool isTrue = false;

        private string operatorId;

        public string OperatorId
        {
            get { return operatorId; }
            set { operatorId = value; }
        }

        private string operatorName;

        public string OperatorName
        {
            get { return operatorName; }
            set { operatorName = value; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmControl_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnSingleAudit", sysToolBar1.BtnSinglePrint.Name, "BtnClose" });


            List<EntityDicPubEvaluate> listEva = CacheClient.GetCache<EntityDicPubEvaluate>();

            foreach (EntityDicPubEvaluate drBscripe in listEva)
            {
                if (drBscripe.EvaFlag == "5")
                    repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
                if (drBscripe.EvaFlag == "6")
                    repositoryItemComboBox2.Items.Add(drBscripe.EvaContent.Trim());
            }

            lue_instrmt.DataSource = CacheClient.GetCache<EntityDicInstrument>();
        }

        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (gridControl1 != null)
            {
                gridControl1.Print();

            }
        }

        private void sysToolBar1_OnBtnSingleAuditClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            List<EntityObrQcResult> dvValue = (List<EntityObrQcResult>)gridControl1.DataSource;

            if (dvValue.FindAll(w => w.QresRunawayFlag == "2" && (string.IsNullOrEmpty(w.QresReasons) || string.IsNullOrEmpty(w.QresProcess))).Count > 0)
            {
                MessageBox.Show("失控原因与解决措施不能为空!", "提示");

                return;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", "", "", "");
            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                ProxyObrQcResult proxyQc = new ProxyObrQcResult();

                ListQcResult.AddRange(dvValue.ToArray());

                proxyQc.Service.QcResultAudit(ListQcResult, frmCheck.OperatorID);

                isTrue = true;
                operatorId = frmCheck.OperatorID;
                operatorName = frmCheck.OperatorName;
                this.DialogResult = DialogResult.OK;
            }
        }


    }
}
