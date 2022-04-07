using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace dcl.client.sample
{
    public partial class AdviceTime : UserControl
    {
        public AdviceTime()
        {
            InitializeComponent();
        }

        public bool HasTime
        {
            get { return StartTime > DateTime.MinValue && EndTime > DateTime.MinValue; }
        }

        public DateTime StartTime
        {
            get
            {
                return dateTimeControl1.Value;
            }
            set
            {
                dateTimeControl1.Value = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return dateTimeControl2.Value;
            }
            set
            {
                dateTimeControl2.Value = value;
            }
        }

        public DateTimeRange Value
        {
            get { return new DateTimeRange(StartTime, EndTime); }
            set
            {
                StartTime = value.Start;
                EndTime = value.End;
            }
        }

        public string StartText
        {
            get
            { return lblStart.Text; }
            set { lblStart.Text = value; }
        }
        public string EndText
        {
            get
            { return ""; }
            set
            {
                string aa;
                aa = value;
            }
        }

        /// <summary>
        /// 是否显示时间
        /// </summary>
        public bool ShowTime
        {
            get { return this.dateTimeControl1.ShowTime; }
            set
            {
                this.dateTimeControl1.ShowTime = value;
                this.dateTimeControl2.ShowTime = value;
            }
        }
    }
}
