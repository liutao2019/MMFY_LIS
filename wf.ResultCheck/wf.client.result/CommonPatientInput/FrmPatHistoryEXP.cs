using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using DevExpress.XtraEditors;
using dcl.entity;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmPatHistoryEXP : FrmCommon
    {
        public FrmPatHistoryEXP()
        {
            InitializeComponent();
            gvHistoryExp.DoubleClick += new EventHandler(gvHistoryExp_DoubleClick);
        }
        public delegate void ExpDouclickEventHandler(object sender, ExpDouclickEventArgs args);

        public event ExpDouclickEventHandler ExpDouclick;
        void gvHistoryExp_DoubleClick(object sender, EventArgs e)
        {
            var row = gvHistoryExp.GetFocusedRow() as EntityPidReportMain; 

            if (row != null && row.RepRemark!=null)
            {
                if (ExpDouclick != null)
                {
                    ExpDouclick(sender, new ExpDouclickEventArgs(row.RepRemark));
                    this.Hide();
                }
            }
        }

        public String PatInNo { get; set; }
        public String PatID { get; set; }

        public String HistoryExp { get; set; }

        public void GetHistoryExp()
        {
            if (PatInNo.Trim() == String.Empty)
            {
                gcHistoryExp.DataSource = null;
            }
            else
            {
                EntityPatientQC qc = new EntityPatientQC();
                qc.PidInNo = PatInNo;
                qc.NotInRepId = true;
                qc.RepId = PatID;
                qc.RepRemarkIsNotNull = true;
                var dtHistoryExp = new ProxyPidReportMain().Service.PatientQuery(qc);

                gcHistoryExp.DataSource = dtHistoryExp;
            }
        }

        private void FrmPatHistoryEXP_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

    }
    public class ExpDouclickEventArgs : EventArgs
    {
        public string Pat_Exp { get; private set; }

        public ExpDouclickEventArgs(string pat_exp)
        {
            this.Pat_Exp = pat_exp;
        }
    }
}
