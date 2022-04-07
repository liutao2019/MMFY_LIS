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
using lis.client.control;
using System.Linq;
using dcl.entity;

namespace dcl.client.dicbasic
{
    public partial class ConDepart : ConDicCommon, IBarActionExt
    {
        ///// <summary>
        ///// 是否为新增事件
        ///// </summary>
        bool blIsNew = false;
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private List<EntityDicPubDept> list = new List<EntityDicPubDept>();

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

            EntityDicPubDept depart = (EntityDicPubDept)bindingSource1.AddNew();
            depart.DeptId = string.Empty;
            depart.DeptDelFlag = LIS_Const.del_flag.OPEN;

            bindingSource1.EndEdit();
            bindingSource1.ResetCurrentItem();

            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("科室名称不能为空", "提示信息");
                return;
            }

            this.bindingSource1.EndEdit();
            if (bindingSource1.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicPubDept depart = (EntityDicPubDept)bindingSource1.Current;

            String itm_id = depart.DeptId;

            request.SetRequestValue(depart);

            EntityResponse result = new EntityResponse();
            if (itm_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (itm_id == "")
                {
                    depart.DeptId = result.GetResult<EntityDicPubDept>().DeptId;
                }
                depart.OriName = selectDict_Origin2.displayMember;
                MessageDialog.ShowAutoCloseDialog("保存成功");
                
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //如果是新增事件则重新赋值过滤信息
            if (blIsNew)
            {
                blIsNew = false;//取消新增标志
            }
            bindingSource1.ResetCurrentItem();
        }

        public void Delete()
        {
            this.bindingSource1.EndEdit();
            if (bindingSource1.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicPubDept depart = (EntityDicPubDept)bindingSource1.Current;

            String br_id = depart.DeptId;

            request.SetRequestValue(depart);
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
            else
            {

            }
        }

        public void DoRefresh()
        {
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = result.GetResult() as List<EntityDicPubDept>;
                bindingSource1.DataSource = list;
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

        public ConDepart()
        {
            InitializeComponent();
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
            if (bindingSource1.Current != null)
            {
                EntityDicPubDept depart = (EntityDicPubDept)bindingSource1.Current;
                depart.DeptPyCode = tookit.GetSpellCode(this.btnName.Text);
                depart.DeptWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }

        private void btnDM_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSRM_TextChanged(object sender, EventArgs e)
        {
            this.txtSelectCode.EditValue = btnSRM.EditValue;
        }

        #region IBarActionExt 成员

        void IBarActionExt.Cancel()
        {
            //如果是新增事件则重新赋值过滤信息
            if (blIsNew)
            {
                //dep_class.FilterInfo = cfiClass;
                //dep_code.FilterInfo = cfiCode;
                //dep_id.FilterInfo = cfiId;
                //dep_incode.FilterInfo = cfiInCode;
                //dep_name.FilterInfo = cfiName;
                //colori_name.FilterInfo = cfiOriType;
                //coldep_select_code.FilterInfo = cfiSelectCode;
                blIsNew = false;//取消新增标志
            }
        }

        void IBarActionExt.Close()
        {

        }

        void IBarActionExt.Edit()
        {
            this.gridControl1.Enabled = false;
        }

        void IBarActionExt.MoveNext()
        {

        }

        void IBarActionExt.MovePrev()
        {

        }

        #endregion
       
        private void btnSynchro_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm frm = new DevExpress.Utils.WaitDialogForm("请稍后......", "正在同步科室数据！");
            frm.Visible = true;
            try
            {
                
                EntityRequest request = new EntityRequest();
                EntityResponse respone = new EntityResponse();
                respone = base.Other(request);
                frm.Visible = false;
                if (respone.Scusess)
                {
                    MessageDialog.ShowAutoCloseDialog("同步成功");
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("同步失败");
                }

            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                if (frm.Visible)
                    frm.Visible = false;
            }
           
        }
    }
}
