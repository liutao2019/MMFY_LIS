using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraReports.UI;

using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using dcl.client.common;
using dcl.client.report;
using DevExpress.XtraGrid;

using System.Diagnostics;
using lis.client.control;
using System.IO;

using dcl.client.wcf;
using Lib.LogManager;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System;

namespace dcl.client.resultquery
{
    public partial class FrmPatSelector : FrmCommon
    {
        public FrmPatSelector()
        {
            InitializeComponent();
        }
        public List<string> m_Retlist { get; set; }
        DataTable m_Source;
        public FrmPatSelector(DataTable source)
        {
            InitializeComponent();
            m_Source = source;
            m_Retlist = new List<string>();
        }

        private void FrmSerLayout_Load(object sender, EventArgs e)
        {
            if (m_Source != null && m_Source.Rows.Count > 0)
            {
                DataTable dtLayout = new DataTable();
                dtLayout.Columns.Add("flag");
                dtLayout.Columns.Add("pat_in_no");
                dtLayout.Columns.Add("pat_name");
                dtLayout.Columns.Add("allcount");
                dtLayout.Columns.Add("reportcount");
                dtLayout.Columns.Add("unreportcount");
                List<string> list = new List<string>();

                for (int i = 0; i < m_Source.Rows.Count; i++)
                {

                    string patinno = m_Source.Rows[i]["pat_in_no"].ToString();
                    if (string.IsNullOrEmpty(patinno)) continue;

                    if (!list.Contains(patinno))
                    {
                        list.Add(patinno);

                        DataRow row = dtLayout.NewRow();
                        row["flag"] = "0";
                        row["pat_in_no"] = patinno;
                        row["pat_name"] = m_Source.Rows[i]["pat_name"];
                        row["allcount"] = m_Source.Select(string.Format("pat_in_no='{0}'", patinno)).Length;
                        row["reportcount"] = m_Source.Select(string.Format("pat_in_no='{0}' and pat_flag>1", patinno)).Length;
                        row["unreportcount"] = m_Source.Select(string.Format("pat_in_no='{0}' and (pat_flag<2 or pat_flag is null)", patinno)).Length;
                        dtLayout.Rows.Add(row);
                    }

                }

                for (int j = 0; j < dtLayout.Rows.Count; j++)
                {
                    if (Convert.ToInt32(dtLayout.Rows[j]["reportcount"].ToString()) == Convert.ToInt32(dtLayout.Rows[j]["allcount"].ToString()))
                    {
                        dtLayout.Rows[j]["flag"] = 1;
                    }
                }

                this.gridControl1.DataSource = dtLayout;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //DataRow dr = this.gridView1.GetFocusedDataRow();
            //DataTable sys = CommonClient.CreateDT(new string[] { "des_id", "des_type", "des_object"}, "sys_form_desig");

            //if (dr["type_id"].ToString() == "0")
            //{
            //    sys.Rows.Add("0", "0", "细菌报告");
            //}
            //else
            //{
            //    sys.Rows.Add(dr["type_id"].ToString(), "1", "细菌报告");
            //}
            //base.doOther(sys);
            //DBHelper helper = new DBHelper();

            //SqlCommand cmd = new SqlCommand();
            //string sql = "insert into sys_form_desig(des_id,des_type,des_object,des_value,des_flag) values(@des_id,@des_type,@des_object,@des_value,@des_flag)";
            //cmd.CommandText = sql;
            //cmd.Parameters.AddWithValue("des_id", sys.Rows[0]["des_id"]);
            //cmd.Parameters.AddWithValue("des_type", sys.Rows[0]["des_type"]);
            //cmd.Parameters.AddWithValue("des_object", "细菌报告");
            //cmd.Parameters.AddWithValue("des_value", buff);
            //cmd.Parameters.AddWithValue("des_flag", 0);
            //helper.ExecuteNonQuery(cmd);
            //lis.client.control.MessageDialog.Show("保存成功！", "提示");
            //this.Close();
        }

        


        private void spbSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                gridView1.CloseEditor();
                DataTable dtType = (DataTable)this.gridControl1.DataSource;

                DataRow[] rows = dtType.Select("flag='1'");
                if (rows.Length == 0)
                {
                    lis.client.control.MessageDialog.Show("请勾引选择病人列表！", "提示");
                    return;
                }

                foreach (DataRow row in rows)
                {
                    m_Retlist.Add(row["pat_in_no"].ToString());
                }

                DialogResult = DialogResult.Yes;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                lis.client.control.MessageDialog.Show("选择失败！", "提示");
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cbAllSel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllSel.Checked)
            {
                gridView1.CloseEditor();
                DataTable dtType = (DataTable)this.gridControl1.DataSource;

                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    dtType.Rows[i]["flag"] = 1;
                }
                dtType.AcceptChanges();
            }
            else
            {
                gridView1.CloseEditor();
                DataTable dtType = (DataTable)this.gridControl1.DataSource;

                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    dtType.Rows[i]["flag"] = 0;
                }
                dtType.AcceptChanges();
            }
        }
    }
}
