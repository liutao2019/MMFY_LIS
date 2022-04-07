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

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConElisaModControl : ConDicCommon, IBarActionExt
    {
        //public EiasaModControl()
        //{
        //    InitializeComponent();           
        //}

        //public override  void InitParamters()
        //{
        //    this.subTable = "imm_mod";
        //    this.gcSub = gcImmMod;
        //    this.gvSub = gridView1;
        //    this.primaryKeyOfSubTable = "mod_id";
        //    this.bsSub = bsEiasaMod;
        //    barControl1.BarManager = this;
        //}

        //public override void AddActiveCtrls(ref Dictionary<string, bool> controlsList)
        //{
        //    controlsList.Add(gcImmMod.Name, true);
        //}
        public ConElisaModControl()
        {
            InitializeComponent();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }
        private SaveAction saveAction = SaveAction.Unknown;
        List<EntityDicElisaMeaning> listMean = new List<EntityDicElisaMeaning>();

        private void ConElisaModControl_Load(object sender, EventArgs e)
        {
            DoRefresh();
            EnableGridView(false);

        }

        /// <summary>
        /// 可编辑
        /// </summary>
        public virtual bool EnableGridView(bool enable)
        {
            return gridView1.OptionsBehavior.Editable = enable;
        }
        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            EntityDicElisaMeaning dr = (EntityDicElisaMeaning)bsEiasaMod.AddNew();
            EnableGridView(true);
            saveAction = SaveAction.Add;
        }

        public void Save()
        {
            bsEiasaMod.EndEdit();
            if (this.gridView1.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }

            if (saveAction == SaveAction.Add)
            {
                EntityDicElisaMeaning dt = (EntityDicElisaMeaning)bsEiasaMod.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dt);
                EntityResponse result = base.New(request);
            }
            else if (saveAction == SaveAction.Edit)
            {
                List<EntityDicElisaMeaning> dt = bsEiasaMod.DataSource as List<EntityDicElisaMeaning>;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dt);
                EntityResponse result = base.Update(request);
            }
            else
            {
                return;
            }

            if (base.isActionSuccess)
            {
                DoRefresh();
            }
        }

        public void Delete()
        {
            try
            {
                this.bsEiasaMod.EndEdit();
                if (this.gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                EntityDicElisaMeaning calu = (EntityDicElisaMeaning)bsEiasaMod.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(calu);
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        base.Delete(request);
                        break;
                    case DialogResult.Cancel:
                        return;
                }

                if (base.isActionSuccess)
                {
                    listMean.Remove(calu);
                    gridView1.RefreshData();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException("删除出错", ex);
            }
        }

        public void DoRefresh()
        {
            this.isActionSuccess = true;
            EntityRequest request = new EntityRequest();
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                listMean = ds.GetResult() as List<EntityDicElisaMeaning>;
                this.bsEiasaMod.DataSource = listMean;
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gcImmMod.Name, true);
            return dlist;
        }
        public void Cancel()
        {

        }

        public void Edit()
        {
            saveAction = SaveAction.Edit;
            EnableGridView(true);
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
    }
}

