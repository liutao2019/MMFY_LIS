using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using dcl.entity;
using lis.client.control;
using System.Linq;
using dcl.root.logon;

namespace dcl.client.dicbasic
{
    public partial class ConMitm_No : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool bFilter = false;

        private bool isSaveFail = false;  //用作保存失败时的记录标签

        private List<EntityDicMachineCode> list = new List<EntityDicMachineCode>();

        private List<EntityDicItmItem> itmList = new List<EntityDicItmItem>();
        #region IBarAction 成员

        public void Add()
        {
            //项目根据仪器的专业组别过滤
            if (itmList.Count > 0)
            {
                EntityDicInstrument drIns = bsInstrmt.Current as EntityDicInstrument;

                if (drIns != null)
                {
                    string itr_pro_id = drIns.ItrProId;//获取专业组ID

                    List<EntityDicItmItem> itmProIdList = new List<EntityDicItmItem>();
                    itmProIdList = itmList.Where(w => w.ItmPriId == itr_pro_id).ToList();

                    this.bsItem.DataSource = itmProIdList;
                }
            }

            bFilter = true;//标记为新增事件

            Editable(gridView1, true); //改变GridView的编辑状态

            this.bsBscript.EndEdit();
            EntityDicMachineCode dr = (EntityDicMachineCode)bsBscript.AddNew();
            dr.MacId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;

            EntityDicInstrument itrID = gridView2.GetFocusedRow() as EntityDicInstrument;
            if (itrID == null)
            {
                MessageBox.Show("请选择新增的对象！");
            }
            dr.ItrId = itrID.ItrId;

            //dr.ItrId= gridView2.GetFocusedDataRow()["itr_id"].ToString();

            BtnResultView.Enabled = false;

            //this.GetTable(bFilter);
            this.gridControl2.Enabled = false;
        }

        public void Save()
        {
            Editable(gridView1, false);

            this.bsBscript.EndEdit();
            bFilter = false;
            // this.GetTable(bFilter);

            if (bsBscript.Current == null)
            {
                return;
            }

            //for (int i = 0; i < gridView1.RowCount; i++)
            //{
            //    DataRow row = gridView1.GetDataRow(i);
            //    if (row["mit_cno"].ToString() == "" || row["mit_itr_id"].ToString() == "")
            //    {
            //        lis.client.control.MessageDialog.Show("通道码或项目不能为空", "提示");
            //        DoRefresh();
            //        return;
            //    }
            //}

            //DataTable dtChange = dtItem.GetChanges();
            //if (dtChange == null)
            //{
            //    return;
            //}
            //DataSet dsChange = new DataSet();
            //dsChange.Tables.Add(dtChange);

            //base.doOther(dsChange);//保存插入的数据

            //EntityDicMachineCode dr = (EntityDicMachineCode)bsBscript.Current;
            List<EntityDicMachineCode> dt = bsBscript.DataSource as List<EntityDicMachineCode>;

            //用来判断小数位输入值不能超过10（含）以上
            foreach (var info in dt)
            {
                if (info.MacDecPlace >= 10)
                {
                    MessageBox.Show("通道码为'" + info.MacCode + "' 的小数位的值需小于或等于9!", "提示");
                    isActionSuccess = true;
                    isSaveFail = true;
                    return;
                }
            }

            int k = 0;
            foreach (var dr in dt)
            {
                EntityRequest request = new EntityRequest();
                String type_id = dr.MacId;

                request.SetRequestValue(dr);

                EntityResponse result = new EntityResponse();
                if (type_id == "")
                {
                    result = base.New(request);
                }
                else
                {
                    result = base.Update(request);
                }

                if (base.isActionSuccess)
                {
                    k++;
                    if (type_id == "")
                    {
                        dr.MacId = result.GetResult<EntityDicMachineCode>().MacId;
                    }
                    //MessageDialog.ShowAutoCloseDialog("保存成功");
                }
                //else
                //{
                //MessageDialog.ShowAutoCloseDialog("保存失败");
                //}
            }
            if (k == dt.Count)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (bFilter)
            {
                bFilter = false;//取消新增事件
                this.gridControl1.Enabled = true;
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicMachineCode dr = (EntityDicMachineCode)bsBscript.Current;
            String mit_id = dr.MacId;

            request.SetRequestValue(dr);

            //DataTable dtUpdate = this.dtItem.Clone();
            //DataRow[] drDel = dtItem.Select(string.Format("mit_id = '{0}'", mit_id));

            //if (drDel == null || drDel.Length == 0)
            //{
            //    lis.client.control.MessageDialog.Show("无法定位当前数据!");
            //    return;
            //}

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
        }

        public void DoRefresh()
        {
            EntityResponse ds = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicMachineCode>;
                bsBscript.DataSource = list;

                this.GetTable(bFilter);
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl2", true);
            dlist.Add(btnCopy.Name, true);
            dlist.Add(BtnResultView.Name, true);

            return dlist;
        }
        #endregion
        private DataTable dtItem = new DataTable();
        private DataTable dtItemDetail = new DataTable();

        private DataTable dtAll = new DataTable();
        string strItr = string.Empty;

        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        public ConMitm_No()
        {
            InitializeComponent();
            this.Name = "ConMitm_No";

            //[项目字典]是否关联用户可用物理组
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypes") == "是")
            {
                IsBindingUserTypes = true;
            }
            else
            {
                IsBindingUserTypes = false;//默认不绑定用户可用物理组
            }

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView2, _gridLocalizer);

            EntityResponse instrut = base.Other(new EntityRequest());
            List<EntityDicInstrument> listInstru = new List<EntityDicInstrument>();
            listInstru = instrut.GetResult() as List<EntityDicInstrument>;
            bsInstrmt.DataSource = FiltrateUserTypes(listInstru);

            this.GetTable(bFilter);

            EntityResponse item = base.View(new EntityRequest());
            List<EntityDicItmItem> itmCombine = new List<EntityDicItmItem>();
            itmCombine = item.GetResult() as List<EntityDicItmItem>;
            itmCombine = itmCombine.Where(w => w.ItmDelFlag == "0").ToList(); //过滤掉停用的项目
            itmList = itmCombine; //给全局变量赋值
            this.bsItem.DataSource = itmCombine;

        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            Editable(gridView1, false);

            if (UserInfo.GetSysConfigValue("seq_visible") == "是")
            {
                colitr_seq.Visible = true;
                colitr_seq.VisibleIndex = 0;
                colitr_id.Visible = false;
            }
        }

