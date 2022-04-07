using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Data;
using dcl.entity;
using System.Collections.Generic;
using DevExpress.XtraEditors;

namespace dcl.client.common
{
    /// <summary>
    /// 多全选框生成类 
    /// 
    /// 例子:
    /// <example>
    /// <code>
    /// 
    /// using dcl.client.common;
    ///  
    ///   internal GridCheckMarksSelection Selection { get; private set; }
    ///   
    ///    private void Form1_Load(object sender, System.EventArgs e)
    ///    { 
    ///       MainGridView.ExpandAllGroups();
    ///       Selection = new GridCheckMarksSelection(MainGridView);
    ///       Selection.CheckMarkColumn.VisibleIndex = 0;
    ///    }
    ///   </code>
    ///  </example>
    ///    
    /// 注:dataTable1必须绑定到gridView1
    ///    
    /// 
    /// 获取选择行的值 : (selection.GetSelectedRow(i) as DataRowView)[0]
    ///    
    /// 选择全部: selection.SelectAll();
    /// 
    /// 清空选择: selection.ClearSelection();
    /// 
    /// 点击事件: OnCheckMarksColumnClick
    /// 
    /// </summary>
    public class GridCheckMarksSelection
    {
        protected GridView view;
        protected ArrayList selection;
        private GridColumn column;
        private RepositoryItemCheckEdit edit;


        public GridCheckMarksSelection()
            : base()
        {
            selection = new ArrayList();
        }

        public GridCheckMarksSelection(GridView view)
            : this()
        {
            View = view;
        }

        protected virtual void Attach(GridView view)
        {
            if (view == null) return;
            selection.Clear();
            this.view = view;
            edit = view.GridControl.RepositoryItems.Add("CheckEdit") as RepositoryItemCheckEdit;
            edit.EditValueChanged += new EventHandler(edit_EditValueChanged);

            column = view.Columns.Add();
            column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            column.VisibleIndex = int.MaxValue;
            column.FieldName = "CheckMarkSelection";
            column.Caption = "Mark";
            column.OptionsColumn.ShowCaption = false;
            column.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            column.ColumnEdit = edit;

            view.Click += new EventHandler(View_Click);
            view.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(View_CustomDrawColumnHeader);
            view.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(View_CustomDrawGroupRow);
            //view.CustomUnboundColumnData += new CustomColumnDataEventHandler(view_CustomUnboundColumnData);
            view.RowStyle += new RowStyleEventHandler(view_RowStyle);
            view.MouseDown += new MouseEventHandler(view_MouseDown); // clear selection

        }



        protected virtual void Detach()
        {
            if (view == null) return;
            if (column != null)
                column.Dispose();
            if (edit != null)
            {
                view.GridControl.RepositoryItems.Remove(edit);
                edit.Dispose();
            }

            view.Click -= new EventHandler(View_Click);
            view.CustomDrawColumnHeader -= new ColumnHeaderCustomDrawEventHandler(View_CustomDrawColumnHeader);
            view.CustomDrawGroupRow -= new RowObjectCustomDrawEventHandler(View_CustomDrawGroupRow);
            //view.CustomUnboundColumnData -= new CustomColumnDataEventHandler(view_CustomUnboundColumnData);
            view.RowStyle -= new RowStyleEventHandler(view_RowStyle);
            view.MouseDown -= new MouseEventHandler(view_MouseDown);

            view = null;
        }

