using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class FrmPatInfoExt : FrmCommon
    {
        public FrmPatInfoExt()
        {
            InitializeComponent();
            this.Deactivate += FrmPatInfoExt_Deactivate;

            List<EntityDicMicBacteria> dtBacteri = CacheClient.GetCache<EntityDicMicBacteria>();
            List<EntityDicMicBacteria> dtBacteriCopy = EntityManager<EntityDicMicBacteria>.ListClone(dtBacteri);

            List<EntityDicMicAntibio> listAntibio = CacheClient.GetCache<EntityDicMicAntibio>();
            List<EntityDicMicAntibio> listAntibioCopy = EntityManager<EntityDicMicAntibio>.ListClone(listAntibio);

            List<EntityDicMicBacttype> listBType = CacheClient.GetCache<EntityDicMicBacttype>();

            repositoryItemLookUpEdit7.DataSource = dtBacteri;
            relueOldBac.DataSource = dtBacteriCopy;
            relueOldBtype.DataSource = listBType;
            repositoryItemLookUpEdit8.DataSource = listAntibio;
            repositoryItemLookUpEdit9.DataSource = listAntibioCopy;

        }

        private void FrmPatInfoExt_Deactivate(object sender, EventArgs e)
        {
            //  Close();
        }

        public void LoadImage(string patid)
        {
            pnlImage.Visible = true;
            pnlImage.Dock = DockStyle.Fill;
            this.patPhotoList1.LoadPhotos(patid);
        }
        public void LoadImage(List<EntityObrResultImage> listResultoP)
        {
            pnlImage.Visible = true;
            pnlImage.Dock = DockStyle.Fill;
            this.patPhotoList1.LoadPhotos(listResultoP,false);
        }
        public void LoadHistory(string patid, List<EntityObrResult> tabble)
        {
            pnlHistory.Visible = true;
            pnlHistory.Dock = DockStyle.Fill;
            this.patHistory1.dtResultShowDataSort = null;
            this.patHistory1.dtResultShowDataSort = tabble;
            this.patHistory1.LoadPatHistory(patid);
        }
        public void LoadRelateRes(EntityPidReportMain patient)
        {
            pnlRelateRes.Visible = true;
            pnlRelateRes.Dock = DockStyle.Fill;
            this.patRelateResult1.LoadRelateResult(patient);
        }

        public void LoadInfo(string pat_id, List<EntityPidReportDetail> listPatCombine, string barcode)//参数dtPatCombine改为实体之后记得把下面的new List<EntityPidReportDetail>()去掉
        {
            pnlInfo.Visible = true;
            pnlInfo.Dock = DockStyle.Fill;
            if (pat_id != string.Empty)
            {
                this.barcodeSampleInfo1.LoadData(pat_id, listPatCombine, barcode);
            }
            else
            {
                this.barcodeSampleInfo1.Reset();
            }
        }

        public void LoadBacHistory(EntityPidReportMain irow)
        {
            dateEditFrom.EditValue = DateTime.Now.AddDays(-7);
            dateEditTo.EditValue = DateTime.Now;
            pnlBacHistory.Visible = true;
            pnlBacHistory.Dock = DockStyle.Fill;
            dr = irow;
            relueOldBtype.DataSource= CacheClient.GetCache<EntityDicMicBacttype>();
            rlue_pat_no_id.DataSource = CacheClient.GetCache<EntityDicPubIdent>();
            rlue_pat_i_code.DataSource = CacheClient.GetCache<EntityDicCheckPurpose>();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        EntityPidReportMain dr;
        void LoadOldPat()
        {

            EntityPatientQC qc = new EntityPatientQC();
            qc.LessPatDate = dr.RepInDate;
            qc.PidIdtId = dr.PidIdtId;
            qc.PidInNo = dr.PidInNo;
            qc.PidName = dr.PidName;
            qc.ListItrId = new List<string> { dr.RepItrId };
            qc.SamId = dr.PidSamId;
            qc.DateStart = dateEditFrom.DateTime.Date;
            qc.DateEnd = dateEditTo.DateTime.Date.AddDays(1).AddSeconds(-1);
            List<EntityPidReportMain> listPatients = new ProxyPidReportMain().Service.PatientQuery(qc);
            this.bsOldPat.DataSource = listPatients;
        }

        private void gcOldPatients_Click(object sender, EventArgs e)
        {
            EntityPidReportMain dr = this.gridView5.GetFocusedRow() as EntityPidReportMain;

            if (dr != null)
            {
                var resList = new ProxyMicEnter().Service.GetMicPatinentData(dr.RepId);

                if (resList != null && resList.patient != null)
                {
                    this.gridControl7.DataSource = resList.listAnti;
                    this.gridControl6.DataSource = resList.listDesc;
                    this.gcOldBac.DataSource = resList.listBact;
                    gcOldBac.RefreshDataSource();
                    gridControl6.RefreshDataSource();
                    gridControl7.RefreshDataSource();
                    if (resList.listDesc.Count > 0)
                    {
                        panel4.Visible = false;
                        panel5.Visible = true;
                        panel5.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        panel5.Visible = false;
                        panel4.Visible = true;
                        panel4.Dock = DockStyle.Fill;
                    }

                    return;
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            LoadOldPat();
        }

        internal void LoadRelateMergeRes(string p1, string p2, string p3, string p4, string p5, DateTime dateTime)
        {
            pnlMerge.Visible = true;
            pnlMerge.Dock = DockStyle.Fill;
            this.controlResultMerge1.PatDate = dateTime;
            this.controlResultMerge1.ItrID = p5;
            this.controlResultMerge1.LoadData(p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 复制历史结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyRlts_Click(object sender, EventArgs e)
        {
            EntityPidReportMain droldpatinfo = this.gridView5.GetFocusedRow() as EntityPidReportMain;
            if (droldpatinfo != null)
            {
                EntityQcResultList qcResultList = new EntityQcResultList();

                qcResultList =new ProxyMicEnter().Service.GetMicPatinentData(droldpatinfo.RepId);
                if (qcResultList!=null)
                {
                    List<EntityObrResultAnti> list_oldAnti = qcResultList.listAnti;
                    List<EntityObrResultBact> list_oldBac = qcResultList.listBact;
                    List<EntityObrResultDesc> list_oldCs = qcResultList.listDesc;
                    if ((list_oldCs != null && list_oldCs.Count > 0) || (list_oldAnti != null && list_oldAnti.Count > 0))
                    {
                        if (dr == null)
                        {
                            return;
                        }

                        if (dr.RepStatus.ToString() == "0")
                        {
                            string strMsgTemp = string.Format("当前:样本号[{0}];姓名[{1}]\r\n已有的结果将被覆盖,是否继续复制？", dr.RepSid, dr.PidName);
                            if (lis.client.control.MessageDialog.Show(strMsgTemp, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                               bool result= new ProxyMicEnter().Service.CopyHistroyResult(qcResultList, dr.RepId, dr.RepItrId,dr.RepSid);
                                if (result)
                                {
                                    lis.client.control.MessageDialog.Show("复制成功");
                                }
                            }
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show("当前报告已审核！不能再复制！");
                            return;
                        }
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("没有可复制的历史数据！");
                        return;
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("没有可复制的历史数据！");
                    return;
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要复制的历史数据！");
                return;
            }
        }
    }
}