        /// <summary>
        /// 改变GridView编辑状态
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="enable"></param>
        private void Editable(GridView gridView, bool enable)
        {
            foreach (GridColumn column in gridView.Columns)
            {
                //不影响ID列和仪器列
                if (column.Name == "colmit_id" || column.Name == "colmit_itr_id")
                    continue;

                column.OptionsColumn.AllowEdit = enable;
            }

        }

        private void initData()
        {
            this.DoRefresh();
        }

        /// <summary>
        /// 过滤用户可用物理组
        /// </summary>
        /// <param name="dtObjData"></param>
        /// <returns></returns>
        private List<EntityDicInstrument> FiltrateUserTypes(List<EntityDicInstrument> dtObjData)
        {
            List<EntityDicInstrument> result = new List<EntityDicInstrument>();


            if (dtObjData == null || dtObjData.Count <= 0)
                return dtObjData;//为空,不过滤

            if (IsBindingUserTypes && !UserInfo.isAdmin) //是否绑定用户可用物理组,并且非admin用户
            {
                result = dtObjData.Where(w => UserInfo.sqlUserTypesFilter.Contains(w.ItrLabId)).ToList();
                return result;
            }
            else
            {
                return dtObjData;//没绑定,不过滤
            }
        }

        #region 响应按钮菜单点击事件



        private void toModify()
        {
            Editable(gridView1, true);
            BtnResultView.Enabled = false;
            bFilter = true;
            if (!isSaveFail)
            {
                this.GetTable(bFilter);

                //项目根据仪器的专业组别过滤
                if (itmList.Count > 0)
                {
                    EntityDicInstrument drIns = bsInstrmt.Current as EntityDicInstrument;

                    if (drIns != null)
                    {
                        string itr_pro_id = drIns.ItrProId;//获取专业组ID

                        List<EntityDicItmItem> itmProIdList = new List<EntityDicItmItem>();
                        itmProIdList = itmList.Where(w => w.ItmPriId == itr_pro_id).ToList();

                        this.bsItem.DataSource = itmProIdList;
                    }
                }
            }
            this.gridControl2.Enabled = false;
        }
        #endregion

