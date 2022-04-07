using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.result.PatControl;
using dcl.client.frame;
using dcl.common;
using dcl.entity;

namespace dcl.client.result
{
    public partial class FrmItmPropLst : FrmCommon
    {

        public List<EntityDefItmProperty> listSource
        {
            set
            {
                this.gridControl1.DataSource = value;
                this.gridControl1.RefreshDataSource();
            }
        }
        public DataTable dtSource
        {
            set
            {
                this.gridControl1.DataSource = value;
            }
        }
        public DataRow selectResultoRow;
        public int row = 0;
        public int col = 0;
        public DevExpress.XtraGrid.Views.Grid.GridView parent_grid;
        public PatResult parentControl;

        public string strResColumnCount = string.Empty;

        public FrmItmPropLst()
        {
            InitializeComponent();
        }

        private void FrmItmPropLst_Load(object sender, EventArgs e)
        {

        }



        private void FrmItmPropLst_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

     
        private void FrmItmPropLst_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {


            EntityDefItmProperty prop = this.gridView1.GetFocusedRow() as EntityDefItmProperty;
            int rowIndex = parent_grid.FocusedRowHandle;

            EntityObrResult Result = parent_grid.GetFocusedRow() as EntityObrResult;
            DataRow Result2 = parent_grid.GetFocusedDataRow() as DataRow;//新添加，双列点击时

            if (prop == null)
            {
                return;
            }

            if (!Compare.IsEmpty(prop.PtyItmProperty))
            {
                string itemprops = prop.PtyItmProperty;
                string[] props = itemprops.Split(';');
                //if (parentControl!=null)
                // parentControl.isPo = props.Length>1;

                for (int i = 0; i < props.Length; i++)
                {
                    if (parentControl != null && i > 0)
                    {
                        parentControl.isPo = true;
                        parentControl.isEnt = false;
                    }
                    if (parent_grid.RowCount >= (rowIndex + i) + 1)
                    {
                        if ((Result != null && Result.ObrType.ToString() == LIS_Const.PatResultType.Cal) ||
                            (Result2 != null && Result2["res_type"].ToString() == LIS_Const.PatResultType.Cal))
                        {
                            this.toolTipController1.ShowHint("当前项目为关联计算项目，不能修改");
                        }
                        else
                        {
                            if (Result != null)
                                parent_grid.SetRowCellValue(rowIndex + i, "ObrValue", props[i]);
                            if (Result2 != null)
                                parent_grid.SetRowCellValue(rowIndex + i, "res_chr" + strResColumnCount, props[i]);
                        }
                    }
                }
                if (parentControl != null)
                    parentControl.isPo = false;
            }

            if (UserInfo.GetSysConfigValue("ItemPropFormSelectMode") == "多选择")
            {
                if (parent_grid.FocusedRowHandle == (parent_grid.RowCount - 1))
                    this.Close();
                else
                    parent_grid.MoveNext();
            }
            else
            {
                this.Close();
            }
        }
    }
}
