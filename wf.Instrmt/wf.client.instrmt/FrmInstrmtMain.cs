using dcl.client.frame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.instrmt
{
    public partial class FrmInstrmtMain : FrmCommon
    {
        public FrmInstrmtMain()
        {
            InitializeComponent();
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }

        private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Tag != null)
            {
                plControl.Controls.Clear();

                string controlName = e.Link.Item.Tag.ToString();

                Control ins = new Control();
                switch (controlName)
                {
                    case "InstrmtRegistration":
                        ins = new FrmInstrmtRegistration();
                        break;
                    case "DictInstrmtMaintain":
                        ins = new FrmDictInstrmtMaintain();
                        break;
                    case "MaintainSelect":
                        ins = new FrmMaintainSelect();
                        break;
                    default:
                        return;
                }

                plControl.Controls.Add(ins);
                ins.Dock = DockStyle.Fill;


            }



        }
    }
}
