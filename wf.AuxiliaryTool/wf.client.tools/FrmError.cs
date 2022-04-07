using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;

namespace dcl.client.tools
{
    public partial class FrmError : FrmCommon
    {
        public FrmError()
        {
            InitializeComponent();        
        }
        
        //构造函数
        public FrmError(DataTable dt)
        {
            InitializeComponent();
            bsResultMessage.DataSource = dt;
        }
       
    }
}
