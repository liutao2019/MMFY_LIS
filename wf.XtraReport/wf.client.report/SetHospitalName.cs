using System;
using System.Collections.Generic;

using System.Text;
using DevExpress.XtraReports.UI;
using dcl.client.frame;
using System.Drawing;

namespace dcl.client.report
{
    public static class SetHospitalName
    {
        public static void setName(BandCollection Bands)
        {
            foreach (Band band in Bands)
            {
                XRControl ctrl = band.FindControl("xrlblHospitalName", true);
                if (ctrl != null)
                {
                    ctrl.Text = UserInfo.GetSysConfigValue("HospitalName");
                    break;
                }
            }
        }
    }
}
