using System;
using System.Windows.Forms;


namespace dcl.client.notifyclient
{
    public partial class FrmShowClew : Form
    {
        public FrmShowClew()
        {
            InitializeComponent();
        }

        public void timerStart()
        {
            timer1.Enabled = true;
        }

        public void timerStop()
        {
            timer1.Enabled = false;
            this.scrollingText1.BackColor = System.Drawing.Color.Moccasin;
        }

        public void showText(string str)
        {
            scrollingText1.ScrollText = str;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.scrollingText1.BackColor == System.Drawing.Color.Moccasin)
            {
                this.scrollingText1.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                this.scrollingText1.BackColor = System.Drawing.Color.Moccasin;
            }
        }

        private void FrmShowClew_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
