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
using DevExpress.XtraGrid.Columns;

using dcl.entity;
using lis.client.control;

namespace dcl.client.dicbasic
{
    public partial class ConTemperature : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增
        /// </summary>
        private bool IsNew = false;

        private List<EntityDicTemperature> tempList = new List<EntityDicTemperature>();

        public ConTemperature()
        {
            InitializeComponent();
            this.Name = "ConTemperature";
        }

        #region IBarActionExt接口

        public void Add()
        {
            IsNew = true;
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnDtCode.Focus();
            this.btnPyCode.ReadOnly = true;
            this.btnWbCode.ReadOnly = true;

            EntityDicTemperature temp = (EntityDicTemperature)bsTemperature.AddNew();
            temp.DtId = string.Empty;
            temp.DelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Cancel()
        {
            if (IsNew)
                IsNew = false;
        }

        public void Close()
        {

        }

        public void Delete()
        {
            this.bsTemperature.EndEdit();
            if (bsTemperature.Current == null)
                return;

            EntityRequest request = new EntityRequest();
            EntityDicTemperature current = bsTemperature.Current as EntityDicTemperature;

            request.SetRequestValue(current);

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
                tempList = ds.GetResult() as List<EntityDicTemperature>;
                bsTemperature.DataSource = tempList;
            }
        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
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

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        public void Save()
        {
            this.bsTemperature.EndEdit();
            if (bsTemperature.Current == null)
                return;

            string dtcode = btnDtCode.Text.Trim();
            string dtname = btnDtName.Text.Trim();
            string dtllimit = btnDtLLimit.Text.Trim();
            string dthlimit = btnDtHLimit.Text.Trim();
            if (!Check(dtcode, dtname, dtllimit, dthlimit))
            {
                DoRefresh();
                return;
            }


            EntityRequest request = new EntityRequest();
            EntityDicTemperature current = this.bsTemperature.Current as EntityDicTemperature;
            string dtId = current.DtId;

            request.SetRequestValue(current);

            EntityResponse response = new EntityResponse();
            if (dtId == "")
            {
                response = base.New(request);
            }
            else
            {
                response = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (dtId == "")
                {
                    current.DtId = response.GetResult<EntityDicTemperature>().DtId;
                }
                //DoRefresh();

                MessageDialog.ShowAutoCloseDialog("保存成功");
                this.bsTemperature.ResetCurrentItem();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                DoRefresh();
            }
            if (IsNew)
            {
                IsNew = false;
                this.gridControl1.Enabled = true;
            }
        }


        #endregion

        private void ConTemperature_Load(object sender, EventArgs e)
        {
            this.initData();
            SetGridControl();
        }

        private void initData()
        {
            this.DoRefresh();
        }
        private void SetGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void btnDtName_Leave(object sender, EventArgs e)
        {
            if (bsTemperature.Current != null)
            {
                EntityDicTemperature current = bsTemperature.Current as EntityDicTemperature;
                current.PyCode = tookit.GetSpellCode(btnDtName.Text);
                current.WbCode = tookit.GetWBCode(btnDtName.Text);
            }
        }

        bool Check(string dtcode, string dtname, string dtllimit, string dthlimit)
        {
            bool check = true;
            int msg;
            bool b = int.TryParse(dthlimit, out msg);
            bool b2 = int.TryParse(dtllimit, out msg);
            if (string.IsNullOrEmpty(dtcode))
            {
                MessageBox.Show("范围编码不能为空");
                this.btnDtCode.Focus();
                check = false;
            }
            else if (string.IsNullOrEmpty(dtname))
            {
                MessageBox.Show("范围名称不能为空");
                this.btnDtName.Focus();
                check = false;
            }
            else if (b == false)
            {
                MessageBox.Show("请输入有效的最高温");
                this.btnDtHLimit.Focus();
                check = false;
            }
            else if (b2 == false)
            {
                MessageBox.Show("请输入有效的最低温");
                this.btnDtLLimit.Focus();
                check = false;
            }
            else if (Convert.ToInt32(dthlimit) <= Convert.ToInt32(dtllimit))
            {
                MessageBox.Show("最高温应高于最低温");
                this.btnDtLLimit.Focus();
                check = false;
            }
            return check;
        }

        private void btnDtLLimit_Leave(object sender, EventArgs e)
        {
            //当按保存按钮时会一直触发这个事件
            //string high = btnDtHLimit.Text.Trim();
            //string lower = btnDtLLimit.Text.Trim();
            //if (Convert.ToInt32(high) <= Convert.ToInt32(lower))
            //{
            //    MessageBox.Show("最低温应小于最高温");
            //    this.btnDtLLimit.Focus();
            //}
        }
    }
}
