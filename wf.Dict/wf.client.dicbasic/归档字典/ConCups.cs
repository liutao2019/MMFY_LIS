using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.entity;
using lis.client.control;
using System.Linq;
using DevExpress.XtraGrid.Columns;

namespace dcl.client.dicbasic
{
    public partial class ConCups : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员

        public void Add()
        {
            this.bsCups.EndEdit();
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            EntityDicSampStoreArea dr = (EntityDicSampStoreArea)bsCups.AddNew();
            EntityRequest request = new EntityRequest();
            EntityDicSampStore dr1 = (EntityDicSampStore)bsIceBox.Current;
            if (dr1 != null)
            {
                dr.StoreId = dr1.StoreId;
            }
            else {
                MessageDialog.Show("请先设置冰箱!", "提示信息");
            }
            gridControl2.Enabled = false;
            SetEnableEdit(true);
        }

        public void Save()
        {
            this.bsCups.EndEdit();
            if (bsIceBox.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicSampStoreArea area = (EntityDicSampStoreArea)bsCups.Current;
            String type_id = area.AreaId;

            request.SetRequestValue(area);
            EntityResponse result = new EntityResponse();
            if (type_id == null)
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }

            if (base.isActionSuccess)
            {
                if (type_id == null)
                {
                    area.AreaId = result.GetResult<EntityDicSampStoreArea>().AreaId;
                }
                MessageDialog.Show("保存成功", "提示信息");
                DoRefresh();
                SetEnableEdit(false);
            }
            else
            {
                MessageDialog.Show("保存失败", "提示信息");
                throw new Exception(result.ErroMsg);
            }
        }

        public void Delete()
        {
            this.bsCups.EndEdit();
            if (bsCups.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicSampStoreArea dr = (EntityDicSampStoreArea)bsCups.Current;
            String br_id = dr.AreaId;

            request.SetRequestValue(dr);
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

        public void DoRefresh()
        {
            EntityRequest request = new EntityRequest();
            EntityDicSampStore dr = new EntityDicSampStore();
            request.SetRequestValue(dr);
            EntityResponse result = base.View(request);
            if (isActionSuccess)
            {
                IceBoxlist = result.GetResult() as List<EntityDicSampStore>;
                this.bsIceBox.DataSource = IceBoxlist;
            }
            this.bsIceBox.EndEdit();
            BindingData();
        }
        public void BindingData()
        {
            EntityRequest request = new EntityRequest();
            EntityDicSampStore Itrdr = (EntityDicSampStore)bsIceBox.Current;
            if (Itrdr == null)
                return;
            EntityDicSampStoreArea area = new EntityDicSampStoreArea();
            area.StoreId = Itrdr.StoreId;
            request.SetRequestValue(area);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicSampStoreArea>;

                this.bsCups.DataSource = list.Where(i => i.StoreId == Itrdr.StoreId).ToList();
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl2", true);
            dlist.Add("gridControl1", true);
            return dlist;
        }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicSampStoreArea> list = new List<EntityDicSampStoreArea>();
        private List<EntityDicSampStore> IceBoxlist = new List<EntityDicSampStore>();
        private List<EntityDicSampStoreStatus> statusList = new List<EntityDicSampStoreStatus>();

        public ConCups()
        {
            InitializeComponent();
            BindDataStatus();


        }

        public void BindDataStatus()
        {
            EntityResponse result = base.Other(new EntityRequest());
            if (isActionSuccess)
            {
                statusList = result.GetResult() as List<EntityDicSampStoreStatus>;
                bsStatus.DataSource = statusList;
            }
        }

        private void ConCups_Load(object sender, EventArgs e)
        {
            this.DoRefresh();
            SetEnableEdit(false);
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        
        public void SetEnableEdit(bool isEdit)
        {
            gridView1.OptionsBehavior.Editable = isEdit;
        }

        #region IBarActionExt 成员

        public void Cancel()
        {
            SetEnableEdit(false);
        }

        void IBarActionExt.Edit()
        {
            this.gridControl2.Enabled = false;
            SetEnableEdit(true);
        }

        public void Close()
        {

        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion

        private void bsIceBox_CurrentChanged(object sender, EventArgs e)
        {
            BindingData();
        }
    }
}
