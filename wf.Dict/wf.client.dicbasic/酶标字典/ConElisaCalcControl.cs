using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using DevExpress.XtraEditors;

using dcl.entity;

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConElisaCalcControl : ConDicCommon, IBarActionExt
    {
        public ConElisaCalcControl()
        {
            InitializeComponent();
        }
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }
        private SaveAction saveAction = SaveAction.Unknown;
        List<EntityDicElisaCalu> listCalu = new List<EntityDicElisaCalu>();
        List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();

        private void ConElisaCalcControl_Load(object sender, EventArgs e)
        {
            DoRefresh();
            EnableGridView(false);

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView3, _gridLocalizer);

        }
        private void CalcButton_Click(object sender, EventArgs e)
        {
            if (sender is SimpleButton)
            {
                SimpleButton source = sender as SimpleButton;
                if (source != null)
                {
                    CalcText(source);
                }
            }
        }
        private void CalcText(SimpleButton source)
        {
            TextEditHelper.InsertStringIntoMemoEdit(memoEdit1, source.Tag.ToString());
        }
        /// <summary>
        /// 可编辑
        /// </summary>
        public virtual bool EnableGridView(bool enable)
        {
            return gridView3.OptionsBehavior.Editable = enable;
        }
        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            EntityDicElisaCalu dr = (EntityDicElisaCalu)bsEiasaCalc.AddNew();
            EnableGridView(true);
            saveAction = SaveAction.Add;
        }

        public void Save()
        {
            bsEiasaCalc.EndEdit();
            if (this.gridView3.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }

            if (saveAction == SaveAction.Add)
            {
                EntityDicElisaCalu dt = (EntityDicElisaCalu)bsEiasaCalc.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dt);
                EntityResponse result = base.New(request);
            }
            else if (saveAction == SaveAction.Edit)
            {
                EntityDicElisaCalu dt = (EntityDicElisaCalu)bsEiasaCalc.Current;
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
                this.bsEiasaCalc.EndEdit();
                if (this.gridView3.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                EntityDicElisaCalu calu = (EntityDicElisaCalu)bsEiasaCalc.Current;
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
                    listCalu.Remove(calu);
                    gridView3.RefreshData();
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
                Dictionary<string, object> dict = ds.GetResult() as Dictionary<string, object>;
                object objCalu = dict["Calu"];
                object onjItem = dict["Item"];
                if (objCalu != null)
                {
                    listCalu = objCalu as List<EntityDicElisaCalu>;
                }
                if (onjItem != null)
                {
                    listItem = onjItem as List<EntityDicItmItem>;
                }
                this.bsEiasaCalc.DataSource = listCalu;
                this.bsDictItem.DataSource = listItem;
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gridControl1.Name, true);
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


    }

    #endregion


}


