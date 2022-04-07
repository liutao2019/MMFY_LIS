using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.notifyclient
{
    public partial class Form1 : Form
    {
        FrmUrgentNotify frmUNy = null;

        public Form1()
        {
            InitializeComponent();

            //检验是否启动危急值内部提醒
            string srConfig = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_In_IsNotify");
            //if (srConfig=="是")
            //{
            frmUNy = new FrmUrgentNotify();
            frmUNy.startShowFrm();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frmUNy != null)
            {
                frmUNy.StriogClose();
                frmUNy = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (frmUNy == null)
            {
                frmUNy = new FrmUrgentNotify();
                frmUNy.startShowFrm();
            }

        }
    }
}
