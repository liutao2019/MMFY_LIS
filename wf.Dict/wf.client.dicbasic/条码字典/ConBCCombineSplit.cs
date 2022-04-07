using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;

using dcl.client.frame;
using DevExpress.XtraGrid.Views.Grid;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    [DesignTimeVisible(false)]
    public partial class ConBCCombineSplit : ConDicCommon, IBarActionExt
    {
        public ConBCCombineSplit()
        {
            InitializeComponent();
            DoRefresh();
            this.Load += FrmCombineSplit_Load;
        }

        private void FrmCombineSplit_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            FrmDictMainDev parentForm;
            parentForm = this.GetParentForm();
            if (parentForm != null)
            {
                parentForm.ModifyButton();
            }
        }
        private FrmDictMainDev GetParentForm()
        {
            Control parent = this.Parent;
            while (parent != null)
            {
                if (parent is FrmDictMainDev)
                {
                    return parent as FrmDictMainDev;
                }

                parent = parent.Parent;
            }
            return null;
        }

        private List<EntityDicSampDivergeRule> dtItemIn = new List<EntityDicSampDivergeRule>();
        private List<EntityDicSampDivergeRule> dtItemNotIn = new List<EntityDicSampDivergeRule>();
        private List<EntityDicCombine> dtCombine = new List<EntityDicCombine>();
        private DataTable dtAll = new DataTable();

        public void Cancel()
        {
            SetButton(false);
            DoRefresh();
        }

        private void getTable()
        {
            if (bsCombine.Current != null)
            {
                EntityDicCombine dr = (EntityDicCombine)(bsCombine.Current);
                this.initDetail(dr.ComId.ToString());
            }
            else
                this.initDetail("0");
        }

        private void initDetail(String comId)
        {
            if (comId == "")
            {
                this.dtItemNotIn.Clear();
                this.dtItemIn.Clear();
            }

            EntityDicSampDivergeRule dt =new EntityDicSampDivergeRule();
            dt.ComId = comId;
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dt);
            EntityResponse ItemNotIn = new EntityResponse();
            EntityResponse ItemIn = new EntityResponse();
            ItemNotIn = base.Other(request);
            ItemIn = base.Search(request);
            if (isActionSuccess)
            {
                dtItemNotIn = ItemNotIn.GetResult() as List<EntityDicSampDivergeRule>;
                foreach(var combine in list)
                {
                    dtItemNotIn = dtItemNotIn.Where(i => i.ComSplitId != combine.ComId).ToList();
                }
                this.bsItemNotIn.DataSource = dtItemNotIn;
                this.bsItemIn.DataSource = dtItemIn = ItemIn.GetResult() as List<EntityDicSampDivergeRule>;
                gcItemIn.DataSource = dtItemIn;
                gcItemNotIn.DataSource = dtItemNotIn;
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        private void btnAddAllCombines_Click(object sender, EventArgs e)
        {
            List<EntityDicSampDivergeRule> combineNotIn = dtItemNotIn;
            if (combineNotIn.Count > 0)
            {
                List<EntityDicSampDivergeRule> combineIn = dtItemIn;
                foreach (EntityDicSampDivergeRule itr in combineNotIn)
                {
                    combineIn.Add(itr);
                }
                gcItemNotIn.DataSource = new List<EntityDicSampDivergeRule>();
                dtItemNotIn = new List<EntityDicSampDivergeRule>();
                dtItemIn = combineIn;
                gcItemIn.DataSource = combineIn;
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        private void btnAddCombines_Click(object sender, EventArgs e)
        {
            List<EntityDicSampDivergeRule> combineNotIn = dtItemNotIn;
            EntityDicSampDivergeRule combineNotInRow = gvItemNotIn.GetFocusedRow() as EntityDicSampDivergeRule;
            if (combineNotIn != null)
            {
                List<EntityDicSampDivergeRule> combineIn = dtItemIn;
                combineIn.Add(combineNotInRow);
                combineNotIn.Remove(combineNotInRow);
                dtItemIn = combineIn;
                dtItemNotIn = combineNotIn;
                gcItemNotIn.DataSource = combineNotIn;
                gcItemIn.DataSource = combineIn;
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        private void btnDelCombines_Click(object sender, EventArgs e)
        {
            List<EntityDicSampDivergeRule> combineNotIn = dtItemNotIn;
            EntityDicSampDivergeRule combineInRow = gvItemIn.GetFocusedRow() as EntityDicSampDivergeRule;
            if (combineNotIn != null)
            {
                List<EntityDicSampDivergeRule> combineIn = dtItemIn;
                combineIn.Remove(combineInRow);
                combineNotIn.Add(combineInRow);
                dtItemIn = combineIn;
                dtItemNotIn = combineNotIn;
                gcItemNotIn.DataSource = combineNotIn;
                gcItemIn.DataSource = combineIn;
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        private void btnDelAllCombines_Click(object sender, EventArgs e)
        {
            List<EntityDicSampDivergeRule> combineIn = dtItemIn;
            if (combineIn.Count > 0)
            {
                List<EntityDicSampDivergeRule> combineNotIn = dtItemNotIn;
                foreach (EntityDicSampDivergeRule itr in combineIn)
                {
                    combineNotIn.Add(itr);
                }
                gcItemIn.DataSource = new List<EntityDicSampDivergeRule>();
                dtItemIn = new List<EntityDicSampDivergeRule>();
                dtItemNotIn = combineNotIn;
                gcItemNotIn.DataSource = combineNotIn;
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        private void gcItemNotIn_DoubleClick(object sender, EventArgs e)
        {
            btnAddCombines_Click(sender,e);
        }

        private void gcItemNotIn_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (this.gvItemNotIn.SelectedRowsCount > 0)
            {
                this.gcItemNotIn.DoDragDrop(new object[] { this.gvItemNotIn.GetSelectedRows(), "notin" }, DragDropEffects.All);
            }
        }

        private void gcItemNotIn_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                //this.moveSelectItem(dtItemIn, dtItemNotIn, gvItemIn, gvItemNotIn, "");
            }
        }

        private void gcItemNotIn_DragOver(object sender, DragEventArgs e)
        {
            object[] obj = e.Data.GetData(typeof(object[])) as object[];
            if (obj != null && obj[1].ToString() == "in")
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void gcItemIn_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (this.gvItemIn.SelectedRowsCount > 0)
            {
                this.gcItemIn.DoDragDrop(new object[] { this.gvItemIn.GetSelectedRows(), "in" }, DragDropEffects.All);
            }
        }

        private void gcItemIn_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                //this.moveSelectItem(dtItemNotIn, dtItemIn, gvItemNotIn, gvItemIn, "");

            }
        }

        private void gcItemIn_DragOver(object sender, DragEventArgs e)
        {
            object[] obj = e.Data.GetData(typeof(object[])) as object[];
            if (obj != null && obj[1].ToString() == "notin")
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void gcItemIn_DoubleClick(object sender, EventArgs e)
        {
            btnDelCombines_Click(sender,e);

        }

        public void Edit()
        {
            SetButton(true);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }

        public void Add()
        {
            return;
        }

        public void Save()
        {
            this.bsCombine.EndEdit();

            if (bsCombine.Current == null)
            {
                return;
            }

            this.bsItemIn.EndEdit();
            EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
            String com_id = dr.ComId.ToString();
            List<EntityDicSampDivergeRule> SampRule = dtItemIn as List<EntityDicSampDivergeRule>;
            EntityRequest request = new EntityRequest();
            EntityResponse result = new EntityResponse();
            if (SampRule.Count == 0)
            {
                EntityDicSampDivergeRule samp = new EntityDicSampDivergeRule();
                samp.ComId = com_id;
                request.SetRequestValue(samp);

                result = base.Delete(request);
            }
            else
            {
                foreach (var samp in SampRule)
                {
                    samp.ComId = com_id;
                }
                request.SetRequestValue(SampRule);

                result = base.New(request);
            }
            if (isActionSuccess)
            {
                bsCombine.ResetCurrentItem();
                getTable();
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
        private List<EntityDicCombine> list = new List<EntityDicCombine>();
        public void DoRefresh()
        {
            SetButton(false);
            EntityResponse ds = base.View(new EntityRequest());
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicCombine>;
                bsCombine.DataSource = list;
            }
            getTable();

        }

        private void SetButton(bool btn)
        {
            btnAddAllUser.Enabled = btn;
            btnAddUser.Enabled = btn;
            btnDelAllUser.Enabled = btn;
            btnDelUser.Enabled = btn;
        }
        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        private void bsCombine_CurrentChanged(object sender, EventArgs e)
        {
            getTable();
        }
    }
}
