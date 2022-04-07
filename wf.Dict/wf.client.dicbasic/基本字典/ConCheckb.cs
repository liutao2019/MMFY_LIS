using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using DevExpress.XtraGrid.Columns;
using dcl.entity;

using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConCheckb : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicCheckPurpose> list = new List<EntityDicCheckPurpose>();

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.memoEdit1.Focus();
            buttonEdit2.Properties.ReadOnly = true;
            buttonEdit5.Properties.ReadOnly = true;

            EntityDicCheckPurpose dr = (EntityDicCheckPurpose)bsDiagnos.AddNew();
            dr.PurpId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsDiagnos.EndEdit();
            if (bsDiagnos.Current == null)
            {
                return;
            }

            if(this.memoEdit1.Text.Trim(null)==string.Empty)
            {
                lis.client.control.MessageDialog.Show("检查目的不能为空！","提示");
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicCheckPurpose dr = (EntityDicCheckPurpose)bsDiagnos.Current;
            String type_id = dr.PurpId;

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
                    dr.PurpId = result.GetResult<EntityDicCheckPurpose>().PurpId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
                DoRefresh();
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
            this.bsDiagnos.EndEdit();
            if (bsDiagnos.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicCheckPurpose dr = (EntityDicCheckPurpose)bsDiagnos.Current;
            String br_id = dr.PurpId;

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
                list = ds.GetResult() as List<EntityDicCheckPurpose>;
                bsDiagnos.DataSource = list;
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

        public ConCheckb()
        {
            InitializeComponent();
            this.Name = "ConCheckb";
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


        private void memoEdit1_Leave(object sender, EventArgs e)
        {
            if (bsDiagnos.Current != null)
            {
                EntityDicCheckPurpose dr = (EntityDicCheckPurpose)bsDiagnos.Current;
                dr.PyCode = tookit.GetSpellCode(this.memoEdit1.Text);
                dr.WbCode = tookit.GetWBCode(this.memoEdit1.Text);
            }

            buttonEdit2.Text = tookit.GetSpellCode(this.memoEdit1.Text);
            buttonEdit5.Text = tookit.GetWBCode(this.memoEdit1.Text);
            if (buttonEdit2.Text.Length > 12)
            {
                buttonEdit2.Text = buttonEdit2.Text.Substring(0, 12);
            }
            if (buttonEdit5.Text.Length > 12)
            {
                buttonEdit5.Text = buttonEdit5.Text.Substring(0, 12);
            }

        }
    }
}
