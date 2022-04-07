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
    public partial class ConDictHarvester : ConDicCommon, IBarActionExt
    {
        private bool IsNew = false;

        private List<EntityDictHarvester> list = new List<EntityDictHarvester>();

        #region IBarActionExt接口
        public void Add()
        {
            IsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            this.btnHdCode.Focus();
            btnPyCode.Properties.ReadOnly = true;
            btnWbCode.Properties.ReadOnly = true;

            EntityDictHarvester newcur = bsHarvester.AddNew() as EntityDictHarvester;
            newcur.DhId = string.Empty;
            newcur.DelFlag =Convert.ToInt32( LIS_Const.del_flag.OPEN);
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
            this.bsHarvester.EndEdit();
            if (bsHarvester.Current == null)
                return;

            EntityRequest request = new EntityRequest();
            EntityDictHarvester current = bsHarvester.Current as EntityDictHarvester;

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
            EntityResponse response = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list=response.GetResult() as List<EntityDictHarvester>;
                bsHarvester.DataSource = list;
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
            this.bsHarvester.EndEdit();
            if (bsHarvester.Current == null)
                return;

            EntityRequest request = new EntityRequest();
            EntityDictHarvester current = bsHarvester.Current as EntityDictHarvester;
            string dh_id = current.DhId;

            request.SetRequestValue(current);

            EntityResponse response = new EntityResponse();
            if (dh_id == "")
            {
                response = base.New(request);
            }
            else
            {
                response = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (dh_id == "")
                {
                    current.DhId = response.GetResult<EntityDictHarvester>().DhId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
                DoRefresh();
                this.bsHarvester.ResetCurrentItem();//锁定在该行
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
        public ConDictHarvester()
        {
            InitializeComponent();
            this.Name = "ConDictHarvester";
        }

        private void ConDictHarvester_Load(object sender, EventArgs e)
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
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void btnHdName_Leave(object sender, EventArgs e)
        {
            if (bsHarvester.Current != null)
            {
                EntityDictHarvester current = bsHarvester.Current as EntityDictHarvester;
                current.PyCode = tookit.GetSpellCode(this.btnHdName.Text);
                current.WbCode = tookit.GetWBCode(this.btnHdName.Text);
            }
        }
    }
}
