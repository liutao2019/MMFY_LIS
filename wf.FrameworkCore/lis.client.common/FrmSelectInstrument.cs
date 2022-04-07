using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.common.extensions;
using dcl.entity;

namespace dcl.client.common
{
    public partial class FrmSelectInstrument : DevExpress.XtraEditors.XtraForm
    {
        public FrmSelectInstrument()
        {
            InitializeComponent();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView sourceGrid = sender as GridView;
            GridHitInfo info;
            Point pt = sourceGrid.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = sourceGrid.CalcHitInfo(pt);
            if (info == null || info.Column == null)
                return;

            if (info.InRow)
            {
                EntityDicInstrument row = (EntityDicInstrument)gridView1.GetFocusedRow();
                //´æ´¢ÒÇÆ÷
                if (!string.IsNullOrEmpty(row.ItrReportItrId))
                {
                    this.SelectInstrumentID = row.ItrReportItrId;
                    this.SelectInstrumentName = row.ItrEname;
                }
                else
                {
                    this.SelectInstrumentID = row.ItrId;
                    this.SelectInstrumentName = row.ItrEname;
                }
                this.Hide();
            }
        }

        public void ShowDialogByData()
        {
            if (Instrments != null && Instrments.Count > 0)
            {
                this.gridControl1.DataSource = Instrments;
                this.ShowDialog();
            }
        }

        public string SelectInstrumentID { get; set; }
        public string SelectInstrumentName { get; set; }

        public List<EntityDicInstrument> Instrments { get; set; }
    }
}