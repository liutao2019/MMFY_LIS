using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using dcl.entity;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConItemCombineDetail : ConDicCommon, IBarActionExt
    {

        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicItmCombine> ItemIn = new List<EntityDicItmCombine>();
        private List<EntityDicItmCombine> ItemNotIn = new List<EntityDicItmCombine>();
        //存放未包含明细中过滤后的项目数据 2018-01-19 SJC
        private List<EntityDicItmCombine> ItemNotInFilter = new List<EntityDicItmCombine>();

        List<EntityDicItmCombine> listItmCombine = new List<EntityDicItmCombine>();
        private List<EntityDicCombine> list = new List<EntityDicCombine>();
        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        public ConItemCombineDetail()
        {
            InitializeComponent();
            this.Name = "ConCombine";
            //  SetDefaultOrigin();



        }




        FrmDictMainDev parentForm;
        private void FrmRoleManage_Load(object sender, EventArgs e)
        {
            try
            {
                //[项目字典]是否关联用户可用物理组
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypes") == "是")
                {
                    IsBindingUserTypes = true;
                }
                else
                {
                    IsBindingUserTypes = false;//默认不绑定用户可用物理组
                }

                DoRefresh();
                parentForm = this.GetParentForm();
                if (parentForm != null)
                {
                    parentForm.ModifyButton();
                    parentForm.AfterControlsEnableSetted += new FrmDictMainDev.EventHandler(parentForm_AfterControlsEnableSetted);
                }

                //系统配置：组合字典显示首列为
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("DictCombine_showColName") == "代码")
                {
                    colcom_id.Visible = true;
                    colcom_code.Visible = true;
                }
                else
                {
                    colcom_code.Visible = false;
                }


                if (UserInfo.GetSysConfigValue("seq_visible") == "是" || UserInfo.GetSysConfigValue("seq_visibleForCombine") == "是")
                {
                    colcom_seq.Visible = true;
                    colcom_id.Visible = true;
                    colcom_itm_id.FieldName = "com_itm_id";
                    colcom_itm_id.Caption = "序号";
                    gridColumn5.FieldName = "com_itm_id";
                    gridColumn5.Caption = "ID";
                }
            }
            finally
            {
                // this.lstRoleLeft.SelectedValueChanged += this.lstRoleLeft_SelectedIndexChanged;
            }
        }

        void parentForm_AfterControlsEnableSetted(object obj, EventArgs args)
        {
            if (parentForm.defaultEnableStatus == false)
            {
                this.gvItemIn.Columns["ItmMustItem"].OptionsColumn.AllowEdit = false;
                this.gvItemIn.Columns["ItmPrintFlag"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                this.gvItemIn.Columns["ItmMustItem"].OptionsColumn.AllowEdit = true;
                this.gvItemIn.Columns["ItmPrintFlag"].OptionsColumn.AllowEdit = true;
            }
        }

        DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[] filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];


        private void GetViewColumnFilterInfo()
        {
            DevExpress.XtraGrid.Views.Base.ViewFilter xxx = gridView_combine.SortInfo.View.ActiveFilter;

            for (int i = 0; i < xxx.Count; i++)
            {
                filter[i] = xxx[i];
            }
            xxx.Clear();
        }

        private void SetViewColumnFilterInfo()
        {
            for (int j = 0; j < filter.Length; j++)
            {
                gridView_combine.SortInfo.View.ActiveFilter.Add(filter[j]);
            }

            filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];

        }



        private void calcHead()
        {
            //this.txtCNT.Text = this.dtItemIn.Rows.Count.ToString();
            //float totalCost = 0;
            //float totalPri = 0;
            //foreach (DataRow dr in dtItemIn.Rows)
            //{
            //    float cost = dr["itm_cost"].ToString() == "" ? 0 : float.Parse(dr["itm_cost"].ToString());
            //    float pri = dr["itm_pri"].ToString() == "" ? 0 : float.Parse(dr["itm_pri"].ToString());
            //    totalCost += cost;
            //    totalPri += pri;
            //}
            //this.txtPri.Text = totalPri.ToString();
            //this.txtCost.Text = totalCost.ToString();
        }

        /// <summary>
        /// 移动全部未包含项目到已包含
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAllUser_Click(object sender, EventArgs e)
        {

            List<EntityDicItmCombine> listComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
            if (listComNotIn.Count > 0)
            {
                List<EntityDicItmCombine> listComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
                foreach (EntityDicItmCombine com in listComNotIn)
                {
                    listComIn.Add(com);
                }
                for (int i = 0; i < listComIn.Count; i++)
                {
                    listComIn[i].ItmSort = i;
                }
                bsItemNotIn.DataSource = new List<EntityDicItmCombine>();
                gvItemNotIn.RefreshData();
                gvItemIn.RefreshData();
            }

        }

        /// <summary>
        /// 移动一条"未包含项目"到"已包含项目"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            EntityDicItmCombine drCom = (EntityDicItmCombine)gvItemNotIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicItmCombine> dtComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
            drCom.ItmSort = dtComIn.Count;
            dtComIn.Add(drCom);
            List<EntityDicItmCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
            dtComNotIn.Remove(drCom);
            listItmCombine.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            EntityDicItmCombine drCom = (EntityDicItmCombine)gvItemIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicItmCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
            dtComNotIn.Add(drCom);
            List<EntityDicItmCombine> dtComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
            dtComIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }

        private void btnDelAllUser_Click(object sender, EventArgs e)
        {
            //this.moveAllItem(dtItemIn, dtItemNotIn, false);
            List<EntityDicItmCombine> listComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
            if (listComIn.Count > 0)
            {
                List<EntityDicItmCombine> listComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
                foreach (EntityDicItmCombine com in listComIn)
                {
                    listComNotIn.Add(com);
                }
                bsItemIn.DataSource = new List<EntityDicItmCombine>();
                gvItemIn.RefreshData();
                gvItemNotIn.RefreshData();
            }
        }


        private void toNew()
        {
            if (bsCombine.Current == null) return;
            this.gridControl1.Enabled = false;
        }



        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gridControl1.Name, true);
            dlist.Add(this.gcItemIn.Name, false);
            dlist.Add(this.gcItemNotIn.Name, false);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);


            //dlist.Add(gcItemIn.Name, true);


            return dlist;
        }

        bool inLoadBCCombineDeatil = false;

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            if (bsCombine.Current != null)
            {
                if (inLoadBCCombineDeatil == false)
                {
                    inLoadBCCombineDeatil = true;

                    if (bsCombine.Position > -1)
                    {
                        LoadBCCombineDeatil();
                    }

                    inLoadBCCombineDeatil = false;
                }
            }

        }
        /// <summary>
        /// 加载组合
        /// </summary>
        private void LoadBCCombineDeatil()
        {
            inLoadBCCombineDeatil = true;

            if (bsCombine.Position > -1)
            {

                EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dr);
                EntityResponse ds = base.View(request);
                if (isActionSuccess)
                {
                    Dictionary<string, object> dict = ds.GetResult() as Dictionary<string, object>;
                    List<EntityDicCombineDetail> listDetail = new List<EntityDicCombineDetail>();

                    object objDetail = dict["Detail"];
                    object objItm = dict["ItmCombine"];
                  //  object objItemIn = dict["ItmIn"];
                    if (objDetail != null)
                    {
                        listDetail = objDetail as List<EntityDicCombineDetail>;
                    }
                    if (objItm != null)
                    {
                        listItmCombine = objItm as List<EntityDicItmCombine>;
                    }
                    //if (objItemIn != null)
                    //{
                    //    ItemIn = objItemIn as List<EntityDicItmCombine>;
                    //}
                    ItemIn = listItmCombine.Where(p => listDetail.Where(g => g.ComItmId == p.ItmId).Any()).ToList();
                    //for (int i = 0; i < ItemIn.Count; i++)
                    //{
                    //    ItemIn[i].ItmSort = listDetail[i].ComSortNo;
                    //}
                    foreach (EntityDicItmCombine com in ItemIn)
                    {
                        foreach (EntityDicCombineDetail detail in listDetail)
                        {
                            if (com.ItmId == detail.ComItmId)
                            {
                                com.ItmSort = detail.ComSortNo;
                                com.ItmMustItem = int.Parse(detail.ComMustItem);
                                com.ItmPrintFlag = detail.ComPrintFlag;
                            }
                        }
                    }
                    ItemNotIn = listItmCombine.Where(p => !listDetail.Where(g => g.ComItmId == p.ItmId).Any() && p.ItmPriId == dr.ComPriId).ToList();
                    this.bsItemIn.DataSource = ItemIn;
                    this.bsItemNotIn.DataSource = ItemNotIn;
                    ItemNotInFilter = ItemNotIn;
                }
            }


            inLoadBCCombineDeatil = false;
        }


        private void getTable()
        {
            //if (bsCombine.Current != null)
            //{
            //    DataRow dr = ((DataRowView)(bsCombine.Current)).Row;
            //    this.initDetail(dr["com_id"].ToString(), dr["com_ptype"].ToString(), dr["com_sam_id"].ToString());
            //}
            //else
            //    this.initDetail("0", "0", "0");
        }


        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            this.toNew();

        }

        public void Save()
        {
            this.isActionSuccess = false;
            this.bsCombine.EndEdit();

            if (bsCombine.Current == null)
            {
                return;
            }
            EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
            String com_id = "";
            if (dr != null)
            {
                com_id = dr.ComId.ToString();
            }
            List<EntityDicItmCombine> listCom = (List<EntityDicItmCombine>)bsItemIn.DataSource;
            if (bsItemIn.Count > 0)
            {
                for (int i = 0; i < gvItemIn.RowCount; i++)
                {
                    EntityDicItmCombine com = (EntityDicItmCombine)gvItemIn.GetRow(i);
                    //com.ItmSort = i;
                }
            }
            Dictionary<string, object> d = new Dictionary<string, object>();
            List<EntityDicCombineDetail> listDetail = new List<EntityDicCombineDetail>();

            if (listCom != null)
            {
                foreach (var listItmCom in listCom)
                {
                    EntityDicCombineDetail entityDetail = new EntityDicCombineDetail();
                    entityDetail.ComId = com_id;
                    entityDetail.ComItmId = listItmCom.ItmId;
                    entityDetail.ComItmEname = listItmCom.ItmEname;
                    entityDetail.ComMustItem = listItmCom.ItmMustItem.ToString();
                    entityDetail.ComFlag = listItmCom.ItmComFlag;
                    entityDetail.ComPrintFlag = listItmCom.ItmPrintFlag;
                    entityDetail.ComSortNo = listItmCom.ItmSort;
                    listDetail.Add(entityDetail);
                }

            }
            d.Add("Combine", dr);
            d.Add("ComDetail", listDetail);
            EntityRequest request = new EntityRequest();
            this.isActionSuccess = true;
            EntityResponse result = new EntityResponse();
            if (bsItemIn.Count <= 0)
            {
                EntityDicCombineDetail detail = new EntityDicCombineDetail();
                Dictionary<string, object> dict = new Dictionary<string, object>();
                detail.ComId = dr.ComId;
                dict.Add("ItmDetail", detail);
                request.SetRequestValue(dict);
                result = base.Delete(request);
            }
            else
            {
                EntitySysOperationLog etOperation = CreateOperateInfo("修改");
                d.Add("Operation", etOperation);
                request.SetRequestValue(d);
                result = base.Update(request);
                txtItemSort.Text = "";
            }

            if (base.isActionSuccess)
            {
                LoadBCCombineDeatil();
            }
            else
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// 生成操作用户信息记录
        /// </summary>
        /// <returns></returns>
        private EntitySysOperationLog CreateOperateInfo(string type)
        {
            EntitySysOperationLog log = new EntitySysOperationLog();
            log.OperatUserId = UserInfo.loginID;
            log.OperatUserName = UserInfo.userName;
            log.OperatAction = type;
            return log;
        }
        public void Delete()
        {
            //this.toDel();
        }

        public void DoRefresh()
        {
            gcItemNotIn.Enabled = false;
            gcItemIn.Enabled = false;
            this.isActionSuccess = true;
            EntityRequest request = new EntityRequest();
            List<EntityDicCombine> listComb = new List<EntityDicCombine>();
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                GetViewColumnFilterInfo();
                list = ds.GetResult() as List<EntityDicCombine>;
                this.bsCombine.DataSource = list;
                SetViewColumnFilterInfo();
            }
        }

        #endregion

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

        private void gcItemIn_DragDrop(object sender, DragEventArgs e)
        {


            //if (e.Effect == DragDropEffects.Copy)
            //{
            //    this.moveSelectItem(dtItemNotIn, dtItemIn, gvItemNotIn, gvItemIn, "", false);
            //}

        }

        private void gcItemIn_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void gcItemNotIn_DragOver(object sender, DragEventArgs e)
        {
            object[] obj = e.Data.GetData(typeof(object[])) as object[];
            if (obj != null && obj[1].ToString() == "in")
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void gcItemNotIn_DragDrop(object sender, DragEventArgs e)
        {


            //if (e.Effect == DragDropEffects.Copy)
            //{
            //    this.moveSelectItem(dtItemIn, dtItemNotIn, gvItemIn, gvItemNotIn, "", false);
            //}

        }


        private void gcItemNotIn_DoubleClick(object sender, EventArgs e)
        {
            EntityDicItmCombine drCom = (EntityDicItmCombine)gvItemNotIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicItmCombine> dtComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
            dtComIn.Add(drCom);
            List<EntityDicItmCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
            dtComNotIn.Remove(drCom);
            listItmCombine.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }


        private void gcItemIn_DoubleClick(object sender, EventArgs e)
        {
            EntityDicItmCombine drCom = (EntityDicItmCombine)gvItemIn.GetFocusedRow();
            if (drCom == null)
                return;
            List<EntityDicItmCombine> dtComNotIn = bsItemNotIn.DataSource as List<EntityDicItmCombine>;
            dtComNotIn.Add(drCom);
            List<EntityDicItmCombine> dtComIn = bsItemIn.DataSource as List<EntityDicItmCombine>;
            dtComIn.Remove(drCom);
            gvItemNotIn.RefreshData();
            gvItemIn.RefreshData();
        }






        #region IBarActionExt 成员

        public void Cancel()
        {
            //SetEmbeddedNavigator(false);
            gcItemNotIn.Enabled = false;
            gcItemIn.Enabled = false;
            txtPTypeFilter.displayMember = "生化组";
            txtItemSort.Text = "";
        }

        public void Edit()
        {
            isAdd = false;
            gridControl1.Enabled = false;
            // gcBCCombines.Enabled = true;
            //SetEmbeddedNavigator(true);
            //xtraTabControl1.Enabled = true;
            gcItemNotIn.Enabled = true;
            gcItemIn.Enabled = true;
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


        private void bsCombine_CurrentChanged(object sender, EventArgs e)
        {
            if (bsCombine.Position > -1)
            {
                LoadBCCombineDeatil();
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


        private void btnUp_Click(object sender, EventArgs e)
        {
            dataSort(-1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            dataSort(1);
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
                EntityDicItmCombine drv = (EntityDicItmCombine)gvItemIn.GetRow(current);
                EntityDicItmCombine drvUp = (EntityDicItmCombine)gvItemIn.GetRow(current + sortId);
                object sort = drv.ItmSort;
                object id = drv.ItmId;
                object itmName = drv.ItmName;
                object itmEname = drv.ItmEname;
                object itmMustItm = drv.ItmMustItem;
                object itmPrintFlag = drv.ItmPrintFlag;

                drv.ItmSort = drv.ItmSort;
                drv.ItmId = drvUp.ItmId;
                drv.ItmName = drvUp.ItmName;
                drv.ItmEname = drvUp.ItmEname;
                drv.ItmMustItem = drvUp.ItmMustItem;
                drv.ItmPrintFlag = drvUp.ItmPrintFlag;

                drvUp.ItmSort = drvUp.ItmSort;
                drvUp.ItmId = id.ToString();
                drvUp.ItmName = itmName.ToString();
                drvUp.ItmEname = itmEname.ToString();
                drvUp.ItmMustItem = Convert.ToInt32(itmMustItm);
                drvUp.ItmPrintFlag = Convert.ToInt32(itmPrintFlag);
                gvItemIn.FocusedRowHandle = current + sortId;
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //DataRow dr = this.gridView_combine.GetDataRow(e.RowHandle);
            //if (dr != null)
            //{
            //    if (dr["bar_type"].ToString() == "1")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
            //    }
            //}
        }


        /// <summary>
        /// 选择框发生改变时,刷新
        /// </summary>
        private void radioGroup_split_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.doRefresh();
            Filter();
        }


        private void txtSort_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            List<EntityDicCombine> listComb = new List<EntityDicCombine>();
            listComb = list;
            string filter = radioGroup_split.Text.Trim();
            string filterSort = txtSort.Text.Trim();
            int sort = 0;
            int.TryParse(filter, out sort);
            if (filter == "0")
                bsCombine.DataSource = list;
            else if (filter == "1")
            {
                sort = 0;
                listComb = listComb.Where(w => w.ComBarSplit == sort).ToList();
            }
            else if (filter == "2")
            {
                sort = 1;
                listComb = listComb.Where(w => w.ComBarSplit == sort).ToList();
            }
            if (!string.IsNullOrEmpty(filterSort))
            {
                listComb = listComb.Where(w => w.ComId.Contains(filterSort) ||
                                                       w.ComName != null && w.ComName.Contains(filterSort) ||
                                                       w.ComCode != null && w.ComCode.Contains(filterSort) ||
                                                       w.ComPyCode != null && w.ComPyCode.Contains(filterSort.ToUpper()) ||
                                                       w.ComWbCode != null && w.ComWbCode.Contains(filterSort.ToUpper()) ||
                                                       w.PTypeName != null && w.PTypeName.Contains(filterSort) ||
                                                       w.CTypeName != null && w.CTypeName.Contains(filterSort) ||
                                                       w.ComSortNo.ToString() != null && w.ComSortNo.ToString() == filterSort).ToList();
            }
            this.bsCombine.DataSource = listComb;
        }

        private void gridView_combine_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView grid = sender as GridView;
            EntityDicCombine row = grid.GetRow(e.RowHandle) as EntityDicCombine;
            if (row != null && row.ComUrgentFlag.ToString() == "1")
            {
                e.Appearance.BackColor = Color.Silver;
            }
        }
        private void txtPTypeFilter_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //if (txtPTypeFilter.valueMember != string.Empty)
            //{
            //    bsItemNotIn.Filter = "itm_ptype='" + txtPTypeFilter.valueMember + "'";
            //}
            //else
            //    bsItemNotIn.Filter = "1<>1";

            if (txtPTypeFilter.valueMember == string.Empty)
            {
                bsItemNotIn.DataSource = ItemNotIn;
                ItemNotInFilter = ItemNotIn;
            }
            else
            {
                foreach (EntityDicItmCombine combine in ItemIn)
                {
                    listItmCombine.Remove(combine);
                }
                //ItemNotIn = listItmCombine.Where(w => w.ItmPriId.Contains(txtPTypeFilter.valueMember)).ToList();
                //bsItemNotIn.DataSource = ItemNotIn;
                ItemNotInFilter = listItmCombine.Where(w => w.ItmPriId.Contains(txtPTypeFilter.valueMember)).ToList();
                bsItemNotIn.DataSource = ItemNotInFilter;
            }
        }

        private void txtItemSort_TextChanged(object sender, EventArgs e)
        {
            string sort = txtItemSort.Text.Trim().ToUpper().Replace("'", "''");
            List<EntityDicItmCombine> listItemNotIn = new List<EntityDicItmCombine>();
            //listItemNotIn = ItemNotIn;
            listItemNotIn = ItemNotInFilter;
            if (sort.Trim() == string.Empty)
            {
                bsItemNotIn.DataSource = listItemNotIn;
            }
            else
            {
                listItemNotIn = listItemNotIn.Where(w => w.ItmId.Contains(sort) ||
                                                        (!string.IsNullOrEmpty(w.ItmEname) && w.ItmEname.ToUpper().Contains(sort.ToUpper())) ||
                                                        (!string.IsNullOrEmpty(w.ItmName) && w.ItmName.ToUpper().Contains(sort.ToUpper()))
                                                    ).ToList();
            }
            bsItemNotIn.DataSource = listItemNotIn;
        }

    }
}
