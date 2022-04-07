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
    public partial class RoundPanelResult : XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundPanelGroupClick;

        private RoundPanel curRoundPanel;

        public RoundPanelResult()
        {
            InitializeComponent();
   
            rpAll.Click += RoundPanel_Click;
            rpHasResult.Click += RoundPanel_Click;
            rpNonResult.Click += RoundPanel_Click;
            rpView.Click += RoundPanel_Click;
            curRoundPanel = rpAll;
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

            rpHasResult.BeginColor = Color.LimeGreen;
            rpHasResult.EndColor = Color.LimeGreen;

            rpNonResult.BeginColor = Color.LimeGreen;
            rpNonResult.EndColor = Color.LimeGreen;

            rpView.BeginColor = Color.LimeGreen;
            rpView.EndColor = Color.LimeGreen;
        }

        public RoundPanel GetCurRoundPanel()
        {
            return curRoundPanel;
        }

        /// <summary>
        /// rpAll,rpNonAudit,rpNonReport,rpUrgent,rpNunPrint,rpReCheck
        /// </summary>
        /// <param name="name"></param>
        /// <param name="visable"></param>
        public void SetRoundPanelVisble(string name, bool visable)
        {
            Control []cls = autoFixSonPanel1.Controls.Find(name, true);
            if (cls == null || cls.Count() <= 0)
                return;

            cls[0].Visible = visable;
        }
    }
}
