using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.result.CommonPatientInput;

namespace dcl.client.result
{
    public partial class MultiSampleResult : UserControl
    {
        public string itm_id { get; set; }

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

        private int startpid = 0;

        //样本特征下拉框数据源
        private DataTable itemProp = new DataTable();

        //public DataTable ItemSid = null;
        public List<String> listSid = null;
        public bool FilterItem = false;

        //显示列表的数据源
        private DataTable dataSource = new DataTable();
        /// <summary>
        /// 是否在结果里补加内容
        /// </summary>
        public bool blnMergCut = false;
        /// <summary>
        /// 在结果里补加的内容
        /// </summary>
        public string strMergCut = string.Empty;

        public MultiSampleResult()
        {
            InitializeComponent();
            gvMultiSid.CustomRowCellEdit += new CustomRowCellEditEventHandler(gvMultiSid_CustomRowCellEdit);
        }

      

        /// <summary>
        /// 清空当前所有结果并更新特征值选择源
        /// </summary>
        /// <param name="dt"></param>
        public void SetPropSource(DataTable dt)
        {
            if (dataSource.Columns.Count == 0)
            {
                //防止还未初始化表结构时即执行
                return;
            }

            //清空当前所有结果
            for (int i = 0; i < dataSource.Rows.Count; i++)
            {
                for (int j = 1; j < dataSource.Columns.Count; j = j + 2)
                {
                    dataSource.Rows[i][j] = "";
                }
            }

            itemProp = dt;
            repProp.DataSource = dt;
        }

        /// <summary>
        /// 返回数据填写结果
        /// </summary>
        public DataTable GetDataSource()
        {
            return dataSource;
        }

        internal void QuickInput(int startSid, int endSid, int cols, List<int> list, string input)
        {
            //总样本数
            int length = endSid - startSid + 1;

            //总行数
            int rowCount = length / cols;
            if (length % cols != 0)
            {
                rowCount += 1;
            }

            DataTable dtSource = (gvMultiSid.DataSource as DataView).Table;
            //填充数据
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {

                //步长为2,跳过结果录入列
                for (int j = 1; j < dtSource.Columns.Count; j = j + 2)
                {

                    //string colName = (dr[j] as GridColumn).ColumnName;
                    //int colIndex = int.Parse(colName.Substring(3));

                    ////计算当前样本号
                    int thisSample = startSid + (j / 2) * rowCount + i;

                    //if (thisSample <= endSid)
                    //{
                    //    dr[j] = thisSample.ToString();
                    //}
                    //else
                    if (thisSample <= endSid && list.Contains(thisSample))
                        dtSource.Rows[i][j] = input;
                    //}
                }
            }
        }


        void repositoryItemTextEdit1_Click(object sender, System.EventArgs e)
        {
            //TextEdit thisEdit = (TextEdit)sender;
            //if (thisEdit != null)
            //{
            //    thisEdit.SelectAll();
            //}
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="startSid">样本始号</param>
        /// <param name="endSid">样本终号</param>
        /// <param name="cols">每1行显示样本号的列数_1列样本实际对应列表中的2列</param>
        internal void QuickInput(int startSid, int endSid, int cols, string input)
        {
            //总样本数
            int length = endSid - startSid + 1;

            //总行数
            int rowCount = length / cols;
            if (length % cols != 0)
            {
                rowCount += 1;
            }

            DataTable dtSource = (gvMultiSid.DataSource as DataView).Table;
            //填充数据
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {

                //步长为2,跳过结果录入列
                for (int j = 1; j < dtSource.Columns.Count; j = j + 2)
                {

                    //string colName = (dr[j] as GridColumn).ColumnName;
                    //int colIndex = int.Parse(colName.Substring(3));

                    ////计算当前样本号
                    int thisSample = startSid + (j / 2) * rowCount + i;

                    //if (thisSample <= endSid)
                    //{
                    //    dr[j] = thisSample.ToString();
                    //}
                    //else
                    if (thisSample <= endSid)
                        dtSource.Rows[i][j] = input;
                    //}
                }
            }
        }

        private void gvMultiSid_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
           
        }

        public string sidName
        {
            set
            {
                foreach (GridColumn col in gvMultiSid.Columns)
                {
                    if (col.FieldName.Contains("sid"))
                    {
                        col.Caption = value;
                    }
                }
            }
        }

