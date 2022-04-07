using dcl.client.control;
using dcl.client.frame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.sample
{
    public partial class FrmVerifOrder : BaseButtonFormEx
    {
        public IPrint Printer { get; set; }

        private PrintType _printType;
        public PrintType PrintType
        {
            get { return _printType; }
            set
            {
                _printType = value;
                Printer = PrintFactory.Create(this.PrintType);
                dateTimeRange = Printer.GetDefaultAdviceTime();
            }
        }

        public DateTimeRange dateTimeRange
        {
            get { return new DateTimeRange((DateTime)deStart.EditValue, (DateTime)deEnd.EditValue); }
            set
            {
                deStart.EditValue = value.Start;
                deEnd.EditValue = value.End;
            }
        }

        public FrmVerifOrder(EntitySampMain samp)
        {
            InitializeComponent();
            GenComandButton();

            txtPatientName.Text = samp?.PidName;

            PrintType = PrintType.Outpatient;
            this.Load += FrmVerifOrder_Load;
            selectPatSource.onAfterSelected += SelectPatSource_onAfterSelected;
        }

        private void FrmVerifOrder_Load(object sender, EventArgs e)
        {
            selectPatSource.SetFilter(i => i.SrcId == "107" || i.SrcId == "108");
            this.selectPatSource.SelectByID("107");
        }

        private void GenComandButton()
        {
            try
            {
                AddComandButton("查询(F1)", dcl.client.common.Properties.Resources._32_查询, Search_Click, (int)Keys.F1);
                AddComandButton("刷新", dcl.client.common.Properties.Resources._32_刷新, Refresh_Click, 0);
                AddComandButton("关闭", dcl.client.common.Properties.Resources._32_关闭, Close_Click, 0);
            }
            catch (Exception ex)
            {

            }
        }

        private void SelectPatSource_onAfterSelected(object sender, EventArgs e)
        {
            lcID.Text = selectPatSource.displayMember + "ID";

            if(this.selectPatSource.valueMember == "107")
                PrintType = PrintType.Outpatient;
            if (this.selectPatSource.valueMember == "108")
                PrintType = PrintType.Inpatient;
            if (this.selectPatSource.valueMember == "109")
                PrintType = PrintType.TJ;
        }

        private void Search_Click(object sender, ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatientName.Text) && string.IsNullOrEmpty(txtPatientID.Text))
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请输入病人姓名，或者ID");
                return;
            }


            EntityInterfaceExtParameter downLoadInfo = new EntityInterfaceExtParameter();
            if (this.selectPatSource.valueMember == "107")
                downLoadInfo.DownloadType = InterfaceType.MZDownload;
            if (this.selectPatSource.valueMember == "108")
                downLoadInfo.DownloadType = InterfaceType.ZYDownload;
            downLoadInfo.PatientName = txtPatientName.Text;
            downLoadInfo.PatientID = txtPatientID.Text;
            downLoadInfo.StartTime = Convert.ToDateTime(deStart.DateTime.ToString("yyyy-MM-dd 0:00:00"));
            downLoadInfo.EndTime = Convert.ToDateTime(deEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"));

            List<EntitySampMain> order = new List<EntitySampMain>();
            order = new ProxySampMainDownload().Service.DownloadOrderData(downLoadInfo);
            bsPatient.DataSource = order;
        }


        private void Refresh_Click(object sender, ItemClickEventArgs e)
        {
            deStart.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 0:00:00"));
            deEnd.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 0:00:00"));
            txtPatientName.Text = string.Empty;
            txtPatientID.Text = string.Empty;
        }


        private void Close_Click(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
