using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.tools
{
    public partial class FrmSource : FrmCommon
    {

        public FrmSource()
        {
            InitializeComponent();

        }

        //构造函数
        public FrmSource(List<EntityPidReportMain> dt)
        {
            InitializeComponent();

            dsLabBindingSource.DataSource = FormatDataTable(dt);
        }

        private List<EntityPidReportMain> FormatDataTable(List<EntityPidReportMain> dt)
        {
            foreach (EntityPidReportMain row in dt)
            {

                string ageText = row.PidAgeExp.ToString();
                if (!string.IsNullOrEmpty(ageText))
                {
                    ageText = ageText.Remove(ageText.IndexOf("Y"));
                    row.PidAgeExp = ageText;
                }
            }

            return dt;
        }

    }
}
