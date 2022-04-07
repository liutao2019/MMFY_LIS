using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using lis.client.control;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.client.result.CommonPatientInput;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class frmBarcodeCombineSelect : FrmCommon
    {
        /// <summary>
        /// 可以选择的组合
        /// </summary>
        private List<EntityPidReportDetail> CombineToSelect;

        string itr_id;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="combineToConfirm">待确认的组合列表</param>
        /// <param name="confirmType">确认类型</param>
        public frmBarcodeCombineSelect(string itr_id,
            List<EntityPidReportDetail> combineToSelect
            )
        {
            InitializeComponent();
            this.itr_id = itr_id;
            this.CombineToSelect = combineToSelect;
            this.listCombineSelected = new List<EntityPidReportDetail>();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.gridViewCombineList.CloseEditor();
            if (this.bindingSource1.DataSource != null)
            {
                listCombineSelected.Clear();

                foreach (EntityPidReportDetail item in this.bindingSource1.DataSource as List<EntityPidReportDetail>)
                {
                    if (item.Selected == false)
                    {
                        continue;
                    }

                    listCombineSelected.Add(item);
                }
            }
            else
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public List<EntityPidReportDetail> listCombineSelected { get; private set; }
        RepositoryItem emptyEditor;
        private void frmAdviceConfirm_Load(object sender, EventArgs e)
        {
            emptyEditor = new RepositoryItem();
            gridControlCombineList.RepositoryItems.Add(emptyEditor);

            gridViewCombineList.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewCombineList.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewCombineList.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            //this.bindingSource1.DataSource = this.CombineToSelect;

            //查找当前仪器的组合
            List<string> currentItrComIDs = null;
            if (!string.IsNullOrEmpty(this.itr_id))//没有选择仪器
            {
                currentItrComIDs = DictInstrmt.Instance.GetItrCombineID(this.itr_id, true);
            }

            //遍历每一个组合
            foreach (EntityPidReportDetail comId in this.CombineToSelect)
            {
                string com_id = comId.ComId;

                if (!string.IsNullOrEmpty(itr_id))
                {
                    if (comId.SampFlag == 0)//未登记的组合
                    {
                        if (currentItrComIDs.Contains(com_id))//当前仪器包含此组合
                        {
                            comId.CanSelect = true;
                        }
                        else
                        {
                            comId.CanSelect = false;
                            comId.Description = "此组合不能在此仪器登记";
                        }
                    }
                    else//已登记的组合
                    {
                        //comId.CanSelect = false;
                        comId.Description = "此组合已登记";
                    }
                }
            }
            this.bindingSource1.DataSource = this.CombineToSelect;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.bindingSource1.DataSource != null)
            {
                foreach (EntityPidReportDetail item in this.bindingSource1.DataSource as List<EntityPidReportDetail>)
                {
                    if (this.chkSelectAll.Checked && item.CanSelect)
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                this.bindingSource1.ResetBindings(true);
            }
        }

        private void gridViewCombineList_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            GridView grid = sender as GridView;
            if (e.Column.FieldName == "selected")
            {
                if (!Convert.ToBoolean(grid.GetRowCellValue(e.RowHandle, "CanSelect")))
                {
                    e.RepositoryItem = emptyEditor;
                }
            }
        }
    }
}
