using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
//using dcl.client.dicbasic;
using lis.client.control;
using dcl.entity;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConItemCombineBarcode : ConDicCommon, IBarActionExt
    {
        List<EntityDicCombine> combineList = new List<EntityDicCombine>();
        List<EntitySampMergeRule> mergeRuleList = new List<EntitySampMergeRule>();

        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        public ConItemCombineBarcode()
        {
            InitializeComponent();
            SetDefaultOrigin();
            selectDict_Cuv1.BindByValue = true;
            selectDict_Type2.BindByValue = true;
            selectDict_Sample_type1.BindByValue = true;
            txtSplitCode.ReadOnly = true;
            SetEmbeddedNavigator(false);
            //this.searchControl1.FilterOnEnterKeyDown = true;

        }

        /// <summary>
        /// 设置默认的组合来源
        /// </summary>
        private void SetDefaultOrigin()
        {
            selectDict_Origin1.SelectByDispaly("住院");
        }

        private void initDetail(String comId, String typeId, string samId)
        {

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

                this.doRefresh();
                CheckBarcodeHasUsed();

                parentForm = this.GetParentForm();
                if (parentForm != null)
                {
                    parentForm.ModifyButton();
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

            }
            finally
            {
                // this.lstRoleLeft.SelectedValueChanged += this.lstRoleLeft_SelectedIndexChanged;
            }
        }

        void parentForm_AfterControlsEnableSetted(object obj, EventArgs args)
        {

            this.btnCombineView.Enabled = true;


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

        }



        #region 响应按钮菜单点击事件

        /// <summary>
        /// 设置列表控件自带按钮Enable属性
        /// </summary>
        private void SetEmbeddedNavigator(bool enable)
        {
            gvBCCombines.OptionsBehavior.Editable = enable;
            //gcBCCombines.EmbeddedNavigator.Buttons.CustomButtons.n = enable;
            //gcBCCombines.EmbeddedNavigator.Buttons.Remove.Enabled = enable;
            //gcBCCombines.UseEmbeddedNavigator = enable;
            pnlBtn.Enabled = enable;
        }
        private void doRefresh()
        {
            this.isActionSuccess = true;
            combineList = base.View(new EntityRequest()).GetResult() as List<EntityDicCombine>;
            combineList = FiltrateComSplit(combineList);
            if (isActionSuccess)
            {
                GetViewColumnFilterInfo();
                bsCombine.DataSource = combineList;
                SetViewColumnFilterInfo();
            }
        }
        /// <summary>
        /// 过滤组合拆分标志
        /// </summary>
        /// <param name="dtDcom">DataTable</param>
        /// <returns>DataTable</returns>
        private List<EntityDicCombine> FiltrateComSplit(List<EntityDicCombine> dtDcom)
        {
            List<EntityDicCombine> filterList = new List<EntityDicCombine>();
            if (dtDcom == null | dtDcom.Count <= 0)
                return dtDcom;//为空不过滤

            //int bar_split_flag = 0;//1:有拆分 0:没拆分

            if (radioGroup_split.SelectedIndex == 0)//全部
            {
                filterList = dtDcom;//不过滤
            }
            else if (radioGroup_split.SelectedIndex == 1)//没拆分
            {
                filterList = dtDcom.Where(w => w.ComBarSplit == 0).ToList();
            }
            else if (radioGroup_split.SelectedIndex == 2)//有拆分
            {
                filterList = dtDcom.Where(w => w.ComBarSplit == 1).ToList();
            }
            return filterList;
        }
        /// <summary>
        /// 过滤用户可用物理组
        /// </summary>
        /// <param name="dtObjData"></param>
        /// <returns></returns>
        private DataTable FiltrateUserTypes(DataTable dtObjData)
        {
            if (dtObjData == null || dtObjData.Rows.Count <= 0)
                return dtObjData;//为空,不过滤

            if (IsBindingUserTypes && !UserInfo.isAdmin) //是否绑定用户可用物理组,并且非admin用户
            {
                if (dtObjData.Columns.Contains("com_ctype")) //是否存在列名,存在则过滤
                {
                    DataTable dtObjRv = dtObjData.Clone();//克隆表结构
                    if (UserInfo.sqlUserTypesFilter != string.Empty)//可用物理组是否不为空
                    {
                        //筛选用户可用物理组
                        DataRow[] drObjRv = dtObjData.Select(string.Format("com_ctype in ({0}) or com_ctype=''", UserInfo.sqlUserTypesFilter));
                        for (int i = 0; i < drObjRv.Length; i++)
                        {
                            dtObjRv.ImportRow(drObjRv[i]);
                        }
                        return dtObjRv;//过滤后
                    }
                    else
                    {
                        return dtObjRv;//可用物理组为空,则返回空数据表
                    }
                }
                else
                {
                    return dtObjData;//没列名,不过滤
                }
            }
            else
            {
                return dtObjData;//没绑定,不过滤
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.gridControl1.Name, true);
            dlist.Add(this.gcBCCombines.Name, true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);


            //dlist.Add(gcItemIn.Name, true);


            return dlist;
        }
        #endregion

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            //getTable();


            if (bsCombine.Current != null)
            {
                if (inLoadBCCombines == false)
                {
                    inLoadBCCombines = true;

                    if (bsCombine.Position > -1)
                    {
                        LoadBCCombines();
                        ChangeSeleteType();
                    }

                    inLoadBCCombines = false;
                }
            }
        }
        //选中不同规则是改变条码类型
        public void ChangeSeleteType()
        {
            if (bsBCCombine.Current == null) return;
            EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
            cbBarcodeType.Text = rule.ComBarcodeType.Value == 0 ? "自动条码" : "预置条码";
            if (rule.ComRulId == 0)
                GenerateTypeCode();
        }



        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            this.gridControl1.Enabled = false;
            SetEmbeddedNavigator(false);
        }

        public void Save()
        {

            this.isActionSuccess = false;
            this.bsBCCombine.EndEdit();

            this.bsCombine.EndEdit();

            if (bsCombine.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();

            EntityDicCombine dr = bsCombine.Current as EntityDicCombine;

            String com_id = dr.ComId;

            List<EntitySampMergeRule> rules = bsBCCombine.DataSource as List<EntitySampMergeRule>;
            var listNew = new List<EntitySampMergeRule>();
            var listUpdate = new List<EntitySampMergeRule>();
            if (rules.Count == 0)
            {
                MessageDialog.Show("没有保存的项");
                return;
            }
            foreach (EntitySampMergeRule item in rules)
            {
                if (item.ComRulId == 0)
                    listNew.Add(item);
                else
                    listUpdate.Add(item);
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("new", listNew);
            dict.Add("update", listUpdate);

            request.SetRequestValue(dict);
            EntityResponse result = new EntityResponse();

            result = base.Other(request);
            if (base.isActionSuccess)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
                LoadBCCombines();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }

            if (this.isActionSuccess)
            {
                SetEmbeddedNavigator(false);
            }
        }

        public void Delete()
        {
            this.gvBCCombines.CloseEditor();
            this.bsBCCombine.EndEdit();

            if (bsBCCombine.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
            if (rule.ComRulId == 0)
            {
                bsBCCombine.RemoveCurrent();
                return;
            }
            request.SetRequestValue(rule);

            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult == DialogResult.OK)
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
            isAdd = false;
            this.doRefresh();
        }

        #endregion


        ///// <summary>
        ///// 拆分代码相关的控件Text有变时
        ///// </summary>
        ///// <param name="oldRow"></param>
        //private void AfterSplitCodeControlChange(DataRow oldRow)
        //{
        //    GenerateTypeCode();
        //}


        /// <summary>selectDict_Type2_onAfterChange
        /// 焦点定位到新增项目
        /// </summary>
        private void FocusToNewItem()
        {
            for (int i = 0; i < this.gridView_combine.RowCount; i++)
            {
                DataRow dr = this.gridView_combine.GetDataRow(i);

                if (dr["com_id"].ToString() == string.Empty)
                {

                    this.gridView_combine.FocusedRowHandle = i;
                    break;
                }
            }
        }


        private void GenerateTypeCode()
        {
            string type = FomartSplitSubCode(selectDict_Type2.valueMember);
            string cuv = FomartSplitSubCode(selectDict_Cuv1.valueMember);
            string sampleType = FomartSplitSubCode(selectDict_Sample_type1.valueMember);
            string seq = FomartSplitSubCode(cbeSeq.Text);
            //全部为空时不生成
            if (string.IsNullOrEmpty(selectDict_Type2.valueMember) && string.IsNullOrEmpty(selectDict_Cuv1.valueMember)
                && string.IsNullOrEmpty(selectDict_Sample_type1.valueMember) && string.IsNullOrEmpty(cbeSeq.Text))
            {
                txtSplitCode.EditValue = "";
                if (bsBCCombine.Current != null)
                {
                    EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                    rule.ComSplitCode = "";
                }
            }

            else
            {
                txtSplitCode.EditValue = type + cuv + sampleType + seq.Trim();
                if (bsBCCombine.Current != null)
                {
                    EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                    rule.ComSplitCode = type + cuv + sampleType + seq.Trim();
                }
            }

        }

        /// <summary>
        /// 如果为空时,
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string FomartSplitSubCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "A";
            else
                return code;
        }

        /// <summary>
        /// 新增或删除条码组合
        /// </summary>
        private void gdItemSam_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        /// <summary>
        /// 清除生成拆分条码的控件文本
        /// </summary>
        private void ClearSplitSubCodeControls()
        {
            selectDict_Type2.ClearSelect();
            selectDict_Cuv1.ClearSelect();
            selectDict_Sample_type1.ClearSelect();
            cbeSeq.Text = "";
            txtSplitCode.EditValue = "";
            if (bsBCCombine.Current != null)
            {
                EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                rule.ComExecCode = "";
                rule.ComTubeCode = "";
                rule.ComSamId = "";
                rule.ComSortNo = "";
                rule.ComSplitCode = "";
            }
        }

        #region IBarActionExt 成员

        public void Cancel()
        {
            SetEmbeddedNavigator(false);
            //gridView1_FocusedRowChanged(null, null);
        }

        public void Edit()
        {
            isAdd = false;
            gridControl1.Enabled = false;
            // gcBCCombines.Enabled = true;
            SetEmbeddedNavigator(true);
            //xtraTabControl1.Enabled = true;


            if (gvBCCombines.RowCount == 0)
                gcCombines.Enabled = false;
            else
                gcCombines.Enabled = true;

            ceFatherFlag.Enabled = ceSplit.Checked;


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
            if (bsCombine.Current != null)
            {
                if (inLoadBCCombines == false)
                {
                    inLoadBCCombines = true;

                    if (bsCombine.Position > -1)
                    {
                        LoadBCCombines();
                        ChangeSeleteType();
                    }

                    inLoadBCCombines = false;
                }
            }
        }

        private void LoadBCCombines()
        {
            inLoadBCCombines = true;

            if (bsCombine.Current != null)
            {
                EntityRequest request = new EntityRequest();
                EntityDicCombine dr = bsCombine.Current as EntityDicCombine;


                EntityResponse result = new EntityResponse();

                String comId = dr.ComId;

                request.SetRequestValue(comId);

                result = base.Search(request);

                if (isActionSuccess)
                {
                    mergeRuleList = result.GetResult() as List<EntitySampMergeRule>;
                    this.bsBCCombine.DataSource = mergeRuleList;
                    //this.bsBCCombine.ResetBindings(true);
                    //gcBCCombines.Focus();
                    //gcBCCombines.Refresh();
                }
            }
            else
            {
                //bsBCCombine.Filter = "1<>1";
                bsBCCombine.DataSource = new List<EntitySampMergeRule>();
            }

            inLoadBCCombines = false;
        }

        private void selectDict_Sample_type1_onBeforeFilter(ref string strFilter)
        {
            strFilter += "AND sam_id <> '-1'";
        }

        private void ceSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (ceSplit.Checked) //拆分时清空合并控件
            {
                ClearSplitSubCodeControls();
            }
            else
            {
                //如果不拆分时，清掉‘拆分保存大组合(特殊合并)’标记
                ceFatherFlag.Checked = false;
            }

            //需要拆分时不用显示合并代码
            txtSplitCode.Enabled = selectDict_Cuv1.Enabled = selectDict_Sample_type1.Enabled = cbeSeq.Enabled = selectDict_Type2.Enabled = !ceSplit.Checked;
            ceFatherFlag.Enabled = ceSplit.Checked;//‘拆分保存大组合(特殊合并)’标记

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

        private void btnCombineView_Click(object sender, EventArgs e)
        {
            frmCombineView frm = new frmCombineView(this);
            frm.Show();
        }



        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            EntityDicCombine dr = this.gridView_combine.GetRow(e.RowHandle) as EntityDicCombine;

            if (dr != null)
            {
                if (dr.BarType.ToString() == "1")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                }
            }
        }




        /// <summary>
        /// 选择框发生改变时,刷新
        /// </summary>
        private void radioGroup_split_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.doRefresh();
        }

        /// <summary>
        /// 过滤物理组下拉框数据
        /// </summary>
        /// <param name="strFilter"></param>
        private void selectDict_Type1_onBeforeFilter(ref string strFilter)
        {
            if (!UserInfo.isAdmin && IsBindingUserTypes)//是否绑定用户可用物理组,是则过滤,否则不过滤
            {
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    strFilter += string.Format(" and type_id in ({0}) ", UserInfo.sqlUserTypesFilter);
                }
                else
                {
                    strFilter += " and 1=2";
                }
            }
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            //DataRow drCom = gvBCCombines.GetFocusedDataRow();
            EntitySampMergeRule drCom = gvBCCombines.GetFocusedRow() as EntitySampMergeRule;
            if (drCom == null)
            {
                lis.client.control.MessageDialog.Show("请选择要复制的数据");
                return;
            }

            EntitySampMergeRule drBCom = drCom.Clone() as EntitySampMergeRule;

            bsBCCombine.EndEdit();
            drBCom.ComRulId = 0;
            drBCom.ComHisFeeCode += "复制";

            mergeRuleList.Add(drBCom);
            bsBCCombine.DataSource = mergeRuleList;
            bsBCCombine.ResetBindings(true);
            //
            gvBCCombines.MoveLast();
            bsBCCombine.ResetCurrentItem();
        }

        private void txtSort_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSort.Text.Trim();

            if (filter == "")
                bsCombine.DataSource = combineList;
            else
            {
                bsCombine.DataSource = combineList.Where(w => w.ComId.Contains(filter) ||
                                                        w.ComName.Contains(filter) ||
                                                        w.ComCCode.Contains(filter) ||
                                                        w.ComPyCode.Contains(filter.ToUpper()) ||
                                                        w.ComWbCode.Contains(filter.ToUpper()) ||
                                                        w.ComHisCode != null && w.ComHisCode.Contains(filter) ||
                                                        w.CTypeName != null && w.CTypeName.Contains(filter) ||
                                                        w.PTypeName != null && w.PTypeName.Contains(filter) ||
                                                        w.ComSortNo.ToString() == filter
                                                             ).ToList();
            }
        }

        private void gridView_combine_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //GridView grid = sender as GridView;
            ////DataRow row = grid.GetDataRow(e.RowHandle);
            //EntityDicCombine row = EntityManager<EntityDicCombine>.ConvertToEntity(grid.GetDataRow(e.RowHandle));
            //if (row != null && row.ComUrgentFlag.ToString() == "1")
            //{
            //    e.Appearance.BackColor = Color.Silver;
            //}
        }

        private void AfterSplitCodeControlChange(EntityDicPubProfession oldRow)
        {
            GenerateTypeCode();
        }

        private void selectDict_Cuv1_onAfterChange(EntityDicTestTube oldRow)
        {
            GenerateTypeCode();
        }

        private void gvBCCombines_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ChangeSeleteType();
        }

        private void selectDict_Sample_type1_onAfterChange(EntityDicSample oldRow)
        {
            GenerateTypeCode();
        }

        private void selectDict_Origin1_onAfterSelected(object sender, EventArgs e)
        {
            if (bsBCCombine.Current != null)
            {
                EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                rule.SrcName = selectDict_Origin1.displayMember;
            }
        }

        private void cbeSeq_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerateTypeCode();
        }

        private void cbBarcodeType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (bsBCCombine.Current != null)
            {
                EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                rule.ComBarcodeType = cbBarcodeType.SelectedIndex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //非编辑状态时不做任何操作
            if (!txtHisCombineCode.Enabled)
            {
                return;
            }

            //新增标本
            if (this.bsCombine.Position != -1)
            {
                EntityDicCombine drCombine = bsCombine.Current as EntityDicCombine;

                ////没有费用代码
                //if (drCombine["com_his_fee_code"] == null || drCombine["com_his_fee_code"].ToString() == "")
                //{
                //    lis.client.control.MessageDialog.Show("请输入HIS费用代码!");
                //    txtHISFeeCode.Focus();
                //    return;
                //}
                if (!gcCombines.Enabled)
                    gcCombines.Enabled = true;

                if (drCombine.ComId != null && drCombine.ComId != "")
                {
                    xtraTabControl1.SelectedTabPage = tabBarcodeCombines;
                    EntitySampMergeRule drBCCombine = bsBCCombine.AddNew() as EntitySampMergeRule;
                    //绑定编号与费用类别
                    drBCCombine.ComId = drCombine.ComId;
                    txtComLisCode.Text = drCombine.ComHisCode;
                    drBCCombine.ComHisFeeCode = drCombine.ComHisCode;

                    drBCCombine.ComSplitCode = "";
                    spinPrintCount.EditValue = 1;//打印次数默认为1
                                                 // ceSplit.Checked = true; //拆分标志默认有
                                                 //drBCCombine["com_print_name"] = drCombine["com_name"]; //条码打印名称默认为组合名称
                    if (cbBarcodeType.SelectedIndex > -1) //默认为0:自动条码  为1时是预置条码
                        drBCCombine.ComBarcodeType = cbBarcodeType.SelectedIndex;
                    bsBCCombine.EndEdit();
                    bsBCCombine.ResetCurrentItem();
                    // SetDefaultOrigin();
                    this.cbeSeq.Focus();
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //非编辑状态时不做任何操作
            if (!txtHisCombineCode.Enabled)
            {
                return;
            }

            //删除
            if (gvBCCombines.RowCount == 1)
                gcCombines.Enabled = false;
            Delete();
        }
    }
}