        /// <summary>
        /// 生成用户录入列表
        /// </summary>
        /// <param name="startSid">样本始号</param>
        /// <param name="endSid">样本终号</param>
        /// <param name="cols">每1行显示样本号的列数_1列样本实际对应列表中的2列</param>
        public void LoadGrid(int startSid, int endSid, int cols)
        {
            startpid = startSid - 1;
            //总样本数
            int length = endSid - startSid + 1;

            //总行数
            int rowCount = length / cols;
            if (length % cols != 0)
            {
                rowCount += 1;
            }

            dataSource = new DataTable();

            //生成列
            for (int i = 0; i < cols; i++)
            {
                DataColumn colSample = new DataColumn();
                colSample.ColumnName = "sid" + i.ToString();
                colSample.Caption = "样本号";
                dataSource.Columns.Add(colSample);

                DataColumn colResult = new DataColumn();
                colResult.ColumnName = "res" + i.ToString();
                colResult.Caption = "测定结果";
                dataSource.Columns.Add(colResult);
            }

            //填充数据
            for (int i = 0; i < rowCount; i++)
            {
                DataRow dr = dataSource.NewRow();

                //步长为2,跳过结果录入列
                for (int j = 0; j < dataSource.Columns.Count; j = j + 2)
                {
                    string colName = dataSource.Columns[j].ColumnName;
                    int colIndex = int.Parse(colName.Substring(3));

                    //计算当前样本号
                    int thisSample = startSid + (j / 2) * rowCount + i;

                    if (thisSample <= endSid)
                    {
                        dr[j] = thisSample.ToString();
                    }
                    //else
                    //{
                    //    dr[j] = "";
                    //}
                }

                dataSource.Rows.Add(dr);
            }

            gdMultiSid.DataSource = dataSource;

            //控制列表样式
            for (int i = 0; i < gvMultiSid.Columns.Count; i++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = gvMultiSid.Columns[i];
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                if (i % 2 == 0)
                {
                    //样本号列
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.AllowFocus = false;
                    col.Width = 80;
                    col.OptionsColumn.FixedWidth = true;
                    col.AppearanceCell.Options.UseBackColor = true;
                    col.AppearanceCell.BackColor = Color.LightYellow;
                }
                else
                {
                    //结果列
                    col.ColumnEdit = repositoryItemTextEdit1;
                    col.ColumnEdit.DoubleClick += new EventHandler(ColumnEdit_DoubleClick);
                }
            }
        }



        /// <summary>
        /// 使控件能临时录入值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repProp_Leave(object sender, EventArgs e)
        {
            LookUpEdit thisEdit = (LookUpEdit)sender;
            string filter = thisEdit.Text.Trim();

            // AddTmpProp(filter);
            //QuickInput(ref thisEdit);
        }

        /// <summary>
        /// 快速录入，把“－”“+”号换成“阴性”“阳性”
        /// </summary>
        /// <param name="input"></param>
        private void QuickInput(ref LookUpEdit input)
        {

            if (input != null && input.EditValue != null)
            {
                string filter = input.Text.Trim();

                //DataTable thisTable = (DataTable)input.Properties.DataSource;
                //AddTmpProp(filter);
                //string inputStr = input.EditValue.ToString().Trim();
                //if (inputStr == "-")
                //{
                //    gvMultiSid.SetFocusedValue("阴   性");
                //}
                //else if (inputStr == "+")
                //{
                //    gvMultiSid.SetFocusedValue("阳   性");
                //}
                //else if (inputStr == "*")
                //{
                //    gvMultiSid.SetFocusedValue("弱阳性");
                //}


            }
        }

        /// <summary>
        /// 临时添加可用特征值
        /// </summary>
        private void AddTmpProp(string filter)
        {

            DataRow[] drs = itemProp.Select("itm_prop='" + filter.Replace("'", "''") + "'");
            if (drs.Length < 1)
            {
                DataRow drPropNull = itemProp.NewRow();
                drPropNull["pro_id"] = "录入";
                drPropNull["itm_prop"] = filter;

                itemProp.Rows.Add(drPropNull);


                itemProp.AcceptChanges();
                dataSource.AcceptChanges();
                gvMultiSid.SetFocusedValue(filter);
            }
        }

        void repositoryItemTextEdit1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            TextEdit thisEdit = (TextEdit)sender;

