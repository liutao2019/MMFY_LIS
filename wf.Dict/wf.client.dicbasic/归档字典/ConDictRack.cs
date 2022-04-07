using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using DevExpress.XtraGrid.Columns;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConDictRack : ConDicCommon, IBarActionExt
    {
        List<EntityDicSampTubeRack> list = new List<EntityDicSampTubeRack>();

        public ConDictRack()
        {
            InitializeComponent();

            cmbColor.Properties.Items.AddRange(RackColorConsts.GetRackColorList());
            Dictionary<string, object> dict = base.View(new EntityRequest()).GetResult() as Dictionary<string, object>;
            bsStatus.DataSource = dict["status"];
            bsSpec.DataSource = dict["tubeRacks"];

            InitData();
        }

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {

                dateFrom.EditValue = DateTime.Now.Date;
                dateEditTo.EditValue = DateTime.Now.Date;
                isFirstLoad = true;
                LoadData();
            }
            catch (Exception ex)
            {


            }

        }

        void LoadData()
        {
            EntityRequest request = new EntityRequest();

            DateTime date1 = Convert.ToDateTime(dateFrom.EditValue);
            DateTime date2 = Convert.ToDateTime(dateEditTo.DateTime.AddDays(1).AddSeconds(-1));
            if (date1 > date2)
            {
                lis.client.control.MessageDialog.Show("结束时间不能小于开始时间！", "提示");
                return;
            }
            else
            {
                DateTime[] dates = new DateTime[] { date1, date2 };
                request.SetRequestValue(dates);
                EntityResponse result = base.Search(request);
                list = result.GetResult() as List<EntityDicSampTubeRack>;
                bindingSource1.DataSource = list;
            }
        }

        #endregion

        #region 编辑修改的状态
        /// <summary>
        /// 编辑修改时右边编辑项的状态
        /// </summary>
        /// <param name="p">true 为只读不可编辑</param>
        private void SetStatus(bool p)
        {
            lueCtype.Readonly = p;
            lueRackSpec.Properties.ReadOnly = p;
            cmbColor.Properties.ReadOnly = p;
        }
        #endregion

        #region IBarActionExt成员
        public void Cancel()
        {
            this.SetStatus(true);
        }

        public void Edit()
        {
            SetReadOnly();
            if (!string.IsNullOrEmpty(this.txtRackID.Text))
            {
                //这里要先判断状态是否为空闲，否则不能删除
                if (txtStatus.Text.Equals("空闲"))
                {
                    SetStatus(false);
                }
                else
                {
                    MessageDialog.Show("该试管架正在使用中，不能被修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }

            }

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
        #endregion

        public void SetReadOnly()
        {
            txtBarcode.ReadOnly = true;
            txtStatus.ReadOnly = true;
            txtRackID.ReadOnly = true;
        }
        #region IBarAction成员
        public void Add()
        {
            SetReadOnly();
            try
            {
                foreach (GridColumn column in gvDictRock.Columns)
                {
                    column.FilterInfo = new ColumnFilterInfo();
                }

                //isActionSuccess = true;
                //新增一行
                EntityDicSampTubeRack tubeRack = bindingSource1.AddNew() as EntityDicSampTubeRack;
                //改变编辑状态
                SetStatus(false);
                lueRackSpec.Focus();
                string maxRackCode = list.OrderByDescending(i => i.RackCode).FirstOrDefault().RackCode;

                if (maxRackCode == null)
                {
                    tubeRack.RackCode = "1";
                    txtNum.Text = "1";
                }
                else
                {
                    tubeRack.RackCode = (int.Parse(maxRackCode) + 1).ToString();
                    txtNum.Text = tubeRack.RackCode;
                    EntityRequest request = new EntityRequest();

                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("rackCode", maxRackCode);

                    request.SetRequestValue(dict);
                    EntityResponse result = new EntityResponse();
                    result = base.Search(request);
                    List<EntityDicSampTubeRack> racks = result.GetResult() as List<EntityDicSampTubeRack>;

                    if (racks.Count > 0)
                    {
                        EntityDicSampTubeRack rack = racks.FirstOrDefault();
                        string color = rack.RackColour;

                        tubeRack.RackType = rack.RackType;
                        tubeRack.RackSpec = rack.RackSpec;
                        lueCtype.valueMember = rack.RackType;
                        lueRackSpec.EditValue = rack.RackSpec;
                        if (rack.RackColour != null)
                        {
                            cmbColor.SelectedIndex = cmbColor.Properties.Items.IndexOf(rack.RackColour);
                        }
                    }
                    else
                    {
                        cmbColor.SelectedIndex = 0;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
        }

        public void Save()
        {

            #region 判断必填项是否为空
            if (string.IsNullOrEmpty(this.txtNum.Text))
            {
                SetStatus(true);
                MessageDialog.Show("架子序号不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(this.lueCtype.valueMember))
            {
                SetStatus(true);
                MessageDialog.Show("架子所属物理组别不能为空！");
                //InitData();
                return;
            }
            if (string.IsNullOrEmpty(this.lueRackSpec.Text))
            {

                SetStatus(true);
                MessageDialog.Show("架子规格不能为空！");
                return;
            }
            #endregion

            EntityRequest request = new EntityRequest();

            EntityDicSampTubeRack tubeRack = bindingSource1.Current as EntityDicSampTubeRack;
            tubeRack.RackColour = cmbColor.EditValue.ToString();
            String rack_id = tubeRack.RackId;

            request.SetRequestValue(tubeRack);

            EntityResponse result = new EntityResponse();

            if (rack_id == null)
            {
                #region 添加保存

                #region 新增班次的ID值是自己增长的
                result = base.New(request);
                if (base.isActionSuccess)
                {
                    EntityDicSampTubeRack rack = result.GetResult<EntityDicSampTubeRack>();
                    txtBarcode.EditValue = rack.RackBarcode;
                    MessageDialog.ShowAutoCloseDialog("保存成功");

                    #region 同时添加一条记录到对应的SamStore_Rack表
                    EntitySampStoreRack storeRack = new EntitySampStoreRack();
                    storeRack.SrRackId = rack.RackId;
                    storeRack.SrStatus = rack.RackStatus;
                    storeRack.SrAmount = 0;
                    storeRack.SrStoreDate = DateTime.Now;

                    request.SetRequestValue(storeRack);

                    result = base.Other(request);

                    if (base.isActionSuccess)
                    {
                        DoRefresh();
                        return;
                    }
                    else
                    {
                        return;
                    }
                    #endregion
                }

                #endregion
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }

            SetStatus(true);

            LoadData();
        }

        public void Delete()
        {
            gvDictRock.CloseEditor();

            List<EntityDicSampTubeRack> selectedRack = list.Where(i => i.IsSelected == true && i.RackPrintFlag != 1).ToList();


            if (selectedRack.Count > 0)
            {
                foreach (EntityDicSampTubeRack tubeRack in selectedRack)
                {
                    if (tubeRack.RackStatus != 0 && tubeRack.RackStatus != 20)
                    {
                        MessageDialog.ShowAutoCloseDialog("只能删除空闲或已销毁的架子");
                        return;
                    }
                }
                DialogResult dresult = MessageDialog.Show("是否要删除选中数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dresult == DialogResult.No) return;

                EntityRequest request = new EntityRequest();
                request.SetRequestValue(selectedRack);

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
                MessageDialog.ShowAutoCloseDialog("没有选中要删除的数据");
            }
        }

        public void DoRefresh()
        {
            LoadData();
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gcDictRock", true);
            dlist.Add("panelControl1", true);
            dlist.Add(this.splitContainerControl1.Panel2.Name, true);

            return dlist;
        }


        #endregion

        private void txtFilter_EditValueChanged(object sender, EventArgs e)
        {
            string filter = txtFilter.Text.Trim();

            if (filter == "")
                bindingSource1.DataSource = list;
            else
            {
                bindingSource1.DataSource = list.Where(w => w.RackId.Contains(filter)
                                                       || w.RackBarcode.Contains(filter)).ToList();
            }
        }

        bool isFirstLoad = false;
        private void dateFrom_EditValueChanged(object sender, EventArgs e)
        {
            if (isFirstLoad)
                LoadData();

        }

        private void gvDictRock_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bindingSource1.Current != null)
            {
                EntityDicSampTubeRack rack = bindingSource1.Current as EntityDicSampTubeRack;
                cmbColor.SelectedIndex = cmbColor.Properties.Items.IndexOf(rack.RackColour);
            }
        }
    }


    public class RackColorConsts
    {
        internal static List<string> GetRackColorList()
        {
            return new List<string> { "红色", "紫色", "蓝色", "IT3000专用", "白色", "黑色", "其它", };
        }
    }

}
#endregion