        protected void DrawCheckBox(Graphics g, Rectangle r, bool Checked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = edit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = edit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        private bool showGroup = false;
        public bool ShowGroup { get { return showGroup; } set { showGroup = value; } }

        private void View_Click(object sender, EventArgs e)
        {
            GridHitInfo info;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            info = view.CalcHitInfo(pt);
            if (info.InColumn && info.Column == column)
            {
                bool check = false;
                if (SelectedCount == view.DataRowCount)
                    ClearSelection();
                else
                {
                    SelectAll();
                    check = true;
                }

                OnCheckMarksColumnClick(sender, new CheckMarkColumnClickEventArgs(-1, check));
            }



            if (info.InRow && view.IsGroupRow(info.RowHandle) && info.HitTest != GridHitTest.RowGroupButton)
            {
                bool selected = IsGroupRowSelected(info.RowHandle);
                SelectGroup(info.RowHandle, !selected);
            }
        }

        private void view_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                GridHitInfo info;
                Point pt = view.GridControl.PointToClient(Control.MousePosition);
                info = view.CalcHitInfo(pt);

                //if (info.InRow && info.Column == column)// && view.IsDataRow(info.RowHandle))
                //{
                //    if (info.RowHandle >= 0)
                //    {
                //        SelectRow(info.RowHandle, true);
                //        OnCheckMarksColumnClick(sender, new CheckMarkColumnClickEventArgs(info.RowHandle, true));
                //    }
                //}

                //if (info.InRow && info.Column != column && view.IsDataRow(info.RowHandle))
                //{
                //    //ClearSelection();
                //    // SelectRow(info.RowHandle, true);
                //}
            }
        }

