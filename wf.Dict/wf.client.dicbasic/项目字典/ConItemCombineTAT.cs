using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using dcl.entity;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConItemCombineTAT : ConDicCommon, IBarActionExt
    {

        dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicCombine> list = new List<EntityDicCombine>();
        private List<EntityDicCombineTimeRule> RuleIn = new List<EntityDicCombineTimeRule>();
        private List<EntityDicCombineTimeRule> RuleNotIn = new List<EntityDicCombineTimeRule>();
        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        public ConItemCombineTAT()
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

                
                if (UserInfo.GetSysConfigValue("seq_visible") == "是" || UserInfo.GetSysConfigValue("seq_visibleForCombine") == "是")
                {
                    colcom_seq.Visible = true;
                    colcom_seq.VisibleIndex = 0;
                    colcom_id.Visible = false;
                   
                }
            }
            finally
            {
                // this.lstRoleLeft.SelectedValueChanged += this.lstRoleLeft_SelectedIndexChanged;
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

        /// <summary>
        /// 过滤用户可用物理组
        /// </summary>
        /// <param name="dtObjData"></param>
        /// <returns></returns>
        private List<EntityDicCombine> FiltrateUserTypes(List<EntityDicCombine> dtObjData)
        {
            List<EntityDicCombine> result = new List<EntityDicCombine>();


            if (dtObjData == null || dtObjData.Count <= 0)
                return dtObjData;//为空,不过滤

            if (IsBindingUserTypes && !UserInfo.isAdmin) //是否绑定用户可用物理组,并且非admin用户
            {
                result = dtObjData.Where(w => UserInfo.sqlUserTypesFilter.Contains(w.ComLabId)).ToList();
                return result;
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
            dlist.Add(this.gcComTimeNotIn.Name, false);
            dlist.Add(this.gcComTimeIn.Name, false);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);


            //dlist.Add(gcItemIn.Name, true);


            return dlist;
        }

      
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           

            if (bsCombine.Current != null)
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
        }




      

        #region IBarAction 成员

        bool isAdd = false;

        public void Add()
        {
            isAdd = true;
            this.gridControl1.Enabled = false;
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
            List<EntityDicCombineTimeRule> listComTime = (List<EntityDicCombineTimeRule>)gcComTimeIn.DataSource;
            Dictionary<string, object> d = new Dictionary<string, object>();
            List<EntityDicCombineTimeruleRelated> listRelated = new List<EntityDicCombineTimeruleRelated>();

            if (listComTime != null)
            {
                foreach (var listTime in listComTime)
                {
                    EntityDicCombineTimeruleRelated entityRelated = new EntityDicCombineTimeruleRelated();
                    entityRelated.ComId = com_id;
                    entityRelated.ComTimeId = listTime.ComTimeId;
                    listRelated.Add(entityRelated);
                }

            }
            d.Add("Combine", dr);
            d.Add("Related", listRelated);
            EntityRequest request = new EntityRequest();
            this.isActionSuccess = true;
            request.SetRequestValue(d);
            EntityResponse ds = base.Update(request);
            if (base.isActionSuccess)
            {
                LoadBCCombines();
            }
            else
            {
                throw new Exception();
            }
        }

        public void Delete()
        {
           
        }

        public void DoRefresh()
        {
            gcComTimeIn.Enabled = false;
            gcComTimeNotIn.Enabled = false;
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
            gcComTimeIn.Enabled = false;
            gcComTimeNotIn.Enabled = false;
        }

        public void Edit()
        {
            isAdd = false;
            gridControl1.Enabled = false;
            gcComTimeIn.Enabled = true;
            gcComTimeNotIn.Enabled = true;
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

        private void LoadBCCombines()
        {
            inLoadBCCombines = true;

            if (bsCombine.Position > -1)
            {
               
                EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dr);
                EntityResponse ds = base.Other(request);
                if (isActionSuccess)
                {
                    Dictionary<string, object> dict = ds.GetResult() as Dictionary<string, object>;
                    List<EntityDicCombineTimeRule> listRule = new List<EntityDicCombineTimeRule>();
                    List<EntityDicCombineTimeruleRelated> listRelated = new List<EntityDicCombineTimeruleRelated>();
                    object objRule = dict["Rule"];
                    object objRelated = dict["Related"];
                    if (objRule != null)
                    {
                          listRule = objRule as List<EntityDicCombineTimeRule>;
                    }
                    if (objRelated != null)
                    {
                         listRelated = objRelated as List<EntityDicCombineTimeruleRelated>;
                    }
                    RuleIn = listRule.Where(p => listRelated.Where(g => g.ComTimeId == p.ComTimeId).Any()).ToList();
                    RuleNotIn = listRule.Where(p => !listRelated.Where(g => g.ComTimeId == p.ComTimeId).Any()).ToList();
                    gcComTimeNotIn.DataSource = RuleNotIn;
                    gcComTimeIn.DataSource = RuleIn;
                    gvComTimeIn.ExpandAllGroups();
                    gvComTimeNotIn.ExpandAllGroups();
                }
            }
           

            inLoadBCCombines = false;
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

  
 
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //DataRow dr = this.gridView_combine.GetDataRow(e.RowHandle);
            //EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
            //if (dr != null)
            //{
            //    if (dr.BarType == "1")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
            //    }
            //}
        }

      

        private void btnComTimeAdd_Click(object sender, EventArgs e)
        {
            EntityDicCombineTimeRule drComTime = (EntityDicCombineTimeRule)gvComTimeNotIn.GetFocusedRow();
            if (drComTime == null)
                return;
            List<EntityDicCombineTimeRule> dtComTimeIn = gcComTimeIn.DataSource as List<EntityDicCombineTimeRule>;
            dtComTimeIn.Add(drComTime);
            List<EntityDicCombineTimeRule> dtComTimeNotIn = gcComTimeNotIn.DataSource as List<EntityDicCombineTimeRule>;
            dtComTimeNotIn.Remove(drComTime);
            gcComTimeNotIn.DataSource = dtComTimeNotIn;
            gcComTimeIn.DataSource = dtComTimeIn;
            gcComTimeNotIn.RefreshDataSource();
            gcComTimeIn.RefreshDataSource();
            gvComTimeIn.ExpandAllGroups();
            gvComTimeNotIn.ExpandAllGroups();

        }

        private void btnComTimeDel_Click(object sender, EventArgs e)
        {
            EntityDicCombineTimeRule drComTime = (EntityDicCombineTimeRule)gvComTimeIn.GetFocusedRow();
            if (drComTime == null)
                return;
            List<EntityDicCombineTimeRule> dtComTimeNotIn = gcComTimeNotIn.DataSource as List<EntityDicCombineTimeRule>;
            dtComTimeNotIn.Add(drComTime);
            List<EntityDicCombineTimeRule> dtComTimeIn = gcComTimeIn.DataSource as List<EntityDicCombineTimeRule>;
            dtComTimeIn.Remove(drComTime);
            gcComTimeNotIn.RefreshDataSource();
            gcComTimeIn.RefreshDataSource();
            gvComTimeIn.ExpandAllGroups();
            gvComTimeNotIn.ExpandAllGroups();
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
            //EntityDicCombine dr = (EntityDicCombine)bsCombine.Current;
            //if (dr != null && dr.ComUrgentFlag.ToString() == "1")
            //{
            //    e.Appearance.BackColor = Color.Silver;
            //}
        }

    }
}
