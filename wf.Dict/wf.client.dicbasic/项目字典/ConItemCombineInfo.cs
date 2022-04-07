using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using lis.client.control;   
using dcl.entity;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConItemCombineInfo : ConDicCommon, IBarActionExt
    {


        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicCombine> list = new List<EntityDicCombine>();
        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        public ConItemCombineInfo()
        {
            InitializeComponent();
            this.Name = "ConCombine";

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
                CheckBarcodeHasUsed();

                parentForm = this.GetParentForm();
                if (parentForm != null)
                {
                    parentForm.AfterControlsEnableSetted += new FrmDictMainDev.EventHandler(parentForm_AfterControlsEnableSetted);
                }

                //系统配置：组合字典显示首列为
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("DictCombine_showColName") == "代码")
                {
                    colcom_id.Visible = false;
                    colcom_code.Visible = true;
                    colcom_code.VisibleIndex = 0;
                }
                else
                {
                    colcom_code.Visible = false;
                }

                if (UserInfo.GetSysConfigValue("Enable_CombineUrgentFlag") == "是")
                {
                    checkEditUrgentFlag.Visible = true;
                }
                if (UserInfo.GetSysConfigValue("seq_visible") == "是" || UserInfo.GetSysConfigValue("seq_visibleForCombine") == "是")
                {
                    colcom_seq.Visible = true;
                    colcom_seq.VisibleIndex = 0;
                    colcom_id.Visible = false;
                }
                //开启报告解读
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Interpretation_Report") == "是")
                {
                    meComSamNotice1.Visible = true;
                    meComContent.Visible = true;
                    meComCollectionNotice.Visible = true;
                    meComSaveNotice1.Visible = true;
                    meComInfluence.Visible = true;
                }
            }
            finally
            {
                // this.lstRoleLeft.SelectedValueChanged += this.lstRoleLeft_SelectedIndexChanged;
            }
        }

        void parentForm_AfterControlsEnableSetted(object obj, EventArgs args)
        {
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


        /// <summary>
        /// 条码是否启用
        /// </summary>
        private void CheckBarcodeHasUsed()
        {
            string userBarcode = UserInfo.GetSysConfigValue("UseBarcode");
            if (userBarcode == "否")
            {
                //tabBarcodeCombines.PageVisible = false;
            }
            else
            {
                //有启用条码时组合的报告时间不用
                Com_Rep_unit.Visible = spinEdit1.Visible  = false;
            }
        }


        /// <summary>
        /// 设置列表控件自带按钮Enable属性
        /// </summary>
        private void SetEmbeddedNavigator(bool enable)
        {
            btnModify.Enabled = true;
        }

        private void toSave()
        {
            this.isActionSuccess = false;

            if (txtCombineName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("组合名称不能为空", "信息");
                txtCombineName.Focus();
                return;
            }

            if (selectDict_Type1.valueMember == null || selectDict_Type1.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("物理组不能为空", "信息");
                selectDict_Type1.Focus();
                return;
            }

            if (txtPType.valueMember == null || txtPType.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("专业组不能为空", "信息");
                txtPType.Focus();
                return;
            }
            this.bsCombine.EndEdit();

            if (bsCombine.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityRequest requestUpdate = new EntityRequest();
            EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;

            //五笔与拼音
            dr.ComPyCode = txtPY.Text;
            dr.ComWbCode = txtWB.Text;
            String com_id = dr.ComId;
            request.SetRequestValue(dr);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Combine", dr);

            EntityResponse result = new EntityResponse();
            if (com_id == "")
            {
                EntitySysOperationLog etOperation = CreateOperateInfo("新增");
                dict.Add("Operation", etOperation);
                request.SetRequestValue(dict);
                result = base.New(request);
            }
            else
            {
                EntitySysOperationLog etOperation = CreateOperateInfo("修改");
                dict.Add("Operation", etOperation);
                requestUpdate.SetRequestValue(dict);
                result = base.Update(requestUpdate);
            }
            if (base.isActionSuccess)
            {
                DoRefresh();
                int index = list.FindIndex(w => w.ComName == dr.ComName);
                gridView_combine.FocusedRowHandle = index;
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
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gridControl1.Name, true);

            dlist.Add(this.splitContainerControl1.Panel1.Name, true);


            //dlist.Add(gcItemIn.Name, true);


            return dlist;
        }


    

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            colorAuditedBack.Color = Color.White;
            //getTable();


            //if (bsCombine.Current != null)
            //{
            //    EntityDicCombine drCom = (EntityDicCombine)bsCombine.Current;

            //    colorAuditedBack.Color = Color.FromArgb((int)drCom.ComOnlineClolr);

            //    //if (drCom.["com_line_color"] != null && drCom["com_line_color"] != DBNull.Value)
            //    //{
            //    //    colorAuditedBack.Color = Color.FromArgb((int)drCom["com_line_color"]);
            //    //}
            //    //LoadBCCombines();
            //}
        }


        private void LoadBCCombines()
        {
            inLoadBCCombines = true;

            if (bsCombine.Position > -1)
            {
                ProxyCommonDic proxy = new ProxyCommonDic("svc.ConItemCombineBarcode");
                EntityDicCombine combine = (EntityDicCombine)bsCombine.Current;
                EntityRequest request = new EntityRequest();
                if (!string.IsNullOrEmpty(combine.ComId))
                {
                    request.SetRequestValue(combine.ComId);
                    EntityResponse result = new EntityResponse();
                    result = proxy.Search(request);
                    if (isActionSuccess)
                    {
                        this.bsBCCombine.DataSource = result.GetResult() as List<EntitySampMergeRule>;
                    }
                }
                else {
                    bsBCCombine.DataSource = new List<EntitySampMergeRule>();
                }
            }
            inLoadBCCombines = false;
        }

        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            EntityDicCombine dr = (EntityDicCombine)bsCombine.AddNew();
            dr.ComId = string.Empty;
            dr.ComDelFlag = LIS_Const.del_flag.OPEN;

            Com_Rep_unit.SelectedIndex = 1;
            this.txtCombineName.Focus();
            txtPY.Properties.ReadOnly = true;
            txtWB.Properties.ReadOnly = true;
            selectDict_Type1.displayMember = string.Empty;
            txtPType.displayMember = string.Empty;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (!isAdd)
            {
                gridView_combine.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                GetViewColumnFilterInfo();
                this.toSave();
                SetViewColumnFilterInfo();
                gridView_combine.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                gridView1_FocusedRowChanged(null, null);
            }
            else
                this.toSave();
            if (this.isActionSuccess)
            {
                SetEmbeddedNavigator(false);
            }
        }

        public void Delete()
        {
            if (lis.client.control.MessageDialog.Show("你确定要删除吗？(包括该项目组合条码信息、组合项目明细、仪器组合信息、组合拆分明细)", "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.bsCombine.EndEdit();

                if (bsCombine.Current == null)
                {
                    return;
                }
                EntityRequest request = new EntityRequest();
                EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
                String br_id = dr.ComId;
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("Combine", dr);
                request.SetRequestValue(d);
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
            this.isActionSuccess = true;
            EntityRequest request = new EntityRequest();
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



        #region IBarActionExt 成员

        public void Cancel()
        {
            SetEmbeddedNavigator(false);
        }

        public void Edit()
        {
            isAdd = false;
            gridControl1.Enabled = false;
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

        bool inLoadBCCombines = false;

        private void bsCombine_CurrentChanged(object sender, EventArgs e)
        {
            if (inLoadBCCombines == false)
            {
                inLoadBCCombines = true;

                if (bsCombine.Position > -1)
                {
                    LoadBCCombines();
                }

                inLoadBCCombines = false;
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
        

        /// <summary>
        /// 选择框发生改变时,刷新
        /// </summary>
        private void radioGroup_split_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            //GridView grid = sender as GridView;
            //DataRow row = grid.GetDataRow(e.RowHandle);
            //if (row != null && row.Table.Columns.Contains("com_urgent_flag") && row["com_urgent_flag"].ToString() == "1")
            //{
            //    e.Appearance.BackColor = Color.Silver;
            //}
        }

        private void txtCost_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            ProxyCommonDic proxy = new ProxyCommonDic("svc.ConItemCombineBarcode");
            EntityDicCombine combine = (EntityDicCombine)bsCombine.Current;
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(combine.ComId);
            EntityResponse result = new EntityResponse();
            List<EntitySampMergeRule> rule = new List<EntitySampMergeRule>();
            result = proxy.Search(request);
            if (isActionSuccess)
            {
                rule = result.GetResult() as List<EntitySampMergeRule>;
                this.bsBCCombine.DataSource = rule;
            }
            frmCombineBarcode form = new frmCombineBarcode();
            form.InputData = rule;
            form.inputDataRow = combine;
            //form.inputDataTable = dt;
            form.Show();
        }

        private void txtCombineName_Leave(object sender, EventArgs e)
        {
            if (bsCombine.Current != null)
            {
                EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
                dr.ComPyCode = tookit.GetSpellCode(this.txtCombineName.Text);
                dr.ComWbCode = tookit.GetWBCode(this.txtCombineName.Text);
            }
        }
    }
}
