using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;

namespace dcl.client.samstock
{
    public partial class FrmPatientsView : FrmCommon
    {

        public FrmPatientsView()
        {
            InitializeComponent();

        }

        //构造函数
        public FrmPatientsView(DataTable dt)
        {
            InitializeComponent();

            dsLabBindingSource.DataSource = dt;
        }


    }
}
