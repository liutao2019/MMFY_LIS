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

namespace dcl.client.dicmic
{
    public partial class ConDict_Btype : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员
        List<EntityDicMicBacttype> list = new List<EntityDicMicBacttype>();
        public void Add()
        {
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            EntityDicMicBacttype dr = (bsBtype.AddNew()) as EntityDicMicBacttype;
            dr.BtypeId = string.Empty;
            dr.BtypeDelFlag = "0";

            txtEname.Focus();
            txtPY.Properties.ReadOnly = true;
            txtWB.Properties.ReadOnly = true;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsBtype.EndEdit();
            if (this.bsBtype.Current == null)
            {
                isActionSuccess = false;
                return;
            }

            if (txtEname.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("英文代码不能为空！", "提示");
                txtEname.Focus();
                isActionSuccess = false;
                return;
            }

            if (txtCname.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("菌类名称不能为空！", "提示");
                txtCname.Focus();
                isActionSuccess = false;
                return;
            }

            if (selectAn_SType1.displayMember == "")
            {
                lis.client.control.MessageDialog.Show("药敏卡不能为空！", "提示");
                selectAn_SType1.Focus();
                isActionSuccess = false;
                return;
            }

            EntityDicMicBacttype dr = bsBtype.Current as EntityDicMicBacttype;
            String itm_id = dr.BtypeId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

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
                    dr.BtypeId = result.GetResult<EntityDicMicBacttype>().BtypeId;
                    dr.ATypeName = result.GetResult<EntityDicMicBacttype>().ATypeName;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.bsBtype.EndEdit();
            if (bsBtype.Current == null)
            {
                return;
            }

            EntityDicMicBacttype dr = bsBtype.Current as EntityDicMicBacttype;
            String br_id = dr.BtypeId;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);
            if (br_id == "")
            {
                return;
            }
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
                list = result.GetResult() as List<EntityDicMicBacttype>;
                bsBtype.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);

            return dlist;
        }

        #endregion

        public ConDict_Btype()
        {
            InitializeComponent();
            this.lookUpEdit1.Properties.DataSource = this.m_dtbBindingResultType();
            this.lookUpEdit1.Properties.ValueMember = "Id";
            this.lookUpEdit1.Properties.DisplayMember = "Value";
            this.repositoryItemLookUpEdit1.DataSource = this.m_dtbBindingResultType();
            this.repositoryItemLookUpEdit1.DisplayMember = "Value";
            this.repositoryItemLookUpEdit1.ValueMember = "Id";
        }

        private void on_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();

        }

        private void initData()
        {
            this.DoRefresh();

        }

        #region 鼠标离开事件
        private void txtCname_Leave(object sender, EventArgs e)
        {
            SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();
            if (bsBtype.Current != null)
            {
                EntityDicMicBacttype bacttype = bsBtype.Current as EntityDicMicBacttype;
                bacttype.BtypePyCode = spellcode.GetSpellCode(this.txtCname.Text);
                bacttype.BtypeWbCode = spellcode.GetWBCode(this.txtCname.Text);

                if (txtPY.Text.Length > 12)
                {
                    bacttype.BtypePyCode = bacttype.BtypePyCode.Substring(1, 12);
                }
                if (txtWB.Text.Length > 12)
                {
                    bacttype.BtypeWbCode = bacttype.BtypeWbCode.Substring(1, 12);
                }
            }

        }
        #endregion


        /// <summary>
        /// 绑定项目细菌大类初始化数据
        /// </summary>
        private List<EntityBBtype> m_dtbBindingResultType()
        {
            #region 绑定项目细菌大类初始化数据



            //DataTable result = new DataTable("BBtype");
            //result.Columns.Add("id");
            //result.Columns.Add("value");
            //result.Rows.Add(new Object[] { "0", "细菌" });
            //result.Rows.Add(new Object[] { "1", "真菌" });


            //return result;
            List<EntityBBtype> bBTypes = new List<EntityBBtype>();
            bBTypes.Add(new EntityBBtype() { Id = "0", Value = "细菌" });
            bBTypes.Add(new EntityBBtype() { Id = "1", Value = "真菌" });
            bBTypes.Add(new EntityBBtype() { Id = "3", Value = "结核细菌" });
            return bBTypes;
            #endregion

        }

        #region IBarActionExt 成员

        public void Cancel()
        {

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
    public class EntityBBtype
    {
        public string Id { get; set; }
        public String Value { get; set; }
    }
}
