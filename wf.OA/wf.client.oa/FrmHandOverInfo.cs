using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;

using dcl.client.wcf;
using System.Reflection;
using dcl.client.common;

using dcl.client.report;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.oa
{
    public partial class FrmHandOverInfo : FrmCommon
    {
        public FrmHandOverInfo()
        {
            InitializeComponent();
        }
        ucHandOverLayout ucHandOverLayout1;

        string hoID = string.Empty;

        public bool IsManue { get; internal set; }

        public EntityHoRecord info;

        private void FrmHandOverInfo_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[]
                {
                    sysToolBar1.BtnResultJudge.Name,
                    sysToolBar1.BtnPrint.Name,
                    sysToolBar1.BtnDeSpe.Name,
                    "BtnClose"
                });
            sysToolBar1.BtnResultJudge.Caption = "交班确认";
            sysToolBar1.BtnDeSpe.Caption = "交接标本登记";
            ucHandOverLayout1 = new ucHandOverLayout();

            panelLayout.Controls.Add(ucHandOverLayout1);
            ucHandOverLayout1.Dock = DockStyle.Fill;
            ucHandOverLayout1.SetLayout(LocalSetting.Current.Setting.CType_id);
            ucHandOverLayout1.Init();
            if(info!=null)
            {
                ucHandOverLayout1.BindToUI(info);
                sysToolBar1.BtnResultJudge.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            this.Text = string.Format("检验{0}交接班", LocalSetting.Current.Setting.CType_name);

            List<EntityDicPubEvaluate> listPubEve = CacheClient.GetCache<EntityDicPubEvaluate>();

            ucHandOverLayout1.SetDictData(listPubEve);
        }


        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (!IsManue)
            {
                if (lis.client.control.MessageDialog.Show("您确定要退出交班确认界面吗？", "确认", MessageBoxButtons.YesNo) !=
                       DialogResult.Yes)
                {
                    return;
                }
            }
            Close();
        }

        private void sysToolBar1_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            FrmCheckPassword frm = new FrmCheckPassword();

            frm.Text = "交班签名";

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ucHandOverLayout1.SetHandConfirm(frm.OperatorID);
            FrmCheckPassword frm2 = new FrmCheckPassword();

            frm2.Text = "接班签名";

            if (frm2.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ucHandOverLayout1.SetReciveConfirm(frm2.OperatorID);

            EntityHoRecord info = ucHandOverLayout1.GetDataFormUI();
            hoID = Guid.NewGuid().ToString();
            info.HrId = hoID;
            info.IsNew = true;
            info.HrTypeId = LocalSetting.Current.Setting.CType_id;


            new ProxyOaHoRecord().Service.UpdateHandoverInfo(info);

            if (IsManue)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void sysToolBar1_OnBtnPrintClicked_1(object sender, EventArgs e)
        {
            EntityDCLPrintParameter para = new EntityDCLPrintParameter();
            para.ReportCode = "HandOverReport";
            para.CustomParameter.Add("HrId", hoID);
            try
            {
                DCLReportPrint.Print(para);
            }
            catch (ReportNotFoundException ex)
            {
                lis.client.control.MessageDialog.Show("报表代码为[HandOverReport]的报表不存在！");
            }
            catch (Exception ex2)
            {
                lis.client.control.MessageDialog.Show(ex2.ToString());
            }
        }

        private void sysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            EntityDCLPrintParameter para = new EntityDCLPrintParameter();
            para.ReportCode = "HandOverReport";
            para.CustomParameter.Add("HrId", hoID);
            try
            {
                DCLReportPrint.PrintPreview(para);
            }
            catch (ReportNotFoundException ex)
            {
                lis.client.control.MessageDialog.Show("报表代码为[HandOverReport]的报表不存在！");
            }
            catch (Exception ex2)
            {
                lis.client.control.MessageDialog.Show(ex2.ToString());
            }
        }

        /// <summary>
        /// 交接标本登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnDeSpeClick(object sender, EventArgs e)
        {
            if (ucHandOverLayout1 != null && ucHandOverLayout1.dtNullRes != null && ucHandOverLayout1.dtNullRes.Rows.Count > 0)
            {
                FrmHandOverNullRes frmhandNullRes = new FrmHandOverNullRes(!ucHandOverLayout1.SaveDtNullRes, false, ucHandOverLayout1.dtNullRes);
                frmhandNullRes.btnSaveNullResData += new FrmHandOverNullRes.degSaveNullResData(frmhandNullRes_btnSaveNullResData);
                frmhandNullRes.ShowDialog();
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("没有可以登记的标本信息!");
            }
        }

        /// <summary>
        /// 保存未登记信息
        /// </summary>
        /// <param name="dt"></param>
        void frmhandNullRes_btnSaveNullResData(DataTable dt)
        {
            ucHandOverLayout1.dtNullRes = dt;
            ucHandOverLayout1.SaveDtNullRes = true;
        }
    }
}
