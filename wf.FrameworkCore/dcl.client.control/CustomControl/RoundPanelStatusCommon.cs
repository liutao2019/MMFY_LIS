using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using Lis.CustomControls;
using System.Windows.Forms.Design;

namespace dcl.client.control
{
    public partial class RoundPanelStatusCommon : DevExpress.XtraEditors.XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundPanelGroupClick;

        private RoundPanel curRoundPanel;


        public RoundPanelStatusCommon()
        {
            InitializeComponent();
            _items = new List<RoundPanelStatusItem>();
        }

        List<RoundPanelStatusItem> _items;
        
        public List<RoundPanelStatusItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                LoadItems(value);
                this.Invalidate();
            }
        }

        private void LoadItems(List<RoundPanelStatusItem> value)
        {
            autopanel.Controls.Clear();

            if (value==null||value.Count==0)
            {
                return;
            }

            int i = 0;
            foreach(RoundPanelStatusItem item in value)
            {
                RoundPanel rp = new RoundPanel();
                rp.AutoSetFont = true;
                rp.BeginColor = System.Drawing.Color.DarkGreen;
                rp.Dock = System.Windows.Forms.DockStyle.Left;
                rp.EndColor = System.Drawing.Color.DarkGreen;
                rp.InnerText = item.Caption;
                rp.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                rp.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
                rp.Location = new System.Drawing.Point(0, 0);
                rp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                rp.Radius = 1;
                rp.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
                rp.Size = new System.Drawing.Size(60, 26);
                rp.TabIndex = 6;
                rp.Tag = item.Value;
                rp.Click += Rp_Click;
                autopanel.Controls.Add(rp);
            }
        }

        private void Rp_Click(object sender, EventArgs e)
        {
            GenDefaultColor();
            (sender as RoundPanel).BeginColor = Color.DarkGreen;
            (sender as RoundPanel).EndColor = Color.DarkGreen;
            curRoundPanel = (sender as RoundPanel);
            RoundPanelGroupClick?.Invoke(sender, e);
        }

        private void GenDefaultColor()
        {

            foreach(Control col in autopanel.Controls)
            {
                if(col is RoundPanel)
                {
                    (col as RoundPanel).BeginColor = Color.LimeGreen;
                    (col as RoundPanel).EndColor = Color.LimeGreen;
                }
            }
        }

        public RoundPanel GetCurRoundPanel()
        {
            return curRoundPanel;
        }

        public void SetValue(string value)
        {
            if (Items == null || Items.Count == 0)
                return;
            foreach(Control col in autopanel.Controls)
            {
                if(col is RoundPanel)
                {
                    RoundPanel rp = col as RoundPanel;
                    if(rp.Tag?.ToString()!=value)
                    {
                        rp.BeginColor = Color.LimeGreen;
                        rp.EndColor = Color.LimeGreen;
                    }
                    else
                    {
                        rp.BeginColor = Color.DarkGreen;
                        rp.EndColor = Color.DarkGreen;
                        curRoundPanel = rp;
                        RoundPanelGroupClick?.Invoke(rp, null);
                    }

                }
            }
        }
    }

    
    [Serializable]
    public class RoundPanelStatusItem
    {
        public string Caption { get; set; }

        public string Value { get; set; }


    }
}
