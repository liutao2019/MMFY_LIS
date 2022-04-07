using System;
using System.Collections.Generic;

using System.Text;
using DevExpress.XtraGrid.Views.Grid;

namespace dcl.client.common
{
    public class GridViewHelper
    {
        /// <summary>
        /// 设置焦点,并使其可编辑
        /// </summary>
        /// <param name="gridView">操作的GridView</param>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnName">列名</param>
        public static void FocusCell(GridView gridView, int rowIndex, string columnName)
        {
            if (gridView.RowCount > 0)
            {
                gridView.Focus();
                gridView.FocusedRowHandle = rowIndex;
                gridView.FocusedColumn = gridView.Columns[columnName];
            }
        }
    }
}
