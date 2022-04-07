using dcl.client.cache;
using dcl.client.common;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wf.client.reagent
{
    public partial class FrmPurDetail : FrmCommon
    {
        public FrmPurDetail()
        {
            InitializeComponent();
        }
        public List<EntityReaPurchaseDetail> detailList = new List<EntityReaPurchaseDetail>();
        public List<EntityReaPurchaseDetail> selectDataRows = new List<EntityReaPurchaseDetail>();
        private bool ReagentInPriceGreat0;
        private void FrmPurDetail_Load(object sender, EventArgs e)
        {
            List<EntityDicPubEvaluate> listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>();
            ctlRepositoryItemLookUpEdit1.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "26").OrderBy(i => i.EvaSortNo).ToList();
            ctlRepositoryItemLookUpEdit2.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "27").OrderBy(i => i.EvaSortNo).ToList();
            ctlRepositoryItemLookUpEdit3.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "25").OrderBy(i => i.EvaSortNo).ToList();
            //ctlRepositoryItemLookUpEdit1.DisplayMember = ctlRepositoryItemLookUpEdit2.DisplayMember =
            //    ctlRepositoryItemLookUpEdit3.DisplayMember = "合格";
            ReagentInPriceGreat0 = ConfigHelper.GetSysConfigValueWithoutLogin("ReagentInPriceGreat0") == "是";
            if (detailList != null && detailList.Count > 0)
            {
                EntityReaQC qc = new EntityReaQC();
                qc.PurNo = detailList[0].Rpcd_no;

                List<EntityReaStorageDetail> stoList = new ProxyReaStorageDetail().Service.GetDetail(qc);
                foreach (var item in detailList)
                {
                    if (stoList != null && stoList.Count > 0)
                    {
                        List<EntityReaStorageDetail> list = new List<EntityReaStorageDetail>();
                        list = stoList.Where(m => m.Rsd_reaid == item.Rpcd_reaid).ToList<EntityReaStorageDetail>();
                        item.HasStoreCount = list.Count;
                    }
                    item.Report = "合格";
                    item.Temparate = "合格";
                    item.OutPackage = "合格";
                    item.EvaContent = "合格";
                    item.StoreCount = item.Rpcd_reacount;
                }
                

                this.bsDetail.DataSource = detailList;
                gridControl1.RefreshDataSource();
                gridView1.RefreshData();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            selectDataRows.Clear();
            int[] selectedRowHandler = gridView1.GetSelectedRows();

            if (selectedRowHandler.Length == 0)
            {
                lis.client.control.MessageDialog.Show("没有选中任何记录", "提示");
                return;
            }

            foreach (int rowHandler in selectedRowHandler)
            {
                EntityReaPurchaseDetail row = gridView1.GetRow(rowHandler) as EntityReaPurchaseDetail;
                if (row != null)
                {
                    selectDataRows.Add(row);
                }
            }
            foreach (var item in selectDataRows)
            {
                if (item.HasStoreCount + item.StoreCount > item.Rpcd_reacount)
                {
                    DialogResult diaRv = lis.client.control.MessageDialog.Show($"{item.ReagentName}入库数已经大于采购数，是否修改？", "提示",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    if (diaRv == DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (!CheckItems())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool CheckItems()
        {
            bool check = false;
            foreach (var item in selectDataRows)
            {
                if (string.IsNullOrEmpty(item.BatchNo))
                {
                    MessageDialog.Show("批号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (ReagentInPriceGreat0 && item.Rpcd_price <= 0)
                {
                    lis.client.control.MessageDialog.Show("价格不能为0！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.OutPackage))
                {
                    MessageDialog.Show("外包装不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Temparate))
                {
                    MessageDialog.Show("到达温度不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Report))
                {
                    MessageDialog.Show("检验报告不能为空！", "提示");
                    check = false;
                    break;
                }
                if (item.ValidDate < DateTime.Now)
                {
                    MessageDialog.Show("有效日期不能小于今天！", "提示");
                    check = false;
                    break;
                }
                if (item.StoreCount <= 0)
                {
                    MessageDialog.Show("入库数量不能小于0！", "提示");
                    check = false;
                    break;
                }
                check = true;
            }
            return check;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
