using System;
using System.Data;
using System.Drawing;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.entity;
using System.Collections.Generic;

namespace dcl.client.samstock
{
    public partial class FrmRackUseStatus  : FrmCommon
    {
        DataRow InnerRow { get; set; }
        public FrmRackUseStatus(DataRow row)
        {
            InitializeComponent();
            InnerRow = row;
        }

        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmRackUseStatus_Load(object sender, EventArgs e)
        {
            sysToolBar.SetToolButtonStyle(new[] {sysToolBar.BtnClose.Name });
            if (InnerRow != null)
            {
                txtSsid.Text = InnerRow["ss_id"].ToString();
                txtRackId.Text = InnerRow["rack_id"].ToString();
                txtRackName.Text = InnerRow["rack_name"].ToString();

                int xNum;
                int yNum;
                if (InnerRow["cus_x_num"]!=null&&int.TryParse(InnerRow["cus_x_num"].ToString(), out xNum) &&
                    InnerRow["cus_y_num"] != null && int.TryParse(InnerRow["cus_y_num"].ToString(), out yNum))
                {
                    BindGridView(xNum, yNum, InnerRow["ss_id"].ToString());
                    textEditX.EditValue = xNum;
                    textEditY.EditValue = yNum;

                }
                else
                {
                    lis.client.control.MessageDialog.Show("试管字典设置横向数或者纵向数异常", "信息");
                }
            }
        }

        private void BindGridView(int xNum, int yNum, string ssId)
        {
            DataTable table=new DataTable();
            for (int i = 1; i <= xNum; i++)
            {
                table.Columns.Add(i.ToString());
            }
            for (int j = 1; j <=yNum; j++)
            {
                table.Rows.Add(table.NewRow());
            }

            ProxySamManage proxy = new ProxySamManage();
            List<EntitySampStoreDetail> dtRack = proxy.Service.GetSimpleRackDetail(ssId);
            foreach (EntitySampStoreDetail row in dtRack)
            {
                int x = int.Parse(row.DetX.ToString());
                int y = int.Parse(row.DetY.ToString());
                table.Rows[x-1][y-1] = 1;
            }

            gridControlRack.DataSource = table;
        }

        private void gridViewRack_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowIndex = e.RowHandle;
            if (rowIndex >= 0)
            {
                rowIndex++;
                e.Info.DisplayText = rowIndex.ToString();
            }
        }
    }

}
