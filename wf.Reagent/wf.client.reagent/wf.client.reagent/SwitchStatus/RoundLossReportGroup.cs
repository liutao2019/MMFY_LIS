using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Lis.CustomControls;

namespace wf.client.reagent
{
    public partial class RoundLossReportGroup : XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundLossReportGroupClick;

        private RoundPanel curRoundPanel;

        public RoundLossReportGroup()
        {
            InitializeComponent();
            Load += RoundLossReportGroup_Load;
            rpAll.Click += RoundPanel_Click;
            rpNonAudit.Click += RoundPanel_Click;
            rpAudit.Click += RoundPanel_Click;
            rpReturn.Click += RoundPanel_Click;

            curRoundPanel = rpAll;
        }

        private void RoundLossReportGroup_Load(object sender, EventArgs e)
        {
            //rpAudit.InnerText = "未" + LocalSetting.Current.Setting.ReportWord;
            //rpNonAudit.InnerText = "未" + LocalSetting.Current.Setting.AuditWord;
            //if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "否")
            //    rpNonAudit.Visible = false;
        }

        private void RoundPanel_Click(object sender, EventArgs e)
        {
            GenDefaultColor();
            (sender as RoundPanel).BeginColor = Color.Teal;
            (sender as RoundPanel).EndColor = Color.Teal;
            curRoundPanel = (sender as RoundPanel);
            RoundLossReportGroupClick?.Invoke(sender, e);
        }


        private void GenDefaultColor()
        {
            rpAll.BeginColor = Color.MediumTurquoise;
            rpAll.EndColor = Color.MediumTurquoise;

            rpNonAudit.BeginColor = Color.MediumTurquoise;
            rpNonAudit.EndColor = Color.MediumTurquoise;

            rpAudit.BeginColor = Color.MediumTurquoise;
            rpAudit.EndColor = Color.MediumTurquoise;

            rpReturn.BeginColor = Color.MediumTurquoise;
            rpReturn.EndColor = Color.MediumTurquoise;
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