        private void buttonEdit2_Modified(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit beObject = sender as DevExpress.XtraEditors.ButtonEdit;
            if (Convert.ToUInt32(beObject.Text) > 0 && Convert.ToUInt32(beObject.Text) < 10)
            {

            }
            else
            {
                beObject.Text = "";
            }

        }

        private void GetTable(bool bFilter)
        {
            string itr_id = "0";
            string itr_pro_id = "";

            try
            {
                //EntityDicInstrument drIns = gridView2.GetFocusedRow() as EntityDicInstrument;
                EntityDicInstrument drIns = bsInstrmt.Current as EntityDicInstrument;
                if (drIns == null) return;

                itr_id = drIns.ItrId;
                itr_pro_id = drIns.ItrProId;//获取专业组ID
                //if (drIns != null && bFilter)
                //{
                //    string itr_ptype = drIns.ItrTypeName;
                //}
                //else
                //{
                //    bsItem.Filter = " 1=1";
                //}
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.ToString());
            }

            EntityResponse instrut = base.Search(new EntityRequest());
            List<EntityDicMachineCode> list2 = new List<EntityDicMachineCode>();

            list2 = instrut.GetResult() as List<EntityDicMachineCode>;

            bsBscript.DataSource = list2.Where(w => w.ItrId.Contains(itr_id)).ToList();

            this.bsItem.DataSource = itmList;//给项目赋值

        }
        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Add();
            }
        }

        private void ConMitm_No_KeyPress(object sender, KeyPressEventArgs e)
        {

        }


        #region IBarActionExt 成员

        public void Cancel()
        {
            Editable(gridView1, false);
            bFilter = false;
            this.GetTable(bFilter);
        }

        public void Edit()
        {
            this.toModify();
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

        private void repositoryItemLookUpEdit4_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lue = (LookUpEdit)sender;
            if (lue != null)
            {
                if (bsBscript.Current != null)
                {
                    //DataRow dr = ((DataRowView)bsBscript.Current).Row;
                    //dr["mit_itm_ecd"] = lue.Text;
                    EntityDicMachineCode dr = bsBscript.Current as EntityDicMachineCode;
                    dr.MacItmEcd = lue.Text;
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //if (gridView2.GetFocusedDataRow() == null)
            //{
            //    lis.client.control.MessageDialog.Show("请选择仪器！");
            //    return;
            //}

            //FrmMitmOrAdjustCopy frmMitm = new FrmMitmOrAdjustCopy();
            //frmMitm.TypeName = "仪器通道";
            //frmMitm.OriItrId = gridView2.GetFocusedDataRow()["itr_id"].ToString();
            //frmMitm.ShowDialog();

            EntityDicInstrument copyIns = gridView2.GetFocusedRow() as EntityDicInstrument;
            if (copyIns == null)
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }

            FrmMitmOrAdjustCopy frmMitm = new FrmMitmOrAdjustCopy();
            frmMitm.TypeName = "仪器通道";
            frmMitm.OriItrId = copyIns.ItrId;
            frmMitm.ShowDialog();
        }

        private void BtnResultView_Click(object sender, EventArgs e)
        {
            #region 旧代码
            //FrmDictMitmNoResultView frm = new FrmDictMitmNoResultView();
            //if (frm.ShowDialog() == DialogResult.Yes)
            //{
            //    strItr = frm.Itr_id;
            //    //dtCno = frm.ReturnData;

            //    for (int i = 0; i < gridView2.RowCount; i++)
            //    {
            //        DataRow row = gridView2.GetDataRow(i);
            //        if (row != null && row["itr_id"].ToString() == strItr)
            //        {
            //            gridView2.FocusedRowHandle = i;
            //            break;
            //        }
            //    }

            //    DoRefresh();
            //}
            #endregion

            FrmDictMitmNoResultView frm = new FrmDictMitmNoResultView();
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                strItr = frm.Itr_id;
                List<EntityDicInstrument> listIns = bsInstrmt.DataSource as List<EntityDicInstrument>;
                int i = 0;
                foreach (EntityDicInstrument instrument in listIns)
                {
                    if (instrument != null && instrument.ItrId == strItr)
                    {
                        gridView2.FocusedRowHandle = i;
                        break;
                    }
                    i++;
                }
                DoRefresh();
            }
        }

        private void bsInstrmt_CurrentChanged(object sender, EventArgs e)
        {
            GetTable(bFilter);
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //GetTable(bFilter);
        }

    }
}
