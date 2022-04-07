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
using dcl.entity;
using lis.client.control;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.dicbasic
{
    public partial class ConCombineTimerule : ConDicCommon, IBarActionExt
    {

        #region 保存数据表临时过滤信息

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion

        private DataTable dtType = new DataTable();

        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;
            gcComTime.Enabled = false;
            EntityDicCombineTimeRule dr = (EntityDicCombineTimeRule)bsCombineTimerule.AddNew();
            this.gcComTime.Enabled = false;
        }

        public void Save()
        {
            bsCombineTimerule.EndEdit();
            if (bsCombineTimerule.Current == null)
                return;

            if (ConfigHelper.IsNotOutlink())
            {
                if (lueTimeOri.valueMember == null || lueTimeOri.valueMember.Trim() == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("请输入来源！");
                    lueTimeOri.Focus();
                    return;
                }
            }
            if (lueTimeStartType.EditValue == null || lueTimeStartType.EditValue.ToString().Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入开始类型！");
                lueTimeStartType.Focus();
                return;
            }
            if (lueTimeEndType.EditValue == null || lueTimeEndType.EditValue.ToString().Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入结束类型！");
                lueTimeEndType.Focus();
                return;
            }
            if (txtTime.EditValue == null || txtTime.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入间隔时间！");
                txtTime.Focus();
                return;
            }
            if (Convert.ToInt32(lueTimeStartType.EditValue) >= Convert.ToInt32(lueTimeEndType.EditValue))
            {
                lis.client.control.MessageDialog.Show("结束步骤应该在开始步骤之前！");
                lueTimeEndType.Focus();
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicCombineTimeRule dr = (EntityDicCombineTimeRule)bsCombineTimerule.Current;
            String type_id = dr.ComTimeId;
            dr.DelFlag = "0";
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            if (type_id == "" || type_id == null)
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                DoRefresh();
            }
            else
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }
        }

        public void Delete()
        {
            this.bsCombineTimerule.EndEdit();
            if (bsCombineTimerule.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicCombineTimeRule dr = (EntityDicCombineTimeRule)bsCombineTimerule.Current;

            request.SetRequestValue(dr);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
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
        private List<EntityDicCombineTimeRule> list = new List<EntityDicCombineTimeRule>();
        public void DoRefresh()
        {
            EntityRequest request = new EntityRequest();
            EntityDicCombineTimeRule dr = new EntityDicCombineTimeRule();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicCombineTimeRule>;
                this.bsCombineTimerule.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gcComTime", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }
        #endregion
        public ConCombineTimerule()
        {
            InitializeComponent();
            //系统配置：[组合时间限定]字典使用预报时间
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CombineTimerule_UseReaTime") != "是")
            {
                txtReaTime.Visible = false;
                labelControl8.Visible = false;
            }
        }

        private void on_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gvComTime, _gridLocalizer);

            this.initData();
        }

        private void initData()
        {
            this.DoRefresh();

            List<EntityDicPubStatus> dtBcStatus = CacheClient.GetCache<EntityDicPubStatus>();
            lueTimeStartType.Properties.DataSource = dtBcStatus.FindAll(i => i.StatusId == "1" || i.StatusId == "2" || i.StatusId == "3" || i.StatusId == "4" || i.StatusId == "5" || i.StatusId=="8" ||
                    i.StatusId == "20" || i.StatusId == "40" || i.StatusId=="60").OrderBy(i => i.StatusId.Length).ThenBy(w => w.StatusId);
            lueTimeEndType.Properties.DataSource = dtBcStatus.FindAll(i => i.StatusId == "1" || i.StatusId == "2" || i.StatusId == "3" || i.StatusId == "4" || i.StatusId == "5" || i.StatusId == "8" ||
                   i.StatusId == "20" || i.StatusId == "40" || i.StatusId == "60").OrderBy(i => i.StatusId.Length).ThenBy(w => w.StatusId);


        }

        //private void doRefresh()
        //{
        //    bsCombineTimerule.EndEdit();
        //    DataSet ds = base.doSearch();

        //    if (isActionSuccess)
        //    {
        //        dtType.AcceptChanges();
        //        DataTable dtComTime = ds.Tables["dict_combine_timerule"];
        //        dtType = dtComTime;
        //        this.bsCombineTimerule.DataSource = dtType;
        //        searchControl1.Initialize(gvComTime, bsCombineTimerule, dtType, dtComTime);
        //    }
        //}

        #region 响应按钮菜单点击事件

        //private void toNew()
        //{
        //}
        //private void toSave()
        //{
        //    bsCombineTimerule.EndEdit();
        //    if (bsCombineTimerule.Current == null)
        //        return;

        //    this.isActionSuccess = false;
        //    if (ConfigHelper.IsNotOutlink())
        //    {
        //        if (lueTimeOri.valueMember == null || lueTimeOri.valueMember.Trim() == string.Empty)
        //        {
        //            lis.client.control.MessageDialog.Show("请输入来源！");
        //            lueTimeOri.Focus();
        //            return;
        //        }
        //    }
        //    if (lueTimeStartType.EditValue == null || lueTimeStartType.EditValue.ToString().Trim() == string.Empty)
        //    {
        //        lis.client.control.MessageDialog.Show("请输入开始类型！");
        //        lueTimeStartType.Focus();
        //        return;
        //    }
        //    if (lueTimeEndType.EditValue == null || lueTimeEndType.EditValue.ToString().Trim() == string.Empty)
        //    {
        //        lis.client.control.MessageDialog.Show("请输入结束类型！");
        //        lueTimeEndType.Focus();
        //        return;
        //    }
        //    if (txtTime.EditValue == null || txtTime.Text.Trim() == string.Empty)
        //    {
        //        lis.client.control.MessageDialog.Show("请输入间隔时间！");
        //        txtTime.Focus();
        //        return;
        //    }
        //    if (Convert.ToInt32(lueTimeStartType.EditValue) >= Convert.ToInt32(lueTimeEndType.EditValue))
        //    {
        //        lis.client.control.MessageDialog.Show("结束步骤应该在开始步骤之前！");
        //        lueTimeEndType.Focus();
        //        return;
        //    }
        //    this.isActionSuccess = true;

        //    DataRow drComTime = ((DataRowView)bsCombineTimerule.Current).Row;

        //    DataTable dtComTime = drComTime.Table.Clone();
        //    dtComTime.Rows.Add(drComTime.ItemArray);


        //    if (blIsNew)
        //        base.doNew(dtComTime);
        //    else
        //        base.doUpdate(dtComTime);

        //    if (base.isActionSuccess)
        //    {
        //        gcComTime.Enabled = true;
        //        doRefresh();
        //    }
        //}
        //private void toDel()
        //{

        //    if (bsCombineTimerule.Current == null)
        //        return;

        //    if (lis.client.control.MessageDialog.Show("是否要删除选中数据？", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        DataRow drComTime = ((DataRowView)bsCombineTimerule.Current).Row;

        //        DataTable dtComTime = drComTime.Table.Clone();
        //        dtComTime.Rows.Add(drComTime.ItemArray);
        //        base.doDel(dtComTime);

        //        if (base.isActionSuccess)
        //        {
        //            gcComTime.Enabled = true;
        //            doRefresh();
        //        }
        //    }
        //}
        //private void toRefresh()
        //{
        //    this.doRefresh();
        //}
        #endregion




        #region IBarActionExt 成员

        public void Cancel()
        {
            this.DoRefresh();
        }

        public void Close()
        {

        }

        public void Edit()
        {
            if (bsCombineTimerule.Current == null)
            {
                lis.client.control.MessageDialog.Show("无修改数据！");
                this.isActionSuccess = false;
                return;
            }
            gcComTime.Enabled = false;
            blIsNew = false;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion

        
    }
}
