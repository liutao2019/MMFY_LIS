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
    public partial class RoundPanelType : XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundPanelGroupClick;

        private RoundPanel curRoundPanel;

        public RoundPanelType()
        {
            InitializeComponent();
            Load += RoundPanelGroup_Load;
            rpAll.Click += RoundPanel_Click;
            rpOutpatient.Click += RoundPanel_Click;
            rpIutpatient.Click += RoundPanel_Click;
            rpPhysicalExam.Click += RoundPanel_Click;
            rpUrgentInvestigation.Click += RoundPanel_Click;
            rpChecked.Click += RoundPanel_Click;
            curRoundPanel = rpAll;
        }

        private void RoundPanelGroup_Load(object sender, EventArgs e)
        {

        }

        private void RoundPanel_Click(object sender, EventArgs e)
        {
            GenDefaultColor();
            (sender as RoundPanel).BeginColor = Color.DarkGreen;
            (sender as RoundPanel).EndColor = Color.DarkGreen;
            curRoundPanel = (sender as RoundPanel);
            RoundPanelGroupClick?.Invoke(sender, e);
        }


        private void GenDefaultColor()
        {
            rpAll.BeginColor = Color.LimeGreen;
            rpAll.EndColor = Color.LimeGreen;

            rpOutpatient.BeginColor = Color.LimeGreen;
            rpOutpatient.EndColor = Color.LimeGreen;

            rpIutpatient.BeginColor = Color.LimeGreen;
            rpIutpatient.EndColor = Color.LimeGreen;

            rpPhysicalExam.BeginColor = Color.LimeGreen;
            rpPhysicalExam.EndColor = Color.LimeGreen;

            rpUrgentInvestigation.BeginColor = Color.LimeGreen;
            rpUrgentInvestigation.EndColor = Color.LimeGreen;

            rpChecked.BeginColor = Color.LimeGreen;
            rpChecked.EndColor = Color.LimeGreen;
        }

        public RoundPanel GetCurRoundPanel()
        {
            return curRoundPanel;
        }
    }
}