            if (e.KeyCode == Keys.Enter)
            {
                //string filter = thisEdit.Text.Trim();

                //AddTmpProp(filter);
                string inputStr = thisEdit.EditValue.ToString().Trim();

                //if (blnMergCut)
                //{
                //    gvMultiSid.SetFocusedValue(inputStr + strMergCut);
                //}

                //if (inputStr == "-")
                //{
                //    gvMultiSid.SetFocusedValue("阴性(-)");
                //}
                //else if (inputStr == "+")
                //{
                //    gvMultiSid.SetFocusedValue("阳性(+)");
                //}
                //else if (inputStr == "*")
                //{
                //    gvMultiSid.SetFocusedValue("弱阳性");
                //}

                //if (blnMergCut)
                //{
                //    gvMultiSid.SetFocusedValue(inputStr + strMergCut);
                //}

                if (gvMultiSid.FocusedRowHandle < gvMultiSid.RowCount - 1)
                {
                    gvMultiSid.MoveNext();
                }
                else
                {
                    if (gvMultiSid.FocusedColumn.VisibleIndex < gvMultiSid.VisibleColumns.Count - 1)
                    {
                        gvMultiSid.FocusedColumn = gvMultiSid.VisibleColumns[gvMultiSid.FocusedColumn.VisibleIndex + 2];
                        gvMultiSid.MoveFirst();
                    }
                }
            }
        }


        /// <summary>
        /// 回车跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repProp_KeyDown(object sender, KeyEventArgs e)
        {
            LookUpEdit thisEdit = (LookUpEdit)sender;

            if (thisEdit.IsPopupOpen == false && e.KeyCode == Keys.Enter)
            {
                string filter = thisEdit.Text.Trim();

                AddTmpProp(filter);

                if (gvMultiSid.FocusedRowHandle < gvMultiSid.RowCount - 1)
                {
                    gvMultiSid.MoveNext();
                }
                else
                {

                    if (gvMultiSid.FocusedColumn.VisibleIndex < gvMultiSid.VisibleColumns.Count - 1)
                    {
                        gvMultiSid.FocusedColumn = gvMultiSid.VisibleColumns[gvMultiSid.FocusedColumn.VisibleIndex + 2];
                        gvMultiSid.MoveFirst();
                    }
                }
            }
        }

        private void gvMultiSid_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRow dr = gvMultiSid.GetDataRow(e.RowHandle);
            setColor(e, dr[1].ToString(), "res0");
            setColor(e, dr[3].ToString(), "res1");
            setColor(e, dr[5].ToString(), "res2");
            setColor(e, dr[7].ToString(), "res3");
            setColor(e, dr[9].ToString(), "res4");

            //if (FilterItem && ItemSid != null && e.Column.Name.Contains("res"))
            //{
            //    int colIndex = int.Parse(e.Column.Name.Substring(6));
            //    int sid = (e.RowHandle + 1) * (colIndex + 1);

