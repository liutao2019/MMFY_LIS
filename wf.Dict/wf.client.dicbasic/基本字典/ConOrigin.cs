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
    public partial class ConOrigin : ConDicCommon, IBarActionExt 
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicOrigin> list = new List<EntityDicOrigin>();

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

            EntityDicOrigin dr = (EntityDicOrigin)bsType.AddNew();
            dr.SrcId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }

            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("来源名称不能为空", "提示信息");
                return;
            }


            EntityRequest request = new EntityRequest();

            EntityDicOrigin dr = (EntityDicOrigin)bsType.Current;
            String type_id = dr.SrcId;

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
                    dr.SrcId = result.GetResult<EntityDicOrigin>().SrcId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");

                DoRefresh(); //刷新数据
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
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("来源名称不能为空", "提示信息");
                return;
            }

            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicOrigin dr = (EntityDicOrigin)bsType.Current;
            String br_id = dr.SrcId;

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
                list = ds.GetResult() as List<EntityDicOrigin>;
                bsType.DataSource = list;
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

        public ConOrigin()
        {
            InitializeComponent();
            this.Name = "ConOrigin";
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
            if (bsType.Current != null)
            {
                EntityDicOrigin dr = (EntityDicOrigin)bsType.Current;
                dr.PyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.WbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }

        
    }
}
