using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using dcl.client.frame;

namespace dcl.client.qc
{
    public partial class FrmQcView : FrmCommon
    {
        public FrmQcView()
        {
            InitializeComponent();
        }
        public void SetControl(ChartControl maxQc)
        {
            maxQc.ObjectHotTracked += new HotTrackEventHandler(maxQc_ObjectHotTracked);
            maxQc.Dock = DockStyle.Fill;
            panelControl1.Controls.Add(maxQc);
        }
        void maxQc_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;

            if (point != null)
            {
                string pt = point.Tag.ToString();
                toolTipController1.ShowHint(pt);
            }
            else
            {
                toolTipController1.HideHint();
            }
        }
    }
}
