using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.common;
using Lis.CustomControls;
using dcl.client.frame;

namespace dcl.client.result.PatControl
{
    public partial class RoundPanelInstrument : XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundPanelGroupClick;

        private RoundPanel curRoundPanel;

        public RoundPanelInstrument()
        {
            InitializeComponent();
            rpAll.Click += RoundPanel_Click;
            rpThrough.Click += RoundPanel_Click;
            rpNunThrough.Click += RoundPanel_Click;
            rpTesting.Click += RoundPanel_Click;
            rpNunTesting.Click += RoundPanel_Click;
            curRoundPanel = rpAll;
        }

        private void RoundPanel_Click(object sender, EventArgs e)
        {
            GenDefaultColor();
            (sender as RoundPanel).BeginColor = Color.Sienna;
            (sender as RoundPanel).EndColor = Color.Sienna;
            curRoundPanel = (sender as RoundPanel);
            RoundPanelGroupClick?.Invoke(sender, e);
        }


        private void GenDefaultColor()
        {
            rpAll.BeginColor = Color.DarkOrange;
            rpAll.EndColor = Color.DarkOrange;

            rpThrough.BeginColor = Color.DarkOrange;
            rpThrough.EndColor = Color.DarkOrange;

            rpNunThrough.BeginColor = Color.DarkOrange;
            rpNunThrough.EndColor = Color.DarkOrange;

            rpTesting.BeginColor = Color.DarkOrange;
            rpTesting.EndColor = Color.DarkOrange;

            rpNunTesting.BeginColor = Color.DarkOrange;
            rpNunTesting.EndColor = Color.DarkOrange;
        }

        public RoundPanel GetCurRoundPanel()
        {
            return curRoundPanel;
        }
    }
}
