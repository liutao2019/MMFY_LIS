using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;

namespace dcl.client.dicmic
{
    public partial class ConDict_An_Stype : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员
        List<EntityDicMicAntitype> list = new List<EntityDicMicAntitype>();
        public void Add()
        {
            this.txtCname.Focus();
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            EntityDicMicAntitype dr = dsBasicDictBindingSource.AddNew() as EntityDicMicAntitype;
            dr.AtypeId = string.Empty;
            dr.ADelFlag = 0;

            txtPy.Properties.ReadOnly = true;
            txtWb.Properties.ReadOnly = true;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.dsBasicDictBindingSource.EndEdit();
            if (this.dsBasicDictBindingSource.Current == null)
            {
                return;
            }
            if (this.txtCname.Text == "")
            {
                lis.client.control.MessageDialog.Show("药敏卡名称不能为空", "提示");
                txtCname.Focus();
                isActionSuccess = false;
                return;

            }

            EntityDicMicAntitype dr = (dsBasicDictBindingSource.Current) as EntityDicMicAntitype;
            String itm_id = dr.AtypeId.ToString();
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();
            if (itm_id == "")
            {
                result = base.New(request);
                gridView1.FocusedRowHandle = gridView1.DataRowCount - 1;

            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (itm_id == "")
                {
                    dr.AtypeId = result.GetResult<EntityDicMicAntitype>().AtypeId;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败"+ result.ErroMsg);
                //Lib.LogManager.Logger.LogInfo(result.ErroMsg);
            }
            dsBasicDictBindingSource.ResetCurrentItem();
        }

        public void Delete()
        {
            this.dsBasicDictBindingSource.EndEdit();
            if (dsBasicDictBindingSource.Current == null)
            {
                return;
            }

            EntityDicMicAntitype dr = (dsBasicDictBindingSource.Current) as EntityDicMicAntitype;
            String br_id = dr.AtypeId;

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
                list = result.GetResult() as List<EntityDicMicAntitype>;
                dsBasicDictBindingSource.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);

            dlist.Add("gridControl1", true);

            return dlist;
        }

        #endregion

        public ConDict_An_Stype()
        {
            InitializeComponent();
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

        private void txtEname_Leave(object sender, EventArgs e)
        {
            SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();
            if (dsBasicDictBindingSource.Current != null)
            {
                EntityDicMicAntitype antiType = dsBasicDictBindingSource.Current as EntityDicMicAntitype;
                antiType.APyCode = spellcode.GetSpellCode(this.txtCname.Text);
                antiType.AWbCode = spellcode.GetWBCode(txtCname.Text);
            }

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
}