        private void View_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == column)
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(e.Graphics, e.Bounds, SelectedCount == view.DataRowCount);
                e.Handled = true;
            }
        }

        private void View_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            //if (!ShowGroup)
            //    return;
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo info;
            info = e.Info as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo;

            info.GroupText = "         " + info.GroupText.TrimStart();
            e.Info.Paint.FillRectangle(e.Graphics, e.Appearance.GetBackBrush(e.Cache), e.Bounds);
            e.Painter.DrawObject(e.Info);

            Rectangle r = info.ButtonBounds;
            r.Offset(r.Width * 2, 0);
            DrawCheckBox(e.Graphics, r, IsGroupRowSelected(e.RowHandle));
            e.Handled = true;
        }

        private void view_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (IsRowSelected(e.RowHandle) && HighLight)
            {
                e.Appearance.BackColor = SystemColors.Highlight;
                e.Appearance.ForeColor = SystemColors.HighlightText;
            }
        }

        private bool highLight = false;
        public bool HighLight { get { return highLight; } set { highLight = value; } }

        public GridView View
        {
            get
            {
                return view;
            }
            set
            {
                if (view != value)
                {
                    Detach();
                    Attach(value);
                }
            }
        }

        public GridColumn CheckMarkColumn
        {
            get
            {
                return column;
            }
        }

        public int SelectedCount
        {
            get
            {
                return selection.Count;
            }
        }


        /// <summary>
        /// 获取所有选择行
        /// </summary>
        /// <returns></returns>
        public DataRowView[] GetAllSelectRow()
        {
            DataRowView[] rows = new DataRowView[this.selection.Count];
            for (int i = 0; i < this.selection.Count; i++)
            {
                rows[i] = selection[i] as DataRowView;
            }

            return rows;
        }


        public List<EntitySampMain> GetAllSelectSamp()
        {
            List<EntitySampMain> listSamp = new List<EntitySampMain>();
            for (int i = 0; i < selection.Count; i++)
            {
                listSamp.Add((EntitySampMain)selection[i]);
            }
            return listSamp;
        }

        public object GetSelectedRow(int index)
        {
            return selection[index];
        }

        public int GetSelectedIndex(object row)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                if (((EntitySampMain)selection[i]).SampBarId == ((EntitySampMain)row).SampBarId)
                {
                    return i;
                }
            }

            return -1;
         //   return  selection.IndexOf(row);
        }

        /// <summary>
        /// 清除已选择的
        /// </summary>
        public void ClearSelection()
        {
            selection.Clear();
            for (int i = 0; i < view.DataRowCount; i++)
            {
                EntitySampMain sm = (EntitySampMain)view.GetRow(i);
                if (sm != null)
                    sm.CheckMarkSelection = false;
            }
            Invalidate();
        }

        private void Invalidate()
        {
            view.BeginUpdate();
            view.EndUpdate();
        }

        /// <summary>
        /// 选择全部
        /// </summary>
        public void SelectAll()
        {
            selection.Clear();
            if (view.DataSource is ICollection)
            {
                selection.AddRange(((ICollection)view.DataSource));  // fast
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    EntitySampMain sm = (EntitySampMain)view.GetRow(i);
                    sm.CheckMarkSelection = true;
                }
            }
            else
                for (int i = 0; i < view.DataRowCount; i++)  // slow
                    selection.Add(view.GetRow(i));
            Invalidate();
        }

        public void SelectGroup(int rowHandle, bool select)
        {
            //if (!ShowGroup)
            //    return;

            if (IsGroupRowSelected(rowHandle) && select) return;
            for (int i = 0; i < view.GetChildRowCount(rowHandle); i++)
            {
                int childRowHandle = view.GetChildRowHandle(rowHandle, i);
                if (view.IsGroupRow(childRowHandle))
                    SelectGroup(childRowHandle, select);
                else
                    SelectRow(childRowHandle, select, false);
            }
            Invalidate();
        }

        public void SelectRow(int rowHandle, bool select)
        {
            SelectRow(rowHandle, select, true);
        }

        private void SelectRow(int rowHandle, bool select, bool invalidate)
        {
            if (IsRowSelected(rowHandle) == select) return;
            object row = view.GetRow(rowHandle);
            if (select)
                selection.Add(row);
            else
                selection.Remove(row);
            if (invalidate)
            {
                Invalidate();
            }
        }

        public bool IsGroupRowSelected(int rowHandle)
        {
            //if (!ShowGroup)
            //    return false;
            for (int i = 0; i < view.GetChildRowCount(rowHandle); i++)
            {
                int row = view.GetChildRowHandle(rowHandle, i);
                if (view.IsGroupRow(row))
                {
                    if (!IsGroupRowSelected(row)) return false;
                }
                else
                    if (!IsRowSelected(row)) return false;
            }
            return true;
        }

        public bool IsRowSelected(int rowHandle)
        {
            if (view.IsGroupRow(rowHandle) /*&& ShowGroup*/)
                return IsGroupRowSelected(rowHandle);

            object row = view.GetRow(rowHandle);
            return GetSelectedIndex(row) != -1;
        }

        private void view_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column == CheckMarkColumn)
            {
                int rowhan = view.GetRowHandle(e.ListSourceRowIndex);
                if (e.IsGetData)
                    e.Value = IsRowSelected(rowhan);
                else
                {
                    bool check = (bool)e.Value;
                    int index1 = view.FocusedRowHandle;
                    //上面两行保存值 ,因下行的操作会使e.RowHandle,e.Value,改变,所以上面两行不行删除
                    SelectRow(rowhan, (bool)e.Value);
                    //OnCheckMarksColumnClick(sender, new CheckMarkColumnClickEventArgs(index, check));
                }
            }
        }

        private void edit_EditValueChanged(object sender, EventArgs e)
        {
            //view.PostEditor();

            if (view.GetFocusedRow() != null)
            {
                CheckEdit ck = (CheckEdit)sender;
                if (ck.Checked)
                {
                    EntitySampMain samp = view.GetFocusedRow() as EntitySampMain;
                    selection.Add(view.GetFocusedRow());
                  
                }
                else
                {
                    selection.Remove(view.GetFocusedRow());
                }

            }
        }

        public event CheckMarksEventHandler CheckMarksColumnClick;
        public void OnCheckMarksColumnClick(object sender, CheckMarkColumnClickEventArgs e)
        {
            if (CheckMarksColumnClick != null)
                CheckMarksColumnClick(sender, e);
        }
    }

    public delegate void CheckMarksEventHandler(object sender, CheckMarkColumnClickEventArgs e);

    public class CheckMarkColumnClickEventArgs : EventArgs
    {
        /// <summary>
        /// 为-1时表示列头,0表示第1行,由自类推
        /// </summary>
        public int RowHandle { get; set; }
        /// <summary>
        /// 当前行是否被打勾
        /// </summary>
        public bool IsChecked { get; set; }

        public CheckMarkColumnClickEventArgs(int rowHandle, bool isChecked)
        {
            this.RowHandle = rowHandle;
            this.IsChecked = isChecked;
        }
    }
}

