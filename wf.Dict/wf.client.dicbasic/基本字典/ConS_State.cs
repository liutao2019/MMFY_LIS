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

namespace dcl.client.dicbasic
{
    public partial class ConS_State : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicSState> list = new List<EntityDicSState>();

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;

            EntityDicSState dr = (EntityDicSState)bsItem_Prop.AddNew();
            dr.StauId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                return;
            }

            if (this.btnName.Text.Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("标本状态不能为空!", "提示");
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicSState dr = (EntityDicSState)bsItem_Prop.Current;
            String type_id = dr.StauId;

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
                if (type_id == "")
                {
                    dr.StauId = result.GetResult<EntityDicSState>().StauId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
                //DoRefresh();
                this.bsItem_Prop.ResetCurrentItem();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事件
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicSState dr = (EntityDicSState)bsItem_Prop.Current;
            String br_id = dr.StauId;

            request.SetRequestValue(dr);

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
                list = ds.GetResult() as List<EntityDicSState>;
                bsItem_Prop.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
                blIsNew = false;//取消新增事件
        }

        public void Close() { }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext() { }

        public void MovePrev() { }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConS_State()
        {
            InitializeComponent();
            this.Name = "ConS_State";
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            setGridControl();
        }
        private void setGridControl()
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
        private void initData()
        {
            this.DoRefresh();
        }

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsItem_Prop.Current != null)
            {
                EntityDicSState dr = (EntityDicSState)bsItem_Prop.Current;
                dr.PyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.WbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }
        
    }
}
