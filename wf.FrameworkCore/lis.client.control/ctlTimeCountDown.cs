using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace lis.client.control
{
    public partial class ctlTimeCountDown : UserControl
    {
        public ctlTimeCountDown()
        {
            InitializeComponent();
            lblTime.Text = timeSeconds.ToString();
        }

        #region event
        public delegate void TimeOutEventHandler(object sender, EventArgs args);
        public event TimeOutEventHandler TimeOut;
        protected virtual void OnTimeOut()
        {
            if (TimeOut != null)
            {
                TimeOut(this, EventArgs.Empty);
            }
        }
        #endregion
        public string LblTimeText
        {
            get
            {
                return lblTime.Text;
            }
        }
        private int timeSeconds = 10;
        private int counter;

        public int TimeSeconds
        {
            get { return timeSeconds; }
            set
            {
                timeSeconds = value;
                lblTime.Text = timeSeconds.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            lblTime.Text = counter.ToString();

            if (counter > 0)
            {

            }
            else
            {
                this.timer1.Stop();
                timerStopped = true;
                OnTimeOut();

            }
        }

        private void ctlTimeCountDown_Load(object sender, EventArgs e)
        {

        }

        public void ReCount()
        {
            counter = timeSeconds;
            if (timerStopped)
            {
                this.timer1.Start();
                timerStopped = false;
            }
        }

        public void Reset()
        {
            timer1.Stop();
            lblTime.Text = timeSeconds.ToString();
            timerStopped = true;
        }

        private bool timerStopped = true;

    }
}
