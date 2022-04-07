using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace lis.client.control
{
    public partial class WaitingDailog : XtraForm
    {
        public WaitingDailog(string message)
        {
            InitializeComponent();
            this.label1.Text = message;
        }
    }
}
