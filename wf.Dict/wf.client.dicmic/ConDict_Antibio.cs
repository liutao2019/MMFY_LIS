using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Collections;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicmic
{
    public partial class ConDict_Antibio : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员
        List<EntityDicMicAntibio> list = new List<EntityDicMicAntibio>();
        public void Add()
        {
            this.txtEname.Focus();
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            EntityDicMicAntibio dr = dsBasicDictBindingSource.AddNew() as EntityDicMicAntibio;
            this.lisCheckBox2.valueMember = 1;
            dr.AntId = string.Empty;
            dr.AntDelFlag = "0";

            txtPy.Properties.ReadOnly = true;
            txtWb.Properties.ReadOnly = true;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.dsBasicDictBindingSource.EndEdit();
            if (this.dsBasicDictBindingSource.Current == null)
            {
                lis.client.control.MessageDialog.Show("无保存数据！", "提示");
                isActionSuccess = false;
                return;
            }
            if (this.txtEname.Text == "")
            {
                lis.client.control.MessageDialog.Show("英文名称不能为空!", "提示信息");
                txtEname.Focus();
                isActionSuccess = false;
                return;
            }
            if (this.txtCname.Text == "")
            {
                lis.client.control.MessageDialog.Show("中文名称不能为空!", "提示信息");
                txtCname.Focus();
                isActionSuccess = false;
                return;
            }
            EntityDicMicAntibio dr = dsBasicDictBindingSource.Current as EntityDicMicAntibio;
            String itm_id = dr.AntId;

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
                    dr.AntId = result.GetResult<EntityDicMicAntibio>().AntId;
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

            EntityDicMicAntibio dr = dsBasicDictBindingSource.Current as EntityDicMicAntibio;
            String br_id = dr.AntId;

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
                list = result.GetResult() as List<EntityDicMicAntibio>;
                dsBasicDictBindingSource.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("txtFilter", true);
            dlist.Add("gridControl1", true);


            return dlist;
        }

        #endregion



        public ConDict_Antibio()
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


        private void txtCname_Leave_1(object sender, EventArgs e)
        {
            SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();
            if (dsBasicDictBindingSource.Current != null)
            {
                EntityDicMicAntibio antibio = dsBasicDictBindingSource.Current as EntityDicMicAntibio;
                antibio.AntPyCode = spellcode.GetSpellCode(this.txtCname.Text);
                antibio.AntWbCode = spellcode.GetWBCode(txtCname.Text);
            }

        }


        #region IBarActionExt 成员

        public void Cancel()
        {

        }

        //编辑按钮
        public void Edit()
        {
            if (this.dsBasicDictBindingSource.Current == null)
            {
                lis.client.control.MessageDialog.Show("无修改数据！", "提示");
                if (this.ParentForm is FrmCommon)
                {
                    ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
                }

                return;

            }
            this.gridControl1.Enabled = false;
        }

        public void Close()
        {

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
