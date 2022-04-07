using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.common;
using dcl.common.extensions;
using DevExpress.XtraGrid.Views.Grid;

namespace dcl.client.dicbasic
{
    /// <summary>
    /// 检索控件
    /// <para>1.拉此控件到项目的界面</para>
    /// <para>2.在doReseach方法和save方法成功后添加 searchControl1.Initialize(this, gridView1, bsDiagnos, dtHospital, dtAll);</para>
    /// <para>（具体参考dcl.client.dicbasic.ConCheckb的82与129行）</para>
    /// </summary>
    public partial class SearchControl : DevExpress.XtraEditors.XtraUserControl
    {
        public SearchControl()
        {
            InitializeComponent();

            this.FilterOnEnterKeyDown = false;
            this.txtSort.EnterMoveNextControl = false;
        }

        public DataTable dtData;
        public DataTable dtAll;
        public BindingSource BindingSource;
        public string[] Columns;

        public bool FilterOnEnterKeyDown { get; set; }

        /// <summary>
        /// 输入文本有变化时触发
        /// </summary>
        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            if (!Useable)
                return;

            if (FilterOnEnterKeyDown && this.txtSort.Text != string.Empty)
                return;

            if (this.txtSort.Text == "")
            {
                dtData.Clear();
                dtData.Merge(dtAll);
                BindingSource.DataSource = dtData;
            }
            else
            {
                string sortText = dcl.common.SQLFormater.Format(txtSort.Text);
                sortText = sortText.Replace("，", " ").Replace(",", " ").Replace("　", " ");

                //   DataTable dt = dtData.Clone();
                dtData.Clear();
                if (Extensions.IsEmpty(dtAll))
                    return;
                DataRow[] dr = dtAll.Select(GetFitler(Columns, sortText));
                for (int i = 0; i < dr.Length; i++)
                {
                    dtData.ImportRow(dr[i]);
                }
                BindingSource.DataSource = dtData;
            }
        }

        private void txtSort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && FilterOnEnterKeyDown)
            {
                bool b = FilterOnEnterKeyDown;
                FilterOnEnterKeyDown = false;
                txtSort_EditValueChanged(this.txtSort, EventArgs.Empty);
                FilterOnEnterKeyDown = b;
            }
        }

        /// <summary>
        /// 动态获取列表的多列查询字符串，此功能支持多词查询
        /// </summary>
        /// <param name="Columns">列表列名</param>
        /// <param name="input">多词</param>
        /// <returns></returns>
        public string GetFitler(string[] Columns, string input)
        {
            if (Columns == null || Columns.Length == 0 || Extensions.IsEmpty(input))
                return "";
            string result = "";

            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                String sb = "";
                foreach (string item in Columns)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item == "com_name")
                        {
                            sb += string.Format(" or {0} like '%{1}%'", item, word);
                        }
                        else
                        {
                            sb += string.Format(" or {0} like '%{1}%'", item, word);
                        }
                    }
                }
                sb = sb.Remove(0, 4);

                result += string.Format("And ( {0} ) ", sb);
            }

            result = result.Remove(0, 3);
            return result;
        }


        /// <summary>
        /// 初始化检索控件所需数据
        /// </summary>     
        /// <param name="gridView">绑定的GridView</param>
        /// <param name="bindingSource">数据源</param>
        /// <param name="dtData">源表</param>
        /// <param name="dtAll">源表</param>
        public void Initialize(GridView gridView, BindingSource bindingSource, DataTable dtData, DataTable dtAll)
        {
            if (gridView != null && gridView.Columns != null)
            {
                List<string> columnsName = new List<string>();
                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (
                            (
                            gridView.Columns[i].Visible
                            && gridView.Columns[i].ColumnType != Type.GetType("System.Int32")
                            && gridView.Columns[i].ColumnType != Type.GetType("System.Int16")
                            && gridView.Columns[i].ColumnType != Type.GetType("System.Int64")
                            && gridView.Columns[i].ColumnType != Type.GetType("System.DateTime")
                            && gridView.Columns[i].ColumnType != Type.GetType("System.Boolean")
                            && gridView.Columns[i].ColumnType != Type.GetType("System.Decimal")
                            ) && (gridView.Columns[i].FieldName != "ctype_name" && gridView.Columns[i].FieldName != "ptype_name")
                        )
                        columnsName.Add(gridView.Columns[i].FieldName);
                }

                Columns = columnsName.ToArray();
            }

            this.BindingSource = bindingSource;
            this.dtAll = dtAll.Copy();
            this.dtData = dtData;
            Columns = RemoveNotStringColumns(Columns, dtAll);
        }

        /// <summary>
        /// 忽略非string的列
        /// </summary>
        /// <param name="columns">当前可见列</param>
        /// <param name="dtData">源数据表</param>
        /// <returns>整理后的列集合</returns>
        private string[] RemoveNotStringColumns(string[] columns, DataTable dtData)
        {
            if (Extensions.IsEmpty(columns))
                return columns;

            List<string> columnsList = new List<string>(columns);
            foreach (DataColumn column in dtData.Columns)
            {
                if (column.DataType.FullName == "System.Int32"
                    || column.DataType.FullName == "System.Int16"
                    || column.DataType.FullName == "System.Int64"
                    || column.DataType.FullName == "System.DateTime"
                    || column.DataType.FullName == "System.Boolean"
                    || column.DataType.FullName == "System.Decimal")

                    columnsList.Remove(column.ColumnName);

                //五笔拼音码应添加
                if (column.ColumnName.IndexOf("_py") >= 0 || column.ColumnName.IndexOf("_wb") >= 0)
                    columnsList.Add(column.ColumnName);
            }

            return columnsList.ToArray();
        }

        /// <summary>
        /// 初始化检索控件所需数据
        /// </summary>    
        /// <param name="columns">要检索的列名</param>
        /// <param name="bindingSource"></param>
        /// <param name="dtHospital"></param>
        /// <param name="dtAll"></param>
        public void Initialize(string[] columns, BindingSource bindingSource, DataTable dtData, DataTable dtAll)
        {
            Columns = columns;
            this.BindingSource = bindingSource;
            this.dtAll = dtAll.Copy();
            this.dtData = dtData;
            // Columns = RemoveNotStringColumns(columns, dtAll);
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Useable
        {
            get
            {
                return Extensions.IsNotEmpty(Columns) && BindingSource != null && dtData != null && dtAll != null;
            }
        }


    }
}
