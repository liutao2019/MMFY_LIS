using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;

namespace dcl.client.result.PatControl
{
    public partial class ItrWarningMsg : UserControl
    {
        public ItrWarningMsg()
        {
            InitializeComponent();
        }

        public bool LoadItrWarningMsg(string patID, bool onlyshowRecheckMsg)
        {
            if (string.IsNullOrEmpty(patID))
            {
                this.gridControl1.DataSource = null;
                return false;
            }
            else
            {
                ProxyInstrmtWardingMsg proxyMsg = new ProxyInstrmtWardingMsg();
                List<EntityInstrmtWarningMsg> listMsg = proxyMsg.Service.CheckHasInstrmtWardMsgByPatItrId(patID);
                listMsg = listMsg.FindAll(i => !string.IsNullOrEmpty(i.WarnRecheckType));
                listMsg = listMsg.OrderByDescending(w => w.WarnMsgCode).ToList();
                this.gridControl1.DataSource = listMsg;
                return listMsg.Count > 0;
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gridView1.FocusedRowHandle)
            {
                e.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
            GridView grid = sender as GridView;
            EntityInstrmtWarningMsg row = grid.GetRow(e.RowHandle) as EntityInstrmtWarningMsg;
            //设置颜色和字体
            if(row!=null && row.WarnMsgCode=="1")
            {
                Font font = new System.Drawing.Font("Tahoma", 10, FontStyle.Bold);
                e.Appearance.Font = font;
               // e.Appearance.Font.Bold;
                e.Appearance.ForeColor = Color.Red;
            }
        }
    }


}
