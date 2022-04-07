using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Configuration;
using dcl.entity;
using lis.client.control;

namespace dcl.client.dicbasic
{
    public partial class ConIcdCombine : ConDicCommon, IBarActionExt
    {

        private List<EntityDicCombine> ItemIn = new List<EntityDicCombine>();
        private List<EntityDicCombine> ItemNotIn = new List<EntityDicCombine>();
        private List<EntityDicCombine> AllCombines = new List<EntityDicCombine>();
        private List<EntityDicPubIcd> list = new List<EntityDicPubIcd>();
        public ConIcdCombine()
        {
            InitializeComponent();
        }

        private void ConIcdCombine_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gvIcd, _gridLocalizer);
            Hans_GridHelper.HansButtonText(gvItemNotIn, _gridLocalizer);
            DoRefresh();
        }

        public void Add()
        {

        }

        public void Cancel()
        {

        }

        public void Close()
        {

        }

        public void Delete()
        {

        }

        public void DoRefresh()
        {
            gcItemNotIn.Enabled = false;
            gcItemIn.Enabled = false;
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            List<EntityDicCombine> listComb = new List<EntityDicCombine>();
            LoadAllCombine(hosID);
            LoadIcd();
        }

        private void LoadAllCombine(string hosID)
        {
            EntityResponse response = new ProxyCommonDic("svc.ConCombine").Search(new EntityRequest());
            this.isActionSuccess = true;
            if (isActionSuccess)
            {
                AllCombines = response.GetResult() as List<EntityDicCombine>;
            }
        }

        /// <summary>
        /// 加载icd
        /// </summary>
        private void LoadIcd()
        {
            EntityResponse response = new ProxyCommonDic("svc.ConDiagnos").Search(new EntityRequest());
            //EntityResponse response = base.Search(new EntityRequest());
            this.isActionSuccess = true;
            if (isActionSuccess)
            {
                GetViewColumnFilterInfo();
                list = response.GetResult() as List<EntityDicPubIcd>;
                this.bsIcd.DataSource = list;
                SetViewColumnFilterInfo();
            }
        }

        private void SetViewColumnFilterInfo()
        {
            for (int j = 0; j < filter.Length; j++)
            {
                if (filter[j] != null)
                    gvIcd.SortInfo.View.ActiveFilter.Add(filter[j]);
            }

            filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];
        }

        DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[] filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];
        private void GetViewColumnFilterInfo()
        {
            DevExpress.XtraGrid.Views.Base.ViewFilter xxx = gvIcd.SortInfo.View.ActiveFilter;

            for (int i = 0; i < xxx.Count; i++)
            {
                filter[i] = xxx[i];
            }
            xxx.Clear();
        }

        public void Edit()
        {

        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gcIcd", true);
            dlist.Add("panel1", true);
            dlist.Add("txtFilterIcd", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        public void Save()
        {
            this.isActionSuccess = false;
            this.bsIcd.EndEdit();

            if (bsIcd.Current == null)
                return;

            EntityDicPubIcd cur = bsIcd.Current as EntityDicPubIcd;
            string icd_id = "";
            if (cur != null)
            {
                icd_id = cur.IcdId;
            }
            //先将数据库中的该icd_id全部删除，然后再插入
            List<EntityDicCombine> listCom = bsItemIn.DataSource as List<EntityDicCombine>;

            List<EntityDicPubIcdCombine> addList = new List<EntityDicPubIcdCombine>();
            try
            {
                EntityDicPubIcdCombine com = new EntityDicPubIcdCombine();
                com.IcdId = icd_id;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(com);
                base.Delete(request);

                int count = 1;
                if (listCom != null && listCom.Count > 0)
                {
                    foreach (var item in listCom)
                    {
                        EntityDicPubIcdCombine icdCom = new EntityDicPubIcdCombine();
                        icdCom.ComId = item.ComId;
                        icdCom.IcdId = icd_id;
                        icdCom.SortNo = count;
                        count++;
                        addList.Add(icdCom);
                    }
                    EntityRequest req = new EntityRequest();
                    req.SetRequestValue(addList);
                    base.Update(req);
                }
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("保存成功");
                    gcItemNotIn.Enabled = false;
                    gcItemIn.Enabled = false;
                    LoadBCCombineDeatil();
                    DoRefresh();
                }

            }catch(Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("保存失败" + ex.Message);
            }
        }
        bool inLoadBCCombineDeatil = false;
        private void gvIcd_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bsIcd.Current != null)
            {
                if (inLoadBCCombineDeatil == false)
                {
                    inLoadBCCombineDeatil = true;

                    if (bsIcd.Position > -1)
                    {
                        LoadBCCombineDeatil();
                    }

                    inLoadBCCombineDeatil = false;
                }
            }
        }

        private void LoadBCCombineDeatil()
        {
            inLoadBCCombineDeatil = true;
            ItemIn = new List<EntityDicCombine>();
            bsItemNotIn.DataSource = new List<EntityDicCombine>();
            if (bsIcd.Position > -1)
            {
                EntityDicPubIcd CUR = bsIcd.Current as EntityDicPubIcd;
                List<EntityDicPubIcdCombine> listIcdCom = new List<EntityDicPubIcdCombine>();
                EntityRequest request = new EntityRequest();
                EntityDicPubIcdCombine combine = new EntityDicPubIcdCombine();
                combine.IcdId = CUR.IcdId;
                request.SetRequestValue(combine);
                EntityResponse response = base.Search(request);
                listIcdCom = response.GetResult() as List<EntityDicPubIcdCombine>;
                foreach (EntityDicPubIcdCombine detail in listIcdCom)
                {
                    EntityDicCombine com = AllCombines.Find(i => i.ComId == detail.ComId);
                    if (com != null)
                        ItemIn.Add(com);
                }
                this.bsItemIn.DataSource = ItemIn;
                this.bsItemNotIn.DataSource = AllCombines.Except(ItemIn).ToList();
                ItemNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            }
            inLoadBCCombineDeatil = false;
        }

        private void btnAddAllUser_Click(object sender, EventArgs e)
        {
            List<EntityDicCombine> listComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            if (listComNotIn.Count > 0)
            {
                List<EntityDicCombine> listComIn = bsItemIn.DataSource as List<EntityDicCombine>;
                foreach (EntityDicCombine com in listComNotIn)
                {
                    listComIn.Add(com);
                }
                bsItemNotIn.DataSource = new List<EntityDicCombine>();
                gvItemNotIn.RefreshData();
                gvItemIn.RefreshData();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            EntityDicCombine drCom = (EntityDicCombine)gvItemNotIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicCombine> dtComIn = bsItemIn.DataSource as List<EntityDicCombine>;
            //drCom.IcdSortNo = dtComIn.Count;
            dtComIn.Add(drCom);
            List<EntityDicCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            dtComNotIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            EntityDicCombine drCom = (EntityDicCombine)gvItemIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            dtComNotIn.Add(drCom);
            List<EntityDicCombine> dtComIn = bsItemIn.DataSource as List<EntityDicCombine>;
            dtComIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void btnDelAllUser_Click(object sender, EventArgs e)
        {
            List<EntityDicCombine> listComIn = bsItemIn.DataSource as List<EntityDicCombine>;
            if (listComIn.Count > 0)
            {
                List<EntityDicCombine> listComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
                foreach (EntityDicCombine com in listComIn)
                {
                    listComNotIn.Add(com);
                }
                bsItemIn.DataSource = new List<EntityDicCombine>();
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortId"></param>
        private void dataSort(int sortId)
        {
            int current = gvItemIn.FocusedRowHandle;
            if (current + sortId >= 0 && current + sortId < gvItemIn.RowCount)
            {
                //DataRowView drv = (DataRowView)gvItemIn.GetRow(current);
                //DataRowView drvUp = (DataRowView)gvItemIn.GetRow(current + sortId);
                EntityDicCombine drv = (EntityDicCombine)gvItemIn.GetRow(current);
                EntityDicCombine drvUp = (EntityDicCombine)gvItemIn.GetRow(current + sortId);
                object sort = drv.ComSortNo;
                object id = drv.ComId;
                object itmName = drv.ComName;


                drv.ComSortNo = drv.ComSortNo;
                drv.ComId = drvUp.ComId;
                drv.ComName = drvUp.ComName;


                drvUp.ComSortNo = drvUp.ComSortNo;
                drvUp.ComId = id.ToString();
                drvUp.ComName = itmName.ToString();

                gvItemIn.FocusedRowHandle = current + sortId;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            dataSort(-1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            dataSort(1);
        }

        private void bsIcd_CurrentChanged(object sender, EventArgs e)
        {
            if (bsIcd.Position > -1)
            {
                LoadBCCombineDeatil();
            }
        }

        private void gcItemIn_DoubleClick(object sender, EventArgs e)
        {
            EntityDicCombine drCom = (EntityDicCombine)gvItemIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            dtComNotIn.Add(drCom);
            List<EntityDicCombine> dtComIn = bsItemIn.DataSource as List<EntityDicCombine>;
            dtComIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void gcItemNotIn_DoubleClick(object sender, EventArgs e)
        {
            EntityDicCombine drCom = (EntityDicCombine)gvItemNotIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicCombine> dtComIn = bsItemIn.DataSource as List<EntityDicCombine>;
            dtComIn.Add(drCom);
            List<EntityDicCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicCombine>;
            dtComNotIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void gcItemNotIn_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (this.gvItemNotIn.SelectedRowsCount > 0)
            {
                this.gcItemNotIn.DoDragDrop(new object[] { this.gvItemNotIn.GetSelectedRows(), "notin" }, DragDropEffects.All);
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

        private void gcItemNotIn_DragOver(object sender, DragEventArgs e)
        {
            object[] obj = e.Data.GetData(typeof(object[])) as object[];
            if (obj != null && obj[1].ToString() == "in")
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        //private void txtFilterIcd_TextChanged(object sender, EventArgs e)
        //{
        //    //string fiter = txtFilterIcd.Text;
        //    if (!string.IsNullOrEmpty(fiter))
        //    {
        //        bsIcd.DataSource = list.Where(a => a.IcdId.Contains(fiter)
        //                                      || a.IcdName.Contains(fiter)).ToList();
        //    }
        //    else
        //        bsIcd.DataSource = list;
        //}

        //private void txtFilterCombine_TextChanged(object sender, EventArgs e)
        //{
        //    //string fiter = txtFilterCombine.Text;
        //    if (!string.IsNullOrEmpty(fiter))
        //    {
        //        bsItemNotIn.DataSource = ItemNotIn.Where(a => a.ComId.Contains(fiter)
        //                                                    || a.ComName.Contains(fiter)).ToList();
        //    }
        //    else
        //        bsItemNotIn.DataSource = ItemNotIn;
        //}
    }
}
