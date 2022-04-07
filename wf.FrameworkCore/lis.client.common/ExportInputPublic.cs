using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraExport;
using System.Data.OleDb;
//using Microsoft.Office.Interop;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data;

namespace dcl.client.common
{
    public class ExportInputPublic
    {
        public ProgressBarControl progressBarControl1=new ProgressBarControl();

        public DevExpress.XtraGrid.Views.Grid.GridView gridViewExport;

        public XtraUserControl XUserControl=new XtraUserControl();

        public ExportInputPublic()
        {

        }

        #region 导出数据处理
        /// <summary>
        /// 打开选择保存文件路径
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <param name="p_fileName"></param>
        /// <returns></returns>
        public string ShowSaveFileDialog(string p_fileName)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = p_fileName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + "Microsoft Excel Document";
            dlg.FileName = name;
            dlg.Filter = "Microsoft Excel|*.xls";
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        /// <summary>
        /// 打开导入来的文件
        /// </summary>
        /// <param name="fileName">文件路径名</param>
        public void OpenFile(string fileName)
        {
            if (XtraMessageBox.Show("是否要打开已导出的文件？", "文件导出...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("不能找到导入来的数据文件！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            progressBarControl1.Position = 0;

        }

        /// <summary>
        /// 数据导出操作
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="gridViewExport">需要导入数据的gridview</param>
        public void ExportTo(IExportProvider provider)
        {
            
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            BaseExportLink link = gridViewExport.CreateExportLink(provider);
            (link as GridViewExportLink).ExpandAll = false;
            link.Progress += new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);
            link.ExportTo(true);

            provider.Dispose();
            link.Progress -= new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);



            Cursor.Current = currentCursor;
        }


        private void Export_Progress(object sender, DevExpress.XtraGrid.Export.ProgressEventArgs e)
        {
            if (e.Phase == DevExpress.XtraGrid.Export.ExportPhase.Link)
            {
                
                    progressBarControl1.Position = e.Position;
                 XUserControl.Update();
                
                

            }
        }



        #endregion

        /// <summary>
        /// 读取Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="p_tableName">查询数据表的名字</param>
        /// <returns>返回一个数据集</returns>
        public static DataSet ExcelToDS(string Path, string p_tableName)
        {

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable table = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = table.Rows[0]["Table_Name"].ToString();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [" + tableName + "]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, tableName);
            ds.Tables[tableName].TableName = p_tableName;
            conn.Close();
            return ds;


        }
    }
}
