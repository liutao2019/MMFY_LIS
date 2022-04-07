using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConHospital : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;
        private List<EntityDicPubOrganize> list = new List<EntityDicPubOrganize>();

        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;

            EntityDicPubOrganize dr = (EntityDicPubOrganize)bsHospital.AddNew();
            dr.OrgId = string.Empty;
            dr.OrgDelFlag = LIS_Const.del_flag.OPEN;

            bsHospital.EndEdit();
            bsHospital.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("机构名称不能为空", "提示信息");
                this.ActiveControl = this.btnName;
                this.btnName.Focus();
                return;
            }
            if (this.btnDM.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("机构代码不能为空", "提示信息");
                this.ActiveControl = this.btnDM;
                this.btnDM.Focus();

                return;
            }
            this.bsHospital.EndEdit();
            if (bsHospital.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicPubOrganize dr = (EntityDicPubOrganize)bsHospital.Current;
            String hos_id = dr.OrgId;

            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();

            if (hos_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (hos_id == "")
                {
                    dr.OrgId = result.GetResult<EntityDicPubOrganize>().OrgId;
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
                blIsNew = false;//取消新增事物
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsHospital.EndEdit();
            if (bsHospital.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicPubOrganize dr = (EntityDicPubOrganize)bsHospital.Current;
            String br_id = dr.OrgId;

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
                list = ds.GetResult() as List<EntityDicPubOrganize>;
                bsHospital.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            return dlist;
        }
        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConHospital()
        {
            InitializeComponent();
            this.Name = "ConHospital";
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            setGridControl();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");           
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }

        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
            
            
        }
        

        private void initData()
        {
            this.DoRefresh();
        }

        private void btnName_Leave(object sender, EventArgs e)
        {
            //this.btnPYM.Text = tookit.GetSpellCode(this.btnName.Text);
            //this.btnWBM.Text = tookit.GetWBCode(this.btnName.Text);

            if (bsHospital.Current != null)
            {
                EntityDicPubOrganize dr = (EntityDicPubOrganize)bsHospital.Current;

                dr.OrgPyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.OrgWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }
        

        #region IBarActionExt 成员

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridControl1.Enabled = true;
            }
        }

        public void Close()
        {

        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
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
