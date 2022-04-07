using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using dcl.client.common;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmAudiData : FrmCommon
    {
        public FrmAudiData()
        {
            InitializeComponent();
        }

        public FrmAudiData(List<EntityObrQcResult> dtQcData, AuditType type)
        {
            InitializeComponent();
            dtQcAutiData = EntityManager<EntityObrQcResult>.ListClone(dtQcData);
            if (type == AuditType.Audit)
            {
                gcIsEff.OptionsColumn.AllowEdit = !(UserInfo.GetSysConfigValue("QCAuditMode") == "启用");
                foreach (EntityObrQcResult drQc in dtQcAutiData)
                {
                    drQc.QresDisplay = gcIsEff.OptionsColumn.AllowEdit ? 1 : 0;
                }
            }
            auditType = type;

        }

        private List<EntityObrQcResult> dtQcAutiData;

        private AuditType auditType;

        private void FrmAudiData_Load(object sender, EventArgs e)
        {
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            switch (auditType)
            {
                case AuditType.Audit:
                    dtQcAutiData = dtQcAutiData.FindAll(w => w.QresAuditFlag == 0);
                    sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnQualityAudit.Name });
                    break;
                case AuditType.CancelAudit:
                    dtQcAutiData = dtQcAutiData.FindAll(w => w.QresAuditFlag == 1);
                    sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnQualityData.Name });
                    break;
                case AuditType.TwoAudit:
                    dtQcAutiData = dtQcAutiData.FindAll(w => w.QresAuditFlag == 1 && string.IsNullOrEmpty(w.QresSecondauditUserId));
                    sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnQualityRule.Name });
                    break;
                default:
                    break;
            }
            gcLot.DataSource = dtQcAutiData;

            sysToolBar1.BtnQualityData.Caption = "反审";
            sysToolBar1.BtnQualityRule.Caption = "二次审核";

            List<EntityDicPubEvaluate> listPub = CacheClient.GetCache<EntityDicPubEvaluate>();

            foreach (EntityDicPubEvaluate drBscripe in listPub)
            {
                if (drBscripe.EvaFlag == "5")
                    repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
                if (drBscripe.EvaFlag == "6")
                    repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
            }
        }

        #region CheckBoxOnGridHeader 全选按钮

        void gridViewPatientList_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "gcIsEff")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {


            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;

            info = gridheadercheckbox.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = gridheadercheckbox.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }
        private bool bGridHeaderCheckBoxState = false;
        protected virtual void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {

            Point pt = this.gvLot.GridControl.PointToClient(Control.MousePosition);
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo info = this.gvLot.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "gcIsEff")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gvLot.InvalidateColumnHeader(this.gvLot.Columns["gcIsEff"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            for (int i = 0; i < this.gvLot.RowCount; i++)
            {
                EntityObrQcResult obr = (EntityObrQcResult)gvLot.GetRow(i);
                if (obr != null)
                    obr.QresDisplay = bGridHeaderCheckBoxState ? 0 : 1;
            }
            gcLot.RefreshDataSource();
        }
        #endregion

        private void sysToolBar1_OnBtnQualityDataClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();

            List<EntityObrQcResult> drQC = ((List<EntityObrQcResult>)gcLot.DataSource).FindAll(w => w.QresDisplay == 0 && w.QresAuditFlag == 1);//.Table.Select("qcm_view_flag='0' and qcm_flg='1' ");

            if (drQC.Count <= 0)
            {
                MessageBox.Show("请选择要反审的数据！", "提示");
                return;
            }

            if (!UserInfo.HaveFunction("lis.client.quality.FrmChart", "QCUndoAutiArchive"))
            {
                DateTime dtNow = ServerDateTime.GetServerDateTime().AddMonths(-6);

                List<EntityObrQcResult> listQcArchive = drQC.FindAll(w => w.QresDate > dtNow);

                if (listQcArchive.Count == 0)
                {
                    MessageBox.Show("无权反审已归档数据！", "提示");
                    return;
                }

                drQC = listQcArchive;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", "", "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                List<string> listQresSn = new List<string>();

                foreach (EntityObrQcResult item in drQC)
                {
                    listQresSn.Add(item.QresSn.ToString());
                }


                ProxyObrQcResult proxyQc = new ProxyObrQcResult();
                if (proxyQc.Service.QcResultUndoAudit(listQresSn))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("反审成功！");
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("反审失败！");
                }
            }
        }

        private void sysToolBar1_OnBtnQualityAuditClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            bool isAnti = false;
            string operatorId = null;
            string operatorName = null;
            List<EntityObrQcResult> drnotAuditLot = dtQcAutiData.FindAll(w => w.QresAuditFlag == 0 && w.QresDisplay == 0);
            List<EntityObrQcResult> drLot = dtQcAutiData.FindAll(w => w.QresAuditFlag == 0 && w.QresRunawayFlag == "2" && w.QresDisplay == 0);

            if (drnotAuditLot.Count == 0 && drLot.Count == 0)
            {
                MessageBox.Show("请选择要审核的数据！", "提示");
                return;
            }
            if (drLot.Count > 0)
            {
                FrmControl fra = new FrmControl(drnotAuditLot);
                if (fra.ShowDialog() == DialogResult.OK)
                    isAnti = fra.isTrue;
            }
            else
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", "", "", "");

                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    ProxyObrQcResult proxyQc = new ProxyObrQcResult();
                    proxyQc.Service.QcResultAudit(drnotAuditLot, frmCheck.OperatorID);

                    isAnti = true;
                    operatorId = frmCheck.OperatorID;
                    operatorName = frmCheck.OperatorName;
                }

            }
            if (isAnti)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }

        private void sysToolBar1_OnBtnQualityRuleClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();

            List<EntityObrQcResult> drQC = ((List<EntityObrQcResult>)gcLot.DataSource).FindAll(w => w.QresDisplay == 0);//.Table.Select("qcm_view_flag='0'");

            if (drQC.Count <= 0)
            {
                MessageBox.Show("请选择要二审的数据！", "提示");
                return;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 二审", "", "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                ProxyObrQcResult proxyQc = new ProxyObrQcResult();
                if (proxyQc.Service.QcResultSecondAudit(drQC, frmCheck.OperatorID))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("二审成功！");
                    this.DialogResult = DialogResult.OK;
                }
                else
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("二审失败！");
            }
        }
    }

    public enum AuditType
    {
        Audit,
        CancelAudit,
        TwoAudit
    }
}
