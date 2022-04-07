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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using dcl.entity;

namespace dcl.client.result.PatControl
{
    public partial class frmAdviceSelect : FrmCommon
    {
        private List<EntityPidReportDetail> CombineToConfirm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combineToConfirm">待确认的组合列表</param>
        /// <param name="confirmType">确认类型</param>
        public frmAdviceSelect(List<EntityPidReportDetail> combineToConfirm)
        {
            InitializeComponent();
            this.CombineToConfirm = combineToConfirm;
            this.listCombineConfirmed = new List<EntityPidReportDetail>();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.gridViewCombineList.CloseEditor();
            if (this.bindingSource1.DataSource != null)
            {
                listCombineConfirmed.Clear();
                foreach (EntityPidReportDetail item in this.bindingSource1.DataSource as List<EntityPidReportDetail>)
                {
                    if (item.Selected == false)
                    {
                        continue;
                    }

                    if (item.AllowSelect == false)
                    {
                        continue;
                    }

                    listCombineConfirmed.Add(item);
                }
            }
            else
            {
                return;
            }
            //}

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public List<EntityPidReportDetail> listCombineConfirmed { get; private set; }

        private void frmAdviceConfirm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = this.CombineToConfirm;
            emptyEditor = new RepositoryItem();
            gridControlCombineList.RepositoryItems.Add(emptyEditor);
            //this.gridControlCombineList.DataSource = this.CombineToConfirm;
            checkBox1.Checked = true;

            // checkBox1_CheckedChanged(null, null);//默认全选组合
        }

        private void gridViewCombineList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // SelectSingleRow();
                btnOK_Click(null, null);
            }
        }

        private void gridViewCombineList_DoubleClick(object sender, EventArgs e)
        {
            SelectSingleRow();
        }

        private void SelectSingleRow()
        {
            if (this.gridViewCombineList.FocusedRowHandle >= 0)
            {

                foreach (EntityPidReportDetail item in this.bindingSource1.DataSource as List<EntityPidReportDetail>)
                {
                    item.Selected = false;
                }

                this.gridViewCombineList.SetRowCellValue(this.gridViewCombineList.FocusedRowHandle, this.gridViewCombineList.Columns["selected"], true);
                btnOK_Click(null, EventArgs.Empty);
            }
        }
        RepositoryItem emptyEditor;
        private void gridViewCombineList_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            GridView grid = sender as GridView;
            if (e.Column.FieldName == "selected")
            {
                if (this.gridViewCombineList.GetRowCellValue(e.RowHandle, "AllowSelect").ToString().ToLower() == "false")
                {
                    e.RepositoryItem = emptyEditor;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.bindingSource1.DataSource != null)
            {
                foreach (EntityPidReportDetail item in this.bindingSource1.DataSource as List<EntityPidReportDetail>)
                {
                    if (this.checkBox1.Checked)
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
    }
}