            //    if (ItemSid.Select(string.Format("pat_sid='{0}'", sid)).Length > 0)
            //    {
            //        e.Column.OptionsColumn.AllowEdit = true;
            //    }
            //    else
            //    {
            //        e.Column.OptionsColumn.AllowEdit = false;
            //    }
            //}
        }

        private void setColor(DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (value.Contains("弱阳性"))
            {
                if (e.Column.FieldName == name)
                    e.Appearance.ForeColor = Color.Blue;
            }
            else if (value.Contains("阳性"))
            {
                if (e.Column.FieldName == name)
                    e.Appearance.ForeColor = Color.Red;
            }

        }

        private void gvMultiSid_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gvItem = sender as GridView;
            if (e.Column.FieldName.StartsWith("res"))
            {
                //string itemFieldName = "itm" + e.Column.FieldName.Substring(3);

                DataRow row = gvItem.GetFocusedDataRow();
                //string itm_id = row[itemFieldName].ToString();
                //if (itm_id != string.Empty)
                //{
                string currVal = e.Value.ToString();

                string val = DictItem.Instance.GetItmProp(this.itm_id, currVal);

                if (blnMergCut)
                {
                    if (val != string.Empty)
                    {
                        val = val + strMergCut;
                    }
                    else
                    {
                        val = currVal + strMergCut;
                    }
                }
                if (val != string.Empty)
                {
                    row[e.Column.FieldName] = val;
                }
                //}
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
        private void ColumnEdit_DoubleClick(object sender, EventArgs e)
        {
            if (colHashCode == e.GetHashCode())
                return;

            colHashCode = e.GetHashCode();//防止多次执行相同事件

            if (this.itm_id == null)//项目ID不为空
                return;

            //int colCount = this.gvItem.Columns.Count;

            DataRow rowItem = gvMultiSid.GetFocusedDataRow();
            DevExpress.XtraGrid.Columns.GridColumn douColA = this.gvMultiSid.FocusedColumn;
            int douRowIndex = gvMultiSid.FocusedRowHandle;
            int douColIndex = douColA.AbsoluteIndex;
            string fieldName = douColA.FieldName;

            if (!fieldName.StartsWith("res"))
                return;

            if (douColIndex % 2 != 1)
                return;

            //DevExpress.XtraGrid.Columns.GridColumn douColB = this.gvItem.GetVisibleColumn(douColIndex - 1);
            #region 双击记录赋值

            noteColName = fieldName;
            noteRowIndex = douRowIndex;

            #endregion

            if (!string.IsNullOrEmpty(this.itm_id))
            {
                ShowItemProp(this.itm_id);
            }
            else
            {
                if (prop != null)
                {
                    prop.Hide();//隐藏项目特征选择框
                }
            }

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
                            if (gvMultiSid.RowCount >= (noteRowIndex + i) + 1)
                            {
                                gvMultiSid.SetRowCellValue(noteRowIndex + i, noteColName, p_itemProp.Split(';')[i]);
                            }
                        }
                    }
                    else
                    {
                        gvMultiSid.SetRowCellValue(noteRowIndex, noteColName, p_itemProp);
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

        private void gvMultiSid_Click(object sender, EventArgs e)
        {
            //if (FilterItem && ItemSid != null && gvMultiSid.FocusedColumn.Name.Contains("res"))
            //{
            //    int colIndex = int.Parse(gvMultiSid.FocusedColumn.Name.Substring(6));
            //    int sid = (gvMultiSid.FocusedRowHandle + 1) * (colIndex + 1);

            //    if (ItemSid.Select(string.Format("pat_sid='{0}'", sid)).Length > 0)
            //    {
            //        gvMultiSid.FocusedColumn.OptionsColumn.AllowEdit = true;
            //    }
            //    else
            //    {
            //        gvMultiSid.FocusedColumn.OptionsColumn.AllowEdit = false;
            //    }
            //}
        }

        private void gdMultiSid_Click(object sender, EventArgs e)
        {
            //if (FilterItem && ItemSid != null && gvMultiSid.FocusedColumn.Name.Contains("res"))
            //{
            //    int colIndex = int.Parse(gvMultiSid.FocusedColumn.Name.Substring(6));
            //    int sid = (gvMultiSid.FocusedRowHandle + 1) * (colIndex + 1);

            //    if (ItemSid.Select(string.Format("pat_sid='{0}'", sid)).Length > 0)
            //    {
            //        gvMultiSid.FocusedColumn.OptionsColumn.AllowEdit = true;
            //    }
            //    else
            //    {
            //        gvMultiSid.FocusedColumn.OptionsColumn.AllowEdit = false;
            //    }
            //}
        }


        public void SetEditableSource()
        {
            //if (FilterItem && ItemSid != null)
            //{
            //    foreach (GridColumn col in gvMultiSid.Columns)
            //    {
            //        string fieldName = col.FieldName;

            //        if (!fieldName.StartsWith("res"))
            //            return;
            //        int colIndex = int.Parse(col.Name.Substring(6));
                 
            //        for (int i = 0; i < gvMultiSid.RowCount; i++)
            //        {
            //            int sid = (i + 1) * (colIndex + 1);
            //            if (ItemSid.Select(string.Format("pat_sid='{0}'", sid)).Length == 0)
            //            {
            //                gvMultiSid.CustomDrawCell
            //            }
            //        }
            //    }

            //    if (ItemSid.Select(string.Format("pat_sid='{0}'", sid)).Length == 0)
            //    {
            //        row[e.Column.FieldName] = "";
            //    }
            //    return;
            //}
        }

        private void gvMultiSid_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (FilterItem && listSid != null)
            {
                string fieldName = e.Column.FieldName;

                if (!fieldName.StartsWith("res"))
                    return;
                int colIndex = int.Parse(e.Column.Name.Substring(6));

                int rowcount = gvMultiSid.RowCount;
                int sid = (e.RowHandle + 1) + colIndex*rowcount;
                if ( listSid.FindAll(i=>i==(sid+startpid).ToString()).Count>0)
                {
                    e.Appearance.BackColor = Color.Silver;
                }
                else
                {
                    e.Appearance.BackColor = Color.White;
                }


            }
        }
    }
}
