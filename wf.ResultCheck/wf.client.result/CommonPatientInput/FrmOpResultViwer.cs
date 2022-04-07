using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using dcl.client.frame;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmOpResultViwer : FrmCommon
    {
        List<EntityOperationResult> listOpResult = null;

        EntityOperationResultList opResultCollecton = null;

        public FrmOpResultViwer(List<EntityOperationResult> list)
        {
            listOpResult = list;
            InitializeComponent();
        }

        public FrmOpResultViwer(EntityOperationResultList list)
        {
            opResultCollecton = list;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmOpResultViwer_Load(object sender, EventArgs e)
        {
            this.gridControlPatList.DataSource = EntityToDataTable();
        }

        private DataTable EntityToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PatSID");
            dt.Columns.Add("PatName");
            dt.Columns.Add("Message");
            dt.Columns.Add("Success", typeof(bool));
            if (this.listOpResult != null)
            {
                foreach (EntityOperationResult item in listOpResult)
                {
                    if (!item.Success)
                    {
                        DataRow dr = dt.NewRow();
                        dr["PatSID"] = item.Data.Patient.RepSid;
                        dr["PatName"] = item.Data.Patient.PidName;
                        dr["Success"] = item.Success;
                        dr["Message"] = "删除失败：" + OperationMessageHelper.GetErrorMessage(item.Message);// item.GetErrorMessage();
                        dt.Rows.Add(dr);
                    }
                }
            }
            else if (this.opResultCollecton != null)
            {
                foreach (EntityOperationResult item in opResultCollecton)
                {
                    DataRow dr = dt.NewRow();
                    dr["PatSID"] = item.Data.Patient.RepSid;
                    dr["PatName"] = item.Data.Patient.PidName;
                    dr["Success"] = item.Success;

                    if (!item.Success)
                    {
                        dr["Message"] = OperationMessageHelper.GetErrorMessage(item.Message);
                    }
                    else
                    {
                        dr["Message"] = item.OperationName + "成功";
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private void gridViewPatList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Message")
            {
                DataRow dr = this.gridViewPatList.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    if ((bool)dr["Success"] == true)
                    {
                        e.Appearance.ForeColor = Color.Green;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
        }

    }
}
