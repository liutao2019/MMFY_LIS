using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace dcl.client.tools
{
    public partial class FrmTarget : Form
    {

        public FrmTarget()
        {
            InitializeComponent();
        }

        public FrmTarget(List<EntityPidReportMain> dt)
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
