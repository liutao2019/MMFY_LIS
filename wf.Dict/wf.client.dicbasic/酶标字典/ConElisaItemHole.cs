using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;

using dcl.entity;
using lis.client.control;
using dcl.client.wcf;

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConElisaItemHole : ConDicCommon, IBarActionExt
    {
        public ConElisaItemHole()
        {
            InitializeComponent();
            DoRefresh();
            BindingResultType();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }

        public void Add()
        {
            this.gridColumn2.OptionsColumn.AllowEdit = true;
            this.gridColumn3.OptionsColumn.AllowEdit = true;
            this.gridColumn4.OptionsColumn.AllowEdit = true;
            this.gridColumn5.OptionsColumn.AllowEdit = true;
            this.gridColumn6.OptionsColumn.AllowEdit = true;
            EntityDicElisaItem dr = (EntityDicElisaItem)bsEiasaItem.AddNew();
        }

        public void Cancel()
        {
            DoRefresh();
        }

        public void Close()
        {
        }

        public void Delete()
        {
            this.bsEiasaItem.EndEdit();
            if (bsEiasaItem.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicElisaItem dr = (EntityDicElisaItem)bsEiasaItem.Current;
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    this.DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
        }
        private List<EntityDicElisaSort> Sortlist = new List<EntityDicElisaSort>();
        private List<EntityDicElisaStatus> Stalist = new List<EntityDicElisaStatus>();
        private List<EntityDicElisaItem> Itemlist = new List<EntityDicElisaItem>();
        public void DoRefresh()
        {
            ProxyCacheData proxy = new ProxyCacheData();
            List<EntityDicItmItem> DicItem = new List<EntityDicItmItem>();
            EntityResponse response = proxy.Service.GetCacheData("EntityDicItmItem");
            DicItem = response.GetResult() as List<EntityDicItmItem>;
            bsDictItem.DataSource = DicItem;

            EntityDicElisaSort dtSort = new EntityDicElisaSort();
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dtSort);
            EntityResponse dsSort = base.Other(request);

            EntityDicElisaStatus dtSta = new EntityDicElisaStatus();
            EntityRequest requestSta = new EntityRequest();
            request.SetRequestValue(dtSta);
            EntityResponse dsSta = base.View(requestSta);

            EntityDicElisaStatus dtItem = new EntityDicElisaStatus();
            EntityRequest requestItem = new EntityRequest();
            request.SetRequestValue(dtItem);
            EntityResponse dsItem = base.Search(requestItem);

            if (isActionSuccess)
            {
                Sortlist = dsSort.GetResult() as List<EntityDicElisaSort>;
                bsHoleMode.DataSource = Sortlist;
                Stalist = dsSta.GetResult() as List<EntityDicElisaStatus>;
                bsHoleStatus.DataSource = Stalist;
                Itemlist = dsItem.GetResult() as List<EntityDicElisaItem>;
                bsEiasaItem.DataSource = new List<EntityDicElisaItem>();
                bsEiasaItem.DataSource = Itemlist;
            }
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowEdit = false;
        }

        public void Edit()
        {
            this.gridColumn2.OptionsColumn.AllowEdit = true;
            this.gridColumn3.OptionsColumn.AllowEdit = true;
            this.gridColumn4.OptionsColumn.AllowEdit = true;
            this.gridColumn5.OptionsColumn.AllowEdit = true;
            this.gridColumn6.OptionsColumn.AllowEdit = true;
            sampleControl1.Enabled = false;
            sampleControl2.Enabled = false;
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gcItemHole", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.bsEiasaItem.EndEdit();
            if (bsEiasaItem.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityResponse result = new EntityResponse();
            List<EntityDicElisaItem> dt = bsEiasaItem.DataSource as List<EntityDicElisaItem>;
            foreach (var dr in dt)
            {
                String sort_id = dr.IplateId;
                request.SetRequestValue(dr);
                if (sort_id == "" || sort_id == null)
                {
                    result = base.New(request);
                }
                else
                {
                    result = base.Update(request);
                }

            }
            if (base.isActionSuccess)
            {
                lis.client.control.MessageDialog.Show("保存成功", "提示信息");
                DoRefresh();
            }
            else
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }
        }

        /// <summary>
        /// 绑定项目酶标保存类型控件数据
        /// </summary>
        private void BindingResultType()
        {
            #region 绑定项目酶标保存类型控件数据

            DataTable dtResultType = new DataTable();
            dtResultType.Columns.Add("colCode", typeof(string));
            dtResultType.Columns.Add("colCou", typeof(string));
            DataRow dr = dtResultType.NewRow();
            dr["colCode"] = 0;
            dr["colCou"] = "全部保存(定性结果+OD值结果)";
            dtResultType.Rows.Add(dr);
            dr = dtResultType.NewRow();
            dr["colCode"] = 1;
            dr["colCou"] = "只保存定性结果";
            dtResultType.Rows.Add(dr);
            dr = dtResultType.NewRow();
            dr["colCode"] = 2;
            dr["colCou"] = "只保存OD值结果";
            dtResultType.Rows.Add(dr);
            this.repositoryItemLookUpEdit1.DataSource = dtResultType;
            this.repositoryItemLookUpEdit1.DisplayMember = "colCou";
            this.repositoryItemLookUpEdit1.ValueMember = "colCode";
            #endregion
        }

        private void bsEiasaItem_CurrentChanged(object sender, EventArgs e)
        {
            this.bsEiasaItem.EndEdit();
            if(bsEiasaItem.Current == null)
            {
                return;
            }
            EntityDicElisaItem dr = (EntityDicElisaItem)bsEiasaItem.Current;
            foreach(var sort in Sortlist)
            {
                if(sort.SortId == dr.IplateSortId)
                {
                    sampleControl1.FormatHoleValues = sort.SortHoleSorting;
                }
            }
            foreach (var status in Stalist)
            {
                if (status.StaId == dr.IplateStaId)
                {
                    sampleControl2.FormatHoleValues = status.StaHoleStaus;
                }
            }
        }
    }
}
