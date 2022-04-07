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

namespace dcl.client.sample
{
    /// <summary>
    /// 检索控件
    /// <para>1.拉此控件到项目的界面</para>
    /// <para>2.在doReseach方法和save方法成功后添加 searchControl1.Initialize(this, gridView1, bsDiagnos, dtHospital, dtAll);</para>
    /// <para>（具体参考lis.client.dict.ConCheckb的82与129行）</para>
    /// </summary>
    public partial class SearchControl : DevExpress.XtraEditors.XtraUserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        public DataTable dtData;
        public DataTable dtAll;
        public BindingSource BindingSource;
        public string[] Columns;

        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            if (!Useable)
                return;

            if (this.txtSort.Text == "")
            {
                BindingSource.DataSource = dtData = dtAll;
            }
            else
            {
                string sortText = dcl.common.SQLFormater.Format(txtSort.Text);
                sortText = sortText.Replace("，", " ").Replace(",", " ").Replace("　", " ");

                DataTable dt = dtData.Clone();
                 if (Extensions.IsEmpty(dtAll))
                    return;
                DataRow[] dr = dtAll.Select(GetFitler(Columns, sortText));
                for (int i = 0; i < dr.Length; i++)
                {
                    dt.ImportRow(dr[i]);
                }
                BindingSource.DataSource = dtData = dt;
            }
        }

        /// <summary>
        /// 重新查询
        /// </summary>
        public void SearchAgain()
        {
            txtSort_EditValueChanged(this, EventArgs.Empty);
        }

        //public string GetFitler(string[] Columns, string input)
        //{
        //    if (Columns == null || Columns.Length == 0)
        //        return "";

        //    StringBuilder sb = new StringBuilder();

        //    foreach (string item in Columns)
        //    {
        //        if (!string.IsNullOrEmpty(item))
        //        {
        //            sb.Append(string.Format(" or {0} like '%{1}%'", item, input));
        //        }
        //    }
        //    sb.Remove(0, 4);

        //    return sb.ToString();
        //}

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

                        sb += string.Format(" or {0} like '%{1}%'", item, word);

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
        /// <param name="action"></param>
        /// <param name="gridView">绑定的GridView</param>
        /// <param name="bindingSource"></param>
        /// <param name="dtHospital"></param>
        /// <param name="dtAll"></param>
        public void Initialize(GridView gridView, BindingSource bindingSource, DataTable dtData, DataTable dtAll)
        {
            if (gridView != null && gridView.Columns != null)
            {
                List<string> columnsName = new List<string>();
                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (gridView.Columns[i].Visible == true && gridView.Columns[i].ColumnType != Type.GetType("System.Int32")
                        && gridView.Columns[i].ColumnType != Type.GetType("System.Int16")
                        && gridView.Columns[i].ColumnType != Type.GetType("System.Int64")
                        && gridView.Columns[i].ColumnType != Type.GetType("System.DateTime")
                        && gridView.Columns[i].ColumnType != Type.GetType("System.Boolean")
                        && gridView.Columns[i].ColumnType != Type.GetType("System.Decimal"))
                        columnsName.Add(gridView.Columns[i].FieldName);
                }

                Columns = columnsName.ToArray();
            }

            this.BindingSource = bindingSource;
            this.dtAll = dtAll;
            this.dtData = dtData;
            Columns = RemoveNotStringColumns(Columns, dtAll);
        }

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
            }

            return columnsList.ToArray();
        }

        /// <summary>
        /// 初始化检索控件所需数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="columns">要检索的列名</param>
        /// <param name="bindingSource"></param>
        /// <param name="dtHospital"></param>
        /// <param name="dtAll"></param>
        public void Initialize(string[] columns, BindingSource bindingSource, DataTable dtData, DataTable dtAll)
        {
            Columns = columns;
            this.BindingSource = bindingSource;
            this.dtAll = dtAll;
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

        /// <summary>
        /// 查询框文本
        /// </summary>
        public new string Text
        {
            get
            {
                return txtSort.Text;
            }
            set
            {
                txtSort.Text = value;
            }
        }
    }
}
