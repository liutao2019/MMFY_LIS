using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace wf.ShelfPrint
{
    public partial class FrmMssage : Form
    {
        public FrmMssage()
        {
            InitializeComponent();
        }

        int timing = 1;

        private void FrmMssage_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.84;
            //Thread.Sleep(200);
            //pb2.Visible = true;
            //Thread.Sleep(200);
            //pb3.Visible = true;
            //Thread.Sleep(200);
            //pb4.Visible = true;
            //Thread.Sleep(200);
            //pb5.Visible = true;
            //Thread.Sleep(200);
            //pb6.Visible = true;

            //tmMsg.Start();
        }

        private void tmMsg_Tick(object sender, EventArgs e)
        {
            if (timing >= 2)
            {
                tmMsg.Stop();
                lblMsg.Text = " 未查询到您的信息！";
                lblMsg.ForeColor = Color.Red;
                Thread th2 = new Thread(close);
                th2.Start();
            }
            timing += 1;
        }

        public void ShowMsg(string msg, Color color)
        {
            lblMsg.Text = msg;
            lblMsg.ForeColor = color;
        }

        private void close()
        {
            //Thread.Sleep(1500);
            //this.DialogResult = DialogResult.OK;
        }
    }
}
