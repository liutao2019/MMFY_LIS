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
using dcl.root.logon;
using DevExpress.XtraEditors;
using System.Linq;

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConElisaJudgeControl : ConDicCommon, IBarActionExt
    {
        //public EiasaJudgeControl()
        //{
        //    InitializeComponent();
        //}

        //public override void InitParamters()
        //{
        //    this.subTable = "imm_judge";
        //    this.gcSub = gridControl2;
        //    this.gvSub = gridView4;
        //    this.primaryKeyOfSubTable = "imj_id";
        //    this.bsSub = bsEiasaJudge;
        //    barControl1.BarManager = this;
        //}
        public ConElisaJudgeControl()
        {
            InitializeComponent();
        }
        //public override void AddActiveCtrls(ref Dictionary<string, bool> controlsList)
        //{
        //    controlsList.Add(gridControl2.Name, true);
        //}
        //DataTable dtRej = new DataTable("imj_res");
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }
        DataTable dtRej = new DataTable("imj_res");
        private SaveAction saveAction = SaveAction.Unknown;
        private List<EntityDicElisaCriter> listCriter = new List<EntityDicElisaCriter>();
        private List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
        private void ConElisaJudgeControl_Load(object sender, EventArgs e)
        {
            DoRefresh();
            dtRej.Columns.Add("imj_res");
            dtRej.Columns.Add("imj_display");
            dtRej.Rows.Add("pos", "阳性");
            dtRej.Rows.Add("neg", "阴性");
            bsRej.DataSource = dtRej;
            EnableGridView(false);

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView4, _gridLocalizer);
        }

        ///// <summary>
        ///// 重写更新修改状态的需要更新数据源获取
        ///// </summary>
        ///// <returns></returns>
        //protected override DataTable m_dtbGetUpdateData()
        //{
        //    DataTable dtUpdate = ((DataTable)bsSub.DataSource).Copy();

        //    foreach (DataRow dr in dtUpdate.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(otherRequireColumn) && dr[otherRequireColumn].ToString() == "")
        //        {
        //            MessageBox.Show(warnOfOtherRequireColumn, "提示");
        //            throw new Exception();
        //        }
        //    }






        //    return dtUpdate;
        //}

        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            EntityDicElisaCriter dr = (EntityDicElisaCriter)bsEiasaJudge.AddNew();
            EnableGridView(true);
            saveAction = SaveAction.Add;
        }

        public void Save()
        {
            bsEiasaJudge.EndEdit();
            if (this.gridView4.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }

            if (saveAction == SaveAction.Add)
            {
                EntityDicElisaCriter dt = (EntityDicElisaCriter)bsEiasaJudge.Current;
                if (string.IsNullOrEmpty(dt.CriIgnoreNullSam))
                {
                    dt.CriIgnoreNullSam = "0";
                }
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dt);
                EntityResponse result = base.New(request);
            }
            else if (saveAction == SaveAction.Edit)
            {
                List< EntityDicElisaCriter> list = bsEiasaJudge.DataSource as List<EntityDicElisaCriter>;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(list);
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
                this.bsEiasaJudge.EndEdit();
                if (this.gridView4.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                EntityDicElisaCriter criter = (EntityDicElisaCriter)bsEiasaJudge.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(criter);
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
                    listCriter.Remove(criter);
                    gridView4.RefreshData();
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
                object objCriter = dict["Criter"];
                object onjItem = dict["Item"];
                if (objCriter != null)
                {
                    listCriter = objCriter as List<EntityDicElisaCriter>;
                }
                if (onjItem != null)
                {
                    listItem = onjItem as List<EntityDicItmItem>;
                }
                this.bsEiasaJudge.DataSource = listCriter;
                this.bsDictItem.DataSource = listItem.Where(w=>w.ItmDelFlag.Contains("0"));
                EnableGridView(false);
            }
        }

        #endregion

        #region IBarActionExt 成员

        public void Cancel()
        {
            //SetEmbeddedNavigator(false);
            EnableGridView(false);
        }

        public void Edit()
        {
            isAdd = false;
            EnableGridView(true);
            saveAction = SaveAction.Edit;
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

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gridControl2.Name, true);
            return dlist;
        }
        /// <summary>
        /// 可编辑
        /// </summary>
        public virtual bool EnableGridView(bool enable)
        {
            return gridView4.OptionsBehavior.Editable = enable;
        }



    }
}
