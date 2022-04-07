using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.report
{
    public partial class FrmShowPreview : FrmCommon
    {
        public FrmShowPreview()
        {
            InitializeComponent();
            string strProp = UserInfo.GetSysConfigValue("ReportProportional");
            if (strProp != "")
            {
                printControl1.Zoom = float.Parse(strProp);
            }
            
        }
    }
}
