using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.common;
using DevExpress.XtraGrid.Columns;
using lis.client.control;
using System.Linq;
using dcl.entity;

namespace dcl.client.dicbasic
{
    public partial class ConIceBox : ConDicCommon, IBarActionExt
    {
        ///// <summary>
        ///// 是否为新增事件
        ///// </summary>
        bool blIsNew = false;
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private List<EntityDicSampStore> list = new List<EntityDicSampStore>();
        private List<EntityDicSampStore> listComp = new List<EntityDicSampStore>();
        private List<EntityDicSampStoreStatus> status = new List<EntityDicSampStoreStatus>();

        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;

            foreach(GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            
            this.txtCode.Focus();
            btnPY.Properties.ReadOnly = true;
            btnWB.Properties.ReadOnly = true;

            EntityDicSampStore depart = (EntityDicSampStore)bsIceBox.AddNew();
            depart.StoreId = string.Empty;

            bsIceBox.EndEdit();
            bsIceBox.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (this.txtCode.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("冰箱名称不能为空", "提示信息");
                return;
            }

            this.bsIceBox.EndEdit();
            if (bsIceBox.Current == null)
            {
                return;
            }

            string dh_id = selectDicHarvester1.valueMember;
            foreach (EntityDicSampStore item in listComp)
            {
                if (item.StoreDhId == dh_id)
                {
                    MessageBox.Show("该冰箱被占用");
                    return;
                }
            }

            EntityRequest request = new EntityRequest();

            EntityDicSampStore store = (EntityDicSampStore)bsIceBox.Current;

            String itm_id = store.StoreId;

            request.SetRequestValue(store);

            EntityResponse result = new EntityResponse();
            if (itm_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (itm_id == "")
                {
                    store.StoreId = result.GetResult<EntityDicSampStore>().StoreId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //如果是新增事件则重新赋值过滤信息
            if (blIsNew)
            {
                blIsNew = false;//取消新增标志
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.bsIceBox.EndEdit();
            if (bsIceBox.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicSampStore store = (EntityDicSampStore)bsIceBox.Current;

            request.SetRequestValue(store);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult== DialogResult.OK)
            {
                    base.Delete(request);
                    if (base.isActionSuccess)
                    {
                        MessageDialog.ShowAutoCloseDialog("删除成功");
                        DoRefresh();
                    }
                    else
                    {
                        MessageDialog.ShowAutoCloseDialog("删除失败");
                    }
            }
            else
            {

            }
        }

        public void DoRefresh()
        {
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = result.GetResult() as List<EntityDicSampStore>;
                //深度复制  
                listComp = EntityManager<EntityDicSampStore>.ListClone(list);
                bsIceBox.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);

            return dlist;
        }
        #endregion
        
        public ConIceBox()
        {
            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            BindDataStatus();
            setGridControl();
        }

        public void BindDataStatus()
        {
            EntityResponse result = base.Other(new EntityRequest());
            if (isActionSuccess)
            {
                status = result.GetResult() as List<EntityDicSampStoreStatus>;
                bsStatus.DataSource = status;
            }
        }
        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        private void initData()
        {
            this.DoRefresh();

            //过滤掉已经停用的项目(下拉控件)
            selectDicHarvester1.SetFilter(selectDicHarvester1.getDataSource().FindAll(w => w.DelFlag == 0).ToList());
            selectDicTemperature1.SetFilter(selectDicTemperature1.getDataSource().FindAll(w => w.DelFlag == "0").ToList());

        }


        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsIceBox.Current != null)
            {
                EntityDicSampStore store = (EntityDicSampStore)bsIceBox.Current;
                store.StorePyCode = tookit.GetSpellCode(this.txtName.Text);
                store.StoreWbCode = tookit.GetWBCode(this.txtName.Text);
            }
        }

        private void btnDM_EditValueChanged(object sender, EventArgs e)
        {

        }
        

        #region IBarActionExt 成员

        void IBarActionExt.Cancel()
        {
            //如果是新增事件则重新赋值过滤信息
            if (blIsNew)
            {
                //dep_class.FilterInfo = cfiClass;
                //dep_code.FilterInfo = cfiCode;
                //dep_id.FilterInfo = cfiId;
                //dep_incode.FilterInfo = cfiInCode;
                //dep_name.FilterInfo = cfiName;
                //colori_name.FilterInfo = cfiOriType;
                //coldep_select_code.FilterInfo = cfiSelectCode;
                blIsNew = false;//取消新增标志
            }
        }

        void IBarActionExt.Close()
        {
            
        }

        void IBarActionExt.Edit()
        {
            this.gridControl1.Enabled = false;
        }

        void IBarActionExt.MoveNext()
        {
            
        }

        void IBarActionExt.MovePrev()
        {
            
        }

        #endregion
        
    }
}
