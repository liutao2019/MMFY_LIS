using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.dicbasic
{
    public partial class ConItemPro : ConDicCommon, IBarActionExt
    {
        #region 全局变量

        private List<EntityDicItmItem> list = new List<EntityDicItmItem>();
        private List<EntityDicItmRefdetail> listreft = new List<EntityDicItmRefdetail>();
        private List<EntityDicItemSample> listsamp = new List<EntityDicItemSample>();
        #endregion
        public ConItemPro()
        {
            InitializeComponent();
        }

        bool isTrue = true;//标明金额是否显示到项目一级中
        bool isChanged = false;//是否执行改变时间标志
        bool isEdit = false;//纪律状态

        /// <summary>
        /// 当前选中的项目ID，用于刷新后重新选中该项目
        /// </summary>
        string currentItemID = null;

        /// <summary>
        /// 是否绑定用户可用专业组
        /// </summary>
        private bool IsBindingUserTypeSp { get; set; }

        /// <summary>
        /// 放弃
        /// </summary>
        public void Cancel()
        {
            //主页面放弃事件中调用了刷新的方法
            if (!isEdit)
            {
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        { }

        /// <summary>
        ///下一个
        /// </summary>
        public void MoveNext()
        { }

        /// <summary>
        /// 上一个
        /// </summary>
        public void MovePrev()
        { }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            
        }

        ColumnFilterInfo cfiEcd = new ColumnFilterInfo();
        ColumnFilterInfo cfiName = new ColumnFilterInfo();
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        ColumnFilterInfo cfiRepEcd = new ColumnFilterInfo();

        /// <summary>
        /// 新增
        /// </summary>
        public void Add()
        {
            isEdit = false;
            EntityRequest request = new EntityRequest();
            EntityDicItmItem dr = new EntityDicItmItem();
            dr.ItmMicrType = "0";
            dr.ItmChargeFlag = 0;
            dr.ItmQcFlag = 0;
            dr.ItmPrtFlag = 1;
            dr.ItmCaluFlag = 0;
            dr.ItmInfectionFlag = 0;
            dr.ItmRepCode = "0";
            dr.ItmQcType = 0;//质评标志 0-不需要 1-需要
            dr.ItmStartDate = ServerDateTime.GetServerDateTime();
            dr.ItmEndDate = ServerDateTime.GetServerDateTime().AddYears(100);//默认项目终止日期为100年后
            dr.ItmDelFlag = LIS_Const.del_flag.OPEN;

            ConItemProInfo INFO = new ConItemProInfo();


            INFO.LoadItem(dr);
            if (INFO.ShowDialog() == DialogResult.OK)
            {
                DoRefresh();
                List<EntityDicItmItem> listItm = (List<EntityDicItmItem>)bsItem.DataSource;
                int index =  listItm.FindIndex(w => w.ItmName == INFO.itm.ItmName);
                gvItem.FocusedRowHandle = index;
            }
            var pForm = GetParentForm();
            if (pForm != null)
            {
                pForm.EnableButton();
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public void Edit()
        {
            EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;

            ConItemProInfo INFO = new ConItemProInfo();


            INFO.LoadItem(dr);
            //INFO.ShowDialog();

            if (INFO.ShowDialog() == DialogResult.OK)
            {
                //DataRow row = gvItem.GetFocusedDataRow();
                //dtItem.Rows[dtItem.Rows.IndexOf(row)].ItemArray = INFO.curRow.ItemArray;
                LoadItemSam();
                LoadItemMi();
            }
            var pForm = GetParentForm();

            if (pForm != null)
            {
                pForm.EnableButton();
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
        /// 删除项目
        /// </summary>
        public void Delete()
        {
            if (lis.client.control.MessageDialog.Show("确定要设置该项目为已删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.bsItem.EndEdit();
                this.bsItemSam.EndEdit();
                this.bsItemMi.EndEdit();
                if (bsItem.Current == null)
                {
                    return;
                }
                List<EntityDicItemSample> etUpdateSam = new List<EntityDicItemSample>();
                List<EntityDicItmRefdetail> etUpdateMi = new List<EntityDicItmRefdetail>();
                EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;
                String itm_id = dr.ItmId.ToString();

                dr.ItmDelFlag = "1";//新代码：设置删除标志
                #region 更新删除标志同步项目标本与项目参考值

                if (bsItemSam.DataSource != null)
                {
                    int intRows = ((List<EntityDicItemSample>)bsItemSam.DataSource).Count;
                    if (intRows > 0)
                    {
                        EntityDicItemSample drTemp = null;
                        for (int i = 0; i < intRows; i++)
                        {
                            drTemp = ((List<EntityDicItemSample>)bsItemSam.DataSource)[i];
                            drTemp.ItmDelFlag = "1";
                            etUpdateSam.Add(drTemp);
                        }
                    }
                }

                if (bsItemMi.DataSource != null)
                {
                    int intRows = ((List<EntityDicItmRefdetail>)bsItemMi.DataSource).Count;
                    if (intRows > 0)
                    {
                        EntityDicItmRefdetail drTemp = null;
                        for (int i2 = 0; i2 < intRows; i2++)
                        {
                            drTemp = ((List<EntityDicItmRefdetail>)bsItemMi.DataSource)[i2];
                            drTemp.ItmDelFlag = "1";
                            etUpdateMi.Add(drTemp);
                        }
                    }
                }
                #endregion

                //ds.Tables.Add(CreateOperateInfo("删除"));
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Item", dr);
                dict.Add("Sam", etUpdateSam);
                dict.Add("Detail", etUpdateMi);
                EntityRequest request = new EntityRequest();
                //DataSet result = new DataSet();
                if (itm_id == "")
                {
                    bsItem.Remove(bsItem.Current);
                }
                else
                {
                    EntitySysOperationLog etOperation = CreateOperateInfo("修改");
                    dict.Add("Operation", etOperation);
                    request.SetRequestValue(dict);
                    EntityResponse result = base.Update(request);
                    if (base.isActionSuccess)
                    {
                        //bsItem.Remove(bsItem.Current);//旧代码：删除后移除
                        bsItem.Remove(bsItem.Current);
                    }
                }

                LoadItemSam();
                LoadItemMi();
            }
            gdItem.Enabled = true;
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
        /// <summary>
        /// 刷新
        /// </summary>
        public void DoRefresh()
        {
            logItemChanged = false;

            this.isActionSuccess = true;
            LoadItem();

            //避免重复触发事件,仅在父级列表为空时清空子级使用
            if (bsItem.Count == 0)
            {
                LoadItemSam();
            }
            //if (bsItemSam.Count == 0)
            //{
            //    LoadItemMi();
            //}
            gdItem.Enabled = true;
        
            //panel1.Enabled = true;

            logItemChanged = true;

            FocusToItem();
            Filter();
        }

        /// <summary>
        /// 非编辑状态下可用控件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gdItem", true);
            dlist.Add("gdItemSam", true);
            dlist.Add("gdItemMi", true);
            return dlist;
        }

        /// <summary>
        /// 用户控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConItemPro_Load(object sender, EventArgs e)
        {
            //[项目字典]是否关联用户可用专业组
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypeSp") == "是")
            {
                IsBindingUserTypeSp = true;
            }
            else
            {
                IsBindingUserTypeSp = false;//默认不绑定用户可用专业组
            }
            //sysItem.SetToolButtonStyle(new string[] { sysItem.BtnDeRef.Name, sysItem.BtnDeSpe.Name });
           
            lookSex.DataSource = getSex();
           
           
            colitm_pri.Visible = !isTrue;
            colitm_cost.Visible = !isTrue;


            if (UserInfo.GetSysConfigValue("seq_visible") == "是")
            {
                colitm_seq.Visible = true;
                colitm_seq.VisibleIndex = 0;
                colitm_id.Visible = false;
            }
          
            DoRefresh();

        }

        /// <summary>
        /// 不知什么原因和系统缓存里的性别偶尔会发生冲突(缓存里sex存在但无数据),先用单独的
        /// </summary>
        /// <returns></returns>
        private DataTable getSex()
        {
            DataTable result = new DataTable("sex");
            result.Columns.Add("id");
            result.Columns.Add("value");
            result.Rows.Add(new Object[] { "0", "" });
            result.Rows.Add(new Object[] { "1", "男" });
            result.Rows.Add(new Object[] { "2", "女" });

            return result;
        }


        DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[] filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];

        private void GetViewColumnFilterInfo()
        {
            DevExpress.XtraGrid.Views.Base.ViewFilter xxx = gvItem.SortInfo.View.ActiveFilter;

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
                if (filter[j] != null)
                    gvItem.SortInfo.View.ActiveFilter.Add(filter[j]);
            }

            filter = new DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo[5];

        }

        /// <summary>
        /// 读取项目列表
        /// </summary>
        private void LoadItem()
        {
            List<EntityDicItmItem> listItm = new List<EntityDicItmItem>(); 
            EntityRequest request = new EntityRequest();
            this.isActionSuccess = true;
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                GetViewColumnFilterInfo();
                list = ds.GetResult() as List<EntityDicItmItem>;
                this.bsItem.DataSource = list;
                SetViewColumnFilterInfo();
            }
        }

        /// <summary>
        /// 筛选项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSort_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }


        private void Filter()
        {
            List<EntityDicItmItem> listItm = new List<EntityDicItmItem>();
            listItm = list;
            string strDelFilter = string.Empty;
            //选择项目停用、启用过滤
            string filter = txtSort.Text.Trim();
            if (radioGroup_Item.EditValue.ToString() == "1")
            {
                strDelFilter = LIS_Const.del_flag.OPEN;

            }
            else if (radioGroup_Item.EditValue.ToString() == "2")
            {
                strDelFilter = LIS_Const.del_flag.DEL;
            }
            if (!string.IsNullOrEmpty(strDelFilter))
            {
                listItm = listItm.Where(w => w.ItmDelFlag.Contains(strDelFilter)).ToList();
            }
            if (!string.IsNullOrEmpty(filter))
            {
                listItm = listItm.Where(w => w.ItmId.Contains(filter) ||
                                                         w.ItmName != null && w.ItmName.Contains(filter) ||
                                                         w.ItmEcode.Contains(filter.ToUpper()) || 
                                                         w.ItmPyCode != null && w.ItmPyCode.Contains(filter.ToUpper()) ||
                                                         w.ItmWbCode != null && w.ItmWbCode.Contains(filter.ToUpper()) ||
                                                         w.ItmSortNo.ToString() != null && w.ItmSortNo.ToString() == filter).ToList();
            }
            this.bsItem.DataSource = listItm;
        }


        /// <summary>
        /// 时间转换函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public int GetDateValue(int value, string unit)
        {

            int yearUnit = 12;
            int monthUnit = 30;
            int hourUnit = 24;
            int minuteUnit = 60;

            int num;

            switch (unit)
            {
                case "岁":
                    {
                        num = value * yearUnit * monthUnit * hourUnit * minuteUnit;
                        break;
                    }
                case "月":
                    {
                        num = value * monthUnit * hourUnit * minuteUnit;
                        break;
                    }
                case "天":
                    {
                        num = value * hourUnit * minuteUnit;
                        break;
                    }
                default:
                    {
                        //时
                        num = value * minuteUnit;
                        break;
                    }
            }

            return num;
        }



        bool inLoadItemSam = false;
        /// <summary>
        /// 读取标本信息
        /// </summary>
        /// <param name="itm_id"></param>
        private void LoadItemSam()
        {
            inLoadItemSam = true;

            if (bsItem.Position > -1)
            {
                EntityRequest request = new EntityRequest();
                EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;
                request.SetRequestValue(dr);
                EntityResponse ds = base.Other(request);
                EntityResponse dsdetail = base.View(request);
                //result = base.doOther(ds);
                if (isActionSuccess)
                {
                    listsamp = ds.GetResult() as List<EntityDicItemSample>;
                    bsItemSam.DataSource =listsamp;
                    listreft = dsdetail.GetResult() as List<EntityDicItmRefdetail>;
                    foreach (EntityDicItmRefdetail refdetail in listreft)
                    {
                        List<EntityDicItemSample> list = listsamp.Where(w => w.ItmSamId == refdetail.ItmSamId && w.ItmItrId == refdetail.ItmItrId).ToList();
                        if (list.Count > 0)
                        {
                            refdetail.ItmSort = list[0].ItmSort;
                        }
                    }
                    bsItemMi.DataSource = listreft;
                }
                bsItemSam.Filter = "1=1";
            }
            else
            {
                bsItemSam.Filter = "1<>1";
            }

            //避免重复设置Filter造成界面刷新
            if (bsItemSam.Count == 0)
            {
                bsItemMi.Filter = "1<>1";
            }

            inLoadItemSam = false;
        }

        /// <summary>
        /// 读取参考值信息
        /// </summary>
        /// <param name="itm_id"></param>
        private void LoadItemMi()
        {
            inLoadItemSam = true;
            if (bsItem.Position > -1 && bsItemSam.Position > -1 && bsItemSam.Current != null)
            {
                EntityDicItemSample samp = (EntityDicItemSample)bsItemSam.Current;

                if (samp.ItmSamId != null && samp.ItmSamId != "")
                {
                    string itm_sam_id = samp.ItmSamId;
                    string itm_itr_id = samp.ItmItrId.Trim() == string.Empty ? "-1" : samp.ItmItrId.Trim();
                    int itm_sort = samp.ItmSort;
                    bsItemMi.DataSource = listreft.Where(w => w.ItmSamId==itm_sam_id
                                                                             && w.ItmItrId==itm_itr_id && w.ItmSort==itm_sort ).ToList();
                }
                else
                {
                    bsItemMi.Filter = "1<>1";
                }
            }
            else
            {
                bsItemMi.Filter = "1<>1";
            }
            inLoadItemSam = false;
        }

       

        /// <summary>
        /// 控制录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsItemSam_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
        }

        /// <summary>
        /// 控制录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsItemMi_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
        }

        private void bsItem_CurrentChanged(object sender, EventArgs e)
        {
            this.isActionSuccess = true;
            if (inLoadItemSam == false)
            {
                inLoadItemSam = true;

                if (bsItem.Position > -1)
                {
                    LoadItemSam();
                    LoadItemMi();
                }

                inLoadItemSam = false;
            }

            if (logItemChanged)
            {
                if (this.bsItem.Current != null)
                {
                    EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;
                    currentItemID = dr.ItmId;
                }
                else
                {
                    currentItemID = null;
                }
            }
        }

        bool logItemChanged = true;
        /// <summary>
        /// 焦点定位到当前选中的项目
        /// </summary>
        private void FocusToItem()
        {
            //for (int i = 0; i < this.gvItem.RowCount; i++)
            //{
            //    DataRow dr = this.gvItem.GetDataRow(i);
            //    if (currentItemID == dr["itm_id"].ToString())
            //    {

            //        this.gvItem.FocusedRowHandle = i;
            //        //this.OnPatientChanged(dr["pat_id"].ToString(), dr);

            //        break;
            //    }
            //}
        }


        private void bsItemSam_CurrentChanged(object sender, EventArgs e)
        {
            if (inLoadItemSam == false)
            {
                inLoadItemSam = true;

                if (bsItem.Position > -1 && bsItemSam.Position > -1)
                {
                    LoadItemMi();
                  
                }
                

                inLoadItemSam = false;
            }
        }

        private void bsItemMi_CurrentChanged(object sender, EventArgs e)
        {
            if (bsItemMi.Current != null)
            {
                //DataRowView dr = (DataRowView)bsItemMi.Current;
                //this.txtItmRefStages.displayMember = dr.Row["itm_ref_stages"].ToString();
                //txtItmRefStages.ValueChanged -= new HopePopSelect.ValueChangedEventHandler(txtItmRefStages_ValueChanged);
                //this.txtItmRefStages.valueMember = dr.Row["itm_ref_stages"].ToString();
                //txtItmRefStages.ValueChanged += new HopePopSelect.ValueChangedEventHandler(txtItmRefStages_ValueChanged);
            }
        }



        private void radioGroup_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }
       

        private void gvItem_DoubleClick(object sender, EventArgs e)
        {
            if (bsItem.Current != null) 
            Edit();
        }

        private void gvItemSam_DoubleClick(object sender, EventArgs e)
        {
            if (bsItem.Current != null)
            {
                EntityDicItmItem dr = (EntityDicItmItem)bsItem.Current;

                ConItemProInfo INFO = new ConItemProInfo();
                INFO.LoadItem(dr);
                INFO.IsSam = true;
                if (INFO.ShowDialog() == DialogResult.OK)
                {
                    EntityDicItmItem row = (EntityDicItmItem)bsItem.Current;
                    LoadItemSam();
                    LoadItemMi();
                }
                var pForm = GetParentForm();

                if (pForm != null)
                {
                    pForm.EnableButton();
                }
            }
        }

        private void gvItemMi_DoubleClick(object sender, EventArgs e)
        {
            if (gvItem.GetFocusedDataRow() != null)
            {
                DataTable dtItem = (DataTable)bsItem.DataSource;

                ConItemProInfo INFO = new ConItemProInfo();

                //INFO.LoadItem(dtItem, ((DataRowView)bsItem.Current).Row);
                //INFO.ShowDialog();
                INFO.IsRef = true;
                if (INFO.ShowDialog() == DialogResult.OK)
                {
                    EntityDicItmItem row = (EntityDicItmItem)bsItem.Current;
                    //dtItem.Rows[dtItem.Rows.IndexOf(row)].ItemArray = INFO.curRow.ItemArray;

                    LoadItemSam();
                    LoadItemMi();
                }


                var pForm = GetParentForm();

                if (pForm != null)
                {
                    pForm.EnableButton();
                }
            }
        }

    }
}
