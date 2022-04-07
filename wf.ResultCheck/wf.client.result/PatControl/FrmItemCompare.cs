using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using dcl.client.wcf;

using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using dcl.entity;

namespace dcl.client.result.PatControl
{
    public partial class FrmItemCompare : FrmCommon
    {
        public Dictionary<string, int> ColumnList { get; set; }
        public List<EntityObrResult> ResData { get; set; }
        string itr_mid;
        int mI = 2;
        DataRow mainRow;
        DataTable CompareData;
        public FrmItemCompare()
        {
            InitializeComponent();
        }

        private void FrmItemCompare_Load(object sender, EventArgs e)
        {
            txtDate.EditValue = DateTime.Now;
            ColumnList = new Dictionary<string, int>();
            mI = 2;
        }

        private void FrmItemCompare_Shown(object sender, EventArgs e)
        {
            CompareData = new DataTable();
            CompareData.Columns.Add("样本编号");
            if (ResData != null && ResData.Count > 0)
            {
                foreach (EntityObrResult dr in ResData)
                {
                    if (!string.IsNullOrEmpty(dr.ItmContrastFactor))
                    {
                        itr_mid = dr.ItrEname;
                        CompareData.Columns.Add(dr.ItmEname);
                        ColumnList.Add(dr.ItmEname, 1);
                    }
                }


                if (CompareData.Columns.Count < 2)
                {
                    MessageDialog.ShowAutoCloseDialog("请到[字典管理]--[项目字典]--[项目字典]设置对比系数", 3);
                    return;
                }
                DataRow ftorRow = CompareData.NewRow();
                ftorRow["样本编号"] = "允许偏差";
                DataRow ecdRow = CompareData.NewRow();
                ecdRow["样本编号"] = ResData[0].ObrSid;
                foreach (EntityObrResult dr in ResData)
                {
                    if (!string.IsNullOrEmpty(dr.ItmContrastFactor))
                    {
                        ftorRow[dr.ItmEname] = dr.ItmContrastFactor;
                        ecdRow[dr.ItmEname] = dr.ObrValue;
                        // CompareData.Columns.Add(dr["res_itm_ecd"].ToString());
                    }
                }
                CompareData.Rows.Add(ftorRow);
                CompareData.Rows.Add(ecdRow);
                mainRow = ecdRow;
                gcData.DataSource = CompareData;
                int i = 0;
                foreach (GridColumn item in gridViewPatientList.Columns)
                {
                    if (i == 0)
                        item.Width = 120;
                    else
                        item.Width = 75;
                    i++;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtItr.valueMember) || string.IsNullOrEmpty(txtSID.Text) || string.IsNullOrEmpty(txtDate.Text))
                {
                    MessageDialog.ShowAutoCloseDialog("请输入查询条件", 3);
                    return;
                }

                string patid = txtItr.valueMember + txtDate.DateTime.ToString("yyyyMMdd") + txtSID.Text;

                ProxyPatResult proxy = new ProxyPatResult();

                EntityQcResultList dsPatData = proxy.Service.GetPatientCommonResult(patid,false);

                if (dsPatData == null || dsPatData.listResulto.Count == 0)
                {
                    MessageDialog.ShowAutoCloseDialog("无匹配数据", 3);
                    return;
                }
                List<EntityObrResult> dtPatResult = dsPatData.listResulto;

                bool isHasData = false;
                DataRow newRow = CompareData.NewRow();
                string newItrMid = "";
                List<string> list = new List<string>();

                foreach (EntityObrResult dr in dtPatResult)
                {
                    if (ColumnList.ContainsKey(dr.ItmEname))
                    {
                        isHasData = true;
                        newItrMid = dr.ItrEname;
                        newRow["样本编号"] = dr.ObrSid;
                        newRow[dr.ItmEname] = dr.ObrValue;

                        list.Add(dr.ItmEname);
                        // CompareData.Columns.Add(dr["res_itm_ecd"].ToString());±
                    }
                }


                if (isHasData)
                {

                    try
                    {

                        CompareData.Rows.InsertAt(newRow, mI);

                    }
                    catch
                    {
                        CompareData.Rows.InsertAt(newRow, 2);
                    }

                    DataRow cRow = CompareData.NewRow();

                    cRow["样本编号"] = string.Format("{0}与{1}对比", newItrMid, itr_mid);

                    double mR;
                    double cR;

                    foreach (string ecd in list)
                    {
                        try
                        {
                            if (double.TryParse(mainRow[ecd].ToString(), out mR) && double.TryParse(newRow[ecd].ToString(), out cR))
                            {
                                cRow[ecd] = (Math.Round((mR - cR) / mR, 4) * 100).ToString() + "%";
                            }
                        }
                        catch
                        {

                        }
                    }
                    CompareData.Rows.Add(cRow);

                    gcData.DataSource = CompareData;
                    txtSID.Text = string.Empty;
                    mI++;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }


        string RemoveSamp(string str)
        {
            if (str == null) return "";
            return str.Replace("±", "").Replace(">", "").Replace("<", "").Replace("%", "").Replace("-", "").Replace("+", "");
        }

        private void gridViewPatientList_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.Column.Name == "样本编号") return;
            if (gridViewPatientList.RowCount <= 2) return;
            //GridView grid = sender as GridView;

            DataRow frow = gridViewPatientList.GetDataRow(0);
            DataRow row = gridViewPatientList.GetDataRow(e.RowHandle);

            if (row["样本编号"].ToString().Contains("对比"))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName == "样本编号") continue;
                    try
                    {
                        if (Convert.ToDouble(RemoveSamp(row[col.ColumnName].ToString())) > Convert.ToDouble(RemoveSamp(frow[col.ColumnName].ToString())))
                        {
                            if (e.Column.FieldName == col.ColumnName)
                                e.Appearance.ForeColor = Color.Red;
                        }
                    }
                    catch
                    {

                    }



                }
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (gcData.DataSource != null && ((DataTable)gcData.DataSource).Rows.Count > 0)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcData.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("无对比数据！", "提示");
        }
    }
}
