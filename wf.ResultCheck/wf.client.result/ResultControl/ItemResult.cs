using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraEditors;
using System.Collections;
using lis.client.control;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.result.CommonPatientInput;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class ItemResult : UserControl
    {
        //显示列表的数据源
        private DataTable dataSource = new DataTable();

        //用于在选择项目往dtItemResult表填充项目代码和打印代码
        List<EntityDicItmItem> listItem = null;
        
        ProxyResultTemp proxy = null;
        //项目特征值
        List<EntityDefItmProperty> listItmProp = new List<EntityDefItmProperty>();

        /// <summary>
        /// 记录双击的行索引
        /// </summary>
        private int noteRowIndex = -1;

        /// <summary>
        /// 记录双击的列名
        /// </summary>
        private string noteColName = null;

        /// <summary>
        /// 项目特征选择窗体
        /// </summary>
        FrmItmPropLstV3 prop = null;// 

        public ItemResult()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.ItemResult_Load);
            if (DesignMode)
                return;
            proxy = new ProxyResultTemp();
            listItem = CacheClient.GetCache<EntityDicItmItem>();
        }

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        /// <summary>
        /// 生成列表
        /// </summary>
        /// <param name="rows">项目行数</param>
        /// <param name="cols">项目列数_实际每1列项目对应项目和结果2列</param>
        public void LoadGrid(int rows, int cols)
        {
            dataSource = new DataTable();

            //生成列
            for (int i = 0; i < cols; i++)
            {
                DataColumn colSample = new DataColumn();
                colSample.ColumnName = "itm" + i.ToString();
                colSample.Caption = "项目代码";
                dataSource.Columns.Add(colSample);

                DataColumn colResult = new DataColumn();
                colResult.ColumnName = "chr" + i.ToString();
                colResult.Caption = "测定结果";
                dataSource.Columns.Add(colResult);
            }

            //填充数据
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dataSource.NewRow();

                dataSource.Rows.Add(dr);
            }

            QuickResult("");

            gdItem.DataSource = dataSource;

            //控制列表样式
            for (int i = 0; i < gvItem.Columns.Count; i++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = gvItem.Columns[i];
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                col.OptionsColumn.AllowEdit = true;
                col.Visible = true;
                col.VisibleIndex = i;

                if (i % 2 == 0)
                {
                    //项目代码列
                    col.AppearanceCell.Options.UseBackColor = true;
                    col.AppearanceCell.BackColor = Color.LightYellow;
                    col.ColumnEdit = repItem;
                }
                else
                {
                    //测定结果列
                    //col.ColumnEdit = repProp1;
                    col.ColumnEdit = repResulto;//repComProp;
                    col.ColumnEdit.DoubleClick += new EventHandler(ColumnEdit_DoubleClick);
                }
            }
        }

        public void FocusCellAfterSave()
        {
            gvItem.Focus();
            if (gvItem.RowCount > 0)
            {
                gvItem.FocusedRowHandle = 0;
                gvItem.FocusedColumn = gvItem.Columns["chr0"];
                gvItem.SelectCell(0, gvItem.Columns["chr0"]);
                gvItem.ShowEditor();
            }
        }

        /// <summary>
        /// 快速设置所有数据为同一值
        /// </summary>
        /// <param name="result"></param>
        public void QuickResult(string result)
        {
            for (int i = 0; i < dataSource.Rows.Count; i++)
            {
                for (int j = 0; j < dataSource.Columns.Count; j++)
                {
                    dataSource.Rows[i][j] = result;
                }
            }
        }

        /// <summary>
        /// 取得当前操作结果
        /// </summary>
        /// <returns></returns>
        public DataTable DataSource
        {
            get
            {
                dataSource.AcceptChanges();

                return dataSource;
            }
        }

        /// <summary>
        /// 选择组合后列出该组合下所有项目
        /// </summary>
        /// <param name="dtItemResult"></param>
        public void SetDataSource(List<EntityDicCombineDetail> dtItemResult)
        {
            QuickResult(string.Empty);

            //显示项目
            int index = 0;
            for (int j = 0; j < dataSource.Columns.Count; j++)
            {
                for (int i = 0; i < dataSource.Rows.Count; i++)
                {
                    if (index >= dtItemResult.Count)
                        return;
                    if (j % 2 == 0)
                    {
                        dataSource.Rows[i][j] = dtItemResult[index].ComItmId.ToString();
                        dataSource.Rows[i][j + 1] = dtItemResult[index].ItmDefault.ToString();
                        index++;
                    }
                }
            }
        }

        
        /// <summary>
        /// 控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemResult_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            repItem.DataSource = listItem;
            string where = string.Empty;
            List<string> listStr = new List<string>();
            listStr.Add(where);

            listItmProp = proxy.Service.GetItemProp(listStr);


            EntityDefItmProperty drPropNull = new EntityDefItmProperty();
            drPropNull.PtyId = "";
            drPropNull.PtyItmId = "";
            drPropNull.PtyItmProperty = "";
            listItmProp.Add(drPropNull);

            bsItemProp.DataSource = listItmProp;

            repComProp.Items.Clear();
            foreach (EntityDefItmProperty rowProp in listItmProp)
            {
                if (rowProp.PtyItmProperty.ToString() != "")
                {
                    repComProp.Items.Add(rowProp.PtyItmProperty.ToString());
                }
            }

            //初始化列表
            LoadGrid(25, 3);
        }

        /// <summary>
        /// 设置可用项目
        /// </summary>
        /// <param name="dt"></param>
        public void SetItem(string filter)
        {
            //dvItem.RowFilter = filter;
            if (!string.IsNullOrEmpty(filter))
                repItem.DataSource = listItem.FindAll(i => i.ItmPriId == filter);
        }

        /// <summary>
        /// 设置项目特征值数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repProp_Enter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 选择项目后更新数据,以保证选择特征值时itm_id不为空_放到EditValueChanged中会导致值丢失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repItem_Leave(object sender, EventArgs e)
        {
            dataSource.AcceptChanges();
        }

        /// <summary>
        /// 选择项目后更新数据,以保证选择特征值时itm_id不为空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repItem_EditValueChanged(object sender, EventArgs e)
        {
            gvItem.SetFocusedRowCellValue(gvItem.VisibleColumns[gvItem.FocusedColumn.VisibleIndex + 1], "");
        }

        /// <summary>
        /// 选择特征值,如有不存在数据字典中的值临时添加,以便用户可手工录入结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repProp_Leave(object sender, EventArgs e)
        {
            ComboBoxEdit thisEdit = (ComboBoxEdit)sender;
            string inputStr = thisEdit.EditValue.ToString().Trim();
            if (inputStr == "-")
            {
                gvItem.SetFocusedValue("阴性(-)");
            }
            else if (inputStr == "+")
            {
                gvItem.SetFocusedValue("阳性(+)");
            }
            else if (inputStr == "*")
            {
                gvItem.SetFocusedValue("弱阳性");
            }

        }

        List<string> arrayStr = new List<string> { "阴   性", "阳   性", "弱阳性" };
        List<string> quickInputs = new List<string> { "+", "＋", "-", "－", "*" };
     
        /// <summary>
        /// 临时添加可用特征值
        /// </summary>
        /// <param name="filter"></param>
        private void AddTmpProp(DataTable thisTable, string filter, string itmId)
        {
            DataRow[] drs = thisTable.Select("itm_prop='" + filter.Replace("'", "''") + "'");
            if (drs.Length < 1)
            {
                AddIntoDataSource(thisTable, filter, itmId);
            }
        }

        private void AddIntoDataSource(DataTable thisTable, string filter, string itmId)
        {
            EntityDefItmProperty item = new EntityDefItmProperty();
            item.PtyId = "录入";
            item.PtyItmId = itmId;
            item.PtyItmProperty = filter;
            listItmProp.Add(item);


            dataSource.AcceptChanges();
            gvItem.SetFocusedValue(filter);
        }

        /// <summary>
        /// 快速录入，把“－”“+”号换成“阴性”“阳性”
        /// </summary>
        /// <param name="input"></param>
        private void QuickInput(ref ctlLookUpEdit input, string itmId)
        {
            if (input != null && input.EditValue != null)
            {
                string filter = input.Text.Trim();

                DataTable thisTable = (DataTable)input.Properties.DataSource;
                AddTmpProp(thisTable, filter, itmId);
                string inputStr = input.EditValue.ToString().Trim();
                if (inputStr == "-")
                {
                    gvItem.SetFocusedValue("阴   性");
                }
                else if (inputStr == "+")
                {
                    gvItem.SetFocusedValue("阳   性");
                }
                else if (inputStr == "*")
                {
                    gvItem.SetFocusedValue("弱阳性");
                }


            }
        }

        private void QuickInput(ref LookUpEdit input, string itmId)
        {
            if (input != null && input.EditValue != null)
            {
                string filter = input.Text.Trim();

                DataTable thisTable = (DataTable)input.Properties.DataSource;
                AddTmpProp(thisTable, filter, itmId);
                string inputStr = input.EditValue.ToString().Trim();
                if (inputStr == "-")
                {
                    gvItem.SetFocusedValue("阴   性");
                }
                else if (inputStr == "+")
                {
                    gvItem.SetFocusedValue("阳   性");
                }
                else if (inputStr == "*")
                {
                    gvItem.SetFocusedValue("弱阳性");
                }


            }
        }

        private void QuickInput(ref TextEdit input, string itmId)
        {
            if (input != null && input.EditValue != null)
            {
                string filter = input.Text.Trim();

                string inputStr = input.EditValue.ToString().Trim();
                if (inputStr == "-")
                {
                    gvItem.SetFocusedValue("阴   性");
                }
                else if (inputStr == "+")
                {
                    gvItem.SetFocusedValue("阳   性");
                }
                else if (inputStr == "*")
                {
                    gvItem.SetFocusedValue("弱阳性");
                }


            }
        }
        /// <summary>
        /// 切换到下一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repProp_KeyDown(object sender, KeyEventArgs e)
        {
            //ctlLookUpEdit thisEdit = (ctlLookUpEdit)sender;
            //LookUpEdit thisEdit = (LookUpEdit)sender;
            //if (thisEdit.IsPopupOpen == false && e.KeyCode == Keys.Enter)
            //{
            //    DataRowView dvr = (DataRowView)(gvItem.GetFocusedRow());
            //    string itmId = dvr[gvItem.FocusedColumn.VisibleIndex - 1].ToString();
            //    string filter = thisEdit.Text.Trim();
            //    DataTable thisTable = (DataTable)thisEdit.Properties.DataSource;

            //    AddTmpProp(thisTable, filter, itmId);

            //    if (gvItem.FocusedRowHandle < gvItem.RowCount - 1)
            //    {
            //        gvItem.MoveNext();
            //    }
            //    else
            //    {
            //        if (gvItem.FocusedColumn.VisibleIndex < gvItem.VisibleColumns.Count - 1)
            //        {
            //            gvItem.FocusedColumn = gvItem.VisibleColumns[gvItem.FocusedColumn.VisibleIndex + 2];
            //            gvItem.MoveFirst();
            //        }
            //    }
            //}
            DataRowView dvr = (DataRowView)(gvItem.GetFocusedRow());
            ComboBoxEdit thisEdit = (ComboBoxEdit)sender;
            string filter = dvr[gvItem.FocusedColumn.VisibleIndex - 1].ToString();

            List<EntityDefItmProperty> listProps = new List<EntityDefItmProperty>();
            if (filter == "")
            {
                listProps = listItmProp;
            }
            else
            {
                listProps = listItmProp.FindAll(i => i.PtyItmId == filter || i.PtyId == "" || i.PtyItmFlag == 1);
            }

            thisEdit.Properties.Items.Clear();
            foreach (EntityDefItmProperty row in listProps)
            {
                thisEdit.Properties.Items.Add(row.PtyItmProperty.ToString());
            }



            if (thisEdit.IsPopupOpen == false && e.KeyCode == Keys.Enter)
            {
                if (gvItem.FocusedRowHandle < gvItem.RowCount - 1)
                {
                    gvItem.MoveNext();
                }
                else
                {
                    if (gvItem.FocusedColumn.VisibleIndex < gvItem.VisibleColumns.Count - 1)
                    {
                        gvItem.FocusedColumn = gvItem.VisibleColumns[gvItem.FocusedColumn.VisibleIndex + 2];
                        gvItem.MoveFirst();
                    }
                }
            }
        }

        private void gvItem_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRow dr = gvItem.GetDataRow(e.RowHandle);
            string strItm1 = dr[1].ToString();
            string strItm3 = dr[3].ToString();
            string strItm5 = dr[5].ToString();
            if (strItm1.IndexOf("阳") >= 0 && strItm1.IndexOf("弱") < 0)
            {
                if (e.Column.FieldName == "chr0")
                    e.Appearance.ForeColor = Color.Red;
            }
            if (strItm1 == "弱阳性")
            {
                if (e.Column.FieldName == "chr0")
                    e.Appearance.ForeColor = Color.Blue;
            }

            if (strItm3.IndexOf("阳") >= 0 && strItm3.IndexOf("弱") < 0)
            {
                if (e.Column.FieldName == "chr1")
                    e.Appearance.ForeColor = Color.Red;
            }
            if (strItm3 == "弱阳性")
            {
                if (e.Column.FieldName == "chr1")
                    e.Appearance.ForeColor = Color.Blue;
            }

            if (strItm5.IndexOf("阳") >= 0 && strItm5.IndexOf("弱") < 0)
            {
                if (e.Column.FieldName == "chr2")
                    e.Appearance.ForeColor = Color.Red;
            }
            if (strItm5 == "弱阳性")
            {
                if (e.Column.FieldName == "chr2")
                    e.Appearance.ForeColor = Color.Blue;
            }

        }

        private void repResulto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gvItem.FocusedRowHandle < gvItem.RowCount - 1)
                {
                    gvItem.MoveNext();
                }
                else
                {
                    if (gvItem.FocusedColumn.VisibleIndex < gvItem.VisibleColumns.Count - 1)
                    {
                        gvItem.FocusedColumn = gvItem.VisibleColumns[gvItem.FocusedColumn.VisibleIndex + 2];
                        gvItem.MoveFirst();
                    }
                }
            }
        }

        /// <summary>
        /// 记录双击事件HashCode
        /// </summary>
        private int colHashCode = -1;//确保相同HashCode只执行一次

        /// <summary>
        /// 双击测定结果弹出项目特征框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColumnEdit_DoubleClick(object sender, EventArgs e)
        {
            if (colHashCode == e.GetHashCode())
                return;

            colHashCode = e.GetHashCode();//防止多次执行相同事件

            if (this.gvItem == null)
                return;

            //int colCount = this.gvItem.Columns.Count;

            DataRow rowItem = gvItem.GetFocusedDataRow();
            DevExpress.XtraGrid.Columns.GridColumn douColA = this.gvItem.FocusedColumn;
            int douRowIndex = gvItem.FocusedRowHandle;
            int douColIndex = douColA.AbsoluteIndex;
            string fieldName = douColA.FieldName;

            if (!fieldName.StartsWith("chr"))
                return;

            if (douColIndex % 2 != 1)
                return;

            //DevExpress.XtraGrid.Columns.GridColumn douColB = this.gvItem.GetVisibleColumn(douColIndex - 1);
            #region 双击记录赋值

            noteColName = fieldName;
            noteRowIndex = douRowIndex;

            #endregion

            string item_id = rowItem[douColIndex - 1].ToString();
            if (!string.IsNullOrEmpty(item_id))
            {
                ShowItemProp(item_id);
            }
            else
            {
                if (prop != null)
                {
                    prop.Hide();//隐藏项目特征选择框
                }
            }

            //string fieldName = this.gvItem.FocusedColumn.FieldName;

            //string itemFieldName;
            //if (fieldName.StartsWith("chr"))
            //{
            //    itemFieldName = "itm" + fieldName.Substring(3);
            //}
            //else
            //{
            //    itemFieldName = fieldName;
            //}
            //string itm_id = rowItem[itemFieldName].ToString();

            //if(this.fr
        }

        /// <summary>
        /// 显示项目特征选择窗体
        /// </summary>
        /// <param name="p_itmID"></param>
        private void ShowItemProp(string p_itmID)
        {
            if (string.IsNullOrEmpty(p_itmID)) return;

            if (prop == null)
            {
                prop = new FrmItmPropLstV3();
                prop.OnClickSelected += new FrmItmPropLstV3.ClickSelectedEventHandler(get_ItemProp);
                //Point p = this.PointToClient(this.PointToClient(new Point(0, 0)));
                prop.Left = this.PointToScreen(new Point(0, 0)).X + 300;
                prop.Top = this.PointToScreen(new Point(0, 0)).Y + 100;
            }

            prop.Visible = true;

            prop.SetItemID(p_itmID);

            try
            {
                prop.Show();
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "ShowProp", ex.ToString());
            }
            finally
            {

            }
        }

        /// <summary>
        /// 获取项目特征值
        /// </summary>
        /// <param name="item_prop"></param>
        private void get_ItemProp(string p_itemProp)
        {
            if (string.IsNullOrEmpty(p_itemProp))
                return;

            try
            {
                if ((!string.IsNullOrEmpty(noteColName)) && (noteRowIndex != -1))
                {
                    if (p_itemProp.Split(';').Length > 1)
                    {
                        int count = p_itemProp.Split(';').Length;

                        for (int i = 0; i < count; i++)
                        {
                            if (gvItem.RowCount >= (noteRowIndex + i) + 1)
                            {
                                gvItem.SetRowCellValue(noteRowIndex + i, noteColName, p_itemProp.Split(';')[i]);
                            }
                        }
                    }
                    else
                    {
                        gvItem.SetRowCellValue(noteRowIndex, noteColName, p_itemProp);
                    }
                    //清空记录
                    noteColName = null;
                    noteRowIndex = -1;
                }
            }
            catch (Exception e)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "GetItemProp", e.ToString());
            }

        }
        private void gdItem_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gvItem_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName.StartsWith("chr"))
            {
                string itemFieldName = "itm" + e.Column.FieldName.Substring(3);

                DataRow row = gvItem.GetFocusedDataRow();
                string itm_id = row[itemFieldName].ToString();
                if (itm_id != string.Empty)
                {
                    string currVal = e.Value.ToString();

                    EntityDefItmProperty item = listItmProp.Find(i => i.PtyItmId == itm_id && i.PtyCCode == currVal);
                    string val = item != null ? item.PtyItmProperty : string.Empty;
                    if (val != string.Empty)
                    {
                        row[e.Column.FieldName] = val;
                    }
                }
            }
        }
    }
}
