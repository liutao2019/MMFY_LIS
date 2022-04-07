using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace dcl.client.sample
{
    public partial class DateTimeControl : UserControl
    {
        public DateTimeControl()
        {
            InitializeComponent();
            this.date.DateTime = new DateTime(1900, 1, 1);
        }

        public DateTime Value
        {
            get
            {
                DateTime result = date.DateTime;
                result = result.AddHours(timeEdit1.Time.Hour);
                result = result.AddMinutes(timeEdit1.Time.Minute);
                return result;
            }

            set
            {
                date.DateTime = value.Date;
                timeEdit1.Time = value;
            }
        }

        public void Clear()
        {
            timeEdit1.Reset();
        }

        public int Year
        {
            get
            {
                return date.DateTime.Year;
            }

            set
            {
                date.DateTime = new DateTime(value, date.DateTime.Month, date.DateTime.Day);
            }
        }


        public int Month
        {
            get
            {
                return date.DateTime.Month;
            }

            set
            {
                date.DateTime = new DateTime(date.DateTime.Year, value, date.DateTime.Day);
            }
        }

        public int Day
        {
            get
            {
                return date.DateTime.Day;
            }

            set
            {
                date.DateTime = new DateTime(date.DateTime.Year, date.DateTime.Month, (int)value);
            }
        }



        public int Hour
        {
            get
            {
                return timeEdit1.Time.Hour;
            }

            set
            {
                timeEdit1.Time = GenerateTime((int)value, timeEdit1.Time.Minute);
            }
        }

        public int Minute
        {
            get
            {
                return timeEdit1.Time.Minute;
            }

            set
            {
                timeEdit1.Time = GenerateTime(timeEdit1.Time.Hour, (int)value);
            }
        }

        private DateTime GenerateTime(int hour, int minute)
        {
            return new DateTime(1900, 1, 1, hour, minute, 0);
        }

        /// <summary>
        /// 是否显示时间
        /// </summary>
        public bool ShowTime { get { return timeEdit1.Visible; } set{ timeEdit1.Visible = value;} }
    }
}
