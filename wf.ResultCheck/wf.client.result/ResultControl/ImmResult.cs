using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace dcl.client.result
{
    public partial class ImmResult : UserControl
    {
        //显示列表的数据源
        private DataTable dataSource = new DataTable();

        string[] rowChar = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
        string[] valueChar = { "-", "+", "?" };

        public ImmResult()
        {
            InitializeComponent();
        }

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        /// <summary>
        /// 返回数据填写结果
        /// </summary>
        public DataTable GetDataSource()
        {
            return dataSource;
        }

        /// <summary>
        /// 用户控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImmResult_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            //载入8*12酶标板
            LoadGrid(8, 12);
        }

        /// <summary>
        /// 生成酶标板
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="cols">列数</param>
        public void LoadGrid(int rows, int cols)
        {
            dataSource = new DataTable();

            //生成列
            for (int i = 0; i < cols+1; i++)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = "col" + i.ToString();
                if (i == 0)
                {
                    col.Caption = "";
                }
                else
                {
                    col.Caption = i.ToString();
                }
                dataSource.Columns.Add(col);
            }

            //填充数据
            for (int i = 1; i < rows+1; i++)
            {
                DataRow dr = dataSource.NewRow();
                dr[0] = i.ToString() + rowChar[i-1];

                dataSource.Rows.Add(dr);
            }

            QuickResult("");

            gdImm.DataSource = dataSource;

            //控制列表样式
            for (int i = 0; i < gvImm.Columns.Count; i++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = gvImm.Columns[i];
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                if (i == 0)
                {
                    //编号列
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.AllowFocus = false;
                    col.Width = 80;
                    col.OptionsColumn.FixedWidth = true;
                    col.AppearanceCell.Options.UseBackColor = true;
                    col.AppearanceCell.BackColor = Color.LightYellow;
                }
                else
                {
                    //双击改变结果的实现
                    col.ColumnEdit = repText;
                }
            }
        }

        /// <summary>
        /// 快速设置所有数据为同一值
        /// </summary>
        /// <param name="result"></param>
        public void QuickResult(string result)
        {
            for (int i = 0; i < dataSource.Rows.Count;i++ )
            {
                for (int j = 1; j < dataSource.Columns.Count; j++)
                {
                    dataSource.Rows[i][j] = result;
                }
            }
        }

        /// <summary>
        /// 只允许录入特定值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvImm_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string thisValue = e.Value.ToString();

            if (thisValue!="" && Array.IndexOf(valueChar, thisValue) == -1)
            {
                gvImm.SetRowCellValue(e.RowHandle, e.Column, "");
            }
        }


        /// <summary>
        /// 阳性显示为红色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvImm_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            string thisValue = gvImm.GetRowCellValue(e.RowHandle,e.Column).ToString();
            if (thisValue == "+")
            {
                e.Appearance.ForeColor = Color.Red;
            }
            else
            {
                e.Appearance.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 双击切换结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repText_DoubleClick(object sender, EventArgs e)
        {
            TextEdit thisEdit = (TextEdit)sender;
            string thisValue = thisEdit.Text;

            int index = Array.IndexOf(valueChar, thisValue);

            if (index == valueChar.Length - 1)
            {
                //结果为最后一项时循环到首项
                index = -1;
            }

            index++;

            gvImm.SetFocusedValue(valueChar[index]);
        }

    }
}
